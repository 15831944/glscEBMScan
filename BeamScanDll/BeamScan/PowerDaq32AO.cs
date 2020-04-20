using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UeiDaq;
using log4net;
namespace EBMCtrl2._0.ebmScan
{
    internal class PowerDaq32AO : ISignalCardOut, ISignalCard, IDisposable
    {
        private ILog m_Logger;
        private SignalCardModeEnum m_Mode;
        private AnalogRawWriter m_Writer;
        private int m_DeviceIndex = -1;
        private Session m_Session;
        private ushort[,] m_ScanBuffer;
        private int m_Channels;
        private bool m_IsRunning;

        void ISignalCard.Initialize(ILog logger) {
            this.m_Logger = logger;
            LoadDevice();
        }
        void ISignalCard.Start(SignalCardStartArgs args) {
            this.m_IsRunning = false;
            this.m_Channels = args.Channels;
            this.m_ScanBuffer = new ushort[args.FrameSize, args.Channels];
            ((ISignalCard)this).Stop();
            this.m_Mode = args.Mode;
            this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Setup card...");
            this.m_Session = new Session();
            this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Create output channels");
            this.m_Session.CreateAOChannel($"pwrdaq://dev{this.m_DeviceIndex}/Ao0:{7}", -10.0, 10.0);
            switch (this.m_Mode) {
                case SignalCardModeEnum.BufferedOut:
                    this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Configure timing for buffered output");
                    this.m_Session.ConfigureTimingForBufferedIO(args.FrameSize, TimingClockSource.Internal, (double)args.Rate, DigitalEdge.Rising, TimingDuration.Continuous);
                    this.m_Session.GetTiming().SetTimeout(args.Timeout);
                    this.m_Session.GetDataStream().SetNumberOfFrames(args.BufferFrames);
                    this.m_Session.GetDataStream().SetNumberOfScans(args.FrameSize);
                    this.m_Session.GetDataStream().SetOverUnderRun(0);
                    this.m_Session.GetDataStream().SetRegenerate(0);
                    break;

                case SignalCardModeEnum.SingleOut:
                    this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Configure timing for simple io");
                    this.m_Session.ConfigureTimingForSimpleIO();
                    break;
            }
            this.m_Writer = new AnalogRawWriter(this.m_Session.GetDataStream());
            this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Setup card done!");
        }
        void ISignalCard.Stop() {
            if (this.m_Session != null) {
                this.m_IsRunning = false;
                this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "available scans: " + this.m_Session.GetDataStream().GetAvailableScans());
                if (this.m_Session.IsRunning()) {
                    this.m_Session.Stop();
                }
                this.m_Session.Dispose();
                this.m_Session = null;
            }
        }
        public void Dispose() {
            ((ISignalCard)this).Stop();
        }
        IAsyncResult ISignalCardOut.BeginWrite(double[,] data, int length) {
            if (this.m_Mode == SignalCardModeEnum.BufferedOut) {
                this.Convert(data, this.m_ScanBuffer, length);
                int state = 0;
                if (this.m_IsRunning) {
                    state = this.m_Session.GetDataStream().GetAvailableScans();
                }
                try {
                    return this.m_Writer.BeginWriteMultipleScansUInt16(null, state, length, this.m_ScanBuffer);
                }
                catch (UeiDaqException exception) {
                    //if (exception.Error == Error.BufferUnderrun) {
                    // throw new SignalCardBufferUnderrunException(exception.Message, exception);
                    //}
                    //throw new SignalCardException(exception.Message, exception);
                }
            }
            return null;
        }
        int ISignalCardOut.EndWrite(IAsyncResult ar)//等待异步写数据
        {
            try {
                this.m_Writer.EndWriteMultipleScansUInt16(ar);
                this.m_IsRunning = true;
            }
            catch (UeiDaqException exception) {
                //if (exception.Error == Error.BufferUnderrun) {
                //      throw new SignalCardBufferUnderrunException(exception.Message, exception);
                //}
                //  throw new SignalCardException(exception.Message, exception);
            }
            return (int)ar.AsyncState;//异步操作的信息
        }
        void ISignalCardOut.Write(double[] data) {
            try {
                this.m_Writer.WriteSingleScanUInt16(Convert(data));
                this.m_IsRunning = true;
            }
            catch (UeiDaqException exception) {
                //if (exception.Error == Error.BufferUnderrun) {
                //  throw new SignalCardBufferUnderrunException(exception.Message, exception);
                //}
                // throw new SignalCardException(exception.Message, exception);
            }
        }
        private static ushort[] Convert(double[] inData) {
            ushort[] numArray = new ushort[inData.Length];
            for (int i = 0; i < inData.Length; i++) {
                double num2 = inData[i];
                if (num2 < 0.0) {
                    num2 = 0.0;
                }
                else if (num2 > 65535.0) {
                    num2 = 65535.0;
                }
                numArray[i] = (ushort)num2;
            }
            return numArray;
        }

        private void Convert(double[,] inData, ushort[,] outData, int length) {
            for (int i = 0; i < length; i++) {
                for (int j = 0; j < this.m_Channels; j++) {
                    double num3 = inData[i, j];
                    if (j == 0 || j == 1) {
                        //控制DA输出区间 added by haitong 20190830
                       /* if (num3 < Parameter.MinDaqAOBitValue) {
                            num3 = Parameter.MinDaqAOBitValue;
                        }
                        else if (num3 > Parameter.MaxDaqAOBitValue) {
                            num3 = Parameter.MaxDaqAOBitValue;
                        }*/
                    }
                    else {
                        if (num3 < 0.0) {
                            num3 = 0.0;
                        }
                        else if (num3 > 65535.0) {
                            num3 = 65535.0;
                        }
                    }
                    outData[i, j] = (ushort)num3;
                }
            }
        }

        private Device FindDevice() {
            DeviceEnumerator enumerator = new DeviceEnumerator("pwrdaq://");
            while (enumerator.MoveNext()) {
                Device current = (Device)enumerator.Current;
                if (current.GetDeviceName().Substring(4, 2) == "AO") {
                    this.m_Logger.DebugFormat("{0} > {1}", base.GetType().Name, "Analog out device found");
                    return current;
                }
            }
            return null;
        }
        private void LoadDevice() {
            if (this.m_DeviceIndex == -1) {
                Device device = this.FindDevice();
                if (device != null) {
                    this.m_DeviceIndex = device.GetIndex();
                }
            }
            if (this.m_DeviceIndex == -1) {
                throw new ApplicationException("Signalcard device not found!");
            }
        }



    }
}
