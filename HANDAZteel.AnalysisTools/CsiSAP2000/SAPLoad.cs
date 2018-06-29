using HANDAZ.Entities;
using System;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
   
    public abstract class SAPLoad:ISAPAPIComponent
    {
        protected SAPLoad(string name, SAPLoadPattern loadType, HndzLoadDirectionEnum loadDirection, bool isReplacement = true)
        {
            Name = name;
            LoadType = loadType;
            LoadDirection = loadDirection;
            IsReplacement = isReplacement;
        }

        public string Name { get; set; }
        public SAPLoadPattern LoadType { get; set; }
        public HndzLoadDirectionEnum LoadDirection { get; set; }
        public bool IsReplacement { get; set; }

        public bool IsDefinedInSAP { get; set; } = false;
    }
}