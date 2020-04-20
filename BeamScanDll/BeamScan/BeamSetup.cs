using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBMCtrl2._0.ebmScan
{
    class BeamSetup : IBeamSetup
    {
        public IBeamSetupDummySweep DummySweep { get; set; }

        public string FullName => throw new NotImplementedException();

        public int BufferFrameSize { get ; set; }
        public int WriteTimeout { get ; set ; }
        public int ReadTimeout { get ; set ; }
        public int Rate { get ; set ; }
        public int NumberOfFrames { get ; set ; }
     
    }
}
