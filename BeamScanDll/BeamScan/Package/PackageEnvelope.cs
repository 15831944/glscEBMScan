namespace EBMCtrl2._0.ebmScan.Package
{
    //using Arcam.EBMControl.Framework.BeamControl;
    //using Arcam.EBMControl.Framework.Core.IO;
    //using Arcam.Utilities.System;
    using System;
    using System.Timers;

    internal class PackageEnvelope : IBeamControlPackage
    {
        private IBeamControlPackage package;
        private BeamControlPackageState state;
        private int outputCount;
        private int inputCount;
        public Timer EnqueueingTime = new Timer();
        public Timer OutputTime = new Timer();

        public PackageEnvelope(IBeamControlPackage package)
        {
            this.package = package;
            this.EnqueueingTime.Start();
        }

        public void OnDataChanged(string[] ids, IOValue[] values)
        {
            this.package.OnDataChanged(ids, values);
        }

        public int Read(ref double[,] scans)
        {
            int num = this.package.Read(ref scans);
            this.outputCount += num;
            return num;
        }

        public void ResetCursors()
        {
            this.outputCount = 0;
            this.inputCount = 0;
            this.package.ResetCursors();
        }

        public void Rewind(int count)//倒回
        {
            this.outputCount = Math.Max(0, this.outputCount - count);
            this.inputCount = Math.Max(0, this.inputCount - count);
            this.package.Rewind(count);
        }

        public override string ToString() => 
            string.Concat(new object[] { "ID:", this.ID, " Length:", this.Length });

        public int Write(double[,] data) => 
            this.package.Write(data);

        public int Write(double[,] data, int sourceOffset) => 
            this.package.Write(data, sourceOffset);

        public BeamControlPackageState BeamControlPackageState
        {
            get => 
                this.state;
            set => 
                this.state = value;
        }

        public float ID
        {
            get => 
                this.package.ID;
            set => 
                this.package.ID = value;
        }

        public IBeamControlPackage Package =>
            this.package;

        public int OutputCount
        {
            get => 
                this.outputCount;
            set => 
                this.outputCount = value;
        }

        public int InputCount
        {
            get => 
                this.inputCount;
            set => 
                this.inputCount = value;
        }

        public bool IsReadOnly =>
            this.package.IsReadOnly;

        public float LayerThickness =>
            this.package.LayerThickness;

        public int Length =>
            this.package.Length;

        public ContentInformation Contents =>
            this.package.Contents;

        public string OutputDescription =>
            this.package.OutputDescription;
    }
}

