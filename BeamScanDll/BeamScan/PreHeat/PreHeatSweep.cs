using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBMCtrl2._0.ebmScan;
using BeamScanDll;
namespace EBMCtrl2._0.BeamScan.PreHeat {
    internal class PreHeatSweep {
        private ushort _preHeatSize;
        private int _preHeatLineOrder;
        private float _preHeatLineOff;
        private float _preHeatSpeed;
        private double _preHeatFrequency;
        private double[] _preHeatScan;
        private Point[] _horizonFrame;
        private Point[] _halfHorizonFrame;
        private Point[] _verticalFrame;
        private List<Point> horizonFrameList;
        private int _scanHorizonIndex;
        private int _halfHorizonIndex;
        private int _scanVerticalnIndex;
        private int _scanVerticalTimes;
        private int _scanHorizonTimes;
        private List<FocusAst> _focusHorizonAstList;
        private List<FocusAst> _focusverticalAstList;
        private double _beamValue;
        private double _focusOffset;
        private bool _isPreHeat;
        private struct FocusAst {
            public ushort focus;
            public ushort astX;
            public ushort astY;
            public FocusAst(ushort focus, ushort astx, ushort asty) {
                this.astX = astx;
                this.astY = asty;
                this.focus = focus;
            }
        }
        public PreHeatSweep(ushort size, int lineOrder, float lineOffset, float speed, double frequency, double[] scan, double beamValue,double focusOffset,bool isPreHeat) {
            if (size < 1) {
                throw new ArgumentOutOfRangeException("Size must be greater than zero");
            }
            if (lineOrder < 0 || lineOrder > 0xf4240)//1000000
            {
                throw new ArgumentOutOfRangeException("LineOrder must be greater than zero and max 1000000");
            }
            if (lineOffset < 0 || lineOffset > 1000000f) {
                throw new ArgumentOutOfRangeException("LineOffsetr must be greater than zero and max 1000000");
            }
            if (speed < 100f || float.IsNaN(speed)) {
                throw new ArgumentOutOfRangeException("Speed must be greater than 100");
            }
            if (scan.Length != 8) {
                throw new ArgumentOutOfRangeException("PreHeat scan length must be 8");
            }

            if (frequency <= 0 || frequency > 1000000.0 || double.IsNaN(frequency)) {
                throw new ArgumentOutOfRangeException("Frequency must be greater than zero and max 1000000");
            }
            this._preHeatSize = size;
            this._preHeatFrequency = frequency;
            this._preHeatLineOff = lineOffset;
            this._preHeatLineOrder = lineOrder;
            this._preHeatSpeed = speed;
            this._preHeatScan = scan;
            this._beamValue = beamValue;
            this._focusOffset = focusOffset;
            this._isPreHeat = isPreHeat;
            this.Create();

        }
        private void Create() {
            double centerX = this._preHeatScan[0];
            double centerY = this._preHeatScan[1];
            float num = ((float)this._preHeatSize) / 2f;
            double sy = Math.Max((double)0.0, (double)(centerY - num) + 0.5);//+0.5是防止扫出界外
            double ey = Math.Min((double)65535.0, (double)(centerY + num) + 0.5);
            double sx = Math.Max((double)0.0, (double)(centerX - num) + 0.5);
            double ex = Math.Min((double)65535.0, (double)(centerX + num) + 0.5);
            
            BeamScanDll.Parameter.MinDaqAOBitValue = (ushort)Math.Min(sy, sx);
            BeamScanDll.Parameter.MaxDaqAOBitValue = (ushort)Math.Max(ey, ex);
            List<Line> horizonLines = new List<Line>();
            List<Line> verticalLines = new List<Line>();
            _focusHorizonAstList = new List<FocusAst>();
            _focusverticalAstList = new List<FocusAst>();
            double num3 = Math.Max((double)0.0, (double)(centerX - num));
            double num4 = Math.Min((double)65535.0, (double)(centerX + num));
            double num5 = Math.Max((double)0.0, (double)(centerY - num));
            double num6 = Math.Min((double)65535.0, (double)(centerY + num));
            for (double i = num3; i < num4; i += this._preHeatLineOff) {
                Point start = new Point((double)(ushort)i, sy);
                Point end = new Point((double)(ushort)i, ey);
                verticalLines.Add(new Line(start, end));
            }
            for (double j = num5; j < num6; j += this._preHeatLineOff) {
                Point start = new Point(sx, (double)(ushort)j);
                Point end = new Point(ex, (double)(ushort)j);
                horizonLines.Add(new Line(start, end));
            }
            List<Line> verOrderLines = this.LineOrderSequence(verticalLines);
            List<Line> horOrderLines = this.LineOrderSequence(horizonLines);
            /*  using (FileStream fs = new FileStream("8区.txt", FileMode.OpenOrCreate))
              {
                  StreamWriter sw = new StreamWriter(fs);
                 foreach (var item in horOrderLines)
                  {
                      sw.WriteLine("start:" + item.Start.ToString() + "," + "end:" + item.End.ToString());
                  }
                  sw.Close();
              }*/

            this._horizonFrame = Hack(horOrderLines, out _focusHorizonAstList);
            this.horizonFrameList = _horizonFrame.ToList();
            int len = this._horizonFrame.Count() / 2;
            this._halfHorizonFrame = new Point[len];
            Array.Copy(this._horizonFrame, this._halfHorizonFrame, len);
            this._verticalFrame = Hack(verOrderLines, out _focusverticalAstList);
            //
            //using (FileStream fs = new FileStream("data1.txt", FileMode.Append))
            //{
            //    StreamWriter sw = new StreamWriter(fs);
            //    int x = _verticalFrame.GetLength(0);
            //    for (int i = 0; i < _verticalFrame.GetLength(0); i++)
            //    {

            //        sw.Write(_verticalFrame[i].X.ToString() + ","+ _verticalFrame[i].Y.ToString());

            //        sw.WriteLine();
            //    }
            //    sw.Close();
            //}
            if (verLength < 0 || horLength < 0) {
                throw new ArgumentException("preHeat sweep parameters are incorrect,creat sweep failed");

            }
        }
        public void DeleleHorizonFrame() {
            /* if (length > this._horizonFrame.Length)
             {
                 throw new ArgumentOutOfRangeException("Delete length out of array bounds error");
             }*/
            if (_horizonFrame.Length == 0) {
                return;
            }
            List<Point> hPointList = _horizonFrame.ToList();
            int beginIndex = _horizonFrame.Length / 2;
            hPointList.RemoveRange(beginIndex, _horizonFrame.Length - beginIndex);
            _horizonFrame = hPointList.ToArray();
        }
        public void ResumeHorizonFrame() {
            _horizonFrame = horizonFrameList.ToArray();
        }
        //读取前半部分数据
        public void ReadHalfHorizon(ref double[,] scans, int startIndex, int length) {
            if (length > scans.GetLength(0)) {
                throw new ArgumentOutOfRangeException("Reading out of array bounds error");
            }
            if (scans.GetLength(1) != this._preHeatScan.GetLength(0)) {
                throw new FormatException("Not correct scan format");
            }
            int num = this._halfHorizonFrame.Length;
            double beamVal = 32000;
            for (int i = 0; i < length; i++) {
                Point point = this._halfHorizonFrame[this._halfHorizonIndex];
                FocusAst focusAst = this._focusHorizonAstList[this._halfHorizonIndex++];
                if (_isPreHeat) {
                    if (_scanHorizonTimes > 0) {
                        beamVal = _beamValue;//Parameter.PreHeat.BeamValue;
                    }
                    else {
                        beamVal = 32000;
                    }
                }
                else {
                    beamVal = _beamValue;
                }
                scans[startIndex + i, 0] = point.X;
                scans[startIndex + i, 1] = point.Y;
                scans[startIndex + i, 2] = focusAst.focus; //this._preHeatScan[2];//聚焦
                scans[startIndex + i, 3] = this._preHeatScan[3];
                scans[startIndex + i, 4] = this._preHeatScan[4];
                scans[startIndex + i, 5] = beamVal;//this._preHeatScan[5];//power
                scans[startIndex + i, 6] = focusAst.astX;//astX
                scans[startIndex + i, 7] = focusAst.astY; //astY
                if (this._halfHorizonIndex == num) {
                    this._halfHorizonIndex = 0;
                }
            }
        }
        public void ReadHorizon(ref double[,] scans, int startIndex, int length) {
            if (length > scans.GetLength(0)) {
                throw new ArgumentOutOfRangeException("Reading out of array bounds error");
            }
            if (scans.GetLength(1) != this._preHeatScan.GetLength(0)) {
                throw new FormatException("Not correct scan format");
            }
            int num = this._horizonFrame.Length;
            double beamVal = 32000;
            for (int i = 0; i < length; i++) {
                Point point = this._horizonFrame[this._scanHorizonIndex];
                FocusAst focusAst = this._focusHorizonAstList[this._scanHorizonIndex++];
                if (_isPreHeat) {
                    if (_scanHorizonTimes > 0) {
                        beamVal = _beamValue;//Parameter.PreHeat.BeamValue;
                    }
                    else {
                        beamVal = 32000;
                    }
                }
                else {
                    beamVal = _beamValue;
                }
                scans[startIndex + i, 0] = point.X;
                scans[startIndex + i, 1] = point.Y;
                scans[startIndex + i, 2] = focusAst.focus; //this._preHeatScan[2];//聚焦
                scans[startIndex + i, 3] = this._preHeatScan[3];
                scans[startIndex + i, 4] = this._preHeatScan[4];
                scans[startIndex + i, 5] = beamVal;//this._preHeatScan[5];//power
                scans[startIndex + i, 6] = focusAst.astX;//astX
                scans[startIndex + i, 7] = focusAst.astY; //astY
                if (this._scanHorizonIndex == num) {
                    this._scanHorizonIndex = 0;
                    this._scanHorizonTimes++;
                }
            }
        }
        public void ReadVertical(ref double[,] scans, int startIndex, int length) {
            if ((length - startIndex) > scans.GetLength(0)) {
                throw new ArgumentOutOfRangeException("Reading out of array bounds error");
            }
            if (scans.GetLength(1) != this._preHeatScan.GetLength(0)) {
                throw new FormatException("Not correct scan format");
            }
            int num = this._verticalFrame.Length;
            double beamVal = 32000;
            for (int i = 0; i < length; i++) {
                if (_isPreHeat) {
                    if (_scanVerticalTimes > 0)//第一遍扫描的时候，将下束屏蔽掉
{
                        beamVal = _beamValue;//Parameter.PreHeat.BeamValue;
                    }
                    else {
                        beamVal = 32000;
                    }
                }
                else {
                    beamVal = _beamValue;
                }
                Point point = this._verticalFrame[this._scanVerticalnIndex];
                FocusAst focusAst = this._focusverticalAstList[this._scanVerticalnIndex++];
                scans[startIndex + i, 0] = point.X;
                scans[startIndex + i, 1] = point.Y;
                scans[startIndex + i, 2] = focusAst.focus; //this._preHeatScan[2];//聚焦
                scans[startIndex + i, 3] = this._preHeatScan[3];
                scans[startIndex + i, 4] = this._preHeatScan[4];
                scans[startIndex + i, 5] = beamVal;//this._preHeatScan[5];//power
                scans[startIndex + i, 6] = focusAst.astX;//astX
                scans[startIndex + i, 7] = focusAst.astY;//astY
                if (this._scanVerticalnIndex == num) {
                    this._scanVerticalnIndex = 0;
                    _scanVerticalTimes++;
                }
            }
        }
        public int horLength =>
            this._horizonFrame.Length;
        public int halfHorLength =>
            this._halfHorizonFrame.Length;
        public int verLength =>
            this._verticalFrame.Length;
        private Point[] Hack(List<Line> lines, out List<FocusAst> focusAstlist) {
            List<Point> list = new List<Point>();
            focusAstlist = new List<FocusAst>();
            double rest = 0.0;
            Line lastLine = new Line();
            List<FocusAst> focusAsts;
            for (int i = 0; i < lines.Count; i++) {
                Line line = lines[i];
                if (i > 0) {
                    list.AddRange(Jump(lastLine.End, line.Start, out focusAsts));
                    focusAstlist.AddRange(focusAsts);
                }
                list.AddRange(this.Hack(line, ref rest, out focusAsts));
                focusAstlist.AddRange(focusAsts);
                lastLine = line;
            }
            return list.ToArray();
        }
        private List<Point> Jump(Point start, Point end, out List<FocusAst> focusAsts) {
            focusAsts = new List<FocusAst>();
            double addX = 0, addY = 0;
            Line jumpLine = new Line(start, end);
            int jumpPointsNum = 200;//要求插点时间为200us
            //计算插点个数
            int num = (int)(jumpPointsNum * this._preHeatFrequency / 1000000);//除以1000000，将s转为us
            double lineX1 = jumpLine.Dx * 0.85;
            double lineX2 = jumpLine.Dx - lineX1;
            double lineY1 = jumpLine.Dy * 0.85;
            double lineY2 = jumpLine.Dy - lineY1;
            if (num != 1) {
                addX = lineX2 / (num - 1);
                addY = lineY2 / (num - 1);
            }
            List<Point> jumpPoints = new List<Point>();
            for (int i = 0; i < num; i++)//从0开始，起点要扫一次
            {
                ushort x = (ushort)(start.X + i * addX + lineX1);
                ushort y = (ushort)(start.Y + i * addY + lineY1);
                jumpPoints.Add(new Point(x, y));
                ushort focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius((uint)x, (uint)y)) + _focusOffset);
                ushort astX = (ushort)(StaticTool.CaculateLinerVal((uint)x, (uint)y, true));
                ushort astY = (ushort)(StaticTool.CaculateLinerVal((uint)x, (uint)y, false));
                focusAsts.Add(new FocusAst(focus, astX, astY));
            }
            return jumpPoints;
        }
        private List<Point> Hack(Line line, ref double rest, out List<FocusAst> focusAsts) {
            double a = ((line.Length / ((double)this._preHeatSpeed)) * this._preHeatFrequency) - rest;//点的个数减去偏移
            double num3 = line.Dx / a;//x增量
            double num4 = line.Dy / a;//y增量
            double x = line.Start.X;//起点x
            double y = line.Start.Y;//起点y
            List<Point> list = new List<Point>();
            focusAsts = new List<FocusAst>();
            for (int i = 0; i < a; i++) {
                ushort num8 = (ushort)((num3 * (i + rest)) + x);
                ushort num9 = (ushort)((num4 * (i + rest)) + y);
                ushort focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius((uint)num8, (uint)num9)) + _focusOffset);
                ushort astX = (ushort)(StaticTool.CaculateLinerVal((uint)num8, (uint)num9, true));
                ushort astY = (ushort)(StaticTool.CaculateLinerVal((uint)num8, (uint)num9, false));
                list.Add(new Point((double)num8, (double)num9));
                focusAsts.Add(new FocusAst(focus, astX, astY));
            }
            rest = Math.Ceiling(a) - a;
            return list;
        }
        private List<Line> LineOrderSequence(List<Line> OrderLines) {
            List<Line> list = new List<Line>();
            int count = OrderLines.Count();
            int num = Math.Min(this._preHeatLineOrder, count);
            for (int i = 0; i < num; i++) {
                for (int j = i; j < count; j += this._preHeatLineOrder) {
                    list.Add(OrderLines[j]);
                }
            }
            return list;
        }
        internal struct Point {
            public double X;
            public double Y;
            public Point(double x, double y) {
                this.X = x;
                this.Y = y;
            }

            public override string ToString() =>
                $"({this.X}, {this.Y})";
        }
        internal struct Line {
            public Point Start;
            public Point End;
            public Line(Point start, Point end) {
                this.End = end;
                this.Start = start;
            }
            public double Length {
                get {
                    double dx = this.Dx;
                    double dy = this.Dy;
                    return Math.Sqrt((dx * dx) + (dy * dy));
                }
            }
            public double Dx =>
                (this.End.X - this.Start.X);
            public double Dy =>
                (this.End.Y - this.Start.Y);
        }
    }
}

