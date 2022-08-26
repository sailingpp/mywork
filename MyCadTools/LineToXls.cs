using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;

namespace MyCadTools
{
    #region MyRegion
    //public static class LineToXls
    //{
    //    [CommandMethod("textline")]
    //    public static void toexcel()
    //    {
    //        Database db = HostApplicationServices.WorkingDatabase;
    //        System.Data.DataTable dt = new System.Data.DataTable();
    //        dt.TableName = "linetable";
    //        dt.Columns.Add("handle", typeof(string));
    //        dt.Columns.Add("startpointx", typeof(double));
    //        dt.Columns.Add("startpointy", typeof(double));
    //        dt.Columns.Add("startpointz", typeof(double));
    //        dt.Columns.Add("endpointx", typeof(double));
    //        dt.Columns.Add("endpointy", typeof(double));
    //        dt.Columns.Add("endpointz", typeof(double));
    //        using (Transaction tr = db.TransactionManager.StartTransaction())
    //        {
    //            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
    //            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
    //            foreach (ObjectId id in btr)
    //            {
    //                System.Data.DataRow dr = dt.NewRow();
    //                Line line = tr.GetObject(id, OpenMode.ForRead) as Line;
    //                if (line != null)
    //                {
    //                    dr[0] = line.Handle.ToString();
    //                    dr[1] = line.StartPoint.X;
    //                    dr[2] = line.StartPoint.Y;
    //                    dr[3] = line.StartPoint.Z;
    //                    dr[4] = line.EndPoint.X;
    //                    dr[5] = line.EndPoint.Y;
    //                    dr[6] = line.EndPoint.Z;
    //                }
    //                dt.Rows.Add(dr);
    //            }
    //            tr.Commit();

    //        }
    //        saveto(dt, @"C:\Users\Administrator\Desktop\myexcel.xlsx");

    //    }
    //    public static void saveto(System.Data.DataTable dt, string fileName)
    //    {
    //        int columnIndex = 1;
    //        int rowIndex = 1;
    //        Microsoft.Office.Interop.Excel._Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
    //        xlApp.DefaultFilePath = fileName;
    //        xlApp.DisplayAlerts = true;
    //        xlApp.SheetsInNewWorkbook = 1;
    //        Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
    //        foreach (System.Data.DataColumn column in dt.Columns)
    //        {
    //            xlApp.Cells[rowIndex, columnIndex] = column.ColumnName;
    //            columnIndex++;
    //        }
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            columnIndex = 1;
    //            rowIndex++;
    //            for (int j = 0; j < dt.Columns.Count; j++)
    //            {
    //                xlApp.Cells[rowIndex, columnIndex] = dt.Rows[i][j].ToString();
    //                columnIndex++;

    //            }
    //        }
    //        xlBook.SaveCopyAs(fileName);
    //        xlApp = null;
    //        xlBook = null;
    //    }
    //}
    #endregion
    
}
