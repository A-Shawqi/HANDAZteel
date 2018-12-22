using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.Entities
{
    [DataContract]
    public class HndzOpening : HndzExtrudedElement
    {
        #region Properties
        [DataMember]
        public HndzExtrudedElement RelatedProduct { get; set; }
        #endregion

        #region Constr
        public HndzOpening(string name, string description,HndzProfile profile = null, HndzLine extrusionLine = null, HndzExtrudedElement relatedProduct = null, Double baseOffset = 0) : base()
        {
            Name = name;
            Description = description;
            Profile = profile;
            ExtrusionLine = extrusionLine;
            RelatedProduct = relatedProduct;
            BaseOffset = baseOffset;
            if (RelatedProduct != null)
            {
                base.BuildingStorey = RelatedProduct.BuildingStorey;
            }
            AddToAssociatedProduct();
            base.AddToAssociatedStorey();
        }

        public HndzOpening(HndzProfile profile, HndzLine extrusionLine, HndzExtrudedElement relatedProduct = null, 
                           Double baseOffset = 0) :
                           this(HndzResources.DefaultName, HndzResources.DefaultDescription, profile, 
                           extrusionLine, relatedProduct, baseOffset)
        {

        }
        public HndzOpening() : this(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {

        }
        #endregion

        #region Methods
        private void AddToAssociatedProduct()
        {
            if (RelatedProduct != null)
            {
                if (RelatedProduct.Openings != null)
                {
                    RelatedProduct.Openings.Add(this);
                }
                else
                {
                    RelatedProduct.Openings = new List<HndzOpening> { this };
                }
            }
        }
        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Slab Opening";
        #endregion

    }
}
