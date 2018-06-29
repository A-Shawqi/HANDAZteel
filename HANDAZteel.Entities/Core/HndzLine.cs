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
   public class HndzLine : HndzRoot
    {
       [DataMember, XmlAttribute]
        static uint Id;
       [DataMember, XmlAttribute]
        public HndzNode baseNode { get; set; }
       [DataMember, XmlAttribute]
        public HndzNode EndNode { get; set; }
       [DataMember, XmlAttribute]
        public uint LocalId { get; set; }
       [DataMember, XmlAttribute]
        public Line RhinoLine { get; set; }
        #region Constructors

        public HndzLine(string name, string description, double x1, double y1, double z1, double x2, double y2, double z2) : base(name, description)
        {
            LocalId = ++Id;
            baseNode = new HndzNode(x1, y1, z1);
            EndNode = new HndzNode(x2, y2, z2);
            RhinoLine = new Line(baseNode.Point, EndNode.Point);
        }
       
        public HndzLine(double x1, double y1, double z1, double x2, double y2, double z2) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, x1, y1, z1, x2, y2, z2)
        {
        }
        public HndzLine(Point3d startPoint, Point3d endPoint) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, startPoint.X, startPoint.Y, startPoint.Z, endPoint.X, endPoint.Y, endPoint.Z)
        {
        }
        public HndzLine(Line line) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, line.FromX, line.FromY, line.FromZ, line.ToX, line.ToY, line.ToZ)
        {
        }
        public HndzLine() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, 0, 0, 0, 0, 0, 0)
        {
        }
        #endregion
        #region Methods
       
        #endregion


    }
}
