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
    public static class Shape
    {
        /// <summary>
        /// 画桩
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="radius">圆半径</param>
        /// <param name="positon">位置</param>
        /// <returns></returns>
        public static Entity[] DrawPile(this Database db, int radius, Point3d positon)
        {
            Point3d pt3d = positon;
            Circle circle = new Circle();
            circle.Radius = radius;
            Line lineX = new Line();
            Line lineY = new Line();
            lineX.StartPoint = circle.Center - new Vector3d(circle.Radius, 0, 0);
            lineX.EndPoint = circle.Center + new Vector3d(circle.Radius, 0, 0);
            lineY.StartPoint = circle.Center - new Vector3d(0, circle.Radius, 0);
            lineY.EndPoint = circle.Center + new Vector3d(0, circle.Radius, 0);
            Entity[] ents = new Entity[] { circle, lineX, lineY };
            return ents;
        }
        /// <summary>
        /// 画承台
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="radius">圆半径</param>
        /// <param name="positon">位置</param>
        /// <returns></returns>
        public static Entity[] DrawOneCap(this Database db, int radius, Point3d positon)
        {
            Circle circle = new Circle();
            circle.Radius = radius;
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
            Entity[] ents = new Entity[] { circle, lineX, lineY, pl };
            return ents;
        }
        /// <summary>
        /// 画两桩承台
        /// </summary>
        /// <param name="db"></param>
        /// <param name="radius"></param>
        /// <param name="positon"></param>
        /// <returns></returns>
        public static Entity[] DrawTwoCap(this Database db, int radius, Point3d positon)
        {
            Point3d fisrtConner = Point3d.Origin + new Vector3d(-5 * radius, 2 * radius, 0);
            Point3d secondConner = Point3d.Origin + new Vector3d(5 * radius, 2 * radius, 0);
            Point3d thirdConner = Point3d.Origin + new Vector3d(5 * radius, -2 * radius, 0);
            Point3d fourthConner = Point3d.Origin + new Vector3d(-5 * radius, -2 * radius, 0);
            Point3dCollection vertices = new Point3dCollection() { fisrtConner, secondConner, thirdConner, fourthConner };
            Polyline3d pl = new Polyline3d(Poly3dType.SimplePoly, vertices, true);
            Entity[] leftPile = DrawOnePile(radius, new Vector3d(-3 * radius, 0, 0));
            Entity[] rightPile = DrawOnePile(radius, new Vector3d(3 * radius, 0, 0));

            Entity[] ents = new Entity[]
            {
                leftPile[0],leftPile[1],leftPile[2],rightPile[0],rightPile[1],rightPile[2], pl
            };


            return ents;
        }
        /// <summary>
        /// 画单桩可以复用
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="vector">圆心的位置</param>
        /// <returns></returns>
        public static Entity[] DrawOnePile(int radius, Vector3d vector)
        {

            Circle circle = new Circle(Point3d.Origin + vector, new Vector3d(0, 0, 1), radius);
            Line lineX = new Line();
            Line lineY = new Line();
            lineX.StartPoint = circle.Center - new Vector3d(circle.Radius, 0, 0);
            lineX.EndPoint = circle.Center + new Vector3d(circle.Radius, 0, 0);
            lineY.StartPoint = circle.Center - new Vector3d(0, circle.Radius, 0);
            lineY.EndPoint = circle.Center + new Vector3d(0, circle.Radius, 0);
            Entity[] ents = new Entity[] { circle, lineX, lineY };
            return ents;
        }
    }

}
