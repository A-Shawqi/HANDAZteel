using System.Collections.Generic;

namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProLoadCombination
    {
        public STAADProLoadCombination(string name, Dictionary<float, STAADProLoadPattern> loadPatterns)
        {
            Number = ++counter;
            Name = name;
            LoadPatterns = loadPatterns;
        }
        private static int counter=1000; 
        public int Number { get; set; }

        public string Name { get; set; }

        public Dictionary<float,STAADProLoadPattern> LoadPatterns { get; set; }
    }
}