using Rhino.Geometry;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzNode : HndzRoot
    {
       [DataMember, XmlAttribute]
        static uint Id;
       [DataMember, XmlAttribute]
        public Point3d Point { get; set; }
       [DataMember, XmlAttribute]
        public uint LocalId { get; set; }
        public HndzNodeReactions Reactions { get; set; }
        #region Constructors
        public HndzNode() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, 0, 0, 0)
        {
            LocalId = ++Id;
        }
        public HndzNode(Point3d point) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, point.X, point.Y, point.Z)
        {

            LocalId = ++Id;
            Point = point;
        }
        public HndzNode(double x, double y, double z) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, x, y, z)
        {
            LocalId = ++Id;
            Point = new Point3d(x, y, z);
        }
        public HndzNode(string name, string description, double x, double y, double z) : base(name, description)
        {
            LocalId = ++Id;
            Point = new Point3d(x, y, z);
        }
        #endregion
        public override string ToString()
        {
            return string.Format("{0} ,{1}, {2}",Point.X,Point.Y,Point.Z );
        }



    }
}