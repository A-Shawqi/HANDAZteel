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
    public class HndzWindow : HndzWallOpening
    {
        #region prop


        #endregion


        #region constr
        public HndzWindow(string name, string description, Double width = 0, Double height = 0,
                        Point3d position = default(Point3d), HndzWall wall = null, Double baseOffset = 0) : 
                        base(name,description,width,height,position,wall,baseOffset)
        {
        }

        public HndzWindow(Double width = 0, Double height = 0, Point3d position = default(Point3d),
                        HndzWall wall = null, Double baseOffset = 0) :
                        this(HndzResources.DefaultName, HndzResources.DefaultDescription, width, height,
                        position, wall, baseOffset)
        {
        }
        public HndzWindow() :this(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
        }

        #endregion
    }
}
