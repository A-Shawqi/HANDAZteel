using HANDAZ.PEB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.PEB.Entities
{
    public class BeamTaperedProfile
    {
        //there are the robot api Section names and properties 
        public string SectionName { get; set; }
        public string SectionTag { get; set; }
        public double B1 { get; set; }
        public double TF1 { get; set; }
        public double B2 { get; set; }
        public double TF2 { get; set; }
        public double Tw { get; set; }
        public double Height { get; set; } 

        Material Material;
        Node Midpoint;

        public BeamTaperedProfile(string _SectionName, string _SectionTag, double _B1, double _TF1, double _B2, double _TF2, double _Tw, double _Height)
        {
            SectionName = _SectionName;
            SectionTag = _SectionTag;
            B1 = _B1;
            TF1 = _TF1;
            B2 = _B2;
            TF2 = _TF2;
            Tw = _Tw;
            Height = _Height;
            Midpoint = new Node( _Tw / 2, Height / 2, 0);

        }

    }
}
