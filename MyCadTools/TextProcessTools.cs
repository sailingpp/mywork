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
    public static class TextProcessTools
    {
        /// <summary>
        /// 将类似4%%13220
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double GetBeamSteelArea(string str)
        {
            string[] sArray = str.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
            return Convert.ToDouble(sArray[0]) * Convert.ToDouble(sArray[1]) * Convert.ToDouble(sArray[1]) * 3.14 / 4;
        }
        /// <summary>
        /// 4%%13225+8%%13220
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] sGetAllSteelArea(string str)
        {
            string[] sArray = str.Split(new string[] { "%%132", "+" }, StringSplitOptions.RemoveEmptyEntries);
            return sArray;
        }
        public static double[] dGetAllSteelArea(string str)
        {
            string[] sArray = str.Split(new string[] { "%%132", "+" }, StringSplitOptions.RemoveEmptyEntries);
            double[] dArray = new double[sArray.Length];
            for (int i = 0; i < sArray.Length; i++)
            {
                dArray[i] = Convert.ToDouble(sArray[i]);
            }
            return dArray;
        }
        public static double GetSumSteelArea(string str)
        {
            double temp;
            string[] sArray = str.Split(new string[] { "%%132", "+" }, StringSplitOptions.RemoveEmptyEntries);
            double[] dArray = new double[sArray.Length];
            for (int i = 0; i < sArray.Length; i++)
            {
                dArray[i] = Convert.ToDouble(sArray[i]);
            }
            temp = dArray[0] * dArray[1] * dArray[1] / 4 * 3.14 + dArray[2] * dArray[3] * dArray[3] / 4 * 3.14;
            return temp;
        }
        public static void AddText(this Database db, string str, DBText text, Vector3d v, int corlorindex)
        {
            DBText dbtext = new DBText();
            dbtext.TextString = str;
            dbtext.Position = text.Position + v;
            dbtext.Height = 100;
            dbtext.ColorIndex = corlorindex;
            db.AddToModelSpace(dbtext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="str">要添加的文字内容</param>
        /// <param name="text">要添加文字的参考位置点</param>
        /// <param name="v">位置调整向量</param>
        /// <param name="corlorindex">添加文字的颜色</param>
        /// <param name="limit">添加文字的限制</param>
        public static void AddText(this Database db, string str, DBText text, Vector3d v, int corlorindex, double limit)
        {
            DBText dbtext = new DBText();
            dbtext.TextString = str;
            dbtext.Position = text.Position + v;
            dbtext.Height = 150;
            if (Convert.ToDouble(str.Substring(0, str.Length - 1)) >= limit)
            {
                dbtext.ColorIndex = corlorindex;
            }
            else
            {
                dbtext.ColorIndex = 1;
            }

            db.AddToModelSpace(dbtext);
        }
        /// <summary>
        /// 获取配筋率
        /// </summary>
        /// <param name="str">400x400</param>
        /// <param name="text">4%%13220+8%%13218</param>
        /// <returns></returns>
        public static ObjectId GetPeiJingLv(Database db, double num, DBText text)
        {
            ObjectId oid = ObjectId.Null;
            double jm = num;//TextProcessTools.GetB(str) * TextProcessTools.GetH(str) / num;
            DBText dbtextpeijinglv = new DBText();
            dbtextpeijinglv.TextString = jm.ToString();
            dbtextpeijinglv.Height = 100;
            dbtextpeijinglv.Position = text.Position;
            db.AddToModelSpace(dbtextpeijinglv);
            return oid;
        }
        public static double GetSteelNumber(string str)
        {
            string[] sArray = str.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
            return Convert.ToDouble(sArray[0]);
        }
        public static double GetSteelArea(string str)
        {
            string[] sArray = str.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
            return Convert.ToDouble(sArray[1]);
        }
        /// <summary>
        /// %%1328@100/200
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double GetPlanSteelArea(string str)
        {
            string[] sArray = str.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
            return Convert.ToDouble(sArray[0]) * Convert.ToDouble(sArray[1]) * Convert.ToDouble(sArray[1]) * 3.14 / 4;
        }
        /// <summary>
        /// 500x400
        /// </summary>
        /// <param name="str"></param>
        /// <returns>500</returns>
        public static double GetB(string str)
        {
            string b = str.Substring(0, str.IndexOf('x'));
            return Convert.ToDouble(b);
        }
        /// <summary>
        /// 500x400
        /// </summary>
        /// <param name="str"></param>
        /// <returns>400</returns>
        public static double GetH(string str)
        {
            string h = str.Substring(str.IndexOf('x') + 1);
            return Convert.ToDouble(h);
        }
        public static void ChangeDbtext(this Database db, ObjectId oid, string str)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBText dbtext = (DBText)oid.GetObject(OpenMode.ForWrite);
                dbtext.TextString = str;
                trans.Commit();
            }
        }
        public static void ChangeDbtext(this Database db, ObjectId oid, double num)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBText dbtext = (DBText)oid.GetObject(OpenMode.ForWrite);

                dbtext.TextString = (Convert.ToDouble(dbtext.TextString) * num).ToString();
                trans.Commit();
            }
        }
        public static void ChangeHeightDbtext(this Database db, ObjectId oid, double num)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBText dbtext = (DBText)oid.GetObject(OpenMode.ForWrite);

                dbtext.TextString = num.ToString();
                trans.Commit();
            }
        }
        public static void ChangeHeightDbtext(this Database db, ObjectId oid, string str)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBText dbtext = (DBText)oid.GetObject(OpenMode.ForWrite);

                dbtext.TextString = str.ToString();
                trans.Commit();
            }
        }
        /// <summary>
        /// 改变文字内容
        /// </summary>
        /// <param name="db"></param>
        /// <param name="oidList"></param>
        /// <param name="str"></param>
        public static void ChangeAllText(this Database db, List<ObjectId> oidList, string str)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in oidList)
                {
                    DBText dbtext = (DBText)item.GetObject(OpenMode.ForWrite);
                    dbtext.TextString = str;
                }
                trans.Commit();
            }
        }
        /// <summary>
        /// 将所有选择的文字乘以某个数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="oidList"></param>
        /// <param name="num"></param>
        public static void ChangeAllText(this Database db, List<ObjectId> oidList, double num)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in oidList)
                {
                    DBText dbtext = (DBText)item.GetObject(OpenMode.ForWrite);
                    dbtext.TextString = (Convert.ToDouble(dbtext.TextString) * num).ToString();
                }
                trans.Commit();
            }
        }
        /// <summary>
        /// 将所有选择的文字加上某个数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="oidList"></param>
        /// <param name="num"></param>
        public static void ChangeAllText_AddNum(this Database db, List<ObjectId> oidList, double num)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId item in oidList)
                {
                    DBText dbtext = (DBText)item.GetObject(OpenMode.ForWrite);
                    dbtext.TextString = (Convert.ToDouble(dbtext.TextString) + num).ToString();
                }
                trans.Commit();
            }
        }
        /// <summary>
        /// 选择文字
        /// </summary>
        /// <param name="db">db</param>
        /// <returns>文字的Objetid</returns>
        public static List<ObjectId> SeletTexts(this Database db)
        {
            List<ObjectId> oidList = new List<ObjectId>();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            TypedValue[] typevalue = new TypedValue[]
            {
                  new TypedValue((int)DxfCode.Start, "TEXT"),
            };
            SelectionFilter selectfilter = new SelectionFilter(typevalue);
            PromptSelectionResult psr = ed.GetSelection(selectfilter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    if (sset != null)
                    {
                        foreach (SelectedObject item in sset)
                        {
                            oidList.Add(item.ObjectId);
                        }
                    }
                }
                trans.Commit();
            }
            return oidList;
        }
    }
    public static class TextTestTools
    {
        [CommandMethod("td")]
        public static void texttestdemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult pr = ed.GetString("请输入你要改成的文字 ");
            DBText dbtext = new DBText();
            dbtext.TextString = "hello";
            dbtext.Height = 200;
            dbtext.Position = new Point3d(0, 0, 0);
            ObjectId oid = db.AddToModelSpace(dbtext);
            TextProcessTools.ChangeDbtext(db, oid, pr.StringResult);
        }
        [CommandMethod("td2")]
        public static void texttestdemo2()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult pr = ed.GetString("请输入你要改成的文字 ");
            DBText dbtext = new DBText();
            dbtext.TextString = "hello";
            dbtext.Height = 200;
            dbtext.Position = new Point3d(0, 0, 0);
            DBText dbtext1 = new DBText();
            dbtext1.TextString = "hello";
            dbtext1.Height = 200;
            dbtext1.Position = new Point3d(400, 0, 0);
            List<ObjectId> oidList = new List<ObjectId>();
            ObjectId[] oids = db.AddToModelSpace(dbtext, dbtext1);
            for (int i = 0; i < oids.Length; i++)
            {
                oidList.Add(oids[i]);
            }
            TextProcessTools.ChangeAllText(db, oidList, pr.StringResult);
        }
        [CommandMethod("td3")]
        public static void texttestdemo3()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult pr = ed.GetString("请输入你要改成的文字 ");
            List<ObjectId> oidList = TextProcessTools.SeletTexts(db);
            TextProcessTools.ChangeAllText(db, oidList, pr.StringResult);
        }
        [CommandMethod("mwzs")]
        public static void texttestdemo4()
        {

            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions pso = new PromptEntityOptions("选择目标文字");
            PromptEntityResult per = ed.GetEntity(pso);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK)
                {
                    DBText dbtext = trans.GetObject(per.ObjectId, OpenMode.ForWrite) as DBText;
                    dbtext.ColorIndex = 3;
                    List<ObjectId> oidList = TextProcessTools.SeletTexts(db);
                    TextProcessTools.ChangeAllText(db, oidList, dbtext.TextString);
                }
                trans.Commit();

            }

        }
        [CommandMethod("td5")]
        public static void texttestdemo5()
        {
            try
            {
                Database db = HostApplicationServices.WorkingDatabase;
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("将选择的文字都乘以一个数字\n");
                PromptDoubleOptions pdo = new PromptDoubleOptions("请输入你想乘以的数字:\n");
                PromptDoubleResult pdr = ed.GetDouble(pdo);
                List<ObjectId> oidList = TextProcessTools.SeletTexts(db);
                TextProcessTools.ChangeAllText(db, oidList, pdr.Value);
            }
            catch (System.Exception)
            {

                Application.ShowAlertDialog("请检查选择的 是否是数字");
            }

        }
        [CommandMethod("td6")]
        public static void texttestdemo6()
        {
            try
            {
                Database db = HostApplicationServices.WorkingDatabase;
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("将选择的文字都加上一个数字\n");
                PromptDoubleOptions pdo = new PromptDoubleOptions("请输入你想加上的数字:\n");
                PromptDoubleResult pdr = ed.GetDouble(pdo);
                List<ObjectId> oidList = TextProcessTools.SeletTexts(db);
                TextProcessTools.ChangeAllText_AddNum(db, oidList, pdr.Value);
            }
            catch (System.Exception)
            {

                Application.ShowAlertDialog("请检查选择的文字是否是数字");
            }

        }
        [CommandMethod("bgjc")]
        public static void texttestdemo7()
        {

            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions pso = new PromptEntityOptions("选择目标文字");
            PromptEntityResult per = ed.GetEntity(pso);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status != PromptStatus.OK) return;
                DBText dbtext = trans.GetObject(per.ObjectId, OpenMode.ForWrite) as DBText;
                dbtext.ColorIndex = 3;
                List<ObjectId> oidList = TextProcessTools.SeletTexts(db);
                foreach (ObjectId item in oidList)
                {
                    double height = Convert.ToDouble(dbtext.TextString) + (((DBText)(item.GetObject(OpenMode.ForWrite))).Position.Y - dbtext.Position.Y) / 1000;
                    db.ChangeHeightDbtext(item, height.ToString("#0.000"));
                    //db.ChangeHeightDbtext(item, ((DBText)(item.GetObject(OpenMode.ForWrite))).Position.Y);
                }

                trans.Commit();

            }

        }
    }
}
