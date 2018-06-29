using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.Entities
{
    [DataContract]
    [Serializable]
    [XmlSerializerFormat]
    public class HndzFrameMultiSpan33D : HndzFrame3D
    {
        private double baySpacing;
        private HndzImportanceFactorEnum iI;
        private object p1;
        private object p2;
        private object p3;
        private object p4;
        private object p5;
        private HndzBuildingEnclosingEnum partiallyEnclosed;
        private HndzLocationEnum selectedIndex1;
        private HndzRoofSlopeEnum selectedIndex2;
        private HndzRoofAccessibilityEnum selectedIndex3;
        private HndzStorey storey;
        private int v;

        public HndzFrameMultiSpan33D()
        {
        }

        public HndzFrameMultiSpan33D(double length, double baySpacing, double width, double eaveHeight, int v, HndzLocationEnum selectedIndex1, HndzRoofSlopeEnum selectedIndex2, HndzRoofAccessibilityEnum selectedIndex3, HndzBuildingEnclosingEnum partiallyEnclosed, HndzImportanceFactorEnum iI, object p1, object p2, object p3, object p4, object p5, HndzStorey storey)
        {
            Length = length;
            this.baySpacing = baySpacing;
            Width = width;
            EaveHeight = eaveHeight;
            this.v = v;
            this.selectedIndex1 = selectedIndex1;
            this.selectedIndex2 = selectedIndex2;
            this.selectedIndex3 = selectedIndex3;
            this.partiallyEnclosed = partiallyEnclosed;
            this.iI = iI;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
            this.storey = storey;
        }

        [DataMember, XmlAttribute]
        [XmlArray("Frames2D")]
        public ICollection<HndzFrameMultiSpan32D> Frames2D { get; set; }
        

        public override void AssemblePEB(SectionI columnsStartSection = null, SectionI columnsEndSection = null, SectionI beamsStartSection = null, SectionI beamsEndSection = null, SectionChannel purlinsSection = null)
        {
            throw new NotImplementedException();
        }
    }
}