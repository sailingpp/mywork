using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Windows.Interop;
using System.IO;
using System.Threading;


namespace MyCadTools
{
    public static class Test
    {
        public static Document doc = Application.DocumentManager.MdiActiveDocument;
        public static Database db = HostApplicationServices.WorkingDatabase;
        public static Editor ed = doc.Editor;

        /// <summary>
        /// 选择变绿
        /// </summary>
        [CommandMethod("getatttest")]
        public static void GetAttributeDemo()
        {
            // 获得当前文档和数据库   Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;


            // 启动一个事务  Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // 要求在图形区域中选择对象    Request for objects to be selected in the drawing area
                PromptSelectionResult acSSPrompt = acDoc.Editor.GetSelection();

                // 如果提示状态是 OK，对象就被选择了    If the prompt status is OK, objects were selected
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;

                    // 遍历选择集中的对象   Step through the objects in the selection set
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        // 检查以确定返回的 SelectedObject 对象是有效的     Check to make sure a valid SelectedObject object was returned
                        if (acSSObj != null)
                        {
                            // 以写的方式打开选择的对象   Open the selected object for write
                            Entity acEnt = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Entity;
                            // PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter your name: ");
                            // pStrOpts.AllowSpaces = true;
                            // PromptResult pStrRes = acDoc.Editor.GetString(pStrOpts);
                            //用于从命令行得到
                            //Application.ShowAlertDialog("对象颜色是： " + acEnt.Color.ToString());
                            //用于提示弹窗
                            if (acEnt != null)
                            {
                                acDoc.Editor.WriteMessage("\n对象颜色是： " + acEnt.Color.ToString());
                                //用于命令行提示
                                acDoc.Editor.WriteMessage("\n对象颜色值是： " + acEnt.ColorIndex.ToString());
                                acDoc.Editor.WriteMessage("\n对象块名是： " + acEnt.BlockName.ToString());
                                acDoc.Editor.WriteMessage("\n对象图层是： " + acEnt.Layer.ToString());
                                //acEnt.IntersectWith(L, Intersect.ExtendThis, po, System.IntPtr.Zero, System.IntPtr.Zero);           
                                Line line1 = (Line)acEnt;
                                acDoc.Editor.WriteMessage("\n起点坐标： " + line1.StartPoint.X);
                                acDoc.Editor.WriteMessage("\n起点坐标： " + line1.Angle);
                            }
                        }
                    }
                    // 保存新对象到数据库中   Save the new object to the database
                    acTrans.Commit();
                }
                // Dispose of the transaction
            }
        }

        /// <summary>
        /// 向图中增加圆
        /// </summary>
        [CommandMethod("addcircletest")]
        public static void AddCirceTest()
        {
            Circle c = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), 100);
            db.AddToModelSpace(c);
            Circle c1 = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), 200);
            Circle c2 = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), 300);
            db.AddToModelSpace(c1, c2);

        }

        /// <summary>
        ///将所有对象编号并标记面积  
        /// </summary>
        [CommandMethod("bhptest")]
        public static void BianHaoPlus()
        {
            int i = 1;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = (Entity)item.GetObject(OpenMode.ForWrite);
                    //ent.ColorIndex = 3;
                    //Polyline pl = (Polyline)item.GetObject(OpenMode.ForWrite);
                    if (ent is Polyline)
                    {
                        Polyline pl = ent as Polyline;
                        double x = CadHelper.outcenterx(pl);
                        double y = CadHelper.outcentery(pl);
                        Point3d po = new Point3d(x, y, 0);
                        DBText dbtext = new DBText();
                        dbtext.TextString = Math.Round(pl.Area, 3).ToString();
                        dbtext.Height = 200;
                        dbtext.Position = po;
                        dbtext.ColorIndex = 3;

                        DBText dbtext1 = new DBText();
                        dbtext1.TextString = i.ToString();
                        dbtext1.Height = 300;
                        dbtext1.Position = pl.StartPoint;
                        dbtext1.ColorIndex = 1;

                        btr.AppendEntity(dbtext);
                        trans.AddNewlyCreatedDBObject(dbtext, true);
                        btr.AppendEntity(dbtext1);
                        trans.AddNewlyCreatedDBObject(dbtext1, true);
                        i++;
                    }

                }

                trans.Commit();
            }
        }

        [CommandMethod("turngreentest")]
        public static void TurnGreen()
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = (Entity)item.GetObject(OpenMode.ForWrite);
                    ent.ColorIndex = 3;
                }

                trans.Commit();
            }
        }

        /// <summary>
        /// 标记面积
        /// </summary>
        [CommandMethod("showareatest")]
        public static void ShowArea()
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = (Entity)item.GetObject(OpenMode.ForWrite);
                    //ent.ColorIndex = 3;
                    //Polyline pl = (Polyline)item.GetObject(OpenMode.ForWrite);
                    if (ent is Polyline)
                    {
                        Polyline pl = ent as Polyline;
                        double x = CadHelper.outcenterx(pl);
                        double y = CadHelper.outcentery(pl);
                        Point3d po = new Point3d(x, y, 0);
                        DBText dbtext = new DBText();
                        dbtext.TextString = Math.Round(pl.Area, 3).ToString();
                        dbtext.Height = 200;
                        dbtext.Position = po;
                        dbtext.ColorIndex = 3;
                        btr.AppendEntity(dbtext);
                        trans.AddNewlyCreatedDBObject(dbtext, true);

                    }

                }

                trans.Commit();
            }
        }

        /// <summary>
        ///将所有对象/编号
        /// </summary>
        [CommandMethod("entbhtest")]
        public static void EntiyBianHao()
        {
            int i = 1;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = (Entity)item.GetObject(OpenMode.ForWrite);
                    if (ent is Polyline)
                    {
                        Polyline pl = ent as Polyline;
                        double x = CadHelper.outcenterx(pl);
                        double y = CadHelper.outcentery(pl);
                        Point3d po = new Point3d(x, y, 0);
                        DBText dbtext = new DBText();
                        dbtext.TextString = i.ToString();
                        dbtext.Height = 300;
                        dbtext.Position = pl.StartPoint;
                        dbtext.ColorIndex = 1;
                        btr.AppendEntity(dbtext);
                        trans.AddNewlyCreatedDBObject(dbtext, true);
                        i++;
                    }

                }

                trans.Commit();
            }
        }

        /// <summary>
        /// 获取多段线每一段的长度
        /// </summary>
        [CommandMethod("gfxtest")]
        public static void GetNorm()
        {

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Polyline polyline = (Polyline)item.GetObject(OpenMode.ForWrite);

                    Application.ShowAlertDialog(polyline.GetLineSegmentAt(2).ToString());
                }

                trans.Commit();
            }
        }

        [CommandMethod("insertblocktest")]
        public static void InSertBlockDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Circle c = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), 10);
            db.InsertBlock(c);
        }

        /// <summary>
        /// 标记多段线每一段的长度
        /// </summary>
        [CommandMethod("bjldtest")]
        public static void ShowLength()
        {

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = trans.GetObject(item, OpenMode.ForRead) as Entity;
                    if (ent is Polyline)
                    {
                        Polyline polyline = (Polyline)item.GetObject(OpenMode.ForWrite);

                        for (int i = 0; i < polyline.NumberOfVertices - 1; i++)
                        {
                            DBText dbtext = new DBText();
                            dbtext.Height = 400;
                            dbtext.Position = CadHelper.GetMiddle(polyline.GetPoint3dAt(i), polyline.GetPoint3dAt(i + 1));
                            dbtext.TextString = CadHelper.GetDist(polyline)[i].ToString();
                            // Application.ShowAlertDialog(AddToModelTools.GetDist(polyline)[i].ToString());
                            btr.AppendEntity(dbtext);
                            trans.AddNewlyCreatedDBObject(dbtext, true);
                        }
                    }




                }

                trans.Commit();
            }

        }

        /// <summary>
        /// 将多个entity放入到块中
        /// </summary>
        [CommandMethod("blkstest")]
        public static void BksDemo()
        {
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                List<Entity> ents = new List<Entity>();
                foreach (ObjectId item in btr)
                {
                    ents.Add((Entity)tran.GetObject(item, OpenMode.ForRead));
                }

                db.AddToMyBlockTable(ents);
                tran.Commit();
            }

        }

        /// <summary>
        /// 读取单个cad文件全图做块复制
        /// </summary>
        [CommandMethod("makeblocktest")]
        public static void MakeBlockDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Point3d pt = new Point3d(0, 0, 0);
            string path = @"D:\test.dwg";
            Makeblock.InsertBlockReference(pt, 1, "test", path);
        }

        [CommandMethod("mkstest")]
        public static void MakeBlocksDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            string path = @"D:\cad合并";


            string[] paths = Directory.GetFiles(path);

            string[] name = new string[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                if (Path.GetExtension(paths[i]) == ".dwg" || Path.GetExtension(paths[i]) == ".DWG")
                {
                    name[i] = (10001 * i).ToString();
                    Makeblock.InsertBlockReference(new Point3d(0, -i * 200, 0), i, name[i], paths[i]);
                }

            }

        }

        /// <summary>
        /// 移动所有物体
        /// </summary>
        [CommandMethod("moveentitytest")]
        public static void MoveEntityDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = tran.GetObject(item, OpenMode.ForWrite) as Entity;
                    if (ent is Polyline)
                    {
                        db.MoveEnt(ent.ObjectId, new Point3d(0, 0, 0), new Point3d(100, 100, 0));
                    }
                }
                tran.Commit();
            }
        }

        [CommandMethod("selectebctest")]
        public static void SelectEntiyByColor()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //PromptEntityResult per = ed.GetEntity("请选择对象");
            TypedValue[] fvalue = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<or"),
                new TypedValue((int)DxfCode.Start, "LWPOLYLINE"),
                new TypedValue((int)DxfCode.Color, "7"),
                new TypedValue((int)DxfCode.Operator, "or>")
            };
            // fvalue.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psr = ed.GetSelection(ft);

            //if (per.Status != PromptStatus.OK) return;
            //using (Transaction tran = db.TransactionManager.StartTransaction())
            //{
            //    Entity ent = (Entity)per.ObjectId.GetObject(OpenMode.ForWrite);
            //    ent.ColorIndex = 3;
            //    tran.Commit();
            //}
            if (psr.Status != PromptStatus.OK) return;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {

                SelectionSet psrSet = psr.Value;
                ObjectId[] oids = psrSet.GetObjectIds();
                foreach (ObjectId item in oids)
                {
                    Entity ent = (Entity)item.GetObject(OpenMode.ForWrite);
                    ent.ColorIndex = 4;

                }
                tran.Commit();
            }
        }

        [CommandMethod("myt")]
        public static void showmytxt()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            DBText dbtext = new DBText();
            dbtext.TextString = "%%1328@200";
            dbtext.Position = new Point3d(0, 0, 0);
            dbtext.Height = 400;
            dbtext.WidthFactor = 0.7;
            dbtext.LayerId = db.AddLayerTool("钢筋");
            dbtext.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            DBText dbtext1 = new DBText();
            dbtext1.TextString = "%%1328@200";
            dbtext1.Position = new Point3d(0, 0, 0);
            dbtext1.Height = 400;
            dbtext1.WidthFactor = 0.7;
            dbtext1.LayerId = db.AddLayerTool("钢筋");
            dbtext1.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");

            //写入文字
            //DBText txt = new DBText();
            //txt.TextString = mianji.ToString();
            //txt.Height = 250;
            //txt.LayerId = db.AddLayer("noprint");
            //txt.HorizontalMode = TextHorizontalMode.TextCenter;
            //txt.VerticalMode = TextVerticalMode.TextBottom;
            //txt.Position = pt1;
            //txt.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            //txt.WidthFactor = 0.7;
            //txt.ColorIndex = 7;
            //acCurDb.AddToModelSpace(txt); 

            db.AddToModelSpace(dbtext);
        }

        [CommandMethod("addword")]
        public static void AddWord()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            DBText dbtext = new DBText();
            dbtext.SetLayerId(db.AddLayerTool("TAB"), true);

            dbtext.TextString = "%%1328@200";
            dbtext.Position = new Point3d(0, 0, 0);
            dbtext.Height = 400;
            dbtext.TextStyleId = db.AddTextStyle1("TSSD_Norm", "tssdeng", "hztxt");

            DBText dbtext1 = new DBText();
            dbtext1.TextString = "%%1328@200";
            dbtext1.Position = new Point3d(1400, 0, 0);
            dbtext1.Height = 400;
            dbtext1.TextStyleId = db.AddTextStyle1("TSSD_Norm", "tssdeng", "hztxt");
            db.AddToModelSpace(dbtext, dbtext1);

            DbtextProcessTool.DoubletoDbtext(db, "sdfdf", 400, new Point3d(0, 1000, 0), new Vector3d(100, 0, 0), "new");
            DbtextProcessTool.DoubletoDbtext(db, "sdfd1f", 800, new Point3d(0, 2000, 0), new Vector3d(100, 0, 0), "new");
        }

        [CommandMethod("ssp")]
        public static void ssp()
        {
            ObjectId oid = ObjectId.Null;
            ObjectId oid1 = ObjectId.Null;
            Polyline bk = new Polyline(4);
            bk.AddVertexAt(0, new Point2d(0, 10000), 0, 0, 0);
            bk.AddVertexAt(1, new Point2d(10000, 10000), 0, 0, 0);
            bk.AddVertexAt(2, new Point2d(10000, 0), 0, 0, 0);
            bk.AddVertexAt(3, new Point2d(0, 0), 0, 0, 0);
            bk.Closed = true;
            bk.ColorIndex = 3;
            db.AddToModelSpace(bk);

            oid = GameObject(oid);
            oid1 = GameObject2(oid1);

        }

        private static ObjectId GameObject(ObjectId oid)
        {
            Polyline pl = new Polyline(4);
            pl.AddVertexAt(0, new Point2d(0, 1000), 0, 0, 0);
            pl.AddVertexAt(1, new Point2d(1000, 1000), 0, 0, 0);
            pl.AddVertexAt(2, new Point2d(1000, 0), 0, 0, 0);
            pl.AddVertexAt(3, new Point2d(0, 0), 0, 0, 0);
            pl.Closed = true;

            oid = db.AddToModelSpace(pl);
            ed.UpdateScreen();
            ed.Regen();
            Thread.Sleep(1000);
            //Vector3d v = new Vector3d(0, 1000, 0);
            //Matrix3d mtr = Matrix3d.Displacement(v);
            //DBObject dbobj = oid.GetObject(OpenMode.ForWrite);
            for (int i = 0; i < 9; i++)
            {
                db.MoveEnt(oid, new Point3d(0, i * 1000, 0), new Point3d(0, i * 1000 + 1000, 0));
                ed.UpdateScreen();
                //ed.Regen();
                Thread.Sleep(100);
            }
            return oid;
        }
        private static ObjectId GameObject2(ObjectId oid)
        {

            Polyline pl = new Polyline(4);
            pl.AddVertexAt(0, new Point2d(1000, 1000), 0, 0, 0);
            pl.AddVertexAt(1, new Point2d(2000, 1000), 0, 0, 0);
            pl.AddVertexAt(2, new Point2d(2000, 0), 0, 0, 0);
            pl.AddVertexAt(3, new Point2d(1000, 0), 0, 0, 0);
            pl.Closed = true;
            oid = db.AddToModelSpace(pl);
            ed.UpdateScreen();
            ed.Regen();
            Thread.Sleep(1000);
            //Vector3d v = new Vector3d(0, 1000, 0);
            //Matrix3d mtr = Matrix3d.Displacement(v);
            //DBObject dbobj = oid.GetObject(OpenMode.ForWrite);
            for (int i = 0; i < 9; i++)
            {
                db.MoveEnt(oid, new Point3d(1000, i * 1000, 0), new Point3d(1000, i * 1000 + 1000, 0));
                ed.UpdateScreen();
                //ed.Regen();
                Thread.Sleep(200);
            }
            return oid;
        }
    }
}
