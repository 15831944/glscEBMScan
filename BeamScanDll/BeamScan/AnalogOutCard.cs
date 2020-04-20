using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Threading;
using System.Timers;
using System.IO;
using EBMCtrl2._0.ebmScan.Package;
using System.Diagnostics;
using BeamScanDll;
namespace EBMCtrl2._0.ebmScan
{
    class AnalogOutCard : IAnalogCard
    {

        private IBeam m_Beam;
        private IBuilds m_Builds;
        private IThemesPowerSupplyTheme m_PowerSupply;
        private IBeam m_BeamSnapshot;
        //  private IBeamCalibrationAuto m_AutocalibrationSnapshot;
        private ILog m_Logger;
        private PackageManager m_PackageManager;
        private Thread m_SignalCardThread;
        private AutoResetEvent m_EvtKillCardThread = new AutoResetEvent(false);
        private AutoResetEvent m_EvtPause = new AutoResetEvent(false);
        private ManualResetEvent m_EvtPaused = new ManualResetEvent(false);
        private ulong m_ScansWritten;
        public bool bPreHeat;
        //    private LocalIOCache m_IO;
        private PackageEnvelope m_CurrentPackage;
        private SignalCardModeEnum m_Mode;
        private double[,] m_ScanFrame;
        private DummySweep m_DummySweep;
        private ISignalCardOut m_SignalCard;
        private int m_TotalNumberOfScansInBuffer;
        private long m_BufferUnderruns;

        public event PowerOffDelegte OnPowerOffDelegate;//使能关闭
        public Action<string> actionScanDone;
        public Action actionPerPreHeatDone;
        public bool m_sweepScanStatus;
        public AnalogOutCard(ISignalCardOut signalCard, IBeam beamInterface, PackageManager packageManager) {
            this.m_SignalCard = signalCard;
            this.m_Logger = LogManager.GetLogger("sanxinScan.log");
            this.m_Beam = beamInterface;
            this.m_PackageManager = packageManager;
            bPreHeat = false;
            m_sweepScanStatus = true;
            // this.m_Builds = buildInterface;

            this.UpdateSettings();
            this.m_SignalCard.Initialize(this.m_Logger);

        }


        public void GetDummySweepFrame(ref double[,] frame, int beginIndex, int length) {
            this.m_DummySweep.Read(ref frame, beginIndex, length);
        }
        private void LatchDummyData() {
            this.m_SignalCard.Start(new SignalCardStartArgs(SignalCardModeEnum.SingleOut, 40000, 8, 800000, 8, 0));
            this.m_SignalCard.Write(this.GetDummy());
            this.m_SignalCard.Stop();
        }
        public void Pause() {
            this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Pausing Analog Out..");
            this.m_EvtPause.Set();
            if (!this.m_EvtPaused.WaitOne(0xbb8, false))//3s
            {
                this.m_Logger.WarnFormat("{0} > {1}", base.GetType().Name, "Pause timed out!!!!");
            }
            this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Paused Analog Out!");
        }

        public void Resume() {
            this.m_EvtPaused.Reset();
        }

        public void Rewind(int msecs) {
            if ((this.m_CurrentPackage != null) && (msecs >= 0)) {
                double num = (((double)this.m_BeamSnapshot.Setup.Rate) / 1000.0) * msecs;
                int count = (int)Math.Min(2147483646.0, num);
                this.m_CurrentPackage.Rewind(count);
            }
        }
        private void SignalCardThreadExecute() {
            IAsyncResult ar = null;
            try {
                int num3;
                WaitHandle[] waitHandles = new WaitHandle[] { this.m_EvtKillCardThread, this.m_EvtPause };//线程管理，自动事件
                int num = 0;
                bool packageDone = false;
                // ************空扫设固定值 haitong20190816 *****************
                this.m_BeamSnapshot.Setup.DummySweep.Size = 21600;
                this.m_BeamSnapshot.Setup.DummySweep.LineOffset = 216;
                this.m_BeamSnapshot.Setup.DummySweep.LineOrder = 20;
                //***********************************************************
                this.m_DummySweep = new DummySweep(this.m_BeamSnapshot.Setup.DummySweep.Size, this.m_BeamSnapshot.Setup.DummySweep.LineOffset, this.m_BeamSnapshot.Setup.DummySweep.LineOrder, this.m_BeamSnapshot.Setup.DummySweep.Speed, this.GetDummy(), (double)this.m_BeamSnapshot.Setup.Rate);
                int bufferFrameSize = this.m_BeamSnapshot.Setup.BufferFrameSize;
                while ((num3 = WaitHandle.WaitAny(waitHandles, 0, true)) != 0)//没有收到kill和pause事件，执行while循环
                {
                    if (num3 == 1) {
                        this.m_EvtPaused.Set();//手动停止
                    }
                    try {
                        try {
                            //  this.m_IO.LatchIn();//？？？加载
                            int beginIndex = 0;
                            /*if (this.m_IO.Read<bool>(IONamespace.Beam.Control.MeltSameLayer))//是否是扫描同一层
                            {
                                this.m_PackageManager.UsePreviousPackage();
                                this.m_IO.Write(IONamespace.Beam.Control.MeltSameLayer, false);//将其置为false复位
                            }*/
                            this.m_CurrentPackage = this.m_PackageManager.GetActiveOutPackage();//获取数据
                        // this.UpdateBeamControlInfo(this.m_CurrentPackage, packageDone);//更新当前数据包信息
                        /*   if ((((StateBoolean)this.m_IO.Read<StateBoolean>(IONamespace.Beam.Control.Break)) == StateBoolean.On) && this.m_IO.Read<bool>(IONamespace.Beam.Control.UseBreak))
                            {
                                this.m_CurrentPackage = null;
                            }*/
                            packageDone = false;
                            if ((this.m_CurrentPackage == null) || this.m_EvtPaused.WaitOne(0, true))//手动暂停，预热数据
                            {
                                this.GetDummySweepFrame(ref this.m_ScanFrame, 0, bufferFrameSize);//获取预热扫描数据
                                beginIndex = bufferFrameSize;
                                this.m_sweepScanStatus = true;

                            }
                            else {
                                this.m_sweepScanStatus = false;
                                beginIndex = this.m_CurrentPackage.Read(ref this.m_ScanFrame);//从数据包中读取扫描数据
                                if (beginIndex != bufferFrameSize)//如果取得数据不够一个framesize，则从预热区域取数据 
                                {
                                    if (beginIndex == 0) {
                                        packageDone = true;//图形扫描完成
                                                           /*  if (this.actionScanDone!=null)
                                                             {
                                                                 this.actionScanDone.Invoke("完全扫完，index=0");
                                                             }*/
                                                           //OnPowerOffDelegate?.Invoke();//add by hitong 20190806
                                    }
                                    else {
                                        this.GetDummySweepFrame(ref this.m_ScanFrame, beginIndex, bufferFrameSize - beginIndex);
                                        packageDone = true;//图形扫描完成标志位
                                        //if (this.actionScanDone != null)
                                        //{
                                        //    this.actionScanDone.Invoke("完全扫完，index!=0");
                                        //}
                                        //OnPowerOffDelegate?.Invoke();//add by hitong 20190806
                                    }
                                }
                            }
                            if (beginIndex > 0)//取数据成功
                            {
                                if (ar != null) {
                                    ar.AsyncWaitHandle.WaitOne();//异步调用
                                    try {
                                        num = this.m_SignalCard.EndWrite(ar);//等待异步写数据完成，返回同步状态
                                    }
                                    catch { }
                                    /*catch (SignalCardBufferUnderrunException)
                                    {
                                        this.m_BufferUnderruns += 1L;
                                    }
                                    catch (SignalCardException exception)
                                    {
                                        this.m_Logger.ErrorFormat("Beam Control[out]: {0}", exception.Message);
                                        throw;
                                    }*/
                                }
       
                             /* using (FileStream fs = new FileStream("data.txt", FileMode.Append)) {
                                    StreamWriter sw = new StreamWriter(fs);
                                    int x= m_ScanFrame.GetLength(0);
                                    for (int i = 0; i < m_ScanFrame.GetLength(0); i++) {
                                        int y = m_ScanFrame.GetLength(1);
                                        for (int j = 0; j < m_ScanFrame.GetLength(1); j++) {
                                            sw.Write(m_ScanFrame[i, j].ToString() + ",");
                                        }                           
                                        sw.WriteLine();
                                    }
                                    sw.Close();
                                }*/

                                ar = this.m_SignalCard.BeginWrite(this.m_ScanFrame, beginIndex);//等待上一次输出完成，再取数据   ????
                                float num5 = 100f - ((((float)num) / ((float)this.m_TotalNumberOfScansInBuffer)) * 100f);
                                num5 = Math.Max(0f, Math.Min(100f, num5));
                                // this.m_IO.Write(new string[] { IONamespace.Beam.Statistics.BufferRemaining, IONamespace.Beam.Statistics.BufferUnderrunsDuringPackage }, new object[] { num5, this.m_BufferUnderruns });
                                // this.m_ScansWritten += beginIndex;
                            }
                            if (packageDone) {
                                this.m_BufferUnderruns = 0L;
                                //this.m_IO.Write(new string[] { IONamespace.Beam.Control.Break, IONamespace.Beam.Control.PackageDone, IONamespace.Builds.State.CurrentBuild.MeltProgress }, new object[] { StateBoolean.On, true, 100 });
                                this.m_PackageManager.SetOutPackageDone();
                            }
                            // this.m_IO.Write(IONamespace.Beam.PackageAvailable, this.m_PackageManager.GetActiveOutPackage() != null);
                        }
                        catch (Exception exception2) {
                            if (exception2 is OutOfMemoryException) {
                                //  this.m_IO.Write(IONamespace.Beam.Alarms.SystemOutOfMemory, true);
                            }
                            this.m_Logger.ErrorFormat("{0} > {1}", base.GetType().Name, "[OUT] " + exception2.Message);
                            LogHelper.Error(exception2.ToString());
                            // this.m_IO.Write(IONamespace.Beam.Alarms.BeamControlError, true);
                        }
                        // continue;
                    }
                    finally {
                        //this.m_IO.LatchOut();
                    }
                }
            }
            catch (Exception exception3) {
                this.m_Logger.FatalFormat("{0} > {1}", base.GetType().Name, "Analog out card fatal exit: " + exception3.ToString());
                // this.m_IO.Write(IONamespace.Beam.Alarms.BeamControlError, true);
                // this.m_IO.LatchOut();
            }
        }
        public void Start(SignalCardModeEnum mode) {
            System.Timers.Timer timer = new System.Timers.Timer();
            try {
                //临时注释   this.Stop();
                timer.Start();
                this.m_Mode = mode;
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Starting analog out in {0} mode...", mode.ToString());
                this.UpdateSettings();
                this.m_SignalCard.Start(new SignalCardStartArgs(mode, this.m_Beam.Setup.Rate, 8, this.m_Beam.Setup.NumberOfFrames, this.m_Beam.Setup.BufferFrameSize, this.m_Beam.Setup.WriteTimeout));
                if (this.m_Mode == SignalCardModeEnum.BufferedOut) {
                    if (this.m_SignalCardThread != null) {
                        throw new ApplicationException("Signalcard already started!");
                    }
                    this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Starting analog out thread");
                    Thread thread = new Thread(new ThreadStart(this.SignalCardThreadExecute))
                    {
                        Name = "AnalogOutCard.SignalCardThread",
                        Priority = ThreadPriority.Normal,
                        IsBackground = false
                    };
                    this.bPreHeat = true;
                    this.m_SignalCardThread = thread;
                    this.m_SignalCardThread.Start();
                }
                else if (this.m_Mode == SignalCardModeEnum.SingleOut) {
                    double[] dummy = this.GetDummy();
                    this.m_SignalCard.Write(dummy);
                }
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Analog out started...");
            }
            catch (Exception exception) {
                this.m_Logger.FatalFormat("{0} > {1}", base.GetType().Name, "Error in Start!" + exception.ToString());
                throw;
            }
            finally {
                timer.Stop();
                // this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Startup time: " + timer.Duration);
            }
        }

        public void Stop() {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Start();
            try {
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Session stopping...");
                if (this.m_SignalCardThread != null) {
                    this.m_EvtKillCardThread.Set();
                    this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Wait for analog out thread...");
                    if (!this.m_SignalCardThread.Join(0x2710))//10s
                    {
                        this.m_SignalCardThread = null;
                        this.m_Logger.ErrorFormat("{0} > {1}", base.GetType().Name, "Signalcard thread didn't terminate in time!");
                        throw new ApplicationException("Signalcard thread didn't terminate in time!");
                    }
                    this.m_SignalCardThread = null;
                }
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Stopping signal card...");
                this.m_SignalCard.Stop();
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Signal card stopped.");
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Latch dummy data...");
                this.LatchDummyData();
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Dummy data latched.");
            }
            catch (Exception exception) {
                this.m_Logger.ErrorFormat("{0} > {1}", base.GetType().Name, "Error in Stop!" + exception.ToString());
                throw;
            }
            finally {
                timer.Stop();
                // this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Stop time: " + timer.Duration);
            }
        }
        public double[] GetDummy() => new double[]
        {
             //32767,32767,0,0,0,0,0,0
             this.m_Beam.Scan.X,
             this.m_Beam.Scan.Y,
             this.m_Beam.Scan.Focus,
             32767,
             32767,
             this.m_Beam.Scan.BeamCurrent,
             this.m_Beam.Scan.Astig1,
             this.m_Beam.Scan.Astig2,

        };
        private void UpdateSettings() {
            this.m_BeamSnapshot = this.m_Beam;
            this.m_ScanFrame = new double[m_Beam.Setup.BufferFrameSize, 8];
            // this.m_ScanFrame = new double[this.m_BeamSnapshot.Setup.BufferFrameSize, 8];
            this.m_TotalNumberOfScansInBuffer = this.m_BeamSnapshot.Setup.BufferFrameSize * this.m_BeamSnapshot.Setup.NumberOfFrames;
        }
        public void WriteRaw(double[] scan) {
            if (this.m_Mode == SignalCardModeEnum.SingleOut) {
                this.m_SignalCard.Write(scan);
            }
        }

    }
}
