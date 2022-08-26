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

namespace MyCadTools
{
    public static class SelectPointToPlineTools
    {
        /// <summary>
        /// 选取定点组成多段线
        /// </summary>
        [CommandMethod("mp")]
        public static void MPtools()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point3dCollection pt = new Point3dCollection();
            List<Point3d> pt3d = new List<Point3d>();
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            List<Point2d> listPt = new List<Point2d>();
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                if (re1.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet1 = re1.Value;

                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            DBText acEnt = tran.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (acEnt != null)
                            {
                                // 修改对象的颜色为黄色   Change the object's color to Green
                                acEnt.ColorIndex = 5;
                                listPt.Add(new Point2d(acEnt.Position.X, acEnt.Position.Y));
                                pt.Add(acEnt.Position);
                            }
                        }
                    }
                    db.AddToModelSpace(MakePLine(pt));
                    // AddToModelTools.AddToModelSpace(db, MakePLine(listPt));   
                }
                tran.Commit();
            }
        }

        #region MyRegion
        //[CommandMethod("mp2")]
        //public static void MPtools2()
        //{
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    Database db = doc.Database;
        //    Editor ed = doc.Editor;
        //    Point3dCollection pt = new Point3dCollection();
        //    List<Point3d> pt3d = new List<Point3d>();
        //    TypedValue[] acTypValAr = new TypedValue[1];
        //    acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
        //    SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
        //    List<Point2d> listPt = new List<Point2d>();
        //    using (Transaction tran = db.TransactionManager.StartTransaction())
        //    {
        //        BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
        //        BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
        //        PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
        //        if (re1.Status == PromptStatus.OK)
        //        {
        //            SelectionSet acSSet1 = re1.Value;

        //            foreach (SelectedObject acSSObj in acSSet1)
        //            {
        //                if (acSSObj != null)
        //                {
        //                    DBText acEnt = tran.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
        //                    if (acEnt != null)
        //                    {
        //                        // 修改对象的颜色为黄色   Change the object's color to Green
        //                        acEnt.ColorIndex = 5;
        //                        listPt.Add(new Point2d(acEnt.Position.X, acEnt.Position.Y));
        //                        pt.Add(acEnt.Position);
        //                    }
        //                }
        //            }
        //            db.AddToModelSpace(MakePLine(listPt));
        //        }
        //        tran.Commit();
        //    }
        //}
        #endregion
        
        /// <summary>
        /// 一排一排实现
        /// </summary>
        [CommandMethod("mp3")]
        public static void MPtools3()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptIntegerResult pirm = ed.GetInteger("请输入m");
            Point3dCollection pt = new Point3dCollection();
            List<Point3d> pt3d = new List<Point3d>();
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            List<Point2d> listPt = new List<Point2d>();
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                if (re1.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet1 = re1.Value;

                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            DBText acEnt = tran.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (acEnt != null)
                            {
                                // 修改对象的颜色为黄色   Change the object's color to Green
                                acEnt.ColorIndex = 5;
                                listPt.Add(new Point2d(acEnt.Position.X, acEnt.Position.Y));
                            }
                        }
                    }

                    for (int i = 0; i < pirm.Value - 1; i++)
                    {
                        Polyline pl = new Polyline();
                        pl.AddVertexAt(0, listPt[i], 0, 0, 0);
                        pl.AddVertexAt(1, listPt[i + 1], 0, 0, 0);
                        pl.AddVertexAt(2, listPt[i + pirm.Value + 1], 0, 0, 0);
                        pl.AddVertexAt(3, listPt[i + pirm.Value], 0, 0, 0);
                        pl.Closed = true;
                        db.AddToModelSpace(pl);
                    }
                }
                tran.Commit();
            }
        }
        /// <summary>
        /// 批量实现
        /// </summary>
        [CommandMethod("mp4")]
        public static void MPtools4()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptIntegerResult pirm = ed.GetInteger("请输入m");
            PromptIntegerResult pirn = ed.GetInteger("请输入n");
            Point3dCollection pt = new Point3dCollection();
            List<Point3d> pt3d = new List<Point3d>();
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            List<Point2d> listPt = new List<Point2d>();
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                if (re1.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet1 = re1.Value;

                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            DBText acEnt = tran.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (acEnt != null)
                            {
                                // 修改对象的颜色为黄色   Change the object's color to Green
                                acEnt.ColorIndex = 5;
                                listPt.Add(new Point2d(acEnt.Position.X, acEnt.Position.Y));
                            }
                        }
                    }
                    for (int j = 0; j < pirn.Value - 1; j++)
                    {
                        for (int i = 0; i < pirm.Value - 1; i++)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, listPt[i + j * pirm.Value], 0, 0, 0);
                            pl.AddVertexAt(1, listPt[i + 1 + j * pirm.Value], 0, 0, 0);
                            pl.AddVertexAt(2, listPt[i + pirm.Value + 1 + j * pirm.Value], 0, 0, 0);
                            pl.AddVertexAt(3, listPt[i + pirm.Value + j * pirm.Value], 0, 0, 0);
                            pl.Closed = true;
                            db.AddToModelSpace(pl);

                        }
                    }

                }
                tran.Commit();
            }
        }

        public static Polyline MakePLine(Database db, List<Point2d> listPt)
        {
            double x;
            double y;
            Polyline pl = new Polyline();
            //for (int i = 0; i < listPt.Count; i++)
            //{
            //    pl.AddVertexAt(i, listPt[i], 0, 0, 0);
            //}
            pl.AddVertexAt(0, listPt[0], 0, 0, 0);
            pl.AddVertexAt(1, listPt[1], 0, 0, 0);
            pl.AddVertexAt(2, listPt[3], 0, 0, 0);
            pl.AddVertexAt(3, listPt[2], 0, 0, 0);
            pl.Closed = true;
            DBText dbtext = new DBText();
            dbtext.TextString = pl.Area.ToString();
            x = (listPt[0].X * pl.Area + listPt[1].X * pl.Area + listPt[3].X * pl.Area + listPt[2].X * pl.Area) / pl.Area;
            y = (listPt[0].Y * pl.Area + listPt[1].Y * pl.Area + listPt[3].Y * pl.Area + listPt[2].Y * pl.Area) / pl.Area;
            Point3d p0 = new Point3d(x, y, 0);
            dbtext.Position = p0;
            dbtext.Height = 400;
            db.AddToModelSpace(dbtext);

            return pl;
        }
        public static Polyline3d MakePLine(Point3dCollection pct)
        {
            Polyline3d pl = new Polyline3d(0, pct, true);

            return pl;
        }
        public static List<Point3d> SortPoint(Point3d pt, List<Point3d> listpt3d)
        {
            List<Point3d> resultListPt3d = new List<Point3d>();
            return resultListPt3d;
        }


    }
}
