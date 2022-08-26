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
    public class AdjustStructreDemo
    {
        /// <summary>
        /// 调整柱x方向距离
        /// </summary>
        [CommandMethod("tcx")]
        public void tcx()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择对齐的x向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个


            PromptDoubleOptions pdo = new PromptDoubleOptions("请输入间距\r\n");
            PromptDoubleResult pdr = ed.GetDouble(pdo);
            double gap = pdr.Value;

            PromptSelectionOptions psoCols = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                 new TypedValue((int)DxfCode.LayerName, "COLU"),
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrCols = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;
                    SelectionSet setCols = psrCols.Value;
                    ObjectId[] oids = setCols.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Polyline col = (Polyline)item.GetObject(OpenMode.ForWrite);
                        double dis = zx.GetClosestPointTo(col.StartPoint, true).DistanceTo(col.StartPoint);
                        if (dis != gap)
                        {
                            Point3d t = new Point3d(col.StartPoint.X, zx.StartPoint.Y + gap, col.StartPoint.Z);
                            db.MoveEnt(col.ObjectId, col.StartPoint, t);
                        }
                    }
                }

                trans.Commit();
            }




        }
        /// <summary>
        /// 调整柱y方向距离
        /// </summary>
        [CommandMethod("tcy")]
        public void tcy()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择对齐的y向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个

            PromptDoubleOptions pdo = new PromptDoubleOptions("请输入间距\r\n");
            PromptDoubleResult pdr = ed.GetDouble(pdo);
            double gap = pdr.Value;

            PromptSelectionOptions psoCols = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                 new TypedValue((int)DxfCode.LayerName, "COLU"),
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrCols = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;
                    // zx.ColorIndex = 3;

                    SelectionSet setCols = psrCols.Value;
                    ObjectId[] oids = setCols.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Polyline col = (Polyline)item.GetObject(OpenMode.ForWrite);
                        // col.ColorIndex = 1;
                        double dis = zx.GetClosestPointTo(col.StartPoint, true).DistanceTo(col.StartPoint);
                        if (dis != gap)
                        {
                            Point3d t = new Point3d(zx.StartPoint.X - gap, col.StartPoint.Y, col.StartPoint.Z);
                            db.MoveEnt(col.ObjectId, col.StartPoint, t);
                        }
                    }
                }

                trans.Commit();
            }




        }
        /// <summary>
        /// 调整x方向梁宽
        /// </summary>
        [CommandMethod("tbx")]
        public void tbx()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择对齐的x向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个

            PromptDoubleOptions pdoup = new PromptDoubleOptions("请输入上间距\r\n");
            PromptDoubleResult pdrup = ed.GetDouble(pdoup);
            double gapup = pdrup.Value;

            PromptDoubleOptions pdodown = new PromptDoubleOptions("请输入下间距\r\n");
            PromptDoubleResult pdrdown = ed.GetDouble(pdodown);
            double gapdown = pdrdown.Value;
            PromptSelectionOptions psoBeam = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                 new TypedValue((int)DxfCode.Operator, "<or"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM_CON"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM"),
                 new TypedValue((int)DxfCode.Operator, "or>")
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrBeam = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;

                    SelectionSet setBeam = psrBeam.Value;
                    ObjectId[] oids = setBeam.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Line beam = (Line)item.GetObject(OpenMode.ForWrite);
                        double dis = zx.GetClosestPointTo(beam.StartPoint, true).DistanceTo(beam.StartPoint);
                        if (beam.StartPoint.Y >= zx.StartPoint.Y)
                        {
                            if (dis != gapup)
                            {
                                Point3d t1 = new Point3d(beam.StartPoint.X, zx.StartPoint.Y + gapup, beam.StartPoint.Z);
                                db.MoveEnt(beam.ObjectId, beam.StartPoint, t1);
                            }
                        }
                        else
                        {
                            if (dis != gapdown)
                            {
                                Point3d t1 = new Point3d(beam.StartPoint.X, zx.StartPoint.Y - gapdown, beam.StartPoint.Z);
                                db.MoveEnt(beam.ObjectId, beam.StartPoint, t1);
                            }

                        }

                    }
                }



                //#endregion




                trans.Commit();
            }




        }
        /// <summary>
        /// 调整y方向梁宽
        /// </summary>
        [CommandMethod("tby")]
        public void tby()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择对齐的y向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个

            PromptDoubleOptions pdoup = new PromptDoubleOptions("请输入左间距\r\n");
            PromptDoubleResult pdrup = ed.GetDouble(pdoup);
            double gapup = pdrup.Value;

            PromptDoubleOptions pdodown = new PromptDoubleOptions("请输入右间距\r\n");
            PromptDoubleResult pdrdown = ed.GetDouble(pdodown);
            double gapdown = pdrdown.Value;
            PromptSelectionOptions psoBeam = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                   new TypedValue((int)DxfCode.Operator, "<or"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM_CON"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM"),
                 new TypedValue((int)DxfCode.Operator, "or>")
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrBeam = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;

                    SelectionSet setBeam = psrBeam.Value;
                    ObjectId[] oids = setBeam.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Line beam = (Line)item.GetObject(OpenMode.ForWrite);
                        double dis = zx.GetClosestPointTo(beam.StartPoint, true).DistanceTo(beam.StartPoint);
                        if (beam.StartPoint.X >= zx.StartPoint.X)
                        {
                            if (dis != gapup)
                            {
                                Point3d t1 = new Point3d(zx.StartPoint.X + gapup, beam.StartPoint.Y, beam.StartPoint.Z);
                                db.MoveEnt(beam.ObjectId, beam.StartPoint, t1);
                            }
                        }
                        else
                        {
                            if (dis != gapdown)
                            {
                                Point3d t1 = new Point3d(zx.StartPoint.X - gapdown, beam.StartPoint.Y, beam.StartPoint.Z);
                                db.MoveEnt(beam.ObjectId, beam.StartPoint, t1);
                            }

                        }

                    }
                }




                trans.Commit();
            }




        }
        /// <summary>
        /// 调整x方向轴线
        /// </summary>
        [CommandMethod("tdx")]
        public void tdx()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择最上边对齐的x向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个

            PromptSelectionOptions psoCols = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                  new TypedValue((int)DxfCode.Operator, "<or"),
                 new TypedValue((int)DxfCode.LayerName, "DOTE"),
                 new TypedValue((int)DxfCode.LayerName, "AXIS"),
                  new TypedValue((int)DxfCode.Operator, "or>")
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrDotes = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;

                    SelectionSet setDotes = psrDotes.Value;
                    ObjectId[] oids = setDotes.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Line dote = (Line)item.GetObject(OpenMode.ForWrite);
                        double dis = zx.GetClosestPointTo(dote.StartPoint, true).DistanceTo(dote.StartPoint);
                        if (dis % 50 != 0)
                        {
                            Point3d t = new Point3d(dote.StartPoint.X, zx.StartPoint.Y - Math.Round(dis / 10) * 10, dote.StartPoint.Z);
                            db.MoveEnt(dote.ObjectId, dote.StartPoint, t);
                        }
                    }
                }




                trans.Commit();
            }




        }
        /// <summary>
        /// 调整y方向轴线
        /// </summary>
        [CommandMethod("tdy")]
        public void tdy()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择最左边的y向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个

            PromptSelectionOptions psoCols = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                  new TypedValue((int)DxfCode.Operator, "<or"),
                 new TypedValue((int)DxfCode.LayerName, "DOTE"),
                 new TypedValue((int)DxfCode.LayerName, "AXIS"),
                  new TypedValue((int)DxfCode.Operator, "or>")
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrDotes = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;

                    SelectionSet setDotes = psrDotes.Value;
                    ObjectId[] oids = setDotes.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Line dote = (Line)item.GetObject(OpenMode.ForWrite);
                        double dis = zx.GetClosestPointTo(dote.StartPoint, true).DistanceTo(dote.StartPoint);
                        if (dis % 50 != 0)
                        {
                            Point3d t = new Point3d(zx.StartPoint.X + Math.Round(dis / 10) * 10, dote.StartPoint.Y, dote.StartPoint.Z);
                            db.MoveEnt(dote.ObjectId, dote.StartPoint, t);
                        }
                    }
                }




                trans.Commit();
            }




        }
        /// <summary>
        /// 调整直线左端点
        /// </summary>
        [CommandMethod("tlp")]
        public static void AdjustLineByStartPoint()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择直线");
            PromptEntityResult per = ed.GetEntity(peo);
            db.MoveLineStartPointbyY(per.ObjectId, 100);
            #region MyRegion
            //using (Transaction trans=db.TransactionManager.StartTransaction())
            //{
            //    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            //    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
            //    Entity ent = per.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
            //    if (ent is Line)
            //    {
            //        ((Line)ent).StartPoint = new Point3d(0, 0, 0);
            //    }
            //    trans.Commit();
            #endregion


        }
        /// <summary>
        /// 调整直线右端点
        /// </summary>
        [CommandMethod("trp")]
        public static void AdjustLineByEndPoint()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择直线");
            PromptEntityResult per = ed.GetEntity(peo);
            db.MoveLineEndPointbyY(per.ObjectId, 100);



        }
        /// <summary>
        /// 调整直线两端点
        /// </summary>
        [CommandMethod("tbp")]
        public static void AdjustLineByStartEndPoint()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("请选择直线");
            PromptEntityResult per = ed.GetEntity(peo);
            db.MoveLineStartEndPointbyY(per.ObjectId, 100, 100);

        }
        /// <summary>
        /// 按端点调整梁线拉平x方向
        /// </summary>
        [CommandMethod("adx")]
        public void adx()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择对齐的x向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个

            PromptDoubleOptions pdoup = new PromptDoubleOptions("请输入上间距\r\n");
            PromptDoubleResult pdrup = ed.GetDouble(pdoup);
            double gapup = pdrup.Value;

            PromptDoubleOptions pdodown = new PromptDoubleOptions("请输入下间距\r\n");
            PromptDoubleResult pdrdown = ed.GetDouble(pdodown);
            double gapdown = pdrdown.Value;
            PromptSelectionOptions psoBeam = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                  new TypedValue((int)DxfCode.Operator, "<or"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM_CON"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM"),
                 new TypedValue((int)DxfCode.Operator, "or>")
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrBeam = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;

                    SelectionSet setBeam = psrBeam.Value;
                    ObjectId[] oids = setBeam.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Line beam = (Line)item.GetObject(OpenMode.ForWrite);
                        double dis = zx.GetClosestPointTo(beam.StartPoint, true).DistanceTo(beam.StartPoint);
                        if (beam.StartPoint.Y >= zx.StartPoint.Y)
                        {
                            if (dis != gapup)
                            {
                                db.SetLineStartEndPointbyY(beam.ObjectId, zx.StartPoint.Y + gapup, zx.StartPoint.Y + gapup);
                            }
                        }
                        else
                        {
                            if (dis != gapdown)
                            {
                                db.SetLineStartEndPointbyY(beam.ObjectId, zx.StartPoint.Y - gapdown, zx.StartPoint.Y - gapdown);
                            }

                        }

                    }
                }





                trans.Commit();
            }




        }
        /// <summary>
        /// 按端点调整梁线拉平y方向
        /// </summary>
        [CommandMethod("ady")]
        public void ady()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peoZx = new PromptEntityOptions("请选择对齐的y向轴线\r\n");
            PromptEntityResult psrZx = ed.GetEntity(peoZx);//选择单个

            PromptDoubleOptions pdoup = new PromptDoubleOptions("请输入左间距\r\n");
            PromptDoubleResult pdrup = ed.GetDouble(pdoup);
            double gapup = pdrup.Value;

            PromptDoubleOptions pdodown = new PromptDoubleOptions("请输入右间距\r\n");
            PromptDoubleResult pdrdown = ed.GetDouble(pdodown);
            double gapdown = pdrdown.Value;
            PromptSelectionOptions psoBeam = new PromptSelectionOptions();//批量选择

            TypedValue[] fvalue = new TypedValue[]
            {
                 new TypedValue((int)DxfCode.Operator, "<or"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM_CON"),
                 new TypedValue((int)DxfCode.LayerName, "BEAM"),
                 new TypedValue((int)DxfCode.Operator, "or>")
            };
            SelectionFilter ft = new SelectionFilter(fvalue);
            PromptSelectionResult psrBeam = ed.GetSelection(ft);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (psrZx.Status != PromptStatus.OK) return;
                Entity polyZx = psrZx.ObjectId.GetObject(OpenMode.ForWrite) as Entity;
                if (polyZx is Line)
                {
                    Line zx = polyZx as Line;
                    SelectionSet setBeam = psrBeam.Value;
                    ObjectId[] oids = setBeam.GetObjectIds();
                    foreach (ObjectId item in oids)
                    {
                        Line beam = (Line)item.GetObject(OpenMode.ForWrite);
                        double dis = zx.GetClosestPointTo(beam.StartPoint, true).DistanceTo(beam.StartPoint);
                        if (beam.StartPoint.X >= zx.StartPoint.X)
                        {
                            if (dis != gapup)
                            {
                                db.SetLineStartEndPointbyX(beam.ObjectId, zx.StartPoint.X + gapup, zx.StartPoint.X + gapup);
                            }
                        }
                        else
                        {
                            if (dis != gapdown)
                            {
                                db.SetLineStartEndPointbyX(beam.ObjectId, zx.StartPoint.X - gapdown, zx.StartPoint.X - gapdown);
                            }

                        }

                    }
                }

                trans.Commit();
            }
        }
        /// <summary>
        /// 标记距离
        /// </summary>
        [CommandMethod("adt")]

        public void adjustall()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<Line> lineDote = new List<Line>();//所有的轴线
            List<Line> lineDoteX = new List<Line>();//所有x向轴线
            List<Line> lineDoteY = new List<Line>();//所有y向轴线
            List<Line> lineBeam = new List<Line>();//所有的梁线
            List<Line> lineBeamX = new List<Line>();//所有x向梁线
            List<Line> lineBeamY = new List<Line>();//所有y向梁线
            List<Polyline> PlineCol = new List<Polyline>();//所有柱子
            List<Polyline> PlineWall = new List<Polyline>();//所有墙
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId oid in btr)
                {
                    Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent is Line)
                    {
                        if (ent.Layer == "DOTE" || ent.Layer == "AXIS")
                        {
                            lineDote.Add((Line)ent);
                        }
                        else if (ent.Layer == "BEAM" || ent.Layer == "BEAM_CON")
                        {
                            lineBeam.Add((Line)ent);
                        }

                    }
                    else if (ent is Polyline)
                    {
                        if (ent.Layer == "COLU")
                        {
                            PlineCol.Add((Polyline)ent);
                        }
                        else if (ent.Layer == "Wall")
                        {
                            PlineWall.Add((Polyline)ent);
                        }
                    }

                }

                for (int i = 0; i < lineDote.Count; i++)
                {
                    Vector3d vx = new Vector3d(1, 0, 0);
                    Vector3d vy = new Vector3d(0, 0, 0);
                    if (lineDote[i].StartPoint.GetVectorTo(lineDote[i].EndPoint).GetAngleTo(vx) < 0.3
                        && lineDote[i].StartPoint.GetVectorTo(lineDote[i].EndPoint).GetAngleTo(vx) > -0.3)
                    {
                        lineDoteX.Add(lineDote[i]);
                    }
                    else
                    {
                        lineDoteY.Add(lineDote[i]);
                    }
                }
                for (int i = 0; i < lineBeam.Count; i++)
                {
                    Vector3d vx = new Vector3d(1, 0, 0);
                    Vector3d vy = new Vector3d(0, 0, 0);
                    if (lineBeam[i].StartPoint.GetVectorTo(lineBeam[i].EndPoint).GetAngleTo(vx) < 0.3
                        && lineBeam[i].StartPoint.GetVectorTo(lineBeam[i].EndPoint).GetAngleTo(vx) > -0.3)
                    {
                        lineBeamX.Add(lineBeam[i]);
                    }
                    else
                    {
                        lineBeamY.Add(lineBeam[i]);
                    }
                }
                ed.WriteMessage("\r\n总共轴线{0}个\r\n", lineDote.Count);
                ed.WriteMessage("x方向轴线{0}个\r\n", lineDoteX.Count);
                ed.WriteMessage("y方向轴线{0}个\r\n", lineDoteY.Count);
                ed.WriteMessage("总共梁线{0}个\r\n", lineBeam.Count);
                ed.WriteMessage("x方向梁线{0}个\r\n", lineBeamX.Count);
                ed.WriteMessage("y方向梁线{0}个\r\n", lineBeamY.Count);
                ed.WriteMessage("总共有柱子{0}个\r\n", PlineCol.Count);
                ed.WriteMessage("总共有墙{0}个\r\n", PlineWall.Count);


                for (int i = 0; i < lineDoteX.Count; i++)
                {
                    List<Polyline> listColX = new List<Polyline>();
                    for (int j = 0; j < PlineCol.Count; j++)
                    {
                        if (lineDoteX[i].GetClosestPointTo(PlineCol[j].StartPoint, true).DistanceTo(PlineCol[j].StartPoint) < 600)
                        {
                            listColX.Add(PlineCol[j]);
                        }
                    }
                    ed.WriteMessage("X向第{0}条轴线,有{1}个柱子\r\n", i, listColX.Count);
                    for (int j = 0; j < listColX.Count; j++)
                    {
                        double dis = lineDoteX[i].GetClosestPointTo(PlineCol[j].StartPoint, true).DistanceTo(PlineCol[j].StartPoint);
                        if (dis % 50 != 0)
                        {
                            ed.WriteMessage("X向第{0}条轴线的第{1}个柱子间距为{2}不对 \r\n", i, j, dis);
                            listColX[j].ColorIndex = 3;
                        }
                    }
                }

                for (int i = 0; i < lineDoteY.Count; i++)
                {
                    List<Polyline> listColY = new List<Polyline>();
                    for (int j = 0; j < PlineCol.Count; j++)
                    {
                        if (lineDoteY[i].GetClosestPointTo(PlineCol[j].StartPoint, true).DistanceTo(PlineCol[j].StartPoint) < 600)
                        {
                            listColY.Add(PlineCol[j]);
                        }
                    }
                    ed.WriteMessage("Y向第{0}条轴线,有{1}个柱子\r\n", i, listColY.Count);

                    for (int j = 0; j < listColY.Count; j++)
                    {
                        double dis = lineDoteY[i].GetClosestPointTo(PlineCol[j].StartPoint, true).DistanceTo(PlineCol[j].StartPoint);
                        if (dis % 50 != 0)
                        {
                            ed.WriteMessage("Y向第{0}条轴线的第{1}个柱子间距为{2}不对 \r\n", i, j, dis);
                            listColY[j].ColorIndex = 1;
                        }
                    }
                }
                trans.Commit();
            }

        }
        /// <summary>
        /// 调整梁柱间距
        /// </summary>
        [CommandMethod("ada")]
        public void adjustall2()
        {
            #region 定义变量
            double gap = 600;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<Line> lineDote = new List<Line>();//所有的轴线
            List<Line> lineDoteX = new List<Line>();//所有x向轴线
            List<Line> lineDoteY = new List<Line>();//所有y向轴线
            List<Line> lineBeam = new List<Line>();//所有的梁线
            List<Line> lineBeamX = new List<Line>();//所有x向梁线
            List<Line> lineBeamY = new List<Line>();//所有y向梁线
            List<Polyline> PlineCol = new List<Polyline>();//所有柱子
            List<Polyline> PlineWall = new List<Polyline>();//所有墙
            PromptDoubleOptions pdoGapCX = new PromptDoubleOptions("\r\n请输入柱上间距\r\n");
            pdoGapCX.DefaultValue = 250;
            PromptDoubleOptions pdoGapCY = new PromptDoubleOptions("请输入柱左间距\r\n");
            pdoGapCY.DefaultValue = 250;
            PromptDoubleOptions pdoGapBXup = new PromptDoubleOptions("请输入梁上间距\r\n");
            pdoGapBXup.DefaultValue = 200;
            PromptDoubleOptions pdoGapBXdown = new PromptDoubleOptions("请输入梁下间距\r\n");
            pdoGapBXdown.DefaultValue = 200;
            PromptDoubleOptions pdoGapBYup = new PromptDoubleOptions("请输入梁左间距\r\n");
            pdoGapBYup.DefaultValue = 200;
            PromptDoubleOptions pdoGapBYdown = new PromptDoubleOptions("请输入梁右间距\r\n");
            pdoGapBYdown.DefaultValue = 200;
            PromptDoubleResult pdrCX = ed.GetDouble(pdoGapCX);
            PromptDoubleResult pdrCY = ed.GetDouble(pdoGapCY);
            PromptDoubleResult pdrBXup = ed.GetDouble(pdoGapBXup);
            PromptDoubleResult pdrBXdown = ed.GetDouble(pdoGapBXdown);
            PromptDoubleResult pdrBYup = ed.GetDouble(pdoGapBYup);
            PromptDoubleResult pdrBYdown = ed.GetDouble(pdoGapBYdown);
            #endregion
            #region 事务处理
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                #region 根据图层初步分类
                foreach (ObjectId oid in btr)
                {
                    Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent is Line)
                    {
                        if (ent.Layer == "DOTE" || ent.Layer == "AXIS") //图层是dote或axis分类为轴线
                        {
                            lineDote.Add((Line)ent);
                        }
                        else if (ent.Layer == "BEAM" || ent.Layer == "BEAM_CON") //图层是beam或beam_con分类为梁线
                        {
                            lineBeam.Add((Line)ent);
                        }
                    }
                    else if (ent is Polyline)
                    {
                        if (ent.Layer == "COLU" || ent.Layer == "COLLUMN")//图层是COLU分类为柱线
                        {
                            PlineCol.Add((Polyline)ent);
                        }
                        else if (ent.Layer == "Wall")//图层是Wall分类为墙线
                        {
                            PlineWall.Add((Polyline)ent);
                        }
                    }

                }

                for (int i = 0; i < lineDote.Count; i++)
                {
                    Vector3d vx = new Vector3d(1, 0, 0);
                    Vector3d vy = new Vector3d(0, 0, 0);
                    if (lineDote[i].StartPoint.GetVectorTo(lineDote[i].EndPoint).GetAngleTo(vx) < 0.3
                        && lineDote[i].StartPoint.GetVectorTo(lineDote[i].EndPoint).GetAngleTo(vx) > -0.3)
                    {
                        lineDoteX.Add(lineDote[i]);
                    }
                    else
                    {
                        lineDoteY.Add(lineDote[i]);
                    }
                }
                for (int i = 0; i < lineBeam.Count; i++)
                {
                    Vector3d vx = new Vector3d(1, 0, 0);
                    Vector3d vy = new Vector3d(0, 0, 0);
                    if (lineBeam[i].StartPoint.GetVectorTo(lineBeam[i].EndPoint).GetAngleTo(vx) < 0.3
                        && lineBeam[i].StartPoint.GetVectorTo(lineBeam[i].EndPoint).GetAngleTo(vx) > -0.3)
                    {
                        lineBeamX.Add(lineBeam[i]);
                    }
                    else
                    {
                        lineBeamY.Add(lineBeam[i]);
                    }
                }
                ed.WriteMessage("\r\n总共轴线{0}个\r\n", lineDote.Count);
                ed.WriteMessage("x方向轴线{0}个\r\n", lineDoteX.Count);
                ed.WriteMessage("y方向轴线{0}个\r\n", lineDoteY.Count);
                ed.WriteMessage("总共梁线{0}个\r\n", lineBeam.Count);
                ed.WriteMessage("x方向梁线{0}个\r\n", lineBeamX.Count);
                ed.WriteMessage("y方向梁线{0}个\r\n", lineBeamY.Count);
                ed.WriteMessage("总共有柱子{0}个\r\n", PlineCol.Count);
                ed.WriteMessage("总共有墙{0}个\r\n", PlineWall.Count);
                #endregion
                #region 处理X向梁柱
                for (int i = 0; i < lineDoteX.Count; i++)
                {
                    #region 处理X柱子
                    //首先按与轴线距离600的范围内的柱子划分到该轴线一组
                    List<Polyline> listColX = new List<Polyline>();
                    for (int j = 0; j < PlineCol.Count; j++)
                    {
                        double dis = lineDoteX[i].GetClosestPointTo(PlineCol[j].StartPoint, true).DistanceTo(PlineCol[j].StartPoint);
                        if (dis < gap)
                        {
                            listColX.Add(PlineCol[j]);
                        }
                    }
                    ed.WriteMessage("X向第{0}条轴线,有{1}个柱子\r\n", i, listColX.Count);
                    #region 给柱子编号
                    //for (int k = 0; k < listColX.Count; k++)
                    //{
                    //    DBText dbtext = new DBText();
                    //    dbtext.TextString = k.ToString();
                    //    dbtext.Position = listColX[k].StartPoint;
                    //    dbtext.Height = 100;
                    //    db.AddToModelSpace(dbtext);
                    //}
                    #endregion

                    for (int m = 0; m < listColX.Count; m++)
                    {
                        double dis = lineDoteX[i].GetClosestPointTo(listColX[m].StartPoint, true).DistanceTo(listColX[m].StartPoint);
                        if (dis != pdrCX.Value)
                        {
                            Point3d t = new Point3d(listColX[m].StartPoint.X, lineDoteX[i].StartPoint.Y + pdrCX.Value, listColX[m].StartPoint.Z);
                            db.MoveEnt(listColX[m].ObjectId, listColX[m].StartPoint, t);
                        }
                    }
                    #endregion

                    #region 处理X梁线
                    List<Line> listBeamXN = new List<Line>();
                    for (int j = 0; j < lineBeamX.Count; j++)
                    {
                        double dis = lineDoteX[i].GetClosestPointTo(lineBeamX[j].StartPoint, true).DistanceTo(lineBeamX[j].StartPoint);
                        if (dis < gap)
                        {
                            listBeamXN.Add(lineBeamX[j]);
                        }
                    }
                    for (int j = 0; j < listBeamXN.Count; j++)
                    {
                        double dis = lineDoteX[i].GetClosestPointTo(listBeamXN[j].StartPoint, true).DistanceTo(listBeamXN[j].StartPoint);
                        if (listBeamXN[j].StartPoint.Y >= lineDoteX[i].StartPoint.Y)//判断x向梁线是否在轴线之上
                        {
                            if (dis != pdrBXup.Value)//判断上梁线到轴线的距离
                            {
                                db.SetLineStartEndPointbyY(listBeamXN[j].ObjectId, lineDoteX[i].StartPoint.Y + pdrBXup.Value, lineDoteX[i].StartPoint.Y + pdrBXup.Value);
                            }
                        }
                        else
                        {
                            if (dis != pdrBXdown.Value)//判断下梁线到轴线的距离
                            {
                                db.SetLineStartEndPointbyY(listBeamXN[j].ObjectId, lineDoteX[i].StartPoint.Y - pdrBXdown.Value, lineDoteX[i].StartPoint.Y - pdrBXdown.Value);
                            }

                        }
                    }
                    #endregion
                }
                #endregion
                #region 处理Y向梁柱
                for (int i = 0; i < lineDoteY.Count; i++)
                {
                    #region 处理Y柱子
                    List<Polyline> listColY = new List<Polyline>();
                    for (int j = 0; j < PlineCol.Count; j++)
                    {
                        if (lineDoteY[i].GetClosestPointTo(PlineCol[j].StartPoint, true).DistanceTo(PlineCol[j].StartPoint) < gap)
                        {
                            listColY.Add(PlineCol[j]);
                        }
                    }
                    ed.WriteMessage("Y向第{0}条轴线,有{1}个柱子\r\n", i, listColY.Count);

                    for (int m = 0; m < listColY.Count; m++)
                    {
                        double dis = lineDoteY[i].GetClosestPointTo(listColY[m].StartPoint, true).DistanceTo(listColY[m].StartPoint);
                        if (dis != pdrCY.Value)
                        {
                            Point3d t = new Point3d(lineDoteY[i].StartPoint.X - pdrCY.Value, listColY[m].StartPoint.Y, listColY[m].StartPoint.Z);
                            db.MoveEnt(listColY[m].ObjectId, listColY[m].StartPoint, t);
                        }
                    }
                    #endregion
                    #region 处理Y梁线
                    List<Line> listBeamYN = new List<Line>();
                    for (int j = 0; j < lineBeamY.Count; j++)
                    {
                        double dis = lineDoteY[i].GetClosestPointTo(lineBeamY[j].StartPoint, true).DistanceTo(lineBeamY[j].StartPoint);
                        if (dis < gap)
                        {
                            listBeamYN.Add(lineBeamY[j]);
                        }
                    }
                    for (int j = 0; j < listBeamYN.Count; j++)
                    {
                        double dis = lineDoteY[i].GetClosestPointTo(listBeamYN[j].StartPoint, true).DistanceTo(listBeamYN[j].StartPoint);
                        if (listBeamYN[j].StartPoint.X >= lineDoteY[i].StartPoint.X)//判断y向梁线是否在轴线左
                        {
                            if (dis != pdrBYup.Value)//判断y向左梁线与轴线的距离
                            {
                                db.SetLineStartEndPointbyX(listBeamYN[j].ObjectId, lineDoteY[i].StartPoint.X + pdrBYup.Value, lineDoteY[i].StartPoint.X + pdrBYup.Value);

                            }
                        }
                        else
                        {
                            if (dis != pdrBYdown.Value)//判断y向右梁线与轴线的距离
                            {
                                db.SetLineStartEndPointbyX(listBeamYN[j].ObjectId, lineDoteY[i].StartPoint.X - pdrBYdown.Value, lineDoteY[i].StartPoint.X - pdrBYdown.Value);
                            }

                        }
                    }
                    #endregion

                }
                #endregion
                trans.Commit();
            }
            #endregion
        }
        /// <summary>
        /// 画全图负筋
        /// </summary>
        [CommandMethod("adfj")]
        public void adjustall3()
        {
            #region 定义变量
            double gap = 600;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<Line> lineDote = new List<Line>();//所有的轴线
            List<Line> lineDoteX = new List<Line>();//所有x向轴线
            List<Line> lineDoteY = new List<Line>();//所有y向轴线
            List<Line> lineBeam = new List<Line>();//所有的梁线
            List<Line> lineBeamX = new List<Line>();//所有x向梁线
            List<Line> lineBeamY = new List<Line>();//所有y向梁线

            List<Polyline> PlineWall = new List<Polyline>();//所有墙

            PromptDoubleOptions pdoGapBXup = new PromptDoubleOptions("请输入梁上间距\r\n");
            pdoGapBXup.DefaultValue = 200;
            PromptDoubleOptions pdoGapBXdown = new PromptDoubleOptions("请输入梁下间距\r\n");
            pdoGapBXdown.DefaultValue = 200;
            PromptDoubleOptions pdoGapBYup = new PromptDoubleOptions("请输入梁左间距\r\n");
            pdoGapBYup.DefaultValue = 200;
            PromptDoubleOptions pdoGapBYdown = new PromptDoubleOptions("请输入梁右间距\r\n");
            pdoGapBYdown.DefaultValue = 200;

            PromptDoubleResult pdrBXup = ed.GetDouble(pdoGapBXup);
            PromptDoubleResult pdrBXdown = ed.GetDouble(pdoGapBXdown);
            PromptDoubleResult pdrBYup = ed.GetDouble(pdoGapBYup);
            PromptDoubleResult pdrBYdown = ed.GetDouble(pdoGapBYdown);
            #endregion
            #region 事务处理
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                #region 根据图层初步分类
                foreach (ObjectId oid in btr)
                {
                    Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent is Line)
                    {
                        if (ent.Layer == "DOTE" || ent.Layer == "AXIS") //图层是dote或axis分类为轴线
                        {
                            lineDote.Add((Line)ent);
                        }
                        else if (ent.Layer == "BEAM" || ent.Layer == "BEAM_CON") //图层是beam或beam_con分类为梁线
                        {
                            lineBeam.Add((Line)ent);
                        }
                    }
                    else if (ent is Polyline)
                    {
                        if (ent.Layer == "Wall")//图层是Wall分类为墙线
                        {
                            PlineWall.Add((Polyline)ent);
                        }
                    }

                }

                for (int i = 0; i < lineDote.Count; i++)
                {
                    Vector3d vx = new Vector3d(1, 0, 0);
                    Vector3d vy = new Vector3d(0, 0, 0);
                    if (lineDote[i].StartPoint.GetVectorTo(lineDote[i].EndPoint).GetAngleTo(vx) < 0.3
                        && lineDote[i].StartPoint.GetVectorTo(lineDote[i].EndPoint).GetAngleTo(vx) > -0.3)
                    {
                        lineDoteX.Add(lineDote[i]);
                    }
                    else
                    {
                        lineDoteY.Add(lineDote[i]);
                    }
                }
                for (int i = 0; i < lineBeam.Count; i++)
                {
                    Vector3d vx = new Vector3d(1, 0, 0);
                    Vector3d vy = new Vector3d(0, 0, 0);
                    if (lineBeam[i].StartPoint.GetVectorTo(lineBeam[i].EndPoint).GetAngleTo(vx) < 0.3
                        && lineBeam[i].StartPoint.GetVectorTo(lineBeam[i].EndPoint).GetAngleTo(vx) > -0.3)
                    {
                        lineBeamX.Add(lineBeam[i]);
                    }
                    else
                    {
                        lineBeamY.Add(lineBeam[i]);
                    }
                }
                ed.WriteMessage("\r\n总共轴线{0}个\r\n", lineDote.Count);
                ed.WriteMessage("x方向轴线{0}个\r\n", lineDoteX.Count);
                ed.WriteMessage("y方向轴线{0}个\r\n", lineDoteY.Count);
                ed.WriteMessage("总共梁线{0}个\r\n", lineBeam.Count);
                ed.WriteMessage("x方向梁线{0}个\r\n", lineBeamX.Count);
                ed.WriteMessage("y方向梁线{0}个\r\n", lineBeamY.Count);

                ed.WriteMessage("总共有墙{0}个\r\n", PlineWall.Count);
                #endregion
                #region 处理X向梁柱
                for (int i = 0; i < lineDoteX.Count; i++)
                {

                    #region 处理X梁线
                    List<Line> listBeamXN = new List<Line>();
                    for (int j = 0; j < lineBeamX.Count; j++)
                    {
                        double dis = lineDoteX[i].GetClosestPointTo(lineBeamX[j].StartPoint, true).DistanceTo(lineBeamX[j].StartPoint);
                        if (dis < gap)
                        {
                            listBeamXN.Add(lineBeamX[j]);
                        }
                    }
                    for (int j = 0; j < listBeamXN.Count; j++)
                    {
                        double dis = lineDoteX[i].GetClosestPointTo(listBeamXN[j].StartPoint, true).DistanceTo(listBeamXN[j].StartPoint);
                        if (listBeamXN[j].StartPoint.Y >= lineDoteX[i].StartPoint.Y || listBeamXN[j].EndPoint.Y >= lineDoteX[i].StartPoint.Y)//判断x向梁线是否在轴线之上
                        {
                            db.AddZiZuoFuJinToModelSpace(listBeamXN[j], 750, pdrBXup.Value + pdrBXdown.Value);
                        }

                    }
                    #endregion
                }
                #endregion
                #region 处理Y向梁柱
                for (int i = 0; i < lineDoteY.Count; i++)
                {

                    #region 处理Y梁线
                    List<Line> listBeamYN = new List<Line>();
                    for (int j = 0; j < lineBeamY.Count; j++)
                    {
                        double dis = lineDoteY[i].GetClosestPointTo(lineBeamY[j].StartPoint, true).DistanceTo(lineBeamY[j].StartPoint);
                        if (dis < gap)
                        {
                            listBeamYN.Add(lineBeamY[j]);
                        }
                    }
                    for (int j = 0; j < listBeamYN.Count; j++)
                    {
                        double dis = lineDoteY[i].GetClosestPointTo(listBeamYN[j].StartPoint, true).DistanceTo(listBeamYN[j].StartPoint);
                        if (listBeamYN[j].StartPoint.X <= lineDoteY[i].StartPoint.X)//判断y向梁线是否在轴线左
                        {
                            db.AddZiZuoFuJinToModelSpace(listBeamYN[j], 750, pdrBYup.Value + pdrBYdown.Value);

                        }

                    }
                    #endregion
                }
                #endregion
                trans.Commit();
            }
            #endregion
        }
        /// <summary>
        /// 检查轴线间距
        /// </summary>
        [CommandMethod("cdx")]
        public void checkzhouxian()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<Line> doteList = new List<Line>();
            List<Line> xdoteList = new List<Line>();
            List<Line> ydoteList = new List<Line>();
            List<Line> xdoteListTemp = new List<Line>();
            List<Line> ydoteListTemp = new List<Line>();
            //double gap = 0.001;

            ed.WriteMessage("\r\n如果x，y不是完全水平和竖直，轴线显红色\r\n");
            ed.WriteMessage("如果x向轴线间距不是整数显绿色\r\n");
            ed.WriteMessage("如果y向轴线间距不是整数显洋红色\r\n");
            ed.WriteMessage("精度0.001\r\n");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                foreach (ObjectId item in btr)
                {
                    Entity ent = item.GetObject(OpenMode.ForWrite) as Entity;
                    if (ent is Line && (ent.Layer == "DOTE" || ent.Layer == "AXIS"))
                    {
                        doteList.Add((Line)ent);
                    }
                }
                for (int i = 0; i < doteList.Count; i++)
                {
                    if (doteList[i].Angle == 0)
                    {
                        xdoteList.Add(doteList[i]);
                    }
                    else if (doteList[i].Angle == Math.PI / 2)
                    {
                        ydoteList.Add(doteList[i]);
                    }
                    else
                    {
                        doteList[i].ColorIndex = 1;
                    }
                }
                xdoteListTemp = OrderEntityTools.PaiXuByX(xdoteList);
                for (int i = 0; i < xdoteListTemp.Count - 1; i++)
                {
                    if (Math.Abs(xdoteListTemp[i].StartPoint.Y - xdoteListTemp[i + 1].StartPoint.Y) % 1 >= 0.001)
                    {
                        xdoteListTemp[i].ColorIndex = 3;
                    }
                }
                ydoteListTemp = OrderEntityTools.PaiXuByY(ydoteList);

                for (int i = 0; i < ydoteListTemp.Count - 1; i++)
                {

                    if (Math.Abs(ydoteListTemp[i].StartPoint.X - ydoteListTemp[i + 1].StartPoint.X) % 1 >= 0.001)
                    {
                        ydoteListTemp[i].ColorIndex = 6;
                    }

                }



                trans.Commit();
            }

        }
        /// <summary>
        /// 轴线粉2等分
        /// </summary>
        [CommandMethod("sdx2")]
        public void makezhouxian2()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<Line> lineList = new List<Line>();
            List<Line> xlineList = new List<Line>();
            List<Line> ylineList = new List<Line>();
            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<or"),
                new TypedValue((int)DxfCode.LayerName, "AXIS"),
                new TypedValue((int)DxfCode.LayerName, "DOTE"),
                new TypedValue((int)DxfCode.Operator, "or>"), 
            };
            SelectionFilter filter = new SelectionFilter(tv);
            //PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    if (sset != null)
                    {
                        foreach (SelectedObject item in sset)
                        {
                            Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;

                            if (ent is Line)
                            {
                                lineList.Add((Line)ent);
                            }

                        }
                        for (int i = 0; i < lineList.Count; i++)
                        {
                            if (lineList[i].Angle < CadMathTools.DegreeToRad(10))
                            {
                                xlineList.Add(lineList[i]);
                            }
                            else
                            {
                                ylineList.Add(lineList[i]);
                            }
                        }
                        for (int i = 0; i < xlineList.Count - 1; i++)
                        {
                            Line line = new Line();
                            line.ColorIndex = 6;
                            double x0 = (xlineList[i].StartPoint.X + xlineList[i + 1].StartPoint.X) / 2;
                            double y0 = (xlineList[i].StartPoint.Y + xlineList[i + 1].StartPoint.Y) / 2;
                            line.StartPoint = new Point3d(x0, y0, 0);
                            double x1 = (xlineList[i].EndPoint.X + xlineList[i + 1].EndPoint.X) / 2;
                            double y1 = (xlineList[i].EndPoint.Y + xlineList[i + 1].EndPoint.Y) / 2;
                            line.EndPoint = new Point3d(x1, y1, 0);
                            db.AddToModelSpace(line);
                        }
                        for (int i = 0; i < ylineList.Count - 1; i++)
                        {
                            Line line = new Line();
                            line.ColorIndex = 6;
                            double x0 = (ylineList[i].StartPoint.X + ylineList[i + 1].StartPoint.X) / 2;
                            double y0 = (ylineList[i].StartPoint.Y + ylineList[i + 1].StartPoint.Y) / 2;
                            line.StartPoint = new Point3d(x0, y0, 0);
                            double x1 = (ylineList[i].EndPoint.X + ylineList[i + 1].EndPoint.X) / 2;
                            double y1 = (ylineList[i].EndPoint.Y + ylineList[i + 1].EndPoint.Y) / 2;
                            line.EndPoint = new Point3d(x1, y1, 0);
                            db.AddToModelSpace(line);
                        }

                    }

                }


                trans.Commit();
            }
        }
        /// <summary>
        /// 轴线分3等分
        /// </summary>
        [CommandMethod("sdx3")]
        public void makezhouxian3()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<Line> lineList = new List<Line>();
            List<Line> xlineList = new List<Line>();
            List<Line> ylineList = new List<Line>();
            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<or"),
                new TypedValue((int)DxfCode.LayerName, "AXIS"),
                new TypedValue((int)DxfCode.LayerName, "DOTE"),
                new TypedValue((int)DxfCode.Operator, "or>"), 
            };
            SelectionFilter filter = new SelectionFilter(tv);
            //PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    if (sset != null)
                    {
                        foreach (SelectedObject item in sset)
                        {
                            Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;

                            if (ent is Line)
                            {
                                lineList.Add((Line)ent);
                            }

                        }
                        for (int i = 0; i < lineList.Count; i++)
                        {
                            if (lineList[i].Angle < CadMathTools.DegreeToRad(10))
                            {
                                xlineList.Add(lineList[i]);
                            }
                            else
                            {
                                ylineList.Add(lineList[i]);
                            }
                        }
                        for (int i = 0; i < xlineList.Count - 1; i++)
                        {
                            Line line1 = new Line();
                            double x0 = (xlineList[i].StartPoint.X + 2 * xlineList[i + 1].StartPoint.X) / 3;
                            double y0 = (xlineList[i].StartPoint.Y + 2 * xlineList[i + 1].StartPoint.Y) / 3;
                            line1.StartPoint = new Point3d(x0, y0, 0);
                            double x1 = (xlineList[i].EndPoint.X + 2 * xlineList[i + 1].EndPoint.X) / 3;
                            double y1 = (xlineList[i].EndPoint.Y + 2 * xlineList[i + 1].EndPoint.Y) / 3;
                            line1.EndPoint = new Point3d(x1, y1, 0);
                            line1.ColorIndex = 6;
                            Line line12 = new Line();
                            double x02 = (2 * xlineList[i].StartPoint.X + 1 * xlineList[i + 1].StartPoint.X) / 3;
                            double y02 = (2 * xlineList[i].StartPoint.Y + 1 * xlineList[i + 1].StartPoint.Y) / 3;
                            line12.StartPoint = new Point3d(x02, y02, 0);
                            double x12 = (2 * xlineList[i].EndPoint.X + 1 * xlineList[i + 1].EndPoint.X) / 3;
                            double y12 = (2 * xlineList[i].EndPoint.Y + 1 * xlineList[i + 1].EndPoint.Y) / 3;
                            line12.EndPoint = new Point3d(x12, y12, 0);
                            line12.ColorIndex = 6;

                            db.AddToModelSpace(line1, line12);
                        }
                        for (int i = 0; i < ylineList.Count - 1; i++)
                        {
                            Line line1 = new Line();
                            double x0 = (ylineList[i].StartPoint.X + 2 * ylineList[i + 1].StartPoint.X) / 3;
                            double y0 = (ylineList[i].StartPoint.Y + 2 * ylineList[i + 1].StartPoint.Y) / 3;
                            line1.StartPoint = new Point3d(x0, y0, 0);
                            double x1 = (ylineList[i].EndPoint.X + 2 * ylineList[i + 1].EndPoint.X) / 3;
                            double y1 = (ylineList[i].EndPoint.Y + 2 * ylineList[i + 1].EndPoint.Y) / 3;
                            line1.EndPoint = new Point3d(x1, y1, 0);
                            line1.ColorIndex = 6;
                            Line line12 = new Line();
                            double x02 = (2 * ylineList[i].StartPoint.X + 1 * ylineList[i + 1].StartPoint.X) / 3;
                            double y02 = (2 * ylineList[i].StartPoint.Y + 1 * ylineList[i + 1].StartPoint.Y) / 3;
                            line12.StartPoint = new Point3d(x02, y02, 0);
                            double x12 = (2 * ylineList[i].EndPoint.X + 1 * ylineList[i + 1].EndPoint.X) / 3;
                            double y12 = (2 * ylineList[i].EndPoint.Y + 1 * ylineList[i + 1].EndPoint.Y) / 3;
                            line12.EndPoint = new Point3d(x12, y12, 0);
                            line12.ColorIndex = 6;

                            db.AddToModelSpace(line1, line12);

                        }

                    }

                }


                trans.Commit();
            }
        }
        /// <summary>
        /// 轴线分4等分
        /// </summary>
        [CommandMethod("sdx4")]
        public void makezhouxian4()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            List<Line> lineList = new List<Line>();
            List<Line> xlineList = new List<Line>();
            List<Line> ylineList = new List<Line>();
            TypedValue[] tv = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Operator, "<or"),
                new TypedValue((int)DxfCode.LayerName, "AXIS"),
                new TypedValue((int)DxfCode.LayerName, "DOTE"),
                new TypedValue((int)DxfCode.Operator, "or>"), 
            };
            SelectionFilter filter = new SelectionFilter(tv);
            //PromptSelectionOptions pso = new PromptSelectionOptions();
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                SelectionSet sset = psr.Value;
                if (psr.Status == PromptStatus.OK)
                {
                    if (sset != null)
                    {
                        foreach (SelectedObject item in sset)
                        {
                            Entity ent = item.ObjectId.GetObject(OpenMode.ForWrite) as Entity;

                            if (ent is Line)
                            {
                                lineList.Add((Line)ent);
                            }

                        }
                        for (int i = 0; i < lineList.Count; i++)
                        {
                            if (lineList[i].Angle < CadMathTools.DegreeToRad(10))
                            {
                                xlineList.Add(lineList[i]);
                            }
                            else
                            {
                                ylineList.Add(lineList[i]);
                            }
                        }
                        for (int i = 0; i < xlineList.Count - 1; i++)
                        {
                            Line line1 = new Line();
                            double x0 = (xlineList[i].StartPoint.X + 3 * xlineList[i + 1].StartPoint.X) / 4;
                            double y0 = (xlineList[i].StartPoint.Y + 3 * xlineList[i + 1].StartPoint.Y) / 4;
                            line1.StartPoint = new Point3d(x0, y0, 0);
                            double x1 = (xlineList[i].EndPoint.X + 3 * xlineList[i + 1].EndPoint.X) / 4;
                            double y1 = (xlineList[i].EndPoint.Y + 3 * xlineList[i + 1].EndPoint.Y) / 4;
                            line1.EndPoint = new Point3d(x1, y1, 0);
                            line1.ColorIndex = 6;

                            Line line12 = new Line();
                            double x02 = (2 * xlineList[i].StartPoint.X + 2 * xlineList[i + 1].StartPoint.X) / 4;
                            double y02 = (2 * xlineList[i].StartPoint.Y + 2 * xlineList[i + 1].StartPoint.Y) / 4;
                            line12.StartPoint = new Point3d(x02, y02, 0);
                            double x12 = (2 * xlineList[i].EndPoint.X + 2 * xlineList[i + 1].EndPoint.X) / 4;
                            double y12 = (2 * xlineList[i].EndPoint.Y + 2 * xlineList[i + 1].EndPoint.Y) / 4;
                            line12.EndPoint = new Point3d(x12, y12, 0);
                            line12.ColorIndex = 6;

                            Line line13 = new Line();
                            double x03 = (3 * xlineList[i].StartPoint.X + 1 * xlineList[i + 1].StartPoint.X) / 4;
                            double y03 = (3 * xlineList[i].StartPoint.Y + 1 * xlineList[i + 1].StartPoint.Y) / 4;
                            line13.StartPoint = new Point3d(x03, y03, 0);
                            double x13 = (3 * xlineList[i].EndPoint.X + 1 * xlineList[i + 1].EndPoint.X) / 4;
                            double y13 = (3 * xlineList[i].EndPoint.Y + 1 * xlineList[i + 1].EndPoint.Y) / 4;
                            line13.EndPoint = new Point3d(x13, y13, 0);
                            line13.ColorIndex = 6;

                            db.AddToModelSpace(line1, line12, line13);
                        }
                        for (int i = 0; i < ylineList.Count - 1; i++)
                        {
                            Line line1 = new Line();
                            double x0 = (ylineList[i].StartPoint.X + 3 * ylineList[i + 1].StartPoint.X) / 4;
                            double y0 = (ylineList[i].StartPoint.Y + 3 * ylineList[i + 1].StartPoint.Y) / 4;
                            line1.StartPoint = new Point3d(x0, y0, 0);
                            double x1 = (ylineList[i].EndPoint.X + 3 * ylineList[i + 1].EndPoint.X) / 4;
                            double y1 = (ylineList[i].EndPoint.Y + 3 * ylineList[i + 1].EndPoint.Y) / 4;
                            line1.EndPoint = new Point3d(x1, y1, 0);
                            line1.ColorIndex = 6;

                            Line line12 = new Line();
                            double x02 = (2 * ylineList[i].StartPoint.X + 2 * ylineList[i + 1].StartPoint.X) / 4;
                            double y02 = (2 * ylineList[i].StartPoint.Y + 2 * ylineList[i + 1].StartPoint.Y) / 4;
                            line12.StartPoint = new Point3d(x02, y02, 0);
                            double x12 = (2 * ylineList[i].EndPoint.X + 2 * ylineList[i + 1].EndPoint.X) / 4;
                            double y12 = (2 * ylineList[i].EndPoint.Y + 2 * ylineList[i + 1].EndPoint.Y) / 4;
                            line12.EndPoint = new Point3d(x12, y12, 0);
                            line12.ColorIndex = 6;

                            Line line13 = new Line();
                            double x03 = (3 * ylineList[i].StartPoint.X + 1 * ylineList[i + 1].StartPoint.X) / 4;
                            double y03 = (3 * ylineList[i].StartPoint.Y + 1 * ylineList[i + 1].StartPoint.Y) / 4;
                            line13.StartPoint = new Point3d(x03, y03, 0);
                            double x13 = (3 * ylineList[i].EndPoint.X + 1 * ylineList[i + 1].EndPoint.X) / 4;
                            double y13 = (3 * ylineList[i].EndPoint.Y + 1 * ylineList[i + 1].EndPoint.Y) / 4;
                            line13.EndPoint = new Point3d(x13, y13, 0);
                            line13.ColorIndex = 6;

                            db.AddToModelSpace(line1, line12, line13);

                        }

                    }

                }


                trans.Commit();
            }
        }
    }
}
