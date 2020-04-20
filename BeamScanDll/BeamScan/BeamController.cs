namespace EBMCtrl2._0.ebmScan
{
  
   
    using log4net;
    using System;
    using System.Runtime.CompilerServices;

    public sealed class BeamController : IDisposable
    {
        private const string LoggerName = "BeamControlLog";
      //  private PackageManager m_PackageManager = new PackageManager();
        private AnalogOutCard m_AnalogOut;
        private ILog m_Log;
        private IBeam m_IO;
        private double[] m_ManualScan;
        IBeam _beam;
        //private readonly SignalCardType m_SignalCardType;
        // private IProcess m_ProcessManager;
        // private ProcessState m_ProcessState;
        //private readonly string[] m_PacketsSubscriptions = new string[] { IONamespace.Analyse.Control.HeatStartPlateBeamCurrentOverride, IONamespace.Analyse.Control.HeatStartPlate };

       // public event BeamPackageCompleted OnPackageCompleted;

        public BeamController(IBeam beam/*SignalCardType signalCardType*/)
        {
            _beam = beam;
            //this.m_SignalCardType = signalCardType;
          //  this.m_PackageManager.OnPackageCompleted += new PackageOfficeEvent(this.OnPackageManagerPackageCompleted);
        }

        protected  void ActionClear(object[] args)
        {
            this.m_Log.Debug("Clear started...");
            //this.m_PackageManager.Reset();
            this.m_Log.Debug("Clear done!");
        }

        protected  void ActionPause(object[] args)
        {
            this.m_Log.Debug("Pause started...");
            this.m_AnalogOut.Pause();
           // this.m_IO.State.CurrentState = BeamState.Paused;
            this.m_Log.Debug("Pause Done!");
        }

        protected  void ActionResume(object[] args)
        {
            this.m_Log.Debug("Resume started...");
            this.m_AnalogOut.Resume();
            //this.m_IO.State.CurrentState = BeamState.Running;
            this.m_Log.Debug("Resume Done!");
        }

        protected  void ActionRewind(object[] args)
        {
            this.m_Log.Debug("Rewind started...");
            this.m_AnalogOut.Rewind((int) args[1]);
            this.m_Log.Debug("Rewind Done!");
        }

        protected  void ActionStart(object[] args)
        {
            this.m_Log.Debug("Start started...");
            this.m_AnalogOut.Start(SignalCardModeEnum.BufferedOut);
           // this.m_IO.State.CurrentState = BeamState.Running;
            this.m_Log.Debug("Start Done!");
        }

        protected void ActionStop(object[] args)
        {
            this.m_Log.Debug("Stop started...");
            this.m_AnalogOut.Stop();
            this.ActionClear(args);
           // this.m_IO.State.CurrentState = BeamState.Stopped;
            this.m_Log.Debug("Stop Done!");
        }

        public void AddPackage(/*IBeamControlPackage package*/)
        {
           /* if (package.Length == -1)
            {
                this.m_Log.Debug("Received manual package");
            }
            this.m_PackageManager.Add(new PackageEnvelope(package));*/
        }

        void Shutdown()
        {
           // this.m_PackageManager.ThrowAwayNewPackages = true;
            if (this.m_AnalogOut != null)
            {
                this.m_AnalogOut.Stop();
            }
        }

        void Startup()
        {
          //  this.m_IO = core.GetTypedRoot<IBeam>();
           // this.m_IO.State.CurrentState = BeamState.UnInitialized;
          //  this.m_ProcessManager = core.GetTypedRoot<IProcess>();
          //  this.m_ProcessState = this.m_ProcessManager.ProcessManager.InternalProcessManagerState;
           // core.Cache.AddSubscription(new string[] { IONamespace.Beam.Control.Start, IONamespace.Beam.Control.Stop, IONamespace.Beam.Control.Pause, IONamespace.Beam.Control.Resume, IONamespace.Beam.Control.Rewind, IONamespace.Beam.Control.Clear, IONamespace.Beam.RunModeDemand, IONamespace.Beam.LockPackageQueue, IONamespace.Process.ProcessManager.InternalProcessManagerState }, new Arcam.EBMControl.Framework.Core.IO.OnDataChange(this.OnDataChange));
           // core.Cache.AddSubscription(new string[] { IONamespace.Beam.Scan.X, IONamespace.Beam.Scan.Y, IONamespace.Beam.Scan.Focus, IONamespace.Beam.Scan.Astig1, IONamespace.Beam.Scan.Astig2, IONamespace.Beam.Scan.BeamCurrent }, new Arcam.EBMControl.Framework.Core.IO.OnDataChange(this.OnManualDataChange));
          //  core.Cache.AddSubscription(this.m_PacketsSubscriptions, new Arcam.EBMControl.Framework.Core.IO.OnDataChange(this.OnPackedManagerDataChange));
            this.m_Log = LogManager.GetLogger("BeamControlLog");
            // IBeam typedRoot = core.GetTypedRoot<IBeam>();
           // IBuilds buildInterface = core.GetTypedRoot<IBuilds>();
           //  IThemesPowerSupplyTheme powerSupply = core.GetTypedRoot<IThemes>().PowerSupply.Theme;
            ISignalCardOut signalCard = null;
            //         switch (this.m_SignalCardType)
            //        {
            //            case SignalCardType.PowerDaqAO:
            //    signalCard = new PowerDaq32AO();
            //             break;

            /*        case SignalCardType.Simulated:
                        signalCard = new SimulatorCard(core.Cache, TimeSpan.FromMilliseconds(10.0), powerSupply.Beam.Range);
                        break;

                    case SignalCardType.File:
                        signalCard = new StreamWriterCard(core.GetTypedRoot<IThemes>(), core.Cache, TimeSpan.FromMilliseconds(10.0), powerSupply.Beam.Range);
                        break;
                }*/
            signalCard = new PowerDaq32AO();
           //this.m_AnalogOut = new AnalogOutCard( signalCard, _beam);
            this.m_ManualScan = this.m_AnalogOut.GetDummy();
            this.m_IO.Scan.X = (ushort) this.m_ManualScan[0];
            this.m_IO.Scan.Y = (ushort) this.m_ManualScan[1];
            this.m_IO.Scan.Focus = (ushort) this.m_ManualScan[2];
            this.m_IO.Scan.BeamCurrent = (ushort) this.m_ManualScan[3];
            this.m_IO.Scan.Astig1 = (ushort) this.m_ManualScan[4];
            this.m_IO.Scan.Astig2 = (ushort) this.m_ManualScan[5];
           // base.Send(new object[] { 0 });
            this.m_IO.State.CurrentState = BeamState.Initialized;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && (this.m_AnalogOut != null))
            {
                this.m_AnalogOut.Stop();
            }
        }

       /* private void OnDataChange(string[] ids, IOValue[] values)
        {
            for (int i = 0; i < ids.Length; i++)
            
                if ((ids[i] == _beam.Control.Start) && this.m_IO.Control.Start)
                {
                    this.m_IO.Control.Start = false;
                    base.Send(new object[] { 6 });
                    continue;
                }
                if ((ids[i] == IONamespace.Beam.Control.Stop) && this.m_IO.Control.Stop)
                {
                    this.m_IO.Control.Stop = false;
                    base.Send(new object[] { 2 });
                    continue;
                }
                if ((ids[i] == IONamespace.Beam.Control.Pause) && this.m_IO.Control.Pause)
                {
                    this.m_IO.Control.Pause = false;
                    base.Send(new object[] { 1 });
                    continue;
                }
                if ((ids[i] == IONamespace.Beam.Control.Resume) && this.m_IO.Control.Resume)
                {
                    this.m_IO.Control.Resume = false;
                    base.Send(new object[] { 4 });
                    continue;
                }
                if ((ids[i] == IONamespace.Beam.Control.Clear) && this.m_IO.Control.Clear)
                {
                    this.m_IO.Control.Clear = false;
                    this.ActionClear(null);
                    continue;
                }
                if ((ids[i] == IONamespace.Beam.Control.Rewind) && this.m_IO.Control.Rewind)
                {
                    this.m_IO.Control.Rewind = false;
                    base.Send(new object[] { 3, this.m_IO.Control.RewindInterval });
                    continue;
                }
                if (ids[i] == IONamespace.Beam.LockPackageQueue)
                {
                    this.m_PackageManager.ThrowAwayNewPackages = (bool) values[i].Value;
                    continue;
                }
                if (ids[i] == IONamespace.Process.ProcessManager.InternalProcessManagerState)
                {
                    this.m_ProcessState = (ProcessState) values[i].Value;
                    continue;
                }
                if ((ids[i] == IONamespace.Beam.RunModeDemand) && ((this.m_ProcessState == ProcessState.Disabled) || (this.m_ProcessState == ProcessState.Stopped)))
                {
                    BeamRunMode mode = (BeamRunMode) values[i].Value;
                    switch (mode)
                    {
                        case BeamRunMode.Simple:
                            this.m_ManualScan[0] = this.m_IO.Scan.X;
                            this.m_ManualScan[1] = this.m_IO.Scan.Y;
                            this.m_ManualScan[2] = this.m_IO.Scan.Focus;
                            this.m_ManualScan[3] = this.m_IO.Scan.BeamCurrent;
                            this.m_ManualScan[4] = this.m_IO.Scan.Astig1;
                            this.m_ManualScan[5] = this.m_IO.Scan.Astig2;
                            this.m_AnalogOut.Stop();
                            this.m_AnalogOut.Start(SignalCardModeEnum.SingleOut);
                            this.m_AnalogOut.WriteRaw(this.m_ManualScan);
                            break;

                        case BeamRunMode.BufferedOut:
                            this.m_AnalogOut.Stop();
                            this.m_AnalogOut.Start(SignalCardModeEnum.BufferedOut);
                            break;
                    }
                    this.m_IO.RunMode = mode;
                }
            }
        }

        private void OnManualDataChange(string[] ids, IOValue[] values)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == IONamespace.Beam.Scan.X)
                {
                    this.m_ManualScan[0] = (ushort) values[i].Value;
                }
                else if (ids[i] == IONamespace.Beam.Scan.Y)
                {
                    this.m_ManualScan[1] = (ushort) values[i].Value;
                }
                else if (ids[i] == IONamespace.Beam.Scan.Focus)
                {
                    this.m_ManualScan[2] = (ushort) values[i].Value;
                }
                else if (ids[i] == IONamespace.Beam.Scan.BeamCurrent)
                {
                    this.m_ManualScan[3] = (ushort) values[i].Value;
                }
                else if (ids[i] == IONamespace.Beam.Scan.Astig1)
                {
                    this.m_ManualScan[4] = (ushort) values[i].Value;
                }
                else if (ids[i] == IONamespace.Beam.Scan.Astig2)
                {
                    this.m_ManualScan[5] = (ushort) values[i].Value;
                }
            }
            this.m_AnalogOut.WriteRaw(this.m_ManualScan);
        }

        private void OnPackageManagerPackageCompleted(object sender, IBeamControlPackage package)
        {
            if (this.OnPackageCompleted != null)
            {
                this.OnPackageCompleted(this, package);
            }
        }

        private void OnPackedManagerDataChange(string[] ids, IOValue[] values)
        {
            this.m_PackageManager.OnDataChanged(ids, values);
        }*/
    }
}

