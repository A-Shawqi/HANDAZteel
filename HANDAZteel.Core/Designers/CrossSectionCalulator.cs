using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.Entities;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.PEB.Core.Designers
{
    public static class CrossSectionCalulator
    {
        public static SectionI AssumedSection { get; set; }
        public static HndzAnalysisResults AppliedLoads { get; set; }
        private static double dw1;
        private static double dw2;
        // static double tw;
        private static double tw1;
        private static double tw2;

        public enum SteelGrade
        {
            st37,
            st44,
            st52
        }

        public static SectionI CalculateColumnSection(SteelGrade Grade, double ColumnHeight, double AxialForce, double MxEnd)
        {
            double Fy = 0;
            switch (Grade)
            {
                case SteelGrade.st37:
                    Fy = 2.4;
                    break;
                case SteelGrade.st44:
                    Fy = 2.8;
                    break;
                case SteelGrade.st52:
                    Fy = 3.6;
                    break;
                default:
                    break;
            }
            dw1 = (ColumnHeight * 100) / 15;
            dw2 = (ColumnHeight * 100) / 20;
            double Dw = ((dw1 + dw2) / 2);
            tw1 = Dw / (190 / Math.Sqrt(Fy));
            double Tw = Math.Max(tw1, 5) + 2;
            Tw = Tw / 10;
            double C = (Math.Abs(AxialForce) / 2) + (Math.Abs(MxEnd) * 100 / Dw);
            double AssumedFc = 1.3;
            double FlangArea = C / AssumedFc;
            double AssumedTflang = Math.Round(Math.Sqrt(FlangArea / 20), 1);
            double AssumedBflang = 20 * AssumedTflang;
            AssumedBflang = (int)AssumedBflang;
            double CondA = Dw / Tw; // <=180
            double CondB = AssumedTflang / Tw; // <=2
            double CondC = Dw / AssumedBflang; // <= 4.5
            double CondD = AssumedBflang / AssumedTflang; // <= 20

            if (CondA > 180 || CondB > 2 || CondC > 4.5 || CondD > 20)
            {

                if (CondA > 180)
                {
                    Dw = 180 * Tw;
                }
                if (CondB > 2)
                {
                    AssumedTflang = 2 * Tw;
                    AssumedTflang = 2 * Tw;
                }
                if (CondC > 4.5)
                {
                    Dw = AssumedBflang * 4.5;
                }
                if (CondD > 20)
                {
                    AssumedBflang = 20 * AssumedTflang;
                    AssumedBflang = 20 * AssumedTflang;
                }
            }
            return AssumedSection = new SectionI("AssumedColSection", Dw, AssumedBflang, AssumedBflang, AssumedTflang, AssumedTflang, Tw);
        }
        public static SectionI CalculateBeamSection(SteelGrade Grade, double BeamLength, double AxialForce, double MxEnd)
        {
            double Fy = 0;
            switch (Grade)
            {
                case SteelGrade.st37:
                    Fy = 2.4;
                    break;
                case SteelGrade.st44:
                    Fy = 2.8;
                    break;
                case SteelGrade.st52:
                    Fy = 3.6;
                    break;
                default:
                    break;
            }
            dw1 = (BeamLength *100 ) / 20;
            dw2 = (BeamLength * 100) / 40;
            double Dw = ((dw1 + dw2) / 2);
            tw1 = Dw / (190 / Math.Sqrt(Fy));
            double Tw = Math.Max(tw1, 5) + 2;
            Tw = Tw / 10;
            double C = (Math.Abs(AxialForce) / 2) + (Math.Abs(MxEnd) * 100 / Dw);
            double AssumedFc = 1.3;
            double FlangArea = C / AssumedFc;
            double AssumedTflang = Math.Round(Math.Sqrt(FlangArea / 20), 1);
            double AssumedBflang = 20 * AssumedTflang;
            AssumedBflang = (int)AssumedBflang;

            double CondA = Dw / Tw; // <=180
            double CondB = AssumedTflang / Tw; // <=2
            double CondC = Dw / AssumedBflang; // <= 4.5
            double CondD = AssumedBflang / AssumedTflang; // <= 20

            if (CondA > 180 || CondB > 2 || CondC > 4.5 || CondD > 20)
            {

                if (CondA > 180)
                {
                    Dw = 180 * Tw;
                }
                if (CondB > 2)
                {
                    AssumedTflang = 2 * Tw;
                    AssumedTflang = 2 * Tw;
                }
                if (CondC > 4.5)
                {
                    Dw = AssumedBflang * 4.5;
                }
                if (CondD > 20)
                {
                    AssumedBflang = 20 * AssumedTflang;
                    AssumedBflang = 20 * AssumedTflang;
                }
            }

            return AssumedSection = new SectionI("AssumedBeamSection", Dw, AssumedBflang, AssumedBflang, AssumedTflang, AssumedTflang, Tw);
        }
        public static SectionI SectionTaperedColumn(SectionI AssumedSection)
        {
            double WebHeight = AssumedSection.d / 3;
            double WebThickness = AssumedSection.t_w;
            double FlangTopWidth = AssumedSection.b_fTop;
            double FlangBotWidth = AssumedSection.b_fBot;
            double FlangTopThickness = AssumedSection.t_fTop;
            double FlangBotThickness = AssumedSection.t_fBot;


            return new SectionI("TaperedEnd", WebHeight, FlangTopWidth, FlangBotWidth, FlangTopThickness, FlangBotThickness, WebThickness);
        }
        public static SectionI SectionTaperedBeam(SectionI AssumedSection)
        {
            double WebHeight = AssumedSection.d / 1.5;
            double WebThickness = AssumedSection.t_w;
            double FlangTopWidth = AssumedSection.b_fTop;
            double FlangBotWidth = AssumedSection.b_fBot;
            double FlangTopThickness = AssumedSection.t_fTop;
            double FlangBotThickness = AssumedSection.t_fBot;

            return new SectionI("TaperedEndBeam", WebHeight, FlangTopWidth, FlangBotWidth, FlangTopThickness, FlangBotThickness, WebThickness);
        }
        public static SectionI PostProcessing(SectionI FinalSection)
        {
            string Name = FinalSection.Name;
            double WebHeight = (int)(FinalSection.d * 10) + 1;
            double WebThickness = (int)(FinalSection.t_w * 10 )+ 1;
            double FlangTopWidth = (int)(FinalSection.b_fTop * 10) + 1;
            double FlangBotWidth = (int)(FinalSection.b_fBot * 10) + 1;
            double FlangTopThickness = (int)(FinalSection.t_fTop * 10 )+ 1;
            double FlangBotThickness = (int)(FinalSection.t_fBot * 10) + 1;

            return new SectionI(Name, WebHeight, FlangTopWidth, FlangTopThickness, WebThickness);
          //  return new SectionI(Name, WebHeight, FlangTopWidth, FlangBotWidth, FlangTopThickness, FlangBotThickness, WebThickness);

        }
        public static SectionI SectionCompare(SectionI A , SectionI B)
        {
            double WebHeightA= A.d;
            double WebThicknessA = A.t_w;
            double FlangTopWidthA = A.b_fTop;
            double FlangBotWidthA = A.b_fBot;
            double FlangTopThicknessA = A.t_fTop;
            double FlangBotThicknessA = A.t_fBot;
            //==============================
            double WebHeightB = B.d;
            double WebThicknessB = B.t_w;
            double FlangTopWidthB = B.b_fTop;
            double FlangBotWidthB = B.b_fBot;
            double FlangTopThicknessB = B.t_fTop;
            double FlangBotThicknessB = B.t_fBot;
            //========================
            double SectionAIx = ((WebHeightA * WebHeightA * WebHeightA * WebThicknessA) / 12) + (2 * FlangTopWidthA * FlangTopThicknessA * (((WebHeightA / 2) + (FlangTopThicknessA / 2)) * ((WebHeightA / 2) + (FlangTopThicknessA / 2))));
            double SectionBIx = ((WebHeightB * WebHeightB * WebHeightB * WebThicknessB) / 12) + (2 * FlangTopWidthB * FlangTopThicknessB * (((WebHeightB / 2) + (FlangTopThicknessB / 2)) * ((WebHeightB / 2) + (FlangTopThicknessB / 2))));
            if (SectionAIx > SectionBIx)
            {
                return A;
            }
            else
            {
                return B;
            }
        }

    }
}
