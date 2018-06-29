using System.ComponentModel;

namespace HANDAZ.Entities
{
    public enum HndzWCS
    {
        WGS84,
        [Description("Egypt Red Belt")]
        Egypt_Red_Belt,
        [Description("NTF Lambert Zone II")]
        NTF_Lambert_Zone_II
    }
    public enum HndzLengthUnitSystem
    {
        mm=1,
        cm=100,
        inch=25,//25.4
        m=1000,
        ft=305 //304.8
    }
    public enum HndzForceUnitSystem
    {
        Kgf,
        N,
        KN,
        Tonf,
        lb,
        Kips
    }
    public enum HndzProductDiscipline
    {
        None,
        Architectural,
        Structural

    }
    public enum HndzMaterialType
    {
        Steel = 1,
        Concrete = 2,
        NoDesign = 3,
        Aluminum = 4,
        ColdFormed = 5,
        Rebar = 6,
        Tendon = 7,
        Masonry = 8
    }
    //public enum HndzProfile
    //{
    //    None,
    //    Rectangular,
    //    Circular,
    //    Irregular
    //}

    #region Steel
    public enum HndzLocationEnum
    {
        //TODO: Find the real values of this enum
        Cairo /*= 70*/,
        Alexandria /*= 90*/, //to allow selected index casting
        Matrouh /*= 130*/,
        Aswan /*= 130*/,
        Sinai /*= 130*/
    }
    public enum HndzRoofSlopeEnum
    {
        From1To5 ,
        From1To10,
        From1To20
    }

    public enum HndzRoofAccessibilityEnum
    {
        Accessible,
        Inaccessible
    }
    public enum HndzBuildingEnclosingEnum
    {
        Enclosed,
        PartiallyEnclosed,
        Open
    }
    public enum HndzImportanceFactorEnum
    {
        I,
        II,
        III,
        IV
    }
    public enum HndzProductTypeEnum
    {
        None,
        Beam,
        Column,
        Purlin,
        Girt,
        EaveStrut,
        CableBracing,
        RoofPanel,
        WallPanel,
        FlangeBrace,
        Gutter
    }
    public enum HndzSectionTypeEnum
    {
        HotRolledC,
        HotRolledI,
        BuiltUpI,
        TaperedI,
        
    }
    public enum HndzShapeTypeEnum
    {
        Standrd,
        Tapered
    }
    public enum HndzSupportTypeEnum
    {
        Fixed,
        SemiFixed,
        Pinned,
        Free
    }
    public enum HndzRiskCategoryEnum
    {
        I,II,III,IV
    }
    public enum HndzExposureCategoryEnum
    {
        B,C,D
    }
    #endregion

    #region Analysis Tools Enums
    //=======================================================================================================================
    //DISCLAIMER : THIS IS A HIGHLY SENSETIVE AREA, ANY MODIFICATION HERE CAN CAUSE A CATOSTROPHY, YOU MUST GET A PRESMISSION
    //              FROM THE CREATOR OF THESE ENUMS BEFORE MODIFYING IT, AS IT WILL CAUSE THE ANALYSIS MODEL TO COLLAPSE
    //=======================================================================================================================

    public enum HndzUnitsEnum
    {
        lb_in_F,
        lb_ft_F,
        kip_in_F,
        kip_ft_F,
        kN_mm_C,
        kN_m_C,
        kgf_mm_C,
        kgf_m_C,
        N_mm_C,
        N_m_C,
        Ton_mm_C,
        Ton_m_C,
        kN_cm_C,
        kgf_cm_C,
        N_cm_C,
        Ton_cm_C,
    }
    public enum HndzRestraintEnum
    {
        Roller,
        Pinned,
        Fixed
    }
    public enum HndzAnalysisDOFs
    {
        SpaceFrame,
        PlaneFrame,
        PlaneGrid,
        SpaceTruss
    }
    public enum HndzLoadDirectionEnum
    {
        Local1axis=1,
        Local2axis=2,
        Local3axis=3,
        Xdirection=4,
        Ydirection=5,
        Zdirection=6,
        XProjected=7,
        YProjected=8,
        ZProjected=9,
        Gravity=10,
        GravityProjected=11
    }

    public enum HndzLoadPatternType
    {
        Dead = 1,
        SuperDead = 2,
        Live = 3,
        ReduceLive = 4,
        Quake = 5,
        Wind = 6,
        Snow = 7,
        Other = 8,
        Move = 9,
        Temperature = 10,
        Rooflive = 11,
        Notional = 12,
        PatternLive = 13,
        Wave = 14,
        Braking = 15,
        Centrifugal = 16,
        Friction = 17,
        Ice = 18,
        WindOnLiveLoad = 19,
        HorizontalEarthPressure = 20,
        VerticalEarthPressure = 21,
        EarthSurcharge = 22,
        DownDrag = 23,
        VehicleCollision = 24,
        VesselCollision = 25,
        TemperatureGradient = 26,
        Settlement = 27,
        Shrinkage = 28,
        Creep = 29,
        WaterloadPressure = 30,
        LiveLoadSurcharge = 31,
        LockedInForces = 32,
        PedestrianLL = 33,
        Prestress = 34,
        Hyperstatic = 35,
        Bouyancy = 36,
        StreamFlow = 37,
        Impact = 38,
        Construction = 39,
        DeadWearing = 40,
        DeadWater = 41,
        DeadManufacture = 42,
        EarthHydrostatic = 43,
        PassiveEarthPressure = 44,
        ActiveEarthPressure = 45,
        PedestrianLLReduced = 46,
        SnowHighAltitude = 47,
        EuroLm1Char = 48,
        EuroLm1Freq = 49,
        EuroLm2 = 50,
        EuroLm3 = 51,
        EuroLm4 = 52,
        SeaState = 53,
        Permit = 54,
        MoveFatigue = 55,
        MoveFatiguePermit = 56
    }
    public enum HndzLoadCombinationsEnum
    {
        LinearAdditive,
        Envelope,
        AbsoluteAdditive,
        SRSS,
        RangeAdditive
    }
    public enum HndzFrameTypeEnum
    {
        /// <summary>
        /// just for future development
        /// </summary>
        Undefined,
        ClearSpan,
        SingleSlope,
        MultiSpan1,
        MultiSpan2,
        MultiSpan3,
        MultiGable
    }
    #endregion
}


