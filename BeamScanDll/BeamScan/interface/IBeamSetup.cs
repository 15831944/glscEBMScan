namespace EBMCtrl2._0.ebmScan
{
    using System;

    public interface IBeamSetup
    {
       /* SnapshotCode.BeamBranch.SetupBranch GetSnapshot();

        IBeamSetupInputChannels InputChannels { get; }

        ITemplateDummyDataChannels OutputChannels { get; }*/

        IBeamSetupDummySweep DummySweep { get; set; }

        string FullName { get; }

        int BufferFrameSize { get; set; }

        int WriteTimeout { get; set; }

        int ReadTimeout { get; set; }

        int Rate { get; set; }

        int NumberOfFrames { get; set; }
    }
}

