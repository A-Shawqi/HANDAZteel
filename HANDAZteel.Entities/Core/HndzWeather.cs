using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzWeather
    {
       [DataMember, XmlAttribute]
        public double WindSpeed { get; set; }
        public HndzWeather()
        {

        }
        
        //TODO
    }
}