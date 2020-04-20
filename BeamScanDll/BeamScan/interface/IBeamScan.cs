namespace EBMCtrl2._0.ebmScan
{
    using System;

    public interface IBeamScan
    {
       // SnapshotCode.BeamBranch.ScanBranch GetSnapshot();

        string FullName { get; }

        ushort X { get; set; }

        ushort Y { get; set; }

        ushort Focus { get; set; }

        ushort Astig1 { get; set; }

        ushort Astig2 { get; set; }

        ushort BeamCurrent { get; set; }
    }
}

