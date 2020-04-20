using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlscEBMProject;
using EBMCtrl2._0.ebmScan;
using BeamScanDll;
using System.Diagnostics;
using EBMCtrl2._0;
using System.IO;

namespace GlscEBMProject.UC
{
    public partial class ManuScan : UserControl,IBeamScanController
    {
        private beamScan _beamScan;
        private bool isAstXAdd;
        private bool isAstXMinus;
        private bool isAstYAdd;
        private bool isAstYMinus;
        private bool isFocusAdd;
        private bool isFocusMinus;
        private bool isXAdd;
        private bool isXMinus;
        private bool isYMinus;
        private bool isYAdd;
        private bool _isDirecStop=false;
        private int flag;
        List<ushort> pointList = new List<ushort>();
        List<ushort> astList = new List<ushort>();
        private string focus;
        private string focus_Add;
        private string focus_Minus;

        public ManuScan()
        {
            InitializeComponent();
            _beamScan = new beamScan();
            ReadParameters();
           // GlobleParameter._eBMBeamScan.DummySweep(800000);
        }

 
        public void BeamStart()
        {
            bool bUseCalibrated = this.chbCal.Checked;
            this._beamScan.X = ushort.Parse(this.txtX.Text);
            this._beamScan.Y = ushort.Parse(this.txtY.Text);
            this._beamScan.Focus = ushort.Parse(this.txtFo.Text);
            this._beamScan.Astig1 = ushort.Parse(this.txtast_X.Text);
            this._beamScan.Astig2 = ushort.Parse(this.txtast_Y.Text);
            this._beamScan.BeamCurrent = ushort.Parse(this.txtBeamValue.Text);
            ReadParameters();
            GlobleParameter._eBMBeamScan.RunSingleOut(bUseCalibrated,_beamScan,ushort.Parse(txtFocusAdd.Text));
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
             this._beamScan.X = ushort.Parse(this.txtX.Text);
            this._beamScan.Y = ushort.Parse(this.txtY.Text);
            this._beamScan.Focus = ushort.Parse(this.txtFo.Text);
            this._beamScan.Astig1 = ushort.Parse(this.txtast_X.Text);
            this._beamScan.Astig2 = ushort.Parse(this.txtast_Y.Text);
            this._beamScan.BeamCurrent = ushort.Parse(this.txtBeamValue.Text);
            WriteSingle();
            MessageBox.Show("参数应用成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

             }
            catch (Exception)
            {

                throw new Exception("参数应用发生错误，请检查参数格式");
            }
        }
        void WriteSingle()
        {
            if (File.Exists(Program.Config))
            {
                IniFiles file = new IniFiles(Program.Config);
                file.IniWritevalue("Powder", "DC_X", txtX.Text.Trim());
                file.IniWritevalue("Powder", "DC_Y", txtY.Text.Trim());
                file.IniWritevalue("Powder", "ASTX", txtast_X.Text.Trim());
                file.IniWritevalue("Powder", "ASTY", txtast_Y.Text.Trim());
                file.IniWritevalue("Powder", "FO", txtF0.Text.Trim());
                file.IniWritevalue("Powder", "BeamVal", txtBeamValue.Text.Trim());//
     
            }
        }


        private void AddValuesTimer_Tick(object sender, EventArgs e)
        {
            if (isAstXAdd)
            {
                ushort.TryParse(txtast_X.Text, out ushort value);
                value += 150;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), true);
                    }
                    else
                    {
                        _beamScan.Astig1 = value;
                    }
                }
                txtast_X.Text = value.ToString();
            }
            else if (isAstXMinus)
            {
                ushort.TryParse(txtast_X.Text, out ushort value);
                value -= 150;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), true);
                    }
                    else
                    {
                        _beamScan.Astig1 = value;
                    }
                }
                txtast_X.Text = value.ToString();
            }
            else if (isAstYAdd)
            {
                ushort.TryParse(txtast_Y.Text, out ushort value);
                value += 150;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), false);
                    }
                    else
                    {
                        _beamScan.Astig2 = value;
                    }
                }
                txtast_Y.Text = value.ToString();
            }
            else if (isAstYMinus)
            {
                ushort.TryParse(txtast_Y.Text, out ushort value);
                value -= 150;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), false);
                    }
                    else
                    {
                        _beamScan.Astig2 = value;
                    }
                }
                txtast_Y.Text = value.ToString();
            }
            else if (isFocusAdd)
            {
                ushort.TryParse(txtFo.Text, out ushort value);
                value += 10;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()))) + ushort.Parse(txtFocusAdd.Text.Trim()));
                    }
                    else
                    {
                        _beamScan.Focus = value;
                    }
                }
                txtFo.Text = value.ToString();
            }
            else if (isFocusMinus)
            {
                ushort.TryParse(txtFo.Text, out ushort value);
                value -= 10;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()))) + ushort.Parse(txtFocusAdd.Text.Trim()));
                    }
                    else
                    {
                        _beamScan.Focus = value;
                    }
                }
                txtFo.Text = value.ToString();
            }
            else if (isXAdd)
            {
                ushort.TryParse(txtX.Text, out ushort value);
                if (value > Parameter.Points.X22)
                {
                    _beamScan.X = (ushort)Parameter.Points.X22;
                    txtX.Text = Parameter.Points.X22.ToString();
                    return;
                }
                value += 50;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.X = value;
                        _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()))) + ushort.Parse(txtFocusAdd.Text.Trim()));
                        _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), true);
                        _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), false);
                    }
                    else
                    {
                        _beamScan.X = value;
                        //Debug.Write("X=" + _beamScan.X.ToString());
                    }
                }
                txtX.Text = value.ToString();
            }
            else if (isXMinus)
            {
                ushort.TryParse(txtX.Text, out ushort value);
                if (value < Parameter.Points.X11)
                {
                    _beamScan.X = (ushort)Parameter.Points.X11;
                    txtX.Text = Parameter.Points.X11.ToString();
                    return;
                }
                value -= 50;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.X = value;
                        _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()))) + ushort.Parse(txtFocusAdd.Text.Trim()));
                        _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), true);
                        _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), false);

                    }
                    else
                    {
                        _beamScan.X = value;
                        Debug.WriteLine("X=" + value.ToString());
                    }
                }
                txtX.Text = value.ToString();
            }
            else if (isYAdd)
            {
                ushort.TryParse(txtY.Text, out ushort value);
                if (value > Parameter.Points.Y22)
                {
                    _beamScan.X = (ushort)Parameter.Points.Y22;
                    txtY.Text = Parameter.Points.Y22.ToString();
                    return;
                }
                value += 50;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Y = value;
                        _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()))) + ushort.Parse(txtFocusAdd.Text.Trim()));
                        _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), true);
                        _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), false);

                    }
                    else
                    {
                        _beamScan.Y = value;
                    }
                }
                txtY.Text = value.ToString();
            }
            else if (isYMinus)
            {
                ushort.TryParse(txtY.Text, out ushort value);
                if (value < Parameter.Points.Y11)
                {
                    _beamScan.X = (ushort)Parameter.Points.Y11;
                    txtY.Text = Parameter.Points.Y11.ToString();
                    return;
                }
                value -= 50;
                if (_beamScan != null)
                {
                    if (chbCal.Checked)
                    {
                        _beamScan.Y = value;
                        _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()))) + ushort.Parse(txtFocusAdd.Text.Trim()));
                        _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), true);
                        _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), false);

                    }
                    else
                    {
                        _beamScan.Y = value;
                    }
                }
                txtY.Text = value.ToString();
            }
            // double[] scan = { _beamScan.X, _beamScan.Y ,_beamScan.Focus,
            //32767,32767, _beamScan.Astig1, _beamScan.Astig2,_beamScan.BeamCurrent};
            double[] scan = { _beamScan.X, _beamScan.Y ,_beamScan.Focus,
           32767,32767,_beamScan.BeamCurrent, _beamScan.Astig1, _beamScan.Astig2};
            //BeamStart(SignalCardModeEnum.SingleOut);
            if (!_isDirecStop)
            {
              GlobleParameter._eBMBeamScan.WriteRaw(scan);
            }
        }
        private void ReadParameters()
        {
            try
            {
                if (!File.Exists(Program.CalibrationPath))
                {
                    File.Create(Program.CalibrationPath);
                }
                IniFiles iniFiles = new IniFiles(Program.CalibrationPath);
                txtX21.Text = iniFiles.IniReadvalue("Calibration", "X21");
                Parameter.Points.X21 = double.Parse(txtX21.Text.Trim());
                txtY21.Text = iniFiles.IniReadvalue("Calibration", "Y21");
                Parameter.Points.Y21 = double.Parse(txtY21.Text.Trim());
                txtX22.Text = iniFiles.IniReadvalue("Calibration", "X22");
                Parameter.Points.X22 = double.Parse(txtX22.Text.Trim());
                txtY22.Text = iniFiles.IniReadvalue("Calibration", "Y22");
                Parameter.Points.Y22 = double.Parse(txtY22.Text.Trim());
                txtX11.Text = iniFiles.IniReadvalue("Calibration", "X11");
                Parameter.Points.X11 = double.Parse(txtX11.Text.Trim());
                txtY11.Text = iniFiles.IniReadvalue("Calibration", "Y11");
                Parameter.Points.Y11 = double.Parse(txtY11.Text.Trim());
                txtX12.Text = iniFiles.IniReadvalue("Calibration", "X12");
                Parameter.Points.X12 = double.Parse(txtX12.Text.Trim());
                txtY12.Text = iniFiles.IniReadvalue("Calibration", "Y12");
                Parameter.Points.Y12 = double.Parse(txtY12.Text.Trim());

                txtastX.Text = iniFiles.IniReadvalue("Calibration", "astX11");
                Parameter.AstPoint.astX11 = double.Parse(txtastX.Text.Trim());
                txtastY.Text = iniFiles.IniReadvalue("Calibration", "astY11");
                Parameter.AstPoint.astY11 = double.Parse(txtastY.Text.Trim());
                Parameter.AstPoint.astX12 = double.Parse(iniFiles.IniReadvalue("Calibration", "astX12"));
                Parameter.AstPoint.astY12 = double.Parse(iniFiles.IniReadvalue("Calibration", "astY12"));
                Parameter.AstPoint.astX21 = double.Parse(iniFiles.IniReadvalue("Calibration", "astX21"));
                Parameter.AstPoint.astY21 = double.Parse(iniFiles.IniReadvalue("Calibration", "astY21"));
                Parameter.AstPoint.astX22 = double.Parse(iniFiles.IniReadvalue("Calibration", "astX22"));
                Parameter.AstPoint.astY22 = double.Parse(iniFiles.IniReadvalue("Calibration", "astY22"));

                focus = txtFocus.Text = iniFiles.IniReadvalue("Calibration", "focus");

                focus_Add = txtFocusAdd.Text = iniFiles.IniReadvalue("Calibration", "focus_Add");
                focus_Minus = txtFocusMinus.Text = iniFiles.IniReadvalue("Calibration", "focus_Minus");
                txtF1.Text = iniFiles.IniReadvalue("Calibration", "F1");
                Parameter.CalibFocus.F1 = double.Parse(txtF1.Text);
                txtF0.Text = iniFiles.IniReadvalue("Calibration", "F0");
                Parameter.CalibFocus.F0 = double.Parse(txtF0.Text);
                txtR1.Text = iniFiles.IniReadvalue("Calibration", "R1");
                Parameter.CalibFocus.R1 = double.Parse(txtR1.Text);

                AddValuesTimer = new System.Windows.Forms.Timer();
                AddValuesTimer.Interval = 100;
                AddValuesTimer.Tick += AddValuesTimer_Tick;

                panelA.BackColor = Color.DarkGreen;
                //btnBeam.BackColor = Color.LightGreen;

                //默认是存取panelA中的坐标
                pointList.AddRange(new ushort[] { (ushort)Parameter.Points.X21, (ushort)Parameter.Points.Y21 });
                astList.AddRange(new ushort[] { (ushort)Parameter.AstPoint.astX21, (ushort)Parameter.AstPoint.astY21 });

                IniFiles file = new IniFiles(Program.Config);
                txtX.Text = file.IniReadvalue("Powder", "DC_X");
                txtY.Text = file.IniReadvalue("Powder", "DC_Y");
                txtast_X.Text = file.IniReadvalue("Powder", "ASTX");
                txtast_Y.Text = file.IniReadvalue("Powder", "ASTY");
                txtFo.Text = file.IniReadvalue("Powder", "FO");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要保存当前设定的值？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               == DialogResult.No)
            {
                return;
            }
            if (!File.Exists(Program.CalibrationPath))
            {
                File.Create(Program.CalibrationPath);
            }
            IniFiles iniFiles = new IniFiles(Program.CalibrationPath);
            iniFiles.IniWritevalue("Calibration", "X21", txtX21.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "Y21", txtY21.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "X22", txtX22.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "Y22", txtY22.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "X11", txtX11.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "Y11", txtY11.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "X12", txtX12.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "Y12", txtY12.Text.Trim());

            iniFiles.IniWritevalue("Calibration", "focus", txtFocus.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "focus_Add", txtFocusAdd.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "focus_Minus", txtFocusMinus.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "F1", txtF1.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "F0", txtF0.Text.Trim());
            iniFiles.IniWritevalue("Calibration", "R1", txtR1.Text.Trim());

            switch (flag)
            {
                case 1:
                    iniFiles.IniWritevalue("Calibration", "astX21", txtastX.Text.Trim());
                    iniFiles.IniWritevalue("Calibration", "astY21", txtastY.Text.Trim());
                    Parameter.AstPoint.astX21 = double.Parse(txtastX.Text.Trim());
                    Parameter.AstPoint.astY21 = double.Parse(txtastY.Text.Trim());
                    break;
                case 2:
                    iniFiles.IniWritevalue("Calibration", "astX22", txtastX.Text.Trim());
                    iniFiles.IniWritevalue("Calibration", "astY22", txtastY.Text.Trim());
                    Parameter.AstPoint.astX22 = double.Parse(txtastX.Text.Trim());
                    Parameter.AstPoint.astY22 = double.Parse(txtastY.Text.Trim());
                    break;
                case 3:
                    iniFiles.IniWritevalue("Calibration", "astX11", txtastX.Text.Trim());
                    iniFiles.IniWritevalue("Calibration", "astY11", txtastY.Text.Trim());
                    Parameter.AstPoint.astX11 = double.Parse(txtastX.Text.Trim());
                    Parameter.AstPoint.astY11 = double.Parse(txtastY.Text.Trim());
                    break;
                case 4:
                    iniFiles.IniWritevalue("Calibration", "astX12", txtastX.Text.Trim());
                    iniFiles.IniWritevalue("Calibration", "astY12", txtastY.Text.Trim());
                    Parameter.AstPoint.astX12 = double.Parse(txtastX.Text.Trim());
                    Parameter.AstPoint.astY12 = double.Parse(txtastY.Text.Trim());
                    break;
                default:
                    break;
            }

            MessageBox.Show("保存完毕", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        enum Orientation
        {
            Up,
            Down,
            Left,
            Right
        }

        void SetDirection(Orientation orit)
        {
            if (panelA.BackColor == Color.DarkGreen)
            {
                switch (orit)
                {
                    case Orientation.Down:
                        flag = 3;
                        panelA.BackColor = Color.DarkGray;
                        panelC.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX11.ToString();
                        txtastY.Text = Parameter.AstPoint.astY11.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX11.Text.Trim()), ushort.Parse(txtY11.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX11;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY11;
                        }
                        break;
                    case Orientation.Right:
                        flag = 2;
                        panelA.BackColor = Color.DarkGray;
                        panelB.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX22.ToString();
                        txtastY.Text = Parameter.AstPoint.astY22.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX22.Text.Trim()), ushort.Parse(txtY22.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX22;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY22;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (panelB.BackColor == Color.DarkGreen)
            {
                switch (orit)
                {
                    case Orientation.Down:
                        flag = 4;
                        panelB.BackColor = Color.DarkGray;
                        panelD.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX12.ToString();
                        txtastY.Text = Parameter.AstPoint.astY12.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX12.Text.Trim()), ushort.Parse(txtY12.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX12;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY12;
                        }
                        break;
                    case Orientation.Left:
                        flag = 1;
                        panelB.BackColor = Color.DarkGray;
                        panelA.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX21.ToString();
                        txtastY.Text = Parameter.AstPoint.astY21.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX21.Text.Trim()), ushort.Parse(txtY21.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX21;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY21;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (panelC.BackColor == Color.DarkGreen)
            {
                switch (orit)
                {
                    case Orientation.Up:
                        flag = 1;
                        panelC.BackColor = Color.DarkGray;
                        panelA.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX21.ToString();
                        txtastY.Text = Parameter.AstPoint.astY21.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX21.Text.Trim()), ushort.Parse(txtY21.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX21;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY21;
                        }
                        break;
                    case Orientation.Right:
                        flag = 4;
                        panelC.BackColor = Color.DarkGray;
                        panelD.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX12.ToString();
                        txtastY.Text = Parameter.AstPoint.astY12.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX12.Text.Trim()), ushort.Parse(txtY12.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX12;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY12;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (panelD.BackColor == Color.DarkGreen)
            {
                switch (orit)
                {
                    case Orientation.Up:
                        flag = 2;
                        panelD.BackColor = Color.DarkGray;
                        panelB.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX22.ToString();
                        txtastY.Text = Parameter.AstPoint.astY22.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX22.Text.Trim()), ushort.Parse(txtY22.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX22;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY22;
                        }
                        break;
                    case Orientation.Left:
                        flag = 3;
                        panelD.BackColor = Color.DarkGray;
                        panelC.BackColor = Color.DarkGreen;
                        txtastX.Text = Parameter.AstPoint.astX11.ToString();
                        txtastY.Text = Parameter.AstPoint.astY11.ToString();
                        pointList.Clear();
                        pointList.AddRange(new ushort[] { ushort.Parse(txtX11.Text.Trim()), ushort.Parse(txtX11.Text.Trim()) });
                        if (_beamScan != null)
                        {
                            _beamScan.X = pointList[0];
                            _beamScan.Y = pointList[1];
                            _beamScan.Astig1 = (ushort)Parameter.AstPoint.astX11;
                            _beamScan.Astig2 = (ushort)Parameter.AstPoint.astY11;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void BtnLeft_Click(object sender, EventArgs e)
        {
            SetDirection(Orientation.Left);
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            SetDirection(Orientation.Up);
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            SetDirection(Orientation.Right);
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            SetDirection(Orientation.Down);
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tx.Text.Trim()) || string.IsNullOrEmpty(ty.Text.Trim()))
            {
                return;
            }
            testX.Text = StaticTool.CaculateLinerVal(uint.Parse(tx.Text.Trim()), uint.Parse(ty.Text.Trim()), true).ToString();
            testY.Text = StaticTool.CaculateLinerVal(uint.Parse(tx.Text.Trim()), uint.Parse(ty.Text.Trim()), false).ToString();
        }

    

        private void tabControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (chbKey.Checked && this.tabControl2.SelectedIndex== 0)
            {
                switch (e.KeyCode)
                {
                    case Keys.E://astX 
                        isAstXAdd = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.D:
                        isAstXMinus = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.R://astY 
                        isAstYAdd = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.F:
                        isAstYMinus = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.W:
                        isFocusAdd = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.S:
                        isFocusMinus = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.Up:
                        isXAdd = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.Down:
                        isXMinus = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.Left:
                        txtY.Focus();
                        isYMinus = true;
                        AddValuesTimer.Start();
                        break;
                    case Keys.Right:
                        isYAdd = true;
                        AddValuesTimer.Start();
                        break;
                    default:
                        break;
                }
            }
        }

        private void tabControl2_KeyUp(object sender, KeyEventArgs e)
        {
            if (chbKey.Checked)
            {
                switch (e.KeyCode)
                {
                    case Keys.E://astX 
                        isAstXAdd = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.D:
                        isAstXMinus = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.R:
                        isAstYAdd = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.F:
                        isAstYMinus = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.W:
                        isFocusAdd = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.S:
                        isFocusMinus = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.Up:
                        isXAdd = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.Down:
                        isXMinus = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.Left:
                        isYMinus = false;
                        AddValuesTimer.Stop();
                        break;
                    case Keys.Right:
                        isYAdd = false;
                        AddValuesTimer.Stop();
                        break;
                    default:
                        break;
                }
            }
        }
        private void tabControl2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 && this.tabControl2.SelectedIndex ==0)
            {
                btnApply_Click(null, null);
            }
        }

        private void ChbCal_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCal.Checked && _beamScan != null)
            {

                _beamScan.Astig1 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), true);
                _beamScan.Astig2 = (ushort)StaticTool.CaculateLinerVal(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()), false);

                _beamScan.Focus = (ushort)(StaticTool.CaculateFocus((uint)StaticTool.GetRadius(ushort.Parse(txtX.Text.Trim()), ushort.Parse(txtY.Text.Trim()))) + ushort.Parse(txtFocusAdd.Text.Trim()));
            }
        }

        private void ChbKey_CheckedChanged(object sender, EventArgs e)
        {
            txtastX.ReadOnly = txtastY.ReadOnly = txtFocus.ReadOnly = (sender as CheckBox).Checked;

        }
    }

    public class beamScan : IBeamScan
    {
        private ushort x;
        private ushort y;
        private ushort focus;
        private ushort ast1;
        private ushort ast2;
        private ushort beamCurrent;
        public string FullName => "ebmScan";

        public ushort X { get => x; set => x=value; }
        public ushort Y { get => y; set => y=value; }
        public ushort Focus { get => focus; set => focus = value; }
        public ushort Astig1 { get => ast1; set => ast1 = value; }
        public ushort Astig2 { get => ast2; set => ast2 = value; }
        public ushort BeamCurrent { get=>beamCurrent; set => beamCurrent = value; }
        public  beamScan()
        {
            this.x = 32767;
            this.y = 32767;
            this.focus = 32767;
            this.ast1 = 32767;
            this.ast2 = 32767;
            this.beamCurrent = 32000;
        }
    }

}
