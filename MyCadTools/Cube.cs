using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public class Cube
    {
        public Database db = HostApplicationServices.WorkingDatabase;
        public ObjectId oid = ObjectId.Null;
        public Cube(Point3d po, double width, double height)
        {
            Polyline pl = new Polyline();
            Point2d p0 = new Point2d(po.X, po.Y);
            Point2d p1 = new Point2d(po.X + width, po.Y);
            Point2d p2 = new Point2d(po.X + width, po.Y - height);
            Point2d p3 = new Point2d(po.X, po.Y - height);

            pl.AddVertexAt(0, p0, 0, 0, 0);
            pl.AddVertexAt(1, p1, 0, 0, 0);
            pl.AddVertexAt(2, p2, 0, 0, 0);
            pl.AddVertexAt(3, p3, 0, 0, 0);
            pl.Closed = true;
            oid = db.AddToModelSpace(pl);
        }
        public Cube() { ;}
        public void MoveCube(Vector3d v)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                Matrix3d mt = Matrix3d.Displacement(v);
                ent.TransformBy(mt);
                trans.Commit();
            }
        }
        public void DelCube()
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                ent.Erase(true);
                trans.Commit();
            }
        }

    }
    public class CubeTest
    {
        [CommandMethod("mo")]
        public void mo()
        {

            Cube cube = new Cube(new Point3d(0, 0, 0), 100, 100);
            //cube.MoveCube(new Vector3d(100,0,0));
        }
    }
}
