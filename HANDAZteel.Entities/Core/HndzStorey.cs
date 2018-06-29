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
    public class HndzStorey : HndzRoot
    {
       [DataMember, XmlAttribute]
        #region Properties
        public Double Elevation { get; set; }
       [DataMember, XmlAttribute]
        public HndzBuilding Building { get; set; }
       [DataMember, XmlAttribute]
        [XmlArray("HndzProducts")]
        public ICollection<HndzProduct> HndzProducts { get; set; }
        #endregion
        #region Constructors
        public HndzStorey() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, 0)
        {
        }
        public HndzStorey(HndzBuilding buildingAssociated) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, buildingAssociated, 0)
        {
        }
        public HndzStorey(HndzBuilding buildingAssociated, Double elevation) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, buildingAssociated, elevation)
        {
        }
        public HndzStorey(String name, String description, HndzBuilding buildingAssociated, Double elevation) : base(name, description)
        {

            Name = name;
            Description = description;
            Elevation = elevation;
            Building = buildingAssociated;
            AddToAssociatedBuilding();
        }
        #endregion

        #region methods
        private void AddToAssociatedBuilding()
        {
            if (Building.Stories != null)
            {
                Building.Stories.Add(this);
            }
            else
            {
                Building.Stories = new List<HndzStorey> { this };
            }
        }
        #endregion
    }
}