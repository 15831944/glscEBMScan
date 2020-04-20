using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBMCtrl2._0.ebmScan
{
    internal interface IAnalogCard
    {

        void Pause();
        void Resume();
        void Rewind(int msecs);
        void Start(SignalCardModeEnum mode);
        void Stop();
    }
}
