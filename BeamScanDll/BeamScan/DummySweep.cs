namespace EBMCtrl2._0.ebmScan
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using BeamScanDll;
    internal class DummySweep
    {
        private ushort m_Size;
        private float m_LineOffset;
        private int m_LineOrder;
        private float m_Speed;
        private double[] m_DummyScan;
        private double m_Frequency;
        private int m_ScanIndex;
        private Point[] m_Scans;

        internal DummySweep(ushort size, float lineOffset, int lineOrder, float speed, double[] dummyScan, double frequency)
        {
            if (size <= 1)
            {
                throw new ArgumentOutOfRangeException("Size must be greater than zero");
            }
            if (((lineOffset < 0.1) || (lineOffset > 100000f)) || float.IsNaN(lineOffset))
            {
                throw new ArgumentOutOfRangeException("LineOffset must be greater than or equal to 0.1 and max 100000");
            }
            if ((lineOrder <= 0) || (lineOrder > 0xf4240))
            {
                throw new ArgumentOutOfRangeException("LineOrder must be greater than zero and max 1000000");
            }
            if ((speed <= 100f) || float.IsNaN(speed))
            {
                throw new ArgumentOutOfRangeException("Speed must be greater than 100");
            }
            if (dummyScan.Length != 8)
            {
                throw new ArgumentOutOfRangeException("Dummy scan length must be 8");
            }
            if (((frequency <= 0.0) || (frequency > 1000000.0)) || double.IsNaN(frequency))
            {
                throw new ArgumentOutOfRangeException("Frequency must be greater than zero and max 1000000");
            }
            this.m_Size = size;
            this.m_LineOffset = lineOffset;
            this.m_LineOrder = lineOrder;
            this.m_Speed = speed;
            this.m_DummyScan = dummyScan;
            this.m_Frequency = frequency;
            this.Create();
        }

        private void Create()
        {
            double num = this.m_DummyScan[0];//指定中心点
            double num2 = this.m_DummyScan[1];
            float num3 = ((float) this.m_Size) / 2f;
            double y = Math.Max((double) 0.0, (double) ((num2 - num3) + 0.5));
            double num5 = Math.Min((double) 65535.0, (double) ((num2 + num3) + 0.5));
            double x = Math.Max((double) 0.0, (double) ((num - num3) + 0.5));
            double num7 = Math.Min((double) 65535.0, (double) ((num + num3) + 0.5));
            List<Line> verticallyLines = new List<Line>();
            List<Line> list2 = new List<Line>();
            double num8 = Math.Max((double) 0.0, (double) (num - num3));
            double num9 = Math.Min((double) 65535.0, (double) (num + num3));
            double num10 = Math.Max((double) 0.0, (double) (num2 - num3));
            double num11 = Math.Min((double) 65535.0, (double) (num2 + num3));
            for (double i = num8; i < num9; i += this.m_LineOffset)
            {
                Point start = new Point((double) ((ushort) i), y);
                Point end = new Point((double) ((ushort) i), num5);
                verticallyLines.Add(new Line(start, end));
            }
            for (double j = num10; j < num11; j += this.m_LineOffset)
            {
                Point start = new Point(x, (double) ((ushort) j));
                Point end = new Point(num7, (double) ((ushort) j));
                list2.Add(new Line(start, end));
            }
            List<Line> lines = this.LineOrderSequence(verticallyLines);
            lines.AddRange(this.LineOrderSequence(list2));
            this.Hack(lines);
            if (this.Length == 0)
            {
                throw new ArgumentException("Dummy Sweep parameters are incorrect, no dummy sweep can be generated.");
            }
        }

        private void Hack(List<Line> lines)
        {
            List<Point> list = new List<Point>();
            double rest = 0.0;
            for (int i = 0; i < lines.Count; i++)
            {
                Line line = lines[i];
                list.AddRange(this.Hack(line, ref rest));
            }
            this.m_Scans = list.ToArray();
        }

        private List<Point> Hack(Line line, ref double rest)
        {
            double a = ((line.Length / ((double) this.m_Speed)) * this.m_Frequency) - rest;//点的个数减去偏移
            double num3 = line.Dx / a;//x增量
            double num4 = line.Dy / a;//y增量
            double x = line.Start.X;//起点x
            double y = line.Start.Y;//起点y
            List<Point> list = new List<Point>();
            for (int i = 0; i < a; i++)
            {
                ushort num8 = (ushort) ((num3 * (i + rest)) + x);
                ushort num9 = (ushort) ((num4 * (i + rest)) + y);
                list.Add(new Point((double) num8, (double) num9));
            }
            rest = Math.Ceiling(a) - a;
            return list;
        }

        private List<Line> LineOrderSequence(List<Line> verticallyLines)
        {
            List<Line> list = new List<Line>();
            int count = verticallyLines.Count;
            int num2 = Math.Min(this.m_LineOrder, count);
            for (int i = 0; i < num2; i++)
            {
                for (int j = i; j < count; j += this.m_LineOrder)
                {
                    list.Add(verticallyLines[j]);
                }
            }
            return list;
        }

        public void Read(ref double[,] scans, int startIndex, int length)
        {
            if ((length - startIndex) > scans.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("Reading out of array bounds error");
            }
            if (scans.GetLength(1) != this.m_DummyScan.GetLength(0))
            {
                throw new FormatException("Not correct scan format");
            }
            int num = this.m_Scans.Length;
            double zaro = 32767;
            for (int i = 0; i < length; i++)
            {
                Point point = this.m_Scans[this.m_ScanIndex++];
                scans[startIndex + i, 0] = zaro;
                scans[startIndex + i, 1] = zaro;
                scans[startIndex + i, 2] = zaro;//StaticTool.CaculateFocus((uint)StaticTool.GetRadius((uint)point.X, (uint)point.Y)) + Parameter.PreHeat.FocusOffset;//this.m_DummyScan[2];
                scans[startIndex + i, 3] = zaro;
                scans[startIndex + i, 4] = zaro;
                scans[startIndex + i, 5] = zaro;//this.m_DummyScan[5];
                scans[startIndex + i, 6] = zaro;
                scans[startIndex + i, 7] = zaro;
                if (this.m_ScanIndex == num)
                {
                    this.m_ScanIndex = 0;
                }
            }
        }

        public int Length =>
            this.m_Scans.Length;

        /*   [TestFixture]
           public class DummySweepTest
           {
               private ushort[] okSize = new ushort[] { 2, 5, 700, 0x1388 };
               private float[] okLineOffset = new float[] { 10f, 500f, 100000f };
               private int[] okLineOrder = new int[] { 1, 2, 3, 0x3e8, 0xf4240 };
               private float[] okSpeed = new float[] { 999f, 1000000f };
               private double[][] okDummyScan;
               private double[] okFrequency;
               private ushort[] errorSize;
               private float[] errorLineOffset;
               private int[] errorLineOrder;
               private float[] errorSpeed;
               private double[][] errorDummyScan;
               private double[] errorFrequency;

               public DummySweepTest()
               {
                   double[][] numArray2 = new double[3][];
                   numArray2[0] = new double[8];
                   double[] numArray4 = new double[8];
                   numArray4[0] = 10.0;
                   numArray4[1] = 10.0;
                   numArray2[1] = numArray4;
                   numArray2[2] = new double[] { 65535.0, 65535.0, 65535.0, 65535.0, 65535.0, 65535.0, 65535.0, 65535.0 };
                   this.okDummyScan = numArray2;
                   this.okFrequency = new double[] { 0.001, 67.0, 999.0 };
                   ushort[] numArray5 = new ushort[2];
                   numArray5[1] = 1;
                   this.errorSize = numArray5;
                   this.errorLineOffset = new float[] { -0.001f, 0f, -500f, float.NaN, float.MaxValue };
                   int[] numArray6 = new int[3];
                   numArray6[1] = -2147483648;
                   numArray6[2] = 0x7fffffff;
                   this.errorLineOrder = numArray6;
                   this.errorSpeed = new float[] { 0f, -5f, float.MinValue, float.NaN };
                   double[][] numArray7 = new double[2][];
                   numArray7[0] = new double[5];
                   numArray7[1] = new double[] { 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0 };
                   this.errorDummyScan = numArray7;
                   this.errorFrequency = new double[] { 0.0, -0.1, -678.0, double.MinValue, double.NaN };
               }

               [Test]
               public void ConstructorTestInvalidDummyScan()
               {
                   bool flag = false;
                   foreach (ushort num in this.okSize)
                   {
                       foreach (float num2 in this.okLineOffset)
                       {
                           foreach (int num3 in this.okLineOrder)
                           {
                               foreach (float num4 in this.okSpeed)
                               {
                                   foreach (double[] numArray in this.errorDummyScan)
                                   {
                                       foreach (double num5 in this.okFrequency)
                                       {
                                           try
                                           {
                                               new DummySweep(num, num2, num3, num4, numArray, num5);
                                               flag = true;
                                           }
                                           catch
                                           {
                                           }
                                           if (flag)
                                           {
                                               throw new Exception("Some error dummy scan parameters are valid!");
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }
               }

               [Test]
               public void ConstructorTestInvalidFrequency()
               {
                   bool flag = false;
                   foreach (ushort num in this.okSize)
                   {
                       foreach (float num2 in this.okLineOffset)
                       {
                           foreach (int num3 in this.okLineOrder)
                           {
                               foreach (float num4 in this.okSpeed)
                               {
                                   foreach (double[] numArray in this.okDummyScan)
                                   {
                                       foreach (double num5 in this.errorFrequency)
                                       {
                                           try
                                           {
                                               new DummySweep(num, num2, num3, num4, numArray, num5);
                                               flag = true;
                                           }
                                           catch
                                           {
                                           }
                                           if (flag)
                                           {
                                               throw new Exception("Some error frequency parameters are valid!");
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }
               }

               [Test]
               public void ConstructorTestInvalidLineOffset()
               {
                   bool flag = false;
                   foreach (ushort num in this.okSize)
                   {
                       foreach (float num2 in this.errorLineOffset)
                       {
                           foreach (int num3 in this.okLineOrder)
                           {
                               foreach (float num4 in this.okSpeed)
                               {
                                   foreach (double[] numArray in this.okDummyScan)
                                   {
                                       foreach (double num5 in this.okFrequency)
                                       {
                                           try
                                           {
                                               new DummySweep(num, num2, num3, num4, numArray, num5);
                                               flag = true;
                                           }
                                           catch
                                           {
                                           }
                                           if (flag)
                                           {
                                               throw new Exception("Some error line offset parameters are valid!");
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }
               }

               [Test]
               public void ConstructorTestInvalidLineOrder()
               {
                   bool flag = false;
                   foreach (ushort num in this.okSize)
                   {
                       foreach (float num2 in this.okLineOffset)
                       {
                           foreach (int num3 in this.errorLineOrder)
                           {
                               foreach (float num4 in this.okSpeed)
                               {
                                   foreach (double[] numArray in this.okDummyScan)
                                   {
                                       foreach (double num5 in this.okFrequency)
                                       {
                                           try
                                           {
                                               new DummySweep(num, num2, num3, num4, numArray, num5);
                                               flag = true;
                                           }
                                           catch
                                           {
                                           }
                                           if (flag)
                                           {
                                               throw new Exception("Some error line order parameters are valid!");
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }
               }

               [Test]
               public void ConstructorTestInvalidSize()
               {
                   bool flag = false;
                   foreach (ushort num in this.errorSize)
                   {
                       foreach (float num2 in this.okLineOffset)
                       {
                           foreach (int num3 in this.okLineOrder)
                           {
                               foreach (float num4 in this.okSpeed)
                               {
                                   foreach (double[] numArray in this.okDummyScan)
                                   {
                                       foreach (double num5 in this.okFrequency)
                                       {
                                           try
                                           {
                                               new DummySweep(num, num2, num3, num4, numArray, num5);
                                               flag = true;
                                           }
                                           catch
                                           {
                                           }
                                           if (flag)
                                           {
                                               throw new Exception("Some error size parameters are valid!");
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }
               }

               [Test]
               public void ConstructorTestInvalidSpeed()
               {
                   bool flag = false;
                   foreach (ushort num in this.okSize)
                   {
                       foreach (float num2 in this.okLineOffset)
                       {
                           foreach (int num3 in this.okLineOrder)
                           {
                               foreach (float num4 in this.errorSpeed)
                               {
                                   foreach (double[] numArray in this.okDummyScan)
                                   {
                                       foreach (double num5 in this.okFrequency)
                                       {
                                           try
                                           {
                                               new DummySweep(num, num2, num3, num4, numArray, num5);
                                               flag = true;
                                           }
                                           catch
                                           {
                                           }
                                           if (flag)
                                           {
                                               throw new Exception("Some error speed parameters are valid!");
                                           }
                                       }
                                   }
                               }
                           }
                       }
                   }
               }

               [Test]
               public void ConstructorTestValid()
               {
                   foreach (ushort num in this.okSize)
                   {
                       foreach (float num2 in this.okLineOffset)
                       {
                           foreach (int num3 in this.okLineOrder)
                           {
                               foreach (float num4 in this.okSpeed)
                               {
                                   foreach (double[] numArray in this.okDummyScan)
                                   {
                                       foreach (double num5 in this.okFrequency)
                                       {
                                           new DummySweep(num, num2, num3, num4, numArray, num5);
                                       }
                                   }
                               }
                           }
                       }
                   }
               }

               [Test]
               public void HackingTest()
               {
                   double[] numArray2 = new double[8];
                   numArray2[0] = 32000.0;
                   numArray2[1] = 32000.0;
                   double[] dummyScan = numArray2;
                   DummySweep sweep = new DummySweep(0x3e8, 1f, 1, 1000f, dummyScan, 1000.0);
                   Assert.AreEqual(sweep.Length, 0x1e8480, "Hacking error in #1");
                   sweep = new DummySweep(2, 5f, 5, 100.1f, dummyScan, 100.09999847412109);
                   Assert.AreEqual(sweep.Length, 4, "Hacking error in #2");
                   sweep = new DummySweep(2, 5f, 5, 1000000f, dummyScan, 10.5);
                   Assert.AreEqual(sweep.Length, 1, "Hacking error in #3");
               }

               [Test, ExpectedException(ExceptionType=typeof(ArgumentOutOfRangeException))]
               public void ReadErrorOutOfRange()
               {
                   double[] dummyScan = new double[] { 30000.0, 30000.0, 40.0, 50.0, 60.0, 70.0, 80.0, 9.0 };
                   DummySweep sweep = new DummySweep(100, 1f, 1, 1000f, dummyScan, 100.0);
                   double[,] scans = new double[500, 8];
                   sweep.Read(ref scans, 0, 0x1f5);
               }

               [ExpectedException(ExceptionType=typeof(FormatException)), Test]
               public void ReadErrorScanFormat()
               {
                   double[] dummyScan = new double[] { 30000.0, 30000.0, 40.0, 50.0, 60.0, 70.0, 80.0, 9.0 };
                   DummySweep sweep = new DummySweep(100, 1f, 1, 1000f, dummyScan, 100.0);
                   double[,] scans = new double[500, 9];
                   sweep.Read(ref scans, 5, 50);
               }

               [Test]
               public void ReadTest()
               {
                   double[] numArray3 = new double[8];
                   numArray3[0] = 32000.0;
                   numArray3[1] = 32000.0;
                   double[] dummyScan = numArray3;
                   DummySweep sweep = new DummySweep(0x3e8, 1f, 1, 1000f, dummyScan, 1000.0);
                   double[,] scans = new double[0x2710, 8];
                   for (int i = 0; i < 50; i++)
                   {
                       sweep.Read(ref scans, 0, 0x2710);
                   }
                   double num2 = scans[0, 0];
                   double num3 = scans[0, 1];
                   Assert.AreEqual(num2, 0x7cf6, "Error in Read");
                   Assert.AreEqual(num3, 0x7b0c, "Error in Read");
                   for (int j = 0; j < 50; j++)
                   {
                       sweep.Read(ref scans, 0, 0x1610);
                   }
                   num2 = scans[0, 0];
                   num3 = scans[0, 1];
                   Assert.AreEqual(num2, 0x7e14, "Error in Read");
                   Assert.AreEqual(num3, 0x7dfc, "Error in Read");
                   sweep = new DummySweep(2, 1f, 1, 1000f, dummyScan, 1005.0);
                   for (int k = 0; k < 50; k++)
                   {
                       sweep.Read(ref scans, 0, 0x1643);
                   }
                   num2 = scans[0, 0];
                   num3 = scans[0, 1];
                   Assert.AreEqual(num2, 0x7d03, "Error in Read");
                   Assert.AreEqual(num3, 0x7d00, "Error in Read");
                   sweep = new DummySweep(2, 5f, 5, 1000000f, dummyScan, 10.5);
                   for (int m = 0; m < 50; m++)
                   {
                       sweep.Read(ref scans, 0, 0x1610);
                   }
                   num2 = scans[0, 0];
                   num3 = scans[0, 1];
                   Assert.AreEqual(num2, 0x7cff, "Error in Read");
                   Assert.AreEqual(num3, 0x7cff, "Error in Read");
               }
           }
           */
        [StructLayout(LayoutKind.Sequential), DebuggerDisplay("{Start} - {End}")]
        internal struct Line
        {
            public DummySweep.Point Start;
            public DummySweep.Point End;
            public Line(DummySweep.Point start, DummySweep.Point end)
            {
                this.Start = start;
                this.End = end;
            }

            public double Length
            {
                get
                {
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

        [StructLayout(LayoutKind.Sequential), DebuggerDisplay("({X}, {Y})")]
        internal struct Point
        {
            public double X;
            public double Y;
            public Point(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString() => 
                $"({this.X}, {this.Y})";
        }
    }
}

