using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//选择多个点按角度排序生成多段线

namespace MyCadTools
{
    public static class PointToPolylineTools
    {
        //选择多个点按角度排序生成多段线
        [CommandMethod("ptp")]
        public static void SelectPointToMakePolylineMethod1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<Point3d> point3dList = new List<Point3d>();
            List<DBText> dbtextList = new List<DBText>();

            TypedValue[] acTypValAr = new TypedValue[1];

            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);

            SelectionFilter acSelFtr = new SelectionFilter(acTypValAr);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                PromptSelectionResult psr = ed.GetSelection(acSelFtr);
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet set = psr.Value;
                    foreach (SelectedObject item in set)
                    {
                        if (item != null)
                        {
                            DBText dbtext = trans.GetObject(item.ObjectId, OpenMode.ForWrite) as DBText;
                            dbtextList.Add(dbtext);
                            point3dList.Add(dbtext.Position);
                        }
                    }
                    db.AddToModelSpace(GetPolylineByAngle(point3dList));
                }
                trans.Commit();
            }
        }
        public static Polyline3d GetPolylineByPoint(List<Point3d> point3dList)
        {
            Point3dCollection ptc = new Point3dCollection();
            for (int i = 0; i < point3dList.Count; i++)
            {
                ptc.Add(point3dList[i]);
            }
            Polyline3d pl = new Polyline3d(0, ptc, true);
            return pl;
        }
        public static Polyline3d GetPolylineByDistance(List<Point3d> point3dList)
        {

            List<Point3d> doubleList = new List<Point3d>();
            Dictionary<double, Point3d> myDic = new Dictionary<double, Point3d>();
            for (int i = 0; i < point3dList.Count; i++)
            {

                myDic.Add((point3dList[0].GetVectorTo(point3dList[i])).Length, point3dList[i]);


            }
            var tempList = from item in myDic
                           orderby myDic.Keys
                           select item;

            foreach (KeyValuePair<double, Point3d> item in tempList)
            {
                doubleList.Add(item.Value);
            }
            Polyline3d pl = GetPolylineByPoint(doubleList);

            return pl;
        }
        public static Polyline3d GetPolylineByAngle(List<Point3d> point3dList)
        {
            List<Point3d> doubleList = new List<Point3d>();
            var tempPoint3dList = from item in point3dList
                                  orderby item.X
                                  select item;

            tempPoint3dList = from item in point3dList
                              orderby item.Y
                              select item;
            Dictionary<double, Point3d> myDic = new Dictionary<double, Point3d>();
            foreach (Point3d item in tempPoint3dList)
            {

                myDic.Add((point3dList[0].GetVectorTo(item)).GetAngleTo(new Vector3d(1, 0, 0)), item);


            }

            var tempList = from item in myDic
                           orderby item.Key
                           select item;

            //  var dicSort = from objDic in myDic
            //orderby objDic.Value
            //select objDic;

            foreach (KeyValuePair<double, Point3d> item in tempList)
            {
                doubleList.Add(item.Value);
            }
            Polyline3d pl = GetPolylineByPoint(doubleList);

            return pl;
        }
    }
}
