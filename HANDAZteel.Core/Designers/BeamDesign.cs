using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.PEB.Core.Designers
{
    public static class BeamDesign
    {
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
            st37,
            st44,
            st52
        }

        public static bool CheckIFWebCompactFixed(double WebHeight, double WebThickness, double FlangTopWidth, double FlangTopThickness, double AxialForce, double MxAtStationEnd, double Fy)
        {
            double Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
            double Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
            double Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));

            bool WebCompact;

            double dwtw = WebHeight / WebThickness;
            double epsi = ((-AxialForce / Area) + (MxAtStationEnd * 100 / Zx)) / ((-AxialForce / Area) - (MxAtStationEnd * 100 / Zx));
            double alpha = 0.5 * (AxialForce / ((WebHeight) * (WebThickness) * Fy) + 1);
            double alphaCondA = (699 / Math.Sqrt(Fy) / (13 * alpha - 1));
            double alphaCondB = (63.6 / alpha) / Math.Sqrt(Fy);
            double alphaResult = (alpha > 0.5) ? alphaCondA : alphaCondB;
            double epsiCondA = (190 / Math.Sqrt(Fy)) / (2 + epsi);
            double epsiCondB = (95 * (1 - epsi) * Math.Sqrt(-epsi)) / Math.Sqrt(Fy);
            double epsiResult = (epsi > -1) ? epsiCondA : epsiCondB;

            bool conditionCompact;
            if (dwtw < (58 / Math.Sqrt(Fy)))
            {
                conditionCompact = true;
            }
            else
            {
                if (dwtw > 58 / Math.Sqrt(Fy) && dwtw < 64 / Math.Sqrt(Fy))
                {
                    conditionCompact = false;
                }
                else
                {
                    conditionCompact = false;
                }
            }

            if (epsi > 1)
            {
                return WebCompact = conditionCompact;
            }
            else
            {
                if (dwtw < alphaResult)
                {
                    return WebCompact = true;
                }
                else
                {
                    if (dwtw > alphaResult && dwtw < epsiResult)
                    {
                        return WebCompact = false;
                    }
                    else
                    {
                        return WebCompact = false;
                    }
                }
            }
        }
        public static bool CheckIfFlangisCompactFixed(double FlangTopWidth, double FlangTopThickness, double WebThickness, double Fy)
        {
            bool FlangCompact;
            double Ctf = ((FlangTopWidth / 2) - (WebThickness / 2) - WebThickness) / FlangTopThickness;
            double CtfCond = 15.3 / Math.Sqrt(Fy);
            double CtfCondB = 21 / Math.Sqrt(Fy);
            if (Ctf <= CtfCond)
            {
                return FlangCompact = true;
            }
            else
            {
                if (Ctf < CtfCondB)
                {
                    return FlangCompact = false;
                }
                else
                {
                    return FlangCompact = false;
                }
            }
        }
        public static void IncreaseWebThicknessFixed(double WebThickness, out double NewWebThickness)
        {
            NewWebThickness = 1.1 * WebThickness;
        }
        public static void IncreaseWebHeightFixed(double WebHeight, out double NewWebHeight)
        {
            NewWebHeight = 1.1 * WebHeight;
        }
        public static void IncreaseFlangThicknessFixed(double FlangThicknessTop, double FlangThicknessBot, out double NewFlangThicknessTop, out double NewFlangThicknessBot)
        {
            NewFlangThicknessTop = 1.1 * FlangThicknessTop;
            NewFlangThicknessBot = 1.1 * FlangThicknessBot;
        }
        public static void IncreaseFlangWidthFixed(double FlangWidthTop, double FlangWidthBot, out double NewFlangWidthTop, out double NewFlangWidthBot)
        {
            NewFlangWidthTop = 1.1 * FlangWidthTop;
            NewFlangWidthBot = 1.1 * FlangWidthBot;
        }
        public static void CheckLateralTortionalBucklingFixed(double LUnsupported, double MxStart, double MxEnd, double Fy, double WebHeight, double WebThickness,
             double FlangTopWidth, double FlangTopThickness, bool SectionCompact, out double Fbcx, out double Fbcy)
        {
            bool LateralTorsionalBucklingExists;
            double FLTB = 0;
            double FLTB1 = 0;
            double FLTB2 = 0;
            double AreaFlang = FlangTopThickness * FlangTopWidth;
            double AlphaMoment = 0;
            AlphaMoment = Math.Min(MxStart, MxEnd) / Math.Max(MxStart, MxEnd);
            if (double.IsNaN(AlphaMoment))
            {
                AlphaMoment = 0;
            }
            double CB = 1.75 + (1.05 * AlphaMoment) + 0.5 * (AlphaMoment * AlphaMoment);
            if (CB > 2.3)
            {
                CB = 2.3;
            }
            double LuCondA = 20 * FlangTopWidth / (Math.Sqrt(Fy));
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
                    if (!SectionCompact)
                    {
                        Fbcx = 0.58 * Fy;
                        Fbcy = 0.58 * Fy;
                    }
                    else
                    {
                        Fbcx = 0.58 * Fy;
                        Fbcy = 0.58 * Fy;
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
                    double Aflang = ((FlangTopThickness * FlangTopWidth) + ((WebThickness * WebHeight) / 6)) / 100;
                    double Iflang = (((FlangTopThickness * FlangTopWidth * FlangTopWidth * FlangTopWidth * 3) / 12) + (WebHeight * WebThickness * WebThickness * WebThickness / 36)) / 10000;
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
                    Fbcx = FLTB = Math.Sqrt((FLTB1 * FLTB1) + (FLTB2 * FLTB2));
                    if (Fbcx > 0.58 * Fy)
                    {
                        Fbcx = 0.58 * Fy;
                    }
                }

            }
            else
            {
                if (!SectionCompact)
                {
                    Fbcx = 0.58 * Fy;
                    Fbcy = 0.58 * Fy;
                }
                else
                {
                    Fbcx = 0.64 * Fy;
                    Fbcy = 0.72 * Fy;
                }
            }

        }
        public static bool CheckBeamBuckling(double Kfactor, double LbInPlan, double LbOutPlan, double Rx, double Ry, out double LmdaIn, out double LmdaOut, out double LmdaMax)
        {
            bool BeamIsSafeForBucklingInPlan;
            bool BeamIsSafeForBucklingOutPlan;
            bool BeamIsSafeForBuckling;

            LbInPlan = Kfactor * LbInPlan;
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
        public static bool CheckAllowableStresses(SwayCondtion Sway, SteelGrade Grade, LoadingType Case, double MxStart, double MxEnd, double AxialForce,
            double LmdaIn, double LmdaOut, double LmdaMax, double Area, double Zx, double Zy, double Fbcx, double Fbcy, out double Stress, out double CaseStress)
        {
            bool SectionSafe;
            double Cm;
            double A1 = 0;
            double A2 = 0;
            double Fc;
            double FcP1 = 0;
            double FcP2 = 0;
            double Fca = AxialForce / Area;
            CaseStress = 0;
            switch (Case)
            {
                case LoadingType.Case1:
                    CaseStress = 1.0;
                    break;
                case LoadingType.Case2:
                    CaseStress = 1.2;
                    break;
            }
            double AlphaMoment = Math.Min(MxStart, MxEnd) / Math.Max(MxStart, MxEnd);
            if (LmdaMax <= 100)
            {
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
                }
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
            double fbxActual = (MxEnd * 100) / Zx;
            Stress = (Fca / Fc) + ((fbxActual / Fbcx) * A1);
            if (Stress < CaseStress)
            {
                return SectionSafe = true;
            }
            else
            {
                return SectionSafe = false;
            }
        }
        public static bool CheckShearStress(double WebHeight, double WebThickness, double ShearForce, double Fy, out double Qw)
        {
            bool BeamIsSafeForShear;
            Qw = ShearForce / (WebHeight * WebThickness);
            if (Qw < 0.35 * Fy)
            {
                return BeamIsSafeForShear = true;
            }
            else
            {
                return BeamIsSafeForShear = false;
            }
        }
        public static bool CheckEquvilantStress(double Stress, double Qw, double CaseStress)
        {
            double equivialntStress = Math.Sqrt((Stress * Stress) + (3 * Qw * Qw));
            bool SectionIsSafe;
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
        public static SectionI DesignEnd(double WebHeight, double WebThickness, double FlangTopWidth, double FlangTopThickness,
         double FlangBotWidth, double FlangBotThickness,
         double AxialForce, double ShearStress, double MxStart, double MxEnd, double Fy,
            double LUnsupported, double Kfactor, double LbInPlan, double LbOutPlan, SwayCondtion Sway, SteelGrade Grade, LoadingType Case)
        {
            SectionI Final;
            double NewWebHeight = WebHeight;
            double NewWebThickness = WebThickness;
            double NewFlangTopWidth = FlangTopWidth;
            double NewFlangBotWidth = FlangBotWidth;
            double NewFlangTopThickness = FlangTopThickness;
            double NewFlangBotThickness = FlangBotThickness;
            //=================
            double Fbcx;
            double Fbcy;
            //===================
            double LmdaIn;
            double LmdaOut;
            double LmdaMax;
            //=====================
            double Stress;
            double CaseStress;
            double Qw;
            //====================
            bool SafeForLocalBuckling;
            bool SafeForStress;
            bool SafeForShear;
            bool SafeForEquivilant;
            //=============================
            bool webCompact = CheckIFWebCompactFixed(WebHeight, WebThickness, FlangTopWidth, FlangTopThickness, AxialForce, MxEnd, Fy);
            bool flangCompact = CheckIfFlangisCompactFixed(FlangTopWidth, FlangTopThickness, WebThickness, Fy);

            //while (!webCompact || !flangCompact)
            //{
            //    if (!webCompact)
            //    {
            //        IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
            //        WebThickness = NewWebThickness;
            //        webCompact = CheckIFWebCompactFixed(WebHeight, WebThickness, AxialForce, FlangTopWidth, FlangTopThickness, MxEnd, Fy);

            //    }
            //    if (!flangCompact)
            //    {
            //        IncreaseFlangThicknessFixed(FlangTopThickness, FlangBotThickness, out NewFlangTopThickness, out NewFlangBotThickness);
            //        FlangTopThickness = NewFlangTopThickness;
            //        FlangBotThickness = NewFlangBotThickness;
            //        flangCompact = CheckIfFlangisCompactFixed(FlangTopWidth, FlangTopThickness, WebThickness, Fy);
            //    }
            //}
            double Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
            double Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
            double Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
            double Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
            double Zy = (Iy / (FlangTopWidth / 2));
            double Rx = Math.Sqrt(Ix / Area);
            double Ry = Math.Sqrt(Iy / Area);

            CheckLateralTortionalBucklingFixed(LUnsupported, MxStart, MxEnd, Fy, WebHeight, WebThickness, FlangTopWidth, FlangTopThickness, true, out Fbcx, out Fbcy);
            SafeForLocalBuckling = CheckBeamBuckling(Kfactor, LbInPlan, LbOutPlan, Rx, Ry, out LmdaIn, out LmdaOut, out LmdaMax);
            SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LmdaIn, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);

            while (!SafeForLocalBuckling)
            {

                //if (LmdaIn > 180)
                //{
                    IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                    WebHeight = NewWebHeight;
                    Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                    Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                    Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                    Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                    Zy = (Iy / (FlangTopWidth / 2));
                    Rx = Math.Sqrt(Ix / Area);
                    Ry = Math.Sqrt(Iy / Area);
                //}
                //else if (LmdaOut > 180)
                //{
                //    IncreaseFlangWidthFixed(FlangTopWidth, FlangBotWidth, out NewFlangTopWidth, out NewFlangBotWidth);
                //    FlangTopWidth = NewFlangTopWidth;
                //    FlangBotWidth = NewFlangBotWidth;
                //    Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                //    Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                //    Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                //    Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                //    Zy = (Iy / (FlangTopWidth / 2));
                //    Rx = Math.Sqrt(Ix / Area);
                //    Ry = Math.Sqrt(Iy / Area);
                //}
               
                SafeForLocalBuckling = CheckBeamBuckling(Kfactor, LbInPlan, LbOutPlan, Rx, Ry, out LmdaIn, out LmdaOut, out LmdaMax);
                SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LbInPlan, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);
            }
            while (!SafeForStress)
            {
                //IncreaseFlangWidthFixed(FlangTopWidth, FlangBotWidth, out NewFlangTopWidth, out NewFlangBotWidth);
                //FlangTopWidth = NewFlangTopWidth;
                //FlangBotWidth = NewFlangBotWidth;

                IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                WebHeight = NewWebHeight;

                //IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
                //WebThickness = NewWebThickness;

                //IncreaseFlangThicknessFixed(FlangTopThickness, FlangBotThickness, out NewFlangTopThickness, out NewFlangBotThickness);
                //FlangTopThickness = NewFlangTopThickness;
                //FlangBotThickness = NewFlangBotThickness;

              
                Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                Zy = (Iy / (FlangTopWidth / 2));
                Rx = Math.Sqrt(Ix / Area);
                Ry = Math.Sqrt(Iy / Area);

                SafeForLocalBuckling = CheckBeamBuckling(Kfactor, LbInPlan, LbOutPlan, Rx, Ry, out LmdaIn, out LmdaOut, out LmdaMax);
                SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LbInPlan, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);
            }
            SafeForShear = CheckShearStress(WebHeight, WebThickness, ShearStress, Fy, out Qw);
            SafeForEquivilant = CheckEquvilantStress(Stress, Qw, CaseStress);
            while (!SafeForShear)
            {
                IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                WebHeight = NewWebHeight;
                //IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
                //WebThickness = NewWebThickness;
                Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                Zy = (Iy / (FlangTopWidth / 2));
                Rx = Math.Sqrt(Ix / Area);
                Ry = Math.Sqrt(Iy / Area);
                SafeForShear = CheckShearStress(WebHeight, WebThickness, ShearStress, Fy, out Qw);

            }
            while (!SafeForEquivilant)
            {
                IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                WebHeight = NewWebHeight;

                //IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
                //WebThickness = NewWebThickness;

                //IncreaseFlangThicknessFixed(FlangTopThickness, FlangBotThickness, out NewFlangTopThickness, out NewFlangBotThickness);
                //FlangTopThickness = NewFlangTopThickness;
                //FlangBotThickness = NewFlangBotThickness;

                //IncreaseFlangWidthFixed(FlangTopWidth, FlangBotWidth, out NewFlangTopWidth, out NewFlangBotWidth);
                //FlangTopWidth = NewFlangTopWidth;
                //FlangBotWidth = NewFlangBotWidth;
            
                Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                Zy = (Iy / (FlangTopWidth / 2));
                Rx = Math.Sqrt(Ix / Area);
                Ry = Math.Sqrt(Iy / Area);
                SafeForShear = CheckShearStress(WebHeight, WebThickness, ShearStress, Fy, out Qw);
                SafeForLocalBuckling = CheckBeamBuckling(Kfactor, LbInPlan, LbOutPlan, Rx, Ry, out LmdaIn, out LmdaOut, out LmdaMax);
                SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LbInPlan, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);
                SafeForEquivilant = CheckEquvilantStress(Stress, Qw, CaseStress);

            }
            return Final = new SectionI("Beam", WebHeight, FlangTopWidth, FlangBotWidth, FlangTopThickness, FlangBotThickness, WebThickness);
        }
        public static SectionI DesignStart(double WebHeight, double WebThickness, double FlangTopWidth, double FlangTopThickness,
         double FlangBotWidth, double FlangBotThickness,
         double AxialForce, double ShearStress, double MxStart, double MxEnd, double Fy,
            double LUnsupported, double Kfactor, double LbInPlan, double LbOutPlan, SwayCondtion Sway, SteelGrade Grade, LoadingType Case)
        {
            SectionI Final;
            double NewWebHeight = WebHeight;
            double NewWebThickness = WebThickness;
            double NewFlangTopWidth = FlangTopWidth;
            double NewFlangBotWidth = FlangBotWidth;
            double NewFlangTopThickness = FlangTopThickness;
            double NewFlangBotThickness = FlangBotThickness;
            //=================
            double Fbcx;
            double Fbcy;
            //===================
            double LmdaIn;
            double LmdaOut;
            double LmdaMax;
            //=====================
            double Stress;
            double CaseStress;
            double Qw;
            //====================
            bool SafeForLocalBuckling;
            bool SafeForStress;
            bool SafeForShear;
            bool SafeForEquivilant;
            //=============================
            bool webCompact = CheckIFWebCompactFixed(WebHeight, WebThickness, FlangTopWidth, FlangTopThickness, AxialForce, MxStart, Fy);
            bool flangCompact = CheckIfFlangisCompactFixed(FlangTopWidth, FlangTopThickness, WebThickness, Fy);

            while (!webCompact || !flangCompact)
            {
                if (!webCompact)
                {
                    IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
                    WebThickness = NewWebThickness;
                    webCompact = CheckIFWebCompactFixed(WebHeight, WebThickness, AxialForce, FlangTopWidth, FlangTopThickness, MxStart, Fy);

                }
                if (!flangCompact)
                {
                    IncreaseFlangThicknessFixed(FlangTopThickness, FlangBotThickness, out NewFlangTopThickness, out NewFlangBotThickness);
                    FlangTopThickness = NewFlangTopThickness;
                    FlangBotThickness = NewFlangBotThickness;
                    flangCompact = CheckIfFlangisCompactFixed(FlangTopWidth, FlangTopThickness, WebThickness, Fy);
                }
            }

            double Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
            double Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
            double Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
            double Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
            double Zy = (Iy / (FlangTopWidth / 2));
            double Rx = Math.Sqrt(Ix / Area);
            double Ry = Math.Sqrt(Iy / Area);

            CheckLateralTortionalBucklingFixed(LUnsupported, MxStart, MxEnd, Fy, WebHeight, WebThickness, FlangTopWidth, FlangTopThickness, true, out Fbcx, out Fbcy);
            SafeForLocalBuckling = CheckBeamBuckling(Kfactor, LbInPlan, LbOutPlan, Rx, Ry, out LmdaIn, out LmdaOut, out LmdaMax);
            SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LmdaIn, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);

            while (!SafeForLocalBuckling)
            {

                if (LmdaIn > 180)
                {
                    IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                    WebHeight = NewWebHeight;
                    Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                    Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                    Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                    Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                    Zy = (Iy / (FlangTopWidth / 2));
                    Rx = Math.Sqrt(Ix / Area);
                    Ry = Math.Sqrt(Iy / Area);
                }
                else if (LmdaOut > 180)
                {
                    IncreaseFlangWidthFixed(FlangTopWidth, FlangBotWidth, out NewFlangTopWidth, out NewFlangBotWidth);
                    FlangTopWidth = NewFlangTopWidth;
                    FlangBotWidth = NewFlangBotWidth;
                    Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                    Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                    Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                    Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                    Zy = (Iy / (FlangTopWidth / 2));
                    Rx = Math.Sqrt(Ix / Area);
                    Ry = Math.Sqrt(Iy / Area);
                }

                SafeForLocalBuckling = CheckBeamBuckling(Kfactor, LbInPlan, LbOutPlan, Rx, Ry, out LmdaIn, out LmdaOut, out LmdaMax);
                SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LbInPlan, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);
            }
            while (!SafeForStress)
            {
                IncreaseFlangWidthFixed(FlangTopWidth, FlangBotWidth, out NewFlangTopWidth, out NewFlangBotWidth);
                FlangTopWidth = NewFlangTopWidth;
                FlangBotWidth = NewFlangBotWidth;

                IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                WebHeight = NewWebHeight;

                IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
                WebThickness = NewWebThickness;

                IncreaseFlangThicknessFixed(FlangTopThickness, FlangBotThickness, out NewFlangTopThickness, out NewFlangBotThickness);
                FlangTopThickness = NewFlangTopThickness;
                FlangBotThickness = NewFlangBotThickness;

                Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                Zy = (Iy / (FlangTopWidth / 2));
                Rx = Math.Sqrt(Ix / Area);
                Ry = Math.Sqrt(Iy / Area);
                SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LbInPlan, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);
            }
            SafeForShear = CheckShearStress(WebHeight, WebThickness, ShearStress, Fy, out Qw);
            SafeForEquivilant = CheckEquvilantStress(Stress, Qw, CaseStress);
            while (!SafeForShear)
            {
                IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                WebHeight = NewWebHeight;
                IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
                WebThickness = NewWebThickness;
                Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                Zy = (Iy / (FlangTopWidth / 2));
                Rx = Math.Sqrt(Ix / Area);
                Ry = Math.Sqrt(Iy / Area);
                SafeForShear = CheckShearStress(WebHeight, WebThickness, ShearStress, Fy, out Qw);

            }
            while (!SafeForEquivilant)
            {
                IncreaseWebHeightFixed(WebHeight, out NewWebHeight);
                WebHeight = NewWebHeight;
                IncreaseWebThicknessFixed(WebThickness, out NewWebThickness);
                WebThickness = NewWebThickness;
                IncreaseFlangWidthFixed(FlangTopWidth, FlangBotWidth, out NewFlangTopWidth, out NewFlangBotWidth);
                FlangTopWidth = NewFlangTopWidth;
                FlangBotWidth = NewFlangBotWidth;
                IncreaseFlangThicknessFixed(FlangTopThickness, FlangBotThickness, out NewFlangTopThickness, out NewFlangBotThickness);
                FlangTopThickness = NewFlangTopThickness;
                FlangBotThickness = NewFlangBotThickness;
                Area = ((2 * FlangTopThickness * FlangTopWidth) + (WebThickness * WebHeight));
                Ix = ((WebHeight * WebHeight * WebHeight * WebThickness) / 12) + (2 * FlangTopWidth * FlangTopThickness * (((WebHeight / 2) + (FlangTopThickness / 2)) * ((WebHeight / 2) + (FlangTopThickness / 2))));
                Iy = ((2 * FlangTopWidth * FlangTopWidth * FlangTopWidth * FlangTopThickness) / 12);
                Zx = (Ix / ((WebHeight / 2) + FlangTopThickness));
                Zy = (Iy / (FlangTopWidth / 2));
                Rx = Math.Sqrt(Ix / Area);
                Ry = Math.Sqrt(Iy / Area);
                SafeForShear = CheckShearStress(WebHeight, WebThickness, ShearStress, Fy, out Qw);
                SafeForStress = CheckAllowableStresses(Sway, Grade, Case, MxStart, MxEnd, AxialForce, LbInPlan, LmdaOut, LmdaMax, Area, Zx, Zy, Fbcx, Fbcy, out Stress, out CaseStress);
                SafeForEquivilant = CheckEquvilantStress(Stress, Qw, CaseStress);

            }
            return Final = new SectionI("Beam", WebHeight, FlangTopWidth, FlangBotWidth, FlangTopThickness, FlangBotThickness, WebThickness);
        }
    }
}


