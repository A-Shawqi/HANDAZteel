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
    [KnownType(typeof(HndzITaperedProfile))]
    [KnownType(typeof(HndzISectionProfile))]
    [KnownType(typeof(HndzCSectionProfile))]
    [KnownType(typeof(HndzRectangularProfile))]
    public abstract class HndzProfile : HndzRoot
    {
       [DataMember, XmlAttribute]
        public Vector2d OrientationInPlane { get; set; } //TODO: declare plane for profile shape


        //public Point3d Centroid { get; set; }
        protected HndzProfile( string name, string description, Vector2d orientation = default(Vector2d)) : base(name, description)
        {
            if (orientation == default(Vector2d))
            {
            OrientationInPlane = new Vector2d(1,0);
            }
            else
            {
                OrientationInPlane = orientation;
            }
        }
        protected HndzProfile() : this(HndzResources.DefaultName,HndzResources.DefaultDescription)
        {
        }
    }
}
