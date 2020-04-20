using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
namespace EBMCtrl2._0.ebmScan
{
    interface ISignalCard:IDisposable
    {
        void Initialize(ILog logger);
        void Start(SignalCardStartArgs args);
        void Stop();
    }
}
