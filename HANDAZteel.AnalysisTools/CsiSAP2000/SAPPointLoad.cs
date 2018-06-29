using HANDAZ.Entities;
namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPPointLoad:SAPLoad
    {
        public SAPPointLoad(string name, SAPLoadPattern loadType,HndzLoadDirectionEnum loadDirection,double distance, double value, bool isReplacement=true) :base(name,loadType,loadDirection,isReplacement)
        {
            Distance = distance;
            Value = value;
        }

        public double Distance { get; set; }
        public double Value { get; set; }
    }
}