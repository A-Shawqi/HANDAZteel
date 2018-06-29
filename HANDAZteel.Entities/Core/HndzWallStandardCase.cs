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
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzWallStandardCase : HndzWall
    {
        #region properties
        [DataMember]
        public Line BaseLine { get; set; }
        #endregion

        #region Constructors

        public HndzWallStandardCase(String name, String description, HndzStorey storey, Double baseOffset = 0,
                                    Line baseLine = default(Line), Double unconnectedHeight = 0, Double wallThickness = 0, ICollection<HndzWallOpening> wallOpenings = null) :
                                    base(name, description, unconnectedHeight, wallThickness, storey, baseOffset, wallOpenings)
        {
            BaseLine = baseLine;
            Point3d StartPoint = new Point3d(BaseLine.PointAt(.5));
            Point3d EndPoint = new Point3d(StartPoint.X + BaseOffset, StartPoint.Y, unconnectedHeight);
            ExtrusionLine = new HndzLine(new Line(StartPoint, EndPoint));
        }

        public HndzWallStandardCase(HndzStorey storey, Double baseOffset = 0, Line baseLine = default(Line),
                                    Double unconnectedHeight = 0, Double wallThickness = 0, ICollection<HndzWallOpening> wallOpenings = null) :
                                    this(HndzResources.DefaultName, HndzResources.DefaultDescription,
                                    storey, baseOffset, baseLine, unconnectedHeight,
                                    wallThickness, wallOpenings)
        {
        }
        public HndzWallStandardCase() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null)
        {
        }
        #endregion


    }
}
