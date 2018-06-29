using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzCity : HndzRoot
    {
        #region Properties
       [DataMember, XmlAttribute]
        public HndzWeather WeatherSpecs { get; set; }
       [DataMember, XmlAttribute]
        public Double[] LatLng { get; set; }


        #endregion

        #region Constructors
        public HndzCity()
        {
        }
        public HndzCity(Guid globalId, String name, String description)
        {
            Name = name;
            Description = description;
        }
        public HndzCity(Guid globalId, String name, String description, HndzWeather weatherSpecs, 
                    Double[] latLng)
        {
            Name = name;
            Description = description;
            WeatherSpecs = weatherSpecs;
            LatLng = latLng;
        }
        #endregion

        #region Method

        #endregion

    }
}