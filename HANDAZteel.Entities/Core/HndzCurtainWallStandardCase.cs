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
    public class HndzCurtainWallStandardCase : HndzWall
    {
        #region properties
        [DataMember]
        public Line BaseLine { get; set; }
        #endregion

        #region Constructors

        public HndzCurtainWallStandardCase(String name, String description, HndzStorey storey, Double baseOffset = 0, 
                                    Line baseLine = default(Line), Double unconnectedHeight=0, Double wallThickness = 0, ICollection<HndzWallOpening> wallOpenings = null) : 
                                    base(name,description, unconnectedHeight,wallThickness,storey,baseOffset, wallOpenings)
        {
            BaseLine = baseLine;
            Point3d StartPoint = new Point3d(BaseLine.PointAt(.5).X, BaseLine.PointAt(.5).Y,BaseOffset);
            Point3d EndPoint = new Point3d(StartPoint.X , StartPoint.Y, unconnectedHeight + BaseOffset);
            ExtrusionLine = new HndzLine(new Line(StartPoint, EndPoint));
        }

        public HndzCurtainWallStandardCase(HndzStorey storey, Double baseOffset = 0,Line baseLine = default(Line), 
                                    Double unconnectedHeight = 0, Double wallThickness = 0, ICollection<HndzWallOpening> wallOpenings = null) : 
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription, 
                                    storey, baseOffset, baseLine, unconnectedHeight,
                                    wallThickness, wallOpenings)
        {
        }
        public HndzCurtainWallStandardCase() : this(HndzResources.DefaultName,HndzResources.DefaultDescription,null)
        {
        }
        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Curtain Wall";
        #endregion

    }
}
