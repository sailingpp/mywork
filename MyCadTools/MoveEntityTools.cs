using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MyCadTools
{
    public static class MoveEntityTools
    {
        /// <summary>
        /// 移动物体
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="vector"></param>
        public static void MoveEntity(this Entity ent, Vector3d vector)
        {
            if (ent.IsNewObject)
            {
                ent.TransformBy(Matrix3d.Displacement(vector));
            }
            else
            {
                ent.ObjectId.MoveEntity(vector);
            }
        }
        public static void MoveEntity(this Database db, Entity ent, Vector3d vector)
        {
            if (ent.IsNewObject)
            {
                ent.TransformBy(Matrix3d.Displacement(vector));
            }
            else
            {
                ent.ObjectId.MoveEntity(vector);
            }
        }
        /// <summary>
        /// 移动物体
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="vector"></param>
        public static void MoveEntity(this ObjectId entId, Vector3d vector)
        {
            Database db = entId.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity entity = entId.GetObject(OpenMode.ForWrite) as Entity;
                entity.TransformBy(Matrix3d.Displacement(vector));
                trans.Commit();
            }
        }
        public static void MoveEntity(this Database db, ObjectId entId, Vector3d vector)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity entity = entId.GetObject(OpenMode.ForWrite) as Entity;
                entity.TransformBy(Matrix3d.Displacement(vector));
                trans.Commit();
            }
        }
        public static void MoveEnt(this Database db, ObjectId oid, Point3d sourcept, Point3d targetpt)
        {
            //Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                Vector3d v = sourcept.GetVectorTo(targetpt);
                Matrix3d mtrx = Matrix3d.Displacement(v);
                ent.TransformBy(mtrx);
                tran.Commit();
            }

        }
        public static void MoveEntbyY(this Database db, ObjectId oid, Point3d sourcept, Point3d targetpt)
        {
            //   Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                //Vector3d v = sourcept.GetVectorTo(targetpt);
                Vector3d v = new Vector3d(0, targetpt.Y - sourcept.Y, 0);
                Matrix3d mtrx = Matrix3d.Displacement(v);
                ent.TransformBy(mtrx);
                tran.Commit();
            }

        }
        public static void MoveEntbyY(this Database db, ObjectId oid, double y)
        {
            //   Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tran.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                //Vector3d v = sourcept.GetVectorTo(targetpt);
                Vector3d v = new Vector3d(0, y, 0);
                Matrix3d mtrx = Matrix3d.Displacement(v);
                ent.TransformBy(mtrx);
                tran.Commit();
            }

        }

        public static void MoveLineStartPointbyY(this Database db, ObjectId oid, double y)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Line line = oid.GetObject(OpenMode.ForWrite) as Line;
                Vector3d v = new Vector3d(0, y, 0);
                Point3d newpoint = line.StartPoint + v;
                line.StartPoint = newpoint;
                trans.Commit();
            }

        }
        public static void MoveLineEndPointbyY(this Database db, ObjectId oid, double y)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Line line = oid.GetObject(OpenMode.ForWrite) as Line;
                Vector3d v = new Vector3d(0, y, 0);
                Point3d newpoint = line.EndPoint + v;
                line.EndPoint = newpoint;
                trans.Commit();
            }

        }
        public static void MoveLineStartEndPointbyY(this Database db, ObjectId oid, double y1, double y2)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Line line = oid.GetObject(OpenMode.ForWrite) as Line;
                Vector3d v1 = new Vector3d(0, y1, 0);
                Vector3d v2 = new Vector3d(0, y2, 0);
                Point3d newpoint1 = line.StartPoint + v1;
                line.StartPoint = newpoint1;
                Point3d newpoint2 = line.EndPoint + v2;
                line.EndPoint = newpoint2;
                trans.Commit();
            }

        }
        public static void SetLineStartEndPointbyY(this Database db, ObjectId oid, double y1, double y2)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Line line = oid.GetObject(OpenMode.ForWrite) as Line;

                Point3d newpoint1 = new Point3d(line.StartPoint.X, y1, line.StartPoint.Z);
                line.StartPoint = newpoint1;
                Point3d newpoint2 = new Point3d(line.EndPoint.X, y2, line.EndPoint.Z);
                line.EndPoint = newpoint2;
                trans.Commit();
            }

        }
        public static void SetLineStartEndPointbyX(this Database db, ObjectId oid, double x1, double x2)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Line line = oid.GetObject(OpenMode.ForWrite) as Line;

                Point3d newpoint1 = new Point3d(x1, line.StartPoint.Y, line.StartPoint.Z);
                line.StartPoint = newpoint1;
                Point3d newpoint2 = new Point3d(x2, line.EndPoint.Y, line.EndPoint.Z);
                line.EndPoint = newpoint2;
                trans.Commit();
            }

        }


    }
}
