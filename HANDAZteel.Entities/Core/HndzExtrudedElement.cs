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
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzStructuralElement))]
    [KnownType(typeof(HndzArchitecturalElement))]
    public abstract class HndzExtrudedElement : HndzBuildingElement
    {
       [DataMember, XmlAttribute]
        static uint Id;
        #region Properties
       [DataMember, XmlAttribute]
        public uint LocalId { get; private set; }
        //public Vector3d ExtrusionDirection { get; set; }
        //public Double ExtrusionLength { get; set; }
       [DataMember, XmlAttribute]
        public HndzLine ExtrusionLine { get; set; }
       [DataMember, XmlAttribute]
        public HndzAnalysisResults[] AnalysisResults { get; set; }
        public HndzAnalysisResults[] AnalysisResultsEnvelope { get; set; }

        #endregion
        #region Constructors
        protected HndzExtrudedElement(string name, string description, HndzLine extrusionLine, HndzStorey storey, double baseOffset,
                                       HndzProductDiscipline discipline) :
                                base(name, description, storey, baseOffset, discipline)
        {
            LocalId = ++Id;
            //ExtrusionDirection = dir;
            //ExtrusionLength = extrusionLength;
            ExtrusionLine = extrusionLine;
        }
        protected HndzExtrudedElement() :
                               base(HndzResources.DefaultName, HndzResources.DefaultDescription, null, 0, HndzProductDiscipline.Structural)
        {
        }
        #endregion
    }
}
