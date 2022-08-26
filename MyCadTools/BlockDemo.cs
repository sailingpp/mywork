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
    public class BlockDemo
    {
        //名字可以自定义，最好不要和CAD快捷键冲突
        [CommandMethod("cad")]
        public static void cad()
        {
            // InsertBlock.insert_block();
        }
    }
}
