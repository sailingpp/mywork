using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace MyCadTools
{
    public static class AddCircleTools
    {
        public static void AddCircles(this Database db, int r)
        {
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Circle c = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), r);
                acBlkTblRec.AppendEntity(c);
                acTrans.AddNewlyCreatedDBObject(c, true);
                acTrans.Commit();
            }
        }
        public static ObjectId ToAddCircles(this Database db, int r)
        {
            ObjectId cid;
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Circle c = new Circle(new Point3d(100, 100, 0), new Vector3d(0, 0, 1), r);
                DBText dbtext = new DBText();
                //
                cid = acBlkTblRec.AppendEntity(c);
                acTrans.AddNewlyCreatedDBObject(c, true);
                acTrans.Commit();
            }
            return cid;
        }
        public static ObjectId ToAddCircles(this Database db, double r, Point3d pt)
        {
            ObjectId cid;
            //多段线线框
            double xk = 30;
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Circle c = new Circle(pt, new Vector3d(0, 0, 1), r);
                DBText dbtext = new DBText();
                Point3d startpoint0 = pt + new Vector3d(-r, 0, 0);
                Point3d endpoint0 = pt + new Vector3d(r, 0, 0);
                Point3d startpoint1 = pt + new Vector3d(0, r, 0);
                Point3d endpoint1 = pt + new Vector3d(0, -r, 0);

                Point2d p0 = new Point2d(startpoint0.X, startpoint0.Y);
                Point2d p1 = new Point2d(endpoint0.X, endpoint0.Y);

                Point2d p3 = new Point2d(startpoint1.X, startpoint1.Y);
                Point2d p4 = new Point2d(endpoint1.X, endpoint1.Y);

                Polyline pl1 = new Polyline();
                pl1.AddVertexAt(0, p0, 0, xk, xk);
                pl1.AddVertexAt(1, p1, 0, xk, xk);
                Polyline pl2 = new Polyline();
                pl2.AddVertexAt(0, p3, 0, xk, xk);
                pl2.AddVertexAt(1, p4, 0, xk, xk);
                db.AddToModelSpace(pl1, pl2);
                cid = acBlkTblRec.AppendEntity(c);
                acTrans.AddNewlyCreatedDBObject(c, true);
                acTrans.Commit();
            }
            return cid;
        }
        public static ObjectId ToModelSpace(this Database db, BlockTableRecord block, Point3d pt)
        {
            BlockReference br = null;
            ObjectId blkrfid = new ObjectId();
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                br = new BlockReference(pt, block.ObjectId);
                blkrfid = acBlkTblRec.AppendEntity(br);
                acTrans.AddNewlyCreatedDBObject(br, true);
                foreach (ObjectId id in block)
                {
                    if (id.ObjectClass.Equals(RXClass.GetClass(typeof(AttributeDefinition))))
                    {
                        AttributeDefinition ad = acTrans.GetObject(id, OpenMode.ForRead) as AttributeDefinition;
                        AttributeReference ar = new AttributeReference(ad.Position, ad.TextString, ad.Tag, new ObjectId());
                        br.AttributeCollection.AppendAttribute(ar);
                    }
                }
                acTrans.Commit();
            }
            return blkrfid;
        }
        #region MyRegion
        ///// <summary>
        ///// 插入带属性的参照快
        ///// </summary>
        ///// <param name="spaceId">空间的ID</param>
        ///// <param name="layer">块要加入的图层名</param>
        ///// <param name="blockName">快参照所属的快名</param>
        ///// <param name="postion">插入点</param>
        ///// <param name="scale">缩放比例</param>
        ///// <param name="rotateAngle">旋转角度</param>
        ///// <param name="attNameValues">属性名称与取值</param>
        ///// <returns></returns>
        ///// 
        //public static ObjectId InsertBlockrefence(this ObjectId spaceId, string layer, string blockName, Point3d postion, Scale3d scale, double rotateAngle, Dictionary<string, string> attNameValues)
        //{
        //    // 获取数据库对象
        //    Database db = spaceId.Database;
        //    //以读的方式打开块表
        //    BlockTable bt = db.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;
        //    //如果没有blockName d的块，则程序返回
        //    if (!bt.Has(blockName))

        //        return ObjectId.Null;//如果没有blockName的块，程序返回
        //    //以写的模式打开空间
        //    BlockTableRecord space = (BlockTableRecord)spaceId.GetObject(OpenMode.ForWrite);
        //    //获取块表的记录ID
        //    ObjectId btrId = bt[blockName];
        //    //打开块表记录
        //    BlockTableRecord record = btrId.GetObject(OpenMode.ForRead) as BlockTableRecord;
        //    //创建一个快参照并设置插入点
        //    BlockReference br = new BlockReference(postion, bt[blockName]);

        //    br.ScaleFactors = scale;

        //    br.Layer = layer;
        //    br.Rotation = rotateAngle;

        //    space.AppendEntity(br);
        //    //判断块表记录是否包含属性定义
        //    if (record.HasAttributeDefinitions)
        //    {
        //        //若包含，则遍历属性定义
        //        foreach (ObjectId id in record)
        //        {
        //            //检查是否是属性定义
        //            AttributeDefinition attDef = id.GetObject(OpenMode.ForRead) as AttributeDefinition;

        //            if (attDef != null)
        //            {

        //                //创建一个新的属性对象
        //                AttributeReference attribute = new AttributeReference();
        //                //从属性定义获取属性对象的对象特性
        //                attribute.SetAttributeFromBlock(attDef, br.BlockTransform);
        //                attribute.Rotation = attDef.Rotation;

        //                attribute.Position = attDef.Position.TransformBy(br.BlockTransform);

        //                attribute.AdjustAlignment(db);
        //                //判断是否包含指定的属性名称
        //                if (attNameValues.ContainsKey(attDef.Tag.ToUpper()))
        //                {
        //                    //设置属性值
        //                    attribute.TextString = attNameValues[attDef.Tag.ToUpper()].ToString();

        //                }
        //                // 向块参照添加属性对象
        //                br.AttributeCollection.AppendAttribute(attribute);
        //                db.TransactionManager.AddNewlyCreatedDBObject(attribute, true);

        //            }
        //        }
        //    }
        //    db.TransactionManager.AddNewlyCreatedDBObject(br, true);
        //    return br.ObjectId;//返回添加的快参照的ID
        //}
        #endregion

    }
}
