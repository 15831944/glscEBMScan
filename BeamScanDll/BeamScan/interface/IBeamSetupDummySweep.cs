namespace EBMCtrl2._0.ebmScan
{
    using System;

    public interface IBeamSetupDummySweep
    {
      //  SnapshotCode.BeamBranch.SetupBranch.DummySweepBranch GetSnapshot();

        string FullName { get; }

        ushort Size { get; set; }

        float Speed { get; set; }

        int LineOrder { get; set; }

        float LineOffset { get; set; }
    }
}

