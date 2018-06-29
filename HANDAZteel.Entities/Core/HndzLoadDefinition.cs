using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzLoadCase))]
    [KnownType(typeof(HndzLoadCombination))]
    [KnownType(typeof(HndzLoadPattern))]
    public class HndzLoadDefinition: HndzRoot
    {
       public HndzLoadDefinition(string name,string description):base(name,description)
        {

        }

        public HndzLoadDefinition() : this(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {

        }
    }
}