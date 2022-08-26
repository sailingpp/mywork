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
    public static class ScaleEntity
    {
        [CommandMethod("mv")]
        public static void MoveEntiyMethod()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = item.GetObject(OpenMode.ForWrite) as Entity;
                    MoveEntitys(ent, new Point3d(0, 0, 0), new Point3d(100, 100, 0));
                    ScaleEntitys(ent, new Point3d(0, 0, 0), 4);
                }
                tran.Commit();
            }
        }
        /// <summary>
        /// 移动对象
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void MoveEntitys(Entity ent, Point3d pt1, Point3d pt2)
        {
            Vector3d v = pt1.GetVectorTo(pt2);
            Matrix3d m = Matrix3d.Displacement(v);
            ent.TransformBy(m);
        }
        /// <summary>
        /// 放大对象
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="pt1"></param>
        /// <param name="num"></param>
        public static void ScaleEntitys(Entity ent, Point3d pt1, double num)
        {
            Matrix3d m = Matrix3d.Scaling(num, pt1);
            ent.TransformBy(m);
        }

    }
}
