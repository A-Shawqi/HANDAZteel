using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HANDAZ.PEB.BusinessComponents;
using HANDAZ.PEB.Entities;



namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    //This page is For Creating a robot object and controller -> there is no reason for this control other than Viewing Robot or 
    //initializing robot
    public partial class RobotCreator : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //================== Robot Initalizers
            RobotObjectUI.Robotstart();
            //=================== Robot Material Definition

            Material mymaterial = new Material("HNDZ-Steel", 77000, 200000000000.00, 0.2, 80000000000.09, 0.000012, 0.06, 200000000.00, 200000000.00, 1.59, 450000000.00);
            RobotObjectUI.SetMaterial(mymaterial);
            //=======================
            // Actual Building Creation 
            //========================
            Node N1 = new Node(0, 0, 0);
            Node N2 = new Node(0, 0, 10);
            Node N3 = new Node(5, 0, 15);
            Node N4 = new Node(10, 0, 10);
            Node N5 = new Node(10, 0, 0);
            
            // If The Frame Has More than One Tapered Section 
            //==============================
            //  Node N2_1 = new Node(2.5, 0, 12.5);
            // Node N3_1 = new Node(7.5,0,12.5);


            //================
            //End Nodes of Purlin
            Node N2D = new Node(0, 10, 0);
            Node N3D = new Node(5, 10, 0);
            Node N4D = new Node(10, 10, 0);
            //===================== 
            // Section Definition 
            BeamTaperedProfile section = new BeamTaperedProfile("HDZ - Col Section", "XXX", 0.20, 0.1 , 0.40 , 0.2 , 0.1 , 0.40 );
            BeamTaperedProfile section2 = new BeamTaperedProfile("HDZ - Col Section - End", "XXX", 0.100 , 0.3 , 0.60 , 0.3 , 0.3 , 0.40 );
            I_BeamSection section3 = new I_BeamSection("PurlinSec", "XXX",300,100,50,50,12, mymaterial);

            //=========================
            //======================
            // Actual Elements Creation 
            // There is A bug The Columns and Beams Have to Be  Drawin in a Specific way 
            //====================================================
            // Columns From Bottom Node to Top 
            // Left Beams From the Left to the right not and right beam from the Right to The left node
            //=========================================

            Column Col1 = new Column(N1, N2, section, section2);
            Column Col2 = new Column(N5, N4, section, section2);
            Beam beam2 = new Beam(N2, N3, section, section2);
            Beam beam4 = new Beam(N4, N3, section, section2);
            Support leftsupport = new Support(N1, SupportTypeEnum.Pinned);
            Support rightSupport = new Support(N5, SupportTypeEnum.Pinned);

            Purlin Purlin1 = new Purlin(N2, N2D, section3);
            Purlin Purlin2 = new Purlin(N3, N3D, section3);
            Purlin Purlin3 = new Purlin(N4, N4D, section3);

            //////----  Beam beam2 = new Beam(N2_1, N3, section, section2);
            // Beam beam1 = new Beam(N2, N2_1, section, section2);
            //  Beam beam3 = new Beam(N3, N3_1, section, section2);
            //-----   Beam beam4 = new Beam(N3_1, N4, section, section2);
            //============== 
            //====================
            // Creation Of a Frame Element

            List<Beam> beams = new List<Beam>();
            beams.Add(beam2);
            beams.Add(beam4);
            //===============
            List<Column> columns = new List<Column>();
            columns.Add(Col1);
            columns.Add(Col2);
            //====================
            List<Support> supports = new List<Support>();
            supports.Add(leftsupport);
            supports.Add(rightSupport);
            //=============================
            Frame Myframe = new Frame(10, 15, beams, columns, supports, N1,10);
            List<Frame> Frames = new List<Frame>();
            Frames.Add(Myframe);

            List<Purlin> Purlins = new List<Purlin>();
            Purlins.Add(Purlin1);
            Purlins.Add(Purlin2);
            Purlins.Add(Purlin3);

            Structure mystructure = new Structure("hdz", new Node(0, 0, 0), 10, 50, 6, 10,Frames,Purlins);
            //===================================
            RobotObjectUI.DrawFrame(Myframe);
            RobotObjectUI.SetColumnSections(Myframe);
            RobotObjectUI.SetBeamSections(Myframe);
            RobotObjectUI.SetSupports(Myframe);
            RobotObjectUI.SetloadCases(mystructure);
            RobotObjectUI.SetLoadCombinations();
            RobotObjectUI.Analyze();
            RobotObjectUI.Design();
            RobotObjectUI.GetResults();
            //========================
            #region MyRegion
            //API_Object_UI.SetColumnSections(Myframe);
            ////=====================
            ////My logic 
            //Node start = new Node(1, 0, 0, 0);
            //Node end = new Node(2, 10, 5, 10);
            //I_BeamSection section = new I_BeamSection("m", "m", 100, 100, 100, 100, 100, null);
            //Beam beam = new Beam(1, start, end, section);
            //List<Node> MyFrame = new List<Node>();
            //Node E1N = new Node(1,0,0,0);
            //Node E2N = new Node(2, 0,0, 10);
            //Node E3N = new Node(3, 5, 0, 15);
            //Node E4N = new Node(4, 10, 0, 10);
            //Node E5N = new Node(5, 10, 0, 0);
            //MyFrame.Add(E1N);
            //MyFrame.Add(E2N);
            //MyFrame.Add(E3N);
            //MyFrame.Add(E4N);
            //MyFrame.Add(E5N);
            //API_Object_UI.DrawFrame(MyFrame);
            //API_Object_UI.SetCases();
            //API_Object_UI.SetSupports(MyFrame);
            //API_Object_UI.SetColumnSections();
            //// Old logic end 
            ////=================
            ////API_Object_UI.DrawGrid(null);
            ////  API_Object_UI.DrawBeam(beam);
            ////API_Object_UI.DrawBeam();
            #endregion

        }
    }
}