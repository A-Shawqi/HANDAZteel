using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HANDAZ.Entities
{
    public class HndzITaperedProfile:HndzProfile
    {


        public HndzISectionProfile StartProfile { get; set; }
        public HndzISectionProfile EndProfile { get; set; }
        public HndzITaperedProfile(string name,string description,HndzISectionProfile startProfile, HndzISectionProfile endProfile,Vector2d orientation)
            :base(name,description,orientation)
        {
            StartProfile = startProfile;
            EndProfile = endProfile;
            //if (StartProfile.OrientationInPlane==EndProfile.OrientationInPlane)
            //{
            //    OrientationInPlane = StartProfile.OrientationInPlane;
            //}
        }
        public HndzITaperedProfile(HndzISectionProfile startProfile, HndzISectionProfile endProfile, Vector2d orientation=default(Vector2d))
           : this(HndzResources.DefaultName,HndzResources.DefaultDescription,startProfile,endProfile, orientation)
        {
        }
        public HndzITaperedProfile()
        {
        }


    }
}