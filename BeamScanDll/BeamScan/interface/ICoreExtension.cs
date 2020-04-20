namespace EBMCtrl2._0.ebmScan
{
   
    using System;

    public interface ICoreExtension
    {
        void Shutdown();
        void Startup(/*ICore core*/);
    }
}

