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
    public class HndzPolylineProfile : HndzProfile
    {
        #region properties
        [DataMember]
        public HndzPolyline MyHndzPolyline { get; set; }
        #endregion

        #region Constructor
        public HndzPolylineProfile(HndzPolyline myHndzPolyline) : base(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
            MyHndzPolyline = myHndzPolyline;
            Centroid = MyHndzPolyline.Centroid;
        }
        public HndzPolylineProfile()
        {
        }

        //ToDo: Other Constructors.
        #endregion
    }
}
