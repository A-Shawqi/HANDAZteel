using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;
using HANDAZ.PEB.Entities;

namespace HANDAZ.PEB.Entities
{
    public class Frame
    {
        private static int idCounter;
        public  int FrameId { get; set; }
        public double RidgeHeight { get; set; }
        public double EaveHeight { get; set; }
        public List<Beam> Beams { get; set; }
        public List<Column> Columns { get; set; }
        public List<Support> Supports { get; set; }
        public Node Origin { get; set; }
        public double HeightDiffrence { get; set; }
        public double Angle { get; set; }
        public double Width { get; set; }
        public Frame(double _ridgeHeight ,double _eaveHeight,List<Beam> _beams,List<Column> _columns,List<Support> _supports,Node _origin, double _Width)
        {
            FrameId = ++idCounter;
            RidgeHeight = _ridgeHeight;
            EaveHeight = _eaveHeight;
            Beams = _beams;
            Columns = _columns;
            Supports = _supports;
            Origin =  _origin;
            Width = _Width;
            HeightDiffrence = RidgeHeight - EaveHeight;
            Angle = Math.Atan2(HeightDiffrence, Width/2);
            foreach (Beam Beam in Beams)
            {
                Beam.Id = Beam.Id + Columns.Count;
            }
        }
    }
}