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
using System.Windows.Interop;
namespace MyCadTools
{
    public static class Makeblock
    {
        /// <summary>
        /// 将DWG文件所有DBObject组成一个块, 如果DWG文件中有属性, 则属性变为块属性
        /// </summary>
        /// <param name="insertPoint">插入点</param>
        /// <param name="scale">插入比例</param>
        /// <param name="blockName">块表记录名</param>
        /// <param name="filePash">DWG文件路径</param>
        /// <returns>块参照</returns>
        public static BlockReference InsertBlockReference(Point3d insertPoint, double scale, string blockName, string filePash)
        {
            BlockReference blockReference = null;

            Document curDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument; //获取当前文档
            using (DocumentLock docLock = curDoc.LockDocument()) //锁定文档
            {
                using (Transaction trans = curDoc.TransactionManager.StartTransaction()) //事务
                {
                    try
                    {
                        BlockTable curBlockTb = trans.GetObject(curDoc.Database.BlockTableId, OpenMode.ForRead) as BlockTable; //当前文档块表
                        ObjectId blockObjId = new ObjectId(); //用于块表记录的编号
                        if (curBlockTb.Has(blockName) && !curBlockTb[blockName].IsErased) //块表中存在该块表记录, 并且没有被删除
                            blockObjId = curBlockTb[blockName];
                        else
                        {
                            if (!System.IO.File.Exists(filePash)) //不存在该文件
                            {
                                trans.Abort(); //事务终止
                                return blockReference;
                            }
                            Database sourceDB = new Database(false, true);
                            sourceDB.ReadDwgFile(filePash, System.IO.FileShare.Read, true, null); //后台读取DWG文件信息; 参数: 文件名, 打开方式, 是否允许转换版本, 密码
                            blockObjId = curDoc.Database.Insert(blockName, sourceDB, false); //将一个数据库插入到当前数据库的一个块中; 参数: 新创建的块表记录名, 资源数据库, 资源数据库是否保存原样
                            sourceDB.CloseInput(true); //是否关闭ReadDwgFile()方法之后打开的文件
                            sourceDB.Dispose();
                        }
                        if (blockObjId != null)
                        {
                            string layoutName = LayoutManager.Current.CurrentLayout; //获得当前布局空间
                            BlockTableRecord block = trans.GetObject(blockObjId, OpenMode.ForWrite) as BlockTableRecord; //根据块表记录编号获取的块表记录, 用于取得它的属性定义
                            block.Explodable = true; //块参照是否能被炸开
                            blockReference = new BlockReference(insertPoint, blockObjId); //新建块参照
                            BlockTableRecord layout = null;
                            if (layoutName.Equals("Model"))
                                layout = trans.GetObject(curBlockTb[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord; //根据当前布局空间获取的表块记录, 用于将块插入布局空间中
                            else
                                layout = trans.GetObject(curBlockTb[BlockTableRecord.PaperSpace], OpenMode.ForWrite) as BlockTableRecord;
                            layout.AppendEntity(blockReference); //在当前空间中追加此块参照
                            trans.AddNewlyCreatedDBObject(blockReference, true);
                            if (!block.HasAttributeDefinitions) //如果该块表记录中不包含任何属性定义
                                goto No_AttributeDefinitions; //直接去往No_AttributeDefinitions

                            AttributeDefinition attriDef = null;
                            AttributeReference attriRefe = null;
                            Matrix3d mtr = Matrix3d.Displacement(block.Origin.GetVectorTo(insertPoint)); //Displacement 取代
                            // Matrix3d mtr = Matrix3d.Displacement(block.Origin); //Displacement 取代
                            foreach (ObjectId entityObjId in block) //遍历块表记录中的实体编号
                            {
                                attriDef = trans.GetObject(entityObjId, OpenMode.ForRead) as AttributeDefinition; //打开实体通过实体编号
                                if (attriDef == null) //若还是为null
                                    continue;
                                attriRefe = new AttributeReference(); //每次循环new一次新的对象, 确保上次属性不会残留
                                attriRefe.SetPropertiesFrom(attriDef); //SetPropertiesFrom 设置属性来自
                                attriRefe.SetAttributeFromBlock(attriDef, mtr); //通过块设置属性 参数: 属性定义, 变形矩阵
                                attriRefe.TextString = ""; //设置属性值为""
                                blockReference.AttributeCollection.AppendAttribute(attriRefe); //块参照中添加此属性参照
                                trans.AddNewlyCreatedDBObject(attriRefe, true);
                                attriRefe = null;
                            }
                        }
                    No_AttributeDefinitions:
                        blockReference.TransformBy(Matrix3d.Scaling(scale, insertPoint)); //缩放块; 修改块比例
                        trans.Commit(); //提交事务
                    }
                    catch
                    {
                        if (blockReference != null)
                        {
                            blockReference = null;
                        }
                        return blockReference;
                    }
                }
            }

            return blockReference;
        }
    }
}
