using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]
    [Serializable]
    [XmlSerializerFormat]
    [KnownType(typeof(HANDAZ.Entities.HndzArchitecturalElement))]
    [KnownType(typeof(HANDAZ.Entities.HndzStructuralElement))]
    public abstract class HndzExtrudedElement : HndzBuildingElement
    {
        [DataMember, XmlAttribute]
        private static uint Id;
        #region Properties
        [DataMember, XmlAttribute]
        public uint LocalId { get; private set; }
        [DataMember]
        public ICollection<HndzOpening> Openings { get; set; }

        [DataMember, XmlAttribute]
        private HndzLine extrusionLine;

        [DataMember, XmlAttribute]
        public HndzProfile Profile { get; set; }

        public HndzLine ExtrusionLine
        {
            get => extrusionLine;
            set => extrusionLine = value;
        }
        [DataMember, XmlAttribute]
        public HndzAnalysisResults[] AnalysisResults { get; set; }
        public HndzAnalysisResults[] AnalysisResultsEnvelope { get; set; }

        #endregion
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="description">Description</param>
        /// <param name="extrusionLine">Extruded Line as HndzLine</param>
        /// <param name="storey">Base Storey</param>
        /// <param name="baseOffset">Base Offset</param>
        /// <param name="discipline">Arch or Str</param>
        protected HndzExtrudedElement(string name, string description, HndzProfile profile, HndzLine extrusionLine, HndzStorey storey, double baseOffset,
                                       HndzProductDiscipline discipline) :
                                base(name, description, storey, baseOffset, discipline)
        {
            //LocalId = ++Id; MS: Commented because of wrong placement
            //ExtrusionDirection = dir;
            //ExtrusionLength = extrusionLength;
            ExtrusionLine = extrusionLine;
            Profile = profile;
        }
        protected HndzExtrudedElement(string name, string description, HndzLine extrusionLine, HndzStorey storey, double baseOffset,
                               HndzProductDiscipline discipline) :
                        base(name, description, storey, baseOffset, discipline)
        {
            LocalId = ++Id;
            //ExtrusionDirection = dir;
            //ExtrusionLength = extrusionLength;
            ExtrusionLine = extrusionLine;
            //profile = MyHndzProfile;
        }
        protected HndzExtrudedElement() :
                               base(HndzResources.DefaultName, HndzResources.DefaultDescription, null, 0, HndzProductDiscipline.Structural)
        {
        }
        #endregion
    }
}
