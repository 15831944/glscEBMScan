using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeamScanDll;
using GlscEBMProject;
using EBMCtrl2._0;
using System.IO;
using System.Diagnostics;

namespace GlscEBMProject.UC
{
    public partial class AreaScan : UserControl, IBeamScanController
    {
       
        public AreaScan()
        {
            InitializeComponent();
            ReadParameters();
        }

        public void BeamStart()
        {
           bool Xdirec = this.rbXdirec.Checked ;
            string str=Xdirec==true? "开始X方向预热": "开始Y方向预热";
            GlobleParameter.DelShowMess(str);
            GlobleParameter._eBMBeamScan.RunPeaHeatOut(ref Parameter._preHeatPara, Xdirec);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                Parameter.PreHeat.Size = double.Parse(this.txtLineSize.Text);
                Parameter.PreHeat.LineOrder = uint.Parse(this.txtLineOrder.Text);
                Parameter.PreHeat.LineOffset = double.Parse(this.txLineOffset.Text);
                Parameter.PreHeat.Speed = double.Parse(this.txtpreHeatSpeed.Text);
                Parameter.Frequency = UInt32.Parse(this.txtpreHeatFreq.Text);
                Parameter._preHeatPara.focusOffset = double.Parse(this.txtFocusOffs.Text);
                Parameter._preHeatPara.scanVolt = double.Parse(this.txtVolt.Text);
                Parameter._preHeatPara.scanCount = uint.Parse(this.txtpreheatCout.Text);
                WritePara();
                MessageBox.Show("参数应用成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            catch (Exception)
            {

                throw new Exception("参数应用发生错误，请检查参数格式");
            }
        }
        void WritePara()
        {
            if (File.Exists(Program.StrPreheatParaPath))
            {
                IniFiles file = new IniFiles(Program.StrPreheatParaPath);
                file.IniWritevalue("preHeatPara", "preheatCout", txtpreheatCout.Text.Trim());
                file.IniWritevalue("preHeatPara", "preHeatFreq", txtpreHeatFreq.Text.Trim());
                file.IniWritevalue("preHeatPara", "preHeatSpeed", txtpreHeatSpeed.Text.Trim());
                file.IniWritevalue("preHeatPara", "FocusOffs", txtFocusOffs.Text.Trim());
                file.IniWritevalue("preHeatPara", "Volt", txtVolt.Text.Trim());
                file.IniWritevalue("preHeatPara", "LineSize", txtLineSize.Text.Trim());
                file.IniWritevalue("preHeatPara", "LineOrder", txtLineOrder.Text.Trim());
                file.IniWritevalue("preHeatPara", "LineOffset", txLineOffset.Text.Trim());

            }
        }
        private void ReadParameters()
        {
            try
            {
                if (!File.Exists(Program.StrPreheatParaPath))
                {
                    File.Create(Program.StrPreheatParaPath);
                }
                IniFiles iniFiles = new IniFiles(Program.StrPreheatParaPath);
                txtpreheatCout.Text = iniFiles.IniReadvalue("preHeatPara", "preheatCout");
                Parameter._preHeatPara.scanCount = uint.Parse(txtpreheatCout.Text);
                txtpreHeatFreq.Text = iniFiles.IniReadvalue("preHeatPara", "preHeatFreq");
                Parameter.Frequency = uint.Parse(this.txtpreHeatFreq.Text);
                txtpreHeatSpeed.Text = iniFiles.IniReadvalue("preHeatPara", "preHeatSpeed");
                Parameter.PreHeat.Speed = double.Parse(txtpreHeatSpeed.Text);
                txtFocusOffs.Text = iniFiles.IniReadvalue("preHeatPara", "FocusOffs");
                Parameter._preHeatPara.focusOffset = double.Parse(txtFocusOffs.Text);
                txtVolt.Text = iniFiles.IniReadvalue("preHeatPara", "Volt");
                Parameter._preHeatPara.scanVolt = double.Parse(this.txtVolt.Text);
                txtLineSize.Text = iniFiles.IniReadvalue("preHeatPara", "LineSize");
                Parameter.PreHeat.Size = double.Parse(txtLineSize.Text);
                txtLineOrder.Text = iniFiles.IniReadvalue("preHeatPara", "LineOrder");
                Parameter.PreHeat.LineOrder = uint.Parse(txtLineOrder.Text);
                txLineOffset.Text = iniFiles.IniReadvalue("preHeatPara", "LineOffset");
                Parameter.PreHeat.LineOffset = double.Parse(txLineOffset.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


    }
}
