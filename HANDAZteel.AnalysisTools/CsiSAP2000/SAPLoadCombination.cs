using System.Collections.Generic;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public enum LoadCombinationsEnum
    {
        LinearAdditive,
        Envelope,
        AbsoluteAdditive,
        SRSS,
        RangeAdditive
    }

    public class SAPLoadCombination : ISAPAPIComponent
    {
        public SAPLoadCombination(string name, LoadCombinationsEnum combType) : this()
        {
            Name = name;
            CombType = combType;
        }


        public SAPLoadCombination()
        {
            LoadCases = new List<SAPLoadCase>();
            LoadCasesFactors = new List<float>();

            LoadCombos = new List<SAPLoadCombination>();
            LoadCombosFactors = new List<float>();
    }

        public SAPLoadCombination(string name, LoadCombinationsEnum combType, List<SAPLoadCase> loadCases, List<float> loadCasesFactors, List<SAPLoadCombination> loadCombos, List<float> loadCombosFactors)
        {
            CombType = combType;
            LoadCases = loadCases;
            LoadCasesFactors = loadCasesFactors;
            LoadCombos = loadCombos;
            LoadCombosFactors = loadCombosFactors;
            Name = name;
        }

        public bool IsDefinedInSAP { get; set; } = false;
        public LoadCombinationsEnum CombType { get; set; }


        internal List<SAPLoadCase> loadCases;

        public List<SAPLoadCase> LoadCases
        {
            get { return loadCases; }
            set { loadCases = value; }
        }
        internal List<float> loadCasesFactors;

        public List<float> LoadCasesFactors
        {
            get { return loadCasesFactors; }
            set { loadCasesFactors = value; }
        }


        internal List<SAPLoadCombination> loadCombos;

        public List<SAPLoadCombination> LoadCombos
        {
            get { return loadCombos; }
            set { loadCombos = value; }
        }

        internal List<float> loadCombosFactors;

        public List<float> LoadCombosFactors
        {
            get { return loadCombosFactors; }
            set { loadCombosFactors = value; }
        }



        public string Name { get; set; }
    }
}