﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Data;
//using Rhino.Geometry;
//using HANDAZ.PEB.Entities;
//using HANDAZ.Entities;

//namespace HANDAZ.PEB.WebUI.UserControls.Designer
//{
//    //Control Is for Viewing the Results in a table format 
//    //This Table Should Have Beam Id , Beam startNode , Beam Endnode , Beam Length , Beam Loads , Internal Forces (axial force) at start , End ,
//    //Mx at Start Node , My At endNode ,  Beam Section , Safety Ratio 
//    public partial class Ctrl_resultsTable : System.Web.UI.UserControl
//    {
//        //Loads Array SHould be Change to Be Arrays of items of Objects of Class BeamLoad to Be made 
//        // BeamLoad [] beamloads

//        //List<Array> DataGridFill = new List<Array>();
//        //int[] BeamIds;
//        //Point3d[] BeamStartNodes;
//        //Point3d[] BeamEndNodes;
//        //double[] BeamLength;
//        //double[] AxialForceStart;
//        //double[] AxialForceEnd;
//        //double[] MxStart;
//        //double[] MxEnd;
//        //double[] MyStart;
//        //double[] MyEnd;



//        List<Beam> Beams = new List<Beam>();
//        Material material;
//        I_BeamSection MySection;
//        Node Beamstart;
//        Node BeamEnd;
//        int beamidOne;
//        Beam Mybeam;
//        Beam Mybeam2;
//        Beam Mybeam3;
//        Beam Mybeam4;
//        Beam Mybeam5;
//        //======================


//        HndzAnalysisResults result;
//        HndzAnalysisResults result2;
//        HndzAnalysisResults result3;
//        List<HndzAnalysisResults> resultslist = new List<HndzAnalysisResults>();
//        //Loads beamLoads;

//        // List<>
//        //DataTable dt;
//        //DataSet ds;
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            ObjectCreator();
//            //Datafill();
//            GridViewFill();
//            grv_resultsTable.DataSource = resultslist;
//            grv_resultsTable.DataBind();

//            // GridViewColumnsGenerator Gen = new GridViewColumnsGenerator();
//            // Gen.CreateAutoGeneratedFields(BeamIds, grv_resultsTable);
//            //// Gen.GenerateFields(grv_resultsTable);
//            //   grv_resultsTable.Columns.Add()
//            //   grv_resultsTable.Columns.Add(BeamIds)

//        }
//        //public void Datafill()
//        //{
//        //    BeamIds = new int[10];
//        //    BeamStartNodes = new Point3d[10];
//        //    BeamEndNodes = new Point3d[10];
//        //    BeamLength = new double[10];
//        //    //AxialForceStart = new double[10];
//        //    //AxialForceEnd = new double[10];
//        //    //MxStart = new double[10];
//        //    //MxEnd = new double[10];
//        //    //MyStart = new double[10];
//        //    //MyEnd = new double[10];
//        //    //Created Objects SHould not be created here once or ever --- Refrence Suitable Ui Page
//        //    for (int i = 0; i < BeamIds.Length; i++)
//        //    {
//        //        BeamIds[i] = Mybeam.BeamId;
//        //        BeamStartNodes[i] = Mybeam.BeamStart;
//        //        BeamEndNodes[i] = Mybeam.BeamEnd;
//        //        BeamLength[i] = Mybeam.Beamline.Length;
//        //        //AxialForceStart[i] = 10;
//        //        //AxialForceEnd[i] = 101;
//        //        //MxStart[i] = 102;
//        //        //MxEnd[i] = 103;
//        //        //MyStart[i] = 104;
//        //        //MyEnd[i] = 105;

//        //        //========================= add to beam object 
//        //        // should be removed thats what im getting from robot 

//        //        Beams.Add(Mybeam);
//        //    }
//        //    //DataGridFill.Add(BeamIds);
//        //    //DataGridFill.Add(BeamStartNodes);
//        //    //DataGridFill.Add(BeamEndNodes);
//        //    //DataGridFill.Add(BeamLength);
//        //    //DataGridFill.Add(BeamLoads);
//        //    //DataGridFill.Add(AxialForceStart);
//        //    //DataGridFill.Add(AxialForceEnd);
//        //    //DataGridFill.Add(MxStart);
//        //    //DataGridFill.Add(MxEnd);
//        //    //DataGridFill.Add(MyStart);
//        //    //DataGridFill.Add(MyEnd);

//        // //   return DataGridFill;

//        //}
//        public void ObjectCreator()
//        {
//            //    material = new Material("steel", 76.97, 199948.02, 0.3, 76904.15, 0.000012, 0.06, 248.21, 248.21, 1.66, 399.90);
//            //    MySection = new I_BeamSection("Test", "Sec!", 100, 100, 20, 20, 12, material);
//            //    Beamstart = new Node(0, 0, 0);
//            //    BeamEnd = new Node(10, 0, 0);
//            //    beamidOne = 1;
//            ////    beamLoads = new BeamLoads(10, 10, 10, 10, 10, 10, 10, 10, 10, 10);
//            //    Mybeam = new Beam( Beamstart, BeamEnd, MySection );
//            //    Mybeam2 = new Beam( Beamstart, BeamEnd, MySection );
//            //    Mybeam3 = new Beam(Beamstart, BeamEnd, MySection );
//            //    Mybeam4 = new Beam( Beamstart, BeamEnd, MySection );
//            //    Mybeam5 = new Beam( Beamstart, BeamEnd, MySection);

//            //    Beams.Add(Mybeam);
//            //    Beams.Add(Mybeam2);
//            //    Beams.Add(Mybeam3);
//            //    Beams.Add(Mybeam4);
//            //    Beams.Add(Mybeam5);

//            double station =    { 1 };
//            string loadCase =   { "2" };
//            double axial =      { 3 };
//            double shear2 =     { 4 };
//            double shear3 =     { 5 };
//            double tortionalMoment = { 6 };
//            double moment2 = { 7 };
//            double moment3 = { 8 };

//            double station2 = { 1 };
//            string loadCase2 = { "2" };
//            double axial2 = { 3 };
//            double shear22 = { 4 };
//            double shear32 = { 5 };
//            double tortionalMoment2 = { 6 };
//            double moment22 = { 7 };
//            double moment32 = { 8 };

//            double station3 = { 1 };
//            string loadCase3 = { "2" };
//            double axial3 = { 3 };
//            double shear23 = { 4 };
//            double shear33 = { 5 };
//            double tortionalMoment3 = { 6 };
//            double moment23 = { 7 };
//            double moment33 = { 8 };

//            result = new HndzAnalysisResults(station, loadCase, axial, shear2, shear3, tortionalMoment, moment2, moment3);
//            result2 = new HndzAnalysisResults(station2, loadCase2, axial2, shear22, shear32, tortionalMoment2, moment22, moment32);
//            result3 = new HndzAnalysisResults(station3, loadCase3, axial3, shear23, shear33, tortionalMoment3, moment23, moment33);
//            resultslist.Add(result);
//            resultslist.Add(result2);
//            resultslist.Add(result3);

//        }
//        public DataSet GridViewFill()
//        {

//            //BeamIds = new int[10];
//            //BeamStartNodes = new Point3d[10];
//            //BeamEndNodes = new Point3d[10];
//            //BeamLength = new double[10];
//            //AxialForceStart = new double[10];
//            //AxialForceEnd = new double[10];
//            //MxStart = new double[10];
//            //MxEnd = new double[10];
//            //MyStart = new double[10];
//            //MyEnd = new double[10];
//            //dt = new DataTable();
//            //ds = new DataSet();
//            //DataColumn BeamIdCol = new DataColumn("BeamId", typeof(int));
//            //DataColumn BeamStartNodeCol = new DataColumn("BeamStartNode", typeof(Point3d));
//            //DataColumn BeamEndNodesCol = new DataColumn("BeamEndNodes", typeof(Point3d));
//            //DataColumn BeamLengthCol = new DataColumn("BeamLength", typeof(double));
//            //DataColumn AxialForceStartCol = new DataColumn("AxialForceStart", typeof(double));
//            //DataColumn AxialForceEndCol = new DataColumn("AxialForceEnd", typeof(double));
//            //DataColumn MxStartCol = new DataColumn("MxStart", typeof(double));
//            //DataColumn MxEndCol = new DataColumn("MxEnd", typeof(double));
//            //DataColumn MyStartCol = new DataColumn("MyStart", typeof(double));
//            //DataColumn MyEndCol = new DataColumn("MyEndCol", typeof(double));

//            //DataColumn[] Datacolumns = new DataColumn[4];
//            //Datacolumns[0] = BeamIdCol;
//            //Datacolumns[1] = BeamStartNodeCol;
//            //Datacolumns[2] = BeamEndNodesCol;
//            //Datacolumns[3] = BeamLengthCol;
//            //dt.Columns.AddRange(Datacolumns);
//            //dt.Columns.Add(BeamIds);
//            //dt.Columns.Add(BeamStartNodeCol);
//            //dt.Columns.Add(BeamEndNodesCol);
//            //dt.Columns.Add(BeamLengthCol);
//            //dt.Columns.Add(AxialForceStartCol);
//            //dt.Columns.Add(AxialForceEndCol);
//            //dt.Columns.Add(MxStartCol);
//            //dt.Columns.Add(MxEndCol);
//            //dt.Columns.Add(MyStartCol);
//            //dt.Columns.Add(MyEndCol);
//            //ds.Tables.Add(dt);

//            // DataRow dr = new DataRow();

//            //  dt.Rows.Add()
//            //for (int i = 0; i < Beams.Count; i++)
//            //{
//            //  DataRow row =  dt.NewRow();
//            //    row["BeamIdCol"] = Beams[i].BeamId;
//            //    row["BeamStartNode"] = Beams[i].BeamStart;
//            //    row["BeamEndNodes"] = Beams[i].BeamEnd;
//            //    row["BeamLength"] = Beams[i].Beamline.Length;
//            //    //row["AxialForceStart"] = Beams[i].Beamload.AxialForceStart;
//            //    //row["AxialForceEnd"] = Beams[i].Beamload.AxialForceEnd;
//            //    //row["MxStart"] = Beams[i].Beamload.MxStart;
//            //    //row["MxEnd"] = Beams[i].Beamload.MxEnd;
//            //    //row["MyStart"] = Beams[i].Beamload.MyStart;
//            //    //row["MyEndCol"] = Beams[i].Beamload.MyEnd;
//            //    dt.Rows.Add(row);
//            //}

//            return null;
//        }

//        // public D
//    }
//}