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
    public class HndzStairLanding : HndzSlab
    {

        #region Properties

        #endregion

        #region Constructors
        public HndzStairLanding(String name, String description, HndzProfile profile, HndzLine extrusionLine, HndzStorey storey = null, Double baseOffset = 0) :
                                    base(name, description, profile, extrusionLine,  storey, baseOffset)
        {
        }

        public HndzStairLanding(HndzLine extrusionLine, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription, profile, extrusionLine, storey, baseOffset)
        {
        }
        public HndzStairLanding(Double thickness, HndzProfile profile, HndzStorey storey, Double finishFloorThickness, Double baseOffset = 0) :
                this(null, profile, storey, baseOffset)
        {
            SlabThickness = thickness;
            ExtrusionLine = new HndzLine(new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation -
                                         SlabThickness - finishFloorThickness + baseOffset),
                              new Point3d(profile.Centroid.X, profile.Centroid.Y, storey.Elevation - finishFloorThickness + baseOffset));
        }

        public HndzStairLanding() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, null)
        {
        }

        #endregion

        #region Methods
        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-StairLanding";
        #endregion
    }
}
