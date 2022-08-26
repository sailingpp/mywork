using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public static class SelectEntityTools
    {
        /// <summary>
        /// 按颜色框择实体
        /// </summary>
        /// <param name="db"></param>
        /// <param name="colorIndex"></param>
        /// <returns></returns>
        public static ObjectIdCollection SelectEntityByColor(this Database db, int colorIndex)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectIdCollection oids = new ObjectIdCollection();
            PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(pso);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (SelectedObject item in psr.Value)
                {
                    Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                    ent.ColorIndex = colorIndex;
                    oids.Add(item.ObjectId);
                }
                trans.Commit();
            }
            return oids;
        }
        /// <summary>
        /// 按图层框择实体
        /// </summary>
        /// <param name="db"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static ObjectIdCollection SelectEntityByLayer(this Database db, string layerName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectIdCollection oids = new ObjectIdCollection();
            PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(pso);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (SelectedObject item in psr.Value)
                {
                    Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent.Layer == layerName)
                    {
                        oids.Add(item.ObjectId);
                    }
                }
                trans.Commit();
            }
            return oids;
        }
        /// <summary>
        /// 改变所有实体的颜色
        /// </summary>
        /// <param name="db"></param>
        /// <param name="colorIndex"></param>
        /// <returns></returns>
        public static ObjectIdCollection ChangeAllEntityByColor(this Database db, int colorIndex)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectIdCollection oids = new ObjectIdCollection();

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = item.GetObject(OpenMode.ForWrite) as Entity;
                    ent.ColorIndex = colorIndex;
                    oids.Add(item);
                }
                trans.Commit();
            }
            return oids;
        }
        /// <summary>
        /// 按颜色选择实体
        /// </summary>
        /// <param name="db"></param>
        /// <param name="colorIndex"></param>
        /// <returns></returns>
        public static ObjectIdCollection SelectAllEntityByColor(this Database db, int colorIndex)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectIdCollection oids = new ObjectIdCollection();

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = item.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent.ColorIndex == colorIndex)
                    {
                        oids.Add(item);
                        ent.Highlight();
                    }
                }
                trans.Commit();
            }
            return oids;
        }
        /// <summary>
        /// 按颜色名选择实体
        /// </summary>
        /// <param name="db"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static ObjectIdCollection SelectAllEntityByLayer(this Database db, string layerName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectIdCollection oids = new ObjectIdCollection();

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in btr)
                {
                    Entity ent = item.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent.Layer == layerName)
                    {
                        oids.Add(item);
                        ent.Highlight();
                    }
                }
                trans.Commit();
            }
            return oids;
        }
        /// <summary>
        /// 写消息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="message"></param>
        public static List<Entity> SelectEntitysByProperty(this Database db, ObjectId entId)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            List<Entity> entityList = new List<Entity>();
            Entity ent;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                ent = entId.GetObject(OpenMode.ForRead) as Entity;
                trans.Commit();
            }
            TypedValue[] tvalue = new TypedValue[]
                   {
                        new TypedValue((int)DxfCode.Operator, "<and"),
                        new TypedValue((int)DxfCode.LayerName,ent.Layer),
                        new TypedValue((int)DxfCode.Operator, "and>"),
                   };
            ed.WriteMessage("请选择图纸范围:\r\n");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            SelectionFilter filter = new SelectionFilter(tvalue);
            PromptSelectionResult psr = ed.GetSelection(pso, filter);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        Entity entity = item.ObjectId.GetObject(OpenMode.ForRead) as Entity;
                        entityList.Add(entity);
                        entity.Highlight();

                    }
                }
                trans.Commit();
            }
            return entityList;
        }
        public static List<Entity> SelectEntitysByLayer(this Database db, string layerName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            List<Entity> entityList = new List<Entity>();
            TypedValue[] tvalue = new TypedValue[]
                   {
                        new TypedValue((int)DxfCode.LayerName,layerName),
                   };
            ed.WriteMessage("请选择图纸范围:\r\n");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            SelectionFilter filter = new SelectionFilter(tvalue);
            PromptSelectionResult psr = ed.GetSelection(pso, filter);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        Entity entity = item.ObjectId.GetObject(OpenMode.ForRead) as Entity;
                        entityList.Add(entity);
                    }
                }
                trans.Commit();
            }
            return entityList;
        }
        /// <summary>
        /// 选择dbtext
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<DBText> SelectDbtext(this Database db)
        {
            List<DBText> dbtextList = new List<DBText>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            TypedValue[] values = new TypedValue[] 
            {
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    if (sset != null)
                    {
                        foreach (ObjectId oid in sset.GetObjectIds())
                        {
                            DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;
                            dbtextList.Add(text);
                        }
                    }
                }
                trans.Commit();
            }
            return dbtextList;
        }
        public static List<DBText> SelectDbtext(this Database db, string layerName)
        {
            List<DBText> dbtextList = new List<DBText>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            TypedValue[] values = new TypedValue[] 
            {
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 new TypedValue ((int)DxfCode.LayerName,layerName),
            };
            SelectionFilter filter = new SelectionFilter(values);
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    if (sset != null)
                    {
                        foreach (ObjectId oid in sset.GetObjectIds())
                        {
                            DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;
                            dbtextList.Add(text);
                        }
                    }
                }
                trans.Commit();
            }
            return dbtextList;
        }


    }
}