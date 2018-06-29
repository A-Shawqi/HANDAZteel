using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract][Serializable] [XmlSerializerFormat]
    public class HndzFrameMonoSlope2D : HndzFrame2D
    {
        public HndzFrameMonoSlope2D(string name, string description, HndzColumn rightColumn, HndzBeam beam, HndzSupport rightSupport, HndzColumn leftColumn, HndzSupport leftSupport, HndzStorey storey = null) : base(name, description, storey)
        {
            RightColumn = rightColumn;
            Beam = beam;
            RightSupport = rightSupport;
            LeftColumn = leftColumn;
            LeftSupport = leftSupport;
        }

        public HndzFrameMonoSlope2D()
        {
        }



        #region Properties
        public HndzColumn RightColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzBeam Beam { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport RightSupport { get; set; }
        [DataMember, XmlAttribute]
        public HndzColumn LeftColumn { get; set; }
        [DataMember, XmlAttribute]
        public HndzSupport LeftSupport { get; set; }
        
        #endregion
    }
}