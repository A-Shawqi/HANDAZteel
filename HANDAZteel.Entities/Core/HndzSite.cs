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
    public class HndzSite:HndzRoot
    {
        #region Properties
       [DataMember, XmlAttribute]
        public HndzProfile BoundaryCurve { get; set; }
       [DataMember, XmlAttribute]
        public HndzProject Project { get; set; }
        #endregion

        #region Methods
        private void AddToAssociatedProject()
        {
            Project.Site = this;
        }
        #endregion
    }
}
