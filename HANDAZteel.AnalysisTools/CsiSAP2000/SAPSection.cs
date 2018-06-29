using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public abstract class SAPSection:ISAPAPIComponent
    {
        public bool IsDefinedInSAP { get; set; } = false;
        public string Name { get; set; }

        public abstract SAPSection GetAssumedSection();
        public abstract SAPSection GetInitialSection(SAPMaterial mat);

    }
}
