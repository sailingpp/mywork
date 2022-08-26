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
    public class Xref
    {
        /// <summary>
        /// 引用外部文件为块参照插入
        /// </summary>
        [CommandMethod("adb")]
        public void Add1()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                ObjectId refid = db.OverlayXref(@"d:\test.dwg", "name");// 通过外部文件获取图块定义的ObjectId
                BlockReference br = new BlockReference(Point3d.Origin, refid); // 通过块定义添加块参照
                btr.AppendEntity(br); //把块参照添加到块表记录
                trans.AddNewlyCreatedDBObject(br, true); // 通过事务添加块参照到数据库
                trans.Commit();
            }
        }
        /// <summary>
        /// 把块定义变为块参照插入
        /// </summary>
        [CommandMethod("AddBlock")]
        public void Add2()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId blkid;

            //创建块定义
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
                BlockTableRecord blk = new BlockTableRecord();
                blk.Name = "BLK";
                Line L = new Line(Point3d.Origin, new Point3d(1000, 1000, 0));
                Circle C = new Circle(Point3d.Origin, Vector3d.ZAxis, 500);
                blk.AppendEntity(L);
                blk.AppendEntity(C);
                blkid = bt.Add(blk);
                trans.AddNewlyCreatedDBObject(blk, true);
                trans.Commit();
            }

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                BlockReference br = new BlockReference(new Point3d(0, 0, 0), blkid); // 通过块定义创建块参照
                btr.AppendEntity(br); //把块参照添加到块表记录
                tr.AddNewlyCreatedDBObject(br, true); // 通过事务添加块参照到数据库
                tr.Commit();
            }
        }
    }
}
