using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EBMCtrl2._0.ebmScan
{

    class SanXinBeam : IBeam
    {
        

        public IBeamScan Scan { get; }

        public IBeamState State { get; set; }

        public IBeamSetup Setup { get; set; }

        public string FullName { get; }

        public float LayerThickness { get ; set ; }
        public bool PackageAvailable { get ; set; }
        public bool LockPackageQueue { get ; set ; }
        public float ScanSequenceDelay { get ; set ; }
        public SanXinBeam(IBeamScan _Scan, IBeamState _State, IBeamSetup _Setup)
        {
            Scan = _Scan;
            State = _State;
            Setup = _Setup;
        }
    }
}
