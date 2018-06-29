namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProUniformLoad
    {
        public STAADProUniformLoad(STAADProDirectionEnum direction, double value)
        {
            Direction = direction;
            Value = value;
        }

        public STAADProDirectionEnum Direction { get; set; }
        public double Value { get; set; }
    }
}