using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace HANDAZ.PEB.Entities
{
    public class Column
    {
        private static int idCounter;
        public Node ColumnStart { get; set; }
        public Node ColumnEnd { get; set; }
        public Line MyColumn { get; set; }
        public I_BeamSection ColumnSection { get; set; }
        public BeamTaperedProfile TaperedAtStartNode { get; set; }
        public BeamTaperedProfile TaperedATEndNode { get; set; }
        public int Id { get; set; }
        public enum ColumnUsage
        {
            EndGable,
            FrameColumn
        }
        public Column(Node _columnStart,Node _columnEnd, I_BeamSection _columnSection)
        {
            Id = ++idCounter;
            ColumnStart = _columnStart;
            ColumnEnd = _columnEnd;
            Point3d _ColumnStart = new Point3d(_columnStart.X, _columnStart.Y, _columnStart.Z);
            Point3d _ColumnEnd = new Point3d(_columnEnd.X, _columnEnd.Y, _columnEnd.Z);
            Line myColumn = new Line(_ColumnStart, _ColumnEnd);
            ColumnSection = _columnSection;
        }
        //public Column(Node _columnStart, Node _columnEnd, I_BeamSection _columnSection, Results _columnLoad) : this(_columnStart, _columnEnd, _columnSection)
        //{
        //    ColumnLoad = _columnLoad;
        //}
        public Column(Node _columnStart, Node _columnEnd, BeamTaperedProfile _taperedAtStartNode ,BeamTaperedProfile _taperedATEndNode) 
        {
            Id = ++idCounter;
            ColumnStart = _columnStart;
            ColumnEnd = _columnEnd;
            Point3d _ColumnStart = new Point3d(_columnStart.X, _columnStart.Y, _columnStart.Z);
            Point3d _ColumnEnd = new Point3d(_columnEnd.X, _columnEnd.Y, _columnEnd.Z);
            Line myColumn = new Line(_ColumnStart, _ColumnEnd);
            TaperedAtStartNode = _taperedAtStartNode;
            TaperedATEndNode = _taperedATEndNode;

        }
        //public Column(Node _columnStart, Node _columnEnd, BeamTaperedProfile _taperedAtStartNode, BeamTaperedProfile _taperedATEndNode, Results _columnLoad):this( _columnStart, _columnEnd, _taperedAtStartNode, _taperedATEndNode)
        //{
        //    ColumnLoad = _columnLoad;
        //}
     
}
}