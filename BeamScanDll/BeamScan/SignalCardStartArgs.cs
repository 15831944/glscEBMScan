using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBMCtrl2._0.ebmScan
{
   internal class SignalCardStartArgs
    {
        public SignalCardStartArgs(SignalCardModeEnum mode, int rate, int channels, int bufferFrames, int frameSize, int timeout)
        {
            this.Mode = mode;
            this.Rate = rate;
            this.Channels = channels;
            this.BufferFrames = bufferFrames;
            this.FrameSize = frameSize;
            this.Timeout = timeout;
        }

        public SignalCardModeEnum Mode { get; private set; }

        public int Rate { get; private set; }

        public int Channels { get; private set; }

        public int BufferFrames { get; private set; }

        public int FrameSize { get; private set; }

        public int Timeout { get; private set; }
    }
}
