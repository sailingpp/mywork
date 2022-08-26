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
    public static class Circlebh
    {
        [CommandMethod("circlebh")]

        public static void CircleBianHaoVersion1()
        {
            // 获得当前文档和数据库   Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "CIRCLE"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            int n = 0;


            // 启动一个事务  Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // 要求在图形区域中选择对象    Request for objects to be selected in the drawing area
                PromptSelectionResult acSSPrompt = acDoc.Editor.GetSelection(acSelFtr);

                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                OpenMode.ForRead) as BlockTable;

                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;



                // 如果提示状态是 OK，对象就被选择了    If the prompt status is OK, objects were selected
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = acSSPrompt.Value;
                    double[] rx = new double[acSSet.Count];
                    double[] ry = new double[acSSet.Count];
                    // 遍历选择集中的对象   Step through the objects in the selection set
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        // 检查以确定返回的 SelectedObject 对象是有效的     Check to make sure a valid SelectedObject object was returned
                        if (acSSObj != null)
                        {
                            // 以写的方式打开选择的对象   Open the selected object for write                     
                            Circle a = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as Circle;
                            DBText objText = new DBText();
                            if (a != null)
                            {
                                objText.SetDatabaseDefaults();
                                objText.TextString = n.ToString();
                                rx[n] = a.Center.X;
                                ry[n] = a.Center.Y;
                                objText.Height = a.Radius / 4;
                                objText.Position = new Autodesk.AutoCAD.Geometry.Point3d(rx[n], ry[n], 0);
                                n = n + 1;
                                // 添加新对象到块表记录和事务中   Add the new object to the block table record and the transaction
                                acBlkTblRec.AppendEntity(objText);
                                acTrans.AddNewlyCreatedDBObject(objText, true);
                            }
                        }
                    }
                    // 保存新对象到数据库中   Save the new object to the database
                    acTrans.Commit();
                }
                // Dispose of the transaction
            }
        }
    }
}
