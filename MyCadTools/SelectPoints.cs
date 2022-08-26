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
    public class SelectPoints
    {
        [CommandMethod("selectpoints")]
        public static void SelectPointsDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int n = 0;

            double cx = 0;
            double cy = 0;
            double cz = 0;

            TypedValue[] acTypValAr = new TypedValue[1];

            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "POINT"), 0);

            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);

                if (re1.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = re1.Value;
                    Application.ShowAlertDialog("Number of objects selected: " + acSSet.Count.ToString());

                    string[] str = new string[acSSet.Count];
                    double[] num = new double[acSSet.Count];
                    double[] x = new double[acSSet.Count];
                    double[] y = new double[acSSet.Count];
                    double[] z = new double[acSSet.Count];

                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        if (acSSObj != null)
                        {
                            DBPoint point1 = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBPoint;
                            x[n] = point1.Position.X;
                            Application.ShowAlertDialog("Number of objects selected: " + x[n]);
                            n = n + 1;
                        }

                    }
                    DBPoint acPoint = new DBPoint(new Point3d(cx, cy, cz));
                    acPoint.SetDatabaseDefaults();
                    acBlkTblRec.AppendEntity(acPoint);
                    acTrans.AddNewlyCreatedDBObject(acPoint, true);
                    db.Pdmode = 3;
                    db.Pdsize = 1;

                    //append the new mtext object to model space


                }
                acTrans.Commit();

            }

        }
    }
}
