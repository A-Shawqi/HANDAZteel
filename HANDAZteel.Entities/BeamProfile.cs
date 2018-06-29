using HANDAZ.PEB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.PEB.Entities
{
    public class BeamProfile
    {
        // work here and add your profile section as whatever , and then in beam add a profile instead of IBeam section 
        //  You should note that the beam shalll carry 3 types of profiles 
        // the profile at start , the profile at end 
        // and the profile if the beam is not tappered 
        //your profile should be implemented as following 
        // B Top flang , B Bottom Flang , TF top flang , Tf Bottom Flang , TW web , web Height 
        // THis should be converted in a polytine 
        //======================
           

        public string SectionName { get; set; }
        public string SectionTag { get; set; }
        public Material Material { get; set; }
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double TF1 { get; set; }
        public double TF2 { get; set; }
        public double Tw { get; set; }

        //your logic to convert into whatever suits xbim as in polyline or whatever comes in the next constructor 
        public BeamProfile(string _SectionName, string _SectionTag, Material _Material, double _B1, double _B2, double _TF1
            ,double _TF2, double _Tw)
        {
            SectionName = _SectionName;
            SectionTag = _SectionTag;
            Material = _Material;
            B1 = _B1;
            B2 = _B2;
            TF1 = _TF1;
            TF2 = _TF2;
            Tw = _Tw;
        }

    }
}
