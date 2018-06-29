using HANDAZ.Entities;
using HANDAZ.PEB.AnalysisTools.CsiSAP2000;
using HANDAZ.PEB.Core;
using SAP2000v18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HANDAZ.PEB.Core.ASCE107Wind;
using UnitsNet.Units;

namespace HANDAZ.PEB.BusinessComponents
{
    public class SAPAnalysisModel
    {
        private SAPLoadPattern Dead;
        private SAPLoadPattern Cover;
        private SAPLoadPattern Live;
        private SAPLoadPattern WLB;
        private SAPLoadPattern WLU;
        private SAPLoadPattern WRB;
        private SAPLoadPattern WRU;
        private SAPLoadPattern WEB;
        private SAPLoadPattern WEU;

        private bool IsDesignLoadCombosDefined; //TODO: Fix the design load combos problem
        private abstract class Frame
        {
        }
        private class ClearSpan : Frame
        {
            public SAPFrameElement RightColumn { get; set; }

            public SAPFrameElement RightBeam { get; set; }

            public SAPFrameElement LeftColumn { get; set; }

            public SAPFrameElement LeftBeam { get; set; }
        }
        private class MonoSlope : Frame
        {
            public SAPFrameElement RightColumn { get; set; }

            public SAPFrameElement LeftColumn { get; set; }

            public SAPFrameElement Beam { get; set; }
        }
        private class MultiGable : Frame
        {
            public SAPFrameElement RightColumn { get; set; }
            public SAPFrameElement MiddleColumn { get; set; }

            public SAPFrameElement LeftColumn { get; set; }

            public SAPFrameElement LeftBeamLeft { get; set; }
            public SAPFrameElement RightBeamLeft { get; set; }
            public SAPFrameElement LeftBeamRight { get; set; }
            public SAPFrameElement RightBeamRight { get; set; }
        }
        private class MultiSpan1 : Frame
        {
            public SAPFrameElement RightColumn { get; set; }
            public SAPFrameElement MiddleColumn { get; set; }

            public SAPFrameElement LeftColumn { get; set; }

            public SAPFrameElement LeftBeam { get; set; }
            public SAPFrameElement RightBeam { get; set; }
        }
        private class MultiSpan2 : Frame
        {
            public SAPFrameElement RightColumn { get; set; }
            public SAPFrameElement MiddleColumnLeft { get; set; }
            public SAPFrameElement MiddleColumnRight { get; set; }

            public SAPFrameElement LeftColumn { get; set; }

            public SAPFrameElement LeftBeam { get; set; }
            public SAPFrameElement MiddleBeamLeft { get; set; }
            public SAPFrameElement MiddleBeamRight { get; set; }


            public SAPFrameElement RightBeam { get; set; }
        }
        private class MultiSpan3 : Frame
        {
            public SAPFrameElement RightColumn { get; set; }
            public SAPFrameElement MiddleColumnLeft { get; set; }
            public SAPFrameElement MiddleColumnMiddle { get; set; }
            public SAPFrameElement MiddleColumnRight { get; set; }
            public SAPFrameElement LeftColumn { get; set; }
            public SAPFrameElement LeftBeamLeft { get; set; }
            public SAPFrameElement RightBeamLeft { get; set; }
            public SAPFrameElement LeftBeamRight { get; set; }
            public SAPFrameElement RightBeamRight { get; set; }
        }


        private ClearSpan csFrame = null;
        private MonoSlope msFrame = null;
        private MultiGable mgFrame = null;
        private MultiSpan1 ms1Frame = null;
        private MultiSpan2 ms2Frame = null;
        private MultiSpan3 ms3Frame = null;


        #region Public Methods, Frame Generation Methods
        public void GenerateFrame(HndzFrame3D hndzFrame, string modelName, bool toDesignModel, bool attachToInstance = true)
        {
            //Type type = hndzFrame.GetType();
            if (hndzFrame is HndzFrameSingleBay3D)
            {
                HndzFrameSingleBay3D frames = (HndzFrameSingleBay3D)hndzFrame;
                GenerateClearSpanFrame(frames, modelName, toDesignModel, attachToInstance);
            }
            else if (hndzFrame is HndzFrameMonoSlope3D)
            {
                HndzFrameMonoSlope3D frames = (HndzFrameMonoSlope3D)hndzFrame;
                GenerateMonoSlopeFrame(frames, modelName, toDesignModel, attachToInstance);
            }
            else if (hndzFrame is HndzFrameMultiGable3D)
            {
                HndzFrameMultiGable3D frames = (HndzFrameMultiGable3D)hndzFrame;
                GenerateMultiGableFrame(frames, modelName, toDesignModel, attachToInstance);
            }
            else if (hndzFrame is HndzFrameMultiSpan13D)
            {
                HndzFrameMultiSpan13D frames = (HndzFrameMultiSpan13D)hndzFrame;
                GenerateMultiSpan1Frame(frames, modelName, toDesignModel, attachToInstance);
            }
            else if (hndzFrame is HndzFrameMultiSpan23D)
            {
                HndzFrameMultiSpan23D frames = (HndzFrameMultiSpan23D)hndzFrame;
                GenerateMultiSpan2Frame(frames, modelName, toDesignModel, attachToInstance);
            }
            else if (hndzFrame is HndzFrameMultiSpan33D)
            {
                HndzFrameMultiSpan33D frames = (HndzFrameMultiSpan33D)hndzFrame;
                GenerateMultiSpan3Frame(frames, modelName, toDesignModel, attachToInstance);
            }
        }
        public void GenerateClearSpanFrame(HndzFrameSingleBay3D hndzFrame, string modelName, bool toDesignModel, bool attachToInstance = true)
        {
            #region Extracted Inputs
            CustomerInputs inputs = new CustomerInputs();
            inputs.Width = hndzFrame.Width / 1000;
            inputs.Length = hndzFrame.Length / 1000;
            inputs.NoFrames = hndzFrame.FramesCount;
            inputs.BaySpacing = hndzFrame.BaySpacing / 1000;
            inputs.EaveHeight = hndzFrame.EaveHeight / 1000;
            inputs.RoofSlope = hndzFrame.RoofSlope;
            inputs.ExposureCategory = (ExposureCategory)hndzFrame.ExposureCategory;
            inputs.RiskCategory = (RiskCategory)hndzFrame.RiskCategory;
            inputs.RoofAccessibility = hndzFrame.RoofAccessibility;             inputs.RidgeHeight = hndzFrame.RidgeHeight/1000;;
            double roofSlope;
            switch (inputs.RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            if (inputs.RidgeHeight == 0)
            {
                inputs.RidgeHeight = inputs.EaveHeight + roofSlope * inputs.Width / 2;
            }
            #endregion//Done
            //TODO: Handle units in here
            #region Starting The program
            InitSAP2000(attachToInstance, modelName);
            #endregion

            #region Defining Materials
            SAPMaterial[] materials = DefineMaterials();
            #endregion

            #region Defining I-Sections and Tapered Sections, assumed
            SAPITaperedSection taperedColumn, taperedBeam;
            //Columns
            SAPISection I30 = new SAPISection("I 20x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.2, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            SAPISection I80 = new SAPISection("I 60x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.6, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            taperedColumn = new SAPITaperedSection("I 30-80x0.5/20x1/20x1", I30, I80, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I30);
            SAP2000API.SetISection(I80);
            SAP2000API.SetITaperedSection(taperedColumn);


            //Beams
            SAPISection I50 = new SAPISection("I 50x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.5, 0.2, 0.008, 0.2, 0.008, 0.005);//TODO: HARDCODED 
            taperedBeam = new SAPITaperedSection("I 50-80x0.5/20x1/20x1", I80, I50, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I50);
            SAP2000API.SetITaperedSection(taperedBeam);
            #endregion //SINGLE Modification is done successfully

            #region Loads Definition
            SAPLoadCombination envelope = null;
            SAPLoadCombination[] combos;
            float[] combosFactors;
            combos = DefineLoadDefinitions();
            if (combos.Length > 1)
            {
                IsDesignLoadCombosDefined = true;
                combosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    combosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), combosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
            }
            else
            {
                IsDesignLoadCombosDefined = false;
            }
            #endregion

            #region Drawing 3D Frame and pre-design assumptions
            csFrame = new ClearSpan();
            //Columns
            csFrame.LeftColumn = new SAPFrameElement("Left Column");
            csFrame.LeftColumn.StartPoint = new SAPPoint(csFrame.LeftColumn.Name + "p1", 0, 0, 0);
            csFrame.LeftColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            csFrame.LeftColumn.EndPoint = new SAPPoint(csFrame.LeftColumn.Name + "p2", 0, 0, inputs.EaveHeight);
            csFrame.LeftColumn.Section = taperedColumn;

            csFrame.RightColumn = new SAPFrameElement("Right Column");
            csFrame.RightColumn.StartPoint = new SAPPoint(csFrame.RightColumn.Name + "p1", inputs.Width, 0, 0);
            csFrame.RightColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            csFrame.RightColumn.EndPoint = new SAPPoint(csFrame.RightColumn.Name + "p2", inputs.Width, 0, inputs.EaveHeight);
            csFrame.RightColumn.Section = taperedColumn;

            SAP2000API.AddFrameElement(csFrame.LeftColumn);
            SAP2000API.AddFrameElement(csFrame.RightColumn);

            //Beams
            csFrame.LeftBeam = new SAPFrameElement("Left Beam");
            csFrame.LeftBeam.StartPoint = new SAPPoint(csFrame.LeftBeam.Name + "p2", csFrame.LeftColumn.EndPoint);
            csFrame.LeftBeam.EndPoint = new SAPPoint(csFrame.LeftBeam.Name + "p1", inputs.Width / 2, 0, csFrame.LeftBeam.StartPoint.Z + csFrame.LeftBeam.StartPoint.Z * roofSlope);
            csFrame.LeftBeam.Section = taperedBeam;

            csFrame.RightBeam = new SAPFrameElement("Right Beam");
            csFrame.RightBeam.StartPoint = new SAPPoint(csFrame.RightBeam.Name + "p2", csFrame.RightColumn.EndPoint);
            csFrame.RightBeam.EndPoint = new SAPPoint(csFrame.RightBeam.Name + "p1", csFrame.LeftBeam.EndPoint);
            csFrame.RightBeam.Section = taperedBeam;


            SAP2000API.AddFrameElement(csFrame.LeftBeam);
            SAP2000API.AddFrameElement(csFrame.RightBeam);

            List<SAPFrameElement> frameElements = new List<SAPFrameElement>();
            frameElements.Add(csFrame.LeftBeam);
            frameElements.Add(csFrame.RightBeam);
            frameElements.Add(csFrame.LeftColumn);
            frameElements.Add(csFrame.RightColumn);

            #endregion

            #region Assigning Loads
            AssignLoads(inputs);
            #endregion
            #region Analysis
            AnalyzeModel(modelName, ref frameElements, combos, envelope);

            #endregion
            #region Design
            DesignModel(ref frameElements, toDesignModel);
            #endregion

            #region temp
            if (IsDesignLoadCombosDefined == false)
            {
                combos = SAP2000API.GetAllLoadCombinations();
                float[] loadCombosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    loadCombosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), loadCombosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
                AnalyzeModel(modelName, ref frameElements, combos, envelope);
                DesignModel(ref frameElements, toDesignModel);
                SAP2000API.AddLoadCombination(envelope);
            }

            #endregion
            #region Mapping to HANDAZ Object
            try
            {
                foreach (HndzFrameSingleBay2D frame in hndzFrame.Frames2D)
                {
                    csFrame.RightBeam.ConvertToHndzElement(frame.RightBeam);
                    csFrame.RightBeam.ConvertToHndzElement(frame.LeftBeam);

                    csFrame.RightColumn.ConvertToHndzElement(frame.RightColumn);
                    csFrame.LeftColumn.ConvertToHndzElement(frame.LeftColumn);

                    frame.RightSupport = csFrame.RightColumn.StartPoint.ConvertToHndzSupport();
                    frame.LeftSupport = csFrame.LeftColumn.StartPoint.ConvertToHndzSupport();

                }
            }
            catch (Exception)
            {

            }

            #endregion

        }
        public void GenerateMonoSlopeFrame(HndzFrameMonoSlope3D hndzFrame, string modelName, bool toDesignModel, bool attachToInstance = true)
        {
            #region Extracted Inputs
            CustomerInputs inputs = new CustomerInputs();
            inputs.Width = hndzFrame.Width / 1000;
            inputs.Length = hndzFrame.Length / 1000;
            inputs.NoFrames = hndzFrame.FramesCount;
            inputs.BaySpacing = hndzFrame.BaySpacing / 1000;
            inputs.EaveHeight = hndzFrame.EaveHeight / 1000;
            inputs.RoofSlope = hndzFrame.RoofSlope;
            inputs.ExposureCategory = (ExposureCategory)hndzFrame.ExposureCategory;
            inputs.RiskCategory = (RiskCategory)hndzFrame.RiskCategory;
            inputs.RoofAccessibility = hndzFrame.RoofAccessibility;             inputs.RidgeHeight = hndzFrame.RidgeHeight/1000;;
            double roofSlope;
            switch (inputs.RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            if (inputs.RidgeHeight == 0)
            {
                inputs.RidgeHeight = inputs.EaveHeight + roofSlope * inputs.Width / 2;
            }
            #endregion//Done
            //TODO: Handle units in here
            #region Starting The program
            InitSAP2000(attachToInstance, modelName);
            #endregion

            #region Defining Materials
            SAPMaterial[] materials = DefineMaterials();
            #endregion

            #region Defining I-Sections and Tapered Sections, assumed
            SAPITaperedSection taperedColumn, taperedBeam;
            //Columns
            SAPISection I30 = new SAPISection("I 20x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.2, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            SAPISection I80 = new SAPISection("I 60x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.6, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            taperedColumn = new SAPITaperedSection("I 30-80x0.5/20x1/20x1", I30, I80, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I30);
            SAP2000API.SetISection(I80);
            SAP2000API.SetITaperedSection(taperedColumn);


            //Beams
            SAPISection I40 = new SAPISection("I 40x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.4, 0.2, 0.008, 0.2, 0.008, 0.005);//TODO: HARDCODED 
            taperedBeam = new SAPITaperedSection("I 40-60x0.5/20x1/20x1", I80, I40, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I40);
            SAP2000API.SetITaperedSection(taperedBeam);
            #endregion //SINGLE Modification is done successfully

            #region Loads Definition
            SAPLoadCombination envelope = null;
            SAPLoadCombination[] combos;
            float[] combosFactors;
            combos = DefineLoadDefinitions();
            if (combos.Length > 1)
            {
                IsDesignLoadCombosDefined = true;
                combosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    combosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), combosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
            }
            else
            {
                IsDesignLoadCombosDefined = false;
            }
            #endregion

            #region Drawing 3D Frame and pre-design assumptions
            msFrame = new MonoSlope();
            //Columns
            msFrame.LeftColumn = new SAPFrameElement("Left Column");
            msFrame.LeftColumn.StartPoint = new SAPPoint(msFrame.LeftColumn.Name + "p1", 0, 0, 0);
            msFrame.LeftColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            msFrame.LeftColumn.EndPoint = new SAPPoint(msFrame.LeftColumn.Name + "p2", 0, 0, inputs.EaveHeight);
            msFrame.LeftColumn.Section = taperedColumn;

            msFrame.RightColumn = new SAPFrameElement("Right Column");
            msFrame.RightColumn.StartPoint = new SAPPoint(msFrame.RightColumn.Name + "p1", inputs.Width, 0, 0);
            msFrame.RightColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            msFrame.RightColumn.EndPoint = new SAPPoint(msFrame.RightColumn.Name + "p2", inputs.Width, 0, inputs.EaveHeight + roofSlope * inputs.Width);
            msFrame.RightColumn.Section = taperedColumn;

            SAP2000API.AddFrameElement(msFrame.LeftColumn);
            SAP2000API.AddFrameElement(msFrame.RightColumn);

            //Beams
            msFrame.Beam = new SAPFrameElement("Beam");
            msFrame.Beam.StartPoint = msFrame.LeftColumn.EndPoint;
            msFrame.Beam.EndPoint = msFrame.RightColumn.EndPoint;
            msFrame.Beam.Section = taperedBeam;


            SAP2000API.AddFrameElement(msFrame.Beam);

            List<SAPFrameElement> frameElements = new List<SAPFrameElement>();
            frameElements.Add(msFrame.Beam);
            frameElements.Add(msFrame.LeftColumn);
            frameElements.Add(msFrame.RightColumn);

            #endregion

            #region Assigning Loads
            AssignLoads(inputs);
            #endregion
            #region Analysis
            AnalyzeModel(modelName, ref frameElements, combos, envelope);

            #endregion
            #region Design
            DesignModel(ref frameElements, toDesignModel);
            #endregion

            #region temp
            if (IsDesignLoadCombosDefined == false)
            {
                combos = SAP2000API.GetAllLoadCombinations();
                float[] loadCombosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    loadCombosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), loadCombosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
                AnalyzeModel(modelName, ref frameElements, combos, envelope);
                DesignModel(ref frameElements, toDesignModel);
                SAP2000API.AddLoadCombination(envelope);
            }

            #endregion

            #region Mapping to HANDAZ Object
            try
            {
                foreach (HndzFrameMonoSlope2D frame in hndzFrame.Frames2D)
                {
                    msFrame.Beam.ConvertToHndzElement(frame.Beam);

                    msFrame.LeftColumn.ConvertToHndzElement(frame.LeftColumn);
                    msFrame.RightColumn.ConvertToHndzElement(frame.RightColumn);

                    frame.RightSupport = msFrame.RightColumn.StartPoint.ConvertToHndzSupport();
                    frame.LeftSupport = msFrame.LeftColumn.StartPoint.ConvertToHndzSupport();

                }
            }
            catch (Exception)
            {

            }


            #endregion

        }
        public void GenerateMultiGableFrame(HndzFrameMultiGable3D hndzFrame, string modelName, bool toDesignModel, bool attachToInstance = true)
        {
            #region Extracted Inputs
            CustomerInputs inputs = new CustomerInputs();
            inputs.Width = hndzFrame.Width / 1000;
            inputs.Length = hndzFrame.Length / 1000;
            inputs.NoFrames = hndzFrame.FramesCount;
            inputs.BaySpacing = hndzFrame.BaySpacing / 1000;
            inputs.EaveHeight = hndzFrame.EaveHeight / 1000;
            inputs.RoofSlope = hndzFrame.RoofSlope;
            inputs.ExposureCategory = (ExposureCategory)hndzFrame.ExposureCategory;
            inputs.RiskCategory = (RiskCategory)hndzFrame.RiskCategory;
            inputs.RoofAccessibility = hndzFrame.RoofAccessibility;             inputs.RidgeHeight = hndzFrame.RidgeHeight/1000;;
            double roofSlope;
            switch (inputs.RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            if (inputs.RidgeHeight == 0)
            {
                inputs.RidgeHeight = inputs.EaveHeight + roofSlope * inputs.Width / 2;
            }
            #endregion//Done
            //TODO: Handle units in here
            #region Starting The program
            InitSAP2000(attachToInstance, modelName);
            #endregion

            #region Defining Materials
            SAPMaterial[] materials = DefineMaterials();
            #endregion

            #region Defining I-Sections and Tapered Sections, assumed
            SAPITaperedSection taperedColumn, taperedBeam;
            //Columns
            SAPISection I20 = new SAPISection("I 20x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.2, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            SAPISection I80 = new SAPISection("I 60x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.6, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            taperedColumn = new SAPITaperedSection("I 30-80x0.5/20x1/20x1", I20, I80, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I20);
            SAP2000API.SetISection(I80);
            SAP2000API.SetITaperedSection(taperedColumn);


            //Beams
            SAPISection I40 = new SAPISection("I 40x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.4, 0.2, 0.008, 0.2, 0.008, 0.005);//TODO: HARDCODED 
            taperedBeam = new SAPITaperedSection("I 40-60x0.5/20x1/20x1", I80, I40, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I40);
            SAP2000API.SetITaperedSection(taperedBeam);
            #endregion //SINGLE Modification is done successfully

            #region Loads Definition
            SAPLoadCombination envelope = null;
            SAPLoadCombination[] combos;
            float[] combosFactors;
            combos = DefineLoadDefinitions();
            if (combos.Length > 1)
            {
                IsDesignLoadCombosDefined = true;
                combosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    combosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), combosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
            }
            else
            {
                IsDesignLoadCombosDefined = false;
            }
            #endregion

            #region Drawing 3D Frame and pre-design assumptions
            mgFrame = new MultiGable();
            //Columns
            mgFrame.LeftColumn = new SAPFrameElement("Left Column");
            mgFrame.LeftColumn.StartPoint = new SAPPoint(mgFrame.LeftColumn.Name + "p1", 0, 0, 0);
            mgFrame.LeftColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            mgFrame.LeftColumn.EndPoint = new SAPPoint(mgFrame.LeftColumn.Name + "p2", 0, 0, inputs.EaveHeight);
            mgFrame.LeftColumn.Section = taperedColumn;

            mgFrame.RightColumn = new SAPFrameElement("Right Column");
            mgFrame.RightColumn.StartPoint = new SAPPoint(mgFrame.RightColumn.Name + "p1", inputs.Width, 0, 0);
            mgFrame.RightColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            mgFrame.RightColumn.EndPoint = new SAPPoint(mgFrame.RightColumn.Name + "p2", inputs.Width, 0, inputs.EaveHeight);
            mgFrame.RightColumn.Section = taperedColumn;

            mgFrame.MiddleColumn = new SAPFrameElement("Middle Column");
            mgFrame.MiddleColumn.StartPoint = new SAPPoint(mgFrame.MiddleColumn.Name + "p1", inputs.Width / 2, 0, 0);
            mgFrame.MiddleColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            mgFrame.MiddleColumn.EndPoint = new SAPPoint(mgFrame.MiddleColumn.Name + "p2", inputs.Width / 2, 0, inputs.EaveHeight);
            mgFrame.MiddleColumn.Section = I20;

            SAP2000API.AddFrameElement(mgFrame.LeftColumn);
            SAP2000API.AddFrameElement(mgFrame.RightColumn);
            SAP2000API.AddFrameElement(mgFrame.MiddleColumn);


            //Beams
            mgFrame.LeftBeamLeft = new SAPFrameElement("Left Beam Left");
            mgFrame.LeftBeamLeft.StartPoint = new SAPPoint(mgFrame.LeftBeamLeft.Name + "p2", mgFrame.LeftColumn.EndPoint);
            mgFrame.LeftBeamLeft.EndPoint = new SAPPoint(mgFrame.LeftBeamLeft.Name + "p1", inputs.Width / 4, 0, mgFrame.LeftBeamLeft.StartPoint.Z + mgFrame.LeftBeamLeft.StartPoint.Z * roofSlope);
            mgFrame.LeftBeamLeft.Section = taperedBeam;

            mgFrame.LeftBeamRight = new SAPFrameElement("Left Beam Right");
            mgFrame.LeftBeamRight.StartPoint = new SAPPoint(mgFrame.LeftBeamRight.Name + "p2", mgFrame.MiddleColumn.EndPoint);
            mgFrame.LeftBeamRight.EndPoint = new SAPPoint(mgFrame.LeftBeamRight.Name + "p1", inputs.Width / 4, 0, mgFrame.LeftBeamRight.StartPoint.Z + mgFrame.LeftBeamRight.StartPoint.Z * roofSlope);
            mgFrame.LeftBeamRight.Section = taperedBeam;

            mgFrame.RightBeamRight = new SAPFrameElement("Right Beam Right");
            mgFrame.RightBeamRight.StartPoint = new SAPPoint(mgFrame.RightBeamRight.Name + "p2", mgFrame.RightColumn.EndPoint);
            mgFrame.RightBeamRight.EndPoint = new SAPPoint(mgFrame.RightBeamRight.Name + "p1", 3 * inputs.Width / 4, 0, mgFrame.RightBeamRight.StartPoint.Z + mgFrame.RightBeamRight.StartPoint.Z * roofSlope);
            mgFrame.RightBeamRight.Section = taperedBeam;

            mgFrame.RightBeamLeft = new SAPFrameElement("Right Beam Left");
            mgFrame.RightBeamLeft.StartPoint = new SAPPoint(mgFrame.RightBeamLeft.Name + "p2", mgFrame.RightBeamRight.EndPoint);
            mgFrame.RightBeamLeft.EndPoint = new SAPPoint(mgFrame.RightBeamLeft.Name + "p1", mgFrame.MiddleColumn.EndPoint);
            mgFrame.RightBeamLeft.Section = taperedBeam;


            SAP2000API.AddFrameElement(mgFrame.LeftBeamLeft);
            SAP2000API.AddFrameElement(mgFrame.LeftBeamRight);
            SAP2000API.AddFrameElement(mgFrame.RightBeamLeft);
            SAP2000API.AddFrameElement(mgFrame.RightBeamRight);




            List<SAPFrameElement> frameElements = new List<SAPFrameElement>();
            frameElements.Add(mgFrame.LeftBeamLeft);
            frameElements.Add(mgFrame.LeftBeamRight);
            frameElements.Add(mgFrame.RightBeamLeft);
            frameElements.Add(mgFrame.RightBeamRight);
            frameElements.Add(mgFrame.MiddleColumn);
            frameElements.Add(mgFrame.LeftColumn);
            frameElements.Add(mgFrame.RightColumn);

            #endregion

            #region Assigning Loads
            AssignLoads(inputs);
            #endregion
            #region Analysis
            AnalyzeModel(modelName, ref frameElements, combos, envelope);

            #endregion
            #region Design
            DesignModel(ref frameElements, toDesignModel);
            #endregion

            #region temp
            if (IsDesignLoadCombosDefined == false)
            {
                combos = SAP2000API.GetAllLoadCombinations();
                float[] loadCombosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    loadCombosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), loadCombosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
                AnalyzeModel(modelName, ref frameElements, combos, envelope);
                DesignModel(ref frameElements, toDesignModel);
                SAP2000API.AddLoadCombination(envelope);
            }

            #endregion

            #region Mapping to HANDAZ Object
            try
            {
                foreach (HndzFrameMultiGable2D frame in hndzFrame.Frames2D)
                {
                    mgFrame.LeftBeamLeft.ConvertToHndzElement(frame.LeftBeamLeft);
                    mgFrame.LeftBeamRight.ConvertToHndzElement(frame.LeftBeamRight);
                    mgFrame.RightBeamLeft.ConvertToHndzElement(frame.RightBeamLeft);
                    mgFrame.RightBeamRight.ConvertToHndzElement(frame.RightBeamRight);


                    mgFrame.LeftColumn.ConvertToHndzElement(frame.LeftColumn);
                    mgFrame.RightColumn.ConvertToHndzElement(frame.RightColumn);
                    mgFrame.MiddleColumn.ConvertToHndzElement(frame.MiddleColumn);


                    frame.RightSupport = mgFrame.RightColumn.StartPoint.ConvertToHndzSupport();
                    frame.LeftSupport = mgFrame.LeftColumn.StartPoint.ConvertToHndzSupport();
                    frame.MiddleSupport = mgFrame.MiddleColumn.StartPoint.ConvertToHndzSupport();


                }
            }
            catch (Exception)
            {

            }


            #endregion

        }
        public void GenerateMultiSpan1Frame(HndzFrameMultiSpan13D hndzFrame, string modelName, bool toDesignModel, bool attachToInstance = true)
        {
            #region Extracted Inputs
            CustomerInputs inputs = new CustomerInputs();
            inputs.Width = hndzFrame.Width / 1000;
            inputs.Length = hndzFrame.Length / 1000;
            inputs.NoFrames = hndzFrame.FramesCount;
            inputs.BaySpacing = hndzFrame.BaySpacing / 1000;
            inputs.EaveHeight = hndzFrame.EaveHeight / 1000;
            inputs.RoofSlope = hndzFrame.RoofSlope;
            inputs.ExposureCategory = (ExposureCategory)hndzFrame.ExposureCategory;
            inputs.RiskCategory = (RiskCategory)hndzFrame.RiskCategory;
            inputs.RoofAccessibility = hndzFrame.RoofAccessibility;             inputs.RidgeHeight = hndzFrame.RidgeHeight/1000;;
            double roofSlope;
            switch (inputs.RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            if (inputs.RidgeHeight == 0)
            {
                inputs.RidgeHeight = inputs.EaveHeight + roofSlope * inputs.Width / 2;
            }
            #endregion//Done
            //TODO: Handle units in here
            #region Starting The program
            InitSAP2000(attachToInstance, modelName);
            #endregion

            #region Defining Materials
            SAPMaterial[] materials = DefineMaterials();
            #endregion

            #region Defining I-Sections and Tapered Sections, assumed
            SAPITaperedSection taperedColumn, taperedBeam;
            //Columns
            SAPISection I20 = new SAPISection("I 20x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.2, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            SAPISection I80 = new SAPISection("I 60x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.6, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            taperedColumn = new SAPITaperedSection("I 30-80x0.5/20x1/20x1", I20, I80, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I20);
            SAP2000API.SetISection(I80);
            SAP2000API.SetITaperedSection(taperedColumn);


            //Beams
            SAPISection I40 = new SAPISection("I 40x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.4, 0.2, 0.008, 0.2, 0.008, 0.005);//TODO: HARDCODED 
            taperedBeam = new SAPITaperedSection("I 40-60x0.5/20x1/20x1", I80, I40, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I40);
            SAP2000API.SetITaperedSection(taperedBeam);
            #endregion //SINGLE Modification is done successfully

            #region Loads Definition
            SAPLoadCombination envelope = null;
            SAPLoadCombination[] combos;
            float[] combosFactors;
            combos = DefineLoadDefinitions();
            if (combos.Length > 1)
            {
                IsDesignLoadCombosDefined = true;
                combosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    combosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), combosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
            }
            else
            {
                IsDesignLoadCombosDefined = false;
            }
            #endregion

            #region Drawing 3D Frame and pre-design assumptions
            ms1Frame = new MultiSpan1();
            //Columns
            ms1Frame.LeftColumn = new SAPFrameElement("Left Column");
            ms1Frame.LeftColumn.StartPoint = new SAPPoint(ms1Frame.LeftColumn.Name + "p1", 0, 0, 0);
            ms1Frame.LeftColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms1Frame.LeftColumn.EndPoint = new SAPPoint(ms1Frame.LeftColumn.Name + "p2", 0, 0, inputs.EaveHeight);
            ms1Frame.LeftColumn.Section = taperedColumn;

            ms1Frame.RightColumn = new SAPFrameElement("Right Column");
            ms1Frame.RightColumn.StartPoint = new SAPPoint(ms1Frame.RightColumn.Name + "p1", inputs.Width, 0, 0);
            ms1Frame.RightColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms1Frame.RightColumn.EndPoint = new SAPPoint(ms1Frame.RightColumn.Name + "p2", inputs.Width, 0, inputs.EaveHeight);
            ms1Frame.RightColumn.Section = taperedColumn;

            ms1Frame.MiddleColumn = new SAPFrameElement("Middle Column");
            ms1Frame.MiddleColumn.StartPoint = new SAPPoint(ms1Frame.MiddleColumn.Name + "p1", inputs.Width / 2, 0, 0);
            ms1Frame.MiddleColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms1Frame.MiddleColumn.EndPoint = new SAPPoint(ms1Frame.MiddleColumn.Name + "p2", inputs.Width / 2, 0, inputs.RidgeHeight);
            ms1Frame.MiddleColumn.Section = I20;

            SAP2000API.AddFrameElement(ms1Frame.LeftColumn);
            SAP2000API.AddFrameElement(ms1Frame.RightColumn);
            SAP2000API.AddFrameElement(ms1Frame.MiddleColumn);


            //Beams
            ms1Frame.LeftBeam = new SAPFrameElement("Left Beam");
            ms1Frame.LeftBeam.StartPoint = new SAPPoint(ms1Frame.LeftBeam.Name + "p2", ms1Frame.LeftColumn.EndPoint);
            ms1Frame.LeftBeam.EndPoint = new SAPPoint(ms1Frame.LeftBeam.Name + "p1", ms1Frame.MiddleColumn.EndPoint);
            ms1Frame.LeftBeam.Section = taperedBeam;

            ms1Frame.RightBeam = new SAPFrameElement("Right Beam");
            ms1Frame.RightBeam.StartPoint = new SAPPoint(ms1Frame.RightBeam.Name + "p2", ms1Frame.RightColumn.EndPoint);
            ms1Frame.RightBeam.EndPoint = new SAPPoint(ms1Frame.RightBeam.Name + "p1", ms1Frame.MiddleColumn.EndPoint);
            ms1Frame.RightBeam.Section = taperedBeam;


            SAP2000API.AddFrameElement(ms1Frame.RightBeam);
            SAP2000API.AddFrameElement(ms1Frame.LeftBeam);




            List<SAPFrameElement> frameElements = new List<SAPFrameElement>();
            frameElements.Add(ms1Frame.LeftBeam);
            frameElements.Add(ms1Frame.RightBeam);
            frameElements.Add(ms1Frame.RightColumn);
            frameElements.Add(ms1Frame.LeftColumn);
            frameElements.Add(ms1Frame.MiddleColumn);

            #endregion

            #region Assigning Loads
            AssignLoads(inputs);
            #endregion
            #region Analysis
            AnalyzeModel(modelName, ref frameElements, combos, envelope);

            #endregion
            #region Design
            DesignModel(ref frameElements, toDesignModel);
            #endregion

            #region temp
            if (IsDesignLoadCombosDefined == false)
            {
                combos = SAP2000API.GetAllLoadCombinations();
                float[] loadCombosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    loadCombosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), loadCombosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
                AnalyzeModel(modelName, ref frameElements, combos, envelope);
                DesignModel(ref frameElements, toDesignModel);
                SAP2000API.AddLoadCombination(envelope);
            }

            #endregion

            #region Mapping to HANDAZ Object
            try
            {
                foreach (HndzFrameMultiSpan12D frame in hndzFrame.Frames2D)
                {
                    ms1Frame.RightBeam.ConvertToHndzElement(frame.RightBeam);
                    ms1Frame.LeftBeam.ConvertToHndzElement(frame.LeftBeam);


                    ms1Frame.RightColumn.ConvertToHndzElement(frame.RightColumn);
                    ms1Frame.LeftColumn.ConvertToHndzElement(frame.LeftColumn);
                    ms1Frame.MiddleColumn.ConvertToHndzElement(frame.MiddleColumn);

                    frame.RightSupport = ms1Frame.RightColumn.StartPoint.ConvertToHndzSupport();
                    frame.LeftSupport = ms1Frame.LeftColumn.StartPoint.ConvertToHndzSupport();
                    frame.MiddleSupport = ms1Frame.MiddleColumn.StartPoint.ConvertToHndzSupport();


                }
            }
            catch (Exception)
            {

            }


            #endregion

        }
        public void GenerateMultiSpan2Frame(HndzFrameMultiSpan23D hndzFrame, string modelName, bool toDesignModel, bool attachToInstance = true)
        {
            #region Extracted Inputs
            CustomerInputs inputs = new CustomerInputs();
            inputs.Width = hndzFrame.Width / 1000;
            inputs.Length = hndzFrame.Length / 1000;
            inputs.NoFrames = hndzFrame.FramesCount;
            inputs.BaySpacing = hndzFrame.BaySpacing / 1000;
            inputs.EaveHeight = hndzFrame.EaveHeight / 1000;
            inputs.RoofSlope = hndzFrame.RoofSlope;
            inputs.ExposureCategory = (ExposureCategory)hndzFrame.ExposureCategory;
            inputs.RiskCategory = (RiskCategory)hndzFrame.RiskCategory;
            inputs.RoofAccessibility = hndzFrame.RoofAccessibility;             inputs.RidgeHeight = hndzFrame.RidgeHeight/1000;;
            double roofSlope;
            switch (inputs.RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            if (inputs.RidgeHeight == 0)
            {
                inputs.RidgeHeight = inputs.EaveHeight + roofSlope * inputs.Width / 2;
            }
            #endregion//Done
            //TODO: Handle units in here
            #region Starting The program
            InitSAP2000(attachToInstance, modelName);
            #endregion

            #region Defining Materials
            SAPMaterial[] materials = DefineMaterials();
            #endregion

            #region Defining I-Sections and Tapered Sections, assumed
            SAPITaperedSection taperedColumn, taperedBeam;
            //Columns
            SAPISection I20 = new SAPISection("I 20x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.2, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            SAPISection I80 = new SAPISection("I 60x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.6, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            taperedColumn = new SAPITaperedSection("I 30-80x0.5/20x1/20x1", I20, I80, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I20);
            SAP2000API.SetISection(I80);
            SAP2000API.SetITaperedSection(taperedColumn);


            //Beams
            SAPISection I40 = new SAPISection("I 40x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.4, 0.2, 0.008, 0.2, 0.008, 0.005);//TODO: HARDCODED 
            taperedBeam = new SAPITaperedSection("I 40-60x0.5/20x1/20x1", I80, I40, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I40);
            SAP2000API.SetITaperedSection(taperedBeam);
            #endregion //SINGLE Modification is done successfully

            #region Loads Definition
            SAPLoadCombination envelope = null;
            SAPLoadCombination[] combos;
            float[] combosFactors;
            combos = DefineLoadDefinitions();
            if (combos.Length > 1)
            {
                IsDesignLoadCombosDefined = true;
                combosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    combosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), combosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
            }
            else
            {
                IsDesignLoadCombosDefined = false;
            }
            #endregion

            #region Drawing 3D Frame and pre-design assumptions
            ms2Frame = new MultiSpan2();
            //Columns
            ms2Frame.LeftColumn = new SAPFrameElement("Left Column");
            ms2Frame.LeftColumn.StartPoint = new SAPPoint(ms2Frame.LeftColumn.Name + "p1", 0, 0, 0);
            ms2Frame.LeftColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms2Frame.LeftColumn.EndPoint = new SAPPoint(ms2Frame.LeftColumn.Name + "p2", 0, 0, inputs.EaveHeight);
            ms2Frame.LeftColumn.Section = taperedColumn;

            ms2Frame.RightColumn = new SAPFrameElement("Right Column");
            ms2Frame.RightColumn.StartPoint = new SAPPoint(ms2Frame.RightColumn.Name + "p1", inputs.Width, 0, 0);
            ms2Frame.RightColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms2Frame.RightColumn.EndPoint = new SAPPoint(ms2Frame.RightColumn.Name + "p2", inputs.Width, 0, inputs.EaveHeight);
            ms2Frame.RightColumn.Section = taperedColumn;

            ms2Frame.MiddleColumnLeft = new SAPFrameElement("Middle Column Left");
            ms2Frame.MiddleColumnLeft.StartPoint = new SAPPoint(ms2Frame.MiddleColumnLeft.Name + "p1", inputs.Width / 4, 0, 0);
            ms2Frame.MiddleColumnLeft.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms2Frame.MiddleColumnLeft.EndPoint = new SAPPoint(ms2Frame.MiddleColumnLeft.Name + "p2", inputs.Width / 4, 0, inputs.EaveHeight + (inputs.RidgeHeight - inputs.EaveHeight) / 2);
            ms2Frame.MiddleColumnLeft.Section = I20;

            ms2Frame.MiddleColumnRight = new SAPFrameElement("Middle Column Right");
            ms2Frame.MiddleColumnRight.StartPoint = new SAPPoint(ms2Frame.MiddleColumnLeft.Name + "p1", 3 * inputs.Width / 4, 0, 0);
            ms2Frame.MiddleColumnRight.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms2Frame.MiddleColumnRight.EndPoint = new SAPPoint(ms2Frame.MiddleColumnLeft.Name + "p2", 3 * inputs.Width / 4, 0, inputs.EaveHeight + (inputs.RidgeHeight - inputs.EaveHeight) / 2);
            ms2Frame.MiddleColumnRight.Section = I20;

            SAP2000API.AddFrameElement(ms2Frame.LeftColumn);
            SAP2000API.AddFrameElement(ms2Frame.RightColumn);
            SAP2000API.AddFrameElement(ms2Frame.MiddleColumnLeft);
            SAP2000API.AddFrameElement(ms2Frame.MiddleColumnRight);



            //Beams
            ms2Frame.LeftBeam = new SAPFrameElement("Left Beam");
            ms2Frame.LeftBeam.StartPoint = new SAPPoint(ms2Frame.LeftBeam.Name + "p2", ms2Frame.LeftColumn.EndPoint);
            ms2Frame.LeftBeam.EndPoint = new SAPPoint(ms2Frame.LeftBeam.Name + "p1", ms2Frame.MiddleColumnLeft.EndPoint);
            ms2Frame.LeftBeam.Section = taperedBeam;

            ms2Frame.RightBeam = new SAPFrameElement("Right Beam");
            ms2Frame.RightBeam.StartPoint = new SAPPoint(ms2Frame.RightBeam.Name + "p2", ms2Frame.RightColumn.EndPoint);
            ms2Frame.RightBeam.EndPoint = new SAPPoint(ms2Frame.RightBeam.Name + "p1", ms2Frame.MiddleColumnRight.EndPoint);
            ms2Frame.RightBeam.Section = taperedBeam;

            ms2Frame.MiddleBeamLeft = new SAPFrameElement("Middle Beam Left");
            ms2Frame.MiddleBeamLeft.StartPoint = new SAPPoint(ms2Frame.RightBeam.Name + "p2", ms2Frame.MiddleColumnLeft.EndPoint);
            ms2Frame.MiddleBeamLeft.EndPoint = new SAPPoint(ms2Frame.RightBeam.Name + "p1", inputs.Width / 2, 0, inputs.RidgeHeight);
            ms2Frame.MiddleBeamLeft.Section = taperedBeam;

            ms2Frame.MiddleBeamRight = new SAPFrameElement("Middle Beam Right");
            ms2Frame.MiddleBeamRight.StartPoint = new SAPPoint(ms2Frame.RightBeam.Name + "p2", ms2Frame.MiddleBeamLeft.EndPoint);
            ms2Frame.MiddleBeamRight.EndPoint = new SAPPoint(ms2Frame.RightBeam.Name + "p1", ms2Frame.MiddleColumnRight.EndPoint);
            ms2Frame.MiddleBeamRight.Section = taperedBeam;


            SAP2000API.AddFrameElement(ms2Frame.RightBeam);
            SAP2000API.AddFrameElement(ms2Frame.LeftBeam);
            SAP2000API.AddFrameElement(ms2Frame.MiddleBeamLeft);
            SAP2000API.AddFrameElement(ms2Frame.MiddleBeamRight);






            List<SAPFrameElement> frameElements = new List<SAPFrameElement>();
            frameElements.Add(ms2Frame.LeftBeam);
            frameElements.Add(ms2Frame.RightBeam);
            frameElements.Add(ms2Frame.MiddleBeamLeft);
            frameElements.Add(ms2Frame.MiddleBeamRight);
            frameElements.Add(ms2Frame.RightColumn);
            frameElements.Add(ms2Frame.LeftColumn);
            frameElements.Add(ms2Frame.MiddleColumnRight);
            frameElements.Add(ms2Frame.MiddleColumnLeft);


            #endregion

            #region Assigning Loads
            AssignLoads(inputs);
            #endregion
            #region Analysis
            AnalyzeModel(modelName, ref frameElements, combos, envelope);

            #endregion
            #region Design
            DesignModel(ref frameElements, toDesignModel);
            #endregion

            #region temp
            if (IsDesignLoadCombosDefined == false)
            {
                combos = SAP2000API.GetAllLoadCombinations();
                float[] loadCombosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    loadCombosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), loadCombosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
                AnalyzeModel(modelName, ref frameElements, combos, envelope);
                DesignModel(ref frameElements, toDesignModel);
                SAP2000API.AddLoadCombination(envelope);
            }

            #endregion

            #region Mapping to HANDAZ Object
            try
            {
                foreach (HndzFrameMultiSpan22D frame in hndzFrame.Frames2D)
                {
                    ms2Frame.RightBeam.ConvertToHndzElement(frame.RightBeam);
                    ms2Frame.LeftBeam.ConvertToHndzElement(frame.LeftBeam);
                    ms2Frame.MiddleBeamLeft.ConvertToHndzElement(frame.MiddleBeamLeft);
                    ms2Frame.MiddleBeamRight.ConvertToHndzElement(frame.MiddleBeamRight);



                    ms2Frame.RightColumn.ConvertToHndzElement(frame.RightColumn);
                    ms2Frame.LeftColumn.ConvertToHndzElement(frame.LeftColumn);
                    ms2Frame.MiddleColumnLeft.ConvertToHndzElement(frame.MiddleColumnLeft);
                    ms2Frame.MiddleColumnRight.ConvertToHndzElement(frame.MiddleColumnRight);


                    frame.RightSupport = ms2Frame.RightColumn.StartPoint.ConvertToHndzSupport();
                    frame.LeftSupport = ms2Frame.LeftColumn.StartPoint.ConvertToHndzSupport();
                    frame.MiddleSupportLeft = ms2Frame.MiddleColumnLeft.StartPoint.ConvertToHndzSupport();
                    frame.MiddleSupportRight = ms2Frame.MiddleColumnRight.StartPoint.ConvertToHndzSupport();

                }
            }
            catch (Exception)
            {

            }
          

            #endregion

        }
        public void GenerateMultiSpan3Frame(HndzFrameMultiSpan33D hndzFrame, string modelName, bool toDesignModel, bool attachToInstance = true)
        {
            #region Extracted Inputs
            CustomerInputs inputs = new CustomerInputs();
            inputs.Width = hndzFrame.Width / 1000;
            inputs.Length = hndzFrame.Length / 1000;
            inputs.NoFrames = hndzFrame.FramesCount;
            inputs.BaySpacing = hndzFrame.BaySpacing / 1000;
            inputs.EaveHeight = hndzFrame.EaveHeight / 1000;
            inputs.RoofSlope = hndzFrame.RoofSlope;
            inputs.ExposureCategory = (ExposureCategory)hndzFrame.ExposureCategory;
            inputs.RiskCategory = (RiskCategory)hndzFrame.RiskCategory;
            inputs.RoofAccessibility = hndzFrame.RoofAccessibility;
            inputs.RidgeHeight = hndzFrame.RidgeHeight/1000;
       
            double roofSlope;
            switch (inputs.RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            if (inputs.RidgeHeight == 0)
            {
                inputs.RidgeHeight = inputs.EaveHeight + roofSlope * inputs.Width / 2;
            }
            #endregion//Done
            //TODO: Handle units in here
            #region Starting The program
            InitSAP2000(attachToInstance, modelName);
            #endregion

            #region Defining Materials
            SAPMaterial[] materials = DefineMaterials();
            #endregion

            #region Defining I-Sections and Tapered Sections, assumed
            SAPITaperedSection taperedColumn, taperedBeam;
            //Columns
            SAPISection I20 = new SAPISection("I 20x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.2, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            SAPISection I80 = new SAPISection("I 60x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.6, 0.2, 0.008, 0.2, 0.008, 0.005); //TODO: HARDCODED 
            taperedColumn = new SAPITaperedSection("I 30-80x0.5/20x1/20x1", I20, I80, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I20);
            SAP2000API.SetISection(I80);
            SAP2000API.SetITaperedSection(taperedColumn);


            //Beams
            SAPISection I40 = new SAPISection("I 40x0.5/20x1/20x1", materials[2], materials[2], materials[2], 0.4, 0.2, 0.008, 0.2, 0.008, 0.005);//TODO: HARDCODED 
            taperedBeam = new SAPITaperedSection("I 40-60x0.5/20x1/20x1", I80, I40, 1, LengthTypeEnum.Relative);
            SAP2000API.SetISection(I40);
            SAP2000API.SetITaperedSection(taperedBeam);
            #endregion //SINGLE Modification is done successfully

            #region Loads Definition
            SAPLoadCombination envelope = null;
            SAPLoadCombination[] combos;
            float[] combosFactors;
            combos = DefineLoadDefinitions();
            if (combos.Length > 1)
            {
                IsDesignLoadCombosDefined = true;
                combosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    combosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), combosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
            }
            else
            {
                IsDesignLoadCombosDefined = false;
            }
            #endregion

            #region Drawing 3D Frame and pre-design assumptions
            ms3Frame = new MultiSpan3();
            //Columns
            ms3Frame.LeftColumn = new SAPFrameElement("Left Column");
            ms3Frame.LeftColumn.StartPoint = new SAPPoint(ms3Frame.LeftColumn.Name + "p1", 0, 0, 0);
            ms3Frame.LeftColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms3Frame.LeftColumn.EndPoint = new SAPPoint(ms3Frame.LeftColumn.Name + "p2", 0, 0, inputs.EaveHeight);
            ms3Frame.LeftColumn.Section = taperedColumn;

            ms3Frame.RightColumn = new SAPFrameElement("Right Column");
            ms3Frame.RightColumn.StartPoint = new SAPPoint(ms3Frame.RightColumn.Name + "p1", inputs.Width, 0, 0);
            ms3Frame.RightColumn.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms3Frame.RightColumn.EndPoint = new SAPPoint(ms3Frame.RightColumn.Name + "p2", inputs.Width, 0, inputs.EaveHeight);
            ms3Frame.RightColumn.Section = taperedColumn;

            ms3Frame.MiddleColumnLeft = new SAPFrameElement("Middle Column Left");
            ms3Frame.MiddleColumnLeft.StartPoint = new SAPPoint(ms3Frame.MiddleColumnLeft.Name + "p1", inputs.Width / 4, 0, 0);
            ms3Frame.MiddleColumnLeft.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms3Frame.MiddleColumnLeft.EndPoint = new SAPPoint(ms3Frame.MiddleColumnLeft.Name + "p2", inputs.Width / 4, 0, inputs.EaveHeight + (inputs.RidgeHeight - inputs.EaveHeight) / 2);
            ms3Frame.MiddleColumnLeft.Section = I20;

            ms3Frame.MiddleColumnRight = new SAPFrameElement("Middle Column Right");
            ms3Frame.MiddleColumnRight.StartPoint = new SAPPoint(ms3Frame.MiddleColumnRight.Name + "p1", 3 * inputs.Width / 4, 0, 0);
            ms3Frame.MiddleColumnRight.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms3Frame.MiddleColumnRight.EndPoint = new SAPPoint(ms3Frame.MiddleColumnRight.Name + "p2", 3 * inputs.Width / 4, 0, inputs.EaveHeight + (inputs.RidgeHeight - inputs.EaveHeight) / 2);
            ms3Frame.MiddleColumnRight.Section = I20;

            ms3Frame.MiddleColumnMiddle = new SAPFrameElement("Middle Column Middle");
            ms3Frame.MiddleColumnMiddle.StartPoint = new SAPPoint(ms3Frame.MiddleColumnMiddle.Name + "p1", inputs.Width / 2, 0, 0);
            ms3Frame.MiddleColumnMiddle.StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
            ms3Frame.MiddleColumnMiddle.EndPoint = new SAPPoint(ms3Frame.MiddleColumnMiddle.Name + "p2", inputs.Width / 2, 0, inputs.RidgeHeight);
            ms3Frame.MiddleColumnMiddle.Section = I20;

            SAP2000API.AddFrameElement(ms3Frame.LeftColumn);
            SAP2000API.AddFrameElement(ms3Frame.RightColumn);
            SAP2000API.AddFrameElement(ms3Frame.MiddleColumnLeft);
            SAP2000API.AddFrameElement(ms3Frame.MiddleColumnRight);
            SAP2000API.AddFrameElement(ms3Frame.MiddleColumnMiddle);

            //Beams
            ms3Frame.LeftBeamLeft = new SAPFrameElement("Left Beam Left");
            ms3Frame.LeftBeamLeft.StartPoint = new SAPPoint(ms3Frame.LeftBeamLeft.Name + "p2", ms3Frame.LeftColumn.EndPoint);
            ms3Frame.LeftBeamLeft.EndPoint = new SAPPoint(ms3Frame.LeftBeamLeft.Name + "p1", ms3Frame.MiddleColumnLeft.EndPoint);
            ms3Frame.LeftBeamLeft.Section = taperedBeam;

            ms3Frame.RightBeamRight = new SAPFrameElement("Right Beam Right");
            ms3Frame.RightBeamRight.StartPoint = new SAPPoint(ms3Frame.RightBeamRight.Name + "p2", ms3Frame.RightColumn.EndPoint);
            ms3Frame.RightBeamRight.EndPoint = new SAPPoint(ms3Frame.RightBeamRight.Name + "p1", ms3Frame.MiddleColumnRight.EndPoint);
            ms3Frame.RightBeamRight.Section = taperedBeam;

            ms3Frame.RightBeamLeft = new SAPFrameElement("Right Beam Left");
            ms3Frame.RightBeamLeft.StartPoint = new SAPPoint(ms3Frame.RightBeamLeft.Name + "p2", ms3Frame.MiddleColumnLeft.EndPoint);
            ms3Frame.RightBeamLeft.EndPoint = new SAPPoint(ms3Frame.RightBeamLeft.Name + "p1", ms3Frame.MiddleColumnMiddle.EndPoint);
            ms3Frame.RightBeamLeft.Section = taperedBeam;

            ms3Frame.LeftBeamRight = new SAPFrameElement("Left Beam Right");
            ms3Frame.LeftBeamRight.StartPoint = new SAPPoint(ms3Frame.LeftBeamRight.Name + "p2", ms3Frame.MiddleColumnRight.EndPoint);
            ms3Frame.LeftBeamRight.EndPoint = new SAPPoint(ms3Frame.LeftBeamRight.Name + "p1", ms3Frame.MiddleColumnMiddle.EndPoint);
            ms3Frame.LeftBeamRight.Section = taperedBeam;


            SAP2000API.AddFrameElement(ms3Frame.LeftBeamLeft);
            SAP2000API.AddFrameElement(ms3Frame.LeftBeamRight);
            SAP2000API.AddFrameElement(ms3Frame.RightBeamLeft);
            SAP2000API.AddFrameElement(ms3Frame.RightBeamRight);

            List<SAPFrameElement> frameElements = new List<SAPFrameElement>();
            frameElements.Add(ms3Frame.RightBeamRight);
            frameElements.Add(ms3Frame.RightBeamLeft);
            frameElements.Add(ms3Frame.LeftBeamRight);
            frameElements.Add(ms3Frame.LeftBeamLeft);
            frameElements.Add(ms3Frame.RightColumn);
            frameElements.Add(ms3Frame.LeftColumn);
            frameElements.Add(ms3Frame.MiddleColumnRight);
            frameElements.Add(ms3Frame.MiddleColumnLeft);


            #endregion

            #region Assigning Loads
            AssignLoads(inputs);
            #endregion
            #region Analysis
            AnalyzeModel(modelName, ref frameElements, combos, envelope);

            #endregion
            #region Design
            DesignModel(ref frameElements, toDesignModel);
            #endregion

            #region temp
            if (IsDesignLoadCombosDefined == false)
            {
                combos = SAP2000API.GetAllLoadCombinations();
                float[] loadCombosFactors = new float[combos.Length];
                for (int i = 0; i < combos.Length; i++)
                {
                    loadCombosFactors[i] = 1;
                }
                envelope = new SAPLoadCombination("Envelope", LoadCombinationsEnum.Envelope, null, null, combos.ToList(), loadCombosFactors.ToList());
                SAP2000API.AddLoadCombination(envelope);
                AnalyzeModel(modelName, ref frameElements, combos, envelope);
                DesignModel(ref frameElements, toDesignModel);
                SAP2000API.AddLoadCombination(envelope);
            }

            #endregion

            #region Mapping to HANDAZ Object
            try
            {
                foreach (HndzFrameMultiSpan32D frame in hndzFrame.Frames2D)
                {
                    ms3Frame.LeftBeamLeft.ConvertToHndzElement(frame.LeftBeamLeft);
                    ms3Frame.LeftBeamRight.ConvertToHndzElement(frame.LeftBeamRight);
                    ms3Frame.RightBeamLeft.ConvertToHndzElement(frame.RightBeamLeft);
                    ms3Frame.RightBeamRight.ConvertToHndzElement(frame.RightBeamRight);



                    ms3Frame.RightColumn.ConvertToHndzElement(frame.RightColumn);
                    ms3Frame.LeftColumn.ConvertToHndzElement(frame.LeftColumn);
                    ms3Frame.MiddleColumnLeft.ConvertToHndzElement(frame.MiddleColumnLeft);
                    ms3Frame.MiddleColumnRight.ConvertToHndzElement(frame.MiddleColumnRight);
                    ms3Frame.MiddleColumnMiddle.ConvertToHndzElement(frame.MiddleColumnMiddle);



                    frame.RightSupport = ms3Frame.RightColumn.StartPoint.ConvertToHndzSupport();
                    frame.LeftSupport = ms3Frame.LeftColumn.StartPoint.ConvertToHndzSupport();
                    frame.MiddleSupportLeft = ms3Frame.MiddleColumnLeft.StartPoint.ConvertToHndzSupport();
                    frame.MiddleSupportRight = ms3Frame.MiddleColumnRight.StartPoint.ConvertToHndzSupport();
                    frame.MiddleSupportMiddle = ms3Frame.MiddleColumnMiddle.StartPoint.ConvertToHndzSupport();


                }
            }
            catch (Exception)
            {

            }
           
            #endregion

        }

        #endregion
        #region Private Methods
        private void DesignModel(ref List<SAPFrameElement> frameElements, bool toDesignModel)
        {
            //if (toDesignModel)
            //{
                SAPDesignStatistics statistics = new SAPDesignStatistics();
                SAP2000API.DesignSteelModel(SAPSteelDesignCode.AISC36010, ref statistics, toDesignModel, frameElements);
            //}
        }

        private void AnalyzeModel(string modelName, ref List<SAPFrameElement> frameElements, SAPLoadCombination[] combos, SAPLoadCombination envelope)
        {
            SAP2000API.SaveModel(modelName);
            SAP2000API.AnalayzeModel();
            if (envelope != null)
            {
                SAP2000API.GetFrameElementAnalysisResults(combos, ref frameElements, false);
                SAP2000API.GetFrameElementAnalysisResults(new SAPLoadCombination[] { envelope }, ref frameElements, true);
                SAP2000API.GetJointAnalysisResults(combos, ref frameElements, false);
                SAP2000API.GetJointAnalysisResults(new SAPLoadCombination[] { envelope }, ref frameElements, true);

            }
            // // ===========================
        }

        private void AssignLoads(CustomerInputs inputs)
        {
            SAPDistributedLoad CoverLoad = new SAPDistributedLoad("Cover", Cover, 0, 1, 0.01 * inputs.BaySpacing, 0.01 * inputs.BaySpacing, HndzLoadDirectionEnum.GravityProjected);
            SAPDistributedLoad LiveLoad = new SAPDistributedLoad("Live", Live, 0, 1, 0.057 * inputs.BaySpacing, 0.057 * inputs.BaySpacing, HndzLoadDirectionEnum.GravityProjected);
            #region WindLoad
            int windSpeed;
            switch (inputs.Location)
            {
                case HndzLocationEnum.Cairo:
                    windSpeed = 130;
                    break;
                case HndzLocationEnum.Alexandria:
                    windSpeed = 90;
                    break;
                case HndzLocationEnum.Matrouh:
                    windSpeed = 130;
                    break;
                case HndzLocationEnum.Aswan:
                    windSpeed = 130;
                    break;
                case HndzLocationEnum.Sinai:
                    windSpeed = 130;
                    break;
                default:
                    windSpeed = 130;
                    break;
            }
            double WL_1, WL_2, WL_3, WL_4, WL_1N, WL_2N, WL_3N, WL_4N, WL_1_B, WL_2_B, WL_1_BN, WL_2_BN;
            ASCE107Wind.LoadParameters(windSpeed, (RiskCategory)inputs.RiskCategory, (ExposureCategory)inputs.ExposureCategory, (float)inputs.RidgeHeight / 1000, (float)inputs.EaveHeight, (float)inputs.Length, (float)inputs.Width, RoofType.Gable, 1, 0.85f, true, false); //TODO: Make it not hard coded
                                                                                                                                                                                                                                                                               //Check_1 = txt_Check_1.Text;
                                                                                                                                                                                                                                                                               //Check_2 = txt_Check_2.Text;
            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 1", out WL_1);
            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 2", out WL_2);
            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 3", out WL_3);
            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 4", out WL_4);

            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 1", out WL_1N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 2", out WL_2N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 3", out WL_3N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 4", out WL_4N);

            ASCE107Wind.netPositivePressureB.TryGetValue("Roof Zone 2", out WL_1_B);
            ASCE107Wind.netPositivePressureB.TryGetValue("Wall Zone 5", out WL_2_B);

            ASCE107Wind.netNegativePressureB.TryGetValue("Roof Zone 2", out WL_1_BN);
            ASCE107Wind.netNegativePressureB.TryGetValue("Wall Zone 5", out WL_2_BN);

            //TODO: Temp conversion from kn to Ton .... till u implement units conversion
            WL_1 = WL_1 / 10;
            WL_2 = WL_2 / 10;
            WL_3 = WL_3 / 10;
            WL_4 = WL_4 / 10;

            WL_1N = WL_1N / 10;
            WL_2N = WL_2N / 10;
            WL_3N = WL_3N / 10;
            WL_4N = WL_4N / 10;

            WL_1_B = WL_1_B / 10;
            WL_2_B = WL_2_B / 10;

            WL_1_BN = WL_1_BN / 10;
            WL_2_BN = WL_2_BN / 10;

            //SAPDistributedLoad windLoadSuction = new SAPDistributedLoad("WindLeft", WindLeft, 0, 1, -15, -15, HndzLoadDirectionEnum.Local3axis);
            // SAPDistributedLoad windLoadCompression = new SAPDistributedLoad("WindRight", WindRight, 0, 1, -17, -17, HndzLoadDirectionEnum.Local3axis);


            //Wind Load Case: WLB
            SAPDistributedLoad WLBLColumn = new SAPDistributedLoad("WLBLColumn", WLB, 0, 1, WL_1 * inputs.BaySpacing, WL_1 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
            SAPDistributedLoad WLBRColumn = new SAPDistributedLoad("WLBRColumn", WLB, 0, 1, -WL_4 * inputs.BaySpacing, -WL_4 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

            SAPDistributedLoad WLBLBeam = new SAPDistributedLoad("WLBLBeam", WLB, 0, 1, -WL_2 * inputs.BaySpacing, -WL_2 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
            SAPDistributedLoad WLBRBeam = new SAPDistributedLoad("WLBRBeam", WLB, 0, 1, -WL_3 * inputs.BaySpacing, -WL_3 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

            //Wind Load Case: WLU
            SAPDistributedLoad WLULColumn = new SAPDistributedLoad("WLULColumn", WLU, 0, 1, -WL_2N * inputs.BaySpacing, -WL_2N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
            SAPDistributedLoad WLURColumn = new SAPDistributedLoad("WLURColumn", WLU, 0, 1, -WL_3N * inputs.BaySpacing, -WL_3N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

            SAPDistributedLoad WLULBeam = new SAPDistributedLoad("WLULBeam", WLU, 0, 1, WL_1N * inputs.BaySpacing, WL_1N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
            SAPDistributedLoad WLURBeam = new SAPDistributedLoad("WLURBeam", WLU, 0, 1, -WL_4N * inputs.BaySpacing, -WL_4N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

            //Wind Load Case: WRB
            SAPDistributedLoad WRBLColumn = new SAPDistributedLoad("WRBLColumn", WRB, 0, 1, WL_4 * inputs.BaySpacing, WL_4 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
            SAPDistributedLoad WRBRColumn = new SAPDistributedLoad("WRBRColumn", WRB, 0, 1, -WL_1 * inputs.BaySpacing, -WL_1 * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

            SAPDistributedLoad WRBLBeam = new SAPDistributedLoad("WRBLBeam", WRB, 0, 1, -WL_3 * inputs.BaySpacing, -WL_3 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
            SAPDistributedLoad WRBRBeam = new SAPDistributedLoad("WRBRBeam", WRB, 0, 1, -WL_2 * inputs.BaySpacing, -WL_2 * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

            //Wind Load Case: WRU
            SAPDistributedLoad WRULColumn = new SAPDistributedLoad("WRULColumn", WRU, 0, 1, WL_4N * inputs.BaySpacing, WL_4N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
            SAPDistributedLoad WRURColumn = new SAPDistributedLoad("WRURColumn", WRU, 0, 1, -WL_1N * inputs.BaySpacing, -WL_1N * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

            SAPDistributedLoad WRULBeam = new SAPDistributedLoad("WRULBeam", WRU, 0, 1, -WL_3N * inputs.BaySpacing, -WL_3N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
            SAPDistributedLoad WRURBeam = new SAPDistributedLoad("WRURBeam", WRU, 0, 1, -WL_2N * inputs.BaySpacing, -WL_2N * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

            //Wind Load Case: WEB
            SAPDistributedLoad WEBLColumn = new SAPDistributedLoad("WEBLColumn", WEB, 0, 1, WL_2_B * inputs.BaySpacing, WL_2_B * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
            SAPDistributedLoad WEBRColumn = new SAPDistributedLoad("WEBRColumn", WEB, 0, 1, -WL_2_B * inputs.BaySpacing, -WL_2_B * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

            SAPDistributedLoad WEBLBeam = new SAPDistributedLoad("WEBLBeam", WEB, 0, 1, -WL_1_B * inputs.BaySpacing, -WL_1_B * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
            SAPDistributedLoad WEBRBeam = new SAPDistributedLoad("WEBRBeam", WEB, 0, 1, -WL_1_B * inputs.BaySpacing, -WL_1_B * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

            //Wind Load Case: WEU 
            SAPDistributedLoad WEULColumn = new SAPDistributedLoad("WEULColumn", WEU, 0, 1, WL_2_BN * inputs.BaySpacing, WL_2_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);
            SAPDistributedLoad WEURColumn = new SAPDistributedLoad("WEURColumn", WEU, 0, 1, -WL_2_BN * inputs.BaySpacing, -WL_2_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Xdirection);

            SAPDistributedLoad WEULBeam = new SAPDistributedLoad("WEULBeam", WEU, 0, 1, WL_1_BN * inputs.BaySpacing, WL_1_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);
            SAPDistributedLoad WEURBeam = new SAPDistributedLoad("WEURBeam", WEU, 0, 1, WL_1_BN * inputs.BaySpacing, WL_1_BN * inputs.BaySpacing, HndzLoadDirectionEnum.Local2axis);

            #endregion
            if (csFrame != null)
            {
                AddClearSpanLoadToSAP(CoverLoad, LiveLoad, WLBLColumn, WLBRColumn, WLBLBeam, WLBRBeam, WLULColumn, WLURColumn, WLULBeam, WLURBeam, WRBLColumn, WRBRColumn, WRBLBeam, WRBRBeam, WRULColumn, WRURColumn, WRULBeam, WRURBeam, WEBLColumn, WEBRColumn, WEBLBeam, WEBRBeam, WEULColumn, WEURColumn, WEULBeam, WEURBeam);
            }
            else if (msFrame != null)
            {
                AddMonoSlopeLoadsToSAP(CoverLoad, LiveLoad, WLBLColumn, WLBRColumn, WLBLBeam, WLBRBeam, WLULColumn, WLURColumn, WLULBeam, WLURBeam, WRBLColumn, WRBRColumn, WRBLBeam, WRBRBeam, WRULColumn, WRURColumn, WRULBeam, WRURBeam, WEBLColumn, WEBRColumn, WEBLBeam, WEBRBeam, WEULColumn, WEURColumn, WEULBeam, WEURBeam);
            }
            else if (mgFrame != null)
            {
                AddMultiGableLoadsToSAP(CoverLoad, LiveLoad, WLBLColumn, WLBRColumn, WLBLBeam, WLBRBeam, WLULColumn, WLURColumn, WLULBeam, WLURBeam, WRBLColumn, WRBRColumn, WRBLBeam, WRBRBeam, WRULColumn, WRURColumn, WRULBeam, WRURBeam, WEBLColumn, WEBRColumn, WEBLBeam, WEBRBeam, WEULColumn, WEURColumn, WEULBeam, WEURBeam);
            }
            else if (ms1Frame != null)
            {
                AddMultiSpan1LoadsToSAP(CoverLoad, LiveLoad, WLBLColumn, WLBRColumn, WLBLBeam, WLBRBeam, WLULColumn, WLURColumn, WLULBeam, WLURBeam, WRBLColumn, WRBRColumn, WRBLBeam, WRBRBeam, WRULColumn, WRURColumn, WRULBeam, WRURBeam, WEBLColumn, WEBRColumn, WEBLBeam, WEBRBeam, WEULColumn, WEURColumn, WEULBeam, WEURBeam);
            }
            else if (ms2Frame != null)
            {
                AddMultiSpan2LoadsToSAP(CoverLoad, LiveLoad, WLBLColumn, WLBRColumn, WLBLBeam, WLBRBeam, WLULColumn, WLURColumn, WLULBeam, WLURBeam, WRBLColumn, WRBRColumn, WRBLBeam, WRBRBeam, WRULColumn, WRURColumn, WRULBeam, WRURBeam, WEBLColumn, WEBRColumn, WEBLBeam, WEBRBeam, WEULColumn, WEURColumn, WEULBeam, WEURBeam);
            }
            else if (ms3Frame != null)
            {
                AddMultiSpan3LoadsToSAP(CoverLoad, LiveLoad, WLBLColumn, WLBRColumn, WLBLBeam, WLBRBeam, WLULColumn, WLURColumn, WLULBeam, WLURBeam, WRBLColumn, WRBRColumn, WRBLBeam, WRBRBeam, WRULColumn, WRURColumn, WRULBeam, WRURBeam, WEBLColumn, WEBRColumn, WEBLBeam, WEBRBeam, WEULColumn, WEURColumn, WEULBeam, WEURBeam);
            }
        }
        private void AddMultiSpan3LoadsToSAP(SAPDistributedLoad CoverLoad, SAPDistributedLoad LiveLoad, SAPDistributedLoad WLBLColumn, SAPDistributedLoad WLBRColumn, SAPDistributedLoad WLBLBeam, SAPDistributedLoad WLBRBeam, SAPDistributedLoad WLULColumn, SAPDistributedLoad WLURColumn, SAPDistributedLoad WLULBeam, SAPDistributedLoad WLURBeam, SAPDistributedLoad WRBLColumn, SAPDistributedLoad WRBRColumn, SAPDistributedLoad WRBLBeam, SAPDistributedLoad WRBRBeam, SAPDistributedLoad WRULColumn, SAPDistributedLoad WRURColumn, SAPDistributedLoad WRULBeam, SAPDistributedLoad WRURBeam, SAPDistributedLoad WEBLColumn, SAPDistributedLoad WEBRColumn, SAPDistributedLoad WEBLBeam, SAPDistributedLoad WEBRBeam, SAPDistributedLoad WEULColumn, SAPDistributedLoad WEURColumn, SAPDistributedLoad WEULBeam, SAPDistributedLoad WEURBeam)
        {
            //=========================
            //TODO let this function take an array, make an overloaded version
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, LiveLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, CoverLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, WLBLBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, WLULBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, WRBLBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, WRULBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, WEBLBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamLeft, WEULBeam);

            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, LiveLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, CoverLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, WLBLBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, WLULBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, WRBLBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, WRULBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, WEBLBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftBeamRight, WEULBeam);

            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, LiveLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, CoverLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, WLBRBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, WLURBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, WRBRBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, WRURBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, WEBRBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamRight, WEURBeam);


            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, LiveLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, CoverLoad);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, WLBRBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, WLURBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, WRBRBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, WRURBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, WEBRBeam);
            SAP2000API.AddDistributedLoad(ms3Frame.RightBeamLeft, WEURBeam);

            //SAP2000API.AddDistributedLoad(LeftCols, windLoadCompression);
            //SAP2000API.AddDistributedLoad(RightCols, windLoadSuction);
            //=================
            SAP2000API.AddDistributedLoad(ms3Frame.LeftColumn, WLBLColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftColumn, WLULColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftColumn, WRBLColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftColumn, WRULColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftColumn, WEBLColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.LeftColumn, WEULColumn);

            SAP2000API.AddDistributedLoad(ms3Frame.RightColumn, WLBRColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.RightColumn, WLURColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.RightColumn, WRBRColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.RightColumn, WRURColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.RightColumn, WEBRColumn);
            SAP2000API.AddDistributedLoad(ms3Frame.RightColumn, WEURColumn);

            // ==========================
        }

        private void AddMultiSpan2LoadsToSAP(SAPDistributedLoad CoverLoad, SAPDistributedLoad LiveLoad, SAPDistributedLoad WLBLColumn, SAPDistributedLoad WLBRColumn, SAPDistributedLoad WLBLBeam, SAPDistributedLoad WLBRBeam, SAPDistributedLoad WLULColumn, SAPDistributedLoad WLURColumn, SAPDistributedLoad WLULBeam, SAPDistributedLoad WLURBeam, SAPDistributedLoad WRBLColumn, SAPDistributedLoad WRBRColumn, SAPDistributedLoad WRBLBeam, SAPDistributedLoad WRBRBeam, SAPDistributedLoad WRULColumn, SAPDistributedLoad WRURColumn, SAPDistributedLoad WRULBeam, SAPDistributedLoad WRURBeam, SAPDistributedLoad WEBLColumn, SAPDistributedLoad WEBRColumn, SAPDistributedLoad WEBLBeam, SAPDistributedLoad WEBRBeam, SAPDistributedLoad WEULColumn, SAPDistributedLoad WEURColumn, SAPDistributedLoad WEULBeam, SAPDistributedLoad WEURBeam)
        {
            //=========================
            //TODO let this function take an array, make an overloaded version
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, LiveLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, CoverLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, WLBLBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, WLULBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, WRBLBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, WRULBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, WEBLBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftBeam, WEULBeam);

            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, LiveLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, CoverLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, WLBLBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, WLULBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, WRBLBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, WRULBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, WEBLBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamLeft, WEULBeam);

            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, LiveLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, CoverLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, WLBRBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, WLURBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, WRBRBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, WRURBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, WEBRBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.RightBeam, WEURBeam);

            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, LiveLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, CoverLoad);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, WLBRBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, WLURBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, WRBRBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, WRURBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, WEBRBeam);
            SAP2000API.AddDistributedLoad(ms2Frame.MiddleBeamRight, WEURBeam);

            //SAP2000API.AddDistributedLoad(LeftCols, windLoadCompression);
            //SAP2000API.AddDistributedLoad(RightCols, windLoadSuction);
            //=================
            SAP2000API.AddDistributedLoad(ms2Frame.LeftColumn, WLBLColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftColumn, WLULColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftColumn, WRBLColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftColumn, WRULColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftColumn, WEBLColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.LeftColumn, WEULColumn);

            SAP2000API.AddDistributedLoad(ms2Frame.RightColumn, WLBRColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.RightColumn, WLURColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.RightColumn, WRBRColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.RightColumn, WRURColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.RightColumn, WEBRColumn);
            SAP2000API.AddDistributedLoad(ms2Frame.RightColumn, WEURColumn);

            // ==========================
        }

        private void AddMultiSpan1LoadsToSAP(SAPDistributedLoad CoverLoad, SAPDistributedLoad LiveLoad, SAPDistributedLoad WLBLColumn, SAPDistributedLoad WLBRColumn, SAPDistributedLoad WLBLBeam, SAPDistributedLoad WLBRBeam, SAPDistributedLoad WLULColumn, SAPDistributedLoad WLURColumn, SAPDistributedLoad WLULBeam, SAPDistributedLoad WLURBeam, SAPDistributedLoad WRBLColumn, SAPDistributedLoad WRBRColumn, SAPDistributedLoad WRBLBeam, SAPDistributedLoad WRBRBeam, SAPDistributedLoad WRULColumn, SAPDistributedLoad WRURColumn, SAPDistributedLoad WRULBeam, SAPDistributedLoad WRURBeam, SAPDistributedLoad WEBLColumn, SAPDistributedLoad WEBRColumn, SAPDistributedLoad WEBLBeam, SAPDistributedLoad WEBRBeam, SAPDistributedLoad WEULColumn, SAPDistributedLoad WEURColumn, SAPDistributedLoad WEULBeam, SAPDistributedLoad WEURBeam)
        {
            //=========================
            //TODO let this function take an array, make an overloaded version
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, LiveLoad);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, CoverLoad);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, WLBLBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, WLULBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, WRBLBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, WRULBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, WEBLBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftBeam, WEULBeam);

            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, LiveLoad);
            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, CoverLoad);
            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, WLBRBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, WLURBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, WRBRBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, WRURBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, WEBRBeam);
            SAP2000API.AddDistributedLoad(ms1Frame.RightBeam, WEURBeam);

            //SAP2000API.AddDistributedLoad(LeftCols, windLoadCompression);
            //SAP2000API.AddDistributedLoad(RightCols, windLoadSuction);
            //=================
            SAP2000API.AddDistributedLoad(ms1Frame.LeftColumn, WLBLColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftColumn, WLULColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftColumn, WRBLColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftColumn, WRULColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftColumn, WEBLColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.LeftColumn, WEULColumn);

            SAP2000API.AddDistributedLoad(ms1Frame.RightColumn, WLBRColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.RightColumn, WLURColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.RightColumn, WRBRColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.RightColumn, WRURColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.RightColumn, WEBRColumn);
            SAP2000API.AddDistributedLoad(ms1Frame.RightColumn, WEURColumn);

            // ==========================
        }

        private void AddMultiGableLoadsToSAP(SAPDistributedLoad CoverLoad, SAPDistributedLoad LiveLoad, SAPDistributedLoad WLBLColumn, SAPDistributedLoad WLBRColumn, SAPDistributedLoad WLBLBeam, SAPDistributedLoad WLBRBeam, SAPDistributedLoad WLULColumn, SAPDistributedLoad WLURColumn, SAPDistributedLoad WLULBeam, SAPDistributedLoad WLURBeam, SAPDistributedLoad WRBLColumn, SAPDistributedLoad WRBRColumn, SAPDistributedLoad WRBLBeam, SAPDistributedLoad WRBRBeam, SAPDistributedLoad WRULColumn, SAPDistributedLoad WRURColumn, SAPDistributedLoad WRULBeam, SAPDistributedLoad WRURBeam, SAPDistributedLoad WEBLColumn, SAPDistributedLoad WEBRColumn, SAPDistributedLoad WEBLBeam, SAPDistributedLoad WEBRBeam, SAPDistributedLoad WEULColumn, SAPDistributedLoad WEURColumn, SAPDistributedLoad WEULBeam, SAPDistributedLoad WEURBeam)
        {
            //=========================
            //TODO let this function take an array, make an overloaded version
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, LiveLoad);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, CoverLoad);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, WLBLBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, WLULBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, WRBLBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, WRULBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, WEBLBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamLeft, WEULBeam);

            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, LiveLoad);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, CoverLoad);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, WLBLBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, WLULBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, WRBLBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, WRULBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, WEBLBeam);
            SAP2000API.AddDistributedLoad(mgFrame.LeftBeamRight, WEULBeam);

            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, LiveLoad);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, CoverLoad);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, WLBRBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, WLURBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, WRBRBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, WRURBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, WEBRBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamRight, WEURBeam);

            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, LiveLoad);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, CoverLoad);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, WLBRBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, WLURBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, WRBRBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, WRURBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, WEBRBeam);
            SAP2000API.AddDistributedLoad(mgFrame.RightBeamLeft, WEURBeam);

            //SAP2000API.AddDistributedLoad(LeftCols, windLoadCompression);
            //SAP2000API.AddDistributedLoad(RightCols, windLoadSuction);
            //=================
            SAP2000API.AddDistributedLoad(mgFrame.LeftColumn, WLBLColumn);
            SAP2000API.AddDistributedLoad(mgFrame.LeftColumn, WLULColumn);
            SAP2000API.AddDistributedLoad(mgFrame.LeftColumn, WRBLColumn);
            SAP2000API.AddDistributedLoad(mgFrame.LeftColumn, WRULColumn);
            SAP2000API.AddDistributedLoad(mgFrame.LeftColumn, WEBLColumn);
            SAP2000API.AddDistributedLoad(mgFrame.LeftColumn, WEULColumn);

            SAP2000API.AddDistributedLoad(mgFrame.RightColumn, WLBRColumn);
            SAP2000API.AddDistributedLoad(mgFrame.RightColumn, WLURColumn);
            SAP2000API.AddDistributedLoad(mgFrame.RightColumn, WRBRColumn);
            SAP2000API.AddDistributedLoad(mgFrame.RightColumn, WRURColumn);
            SAP2000API.AddDistributedLoad(mgFrame.RightColumn, WEBRColumn);
            SAP2000API.AddDistributedLoad(mgFrame.RightColumn, WEURColumn);

            // ==========================
        }

        private void AddMonoSlopeLoadsToSAP(SAPDistributedLoad CoverLoad, SAPDistributedLoad LiveLoad, SAPDistributedLoad WLBLColumn, SAPDistributedLoad WLBRColumn, SAPDistributedLoad WLBLBeam, SAPDistributedLoad WLBRBeam, SAPDistributedLoad WLULColumn, SAPDistributedLoad WLURColumn, SAPDistributedLoad WLULBeam, SAPDistributedLoad WLURBeam, SAPDistributedLoad WRBLColumn, SAPDistributedLoad WRBRColumn, SAPDistributedLoad WRBLBeam, SAPDistributedLoad WRBRBeam, SAPDistributedLoad WRULColumn, SAPDistributedLoad WRURColumn, SAPDistributedLoad WRULBeam, SAPDistributedLoad WRURBeam, SAPDistributedLoad WEBLColumn, SAPDistributedLoad WEBRColumn, SAPDistributedLoad WEBLBeam, SAPDistributedLoad WEBRBeam, SAPDistributedLoad WEULColumn, SAPDistributedLoad WEURColumn, SAPDistributedLoad WEULBeam, SAPDistributedLoad WEURBeam)
        {
            //=========================
            //TODO let this function take an array, make an overloaded version
            SAP2000API.AddDistributedLoad(msFrame.Beam, LiveLoad);
            SAP2000API.AddDistributedLoad(msFrame.Beam, CoverLoad);
            SAP2000API.AddDistributedLoad(msFrame.Beam, WLBLBeam);
            SAP2000API.AddDistributedLoad(msFrame.Beam, WLULBeam);
            SAP2000API.AddDistributedLoad(msFrame.Beam, WRBLBeam);
            SAP2000API.AddDistributedLoad(msFrame.Beam, WRULBeam);
            SAP2000API.AddDistributedLoad(msFrame.Beam, WEBLBeam);
            SAP2000API.AddDistributedLoad(msFrame.Beam, WEULBeam);


            //SAP2000API.AddDistributedLoad(LeftCols, windLoadCompression);
            //SAP2000API.AddDistributedLoad(RightCols, windLoadSuction);
            //=================
            SAP2000API.AddDistributedLoad(msFrame.LeftColumn, WLBLColumn);
            SAP2000API.AddDistributedLoad(msFrame.LeftColumn, WLULColumn);
            SAP2000API.AddDistributedLoad(msFrame.LeftColumn, WRBLColumn);
            SAP2000API.AddDistributedLoad(msFrame.LeftColumn, WRULColumn);
            SAP2000API.AddDistributedLoad(msFrame.LeftColumn, WEBLColumn);
            SAP2000API.AddDistributedLoad(msFrame.LeftColumn, WEULColumn);

            SAP2000API.AddDistributedLoad(msFrame.RightColumn, WLBRColumn);
            SAP2000API.AddDistributedLoad(msFrame.RightColumn, WLURColumn);
            SAP2000API.AddDistributedLoad(msFrame.RightColumn, WRBRColumn);
            SAP2000API.AddDistributedLoad(msFrame.RightColumn, WRURColumn);
            SAP2000API.AddDistributedLoad(msFrame.RightColumn, WEBRColumn);
            SAP2000API.AddDistributedLoad(msFrame.RightColumn, WEURColumn);
        }

        private void AddClearSpanLoadToSAP(SAPDistributedLoad CoverLoad, SAPDistributedLoad LiveLoad, SAPDistributedLoad WLBLColumn, SAPDistributedLoad WLBRColumn, SAPDistributedLoad WLBLBeam, SAPDistributedLoad WLBRBeam, SAPDistributedLoad WLULColumn, SAPDistributedLoad WLURColumn, SAPDistributedLoad WLULBeam, SAPDistributedLoad WLURBeam, SAPDistributedLoad WRBLColumn, SAPDistributedLoad WRBRColumn, SAPDistributedLoad WRBLBeam, SAPDistributedLoad WRBRBeam, SAPDistributedLoad WRULColumn, SAPDistributedLoad WRURColumn, SAPDistributedLoad WRULBeam, SAPDistributedLoad WRURBeam, SAPDistributedLoad WEBLColumn, SAPDistributedLoad WEBRColumn, SAPDistributedLoad WEBLBeam, SAPDistributedLoad WEBRBeam, SAPDistributedLoad WEULColumn, SAPDistributedLoad WEURColumn, SAPDistributedLoad WEULBeam, SAPDistributedLoad WEURBeam)
        {
            //=========================
            //TODO let this function take an array, make an overloaded version
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, LiveLoad);
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, CoverLoad);
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, WLBLBeam);
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, WLULBeam);
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, WRBLBeam);
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, WRULBeam);
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, WEBLBeam);
            SAP2000API.AddDistributedLoad(csFrame.LeftBeam, WEULBeam);



            SAP2000API.AddDistributedLoad(csFrame.RightBeam, LiveLoad);
            SAP2000API.AddDistributedLoad(csFrame.RightBeam, CoverLoad);
            SAP2000API.AddDistributedLoad(csFrame.RightBeam, WLBRBeam);
            SAP2000API.AddDistributedLoad(csFrame.RightBeam, WLURBeam);
            SAP2000API.AddDistributedLoad(csFrame.RightBeam, WRBRBeam);
            SAP2000API.AddDistributedLoad(csFrame.RightBeam, WRURBeam);
            SAP2000API.AddDistributedLoad(csFrame.RightBeam, WEBRBeam);
            SAP2000API.AddDistributedLoad(csFrame.RightBeam, WEURBeam);


            //SAP2000API.AddDistributedLoad(LeftCols, windLoadCompression);
            //SAP2000API.AddDistributedLoad(RightCols, windLoadSuction);
            //=================
            SAP2000API.AddDistributedLoad(csFrame.LeftColumn, WLBLColumn);
            SAP2000API.AddDistributedLoad(csFrame.LeftColumn, WLULColumn);
            SAP2000API.AddDistributedLoad(csFrame.LeftColumn, WRBLColumn);
            SAP2000API.AddDistributedLoad(csFrame.LeftColumn, WRULColumn);
            SAP2000API.AddDistributedLoad(csFrame.LeftColumn, WEBLColumn);
            SAP2000API.AddDistributedLoad(csFrame.LeftColumn, WEULColumn);

            SAP2000API.AddDistributedLoad(csFrame.RightColumn, WLBRColumn);
            SAP2000API.AddDistributedLoad(csFrame.RightColumn, WLURColumn);
            SAP2000API.AddDistributedLoad(csFrame.RightColumn, WRBRColumn);
            SAP2000API.AddDistributedLoad(csFrame.RightColumn, WRURColumn);
            SAP2000API.AddDistributedLoad(csFrame.RightColumn, WEBRColumn);
            SAP2000API.AddDistributedLoad(csFrame.RightColumn, WEURColumn);

            // ==========================
        }

        private SAPLoadCombination[] DefineLoadDefinitions()
        {
            SAPLoadCombination[] combos = new SAPLoadCombination[14];
            //Defining Load Patterns
            Dead = new SAPLoadPattern("Dead", eLoadPatternType.Dead, 1, true);
            Cover = new SAPLoadPattern("Cover", eLoadPatternType.Dead, 0, true);
            Live = new SAPLoadPattern("Live", eLoadPatternType.Live, 0, true);
            WLB = new SAPLoadPattern("WLB", eLoadPatternType.Wind, 0, true);
            WLU = new SAPLoadPattern("WLU", eLoadPatternType.Wind, 0, true);
            WRB = new SAPLoadPattern("WRB", eLoadPatternType.Wind, 0, true);
            WRU = new SAPLoadPattern("WRU", eLoadPatternType.Wind, 0, true);
            WEB = new SAPLoadPattern("WEB", eLoadPatternType.Wind, 0, true);
            WEU = new SAPLoadPattern("WEU", eLoadPatternType.Wind, 0, true);

            // WindLeft = new SAPLoadPattern("WindLeft", eLoadPatternType.Wind, 0, true);
            // WindRight = new SAPLoadPattern("WindRight", eLoadPatternType.Wind, 0, true);
            //TODO: Temporary ECP Wind Load Cases

            //Adding Load Patterns to SAP2000
            SAP2000API.AddLoadPattern(Dead);
            SAP2000API.AddLoadPattern(Cover);
            SAP2000API.AddLoadPattern(Live);

            // SAP2000API.AddLoadPattern(WindRight);
            // SAP2000API.AddLoadPattern(WindLeft);

            SAP2000API.AddLoadPattern(WLB);
            SAP2000API.AddLoadPattern(WLU);
            SAP2000API.AddLoadPattern(WRB);
            SAP2000API.AddLoadPattern(WRU);
            SAP2000API.AddLoadPattern(WEB);
            SAP2000API.AddLoadPattern(WEU);

            //Defining Load Combos:
            //=====================================
            combos = SAP2000API.AddDesignDefaultCombos(eMatType.Steel);
            // ====================================
            return combos;
        }

        private SAPMaterial[] DefineMaterials()
        {
            //TODO: Read the advanced inputs to see if u have to change the grade
            return SAP2000API.SetDefaultMaterials();
            //0 St37, 1 St44, 2 St52, 3 RC30 last one is not available yet
        }

        private void InitSAP2000(bool attachToInstance, string modelName)
        {
            SAP2000API.StartApplication(attachToInstance, true, eUnits.Ton_m_C);
            SAP2000API.NewModel(eUnits.Ton_m_C);
            SAP2000API.SaveModel(modelName);
        }
        #endregion
        public static string GetPath(string modelName)
        {
            return SAP2000API.GetPath(modelName);
        }
    }

}



#region OLD GENERIC WONDERFUL AWESOME CODE
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
//    public class SAPAnalysisModel
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

//            double inputs.Width = inputs.inputs.Width;
//            double length = inputs.Length;
//            int noFrames = 1; //inputs.NoFrames;//TODO
//            double BaySpacing = inputs.BaySpacing;
//            double inputs.EaveHeight = inputs.inputs.EaveHeight;
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
//            SAPFrameElement[] LeftCols, RightCols;
//            SAPFrameElement[,] LeftBeams, RightBeams;
//            //Columns
//            LeftCols = new SAPFrameElement[noFrames];
//            RightCols = new SAPFrameElement[noFrames];
//            for (int i = 0; i < noFrames; i++)
//            {
//                LeftCols[i] = new SAPFrameElement("LeftCol" + i);
//                LeftCols[i].StartPoint = new SAPPoint(LeftCols[i].Name + "p1", 0, i * inputs.BaySpacing, 0);
//                LeftCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                LeftCols[i].EndPoint = new SAPPoint(LeftCols[i].Name + "p2", 0, i * inputs.BaySpacing, inputs.EaveHeight);
//                LeftCols[i].Section = column;

//                RightCols[i] = new SAPFrameElement("RightCol" + i);
//                RightCols[i].StartPoint = new SAPPoint(RightCols[i].Name + "p1", inputs.Width, i * inputs.BaySpacing, 0);
//                RightCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                RightCols[i].EndPoint = new SAPPoint(RightCols[i].Name + "p2", inputs.Width, i * inputs.BaySpacing, inputs.EaveHeight);
//                RightCols[i].Section = column;
//            }
//            SAP2000API.AddFrameElement(LeftCols);
//            SAP2000API.AddFrameElement(RightCols);

//            //Beams
//            int noSegments = (int)((inputs.Width / 2) / 6);
//            LeftBeams = new SAPFrameElement[noFrames, noSegments];
//            RightBeams = new SAPFrameElement[noFrames, noSegments];
//            for (int i = 0; i < noFrames; i++)
//            {
//                for (int j = 0; j < noSegments; j++)
//                {
//                    LeftBeams[i, j] = new SAPFrameElement("LeftBeam" + i + "-" + j);
//                    LeftBeams[i, j].StartPoint = new SAPPoint(LeftBeams[i, j].Name + "p1", j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * inputs.Width / 2 + LeftCols[i].EndPoint.Z);
//                    LeftBeams[i, j].EndPoint = new SAPPoint(LeftBeams[i, j].Name + "p2", (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * inputs.Width / 2 + LeftCols[i].EndPoint.Z);
//                    LeftBeams[i, j].Section = middleBeam;

//                    RightBeams[i, j] = new SAPFrameElement("RightBeam" + i + "-" + j);
//                    RightBeams[i, j].StartPoint = new SAPPoint(RightBeams[i, j].Name + "p2", inputs.Width - j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * inputs.Width / 2 + RightCols[i].EndPoint.Z);
//                    RightBeams[i, j].EndPoint = new SAPPoint(RightBeams[i, j].Name + "p1", inputs.Width - (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * inputs.Width / 2 + RightCols[i].EndPoint.Z);
//                    RightBeams[i, j].Section = middleBeam;
//                }
//                //Modifications for exceptions
//                LeftBeams[i, 0].Section = taperedBeam; //Tapered edge section
//                RightBeams[i, 0].Section = taperedBeam; //Tapered edge section

//                LeftBeams[i, noSegments - 1].StartPoint = new SAPPoint(LeftBeams[i, noSegments - 1].Name + "p1", (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * roofSlope * inputs.Width / 2 + LeftCols[i].EndPoint.Z);
//                LeftBeams[i, noSegments - 1].EndPoint = new SAPPoint(LeftBeams[i, noSegments - 1].Name + "p2", inputs.Width / 2, i * inputs.BaySpacing, (double)inputs.Width / 2 * roofSlope + LeftCols[i].EndPoint.Z);

//                RightBeams[i, noSegments - 1].EndPoint = new SAPPoint(RightBeams[i, noSegments - 1].Name + "p1", inputs.Width - (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * inputs.Width / 2 * roofSlope + RightCols[i].EndPoint.Z);
//                RightBeams[i, noSegments - 1].StartPoint = LeftBeams[i, noSegments - 1].EndPoint;
//            }
//            foreach (SAPFrameElement beam in LeftBeams)
//            {
//                SAP2000API.AddFrameElement(beam);
//            }
//            foreach (SAPFrameElement beam in RightBeams)
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
//            AssignLoads(inputs, LeftCols, RightCols, LeftBeams, RightBeams);
//            #endregion
//            #region Analysis
//            AnalyzeModel(modelName, RightCols, combos, envelope);

//            #endregion
//            #region Design
//            DesignModel(LeftCols, RightCols);
//            #endregion

//            foreach (SAPFrameElement beam in Beams)
//            {

//            }

//        }
//        public void GenerateMultiGableFrame(CustomerInputs inputs, string modelName, bool attachToInstance = true)
//        {

//            #region Extracted Inputs

//            float inputs.Width = inputs.Landinputs.Width;
//            float length = inputs.LandLength;
//            int noFrames = 1; //inputs.NoFrames;//TODO
//            float BaySpacing = inputs.BaySpacing;
//            float inputs.EaveHeight = inputs.inputs.EaveHeight;
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
//            SAPFrameElement[] LeftCols, RightCols;
//            SAPFrameElement[,] LeftBeams, RightBeams;
//            //Columns
//            LeftCols = new SAPFrameElement[noFrames];
//            RightCols = new SAPFrameElement[noFrames];
//            for (int i = 0; i < noFrames; i++)
//            {
//                LeftCols[i] = new SAPFrameElement("LeftCol" + i);
//                LeftCols[i].StartPoint = new SAPPoint(LeftCols[i].Name + "p1", 0, i * inputs.BaySpacing, 0);
//                LeftCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                LeftCols[i].EndPoint = new SAPPoint(LeftCols[i].Name + "p2", 0, i * inputs.BaySpacing, inputs.EaveHeight);
//                LeftCols[i].Section = column;

//                RightCols[i] = new SAPFrameElement("RightCol" + i);
//                RightCols[i].StartPoint = new SAPPoint(RightCols[i].Name + "p1", inputs.Width, i * inputs.BaySpacing, 0);
//                RightCols[i].StartPoint.Restraint = new SAPRestraint(SAPRestraintEnum.Pinned);
//                RightCols[i].EndPoint = new SAPPoint(RightCols[i].Name + "p2", inputs.Width, i * inputs.BaySpacing, inputs.EaveHeight);
//                RightCols[i].Section = column;
//            }
//            SAP2000API.AddFrameElement(LeftCols);
//            SAP2000API.AddFrameElement(RightCols);

//            //Beams
//            int noSegments = (int)((inputs.Width / 2) / 6);
//            LeftBeams = new SAPFrameElement[noFrames, noSegments];
//            RightBeams = new SAPFrameElement[noFrames, noSegments];
//            for (int i = 0; i < noFrames; i++)
//            {
//                for (int j = 0; j < noSegments; j++)
//                {
//                    LeftBeams[i, j] = new SAPFrameElement("LeftBeam" + i + "-" + j);
//                    LeftBeams[i, j].StartPoint = new SAPPoint(LeftBeams[i, j].Name + "p1", j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * inputs.Width / 2 + LeftCols[i].EndPoint.Z);
//                    LeftBeams[i, j].EndPoint = new SAPPoint(LeftBeams[i, j].Name + "p2", (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * inputs.Width / 2 + LeftCols[i].EndPoint.Z);
//                    LeftBeams[i, j].Section = middleBeam;

//                    RightBeams[i, j] = new SAPFrameElement("RightBeam" + i + "-" + j);
//                    RightBeams[i, j].StartPoint = new SAPPoint(RightBeams[i, j].Name + "p2", inputs.Width - j * 6, i * inputs.BaySpacing, (double)j / noSegments * roofSlope * inputs.Width / 2 + RightCols[i].EndPoint.Z);
//                    RightBeams[i, j].EndPoint = new SAPPoint(RightBeams[i, j].Name + "p1", inputs.Width - (1 + j) * 6, i * inputs.BaySpacing, (double)(1 + j) / noSegments * roofSlope * inputs.Width / 2 + RightCols[i].EndPoint.Z);
//                    RightBeams[i, j].Section = middleBeam;
//                }
//                //Modifications for exceptions
//                LeftBeams[i, 0].Section = taperedBeam; //Tapered edge section
//                RightBeams[i, 0].Section = taperedBeam; //Tapered edge section

//                LeftBeams[i, noSegments - 1].StartPoint = new SAPPoint(LeftBeams[i, noSegments - 1].Name + "p1", (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * roofSlope * inputs.Width / 2 + LeftCols[i].EndPoint.Z);
//                LeftBeams[i, noSegments - 1].EndPoint = new SAPPoint(LeftBeams[i, noSegments - 1].Name + "p2", inputs.Width / 2, i * inputs.BaySpacing, (double)inputs.Width / 2 * roofSlope + LeftCols[i].EndPoint.Z);

//                RightBeams[i, noSegments - 1].EndPoint = new SAPPoint(RightBeams[i, noSegments - 1].Name + "p1", inputs.Width - (noSegments - 1) * 6, i * inputs.BaySpacing, (double)(noSegments - 1) / noSegments * inputs.Width / 2 * roofSlope + RightCols[i].EndPoint.Z);
//                RightBeams[i, noSegments - 1].StartPoint = LeftBeams[i, noSegments - 1].EndPoint;
//            }
//            foreach (SAPFrameElement beam in LeftBeams)
//            {
//                SAP2000API.AddFrameElement(beam);
//            }
//            foreach (SAPFrameElement beam in RightBeams)
//            {
//                SAP2000API.AddFrameElement(beam);
//            }
//            #endregion
//        }
//        #endregion
//        #region Private Methods
//        private void DesignModel(SAPFrameElement[] LeftCols, SAPFrameElement[] RightCols)
//        {
//            SAPDesignStatistics statistics = new SAPDesignStatistics();
//            SAP2000API.DesignSteelModel(SAPSteelDesignCode.AISC36010, ref statistics, Beams, Columns);
//        }

//        private void AnalyzeModel(string modelName, SAPFrameElement[] RightCols, SAPLoadCombination[] combos, SAPLoadCombination envelope)
//        {
//            SAP2000API.SaveModel(modelName);
//            SAP2000API.AnalayzeModel();
//            SAP2000API.GetFrameElementAnalysisResults(combos, Beams);
//            SAP2000API.GetFrameElementAnalysisResults(new SAPLoadCombination[] { envelope }, Beams);
//            SAP2000API.GetFrameElementAnalysisResults(combos, Columns);
//            SAP2000API.GetFrameElementAnalysisResults(new SAPLoadCombination[] { envelope }, Columns);


//            // // ===========================
//        }

//        private void AssignLoads(HndzFrame3D inputs, SAPFrameElement[] LeftCols, SAPFrameElement[] RightCols, SAPFrameElement[,] LeftBeams, SAPFrameElement[,] RightBeams)
//        {
//            SAPDistributedLoad CoverLoad = new SAPDistributedLoad("Cover", Cover, 0, 1, 0.01 * inputs.BaySpacing, 0.01 * inputs.BaySpacing, HndzLoadDirectionEnum.GravityProjected);
//            SAPDistributedLoad LiveLoad = new SAPDistributedLoad("Live", Live, 0, 1, 0.057 * inputs.BaySpacing, 0.057 * inputs.BaySpacing, HndzLoadDirectionEnum.GravityProjected);
//            #region WindLoad
//            double WL_1, WL_2, WL_3, WL_4, WL_1N, WL_2N, WL_3N, WL_4N, WL_1_B, WL_2_B, WL_1_BN, WL_2_BN;
//            ASCE107Wind.LoadParameters((int)inputs.Location, (RiskCategory)inputs.RiskCategory, (ExposureCategory)inputs.ExposureCategory, (float)inputs.RidgeHeight, (float)inputs.inputs.EaveHeight, (float)inputs.Length, (float)inputs.inputs.Width, RoofType.Gable, 1, 0.85f, true, false); //TODO: Make it not hard coded
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
//            foreach (SAPFrameElement beam in LeftBeams)
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
//            foreach (SAPFrameElement beam in RightBeams)
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

//            //SAP2000API.AddDistributedLoad(LeftCols, windLoadCompression);
//            //SAP2000API.AddDistributedLoad(RightCols, windLoadSuction);
//            //=================
//            SAP2000API.AddDistributedLoad(LeftCols, WLBLColumn);
//            SAP2000API.AddDistributedLoad(LeftCols, WLULColumn);
//            SAP2000API.AddDistributedLoad(LeftCols, WRBLColumn);
//            SAP2000API.AddDistributedLoad(LeftCols, WRULColumn);
//            SAP2000API.AddDistributedLoad(LeftCols, WEBLColumn);
//            SAP2000API.AddDistributedLoad(LeftCols, WEULColumn);

//            Columns.AddRange(LeftCols);

//            SAP2000API.AddDistributedLoad(RightCols, WLBRColumn);
//            SAP2000API.AddDistributedLoad(RightCols, WLURColumn);
//            SAP2000API.AddDistributedLoad(RightCols, WRBRColumn);
//            SAP2000API.AddDistributedLoad(RightCols, WRURColumn);
//            SAP2000API.AddDistributedLoad(RightCols, WEBRColumn);
//            SAP2000API.AddDistributedLoad(RightCols, WEURColumn);

//            Columns.AddRange(RightCols);
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
//            SAP2000API.AddDesignDefaultCombos(eMatType.Steel); //TODO retrieve combos
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

#endregion