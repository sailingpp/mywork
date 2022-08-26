using Autodesk.AutoCAD.ApplicationServices;
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
    public class KangfuDemo
    {
        Database db = HostApplicationServices.WorkingDatabase;
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        public static double Rt;
        [CommandMethod("kf")]
        public void Kangfudemo()
        {
            List<DBText> GList = new List<DBText>();
            List<DBText> NwList = new List<DBText>();
            ed.WriteMessage(" 依据规范: 建筑工程抗浮技术标准(JGJ476-2019)第3.0.3条，第6.4.1~6.4.3条 \n");
            ed.WriteMessage(" 建筑地基基础设计规范(GB50007-2011)第5.4.3条    \n");
            ed.WriteMessage("  *  γ0,w=1.1设计: 抗浮设计的结构重要性系数\n");
            ed.WriteMessage("  * [Kw]=1.1:  抗浮稳定安全系数\n ");
            ed.WriteMessage("  * Rt:  抗拔桩(锚杆)抗拉承载力\n ");
            ed.WriteMessage("  *  Nw:    水浮力(kN)/r\n");
            ed.WriteMessage("  *  需要抗拔桩数量N=(γ0,w *[Kw]*Nw-G)/Rt/\n");
            PromptDoubleOptions pdOpts = new PromptDoubleOptions("\n请输入单桩承载力Rt(KN)\n ");
            PromptDoubleResult pdres = ed.GetDouble(pdOpts);
            Rt = pdres.Value;
            ed.WriteMessage("请选择yjk抗浮对象");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status == PromptStatus.OK)
                {
                    ed.WriteMessage("选择成功\n");
                    SelectionSet acSSet1 = psr.Value;
                    foreach (SelectedObject acSSObj in acSSet1)
                    {
                        if (acSSObj != null)
                        {
                            Entity ent = acSSObj.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                            if (ent is DBText && ent.Layer == "不符合要求的计算结果")
                            {
                                DBText dbtext = (DBText)ent;
                                if (dbtext.TextString.Contains("G"))
                                {
                                    GList.Add(dbtext);
                                    dbtext.ColorIndex = 3;
                                }
                                else if (dbtext.TextString.Contains("Nw"))
                                {
                                    NwList.Add(dbtext);
                                    dbtext.ColorIndex = 4;
                                }

                            }

                        }
                    }
                }
                for (int i = 0; i < GList.Count; i++)
                {
                    db.AddToModelSpace(ProduceN(GList[i], NwList[i]));
                }
                ed.WriteMessage("图示为需要抗拔桩的个数");

                trans.Commit();
            }
        }
        public static double GetRideSign(DBText dbstr, string sign)
        {
            double d;
            int i = dbstr.TextString.LastIndexOf(sign);
            string str = dbstr.TextString.Substring(i + 1);
            return d = Convert.ToDouble(str);
        }
        public static double GetKw(double g, double nw)
        {
            return Math.Round((nw * 1.1 * 1.1 - g) / Rt, 2);
        }
        public static DBText ProduceN(DBText G, DBText Nw)
        {
            DBText n = new DBText();
            n.Height = G.Height * 2;
            n.Position = Nw.Position + new Vector3d(0, -1000, 0);
            n.Layer = G.Layer;
            n.ColorIndex = 6;
            n.TextString = GetKw(GetRideSign(G, "="), GetRideSign(Nw, "=")) + "";
            return n;
        }

    }
}
