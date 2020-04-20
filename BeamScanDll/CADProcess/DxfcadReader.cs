using System;
using System.Collections.Generic;
using System.Text;
using netDxf;
using netDxf.Entities;
using System.Linq;
using netDxf.Collections;
using System.Diagnostics;
using System.IO;

namespace BeamScanDll.CADProcess
{
   public class DxfcadReader
    {

        private DxfDocument dxf;
        private Point[] _cadDataPoints;
        private double unit =1;
        private double scale = 1;
        private List<Line> cadlines;
        private int currentReadIndex;
        public int Length;
        public delegate void DelShowCADinfo(string type, int num);
        public DelShowCADinfo delCadInfo;
        public DxfcadReader()
        {
            dxf = new DxfDocument();

        }
        public bool ReadCADfile(string filename)
        {
            Parameter.MinCADFileRealPoint = new Point(10000, 10000);
            Parameter.MaxCADFileRealPoint = new Point(-1000, -10000);
           cadlines = new List<Line>();
            dxf = new DxfDocument();
            try
            {
                dxf = DxfDocument.Load(filename.Trim());
                if (dxf != null)
                {
                    ShowDxfDocumentInformation(dxf);
                    StreamWriter strmsave = new StreamWriter("F:\\CADModeTxt.txt", false, Encoding.Default);
                    int line_cout = dxf.Lines.Count();
                    delCadInfo("CAD文件中包含直线数目为:", line_cout);
                    if (line_cout > 0)
                    {
                        strmsave.WriteLine("CAD文件中包含直线数目为" + line_cout.ToString());
                        foreach (netDxf.Entities.Line line in dxf.Lines)
                        {
             
                            strmsave.WriteLine("起点" + line.StartPoint.ToString());
                            strmsave.WriteLine("终点" + line.EndPoint.ToString());
                            Line l = ILineToLine(line, unit, scale);
                            cadlines.Add(l);                            
                        }
                    }
                    int polyline_num = dxf.LwPolylines.Count();
                    delCadInfo("CAD文件中包含轮廓线数目为:", polyline_num);
                    if (polyline_num>0)
                    {
                        strmsave.WriteLine("CAD文件中包含轮廓数目为" + line_cout.ToString());
                        foreach (netDxf.Entities.LwPolyline poly in dxf.LwPolylines)
                        {
                         List<LwPolylineVertex> vertex = poly.Vertexes.ToList(); 
                         cadlines.AddRange(PointsToLines(vertex));
                            foreach (var item in poly.Vertexes)
                            {
                                strmsave.Write(item.ToString() + ",");
                            }
                        }
                    }
                    int circle_num = dxf.Circles.Count();
                    delCadInfo("CAD文件中包含圆数目为:", circle_num);
                    strmsave.WriteLine("CAD文件中包含圆数目为:"+ circle_num.ToString());
                    if (circle_num>0)
                    {
                        foreach(netDxf.Entities.Circle circle in dxf.Circles)
                        {
                            strmsave.WriteLine(circle.Center.ToString()+"半径"+circle.Radius.ToString());
                            List<Line> lines = PointsToLines(CreateCirclePoints(circle));
                          cadlines.AddRange(lines);
                        }
                    }
                    int ellips_num = dxf.Ellipses.Count();
                    delCadInfo("CAD文件中包含椭圆数目为:", ellips_num);
                    if (ellips_num>0)
                    {
                        foreach (var ellips in dxf.Ellipses)
                        {
                            List<Line> lines = PointsToLines(CreateEllipsePoints(ellips));
                            cadlines.AddRange(lines);
                        }
                    }
                    int arc_num = dxf.Arcs.Count();
                    delCadInfo("CAD文件中包含圆弧数目为:", arc_num);
                    if (arc_num > 0)
                    {
                        foreach (var arc in dxf.Arcs)
                        {
                            List<Line> lines = PointsToLines(CreateArcPoints(arc));
                            cadlines.AddRange(lines);
                        }
                    }
                    strmsave.Close();
                    strmsave.Dispose();
                    ProcessCADData();
                }
               

            }
            catch (Exception ex)
            {
                delCadInfo("打开文件失败,"+ex.Message, -1);
                return false;
            }

            return true;
        }
        private void MinMaxPoint(Point pt)
        {
            Parameter.MinCADFileRealPoint.minPoint(pt);
            Parameter.MaxCADFileRealPoint.maxPoint(pt);
        }
        private void MinMaxLine(Line l)
        {
            MinMaxPoint(l.Start);
            MinMaxPoint(l.End);
        }
        private List<Vector3> CreateArcPoints(Arc arc, int incAngel = 1)
        {
            double r = arc.Radius;
            const double pi = 3.1415926;
            
            List<Vector3> temPoints_1 = new List<Vector3>();
            Vector3 temPoint_1=new Vector3();
            double delta = 2 * pi * incAngel / 360;
            double startAngel = 2 * pi * arc.StartAngle / 360;
            double endAngel = 2 * pi * arc.EndAngle / 360;
            Random seed = new Random();
            for (double i = startAngel; i <= endAngel; i += delta)
            {
                temPoint_1.X = r * Math.Cos(i) ;
                temPoint_1.Y =r * Math.Sin(i) ;
                temPoints_1.Add(temPoint_1);

            }
            return temPoints_1;
        }
        #region showDxfInfo
        private static void ShowDxfDocumentInformation(DxfDocument dxf)
        {
            Console.WriteLine("FILE VERSION: {0}", dxf.DrawingVariables.AcadVer);
            Console.WriteLine();
            Console.WriteLine("FILE COMMENTS: {0}", dxf.Comments.Count);
            foreach (var o in dxf.Comments)
            {
                Console.WriteLine("\t{0}", o);
            }
            Console.WriteLine();
            Console.WriteLine("FILE TIME:");
            Console.WriteLine("\tdrawing created (UTC): {0}.{1}", dxf.DrawingVariables.TduCreate, dxf.DrawingVariables.TduCreate.Millisecond.ToString("000"));
            Console.WriteLine("\tdrawing last update (UTC): {0}.{1}", dxf.DrawingVariables.TduUpdate, dxf.DrawingVariables.TduUpdate.Millisecond.ToString("000"));
            Console.WriteLine("\tdrawing edition time: {0}", dxf.DrawingVariables.TdinDwg);
            Console.WriteLine();
            Console.WriteLine("APPLICATION REGISTRIES: {0}", dxf.ApplicationRegistries.Count);
            foreach (var o in dxf.ApplicationRegistries)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.ApplicationRegistries.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("LAYERS: {0}", dxf.Layers.Count);
            foreach (var o in dxf.Layers)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Layers.GetReferences(o).Count);
                Debug.Assert(ReferenceEquals(o.Linetype, dxf.Linetypes[o.Linetype.Name]), "Object reference not equal.");
            }
            Console.WriteLine();

            Console.WriteLine("LINE TYPES: {0}", dxf.Linetypes.Count);
            foreach (var o in dxf.Linetypes)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Linetypes.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("TEXT STYLES: {0}", dxf.TextStyles.Count);
            foreach (var o in dxf.TextStyles)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.TextStyles.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("SHAPE STYLES: {0}", dxf.ShapeStyles.Count);
            foreach (var o in dxf.ShapeStyles)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.ShapeStyles.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("DIMENSION STYLES: {0}", dxf.DimensionStyles.Count);
            foreach (var o in dxf.DimensionStyles)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.DimensionStyles.GetReferences(o.Name).Count);
                Debug.Assert(ReferenceEquals(o.TextStyle, dxf.TextStyles[o.TextStyle.Name]), "Object reference not equal.");
                Debug.Assert(ReferenceEquals(o.DimLineLinetype, dxf.Linetypes[o.DimLineLinetype.Name]), "Object reference not equal.");
                Debug.Assert(ReferenceEquals(o.ExtLine1Linetype, dxf.Linetypes[o.ExtLine1Linetype.Name]), "Object reference not equal.");
                Debug.Assert(ReferenceEquals(o.ExtLine2Linetype, dxf.Linetypes[o.ExtLine2Linetype.Name]), "Object reference not equal.");
                if (o.DimArrow1 != null) Debug.Assert(ReferenceEquals(o.DimArrow1, dxf.Blocks[o.DimArrow1.Name]), "Object reference not equal.");
                if (o.DimArrow2 != null) Debug.Assert(ReferenceEquals(o.DimArrow2, dxf.Blocks[o.DimArrow2.Name]), "Object reference not equal.");
            }
            Console.WriteLine();

            Console.WriteLine("MLINE STYLES: {0}", dxf.MlineStyles.Count);
            foreach (var o in dxf.MlineStyles)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.MlineStyles.GetReferences(o.Name).Count);
                foreach (var e in o.Elements)
                {
                    Debug.Assert(ReferenceEquals(e.Linetype, dxf.Linetypes[e.Linetype.Name]), "Object reference not equal.");
                }
            }
            Console.WriteLine();

            Console.WriteLine("UCSs: {0}", dxf.UCSs.Count);
            foreach (var o in dxf.UCSs)
            {
                Console.WriteLine("\t{0}", o.Name);
            }
            Console.WriteLine();

            Console.WriteLine("BLOCKS: {0}", dxf.Blocks.Count);
            foreach (var o in dxf.Blocks)
            {
                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Blocks.GetReferences(o.Name).Count);
                Debug.Assert(ReferenceEquals(o.Layer, dxf.Layers[o.Layer.Name]), "Object reference not equal.");

                foreach (var e in o.Entities)
                {
                    Debug.Assert(ReferenceEquals(e.Layer, dxf.Layers[e.Layer.Name]), "Object reference not equal.");
                    Debug.Assert(ReferenceEquals(e.Linetype, dxf.Linetypes[e.Linetype.Name]), "Object reference not equal.");
                    Debug.Assert(ReferenceEquals(e.Owner, dxf.Blocks[o.Name]), "Object reference not equal.");
                    foreach (var x in e.XData.Values)
                    {
                        Debug.Assert(ReferenceEquals(x.ApplicationRegistry, dxf.ApplicationRegistries[x.ApplicationRegistry.Name]), "Object reference not equal.");
                    }

                    Text txt = e as Text;
                    if (txt != null) Debug.Assert(ReferenceEquals(txt.Style, dxf.TextStyles[txt.Style.Name]), "Object reference not equal.");

                    MText mtxt = e as MText;
                    if (mtxt != null) Debug.Assert(ReferenceEquals(mtxt.Style, dxf.TextStyles[mtxt.Style.Name]), "Object reference not equal.");

                    Dimension dim = e as Dimension;
                    if (dim != null)
                    {
                        Debug.Assert(ReferenceEquals(dim.Style, dxf.DimensionStyles[dim.Style.Name]), "Object reference not equal.");
                        Debug.Assert(ReferenceEquals(dim.Block, dxf.Blocks[dim.Block.Name]), "Object reference not equal.");
                    }

                    MLine mline = e as MLine;
                    if (mline != null) Debug.Assert(ReferenceEquals(mline.Style, dxf.MlineStyles[mline.Style.Name]), "Object reference not equal.");

                    Image img = e as Image;
                    if (img != null) Debug.Assert(ReferenceEquals(img.Definition, dxf.ImageDefinitions[img.Definition.Name]), "Object reference not equal.");

                    Insert ins = e as Insert;
                    if (ins != null)
                    {
                        Debug.Assert(ReferenceEquals(ins.Block, dxf.Blocks[ins.Block.Name]), "Object reference not equal.");
                        foreach (var a in ins.Attributes)
                        {
                            Debug.Assert(ReferenceEquals(a.Layer, dxf.Layers[a.Layer.Name]), "Object reference not equal.");
                            Debug.Assert(ReferenceEquals(a.Linetype, dxf.Linetypes[a.Linetype.Name]), "Object reference not equal.");
                            Debug.Assert(ReferenceEquals(a.Style, dxf.TextStyles[a.Style.Name]), "Object reference not equal.");
                        }
                    }
                }

                foreach (var a in o.AttributeDefinitions.Values)
                {
                    Debug.Assert(ReferenceEquals(a.Layer, dxf.Layers[a.Layer.Name]), "Object reference not equal.");
                    Debug.Assert(ReferenceEquals(a.Linetype, dxf.Linetypes[a.Linetype.Name]), "Object reference not equal.");
                    foreach (var x in a.XData.Values)
                    {
                        Debug.Assert(ReferenceEquals(x.ApplicationRegistry, dxf.ApplicationRegistries[x.ApplicationRegistry.Name]), "Object reference not equal.");
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("LAYOUTS: {0}", dxf.Layouts.Count);
            foreach (var o in dxf.Layouts)
            {
                Debug.Assert(ReferenceEquals(o.AssociatedBlock, dxf.Blocks[o.AssociatedBlock.Name]), "Object reference not equal.");

                Console.WriteLine("\t{0}; References count: {1}", o.Name, dxf.Layouts.GetReferences(o.Name).Count);
                List<DxfObject> entities = dxf.Layouts.GetReferences(o.Name);
                foreach (var e in entities)
                {
                    EntityObject entity = e as EntityObject;
                    if (entity != null)
                    {
                        Debug.Assert(ReferenceEquals(entity.Layer, dxf.Layers[entity.Layer.Name]), "Object reference not equal.");
                        Debug.Assert(ReferenceEquals(entity.Linetype, dxf.Linetypes[entity.Linetype.Name]), "Object reference not equal.");
                        Debug.Assert(ReferenceEquals(entity.Owner, dxf.Blocks[o.AssociatedBlock.Name]), "Object reference not equal.");
                        foreach (var x in entity.XData.Values)
                        {
                            Debug.Assert(ReferenceEquals(x.ApplicationRegistry, dxf.ApplicationRegistries[x.ApplicationRegistry.Name]), "Object reference not equal.");
                        }
                    }

                    Text txt = e as Text;
                    if (txt != null) Debug.Assert(ReferenceEquals(txt.Style, dxf.TextStyles[txt.Style.Name]), "Object reference not equal.");

                    MText mtxt = e as MText;
                    if (mtxt != null) Debug.Assert(ReferenceEquals(mtxt.Style, dxf.TextStyles[mtxt.Style.Name]), "Object reference not equal.");

                    Dimension dim = e as Dimension;
                    if (dim != null)
                    {
                        Debug.Assert(ReferenceEquals(dim.Style, dxf.DimensionStyles[dim.Style.Name]), "Object reference not equal.");
                        Debug.Assert(ReferenceEquals(dim.Block, dxf.Blocks[dim.Block.Name]), "Object reference not equal.");
                    }

                    MLine mline = e as MLine;
                    if (mline != null) Debug.Assert(ReferenceEquals(mline.Style, dxf.MlineStyles[mline.Style.Name]), "Object reference not equal.");

                    Image img = e as Image;
                    if (img != null) Debug.Assert(ReferenceEquals(img.Definition, dxf.ImageDefinitions[img.Definition.Name]), "Object reference not equal.");

                    Insert ins = e as Insert;
                    if (ins != null)
                    {
                        Debug.Assert(ReferenceEquals(ins.Block, dxf.Blocks[ins.Block.Name]), "Object reference not equal.");
                        foreach (var a in ins.Attributes)
                        {
                            Debug.Assert(ReferenceEquals(a.Layer, dxf.Layers[a.Layer.Name]), "Object reference not equal.");
                            Debug.Assert(ReferenceEquals(a.Linetype, dxf.Linetypes[a.Linetype.Name]), "Object reference not equal.");
                            Debug.Assert(ReferenceEquals(a.Style, dxf.TextStyles[a.Style.Name]), "Object reference not equal.");
                        }
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine("IMAGE DEFINITIONS: {0}", dxf.ImageDefinitions.Count);
            foreach (var o in dxf.ImageDefinitions)
            {
                Console.WriteLine("\t{0}; File name: {1}; References count: {2}", o.Name, o.File, dxf.ImageDefinitions.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("DGN UNDERLAY DEFINITIONS: {0}", dxf.UnderlayDgnDefinitions.Count);
            foreach (var o in dxf.UnderlayDgnDefinitions)
            {
                Console.WriteLine("\t{0}; File name: {1}; References count: {2}", o.Name, o.File, dxf.UnderlayDgnDefinitions.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("DWF UNDERLAY DEFINITIONS: {0}", dxf.UnderlayDwfDefinitions.Count);
            foreach (var o in dxf.UnderlayDwfDefinitions)
            {
                Console.WriteLine("\t{0}; File name: {1}; References count: {2}", o.Name, o.File, dxf.UnderlayDwfDefinitions.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("PDF UNDERLAY DEFINITIONS: {0}", dxf.UnderlayPdfDefinitions.Count);
            foreach (var o in dxf.UnderlayPdfDefinitions)
            {
                Console.WriteLine("\t{0}; File name: {1}; References count: {2}", o.Name, o.File, dxf.UnderlayPdfDefinitions.GetReferences(o.Name).Count);
            }
            Console.WriteLine();

            Console.WriteLine("GROUPS: {0}", dxf.Groups.Count);
            foreach (var o in dxf.Groups)
            {
                Console.WriteLine("\t{0}; Entities count: {1}", o.Name, o.Entities.Count);
            }
            Console.WriteLine();

            // the entities lists contain the geometry that has a graphical representation in the drawing across all layouts,
            // to get the entities that belongs to a specific layout you can get the references through the Layouts.GetReferences(name)
            // or check the EntityObject.Owner.Record.Layout property
            Console.WriteLine("ENTITIES:");
            Console.WriteLine("\t{0}; count: {1}", EntityType.Arc, dxf.Arcs.Count());
            //Console.WriteLine("\t{0}; count: {1}", EntityType.AttributeDefinition, dxf.AttributeDefinitions.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Circle, dxf.Circles.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Dimension, dxf.Dimensions.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Ellipse, dxf.Ellipses.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Face3D, dxf.Faces3d.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Hatch, dxf.Hatches.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Image, dxf.Images.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Insert, dxf.Inserts.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Leader, dxf.Leaders.Count());
            Console.WriteLine("\t{0}; count: {1}", "LwPolyline", dxf.LwPolylines.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Line, dxf.Lines.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Mesh, dxf.Meshes.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.MLine, dxf.MLines.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.MText, dxf.MTexts.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Point, dxf.Points.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.PolyfaceMesh, dxf.PolyfaceMeshes.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Polyline, dxf.Polylines.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Shape, dxf.Shapes.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Solid, dxf.Solids.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Spline, dxf.Splines.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Text, dxf.Texts.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Ray, dxf.Rays.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Underlay, dxf.Underlays.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Viewport, dxf.Viewports.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.Wipeout, dxf.Wipeouts.Count());
            Console.WriteLine("\t{0}; count: {1}", EntityType.XLine, dxf.XLines.Count());
            Console.WriteLine();

            Console.WriteLine("Press a key to continue...");
            Console.ReadLine();
        }
        #endregion
        private void ProcessCADData()
        {
            List<Line> bitLines = new List<Line>();
            if(cadlines != null)
            {
                foreach (var item in cadlines)
                {
                    MinMaxLine(item);
                    Line line = Parameter.dataAdapter.DoubleMmToLine(item);
                    bitLines.Add(line);
                }
            }
            delCadInfo("读取cad文件最大坐标" + Parameter.MaxCADFileRealPoint.ToString(), -1);
            delCadInfo("读取cad文件最小坐标" + Parameter.MinCADFileRealPoint.ToString(), -1);
            if(Parameter.MaxCADFileRealPoint>Parameter.MaxCADFileSetValue||
                Parameter.MinCADFileRealPoint<Parameter.MinCADFileSetValue)
            {
                delCadInfo("读取文件坐标范围超出定义输出范围，请注意检查" , -2);

            }
            _cadDataPoints = Hack(bitLines, Parameter.cadFilescanPara.Speed);
            this.Length = _cadDataPoints.Length;
            using (StreamWriter strmsave = new StreamWriter("F:\\CADDataTxt.txt", false, Encoding.Default))
            {
                for (int i = 0; i < this.Length; i++)
                {
                    strmsave.WriteLine(_cadDataPoints[i].X.ToString() + ","+_cadDataPoints[i].Y.ToString());
                }
            }
        }
        public void ReadCadPoint(ref double[,] scans, int startIndex, int length)
        {
            if (length > scans.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("Reading out of array bounds error");
            }
            int num = this._cadDataPoints.Count();
        
            for (int i = 0; i < length; i++)
            {
                
                Point point = this._cadDataPoints[this.currentReadIndex++];
                scans[startIndex + i, 0] = point.X;
                scans[startIndex + i, 1] = point.Y;
                scans[startIndex + i, 2] = StaticTool.CaculateFocus((uint)StaticTool.GetRadius((uint)point.X, (uint)point.Y)) + Parameter.cadFilescanPara.FocusOffset; //聚焦
                scans[startIndex + i, 3] = 32767;
                scans[startIndex + i, 4] = 32767;
                scans[startIndex + i, 5] = Parameter.cadFilescanPara.BeamValue;//this._preHeatScan[5];//power
                scans[startIndex + i, 6] = StaticTool.CaculateLinerVal((uint)point.X, (uint)point.Y, true);//this._preHeatScan[6];//astX
                scans[startIndex + i, 7] = StaticTool.CaculateLinerVal((uint)point.X, (uint)point.Y, false);//this._preHeatScan[7];//astY
                if (this.currentReadIndex == num)
                {
                    this.currentReadIndex = 0;
                }
            }
        }
        private Point[] Hack(List<Line> lines, double speed)//扫描线插点算法
        {
            List<Point> list = new List<Point>();
            double rest = 0.0;
            for (int i = 0; i < lines.Count; i++)
            {
                Line line = lines[i];
                list.AddRange(this.Hack(line, speed, ref rest));
            }
            return list.ToArray();
        }
        private Point iPointToPoint(PolylineVertex ipt, double unit, double scale)
        {
            double x = ipt.Position.X * unit * scale;
            double y = ipt.Position.Y * unit * scale;
            if (x > Parameter.maxPointXY || y > Parameter.maxPointXY)
            {
                throw new Exception("文件数据点超出打印上界限");
            }
            if (x < Parameter.minPointXY || y < Parameter.minPointXY)
            {
                throw new Exception("文件数据点超出打印下界限");
            }
            return new Point(x, y);
        }
        private Point iPointToPoint(Vector3 ipt, double unit, double scale)
        {
            double x = ipt.X * unit * scale;
            double y = ipt.Y * unit * scale;
            if (x > Parameter.maxPointXY || y > Parameter.maxPointXY)
            {
                throw new Exception("文件数据点超出打印上界限");
            }
            if (x < Parameter.minPointXY || y < Parameter.minPointXY)
            {
                throw new Exception("文件数据点超出打印下界限");
            }
            return new Point(x, y);
        }
        private List<Point> iContourToContour(List<Vector3> iContour, double unit, double scale)
        {
            List<Point> Contour = new List<Point>();
            foreach (var ipoint in iContour)
            {
                Point pt = iPointToPoint(ipoint, unit, scale);
                Contour.Add(pt);
            }
            return Contour;
        }
        private Line ILineToLine(netDxf.Entities.Line iline, double unit, double scale)
        {
            Point spt = iPointToPoint(iline.StartPoint, unit, scale);
            Point ept = iPointToPoint(iline.EndPoint, unit, scale);
            return new Line(spt, ept);
        }
        private List<Point> Hack(Line line, double speed, ref double rest)
        {
            double a = ((line.Length / speed) * Parameter.fileScanFrequency) - rest;//点的个数减去偏移
            double num3 = line.Dx / a;//x增量
            double num4 = line.Dy / a;//y增量
            double x = line.Start.X;//起点x
            double y = line.Start.Y;//起点y
            List<Point> list = new List<Point>();
            //增加判定，如果计算出来的插点个数小于1，
            //即可插点个数太小，这条线段的长度也太小了，则容易导致增量太大
            if (a < 1)
            {
                list.Add(new Point(x, y));
                return list;
            }
            for (int i = 0; i < a; i++)
            {
                UInt32 num8 = /*(ushort)*/(UInt32)((num3 * (i + rest)) + x);
                UInt32 num9 = /*(ushort)*/(UInt32)((num4 * (i + rest)) + y);
                if (num8 > 65535 || num9 > 65535)
                {
                    //throw new Exception("数据超出范围65535");
                    num8 = (UInt32)x;
                    num9 = (UInt32)y;
                }
                list.Add(new Point((double)num8, (double)num9));
            }
            rest = Math.Ceiling(a) - a;
            return list;
        }
        private List<Line> PointsToLines(List<Vector3> Points)//扫描线插点算法
        {
            int PointsNum = Points.Count;
            List<Line> lines = new List<Line>();
            for (int i = 0; i < PointsNum - 1; i++)
            {
                Point st = new Point(Points[i].X, Points[i].Y);
                Point end = new Point(Points[i+1].X, Points[i+1].Y);
                Line temLine = new Line(st,end);
                lines.Add(temLine);
            }
           // lines.Add(new Line(Points[PointsNum - 1], Points[0]));//添加上最后一个点
            return lines;

        }
        private List<Line> PointsToLines(List<LwPolylineVertex> Points)//扫描线插点算法
        {
            int PointsNum = Points.Count();
            List<Line> lines = new List<Line>();
            for (int i = 0; i < PointsNum - 1; i++)
            {
                Point st = new Point(Points[i].Position.X, Points[i].Position.Y);
                Point end = new Point(Points[i+1].Position.X, Points[i+1].Position.Y);
                Line temLine = new Line(st, end);
                lines.Add(temLine);
            }
            // lines.Add(new Line(Points[PointsNum - 1], Points[0]));//添加上最后一个点
            return lines;

        }
        private List<Line> PointsToLines(ObservableCollection<PolylineVertex> Points)//扫描线插点算法
        {
            int PointsNum = Points.Count;
            List<Line> lines = new List<Line>();
            for (int i = 0; i < PointsNum - 1; i++)
            {
                Point st = new Point(Points[i].Position.X, Points[i].Position.Y);
                Point end = new Point(Points[i+1].Position.X, Points[i+1].Position.Y);
                Line temLine = new Line(st, end);
                lines.Add(temLine);
            }
            // lines.Add(new Line(Points[PointsNum - 1], Points[0]));//添加上最后一个点
            return lines;

        }
        private List<Line> PointsToLines(List<Point> Points)//扫描线插点算法
        {
            int PointsNum = Points.Count;
            List<Line> lines = new List<Line>();
            for (int i = 0; i < PointsNum - 1; i++)
            {
                Line temLine = new Line(Points[i], Points[i + 1]);
                lines.Add(temLine);
            }
            lines.Add(new Line(Points[PointsNum - 1], Points[0]));//添加上最后一个点
            return lines;

        }
        private List<Point> CreateCirclePoints(Circle circle, int DivideNum = 360)
        {
            double R = circle.Radius;
            const double pi = 3.1415926;
            List<Point> temPoints_1 = new List<Point>();
            Point temPoint_1;
            double delta = 2 * pi / DivideNum;
            Random seed = new Random();
            int index = seed.Next(0, DivideNum);

            for (int i = 0; i <= 360; i++, index++)
            {

                double angel = index * delta;
                temPoint_1.X = R * Math.Cos(angel) ;
                temPoint_1.Y = R * Math.Sin(angel);
                temPoints_1.Add(temPoint_1);
                if (index >= 360)
                {
                    index = 0;
                }
            }
            return temPoints_1;
        
        }
        private List<Point> CreateEllipsePoints(Ellipse elips, int incAngel = 1)
        {
            double a = elips.MajorAxis;
            double b = elips.MinorAxis;
            const double pi = 3.1415926;
            
            List<Point> temPoints_1 = new List<Point>();
            Point temPoint_1;
            double delta = 2 * pi*incAngel /360;
            double startAngel = 2 * pi * elips.StartAngle / 360;
            double endAngel = 2 * pi * elips.EndAngle / 360;
            Random seed = new Random();
            for (double i = startAngel; i <= endAngel; i+= delta)
            { 
                temPoint_1.X = a * Math.Cos(i) ;
                temPoint_1.Y = b * Math.Sin(i) ;
                temPoints_1.Add(temPoint_1);
        
            }
            return temPoints_1;
        }

   }
}
