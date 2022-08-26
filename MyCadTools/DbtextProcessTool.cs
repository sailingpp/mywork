using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
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
    /// DBTEXT处理工具
    /// </summary>
    public static class DbtextProcessTool
    {
        /// <summary>
        /// 将dbtext转成string
        /// </summary>
        /// <param name="dbtext"></param>
        /// <returns></returns>
        public static string DBTexttoString(DBText dbtext)
        {
            string str = dbtext.TextString;
            return str;
        }
        /// <summary>
        /// 将dbtext转成double
        /// </summary>
        /// <param name="dbtext"></param>
        /// <returns></returns>
        public static double DBTexttoDouble(DBText dbtext)
        {
            string str = dbtext.TextString;
            return Convert.ToDouble(str);
        }
        /// <summary>
        /// 将数字转成dbtext
        /// </summary>
        /// <param name="num"></param>
        /// <param name="dbtext"></param>
        /// <param name="vect"></param>
        /// <returns></returns>
        public static ObjectId DoubletoDbtext(this Database db, double num, DBText dbtext, Vector3d vect)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DBText text = new DBText();
                text.TextString = num.ToString();
                text.Height = 100;
                text.Position = dbtext.Position + vect;
                oid = db.AddToModelSpace(text);
                trans.Commit();
            }
            return oid;

        }
        public static ObjectId DoubletoDbtext(this Database db, double num, Point3d pt0, Vector3d vect)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DBText text = new DBText();
                text.TextString = num.ToString();
                text.Height = 100;
                text.Position = pt0 + vect;
                text.TextStyleId = db.AddTextStyle("TSSD_Norm", "tssdeng", "hztxt");
                oid = db.AddToModelSpace(text);
                trans.Commit();
            }
            return oid;

        }
        /// <summary>
        /// 将string转成dbtext
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dbtext"></param>
        /// <param name="vect"></param>
        /// <returns></returns>
        public static ObjectId DoubletoDbtext(this Database db, string str, DBText dbtext, Vector3d vect)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DBText text = new DBText();
                text.TextString = str;
                text.Height = 100;
                text.Position = dbtext.Position + vect;
                text.TextStyleId = db.AddTextStyle("TSSD_Norm", "tssdeng", "hztxt");

                oid = db.AddToModelSpace(text);
                trans.Commit();
            }
            return oid;
        }
        public static ObjectId DoubletoDbtext(this Database db, string str, double height, Point3d pt0, Vector3d vect)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DBText text = new DBText();
                text.TextString = str;
                text.Height = 100;
                text.Position = pt0 + vect;
                text.TextStyleId = db.AddTextStyle1("TSSD_Norm", "tssdeng", "hztxt");
                text.WidthFactor = 0.7;
                text.Height = height;
                oid = db.AddToModelSpace(text);
                trans.Commit();
            }
            return oid;
        }
        public static ObjectId DoubletoDbtext(this Database db, string str, double height, Point3d pt0, Vector3d vect, string layerName)
        {
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                DBText text = new DBText();
                text.TextString = str;
                text.Height = 100;
                text.Position = pt0 + vect;
                text.TextStyleId = db.AddTextStyle1("TSSD_Norm", "tssdeng", "hztxt");
                text.LayerId = db.AddLayerTool(layerName);
                text.WidthFactor = 0.7;
                text.Height = height;
                oid = db.AddToModelSpace(text);
                trans.Commit();
            }
            return oid;
        }
        /// <summary>
        /// 删除指定图层pl线段内的文字
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="layname">图层名</param>
        /// <param name="pl">线框内</param>
        public static void DelectDbtext(this Database db, string layname, Polyline pl)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.Start, "TEXT"),
                new TypedValue((int)DxfCode.Operator, "and>"), 
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.LayerName, layname),
                new TypedValue((int)DxfCode.Operator, "and>"), 
            };
            SelectionFilter filter = new SelectionFilter(tv);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult psr = ed.SelectCrossingPolygon(PlToPointTool.PltoPint3dCollectionMethod(pl), filter);
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        DBText dbtext = item.ObjectId.GetObject(OpenMode.ForWrite) as DBText;
                        dbtext.Erase(true);
                    }

                }

                trans.Commit();
            }
        }
        public static void DelectDbtext(this Database db, int color, Polyline pl)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start, "TEXT"),
            };
            SelectionFilter filter = new SelectionFilter(tv);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult psr = ed.SelectCrossingPolygon(PlToPointTool.PltoPint3dCollectionMethod(pl), filter);
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        DBText dbtext = item.ObjectId.GetObject(OpenMode.ForWrite) as DBText;
                        if (dbtext.ColorIndex == color || dbtext.ColorIndex == 1)
                        {
                            dbtext.Erase(true);
                        }

                    }

                }

                trans.Commit();
            }
        }

    }
}
