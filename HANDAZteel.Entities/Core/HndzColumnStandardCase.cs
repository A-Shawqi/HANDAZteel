using Rhino.Geometry;
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
    /// Represents Structural Column in the standard case (not tapered) with a generic profile
    /// </summary>
    /// 
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzColumnStandardCase : HndzColumn
    {
        #region properties
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor takes extrusion line and base storey
        /// </summary>
        /// <param name="name">Column Name</param>
        /// <param name="description">Column Description</param>
        /// <param name="extrusionLine">line represents start and end point of the column and its direction</param>
        /// <param name="storey">Base Storey</param>
        /// <param name="baseOffset">Base Offset from base storey Elevation value</param>
        /// <param name="discipline">Discipline as Enum e.g. Arch, Str ... etc</param>
        /// <param name="profile">Column Profile</param>
        public HndzColumnStandardCase(String name, String description, HndzProfile profile , HndzLine extrusionLine, HndzStorey storey = null, Double baseOffset = 0,
                                    HndzProductDiscipline discipline = HndzProductDiscipline.Structural) :
                                    base(name, description,extrusionLine,profile, storey, baseOffset,  discipline)
        {
        }
        /// <summary>
        /// Constructor takes extrusion line and base storey
        /// </summary>
        /// <param name="extrusionLine">line represents base point of the column and end point represents where column extrusion will end </param>
        /// <param name="storey">Base Storey</param>
        /// <param name="baseOffset">Base Offset from base storey Elevation value</param>
        /// <param name="discipline">Discipline as Enum e.g. Arch, Str ... etc</param>
        /// <param name="profile">Column Profile</param>
        public HndzColumnStandardCase( HndzLine extrusionLine, HndzProfile profile , HndzStorey storey = null, Double baseOffset = 0,
                                    HndzProductDiscipline discipline = HndzProductDiscipline.Structural) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription,profile, extrusionLine, storey, baseOffset, discipline)
        {
        }

        //ToDo: where the column location between the two bounding stories
        /// <summary>
        /// Constructor that take base storey and top storey and calculate the height automatically
        /// </summary>
        /// <param name="name">Column Name</param>
        /// <param name="description">Column Description</param>
        /// <param name="baseStorey">Base Storey</param>
        /// <param name="topStorey">Top Storey</param>
        /// <param name="baseOffset">Base Offset from base storey Elevation value</param>
        /// <param name="topOffset">Top Offset from top storey Elevation value</param>
        /// <param name="dir">Extrusion Direction... set it to Vector3d.ZAxis if the column is vertical</param>
        /// <param name="discipline">Discipline as Enum e.g. Arch, Str ... etc</param>
        /// <param name="profile">Column Profile</param>
        //public HndzColumnStandardCase(String name, String description, HndzStorey baseStorey, HndzStorey topStorey,
        //                            Double baseOffset = 0, Double topOffset = 0, Vector3d dir = default(Vector3d),
        //                            HndzProductDiscipline discipline = HndzProductDiscipline.Structural, HndzProfile profile = null) :
        //                            base(name, description, baseStorey, baseOffset, 0, dir, discipline)
        //{
        //    Profile = profile;
        //}

        /// <summary>
        /// Empty Constructor initalizes all properties to default values
        /// </summary>
        public HndzColumnStandardCase() : this(HndzResources.DefaultName, HndzResources.DefaultDescription,null,null)
        {
        }



        #endregion



    }
}
