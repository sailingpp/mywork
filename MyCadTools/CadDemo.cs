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
    public class CadDemo
    {
        [CommandMethod("Sailing")]
        public void CallSaiing()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            db.WriteMesage("功能和实现的命令如下：\r\n ");
            db.WriteMesage("********图层组********\r\n ");
            db.WriteMesage("改变指定图层的颜色：changelayercolor\r\n ");
            db.WriteMesage("改变指定图层的名称：changelayername\r\n ");
            db.WriteMesage("增加图层，并设定图层的名称和颜色：addlayer\r\n ");
            db.WriteMesage("按颜色选择实体：selectbycolor\r\n ");
            db.WriteMesage("按图层选择实体：selectbylayer\r\n ");
            db.WriteMesage("列出图层的名称和图层的颜色：getalllayers\r\n ");
            db.WriteMesage("在指定点插入块：insertblock\r\n ");
            db.WriteMesage("********结构组********\r\n ");
            db.WriteMesage("结构图层初始化：ini\r\n ");
            db.WriteMesage("画单桩块：makepile\r\n ");
            db.WriteMesage("画单桩承台块：makeonecap or makesinglecap\r\n ");
            db.WriteMesage("画两桩承台块：maketwocap\r\n ");
            db.WriteMesage("鼠标监视：monitor\r\n ");
        }

        [CommandMethod("tt")]
        public void Test1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            Line line = new Line(new Point3d(100, 100, 0), Point3d.Origin);
            Line line2 = new Line(new Point3d(200, 100, 0), Point3d.Origin);
            Line line3 = new Line(new Point3d(300, 100, 0), Point3d.Origin);
            Line line4 = new Line(new Point3d(500, 100, 0), Point3d.Origin);
            Line line5 = new Line(new Point3d(600, 100, 0), Point3d.Origin);
            DBText text = new DBText();
            text.TextString = "hello";
            text.Height = 100;
            text.Position = Point3d.Origin;
            text.TextStyleId = db.AddTxtStyle("TSSD_Norm", "tssdeng", "hztxt");
            db.AddToModelSpace(line, line2, text);
            db.InsertEntity(line4, "pp", 1);
            ObjectId oid = db.InsertDbtext("hello", "test", Point3d.Origin, 100, 0.6, 1);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                DBText dbtext = oid.GetObject(OpenMode.ForRead) as DBText;
                MyEntity ent = new MyEntity(dbtext);
                ed.WriteMessage(ent.LayerName);
                trans.Commit();
            }
        }

        [CommandMethod("te")]
        public void Test2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            db.ChangeLayerNameTool("1", "2");
            db.ChangeLayerColorTool("hello", 3);
            db.ChangeTxtStyle("TSSD_Rein", "tssdeng2");

        }

        [CommandMethod("ChangeColor")]
        public void ChangeColorDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            db.SelectEntityByColor(1);

        }

        [CommandMethod("ChangeAllColor")]
        public void ChangeAllColorDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("无需选择，用来改变所有实体颜色\r\n");
            PromptIntegerOptions pio = new PromptIntegerOptions("请输入颜色值:\r\n");
            PromptIntegerResult pir = ed.GetInteger(pio);
            int colorIndex = pir.Value;
            db.ChangeAllEntityByColor(colorIndex);

        }

        [CommandMethod("ChangeLayerName")]
        public void ChangeLayerNameDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("用来改变已有图层的名字\r\n");

            PromptResult opr = ed.GetString("请输入旧的图层名：\r\n");

            string oldName = opr.StringResult;

            PromptResult npr = ed.GetString("请输入新的图层名：\r\n");

            string newName = npr.StringResult;
            db.ChangeLayerNameTool(oldName, newName);

        }

        [CommandMethod("SelectbyColor")]
        public void SelectAllbyColorDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("根据颜色选择实体\r\n");
            PromptIntegerOptions pio = new PromptIntegerOptions("请输入颜色值:\r\n");
            PromptIntegerResult pir = ed.GetInteger(pio);
            int colorIndex = pir.Value;
            db.SelectAllEntityByColor(colorIndex);
        }

        [CommandMethod("SelectbyLayer")]
        public void SelectAllbyLayerDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("根据图层选择实体\r\n");
            PromptResult pr = ed.GetString("请输入图层名：\r\n");
            string layerName = pr.StringResult;
            db.SelectAllEntityByLayer(layerName);
        }

        [CommandMethod("GetAllLayers")]
        public void GetAllLayerDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            Dictionary<string, int> list = db.GetAllLayerName();
            ed.WriteMessage("总共有图层" + list.Count + "个\r\n");
            ed.WriteMessage("图层名----图层颜色号，列如下：\r\n");
            foreach (KeyValuePair<string, int> item in list)
            {
                ed.WriteMessage(item.Key + "----" + item.Value + "\r\n");
            }

        }

        [CommandMethod("AddBlock")]
        public void AddBlockDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            Line line1 = new Line(new Point3d(100, 100, 0), Point3d.Origin);
            Line line2 = new Line(new Point3d(200, 100, 0), Point3d.Origin);
            Line line3 = new Line(new Point3d(300, 100, 0), Point3d.Origin);
            Line line4 = new Line(new Point3d(500, 100, 0), Point3d.Origin);
            Line line5 = new Line(new Point3d(600, 100, 0), Point3d.Origin);
            List<Entity> ents = new List<Entity>() { line1, line2, line3 };
            db.AddBlock("hello", ents);
        }

        [CommandMethod("InsertBBlock")]
        public void InsertBlock()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            db.InsertBlock("hello", "pp", Point3d.Origin, 0);
        }

        [CommandMethod("CheckMark")]
        public void CheckMarkDemo()
        {
            /*指定点生成dbtext*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptPointOptions ppo = new PromptPointOptions("选择点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            Point3d pt3d = ppr.Value;
            Circle circle = new Circle();
            circle.Radius = 1000;
            Line lineX = new Line();
            Line lineY = new Line();
            lineX.StartPoint = circle.Center - new Vector3d(circle.Radius, 0, 0);
            lineX.EndPoint = circle.Center + new Vector3d(circle.Radius, 0, 0);
            lineY.StartPoint = circle.Center - new Vector3d(0, circle.Radius, 0);
            lineY.EndPoint = circle.Center + new Vector3d(0, circle.Radius, 0);
            DBText dbtext = db.MakeDbtext("jiaodui", "ppt", 500, 0.7, 1);
            List<Entity> entList = new List<Entity> { circle, lineX, lineY, dbtext };
            db.AddBlock("mark", entList);
            db.InsertBlock("mark", "pp", pt3d, 0);
        }

        [CommandMethod("Monitor")]
        public static void StartMonitorDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ed.PointMonitor += ShowMouseMonitorMessage;
        }

        /// <summary>
        /// 鼠标监视停留消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ShowMouseMonitorMessage(object sender, PointMonitorEventArgs e)
        {
            /*鼠标监视执行事件*/
            Editor editor = (Editor)sender;
            Document doc = editor.Document;
            Database db = doc.Database;
            FullSubentityPath[] paths = e.Context.GetPickedEntities();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                if (paths.Length > 0)
                {
                    FullSubentityPath path = paths[0];
                    Entity ent = path.GetObjectIds()[0].GetObject(OpenMode.ForRead) as Entity;
                    if (ent != null)
                    {
                        e.AppendToolTipText("图层的名字:" + ent.Layer + ",图层的颜色：" + db.GetLayerRecord(ent).Color.ColorIndex.ToString() + ",实体的颜色：" + ent.ColorIndex.ToString());
                    }
                }
            }

        }

        [CommandMethod("ChangeSingleText")]
        public void ChangeSingleTextDemo()
        {
            /*改变文字的内容*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择文字1:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK)
                {
                    DBText text = (DBText)per.ObjectId.GetObject(OpenMode.ForWrite);
                    text.ChangeText("看我有没有改变你");
                }
                trans.Commit();
            }
        }

        [CommandMethod("CheckTuMing")]
        public void CheckTuming()
        {
            /*检查图名与图框名*/
            bool ready = false;
            string tuming;
            string tukuang;
            double tumingheight;
            double tukuangheight;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            List<DBText> tumingList = new List<DBText>();
            List<DBText> tukuangList = new List<DBText>();
            PromptEntityOptions peotuming = new PromptEntityOptions("请选择图中图名\r\n");
            PromptEntityResult pertuming = ed.GetEntity(peotuming);
            PromptEntityOptions peotukuang = new PromptEntityOptions("请选择图框中图名\r\n");
            PromptEntityResult pertukuang = ed.GetEntity(peotukuang);
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    DBText tumingDBText = pertuming.ObjectId.GetObject(OpenMode.ForRead) as DBText;
                    DBText tukuangDBText = pertukuang.ObjectId.GetObject(OpenMode.ForRead) as DBText;
                    tuming = tumingDBText.Layer;
                    tumingheight = tumingDBText.Height;
                    tukuang = tukuangDBText.Layer;
                    tukuangheight = tukuangDBText.Height;

                    trans.Commit();
                }

                TypedValue[] tvalue = new TypedValue[]
                     {
                        new TypedValue((int)DxfCode.Operator, "<or"),
                        new TypedValue((int)DxfCode.LayerName,tuming),
                        new TypedValue((int)DxfCode.Text,"DBTEXT"),
                        new TypedValue((int)DxfCode.LayerName,tukuang),
                        new TypedValue((int)DxfCode.Operator, "or>"),
                   };
                ed.WriteMessage("请选择:\r\n");
                PromptSelectionOptions pso = new PromptSelectionOptions();
                SelectionFilter filter = new SelectionFilter(tvalue);
                PromptSelectionResult psr = ed.GetSelection(pso, filter);

                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    SelectionSet sset = psr.Value;
                    if (psr.Status == PromptStatus.OK)
                    {
                        foreach (SelectedObject item in sset)
                        {
                            Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                            if (ent is DBText)
                            {
                                DBText dbtext = (DBText)ent;
                                if (dbtext.Height == tumingheight && dbtext.Layer == tuming)
                                {
                                    tumingList.Add(dbtext);
                                }
                                else if (dbtext.Height == tukuangheight && dbtext.Layer == tukuang)
                                {
                                    tukuangList.Add(dbtext);
                                }
                                else
                                {
                                    //dbtext.Highlight();
                                }
                                ready = true;
                            }
                            else
                            {
                                //ent.Highlight(); 
                            }
                        }
                        if (ready)
                        {
                            for (int i = 0; i < tumingList.Count; i++)
                            {
                                db.ChangeText(tukuangList[i], tumingList[i].TextString);
                            }
                            ed.WriteMessage("图名统一成功！\r\n");
                        }
                    }
                    trans.Commit();
                }
            }
            catch
            {
                ed.WriteMessage("请重新操作!\r\n");
            }



        }

        [CommandMethod("OrderbyContent")]
        public void OrderDbtextByContent()
        {
            /*dbtext按内容大小排序*/
            int n = 0;
            bool ready = false;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            List<DBText> dbtextList = new List<DBText>();
            TypedValue[] tvalue = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"TEXT"),
            };
            ed.WriteMessage("请选择文字:\r\n");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            SelectionFilter filter = new SelectionFilter(tvalue);
            PromptSelectionResult psr = ed.GetSelection(pso, filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    foreach (SelectedObject item in sset)
                    {
                        Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                        if (ent is DBText)
                        {
                            DBText dbtext = (DBText)ent;
                            dbtext.ColorIndex = 1;
                            dbtextList.Add(dbtext);
                        }
                    }
                    ready = true;
                }
                else
                {
                    return;
                }
                if (ready)
                {
                    var query = from o in dbtextList orderby Convert.ToInt32(o.TextString) select o;
                    foreach (var item in query)
                    {
                        db.InsertDbtext(n.ToString(), "pp", item.Position, 400, 0.7, 1);
                        n++;
                    }
                }
                else
                {
                    return;
                }
                trans.Commit();
            }
        }

        [CommandMethod("OrderbyY")]
        public void OrderDbtextByY()
        {
            /*dbtext按y轴坐标排序*/
            int n = 0;
            bool ready = false;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            List<DBText> dbtextList = new List<DBText>();
            TypedValue[] tvalue = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"TEXT"),
            };
            ed.WriteMessage("请选择文字:\r\n");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            SelectionFilter filter = new SelectionFilter(tvalue);
            PromptSelectionResult psr = ed.GetSelection(pso, filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    foreach (SelectedObject item in sset)
                    {
                        Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                        if (ent is DBText)
                        {
                            DBText dbtext = (DBText)ent;
                            dbtext.ColorIndex = 1;
                            dbtextList.Add(dbtext);
                        }
                    }
                    ready = true;
                }
                else
                {
                    return;
                }
                if (ready)
                {
                    var query = from o in dbtextList orderby o.Position.Y select o;
                    foreach (var item in query)
                    {
                        db.InsertDbtext(n.ToString(), "pp", item.Position, 400, 0.7, 1);
                        n++;
                    }
                }
                else
                {
                    return;
                }
                trans.Commit();
            }
        }

        [CommandMethod("OrderbyX")]
        public void OrderDbtextByX()
        {
            /*dbtext按x坐标排序*/
            int n = 0;
            bool ready = false;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            List<DBText> dbtextList = new List<DBText>();
            TypedValue[] tvalue = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"TEXT"),
            };
            ed.WriteMessage("请选择文字:\r\n");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            SelectionFilter filter = new SelectionFilter(tvalue);
            PromptSelectionResult psr = ed.GetSelection(pso, filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    foreach (SelectedObject item in sset)
                    {
                        Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                        if (ent is DBText)
                        {
                            DBText dbtext = (DBText)ent;
                            dbtext.ColorIndex = 1;
                            dbtextList.Add(dbtext);
                        }
                    }
                    ready = true;
                }
                else
                {
                    return;
                }
                if (ready)
                {
                    var query = from o in dbtextList orderby o.Position.X select o;
                    foreach (var item in query)
                    {
                        db.InsertDbtext(n.ToString(), "pp", item.Position, 400, 0.7, 1);
                        n++;
                    }
                }
                else
                {
                    return;
                }
                trans.Commit();
            }
        }

        [CommandMethod("Mulu")]
        public void GetMulu()
        {
            /*生成目录*/
            bool ready = false;
            string tuming;
            string xuhao;
            double tumingheight;
            double xuhaoheight;
            int n = 0;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            Dictionary<int, DBText> tumingDic = new Dictionary<int, DBText>();
            List<DBText> tumingList = new List<DBText>();
            List<DBText> xuhaoList = new List<DBText>();
            PromptEntityOptions peotuming = new PromptEntityOptions("请拾取匹配图框中的图名\r\n");
            PromptEntityResult pertuming = ed.GetEntity(peotuming);
            PromptEntityOptions peoxuhao = new PromptEntityOptions("请拾取匹配图框中序号\r\n");
            PromptEntityResult perxuhao = ed.GetEntity(peoxuhao);
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    DBText tumingDBText = pertuming.ObjectId.GetObject(OpenMode.ForRead) as DBText;
                    DBText tukuangDBText = perxuhao.ObjectId.GetObject(OpenMode.ForRead) as DBText;
                    tuming = tumingDBText.Layer;
                    tumingheight = tumingDBText.Height;
                    xuhao = tukuangDBText.Layer;
                    xuhaoheight = tukuangDBText.Height;
                    trans.Commit();
                }

                TypedValue[] tvalue = new TypedValue[]
                   {
                        new TypedValue((int)DxfCode.Operator, "<or"),
                        new TypedValue((int)DxfCode.LayerName,tuming),
                        new TypedValue((int)DxfCode.Start,"TEXT"),
                        new TypedValue((int)DxfCode.LayerName,xuhao),
                        new TypedValue((int)DxfCode.Operator, "or>"),
                   };
                ed.WriteMessage("请选择图纸范围:\r\n");
                PromptSelectionOptions pso = new PromptSelectionOptions();
                SelectionFilter filter = new SelectionFilter(tvalue);
                PromptSelectionResult psr = ed.GetSelection(pso, filter);
                PromptPointOptions ppo = new PromptPointOptions("请选择要放入图中目录文字的点:\r\n");
                PromptPointResult ppr = ed.GetPoint(ppo);
                Point3d position = ppr.Value;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    SelectionSet sset = psr.Value;
                    if (psr.Status == PromptStatus.OK)
                    {
                        foreach (SelectedObject item in sset)
                        {
                            DBText dbtext = item.ObjectId.GetObject(OpenMode.ForWrite) as DBText;

                            if (dbtext.Height == tumingheight && dbtext.Layer == tuming)
                            {
                                tumingList.Add(dbtext);
                            }
                            else if (dbtext.Height == xuhaoheight && dbtext.Layer == xuhao)
                            {
                                xuhaoList.Add(dbtext);
                            }
                            else { }
                            ready = true;
                        }
                        if (ready)
                        {
                            for (int i = 0; i < xuhaoList.Count; i++)
                            {
                                for (int j = 0; j < tumingList.Count; j++)
                                {
                                    if (xuhaoList[i].Position.DistanceTo(tumingList[j].Position) < 4000)
                                    {
                                        tumingDic.Add(Convert.ToInt32(xuhaoList[i].TextString), tumingList[j]);
                                    }
                                }
                            }

                            var query = from o in tumingDic
                                        orderby tumingDic.Keys
                                        select o;

                            foreach (KeyValuePair<int, DBText> item in query)
                            {
                                db.InsertDbtext("结施" + item.Key, "图纸目录", position + new Vector3d(0, n * 700, 0), 500, 0.7, 7);
                                db.InsertDbtext(item.Value.TextString, "图纸目录", position + new Vector3d(2300, n * 700, 0), 500, 0.7, 7);
                                n++;
                            }
                            ed.WriteMessage("目录生成成功！\r\n");
                        }
                        else { }
                    }
                    trans.Commit();
                }
            }
            catch
            {
                ed.WriteMessage("请重新选择;");
            }
        }

        [CommandMethod("MakePile")]
        public void MakePileDemo()
        {
            /*生成单桩*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptIntegerOptions pio = new PromptIntegerOptions("请输入桩直径(mm):\r\n");
            PromptIntegerResult pir = ed.GetInteger(pio);
            PromptPointOptions ppo = new PromptPointOptions("选择要插入的点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            Point3d pt3d = ppr.Value;
            int circleRadius = pir.Value / 2;
            db.AddBlock("D" + 2 * circleRadius + "pile", db.DrawPile(circleRadius, pt3d));
            db.InsertBlock("D" + 2 * circleRadius + "pile", "pile", pt3d, 0);
        }

        [CommandMethod("MakeSingleCap")]
        public void MakeSingleCapDemo()
        {
            /*生成单桩承台*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptIntegerOptions pio = new PromptIntegerOptions("请输入桩直径(mm):\r\n");
            PromptIntegerResult pir = ed.GetInteger(pio);
            PromptPointOptions ppo = new PromptPointOptions("选择要插入的点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            Point3d pt3d = ppr.Value;
            Circle circle = new Circle();
            circle.Radius = pir.Value / 2;
            Line lineX = new Line();
            Line lineY = new Line();
            lineX.StartPoint = circle.Center - new Vector3d(circle.Radius, 0, 0);
            lineX.EndPoint = circle.Center + new Vector3d(circle.Radius, 0, 0);
            lineY.StartPoint = circle.Center - new Vector3d(0, circle.Radius, 0);
            lineY.EndPoint = circle.Center + new Vector3d(0, circle.Radius, 0);
            Point3dCollection vertices = new Point3dCollection();
            vertices.Add(circle.Center + 2 * new Vector3d(-circle.Radius, circle.Radius, 0));
            vertices.Add(circle.Center + 2 * new Vector3d(circle.Radius, circle.Radius, 0));
            vertices.Add(circle.Center + 2 * new Vector3d(circle.Radius, -circle.Radius, 0));
            vertices.Add(circle.Center + 2 * new Vector3d(-circle.Radius, -circle.Radius, 0));
            Polyline3d pl = new Polyline3d(Poly3dType.SimplePoly, vertices, true);
            List<Entity> entList = new List<Entity> { circle, lineX, lineY, pl };
            db.AddBlock("D" + circle.Radius + "cap", entList);
            db.InsertBlock("D" + circle.Radius + "cap", "cap", pt3d, 0);
        }

        [CommandMethod("MakeOneCap")]
        public void MakeOneCapDemo()
        {
            /*生成单桩承台*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptIntegerOptions pio = new PromptIntegerOptions("请输入桩直径(mm):\r\n");
            PromptIntegerResult pir = ed.GetInteger(pio);
            PromptPointOptions ppo = new PromptPointOptions("选择要插入的点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            Point3d pt3d = ppr.Value;
            db.MakeEntityBlock(pir.Value.ToString(), "1cap" + pir.Value, pt3d, 0, db.DrawOneCap(pir.Value / 2, pt3d));
        }

        [CommandMethod("MakeTwoCap")]
        public void MakeTwoCapDemo()
        {
            /*生成两桩承台*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptIntegerOptions pio = new PromptIntegerOptions("请输入桩直径(mm):\r\n");
            PromptIntegerResult pir = ed.GetInteger(pio);
            PromptPointOptions ppo = new PromptPointOptions("选择要插入的点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            Point3d pt3d = ppr.Value;
            db.MakeEntityBlock(pir.Value.ToString(), "2cap" + pir.Value, pt3d, 0, db.DrawTwoCap(pir.Value / 2, pt3d));
        }

        [CommandMethod("Ini")]
        public void IniStructure()
        {
            /*结构图层初始化*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            db.AddLayerTool("后浇带", 1);
            db.AddLayerTool("后浇带dim", 1);
            db.AddLayerTool("Colloum", 7);
            db.AddLayerTool("ColloumDim", 7);
            db.AddLayerTool("Beam", 4);
            db.AddLayerTool("BeamDim", 4);
            db.AddLayerTool("Plate", 3);
            db.AddLayerTool("PlateDim", 3);
            db.AddLayerTool("Wall", 2);
            db.AddLayerTool("WallDim", 2);
            db.AddLayerTool("Detail", 5);
            db.AddLayerTool("DetailDim", 5);
        }

        [CommandMethod("JiaoDian")]
        public void GetJiaoDianDemo()
        {
            /*获取直线的交点,提示错误找不到方法
             */
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择第一跟线\r\n");
            PromptEntityOptions peo1 = new PromptEntityOptions("请选择第二跟线\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            PromptEntityResult per1 = ed.GetEntity(peo1);
            Point3dCollection points = new Point3dCollection();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK && per1.Status == PromptStatus.OK)
                {
                    Polyline line1 = per.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
                    Polyline line2 = per1.ObjectId.GetObject(OpenMode.ForWrite) as Polyline;
                    line1.IntersectWith(line2, Intersect.OnBothOperands, points, 0, 0);

                    ed.WriteMessage(points.Count.ToString());
                }
                else
                {
                    return;
                }
                trans.Commit();
            }
        }

        [CommandMethod("GetCentroids")]
        public void GetCentroidsDemo()
        {
            /*获取形心*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择多段线:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            double area = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK)
                {
                    Polyline pl = (Polyline)per.ObjectId.GetObject(OpenMode.ForRead);

                    Point3d[] pts = new Point3d[pl.NumberOfVertices];
                    for (int i = 0; i < pl.NumberOfVertices; i++)
                    {
                        Point3d pt = pl.GetPoint3dAt(i);
                        pts[i] = pt;
                    }

                    for (int i = 0; i < pts.Length; i++)
                    {
                        ed.WriteMessage("定点" + i + "x:" + pts[i].X + "y:" + pts[i].Y + "\r\n");
                    }
                    for (int i = 0; i < pts.Length - 1; i++)
                    {
                        area += pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                    }
                    area += pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y;
                    area = area / 2;
                    ed.WriteMessage("面积是：" + Math.Abs(area).ToString() + "\r\n");
                    ed.WriteMessage("长度是：" + Math.Abs(pl.Length).ToString() + "\r\n");

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
                    ed.WriteMessage("形心点坐标：" + "x:" + center.X + ",y:" + center.Y + "\r\n");

                    DBPoint dbpoint = new DBPoint(center);
                    dbpoint.ColorIndex = 1;
                    db.Pdmode = 3;
                    db.Pdsize = 100;
                    db.AddToModelSpace(dbpoint);
                }

                trans.Commit();

            }
        }

        [CommandMethod("GetCen")]
        public void GetCentroidsDemo2()
        {
            /*获取形心*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择多段线:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            double area = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK)
                {
                    Polyline pl = (Polyline)per.ObjectId.GetObject(OpenMode.ForRead);

                    Point3d[] pts = new Point3d[pl.NumberOfVertices];
                    for (int i = 0; i < pl.NumberOfVertices; i++)
                    {
                        Point3d pt = pl.GetPoint3dAt(i);
                        pts[i] = pt;
                    }

                    for (int i = 0; i < pts.Length; i++)
                    {
                        ed.WriteMessage("定点" + i + "x:" + pts[i].X + "y:" + pts[i].Y + "\r\n");
                    }
                    for (int i = 0; i < pts.Length - 1; i++)
                    {
                        area += pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                    }
                    area += pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y;
                    area = area / 2;
                    ed.WriteMessage("面积是：" + Math.Abs(area).ToString() + "\r\n");
                    ed.WriteMessage("长度是：" + Math.Abs(pl.Length).ToString() + "\r\n");

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
                    ed.WriteMessage("形心点坐标：" + "x:" + center.X + ",y:" + center.Y + "\r\n");

                    DBPoint dbpoint = new DBPoint(center);
                    dbpoint.ColorIndex = 1;
                    db.Pdmode = 3;
                    db.Pdsize = 100;
                    db.AddToModelSpace(dbpoint);
                }

                trans.Commit();

            }
        }

        [CommandMethod("GetCens")]
        public void GetCentroidsDemo3()
        {
            /*获取形心*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            PromptSelectionOptions pso = new PromptSelectionOptions();
            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start, "*POLYLINE")
            };
            SelectionFilter filter = new SelectionFilter(tv);
            PromptSelectionResult psr = ed.GetSelection(pso, filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status != PromptStatus.OK) return;
                SelectionSet sset = psr.Value;
                if (sset != null)
                {
                    foreach (ObjectId item in sset.GetObjectIds())
                    {
                        Polyline pl = (Polyline)item.GetObject(OpenMode.ForRead);
                        MyPolyline mpl = new MyPolyline(pl);
                        mpl.MakeCentroids();
                    }
                }
                trans.Commit();
            }
        }

        [CommandMethod("GetVertex")]
        public void GetVertexDemo()
        {
            /*获取定点*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择多段线:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            double area = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK)
                {
                    Polyline pl = (Polyline)per.ObjectId.GetObject(OpenMode.ForRead);

                    Point3d[] pts = new Point3d[pl.NumberOfVertices];
                    for (int i = 0; i < pl.NumberOfVertices; i++)
                    {
                        Point3d pt = pl.GetPoint3dAt(i);
                        pts[i] = pt;
                    }

                    for (int i = 0; i < pts.Length; i++)
                    {
                        ed.WriteMessage("定点" + i + "x:" + pts[i].X + "y:" + pts[i].Y + "\r\n");
                    }
                    for (int i = 0; i < pts.Length - 1; i++)
                    {
                        area += pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                    }
                    area += pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y;
                    area = area / 2;
                    ed.WriteMessage("面积是：" + Math.Abs(area).ToString() + "\r\n");
                    ed.WriteMessage("长度是：" + Math.Abs(pl.Length).ToString() + "\r\n");

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
                    ed.WriteMessage("形心点坐标：" + "x:" + center.X + ",y:" + center.Y + "\r\n");

                    DBPoint dbpoint = new DBPoint(center);
                    dbpoint.ColorIndex = 1;
                    db.Pdmode = 3;
                    db.Pdsize = 100;
                    db.AddToModelSpace(dbpoint);
                }

                trans.Commit();

            }
        }

        [CommandMethod("GetAreaDemo")]
        public void GetAreaDemo()
        {
            /*获取面积*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择多段线:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            double area = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK)
                {
                    Polyline pl = (Polyline)per.ObjectId.GetObject(OpenMode.ForRead);

                    Point3d[] pts = new Point3d[pl.NumberOfVertices];
                    for (int i = 0; i < pl.NumberOfVertices; i++)
                    {
                        Point3d pt = pl.GetPoint3dAt(i);
                        pts[i] = pt;
                    }

                    for (int i = 0; i < pts.Length; i++)
                    {
                        ed.WriteMessage("定点" + i + "x:" + pts[i].X + "y:" + pts[i].Y + "\r\n");
                    }
                    for (int i = 0; i < pts.Length - 1; i++)
                    {
                        area += pts[i].X * pts[i + 1].Y - pts[i + 1].X * pts[i].Y;
                    }
                    area += pts[pts.Length - 1].X * pts[0].Y - pts[0].X * pts[pts.Length - 1].Y;
                    area = area / 2;
                    ed.WriteMessage("面积是：" + Math.Abs(area).ToString() + "\r\n");
                    ed.WriteMessage("长度是：" + Math.Abs(pl.Length).ToString() + "\r\n");

                }

                trans.Commit();

            }
        }

        [CommandMethod("GetArea")]
        public void GetAreaDemo2()
        {
            /*获取面积*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择多段线:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK)
                {
                    Polyline pl = (Polyline)per.ObjectId.GetObject(OpenMode.ForRead);

                    MyPolyline mpl = new MyPolyline(pl);
                    ed.WriteMessage("面积是：" + Math.Abs(mpl.Area).ToString() + "\r\n");
                    ed.WriteMessage("长度是：" + Math.Abs(mpl.Length).ToString() + "\r\n");
                }
                trans.Commit();
            }
        }

        [CommandMethod("SelectSameent")]
        public void SelectSameEntity()
        {
            /*按实体的属性选择具有相同属性的实体比如图层*/
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            List<Entity> entityList = new List<Entity>();
            PromptEntityOptions peo = new PromptEntityOptions("请拾取匹配实体\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                entityList = db.SelectEntitysByProperty(per.ObjectId);
                ed.WriteMessage("选择成功!\r\n");
                trans.Commit();
            }
        }

        [CommandMethod("pp")]
        public void ShowStructureBox()
        {
            using (StructureBox form = new StructureBox())
            {
                form.ShowInTaskbar = false;
                Application.ShowModalDialog(form);
                if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
                    Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n" + form.Text);
            }
        }

        [CommandMethod("duanli")]
        public void DLDemo()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] value = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"TEXT"),
                new TypedValue((int)DxfCode.LayerName,"dsptext_walledge"),
            };
            SelectionFilter filter = new SelectionFilter(value);
            PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(filter);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (SelectedObject item in psr.Value)
                {
                    DBText dbtext = item.ObjectId.GetObject(OpenMode.ForWrite) as DBText;
                    DBText text = new DBText();
                    text.TextString = ReplaceWords(dbtext.TextString);
                    text.Position = dbtext.Position;
                    text.Height = dbtext.Height;
                    text.Layer = dbtext.Layer;
                    text.WidthFactor = dbtext.WidthFactor;
                    db.AddTextStyle("dsptext_walledge", "yjkeng", "yjkchn");
                    dbtext.Erase();
                    db.AddToModelSpace(text);
                }
                trans.Commit();
            }
        }

        public static string ReplaceWords(string input)
        {
            if (input.Contains("%"))
            {
                string[] result = input.Split('-');
                string[] re = result[0].Split('(');
                string output = re[0] + "(" + "Asv" + re[1] + "-" + result[1];
                return output;
            }
            else
            {
                return input;
            }

        }

        [CommandMethod("AddKuoHao")]
        public void AddKuoHao()
        {
            //给文字加上括号
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] values = new TypedValue[] 
            {
                 //new TypedValue ((int)DxfCode.Operator,"<and"),
                 //new TypedValue ((int)DxfCode.LayerName,"set"),
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 //new TypedValue ((int)DxfCode.Operator,"and>"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;
                        if (!text.TextString.Contains("("))
                        {
                            text.TextString = "(" + text.TextString + ")";
                        }
                    }
                }

                trans.Commit();
            }
        }

        [CommandMethod("DelKuoHao")]
        public void DelKuoHao()
        {
            //给文字去括号
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] values = new TypedValue[] 
            {
                 //new TypedValue ((int)DxfCode.Operator,"<and"),
                 //new TypedValue ((int)DxfCode.LayerName,"set"),
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 //new TypedValue ((int)DxfCode.Operator,"and>"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;
                        if (text.TextString.Contains("("))
                        {
                            text.TextString = Regex.Replace(text.TextString, @"(.*\()(.*)(\).*)", "$2"); ;
                        }
                    }
                }

                trans.Commit();
            }
        }

        [CommandMethod("Addqianzhui")]
        public void AddQianZhui()
        {
            //给文字加上前缀
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptStringOptions pso = new PromptStringOptions("请输入想加的前缀：\r\n");
            PromptResult pr = ed.GetString(pso);
            TypedValue[] values = new TypedValue[] 
            {
                 //new TypedValue ((int)DxfCode.Operator,"<and"),
                 //new TypedValue ((int)DxfCode.LayerName,"set"),
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 //new TypedValue ((int)DxfCode.Operator,"and>"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            ed.WriteMessage("请选择要加前缀的对象:\r\n");
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (pr.Status != PromptStatus.OK) return;
                string str = pr.StringResult;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;
                        if (!text.TextString.Contains(str))
                        {
                            text.TextString = str + text.TextString;
                        }
                    }
                }
                trans.Commit();
            }
        }

        [CommandMethod("Addhouzhui")]
        public void AddHouZhui()
        {
            //给文字加上后缀
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptStringOptions pso = new PromptStringOptions("请输入想加的后缀：\r\n");
            PromptResult pr = ed.GetString(pso);
            TypedValue[] values = new TypedValue[] 
            {
                 //new TypedValue ((int)DxfCode.Operator,"<and"),
                 //new TypedValue ((int)DxfCode.LayerName,"set"),
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 //new TypedValue ((int)DxfCode.Operator,"and>"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            ed.WriteMessage("请选择要加后缀的对象:\r\n");
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (pr.Status != PromptStatus.OK) return;
                string str = pr.StringResult;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;
                        if (!text.TextString.Contains(str))
                        {
                            text.TextString = text.TextString + str;
                        }
                    }
                }
                trans.Commit();
            }
        }

        [CommandMethod("Delqianzhui")]
        public void DelQianZhui()
        {
            //给文字删除前缀
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptStringOptions pso = new PromptStringOptions("请输入删除的前缀：\r\n");
            PromptResult pr = ed.GetString(pso);
            TypedValue[] values = new TypedValue[] 
            {
                 //new TypedValue ((int)DxfCode.Operator,"<and"),
                 //new TypedValue ((int)DxfCode.LayerName,"set"),
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 //new TypedValue ((int)DxfCode.Operator,"and>"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            ed.WriteMessage("请选择要删除前缀的对象:\r\n");
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (pr.Status != PromptStatus.OK) return;
                string str = pr.StringResult;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;
                        if (text.TextString.Contains(str))
                        {
                            text.TextString = text.TextString.Substring(str.Length);
                        }
                    }
                }
                trans.Commit();
            }
        }

        [CommandMethod("Addnum")]
        public void AddNum()
        {
            //给文字加上数字
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptStringOptions pso = new PromptStringOptions("请输入增加的数字：\r\n");
            PromptResult pr = ed.GetString(pso);
            TypedValue[] values = new TypedValue[] 
            {
                 //new TypedValue ((int)DxfCode.Operator,"<and"),
                 //new TypedValue ((int)DxfCode.LayerName,"set"),
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 //new TypedValue ((int)DxfCode.Operator,"and>"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            ed.WriteMessage("请选择要修改对象:\r\n");
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (pr.Status != PromptStatus.OK) return;
                string str = pr.StringResult;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;

                        text.TextString = Convert.ToDouble(str) + Convert.ToDouble(text.TextString) + "";

                    }
                }
                trans.Commit();
            }
        }

        [CommandMethod("Mulnum")]
        public void MulNum()
        {
            //给文字乘以数字
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptStringOptions pso = new PromptStringOptions("请输入乘的数字：\r\n");
            PromptResult pr = ed.GetString(pso);
            TypedValue[] values = new TypedValue[] 
            {
                 //new TypedValue ((int)DxfCode.Operator,"<and"),
                 //new TypedValue ((int)DxfCode.LayerName,"set"),
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
                 //new TypedValue ((int)DxfCode.Operator,"and>"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            ed.WriteMessage("请选择要修改对象:\r\n");
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (pr.Status != PromptStatus.OK) return;
                string str = pr.StringResult;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;

                        text.TextString = Convert.ToDouble(str) * Convert.ToDouble(text.TextString) + "";

                    }
                }
                trans.Commit();
            }
        }

        [CommandMethod("Addxdata")]
        public void AddXDataDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择实体:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;
            PromptStringOptions pso = new PromptStringOptions("请输入要附加的信息:\r\n");
            PromptResult pr = ed.GetString(pso);
            if (pr.Status != PromptStatus.OK) return;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                RegAppTable rt = trans.GetObject(db.RegAppTableId, OpenMode.ForRead) as RegAppTable;
                if (!rt.Has("appname"))
                {
                    RegAppTableRecord rtr = new RegAppTableRecord();
                    rtr.Name = "appname";
                    rt.UpgradeOpen();
                    rt.Add(rtr);
                    ResultBuffer buffer = new ResultBuffer();
                    buffer.Add(new TypedValue((int)DxfCode.ExtendedDataRegAppName, rtr.Name));
                    buffer.Add(new TypedValue(1000, pr.StringResult));
                    buffer.Add(new TypedValue(1000, ent.ColorIndex));
                    buffer.Add(new TypedValue(1003, ent.Layer));
                    ent.XData = buffer;
                    rt.DowngradeOpen();
                    trans.AddNewlyCreatedDBObject(rtr, true);
                }
                trans.Commit();
            }
        }

        [CommandMethod("Getxdata")]
        public void GetXData()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择实体:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                ResultBuffer buffer = ent.XData;
                if (buffer != null)
                {
                    IEnumerator iter = buffer.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        TypedValue temp = (TypedValue)iter.Current;
                        ed.WriteMessage(temp.TypeCode.ToString() + ":");
                        ed.WriteMessage(temp.Value.ToString() + "\r\n");
                    }
                }

                trans.Commit();
            }
        }

        [CommandMethod("Delxdata")]
        public void DelXData()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择实体:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                RegAppTable rt = trans.GetObject(db.RegAppTableId, OpenMode.ForRead) as RegAppTable;
                string regName = "appname";
                if (!rt.Has(regName))
                {
                    RegAppTableRecord rtr = trans.GetObject(rt[regName], OpenMode.ForWrite) as RegAppTableRecord;
                    rtr.Erase();
                }
                trans.Commit();
            }
        }

        [CommandMethod("DComparedata")]
        public void dbtextCompareTextDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage("code by sailing\r\n");
            ed.WriteMessage("对比图形文字\r\n");
            ed.WriteMessage("请将要对比的文字叠放一起\r\n");
            ed.WriteMessage("请注意文字必须是单行文字\r\n");
            ed.WriteMessage("默认容错间距100\r\n");
            ed.WriteMessage("文字不同显红色\r\n");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(pso);
            SelectionSet sSet = psr.Value;
            List<Entity> entList = new List<Entity>();
            double gapdistance = 100;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status != PromptStatus.OK) return;
                if (sSet == null) return;
                List<DBText> dbtextList = new List<DBText>();
                List<DBText> dbtextList1 = new List<DBText>();
                foreach (SelectedObject item in sSet)
                {
                    Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent is DBText)
                    {
                        DBText text = ent as DBText;

                        dbtextList.Add(text);
                    }

                }
                for (int i = 0; i < dbtextList.Count; i++)
                {
                    for (int j = i + 1; j < dbtextList.Count; j++)
                    {
                        Vector3d v = dbtextList[i].Position.GetVectorTo(dbtextList[j].Position);
                        if (v.Length < gapdistance)
                        {
                            if (dbtextList[i].TextString != dbtextList[j].TextString)
                            {
                                dbtextList1.Add(dbtextList[i]);
                                dbtextList[j].ColorIndex = 1;
                            }
                        }
                    }

                }

                trans.Commit();
            }
        }

        [CommandMethod("MComparedata")]
        public void mtextCompareTextDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage("code by sailing\r\n");
            ed.WriteMessage("对比图形文字\r\n");
            ed.WriteMessage("注意调整多行文字对齐点\r\n");
            ed.WriteMessage("请将要对比的文字叠放一起\r\n");
            ed.WriteMessage("默认容错间距100\r\n");
            ed.WriteMessage("文字不同显洋红色\r\n");
            PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(pso);
            SelectionSet sSet = psr.Value;
            List<Entity> entList = new List<Entity>();
            double gapdistance = 100;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psr.Status != PromptStatus.OK) return;
                if (sSet == null) return;
                List<MText> dbtextList = new List<MText>();
                List<MText> dbtextList1 = new List<MText>();
                foreach (SelectedObject item in sSet)
                {
                    Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent is MText)
                    {
                        MText text = ent as MText;
                        dbtextList.Add(text);
                    }

                }
                for (int i = 0; i < dbtextList.Count; i++)
                {
                    for (int j = i + 1; j < dbtextList.Count; j++)
                    {
                        Vector3d v = dbtextList[i].Location.GetVectorTo(dbtextList[j].Location);
                        if (v.Length < gapdistance)
                        {
                            if (dbtextList[i].Contents.ToString() != dbtextList[j].Contents.ToString())
                            {
                                dbtextList1.Add(dbtextList[i]);
                                dbtextList[j].ColorIndex = 6;
                            }

                        }
                    }

                }

                trans.Commit();
            }
        }

        [CommandMethod("ChangeDBTextColor")]
        public void ChangeDbtextColor()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<DBText> dbtextList = new List<DBText>();
            ;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                dbtextList = db.SelectDbtext();
                foreach (DBText item in dbtextList)
                {
                    item.ColorIndex = 1;
                }
                trans.Commit();
            }

        }

        [CommandMethod("GetSumCal")]
        public void GetDbtextSum()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<DBText> dbtextList = new List<DBText>();
            ed.WriteMessage("请选择需要求和的文字：\r\n");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                dbtextList = db.SelectDbtext();
                DBText sumForce = dbtextList.GetSumText();
                sumForce.Height = 250;
                sumForce.WidthFactor = 0.7;
                sumForce.TextStyleId = db.AddTxtStyle("TSSD_Norm", "tssdeng", "hztxt");
                DBPoint dbpoint = new DBPoint();
                dbpoint.ColorIndex = 1;
                dbpoint.Position = sumForce.Position;
                dbpoint.SetDatabaseDefaults();
                db.Pdmode = 3;
                db.Pdsize = 1;
                db.AddToModelSpace(sumForce, dbpoint);
                trans.Commit();
            }

        }

        [CommandMethod("ChangeText")]
        public void ChangeTextDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptStringOptions pso = new PromptStringOptions("请输入改成之后的内容：\r\n");
            PromptResult pr = ed.GetString(pso);
            List<DBText> dbtextList = new List<DBText>();
            if (pr.Status != PromptStatus.OK) return;
            ed.WriteMessage("请选择需要改变文字对象：\r\n");

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                dbtextList = db.SelectDbtext();
                foreach (DBText item in dbtextList)
                {
                    item.TextString = pr.StringResult;
                }

                trans.Commit();
            }

        }

        [CommandMethod("HeightCheck")]
        public void HeightCheckDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择基准标高：\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            List<DBText> dbtextList = new List<DBText>();
            if (per.Status != PromptStatus.OK) return;
            ed.WriteMessage("请选择需要改变标高文字对象：\r\n");
            ed.WriteMessage("默认tssd标高图层是THIN;\r\n");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                dbtextList = db.SelectDbtext("THIN");
                Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (ent is DBText)
                {
                    DBText baseHight = (DBText)ent;
                    baseHight.ColorIndex = 3;
                    foreach (DBText item in dbtextList)
                    {
                        item.TextString = (Convert.ToDouble(baseHight.TextString) - item.Position.GetVectorTo(baseHight.Position).Y / 1000).ToString("#0.000");
                    }
                }
                trans.Commit();
            }

        }

        [CommandMethod("MoveHeight")]
        public void MoveHeightDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择基准标高：\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            List<Entity> entList = new List<Entity>();
            if (per.Status != PromptStatus.OK) return;
            ed.WriteMessage("请选择需要改变标高文字对象：\r\n");
            ed.WriteMessage("默认tssd标高图层是THIN;\r\n");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                entList = db.SelectEntitysByLayer("THIN");
                Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (ent is DBText)
                {
                    DBText baseHight = (DBText)ent;
                    baseHight.ColorIndex = 3;
                    foreach (Entity item in entList)
                    {
                        if (item is DBText)
                        {
                            db.MoveEntity(item, new Vector3d(0, ((DBText)item).Position.GetVectorTo(baseHight.Position).Y, 0));
                        }
                        else if (item is Line)
                        {
                            if (((Line)item).Length == 600)
                            {
                                db.MoveEntity(item, new Vector3d(0, ((Line)item).StartPoint.GetVectorTo(baseHight.Position).Y - 50, 0));
                            }
                            else if ((((Line)item).Length - 424) < 1)
                            {
                                db.MoveEntity(item, new Vector3d(0, ((Line)item).StartPoint.GetVectorTo(baseHight.Position).Y - 350, 0));
                            }
                            else
                            {
                                db.MoveEntity(item, new Vector3d(0, ((Line)item).StartPoint.GetVectorTo(baseHight.Position).Y - 50, 0));
                            }

                        }

                    }
                }
                trans.Commit();
            }

        }

        [CommandMethod("MoveHeight2")]
        public void MoveHeightDemo2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择基准标高：\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            List<Entity> entList = new List<Entity>();
            if (per.Status != PromptStatus.OK) return;
            ed.WriteMessage("请选择需要改变标高文字对象：\r\n");
            ed.WriteMessage("默认tssd标高图层是THIN;\r\n");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                entList = db.SelectEntitysByLayer("THIN");
                Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (ent is DBText)
                {
                    DBText baseHight = (DBText)ent;
                    baseHight.ColorIndex = 3;
                    foreach (Entity item in entList)
                    {
                        if (item is DBText)
                        {
                            db.MoveEntity(item, new Vector3d(0, ((DBText)item).Position.GetVectorTo(baseHight.Position).Y, 0));
                        }
                        else if (item is Line)
                        {
                            if (((Line)item).Length == 600 * 1.5)
                            {
                                db.MoveEntity(item, new Vector3d(0, ((Line)item).StartPoint.GetVectorTo(baseHight.Position).Y - 50 * 1.5, 0));
                            }
                            else if ((((Line)item).Length - 424 * 1.5) < 1)
                            {
                                db.MoveEntity(item, new Vector3d(0, ((Line)item).StartPoint.GetVectorTo(baseHight.Position).Y - 350 * 1.5, 0));
                            }
                            else
                            {
                                db.MoveEntity(item, new Vector3d(0, ((Line)item).StartPoint.GetVectorTo(baseHight.Position).Y - 50 * 1.5, 0));
                            }

                        }

                    }
                }
                trans.Commit();
            }

        }

        [CommandMethod("DrawPileDetail")]
        public static void DrawPileDetail()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptPointResult ppr = ed.GetPoint("指定点圆心点： ");
            PromptDoubleResult pdr = ed.GetDouble("请输入圆半径：mm");
            PromptIntegerResult pir = ed.GetInteger("请输入钢筋根数：");
            PromptIntegerResult pird = ed.GetInteger("请输入钢筋直径：");
            PromptIntegerResult spir = ed.GetInteger("放大倍数：");
            int n = pir.Value;//钢筋根数
            int sn = spir.Value;//放大倍数
            Point3d p = ppr.Value;//圆心
            double r = pdr.Value;//桩半径
            double rd = pird.Value;//钢筋直径
            Circle c = new Circle(p, new Vector3d(0, 0, 1), r);
            Point3d p1 = p + new Vector3d(0, r - 50, 0);
            Point3d p2 = p + new Vector3d(0, -r + 50, 0);
            Polyline poly1 = new Polyline();
            poly1.ColorIndex = 1;

            Point3d p3 = p + new Vector3d(0, r - 100, 0);
            Point3d p4 = p + new Vector3d(0, -r + 100, 0);
            Polyline poly2 = new Polyline();
            poly2.ColorIndex = 1;

            Point3d p5 = p + new Vector3d(0, r - 60, 0);
            Point3d p6 = p + new Vector3d(0, r - 80, 0);
            Polyline poly3 = new Polyline();
            poly3.ColorIndex = 6;

            poly1.AddVertexAt(0, new Point2d(p1.X, p1.Y), 1, 10, 10);
            poly1.AddVertexAt(1, new Point2d(p2.X, p2.Y), 1, 10, 10);
            poly1.Closed = true;

            poly2.AddVertexAt(0, new Point2d(p3.X, p3.Y), 1, 10, 10);
            poly2.AddVertexAt(1, new Point2d(p4.X, p4.Y), 1, 10, 10);
            poly2.Closed = true;

            poly3.AddVertexAt(0, new Point2d(p5.X, p5.Y), 1, 100, 100);
            poly3.AddVertexAt(1, new Point2d(p6.X, p6.Y), 1, 100, 100);
            poly3.Closed = true;

            Matrix3d mtr = Matrix3d.Scaling(sn, p);
            c.TransformBy(mtr);
            poly1.TransformBy(mtr);
            poly2.TransformBy(mtr);
            poly3.TransformBy(mtr);

            Entity[] ents = new Entity[n];
            ents = ArrayPolar(poly3, p, n, 2 * Math.PI);


            double netspace = (3.14 * r * 2 - n * rd) / n;
            DBText dbtext = new DBText();
            dbtext.TextString = "配筋率：" + 3.14 * rd * rd / 4 * n / (3.14 * r * r) * 100 + "%";
            dbtext.Position = p;
            dbtext.Height = 100;
            dbtext.ColorIndex = 3;

            DBText dbtext1 = new DBText();
            dbtext1.TextString = "钢筋净距Sn:" + +netspace;
            dbtext1.Position = p + new Vector3d(0, -dbtext.Height * 1.2, 0);
            dbtext1.Height = 100;
            dbtext1.ColorIndex = 3;

            db.AddToModelSpace(c, poly1, poly2, dbtext, dbtext1);
            db.AddToModelSpace(ents);


        }
        public static Entity[] ArrayPolar(Entity ent, Point3d pt, int numObj, double Angle)
        {
            Entity[] ents = new Entity[numObj];
            ents[0] = ent;

            for (int i = 1; i < numObj; i++)
            {
                Matrix3d mt = Matrix3d.Rotation(Angle * i / numObj, Vector3d.ZAxis, pt);

                ents[i] = ent.GetTransformedCopy(mt);
                if (ents[i] is Polyline)
                {
                    ((Polyline)ents[i]).ConstantWidth = 100;
                    ((Polyline)ents[i]).ColorIndex = 6;
                }
            }
            return ents;
        }


        [CommandMethod("xs")]
        public void FanWeiXuanShu()
        {
            //范围选数
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptDoubleOptions psoLower = new PromptDoubleOptions("请输入闭区间下限：\r\n");
            PromptDoubleResult pdrLower = ed.GetDouble(psoLower);

            PromptDoubleOptions psoUper = new PromptDoubleOptions("请输入开区间上限：\r\n");
            PromptDoubleResult pdrUper = ed.GetDouble(psoUper);

            PromptIntegerOptions pio = new PromptIntegerOptions("请输入颜色号：\r\n");
            PromptIntegerResult pir = ed.GetInteger(pio);

            TypedValue[] values = new TypedValue[] 
            {
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            ed.WriteMessage("请选择要修改对象:\r\n");
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (pdrLower.Status != PromptStatus.OK) return;
                if (pdrUper.Status != PromptStatus.OK) return;
                if (pir.Status != PromptStatus.OK) return;
                if (psr.Status != PromptStatus.OK) return;
                double Lower = pdrLower.Value;
                double Uper = pdrUper.Value;
                int colorInt = pir.Value % 256;
                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;

                        if (Convert.ToDouble(text.TextString) < Uper && Convert.ToDouble(text.TextString) >= Lower)
                        {
                            text.ColorIndex = colorInt;
                        }

                    }
                }
                trans.Commit();
            }
        }

        [CommandMethod("fsdetail")]
        public void FenSeDetail()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptPointOptions ppo = new PromptPointOptions("请选择生成点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK) return;
            int sum = 0;
            for (int i = 1; i < 10; i++)
            {
                DBText text = new DBText();
                sum = 10 + i * 2;
                text.TextString = "%%132" + sum + "@200";
                text.ColorIndex = i;
                text.Height = 300;
                text.WidthFactor = 0.7;
                text.Position = ppr.Value + new Vector3d(0, -400 * i, 0);
                text.TextStyleId = db.AddTxtStyle("TSSD_Norm", "tssdeng", "hztxt");
                text.LayerId = db.AddLayerTool("steel");
                db.AddToModelSpace(text);
            }

        }

        [CommandMethod("tc")]
        public void Tc()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<Polyline> polylines = new List<Polyline>();
            List<DBText> dbtexts = new List<DBText>();
            //PromptPointOptions ppo = new PromptPointOptions("请选择指定点：");
            //PromptPointResult ppr = ed.GetPoint(ppo);
            //if (ppr.Status != PromptStatus.OK) return;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (Entity ent in db.SelectEntitysByLayer("COLU"))
                {
                    if (ent is Polyline)
                    {
                        polylines.Add((Polyline)ent);
                        ent.ChangeColor(2);
                    }
                }
                foreach (Entity ent in db.SelectEntitysByLayer("COLU_NUM"))
                {
                    if (ent is DBText)
                    {
                        dbtexts.Add((DBText)ent);
                        ent.ChangeColor(3);
                    }
                }
                Dictionary<Polyline, DBText> dic = new Dictionary<Polyline, DBText>();

                for (int i = 0; i < dbtexts.Count; i++)
                {
                    for (int j = 0; j < polylines.Count; j++)
                    {
                        if (dbtexts[i].Position.GetVectorTo(db.GetCentroid(polylines[j])).Length < 1000)
                        {
                            DBPoint dp = new DBPoint();

                            dp.Position = db.GetCentroid(polylines[j]);

                            db.AddToModelSpace(dp);

                            Line line = new Line(dbtexts[i].Position, db.GetCentroid(polylines[j]));

                            db.AddToModelSpace(line);
                            dic[polylines[j]] = dbtexts[i];
           
                        }
                    }
                }
                int n = 0;
                foreach (KeyValuePair<Polyline, DBText> item in dic)
                {

                    db.AddXdata(item.Key.ObjectId, n + "", item.Value.TextString);
                    n++;
                }

                Dictionary<string, int> myDics = new Dictionary<string, int>();
                for (int i = 0; i < dbtexts.Count; i++)
                {
                    if (myDics.ContainsKey(dbtexts[i].TextString))
                    {
                        myDics[dbtexts[i].TextString]++;
                    }
                    else
                    {
                        myDics[dbtexts[i].TextString] = 1;
                    }
                }

                foreach (var kv in myDics)
                {
                    ed.WriteMessage("编号:{0}======个数:{1}\r\n", kv.Key, kv.Value);
                    foreach (var item in dic)
                    {
                        if (item.Value.TextString==kv.Key)
                        {
                            ed.WriteMessage("编号:" + kv.Key + ",面积：" + item.Key.Area + ",长度：" + item.Key.Length+ "\r\n");
                        }
                    }
                }
                #region MyRegion
                //foreach (var kv in myDicd)
                //{
                //     ed.WriteMessage("面积：{0}======个数{1}\r\n", kv.Key, kv.Value);
                //}
                //Table tb = new Table();
                //tb.SetSize(dbtexts.Count, 5);
                //tb.SetRowHeight(1000);
                //tb.SetColumnWidth(2000);
                //tb.Position = ppr.Value;
                //int m = 0;
                //foreach (var kv in myDics)
                //{
                //    tb.Cells[m, 0].TextHeight = 400;
                //    tb.Cells[m, 1].TextHeight = 400;
                //    tb.Cells.TextStyleId = db.AddTxtStyle("TSSD_Norm", "tssdeng", "hztxt");
                //    tb.Cells[m, 0].TextString = kv.Key;
                //    tb.Cells[m, 1].TextString = kv.Value + "";

                //    m++;
                //}

                //db.AddToModelSpace(tb);
                #endregion
 
                
                

                trans.Commit();
            }
        }
    }
}




