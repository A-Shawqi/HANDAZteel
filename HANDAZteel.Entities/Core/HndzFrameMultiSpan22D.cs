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
    public class HndzFrameMultiSpan22D : HndzFrame2D
    {
        public HndzFrameMultiSpan22D(string name, string description, HndzColumn rightColumn, HndzSupport rightSupport, HndzColumn middleColumnLeft, 
            HndzColumn middleColumnRight, HndzColumn leftColumn, HndzSupport leftSupport, HndzBeam leftBeam, 
            HndzBeam rightBeam, HndzBeam middleBeamLeft, HndzBeam middleBeamRight, HndzStorey storey = null) : base(name, description, storey)
        {
            RightColumn = rightColumn;
            RightSupport = rightSupport;
            MiddleColumnLeft = middleColumnLeft;
            MiddleColumnRight = middleColumnRight;
            LeftColumn = leftColumn;
            LeftSupport = leftSupport;
            LeftBeam = leftBeam;
            RightBeam = rightBeam;
            MiddleBeamLeft = middleBeamLeft;
            MiddleBeamRight = middleBeamRight;
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
        public HndzColumn MiddleColumnRight { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport MiddleSupportRight { get; set; }
        [DataMember, XmlAttribute]
        public HndzColumn LeftColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport LeftSupport { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam LeftBeam { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam RightBeam { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam MiddleBeamLeft { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam MiddleBeamRight { get; set; }
        #endregion
    }
}