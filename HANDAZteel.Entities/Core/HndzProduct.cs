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
    [KnownType(typeof(HndzFrame2D))]
    [KnownType(typeof(HndzFrameSingleBay2D))]
    [KnownType(typeof(HndzFrameMonoSlope2D))]
    [KnownType(typeof(HndzFrameMultiGable2D))]
    [KnownType(typeof(HndzFrameMultiSpan12D))]
    [KnownType(typeof(HndzFrameMultiSpan22D))]
    [KnownType(typeof(HndzFrameMultiSpan32D))]
    [KnownType(typeof(HndzBuildingElement))]

    public abstract class HndzProduct : HndzRoot
    {
       [DataMember, XmlAttribute]
        public HndzStorey BuildingStorey { get; set; }
        #region Constructors

        public HndzProduct(string name, string description,HndzStorey storey ) : base(name, description)
        {
            BuildingStorey = storey;
            AddToAssociatedStorey();
        }
        public HndzProduct(HndzStorey storey) : this(HndzResources.DefaultName, HndzResources.DefaultDescription,storey)
        {
        }
        public HndzProduct() : this(HndzResources.DefaultName, HndzResources.DefaultDescription,null)
        {
        }
        #endregion

        #region Method
        //private void AddAssociatedBuilding(HndzBuilding building)
        //{
        //    this.BuildingStorey.Building = building;
        //}
        private void AddToAssociatedStorey()
        {
            if (BuildingStorey != null)
            {

                if (BuildingStorey.HndzProducts != null)
                {
                    BuildingStorey.HndzProducts.Add(this);
                }
                else
                {
                    BuildingStorey.HndzProducts = new List<HndzProduct> { this };
                }
            }

        }
        #endregion
    }
}