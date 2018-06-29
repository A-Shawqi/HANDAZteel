using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace HANDAZ.PEB.Entities
{
    public class Purlin
    {
      
        private static int idCounter;
        public Node PurlinStart { get; set; }
        public Node PurlinEnd { get; set; }
        public Line MyPurlin { get; set; }
        public I_BeamSection PurlinSection { get; set; }
        public Results PurlinLoads { get; set; }
        public int PurlinId { get; set; }
        public double purlinSpacing_Xdirection { get; set; }

        public enum PurlingType
        {
            Single,
            Continous
        }
        public Purlin(Node _purlinStart, Node _purlinEnd, I_BeamSection _purlinSection)
        {
            PurlinId = ++idCounter;
            PurlinStart = _purlinStart;
            PurlinEnd = _purlinEnd;
            Point3d _PurlinStart = new Point3d(PurlinStart.X, PurlinStart.Y, PurlinStart.Z);
            Point3d _PurlinEnd = new Point3d(PurlinEnd.X, PurlinEnd.Y, PurlinEnd.Z);
            MyPurlin = new Line(_PurlinStart, _PurlinEnd);
            PurlinSection = _purlinSection;
        }
        public Purlin(int _purlinId, Node _purlinStart, Node _purlinEnd, I_BeamSection _purlinSection, Results _purlinloads) : this(_purlinStart, _purlinEnd, _purlinSection)
        {
            PurlinLoads = _purlinloads;
        }
    }
}