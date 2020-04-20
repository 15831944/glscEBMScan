using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBMCtrl2._0;
namespace BeamScanDll
{
    /// <summary>
    /// 实现文件数据中的mm和实际使用中的bit转化
    /// </summary>
   public partial class FileDataAdapter
    {
        private double kp,offset;

        public double Kp { get => kp; set => kp = value; }
        public double Offset { get => offset; set => offset = value; }

        public FileDataAdapter(double kp, double offset)
        {
            this.Kp = kp;
            this.Offset = offset;
        }
        public Point DoubleMmToPoint(Point mmPoint)
        {
            Point pt= new Point(mmPoint.X * Kp + Offset, mmPoint.Y * Kp + Offset);
            if (pt.X>Parameter.MaxDaqAOBitValue||pt.Y> Parameter.MaxDaqAOBitValue)
            {
                throw new Exception("文件单位转化Bit输出值超出上界");
            }
            if (pt.X< Parameter.MinDaqAOBitValue||pt.Y< Parameter.MinDaqAOBitValue)
            {
                throw new Exception("文件单位转化Bit输出值超出下界");
            }
            return pt;

        }
        public List<Point> DoubleMmToPointList(List<Point>mmPointList)
        {
            List<Point> ptList = new List<Point>();
            foreach (var item in mmPointList)
            {
                ptList.Add(new Point(DoubleMmToPoint(item)));   
            }
            return ptList;
        }
        public Line DoubleMmToLine(Line mmLine)
        {
            Line line=new Line();
            line.Start = DoubleMmToPoint(mmLine.Start);
            line.End = DoubleMmToPoint(mmLine.End);
            return line;
        }
    }
}
