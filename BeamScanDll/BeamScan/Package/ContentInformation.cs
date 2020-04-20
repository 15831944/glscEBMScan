namespace EBMCtrl2._0.ebmScan.Package
{
    using System;

    public sealed class ContentInformation
    {
        public float MaxCurrentInPackage;
        public PreheatBox PreheatBoxOrigo;

        public ContentInformation()
        {
            this.PreheatBoxOrigo = new PreheatBox();
        }

        public ContentInformation(ContentInformation info)
        {
            this.MaxCurrentInPackage = info.MaxCurrentInPackage;
            this.PreheatBoxOrigo = new PreheatBox(info.PreheatBoxOrigo);
        }

        public sealed class PreheatBox
        {
            public float OrigoX;
            public float OrigoY;
            public bool HasOrigo;

            public PreheatBox()
            {
            }

            public PreheatBox(ContentInformation.PreheatBox box)
            {
                this.OrigoX = box.OrigoX;
                this.OrigoY = box.OrigoY;
                this.HasOrigo = box.HasOrigo;
            }
        }
    }
}

