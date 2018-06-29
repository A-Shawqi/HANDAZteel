namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProMember
    {
        private int number;
        private static int counter = 0;

        public STAADProMember(STAADProPoint startPoint, STAADProPoint endPoint, STAADProSection section):this()
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Section = section;
        }

        public STAADProMember()
        {
            Number = ++counter;
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public STAADProPoint StartPoint { get; set; }
        public STAADProPoint EndPoint { get; set; }
        public STAADProSection Section { get; set; }
        public bool IsSTAADDefined { get; internal set; } = false;
    }
}