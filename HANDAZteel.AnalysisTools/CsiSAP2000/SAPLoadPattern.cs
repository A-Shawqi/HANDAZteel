using SAP2000v18;
namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPLoadPattern:ISAPAPIComponent
    {
        public bool IsDefinedInSAP { get; set; } = false;
        public string Name { get; set; }
        public eLoadPatternType Type { get; set; }
        public double SelfWeightMultiplyer { get; set; }
        public bool IsAnalysisCase { get; set; }

        public SAPLoadPattern()
        {
            SelfWeightMultiplyer = 0;
            IsAnalysisCase = true;
        }
        public SAPLoadPattern(string name, eLoadPatternType type) : this()
        {
            Name = name;
            this.Type = type;
        }

        public SAPLoadPattern(string name, eLoadPatternType type, double selfWeightMultiplyer, bool isAnalysisCase = true) : this(name, type)
        {
            this.SelfWeightMultiplyer = selfWeightMultiplyer;
            this.IsAnalysisCase = isAnalysisCase;
        }
    }
}