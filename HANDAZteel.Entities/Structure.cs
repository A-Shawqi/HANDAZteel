using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;
//using Xbim.IO;

namespace HANDAZ.PEB.Entities
{
    public class Structure
    {
      public List<Frame> Frames { get; set; }
        public List<Purlin> Purlins { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public int NumberOfFrames { get; set; }
        public double BaySpacing { get; set; }
        public Node Origin { get; set; }
        public string ProjectName { get; set; }
        public Structure(string _projectName, Node _Origin,double _Length, double _Width, int _NumberOfFrames, double _BaySpacing , List<Frame> _Frames , List<Purlin> _Purlins)
        {
            ProjectName = _projectName;
            Origin = _Origin;
            Length = _Length;
            Width = _Width;
            NumberOfFrames = _NumberOfFrames;
            BaySpacing = _BaySpacing;
            Frames = _Frames;
            Purlins = _Purlins;
        }



    }
}