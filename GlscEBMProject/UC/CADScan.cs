using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using netDxf;
using netDxf.Entities;
using BeamScanDll.CADProcess;
using System.IO;
using EBMCtrl2._0;
using BeamScanDll;
namespace GlscEBMProject.UC
{
    public partial class CADScan : UserControl, IBeamScanController
    {
        private netDxf.DxfDocument dxfDocument = new netDxf.DxfDocument();
        private DxfcadReader dxfcadReader=null; 
        public CADScan()
        {
            InitializeComponent();
            ReadParameters();
            dxfcadReader = new DxfcadReader();
            dxfcadReader.delCadInfo += ShowCADinfo;
            
        }

        public DxfDocument DxfDocument { get => dxfDocument; set => dxfDocument = value; }

        public void BeamStart()
        {
            if (dxfDocument!=null)
            {
                GlobleParameter._eBMBeamScan.RunCADOut(uint.Parse(txtScanTime.Text), dxfcadReader);
            }
            
        }
        private void ReadParameters()
        {
            try
            {
                if (!File.Exists(Program.StrCadParaPath))
                {
                    File.Create(Program.StrCadParaPath);
                }
                IniFiles iniFiles = new IniFiles(Program.StrCadParaPath);
                txtCADspeed.Text = iniFiles.IniReadvalue("CADpara", "CADspeed");
                Parameter.cadFilescanPara.Speed = double.Parse(txtCADspeed.Text);
                txtCADfocusOffset.Text = iniFiles.IniReadvalue("CADpara", "CADfocusOffset");
                Parameter.cadFilescanPara.FocusOffset = double.Parse(txtCADfocusOffset.Text);
                txtCADBeamVal.Text = iniFiles.IniReadvalue("CADpara", "CADBeamVal");
                Parameter.cadFilescanPara.BeamValue = double.Parse(txtCADBeamVal.Text);
                txtKp.Text = iniFiles.IniReadvalue("CADpara", "kp");
                txtoffset.Text = iniFiles.IniReadvalue("CADpara", "offset");
                Parameter.dataAdapter.Kp = double.Parse(txtKp.Text);
                txtMinX.Text = iniFiles.IniReadvalue("CADpara", "minx");
                txtMinY.Text = iniFiles.IniReadvalue("CADpara", "miny");
                txtMaxX.Text = iniFiles.IniReadvalue("CADpara", "maxx");
                txtMaxY.Text = iniFiles.IniReadvalue("CADpara", "maxy");
                Parameter.dataAdapter.Offset = double.Parse(txtoffset.Text);
                Parameter.MinCADFileSetValue.X = double.Parse(txtMinX.Text);
                Parameter.MinCADFileSetValue.Y = double.Parse(txtMinY.Text);
                Parameter.MaxCADFileSetValue.X = double.Parse(txtMaxX.Text);
                Parameter.MaxCADFileSetValue.Y = double.Parse(txtMaxY.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CAD工程文件(*.dxf)|*.dxf|所有文件(*.*)|*.*";
            ofd.Title = "打开文件";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = ofd.SafeFileName;
                txtFilePath.Text = ofd.FileName;
                if (!dxfcadReader.ReadCADfile(ofd.FileName))
                {

                    MessageBox.Show("打开文件失败");
                    return;
                }
                ShowCADinfo("文件" + txtFileName.Text+"读取成功",-1);
               // Program._SXFileName = ofd.SafeFileName;
               // int fileNum = SxFileProcessor.sxProject._theProject.Count;
                string path = Path.GetFullPath(@".../...");
                path += "\\Models";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileSystemInfo[] infos = dir.GetFileSystemInfos();
                    foreach (FileSystemInfo i in infos)
                    {
                        if (i is DirectoryInfo)            //判断是否文件夹
                        {
                            DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                            subdir.Delete(true);          //删除子目录和文件
                        }
                        else
                        {
                            if (!i.FullName.Contains("PreHeatBox.xml"))
                            {
                                File.Delete(i.FullName);      //删除指定文件
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                Parameter.cadFilescanPara.BeamValue = double.Parse(txtCADBeamVal.Text);
                Parameter.cadFilescanPara.FocusOffset = double.Parse(txtCADfocusOffset.Text);
                Parameter.cadFilescanPara.Speed = double.Parse(txtCADspeed.Text);
                Parameter.dataAdapter.Kp = double.Parse(txtKp.Text);
                Parameter.dataAdapter.Offset = double.Parse(txtoffset.Text);
                double minx = double.Parse(txtMinX.Text);
                double miny = double.Parse(txtMinY.Text);
                double maxx = double.Parse(txtMaxX.Text);
                double maxy = double.Parse(txtMaxY.Text);
                Parameter.MinCADFileSetValue = new BeamScanDll.Point(minx, miny);
                Parameter.MaxCADFileSetValue = new BeamScanDll.Point(maxx, maxy);
                WritePara();
                MessageBox.Show("参数设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          
        }
        void WritePara()
        {
            if (File.Exists(Program.StrCadParaPath))
            {
                IniFiles file = new IniFiles(Program.StrCadParaPath);
                file.IniWritevalue("CADpara", "CADspeed", txtCADspeed.Text.Trim());
                file.IniWritevalue("CADpara", "CADfocusOffset", txtCADfocusOffset.Text.Trim());
                file.IniWritevalue("CADpara", "CADBeamVal", txtCADBeamVal.Text.Trim());
                file.IniWritevalue("CADpara", "kp", txtKp.Text.Trim());
                file.IniWritevalue("CADpara", "offset", txtoffset.Text.Trim());
                file.IniWritevalue("CADpara", "minx", txtMinX.Text.Trim());
                file.IniWritevalue("CADpara", "miny", txtMinY.Text.Trim());
                file.IniWritevalue("CADpara", "maxx", txtMaxX.Text.Trim());
                file.IniWritevalue("CADpara", "maxy", txtMaxY.Text.Trim());
            }
        
        }
        void ShowCADinfo(string shapType,int Num)
        {
            
            if (Num > 0)
            {
                this.listBoxCadInfo.Items.Add(shapType + Num.ToString());
            }
            else if (Num == -1)
                this.listBoxCadInfo.Items.Add(shapType);
            else if (Num == -2)//警报信息，需要重视
            {
                this.listBoxCadInfo.Items.Add(shapType);
                int num = this.listBoxCadInfo.Items.Count-1;
                listBoxCadInfo.Items[num - 1].BackColor = Color.Red;
            }
        }
    }
}
