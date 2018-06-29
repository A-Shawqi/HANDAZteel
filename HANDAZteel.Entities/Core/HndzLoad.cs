using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzPointLoad))]
    [KnownType(typeof(HndzDistributedLoad))]
    public abstract class HndzLoad : HndzRoot
    {
        [DataMember, XmlAttribute]

        public HndzLoadDirectionEnum Direction { get; set; }
        [DataMember, XmlAttribute]

        public HndzLoadPattern Pattern { get; set; }


        public HndzLoad(string name, string description, HndzLoadDirectionEnum loadDirection, HndzLoadPattern loadPattern) :
            base(name, description)
        {
            Direction = loadDirection;
            Pattern = loadPattern;
        }

        public HndzLoad(HndzLoadDirectionEnum loadDirection, HndzLoadPattern loadPattern) :
            this(HndzResources.DefaultName, HndzResources.DefaultDescription, loadDirection, loadPattern)
        {
        }
    }
}
