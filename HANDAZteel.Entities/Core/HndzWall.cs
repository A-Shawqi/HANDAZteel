using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using System.Runtime.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]
    [KnownType(typeof(HndzWallStandardCase))]
    [KnownType(typeof(HndzWallArc))]
    public abstract class HndzWall : HndzArchitecturalElement
    {
        #region properties
        [DataMember]
        public Double WallThickness { get; set; }
        [DataMember]
        public Double UnconnectedHeight { get; set; }
        [DataMember]
        public ICollection<HndzWallOpening> WallOpenings { get; set; }

        #endregion
        #region Constructors
        protected HndzWall(string name, string description, HndzLine extrusionLine, Double wallThickness,
                            HndzStorey storey = null, double baseOffset = 0, ICollection<HndzWallOpening> wallOpenings = null) :
                            base(name, description, extrusionLine, storey, baseOffset)
        {
            if (ExtrusionLine != null)
            {
                UnconnectedHeight = ExtrusionLine.RhinoLine.Length;
            }
            WallThickness = wallThickness;
        }

        protected HndzWall(string name, string description, Double unconnectedHeight, Double wallThickness,
                            HndzStorey storey = null, double baseOffset = 0, ICollection<HndzWallOpening> wallOpenings = null) : 
                            this(name,description, new HndzLine(new Line(new Point3d(), new Vector3d(0, 0, unconnectedHeight))),
                                wallThickness,storey, baseOffset)
        {
        }

        protected HndzWall() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, 0, 0)
        {

        }

        #endregion
    }
}
