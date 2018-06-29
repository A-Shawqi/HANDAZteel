using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools
{
    class RobotMapped : IAnalysisTool
    {
        protected static IRobotApplication robotApp;
        protected static IRobotNodeServer nodeServer;
        protected static IRobotBarServer barServer;
        protected static IRobotCaseServer caseServer;
        protected static IRobotLabelServer labelServer;
        protected static IRobotProjectPreferences PrefrencesServer;
        protected static IRobotBarForceServer barForceServer;

        public bool AnalayzeModel()
        {
            throw new NotImplementedException();
        }

        public bool CloseApplication()
        {
            throw new NotImplementedException();
        }

        public bool NewModel(string modelPath)
        {
            throw new NotImplementedException();
        }

        public bool OpenModel(string modelPath)
        {
            throw new NotImplementedException();
        }

        public bool SaveModel(string modelPath)
        {
            throw new NotImplementedException();
        }

        public bool StartApplication(bool attachToInstance = true, bool isVisible = false)
        {
            throw new NotImplementedException();
        }
    }
}
