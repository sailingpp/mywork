using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public static class CadHelper
    {
        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <param name="ent">实体</param>
        /// <returns></returns>
        public static ObjectId AddToModelSpace(this Database db, Entity ent)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //打开块表
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                //打开块表记录
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                //将实体加入块表记录
                oid = btr.AppendEntity(ent);
                //更新实物
                trans.AddNewlyCreatedDBObject(ent, true);
                ed.WriteMessage("实体增加成功\r\n");
                //确认修改
                trans.Commit();
            }
            return oid;
        }
        /// <summary>
        /// 将多个实体添加到数据库
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <param name="ents">多个实体</param>
        /// <returns></returns>
        public static ObjectId[] AddToModelSpace(this Database db, params Entity[] ents)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId[] oids = new ObjectId[ents.Length];
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //打开块表
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                //打开块表记录
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                //将实体加入块表记录
                for (int i = 0; i < ents.Length; i++)
                {
                    oids[i] = btr.AppendEntity(ents[i]);
                    //更新实物
                    trans.AddNewlyCreatedDBObject(ents[i], true);
                }
                ed.WriteMessage("实体增加成功\r\n");
                //确认修改
                trans.Commit();
            }
            return oids;
        }
        /// <summary>
        /// 增加块表
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="name">块名</param>
        /// <param name="ents">块内的实体</param>
        /// <returns></returns>
        public static ObjectId AddBlock(this Database db, string name, List<Entity> ents)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (!bt.Has(name))
                {
                    BlockTableRecord btr = new BlockTableRecord();
                    btr.Name = name;
                    ents.ForEach(ent => btr.AppendEntity(ent));
                    bt.UpgradeOpen();
                    oid = bt.Add(btr);
                    trans.AddNewlyCreatedDBObject(btr, true);
                    bt.DowngradeOpen();
                    trans.Commit();
                }
                else
                {
                    db.WriteMesage("块名已经存在!");
                }
            }
            return oid;

        }
        /// <summary>
        /// 将ENTITYS增加到块记录
        /// </summary>
        /// <param name="db"></param>
        /// <param name="name"></param>
        /// <param name="ents"></param>
        /// <returns></returns>
        public static ObjectId AddBlock(this Database db, string name,Entity[] ents)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (!bt.Has(name))
                {
                    BlockTableRecord btr = new BlockTableRecord();
                    btr.Name = name;
                    foreach (var item in ents)
                    {
                        btr.AppendEntity(item);
                    }
                    bt.UpgradeOpen();
                    oid = bt.Add(btr);
                    trans.AddNewlyCreatedDBObject(btr, true);
                    bt.DowngradeOpen();
                    trans.Commit();
                }
                else
                {
                    db.WriteMesage("块名已经存在!");
                }
            }
            return oid;

        }
        /// <summary>
        /// 插入块表
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="blockName">块名</param>
        /// <param name="position">插入块的位置</param>
        /// <param name="rotateAngle">插入块的角度</param>
        /// <returns></returns>
        public static ObjectId InsertBlock(this Database db, string blockName,string layerName,Point3d position, double rotateAngle)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (bt.Has(blockName))
                {
                    BlockReference brf = new BlockReference(position, bt[blockName]);
                    brf.Rotation = rotateAngle;
                    brf.LayerId = db.AddLayerTool(layerName);
                    oid = btr.AppendEntity(brf);
                    trans.AddNewlyCreatedDBObject(brf, true);
                }
                trans.Commit();
            }
            return oid;
        }

        /// <summary>
        /// 获取多段线长度相邻点的长度
        /// </summary>
        /// <param name="pl"></param>
        /// <returns></returns>
        public static double[] GetDist(Polyline pl)
        {
            Point2dCollection pts = new Point2dCollection();
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                pts.Add(pl.GetPoint2dAt(i));
            }
            double[] pllength = new double[pts.Count];
            for (int i = 0; i < pts.Count - 1; i++)
            {
                Vector2d v = pts[i].GetVectorTo(pts[i + 1]);
                pllength[i] = v.Length;
            }
            return pllength;
        }
        /// <summary>
        /// 获取两个点的中点
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Point3d GetMiddle(Point3d p1, Point3d p2)
        {
            double x = (p1.X + p2.X) / 2;
            double y = (p1.Y + p2.Y) / 2;
            double z = (p1.Z + p2.Z) / 2;
            Point3d pm = new Point3d(x, y, z);

            return pm;
        }
        /// <summary>
        /// 将多个ent加入到块中
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ents"></param>
        /// <returns></returns>
        public static ObjectId[] AddToMyBlockTable(this Database db, List<Entity> ents)
        {
            ObjectId[] oid = new ObjectId[ents.Count];

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
                BlockTableRecord btr = new BlockTableRecord();
                btr.Name = "hello";
                for (int i = 0; i < ents.Count; i++)
                {
                    oid[i] = btr.AppendEntity(ents[i]);
                }
                bt.Add(btr);
                trans.AddNewlyCreatedDBObject(btr, true);
                trans.Commit();
            }
            return oid;
        }
        /// <summary>
        /// 获取path路径下的dwg
        /// </summary>
        /// <param name="path"></param>
        public static List<string> GetDwgFiles(string path)
        {

            string[] files = Directory.GetFiles(path);
            List<string> dwgnames = new List<string>();
            foreach (var item in files)
            {
                if (Path.GetExtension(item) == ".dwg")
                {
                    dwgnames.Add(item);
                }
            }
            return dwgnames;
        }
        public static double outcenterx(Polyline lwp)
        {
            // Polyline lwp =new Polyline ();                           
            Point3dCollection pts = new Point3dCollection();
            int vn = lwp.NumberOfVertices;  //lwp已知的多段线
            double[] ax = new double[vn + 1];
            double[] by = new double[vn + 1];
            double cenx = 0;
            double aera = lwp.Area;
            // Application.ShowAlertDialog("面积为: " + aera+"，顶点个数："+vn);

            for (int i = 0; i < vn; i++)
            {
                Point2d pt = lwp.GetPoint2dAt(i);
                pts.Add(new Point3d(pt.X, pt.Y, 0));  //将顶点坐标放入集合中
                ax[i] = pts[i].X;
                by[i] = pts[i].Y;
            }

            for (int i = 0; i < vn; i++)//求和用于计算中心坐标
            {
                cenx = cenx + ax[i];
            }

            return cenx / vn; 
        }
        /// <summary>
        /// 获取多段线的中点y
        /// </summary>
        /// <param name="lwp"></param>
        /// <returns></returns>
        public static double outcentery(Polyline lwp)
        {
            // Polyline lwp =new Polyline ();                           
            Point3dCollection pts = new Point3dCollection();
            int vn = lwp.NumberOfVertices;  //lwp已知的多段线
            double[] ax = new double[vn + 1];
            double[] by = new double[vn + 1];
            double ceny = 0;
            double aera = lwp.Area;
            // Application.ShowAlertDialog("面积为: " + aera+"，顶点个数："+vn);

            for (int i = 0; i < vn; i++)
            {
                Point2d pt = lwp.GetPoint2dAt(i);
                pts.Add(new Point3d(pt.X, pt.Y, 0));  //将顶点坐标放入集合中
                ax[i] = pts[i].X;
                by[i] = pts[i].Y;
            }

            for (int i = 0; i < vn; i++)//求和用于计算中心坐标
            {
                ceny = ceny + by[i];
            }

            return ceny / vn;
        }
        public static ObjectId InsertBlock(this Database db, Entity ent)
        {
            ObjectId entid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;
                BlockTableRecord btr = new BlockTableRecord();
                btr.Name = "helo";
                entid = btr.AppendEntity(ent);
                bt.Add(btr);
                trans.AddNewlyCreatedDBObject(btr, true);
                trans.Commit();
            }
            return entid;
        }
        public static void AddZiZuoFuJinToModelSpace(this Database db, Line line, double length, double beamWidth)
        {
            Polyline pl = new Polyline();
            db.AddLayerTool("FLOOR_UP_REIN");
            db.AddLayerTool("FLOOR_UP_TEXT");
            db.AddLayerTool("FLOOR_UP_DIM");
            DBText textSteel = new DBText();
            textSteel.Layer = "FLOOR_UP_TEXT";
            DBText textSteelLength0 = new DBText();
            textSteelLength0.Layer = "FLOOR_UP_DIM";
            textSteelLength0.ColorIndex = 3;

            DBText textSteelLength1 = new DBText();
            textSteelLength1.Layer = "FLOOR_UP_DIM";
            textSteelLength1.ColorIndex = 3;

            pl.Layer = "FLOOR_UP_REIN";
            pl.ColorIndex = 13;
            #region
            Point3d pm;
            Vector3d v;
            if (line.StartPoint.Y >= line.EndPoint.Y || line.StartPoint.X < line.EndPoint.X)
            {
                pm = new Point3d((line.StartPoint.X + line.EndPoint.X) / 2, (line.StartPoint.Y + line.EndPoint.Y) / 2, 0);
                v = line.StartPoint.GetVectorTo(line.EndPoint);//顺梁线的向量  
            }
            else
            {
                Line line1 = new Line();
                line1.StartPoint = line.EndPoint;
                line1.EndPoint = line.StartPoint;

                pm = new Point3d((line1.StartPoint.X + line1.EndPoint.X) / 2, (line1.StartPoint.Y + line1.EndPoint.Y) / 2, 0);
                v = line1.StartPoint.GetVectorTo(line1.EndPoint);//顺梁线的向量
            }
            Vector3d v2 = length * v.GetPerpendicularVector();//垂直梁线的向量
            Point3d p0 = pm - v2;
            Point3d p1 = pm + v2 + v2 / v2.Length * beamWidth;

            Point2d p00 = new Point2d(p0.X, p0.Y);
            Point2d p01 = new Point2d(p1.X, p1.Y);
            pl.AddVertexAt(0, p00, 0, 30, 30);
            pl.AddVertexAt(1, p01, 0, 30, 30);

            textSteel.TextString = "%%1328@200";
            textSteel.Height = 250;
            textSteel.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            textSteel.Position = p1 - 50 * v / v.Length - v2;
            textSteel.WidthFactor = 0.7;
            textSteel.Rotation = v2.GetAngleTo(new Vector3d(1, 0, 0));

            textSteelLength0.TextString = length.ToString();
            textSteelLength0.Height = 250;
            textSteelLength0.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            textSteelLength0.Position = p0 + 300 * v / v.Length;
            textSteelLength0.WidthFactor = 0.7;
            textSteelLength0.Rotation = v2.GetAngleTo(new Vector3d(1, 0, 0)); ;

            textSteelLength1.TextString = length.ToString();
            textSteelLength1.Height = 250;
            textSteelLength1.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            textSteelLength1.Position = p1 + 300 * v / v.Length - v2;
            textSteelLength1.WidthFactor = 0.7;
            textSteelLength1.Rotation = v2.GetAngleTo(new Vector3d(1, 0, 0));
            db.AddToModelSpace(textSteel, textSteelLength0, textSteelLength1);

            db.AddToModelSpace(pl);
            #endregion
        }
        public static Point3dCollection PlineToPoint3dCollection(this Polyline pl)
        {
            Point3dCollection ptc = new Point3dCollection();
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                ptc.Add(pl.GetPoint3dAt(i));
            }
            return ptc;
        }
    }
}

