using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    //墙类
    public class ConWall
    {
        public ConWall()
        { ;}
        public DBText dbtext_name;//名字:如Q1
        public DBText dbtext_biaogao;//标高如:0.000~3.000
        public DBText dbtext_t;//截面如:200
        public DBText dbtext_Hsteel;//水平分布筋如%%1328@100(2排)
        public DBText dbtext_Vsteel;//垂直直分布筋如%%1328@100(2排)
        public DBText dbtext_Fsteel;//拉筋如%%1316@300x300(矩形)
        public ConWall(Database db, DBText name, DBText biaogao, DBText qianghou, DBText hsteel, DBText vsteel, DBText fsteel)
        {
            dbtext_name = name;
            dbtext_biaogao = biaogao;
            dbtext_t = qianghou;
            dbtext_Hsteel = hsteel;
            dbtext_Vsteel = vsteel;
            dbtext_Fsteel = fsteel;
        }
        public double Jiemian_T
        {
            get
            {
                return Convert.ToDouble(dbtext_t.TextString);
            }
        }//截面200
        public double Vsteel_D
        {
            get
            {
                string[] sArray = dbtext_Vsteel.TextString.Split(new string[] { "%%132", "@", "(", "排", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[0]);
            }
        }//水平分布筋直径
        public double Vsteel_Gap
        {

            get
            {
                string[] sArray = dbtext_Vsteel.TextString.Split(new string[] { "%%132", "@", "(", "排", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }

        }//水平分布筋间距
        public double Vsteel_N
        {

            get
            {
                string[] sArray = dbtext_Vsteel.TextString.Split(new string[] { "%%132", "@", "(", "排", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[2]);
            }

        }//水平分布筋排数
        public double Hsteel_D
        {
            get
            {
                string[] sArray = dbtext_Hsteel.TextString.Split(new string[] { "%%132", "@", "(", "排", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[0]);
            }
        }//垂直分布筋直径
        public double Hsteel_Gap
        {

            get
            {
                string[] sArray = dbtext_Hsteel.TextString.Split(new string[] { "%%132", "@", "(", "排", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }

        }//垂直分布筋间距
        public double Hsteel_N
        {

            get
            {
                string[] sArray = dbtext_Hsteel.TextString.Split(new string[] { "%%132", "@", "(", "排", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[2]);
            }

        }//垂直分布筋排数
        public double Hsteel_Area
        {
            get
            {
                return Hsteel_D * Hsteel_D * 3.14 / 4 * Hsteel_N;
            }
        }//由以上数据计算水平钢筋面积
        public string Hsteel_Area_ratio
        {
            get
            {
                return Math.Round(Hsteel_Area / (Vsteel_Gap * Jiemian_T) * 100, 3).ToString();
            }
        }//由以上数据计算截面配筋率
        public double Vsteel_Area
        {
            get
            {
                return Vsteel_D * Vsteel_D * 3.14 / 4 * Vsteel_N;
            }
        }//由以上数据计算水平钢筋面积
        public string Vsteel_Area_ratio
        {
            get
            {
                return Math.Round(Vsteel_Area / (Vsteel_Gap * Jiemian_T) * 100, 3).ToString();
            }
        }//由以上数据计算截面配筋率
    }
    //墙表计算主程序
    public class WallTable
    {
        [CommandMethod("wt")]
        public void CalWallDemo()
        {

            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult psr = ed.GetSelection();
            List<ConWall> ConWallList = new List<ConWall>();
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (psr.Status == PromptStatus.OK)
                    {
                        SelectionSet sset = psr.Value;
                        foreach (SelectedObject item in sset)
                        {
                            Polyline pl = item.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
                            db.DelectDbtext(3, pl);
                            ConWallList = GetWall(db, pl);
                        }
                        for (int i = 0; i < ConWallList.Count; i++)
                        {
                            db.AddText(ConWallList[i].Hsteel_Area_ratio + "%", ConWallList[i].dbtext_Fsteel, new Vector3d(-8400, -150, 0), 3, 0.25);
                            db.AddText(ConWallList[i].Vsteel_Area_ratio + "%", ConWallList[i].dbtext_Fsteel, new Vector3d(-4400, -150, 0), 3, 0.25);
                        }
                    }
                    trans.Commit();
                }
            }
            catch (System.Exception)
            {

                Application.ShowAlertDialog("请先检查钢筋符号是否是%%132");
            }


        }
        /// <summary>
        /// 按dbtext处理选择多段线内的文字
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pl"></param>
        /// <returns></returns>
        public List<ConWall> GetWall(Database db, Polyline pl)
        {

            int number = 6;//表行数
            List<ConWall> ConWallList = new List<ConWall>();
            List<DBText> dbtextList = new List<DBText>();
            List<DBText> resultList = new List<DBText>();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.Start, "TEXT"),
                new TypedValue((int)DxfCode.Operator, "and>"), 
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.LayerName, "TAB_TEXT"),
                new TypedValue((int)DxfCode.Operator, "and>"), 
            };
            SelectionFilter filter = new SelectionFilter(tv);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult psr = ed.SelectCrossingPolygon(PlToPointTool.PltoPint3dCollectionMethod(pl), filter);
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        DBText dbtext = item.ObjectId.GetObject(OpenMode.ForWrite) as DBText;
                        dbtextList.Add(dbtext);
                    }
                    resultList = PlToPointTool.SortDbtext(dbtextList);

                    ConWall[] wall = new ConWall[resultList.Count / number];

                    for (int i = 0; i < wall.Length; i++)
                    {
                        wall[i] = new ConWall(db, resultList[i * number], resultList[i * number + 1], resultList[i * number + 2], resultList[i * number + 3],
                                               resultList[i * number + 4], resultList[i * number + 5]);
                        ConWallList.Add(wall[i]);
                    }


                }

                trans.Commit();
            }

            return ConWallList;
        }
    }
    //画墙表
    public class DrawWallTable
    {
        [CommandMethod("hzwb")]
        public void DrawWallTableDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptPointOptions ppo = new PromptPointOptions("请指点墙表位置");
            PromptPointResult ppr = ed.GetPoint(ppo);
            Point3d pt = ppr.Value;
            Polyline plwaikuang = new Polyline();
            double x = pt.X;
            double y = pt.Y;
            Point2d startPoint = new Point2d(x, y);
            Point2d pt0 = new Point2d(x, y);
            Point2d pt1 = new Point2d(x + 18000, y);
            Point2d pt2 = new Point2d(x + 18000, y - 5200);
            Point2d pt3 = new Point2d(x, y - 5200);
            plwaikuang.AddVertexAt(0, pt0, 0, 0, 0);
            plwaikuang.AddVertexAt(1, pt1, 0, 0, 0);
            plwaikuang.AddVertexAt(2, pt2, 0, 0, 0);
            plwaikuang.AddVertexAt(3, pt3, 0, 0, 0);
            plwaikuang.Closed = true;

            plwaikuang.SetLayerId(db.AddLayerTool("TAB"), true);
            db.AddToModelSpace(plwaikuang);
            db.DrawWallTableDemo(plwaikuang.ObjectId, 18000, 5200, 6, 5);
        }

        [CommandMethod("clearwtb")]
        public void ClearWallTableDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult psr = ed.GetSelection();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        Polyline pl = item.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
                        db.DelectDbtext(3, pl);
                        db.DelectDbtext(1, pl);
                    }

                }
                trans.Commit();
            }
        }
    }
    //画表类
    public static class DrawWallTableTool
    {
        public static void DrawWallTableDemo(this Database db, ObjectId polylineoid, double table_width, double table_height, int row, int collum)
        {

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Polyline plwaikuang = polylineoid.GetObject(OpenMode.ForRead) as Polyline;

                #region MyRegion
                Line xline = new Line();
                xline.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -1000, 0);
                xline.EndPoint = xline.StartPoint + new Vector3d(table_width, 0, 0);

                Line xline1 = new Line();
                xline1.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -1700, 0);
                xline1.EndPoint = xline1.StartPoint + new Vector3d(table_width, 0, 0);

                Line xline2 = new Line();
                xline2.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -2400, 0);
                xline2.EndPoint = xline2.StartPoint + new Vector3d(table_width, 0, 0);

                Line xline3 = new Line();
                xline3.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -3100, 0);
                xline3.EndPoint = xline3.StartPoint + new Vector3d(table_width, 0, 0);

                Line xline4 = new Line();
                xline4.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -3800, 0);
                xline4.EndPoint = xline4.StartPoint + new Vector3d(table_width, 0, 0);

                Line xline5 = new Line();
                xline5.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -4500, 0);
                xline5.EndPoint = xline5.StartPoint + new Vector3d(table_width, 0, 0);

                db.AddToModelSpace(xline, xline1, xline2, xline3, xline4, xline5);
                #endregion
                #region MyRegion
                Line yline = new Line();
                yline.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(1000, 0, 0);
                yline.EndPoint = yline.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline1 = new Line();
                yline1.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(4000, 0, 0);
                yline1.EndPoint = yline1.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline2 = new Line();
                yline2.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(6000, 0, 0);
                yline2.EndPoint = yline2.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline3 = new Line();
                yline3.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(10000, 0, 0);
                yline3.EndPoint = yline3.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline4 = new Line();
                yline4.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(14000, 0, 0);
                yline4.EndPoint = yline4.StartPoint + new Vector3d(0, -table_height, 0);

                db.AddToModelSpace(yline, yline1, yline2, yline3, yline4);
                #endregion
                #region MyRegion
                db.DoubletoDbtext("编号", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700, 0), "TAB");
                db.DoubletoDbtext("标高", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(1530, -700, 0), "TAB");
                db.DoubletoDbtext("墙厚", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(4700, -700, 0), "TAB");
                db.DoubletoDbtext("水平分布筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(7000, -700, 0), "TAB");
                db.DoubletoDbtext("竖向分布筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(11000, -700, 0), "TAB");
                db.DoubletoDbtext("拉筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(14500, -700, 0), "TAB");
                #endregion
                #region MyRegion
                db.DoubletoDbtext("Q1", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1530, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("200", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(4700, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(7000, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(11000, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1306@400x400(矩形)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14500, -700 - 850, 0), "TAB_TEXT");
                #endregion
                #region MyRegion
                db.DoubletoDbtext("Q1", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1530, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("200", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(4700, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(7000, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(11000, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1306@400x400(矩形)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14500, -700 - 850 - 700, 0), "TAB_TEXT");
                #endregion
                #region MyRegion
                db.DoubletoDbtext("Q1", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1530, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("200", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(4700, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(7000, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(11000, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1306@400x400(矩形)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14500, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                #endregion
                #region MyRegion
                db.DoubletoDbtext("Q1", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1530, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("200", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(4700, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(7000, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@200(2排)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(11000, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1306@400x400(矩形)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14500, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                #endregion

                trans.Commit();
            }

        }
    }
}
