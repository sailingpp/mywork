using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//添加图层
namespace MyCadTools
{
    public class LayerDemo
    {
        [CommandMethod("addlayer")]
        public void AddLayerDemo()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            db.AddLayerTool("211");
        }
    }
}
