using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBMCtrl2._0.ebmScan
{
    class BeamSetupDummySweep : IBeamSetupDummySweep
    {
        public string FullName => throw new NotImplementedException();

        public ushort Size { get ; set ; }
        public float Speed { get ; set ; }
        public int LineOrder { get ; set ; }
        public float LineOffset { get ; set ; }
    }
}
