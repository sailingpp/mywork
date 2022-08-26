using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MyCadTools
{
    public class XdataDemo
    {
        public void Xdatademo(ObjectId oid)
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (DocumentLock dl = doc.LockDocument())
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    DBObject obj = trans.GetObject(oid, OpenMode.ForWrite);//id为要设置的实体的ObjectId，也可以对实体直接进行设置也就是obj可以为Polyline、BlockReference、MText、DBText、Hatch...
                    string regAppName = "YourAppName";//你的注册应用程序名
                    RegAppTable rat = (RegAppTable)trans.GetObject(db.RegAppTableId, OpenMode.ForWrite, false);
                    if (!rat.Has(regAppName))
                    {
                        rat.UpgradeOpen();
                        RegAppTableRecord ratr = new RegAppTableRecord();
                        ratr.Name = regAppName;
                        rat.Add(ratr);
                        trans.AddNewlyCreatedDBObject(ratr, true);
                    }
                    ResultBuffer resBuf = new ResultBuffer();
                    resBuf.Add(new TypedValue(1001, regAppName));
                    resBuf.Add(new TypedValue(1000, "YourContents"));
                    resBuf.Add(new TypedValue(1000, "YourContents"));
                    obj.XData = resBuf;
                    trans.Commit();
                }
            }
        }

    }
}
