using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzLoadCombination: HndzLoadDefinition
    {
        public HndzLoadCombination(string name):base(name,HndzResources.DefaultDescription)
        {
            LoadCases = new Dictionary<float, HndzLoadCase>();
            LoadCombos = new Dictionary<float, HndzLoadCombination>();
        }

        public HndzLoadCombination(string name,HndzLoadCombinationsEnum combType, Dictionary<float, HndzLoadCase> loadCases, Dictionary<float, HndzLoadCombination> loadCombos):this(name)
        {
            CombType = combType;
            LoadCases = loadCases;
            LoadCombos = loadCombos;
        }

        public HndzLoadCombinationsEnum CombType { get; set; }

        /// <summary>
        /// Load Cases defined in Key=Multiplier and Value = LoadCase
        /// </summary>
        public Dictionary<float, HndzLoadCase> LoadCases { get; set; }

        /// <summary>
        /// Load Combos defined in Key=Multiplier and Value = LoadCombo
        /// </summary>
        public Dictionary<float, HndzLoadCombination> LoadCombos { get; set; }
    }
}