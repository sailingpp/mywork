using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace MyCadTools
{
    public class PlateSteel
    {

        ObjectId oid = ObjectId.Null;
        Polyline pl = new Polyline();

    }
    public class SteelLine_Plate
    {
        Database db = HostApplicationServices.WorkingDatabase;
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

        public SteelLine_Plate()
        {
            //PromptEntityOptions peo = new PromptEntityOptions("请选择梁线");
            //PromptEntityResult per = ed.GetEntity(peo);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                trans.Commit();
            }
        }
    }
}
