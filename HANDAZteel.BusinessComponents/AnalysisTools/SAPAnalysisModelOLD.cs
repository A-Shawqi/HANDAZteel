//#region OLD GENERIC WONDERFUL AWESOME CODE
//using HANDAZ.Entities;
//using HANDAZ.PEB.AnalysisTools.CsiSAP2000;
//using HANDAZ.PEB.Core;
//using SAP2000v18;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static HANDAZ.PEB.Core.ASCE107Wind;

//namespace HANDAZ.PEB.BusinessComponents
//{
//    [Obsolete("We temporarily use the new version to adjust with ifc, but this is better")]
//    public class SAPAnalysisModelOLD
//    {
//        private SAPLoadPattern Dead;
//        private SAPLoadPattern Cover;
//        private SAPLoadPattern Live;
//        private SAPLoadPattern WLB;
//        private SAPLoadPattern WLU;
//        private SAPLoadPattern WRB;
//        private SAPLoadPattern WRU;
//        private SAPLoadPattern WEB;
//        private SAPLoadPattern WEU;

//        // private  SAPLoadPattern WindLeft;//TODO: Temporary Load Pattern
//        //private  SAPLoadPattern WindRight;//TODO: Temporary Load Pattern
//        public List<SAPFrameElement> Beams { get; set; }
//        public List<SAPFrameElement> Columns { get; set; }
//        #region Public Methods, Frame Generation Methods

//        public void GenerateClearSpanFrame(HndzFrameSingleBay3D inputs, string modelName, bool attachToInstance = true)
//        {
//            #region Extracted Inputs

//            double width = inputs.Width;
//            double length = inputs.Length;
//            int noFrames = 1; //inputs.NoFrames;//TODO
//            double BaySpacing = inputs.BaySpacing;
//            double eaveHeight = inputs.EaveHeight;
//            HndzRoofSlopeEnum slopeEnum = inputs.RoofSlope;
//            double roofSlope;
//            switch (slopeEnum)
//            {
//                case HndzRoofSlopeEnum.From1To5:
//                    roofSlope = 0.2;
//                    break;
//                case HndzRoofSlopeEnum.From1To10:
//                    roofSlope = 0.1;
//                    break;
//                case HndzRoofSlopeEnum.From1To20:
//                    roofSlope = 0.05;
//                    break;
//                default:
//                    roofSlope = 0.1;
//                    break;
//            }
//            #endregion//Done
//            //TODO: Handle units in here
//            #region Starting The program
//            InitSAP2000(attachToInstance);
//            #endregion

//            #region Defining Materials
//            SAPMaterial[] materials = DefineMaterials();
//            #endregion

//            #region Defining I-Sections and Tapered Sections, assumed
//            SAPITaperedSection column, taperedBeam;
//            SAPISection middleBeam;
//            //Columns
//            SAPISection baseColumn = new SAPISection("colBaseSec", materials[2], materials[2], materials[2], 0.3, 0.2, 0.01, 0.2, 0.01, 0.005); //TODO: HARDCODED 
//            SAPISection topColumn = new SAPISection("colTopSec", materials[2], materials[2], materials[2], 0.8, 0.2, 0.01, 0.2, 0.01, 0.005); //TODO: HARDCODED 
//            column = new SAPITaperedSection("column", baseColumn, topColumn, 1, LengthTypeEnum.Relative);
//            SAP2000API.SetISection(baseColumn);
//            SAP2000API.SetISection(topColumn);
//            SAP2000API.SetITaperedSection(column);


//            //Beams
//            SAPISection edgeBeam = new SAPISection("beamEdge", materials[2], materials[2], materials[2], 0.8, 0.2, 0.01, 0.2, 0.01, 0.005);//TODO: HARDCODED 
//            middleBeam = new SAPISection("beamMiddle", materials[2], materials[2], materials[2], 0.5, 0.2, 0.01, 0.2, 0.01, 0.005);//TODO: HARDCODED 
//            taperedBeam = new SAPITaperedSection("taperedBeam", edgeBeam, middleBeam, 1, LengthTypeEnum.Relative);
//            SAP2000API.SetISection(edgeBeam);
//            SAP2000API.SetISection(middleBeam);
//            SAP2000API.SetITaperedSection(taperedBeam);
//            #endregion

//            #region Drawing 3D Frame and pre-design assumptions
//            SAPFrameElement[] leftCols, rightCols;
//            SAPFrameElement[,] leftBeams, rightBeams;
//            //Columns
//            leftCols = new SAPFrameElement[noFrames];
//            rightCols = new SAPFrameElement[noFrames];
//            for (int i = 0; i < noFrames; i++)
//            {
//                leftCols[i] = new SAPFrameElement("leftCol" + i);
//                leftCols[i].StartPoint = new SAPPoint(leftCols[i].Name + "p1", 0, i * inputs.BaySpacing, 0);
//                leftCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                leftCols[i].EndPoint = new SAPPoint(leftCols[i].Name + "p2", 0, i * inputs.BaySpacing, eaveHeight);
//                leftCols[i].Section = column;

//                rightCols[i] = new SAPFrameElement("rightCol" + i);
//                rightCols[i].StartPoint = new SAPPoint(rightCols[i].Name + "p1", width, i * inputs.BaySpacing, 0);
//                rightCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                rightCols[i].EndPoint = new SAPPoint(rightCols[i].Name + "p2", width, i * inputs.BaySpacing, eaveHeight);
//                rightCols[i].Section = column;
//            }
//            SAP2000API.AddFrameElement(leftCols);
//            SAP2000API.AddFrameElement(rightCols);

//            //Beams
//            int noSegments = (int)((width / 2) / 6);
//            leftBeams = new SAPFrameElement[noFrames, noSegments];
//            rightBeams = new SAPFrameElement[noFrames, noSegments];
//            for (int i = 0; i < noFrames; i++)
//            {
//                for (int j = 0; j < noSegments; j++)
//                {
//                    leftBeams[i, j] = new SAPFrameElement("leftBeam" + i + "-" + j);
//                    leftBeams[i, j].StartPoint = new SAPPoint(leftBeams[i, j].Name + "p1", j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Z);
//                    leftBeams[i, j].EndPoint = new SAPPoint(leftBeams[i, j].Name + "p2", (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Z);
//                    leftBeams[i, j].Section = middleBeam;

//                    rightBeams[i, j] = new SAPFrameElement("rightBeam" + i + "-" + j);
//                    rightBeams[i, j].StartPoint = new SAPPoint(rightBeams[i, j].Name + "p2", width - j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * width / 2 + rightCols[i].EndPoint.Z);
//                    rightBeams[i, j].EndPoint = new SAPPoint(rightBeams[i, j].Name + "p1", width - (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * width / 2 + rightCols[i].EndPoint.Z);
//                    rightBeams[i, j].Section = middleBeam;
//                }
//                //Modifications for exceptions
//                leftBeams[i, 0].Section = taperedBeam; //Tapered edge section
//                rightBeams[i, 0].Section = taperedBeam; //Tapered edge section

//                leftBeams[i, noSegments - 1].StartPoint = new SAPPoint(leftBeams[i, noSegments - 1].Name + "p1", (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Z);
//                leftBeams[i, noSegments - 1].EndPoint = new SAPPoint(leftBeams[i, noSegments - 1].Name + "p2", width / 2, i * inputs.BaySpacing, (double)width / 2 * roofSlope + leftCols[i].EndPoint.Z);

//                rightBeams[i, noSegments - 1].EndPoint = new SAPPoint(rightBeams[i, noSegments - 1].Name + "p1", width - (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * width / 2 * roofSlope + rightCols[i].EndPoint.Z);
//                rightBeams[i, noSegments - 1].StartPoint = leftBeams[i, noSegments - 1].EndPoint;
//            }
//            foreach (SAPFrameElement beam in leftBeams)
//            {
//                SAP2000API.AddFrameElement(beam);
//            }
//            foreach (SAPFrameElement beam in rightBeams)
//            {
//                SAP2000API.AddFrameElement(beam);
//            }
//            #endregion
//            #region Loads Definition
//            SAPLoadCombination[] combos;
//            combos = DefineLoadDefinitions();

//            Dictionary<float, SAPLoadCombination> combosDic = new Dictionary<float, SAPLoadCombination>();
//            foreach (SAPLoadCombination combo in combos)
//            {
//                combosDic.Add(1, combo);
//            }
//            SAPLoadCombination envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, combosDic);
//            SAP2000API.AddLoadCombination(envelope);
//            #endregion
//            #region Assigning Loads
//            AssignLoads(inputs, leftCols, rightCols, leftBeams, rightBeams);
//            #endregion
//            #region Analysis
//            AnalyzeModel(modelName, rightCols, combos, envelope);

//            #endregion
//            #region Design
//            DesignModel(leftCols, rightCols);
//            #endregion

//            foreach (SAPFrameElement beam in Beams)
//            {

//            }

//        }
//        public void GenerateMultiGableFrame(CustomerInputs inputs, string modelName, bool attachToInstance = true)
//        {

//            #region Extracted Inputs

//            float width = inputs.LandWidth;
//            float length = inputs.LandLength;
//            int noFrames = 1; //inputs.NoFrames;//TODO
//            float BaySpacing = inputs.BaySpacing;
//            float eaveHeight = inputs.EaveHeight;
//            HndzRoofSlopeEnum slopeEnum = inputs.RoofSlope;
//            double roofSlope;
//            switch (slopeEnum)
//            {
//                case HndzRoofSlopeEnum.From1To5:
//                    roofSlope = 0.2;
//                    break;
//                case HndzRoofSlopeEnum.From1To10:
//                    roofSlope = 0.1;
//                    break;
//                case HndzRoofSlopeEnum.From1To20:
//                    roofSlope = 0.05;
//                    break;
//                default:
//                    roofSlope = 0.1;
//                    break;
//            }
//            #endregion

//            #region Starting The program
//            InitSAP2000(attachToInstance);
//            #endregion

//            #region Defining Materials
//            SAPMaterial[] materials = DefineMaterials();
//            #endregion
//            #region Defining I-Sections and Tapered Sections, assumed
//            SAPITaperedSection column, taperedBeam;
//            SAPISection middleBeam;
//            //Columns
//            SAPISection baseColumn = new SAPISection("colBaseSec", materials[2], materials[2], materials[2], 0.2, 0.2, 0.002, 0.2, 0.002, 0.0012); //TODO: HARDCODED 
//            SAPISection topColumn = new SAPISection("colTopSec", materials[2], materials[2], materials[2], 0.6, 0.2, 0.002, 0.2, 0.002, 0.0012); //TODO: HARDCODED 
//            column = new SAPITaperedSection("column", baseColumn, topColumn, 1, LengthTypeEnum.Relative);
//            SAP2000API.SetISection(baseColumn);
//            SAP2000API.SetISection(topColumn);
//            SAP2000API.SetITaperedSection(column);


//            //Beams
//            SAPISection edgeBeam = new SAPISection("beamEdge", materials[2], materials[2], materials[2], 0.6, 0.2, 0.002, 0.2, 0.002, 0.0012);//TODO: HARDCODED 
//            middleBeam = new SAPISection("beamMiddle", materials[2], materials[2], materials[2], 0.4, 0.2, 0.002, 0.2, 0.002, 0.0012);
//            taperedBeam = new SAPITaperedSection("taperedBeam", edgeBeam, middleBeam, 1, LengthTypeEnum.Relative);
//            SAP2000API.SetISection(edgeBeam);
//            SAP2000API.SetISection(middleBeam);
//            SAP2000API.SetITaperedSection(taperedBeam);
//            #endregion

//            #region Drawing 3D Frame and pre-design assumptions
//            SAPFrameElement[] leftCols, rightCols;
//            SAPFrameElement[,] leftBeams, rightBeams;
//            //Columns
//            leftCols = new SAPFrameElement[noFrames];
//            rightCols = new SAPFrameElement[noFrames];
//            for (int i = 0; i < noFrames; i++)
//            {
//                leftCols[i] = new SAPFrameElement("leftCol" + i);
//                leftCols[i].StartPoint = new SAPPoint(leftCols[i].Name + "p1", 0, i * inputs.BaySpacing, 0);
//                leftCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                leftCols[i].EndPoint = new SAPPoint(leftCols[i].Name + "p2", 0, i * inputs.BaySpacing, eaveHeight);
//                leftCols[i].Section = column;

//                rightCols[i] = new SAPFrameElement("rightCol" + i);
//                rightCols[i].StartPoint = new SAPPoint(rightCols[i].Name + "p1", width, i * inputs.BaySpacing, 0);
//                rightCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                rightCols[i].EndPoint = new SAPPoint(rightCols[i].Name + "p2", width, i * inputs.BaySpacing, eaveHeight);
//                rightCols[i].Section = column;
//            }
//            SAP2000API.AddFrameElement(leftCols);
//            SAP2000API.AddFrameElement(rightCols);

//            //Beams
//            int noSegments = (int)((width / 2) / 6);
//            leftBeams = new SAPFrameElement[noFrames, noSegments];
//            rightBeams = new SAPFrameElement[noFrames, noSegments];
//            for (int i = 0; i < noFrames; i++)
//            {
//                for (int j = 0; j < noSegments; j++)
//                {
//                    leftBeams[i, j] = new SAPFrameElement("leftBeam" + i + "-" + j);
//                    leftBeams[i, j].StartPoint = new SAPPoint(leftBeams[i, j].Name + "p1", j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Z);
//                    leftBeams[i, j].EndPoint = new SAPPoint(leftBeams[i, j].Name + "p2", (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Z);
//                    leftBeams[i, j].Section = middleBeam;

//                    rightBeams[i, j] = new SAPFrameElement("rightBeam" + i + "-" + j);
//                    rightBeams[i, j].StartPoint = new SAPPoint(rightBeams[i, j].Name + "p2", width - j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * width / 2 + rightCols[i].EndPoint.Z);
//                    rightBeams[i, j].EndPoint = new SAPPoint(rightBeams[i, j].Name + "p1", width - (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * width / 2 + rightCols[i].EndPoint.Z);
//                    rightBeams[i, j].Section = middleBeam;
//                }
//                //Modifications for exceptions
//                leftBeams[i, 0].Section = taperedBeam; //Tapered edge section
//                rightBeams[i, 0].Section = taperedBeam; //Tapered edge section

//                leftBeams[i, noSegments - 1].StartPoint = new SAPPoint(leftBeams[i, noSegments - 1].Name + "p1", (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Z);
//                leftBeams[i, noSegments - 1].EndPoint = new SAPPoint(leftBeams[i, noSegments - 1].Name + "p2", width / 2, i * inputs.BaySpacing, (double)width / 2 * roofSlope + leftCols[i].EndPoint.Z);

//                rightBeams[i, noSegments - 1].EndPoint = new SAPPoint(rightBeams[i, noSegments - 1].Name + "p1", width - (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * width / 2 * roofSlope + rightCols[i].EndPoint.Z);
//                rightBeams[i, noSegments - 1].StartPoint = leftBeams[i, noSegments - 1].EndPoint;
//            }
//            foreach (SAPFrameElement beam in leftBeams)
//            {
//                SAP2000API.AddFrameElement(beam);
//            }
//            foreach (SAPFrameElement beam in rightBeams)
//            {
//                SAP2000API.AddFrameElement(beam);
//            }
//            #endregion
//        }
//        #endregion
//        #region Private Methods
//        private void DesignModel(SAPFrameElement[] leftCols, SAPFrameElement[] rightCols)
//        {
//            SAPDesignStatistics statistics = new SAPDesignStatistics();
//            SAP2000API.DesignSteelModel(SAPSteelDesignCode.AISC36010, ref statistics, Beams, Columns);
//        }

//        private void AnalyzeModel(string modelName, SAPFrameElement[] rightCols, SAPLoadCombination[] combos, SAPLoadCombination envelope)
//        {
//            SAP2000API.SaveModel(modelName);
//            SAP2000API.AnalayzeModel();
//            SAP2000API.GetFrameElementAnalysisResults(combos, Beams);
//            SAP2000API.GetFrameElementAnalysisResults(new SAPLoadCombination[] { envelope }, Beams);
//            SAP2000API.GetFrameElementAnalysisResults(combos, Columns);
//            SAP2000API.GetFrameElementAnalysisResults(new SAPLoadCombination[] { envelope }, Columns);


//            // // ===========================
//        }

//        private void AssignLoads(HndzFrame3D inputs, SAPFrameElement[] leftCols, SAPFrameElement[] rightCols, SAPFrameElement[,] leftBeams, SAPFrameElement[,] rightBeams)
//        {
//            SAPDistributedLoad CoverLoad = new SAPDistributedLoad("Cover", Cover, 0, 1, 0.01 * inputs.BaySpacing, 0.01 * inputs.BaySpacing, HndzLoadDirectionEnum.GravityProjected);
//            SAPDistributedLoad LiveLoad = new SAPDistributedLoad("Live", Live, 0, 1, 0.057 * inputs.BaySpacing, 0.057 * inputs.BaySpacing, HndzLoadDirectionEnum.GravityProjected);
//            #region WindLoad
//            double WL_1, WL_2, WL_3, WL_4, WL_1N, WL_2N, WL_3N, WL_4N, WL_1_B, WL_2_B, WL_1_BN, WL_2_BN;
//            ASCE107Wind.LoadParameters((int)inputs.Location, (RiskCategory)inputs.RiskCategory, (ExposureCategory)inputs.ExposureCategory, (float)inputs.RidgeHeight, (float)inputs.EaveHeight, (float)inputs.Length, (float)inputs.Width, RoofType.Gable, 1, 0.85f, true, false); //TODO: Make it not hard coded
//                                                                                                                                                                                                                                                                                   //Check_1 = txt_Check_1.Text;
//                                                                                                                                                                                                                                                                                   //Check_2 = txt_Check_2.Text;
//            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 1", out WL_1);
//            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 2", out WL_2);
//            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 3", out WL_3);
//            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 4", out WL_4);

//            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 1", out WL_1N);
//            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 2", out WL_2N);
//            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 3", out WL_3N);
//            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 4", out WL_4N);

//            ASCE107Wind.netPositivePressureB.TryGetValue("Roof Zone 2", out WL_1_B);
//            ASCE107Wind.netPositivePressureB.TryGetValue("Wall Zone 5", out WL_2_B);

//            ASCE107Wind.netNegativePressureB.TryGetValue("Roof Zone 2", out WL_1_BN);
//            ASCE107Wind.netNegativePressureB.TryGetValue("Wall Zone 5", out WL_2_BN);

//            //TODO: Temp conversion from kn to Ton .... till u implement units conversion
//            WL_1 = WL_1 / 10;
//            WL_2 = WL_2 / 10;
//            WL_3 = WL_3 / 10;
//            WL_4 = WL_4 / 10;

//            WL_1N = WL_1N / 10;
//            WL_2N = WL_2N / 10;
//            WL_3N = WL_3N / 10;
//            WL_4N = WL_4N / 10;

//            WL_1_B = WL_1_B / 10;
//            WL_2_B = WL_2_B / 10;

//            WL_1_BN = WL_1_BN / 10;
//            WL_2_BN = WL_2_BN / 10;

//            //SAPDistributedLoad windLoadSuction = new SAPDistributedLoad("WindLeft", WindLeft, 0, 1, -15, -15, HndzLoadDirectionEnum.Local3axis);
//            // SAPDistributedLoad windLoadCompression = new SAPDistributedLoad("WindRight", WindRight, 0, 1, -17, -17, HndzLoadDirectionEnum.Local3axis);


//            //Wind Load Case: WLB
//            SAPDistributedLoad WLBLColumn = new SAPDistributedLoad("WLBLColumn", WLB, 0, 1, WL_1 * inputs.BaySpacing, WL_1 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
//            SAPDistributedLoad WLBRColumn = new SAPDistributedLoad("WLBRColumn", WLB, 0, 1, -WL_4 * inputs.BaySpacing, -WL_4 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

//            SAPDistributedLoad WLBLBeam = new SAPDistributedLoad("WLBLBeam", WLB, 0, 1, -WL_2 * inputs.BaySpacing, -WL_2 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
//            SAPDistributedLoad WLBRBeam = new SAPDistributedLoad("WLBRBeam", WLB, 0, 1, -WL_3 * inputs.BaySpacing, -WL_3 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

//            //Wind Load Case: WLU
//            SAPDistributedLoad WLULColumn = new SAPDistributedLoad("WLULColumn", WLU, 0, 1, -WL_2N * inputs.BaySpacing, -WL_2N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
//            SAPDistributedLoad WLURColumn = new SAPDistributedLoad("WLURColumn", WLU, 0, 1, -WL_3N * inputs.BaySpacing, -WL_3N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

//            SAPDistributedLoad WLULBeam = new SAPDistributedLoad("WLULBeam", WLU, 0, 1, WL_1N * inputs.BaySpacing, WL_1N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
//            SAPDistributedLoad WLURBeam = new SAPDistributedLoad("WLURBeam", WLU, 0, 1, -WL_4N * inputs.BaySpacing, -WL_4N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

//            //Wind Load Case: WRB
//            SAPDistributedLoad WRBLColumn = new SAPDistributedLoad("WRBLColumn", WRB, 0, 1, WL_4 * inputs.BaySpacing, WL_4 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
//            SAPDistributedLoad WRBRColumn = new SAPDistributedLoad("WRBRColumn", WRB, 0, 1, -WL_1 * inputs.BaySpacing, -WL_1 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

//            SAPDistributedLoad WRBLBeam = new SAPDistributedLoad("WRBLBeam", WRB, 0, 1, -WL_3 * inputs.BaySpacing, -WL_3 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
//            SAPDistributedLoad WRBRBeam = new SAPDistributedLoad("WRBRBeam", WRB, 0, 1, -WL_2 * inputs.BaySpacing, -WL_2 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

//            //Wind Load Case: WRU
//            SAPDistributedLoad WRULColumn = new SAPDistributedLoad("WRULColumn", WRU, 0, 1, WL_4N * inputs.BaySpacing, WL_4N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
//            SAPDistributedLoad WRURColumn = new SAPDistributedLoad("WRURColumn", WRU, 0, 1, -WL_1N * inputs.BaySpacing, -WL_1N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

//            SAPDistributedLoad WRULBeam = new SAPDistributedLoad("WRULBeam", WRU, 0, 1, -WL_3N * inputs.BaySpacing, -WL_3N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
//            SAPDistributedLoad WRURBeam = new SAPDistributedLoad("WRURBeam", WRU, 0, 1, -WL_2N * inputs.BaySpacing, -WL_2N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

//            //Wind Load Case: WEB
//            SAPDistributedLoad WEBLColumn = new SAPDistributedLoad("WEBLColumn", WEB, 0, 1, WL_2_B * inputs.BaySpacing, WL_2_B * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
//            SAPDistributedLoad WEBRColumn = new SAPDistributedLoad("WEBRColumn", WEB, 0, 1, -WL_2_B * inputs.BaySpacing, -WL_2_B * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

//            SAPDistributedLoad WEBLBeam = new SAPDistributedLoad("WEBLBeam", WEB, 0, 1, -WL_1_B * inputs.BaySpacing, -WL_1_B * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
//            SAPDistributedLoad WEBRBeam = new SAPDistributedLoad("WEBRBeam", WEB, 0, 1, -WL_1_B * inputs.BaySpacing, -WL_1_B * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

//            //Wind Load Case: WEU 
//            SAPDistributedLoad WEULColumn = new SAPDistributedLoad("WEULColumn", WEU, 0, 1, WL_2_BN * inputs.BaySpacing, WL_2_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
//            SAPDistributedLoad WEURColumn = new SAPDistributedLoad("WEURColumn", WEU, 0, 1, -WL_2_BN * inputs.BaySpacing, -WL_2_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

//            SAPDistributedLoad WEULBeam = new SAPDistributedLoad("WEULBeam", WEU, 0, 1, WL_1_BN * inputs.BaySpacing, WL_1_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
//            SAPDistributedLoad WEURBeam = new SAPDistributedLoad("WEURBeam", WEU, 0, 1, WL_1_BN * inputs.BaySpacing, WL_1_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

//            #endregion
//            //=========================
//            //TODO let this function take an array, make an overloaded version
//            foreach (SAPFrameElement beam in leftBeams)
//            {
//                SAP2000API.AddDistributedLoad(beam, LiveLoad);
//                SAP2000API.AddDistributedLoad(beam, CoverLoad);

//                //SAP2000API.AddDistributedLoad(beam, windLoadCompression);
//                //SAP2000API.AddDistributedLoad(beam, windLoadSuction);

//                SAP2000API.AddDistributedLoad(beam, WLBLBeam);
//                SAP2000API.AddDistributedLoad(beam, WLULBeam);
//                SAP2000API.AddDistributedLoad(beam, WRBLBeam);
//                SAP2000API.AddDistributedLoad(beam, WRULBeam);
//                SAP2000API.AddDistributedLoad(beam, WEBLBeam);
//                SAP2000API.AddDistributedLoad(beam, WEULBeam);

//                Beams.Add(beam);
//            }
//            foreach (SAPFrameElement beam in rightBeams)
//            {
//                SAP2000API.AddDistributedLoad(beam, LiveLoad);
//                SAP2000API.AddDistributedLoad(beam, CoverLoad);

//                //SAP2000API.AddDistributedLoad(beam, windLoadSuction);
//                //SAP2000API.AddDistributedLoad(beam, windLoadCompression);

//                SAP2000API.AddDistributedLoad(beam, WLBRBeam);
//                SAP2000API.AddDistributedLoad(beam, WLURBeam);
//                SAP2000API.AddDistributedLoad(beam, WRBRBeam);
//                SAP2000API.AddDistributedLoad(beam, WRURBeam);
//                SAP2000API.AddDistributedLoad(beam, WEBRBeam);
//                SAP2000API.AddDistributedLoad(beam, WEURBeam);

//                Beams.Add(beam);
//            }

//            //SAP2000API.AddDistributedLoad(leftCols, windLoadCompression);
//            //SAP2000API.AddDistributedLoad(rightCols, windLoadSuction);
//            //=================
//            SAP2000API.AddDistributedLoad(leftCols, WLBLColumn);
//            SAP2000API.AddDistributedLoad(leftCols, WLULColumn);
//            SAP2000API.AddDistributedLoad(leftCols, WRBLColumn);
//            SAP2000API.AddDistributedLoad(leftCols, WRULColumn);
//            SAP2000API.AddDistributedLoad(leftCols, WEBLColumn);
//            SAP2000API.AddDistributedLoad(leftCols, WEULColumn);

//            Columns.AddRange(leftCols);

//            SAP2000API.AddDistributedLoad(rightCols, WLBRColumn);
//            SAP2000API.AddDistributedLoad(rightCols, WLURColumn);
//            SAP2000API.AddDistributedLoad(rightCols, WRBRColumn);
//            SAP2000API.AddDistributedLoad(rightCols, WRURColumn);
//            SAP2000API.AddDistributedLoad(rightCols, WEBRColumn);
//            SAP2000API.AddDistributedLoad(rightCols, WEURColumn);

//            Columns.AddRange(rightCols);
//            // ==========================
//        }

//        private SAPLoadCombination[] DefineLoadDefinitions()
//        {
//            SAPLoadCombination[] combos = new SAPLoadCombination[14];
//            //Defining Load Patterns
//            Dead = new SAPLoadPattern("Dead", eLoadPatternType.Dead, 1, true);
//            Cover = new SAPLoadPattern("Cover", eLoadPatternType.Dead, 0, true);
//            Live = new SAPLoadPattern("Live", eLoadPatternType.Live, 0, true);
//            WLB = new SAPLoadPattern("WLB", eLoadPatternType.Wind, 0, true);
//            WLU = new SAPLoadPattern("WLU", eLoadPatternType.Wind, 0, true);
//            WRB = new SAPLoadPattern("WRB", eLoadPatternType.Wind, 0, true);
//            WRU = new SAPLoadPattern("WRU", eLoadPatternType.Wind, 0, true);
//            WEB = new SAPLoadPattern("WEB", eLoadPatternType.Wind, 0, true);
//            WEU = new SAPLoadPattern("WEU", eLoadPatternType.Wind, 0, true);

//            // WindLeft = new SAPLoadPattern("WindLeft", eLoadPatternType.Wind, 0, true);
//            // WindRight = new SAPLoadPattern("WindRight", eLoadPatternType.Wind, 0, true);
//            //TODO: Temporary ECP Wind Load Cases

//            //Adding Load Patterns to SAP2000
//            SAP2000API.AddLoadPattern(Dead);
//            SAP2000API.AddLoadPattern(Cover);
//            SAP2000API.AddLoadPattern(Live);

//            // SAP2000API.AddLoadPattern(WindRight);
//            // SAP2000API.AddLoadPattern(WindLeft);

//            SAP2000API.AddLoadPattern(WLB);
//            SAP2000API.AddLoadPattern(WLU);
//            SAP2000API.AddLoadPattern(WRB);
//            SAP2000API.AddLoadPattern(WRU);
//            SAP2000API.AddLoadPattern(WEB);
//            SAP2000API.AddLoadPattern(WEU);

//            //Defining Load Combos:
//            //=====================================
//            combos = SAP2000API.AddDesignDefaultCombos(eMatType.Steel);
//            // ====================================
//            return combos;
//        }

//        private SAPMaterial[] DefineMaterials()
//        {
//            //TODO: Read the advanced inputs to see if u have to change the grade
//            return SAP2000API.SetDefaultMaterials();
//            //0 St37, 1 St44, 2 St52, 3 RC30 last one is not available yet
//        }

//        private void InitSAP2000(bool attachToInstance)
//        {
//            SAP2000API.StartApplication(attachToInstance, true, eUnits.Ton_m_C);
//            SAP2000API.NewModel(eUnits.Ton_m_C);
//        }
//        #endregion

//    }
//}

//#endregion