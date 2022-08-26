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
    public static class StructrueTools
    {
        /// <summary>
        /// 将实体生成块
        /// </summary>
        /// <param name="db"></param>
        /// <param name="blockName">块名</param>
        /// <param name="layerName">图层名</param>
        /// <param name="position">块的插入点</param>
        /// <param name="rotateAngle">块的旋转角度</param>
        /// <param name="ents">要生成的块的实体</param>
        public static void MakeEntityBlock(this Database db, string blockName, string layerName, Point3d position, double rotateAngle, params Entity[] ents)
        {
            db.AddBlock(blockName, ents);
            db.InsertBlock(blockName, layerName,position, rotateAngle);
        }
    }
}
