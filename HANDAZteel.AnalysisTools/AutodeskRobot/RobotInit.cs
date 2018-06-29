using RobotOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools
{
    public class RobotInit
    {
        protected static IRobotApplication robotApp;
        protected static IRobotNodeServer nodeServer;
        protected static IRobotBarServer barServer;
        protected static IRobotCaseServer caseServer;
        protected static IRobotLabelServer labelServer;
        protected static IRobotProjectPreferences PrefrencesServer;
        protected static IRobotBarForceServer barForceServer;

        public static void RobotKickStart()
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
                robotApp.Interactive = 0;
                robotApp.Project.New(IRobotProjectType.I_PT_FRAME_2D);
                PrefrencesServer.SetActiveCode(IRobotCodeType.I_CT_STEEL_STRUCTURES, "ANSI/AISC 360-10");
                PrefrencesServer.SetActiveCode(IRobotCodeType.I_CT_CODE_COMBINATIONS, "LRFD ASCE 7-10");
                PrefrencesServer.Units.UseMetricAsDefault = true;
                robotApp.Interactive = 1;

            }
            return true;
        }
        public static void WriteFile(string FileName)
        {
            robotApp.Interactive = 0;
            // robotApp.Project.SaveAs(FileName + "Structure.rtd");

            //  robotApp.Project.SaveToFormat(IRobotProjectSaveFormat.I_PSF_RTD, "K:\\Mark.rtd");
            robotApp.Project.SaveToFormat(IRobotProjectSaveFormat.I_PSF_RTD, FileName);
            robotApp.Interactive = 1;
           // robotApp.Quit(IRobotQuitOption.I_QO_SAVE_CHANGES);

            // robotApp.Project.Save(FileName + "-HndazSteel.Rtd");
        }
        public static void GetPath(string modelName)
        {
          //  return Path.GetFullPath(ModelDirectory + System.IO.Path.DirectorySeparatorChar + modelName + ".sdb");
        }
    }
}
