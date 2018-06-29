using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotOM;
using Rhino.Geometry;
using HANDAZ.PEB.Entities;

namespace HANDAZ.PEB.AnalysisTools
{
    public class RobotStart
    {
        //what needs to be done 
        //set robot visibility to False to insure robot doesnt show 
        //Set Parameters of material 
        //set Parameteres of project to metric 
        //set load cases as in Nodal force
        static IRobotApplication robotApp;
        static IRobotNodeServer nodeServer;
        static IRobotBarServer barServer;
        static IRobotCaseServer caseServer;
        static IRobotLabelServer labelServer;
        static IRobotProjectPreferences PrefrencesServer;
        static IRobotBarForceServer barForceServer;

        public static void InitRobot()
        {
            robotApp = new RobotApplication();
            nodeServer = robotApp.Project.Structure.Nodes;
            barServer = robotApp.Project.Structure.Bars;
            caseServer = robotApp.Project.Structure.Cases;
            labelServer = robotApp.Project.Structure.Labels;
            PrefrencesServer = robotApp.Project.Preferences;
            barForceServer = robotApp.Project.Structure.Results.Bars.Forces;
        }
        public static bool StartRobot()
        {
            robotApp.Visible = 1;
            return true;
        }
        public static bool New2dFrameProject()
        {
            if (robotApp.Project.IsActive == 0)
            {
                robotApp.Project.New(IRobotProjectType.I_PT_FRAME_2D);
                PrefrencesServer.SetActiveCode(IRobotCodeType.I_CT_STEEL_STRUCTURES, "ANSI/AISC 360-10");
                PrefrencesServer.SetActiveCode(IRobotCodeType.I_CT_CODE_COMBINATIONS, "ASD ASCE 7-10");
                PrefrencesServer.Units.UseMetricAsDefault = true;
            }
            return  true;
        }

        public static bool SetMaterial(Material Mymaterial)
        {
           IRobotLabel material = labelServer.Create(IRobotLabelType.I_LT_MATERIAL, Mymaterial.MaterialName);
           RobotMaterialData materialData = material.Data;
           materialData.Type = IRobotMaterialType.I_MT_STEEL;
           materialData.E = Mymaterial.YoungsModulas;
           materialData.NU = Mymaterial.PoissionRatio;
           materialData.Kirchoff = Mymaterial.ShearModulas;
           materialData.RO = Mymaterial.UnitWeight;
           materialData.LX = Mymaterial.ThermalExpansion;
           materialData.DumpCoef = Mymaterial.DampingRatio;
           materialData.SaveToDBase();
           robotApp.Project.Structure.Labels.Store(material);
            return  true;
        }
        public static bool DrawBeam(int beamId, Node _startPoint, Node _endPoint)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(_startPoint.Id, _startPoint.X, _startPoint.Y, _startPoint.Z);
            nodeServer.Create(_endPoint.Id, _endPoint.X, _endPoint.Y, _endPoint.Z);
            barServer.Create(beamId, _startPoint.Id, _endPoint.Id);
            robotApp.Interactive = 1;
            return  true;
        }
        //=========================
        //Beam overload to Take a Beam Object From Core 
        public static bool DrawBeam(Beam beam)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(beam.BeamStart.Id, beam.BeamStart.X, beam.BeamStart.Y, beam.BeamStart.Z);
            nodeServer.Create(beam.BeamEnd.Id, beam.BeamEnd.X, beam.BeamEnd.Y, beam.BeamEnd.Z);
            barServer.Create(beam.Id, beam.BeamStart.Id, beam.BeamEnd.Id);
            robotApp.Interactive = 1;
            return   true;
        }
        //=====================
        //Draw Column 
        public static bool DrawColumn(int _Id, Node _startPoint, Node _endPoint)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(_startPoint.Id, _startPoint.X, _startPoint.Y, _startPoint.Z);
            nodeServer.Create(_endPoint.Id, _endPoint.X, _endPoint.Y, _endPoint.Z);
            barServer.Create(_Id, _startPoint.Id, _endPoint.Id);
            robotApp.Interactive = 1;
            return  true;
        }
        //overload to take a genric type of column from core 
        public static bool DrawColumn(Column Column)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(Column.ColumnStart.Id, Column.ColumnStart.X, Column.ColumnStart.Y, Column.ColumnStart.Z);
            nodeServer.Create(Column.ColumnEnd.Id, Column.ColumnEnd.X, Column.ColumnEnd.Y, Column.ColumnEnd.Z);
            barServer.Create(Column.Id, Column.ColumnStart.Id, Column.ColumnEnd.Id);
            robotApp.Interactive = 1;
            return  true;
        }
        #region Purlin 
     
        #endregion
        public static bool DrawFrame(Frame Frame)
        {
            robotApp.Interactive = 0;
            for (int i = 0; i < Frame.Beams.Count; i++)
            {
                DrawBeam(Frame.Beams[i]);
            }
            for (int i = 0; i < Frame.Columns.Count; i++)
            {
                DrawColumn(Frame.Columns[i]);
            }
            robotApp.Interactive = 1;
            return   true;
        }
        public static bool DrawFrame(List<Node> frameNodes)
        {
            robotApp.Interactive = 0;
            for (int i = 0; i < frameNodes.Count; i++)
            {
                nodeServer.Create(frameNodes[i].Id, frameNodes[i].X, frameNodes[i].Y, frameNodes[i].Z);
            }
            for (int i = 0; i < frameNodes.Count - 1; i++)
            {
                barServer.Create((i + 1), frameNodes[i].Id, frameNodes[i + 1].Id);
            }
            robotApp.Interactive = 1;
            return  true;
        }
        public static bool DrawGrids(List<Grid> Grids)
        {
            return  true;
        }
        //public static bool SetCases()
        //{
        //    robotApp.Interactive = 0;
        //    IRobotSimpleCase deadLoad = caseServer.CreateSimple(caseServer.FreeNumber, "Frame Weight Dead", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    IRobotSimpleCase coverDaedLoad = caseServer.CreateSimple(caseServer.FreeNumber, "Cover Dead", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    IRobotSimpleCase liveLoad = caseServer.CreateSimple(caseServer.FreeNumber, "Live", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    IRobotSimpleCase windLoadLeft = caseServer.CreateSimple(caseServer.FreeNumber, "Wind_left", IRobotCaseNature.I_CN_WIND, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    IRobotSimpleCase windLoadRight = caseServer.CreateSimple(caseServer.FreeNumber, "Wind_right", IRobotCaseNature.I_CN_WIND, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    IRobotSimpleCase tempretureNegative = caseServer.CreateSimple(caseServer.FreeNumber, "Temprature +VE", IRobotCaseNature.I_CN_TEMPERATURE, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    IRobotSimpleCase tempreturePostive = caseServer.CreateSimple(caseServer.FreeNumber, "Temprature -VE", IRobotCaseNature.I_CN_TEMPERATURE, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
        //    IRobotSimpleCase earthQuake = caseServer.CreateSimple(caseServer.FreeNumber, "EarthQuake", IRobotCaseNature.I_CN_SEISMIC, IRobotCaseAnalizeType.I_CAT_DYNAMIC_SEISMIC);
        //    robotApp.Interactive = 1;

        //    //======================
        //    // live Load = 0.57 
        //    // Shall we Support Snow load or not ? 
        //    //Wind load is to be calculated using Bolt Wind calculator 
        //    // wind load = q = 2.546    * 10 ^ -5 * v ^ 2 * h ^ 2/7
        //    //Thermal load is to be calucalted form the eqution , Changes in Unit Stress  = E e t ; 
        //    // E = 20340 ,  e = 0.0000117 , t 


        //    // Live load = 0.57 
        //    // wind speed = 130 
        //    return true;
        //}
        public static bool SetSupports(List<Node> frameNodes)
        {
            robotApp.Interactive = 0;
            IRobotNode leftSupportnode = (IRobotNode)nodeServer.Get(frameNodes[0].Id);
            leftSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, "Pinned");
            IRobotNode rightSupportnode = (IRobotNode)nodeServer.Get(frameNodes[frameNodes.Count - 1].Id);
            rightSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, "Pinned");
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetSupports(Frame frame)
        {
            robotApp.Interactive = 0;
            IRobotNode leftSupportnode = (IRobotNode)nodeServer.Get(frame.Supports[0].Position.Id);
            leftSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, frame.Supports[0].SupportType.ToString());
            IRobotNode rightSupportnode = (IRobotNode)nodeServer.Get(frame.Supports[1].Position.Id);
            rightSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, frame.Supports[1].SupportType.ToString());
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetColumnSections(Frame frame)
        {
            //needs editing 
            robotApp.Interactive = 0;
            IRobotLabel builtUpSectionLabel = labelServer.Create(IRobotLabelType.I_LT_BAR_SECTION, "Hndz - Tapered");
            RobotBarSectionData builtUpsectionData = builtUpSectionLabel.Data;
            builtUpsectionData.Type = IRobotBarSectionType.I_BST_NS_II ;
            builtUpsectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            RobotBarSectionNonstdData builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1 , frame.Columns[0].TaperedAtStartNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Columns[0].TaperedAtStartNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Columns[0].TaperedAtStartNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Columns[0].TaperedAtStartNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Columns[0].TaperedAtStartNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Columns[0].TaperedAtStartNode.Tw);
            builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(0);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, frame.Columns[0].TaperedATEndNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Columns[0].TaperedATEndNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Columns[0].TaperedATEndNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Columns[0].TaperedATEndNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Columns[0].TaperedATEndNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Columns[0].TaperedATEndNode.Tw);
            builtUpsectionData.CalcNonstdGeometry();
            labelServer.Store(builtUpSectionLabel);


            RobotSelection Selection = robotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            Selection.AddOne(frame.Columns[0].Id);
            Selection.AddOne(frame.Columns[1].Id);
            
            barServer.SetLabel(Selection, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);
           
            //barServer.SetLabel(RightColumn, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);


            robotApp.Interactive = 1;
            return  true;

        }
        public static bool SetBeamSections(Frame frame)
        {
            robotApp.Interactive = 0;
            IRobotLabel builtUpSectionLabel = labelServer.Create(IRobotLabelType.I_LT_BAR_SECTION, "Hndz - Tapered");
            RobotBarSectionData builtUpsectionData = builtUpSectionLabel.Data;
            builtUpsectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            builtUpsectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            RobotBarSectionNonstdData builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, frame.Beams[0].BeamSectionAtStartNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Beams[0].BeamSectionAtStartNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Beams[0].BeamSectionAtStartNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Beams[0].BeamSectionAtStartNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Beams[0].BeamSectionAtStartNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Beams[0].BeamSectionAtStartNode.Tw);
            builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(0);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, frame.Beams[0].BeamSectionATEndNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Beams[0].BeamSectionATEndNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Beams[0].BeamSectionATEndNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Beams[0].BeamSectionATEndNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Beams[0].BeamSectionATEndNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Beams[0].BeamSectionATEndNode.Tw);

            builtUpsectionData.CalcNonstdGeometry();
            labelServer.Store(builtUpSectionLabel);


            RobotSelection Selection = robotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            Selection.AddOne(frame.Beams[0].Id);
            Selection.AddOne(frame.Beams[1].Id);

            barServer.SetLabel(Selection, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);

            //barServer.SetLabel(RightColumn, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);


            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetOwnWeight()
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase OwnWeight = caseServer.CreateSimple(caseServer.FreeNumber, "OwnWeight", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            IRobotLoadRecord2 OwnWeightRecord = OwnWeight.Records.Create(IRobotLoadRecordType.I_LRT_DEAD) as IRobotLoadRecord2;
            OwnWeightRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE ,1);
            OwnWeightRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_COEFF, 1);
            OwnWeightRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Z, -1);
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetCoverLoad(Structure Structure)
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase CoverLoad = caseServer.CreateSimple(caseServer.FreeNumber, "CoverLoad", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            IRobotLoadRecord2 CoverLoadRecord = CoverLoad.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM) as IRobotLoadRecord2;
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_RELATIVE, 1);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PX,0);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PY,0);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, -1000 * 10);
            CoverLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[0].Id);
            CoverLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[1].Id);

            #region A9ba 7aga 3amltha fi 7yaty 

            //        CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, Structure.Purlins[i].PurlinStart.X / Math.Cos(Structure.Frames[0].Angle));


            //for (int i = 0; i < Structure.Purlins.Count; i++)
            //{
            //    IRobotLoadRecord2 CoverLoadRecord = CoverLoad.Records.Create(IRobotLoadRecordType.I_LRT_BAR_FORCE_CONCENTRATED) as IRobotLoadRecord2;
            //    CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_REL, 0);
            //    CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FX, 0);
            //    CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FY, 0);
            //    CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_FZ, -1000 * 10);
            //    CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_CY, 0);

            //    if (Structure.Purlins[0].PurlinStart.X <= Structure.Frames[0].Beams[0].Beamline.Length)
            //    {
            //        CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_LOC, 1);
            //        CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, Structure.Purlins[i].PurlinStart.X / Math.Cos(Structure.Frames[0].Angle));
            //        CoverLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[0].BeamId);
            //    }
            //    else if (Structure.Purlins[0].PurlinStart.X == Structure.Frames[0].Beams[0].Beamline.Length)
            //    {
            //        CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_LOC, 0);
            //        CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X,Structure.Purlins[i].PurlinStart.X / Math.Cos(Structure.Frames[0].Angle));
            //        CoverLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[0].BeamId);
            //    }
            //    else
            //    {
            //        CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_LOC, 1);
            //        CoverLoadRecord.SetValue((short)IRobotBarForceConcentrateRecordValues.I_BFCRV_X, Structure.Purlins[i].PurlinStart.X / Math.Cos(Structure.Frames[0].Angle));
            //        CoverLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[1].BeamId);
            //    }

            //}

            //for (int i = 0; i < Structure.Frames[0].Beams.Count; i++)
            //{
            //    CoverLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[i].BeamId);
            //}
            #endregion
            robotApp.Interactive = 1;
            return true;
        }
        public static  bool SetLiveLoad(Structure Structure)
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase liveLoad = caseServer.CreateSimple(caseServer.FreeNumber, "Live", IRobotCaseNature.I_CN_EXPLOATATION, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            IRobotLoadRecord2 liveLoadRecord = liveLoad.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM) as IRobotLoadRecord2;
            liveLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_RELATIVE, 1);
            liveLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);
            liveLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            liveLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PX, 0);
            liveLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PY, 0);
            liveLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, -1000 * 10);
            liveLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[0].Id);
            liveLoadRecord.Objects.AddOne(Structure.Frames[0].Beams[1].Id);
            robotApp.Interactive = 1;
            return true;

        }
        public static bool SetWindLoad(Structure Structure)
        {
            SetWindloadLeft(Structure);
            SetWindLoadRight(Structure);
            return true;
        }
        public static bool SetWindloadLeft(Structure Structure)
        {
            //there is a known issue with setting the windload to left or right due to the local axis of frame drawing
            //if you draw from bottom to top it will be fine 
            // if you draw from top to bottom , voila , error 
            ////===============
            //For beams
            robotApp.Interactive = 0;
            IRobotSimpleCase windLoadLeft = caseServer.CreateSimple(caseServer.FreeNumber, "Wind_left", IRobotCaseNature.I_CN_WIND, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            //================= For Compression
            IRobotLoadRecord2 windLoadLeftRecordLeft = windLoadLeft.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM) as IRobotLoadRecord2;
            windLoadLeftRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_RELATIVE, 1);
            windLoadLeftRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);
            windLoadLeftRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            windLoadLeftRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PX, 0);
            windLoadLeftRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PY, 0);
            windLoadLeftRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, -1000 * 10);
            //================= For Suction
            IRobotLoadRecord2 windLoadLeftRecordRight = windLoadLeft.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM) as IRobotLoadRecord2;
            windLoadLeftRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_RELATIVE, 1);
            windLoadLeftRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);
            windLoadLeftRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            windLoadLeftRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PX, 0);
            windLoadLeftRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PY, 0);
            windLoadLeftRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, 1000 * 10);
            //=================
            //left
            windLoadLeftRecordLeft.Objects.AddOne(Structure.Frames[0].Columns[0].Id);
            windLoadLeftRecordLeft.Objects.AddOne(Structure.Frames[0].Beams[0].Id);
            windLoadLeftRecordLeft.Objects.AddOne(Structure.Frames[0].Columns[1].Id);

            //=============== Suction
            //right 
            windLoadLeftRecordRight.Objects.AddOne(Structure.Frames[0].Beams[1].Id);

            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetWindLoadRight(Structure Structure)
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase windLoadRight = caseServer.CreateSimple(caseServer.FreeNumber, "Wind_Right", IRobotCaseNature.I_CN_WIND, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            //================= For Compression
            IRobotLoadRecord2 windLoadRightRecordLeft = windLoadRight.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM) as IRobotLoadRecord2;
            windLoadRightRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_RELATIVE, 1);
            windLoadRightRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);
            windLoadRightRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            windLoadRightRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PX, 0);
            windLoadRightRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PY, 0);
            windLoadRightRecordLeft.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, -1000 * 10);
            //================= For Suction
            IRobotLoadRecord2 windLoadRightRecordRight = windLoadRight.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM) as IRobotLoadRecord2;
            windLoadRightRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_RELATIVE, 1);
            windLoadRightRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);
            windLoadRightRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            windLoadRightRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PX, 0);
            windLoadRightRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PY, 0);
            windLoadRightRecordRight.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, 1000 * 10);
            //=================
            //left

            windLoadRightRecordLeft.Objects.AddOne(Structure.Frames[0].Beams[1].Id);

            //=============== Compression
            //=============== Suction
            //right 
            windLoadRightRecordRight.Objects.AddOne(Structure.Frames[0].Beams[0].Id);
            windLoadRightRecordRight.Objects.AddOne(Structure.Frames[0].Columns[0].Id);
            windLoadRightRecordRight.Objects.AddOne(Structure.Frames[0].Columns[1].Id);



            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetTempretureload()
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase tempretureNegative = caseServer.CreateSimple(caseServer.FreeNumber, "Temprature +VE", IRobotCaseNature.I_CN_TEMPERATURE, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            IRobotSimpleCase tempreturePostive = caseServer.CreateSimple(caseServer.FreeNumber, "Temprature -VE", IRobotCaseNature.I_CN_TEMPERATURE, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            robotApp.Interactive = 1;
            return true;

        }
        public static bool  SetEarthQuakeLoad()
        {
            robotApp.Interactive = 0;
           IRobotSimpleCase earthQuake = caseServer.CreateSimple(caseServer.FreeNumber, "EarthQuake", IRobotCaseNature.I_CN_SEISMIC, IRobotCaseAnalizeType.I_CAT_DYNAMIC_SEISMIC);
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetloadCases(Structure Structure)
        {
            SetOwnWeight();
            SetCoverLoad(Structure);
            SetLiveLoad(Structure);
            SetWindLoad(Structure);
            SetTempretureload();
            //SetEarthQuakeLoad();
            return true;
        }
        public static void Design()
        {
            robotApp.Project.CalcEngine.Calculate();
        }

        public static void GetResults()
        {
            IRobotCollection Bars = robotApp.Project.Structure.Bars.GetAll();
            IRobotCaseCollection LoadingCases = robotApp.Project.Structure.Cases.GetAll();
            for (int i = 1; i <= Bars.Count; i++)
            {
                IRobotBar Element = Bars.Get(i);
                for (int j = 1; j <= LoadingCases.Count; j++)
                {
                    IRobotCase Case = LoadingCases.Get(j);
                    int caseNum = Case.Number;
                    IRobotBarForceData Force = barForceServer.Value(i, caseNum, 0.5);
                    double FX = Force.FX;
                    double FY = Force.FY;
                    double FZ = Force.FZ;
                    double MX = Force.MX;
                    double MY = Force.MY;
                    double MZ = Force.MZ;
                }
            }
        }
    }

}
