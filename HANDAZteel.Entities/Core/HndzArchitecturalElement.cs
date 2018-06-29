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

    [KnownType(typeof(HndzWall))]
    public abstract class HndzArchitecturalElement: HndzExtrudedElement
    {
        #region Properties

        #endregion
        #region Constructors
        public HndzArchitecturalElement(string name, string description, HndzLine extrusionLine, HndzStorey storey=null, double baseOffset=0) :
            base(name, description, extrusionLine, storey, baseOffset, HndzProductDiscipline.Architectural)
        {

        }
        public HndzArchitecturalElement() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null)
        {

        }

        #endregion
    }
}
