using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlscEBMProject.UC;
using BeamScanDll;
namespace GlscEBMProject
{
    public partial class MainFm : Form
    {
        ManuScan _manuScan;
        CADScan _cADScan;
        AreaScan _areaScan;
        IBeamScanController beamScan=null;
        public MainFm()
        {
            InitializeComponent();
            GlobleParameter._eBMBeamScan = new EBMBeamScan();
            _manuScan = new ManuScan();
            LoadUserControl(_manuScan);//默认加载手动输入
            GlobleParameter.DelShowMess += ShowInfo;
            GlobleParameter._eBMBeamScan.OnOperation += ShowInfo;

        }

        private void toolStriptn_Click(object sender, EventArgs e)
        {
            try
            {
                var obj = (sender as ToolStripButton).Tag;
                if (obj == null) return;
                var ctrl = (EShortCut)Enum.Parse(typeof(EShortCut), obj.ToString());
                switch (ctrl)
                {
                    case EShortCut.ManuScan:
                        if(_manuScan==null)
                        {
                            _manuScan = new ManuScan();
                        }
                        LoadUserControl(_manuScan);
                        break;
                    case EShortCut.AreaScan:
                        if (_areaScan == null)
                        {
                            _areaScan = new AreaScan();
                        }
                        LoadUserControl(_areaScan);
                        break;
                    case EShortCut.CADScan:
                        if (_cADScan == null)
                        {
                            _cADScan = new CADScan();
                        }
                        LoadUserControl(_cADScan);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        void LoadUserControl(object ucObj)
        {
            try
            {
                if (splitMain.Panel1.Controls.Count > 0)
                {
                    foreach (Control uc in splitMain.Panel1.Controls)
                    {
                        uc.Visible = false;
                        if (uc.Name == ((UserControl)ucObj).Name)
                        {
                            uc.Visible = true;
                            return;
                        }
                    }
                }
                splitMain.Panel1.Controls.Add((UserControl)ucObj);
                ((UserControl)ucObj).Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
               // LogHelper.Error(ex.ToString());
            }
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            if (this.rbDirec.Checked)
            {
                if (_manuScan==null)
                {
                    _manuScan = new ManuScan();
                }
                 beamScan = _manuScan;
            }
            else if (this.rbPreheat.Checked)
            {
                if (_areaScan == null)
                    _areaScan = new AreaScan();
                beamScan = _areaScan;
            }
            else
            {
                if (_cADScan == null)
                    _cADScan = new CADScan();
                beamScan = _cADScan;
            }   
            beamScan.BeamStart();

        }

        private void btnStoptScan_Click(object sender, EventArgs e)
        {
            GlobleParameter._eBMBeamScan.StopBeamOut();
            ShowInfo("停止扫描...");

        }
       public void ShowInfo(string info, bool isfore = true)
        {
            LogHelper.Info(info);
            if (isfore)
            {
               this.toolStripStatusInfo.Text = info;
            }
        }

        private void BtnDummySweep_Click(object sender, EventArgs e)
        {
            GlobleParameter._eBMBeamScan.DummySweep(8000);
            ShowInfo("空扫...");
        }
    }
}
