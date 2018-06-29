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
    public class HndzDistributedLoad : HndzLoad
    {
        public double StartDistance { get; set; }
        public double StartValue { get; set; }
        public double EndDistance { get; set; }
        public double EndValue { get; set; }


        public HndzDistributedLoad(string name, string description, double startDistance, double startValue, double endDistance,
            double endValue, HndzLoadDirectionEnum direction, HndzLoadPattern pattern) : base(name, description, direction, pattern)
        {
            StartDistance = startDistance;
            StartValue = startValue;
            EndDistance = endDistance;
            EndValue = endValue;
            Direction = direction;
            Pattern = pattern;
        }

        public HndzDistributedLoad(double startDistance, double startValue, double endDistance,
           double endValue, HndzLoadDirectionEnum direction, HndzLoadPattern pattern) : this(HndzResources.DefaultName, 
               HndzResources.DefaultDescription,startDistance,startValue,endDistance,endValue, direction, pattern)
        {
        }
    }
}
