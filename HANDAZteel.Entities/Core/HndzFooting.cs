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
    /// Represents Structural Footing 
    /// </summary>
    /// 
    [DataContract]
    public class HndzFooting : HndzStructuralElement
    {
        #region properties

        [DataMember]
        public Double Thickness { get; set; }
        #endregion

        #region Constructors

        public HndzFooting(String name, String description, HndzProfile profile, HndzLine extrusionLine, HndzStorey storey = null, Double baseOffset = 0) :
                                    base(name, description, extrusionLine, profile, storey, baseOffset)
        {
            Profile = profile;
        }

        public HndzFooting(HndzLine extrusionLine, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription, profile, extrusionLine, storey, baseOffset)
        {
        }
        public HndzFooting(Double thickness, HndzProfile profile, HndzStorey storey, Double baseOffset = 0) :
                this(null, profile, storey, baseOffset)
        {
            Thickness = thickness;
            ExtrusionLine = new HndzLine(new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation -
                                         Thickness + baseOffset),
                              new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation + baseOffset));
        }

        public HndzFooting() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, null)
        {
        }



        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Footing";
        #endregion


    }
}
