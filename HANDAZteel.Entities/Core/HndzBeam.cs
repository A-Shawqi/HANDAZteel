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
    [KnownType(typeof(HndzBeamStandrdCase))]
   public abstract class HndzBeam: HndzStructuralElement
    {
        #region properties

        #endregion
        #region Constructors
        protected HndzBeam(String name, String description, HndzLine extrusionLine,HndzProfile profile, HndzStorey storey=null,  double baseOffset=0):
            base(name,description, extrusionLine,profile, storey , baseOffset )
        {
        }
        protected HndzBeam() : this(HndzResources.DefaultName, HndzResources.DefaultDescription,null,null)
        {
        }
        #endregion
    }
}
