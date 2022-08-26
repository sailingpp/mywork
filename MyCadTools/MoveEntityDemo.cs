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
    public class MoveEntityDemo
    {
        public Polyline ent;
        public Polyline ent1;
        public MyPolyline ment;
        public MyPolyline ment1;
        public DBText text1;
        public DBText text2;

        [CommandMethod("moveent")]
        public void MoveEntityMethodDemo()
        {
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
                    Entity ent = (Polyline)per.ObjectId.GetObject(OpenMode.ForWrite);
                    ent.MoveEntity(new Vector3d(100, 0, 0));
                    ent.Modified += ShowMessage;
                }
                trans.Commit();
            }
        }
        private void ShowMessage(object sender, EventArgs e)
        {
            Entity ent = (Entity)sender;
            ent.Database.WriteMesage("我被修改了");
        }
        [CommandMethod("movesame")]
        public void MoveEntityWithSameCenterDemo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptEntityOptions peo = new PromptEntityOptions("请选择多段线1:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);
            PromptEntityOptions peo1 = new PromptEntityOptions("请选择多段线2:\r\n");
            PromptEntityResult per1 = ed.GetEntity(peo1);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK && per1.Status == PromptStatus.OK)
                {
                    ent = (Polyline)per.ObjectId.GetObject(OpenMode.ForWrite);
                    ent1 = (Polyline)per1.ObjectId.GetObject(OpenMode.ForWrite);
                    ment = new MyPolyline(ent);
                    ment1 = new MyPolyline(ent1);
                    if (ment.Centroids == ment1.Centroids)
                    {
                        ent.Modified += ModifyTest;
                    }
                }
                trans.Commit();
            }
        }
        private void ModifyTest(object sender, EventArgs e)
        {
            Database db = ent1.Database;
            db.WriteMesage("ent已经变化了+\r\n");
            db.WriteMesage("ent:"+ment.Area.ToString()+"\r\n");
            db.WriteMesage("ent1:" + ment1.Area.ToString() + "\r\n");
            db.MoveEntity(ent1,ment1.Centroids.GetVectorTo(ment.Centroids));
        }
        [CommandMethod("bindtext")]
        public void BindText()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            ed.WriteMessage("tip!文字不相同的时候才可以绑定");
            PromptEntityOptions peo = new PromptEntityOptions("请选择原文字:\r\n");
            PromptEntityResult per = ed.GetEntity(peo);

            PromptEntityOptions peo1 = new PromptEntityOptions("请选择需要改变的文字:\r\n");
            PromptEntityResult per1 = ed.GetEntity(peo1);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (per.Status == PromptStatus.OK && per1.Status == PromptStatus.OK)
                {
                    text1 = (DBText)per.ObjectId.GetObject(OpenMode.ForWrite);
                    text2 = (DBText)per1.ObjectId.GetObject(OpenMode.ForWrite);
                    if (text1.TextString!=text2.TextString)
                    {
                        text1.Modified += ModifyTextString;
                    }
                }
                else
                {
                    ed.WriteMessage("请重新选择！");
                }
                trans.Commit();
            }
        }
        private void ModifyTextString(object sender, EventArgs e)
        {
            Database db = text2.Database;
            db.WriteMesage("检测到text1改变了");
            db.WriteMesage("text1的原值是" + text1.TextString);
            db.WriteMesage("text2的原值是" + text2.TextString);
            db.ChangeText(text2, text1.TextString);
        }


    }
    public static class MyCadTools
    {
        public static ObjectId MyAddLayer(this Database db, string layerName)
        {
            ObjectId layerId = ObjectId.Null;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                if (!lt.Has(layerName))
                {
                    LayerTableRecord ltr = new LayerTableRecord();
                    ltr.Name = layerName;
                    lt.UpgradeOpen();
                    layerId = lt.Add(ltr);
                    trans.AddNewlyCreatedDBObject(ltr, true);
                    lt.DowngradeOpen();
                }
                else
                {
                    LayerTableRecord ltr = trans.GetObject(lt[layerName], OpenMode.ForWrite) as LayerTableRecord;
                    layerId = lt[layerName];
                    ed.WriteMessage("该图层已经存在");
                }

                trans.Commit();
            }
            return layerId;


        }
        public static ObjectId MyAddBlock(this Database db, string blockName, List<Entity> ents)
        {
            ObjectId blockId = ObjectId.Null;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (!bt.Has(blockName))
                {
                    BlockTableRecord btr = new BlockTableRecord();
                    btr.Name = blockName;
                    bt.UpgradeOpen();
                    foreach (Entity item in ents)
                    {
                        if (item.IsNewObject)
                        {
                            btr.AppendEntity(item);
                            blockId = bt.Add(btr);
                            trans.AddNewlyCreatedDBObject(btr, true);
                            bt.DowngradeOpen();
                        }
                        else
                        {
                            item.ColorIndex = 3;
                        }
                    }
                }
                else
                {
                    BlockTableRecord btr = trans.GetObject(bt[blockName], OpenMode.ForWrite) as BlockTableRecord;
                    blockId = bt[blockName];
                    ed.WriteMessage("块表已经存在");
                }
                trans.Commit();
            }
            return blockId;


        }
        public static ObjectId MyAddBlock(this Database db, string blockName, List<ObjectId> objectids)
        {
            ObjectId blockId = ObjectId.Null;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                if (!bt.Has(blockName))
                {
                    BlockTableRecord btr = new BlockTableRecord();
                    btr.Name = blockName;
                    bt.UpgradeOpen();
                    foreach (ObjectId obj in objectids)
                    {
                        btr.AppendEntity((Entity)obj.GetObject(OpenMode.ForWrite));
                    }

                    blockId = bt.Add(btr);
                    trans.AddNewlyCreatedDBObject(btr, true);
                    bt.DowngradeOpen();
                }
                else
                {
                    BlockTableRecord btr = trans.GetObject(bt[blockName], OpenMode.ForWrite) as BlockTableRecord;
                    blockId = bt[blockName];
                    ed.WriteMessage("块表已经存在");
                }

                trans.Commit();
            }
            return blockId;


        }
    }
    public class MyCadDemo
    {
        [CommandMethod("mylayer")]
        public void MyAddLayerDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult pr = ed.GetString("请输入增加的图层名");
            string str = pr.StringResult;
            db.MyAddLayer(str);
        }
        [CommandMethod("myblock")]
        public void MyAddBlockDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult pr = ed.GetString("请输入增加的块表名");
            string str = pr.StringResult;
            Circle c = new Circle(new Point3d(0, 0, 0), new Vector3d(0, 0, 1), 100);
            Line line = new Line(new Point3d(-50, 0, 0), new Point3d(50, 0, 0));
            Line line1 = new Line(new Point3d(0, -50, 0), new Point3d(0, 50, 0));
            List<Entity> ents = new List<Entity>();
            ents.Add(c);
            ents.Add(line);
            ents.Add(line1);
            db.MyAddBlock(str, ents);
        }
        [CommandMethod("insertblock")]
        public void InsertMyBlockDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
                BlockTableRecord space = db.CurrentSpaceId.GetObject(OpenMode.ForWrite) as BlockTableRecord;
                //判断名为“door”的块是否存在
                if (!bt["11"].IsNull)
                {
                    BlockReference br = new BlockReference(Point3d.Origin, bt["11"]);
                    br.ScaleFactors = new Scale3d(2.0);//设置尺寸为原来2倍
                    space.AppendEntity(br);
                    trans.AddNewlyCreatedDBObject(br, true);

                    trans.Commit();
                }
                else
                {
                    return;
                }

            }
        }
        [CommandMethod("beblock")]
        public void BeBlockDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(pso);
            List<Entity> ents = new List<Entity>();

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;
                if (psr.Status != PromptStatus.OK) return;
                if (sset != null)
                {
                    foreach (SelectedObject item in sset)
                    {
                        Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                        ents.Add(ent);
                    }
                }
                PromptResult pr = ed.GetString("请输入增加的块表名");
                string str = pr.StringResult;

                db.MyAddBlock(str, ents);


                trans.Commit();
            }




        }
    }
}
