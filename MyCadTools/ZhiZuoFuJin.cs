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
    public class ZhiZuoFuJin
    {
        [CommandMethod("fj")]
        public void AddZiZuoFuJin()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择梁线");
            PromptEntityResult per = ed.GetEntity(peo);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                if (per.Status == PromptStatus.OK)
                {
                    Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Line;
                    db.AddZiZuoFuJinToModelSpace((Line)ent, 750, 400);
                }
                trans.Commit();
            }




        }
    }
}
