using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Rhino.Geometry;

namespace HANDAZ.PEB.Entities
{
    public class Node
    {
        private static int idCounter;
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public int Id { get; set; }
        public Node(double _X, double _Y, double _Z)
        {
            Id =  ++idCounter;
            X = _X;
            Y = _Y;
            Z = _Z;
           
        }
    }
}
