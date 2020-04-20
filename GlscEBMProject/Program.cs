using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeamScanDll;
namespace GlscEBMProject
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFm());
        }
        /// <summary>
        /// 默认的静态参数配置文本
        /// </summary>
        public static string Config = Path.Combine(Application.StartupPath, "config.ini");
        /// <summary>
        /// 校准配置文件
        /// </summary>
        public static string CalibrationPath = Path.Combine(Application.StartupPath, "Calibration.ini");
        /// <summary>
        /// XML参数文件
        /// </summary>
        public static string StrParamPath = Path.Combine(Application.StartupPath, "System\\Parameter.xml");
        /// <summary>
        /// CAD参数说明
        /// </summary>
        public static string StrCadParaPath = Path.Combine(Application.StartupPath, "cadPara.ini");
        /// <summary>
        /// 预热参数
        /// </summary>
        public static string StrPreheatParaPath = Path.Combine(Application.StartupPath, "preheatPara.ini");
       
    }
}
