using Autodesk.AutoCAD.ApplicationServices;
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
    /// 取得两组直线的交点，并在交点生成圆
    /// </summary>
    public static class DrawCircleInCrossDot
    {
        [CommandMethod("getcr")]
        public static void GetCrossPointDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] acTypValAr = new TypedValue[1];
            //储存交点集合
            Point3d[] pt;
            List<Point3d> pv = new List<Point3d>();
            //存储第一组直线
            List<Line> listLine1 = new List<Line>();
            //存储第二组直线
            List<Line> listLine2 = new List<Line>();
            //只选取直线过滤
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "LINE"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                ed.WriteMessage("请选择第一组直线");
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                ed.WriteMessage("请选择第二组直线");
                PromptSelectionResult re2 = ed.GetSelection(acSelFtr);
                PromptDoubleResult pdr = ed.GetDouble("请输入圆半径");
                double radius = pdr.Value;
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
                    //求直线系交点，返回交点集合
                    for (int j = 0; j < listLine1.Count; j++)
                    {
                        for (int i = 0; i < listLine2.Count; i++)
                        {
                            pt = GPTools.GetCrossPoint(db, listLine1[j], listLine2[i]);
                            pv.Add(pt[0]);

                        }
                    }
                    //在交点处绘制圆
                    for (int i = 0; i < pv.Count; i++)
                    {
                        db.ToAddCircles(radius, pv[i]);
                    }
                }
                trans.Commit();
            }
        }
    }
}
