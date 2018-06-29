using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    internal interface ISAPAPIComponent
    {
        bool IsDefinedInSAP { get; set; }
    }
}
