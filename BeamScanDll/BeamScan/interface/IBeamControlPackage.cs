namespace EBMCtrl2._0.ebmScan.Package
{
   // using Arcam.EBMControl.Framework.Core.IO;
    using System;

    public interface IBeamControlPackage
    {
        void OnDataChanged(string[] ids, IOValue[] values);
        int Read(ref double[,] frame);
        void ResetCursors();
        void Rewind(int count);
        int Write(double[,] data);
        int Write(double[,] data, int sourceOffset);

        float ID { get; set; }

        float LayerThickness { get; }

        bool IsReadOnly { get; }

        int Length { get; }

        ContentInformation Contents { get; }//包含预热preheatBox扫描的信息

        string OutputDescription { get; }
    }
}

