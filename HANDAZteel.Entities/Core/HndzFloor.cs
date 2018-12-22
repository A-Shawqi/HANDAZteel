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
    [DataContract]
    public class HndzFloor : HndzArchitecturalElement
    {
        #region properties

        [DataMember]
        public Double FloorThickness { get; set; }
        #endregion

        #region Constructors

        public HndzFloor(String name, String description, HndzProfile profile, HndzLine extrusionLine, HndzStorey storey = null, Double baseOffset = 0) :
                                    base(name, description, extrusionLine, storey, baseOffset)
        {
            Profile = profile;
        }

        public HndzFloor(HndzLine extrusionLine, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription, profile, extrusionLine, storey, baseOffset)
        {
        }
        public HndzFloor(Double thickness, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                this(null, profile, storey, baseOffset)
        {
            FloorThickness = thickness;
            ExtrusionLine = new HndzLine(new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation - FloorThickness + baseOffset),
                              new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation + baseOffset));
        }

        public HndzFloor() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, null)
        {
        }



        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Floor";
        #endregion


    }
}
