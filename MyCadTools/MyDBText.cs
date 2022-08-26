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
    public class MyDBText
    {

        private Database _db;
        public Database Db
        {
            get 
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                return db;
            }
            set { _db = value; }
        }


        private DBText _dbtext;
        public DBText Dbtext
        {
            get { return _dbtext; }
            set { _dbtext = value; }
        }

        
        private string _layerName;
        public string LayerName
        {
            get { return _layerName; }
            set { _layerName = value; }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set { _height =value; }
        }

        private Point3d _position;
        public Point3d Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private string _textString;
        public string TextString
        {
            get { return _textString; }
            set { _textString = value; }
        }

        private double _widthFactor;
        public double WidthFactor
        {
            get { return _widthFactor; }
            set { _widthFactor = value; }
        }



        private ObjectId _textStyleId;
        public ObjectId TextStyleId
        {
            get
            {
                return  this.Db.AddTxtStyle("TSSD_Norm", "tssdeng", "hztxt");  ; 
            }
            set { _textStyleId = value ; }
        }

        public MyDBText()
        {
 
        }
    }
}
