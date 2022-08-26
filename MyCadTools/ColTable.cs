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
    //柱类
    public class ConCollum
    {
        public ConCollum()
        { ;}
        #region 操作字符
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _biaogao;
        public string Biaogao
        {
            get { return _biaogao; }
            set { _biaogao = value; }
        }

        private string _jiemian;
        /// <summary>
        /// 400x400
        /// </summary>
        public string Jiemian
        {
            get { return _jiemian; }
            set { _jiemian = value; }
        }

        public double GetColB()
        {
            string b = this.Jiemian.Substring(0, this.Jiemian.IndexOf('x'));
            return Convert.ToDouble(b);
        }
        public double GetColH()
        {
            string h = this.Jiemian.Substring(this.Jiemian.IndexOf('x') + 1);
            return Convert.ToDouble(h);
        }

        private string _allsteel;
        public string Allsteel
        {
            get { return _allsteel; }
            set { _allsteel = value; }
        }
        public double GetAllSteelArea(Database db, DBText pt)
        {
            double re = (GetMBSteelArea() * GetMHSteelArea()) * 2 + GetColSteelArea();
            DBText dbtext = new DBText();
            dbtext.TextString = re.ToString();
            dbtext.Position = pt.Position;
            db.AddToModelSpace(dbtext);
            return re;
        }

        private string _consteel;
        public string Consteel
        {
            get { return _consteel; }
            set { _consteel = value; }
        }

        public double GetColSteelArea()
        {
            return PlToPointTool.GetSteelArea(this.Consteel);
        }

        private string _middlebsteel;
        public string Middlebsteel
        {
            get { return _middlebsteel; }
            set { _middlebsteel = value; }
        }

        public double GetMBSteelArea()
        {
            return PlToPointTool.GetSteelArea(this.Middlebsteel);
        }

        private string _middlehsteel;
        public string Middlehsteel
        {
            get { return _middlehsteel; }
            set { _middlehsteel = value; }
        }

        public double GetMHSteelArea()
        {
            return PlToPointTool.GetSteelArea(this.Middlehsteel);
        }

        private string _gujingtype;
        public string Gujingtype
        {
            get { return _gujingtype; }
            set { _gujingtype = value; }
        }

        private string _wgujing;
        public string WGujing
        {
            get { return _wgujing; }
            set { _wgujing = value; }
        }

        private string _ngujing;
        public string NGujing
        {
            get { return _ngujing; }
            set { _ngujing = value; }
        }

        public ConCollum(Database db, Table tb, int rownum, string name, string biaogao, string jiemian, string allsteel, string consteel,
            string middlebsteel, string middlehsteel, string gujingtype, string wgujing, string ngujing)
        {
            this.Name = name;
            this.Biaogao = biaogao;
            this.Jiemian = jiemian;
            this.Allsteel = allsteel;
            this.Consteel = consteel;
            this.Middlebsteel = middlebsteel;
            this.Middlehsteel = middlehsteel;
            this.Gujingtype = gujingtype;
            this.WGujing = wgujing;
            this.NGujing = ngujing;
            tb.Cells[rownum, 0].TextString = this.Name;
            tb.Cells[rownum, 1].TextString = this.Biaogao;
            tb.Cells[rownum, 2].TextString = this.Jiemian;
            tb.Cells[rownum, 3].TextString = this.Allsteel;
            tb.Cells[rownum, 4].TextString = this.Consteel;
            tb.Cells[rownum, 5].TextString = this.Middlebsteel;
            tb.Cells[rownum, 6].TextString = this.Middlehsteel;
            tb.Cells[rownum, 7].TextString = this.Gujingtype;
            tb.Cells[rownum, 8].TextString = this.WGujing;
            tb.Cells[rownum, 9].TextString = this.NGujing;
        }

        public ConCollum(string name, string biaogao, string jiemian, string allsteel, string consteel,
           string middlebsteel, string middlehsteel, string gujingtype, string wgujing, string ngujing)
        {
            this.Name = name;
            this.Biaogao = biaogao;
            this.Jiemian = jiemian;
            this.Allsteel = allsteel;
            this.Consteel = consteel;
            this.Middlebsteel = middlebsteel;
            this.Middlehsteel = middlehsteel;
            this.Gujingtype = gujingtype;
            this.WGujing = wgujing;
            this.NGujing = ngujing;
        }
        #endregion
        public static double cover = 20;//混凝土保护层
        #region 操作Dbtext
        public DBText dbtext_name;//名字:如KZ1
        public DBText dbtext_biaogao;//标高如:0.000~3.000
        public DBText dbtext_jiemian;//截面如:400x500
        public DBText dbtext_allsteel;//全部配筋：4%%13220（角）+8%%13220 or 4%%13220（角）+8%%13220 +8%%13222
        public DBText dbtext_consteel;//角部钢筋：4%%13220
        public DBText dbtext_middlebsteel;//b中部钢筋：2%%13220
        public DBText dbtext_middlehsteel;//h中部钢筋：2%%13220
        public DBText dbtext_gujingtype;//箍筋类型：1(3x4)
        public DBText dbtext_wgujing;//外箍筋如%%1328@100/200或%%1328@100
        public DBText dbtext_ngujing;//内箍筋如%%1328@100/200或%%1328@100
        public ConCollum(Database db, DBText name, DBText biaogao, DBText jiemian, DBText allsteel, DBText consteel, DBText middlebsteel, DBText middlehsteel, DBText gujingtype, DBText wgujing, DBText ngujing)
        {
            dbtext_name = name;
            dbtext_biaogao = biaogao;
            dbtext_jiemian = jiemian;
            dbtext_allsteel = allsteel;
            dbtext_consteel = consteel;
            dbtext_middlebsteel = middlebsteel;
            dbtext_middlehsteel = middlehsteel;
            dbtext_gujingtype = gujingtype;
            dbtext_wgujing = wgujing;
            dbtext_ngujing = ngujing;
        }
        public double Jiemian_B
        {
            get
            {
                return Convert.ToDouble(dbtext_jiemian.TextString.Substring(0, dbtext_jiemian.TextString.IndexOf('x')));
            }
        }//截面B
        public double Jiemian_H
        {
            get
            {
                return Convert.ToDouble(dbtext_jiemian.TextString.Substring(dbtext_jiemian.TextString.IndexOf('x') + 1));
            }
        }//截面H
        public double Consteel_Number
        {
            get
            {
                string[] sArray = dbtext_consteel.TextString.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[0]);
            }
        }//角筋数量
        public double Consteel_D
        {
            get
            {
                string[] sArray = dbtext_consteel.TextString.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }
        }//角筋直径
        public double Middlebsteel_Number
        {
            get
            {
                string[] sArray = dbtext_middlebsteel.TextString.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[0]);
            }
        }//b中部钢筋数量
        public double Middlebsteel_D
        {
            get
            {
                string[] sArray = dbtext_middlebsteel.TextString.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }
        }//b中部钢筋直径
        public double Middlehsteel_Number
        {
            get
            {
                string[] sArray = dbtext_middlehsteel.TextString.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[0]);
            }
        }//h中部钢筋数量
        public double Middlehsteel_D
        {
            get
            {
                string[] sArray = dbtext_middlehsteel.TextString.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }
        }//h中部钢筋直径
        public double Gujingtype_B
        {
            get
            {
                string[] sArray = dbtext_gujingtype.TextString.Split(new string[] { "(", "x", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }
        }//b中部箍筋数量
        public double Gujingtype_H
        {
            get
            {
                string[] sArray = dbtext_gujingtype.TextString.Split(new string[] { "(", "x", ")" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[2]);
            }
        }//h中部箍筋数量
        public double Wgujingtype_D
        {
            get
            {
                string[] sArray = dbtext_wgujing.TextString.Split(new string[] { "%%132", "@", "/" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[0]);
            }
        }//外箍筋直径
        public double Wgujingtype_Gap
        {
            get
            {
                string[] sArray = dbtext_wgujing.TextString.Split(new string[] { "%%132", "@", "/" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }
        }//外箍筋间距
        public double Ngujingtype_D
        {
            get
            {
                string[] sArray = dbtext_ngujing.TextString.Split(new string[] { "%%132", "@", "/" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[0]);
            }
        }//内箍筋直径
        public double Ngujingtype_Gap
        {
            get
            {
                string[] sArray = dbtext_ngujing.TextString.Split(new string[] { "%%132", "@", "/" }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToDouble(sArray[1]);
            }
        }//内箍筋间距
        public string Allsteel_String
        {
            get
            {
                if (Middlebsteel_D != Middlehsteel_D)
                {
                    return dbtext_consteel.TextString + "(角筋)+" + 2 * Middlebsteel_Number + "%%132" + Middlebsteel_D + "+" + 2 * Middlehsteel_Number + "%%132" + Middlehsteel_D;
                }
                else
                {
                    return dbtext_consteel.TextString + "(角筋)+" + 2 * (Middlebsteel_Number + Middlehsteel_Number) + "%%132" + Middlebsteel_D;
                }

            }
        }//由以上数据计算全截面钢筋的面积字符串
        public double Jiemian_Area
        {
            get
            {
                return Jiemian_B * Jiemian_H;
            }
        }//由以上数据计算截面面积
        public double Consteel_Area
        {
            get
            {
                return 3.14 / 4 * Consteel_D * Consteel_D * Consteel_Number;
            }
        }//由以上数据计算截面角筋面积
        public double Middlebsteel_Area
        {
            get
            {
                return 3.14 / 4 * Consteel_D * Consteel_D * Consteel_Number / 2 + 3.14 / 4 * Middlebsteel_D * Middlebsteel_D * Middlebsteel_Number;
            }
        }//由以上数据计算截面B侧钢筋面积
        public double Middlehsteel_Area
        {
            get
            {
                return 3.14 / 4 * Consteel_D * Consteel_D * Consteel_Number / 2 + 3.14 / 4 * Middlehsteel_D * Middlehsteel_D * Middlehsteel_Number;
            }
        }//由以上数据计算截面H侧钢筋面积
        public double Allsteel_Area
        {
            get
            {
                return Middlebsteel_Area * 2 + Middlehsteel_Area * 2 - Consteel_Area;
            }
        }//由以上数据计算全截面钢筋面积
        public string Allsteel_Area_ratio
        {
            get
            {
                return Math.Round(Allsteel_Area / Jiemian_Area * 100, 3).ToString();
            }
        }//由以上数据计算截面配筋率
        public string Middlebsteel_Area_ratio
        {
            get
            {
                return Math.Round(Middlebsteel_Area / Jiemian_Area * 100, 3).ToString();
            }
        }//由以上数据计算截面B侧配筋率
        public string Middlehsteel_Area_ratio
        {
            get
            {
                return Math.Round(Middlehsteel_Area / Jiemian_Area * 100, 3).ToString();
            }
        }//由以上数据计算截面H侧配筋率
        public string Gujing_Steel_ratio  //外箍筋直径
        {
            get
            {
                double ml = Jiemian_B - 2 * cover - Wgujingtype_D;
                double nl = Jiemian_H - 2 * cover - Wgujingtype_D;
                double m = Gujingtype_B;
                double n = Gujingtype_H;
                double D = Wgujingtype_D;
                double D_As = 3.14 * D * D / 4;
                double d = Ngujingtype_D;
                double d_As = 3.14 * d * d / 4;
                double s = Wgujingtype_Gap;
                double Acor = (ml - Wgujingtype_D) * (nl - Wgujingtype_D);
                return Math.Round(((2 * ml + 2 * nl) * D_As + ((m - 2) * ml + (n - 2) * nl) * d_As) / Acor / s * 100, 3).ToString();
            }
        }
        #endregion
    }
    //柱表计算主程序
    public class ColTable
    {
        /// <summary>
        /// 柱表计算测试1
        /// </summary>
        [CommandMethod("ct1")]
        public void CalColDemo1()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult psr = ed.GetSelection();
            List<ConCollum> ConCollumList = new List<ConCollum>();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        Polyline pl = item.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
                        pl.ColorIndex = 5;
                        ConCollumList = GetConCol1(db, pl);
                    }
                }
                trans.Commit();
            }

        }
        /// <summary>
        /// 按string处理选择多段线内的文字
        /// </summary>
        /// <param name="db">当前数据库</param>
        /// <param name="pl">多段线</param>
        /// <returns>柱list</returns>
        public List<ConCollum> GetConCol1(Database db, Polyline pl)
        {
            List<ConCollum> ConCollumList = new List<ConCollum>();
            List<DBText> dbtextList = new List<DBText>();
            List<DBText> resultList = new List<DBText>();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.Start, "TEXT"),
                new TypedValue((int)DxfCode.Operator, "and>"), 
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.LayerName, "TAB_TEXT"),
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
                        dbtext.ColorIndex = 3;
                        dbtextList.Add(dbtext);
                    }
                    resultList = PlToPointTool.SortDbtext(dbtextList);
                    ConCollum[] col = new ConCollum[resultList.Count / 10];
                    #region MyRegion
                    //for (int i = 0; i < resultList.Count; i++)
                    //{
                    //    DBText dbtext = new DBText();
                    //    dbtext.TextString = i.ToString();
                    //    dbtext.Height = 100;
                    //    dbtext.Position = resultList[i].Position;
                    //    AddToModelTools.AddToModelSpace(db, dbtext);
                    //}
                    #endregion

                    for (int i = 0; i < col.Length; i++)
                    {
                        col[i] = new ConCollum(resultList[i * 10].TextString, resultList[i * 10 + 1].TextString, resultList[i * 10 + 2].TextString, resultList[i * 10 + 3].TextString,
                                               resultList[i * 10 + 4].TextString, resultList[i * 10 + 5].TextString, resultList[i * 10 + 6].TextString, resultList[i * 10 + 7].TextString,
                                               resultList[i * 10 + 8].TextString, resultList[i * 10 + 9].TextString);

                        if (TextProcessTools.GetSteelArea(resultList[i * 10 + 5].TextString) == TextProcessTools.GetSteelArea(resultList[i * 10 + 6].TextString))
                        {
                            double steelNumber = TextProcessTools.GetSteelNumber(resultList[i * 10 + 5].TextString) * 2 + TextProcessTools.GetSteelNumber(resultList[i * 10 + 6].TextString) * 2;
                            double Area = TextProcessTools.GetSteelArea(resultList[i * 10 + 5].TextString);
                            string allstellarea = resultList[i * 10 + 4].TextString.ToString() + "(角筋)" + "+" + steelNumber.ToString() + "%%132" + Area;
                            TextProcessTools.ChangeDbtext(db, resultList[i * 10 + 3].ObjectId, allstellarea);
                            double Jm = (TextProcessTools.GetB(resultList[i * 10 + 2].TextString) * TextProcessTools.GetH(resultList[i * 10 + 2].TextString));
                            double As = (TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 4].TextString) + TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 5].TextString) * 2 +
                            TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 6].TextString) * 2);
                            TextProcessTools.AddText(db, Jm.ToString(), resultList[i * 10 + 2], new Vector3d(1000, -150, 0), 3);
                            double peijinglv = Math.Round(As / Jm, 4);
                            string peijingper = peijinglv * 100 + "%";
                            TextProcessTools.AddText(db, peijingper, resultList[i * 10 + 3], new Vector3d(1500, -150, 0), 3);

                            double cornersteel = TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 4].TextString);
                            TextProcessTools.AddText(db, cornersteel.ToString(), resultList[i * 10 + 4], new Vector3d(500, -150, 0), 3);

                            double bsteel = TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 5].TextString) / Jm * 100;
                            TextProcessTools.AddText(db, bsteel.ToString() + "%", resultList[i * 10 + 5], new Vector3d(500, -150, 0), 3);

                            double hsteel = TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 6].TextString) / Jm * 100;
                            TextProcessTools.AddText(db, hsteel.ToString() + "%", resultList[i * 10 + 6], new Vector3d(500, -150, 0), 3);

                        }
                        else
                        {
                            double steelNumber1 = TextProcessTools.GetSteelNumber(resultList[i * 10 + 5].TextString) * 2;
                            double steelNumber2 = TextProcessTools.GetSteelNumber(resultList[i * 10 + 6].TextString) * 2;
                            double Area1 = TextProcessTools.GetSteelArea(resultList[i * 10 + 5].TextString);
                            double Area2 = TextProcessTools.GetSteelArea(resultList[i * 10 + 6].TextString);
                            string allstellarea = resultList[i * 10 + 4].TextString.ToString() + "(角筋)" + "+" + steelNumber1 + "%%132" + Area1 + "+" + steelNumber2 + "%%132" + Area2;

                            TextProcessTools.ChangeDbtext(db, resultList[i * 10 + 3].ObjectId, allstellarea);
                            TextProcessTools.AddText(db, (TextProcessTools.GetB(resultList[i * 10 + 2].TextString) * TextProcessTools.GetH(resultList[i * 10 + 2].TextString)).ToString(), resultList[i * 10 + 2], new Vector3d(1000, -150, 0), 3);

                            double Jm = (TextProcessTools.GetB(resultList[i * 10 + 2].TextString) * TextProcessTools.GetH(resultList[i * 10 + 2].TextString));
                            double As = (TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 4].TextString) + TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 5].TextString) * 2 +
                            TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 6].TextString) * 2);
                            TextProcessTools.AddText(db, Jm.ToString(), resultList[i * 10 + 2], new Vector3d(1000, -150, 0), 3);
                            double peijinglv = Math.Round(As / Jm, 4);
                            string peijingper = peijinglv * 100 + "%";
                            TextProcessTools.AddText(db, peijingper, resultList[i * 10 + 3], new Vector3d(1500, -150, 0), 3);

                            double cornersteel = TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 4].TextString);
                            TextProcessTools.AddText(db, cornersteel.ToString(), resultList[i * 10 + 4], new Vector3d(500, -150, 0), 3);

                            double bsteel = TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 5].TextString) / Jm * 100;
                            TextProcessTools.AddText(db, bsteel.ToString() + "%", resultList[i * 10 + 5], new Vector3d(500, -150, 0), 3);

                            double hsteel = TextProcessTools.GetBeamSteelArea(resultList[i * 10 + 6].TextString) / Jm * 100;
                            TextProcessTools.AddText(db, hsteel.ToString() + "%", resultList[i * 10 + 6], new Vector3d(500, -150, 0), 3);

                        }
                        ConCollumList.Add(col[i]);
                    }


                }

                trans.Commit();
            }

            return ConCollumList;
        }
        /// <summary>
        /// 柱表计算测试2
        /// </summary>
        [CommandMethod("ct2")]
        public void CalColDemo2()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult psr = ed.GetSelection();
            List<ConCollum> ConCollumList = new List<ConCollum>();
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (psr.Status == PromptStatus.OK)
                    {
                        SelectionSet sset = psr.Value;
                        foreach (SelectedObject item in sset)
                        {
                            Polyline pl = item.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
                            db.DelectDbtext(3, pl);
                            ConCollumList = GetConCol2(db, pl);
                        }

                        for (int i = 0; i < ConCollumList.Count; i++)
                        {
                            db.ChangeDbtext(ConCollumList[i].dbtext_allsteel.ObjectId, ConCollumList[i].Allsteel_String);
                            db.AddText(ConCollumList[i].Allsteel_Area_ratio + "%", ConCollumList[i].dbtext_consteel, new Vector3d(-4450, -150, 0), 3, 0.5);
                            db.AddText(ConCollumList[i].Middlebsteel_Area_ratio + "%", ConCollumList[i].dbtext_consteel, new Vector3d(1050, -150, 0), 3, 0.2);
                            db.AddText(ConCollumList[i].Middlehsteel_Area_ratio + "%", ConCollumList[i].dbtext_consteel, new Vector3d(2550, -150, 0), 3, 0.2);
                            db.AddText(ConCollumList[i].Gujing_Steel_ratio + "%", ConCollumList[i].dbtext_consteel, new Vector3d(5500, -150, 0), 3, 0.8);
                        }
                    }
                    trans.Commit();
                }
            }
            catch
            {
                Application.ShowAlertDialog("请检查钢筋符号是否是%%132");
            }


        }
        /// <summary>
        /// 按dbtext处理选择多段线内的文字
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pl"></param>
        /// <returns></returns>
        public List<ConCollum> GetConCol2(Database db, Polyline pl)
        {
            List<ConCollum> ConCollumList = new List<ConCollum>();
            List<DBText> dbtextList = new List<DBText>();
            List<DBText> resultList = new List<DBText>();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.Start, "TEXT"),
                new TypedValue((int)DxfCode.Operator, "and>"), 
                new TypedValue((int)DxfCode.Operator, "<and"),
                new TypedValue((int)DxfCode.LayerName, "TAB_TEXT"),
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
                        dbtextList.Add(dbtext);
                    }
                    resultList = PlToPointTool.SortDbtext(dbtextList);
                    ConCollum[] col = new ConCollum[resultList.Count / 10];


                    for (int i = 0; i < col.Length; i++)
                    {
                        col[i] = new ConCollum(db, resultList[i * 10], resultList[i * 10 + 1], resultList[i * 10 + 2], resultList[i * 10 + 3],
                                               resultList[i * 10 + 4], resultList[i * 10 + 5], resultList[i * 10 + 6], resultList[i * 10 + 7],
                                               resultList[i * 10 + 8], resultList[i * 10 + 9]);
                        ConCollumList.Add(col[i]);
                    }


                }

                trans.Commit();
            }

            return ConCollumList;
        }
    }
    //画柱表
    public class DrawColTable
    {
        /// <summary>
        /// 画柱表测试1
        /// </summary>
        [CommandMethod("dt1")]
        public void TableDemo1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\r\n code by sailing 176530355 \r\n");
            Table tb = new Table();
            tb.SetSize(8, 10);//表行列数
            tb.SetRowHeight(700);//表行高
            tb.SetColumnWidth(3000);//表列宽
            tb.Cells.Alignment = CellAlignment.MiddleCenter;//表文字对齐
            tb.Cells.TextHeight = 350;//表文字高度
            tb.Cells.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");
            tb.Rows[0].Height = 1000;
            tb.Columns[0].Width = 2000;
            tb.Columns[1].Width = 4000;
            tb.Columns[5].Width = 2500;
            tb.Columns[6].Width = 2500;

            tb.Cells[0, 0].TextString = "柱表";

            ConCollum mulu = new ConCollum(db, tb, 1, "柱号", "标高", "截面", "全部纵筋", "角筋", "B侧中部纵筋", "H侧中部纵筋", "箍筋类型", "外箍筋", "内箍筋");
            ConCollum cc = new ConCollum(db, tb, 2, "KZ1", "基础顶~-0.300", "400x400", "", "4%%13222", "1%%13218", "1%%13222", "1(3x3)", "4%%13222", "4%%13222");
            ConCollum cc1 = new ConCollum(db, tb, 3, "KZ1", "-0.300~3.570", "400x400", "", "4%%13222", "1%%13218", "1%%13222", "1(3x3)", "4%%13222", "4%%13222");
            ConCollum cc2 = new ConCollum(db, tb, 4, "KZ1", "3.570~屋面", "400x400", "", "4%%13222", "1%%13218", "1%%13222", "1(3x3)", "4%%13222", "4%%13222");
            db.AddToModelSpace(tb);
            doc.SendStringToExecute("._zoom _all ", true, false, false);

        }
        /// <summary>
        /// 画柱表测试1
        /// </summary>
        [CommandMethod("dt2")]
        public void TableDemo2()
        {

            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\r\n code by sailing 176530355 \r\n");
            Table tb = new Table();
            tb.SetSize(8, 10);//表行列数
            tb.SetRowHeight(700);//表行高
            tb.SetColumnWidth(3000);//表列宽
            tb.Cells.Alignment = CellAlignment.MiddleCenter;//表文字对齐
            tb.Cells.TextHeight = 350;//表文字高度
            tb.Cells.TextStyleId = db.AddTextStyle("TSSD_Rein", "tssdeng", "hztxt");

            tb.Rows[0].Height = 1000;
            tb.Columns[0].Width = 2000;
            tb.Columns[1].Width = 4000;
            tb.Columns[5].Width = 2500;
            tb.Columns[6].Width = 2500;

            tb.Cells[0, 0].TextString = "柱表";

            ConCollum mulu = new ConCollum(db, tb, 1, "柱号", "标高", "截面", "全部纵筋", "角筋", "B侧中部纵筋", "H侧中部纵筋", "箍筋类型", "外箍筋", "内箍筋");
            ConCollum cc = new ConCollum(db, tb, 2, "KZ1", "基础顶~-0.300", "400x400", "", "4%%13222", "1%%13218", "1%%13222", "1(3x3)", "4%%13222", "4%%13222");
            ConCollum cc1 = new ConCollum(db, tb, 3, "KZ1", "-0.300~3.570", "400x400", "", "4%%13222", "1%%13218", "1%%13222", "1(3x3)", "4%%13222", "4%%13222");
            ConCollum cc2 = new ConCollum(db, tb, 4, "KZ1", "3.570~屋面", "400x400", "", "4%%13222", "1%%13218", "1%%13222", "1(3x3)", "4%%13222", "4%%13222");

            db.AddToModelSpace(tb);

        }
        [CommandMethod("hzzb")]
        public void DrawColTableDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\r\n code by sailing 176530355 \r\n");
            PromptPointOptions ppo = new PromptPointOptions("请指点柱表位置");
            PromptPointResult ppr = ed.GetPoint(ppo);
            Point3d pt = ppr.Value;
            Polyline plwaikuang = new Polyline();
            double x = pt.X;
            double y = pt.Y;
            Point2d startPoint = new Point2d(x, y);
            Point2d pt0 = new Point2d(x, y);
            Point2d pt1 = new Point2d(x + 25000, y);
            Point2d pt2 = new Point2d(x + 25000, y - 7300);
            Point2d pt3 = new Point2d(x, y - 7300);
            plwaikuang.AddVertexAt(0, pt0, 0, 0, 0);
            plwaikuang.AddVertexAt(1, pt1, 0, 0, 0);
            plwaikuang.AddVertexAt(2, pt2, 0, 0, 0);
            plwaikuang.AddVertexAt(3, pt3, 0, 0, 0);
            plwaikuang.Closed = true;

            plwaikuang.SetLayerId(db.AddLayerTool("TAB"), true);
            db.AddToModelSpace(plwaikuang);
            db.DrawColTableDemo(plwaikuang.ObjectId, 25000, 7300, 10, 10);
        }
        [CommandMethod("clearctb")]
        public void ClearColTableDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult psr = ed.GetSelection();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet sset = psr.Value;
                    foreach (SelectedObject item in sset)
                    {
                        Polyline pl = item.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
                        db.DelectDbtext(3, pl);
                        db.DelectDbtext(1, pl);
                    }

                }
                trans.Commit();
            }
        }
    }
    //画表类
    public static class DrawColTableTool
    {
        public static void DrawColTableDemo(this Database db, ObjectId polylineoid, double table_width, double table_height, int row, int collum)
        {

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Polyline plwaikuang = polylineoid.GetObject(OpenMode.ForRead) as Polyline;

                #region 画横线
                Line xline = new Line();
                xline.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -1000, 0);
                xline.EndPoint = xline.StartPoint + new Vector3d(table_width, 0, 0);

                Line xline1 = new Line();
                xline1.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -1700, 0);
                xline1.EndPoint = xline1.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                Line xline2 = new Line();
                xline2.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -2400, 0);
                xline2.EndPoint = xline2.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                Line xline3 = new Line();
                xline3.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -3100, 0);
                xline3.EndPoint = xline3.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                Line xline4 = new Line();
                xline4.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -3800, 0);
                xline4.EndPoint = xline4.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                Line xline5 = new Line();
                xline5.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -4500, 0);
                xline5.EndPoint = xline5.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                Line xline6 = new Line();
                xline6.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -5200, 0);
                xline6.EndPoint = xline6.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                Line xline7 = new Line();
                xline7.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -5900, 0);
                xline7.EndPoint = xline7.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                Line xline8 = new Line();
                xline8.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(0, -6600, 0);
                xline8.EndPoint = xline8.StartPoint + new Vector3d(table_width - 3000, 0, 0);

                db.AddToModelSpace(xline, xline1, xline2, xline3, xline4, xline5, xline6, xline7, xline8);
                #endregion
                #region 画竖线
                Line yline = new Line();
                yline.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(1000, 0, 0);
                yline.EndPoint = yline.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline1 = new Line();
                yline1.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(4000, 0, 0);
                yline1.EndPoint = yline1.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline2 = new Line();
                yline2.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(6000, 0, 0);
                yline2.EndPoint = yline2.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline3 = new Line();
                yline3.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(10000, 0, 0);
                yline3.EndPoint = yline3.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline4 = new Line();
                yline4.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(11500, 0, 0);
                yline4.EndPoint = yline4.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline5 = new Line();
                yline5.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(13000, 0, 0);
                yline5.EndPoint = yline5.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline6 = new Line();
                yline6.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(14500, 0, 0);
                yline6.EndPoint = yline6.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline7 = new Line();
                yline7.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(16000, 0, 0);
                yline7.EndPoint = yline7.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline8 = new Line();
                yline8.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(19000, 0, 0);
                yline8.EndPoint = yline8.StartPoint + new Vector3d(0, -table_height, 0);

                Line yline9 = new Line();
                yline9.StartPoint = plwaikuang.GetPoint3dAt(0) + new Vector3d(22000, 0, 0);
                yline9.EndPoint = yline9.StartPoint + new Vector3d(0, -table_height, 0);

                db.AddToModelSpace(yline, yline1, yline2, yline3, yline4, yline5, yline6, yline7, yline8, yline9);

                #endregion
                #region 表头文字
                db.DoubletoDbtext("柱号", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700, 0), "TAB");
                db.DoubletoDbtext("标高", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(1530, -700, 0), "TAB");
                db.DoubletoDbtext("BxH", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(4700, -700, 0), "TAB");
                db.DoubletoDbtext("全部钢筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(7000, -700, 0), "TAB");
                db.DoubletoDbtext("角筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700, 0), "TAB");
                db.DoubletoDbtext("B一侧", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(11900, -450, 0), "TAB");
                db.DoubletoDbtext("中部钢筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(11700, -850, 0), "TAB");
                db.DoubletoDbtext("H一侧", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(13300, -450, 0), "TAB");
                db.DoubletoDbtext("中部钢筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(13200, -850, 0), "TAB");
                db.DoubletoDbtext("箍筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(14900, -450, 0), "TAB");
                db.DoubletoDbtext("类型号", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -850, 0), "TAB");
                db.DoubletoDbtext("外箍筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(17000, -700, 0), "TAB");
                db.DoubletoDbtext("内箍筋", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(20000, -700, 0), "TAB");
                db.DoubletoDbtext("备注", 400, plwaikuang.GetPoint3dAt(0), new Vector3d(23000, -700, 0), "TAB");
                #endregion
                #region KZ1
                db.DoubletoDbtext("KZ1", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("400x500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(4x4)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850, 0), "TAB_TEXT");
                #endregion
                #region KZ2
                db.DoubletoDbtext("KZ2", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~屋面", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("600x600", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(4x4)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850 - 700, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850 - 700, 0), "TAB_TEXT");
                #endregion
                #region KZ3
                db.DoubletoDbtext("KZ3", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("500x500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(4x4)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850 - 700 * 2, 0), "TAB_TEXT");
                #endregion
                #region KZ4
                db.DoubletoDbtext("KZ4", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("500x500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(4x4)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850 - 700 * 3, 0), "TAB_TEXT");
                #endregion
                #region KZ5
                db.DoubletoDbtext("KZ5", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("基础顶~0.500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("400x600", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(4x5)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850 - 700 * 4, 0), "TAB_TEXT");
                #endregion
                #region KZ6
                db.DoubletoDbtext("KZ1", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("3.600~7.200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("500x500", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(4x4)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850 - 700 * 5, 0), "TAB_TEXT");
                #endregion
                #region KZ7
                db.DoubletoDbtext("KZ7", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("-0.500~3.600", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("400x400", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("2%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(4x4)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850 - 700 * 6, 0), "TAB_TEXT");
                #endregion
                #region KZ8
                db.DoubletoDbtext("KZ8", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(100, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("3.600~屋面", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(1130, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("400x400", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(4100, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220(角筋)+8%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(6500, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("4%%13220", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(10500, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("1%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(12000, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("1%%13218", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(13500, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("1(3x3)", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(14800, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(16600, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                db.DoubletoDbtext("%%1328@100/200", 350, plwaikuang.GetPoint3dAt(0), new Vector3d(19600, -700 - 850 - 700 * 7, 0), "TAB_TEXT");
                #endregion

                trans.Commit();
            }

        }
    }
}
