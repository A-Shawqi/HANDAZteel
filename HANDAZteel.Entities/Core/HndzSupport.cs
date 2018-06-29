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
   public class HndzSupport:HndzRoot
    {

        static uint Id;
        #region Properties
       [DataMember, XmlAttribute]
        public uint LocalId { get; set; }
       [DataMember, XmlAttribute]
        public HndzNode Location { get; set; }
       [DataMember, XmlAttribute]
        public HndzSupportTypeEnum SupportType { get; set; }
        public HndzJointAnalysisResults[] AnalysisResults { get; set; }
        public HndzJointAnalysisResults[] AnalysisResultsEnvelope { get; set; }

        #endregion

        #region Constructors
        public HndzSupport(string name,string description, HndzSupportTypeEnum supportType, HndzNode location=null):
                          base(name,description)
        {
            LocalId = ++Id;
            SupportType = supportType;
            Location = location;
        }
        public HndzSupport( HndzSupportTypeEnum supportType, HndzNode location = null) :
                         this(HndzResources.DefaultName, HndzResources.DefaultDescription,HndzSupportTypeEnum.Pinned)
        {
        }
        #endregion
    }
}
