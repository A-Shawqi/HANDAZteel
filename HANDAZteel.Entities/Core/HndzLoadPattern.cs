using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzLoadPattern: HndzLoadDefinition
    {
        public HndzLoadPatternType Type { get; set; }
        public HndzLoadPattern(string name,HndzLoadPatternType type):base(name,HndzResources.DefaultDescription)
        {
            Type = type;
        }

    }
}