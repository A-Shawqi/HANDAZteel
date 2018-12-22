using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace HANDAZ.Entities
{
    /// <summary>
    /// Represents Structural Column in the standard case (not tapered) with a generic profile 
    /// </summary>
    /// 
    [KnownType(typeof(HndzStairLanding))]
    [DataContract]
    public class HndzSlab : HndzStructuralElement
    {
        #region properties

        [DataMember]
        public Double SlabThickness { get; set; }
        #endregion

        #region Constructors

        public HndzSlab(String name, String description, HndzProfile profile, HndzLine extrusionLine, HndzStorey storey = null, Double baseOffset = 0) :
                                    base(name, description, extrusionLine, profile, storey, baseOffset)
        {
            Profile = profile;
        }

        public HndzSlab(HndzLine extrusionLine, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription, profile, extrusionLine, storey, baseOffset)
        {
        }
        public HndzSlab(Double thickness, HndzProfile profile, HndzStorey storey , Double finishFloorThickness, Double baseOffset = 0) :
                this(null, profile, storey, baseOffset)
        {
            SlabThickness = thickness;
            ExtrusionLine = new HndzLine(new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation - 
                                         SlabThickness - finishFloorThickness + baseOffset),
                              new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation - finishFloorThickness + baseOffset));
        }

        public HndzSlab() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, null)
        {
        }



        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Slab";
        #endregion


    }
}
