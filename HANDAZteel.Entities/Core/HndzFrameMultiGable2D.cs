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
    public class HndzFrameMultiGable2D : HndzFrame2D
    {
        public HndzFrameMultiGable2D(string name, string description, HndzColumn rightColumn, HndzSupport rightSupport, HndzColumn middleColumn, HndzColumn leftColumn, HndzSupport leftSupport, HndzBeam leftBeamLeft,
            HndzBeam rightBeamLeft, HndzBeam leftBeamRight, HndzBeam rightBeamRight, HndzStorey storey = null) : base(name, description, storey)
        {
            RightColumn = rightColumn;
            RightSupport = rightSupport;
            MiddleColumn = middleColumn;
            LeftColumn = leftColumn;
            LeftSupport = leftSupport;
            LeftBeamLeft = leftBeamLeft;
            RightBeamLeft = rightBeamLeft;
            LeftBeamRight = leftBeamRight;
            RightBeamRight = rightBeamRight;
        }
        #region Properties
        [DataMember, XmlAttribute]        
        public HndzColumn RightColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport RightSupport { get; set; }
        public HndzColumn MiddleColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport MiddleSupport { get; set; }
        [DataMember, XmlAttribute]
        public HndzColumn LeftColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport LeftSupport { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam LeftBeamLeft { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam RightBeamLeft { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam LeftBeamRight { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam RightBeamRight { get; set; }
#endregion
    }
}