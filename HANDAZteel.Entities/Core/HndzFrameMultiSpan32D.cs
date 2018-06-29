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
    public class HndzFrameMultiSpan32D : HndzFrame2D
    {
        public HndzFrameMultiSpan32D(string name, string description, HndzColumn rightColumn, HndzSupport rightSupport, HndzColumn middleColumnLeft,
            HndzSupport middleSupportLeft, HndzColumn middleColumnMiddle, HndzSupport middleSupportMiddle,
            HndzColumn middleColumnRight, HndzSupport middleSupportRight, HndzColumn leftColumn, HndzSupport leftSupport,
            HndzBeam leftBeamLeft, HndzBeam rightBeamLeft, HndzBeam leftBeamRight, HndzBeam rightBeamRight, HndzStorey storey = null) : base(name, description, storey)
        {
            RightColumn = rightColumn;
            RightSupport = rightSupport;
            MiddleColumnLeft = middleColumnLeft;
            MiddleSupportLeft = middleSupportLeft;
            MiddleColumnMiddle = middleColumnMiddle;
            MiddleSupportMiddle = middleSupportMiddle;
            MiddleColumnRight = middleColumnRight;
            MiddleSupportRight = middleSupportRight;
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
        [DataMember, XmlAttribute]
        public HndzColumn MiddleColumnLeft { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport MiddleSupportLeft { get; set; }
        [DataMember, XmlAttribute]
        public HndzColumn MiddleColumnMiddle { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport MiddleSupportMiddle { get; set; }
        [DataMember, XmlAttribute]
        public HndzColumn MiddleColumnRight { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport MiddleSupportRight { get; set; }
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