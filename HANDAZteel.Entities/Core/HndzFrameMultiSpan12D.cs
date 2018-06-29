using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]
    [Serializable]
    [XmlSerializerFormat]
    public class HndzFrameMultiSpan12D : HndzFrame2D
    {
        public HndzFrameMultiSpan12D(string name, string description, HndzColumn rightColumn, HndzSupport rightSupport, 
            HndzColumn middleColumn, HndzColumn leftColumn, HndzSupport leftSupport, HndzBeam leftBeam, HndzBeam rightBeam, HndzStorey storey = null) : base(name, description, storey)
        {
            RightColumn = rightColumn;
            RightSupport = rightSupport;
            MiddleColumn = middleColumn;
            LeftColumn = leftColumn;
            LeftSupport = leftSupport;
            LeftBeam = leftBeam;
            RightBeam = rightBeam;
        }
        #region Properties
        [DataMember, XmlAttribute]
        public HndzColumn RightColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport RightSupport { get; set; }
        [DataMember, XmlAttribute]
        public HndzColumn MiddleColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport MiddleSupport { get; set; }
        [DataMember, XmlAttribute]
        public HndzColumn LeftColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport LeftSupport { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam LeftBeam { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam RightBeam { get; set; }
        #endregion
    }
}