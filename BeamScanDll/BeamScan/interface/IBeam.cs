namespace EBMCtrl2._0.ebmScan
{
    
    using System;

    public interface IBeam
    {
      //  SnapshotCode.BeamBranch GetSnapshot();

    //    IBeamControl Control { get; }

        IBeamScan Scan { get;  }

         // IBeamCalibration Calibration { get; }

       IBeamState State { get; set; }

      /*   IBeamStatistics Statistics { get; }

        IBeamAlarms Alarms { get; }*/

        IBeamSetup Setup { get; set; }

        string FullName { get; }

      //  BeamRunMode RunMode { get; set; }

        float LayerThickness { get; set; }

        bool PackageAvailable { get; set; }

        bool LockPackageQueue { get; set; }

     //   BeamRunMode RunModeDemand { get; set; }

        float ScanSequenceDelay { get; set; }
    }
}

