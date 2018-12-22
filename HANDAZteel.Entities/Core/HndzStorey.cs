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
        private Double storeyHeight;


        /// <summary>
        /// Height from current storey to the next upper storey. Works only when there at least one storey above the current storey
        /// </summary>
        [DataMember]
        public Double StoreyHeight
        {
            get
            {
                //Double myStoreyHeight = double.MaxValue;
                //foreach (var item in this.Building.Stories)
                //{
                //    if ((item.Elevation - this.Elevation) > 0 && (item.Elevation - this.Elevation) < myStoreyHeight)
                //    {
                //        myStoreyHeight = item.Elevation - this.Elevation;
                //    }
                //}
                //storeyHeight = myStoreyHeight;
                //if (myStoreyHeight == double.MaxValue)
                //{
                //    //TODO: unit conversion
                //    return 4000.0;
                //}
                return storeyHeight;
            }
            set { storeyHeight = value; }
        }

        [DataMember]
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

        //to use this the building must have a storey above the current storey
        //cannot use with roof storey
        public Double? GetStoreyHeight()
        {
            //HndzStorey topStorey;
            Double myStoreyHeight = double.MaxValue;
            foreach (var item in this.Building.Stories)
            {
                if ((item.Elevation - this.Elevation) > 0 && (item.Elevation - this.Elevation) < myStoreyHeight)
                {
                    myStoreyHeight = item.Elevation - this.Elevation;
                }
            }
            if (myStoreyHeight == double.MaxValue)
            {
                return null;
            }
            return myStoreyHeight;
        }
        public HndzStorey GetNextStorey()
        {
            //HndzStorey topStorey;
            Double myStoreyHeight = double.MaxValue;
            int storeyIndex = -1;
            for (int i = 0; i < this.Building.Stories.Count(); i++)
            {
                if ((this.Building.Stories.ElementAt(i).Elevation - this.Elevation) > 0 && 
                    (this.Building.Stories.ElementAt(i).Elevation - this.Elevation) < myStoreyHeight)
                {
                    myStoreyHeight = this.Building.Stories.ElementAt(i).Elevation - this.Elevation;
                    storeyIndex = i;
                }
            }
            if (storeyIndex != -1)
            {
                return this.Building.Stories.ElementAt(storeyIndex);
            }
            return null;
        }
        #endregion
    }
}