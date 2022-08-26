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
    public class GetCrossDotsDemo
    {
        [CommandMethod("getcdt")]
        public void GetPointTest()
        {
            Line3d line1 = new Line3d(new Point3d(0, 0, 0), new Point3d(1, 1, 0));
            Line3d line2 = new Line3d(new Point3d(1, 0, 0), new Point3d(0, 1, 0));

            Point3d[] pt = GetPointMethod(line1, line2);
            Application.ShowAlertDialog(pt[0].X.ToString() + pt[0].Y.ToString());
        }
        public Point3d[] GetPointMethod(Line3d line1, Line3d line2)
        {
            Point3d[] ptc;
            ptc = line1.IntersectWith(line2);
            return ptc;
        }
        public Line3d LineToLine3d(Line line)
        {
            Line3d line3d = new Line3d(line.StartPoint, line.EndPoint);
            return line3d;
        }
        public Point3d[] GetPointMethod(Line3d line1, Line3d[] line2)
        {
            Point3d[] ptc = new Point3d[line2.Length];
            for (int i = 0; i < line2.Length; i++)
            {
                ptc = line1.IntersectWith(line2[i]);
            }
            return ptc;
        }
    }
}
