using HANDAZ.Entities;
using HANDAZ.PEB.Core.Designers;
using HANDAZ.PEB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.PEB.AnalysisTools
{
    public static class RobotDesigner
    {
        public static bool Design(HndzFrameSingleBay3D CustomerInputsFrame)
        {

            RobotInit.RobotKickStart();
            RobotInit.StartRobot();
            RobotInit.New2dFrameProject();
            Material mymaterial = new Material("HNDZ-Steel", 77000, 200000000000.00, 0.2, 80000000000.09, 0.000012, 0.06, 200000000.00, 200000000.00, 1.59, 450000000.00);
            RobotDefinitions.SetMaterial(mymaterial);
            //=======================
            // Actual Building Creation 
            //========================
            HndzFrameSingleBay2D Frame = CustomerInputsFrame.Frames2D.ElementAt(0);
            double ColumnHeight = Frame.LeftColumn.ExtrusionLine.RhinoLine.Length / 1000;
            double RidgeHeight = Frame.LeftBeam.ExtrusionLine.EndNode.Point.Z / 1000;
            double EaveHeight = Frame.LeftColumn.ExtrusionLine.EndNode.Point.Z / 1000;
            double Width = Frame.RightColumn.ExtrusionLine.baseNode.Point.X / 1000;

            //Node N1 = new Node(0, 0, 0);
            //Node N2 = new Node(0, 0, 10);
            //Node N3 = new Node(5, 0, 15);
            //Node N4 = new Node(10, 0, 10);
            //Node N5 = new Node(10, 0, 0);

            Node N1 = new Node(0, 0, 0);
            Node N2 = new Node(0, 0, CustomerInputsFrame.EaveHeight/1000);
            Node N3 = new Node(CustomerInputsFrame.Width/2000, 0, CustomerInputsFrame.RidgeHeight/1000);
            Node N4 = new Node(CustomerInputsFrame.Width/1000, 0, CustomerInputsFrame.EaveHeight/1000);
            Node N5 = new Node(CustomerInputsFrame.Width/1000, 0, 0);
            ////=================
            BeamTaperedProfile section = new BeamTaperedProfile("HDZ - Col Section", "XXX", 0.20, 0.1, 0.2, 0.1, 0.2, 0.6);
            BeamTaperedProfile section2 = new BeamTaperedProfile("HDZ - Col Section - End", "XXX", 0.2, 0.1, 0.2, 0.1, 0.2, 0.40);
            I_BeamSection section3 = new I_BeamSection("PurlinSec", "XXX", 300, 100, 50, 50, 12, mymaterial);
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
            Beam beam2 = new Beam(N2, N3, section2, section);
            Beam beam4 = new Beam(N4, N3, section2, section);
            Support leftsupport = new Support(N1, SupportTypeEnum.Pinned);
            Support rightSupport = new Support(N5, SupportTypeEnum.Pinned);
            //========================
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
            Frame Myframe = new Frame(EaveHeight, RidgeHeight, beams, columns, supports, N1, Width);
            List<Frame> Frames = new List<Frame>();
            Frames.Add(Myframe);
            //===================================
            RobotGeometry.DrawFrame(Myframe);
            RobotDefinitions.SetColumnSections(Myframe);
            RobotDefinitions.SetBeamSections(Myframe);
            RobotGeometry.SetSupports(Myframe);
            RobotLoadAssigns.SetloadCases(Myframe);
            RobotLoadAssigns.SetLoadCombinations();
            RobotResults.Analyze();
            RobotResults.Design(Myframe);
            return true;
        }
    }
}
