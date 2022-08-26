using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Collections;
using System.Text.RegularExpressions;

namespace MyCadTools
{
    public partial class StructureBox : Form
    {
        public StructureBox()
        {
            InitializeComponent();
        }

        private void btnPile_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.MakePileDemo();
        }

        private void btnOnecap_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.MakeOneCapDemo();
        }

        private void btnTwoCap_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.MakeTwoCapDemo();
        }

        private void btnCheckTuMing_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.CheckTuming();
        }

        private void btnMulu_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.GetMulu();
        }

        private void btnChangeLayerName_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.ChangeLayerNameDemo();
        }

        private void btnGetByColor_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.SelectAllbyColorDemo();
        }

        private void btnAddKuoHao_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.AddKuoHao();
        }

        private void btnDelKuoHao_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.DelKuoHao();
        }

        private void btnAddQianZhui_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.AddQianZhui();
        }

        private void btnAddHouZhui_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.AddHouZhui();
        }

        private void btnDelQianZhui_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.DelQianZhui();
        }

        private void btnAddNum_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.AddNum();
        }

        private void btnMulNum_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.MulNum();
        }

        private void btnChangeColor_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.ChangeDbtextColor();
        }

        private void btnDrawColTable_Click(object sender, EventArgs e)
        {
            DrawColTable dct = new DrawColTable();
            dct.DrawColTableDemo();
        }

        private void btnCalWallTable_Click(object sender, EventArgs e)
        {
            WallTable wt = new WallTable();
            wt.CalWallDemo();
        }

        private void btnCalColTable_Click(object sender, EventArgs e)
        {
            ColTable calcol = new ColTable();
            calcol.CalColDemo2();
        }

        private void btnDelCalResult_Click(object sender, EventArgs e)
        {
            DrawColTable dwc = new DrawColTable();
            dwc.ClearColTableDemo();
        }

        private void btnDrawWallTable_Click(object sender, EventArgs e)
        {
            DrawWallTable dwt = new DrawWallTable();
            dwt.DrawWallTableDemo();
        }

        private void btnGetSum_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.GetDbtextSum();
        }

        private void btnKangFu_Click(object sender, EventArgs e)
        {
            KangfuDemo kangfu = new KangfuDemo();
            kangfu.Kangfudemo();
        }

        private void btnChangeText_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.ChangeTextDemo();
        }

        private void btnCompareDbText_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.dbtextCompareTextDemo();
        }

        private void btnCompareMtext_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.mtextCompareTextDemo();
        }


        private void btnFanWeiXuanShu_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.FanWeiXuanShu();
        }

        private void btnGetRaForce_Click(object sender, EventArgs e)
        {
            GetRaDemo.GetRaVersion1();
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {
        }

        private void button64_Click(object sender, EventArgs e)
        {

        }


        private void btnMouseMonitor_Click(object sender, EventArgs e)
        {
            CadDemo.StartMonitorDemo();
        }

        private void btnBingText_Click(object sender, EventArgs e)
        {
            MoveEntityDemo moveent = new MoveEntityDemo();
            moveent.BindText();
        }

        private void btnMoveHeight2_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.MoveHeightDemo2();
        }

        private void btnMoveHeight_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.MoveHeightDemo();
        }

        private void btnCheckHeight_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.HeightCheckDemo();
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void btnDrawPileDetail_Click(object sender, EventArgs e)
        {
            CadDemo.DrawPileDetail();
        }

        private void btnGetRaForce2_Click(object sender, EventArgs e)
        {
            GetRaDemo.GetRaVersion2();

        }

        private void btnGetForce4_Click(object sender, EventArgs e)
        {
            GetRaDemo.GetRaVersion4();
        }

        private void btnGetRaForce3_Click(object sender, EventArgs e)
        {
            GetRaDemo.GetRaVersion3();
        }

        private void btnColX_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.tcx();
        }

        private void btnBeamX_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.tbx();

        }

        private void btnColY_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.tcy();
        }

        private void btnAxisX_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.tdx();
        }

        private void btnAxisAdjustX_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.adx();
        }

        private void btnBeamY_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.tby();
        }

        private void 网络ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAxisY_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.tdy();
        }

        private void btnAxisAdjustY_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.ady();
        }

        private void btnDiv2Axis_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.makezhouxian2();
        }

        private void btnDiv3Axis_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.makezhouxian3();
        }

        private void btnDiv4Axis_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.makezhouxian4();
        }

        private void btnDelBCX_Click(object sender, EventArgs e)
        {
            Beam.DelBeam();
        }

        private void btnZhiZuoFuJin_Click(object sender, EventArgs e)
        {
            ZhiZuoFuJin zzfj = new ZhiZuoFuJin();
            zzfj.AddZiZuoFuJin();
        }

        private void btnAllFuJin_Click(object sender, EventArgs e)
        {
            AdjustStructreDemo asd = new AdjustStructreDemo();
            asd.adjustall3();
        }

        private void btnToSap2000_Click(object sender, EventArgs e)
        {
            #region MyRegion
            // double k0 = Convert.ToDouble(textBox_K0.Text);
            //double height_Soil = Convert.ToDouble(textBox3.Text);
            //double height_Water = Convert.ToDouble(textBox4.Text);
            //double height_plan_top = Convert.ToDouble(textBox1.Text);
            //double height_plan_bot = Convert.ToDouble(textBox6.Text);
            //double section_b = Convert.ToDouble(textBox2.Text);
            //double section_h = Convert.ToDouble(textBox5.Text);
            //string path = textBox7.Text;
            //Wall wall = new Wall(section_b, section_h, height_plan_top, height_plan_bot, height_Soil, height_Water, 5);
            //#region demo3
            ////dimension variables

            //SAP2000v15.SapObject mySapObject;

            //SAP2000v15.cSapModel mySapModel;

            //int ret;

            //int i;

            //double[] ModValue;

            //double[] PointLoadValue;

            //bool[] Restraint;

            //string[] FrameName;

            //string[] PointName;
            //#region MyRegion

            ////int NumberResults;

            ////string[] Obj;

            ////string[] Elm;

            ////string[] LoadCase;

            ////string[] StepType;

            ////double[] StepNum;

            ////double[] U1;

            ////double[] U2;

            ////double[] U3;

            ////double[] R1;

            ////double[] R2;

            ////double[] R3;

            ////double[] SapResult;

            ////double[] IndResult;

            ////double[] PercentDiff;

            ////string[] SapResultString;

            ////string[] IndResultString;

            ////string[] PercentDiffString;

            ////string msg;
            //#endregion


            //string temp_string1;

            //string temp_string2;

            //bool temp_bool;

            ////create Sap2000 object

            //mySapObject = new SAP2000v15.SapObject();

            ////start Sap2000 application

            //temp_bool = true;

            //mySapObject.ApplicationStart(SAP2000v15.eUnits.kN_m_C, temp_bool, "");

            ////create SapModel object

            //mySapModel = mySapObject.SapModel;

            ////initialize model

            //ret = mySapModel.InitializeNewModel((SAP2000v15.eUnits.kN_m_C));

            ////create new blank model

            //ret = mySapModel.File.NewBlank();

            ////define material property

            //ret = mySapModel.PropMaterial.SetMaterial("CONC", SAP2000v15.eMatType.MATERIAL_CONCRETE, -1, "", "");

            ////assign isotropic mechanical properties to material

            //ret = mySapModel.PropMaterial.SetMPIsotropic("CONC", 3600, 0.2, 0.0000055, 0);

            ////define rectangular frame section property

            //ret = mySapModel.PropFrame.SetRectangle("b1x0.3", "CONC", wall.Section_height, wall.Section_width, -1, "", "");

            ////define frame section property modifiers
            ////定义截面修正

            //ModValue = new double[8];

            //for (i = 0; i <= 7; i++)
            //{

            //    ModValue[i] = 1;

            //}

            //ModValue[0] = 1000;

            //ModValue[1] = 0;

            //ModValue[2] = 0;

            //double[] temp_SystemArray = ModValue;

            //ret = mySapModel.PropFrame.SetModifiers("B1X0.3", ref temp_SystemArray);

            ////switch to k-ft units

            //ret = mySapModel.SetPresentUnits(SAP2000v15.eUnits.kN_m_C);

            ////add frame object by coordinates

            //FrameName = new string[3];

            //temp_string1 = FrameName[0];

            //temp_string2 = FrameName[0];

            //ret = mySapModel.FrameObj.AddByCoord(0, 0, 0, 0, 0, wall.Wall_length, ref temp_string1, "B1X0.3", "1", "Global");

            //FrameName[0] = temp_string1;

            ////ret = mySapModel.FrameObj.AddByCoord(0, 0, 10, 8, 0, 16, ref temp_string1, "R1", "2", "Global");

            ////FrameName[1] = temp_string1;

            ////ret = mySapModel.FrameObj.AddByCoord(-4, 0, 10, 0, 0, 10, ref temp_string1, "R1", "3", "Global");

            ////FrameName[2] = temp_string1;

            ////assign point object restraint at base

            //PointName = new string[2];

            //Restraint = new bool[6];
            ////限制U1,U2,U3
            //for (i = 0; i <= 2; i++)
            //{

            //    Restraint[i] = true;

            //}
            ////限制R1,R2,R3
            //for (i = 3; i <= 5; i++)
            //{

            //    Restraint[i] = true;

            //}

            //ret = mySapModel.FrameObj.GetPoints(FrameName[0], ref temp_string1, ref temp_string2);

            //PointName[0] = temp_string1;

            //PointName[1] = temp_string2;

            //bool[] temp_SystemArray1 = Restraint;

            //ret = mySapModel.PointObj.SetRestraint(PointName[0], ref temp_SystemArray1, 0);

            ////assign point object restraint at top

            //for (i = 0; i <= 2; i++)
            //{

            //    Restraint[i] = true;
            //}

            //for (i = 3; i <= 5; i++)
            //{

            //    Restraint[i] = false;

            //}

            //bool[] temp_SystemArray2 = Restraint;

            //ret = mySapModel.PointObj.SetRestraint(PointName[1], ref temp_SystemArray2, 0);

            ////ret = mySapModel.FrameObj.GetPoints(FrameName[1], ref temp_string1, ref temp_string2);

            ////PointName[0] = temp_string1;

            ////PointName[1] = temp_string2;

            ////temp_SystemArray1 = Restraint;

            ////ret = mySapModel.PointObj.SetRestraint(PointName[1], ref temp_SystemArray1, 0);

            ////refresh view, update (initialize) zoom

            //temp_bool = false;

            //ret = mySapModel.View.RefreshView(0, temp_bool);

            ////add load patterns

            //temp_bool = true;

            //ret = mySapModel.LoadPatterns.Add("Vel", SAP2000v15.eLoadPatternType.LTYPE_OTHER, 0, temp_bool);

            //ret = mySapModel.LoadPatterns.Add("Soil", SAP2000v15.eLoadPatternType.LTYPE_OTHER, 0, temp_bool);

            //ret = mySapModel.LoadPatterns.Add("Water", SAP2000v15.eLoadPatternType.LTYPE_OTHER, 0, temp_bool);

            ////ret = mySapModel.LoadPatterns.Add("4", SAP2000v15.eLoadPatternType.LTYPE_OTHER, 0, temp_bool);

            ////ret = mySapModel.LoadPatterns.Add("5", SAP2000v15.eLoadPatternType.LTYPE_OTHER, 0, temp_bool);

            ////ret = mySapModel.LoadPatterns.Add("6", SAP2000v15.eLoadPatternType.LTYPE_OTHER, 0, temp_bool);

            ////ret = mySapModel.LoadPatterns.Add("7", SAP2000v15.eLoadPatternType.LTYPE_OTHER, 0, temp_bool);

            ////assign loading for load pattern 2

            ////ret = mySapModel.FrameObj.GetPoints(FrameName[2], ref temp_string1, ref temp_string2);

            ////PointName[0] = temp_string1;

            ////PointName[1] = temp_string2;

            //PointLoadValue = new double[6];

            //PointLoadValue[2] = -10;

            //// temp_SystemArray1 = PointLoadValue;

            ////ret = mySapModel.PointObj.SetLoadForce(PointName[0], "2", ref PointLoadValue, false, "Global", 0);

            //ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[0], "Vel", 1, 2, 0, 1, wall.Vel_load_bot, wall.Vel_load_top, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);
            //ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[0], "Soil", 1, 2, 0, 1, wall.Soil_load_bot, wall.Soil_load_top, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);
            //ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[0], "Water", 1, 2, 0, 1, wall.Water_load_bot, wall.Water_load_top, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);

            ////assign loading for load pattern 3

            ////ret = mySapModel.FrameObj.GetPoints(FrameName[2], ref temp_string1, ref temp_string2);

            //PointName[0] = temp_string1;

            //PointName[1] = temp_string2;

            ////PointLoadValue = new double[6];

            ////PointLoadValue[2] = -17.2;

            ////PointLoadValue[4] = -54.4;

            //// temp_SystemArray1 = PointLoadValue;

            ////ret = mySapModel.PointObj.SetLoadForce(PointName[1], "3", ref PointLoadValue, false, "Global", 0);

            ////assign loading for load pattern 4

            ////ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[1], "4", 1, 11, 0, 1, 2, 2, "Global", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);

            ////assign loading for load pattern 5

            ////ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[0], "5", 1, 2, 0, 1, 2, 2, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);

            ////ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[1], "5", 1, 2, 0, 1, -2, -2, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);

            ////assign loading for load pattern 6

            ////ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[0], "6", 1, 2, 0, 1, 0.9984, 0.3744, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);

            ////ret = mySapModel.FrameObj.SetLoadDistributed(FrameName[1], "6", 1, 2, 0, 1, -0.3744, 0, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);

            ////assign loading for load pattern 7

            ////ret = mySapModel.FrameObj.SetLoadPoint(FrameName[1], "7", 1, 2, 0.5, -15, "Local", System.Convert.ToBoolean(-1), System.Convert.ToBoolean(-1), 0);

            ////switch to k-in units

            //ret = mySapModel.SetPresentUnits(SAP2000v15.eUnits.kN_m_C);

            ////save model

            ////ret = mySapModel.File.Save(@"E:\2017项目\sapmodel\demo\API_1-002.sdb");
            //ret = mySapModel.File.Save(@path);
            //#region 分析模块
            //////run model (this will create the analysis model)

            ////ret = mySapModel.Analyze.RunAnalysis();

            //////initialize for Sap2000 results

            ////SapResult = new double[7];

            ////ret = mySapModel.FrameObj.GetPoints(FrameName[1], ref temp_string1, ref temp_string2);

            ////PointName[0] = temp_string1;

            ////PointName[1] = temp_string2;

            //////get Sap2000 results for load patterns 1 through 7

            ////NumberResults = 0;

            ////Obj = new string[1];

            ////Elm = new string[1];

            ////LoadCase = new string[1];

            ////StepType = new string[1];

            ////StepNum = new double[1];

            ////U1 = new double[1];

            ////U2 = new double[1];

            ////U3 = new double[1];

            ////R1 = new double[1];

            ////R2 = new double[1];

            ////R3 = new double[1];

            ////for (i = 0; i <= 6; i++)
            ////{

            ////    ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();

            ////    ret = mySapModel.Results.Setup.SetCaseSelectedForOutput(System.Convert.ToString(i + 1), System.Convert.ToBoolean(-1));

            ////    if (i <= 3)
            ////    {

            ////        string[] temp_SystemArray2 = Obj;

            ////        string[] temp_SystemArray3 = Elm;

            ////        string[] temp_SystemArray4 = LoadCase;

            ////        string[] temp_SystemArray5 = StepType;

            ////        double[] temp_SystemArray6 = StepNum;

            ////        double[] temp_SystemArray7 = U1;

            ////        double[] temp_SystemArray8 = U2;

            ////        double[] temp_SystemArray9 = U3;

            ////        double[] temp_SystemArray10 = R1;

            ////        double[] temp_SystemArray11 = R2;

            ////        double[] temp_SystemArray12 = R3;

            ////        ret = mySapModel.Results.JointDispl(PointName[1], SAP2000v15.eItemTypeElm.ObjectElm, ref NumberResults, ref temp_SystemArray2, ref temp_SystemArray3, ref temp_SystemArray4, ref temp_SystemArray5, ref temp_SystemArray6, ref temp_SystemArray7, ref temp_SystemArray8, ref temp_SystemArray9, ref temp_SystemArray10, ref temp_SystemArray11, ref temp_SystemArray12);

            ////        temp_SystemArray9.CopyTo(U3, 0);

            ////        SapResult[i] = U3[0];

            ////    }

            ////    else
            ////    {

            ////        string[] temp_SystemArray2 = Obj;

            ////        string[] temp_SystemArray3 = Elm;

            ////        string[] temp_SystemArray4 = LoadCase;

            ////        string[] temp_SystemArray5 = StepType;

            ////        double[] temp_SystemArray6 = StepNum;

            ////        double[] temp_SystemArray7 = U1;

            ////        double[] temp_SystemArray8 = U2;

            ////        double[] temp_SystemArray9 = U3;

            ////        double[] temp_SystemArray10 = R1;

            ////        double[] temp_SystemArray11 = R2;

            ////        double[] temp_SystemArray12 = R3;

            ////        ret = mySapModel.Results.JointDispl(PointName[0], SAP2000v15.eItemTypeElm.ObjectElm, ref NumberResults, ref temp_SystemArray2, ref temp_SystemArray3, ref temp_SystemArray4, ref temp_SystemArray5, ref temp_SystemArray6, ref temp_SystemArray7, ref temp_SystemArray8, ref temp_SystemArray9, ref temp_SystemArray10, ref temp_SystemArray11, ref temp_SystemArray12);

            ////        temp_SystemArray7.CopyTo(U1, 0);

            ////        SapResult[i] = U1[0];

            ////    }

            ////}

            ////close Sap2000

            //mySapObject.ApplicationExit(false);

            //mySapModel = null;

            //mySapObject = null;

            //////fill Sap2000 result strings

            ////SapResultString = new string[7];

            ////for (i = 0; i <= 6; i++)
            ////{

            ////    SapResultString[i] = string.Format("{0:0.00000}", SapResult[i]);

            ////    ret = (string.Compare(SapResultString[i], 1, "-", 1, 1, true));

            ////    if (ret != 0)
            ////    {

            ////        SapResultString[i] = " " + SapResultString[i];

            ////    }

            ////}

            //////fill independent results

            ////IndResult = new double[7];

            ////IndResultString = new string[7];

            ////IndResult[0] = -0.02639;

            ////IndResult[1] = 0.06296;

            ////IndResult[2] = 0.06296;

            ////IndResult[3] = -0.2963;

            ////IndResult[4] = 0.3125;

            ////IndResult[5] = 0.11556;

            ////IndResult[6] = 0.00651;

            ////for (i = 0; i <= 6; i++)
            ////{

            ////    IndResultString[i] = string.Format("{0:0.00000}", IndResult[i]);

            ////    ret = (string.Compare(IndResultString[i], 1, "-", 1, 1, true));

            ////    if (ret != 0)
            ////    {

            ////        IndResultString[i] = " " + IndResultString[i];

            ////    }

            ////}

            //////fill percent difference

            ////PercentDiff = new double[7];

            ////PercentDiffString = new string[7];

            ////for (i = 0; i <= 6; i++)
            ////{

            ////    PercentDiff[i] = (SapResult[i] / IndResult[i]) - 1;

            ////    PercentDiffString[i] = string.Format("{0:0%}", PercentDiff[i]);

            ////    ret = (string.Compare(PercentDiffString[i], 1, "-", 1, 1, true));

            ////    if (ret != 0)
            ////    {

            ////        PercentDiffString[i] = " " + PercentDiffString[i];

            ////    }

            ////}

            //////display message box comparing results

            ////msg = "";

            ////msg = msg + "LC  Sap2000  Independent  %Diff\r\n";

            ////for (i = 0; i <= 5; i++)
            ////{

            ////    msg = msg + string.Format("{0:0}", i + 1) + "    " + SapResultString[i] + "   " + IndResultString[i] + "       " + PercentDiffString[i] + "\r\n";

            ////}

            ////msg = msg + string.Format("{0:0}", i + 1) + "    " + SapResultString[i] + "   " + IndResultString[i] + "       " + PercentDiffString[i];
            //#endregion

            //#endregion
            #endregion
        }

        private void btnGetCrossPointV1_Click(object sender, EventArgs e)
        {
            GetLineCrossPoints.GetCrossPointVersion1();
        }

        private void btnGetCrossPointV2_Click(object sender, EventArgs e)
        {
            GetLineCrossPoints.GetCrossPointVersion2();
        }

        private void btnGetCrossPointV3_Click(object sender, EventArgs e)
        {
            GetLineCrossPoints.GetCrossPointVersion3();
        }

        private void btnGetCrossPointV4_Click(object sender, EventArgs e)
        {
            GetLineCrossPoints.GetCrossPointVersion4();
        }

        private void btnGetCrossPointV5_Click(object sender, EventArgs e)
        {
            DrawCircleInCrossDot.GetCrossPointDemo();
        }

        private void btnPointtoPLV1_Click(object sender, EventArgs e)
        {
            SelectPointToPlineTools.MPtools();
        }

        private void btnPointtoPLV2_Click(object sender, EventArgs e)
        {
            //SelectPointToPlineTools.MPtools2();
        }

        private void btnPointtoPLV3_Click(object sender, EventArgs e)
        {
            SelectPointToPlineTools.MPtools3();
        }

        private void btnPointtoPLV4_Click(object sender, EventArgs e)
        {
            SelectPointToPlineTools.MPtools4();
        }

        private void btnGetVer_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.GetVertexDemo();
        }

        private void btnGetCen_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.GetCentroidsDemo3();
        }

        private void btnGetByLayer_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.SelectAllbyLayerDemo();
        }

        private void GetArea_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.GetAreaDemo2();
        }

        private void btnGetByEnt_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.SelectSameEntity();
        }

        private void btnGetLayerName_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
        }

        private void btnDelLayer_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();

        }

        private void btnFenSe_Click(object sender, EventArgs e)
        {

            //范围选数
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            TypedValue[] values = new TypedValue[] 
            {
                 new TypedValue ((int)DxfCode.Start,"TEXT"),
            };
            SelectionFilter filter = new SelectionFilter(values);
            ed.WriteMessage("请选择要修改对象:\r\n");
            PromptSelectionResult psr = ed.GetSelection(filter);
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                SelectionSet sset = psr.Value;

                if (psr.Status != PromptStatus.OK) return;

                if (sset != null)
                {
                    foreach (ObjectId oid in sset.GetObjectIds())
                    {
                        DBText text = oid.GetObject(OpenMode.ForWrite) as DBText;

                        if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB1.Text))
                        {
                            text.ColorIndex = 1;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB2.Text))
                        {
                            text.ColorIndex = 2;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB3.Text))
                        {
                            text.ColorIndex = 3;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB4.Text))
                        {
                            text.ColorIndex = 4;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB5.Text))
                        {
                            text.ColorIndex = 5;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB6.Text))
                        {
                            text.ColorIndex = 6;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB7.Text))
                        {
                            text.ColorIndex = 7;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB8.Text))
                        {
                            text.ColorIndex = 8;
                        }
                        else if (Convert.ToDouble(text.TextString) < Convert.ToDouble(txtBoxRGB9.Text))
                        {
                            text.ColorIndex = 30;
                        }
                        else
                        {
                            text.ColorIndex = 40;
                        }

                    }
                }
                trans.Commit();
            }

        }

        private void btnFenSeDetail_Click(object sender, EventArgs e)
        {
            CadDemo cad = new CadDemo();
            cad.FenSeDetail();
        }

        private void btnCadMenu_Click(object sender, EventArgs e)
        {
            Mycui.SelectObjectsOnscreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double height = Convert.ToDouble(textBoxHeight.Text);
            double width = Convert.ToDouble(textBoxWidth.Text);
            double tw = Convert.ToDouble(textBoxTw.Text);
            double tf = Convert.ToDouble(textBoxTf.Text);
            double WidthThicknessRatio = (width - tw) / (2 * tf);
            double HeightThicknessRatio = (height - 2 * tf) / tw;
            double MaxStress = Convert.ToDouble(textBoxSigmaMax.Text);
            double MinStress = Convert.ToDouble(textBoxSigmaMin.Text);
            double alfa = (MaxStress - MinStress) / MaxStress;
            string FlangeSectionType = JudgeFlangeSectionClass(comboBoxForce.SelectedItem.ToString(), comboBoxScetion.SelectedItem.ToString(), comboBoxType.SelectedItem.ToString(), WidthThicknessRatio);
            string WebSectionType = JudgeWebSectionClass(alfa, comboBoxForce.SelectedItem.ToString(), comboBoxScetion.SelectedItem.ToString(), comboBoxType.SelectedItem.ToString(), HeightThicknessRatio);
            textBoxAlfa.Text = alfa+"";
            label19.Text = FlangeSectionType;
            label20.Text = WebSectionType;
        }
        public string JudgeFlangeSectionClass(string ForceType, string SectionType, string SteelType, double Ratio)
        {
            string SectionClass = "";

            int fy = Convert.ToInt32(SteelType.Substring(1));
            double ek = Math.Sqrt(fy / 235);

            if (ForceType == "受弯")
            {
                if (SectionType == "H型")
                {
                    if (Ratio <= 9 * ek)
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= 11 * ek)
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= 13 * ek)
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= 15 * ek)
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
                else if (SectionType == "箱型")
                {
                    if (Ratio <= 25 * ek)
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= 32 * ek)
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= 37 * ek)
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= 42 * ek)
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
            }
            else if (ForceType == "压弯")
            {
                if (SectionType == "H型")
                {
                    if (Ratio <= 9 * ek)
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= 11 * ek)
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= 13 * ek)
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= 15 * ek)
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
                else if (SectionType == "箱型")
                {
                    if (Ratio <= 30 * ek)
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= 35 * ek)
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= 40 * ek)
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= 45 * ek)
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
            }
            return SectionClass;
        }
        public string JudgeWebSectionClass(double alfa, string ForceType, string SectionType, string SteelType, double Ratio)
        {
            string SectionClass = "";
            int fy = Convert.ToInt32(SteelType.Substring(1));
            double ek = Math.Sqrt(fy / 235);
            if (ForceType == "受弯")
            {
                if (SectionType == "H型")
                {
                    if (Ratio <= 65 * ek)
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= 72 * ek)
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= 93 * ek)
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= 124 * ek)
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
                else if (SectionType == "箱型")
                {
                    if (Ratio <= 25 * ek)
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= 32 * ek)
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= 37 * ek)
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= 42 * ek)
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
            }
            else if (ForceType == "压弯")
            {
                if (SectionType == "H型")
                {
                    if (Ratio <= (33 + 13 * Math.Pow(alfa, 1.33) * ek))
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= (38 + 13 * Math.Pow(alfa, 1.39) * ek))
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= (40 + 18 * Math.Pow(alfa, 1.5) * ek))
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= (45 + 25 * Math.Pow(alfa, 1.66) * ek))
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
                else if (SectionType == "箱型")
                {
                    if (Ratio <= 30 * ek)
                    {
                        SectionClass = "S1";
                    }
                    else if (Ratio <= 35 * ek)
                    {
                        SectionClass = "S2";
                    }
                    else if (Ratio <= 40 * ek)
                    {
                        SectionClass = "S3";
                    }
                    else if (Ratio <= 45 * ek)
                    {
                        SectionClass = "S4";
                    }
                    else
                    {
                        SectionClass = "S5";
                    }
                }
            }
            return SectionClass;
        }
    }
}
