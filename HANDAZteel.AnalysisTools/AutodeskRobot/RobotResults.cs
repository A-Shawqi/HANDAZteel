using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.PEB.Entities;

namespace HANDAZ.PEB.AnalysisTools
{
    public class RobotResults : RobotInit
    {
        public static void Analyze()
        {
            robotApp.Project.CalcEngine.Calculate();
        }

        public static List<ResultsR> GetResults()
        {
            List<ResultsR> Results = new List<ResultsR>();
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
                    double KY = Force.KY;
                    double KZ = Force.KZ;
                    ResultsR barResult = new ResultsR(FX, FY, FZ, MX, MY, MZ, KY, KZ);
                    Results.Add(barResult);
                }
            }
            return Results;
        }
        public static void Design(Frame frame)
        {
            IRDimServer RDmServer;
            RDimStream RDmStream;
            IRDimGroups RDmGrps;
            RDimGroup RDmGrp1;
            RDimGrpProfs RDmGrpProfs;

            int BeamNo;
            int ColumnNo;

            RDmServer = robotApp.Project.DimServer;
            RDmServer.Mode = IRDimServerMode.I_DSM_STEEL;
            RDmGrps = RDmServer.GroupsService;
            BeamNo = 1;
            RDmGrp1 = RDmGrps.New(0, BeamNo);
            RDmGrp1.Name = "HNDZ-Beams";
            RDmStream = RDmServer.Connection.GetStream();
            RDmStream.Clear();
            RDmStream.WriteText("3 4");

            RDmGrp1.SetMembList(RDmStream);
            RDmGrpProfs = RDmServer.Connection.GetGrpProfs();
            RDmGrpProfs.Clear();
            RDmStream.Clear();

            RDmGrp1.SetProfs(RDmGrpProfs);
            RDmGrps.Save(RDmGrp1);


            //Group 2
            ColumnNo = 2;
            RDmGrp1 = RDmGrps.New(0, ColumnNo);
            RDmGrp1.Name = "HNDZ-Columns";
            RDmStream = RDmServer.Connection.GetStream();
            RDmStream.Clear();
            RDmStream.WriteText("1 2");

            RDmGrp1.SetMembList(RDmStream);
            RDmGrpProfs = RDmServer.Connection.GetGrpProfs();
            RDmGrpProfs.Clear();
            RDmStream.Clear();

            RDmGrp1.SetProfs(RDmGrpProfs);
            RDmGrps.Save(RDmGrp1);

            // add Section Data to members 
            IRobotLabel BeamTypeLabel;
            IRDimMembDefData BeamMembDefData;

            BeamTypeLabel = labelServer.Create(IRobotLabelType.I_LT_MEMBER_TYPE, "Hndz-Beam");
            BeamMembDefData = BeamTypeLabel.Data;


            //============================================
            // K factors needs to be reviewed 
            BeamMembDefData.SetDeflectionYZ(IRDimMembDefDeflDataType.I_DMDDDT_DEFL_Y, 1);
            BeamMembDefData.SetDeflectionYZ(IRDimMembDefDeflDataType.I_DMDDDT_DEFL_Z, 1);



            BeamMembDefData.Type = IRDimMembDefType.I_DMDT_USER;
            labelServer.Store(BeamTypeLabel);

            RobotSelection Selection = robotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            Selection.AddOne(frame.Beams[0].Id);
            Selection.AddOne(frame.Beams[1].Id);

            barServer.SetLabel(Selection, IRobotLabelType.I_LT_MEMBER_TYPE, BeamTypeLabel.Name);

            //=========================
            IRobotLabel ColumnTypeLabel;
            IRDimMembDefData ColumnMembDefData;

            ColumnTypeLabel = labelServer.Create(IRobotLabelType.I_LT_MEMBER_TYPE, "Hndz-Column");
            ColumnMembDefData = ColumnTypeLabel.Data;


            ColumnMembDefData.Type = IRDimMembDefType.I_DMDT_USER;
            labelServer.Store(ColumnTypeLabel);
            //========================

            RobotSelection Selection2 = robotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            Selection.AddOne(frame.Columns[0].Id);
            Selection.AddOne(frame.Columns[1].Id);

            barServer.SetLabel(Selection, IRobotLabelType.I_LT_MEMBER_TYPE, ColumnTypeLabel.Name);
            //=================
            //Calutlation enginer 

            IRDimCalcEngine RdmEngine = robotApp.Project.DimServer.CalculEngine;

            IRDimCalcParam RdmCalPar = RdmEngine.GetCalcParam();
            IRDimCalcConf RdmCalCnf = RdmEngine.GetCalcConf();

            RDimStream RdmStream = robotApp.Project.DimServer.Connection.GetStream();
            RdmStream.Clear();
            RdmStream.WriteText("all");
            RdmCalPar.SetObjsList(IRDimCalcParamVerifType.I_DCPVT_GROUPS_DESIGN, RdmStream);
            RdmCalPar.SetLimitState(IRDimCalcParamLimitStateType.I_DCPLST_SERVICEABILITY, 1);
            RdmCalPar.SetLimitState(IRDimCalcParamLimitStateType.I_DCPLST_ULTIMATE, 1);
            RdmStream.Clear();
            RdmStream.WriteText("1 2");
            RdmCalPar.SetLoadsList(RdmStream);
            RdmEngine.SetCalcConf(RdmCalCnf);
            RdmEngine.SetCalcParam(RdmCalPar);


        }
    }
}
