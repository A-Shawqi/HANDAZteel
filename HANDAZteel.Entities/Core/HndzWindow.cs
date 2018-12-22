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
        public HndzWindow(string name, string description, Double width, Double height,
                        Point3d position, HndzWall wall = null, Double sillHeight = 0) : 
                        base(name,description,width,height,position,wall, sillHeight)
        {
        }

        public HndzWindow(Double width, Double height, Point3d position,
                        HndzWall wall = null, Double sillHeight = 0) :
                        this(HndzResources.DefaultName, HndzResources.DefaultDescription, width, height,
                        position, wall, sillHeight)
        {
        }
        public HndzWindow() :this(HndzResources.DefaultName, HndzResources.DefaultDescription,0,0,default(Point3d))
        {
        }

        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Window";
        #endregion

    }
}
