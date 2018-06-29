using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.Core.Designers
{
    public static class BeamColumnDesignerBuiltUp
    {
        //=====================
        // Initialize Applied Loads
        //6 - handle Case oF Non Compact Sections 
        //=======================
        //Public Enums To Help Design 
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
        //Members to Be checked 
        public static SectionI BuiltUpSection { get; set; }
      //  public static SectionI BuiltUpSectionEnd { get; set; }
        public static SectionI BuiltUpSectionRafter { get; set; }
        public static HndzAnalysisResults AppliedLoadsStart { get; set; }
        public static HndzAnalysisResults AppliedLoadsEnd { get; set; }
        //public static HndzColumnStandardCase columnTapered { get; set; }
        //public static HndzBeamStandrdCase beamTapered { get; set; }
        //======================
        public static bool WebReistanceOk { get; set; }
        //Properties for Section Compactness
        //===================================
        static private bool SectionCompact { get; set; }
        static private bool WebIsCompact { get; set; }
        static private bool FlangIsCompact { get; set; }
        static private bool SectionIsCompact { get; set; }
        static public bool SectionIsSafe { get; set; }

        //===================================
        //Section Properties
        static private double webThickness;
        static private double webHeight;
        static private double bFTop;
        static private double tFTop;
        static private double bFBottom;
        static private double tFBottom;
        static private double fillerRadius;
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
        //======================
        //Calculated Section Parameteres 
       // static private double dweb;
        static private double area;
        static private double areaFlang;
        static private double ix;
        static private double rx;
        static private double iy;
        static private double ry;
        static private double zx;
        static private double zy;
        //public static double Dweb
        //{
        //    get
        //    {
        //        return WebHeight - (2 * TFTop) - (2 * TFBottom); ;
        //    }

        //    set
        //    {
        //        dweb = value;
        //    }
        //}
        public static double Area
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
        public static double AreaFlang
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
        public static double Iy
        {
            get
            {
                return((2*BFTop*BFTop*BFTop*TFTop)/12);
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
                return Math.Sqrt(Ix/Area);
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
                return ((WebHeight* WebHeight * WebHeight * WebThickness)/12) + (2 * BFTop * TFTop*(((WebHeight/2) + (TFTop/2))* ((WebHeight / 2) + (TFTop / 2))));
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
                return (Iy / (BFTop/2));
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
                return (Ix / ((WebHeight/ 2) + TFTop));
            }

            set
            {
                zy = value;
            }
        }

        //======================
        private static double LbInPlan { get; set; }
        private static double LbOutPlan { get; set; }
        //============================
        //L Unsupported 
     //   public static double LUnsupported { get; set; }
        public static bool LateralTorsionalBucklingExists { get; set; }
        public static bool ColumnIsSafeForBuckling { get; set; }
        public static bool ColumnIsSafeForBucklingInPlan { get; set; }
        public static bool ColumnIsSafeForBucklingOutPlan { get; set; }
        //=================================
        // LTB
       // public static double A1 { get; set; }
       // public static double A2 { get; set; }

       // public static double FLTB { get; set; }
       // private static double FLTB1 { get; set; }
       // private static double FLTB2 { get; set; }
        //=====================
        // Allowable Stresses 
        private static double Fc { get; set; }
        private static double Fbcx { get; set; }
        public static double FcP1 { get; private set; }
        public static double FcP2 { get; private set; }
        public static double Fbcy { get; private set; }
        private static double fca;

        public static double Fca
        {
            get { return AxialForce / Area; }
            set { fca = value; }
        }
        //======================================
        //Applied Loads
        static private double axialForce;
        static private double mxAtStationStart;
        static private double mxAtStationEnd;
        static private double myAtStationStart;
        static private double myAtStationEnd;
        static private double dweb;

        public static double AxialForce
        {
            get
            {
                //Neeeds To be Changed to actual applied force from resultTable
                return AppliedLoadsStart.Axial;
            }

            set
            {
                axialForce = value;
            }
        }
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
        //======================================
        public static double CaseStress { get; set; }
        private static double LmdaIn { get; set; }
        private static double LmdaOut { get; set; }
        private static double LmdaMax { get;  set; }

        public static double Fy { get; private set; }
        public static double Fu { get; private set; }
        public static double LUnsupported { get; private set; }

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

        //private static void CheckIfWebIsSlender()
        //{
          
        //    //not handled :D 
        //}

        private static bool CheckIfFlangIsCompact()
        {
            //double Ctf = BFTop / (2 * TFTop);
            double Ctf = ((BFTop/2) - (WebThickness/2)  - WebThickness)/ TFTop;
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
            double FLTB1;
            double FLTB2 = 0;
            double FLTB;
            double AlphaMoment = Math.Min(MxAtStationStart, MxAtStationEnd)/ Math.Max(MxAtStationStart, MxAtStationEnd) ;
            double CB = 1.75 + (1.05 * AlphaMoment) + 0.5 * (AlphaMoment * AlphaMoment);
            if (CB > 2.3)
            {
                CB = 2.3;
            }
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
                    if (!SectionIsCompact)
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
                    double rt =  Math.Sqrt(Iflang / Aflang);
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
                    Fbcx = FLTB = Math.Sqrt((FLTB1*FLTB1) + (FLTB2 * FLTB2));
                
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
        public static bool CheckColumnBuckling(double Kfactor)
        {
            LbInPlan = Kfactor * LbInPlan;
            LbOutPlan = /*Kfactor * */LbOutPlan;
            LmdaIn = LbInPlan * 100 / Rx;
            LmdaOut = LbOutPlan *100/ Ry;
            LmdaMax = Math.Max(LmdaIn, LmdaOut);
            if (LmdaIn <= 180)
            {
               ColumnIsSafeForBucklingInPlan = true;
            }
            else
            {
                ColumnIsSafeForBucklingInPlan = false;
            }
            if (LmdaOut <= 180)
            {
                ColumnIsSafeForBucklingOutPlan = true;
            }
            else
            {
                ColumnIsSafeForBucklingOutPlan = false;
            }
            if (ColumnIsSafeForBucklingInPlan && ColumnIsSafeForBucklingOutPlan)
            {
                if (LmdaMax <= 180)
                {
                    return ColumnIsSafeForBuckling = true;

                }
                return ColumnIsSafeForBuckling = false;
            }
            else 
            {
                return ColumnIsSafeForBuckling = false;
            }
        }
        public static bool CheckAllowableStresses(SwayCondtion Sway)
        {
           
            double Cm;
            double A1 = 0;
            double A2 = 0;
            double AlphaMoment = Math.Min(MxAtStationStart, MxAtStationEnd) / Math.Max(MxAtStationStart, MxAtStationEnd);
            if (LmdaMax <= 100)
            {
                Fc = FcP1 - FcP2 * LmdaMax * LmdaMax;
            }
            else
            {
                Fc = 7500/(LmdaMax * LmdaMax);
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
            double fbxActual = (MxAtStationEnd * 100 )/ Zx;
            double fbyActual = (MyAtStationEnd * 100)/ Zy;
            //double Fbcy;
            //double Fbcx;
            //if (!SectionCompact)
            //{
            //    Fbcy = 0.58 * Fy;
            //    Fbcx = 0.64 * Fy;
            //}
            //else
            //{
            //    Fbcy = 0.72 * Fy;
            //    Fbcx = 0.58 * Fy;
            //}
            double stress = (Fca / Fc) + ((fbyActual / Fbcy) * A2) + ((fbxActual / Fbcx) * A1);
            if (stress < CaseStress)
            {
               return SectionIsSafe = true;
            }
            else
            {
               return SectionIsSafe = false;
            }
        }
       
        public static void initDesigner(SteelGrade Grade, LoadingType LoadingType,double _LUnsupported,double _LbInPlan, double _LbOutPlan,
            SectionI _ColumnSection,/*, HndzISectionProfile _BeamSection, HndzColumnStandardCase _columnTapered,
        HndzBeamStandrdCase _beamTapered*/  HndzAnalysisResults _AppliedLoadsStart ,HndzAnalysisResults _AppliedLoadsEnd)
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
           // LbInPlan = _LbInPlan;
            //LbOutPlan = _LbOutPlan;
           // LUnsupported = _LUnsupported;
            BuiltUpSection = _ColumnSection;
            AppliedLoadsStart = _AppliedLoadsStart;
            AppliedLoadsEnd = _AppliedLoadsEnd;
            //BuiltUpSectionRafter = _BeamSection;
            //columnTapered = _columnTapered;
            //beamTapered = _beamTapered;
            switch (Grade)
            {
                case SteelGrade.st37:
                    Fy = 2.4;
                    Fu = 3.6;
                    FcP1 = 1.4;
                    FcP2 = 0.000065;
                    break;
                case SteelGrade.st44:
                    Fy = 2.8;
                    Fu = 4.4;
                    FcP1 = 1.6;
                    FcP2 = 0.000085;
                    break;
                case SteelGrade.st52:
                    Fy = 3.6;
                    Fu = 5.2;
                    FcP1 = 2.1;
                    FcP2 = 0.000135;
                    break;
            }
        }

        public static SectionI Design(HndzAnalysisResults resultStart , HndzAnalysisResults resultEnd,SectionI S, double _LUnsupported, double _LbInPlan,double kFactor,double _LbOutPlan,SteelGrade Grade, LoadingType Case, SwayCondtion Sway)
        {
           // List<SectionI> ColumnSections = new List<SectionI>();
           // Column.AnalysisResults.
          //  SectionI S = CrossSectionCalulator.CalculateSection(CrossSectionCalulator.SteelGrade.st37, resultEnd, 9.5);
          //  SectionI End = CrossSectionCalulator.CalculateSection(CrossSectionCalulator.SteelGrade.st37, resultEnd, 9.5);
            // SectionI s = new SectionI("x", 50, 28, 28, 1.4, 1.4, 0.8);

            BeamColumnDesignerBuiltUp.initDesigner(Grade,Case, _LUnsupported, _LbInPlan, _LbOutPlan, S,resultStart, resultEnd);
            do
            {
               bool a =  BeamColumnDesignerBuiltUp.CheckIfWebComapact();
               bool b =  BeamColumnDesignerBuiltUp.CheckIfFlangIsCompact();
                if (!WebIsCompact)
                {
                    S.d = S.d * 1.05;
                    S.t_w = S.t_w * 1.05;
                    BeamColumnDesignerBuiltUp.CheckIfWebComapact();

                }
                if (!FlangIsCompact)
                {
                    S.t_fTop = S.t_fTop * 1.05;
                    S.b_fTop = S.b_fTop * 1.05;
                    S.t_fBot = S.t_fBot * 1.05;
                    S.b_fBot = S.b_fBot * 1.05;
                    BeamColumnDesignerBuiltUp.CheckIfFlangIsCompact();

                }
              //  BeamColumnDesignerBuiltUp.CheckIfSectionIsCompact();
            } while (!SectionIsCompact);
            BeamColumnDesignerBuiltUp.CheckColumnBuckling(kFactor);
            BeamColumnDesignerBuiltUp.CheckLateralTortionalBuckling(_LUnsupported);
           
            BeamColumnDesignerBuiltUp.CheckAllowableStresses(Sway);
            if (!BeamColumnDesignerBuiltUp.CheckAllowableStresses(Sway))
            {
               Design(resultStart, resultEnd, S, _LUnsupported,_LbInPlan, kFactor, _LbOutPlan, Grade, Case, Sway);
            }
            return S;

        }

        ///////
        //Cross Section Estimation 
        //Check Section Compact 
        //It fires 3 functions 
        //Web resistance 
        //Web is Compact 
        //Flang is compact 

    }
}
