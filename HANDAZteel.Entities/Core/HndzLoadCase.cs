using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzLoadCase: HndzLoadDefinition
    {
       public HndzLoadCase(string name,string description):base(name,description)
        {
        }
        public HndzLoadCase() : this(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
        }
    }
}