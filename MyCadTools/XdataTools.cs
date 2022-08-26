using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyCadTools
{
    public static class XdataTools
    {
        public static void AddXdata(this Database db, Entity ent, string regName, string info)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                RegAppTable rt = trans.GetObject(db.RegAppTableId, OpenMode.ForRead) as RegAppTable;
                if (!rt.Has(regName))
                {
                    RegAppTableRecord rtr = new RegAppTableRecord();
                    rtr.Name = regName;
                    rt.UpgradeOpen();
                    rt.Add(rtr);
                    ResultBuffer buffer = new ResultBuffer();
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataRegAppName, rtr.Name));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString , info));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, ent.ColorIndex));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, ent.GetArea()));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, ent.GetLength()));
                    buffer.Add(new TypedValue(1003, ent.Layer));
                    ent.XData = buffer;
                    rt.DowngradeOpen();
                    trans.AddNewlyCreatedDBObject(rtr, true);
                }
                trans.Commit();
            }
        }

        public static void AddXdata(this Database db, ObjectId entoid, string regName, string info)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = entoid.GetObject(OpenMode.ForWrite) as Entity;
                RegAppTable rt = trans.GetObject(db.RegAppTableId, OpenMode.ForRead) as RegAppTable;
                if (!rt.Has(regName))
                {
                    RegAppTableRecord rtr = new RegAppTableRecord();
                    rtr.Name = regName;
                    rt.UpgradeOpen();
                    rt.Add(rtr);
                    ResultBuffer buffer = new ResultBuffer();
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataRegAppName, rtr.Name));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, info));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, ent.ColorIndex));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, ent.GetArea()));
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, ent.GetLength()));
                    buffer.Add(new TypedValue(1003, ent.Layer));
                    ent.XData = buffer;
                    rt.DowngradeOpen();
                    trans.AddNewlyCreatedDBObject(rtr, true);
                }
                trans.Commit();
            }
        }

    }
}
