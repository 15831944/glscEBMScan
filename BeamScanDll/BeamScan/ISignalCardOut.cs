using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBMCtrl2._0.ebmScan
{
    internal interface ISignalCardOut:ISignalCard,IDisposable
    {
        IAsyncResult BeginWrite(double[,] data, int length);
        int EndWrite(IAsyncResult ar);
        void Write(double[] data);
    }
}
