namespace EBMCtrl2._0.ebmScan.Package
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IBeamController : IDisposable
    {
        event BeamPackageCompleted OnPackageCompleted;

        void AddPackage(IBeamControlPackage package);
    }
}

