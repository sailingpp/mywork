using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public static class OrderEntityTools
    {
        public static void OrdereEntityByX(List<DBText> dbtextList)
        {
            //var query = from o in dbtextList
            //        orderby o.Position.X
            //        select o;
            //return query;
        }
        public static List<Point3d> PaiXuByY(List<Point3d> Sourceptc)
        {
            List<Point3d> p3dList = new List<Point3d>();
            var temp = from item in Sourceptc orderby item.Y select item;
            p3dList.AddRange(temp);
            return p3dList;
        }
        public static List<Point3d> PaiXuByX(List<Point3d> Sourceptc)
        {
            List<Point3d> p3dList = new List<Point3d>();
            var temp = from item in Sourceptc orderby item.X select item;
            p3dList.AddRange(temp);
            return p3dList;
        }
        public static List<Line> PaiXuByY(List<Line> SourceLine)
        {
            List<Line> targetLineList = new List<Line>();
            var temp = from item in SourceLine orderby item.StartPoint.Y select item;
            targetLineList.AddRange(temp);
            return targetLineList;
        }
        public static List<Line> PaiXuByX(List<Line> SourceLine)
        {
            List<Line> targetLineList = new List<Line>();
            var temp = from item in SourceLine orderby item.StartPoint.X select item;
            targetLineList.AddRange(temp);
            return targetLineList;
        }
    }
}
