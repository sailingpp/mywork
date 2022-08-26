using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//画墙身表
//计算墙身表

namespace MyCadTools
{
    public static class DrawShearWallTableTools
    {
        /// <summary>
        /// 画墙表
        /// </summary>
        [CommandMethod("drawswt")]
        public static void DrawShearWallTable()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptPointResult re1 = ed.GetPoint("请指定点");
            Point3d pt3d = new Point3d();
            pt3d = re1.Value;


            //外框
            Polyline pl = new Polyline();
            pl.AddVertexAt(0, new Point2d(pt3d.X, pt3d.Y), 0, 0, 0);
            pl.AddVertexAt(1, new Point2d(pt3d.X + 3000, pt3d.Y), 0, 0, 0);
            pl.AddVertexAt(2, new Point2d(pt3d.X + 3000, pt3d.Y - 1800), 0, 0, 0);
            pl.AddVertexAt(3, new Point2d(pt3d.X, pt3d.Y - 1800), 0, 0, 0);
            pl.Closed = true;
            db.AddToModelSpace(pl);

            DBText dbtext = new DBText();
            dbtext.Height = 400;
            dbtext.WidthFactor = 0.7;
            dbtext.TextString = "墙厚";
            dbtext.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");

            // dbtext.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            dbtext.Position = new Point3d(pt3d.X + 200, pt3d.Y - 500, 0);
            db.AddToModelSpace(dbtext);

            DBText dbtext1 = new DBText();
            dbtext1.Height = 400;
            dbtext1.WidthFactor = 0.7;
            dbtext1.TextString = "配筋";
            dbtext1.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            dbtext1.Position = new Point3d(pt3d.X + 1600, pt3d.Y - 500, 0);
            db.AddToModelSpace(dbtext1);

            DBText dbtext2 = new DBText();
            dbtext2.Height = 300;
            dbtext2.WidthFactor = 0.7;
            dbtext2.TextString = "200";
            dbtext2.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            dbtext2.Position = new Point3d(pt3d.X + 200, pt3d.Y - 1100, 0);
            db.AddToModelSpace(dbtext2);


            DBText dbtext21 = new DBText();
            dbtext21.Height = 300;
            dbtext21.WidthFactor = 0.7;
            dbtext21.TextString = "250";
            dbtext21.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            dbtext21.Position = new Point3d(pt3d.X + 200, pt3d.Y - 1700, 0);
            db.AddToModelSpace(dbtext21);


            DBText dbtext3 = new DBText();
            dbtext3.Height = 300;
            dbtext3.WidthFactor = 0.7;
            dbtext3.TextString = "%%1328@200";
            dbtext3.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            dbtext3.Position = new Point3d(pt3d.X + 1600, pt3d.Y - 1100, 0);
            db.AddToModelSpace(dbtext3);

            DBText dbtext31 = new DBText();
            dbtext31.Height = 300;
            dbtext31.WidthFactor = 0.7;
            dbtext31.TextString = "%%13210@200";
            dbtext31.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            dbtext31.Position = new Point3d(pt3d.X + 1600, pt3d.Y - 1700, 0);
            db.AddToModelSpace(dbtext31);

            //第一条横线
            Polyline pl1 = new Polyline();
            pl1.AddVertexAt(0, new Point2d(pt3d.X, pt3d.Y - 1200), 0, 0, 0);
            pl1.AddVertexAt(1, new Point2d(pt3d.X + 3000, pt3d.Y - 1200), 0, 0, 0);
            db.AddToModelSpace(pl1);

            //第二条横线
            Polyline pl2 = new Polyline();
            pl2.AddVertexAt(0, new Point2d(pt3d.X, pt3d.Y - 600), 0, 0, 0);
            pl2.AddVertexAt(1, new Point2d(pt3d.X + 3000, pt3d.Y - 600), 0, 0, 0);
            db.AddToModelSpace(pl2);

            //第一条竖线
            Polyline pl3 = new Polyline();
            pl3.AddVertexAt(0, new Point2d(pt3d.X + 1000, pt3d.Y), 0, 0, 0);
            pl3.AddVertexAt(1, new Point2d(pt3d.X + 1000, pt3d.Y - 1800), 0, 0, 0);
            db.AddToModelSpace(pl3);
        }
        /// <summary>
        /// 计算墙表
        /// </summary>
        [CommandMethod("js")]
        public static void CalShearWall()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            Point3dCollection pt = new Point3dCollection();
            PromptSelectionResult psr = ed.GetSelection();
            List<DBText> listDbtext = new List<DBText>();
            List<DBText> listLeft = new List<DBText>();
            List<DBText> listReft = new List<DBText>();
            Dictionary<DBText, DBText> myDic = new Dictionary<DBText, DBText>();
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        Polyline pl = tran.GetObject(item.ObjectId, OpenMode.ForRead) as Polyline;
                        for (int i = 0; i < pl.NumberOfVertices; i++)
                        {
                            pt.Add(pl.GetPoint3dAt(i));
                        }
                    }
                }
                PromptSelectionResult psr1 = ed.SelectCrossingPolygon(pt, acSelFtr);
                if (psr1.Status == PromptStatus.OK)
                {
                    SelectionSet sset1 = psr1.Value;

                    foreach (SelectedObject item in sset1)
                    {
                        DBText dbtext = tran.GetObject(item.ObjectId, OpenMode.ForWrite) as DBText;
                        dbtext.ColorIndex = 3;
                        if (dbtext.Height == 300)
                        {
                            listDbtext.Add(dbtext);
                        }

                    }
                }
                //foreach (DBText item in listDbtext)
                //{
                //   Application.ShowAlertDialog(item.TextString.ToString());
                //}

                for (int j = 0; j < listDbtext.Count; j++)
                {
                    double y = listDbtext[j].Position.Y;
                    for (int i = j + 1; i < listDbtext.Count; i++)
                    {
                        if (listDbtext[i].Position.Y == y)
                        {
                            myDic.Add(listDbtext[j], listDbtext[i]);
                        }

                    }
                }
                foreach (KeyValuePair<DBText, DBText> kv in myDic)
                {

                    double result = (2 * GetAs(kv.Key) / Getjm(kv.Value) / 1000) * 100;
                    DBText text = new DBText();
                    text.Height = 100;
                    text.TextString = result.ToString();
                    text.Position = new Point3d(kv.Key.Position.X + 900, kv.Key.Position.Y, kv.Key.Position.Z);
                    db.AddToModelSpace(text);

                }


                tran.Commit();
            }



        }
        /// <summary>
        /// 将%%1328@200转成面积
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double GetAs(this DBText text)
        {
            double area = 0;
            string str = text.TextString;
            string d = MidStrEx_New(str, "%%132", "@");
            double dd = Convert.ToDouble(d);
            string gap = str.Substring(str.IndexOf("@") + 1);
            double gapd = Convert.ToDouble(gap);
            area = 3.14 * dd * dd / 4 * 1000 / gapd;
            return area;
        }
        public static double Getjm(this DBText text)
        {
            double jm = 0;
            string str = text.TextString;
            jm = Convert.ToDouble(str);
            return jm;
        }
        /// <summary>
        /// 匹配字符串中间的字符
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="startstr"></param>
        /// <param name="endstr"></param>
        /// <returns></returns>
        public static string MidStrEx_New(string sourse, string startstr, string endstr)
        {
            Regex rg = new Regex("(?<=(" + startstr + "))[.\\s\\S]*?(?=(" + endstr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(sourse).Value;
        }
    }
}
