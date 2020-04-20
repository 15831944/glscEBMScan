using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace BeamScanDll
{
   public static class Parameter
    {
        /// <summary>
        /// 默认的静态参数配置文本
        /// </summary>
       // public static string Config = Path.Combine(Application.Pdf, "config.ini");
        /// <summary>
        /// 校准配置文件
        /// </summary>
      //  public static string CalibrationPath = Path.Combine(Application.Pdf, "Calibration.ini");
        /// <summary>
        /// XML参数文件
        /// </summary>
      //  public static string StrParamPath = Path.Combine(Application.Pdf, "System\\Parameter.xml");
        /// <summary>
        /// 打印的文件名称
        /// </summary>
        public static string _SXFileName = string.Empty;
        /// <summary>
        /// 操作模式
        /// </summary>
        public static FileDataAdapter dataAdapter = new FileDataAdapter(148, 22352);
        public static ushort MinDaqAOBitValue = 0;
        public static ushort MaxDaqAOBitValue = 65535;
        /// <summary>
        /// CAD文件的设置大小尺寸
        /// </summary>
        public static Point MinCADFileSetValue =new Point(0,0);
        public static Point MaxCADFileSetValue =new Point(200,200);
        /// <summary>
        ///  CAD文件的实际大小尺寸
        /// </summary>
        public static Point MinCADFileRealPoint=new Point(1000,1000);
        public static Point MaxCADFileRealPoint =new Point(-1000,-1000);
        /// <summary>
        /// 正在铺粉标志位
        /// </summary>
        public static bool IsMoveRaking = false;
        /// <summary>
        /// 下束标志位
        /// </summary>
        public static bool IsBeamOn = false;
        //束流大小
        public static string BeamValue = "0";
        public static string Focus = "0";
        public static string HV = "0";

        //DA卡输出区间

        /// <summary>
        /// 实际预热次数
        /// </summary>
        public static ushort ActualPreHeatCount = 0;
        /// <summary>
        /// 当前打印层数
        /// </summary>
        public static int CurrentLayerIndex =0;
        /// <summary>
        /// 起始层数
        /// </summary>
        public static uint BeginLayer = 0;
        /// <summary>
        /// 停止层数
        /// </summary>
        public static uint EndLayer = 65535;
        /// <summary>
        /// 底板温度
        /// </summary>
        public static float BottomTemperature = 20.0f;

        public static float A1 = 0f;
        public static float A2 = 0f;
        public static float K = 0f;
        #region 扫描
        /// <summary>
        /// 频率默认为80K
        /// </summary>
        public static UInt32 Frequency = 80000;
        /// <summary>
        /// cad扫描参数
        /// </summary>
        public static FileScanPara cadFilescanPara = new FileScanPara();
        /// <summary>
        /// 预热
        /// </summary>
        public static PreHeatBox PreHeat = new PreHeatBox();
        /// <summary>
        /// 实体(圆)
        /// </summary>
        public static ComponentScanPara CompScanPara;
        /// <summary>
        /// 文件part扫描参数
        /// </summary>
        public static ScanParamters _preHeatPara=new ScanParamters();
        /// <summary>
        /// 文件结构树
        /// </summary>

        public static List<FileContainerInfo> fileModelInfos = new List<FileContainerInfo>();
        /// <summary>
        /// Process定义
        /// </summary>
        public static Dictionary<string, string> process = new Dictionary<string, string>();
        /// <summary>
        /// 打印区域的范围，单位mm
        /// </summary>
        public static double minPointXY = -2000;
        public static double maxPointXY = 2000;
        /// <summary>
        /// adapter 默认范围 0-65535 打印范围0-200,所以得到kp=327.67,offset=0
        /// </summary>
       // public static FileDataAdapter dataAdapter = new FileDataAdapter(148, 22111);
        /// <summary>
        /// 文件默认的频率为40K，他不是扫描参数，而是卡的一个固定的属性值
        /// </summary>
        public static double fileScanFrequency = 80000;
        public static int downSurfaceLayerCount = 80;//下表面定义层数
        #region Old
        /// <summary>
        /// 预热扫描次数 
        /// </summary>
        public static uint m_ScanTimes { get; set; }
        

        /// <summary>
        /// 预热区域一
        /// </summary>
        public static PreHeatArea PreHeatArea1;
        /// <summary>
        /// 预热区域二
        /// </summary>
        public static PreHeatArea PreHeatArea2;
        /// <summary>
        /// 预热一，步骤一
        /// </summary>
        public static ScanParamters PreHeat1_1;
        /// <summary>
        /// 预热一，步骤二
        /// </summary>
        public static ScanParamters PreHeat1_2;
        /// <summary>
        /// 预热一，步骤三
        /// </summary>
        public static ScanParamters PreHeat1_3;
        /// <summary>
        /// 预热二
        /// </summary>
        public static ScanParamters PreHeat2;
        /// <summary>
        /// 实体
        /// </summary>
        public static ScanParamters Scan;

        #endregion
        #endregion

        #region 校准
        /// <summary>
        /// 三个校准参数F1、F0、R1
        /// </summary>
        public static CalibFocus CalibFocus;
        /// <summary>
        /// 校正的四个坐标点
        /// </summary>
        public static Points Points;
        /// <summary>
        ///  消像散
        /// </summary>
        public static AstPoint AstPoint;
        #endregion
        #region 自动加工参数
        /// <summary>
        /// 自动预热次数
        /// </summary>
        public static uint autoPreheatTimes;
        /// <summary>
        /// 自动补热次数
        /// </summary>
        public static uint autoPostheatTimes;
        /// <summary>
        /// 自动开始层数
        /// </summary>
        public static int autoStartLayerNum;
        /// <summary>
        /// 自动结束层数
        /// </summary>
        public static int autoEndLayerNum;
       
        /// <summary>
        /// 使用 turningPoints功能
        /// </summary>
        public static bool bUseTurningPoints = true;
        /// <summary>
        /// 使用分区标志位
        /// </summary>
        public static bool bUseDistrictArea = true;
        #endregion
        # region 工艺分析参数
        public static float t1 = 7.5f;//预热1需要时间
        public static float t2 = 0f;//扫描内轮廓需要时间
        public static float t3 = 0f;//扫填充需要时间
        public static float t4 = 0f;//扫支撑需要时间
        public static float t5 = 0f;//扫网格需要时间
        public static float t6 = 0f;//扫外边框需要时间
        public static float t8 = 11.5f;//铺粉需要时间
        #endregion
   

    }
}
