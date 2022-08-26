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
    /// <summary>
    /// 用来生成实体
    /// </summary>
    public static class MyEntityTools
    {
        /// <summary>
        /// 插入DbText文字
        /// </summary>
        /// <param name="db"></param>
        /// <param name="textString">插入文字的内容</param>
        /// <param name="layerName">插入文字的图层名字</param>
        /// <param name="textPositon">插入文字的坐标</param>
        /// <param name="textHeight">插入文字的高度</param>
        /// <param name="textWidthFactor">插入文字的宽度因子</param>
        /// <param name="colorIndex">插入文字的颜色</param>
        /// <returns></returns>
        public static ObjectId InsertDbtext(this Database db, string textString, string layerName, Point3d textPositon, double textHeight, double textWidthFactor, short colorIndex)
        {
            ObjectId oid = ObjectId.Null;
            DBText dbtext = new DBText();
            dbtext.TextString = textString;
            dbtext.LayerId = db.AddLayerTool(layerName);
            dbtext.TextStyleId = db.AddTextStyle("TSSD_Norm", "tssdeng", "hztxt");
            dbtext.Position = textPositon;
            dbtext.Height = textHeight;
            dbtext.WidthFactor = textWidthFactor;
            dbtext.ColorIndex = colorIndex;
            oid = db.AddToModelSpace(dbtext);
            return oid;
        }
        /// <summary>
        /// 插入实体entity
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="ent">实体</param>
        /// <param name="layerName">实体的图层名</param>
        /// <param name="colorIndex">实体的颜色</param>
        /// <returns></returns>
        public static ObjectId InsertEntity(this Database db, Entity ent, string layerName, short colorIndex)
        {
            ObjectId oid = ObjectId.Null;
            ent.LayerId = db.AddLayerTool(layerName);
            ent.ColorIndex = colorIndex;
            oid = db.AddToModelSpace(ent);
            return oid;
        }
        /// <summary>
        /// 生成dbtext
        /// </summary>
        /// <param name="db"></param>
        /// <param name="textString">文字内容</param>
        /// <param name="layerName">图层名</param>
        /// <param name="height">字高</param>
        /// <param name="widthFactor">宽度因子</param>
        /// <param name="colorIndex">颜色值</param>
        /// <returns></returns>
        public static DBText MakeDbtext(this Database db, string textString, string layerName, double height, double widthFactor, short colorIndex)
        {
            DBText dbtext = new DBText();
            dbtext.TextString = textString;
            dbtext.LayerId = db.AddLayerTool(layerName, colorIndex);
            dbtext.Height = height;
            dbtext.WidthFactor = widthFactor;
            dbtext.ColorIndex = colorIndex;
            return dbtext;
        }
        /// <summary>
        /// 改变文字的内容
        /// </summary>
        /// <param name="db"></param>
        /// <param name="textString"></param>
        /// <param name="layerName"></param>
        /// <param name="height"></param>
        /// <param name="widthFactor"></param>
        /// <param name="colorIndex"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static DBText MakeDbtext(this Database db, string textString, string layerName, double height, double widthFactor, short colorIndex, Point3d position)
        {
            DBText dbtext = new DBText();
            dbtext.TextString = textString;
            dbtext.LayerId = db.AddLayerTool(layerName, colorIndex);
            dbtext.Height = height;
            dbtext.WidthFactor = widthFactor;
            dbtext.ColorIndex = colorIndex;
            dbtext.Position = position;
            return dbtext;
        }
        /// <summary>
        /// 改变文字的内容
        /// </summary>
        /// <param name="olddbtext"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ObjectId ChangeText(this DBText olddbtext, string text)
        {
            ObjectId oid = ObjectId.Null;
            Database db = olddbtext.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                olddbtext.UpgradeOpen();
                olddbtext.TextString = text;
                oid = olddbtext.ObjectId;
                olddbtext.DowngradeOpen();
                trans.Commit();
            }
            return oid;
        }
        public static ObjectId ChangeColor(this Entity ent, int color)
        {
            ObjectId oid = ObjectId.Null;
            Database db = ent.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                ent.UpgradeOpen();
                ent.ColorIndex = color;
                oid = ent.ObjectId;
                ent.DowngradeOpen();
                trans.Commit();
            }
            return oid;
        }
        /// <summary>
        /// 改变文字的内容
        /// </summary>
        /// <param name="db"></param>
        /// <param name="olddbtext"></param>
        /// <param name="text"></param>
        public static void ChangeText(this Database db, DBText olddbtext, string text)
        {
            if (olddbtext.IsNewObject)
            {
                olddbtext.TextString = text;
            }
            else
            {
                db.ChangeText(olddbtext.ObjectId, text);
            }


        }
        /// <summary>
        /// 根据objectid改变文字的内容
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbtextId"></param>
        /// <param name="text"></param>
        public static void ChangeText(this Database db, ObjectId dbtextId, string text)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBText t = dbtextId.GetObject(OpenMode.ForWrite) as DBText;
                t.TextString = text;
                trans.Commit();
            }
        }
        /// <summary>
        /// 获取新建的dbtext的文字内容
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbtext"></param>
        /// <returns></returns>
        public static string GetText(this Database db, DBText dbtext)
        {
            string str;
            if (dbtext.IsNewObject)
            {
                str = dbtext.TextString;
            }
            else
            {
                str = db.GetText(dbtext.ObjectId);
            }
            return str;
        }
        /// <summary>
        /// 获取已经存在的文字内容
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbtextId"></param>
        /// <returns></returns>
        public static string GetText(this Database db, ObjectId dbtextId)
        {
            string str;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBText t = dbtextId.GetObject(OpenMode.ForWrite) as DBText;
                str = t.TextString;
                trans.Commit();
            }
            return str;
        }
        /// <summary>
        /// 根据dbtext删除dbtext
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbtext"></param>
        public static void DelText(this Database db, DBText dbtext)
        {

            if (dbtext.IsNewObject)
            {
                dbtext.Erase();
            }
            else
            {
                db.DelText(dbtext.ObjectId);
            }

        }
        /// <summary>
        /// 根据objectid删除已经存在是实体
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbtextId"></param>
        public static void DelText(this Database db, ObjectId dbtextId)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                dbtextId.GetObject(OpenMode.ForWrite).Erase();
                trans.Commit();
            }
        }

        public static Point3d GetCentroid(this Database db, Polyline pl)
        {
            double area = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Point3d[] pts = new Point3d[pl.NumberOfVertices];
                for (int i = 0; i < pl.NumberOfVertices; i++)
                {
                    Point3d pt = pl.GetPoint3dAt(i);
                    pts[i] = pt;
                }

                for (int i = 0; i < pts.Length - 1; i++)
                {
                    area += pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                }
                area += pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y;
                area = area / 2;


                double cx = 0;
                double cy = 0;
                for (int i = 0; i < pts.Length - 1; i++)
                {
                    cx += (pts[i].X + pts[i + 1].X) * (pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y);
                    cy += (pts[i].Y + pts[i + 1].Y) * (pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y);
                }
                cx += (pts[pts.Length - 1].X + pts[0].X) * (pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y);

                cy += (pts[pts.Length - 1].Y + pts[0].Y) * (pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y);

                cx = cx / (6 * area);

                cy = cy / (6 * area);

                Point3d center = new Point3d(cx, cy, 0);

                return center;
            }
        }

        public static double GetArea(this Entity ent)
        {

            Database db = ent.Database;
            double area = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (ent is Polyline)
                {
                    Polyline pl = (Polyline)ent.ObjectId.GetObject(OpenMode.ForRead);

                    MyPolyline mpl = new MyPolyline(pl);
                    area = Math.Abs(mpl.Area);
                }

                trans.Commit();
            }
            return area;
        }
        public static double GetLength(this Entity ent)
        {

            Database db = ent.Database;
            double length = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (ent is Polyline)
                {
                    Polyline pl = (Polyline)ent.ObjectId.GetObject(OpenMode.ForRead);

                    MyPolyline mpl = new MyPolyline(pl);
                    length = Math.Abs(mpl.Length);
                }

                trans.Commit();
            }
            return length;
        }
    }
}
