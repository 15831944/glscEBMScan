using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBMCtrl2._0.ebmScan.Package;
using EBMCtrl2._0.BeamScan.PreHeat;
using System.Timers;


namespace EBMCtrl2._0.ebmScan {
    class BeamScanSingleton : IBeamController, ICoreExtension {
        ISignalCardOut _pdao32Card;
        IBeam _beam;
        IBeamSetupDummySweep _iDummySweep;
        PreHeatSweep _preHeat = null;
        protected IBeamScan _beamScan;
        protected IBeamSetup _beamSetup;
        protected IBeamState _beamState;
        public AnalogOutCard analogCard;
        private PackageManager m_PackageManager = new PackageManager();
        protected double[] _preHeatScan = new double[8] { 32767.0, 32767.0, 32767.0, 32767.0, 32767.0, 32767.0, 32767.0, 32767.0 };

        public event BeamPackageCompleted OnPackageCompleted;

        public event EventHandler OnPowerOff;
        public Action<string> actionScanDone;
        public Action actionPerScanDone;

        private static BeamScanSingleton _instance;
        private static object _lock = new object();
        bool _isFirstIn = false;
        public static BeamScanSingleton GetInstance(IBeamSetup beamSetup, IBeamScan beamScan, IBeamState beamState) {
            if (_instance == null) {
                lock (_lock) {
                    if (_instance == null) {
                        _instance = new BeamScanSingleton(beamSetup, beamScan, beamState);
                    }
                }
            }
            return _instance;
        }
        private BeamScanSingleton(IBeamSetup beamSetup, IBeamScan beamScan, IBeamState beamState) {

            _beamScan = beamScan;//
            _beamSetup = beamSetup;//
            _beamState = beamState;//
            _pdao32Card = new PowerDaq32AO();//

            SignalCardStartArgs _cardArgs = new SignalCardStartArgs(SignalCardModeEnum.BufferedOut, 40000, 8, 500000, 8, 100);

            this.m_PackageManager.OnPackageCompleted += new PackageOfficeEvent(this.OnPackageManagerCompleted);

        }

        public void SetBeamScan(IBeamScan beamScan) {
            try {
                if (beamScan != null) {
                    this._beamScan = beamScan;
                    return;
                }
            }
            catch (Exception) {
                throw new Exception("beamScan is null");
            }

        }
        public void SetBeamSetup(IBeamSetup beamSetup) {
            try {
                if (beamSetup != null) {
                    this._beamSetup = beamSetup;
                }
            }
            catch (Exception) {
                throw new Exception("beamSetup is null");
            }
        }
        public void SetBeamState(IBeamState beamState) {
            try {
                if (beamState != null) {
                    this._beamState = beamState;
                }
            }
            catch (Exception) {
                throw new Exception("beamState is null");
            }
        }
        public void CreatePreHeatLinesX(ushort size, int lineOrder, float lineOffset, float speed, double frequency, uint scantimes, double beamvalue,double focusOffset, bool isPreheat) {
            try {
                _preHeat = new PreHeatSweep(size, lineOrder, lineOffset, speed, frequency, _preHeatScan, beamvalue, focusOffset, isPreheat);
                for (int i = 0; i < scantimes; i++) {
                    /*if (i != 0 && i % 10 == 0) {
                        m_PackageManager.Add(new PackageEnvelope(new HalfPreHeatHorizonPackage(_preHeat)));
                    }
                    else {
                        m_PackageManager.Add(new PackageEnvelope(new PreHeatHorizonPackage(_preHeat)));
                    }*/
                    m_PackageManager.Add(new PackageEnvelope(new PreHeatHorizonPackage(_preHeat)));
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
        }

        public void CreatePreHeatLinesY(ushort size, int lineOrder, float lineOffset, float speed, double frequency, uint scantimes, double beamvalue, double focusOffset,bool isPreheat) {
            try {
                _preHeat = new PreHeatSweep(size, lineOrder, lineOffset, speed, frequency, _preHeatScan, beamvalue, focusOffset, isPreheat);
                for (int i = 0; i < scantimes; i++) {
                    m_PackageManager.Add(new PackageEnvelope(new PreHeatVerticalPackage(_preHeat)));
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
        }
      /*  public void CreateCircle(float r, int divideNum, int lineOrder, float speed, double frequency) {
            this.CreateCircle(r, divideNum, lineOrder, speed, frequency, _preHeatScan);
        }
        public void CreateCircle(float r, int divideNum, int lineOrder, float speed, double frequency, double[] scan) {
            _circle = new ScanCircle(r, divideNum, lineOrder, speed, frequency, scan);
            m_PackageManager.Add(new PackageEnvelope(new ScanCirclePackage(_circle)));
        }*/

        public void InitialScan() {

            /*_iDummySweep.LineOffset = 500;
              _iDummySweep.LineOrder = 10;
              _iDummySweep.Size = 20000;
              _iDummySweep.Speed = 2000000;

              _beamSetup.NumberOfFrames=8;
              _beamSetup.Rate = 40000;
              _beamSetup.ReadTimeout = 100;
              _beamSetup.WriteTimeout = 100;
              _beamSetup.BufferFrameSize = 800000;
              _beamSetup.DummySweep = _iDummySweep;*/
        }
        private void OnPackageManagerCompleted(object sender, IBeamControlPackage package) {
            if (this.OnPackageCompleted != null) {
                this.OnPackageCompleted(this, package);
            }
        }
        public void Start(SignalCardModeEnum mode) {
            analogCard.Start(mode);
        }
        public void WriteRaw(double[] scan) {
            if (analogCard != null) {
                analogCard.WriteRaw(scan);
            }
        }
        public void Pause() {
            analogCard.Pause();
        }
        public void Resume() {
            analogCard.Resume();
        }
        public void Rewind(int msecs) {
            analogCard.Rewind(msecs);
        }
        public void Stop() {
            analogCard.Stop();
        }
        public void OndataChange() {

        }
        public void OnManualdataChange() {

        }

        public void AddPackage(IBeamControlPackage package) {
            if (package.Length == -1) {
                //receive manual package
            }
            this.m_PackageManager.Add(new PackageEnvelope(package));
        }

        public void Dispose() {
            this.Dispose(true);
        }
        private void Dispose(bool disposing) {
            if (disposing && analogCard != null) {
                this.Stop();
            }
        }

        public void Shutdown() {
            if (this.analogCard != null) {
                this.Stop();
                _beamState.CurrentState = BeamState.Stopped;
            }
        }

        public void Startup() {
            _beam = new SanXinBeam(_beamScan, _beamState, _beamSetup);
            analogCard = new AnalogOutCard(_pdao32Card, _beam, m_PackageManager);
            analogCard.OnPowerOffDelegate += AnalogCard_OnPowerOffDelegate;
            analogCard.actionScanDone += new Action<string>(ScanDoneAction);
            analogCard.actionPerPreHeatDone += new Action(PerScanDone);
        }
        private void ScanDoneAction(string info) {
            if (this.actionScanDone != null) {
                this.actionScanDone.Invoke(info);
            }
        }
        private void AnalogCard_OnPowerOffDelegate() {
            OnPowerOff?.Invoke(null, null);
        }
        private void PerScanDone() {
            actionPerScanDone?.Invoke();
        }

        public void Reset() {
            m_PackageManager.Reset();
        }
        private void OnPackageManagerPackageCompleted(object sender, IBeamControlPackage package) {
            if (this.OnPackageCompleted != null) {
                this.OnPackageCompleted(this, package);
            }
        }
    }
}
