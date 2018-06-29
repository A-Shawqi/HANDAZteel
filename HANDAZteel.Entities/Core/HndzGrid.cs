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
    public class HndzGrid:HndzRoot
    {
        static int Id;
        #region Props
       [DataMember, XmlAttribute]
        public int LocalId { get; set; }
       [DataMember, XmlAttribute]
        public HndzNode StartNode { get; set; }
       [DataMember, XmlAttribute]
        public HndzNode EndNode { get; set; }
        #endregion
        #region Constructors
        public HndzGrid() : base() { }
        public HndzGrid(HndzNode startNode, HndzNode endNode) : base()
        {
            LocalId = ++Id;
            StartNode = startNode;
            EndNode = endNode;
        }
        public HndzGrid(double x1,double y1,double z1, double x2, double y2, double z2) : base()
        {
            LocalId = ++Id;
            StartNode = new HndzNode(x1,y1,z1);
            EndNode = new HndzNode(x2, y2, z2);
        }
        public HndzGrid(double x1, double y1, double z1, double x2, double y2, double z2,string name,string description) : base(name,description)
        {
            LocalId = ++Id;
            StartNode = new HndzNode(x1, y1, z1);
            EndNode = new HndzNode(x2, y2, z2);
        }

        #endregion
    }
}
