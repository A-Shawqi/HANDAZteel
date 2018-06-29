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
    /// Represents Structural steel purlin in the standard case (not tapered) with a generic profile
    /// </summary>
     [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzPurlinStandrdCase:HndzPurlin
    {
        #region properties
        #endregion

        #region Constructors
        /// <summary>
        /// main constructor
        /// </summary>
        /// <param name="profile">constant profile along column extrude direction</param>
        /// <param name="name">name for the column</param>
        /// <param name="description">description for the column</param>
        /// <param name="height">column actual height(in Z-direction)</param>
        /// <param name="storey">storey which contains the column base elevation</param>
        /// <param name="topLevel">storey which intersects with column top elevation</param>
        /// <param name="baseOffset">offset from base</param>
        /// <param name="topOffset">offset from top</param>
        public HndzPurlinStandrdCase( string name, string description, HndzProfile profile, HndzLine extrusionLine, HndzStorey storey = null,
            double baseOffset = 0) :
            base(name, description, extrusionLine,profile, storey,  baseOffset)
        {
            Profile = profile;
        }
        /// <summary>
        /// our default constructor
        /// </summary>
        /// <param name="profile">constant profile along column extrude direction</param>
        /// <param name="height">column actual height(in Z-direction)</param>
        /// <param name="storey">storey which contains the column base elevation</param>
        /// <param name="topLevel">storey which intersects with column top elevation</param>
        /// <param name="baseOffset">offset from base</param>
        /// <param name="topOffset">offset from top</param>
        public HndzPurlinStandrdCase(HndzProfile profile, HndzLine extrusionLine, HndzStorey storey = null, HndzStorey topLevel = null,
           double baseOffset = 0, double topOffset = 0) :
           this( HndzResources.DefaultName, HndzResources.DefaultDescription, profile, extrusionLine, storey, baseOffset)
        {
        }
        /// <summary>
        /// constructor on the fly
        /// </summary>
        public HndzPurlinStandrdCase() : this( HndzResources.DefaultName, HndzResources.DefaultDescription, null, null)
        {
        }

        #endregion
    }
}
