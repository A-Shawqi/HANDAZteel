using Rhino.Geometry;
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
    [KnownType(typeof(HndzFrameSingleBay3D))]
    [KnownType(typeof(HndzFrameMonoSlope3D))]
    [KnownType(typeof(HndzFrameMultiSpan13D))]
    [KnownType(typeof(HndzFrameMultiSpan23D))]
    [KnownType(typeof(HndzFrameMultiSpan33D))]
    [KnownType(typeof(HndzFrameMultiGable3D))]
    public abstract class HndzFrame3D : HndzProduct
    {
        [DataMember, XmlAttribute]

        private double baySpacing;
        [DataMember, XmlAttribute]

        private int framesCount;
        #region Properties
        [DataMember, XmlAttribute]
        public double Width { get; set; }
        [DataMember, XmlAttribute]
        public double EaveHeight { get; set; }
        [DataMember, XmlAttribute]
        public double RidgeHeight { get; set; }
        [DataMember, XmlAttribute]
        public double Length { get; set; }
        [DataMember, XmlAttribute]
        public double BaySpacing
        {
            get { return baySpacing; }
            set

            {
                baySpacing = value;
                if (baySpacing != 0)
                {
                    framesCount = (int)(Length / baySpacing) + 1;
                }
                return;
            }
        }
        [DataMember, XmlAttribute]
        public double PurlinSpacing { get; set; }
        [DataMember, XmlAttribute]
        public int FramesCount
        {
            get { return framesCount; }
            set
            {
                framesCount = value;
                if (framesCount != 0)
                {
                    baySpacing = Length / framesCount;
                }
                return;
            }
        }
        [DataMember, XmlAttribute]
        public HndzRoofSlopeEnum RoofSlope { get; set; }
        [DataMember, XmlAttribute]
        public HndzLocationEnum Location { get; set; }
        [DataMember, XmlAttribute]
        public HndzRoofAccessibilityEnum RoofAccessibility { get; set; }
        [DataMember, XmlAttribute]
        public HndzBuildingEnclosingEnum BuildingEnclosing { get; set; }
        [DataMember, XmlAttribute]
        public HndzImportanceFactorEnum ImportanceFactor { get; set; }
        [DataMember, XmlAttribute]
        public HndzRiskCategoryEnum RiskCategory { get; set; }
        [DataMember, XmlAttribute]
        public HndzExposureCategoryEnum ExposureCategory { get; set; }
        [DataMember, XmlAttribute]
        public HndzFrameTypeEnum Type { get; set; }


        [DataMember, XmlAttribute]
        [XmlArray("Purlins")]
        public ICollection<HndzPurlinStandrdCase> Purlins { get; set; }
        [XmlArray("Bracing")]
        public ICollection<HndzBracingStandrdCase> Bracing { get; set; }
        [DataMember, XmlAttribute]
        [XmlArray("Girts")]
        public ICollection<HndzGuirt> Girts { get; set; }
        [XmlArray("SideWalls")]
        public ICollection<HndzWallStandardCase> SideWalls { get; set; }

        [DataMember, XmlAttribute]
        [XmlArray("RoofCovering")]
        public ICollection<HndzRoofCovering> RoofCovering { get; set; }
        #endregion
        #region Constructors
        protected HndzFrame3D(string name, string description,  /*float ridgeHeight,*/
                    HndzLocationEnum location, HndzRoofSlopeEnum roofSlope, HndzRoofAccessibilityEnum roofAccessibiity,
                    HndzBuildingEnclosingEnum buildingEnclosing, HndzImportanceFactorEnum importanceFactor, double length, double baySpacing, double purlinSpacing, double width = 0, double eaveHeight = 0, HndzStorey storey = null, HndzStorey topLevel = null) :
                   base(name, description, storey)
        {

            Width = width;
            EaveHeight = eaveHeight;
            Location = location;
            RoofSlope = roofSlope;
            RoofAccessibility = roofAccessibiity;
            BuildingEnclosing = buildingEnclosing;
            ImportanceFactor = importanceFactor;
            Length = length;
            BaySpacing = baySpacing;
            PurlinSpacing = purlinSpacing;
            //FramesCount = (int)(Length / BaySpacing) + 1;
            switch (RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    RidgeHeight = eaveHeight + 0.2 * Width * 0.5;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    RidgeHeight = eaveHeight + 0.1 * Width * 0.5;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    RidgeHeight = eaveHeight + 0.05 * Width * 0.5;
                    break;
            }
        }
        protected HndzFrame3D() : this(HndzResources.DefaultName, HndzResources.DefaultDescription,
                             HndzLocationEnum.Cairo, HndzRoofSlopeEnum.From1To10, HndzRoofAccessibilityEnum.Accessible,
                               HndzBuildingEnclosingEnum.Open, HndzImportanceFactorEnum.II, 0, 0, 0)
        {

        }


        #endregion
        #region Methods
        public abstract void AssemblePEB(SectionI columnsStartSection = null, SectionI columnsEndSection = null, SectionI beamsStartSection = null, SectionI beamsEndSection = null, SectionChannel purlinsSection = null);


        #endregion
    }
}
