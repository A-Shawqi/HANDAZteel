using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]
    public class HndzWallArc : HndzWall
    {
        #region properties
        [DataMember, XmlAttribute]
        public Arc BaseArc { get; set; }
        #endregion

        #region Constructors

        public HndzWallArc(String name, String description, HndzStorey storey, Double baseOffset = 0,
                           Arc baseArc = default(Arc), Double unconnectedHeight = 0, Double wallThickness = 0, 
                           ICollection<HndzWallOpening> wallOpenings = null) :
                           base(name, description, unconnectedHeight, wallThickness, storey, baseOffset, wallOpenings)
        {
            //TODO: Logic for (Profile = hndzprofile)
            BaseArc = baseArc;
            Point3d mid = GetPointOnArc(BaseArc, .5);

            Point3d StartPoint = new Point3d(mid.X, mid.Y, storey.Elevation + BaseOffset);
            Point3d EndPoint = new Point3d(mid.X, mid.Y, storey.Elevation + unconnectedHeight + BaseOffset);

            //Point3d StartPoint = new Point3d(BaseArc.MidPoint.X, BaseArc.MidPoint.Y, BaseOffset);
            //Point3d EndPoint = new Point3d(StartPoint.X, StartPoint.Y, unconnectedHeight + BaseOffset);
            ExtrusionLine = new HndzLine(new Line(StartPoint, EndPoint));
        }
        public HndzWallArc(HndzStorey storey, Double baseOffset = 0,
                   Arc baseArc = default(Arc), Double unconnectedHeight = 0, Double wallThickness = 0,
                   ICollection<HndzWallOpening> wallOpenings = null) :
                   this(HndzResources.DefaultName,HndzResources.DefaultDescription,storey,baseOffset,baseArc, unconnectedHeight, wallThickness, wallOpenings)
        {
        }

        public HndzWallArc() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null)
        {
        }
        #endregion

        #region Method
        /// <summary>
        /// Get a point on arc
        /// </summary>
        /// <param name="rhArc">Rhino Arc</param>
        /// <param name="param">Enter a parameter from 0.0 to 1.0 (0.5 will return the midpoint of rhino arc) 
        /// (0.0 will return the start point) 
        /// (1.0 will return the endpoint)</param>
        /// <returns></returns>
        public static Point3d GetPointOnArc(Arc rhArc, double param)
        {
            double r = rhArc.Radius;
            double angleAtParam = rhArc.StartAngle + (Math.Abs((2.0 * Math.PI) - rhArc.StartAngle + rhArc.EndAngle) % (2.0*Math.PI)) * param;
            double xAtParam = r * Math.Cos(angleAtParam) + rhArc.Center.X;
            double yAtParam = r * Math.Sin(angleAtParam) + rhArc.Center.Y;
            Point3d pointAtParam = new Point3d(xAtParam, yAtParam, 0);
            return pointAtParam;
        }
        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Arched Wall";
        #endregion

    }
}
