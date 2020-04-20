using System;
using System.Collections.Generic;
using System.Text;
using EBMCtrl2._0.ebmScan.Package;
namespace BeamScanDll.CADProcess
{
   public class DxfcadPackage : IBeamControlPackage
    {
        private int readIndex=0;

        public float ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float LayerThickness => 0.0f;

        public bool IsReadOnly => true;

        public int Length => DxfcadReader.Length;

        public ContentInformation Contents => throw new NotImplementedException();

        public string OutputDescription => throw new NotImplementedException();
        private DxfcadReader DxfcadReader;
        public DxfcadPackage(DxfcadReader dxfcad)
        {
            DxfcadReader = dxfcad;
        }
        public void OnDataChanged(string[] ids, IOValue[] values)
        {
            throw new NotImplementedException();
        }

        public int Read(ref double[,] frame)
        {
            int framLength = frame.GetLength(0);
            int rdl = this.Length - readIndex;//剩余数据长度
            if (rdl >= framLength)
            {
                DxfcadReader.ReadCadPoint(ref frame, 0, framLength);
                readIndex += framLength;
                return framLength;
            }
            else
            {
                DxfcadReader.ReadCadPoint(ref frame, 0, rdl);
               //    System.Diagnostics.Debug.Print(DateTime.Now.ToLongTimeString() + " 文件次数：" + Program.ActualPreHeatCount);
                return rdl;
            }
        }

        public void ResetCursors()
        {
            readIndex=0;
        }

        public void Rewind(int count)
        {
            this.readIndex = Math.Max(0, readIndex - count);
        }

        public int Write(double[,] data)
        {
            throw new NotImplementedException();
        }

        public int Write(double[,] data, int sourceOffset)
        {
            throw new NotImplementedException();
        }
    }
}
