using Rhino.Geometry;
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
    [KnownType(typeof(HANDAZ.Entities.HndzExtrudedElement))]

    public abstract class HndzBuildingElement : HndzProduct
    {
        #region Properties
       [DataMember, XmlAttribute]
        public Double BaseOffset { get; set; }
       [DataMember, XmlAttribute]
        public HndzProductDiscipline Discipline { get; set; }
       [DataMember, XmlAttribute]
        public HndzMaterial Material { get; set; }
        #endregion
        #region Constructors
        public HndzBuildingElement(string name, string description, HndzStorey storey = null, Double baseOffset = 0,
                                   HndzProductDiscipline discipline = HndzProductDiscipline.None) :
                                   base(name, description, storey)
        {
            Discipline = discipline;
            BaseOffset = baseOffset;
            //AddToAssociatedStorey();
        }

        public HndzBuildingElement(HndzStorey storey, Double baseOffset, HndzProductDiscipline discipline) :
                                   this(HndzResources.DefaultName, HndzResources.DefaultDescription, storey, baseOffset)
        {
        }
        public HndzBuildingElement() : this(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
        }
        #endregion

        #region Methods
        //private void AddToAssociatedStorey()
        //{
        //    if (BuildingStorey != null)
        //    {

        //        if (BuildingStorey.HndzProducts != null)
        //        {
        //            BuildingStorey.HndzProducts.Add(this);
        //        }
        //        else
        //        {
        //            BuildingStorey.HndzProducts = new List<HndzProduct> { this };
        //        }
        //    }

        //}
        #endregion
    }
}
