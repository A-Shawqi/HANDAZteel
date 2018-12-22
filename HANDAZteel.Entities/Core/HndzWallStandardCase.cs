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
                                    Line baseLine = default(Line), Double unconnectedHeight=0, Double wallThickness = 0, ICollection<HndzWallOpening> wallOpenings = null) : 
                                    base(name,description, unconnectedHeight,wallThickness,storey,baseOffset, wallOpenings)
        {
            BaseLine = baseLine;
            //MS:
            //Step1: Create HndzProfile
            var myWallRec = new Rectangle3d(Plane.WorldXY, wallThickness, BaseLine.Length);
            var orientationVector = new Vector2d(BaseLine.UnitTangent.X, BaseLine.UnitTangent.Y);
            var myHndzProfile = new HndzRectangularProfile(myWallRec, orientationVector);
            Profile = myHndzProfile;

            //Step2: Create Extrusion Line
            Point3d StartPoint = new Point3d(BaseLine.PointAt(.5).X, BaseLine.PointAt(.5).Y,BaseOffset + storey.Elevation);
            Point3d EndPoint = new Point3d(StartPoint.X , StartPoint.Y, unconnectedHeight + BaseOffset + storey.Elevation);
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

        #region Overridden Methods
        public override string ToString() => "Hndz-Wall";
        #endregion

    }
}
