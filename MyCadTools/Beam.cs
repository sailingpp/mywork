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
    public static class Beam
    {
        [CommandMethod("ebeam")]
        public static void DelBeam()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            double gap = 50;

            List<Line> BeamList = new List<Line>();
            List<Line> DoteList = new List<Line>();
            List<Line> Beam_conList = new List<Line>();
            List<Line> AxisList = new List<Line>();
            List<Polyline> ColList = new List<Polyline>();
            List<Polyline> WallList = new List<Polyline>();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                DelChongDie(gap, BeamList, DoteList, Beam_conList, AxisList, ColList, WallList, btr);


                trans.Commit();
            }

        }

        private static void DelChongDie(double gap, List<Line> BeamList, List<Line> DoteList, List<Line> Beam_conList, List<Line> AxisList, List<Polyline> ColList, List<Polyline> WallList, BlockTableRecord btr)
        {
            foreach (ObjectId item in btr)
            {
                Entity ent = item.GetObject(OpenMode.ForWrite) as Entity;
                if (ent is Line)
                {
                    if (ent.Layer == "BEAM")
                    {
                        BeamList.Add((Line)ent);
                    }
                    else if (ent.Layer == "DOTE")
                    {
                        DoteList.Add((Line)ent);
                    }
                    else if (ent.Layer == "BEAM_CON")
                    {
                        Beam_conList.Add((Line)ent);
                    }
                    else if (ent.Layer == "AXIS")
                    {
                        AxisList.Add((Line)ent);
                    }
                }
                else if (ent is Polyline)
                {
                    if (ent.Layer == "COLU")
                    {
                        ColList.Add((Polyline)ent);
                    }
                    else
                    {
                        WallList.Add((Polyline)ent);
                    }
                }

                for (int i = 0; i < BeamList.Count; i++)
                {
                    for (int j = i + 1; j < BeamList.Count; j++)
                    {
                        if (BeamList[i].Angle == BeamList[j].Angle && BeamList[i].StartPoint.DistanceTo(BeamList[j].StartPoint) <= gap)
                        {
                            BeamList[j].Erase();
                        }
                    }
                }
                for (int i = 0; i < DoteList.Count; i++)
                {
                    for (int j = i + 1; j < DoteList.Count; j++)
                    {
                        if (DoteList[i].Angle == DoteList[j].Angle && DoteList[i].StartPoint.DistanceTo(DoteList[j].StartPoint) <= gap)
                        {
                            DoteList[j].Erase();
                        }
                    }
                }
                for (int i = 0; i < Beam_conList.Count; i++)
                {
                    for (int j = i + 1; j < Beam_conList.Count; j++)
                    {
                        if (Beam_conList[i].Angle == Beam_conList[j].Angle && Beam_conList[i].StartPoint.DistanceTo(Beam_conList[j].StartPoint) <= gap)
                        {
                            Beam_conList[j].Erase();
                        }
                    }
                }
                for (int i = 0; i < AxisList.Count; i++)
                {
                    for (int j = i + 1; j < AxisList.Count; j++)
                    {
                        if (AxisList[i].Angle == AxisList[j].Angle && AxisList[i].StartPoint.DistanceTo(AxisList[j].StartPoint) <= gap)
                        {
                            AxisList[j].Erase();
                        }
                    }
                }

                for (int i = 0; i < ColList.Count; i++)
                {
                    for (int j = i + 1; j < ColList.Count; j++)
                    {
                        if (ColList[i].StartPoint == ColList[j].StartPoint && ColList[i].EndPoint == ColList[j].EndPoint)
                        {
                            ColList[j].Erase();
                        }
                    }
                }
                for (int i = 0; i < WallList.Count; i++)
                {
                    for (int j = i + 1; j < WallList.Count; j++)
                    {
                        if (WallList[i].StartPoint == WallList[j].StartPoint && WallList[i].EndPoint == WallList[j].EndPoint)
                        {
                            WallList[j].Erase();
                        }
                    }
                }

            }
        }


    }
}
