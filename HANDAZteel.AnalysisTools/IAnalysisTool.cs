using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools
{
    interface IAnalysisTool
    {
        bool StartApplication(bool attachToInstance=true, bool isVisible= false);
        bool CloseApplication();
        bool NewModel(string modelPath);
        bool OpenModel(string modelPath);
        bool SaveModel(string modelPath);
        bool AnalayzeModel();
    }
}
