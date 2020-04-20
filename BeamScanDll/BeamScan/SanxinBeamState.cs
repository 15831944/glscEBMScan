
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBMCtrl2._0.ebmScan
{
    class SanxinBeamState : IBeamState
    {
        public string FullName => throw new NotImplementedException();

        public BeamState CurrentState { get; set; }
    }
}
