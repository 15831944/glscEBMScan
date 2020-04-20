using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBMCtrl2._0.ebmScan;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

namespace BeamScanDll
{

    public struct Points {
        public double X11;
        public double Y11;
        public double X12;
        public double Y12;
        public double X21;
        public double Y21;
        public double X22;
        public double Y22;
    }

    public struct AstPoint {
        public double astX11;
        public double astY11;
        public double astX12;
        public double astY12;
        public double astX21;
        public double astY21;
        public double astX22;
        public double astY22;
    }

    public struct CalibFocus {
        public double F1;
        public double F0;
        public double R1;
    }

    //实体参数
    public struct ComponentScanPara {
        public uint LineOrder;
        public double LineOffset;
        public double Size;
        /// <summary>
        /// 区域大小
        /// </summary>
        public double Speed;
        /// <summary>
        /// 扫描频率
        /// </summary>
        public double Frequency;
        /// <summary>
        /// 聚焦补偿
        /// </summary>
        public double FocusOffset;
        /// <summary>
        /// 束流大小
        /// </summary>
        public double BeamValue;
    }
    #region 文件扫描参数
    /// <summary>
    /// 文件相关
    /// </summary>
    //文件扫描参数
    [XmlRootAttribute("fileScanPara")]
    public struct FileScanPara {
        [XmlAttribute("Speed")]
        public double Speed;
        [XmlAttribute("FocusOffset")]
        public double FocusOffset;
        [XmlAttribute("BeamValue")]
        public double BeamValue;
        public FileScanPara(double speed, double focusOffset, double beamValue) {
            Speed = speed;
            FocusOffset = focusOffset;
            BeamValue = beamValue;
        }
        public FileScanPara(FileScanPara para) {
            this.BeamValue = para.BeamValue;
            this.FocusOffset = para.FocusOffset;
            this.Speed = para.Speed;
        }
    }

    [XmlRoot("FileSupportPara")]
    public struct FileSupportPara {
        [XmlElementAttribute("s_scanPara")]
        public FileScanPara s_FileScanPara;
    }
    [XmlRoot("FileGridPara")]
    public struct FileGridPara {
        [XmlElementAttribute("g_scanPara")]
        public FileScanPara g_FileScanPara;
    }
    [XmlRootAttribute("FileFillLinePara")]
    public struct FileFillLinePara {
        [XmlAttribute("startAngel")]
        public double StartAngle;
        [XmlAttribute("rotateAngel")]
        public double RotateAngle;
        [XmlAttribute("scanGap")]
        public double scanGap;
        [XmlAttribute("stripeWidth")]
        public double stripeWidth;
        [XmlAttribute("innerOffset")]
        public double innerOffset;
        [XmlAttribute("outterOffset")]
        public double outterOffset;
        [XmlElementAttribute("partFillLineScanPara")]
        public FileScanPara fillScanPara;
        [XmlElementAttribute("partContourOverridePoint")]
        public int partContourOverridePoint;
        [XmlElementAttribute("layerThickness")]
        public double layerThickness;
    }
    [XmlRoot("FilePartLayerPara")]
    public struct FilePartLayerPara {
        [XmlElementAttribute("partFillLinePara")]
        public FileFillLinePara partFillLinePara;
        [XmlElementAttribute("partOutContourtPara")]
        public FileScanPara outContourtScanPara;
        [XmlElementAttribute("partInContourtPara")]
        public FileScanPara inContourtScanPara;

    }
    #endregion
    #region 预热参数
    [XmlRoot("PreHeatBoxPara")]
    public struct PreHeatBox {
        /// <summary>
        /// 底板扫描次数
        /// </summary>
        [XmlElement("BottomScanCount")]
        public uint BottomScanCount;
        /// <summary>
        /// 预热次数
        /// </summary>
        [XmlElement("PreheatScanCount")]
        public uint PreheatScanCount;
        /// <summary>
        /// 补热次数
        /// </summary>
        [XmlElement("CompensateScanCount")]
        public uint CompensateScanCount;
        /// <summary>
        /// 分割线数目
        /// </summary>
        [XmlElement("LineOrder")]
        public uint LineOrder;
        /// <summary>
        /// 扫描间隔
        /// </summary>
        [XmlElement("LineOffset")]
        public double LineOffset;
        /// <summary>
        /// 区域大小
        /// </summary>
        [XmlElement("Size")]
        public double Size;
        /// <summary>
        /// 扫描速度
        /// </summary>
        [XmlElement("Speed")]
        public double Speed;
        /// <summary>
        /// 扫描频率
        /// </summary>
        [XmlElement("Frequency")]
       // public double Frequency;
        /// <summary>
        /// 底板聚焦补偿
        /// </summary>
        [XmlElement("BottomFocusOffset")]
        public double BottomFocusOffset;
        [XmlElement("PreheatFocusOffset")]
        public double PreheatFocusOffset;
        [XmlElement("CompensateFocusOffset")]
        public double CompensateFocusOffset;
        /// <summary>
        /// 下束电压（底板和预热1）(bit)
        /// </summary>
        [XmlElement("BeamValue")]
        public double BeamValue;
    }
    #endregion
    /// <summary>
    /// 扫描参数，针对预热一(步骤1，2，3)，预热二，实体
    /// </summary>
    public struct ScanParamters {
        /// <summary>
        /// 扫描次数
        /// </summary>
        public uint scanCount;
        /// <summary>
        /// 扫描频率
        /// </summary>
     //   public double scanFrequency;
        /// <summary>
        /// 插点步长
        /// </summary>
        public double stepInsertVal;
        /// <summary>
        /// 聚焦补偿
        /// </summary>
        public double focusOffset;
        /// <summary>
        /// 下束电压
        /// </summary>
        public double scanVolt;
        public ScanParamters(uint count,double insertVal,double focusOffs,double volt)
        {
            this.scanCount = count;
          //  this.scanFrequency = freq;
            this.stepInsertVal = insertVal;
            this.focusOffset = focusOffs;
            this.scanVolt = volt;
        }


    }
    /// <summary>
    /// 预热区域设置
    /// </summary>
    public struct PreHeatArea {
        /// <summary>
        /// 扫描间隔
        /// </summary>
        public float LineOffset;
        /// <summary>
        /// 分区数目
        /// </summary>
        public int LineOrder;
        /// <summary>
        /// 区域大小
        /// </summary>
        public ushort Size;
        /// <summary>
        /// 扫描速度
        /// </summary>
        public float Speed;

        /*
        public double StartX;
        public double EndX;
        public double StartY;
        public double EndY;
        public int DivideNum;
        public int PreLineNum;*/
    }

    struct SignalCardStartArgs {
        public SignalCardModeEnum Mode { get; private set; }
        public int Rate { get; private set; }
        public int Channels { get; private set; }
        public int BufferFrames { get; private set; }
        public int FrameSize { get; private set; }
        public int Timeout { get; private set; }
    }

    public struct Point {
        public double X;
        public double Y;
        public Point(double x, double y) {
            this.X = x;
            this.Y = y;
        }
        public Point(Point pt) {
            this.X = pt.X;
            this.Y = pt.Y;
        }
        public Point minPoint(Point pt) { 
            this.X = pt.X < this.X ? pt.X : this.X;
            this.Y= pt.Y < this.Y ? pt.Y : this.Y;
            return this;
        }
        public  Point maxPoint(Point pt)
        {
            this.X = pt.X > this.X ? pt.X : this.X;
            this.Y = pt.Y > this.Y ? pt.Y : this.Y;
            return  this;
        }
        public static bool operator >(Point a,Point b) { return a.X > b.X || a.Y > b.Y; }
        public static bool operator <(Point a, Point b) { return a.X < b.X || a.Y < b.Y; }
        public override string ToString() =>
            $"({this.X}, {this.Y})";
    }
    public struct Line : IDisposable {
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
        public void Dispose() {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 文件信息
    /// </summary>
    /// 
    [XmlRoot("FileModelInfo")]
    public struct FileContainerInfo {
        [XmlAttribute("fileName")]
        public string fileName;
        [XmlElement("maxHeight")]
        public double maxHeight;
        [XmlElement("minHeight")]
        public double minHeight;
        [XmlElement("thickness")]
        public double thickness;
        [XmlElement("layerNumber")]
        public int layerNumber;
        [XmlElement("unit")]
        public double unit;
        [XmlElement("isHavePart")]
        public bool isHavePart;
        [XmlElement("isHaveSupport")]
        public bool isHaveSupport;
        [XmlElement("isHaveGrid")]
        public bool isHaveGrid;

    }


}

