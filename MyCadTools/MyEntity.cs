using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    /// <summary>
    /// MyEntity用来获取实体的属性
    /// </summary>
    public class MyEntity
    {
        public string LayerName { set; get; }//实体的图层
        public short ColorIndex { set; get; }//实体的颜色值
        public ObjectId Oid { set; get; }//实体的oid
        public MyEntity() { ;}
        /// <summary>
        /// 生成entity并设置属性
        /// </summary>
        /// <param name="ent">要生成的entity</param>
        /// <param name="layerName">entity的图层</param>
        /// <param name="colorIndex">entit的颜色</param>
        public MyEntity(Entity ent, string layerName, short colorIndex)
        {
            Database db = ent.ObjectId.Database;
            this.LayerName = layerName;
            this.ColorIndex = colorIndex;
            Oid = db.InsertEntity(ent, LayerName, ColorIndex);

        }
        /// <summary>
        /// 根据ent获取ent的属性
        /// </summary>
        /// <param name="ent"></param>
        public MyEntity(Entity ent)
        {
            this.LayerName = ent.Layer;
            this.ColorIndex = (short)ent.ColorIndex;
            Oid = ent.ObjectId;
        }
    }
    public class MyDbText : MyEntity
    {
        public DBText DbTxt { set; get; }
        public double TextHeight { set; get; }//实体的高度
        public double WidthFactor { set; get; }//实体的颜色值
        public MyDbText(Entity ent)
            : base(ent)
        {
            DBText dbtext = ent as DBText;
            this.DbTxt = dbtext;
            this.LayerName = dbtext.Layer;
            this.ColorIndex = (short)dbtext.ColorIndex;
            this.TextHeight = dbtext.Height;

        }
        public MyDbText(Database db, string textString, string layerName, double height, double widthFactor, short colorIndex)
        {
            this.DbTxt.TextString = textString;
            this.LayerName = layerName;
            this.TextHeight = height;
            this.WidthFactor = widthFactor;
            this.ColorIndex = colorIndex;
            DbTxt.LayerId = db.AddLayerTool(LayerName, ColorIndex);
        }
    }
    public class MyPolyline : MyEntity
    {
        private double area = 0;
        private double cx = 0;
        private double cy = 0;
        public Polyline PolyLine { set; get; }
        public double Length
        {
            get { return this.PolyLine.Length; }
        }
        public double Area
        {
            get
            {
                Point3d[] pts = new Point3d[this.PolyLine.NumberOfVertices];
                for (int i = 0; i < this.PolyLine.NumberOfVertices; i++)
                {
                    Point3d pt = this.PolyLine.GetPoint3dAt(i);
                    pts[i] = pt;
                }
                for (int i = 0; i < pts.Length - 1; i++)
                {
                    area += pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                }
                area += pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y;
                area = area / 2;
                return Math.Abs(area);
            }
        }
        public Point3d Centroids
        {
            get
            {
                Point3d[] pts = new Point3d[this.PolyLine.NumberOfVertices];
                for (int i = 0; i < this.PolyLine.NumberOfVertices; i++)
                {
                    Point3d pt = this.PolyLine.GetPoint3dAt(i);
                    pts[i] = pt;
                }
                for (int i = 0; i < pts.Length - 1; i++)
                {
                    area += pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                }
                area += pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y;
                area = area / 2;
                for (int i = 0; i < pts.Length - 1; i++)
                {
                    cx += (pts[i].X + pts[i + 1].X) * (pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y);
                    cy += (pts[i].Y + pts[i + 1].Y) * (pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y);
                }
                cx += (pts[pts.Length - 1].X + pts[0].X) * (pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y);
                cy += (pts[pts.Length - 1].Y + pts[0].Y) * (pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y);
                cx = cx / (6 * area);
                cy = cy / (6 * area);
                return new Point3d(cx, cy, 0);
            }
        }
        public MyPolyline(Polyline polyLine)
        {
            this.PolyLine = polyLine;
        }
        public ObjectId MakeCentroids()
        {
            using (Transaction trans =this.PolyLine.Database .TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(this.PolyLine.Database.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBPoint dbpoint = new DBPoint(this.Centroids);
                dbpoint.ColorIndex = 1;
                this.PolyLine.Database.Pdmode = 3;
                this.PolyLine.Database.Pdsize = 100;
                trans.Commit();
                return this.PolyLine.Database.AddToModelSpace(dbpoint);
            }
        }

    }
}
