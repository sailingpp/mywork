using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace MyCadTools
{
    public class GetRaDemo
    {
        //启动命令-批量
        [CommandMethod("getra1")]

        public static void GetRaVersion1()
        {
            // 获得当前文档
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int n = 0;
            int m = 0;
            double r;//桩径
            double re;//大直径调整系数
            double qp;
            double Ra;

            double fsi = 0;//侧摩擦力
            double fp;//端阻力
            Double pz;

            PromptDoubleOptions pdOpts = new PromptDoubleOptions("\n请输入桩直径mm为单位 ");
            PromptDoubleResult pdres = doc.Editor.GetDouble(pdOpts);
            r = pdres.Value;
            PromptDoubleOptions pdOpts1 = new PromptDoubleOptions("\n请输入qpi单位kpa ");
            PromptDoubleResult pdres1 = doc.Editor.GetDouble(pdOpts1);
            qp = pdres1.Value;

            PromptDoubleOptions pdOpts2 = new PromptDoubleOptions("\n请输入单桩承载力 ");
            PromptDoubleResult pdres2 = doc.Editor.GetDouble(pdOpts2);
            Ra = pdres2.Value;

            //Application.ShowAlertDialog("Entered value: " + pdres.Value.ToString());//显示输入的数字


            //设置过滤器
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            //启动事务管理器
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {

                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Application.ShowAlertDialog("请选择土层标高");
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                Application.ShowAlertDialog("请选择qsi");
                PromptSelectionResult re2 = ed.GetSelection(acSelFtr);

                // 如果提示状态是 OK，对象就被选择了 
                if (re1.Status == PromptStatus.OK && re2.Status == PromptStatus.OK)
                {
                    //得到选择集
                    SelectionSet acSSet1 = re1.Value;
                    SelectionSet acSSet2 = re2.Value;
                    //提示选择了多少个对象
                    // Application.ShowAlertDialog("Number of lei selected: " + acSSet1.Count.ToString());
                    ed.WriteMessage("\nNumber of lei selected: " + acSSet1.Count.ToString());
                    ed.WriteMessage("\nNumber of qi selected: " + acSSet2.Count.ToString());
                    // Application.ShowAlertDialog("Number of qi selected: " + acSSet2.Count.ToString());
                    string[] str1 = new string[acSSet1.Count];//用来储存选中的文字
                    double[] lei = new double[acSSet1.Count];//用来储存每一个选择的文字转换来的数字值lei
                    double[] x1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点x坐标
                    double[] y1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点y坐标
                    double[] z1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点z坐标

                    string[] str2 = new string[acSSet2.Count];//用来储存选中的文字
                    double[] qsi = new double[acSSet2.Count];//用来储存每一个选择的文字转换来的数字值qsi
                    double[] x2 = new double[acSSet2.Count];//用来存储每一个选择的文字的选择点x坐标
                    double[] y2 = new double[acSSet2.Count];//用来存储每一个选择的文字的选择点y坐标
                    double[] z2 = new double[acSSet2.Count];//用来存储每一个选择的文字的选择点z坐标

                    double[] ht = new double[acSSet1.Count];

                    // 遍历选择集中的对象 
                    //把选中lei的值和坐标赋予数组当中
                    #region
                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            // 检查以确定返回的 SelectedObject 对象是有效的
                            // 以写的方式打开选择的对象 
                            DBText txt1 = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (txt1 != null)
                            {
                                // 修改对象的颜色
                                txt1.ColorIndex = 1;
                                // 得到所选对象的string以便转为数字用来计算
                                str1[n] = txt1.TextString;

                                int i = str1[n].IndexOf("(");
                                str1[n] = str1[n].Substring(0, i);
                                lei[n] = double.Parse(str1[n]);   //储存深度

                                //得到所选择数字的选择点坐标x，y，z
                                x1[n] = txt1.Position.X;
                                y1[n] = txt1.Position.Y;
                                z1[n] = txt1.Position.Z;

                                ht[n] = txt1.Height;
                                // Application.ShowAlertDialog("lei{" +n+"}="+lei[n].ToString());
                                // ed.WriteMessage("\nlei{" + n + "}=" + lei[n].ToString());
                                n = n + 1;

                            }
                            else
                            {
                                Application.ShowAlertDialog("Number of objects selected: 0");
                            }
                        }
                    }
                    int min;
                    for (int i = 0; i < y1.Length; i++)
                    {
                        min = i;
                        for (int j = i + 1; j < y1.Length; j++)
                        {
                            if (y1[j] < y1[min])
                                min = j;
                        }
                        double t = y1[min];
                        double t1 = lei[min];
                        y1[min] = y1[i];
                        lei[min] = lei[i];
                        y1[i] = t;
                        lei[i] = t1;

                        ed.WriteMessage("\n" + y1[i] + "每层长度," + lei[i]);
                    }
                    #endregion



                    //把选中qsi的值和坐标赋予数组当中

                    #region
                    foreach (SelectedObject acSSObj in acSSet2)
                    {
                        if (acSSObj != null)
                        {
                            // 检查以确定返回的 SelectedObject 对象是有效的
                            // 以写的方式打开选择的对象 
                            DBText txt2 = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (txt2 != null)
                            {
                                // 修改对象的颜色
                                txt2.ColorIndex = 5;
                                // 得到所选对象的string以便转为数字用来计算
                                str2[m] = txt2.TextString;
                                int i = str2[m].IndexOf("=");
                                str2[m] = str2[m].Substring(i + 1);
                                qsi[m] = double.Parse(str2[m]);   //force

                                //得到所选择数字的选择点坐标x，y，z
                                x2[m] = txt2.Position.X;
                                y2[m] = txt2.Position.Y;
                                z2[m] = txt2.Position.Z;
                                // Application.ShowAlertDialog(" qsi{" + m.ToString() + "}=" + qsi[m].ToString());
                                //  ed.WriteMessage("\nqsi{" + m.ToString() + "}=" + qsi[m].ToString());
                                m = m + 1;
                            }
                            else
                            {
                                Application.ShowAlertDialog("Number of objects selected: 0");
                            }
                        }
                    }
                    int min1;
                    for (int i = 0; i < y2.Length; i++)
                    {
                        min1 = i;
                        for (int j = i + 1; j < y2.Length; j++)
                        {
                            if (y2[j] < y2[min1])
                                min1 = j;
                        }
                        double t = y2[min1];
                        double t1 = qsi[min1];
                        y2[min1] = y2[i];
                        qsi[min1] = qsi[i];
                        y2[i] = t;
                        qsi[i] = t1;

                        ed.WriteMessage("\n" + y2[i] + "侧摩阻," + qsi[i]);
                    }
                    #endregion



                    //生成测阻力合力大小
                    if (r < 800)
                        re = 1;
                    else
                        re = Math.Pow(800 / r, 1.0 / 5);

                    for (int i = 0; i < acSSet2.Count; i++)
                    {
                        fsi += Math.Round(Math.PI * r * re * (qsi[i] * (lei[i] - lei[i + 1])) / 1000, 1);
                    }
                    //生成测阻力的位置
                    DBText objText = new DBText();
                    objText.SetDatabaseDefaults();
                    objText.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 2 * ht[1], 0);
                    objText.TextString = "qs:" + fsi.ToString();
                    objText.TextStyleId = db.Textstyle;
                    objText.Height = ht[1];
                    objText.ColorIndex = 3;

                    acBlkTblRec.AppendEntity(objText);
                    acTrans.AddNewlyCreatedDBObject(objText, true);

                    fp = Math.Round(Math.PI * r / 1000 * r / 1000 / 4 * qp, 1);
                    DBText objText1 = new DBText();
                    objText1.SetDatabaseDefaults();
                    objText1.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 4 * ht[1], 0);
                    objText1.TextString = "qp:" + fp.ToString();
                    objText1.TextStyleId = db.Textstyle;
                    objText1.Height = ht[1];
                    objText1.ColorIndex = 3;

                    acBlkTblRec.AppendEntity(objText1);
                    acTrans.AddNewlyCreatedDBObject(objText1, true);

                    pz = Math.Round(fp + fsi, 1);

                    DBText objText2 = new DBText();
                    objText2.SetDatabaseDefaults();
                    objText2.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 6 * ht[1], 0);
                    objText2.TextString = "Ra:" + pz.ToString();
                    objText2.TextStyleId = db.Textstyle;
                    objText2.Height = ht[1];
                    objText2.ColorIndex = 6;


                    //append the new mtext object to model space
                    acBlkTblRec.AppendEntity(objText2);
                    acTrans.AddNewlyCreatedDBObject(objText2, true);

                    if (pz < Ra)
                    {
                        DBText objText3 = new DBText();
                        objText3.SetDatabaseDefaults();
                        objText3.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 8 * ht[1], 0);
                        objText3.TextString = "NEEDMORErLen" + Math.Round((Ra - pz) / (Math.PI * r / 1000 * qsi[0]), 1) + "m";

                        objText3.TextStyleId = db.Textstyle;
                        objText3.Height = ht[1];
                        objText3.ColorIndex = 1;
                        acBlkTblRec.AppendEntity(objText3);
                        acTrans.AddNewlyCreatedDBObject(objText3, true);


                        DBText objText4 = new DBText();
                        objText4.SetDatabaseDefaults();
                        objText4.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 10 * ht[1], 0);
                        objText4.TextString = "DLen:" + Math.Round((lei[0] - lei[acSSet2.Count]) + (Ra - pz) / (Math.PI * r / 1000 * qsi[0]), 1) + "m";
                        objText4.TextStyleId = db.Textstyle;
                        objText4.Height = ht[1];
                        objText4.ColorIndex = 1;
                        acBlkTblRec.AppendEntity(objText4);
                        acTrans.AddNewlyCreatedDBObject(objText4, true);


                    }
                    else
                    {
                        ed.WriteMessage("桩长已经满足");
                        DBText objText4 = new DBText();
                        objText4.SetDatabaseDefaults();
                        objText4.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 10 * ht[1], 0);
                        objText4.TextString = "Len:" + Math.Round((lei[0] - lei[acSSet2.Count]), 1) + "m";
                        objText4.TextStyleId = db.Textstyle;
                        objText4.Height = ht[1];
                        objText4.ColorIndex = 3;
                        acBlkTblRec.AppendEntity(objText4);
                        acTrans.AddNewlyCreatedDBObject(objText4, true);

                        DBText objText5 = new DBText();
                        objText5.SetDatabaseDefaults();
                        objText5.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 8 * ht[1], 0);
                        objText5.TextString = "rLen:" + Math.Round((lei[0] - lei[1]), 1) + "m";
                        objText5.TextStyleId = db.Textstyle;
                        objText5.Height = ht[1];
                        objText5.ColorIndex = 3;
                        acBlkTblRec.AppendEntity(objText5);
                        acTrans.AddNewlyCreatedDBObject(objText5, true);

                        DBText objText6 = new DBText();
                        objText6.SetDatabaseDefaults();
                        objText6.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 12 * ht[1], 0);
                        objText6.TextString = "OnlyNeedLen:" + Math.Round((lei[0] - lei[acSSet2.Count]) - (pz - Ra) / (Math.PI * r / 1000 * qsi[0]), 1) + "m";
                        objText6.TextStyleId = db.Textstyle;
                        objText6.Height = ht[1];
                        objText6.ColorIndex = 3;
                        acBlkTblRec.AppendEntity(objText6);
                        acTrans.AddNewlyCreatedDBObject(objText6, true);


                    }



                }



                //确认修改
                acTrans.Commit();

            }

        }

        [CommandMethod("getra2")]
        public static void GetRaVersion2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int n = 0;



            TypedValue[] acTypValAr = new TypedValue[1];

            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);

            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);

                if (re1.Status == PromptStatus.OK)
                {
                    SelectionSet acSSet = re1.Value;
                    Application.ShowAlertDialog("Number of objects selected: " + acSSet.Count.ToString());

                    string[] str = new string[acSSet.Count];

                    double[] num = new double[acSSet.Count];




                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        if (acSSObj != null)
                        {
                            DBText txt1 = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (txt1 != null)
                            {
                                txt1.ColorIndex = 2;
                                str[n] = txt1.TextString;
                                // int i = str[n].IndexOf("(");
                                int i = str[n].IndexOf("=");
                                //str[n] = str[n].Substring(0,i);
                                str[n] = str[n].Substring(i + 1);
                                num[n] = double.Parse(str[n]);   //force
                                Application.ShowAlertDialog("Number of objects selected: " + num[n]);

                                n = n + 1;
                            }
                            else
                            {
                                Application.ShowAlertDialog("Number of objects selected: 0");
                            }


                        }
                    }



                }
                acTrans.Commit();

            }

        }

        //启动命令-批量
        [CommandMethod("getra3")]
        public static void GetRaVersion3()
        {
            // 获得当前文档
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int n = 0;
            int m = 0;
            double r;//桩径
            double re;//大直径调整系数
            double qp;

            double fsi = 0;//侧摩擦力
            double fp;//端阻力
            Double pz;


            PromptDoubleOptions pdOpts = new PromptDoubleOptions("\n请输入桩直径mm为单位 ");
            PromptDoubleResult pdres = doc.Editor.GetDouble(pdOpts);
            r = pdres.Value;
            PromptDoubleOptions pdOpts1 = new PromptDoubleOptions("\n请输入qpi单位kpa ");
            PromptDoubleResult pdres1 = doc.Editor.GetDouble(pdOpts1);
            qp = pdres1.Value;
            //Application.ShowAlertDialog("Entered value: " + pdres.Value.ToString());//显示输入的数字


            //设置过滤器
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            //启动事务管理器
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {

                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Application.ShowAlertDialog("请选择土层标高");
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);
                Application.ShowAlertDialog("请选择qsi");
                PromptSelectionResult re2 = ed.GetSelection(acSelFtr);

                // 如果提示状态是 OK，对象就被选择了 
                if (re1.Status == PromptStatus.OK && re2.Status == PromptStatus.OK)
                {
                    //得到选择集
                    SelectionSet acSSet1 = re1.Value;
                    SelectionSet acSSet2 = re2.Value;
                    //提示选择了多少个对象
                    // Application.ShowAlertDialog("Number of lei selected: " + acSSet1.Count.ToString());
                    ed.WriteMessage("\nNumber of lei selected: " + acSSet1.Count.ToString());
                    ed.WriteMessage("\nNumber of qi selected: " + acSSet2.Count.ToString());
                    // Application.ShowAlertDialog("Number of qi selected: " + acSSet2.Count.ToString());
                    string[] str1 = new string[acSSet1.Count];//用来储存选中的文字
                    double[] lei = new double[acSSet1.Count];//用来储存每一个选择的文字转换来的数字值lei
                    double[] x1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点x坐标
                    double[] y1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点y坐标
                    double[] z1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点z坐标

                    string[] str2 = new string[acSSet2.Count];//用来储存选中的文字
                    double[] qsi = new double[acSSet2.Count];//用来储存每一个选择的文字转换来的数字值qsi
                    double[] x2 = new double[acSSet2.Count];//用来存储每一个选择的文字的选择点x坐标
                    double[] y2 = new double[acSSet2.Count];//用来存储每一个选择的文字的选择点y坐标
                    double[] z2 = new double[acSSet2.Count];//用来存储每一个选择的文字的选择点z坐标

                    double[] ht = new double[acSSet1.Count];

                    // 遍历选择集中的对象 
                    //把选中lei的值和坐标赋予数组当中
                    #region
                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            // 检查以确定返回的 SelectedObject 对象是有效的
                            // 以写的方式打开选择的对象 
                            DBText txt1 = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (txt1 != null)
                            {
                                // 修改对象的颜色
                                txt1.ColorIndex = 1;
                                // 得到所选对象的string以便转为数字用来计算
                                str1[n] = txt1.TextString;

                                int i = str1[n].IndexOf("(");
                                str1[n] = str1[n].Substring(0, i);
                                lei[n] = double.Parse(str1[n]);   //储存深度

                                //得到所选择数字的选择点坐标x，y，z
                                x1[n] = txt1.Position.X;
                                y1[n] = txt1.Position.Y;
                                z1[n] = txt1.Position.Z;

                                ht[n] = txt1.Height;
                                // Application.ShowAlertDialog("lei{" +n+"}="+lei[n].ToString());
                                ed.WriteMessage("\nlei{" + n + "}=" + lei[n].ToString());
                                n = n + 1;

                            }
                            else
                            {
                                Application.ShowAlertDialog("Number of objects selected: 0");
                            }
                        }
                    }
                    #endregion
                    //把选中qsi的值和坐标赋予数组当中

                    #region
                    foreach (SelectedObject acSSObj in acSSet2)
                    {
                        if (acSSObj != null)
                        {
                            // 检查以确定返回的 SelectedObject 对象是有效的
                            // 以写的方式打开选择的对象 
                            DBText txt2 = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (txt2 != null)
                            {
                                // 修改对象的颜色
                                txt2.ColorIndex = 5;
                                // 得到所选对象的string以便转为数字用来计算
                                str2[m] = txt2.TextString;
                                int i = str2[m].IndexOf("=");
                                str2[m] = str2[m].Substring(i + 1);
                                qsi[m] = double.Parse(str2[m]);   //force

                                //得到所选择数字的选择点坐标x，y，z
                                x2[m] = txt2.Position.X;
                                y2[m] = txt2.Position.Y;
                                z2[m] = txt2.Position.Z;
                                // Application.ShowAlertDialog(" qsi{" + m.ToString() + "}=" + qsi[m].ToString());
                                ed.WriteMessage("\nqsi{" + m.ToString() + "}=" + qsi[m].ToString());
                                m = m + 1;
                            }
                            else
                            {
                                Application.ShowAlertDialog("Number of objects selected: 0");
                            }
                        }
                    }
                    #endregion

                    //生成测阻力合力大小
                    if (r < 800)
                        re = 1;
                    else
                        re = Math.Pow(800 / r, 1.0 / 5);

                    for (int i = 0; i < acSSet2.Count; i++)
                    {
                        fsi += Math.Round(Math.PI * r * re * (qsi[i] * (lei[i] - lei[i + 1])) / 1000, 1);
                    }
                    //生成测阻力的位置
                    DBText objText = new DBText();
                    objText.SetDatabaseDefaults();
                    objText.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 2 * ht[1], 0);
                    objText.TextString = "qs:" + fsi.ToString();
                    objText.TextStyleId = db.Textstyle;
                    objText.Height = ht[1];
                    objText.ColorIndex = 3;

                    acBlkTblRec.AppendEntity(objText);
                    acTrans.AddNewlyCreatedDBObject(objText, true);

                    fp = Math.Round(Math.PI * r / 1000 * r / 1000 / 4 * qp, 1);
                    DBText objText1 = new DBText();
                    objText1.SetDatabaseDefaults();
                    objText1.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 4 * ht[1], 0);
                    objText1.TextString = "qp:" + fp.ToString();
                    objText1.TextStyleId = db.Textstyle;
                    objText1.Height = ht[1];
                    objText1.ColorIndex = 3;

                    acBlkTblRec.AppendEntity(objText1);
                    acTrans.AddNewlyCreatedDBObject(objText1, true);

                    pz = Math.Round(fp + fsi, 1);

                    DBText objText2 = new DBText();
                    objText2.SetDatabaseDefaults();
                    objText2.Position = new Autodesk.AutoCAD.Geometry.Point3d(x1[0], y1[0] - 6 * ht[1], 0);
                    objText2.TextString = "Ra:" + pz.ToString();
                    objText2.TextStyleId = db.Textstyle;
                    objText2.Height = ht[1];
                    objText2.ColorIndex = 6;


                    //append the new mtext object to model space
                    acBlkTblRec.AppendEntity(objText2);
                    acTrans.AddNewlyCreatedDBObject(objText2, true);

                }



                //确认修改
                acTrans.Commit();

            }

        }

        //启动命令-批量
        [CommandMethod("getra4")]

        public static void GetRaVersion4()
        {
            // 获得当前文档
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int n = 0;


            //设置过滤器
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);
            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            //启动事务管理器
            using (Transaction acTrans = db.TransactionManager.StartTransaction())
            {

                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // 以写方式打开模型空间块表记录   Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Application.ShowAlertDialog("请选择文字");
                PromptSelectionResult re1 = ed.GetSelection(acSelFtr);


                // 如果提示状态是 OK，对象就被选择了 
                if (re1.Status == PromptStatus.OK)
                {
                    //得到选择集
                    SelectionSet acSSet1 = re1.Value;

                    //提示选择了多少个对象
                    // Application.ShowAlertDialog("Number of lei selected: " + acSSet1.Count.ToString());
                    ed.WriteMessage("\nNumber of lei selected: " + acSSet1.Count.ToString());

                    // Application.ShowAlertDialog("Number of qi selected: " + acSSet2.Count.ToString());
                    string[] str1 = new string[acSSet1.Count];//用来储存选中的文字
                    double[] lei = new double[acSSet1.Count];//用来储存每一个选择的文字转换来的数字值lei
                    double[] x1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点x坐标
                    double[] y1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点y坐标
                    double[] z1 = new double[acSSet1.Count];//用来存储每一个选择的文字的选择点z坐标



                    // 遍历选择集中的对象 
                    //把选中lei的值和坐标赋予数组当中
                    #region
                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            // 检查以确定返回的 SelectedObject 对象是有效的
                            // 以写的方式打开选择的对象 
                            DBText txt1 = acTrans.GetObject(acSSObj.ObjectId, OpenMode.ForWrite) as DBText;
                            if (txt1 != null)
                            {
                                // 修改对象的颜色
                                txt1.ColorIndex = 1;
                                // 得到所选对象的string以便转为数字用来计算
                                str1[n] = txt1.TextString;
                                lei[n] = double.Parse(str1[n]);   //储存深度

                                //得到所选择数字的选择点坐标x，y，z
                                x1[n] = txt1.Position.X;
                                y1[n] = txt1.Position.Y;
                                z1[n] = txt1.Position.Z;
                                // Application.ShowAlertDialog("lei{" +n+"}="+lei[n].ToString());
                                ed.WriteMessage("\nlei{" + n + "}=" + lei[n].ToString());
                                n = n + 1;

                            }
                            else
                            {
                                Application.ShowAlertDialog("Number of objects selected: 0");
                            }

                        }
                    }
                    #endregion
                    //把选中qsi的值和坐标赋予数组当中


                    int min;
                    for (int i = 0; i < y1.Length; i++)
                    {
                        min = i;
                        for (int j = i + 1; j < y1.Length; j++)
                        {
                            if (y1[j] < y1[min])
                                min = j;
                        }
                        double t = y1[min];
                        double t1 = lei[min];
                        y1[min] = y1[i];
                        lei[min] = lei[i];
                        y1[i] = t;
                        lei[i] = t1;

                        ed.WriteMessage("\n" + y1[i] + "," + lei[i]);
                    }

                }



                //确认修改
                acTrans.Commit();

            }

        }
        /// <summary>
        /// 选择排序
        /// </summary>
        public class SelectionSorter
        {
            // public enum comp {COMP_LESS,COMP_EQUAL,COMP_GRTR};
            private int min;
            // private int m=0;
            public int[] Sort(int[] list)
            {

                for (int i = 0; i < list.Length - 1; i++)
                {
                    min = i;
                    for (int j = i + 1; j < list.Length; j++)
                    {
                        if (list[j] < list[min])
                            min = j;
                    }
                    int t = list[min];
                    list[min] = list[i];
                    list[i] = t;

                    // Console.WriteLine("{0}",list[i]);
                }
                return list;

            }
        }
    }
}
