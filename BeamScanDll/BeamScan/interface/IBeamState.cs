namespace EBMCtrl2._0.ebmScan
{
   
    using System;

    public interface IBeamState
    {
       // SnapshotCode.BeamBranch.StateBranch GetSnapshot();

        string FullName { get; }

        BeamState CurrentState { get; set; }
    }
}

