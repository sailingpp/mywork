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
    /// <summary>
    /// 取得两组直线的交点
    /// </summary>
    public static class GetLineCrossPoints
    {
        [CommandMethod("getlcp1")]
        public static void GetCrossPointVersion1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] acTypValAr = new TypedValue[1];
            Point3d[] pt;
            List<Point3d> pv = new List<Point3d>();
            Dictionary<Point3d, int> myDic = new Dictionary<Point3d, int>();

            List<Line> listLine1 = new List<Line>();
            List<Line> listLine2 = new List<Line>();
            // acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);

            //SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                // PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                ed.WriteMessage("请选择第一组直线");
                PromptSelectionResult re1 = ed.GetSelection();
                ed.WriteMessage("请选择第二组直线");
                PromptSelectionResult re2 = ed.GetSelection();
                if (re1.Status == PromptStatus.OK)
                {
                    ed.WriteMessage("第一组直线选取成功");
                    SelectionSet acSSet1 = re1.Value;

                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                            if (acEnt != null)
                            {
                                // 修改对象的颜色为黄色   Change the object's color to Green
                                acEnt.ColorIndex = 2;
                                listLine1.Add(acEnt);
                            }
                        }
                    }
                    if (re2.Status == PromptStatus.OK)
                    {
                        ed.WriteMessage("第二组直线选取成功");
                        SelectionSet acSSet2 = re2.Value;
                        foreach (SelectedObject acSSObj in acSSet2)
                        {
                            if (acSSObj != null)
                            {
                                Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                                if (acEnt != null)
                                {
                                    // 修改对象的颜色为绿色   Change the object's color to Green
                                    acEnt.ColorIndex = 3;
                                    listLine2.Add(acEnt);
                                }
                            }
                        }
                    }
                    for (int j = 0; j < listLine1.Count; j++)
                    {
                        for (int i = 0; i < listLine2.Count; i++)
                        {
                            pt = GPTools.GetCrossPoint(db, listLine1[j], listLine2[i]);
                            pv.Add(pt[0]);
                            ed.WriteMessage("\nX=" + pt[0].X.ToString() + ",Y=" + pt[0].Y.ToString());
                        }
                    }
                    for (int i = 0; i < pv.Count; i++)
                    {
                        // Application.ShowAlertDialog("\nX=" + pv[i].X.ToString() + ",Y=" + pv[i].Y.ToString());
                        DBText dbtext = new DBText();
                        dbtext.SetDatabaseDefaults();
                        dbtext.Height = 200;
                        dbtext.TextString = i.ToString();
                        dbtext.Position = pv[i];
                        dbtext.WidthFactor = 0.7;

                        btr.AppendEntity(dbtext);
                        trans.AddNewlyCreatedDBObject(dbtext, true);
                    }
                }

                trans.Commit();
            }
        }

        [CommandMethod("getlcp2")]
        public static void GetCrossPointVersion2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] acTypValAr = new TypedValue[1];
            Point3d[] pt;
            List<Point3d> pv = new List<Point3d>();
            List<Point3d> pvtemp = new List<Point3d>();
            Dictionary<Point3d, int> myDic = new Dictionary<Point3d, int>();

            List<Line> listLine1 = new List<Line>();
            List<Line> listLine2 = new List<Line>();
            // acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);

            //SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                // PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                ed.WriteMessage("请选择第一组直线");
                PromptSelectionResult re1 = ed.GetSelection();
                ed.WriteMessage("请选择第二组直线");
                PromptSelectionResult re2 = ed.GetSelection();
                if (re1.Status == PromptStatus.OK)
                {
                    ed.WriteMessage("第一组直线选取成功");
                    SelectionSet acSSet1 = re1.Value;

                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                            if (acEnt != null)
                            {
                                // 修改对象的颜色为黄色   Change the object's color to Green
                                acEnt.ColorIndex = 2;
                                listLine1.Add(acEnt);
                            }
                        }
                    }
                    if (re2.Status == PromptStatus.OK)
                    {
                        ed.WriteMessage("第二组直线选取成功");
                        SelectionSet acSSet2 = re2.Value;
                        foreach (SelectedObject acSSObj in acSSet2)
                        {
                            if (acSSObj != null)
                            {
                                Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                                if (acEnt != null)
                                {
                                    // 修改对象的颜色为绿色   Change the object's color to Green
                                    acEnt.ColorIndex = 3;
                                    listLine2.Add(acEnt);
                                }
                            }
                        }
                    }
                    for (int j = 0; j < listLine1.Count; j++)
                    {
                        for (int i = 0; i < listLine2.Count; i++)
                        {
                            pt = GPTools.GetCrossPoint(db, listLine1[j], listLine2[i]);
                            pv.Add(pt[0]);
                        }

                    }
                    var temp = from item in pv orderby item.X select item;
                    pvtemp.AddRange(temp);
                    for (int i = 0; i < pvtemp.Count; i++)
                    {
                        DBText dbtext = new DBText();
                        dbtext.SetDatabaseDefaults();
                        dbtext.Height = 200;
                        dbtext.TextString = i.ToString();
                        dbtext.Position = pvtemp[i];
                        dbtext.WidthFactor = 0.7;
                        btr.AppendEntity(dbtext);
                        trans.AddNewlyCreatedDBObject(dbtext, true);
                    }
                }

                trans.Commit();
            }
        }

        [CommandMethod("getlcp3")]
        public static void GetCrossPointVersion3()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] acTypValAr = new TypedValue[1];
            Point3d[] pt;
            List<Point3d> pv = new List<Point3d>();
            List<Point3d> pvtemp1 = new List<Point3d>();
            List<Point3d> pvtemp2 = new List<Point3d>();
            Dictionary<Point3d, int> myDic = new Dictionary<Point3d, int>();

            List<Line> listLine1 = new List<Line>();
            List<Line> listLine2 = new List<Line>();
            // acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);

            //SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                // PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                ed.WriteMessage("请选择第一组直线");
                PromptSelectionResult re1 = ed.GetSelection();
                ed.WriteMessage("请选择第二组直线");
                PromptSelectionResult re2 = ed.GetSelection();
                if (re1.Status == PromptStatus.OK)
                {
                    ed.WriteMessage("第一组直线选取成功");
                    SelectionSet acSSet1 = re1.Value;

                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                            if (acEnt != null)
                            {
                                // 修改对象的颜色为黄色   Change the object's color to Green
                                acEnt.ColorIndex = 2;
                                listLine1.Add(acEnt);
                            }
                        }
                    }
                    if (re2.Status == PromptStatus.OK)
                    {
                        ed.WriteMessage("第二组直线选取成功");
                        SelectionSet acSSet2 = re2.Value;
                        foreach (SelectedObject acSSObj in acSSet2)
                        {
                            if (acSSObj != null)
                            {
                                Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                                if (acEnt != null)
                                {
                                    // 修改对象的颜色为绿色   Change the object's color to Green
                                    acEnt.ColorIndex = 3;
                                    listLine2.Add(acEnt);
                                }
                            }
                        }
                    }
                    for (int j = 0; j < listLine1.Count; j++)
                    {
                        for (int i = 0; i < listLine2.Count; i++)
                        {
                            pt = GPTools.GetCrossPoint(db, listLine1[j], listLine2[i]);
                            pv.Add(pt[0]);
                        }

                    }
                    var temp1 = from item in pv orderby item.X select item;
                    pvtemp1.AddRange(temp1);
                    var temp2 = from item in pvtemp1 orderby item.Y select item;
                    pvtemp2.AddRange(temp2);
                    for (int i = 0; i < pvtemp2.Count; i++)
                    {
                        DBText dbtext = new DBText();
                        dbtext.SetDatabaseDefaults();
                        dbtext.Height = 200;
                        dbtext.TextString = i.ToString();
                        dbtext.Position = pvtemp2[i];
                        dbtext.WidthFactor = 0.7;
                        btr.AppendEntity(dbtext);
                        trans.AddNewlyCreatedDBObject(dbtext, true);
                    }
                }

                trans.Commit();
            }
        }

        [CommandMethod("getlcp4")]
        public static void GetCrossPointVersion4()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] acTypValAr = new TypedValue[1];
            Point3d[] pt;
            List<Point3d> pv = new List<Point3d>();
            List<Point3d> pvtemp1 = new List<Point3d>();
            List<Point3d> pvtemp2 = new List<Point3d>();
            Dictionary<Point3d, int> myDic = new Dictionary<Point3d, int>();

            List<Line> listLine1 = new List<Line>();
            List<Line> listLine2 = new List<Line>();
            // acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);

            //SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                // PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                ed.WriteMessage("请选择第一组直线");
                PromptSelectionResult re1 = ed.GetSelection();
                ed.WriteMessage("请选择第二组直线");
                PromptSelectionResult re2 = ed.GetSelection();
                if (re1.Status == PromptStatus.OK)
                {
                    ed.WriteMessage("第一组直线选取成功");
                    SelectionSet acSSet1 = re1.Value;
                    SelectionSet acSSet2 = re2.Value;
                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                            if (acEnt != null)
                            {
                                // 修改对象的颜色为黄色   Change the object's color to Green
                                acEnt.ColorIndex = 2;
                                listLine1.Add(acEnt);
                            }
                        }
                    }
                    if (re2.Status == PromptStatus.OK)
                    {
                        ed.WriteMessage("第二组直线选取成功");

                        foreach (SelectedObject acSSObj in acSSet2)
                        {
                            if (acSSObj != null)
                            {
                                Line acEnt = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Line;
                                if (acEnt != null)
                                {
                                    // 修改对象的颜色为绿色   Change the object's color to Green
                                    acEnt.ColorIndex = 3;
                                    listLine2.Add(acEnt);
                                }
                            }
                        }
                    }
                    //   int[,] mtrx = new int[acSSet1.Count, acSSet2.Count];
                    for (int j = 0; j < listLine1.Count; j++)
                    {
                        for (int i = 0; i < listLine2.Count; i++)
                        {
                            pt = GPTools.GetCrossPoint(db, listLine1[j], listLine2[i]);
                            pv.Add(pt[0]);
                        }
                    }
                    var temp1 = from item in pv orderby item.X select item;
                    pvtemp1.AddRange(temp1);
                    var temp2 = from item in pvtemp1 orderby item.Y select item;
                    pvtemp2.AddRange(temp2);
                    for (int i = 0; i < pvtemp2.Count; i++)
                    {
                        DBText dbtext = new DBText();
                        dbtext.SetDatabaseDefaults();
                        dbtext.Height = 200;
                        dbtext.TextString = i.ToString();
                        dbtext.Position = pvtemp2[i];
                        dbtext.WidthFactor = 0.7;
                        btr.AppendEntity(dbtext);
                        trans.AddNewlyCreatedDBObject(dbtext, true);
                    }
                }

                trans.Commit();
            }
        }


    }


    public static class GPTools
    {
        public static Point3d[] GetCrossPoint(this Database db, Line line1, Line line2)
        {
            Point3d[] pc;
            Line3d line11 = LineToLine3d(line1);
            Line3d line22 = LineToLine3d(line2);
            pc = line11.IntersectWith(line22);
            return pc;
        }
        public static Line3d LineToLine3d(Line line)
        {
            Line3d line3d = new Line3d(line.StartPoint, line.EndPoint);
            return line3d;
        }
    }
}
