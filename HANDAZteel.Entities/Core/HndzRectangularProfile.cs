using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Rhino.Geometry;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzRectangularProfile : HndzProfile
    {
        #region properties
       [DataMember, XmlAttribute]
        public Rectangle3d Rectangle { get; set; }
        #endregion

        #region Constructor
        public HndzRectangularProfile(Rectangle3d rectangle, Vector2d orientation) : base(HndzResources.DefaultName, HndzResources.DefaultDescription, 
                                                               orientation)
        {
            Rectangle = rectangle;
            OrientationInPlane = orientation;
        }
        public HndzRectangularProfile(Rectangle3d rectangle) : this(rectangle,default(Vector2d))
        {
        }
        public HndzRectangularProfile()
        {
        }

        //ToDo: Other Constructors.
        #endregion
    }
}
