using System;
using System.Collections.Generic;
using System.Text;

namespace Program
{
    public struct Point
    {
        public double X;
        public double Y;
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public Point(Point pt)
        {
            this.X = pt.X;
            this.Y = pt.Y;
        }

        public override string ToString() =>
            $"({this.X}, {this.Y})";
    }
    public struct Line : IDisposable
    {
        public Point Start;
        public Point End;
        public Line(Point start, Point end)
        {
            this.End = end;
            this.Start = start;
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
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
