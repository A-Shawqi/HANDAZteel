using HANDAZ.Entities;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPDistributedLoad: SAPLoad
    {
        public SAPDistributedLoad(string name, SAPLoadPattern loadType, double dist1, double dist2,
            double val1, double val2, HndzLoadDirectionEnum loadDirection,bool isRelativeDist = true, bool isReplacement = true):
            base(name,loadType,loadDirection,isReplacement)
        {
            IsRelativeDist = isRelativeDist;
            Dist1 = dist1;
            Dist2 = dist2;
            Val1 = val1;
            Val2 = val2;
        }
        private double dist1;

        public double Dist1
    {
            get { return dist1; }
            set
            {
                if (IsRelativeDist)
                {
                    if (value < 0 || value > 1)
                    {
                        throw new System.Exception("Load distance is assigned relative but sent as an absolute value");
                    }
                }

                dist1 = value;
            }
        }
        private double dist2;

        public double Dist2
        {
            get { return dist2; }
            set
            {
                if (IsRelativeDist)
                {
                    if (value < 0 || value > 1)
                    {
                        throw new System.Exception("Load distance is assigned relative but sent as an absolute value");
                    }
                }

                dist2 = value;
            }
        }
        public bool IsRelativeDist { get; internal set; }
        public double Val1 { get; set; }
        public double Val2 { get; set; }

    }
}