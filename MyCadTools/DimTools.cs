using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public class DimTools
    {
        [CommandMethod("dimv")]
        public void DimDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            RotatedDimension rd = new RotatedDimension();
            rd.XLine1Point = new Point3d(0, 0, 0);
            rd.XLine2Point = new Point3d(100, 0, 0);
            rd.DimLinePoint = new Point3d(0, 400, 0);
            db.AddToModelSpace(rd);
        }
    }
}
