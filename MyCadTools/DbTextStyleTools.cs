using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCadTools
{
    public static class DbTextStyleTools
    {
        /// <summary>
        /// 增加字体样式
        /// </summary>
        /// <param name="db"></param>
        /// <param name="styleName">TSSD_Rein</param>
        /// <param name="fontFilename">tssdeng</param>
        /// <param name="bigFontFilename">hztxt</param>
        /// <returns></returns>
        public static ObjectId AddTxtStyle(this Database db, string styleName, string fontFilename, string bigFontFilename)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TextStyleTable tst = trans.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;
                if (!tst.Has(styleName))
                {
                    TextStyleTableRecord ttr = new TextStyleTableRecord();
                    ttr.Name = styleName;//设置文字样式名
                    ttr.FileName = fontFilename;//设置字体文件名
                    ttr.BigFontFileName = bigFontFilename;//设置大字体文件名
                    tst.UpgradeOpen();
                    oid = tst.Add(ttr);
                    trans.AddNewlyCreatedDBObject(ttr, true);
                    tst.DowngradeOpen();
                }
                else
                {
                    oid = tst[styleName];
                }
                trans.Commit();
            }
            return oid;
        }
        /// <summary>
        /// 改变字体的样式
        /// </summary>
        /// <param name="db"></param>
        /// <param name="styleName"></param>
        /// <param name="fontFilename"></param>
        /// <param name="bigFontFilename"></param>
        /// <returns></returns>
        public static ObjectId ChangeTxtStyle(this Database db, string styleName, string shapeFilename)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId oid = ObjectId.Null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TextStyleTable tst = trans.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;
                if (!tst.Has(styleName))
                {
                    TextStyleTableRecord ttr = new TextStyleTableRecord();
                    ttr = tst[styleName].GetObject(OpenMode.ForWrite) as TextStyleTableRecord;
                    ttr.Name = styleName;//设置文字样式名
                    ttr.FileName = shapeFilename;//设置字体文件名
                    ttr.IsShapeFile = true;//文件为形定义文件
                    tst.UpgradeOpen();
                    oid = tst.Add(ttr);
                    ed.WriteMessage("文字样式修改成功！\r\n");
                    trans.AddNewlyCreatedDBObject(ttr, true);
                    tst.DowngradeOpen();
                }
                else
                {
                    oid = tst[styleName];
                    ed.WriteMessage("文字样式已经存在！\r\n");

                }
                trans.Commit();
            }
            return oid;
        }
    }
}
