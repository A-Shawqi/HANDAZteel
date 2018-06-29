using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.Entities;

namespace HANDAZ.PEB.BusinessComponents
{
    [Serializable]
    public class AdvancedInputs
    {
        public HndzBuildingEnclosingEnum BuildingEnclosing { get; set; }
        public HndzImportanceFactorEnum ImportanceFactor { get; set; }

    }
}
