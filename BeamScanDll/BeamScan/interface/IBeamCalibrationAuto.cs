namespace EBMCtrl2._0.ebmScan
{
    using System;

    public interface IBeamCalibrationAuto
    {
       /* SnapshotCode.BeamBranch.CalibrationBranch.AutoBranch GetSnapshot();

        IBeamCalibrationAutoSimulation Simulation { get; }

        IBeamCalibrationAutoDefaults Defaults { get; }

        IBeamCalibrationAutoPositioning Positioning { get; }

        IBeamCalibrationAutoOptimizations Optimizations { get; }

        IBeamCalibrationAutoMachine Machine { get; }

        ITemplateDummyDataChannels DummyDataChannels { get; }*/

        string FullName { get; }

        double DummyThreshold { get; set; }
    }
}

