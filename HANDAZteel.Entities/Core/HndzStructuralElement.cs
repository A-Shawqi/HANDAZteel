using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzColumn))]
    [KnownType(typeof(HndzBeam))]
    [KnownType(typeof(HndzPurlin))]
    [KnownType(typeof(HndzGuirt))]
    public abstract class HndzStructuralElement: HndzExtrudedElement
    {
        #region Properties
       [DataMember, XmlAttribute]
        public HndzProfile Profile { get; set; }
        public bool? IsDesignPassed { get; set; }
        #endregion
        #region Constructors
        protected HndzStructuralElement(string name,string description,HndzLine extrusionLine, HndzProfile profile, HndzStorey storey=null,double baseOffset=0) : 
            base(name, description, extrusionLine, storey, baseOffset, HndzProductDiscipline.Structural)
        {
            Profile = profile;
        }
        public HndzStructuralElement() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null,null)
        {

        }
        #endregion
    }
}
