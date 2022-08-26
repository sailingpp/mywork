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
    public class SortTextDemo
    {
        /// <summary>
        /// 按文字的y坐标排序
        /// </summary>
        [CommandMethod("tpx1")]
        public void TextPaiXuDemo1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor edtor = doc.Editor;
            List<Point3d> ptList = new List<Point3d>();
            List<Point3d> ptList2 = new List<Point3d>();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult psr = edtor.GetSelection();
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject acSSObj in sset)
                    {
                        if (acSSObj != null)
                        {
                            DBText dbtext = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            ptList.Add(dbtext.Position);

                        }
                    }
                    ptList2 = OrderEntityTools.PaiXuByY(ptList);
                    for (int i = 0; i < ptList2.Count; i++)
                    {
                        DBText test1 = new DBText();
                        test1.Position = ptList2[i];
                        test1.TextString = i.ToString();
                        test1.ColorIndex = 3;
                        test1.Height = 450;
                        db.AddToModelSpace(test1);
                    }

                }
                trans.Commit();



            }
        }
        /// <summary>
        /// 按文字的x坐标排序
        /// </summary>
        [CommandMethod("tpx2")]
        public void TextPaiXuDemo2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;
            Editor edtor = doc.Editor;
            List<Point3d> ptList = new List<Point3d>();
            List<Point3d> ptList2 = new List<Point3d>();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult psr = edtor.GetSelection();
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject acSSObj in sset)
                    {
                        if (acSSObj != null)
                        {
                            DBText dbtext = trans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            ptList.Add(dbtext.Position);

                        }
                    }
                    ptList2 = OrderEntityTools.PaiXuByX(ptList);
                    for (int i = 0; i < ptList2.Count; i++)
                    {
                        DBText test1 = new DBText();
                        test1.Position = ptList2[i];
                        test1.TextString = i.ToString();
                        test1.ColorIndex = 3;
                        test1.Height = 450;
                        db.AddToModelSpace(test1);
                    }

                }
                trans.Commit();



            }
        }

    }
}
