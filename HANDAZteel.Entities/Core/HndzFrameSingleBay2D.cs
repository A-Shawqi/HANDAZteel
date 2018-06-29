using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    /// <summary>
    /// Represents Steel Single Bay Spacing Frame
    /// </summary>
    ///
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzFrameSingleBay2D : HndzFrame2D
    {
       [DataMember, XmlAttribute]
        #region Properties
        public HndzColumn RightColumn { get; set; }
       [DataMember, XmlAttribute]
        public HndzBeam RightBeam { get; set; }
       [DataMember, XmlAttribute]
        public HndzSupport RightSupport { get; set; }
       [DataMember, XmlAttribute]
        public HndzColumn LeftColumn { get; set; }
       [DataMember, XmlAttribute]
        public HndzBeam LeftBeam { get; set; }
       [DataMember, XmlAttribute]
        public HndzSupport LeftSupport { get; set; }
        #endregion
        #region Constructors
        public HndzFrameSingleBay2D(string name, string description, HndzColumn rightColumn, HndzColumn leftColumn,
                HndzBeam rightBeam, HndzBeam leftBeam, HndzSupport rightSupport, HndzSupport leftSupport, HndzStorey storey = null) : base(name, description, storey)
        {
            RightColumn = rightColumn;
            RightBeam = rightBeam;
            LeftColumn = leftColumn;
            LeftBeam = leftBeam;
            RightSupport = rightSupport;
            LeftSupport = leftSupport;
        }
        public HndzFrameSingleBay2D(HndzColumn rightColumn, HndzBeam rightBeam, HndzColumn leftColumn, HndzBeam leftBeam,
                                        HndzSupport rightSupport, HndzSupport leftSupport) :
                        this(HndzResources.DefaultName, HndzResources.DefaultDescription, rightColumn, leftColumn, rightBeam,
                                leftBeam, rightSupport, leftSupport)
        {
        }

        public HndzFrameSingleBay2D() :
                       this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, null, null,
                               null, null, null)
        {
        }
        #endregion
    }
}
