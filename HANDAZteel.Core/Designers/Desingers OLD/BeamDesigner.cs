using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;
//using static HANDAZ.Core.Designers.BeamColumnDesignerBuiltUp;

namespace HANDAZ.Core.Designers
{
    public static class BeamDesigner
    {
        //Stresses 

        static private double fca;

        private static double Fca
        {
            get { return AxialForce / Area; }
            set { fca = value; }
        }
        // related To Stresses 
        private static double A1 { get; set; }
        private static double A2 { get; set; }
        private static double LmdaIn { get; set; }
        private static double LmdaOut { get; set; }
        private static double LmdaMax { get; set; }
        private static double Fy { get; set; }
        private static double Fc { get; set; }
        private static double Fbcx { get; set; }
        public static double Stress { get; set; }
        private static double Fu { get; set; }
        private static double Qw { get; set; }
        //private static double FcP1 { get;  set; }
        //private static double FcP2 { get;  set; }
        private static double Fbcy { get; set; }
        private static double CaseStress { get; set; }

        // Compactness 
        static private bool FlangIsCompact { get; set; }
        // Buckling Lengths 
        private static double LbInPlan { get; set; }
        private static double LbOutPlan { get; set; }
        public static double LUnsupported { get; set; }

        // Applied loads 
        static private double axialForce;
        static private double shearForce;
        static private double mxAtStationStart;
        static private double mxAtStationEnd;
        static private double myAtStationStart;
        static private double myAtStationEnd;
        public static double MxAtStationStart
        {
            get
            {
                return AppliedLoadsStart.Moment3 ;
            }

            set
            {
                mxAtStationStart = value;
            }
        }
        public static double MxAtStationEnd
        {
            get
            {
                return AppliedLoadsEnd.Moment3 ;
            }

            set
            {
                mxAtStationEnd = value;
            }
        }
        public static double MyAtStationStart
        {
            get
            {
                return AppliedLoadsStart.Moment2 ;
            }

            set
            {
                myAtStationStart = value;
            }
        }
        public static double MyAtStationEnd
        {
            get
            {
                return AppliedLoadsEnd.Moment2 ;
            }

            set
            {
                myAtStationEnd = value;
            }
        }
        private static double AxialForce
        {
            get
            {
                //Neeeds To be Changed to actual applied force from resultTable
                return AppliedLoadsStart.Axial ;
            }

            set
            {
                axialForce = value;
            }
        }
        private static double ShearForce
        {
            get
            {
                return AppliedLoadsStart.Shear2 ;
            }

            set
            {
                shearForce = value;
            }
        }
    
        public static HndzAnalysisResults AppliedLoadsStart { get; set; }
        public static HndzAnalysisResults AppliedLoadsEnd { get; set; }
        // Sections 

        public static SectionI BuiltUpSection { get; set; }
        static private double webThickness;
        static private double webHeight;
        static private double bFTop;
        static private double tFTop;
        static private double bFBottom;
        static private double tFBottom;
        static private double fillerRadius;
        static private double area;
        public static double WebThickness
        {
            get { return BuiltUpSection.t_w; }
            set { webThickness = value; }
        }
        public static double WebHeight
        {
            get { return BuiltUpSection.d; }
            set { webHeight = value; }
        }
        static private double BFTop
        {
            get
            {
                return BuiltUpSection.b_fTop;
            }

            set
            {
                bFTop = value;
            }
        }
        static private double TFTop
        {
            get
            {
                return BuiltUpSection.t_fTop;
            }

            set
            {
                tFTop = value;
            }
        }
        static private double BFBottom
        {
            get
            {
                return BuiltUpSection.b_fBot;
            }

            set
            {
                bFBottom = value;
            }
        }
        static private double TFBottom
        {
            get
            {
                return BuiltUpSection.t_fBot;
            }

            set
            {
                tFBottom = value;
            }
        }
        public static double FillerRadius
        {
            get
            {
                return WebThickness;
            }

            set
            {
                fillerRadius = value;
            }
        }
        private static double Area
        {
            get
            {
                return ((2 * BFTop * TFTop) + (WebThickness * WebHeight));
            }
            set
            {
                area = value;
            }
        }
        static private double ix;
        static private double rx;
        static private double iy;
        static private double ry;
        static private double zx;
        static private double zy;
        public static double Iy
        {
            get
            {
                return ((2 * BFTop * BFTop * BFTop * TFTop) / 12);
            }

            set
            {
                ix = value;
            }
        }
        public static double Rx
        {
            get
            {
                return Math.Sqrt(Ix / Area);
            }

            set
            {
                rx = value;
            }
        }
        public static double Ix
        {
            get
            {
                return ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * BFTop * TFTop * (((WebHeight / 2) + (TFTop / 2)) * ((WebHeight / 2) + (TFTop / 2))));
            }

            set
            {
                iy = value;
            }
        }
        public static double Ry
        {
            get
            {
                return Math.Sqrt(Iy / Area);
            }

            set
            {
                ry = value;
            }
        }
        public static double Zy
        {
            get
            {
                return (Iy / (BFTop / 2));
            }

            set
            {
                zx = value;
            }
        }

        public static double Zx
        {
            get
            {
                return (Ix / ((WebHeight / 2) + TFTop));
            }

            set
            {
                zy = value;
            }
        }

        static private double areaFlang;
        private static double AreaFlang
        {
            get
            {
                return BFTop * TFTop;
            }

            set
            {
                areaFlang = value;
            }
        }
        // Enums 
        public enum LoadingType
        {
            Case1,
            Case2
        }
        public enum SwayCondtion
        {
            PermitedToSway,
            PreventedFromSway
        }
        public enum SupportType
        {
            Pinned,
            Fixed
        }
        public enum SteelGrade
        {
            //the Fyield and FUltime is in T/Cm2
            st37,
            st44,
            st52
        }
        //check stresses 
        //beam subjected to my and mx 
        //our beam is subjected to MX and MY 
        //fb alllowable = 0.58 fy 
        //check shear 
        //Check Deflection 
        // max Deflection L / 200
        //

        //To Choose Section 

        //1- Check Buckling length 
    
    
    //  static private double xbar;
    

        public static bool BeamIsSafeForBucklingInPlan { get;  set; }
        public static bool BeamIsSafeForBucklingOutPlan { get;  set; }
        public static bool BeamIsSafeForBuckling { get;  set; }
        public static bool BeamIsSafeForShear { get; private set; }
        private static bool LateralTorsionalBucklingExists { get; set; }
       // private static double FLTB { get; set; }
       // private static double FLTB1 { get; set; }
       // private static double FLTB2 { get; set; }
        //=====================
        // Allowable Stresses 
  
        public static bool SectionCompact { get;  set; }
        public static bool WebIsCompact { get; private set; }

      //  public static bool SectionIsCompact { get; private set; }

        public static bool SectionIsSafe { get; private set; }
      
        public static bool WebReistanceOk { get; private set; }

       //// public static double Xbar
       // {
       //     get
       //     {
       //         return BuiltUpSection.x_Bar;
       //     }

       //     set
       //     {
       //         xbar = value;
       //     }
       // }


        //====================
        //check 
        // n /a + mx / zx 

        public static SectionI Design(double rafterLength, double purlinSpacing, double bracingSpacing,SectionI Section, HndzAnalysisResults AppliedLoadsStart, HndzAnalysisResults AppliedLoadsEnd)
        {
            initDesigner(SteelGrade.st37, LoadingType.Case2, purlinSpacing, rafterLength , bracingSpacing, Section, AppliedLoadsStart, AppliedLoadsEnd);
            CheckIfSectionIsCompact();
        
            do
            {
                if (!FlangIsCompact)
                {
                    IncreaseFlang(Section);
                }
                if (!WebIsCompact)
                {
                    IncreaseWeb(Section);
                }
                CheckIfSectionIsCompact();

            } while (!SectionCompact);
            CheckLateralTortionalBuckling(purlinSpacing);
            bool Check1 =CheckBeamBuckling(0.7);
            bool Check2 = CheckAllowableStresses(SwayCondtion.PermitedToSway , SteelGrade.st37);
            bool Check3 = CheckShearStress();
            bool Check4 = CheckEquvilantStress();

            if (!Check1 || !Check2 || !Check3|| !Check4)
            {

                if (!Check1)
                {
                    IncreaseWeb(Section);
                }
                else
                {
                    IncreaseSection(Section);
                }
                //  return Design(rafterLength,purlinSpacing,bracingSpacing,Section,AppliedLoadsStart,AppliedLoadsEnd);
                return null;
            }
            else
            {
                return Section;
            }

        }

        public static bool CheckEquvilantStress()
        {
            double equivialntStress = Math.Sqrt((Stress * Stress) + (3 * Qw * Qw));
            if (CaseStress == 1.2)
            {
                if (equivialntStress < 1.32)
                {
                    return SectionIsSafe = true;

                }
                else
                {
                    return SectionIsSafe = false;
                }
            }
            else
            {
                if (equivialntStress > 1.1)
                {
                    return SectionIsSafe = true;

                }
                else
                {
                    return SectionIsSafe = false;
                }
            }

        }

        private static bool CheckWebResistance()
        {
            //return Increase Web
            double MaxWebResistance = WebHeight * WebThickness * Fy;
            if (MaxWebResistance > AxialForce)
            {
                return WebReistanceOk = true;
            }
            else
            {
                return WebReistanceOk = false;
            }
        }
        private static bool CheckIfWebComapact()
        {

            CheckWebResistance();
            double dwtw = WebHeight / WebThickness;

            if (!WebReistanceOk)
            {
                if (dwtw < 58 * Math.Sqrt(Fy))
                {
                    return WebIsCompact = true;
                }
            }
            else
            {
                double alpha = 0.5 * ((AxialForce / (WebHeight * WebThickness * Fy) + 1));
                double alphaCondA = (699 / (Math.Sqrt(Fy) * ((13 * alpha) - 1)));
                double alphaCondB = (63.6 / alpha) / Math.Sqrt(Fy);
                ////===================== Working
                if (alpha <= 0.5 && dwtw < alphaCondA && dwtw < alphaCondB)
                {
                    return WebIsCompact = true;
                }

                if (alpha > 0.5 && dwtw < alphaCondA)
                {
                    return WebIsCompact = true;
                }
                else
                {

                    //Should Check iF web is Slender Heres
                    WebIsCompact = false;
                }
            }
            //epsi not calculated

            //region This region is from Excel Where It checks Epsi
            #region
            //   double epsi = ((-AxialForce / Area) + (MxAtStationEnd * 100 / Sx)) / ((-AxialForce / Area) - (MxAtStationEnd * 100 / Sx));
            // double alpha = 0.5 * (AxialForce / ((WebHeight / 10) * (WebThickness / 10) * Fy) + 1);
            //double alphaCondA = (699 / Math.Sqrt(Fy) / (13 * alpha - 1));
            //double alphaCondB = (63.6 / alpha) / Math.Sqrt(Fy);
            //double alphaResult = (alpha > 0.5) ? alphaCondA : alphaCondB;
            //double epsiCondA = (190 / Math.Sqrt(Fy)) / (2 + epsi);
            //double epsiCondB = (95 * (1 - epsi) * Math.Sqrt(-epsi)) / Math.Sqrt(Fy);
            //double epsiResult = (epsi > -1) ? epsiCondA : epsiCondB;

            //if (dwtw < 58 / Math.Sqrt(Fy))
            //{
            //    WebIsCompact = true;
            //}
            //else if (dwtw > 58 / Math.Sqrt(Fy) && dwtw < 64 / Math.Sqrt(Fy))
            //{
            //    WebIsCompact = false;
            //}
            //else
            //{
            //    WebIsCompact = null;
            //}

            //if (epsi > 1)
            //{
            //    WebIsCompact = null;
            //}
            //if (dwtw < alphaResult)
            //{
            //    WebIsCompact = true;
            //}
            //else if (dwtw > alphaResult && dwtw < epsiResult)
            //{
            //    WebIsCompact = false;
            //}
            //else
            //{
            //    WebIsCompact = null;
            //}
            #endregion
            return WebIsCompact;
        }

        private static bool CheckIfwebCompactFixed()
        {
            double dwtw = WebHeight / WebThickness;
            double epsi = ((-AxialForce / Area) + (MxAtStationEnd * 100 / Zx)) / ((-AxialForce / Area) - (MxAtStationEnd * 100 / Zx));
            double alpha = 0.5 * (AxialForce / ((WebHeight / 10) * (WebThickness / 10) * Fy) + 1);
            double alphaCondA = (699 / Math.Sqrt(Fy) / (13 * alpha - 1));
            double alphaCondB = (63.6 / alpha) / Math.Sqrt(Fy);
            double alphaResult = (alpha > 0.5) ? alphaCondA : alphaCondB;
            double epsiCondA = (190 / Math.Sqrt(Fy)) / (2 + epsi);
            double epsiCondB = (95 * (1 - epsi) * Math.Sqrt(-epsi)) / Math.Sqrt(Fy);
            double epsiResult = (epsi > -1) ? epsiCondA : epsiCondB;

            if (dwtw < 58 / Math.Sqrt(Fy))
            {
                return WebIsCompact = true;
            }
            else if (dwtw > 58 / Math.Sqrt(Fy) && dwtw < 64 / Math.Sqrt(Fy))
            {
                return WebIsCompact = false;
            }
            else
            {
                return  WebIsCompact = false;
            }

            if (epsi > 1)
            {
                return WebIsCompact = false;
            }
            if (dwtw < alphaResult)
            {
                return WebIsCompact = true;
            }
            else if (dwtw > alphaResult && dwtw < epsiResult)
            {
                return WebIsCompact = false;
            }
            else
            {
               return WebIsCompact = false;
            }
        }
        //private static void CheckIfWebIsSlender()
        //{

        //    //not handled :D 
        //}

        private static bool CheckIfFlangIsCompact()
        {
            //double Ctf = BFTop / (2 * TFTop);
            double Ctf = ((BFTop / 2) - (WebThickness / 2) - WebThickness) / TFTop;
            double CtfCond = 15.3 / Math.Sqrt(Fy);
            if (Ctf <= CtfCond)
            {
                return FlangIsCompact = true;
            }
            else
            {
                return FlangIsCompact = false;
            }
        }
        public static bool CheckIfSectionIsCompact()
        {
            CheckIfWebComapact();
            CheckIfFlangIsCompact();
            if (FlangIsCompact && WebIsCompact)
            {
                return SectionCompact = true;
            }
            else
            {
                return SectionCompact = false;
            }

        }

        public static void CheckLateralTortionalBuckling(double _LUnsupported/*, double _MxAtStationStart ,double _MxAtStationEnd*/)
        {
            double FLTB = 0;
            double FLTB1 = 0;
            double FLTB2 = 0;
            double AlphaMoment = Math.Min(MxAtStationStart, MxAtStationEnd) / Math.Max(MxAtStationStart, MxAtStationEnd);
            double CB = 1.75;
            double LuCondA = 20 * BFTop / (Math.Sqrt(Fy));
            double LuCondB = (1380 * AreaFlang * CB) / (Fy * WebHeight);
            if (LUnsupported < Math.Min(LuCondA, LuCondB))
            {
                LateralTorsionalBucklingExists = false;
            }
            else
            {
                LateralTorsionalBucklingExists = true;
            }
            if (LateralTorsionalBucklingExists)
            {
                if (LUnsupported < LuCondB && LUnsupported > LuCondA)
                {
                    Fbcy = 0.58 * Fy;
                    if (!SectionCompact)
                    {
                        Fbcx = 0.58 * Fy;
                    }
                    else
                    {
                        Fbcx = 0.64 * Fy;
                    }
                }
                else
                {
                    Fbcy = 0.58 * Fy;

                    FLTB1 = (800 * AreaFlang * CB) / (LUnsupported * WebHeight);
                    if (FLTB1 > (0.58 * Fy))
                    {
                        FLTB1 = 0.58 * Fy;
                    }
                    // FLTB = FLTB1;
                    // Fbcx = FLTB;
                    //
                    double Aflang = ((TFTop * BFTop) + ((WebThickness * WebHeight) / 6)) / 100;
                    double Iflang = (((TFTop * BFTop * BFTop * BFTop * 3) / 12) + (WebHeight * WebThickness * WebThickness * WebThickness / 36)) / 10000;
                    double rt = Math.Sqrt(Iflang / Aflang);
                    double luRt = (LUnsupported * 100) / rt;
                    double luRtCondA = 84 * Math.Sqrt(CB / Fy);
                    double luRtCondB = 188 * Math.Sqrt(CB / Fy);
                    if (luRt < luRtCondA)
                    {
                        FLTB2 = 0.58 * Fy;
                    }
                    else if (luRt > luRtCondA && luRt < luRtCondB)
                    {
                        FLTB2 = (0.64 * ((luRt * luRt) * Fy) / (1.176 * 10E5 * CB)) * Fy;
                        if (FLTB2 < 0.58 * Fy)
                        {
                            FLTB2 = 0.58 * Fy;
                        }
                    }
                    else if (luRt > luRtCondB)
                    {
                        FLTB2 = ((12000) / (luRt * luRt)) * CB;
                        if (FLTB2 < 0.58 * Fy)
                        {
                            FLTB2 = 0.58 * Fy;
                        }
                    }
                    //======================
                    Fbcx = FLTB = Math.Sqrt((FLTB1 * FLTB1) + (FLTB2 * FLTB2));

                }

                // return Fbcx = FLTB;
            }
            else
            {
                if (SectionCompact)
                {
                    Fbcy = 0.72 * Fy;
                    Fbcx = 0.64 * Fy;
                }
                else
                {
                    Fbcy = 0.58 * Fy;
                    Fbcx = 0.58 * Fy;
                }
                //return Fbcx;
                //HaS to Be Redesingned
            }

        }
        public static bool CheckBeamBuckling(double Kfactor)
        {
            LbInPlan = 0.7 *LbInPlan;
            LbOutPlan = LbOutPlan;
            LmdaIn = LbInPlan * 100 / Rx;
            LmdaOut = LbOutPlan * 100 / Ry;
            LmdaMax = Math.Max(LmdaIn, LmdaOut);
            if (LmdaIn <= 180)
            {
                BeamIsSafeForBucklingInPlan = true;
            }
            else
            {
                BeamIsSafeForBucklingInPlan = false;
            }
            if (LmdaOut <= 180)
            {
                BeamIsSafeForBucklingOutPlan = true;
            }
            else
            {
                BeamIsSafeForBucklingOutPlan = false;
            }
            if (BeamIsSafeForBucklingInPlan && BeamIsSafeForBucklingOutPlan)
            {
                if (LmdaMax <= 180)
                {
                    return BeamIsSafeForBuckling = true;

                }
                return BeamIsSafeForBuckling = false;
            }
            else
            {
                return BeamIsSafeForBuckling = false;
            }
        }
        public static bool CheckAllowableStresses(SwayCondtion Sway , SteelGrade Grade)
        {
            double FcP1 = 0;
            double FcP2 = 0;
            switch (Grade)
            {
                case SteelGrade.st37:
                    FcP1 = 1.4;
                    FcP2 = 0.000065;
                    break;
                case SteelGrade.st44:
                    FcP1 = 1.6;
                    FcP2 = 0.000085;
                    break;
                case SteelGrade.st52:
                    FcP1 = 2.1;
                    FcP2 = 0.000135;
                    break;
                default:
                    break;
            }
            double A1 = 0;
            double A2 = 0;
            double Cm = 0;
          
            double AlphaMoment = Math.Min(MxAtStationStart, MxAtStationEnd) / Math.Max(MxAtStationStart, MxAtStationEnd);
            if (LmdaMax <= 100)
            {
                Fc = FcP1 - FcP2 * LmdaMax * LmdaMax;
            }
            else
            {
                Fc = 7500 / (LmdaMax * LmdaMax);
            }
            double FcaFc = Fca / Fc;
            double Fex = 7500 / (LmdaIn * LmdaIn);
            double Fey = 7500 / (LmdaOut * LmdaOut);
            if (FcaFc < 0.15)
            {
                A1 = 1;
                A2 = 1;
            }
            else
            {
                switch (Sway)
                {
                    case SwayCondtion.PermitedToSway:
                        Cm = 0.85;
                        A1 = Cm / (1 - (Fca / Fex));
                        A2 = Cm / (1 - (Fca / Fey));
                        break;
                    case SwayCondtion.PreventedFromSway:
                        Cm = 0.6 - (0.4 * AlphaMoment);
                        if (Cm < 0.4)
                        {
                            Cm = 0.4;
                        }
                        A1 = Cm / (1 - (Fca / Fex));
                        A2 = Cm / (1 - (Fca / Fey));
                        break;
                }
            }
            double fbxActual = (MxAtStationStart * 100) / Zx;
            double fbyActual = (MyAtStationStart * 100) / Zy;
            Stress = (Fca / Fc) + ((fbyActual / Fbcy) * A2) + ((fbxActual / Fbcx) * A1);
            if (Stress < CaseStress)
            {
                return SectionIsSafe = true;
            }
            else
            {
                return SectionIsSafe = false;
            }
        }

        public static void initDesigner(SteelGrade Grade, LoadingType LoadingType, double _LUnsupported, double _LbInPlan, double _LbOutPlan,
            SectionI _ColumnSection,/*, HndzISectionProfile _BeamSection, HndzColumnStandardCase _columnTapered,
        HndzBeamStandrdCase _beamTapered*/  HndzAnalysisResults _AppliedLoadsStart, HndzAnalysisResults _AppliedLoadsEnd)
        {
            switch (LoadingType)
            {
                case LoadingType.Case1:
                    CaseStress = 1.0;
                    break;
                case LoadingType.Case2:
                    CaseStress = 1.2;
                    break;

            }
            LbInPlan = _LbInPlan;
            LbOutPlan = _LbOutPlan;
            LUnsupported = _LUnsupported;
            BuiltUpSection = _ColumnSection;
            AppliedLoadsStart = _AppliedLoadsStart;
            AppliedLoadsEnd = _AppliedLoadsEnd;
            switch (Grade)
            {
                case SteelGrade.st37:
                    Fy = 2.4;
                    Fu = 3.6;
                    break;
                case SteelGrade.st44:
                    Fy = 2.8;
                    Fu = 4.4;
                    break;
                case SteelGrade.st52:
                    Fy = 3.6;
                    Fu = 5.2;
                    break;
            }
        }

        public static bool CheckShearStress()
        {
           
            Qw = (ShearForce) / (WebHeight * WebThickness);
            if (Qw < 0.35 * Fy)
            {
                return BeamIsSafeForShear = true;
            }
            else
            {
                return BeamIsSafeForShear = false;
            }
        }

        private static void IncreaseFlang(SectionI Section)
        {
            Section.b_fBot = 1.05*Section.b_fBot ;
            Section.b_fTop = 1.05*Section.b_fTop ;
            Section.t_fBot = 1.05*Section.t_fBot ;
            Section.t_fTop = 1.05*Section.t_fTop;
        }
        private static void IncreaseWeb(SectionI Section)
        {
            double tw = 1.05;
            double d = 1.05 ;
            Section.t_w = tw;
            Section.d = d;
        }
        private static void IncreaseSection(SectionI Section)
        {
            IncreaseFlang(Section);
            IncreaseWeb(Section);
        }

        //step 1 
        //if not compact -> redo 
        public static void DesignFixed(double rafterLength, double purlinSpacing, double bracingSpacing, SectionI Section, HndzAnalysisResults AppliedLoadsStart, HndzAnalysisResults AppliedLoadsEnd) 
        {
            initDesigner(SteelGrade.st37, LoadingType.Case2, purlinSpacing, rafterLength , bracingSpacing, Section, AppliedLoadsStart, AppliedLoadsEnd);
            bool WebCompactDesign = CheckIfwebCompactFixed();
            while (!WebCompactDesign)
            {
                IncreaseWeb(Section);
            }
        }
    }
}
