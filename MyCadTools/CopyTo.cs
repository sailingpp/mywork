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
    public class CopyTo
    {
        /// <summary>
        /// 将外部图形全部复制
        /// 路径D:\test.dwg
        /// </summary>
        [CommandMethod("copyto")]
        public void TestCopyBetweenDwgFiles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                using (Database srcDb = new Database(false, false))
                {
                    srcDb.ReadDwgFile(@"D:\test.dwg", FileOpenMode.OpenForReadAndReadShare, true, "");
                    ObjectIdCollection oids = GetDbModelSpaceEntities(srcDb);
                    if (oids.Count > 0)
                    {
                        IdMapping idMap = new IdMapping();
                        srcDb.WblockCloneObjects(oids, db.CurrentSpaceId, idMap, DuplicateRecordCloning.Ignore, false);
                        tr.AddNewlyCreatedDBObject(acBlkTblRec, true);
                    }
                }
                ToModelSpace(acBlkTblRec, new Point3d(0, 0, 0), db);
                tr.Commit();
            }
        }
        private ObjectId ToModelSpace(Entity ent, Database db)
        {
            ObjectId entId;
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {

                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                entId = acBlkTblRec.AppendEntity(ent);
                acTrans.AddNewlyCreatedDBObject(ent, true);
            }
            return entId;
        }
        private ObjectId ToModelSpace(BlockTableRecord block, Point3d pt, Database db)
        {
            BlockReference br = null;
            ObjectId blkrfid = new ObjectId();
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                br = new BlockReference(pt, block.ObjectId);
                blkrfid = acBlkTblRec.AppendEntity(br);
                acTrans.AddNewlyCreatedDBObject(br, true);
                //foreach (ObjectId  id in block)
                //{
                //    if (id.ObjectClass.Equals(RXClass.GetClass(typeof(AttributeDefinition))))
                //    {
                //        AttributeDefinition ad = acTrans.GetObject(id, OpenMode.ForRead) as AttributeDefinition;
                //        AttributeReference ar = new AttributeReference(ad.Position, ad.TextString, ad.Tag, new ObjectId());
                //        br.AttributeCollection.AppendAttribute(ar);
                //    }
                //}
                acTrans.Commit();
            }
            return blkrfid;
        }
        public BlockReference NewBlockRefrence(BlockTableRecord btr, Point3d location)
        {
            BlockReference br = null;
            Database db = btr.Database;
            if (db != null)
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    br = new BlockReference(location, btr.ObjectId);
                    foreach (ObjectId id in btr)
                    {
                        if (id.ObjectClass.Equals(RXClass.GetClass(typeof(AttributeDefinition))))
                        {
                            AttributeDefinition ad = trans.GetObject(id, OpenMode.ForRead) as AttributeDefinition;
                            AttributeReference ar = new AttributeReference(ad.Position, ad.TextString, ad.Tag, new ObjectId());
                            br.AttributeCollection.AppendAttribute(ar);
                        }
                    }
                    trans.Commit();
                }
            }
            return br;
        }
        /// <summary>
        /// 获取数据库模型空间的所有图元
        /// </summary>
        /// 
        private ObjectIdCollection GetDbModelSpaceEntities(Database db)
        {
            ObjectIdCollection oids = new ObjectIdCollection();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpace = tr.GetObject(
                    blockTbl[BlockTableRecord.ModelSpace],
                    OpenMode.ForRead) as BlockTableRecord;

                foreach (ObjectId oid in modelSpace)
                {
                    DBObject dbobj = tr.GetObject(oid, OpenMode.ForRead);
                    if (dbobj is Entity)
                    {
                        oids.Add(oid);
                    }
                }

            }

            return oids;
        }

    }
}
