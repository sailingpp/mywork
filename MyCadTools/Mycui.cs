using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public class Mycui
    {
        //C:\Users\Administrator\AppData\Roaming\Autodesk\AutoCAD 2010\R18.0\chs\Support\acad.cuix
        [CommandMethod("mycui")]
        public static void SelectObjectsOnscreen()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            //自定义的组名
            
            string strMyGroupName = "MyGroup";
            //保存的CUI文件名（从CAD2010开始，后缀改为了cuix）
            string strCuiFileName = "MyMenu.cuix";
            //创建一个自定义组（这个组中将包含我们自定义的命令、菜单、工具栏、面板等）
            CustomizationSection myCSection = new CustomizationSection();
            myCSection.MenuGroupName = strMyGroupName;
            //创建自定义命令组
            MacroGroup mg = new MacroGroup("MyMethod", myCSection.MenuGroup);
            MenuMacro mm0101 = new MenuMacro(mg, "打开文件", "OF", "");
            MenuMacro mm0102 = new MenuMacro(mg, "打开模板", "OM", "");
            MenuMacro mm0103 = new MenuMacro(mg, "保存", "SV", "");

            MenuMacro mm02 = new MenuMacro(mg, "工具箱窗体", "pp", "");

            MenuMacro mm0301 = new MenuMacro(mg, "加括号", "AddKuoHao", "");
            MenuMacro mm0302 = new MenuMacro(mg, "去括号", "DelKuoHao", "");
            MenuMacro mm0303 = new MenuMacro(mg, "加前缀", "AddQianZhui", "");
            MenuMacro mm0304 = new MenuMacro(mg, "去前缀", "DelQianZhui", "");
            MenuMacro mm0305 = new MenuMacro(mg, "+数字", "AddNum", "");
            MenuMacro mm0306 = new MenuMacro(mg, "*数字", "MulNum", "");

            MenuMacro mm04 = new MenuMacro(mg, "标高检查", "HeightCheck", "");

            MenuMacro mm05 = new MenuMacro(mg, "检查图名", "CheckTuMing", "");

            MenuMacro mm06 = new MenuMacro(mg, "生成目录", "Mulu", "");

            //声明菜单别名
            StringCollection scMyMenuAlias = new StringCollection();
            scMyMenuAlias.Add("MyPop");
            scMyMenuAlias.Add("MyTestPop");
            //菜单项（将显示在项部菜单栏中）
            PopMenu pmParent = new PopMenu("愚公移山(&Y)", scMyMenuAlias, "愚公移山(&Y)", myCSection.MenuGroup);

            //子项的菜单（多级）
            PopMenu pm01 = new PopMenu("打开", new StringCollection(), "", myCSection.MenuGroup);
            PopMenuRef pmr01 = new PopMenuRef(pm01, pmParent, -1);
            PopMenuItem pmi01 = new PopMenuItem(mm0101, "文件", pm01, -1);
            PopMenuItem pmi02 = new PopMenuItem(mm0102, "模板", pm01, -1);
            PopMenuItem pmi03 = new PopMenuItem(mm0103, "保存(&S)", pm01, -1);
            //子项的菜单（单级）

            PopMenuItem pm02 = new PopMenuItem(mm02, "工具箱窗体", pmParent, -1);

            PopMenu pm03 = new PopMenu("文字处理", new StringCollection(), "", myCSection.MenuGroup);
            PopMenuRef pmr03 = new PopMenuRef(pm03, pmParent, -1);
            PopMenuItem pmi031 = new PopMenuItem(mm0301, "加括号", pm03, -1);
            PopMenuItem pmi032 = new PopMenuItem(mm0302, "去括号", pm03, -1);
            PopMenuItem pmi033 = new PopMenuItem(mm0303, "加前缀", pm03, -1);
            PopMenuItem pmi034 = new PopMenuItem(mm0304, "去前缀", pm03, -1);
            PopMenuItem pmi035 = new PopMenuItem(mm0305, "+数字", pm03, -1);
            PopMenuItem pmi036 = new PopMenuItem(mm0306, "*数字", pm03, -1);

            PopMenuItem pm04 = new PopMenuItem(mm04, "标高检查", pmParent, -1);

            PopMenuItem pm05 = new PopMenuItem(mm04, "检查图名", pmParent, -1);

            PopMenuItem pm06 = new PopMenuItem(mm04, "生成目录", pmParent, -1);

            ed.WriteMessage("cuix生成成功!");
            myCSection.SaveAs(strCuiFileName);
        }
    }
}
