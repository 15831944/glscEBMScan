namespace EBMCtrl2._0.ebmScan.Package
{
    using System;

    public sealed class IOValue
    {
        public readonly object Value;
        public readonly object WriterTag;

        public IOValue(object value, object writerTag)
        {
            this.Value = value;
            this.WriterTag = writerTag;
        }
    }
}

