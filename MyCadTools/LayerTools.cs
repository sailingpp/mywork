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
    public static class LayerTools
    {
        /// <summary>
        /// 增加图层
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <param name="layerName">图层名称</param>
        /// <returns></returns>
        public static ObjectId AddLayerTool(this Database db, string layerName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                if (!lt.Has(layerName))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = layerName;
                    lt.UpgradeOpen();
                    oid = lt.Add(ltr);
                    lt.DowngradeOpen();
                    trans.AddNewlyCreatedDBObject(ltr, true);
                    ed.WriteMessage("图层增加成功！\r\n");
                }
                else
                {
                    oid = lt[layerName];
                    ed.WriteMessage("原图层已经存在！\r\n");

                }
                trans.Commit();
            }
            return oid;

        }
        /// <summary>
        /// 增加图层并设置颜色
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <param name="layerName">图层名称</param>
        /// <param name="colorNum">颜色index</param>
        /// <returns></returns>
        public static ObjectId AddLayerTool(this Database db, string layerName, short colorNum)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                if (!lt.Has(layerName))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = layerName;
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByColor, colorNum);
                    lt.UpgradeOpen();
                    oid = lt.Add(ltr);
                    lt.DowngradeOpen();
                    trans.AddNewlyCreatedDBObject(ltr, true);
                    ed.WriteMessage("图层增加成功！\r\n");
                }
                else
                {
                    oid = lt[layerName];
                    ed.WriteMessage("原图层已经存在！\r\n");

                }
                trans.Commit();
            }
            return oid;

        }
        /// <summary>
        /// 列出图层名和对应的颜色号
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetAllLayerName(this Database db)
        {
            Dictionary<string, int> layerNameList = new Dictionary<string, int>();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId item in lt)
                {
                    LayerTableRecord ltr = item.GetObject(OpenMode.ForRead) as LayerTableRecord;
                    layerNameList.Add(ltr.Name, ltr.Color.ColorIndex);
                }
                trans.Commit();
            }
            return layerNameList;
        }
        /// <summary>
        /// 根据ent获取图层
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ent"></param>
        /// <returns></returns>
        public static LayerTableRecord GetLayerRecord(this Database db, Entity ent)
        {
            LayerTableRecord ltr = new LayerTableRecord();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                //BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                ltr = lt[ent.Layer].GetObject(OpenMode.ForRead) as LayerTableRecord;
                trans.Commit();
            }
            return ltr;
        }
        /// <summary>
        /// 修改图层的名字
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <param name="oldLayerName">旧图层的名字</param>
        /// <param name="newLayerName">修改后的图层的名字</param>
        /// <returns></returns>
        public static ObjectId ChangeLayerNameTool(this Database db, string oldLayerName, string newLayerName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                if (lt.Has(oldLayerName))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr = lt[oldLayerName].GetObject(OpenMode.ForWrite) as LayerTableRecord;
                    ltr.Name = newLayerName;
                    ed.WriteMessage("图层修改成功！\r\n");
                }
                else
                {
                    ed.WriteMessage("原图层名不存在！\r\n");

                }
                trans.Commit();
            }
            return oid;

        }
        /// <summary>
        /// 改变指定图层名的颜色
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="layerName">指定图层的名称</param>
        /// <param name="colorNum">图层的颜色</param>
        /// <returns></returns>
        public static ObjectId ChangeLayerColorTool(this Database db, string layerName, short colorNum)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                if (lt.Has(layerName))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr = lt[layerName].GetObject(OpenMode.ForWrite) as LayerTableRecord;
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByColor, colorNum);
                    ed.WriteMessage("图层颜色修改成功！\r\n");
                }
                else
                {
                    ed.WriteMessage("原图层名不存在！\r\n");

                }
                trans.Commit();
            }
            return oid;
        }

        

    }

}
