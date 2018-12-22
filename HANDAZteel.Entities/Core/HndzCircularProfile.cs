using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using System.Runtime.Serialization;

namespace HANDAZ.Entities
{
    [DataContract]
    public class HndzCircularProfile : HndzProfile
    {
        #region properties
        [DataMember]
        public Circle MyCircle { get; set; }
        #endregion

        #region Constructor
        public HndzCircularProfile(Circle myCircle) : base(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
            MyCircle = myCircle;
            Centroid = MyCircle.Center;

        }
        public HndzCircularProfile() : this(default(Circle))
        {
        }

        //ToDo: Other Constructors.
        #endregion
    }
}
