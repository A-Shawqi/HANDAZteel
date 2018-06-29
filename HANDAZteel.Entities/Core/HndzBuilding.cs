using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    [DataContract(IsReference =true)]  [Serializable]  [XmlSerializerFormat]
    public class HndzBuilding : HndzRoot
    {
        #region Properties
       [DataMember, XmlAttribute]
        public String Address { get; set; }
       [DataMember, XmlAttribute]

        public Double RefHeight { get; set; }
       [DataMember]
        [XmlArray("Stories")]
        public ICollection<HndzStorey> Stories { get; set; }
       [DataMember, XmlAttribute]

        public HndzProject Project { get; set; }

        #endregion
        #region Constructors
        public HndzBuilding(String name, String description, String address, HndzProject project = null,
                        ICollection<HndzStorey> stories = null, Double refHeight = 0) : base(name, description)
        {
            Project = project;
            Address = address;
            RefHeight = refHeight;
            Stories = stories;
            AddToAssociatedProject();
        }
        public HndzBuilding(HndzProject project) : this(HndzResources.DefaultName, HndzResources.DefaultDescription,
                                                       HndzResources.DefaultAddress, project)
        {
        }
        public HndzBuilding() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, HndzResources.DefaultAddress)
        {
        }
        #endregion

        #region methods
        private void AddToAssociatedProject()
        {
            if (Project != null)
            {
                if (Project.Buildings != null)
                {
                    Project.Buildings.Add(this);
                }
                else
                {
                    Project.Buildings = new List<HndzBuilding> { this };
                }
            }

        }
        #endregion
    }
}