using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.Entities
{
    [DataContract]
    [KnownType(typeof(HANDAZ.Entities.HndzDoor))]
    [KnownType(typeof(HANDAZ.Entities.HndzWindow))]
    public class HndzWallOpening : HndzBuildingElement
    {
        #region Properties
        [DataMember]
        public Double Width { get; set; }
        [DataMember]
        public Double Height { get; set; }
        [DataMember]
        public HndzWall Wall { get; set; }
        [DataMember]
        public Point3d Position { get; set; }

        #endregion

        #region Constr
        public HndzWallOpening(string name, string description, Double width = 0, Double height = 0,
                               Point3d position = default(Point3d), HndzWall wall = null, Double baseOffset = 0) : base()
        {
            Name = name;
            Description = description;
            Width = width;
            Height = height;
            Position = position;
            Wall = wall;
            BaseOffset = baseOffset;
            if (Wall != null)
            {
                base.BuildingStorey = Wall.BuildingStorey;
            }
            AddToAssociatedWall();
        }

        public HndzWallOpening(Double width = 0, Double height = 0, Point3d position = default(Point3d),
                               HndzWall wall = null, Double baseOffset = 0) :
                               this(HndzResources.DefaultName, HndzResources.DefaultDescription, width, height,
                                    position, wall, baseOffset)
        {

        }
        public HndzWallOpening() : this(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {

        }
        #endregion

        #region Methods
        private void AddToAssociatedWall()
        {
            if (Wall != null)
            {
                if (Wall.WallOpenings != null)
                {
                    Wall.WallOpenings.Add(this);
                }
                else
                {
                    Wall.WallOpenings = new List<HndzWallOpening> { this };
                }
            }
        }
        #endregion
    }
}
