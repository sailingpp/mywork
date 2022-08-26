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
    public static class CadMathTools
    {
        public static DBText GetSumText(this List<DBText> DbtextList)
        {
            DBText sumDBtext = new DBText();
            double sum=0;
            double x=0;
            double y=0;
           
            for (int i = 0; i < DbtextList.Count; i++)
            {
                sum +=Convert.ToDouble(DbtextList[i].TextString);
                x+=Convert.ToDouble(DbtextList[i].TextString)*Convert.ToDouble(DbtextList[i].Position.X);
                y+=Convert.ToDouble(DbtextList[i].TextString)*Convert.ToDouble(DbtextList[i].Position.Y);
            }
            x = x / sum;
            y = y / sum;
            sumDBtext.TextString = sum + "";
            sumDBtext.Position = new Point3d(x, y, 0);
            return sumDBtext;

        }
        public static double DegreeToRad(double degree)
        {
            return degree / 180 * Math.PI;
        }
    }
}
