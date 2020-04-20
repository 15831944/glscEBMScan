namespace EBMCtrl2._0.ebmScan
{
    
    using System;

    public interface IBeamControl
    {
        //SnapshotCode.BeamBranch.ControlBranch GetSnapshot();

        string FullName { get; }

        bool Start { get; set; }

        bool Stop { get; set; }

        bool Rewind { get; set; }

        int RewindInterval { get; set; }

        bool Pause { get; set; }

        bool Resume { get; set; }

      //  StateBoolean Break { get; set; }

        bool MeltSameLayer { get; set; }

        bool Clear { get; set; }

        bool PackageDone { get; set; }

        bool UseBreak { get; set; }

        float SimulatedActualOutputRate { get; set; }

        float MaxCurrentOfLayer { get; set; }
    }
}

