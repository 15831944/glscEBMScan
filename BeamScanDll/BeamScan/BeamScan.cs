using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBMCtrl2._0.ebmScan
{
    class SanxinBeamScan : IBeamScan
    {
        public SanxinBeamScan()
        {
            X = Y = Focus = Astig1 = Astig2 = 32767;
            BeamCurrent = 32000;
            
        }
        public string FullName => throw new NotImplementedException();

        public ushort X { get ; set; }
        public ushort Y { get; set ; }
        public ushort Focus { get ; set ; }
        public ushort Astig1 { get; set ; }
        public ushort Astig2 { get  ; set ; }
        public ushort BeamCurrent { get ; set; }
    }
}
