using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MyCadTools
{
    public static class PlToPointTool
    {
        public static Point3dCollection PltoPint3dCollectionMethod(Polyline pl)
        {
            Point3dCollection ptc = new Point3dCollection();
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                ptc.Add(pl.GetPoint3dAt(i));
            }
            return ptc;
        }
        public static List<Point3d> PltoPint3dList(Polyline pl)
        {
            List<Point3d> ptList = new List<Point3d>();
            Point3dCollection ptc = PltoPint3dCollectionMethod(pl);
            foreach (Point3d item in ptc)
            {
                ptList.Add(item);
            }
            return ptList;
        }
        public static Point2dCollection PltoPint2dCollectionMethod(Polyline pl)
        {
            Point2dCollection ptc = new Point2dCollection();
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                ptc.Add(pl.GetPoint2dAt(i));
            }
            return ptc;
        }
        public static Point3d GetLeftUpPoint(Polyline pl)
        {
            Point3d p = new Point3d();
            List<Point3d> ptList = PltoPint3dList(pl);
            var temp = from temppoint in ptList
                       orderby temppoint.X
                       select temppoint;

            return p;

        }
        public static List<DBText> SortDbtext(List<DBText> target)
        {

            List<DBText> dbtext = new List<DBText>();
            var temp = from tempdbtext in target
                       orderby tempdbtext.Position.X
                       orderby tempdbtext.Position.Y
                       select tempdbtext;
            foreach (var item in temp)
            {
                dbtext.Add(item);
            }

            return dbtext;

        }
        public static double GetSteelArea(string str)
        {
            string[] sArray = str.Split(new string[] { "%%132" }, StringSplitOptions.RemoveEmptyEntries);

            return Convert.ToDouble(sArray[0]) * Convert.ToDouble(sArray[1]) * Convert.ToDouble(sArray[1]) * 3.14 / 4;
        }
    }
}
