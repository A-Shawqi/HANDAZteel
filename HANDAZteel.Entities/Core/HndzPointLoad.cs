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
   public class HndzPointLoad: HndzLoad
    {

        public double Distance { get; set; }
        public double Value { get; set; }
  

        public HndzPointLoad(string name, string description, double distance, double value, HndzLoadDirectionEnum direction, 
            HndzLoadPattern pattern) :base(name, description,direction,pattern)
        {
            Distance = distance;
            Value = value;
            Direction = direction;
            Pattern = pattern;
        }
        public HndzPointLoad(double distance, double value, HndzLoadDirectionEnum direction,
           HndzLoadPattern pattern) : this(HndzResources.DefaultName, HndzResources.DefaultDescription,distance,value, direction, pattern)
        {
        }
    }
}
