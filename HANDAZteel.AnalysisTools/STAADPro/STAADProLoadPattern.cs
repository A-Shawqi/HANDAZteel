namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProLoadPattern
    {
        private static int counter;

        public STAADProLoadPattern(STAADProLoadTypeEnum loadType):this()
        {
            LoadType = loadType;
        }

        public STAADProLoadPattern(STAADProLoadTypeEnum loadType, string name) : this(loadType)
        {
            this.Name = name;
        }

        public STAADProLoadPattern()
        {
            Number = ++counter;
        }

        public int Number { get; set; }
        public STAADProLoadTypeEnum LoadType { get; set; }
        public string Name { get; set; }
    }
}