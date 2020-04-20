using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBMCtrl2._0.ebmScan;
using netDxf;
using BeamScanDll.CADProcess;
namespace BeamScanDll
{
    public class EBMBeamScan
    {
        IBeamSetupDummySweep _iDummySweep;
        IBeamScan _beamScan;
        IBeamSetup _beamSetup;
        IBeamState _beamState;
        public event EventHandler OnPowerOff;
        public event EventHandler OnPowerOn;
        private BeamScanSingleton beamScanFactory;
        private bool _isBeamOn;
        private bool _isDirecStop;

        public event ShowMessDelegate OnOperation;
        public EBMBeamScan()
        {
            _iDummySweep = new BeamSetupDummySweep();
            _beamScan = new SanxinBeamScan();//
            _beamSetup = new BeamSetup();//
            _beamState = new SanxinBeamState();//
            _iDummySweep.Speed = 100000;
            _beamSetup.NumberOfFrames = 8;
            _beamSetup.Rate = 40000;
            _beamSetup.ReadTimeout = 2000;
            _beamSetup.WriteTimeout = 2000;
            _beamSetup.BufferFrameSize = 8000;
            _beamSetup.DummySweep = _iDummySweep;
     
            beamScanFactory = BeamScanSingleton.GetInstance(_beamSetup, _beamScan, _beamState);
        }
        public bool InitPdAoCard()
        {
            return false;
        }
        public void WriteRaw(double[] scan)
        {
            beamScanFactory.WriteRaw(scan);
        }
        public void UpdateSingleOut(double[] scan)
        {
            if (scan.Length != 8)
            {
                throw new FormatException("更新数据格式不正确，必须是个数为8的数组");
            }
            if (!_isDirecStop)
            {
                beamScanFactory.WriteRaw(scan);
            }
        }
        public void RunSingleOut(bool bUseCalibrate, IBeamScan beamScan, ushort focusADD)
        {
            if (_beamScan != null)
            {
                _beamScan.X = beamScan.X;
                _beamScan.Y = beamScan.Y;
                _beamScan.BeamCurrent = beamScan.BeamCurrent;
                if (bUseCalibrate)
                {
                    _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(beamScan.X, beamScan.Y, true);
                    _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(beamScan.X, beamScan.Y, false);
                    _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(beamScan.X, beamScan.Y)) + focusADD);
                }
                else
                {
                    _beamScan.Astig1 = beamScan.Astig1;
                    _beamScan.Astig2 = beamScan.Astig2;
                    _beamScan.Focus = beamScan.Focus;
                }
                BeamStart(SignalCardModeEnum.SingleOut);
                OnPowerOn?.Invoke(null, null);
                OnOperation?.Invoke("直流下束...");

                _isDirecStop = false;
            }
        }
        public void RunPeaHeatOut(ref ScanParamters paramters, bool isX)
        {

            try
            {
                _iDummySweep.Speed =(float) Parameter.PreHeat.Speed;
                _beamSetup.DummySweep = _iDummySweep;
                if (beamScanFactory != null)
                {
                    _beamScan.X = _beamScan.Y = _beamScan.Focus = _beamScan.Astig1 = _beamScan.Astig2 = 32767;
                    beamScanFactory.SetBeamScan(_beamScan);
                    _beamSetup.Rate = (int)Parameter.Frequency;
                    beamScanFactory.SetBeamSetup(_beamSetup);
                    beamScanFactory.SetBeamState(_beamState);
                }
                if (isX)
                {
                    beamScanFactory.CreatePreHeatLinesX(
                                   (ushort)Parameter.PreHeat.Size
                                 , (int)Parameter.PreHeat.LineOrder
                                 , (float)Parameter.PreHeat.LineOffset
                                 , (float)Parameter.PreHeat.Speed
                                 , Parameter.Frequency
                                 , paramters.scanCount
                                 , paramters.scanVolt
                                 , paramters.focusOffset
                                 , true
                                );
                }
                else
                {
                    beamScanFactory.CreatePreHeatLinesY(
                               (ushort)Parameter.PreHeat.Size
                             , (int)Parameter.PreHeat.LineOrder
                             , (float)Parameter.PreHeat.LineOffset
                             , (float)Parameter.PreHeat.Speed
                             , Parameter.Frequency
                             , paramters.scanCount
                             , paramters.scanVolt
                             , paramters.focusOffset
                             , true
                            );
                }
                if (_beamScan != null)
                {
                    if (!_isBeamOn)
                    {
                        BeamStart(SignalCardModeEnum.BufferedOut);
                        _isBeamOn = true;
                    }
                    OnPowerOn?.Invoke(null, null);
                    OnOperation?.Invoke(isX ? "X向预热..." : "Y向预热...");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void RunCADOut(uint count, DxfcadReader dxfcad)
        {
            try
            {
                _iDummySweep.Speed = (float)Parameter.cadFilescanPara.Speed;
                _beamSetup.DummySweep = _iDummySweep;
                if (beamScanFactory != null)
                {
                    _beamScan.X = _beamScan.Y = _beamScan.Focus = _beamScan.Astig1 = _beamScan.Astig2 = 32767;
                    beamScanFactory.SetBeamScan(_beamScan);
                    _beamSetup.Rate = (int)Parameter.Frequency;
                    beamScanFactory.SetBeamSetup(_beamSetup);
                    beamScanFactory.SetBeamState(_beamState);
                }
                double speed = Parameter.cadFilescanPara.Speed;
                double focusOffs = Parameter.cadFilescanPara.FocusOffset;
                double beamVal = Parameter.cadFilescanPara.BeamValue;
                DxfcadPackage dxfcadPackage = new DxfcadPackage(dxfcad);
                for (int i = 0; i < count; i++)
                {
                    beamScanFactory.AddPackage(dxfcadPackage);
                }
                if (_beamScan != null)
                {
                    if (!_isBeamOn)
                    {
                        BeamStart(SignalCardModeEnum.BufferedOut);
                        _isBeamOn = true;
                    }
                    OnPowerOn?.Invoke(null, null);
                    OnOperation?.Invoke("cad  扫描");

                }
            }
            catch(Exception)
            {
                throw;
            }

        }
        public void StopBeamOut()
        {
            try
            {
                //if (rbCircle.Checked) {
                if (beamScanFactory != null)
                {
                    _beamScan.X = 32767;
                    _beamScan.Y = 32767;
                    _beamScan.Focus = 32767;
                    /* if (rbDirec.Checked)
                     {
                         _beamScan.Focus = 32767;
                     }*/
                    _beamScan.Astig1 = 32767;
                    _beamScan.Astig2 = 32767;
                    _beamScan.BeamCurrent = 32000;//32767;
                    beamScanFactory.Shutdown();
                    beamScanFactory.Reset();//停止时候清空包里的数据
                    _isBeamOn = false;
                    _isDirecStop = true;
                    OnPowerOff?.Invoke(null, null);
                    OnOperation?.Invoke("停止");
                    //BtnStop.Enabled = false;
                    //BtnPreHeat.Enabled = btnScan.Enabled = true;


                    //Parameter.ActualPreHeatCount = 0;
                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // LogHelper.Error(ex.ToString());
            }
        }
        public void DummySweep(float speed)
        {
            this._iDummySweep.Speed = speed;
            _beamSetup.DummySweep = _iDummySweep;
            if (beamScanFactory != null)
            {
                _beamScan.X = _beamScan.Y = _beamScan.Focus = _beamScan.Astig1 = _beamScan.Astig2 = 32767;
                beamScanFactory.SetBeamScan(_beamScan);
                _beamSetup.Rate = (int)Parameter.Frequency;
                beamScanFactory.SetBeamSetup(_beamSetup);
                beamScanFactory.SetBeamState(_beamState);
            }
            if (_beamScan != null)
            {
                ///*************暂时去掉
                if (!_isBeamOn)
                {
                    BeamStart(SignalCardModeEnum.BufferedOut);
                    _isBeamOn = true;
                }
                OnPowerOn?.Invoke(null, null);
                OnOperation?.Invoke("空扫...");
            }
        }
        private void BeamStart(SignalCardModeEnum mode)
        {
            // beamScanFactory = new BeamScanFactory(_beamSetup, _beamScan, _beamState);
            beamScanFactory.SetBeamScan(_beamScan);
            beamScanFactory.SetBeamSetup(_beamSetup);
            beamScanFactory.SetBeamState(_beamState);
            beamScanFactory.Startup();
            beamScanFactory.Start(mode);
            _beamState.CurrentState = BeamState.Running;
        }
    }
}
