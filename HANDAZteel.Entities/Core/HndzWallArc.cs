using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace HANDAZ.Entities
{
    [DataContract]
    public class HndzWallArc : HndzWall
    {
        #region properties
        public Arc BaseArc { get; set; }
        #endregion

        #region Constructors

        public HndzWallArc(String name, String description, HndzStorey storey, Double baseOffset = 0,
                           Arc baseArc = default(Arc), Double unconnectedHeight = 0, Double wallThickness = 0, 
                           ICollection<HndzWallOpening> wallOpenings = null) :
                           base(name, description, unconnectedHeight, wallThickness, storey, baseOffset, wallOpenings)
        {
            BaseArc = baseArc;
        }

        public HndzWallArc() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null)
        {
        }
        #endregion


    }
}
