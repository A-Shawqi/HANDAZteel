using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    [DataContract(IsReference = true)]
    [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzLine))]
    [KnownType(typeof(HndzBuilding))]
    [KnownType(typeof(HndzNode))]
    [KnownType(typeof(HndzProduct))]
    [KnownType(typeof(HndzSite))]
    [KnownType(typeof(HndzCity))]
    [KnownType(typeof(HndzRestraint))]
    [KnownType(typeof(HndzProfile))]
    [KnownType(typeof(HndzStorey))]
    [KnownType(typeof(HndzAnalysisResults))]
    [KnownType(typeof(HndzSupport))]
    [KnownType(typeof(HndzLoad))]
    [KnownType(typeof(HndzLoadDefinition))]
    [KnownType(typeof(HndzMaterial))]
    [KnownType(typeof(HndzProject))]
    public abstract class HndzRoot : IHndzRoot
    {
       [DataMember, XmlAttribute]
        private Guid globalId ;
       [DataMember, XmlAttribute]
        public string Description { get; set; }
       [DataMember, XmlAttribute]
        public Guid GlobalId
        {
            get { return globalId; }
            set { globalId = value; }
        }
       [DataMember, XmlAttribute]
        public string Name { get; set; }

        #region Constructors
        protected HndzRoot(string name, string description)
        {
            globalId = Guid.NewGuid();
            Name = name;
            Description = description;
        }
        protected HndzRoot():this(HndzResources.DefaultName,HndzResources.DefaultDescription)
        {
        }
        public HndzRoot(string name):this(name,HndzResources.DefaultDescription)
        {
        }
        #endregion

        #region Methods

        #endregion
    }
}
