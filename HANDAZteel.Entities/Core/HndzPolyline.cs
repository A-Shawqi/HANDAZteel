using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using DXF = netDxf.Entities;
using netDxf;

namespace HANDAZ.Entities
{
    [DataContract]
    public class HndzPolyline : HndzRoot
    {
        [DataMember]
        public DXF.LwPolyline DxfLwPolyline { get; set; }
        private Polyline approxRhPolyline;
        
        [DataMember]
        public Polyline ApproxRhPolyline
        {
            get
            {
                return GetApproxRhinoPolyline();
            }
            set { approxRhPolyline = value; }
        }

        private Point3d centroid;

        [DataMember]
        public Point3d Centroid
        {
            get
            {
                if (ApproxRhPolyline != null)
                {
                    return ApproxRhPolyline.CenterPoint();
                }
                else
                {
                    return default(Point3d);
                }
            }
            set { centroid = value; }
        }

        #region Constructors

        public HndzPolyline(string name, string description, DXF.LwPolyline dxfLwPolyline) : base(name, description)
        {
            DxfLwPolyline = dxfLwPolyline;
        }
        public HndzPolyline(DXF.LwPolyline dxfLwPolyline) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, dxfLwPolyline)
        {
        }
        public HndzPolyline() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null)
        {
        }
        #endregion
        #region Methods
        /// <summary>
        /// Converts dxf lwpolyline into rhino polyline. This method will approximate arcs to small lines. 
        /// TODO: check magic numbers (int precision=36, double lengthTolerance=.001)
        /// </summary>
        /// <param name="precision"></param>
        /// <param name="lengthTolerance"></param>
        /// <returns></returns>
        private Polyline GetApproxRhinoPolyline(int precision = 16, double lengthTolerance = .001)
        {
            if (DxfLwPolyline != null)
            {
                IList<Vector2> vertexes = DxfLwPolyline.PolygonalVertexes(precision, lengthTolerance, .001);
                List<Point3d> RhPlVertexes = new List<Point3d>();

                Polyline pl = new Rhino.Geometry.Polyline();

                foreach (var item in vertexes)
                {
                    RhPlVertexes.Add(new Point3d(item.X, item.Y, 0));
                }
                pl = new Polyline(RhPlVertexes);
                return pl;
            }
            else
            {
                return null;
            }
        }
        #endregion


    }
}
