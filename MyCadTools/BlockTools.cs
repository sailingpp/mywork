using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public class BlockTools
    {
        [CommandMethod("bce")]
        public void BlockCreate02()
        {

            Database db = HostApplicationServices.WorkingDatabase;

            String blockName = "door";
            List<Entity> list = new List<Entity>();
            //创建图形，设置门框的左边线 
            Point3d pt1 = Point3d.Origin;
            Point3d pt2 = new Point3d(0, 1, 0);
            Point3d pt3 = new Point3d(1, 0, 0);
            Line leftLine = new Line(pt1, pt2);
            Line rightLine = new Line(pt1, pt3);
            Arc arc = new Arc(new Point3d(0, 0, 0), 1, 0, Math.PI / 2.0);
            list.Add(leftLine);
            list.Add(rightLine);
            list.Add(arc);
            //调用抽取的公共代码块
            AddBlockThroughDB(db, blockName, list);
        }

        /**
         * 以事务的方式，创建块对象
         * 
         */
        public void AddBlockThroughDB(Database db, String blockName, List<Entity> ents)
        {
            Transaction trans = db.TransactionManager.StartTransaction();
            BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForWrite);
            if (!bt.Has(blockName))
            {
                BlockTableRecord btr = new BlockTableRecord();
                btr.Name = blockName;
                for (int ii = 0; ii < ents.Count; ii++)
                {
                    Entity ent = ents[ii];
                    btr.AppendEntity(ent);
                }
                bt.Add(btr);
                trans.AddNewlyCreatedDBObject(btr, true);
                trans.Commit();
            }
        }
        [CommandMethod("Ind")]
        public void InsertDoor()
        {
            Database db = HostApplicationServices.WorkingDatabase;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                BlockTableRecord space = db.CurrentSpaceId.GetObject(OpenMode.ForWrite) as BlockTableRecord;
                //判断名为“door”的块是否存在
                if (!bt["door"].IsNull)
                {
                    BlockReference br = new BlockReference(Point3d.Origin, bt["door"]);
                    br.ScaleFactors = new Scale3d(2.0);//设置尺寸为原来2倍
                    space.AppendEntity(br);
                    trans.AddNewlyCreatedDBObject(br, true);

                    trans.Commit();
                }
                else
                {
                    return;
                }

            }
        }
        [CommandMethod("mad")]
        public void makeAttDoor()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
                BlockTableRecord btr = new BlockTableRecord();
                btr.Name = "DOOR";

                Line line = new Line(new Point3d(0, 0, 0), new Point3d(0, 1, 0));
                Arc arc = new Arc(new Point3d(0, 0, 0), 1, 0, Math.PI / 2.0);
                btr.AppendEntity(line);
                btr.AppendEntity(arc);

                //属性添加
                AttributeDefinition cjAd = new AttributeDefinition(Point3d.Origin, "喜临门", "厂家", "请输入厂家", ObjectId.Null);
                AttributeDefinition jgAd = new AttributeDefinition(Point3d.Origin + new Vector3d(0, 0.25, 0),
                    "2000", "价格", "请输入价格", ObjectId.Null);
                cjAd.Height = 0.15; jgAd.Height = 0.15;
                btr.AppendEntity(cjAd);
                btr.AppendEntity(jgAd);

                bt.Add(btr);
                trans.AddNewlyCreatedDBObject(btr, true);
                trans.Commit();
            }
        }
    }
}
