using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    /// <summary>
    /// Represents Structural Beam in the standard case (not tapered) with a generic profile
    /// </summary>
    /// 
    [DataContract]
    //[Serializable]
    //[XmlSerializerFormat]
    public class HndzBeamStandrdCase : HndzBeam 
    {
        #region properties
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor takes extrusion line and base storey
        /// </summary>
        /// <param name="name">Beam Name</param>
        /// <param name="description">Beam Description</param>
        /// <param name="extrusionLine">line represents start and end point of the beam and its direction</param>
        /// <param name="storey">Base Storey</param>
        /// <param name="baseOffset">Base Offset from base storey Elevation value</param>
        /// <param name="discipline">Discipline as Enum e.g. Arch, Structure ... etc</param>
        /// <param name="profile">Beam Profile</param>
        public HndzBeamStandrdCase(String name, String description, HndzLine extrusionLine, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                                    base(name, description,extrusionLine,profile, storey, baseOffset)
        {
        }
        /// <summary>
        /// Constructor takes extrusion line and base storey
        /// </summary>
        /// <param name="extrusionLine">line represents base point of the beam and end point represents where beam extrusion will end </param>
        /// <param name="storey">Base Storey</param>
        /// <param name="baseOffset">Base Offset from base storey Elevation value</param>
        /// <param name="discipline">Discipline as Enum e.g. Arch, Str ... etc</param>
        /// <param name="profile">Beam Profile</param>
        public HndzBeamStandrdCase(HndzLine extrusionLine, HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription, extrusionLine, profile,  storey, baseOffset)
        {
        }

        /// <summary>
        /// Empty Constructor initializes all properties to default values
        /// </summary>
        public HndzBeamStandrdCase() : this(HndzResources.DefaultName, HndzResources.DefaultDescription,null,null)
        {
        }



        #endregion



    }
}
