using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBMCtrl2._0.ebmScan.Package;
namespace EBMCtrl2._0.BeamScan.PreHeat
{
    class PreHeatVerticalPackage : IBeamControlPackage
    {
       public PreHeatVerticalPackage(PreHeatSweep sweep)
        {
            this.VerticalSweep = sweep;
        }
        private PreHeatSweep VerticalSweep;
        private int readIndex=0;
        public float ID { get; set ; }

        public float LayerThickness => 0.0f;

        public bool IsReadOnly =>true;

        public int Length => this.VerticalSweep.verLength;

        public ContentInformation Contents => throw new NotImplementedException();

        public string OutputDescription => throw new NotImplementedException();

        public void OnDataChanged(string[] ids, IOValue[] values)
        {
            throw new NotImplementedException();
        }

        public int Read(ref double[,] frame)
        {
            int framLength = frame.GetLength(0);
            int rdl=this.Length-readIndex;//剩余数据长度
            if (rdl>=framLength)
            {
                this.VerticalSweep.ReadVertical(ref frame, 0, framLength);
                readIndex += framLength;
                return framLength;
            }
            else
            {
                this.VerticalSweep.ReadVertical(ref frame, 0, rdl);
                return rdl;
            }
            
        }

        public void ResetCursors()
        {
            this.readIndex=0;
        }

        public void Rewind(int count)
        {
           this.readIndex=Math.Max(0,readIndex-count);
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
