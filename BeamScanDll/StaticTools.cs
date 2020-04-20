using System;
using System.Collections.Generic;
using System.Text;

namespace BeamScanDll
{
   public class StaticTool
    {

        public static uint CaculateFocus(uint radus)
        {
            double a = 0;
            if (Parameter.CalibFocus.R1 != 0)
            {
                a = (double)(Parameter.CalibFocus.R1 / (Parameter.CalibFocus.F0 - Parameter.CalibFocus.F1));
            }
            else
            {
                //MessageBox(_T("输入有误，R1不能为0，请重新输入"));
                return (uint)Parameter.CalibFocus.F0;
            }
            double result = (double)(Parameter.CalibFocus.F0 - radus / a);
            if (result > 65535)
            {
                result = 65535;
            }
            else if (result < 0)
            {
                result = 0;
            }
            return (uint)result;
        }
        public static double GetRadius(uint x, uint y)
        {
            double dis = (x - 32767) * (x - 32767) + (y - 32767) * (y - 32767);
            return Math.Sqrt(dis);
        }

        public static uint CaculateLinerVal(uint ux, uint uy, bool isAstX)
        {
            //UpdateData(TRUE);
            if (Parameter.Points.X11 >= Parameter.Points.X22 || Parameter.Points.Y11 >= Parameter.Points.Y22)
            {
                throw new Exception("坐标参数有误");
            }
            if (ux < (uint)Parameter.Points.X11 || ux > (uint)Parameter.Points.X22)
            {
               // throw new Exception("X坐标值超出矫正范围");
            }
            if (uy < (uint)Parameter.Points.Y11 || uy > (uint)Parameter.Points.Y22)
            {
               // throw new Exception("Y坐标值超出矫正范围");
            }
            uint ast11, ast12, ast21, ast22;
            if (isAstX)
            {
                ast11 = (uint)Parameter.AstPoint.astX11;
                ast12 = (uint)Parameter.AstPoint.astX12;
                ast21 = (uint)Parameter.AstPoint.astX21;
                ast22 = (uint)Parameter.AstPoint.astX22;
            }
            else
            {
                ast11 = (uint)Parameter.AstPoint.astY11;
                ast12 = (uint)Parameter.AstPoint.astY12;
                ast21 = (uint)Parameter.AstPoint.astY21;
                ast22 = (uint)Parameter.AstPoint.astY22;
            }
            double x1 = (Parameter.Points.X22 - ux) * (Parameter.Points.Y22 - uy);
            double x2 = (Parameter.Points.X22 - Parameter.Points.X11) * (Parameter.Points.Y22 - Parameter.Points.Y11);
            double a1 = (double)(x1 / x2);
            double y1 = (Parameter.Points.Y22 - uy) * (ux - Parameter.Points.X11);
            double y2 = (Parameter.Points.X22 - Parameter.Points.X11) * (Parameter.Points.Y22 - Parameter.Points.Y11);
            double a2 = (double)(y1 / y2);
            double r1 = ast11 * a1 + ast21 * a2;
            //////////////////////////////////////////////////////////////////////////
            double x11 = (Parameter.Points.X22 - ux) * (uy - Parameter.Points.Y11);
            double x22 = (Parameter.Points.X22 - Parameter.Points.X11) * (Parameter.Points.Y22 - Parameter.Points.Y11);
            double a11 = x11 / x22;
            double y11 = (ux - Parameter.Points.X11) * (uy - Parameter.Points.Y11);
            double y22 = (Parameter.Points.X22 - Parameter.Points.X11) * (Parameter.Points.Y22 - Parameter.Points.Y11);
            double a22 = y11 / y22;
            double r2 = ast12 * a11 + ast22 * a22;
            uint r = (uint)(r1 + r2);
            return r;
        }
    }

    public delegate void ShowMessDelegate(string str, bool ret = true);

    public delegate void PowerOffDelegte();
}
