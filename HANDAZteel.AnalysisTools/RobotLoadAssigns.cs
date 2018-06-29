using HANDAZ.PEB.Entities;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools
{
    public class RobotLoadAssigns : RobotInit
    {
        public static bool SetOwnWeight()
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase OwnWeight = caseServer.CreateSimple(caseServer.FreeNumber, "OwnWeight", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            IRobotLoadRecord2 OwnWeightRecord = OwnWeight.Records.Create(IRobotLoadRecordType.I_LRT_DEAD) as IRobotLoadRecord2;
            OwnWeightRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_ENTIRE_STRUCTURE, 1);
            OwnWeightRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_COEFF, 1);
            OwnWeightRecord.SetValue((short)IRobotDeadRecordValues.I_DRV_Z, -1);
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetCoverLoad(Frame frame)
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase CoverLoad = caseServer.CreateSimple(caseServer.FreeNumber, "CoverLoad", IRobotCaseNature.I_CN_PERMANENT, IRobotCaseAnalizeType.I_CAT_STATIC_LINEAR);
            IRobotLoadRecord2 CoverLoadRecord = CoverLoad.Records.Create(IRobotLoadRecordType.I_LRT_BAR_UNIFORM) as IRobotLoadRecord2;
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_RELATIVE, 1);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_LOCAL_SYSTEM, 1);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PROJECTED, 1);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PX, 0);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PY, 0);
            CoverLoadRecord.SetValue((short)IRobotUniformRecordValues.I_URV_PZ, -0.01 * 10);
            CoverLoadRecord.Objects.AddOne(frame.Beams[0].Id);
            CoverLoadRecord.Objects.AddOne(frame.Beams[1].Id);

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
        public static bool SetLiveLoad(Frame frame)
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
            liveLoadRecord.Objects.AddOne(frame.Beams[0].Id);
            liveLoadRecord.Objects.AddOne(frame.Beams[1].Id);
            robotApp.Interactive = 1;
            return true;

        }
        public static bool SetWindLoad(Frame frame)
        {
            SetWindloadLeft(frame);
            SetWindLoadRight(frame);
            return true;
        }
        public static bool SetWindloadLeft(Frame frame)
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
            windLoadLeftRecordLeft.Objects.AddOne(frame.Columns[0].Id);
            windLoadLeftRecordLeft.Objects.AddOne(frame.Beams[0].Id);
            windLoadLeftRecordLeft.Objects.AddOne(frame.Columns[1].Id);

            //=============== Suction
            //right 
            windLoadLeftRecordRight.Objects.AddOne(frame.Beams[1].Id);

            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetWindLoadRight(Frame frame)
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

            windLoadRightRecordLeft.Objects.AddOne(frame.Beams[1].Id);

            //=============== Compression
            //=============== Suction
            //right 

            //Neeeeeds EXTREEEEEEEEEEEEEMEEEEEE Revision
            windLoadRightRecordRight.Objects.AddOne(frame.Beams[0].Id);
            windLoadRightRecordRight.Objects.AddOne(frame.Columns[0].Id);
            windLoadRightRecordRight.Objects.AddOne(frame.Columns[1].Id);
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
        public static bool SetEarthQuakeLoad()
        {
            robotApp.Interactive = 0;
            IRobotSimpleCase earthQuake = caseServer.CreateSimple(caseServer.FreeNumber, "EarthQuake", IRobotCaseNature.I_CN_SEISMIC, IRobotCaseAnalizeType.I_CAT_DYNAMIC_SEISMIC);
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetloadCases(Frame frame)
        {
            SetOwnWeight();
            SetCoverLoad(frame);
            SetLiveLoad(frame);
            SetWindLoad(frame);
            SetTempretureload();
            //SetEarthQuakeLoad();
            return true;
        }
        public static bool SetLoadCombinations()
        {
            IRobotCodeCombinationEngine ComboEngine;
            RobotCodeCombinationEngine codeCombination;
            IRobotCodeCmbGenerationParams combParams;


            codeCombination = robotApp.Project.Structure.Cases.CodeCmbEngine;
            codeCombination.Params.GenType = IRobotCodeCmbGenerationType.I_CCGT_FULL;
            codeCombination.Params.SelectCombinationType(IRobotCombinationType.I_CBT_ULS, true);

            combParams = robotApp.Project.Structure.Cases.CodeCmbEngine.Params;
            combParams.Groups.New(IRobotCaseNature.I_CN_PERMANENT, IRobotCodeCmbOperator.I_CCO_AND, "0");
            combParams.Groups.New(IRobotCaseNature.I_CN_EXPLOATATION, IRobotCodeCmbOperator.I_CCO_AND_OR, "0");
            combParams.Groups.New(IRobotCaseNature.I_CN_WIND, IRobotCodeCmbOperator.I_CCO_EXCLUSIVE_OR, "0");
            combParams.Groups.New(IRobotCaseNature.I_CN_SEISMIC, IRobotCodeCmbOperator.I_CCO_EXCLUSIVE_OR, "0");
            combParams.Groups.New(IRobotCaseNature.I_CN_SNOW, IRobotCodeCmbOperator.I_CCO_AND, "0");
            combParams.Groups.New(IRobotCaseNature.I_CN_ACCIDENTAL, IRobotCodeCmbOperator.I_CCO_AND, "0");
            combParams.Groups.New(IRobotCaseNature.I_CN_TEMPERATURE, IRobotCodeCmbOperator.I_CCO_AND, "0");
            codeCombination.Generate();
            return true;
        }
    }
}
