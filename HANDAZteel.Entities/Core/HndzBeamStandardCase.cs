using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Rhino.Geometry;

namespace HANDAZ.Entities
{
    /// <summary>
    /// Represents Structural Beam in the standard case (not tapered) with a generic profile
    /// </summary>
    /// 
    [DataContract]
    public class HndzBeamStandardCase : HndzBeam
    {
        #region properties

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor takes extrusion line and base storey. To use this the building must have a storey above the current storey
        /// </summary>
        /// <param name="name">Beam Name</param>
        /// <param name="description">Beam Description</param>
        /// <param name="extrusionLine">line represents start and end point of the beam and its direction</param>
        /// <param name="storey">Base Storey</param>
        /// <param name="baseOffset">Base Offset from base storey Elevation value</param>
        /// <param name="discipline">Discipline as Enum e.g. Arch, Structure ... etc</param>
        /// <param name="profile">Beam Profile</param>
        public HndzBeamStandardCase(String name, String description, HndzLine extrusionLine, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                                    base(name, description, extrusionLine, profile, storey, baseOffset)
        {
            //to use this, the building must have a storey above the current storey
            double beamTopElevation = this.BuildingStorey.StoreyHeight;
            Point3d extStartPoint = new Point3d(extrusionLine.RhinoLine.From.X, extrusionLine.RhinoLine.From.Y, beamTopElevation);
            Point3d extEndPoint = new Point3d(extrusionLine.RhinoLine.To.X, extrusionLine.RhinoLine.To.Y, beamTopElevation);
            Line extLine = new Line(extStartPoint, extEndPoint);
            Profile = profile;
        }

        /// <summary>
        /// Constructor takes extrusion line and base storey
        /// </summary>
        /// <param name="extrusionLine">line represents base point of the beam and end point represents where beam extrusion will end </param>
        /// <param name="storey">Base Storey</param>
        /// <param name="baseOffset">Base Offset from base storey Elevation value</param>
        /// <param name="discipline">Discipline as Enum e.g. Arch, Str ... etc</param>
        /// <param name="profile">Beam Profile</param>
        public HndzBeamStandardCase(HndzLine extrusionLine, HndzProfile profile, HndzStorey storey=null, Double baseOffset = 0) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription, extrusionLine, profile, storey, baseOffset)
        {
        }
        /// <summary>
        /// Empty Constructor initializes all properties to default values
        /// </summary>
        public HndzBeamStandardCase() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, null)
        {
        }

        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Beam";
        #endregion

    }
}
