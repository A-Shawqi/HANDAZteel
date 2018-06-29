using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.Core.Designers
{
    public static class BuiltUpSectionDesign
    {
    
        public static HndzISectionProfile BuiltUpSection { get; set; }
        public  static HndzISectionProfile BuiltUpSectionRafter { get; set; }
        public static HndzColumnStandardCase columnTapered { get; set; }
        public static HndzBeamStandrdCase beamTapered { get; set; }

        public enum LoadingType
        {
            Case1,
            Case2
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
       static private double bFTop;
       static private double tFTop;
       static private double bFBottom;
       static private double tFBottom;
       static private double webHeight;
       static private double webThickness;
       static private double bfMinimum;
       static private double dweb;
       static private double area;
       static private double ix;
       static private double iy;
       static private double mxAtStationStart;
       static private double mxAtStationEnd;
       static private double myAtStationStart;
       static private double myAtStationEnd;
       static private double shearAtStatonStart;
       static private double shearAtStatonEnd;
       static private double axialForce;
       static private double columnLength;
       static private double bucklingLengthInPLan;
       static private double bucklingLengthOutPLan;
       static private bool? webIsCompact;
       static private bool? flangIsCompact;
       static private double sx;
       static private double sy;
       static private double rafterBeamIx;
       static private double rafterBeamLength;
       static private double zx;
       static private double zy;
       static private double rx;
       static  private double ry;

       static public bool ContainsLateralTorsionalBuckling { get; set; }
       static private double Fbx { get; set; }
       static private double A1 { get; set; }
       static private double A2 { get; set; }
       static private double FYX { get; set; }
       static private double FYXZ { get; set; }
       static private double alpha { get; set; }
       static private double Fu { get; set; }
       static private bool IsSafe { get; set; }
       static private double Fy { get; set; }
       static private double fca { get; set; }
       static private double lmdaMax { get; set; }
       static private double Lumax { get; set; }
       static private double Lu1 { get; set; }
       static private double Lu2 { get; set; }
       static private double LengthOfColumn { get; set; }
       static private bool LamdaIsSafe { get; set; }
       static private double CompressionFlangUnbracedLengthActual { get; set; }
       static private double BucklingInPlanLength { get; set; }
       static private double BucklingOutPlanLength { get; set; }
       static private bool SectionCompact { get; set; }
       static private double fbcx { get; set; }
       static private double CaseStress { get; set; }
       static private double Fc { get; set; }
       static private double Ga { get; set; }
       static private double Gb { get; set; }
       static private double fbcy { get; set; }
       static private double Fbcy { get; set; }
       static private double MomentCoffient { get; set; }
       static private double kFactor { get; set; }
       static private double CB { get; set; }
       static private double BFTop
        {
            get
            {
                return BuiltUpSection.I_Section.b_fTop;
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
                return BuiltUpSection.I_Section.t_fTop;
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
                return BuiltUpSection.I_Section.b_fBot;
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
                return BuiltUpSection.I_Section.t_fBot;
            }

            set
            {
                tFBottom = value;
            }
        }
       static private double WebHeight
        {
            get
            {
                return BuiltUpSection.I_Section.d;
            }

            set
            {
                webHeight = value;
            }
        }
       static private double WebThickness
        {
            get
            {
                return BuiltUpSection.I_Section.t_w;
            }

            set
            {
                webThickness = value;
            }
        }
       static private double Area
        {
            get
            {
                return BuiltUpSection.I_Section.A;
            }

            set
            {
                area = value;
            }
        }
       static private double Ix
        {
            get
            {
                return BuiltUpSection.I_Section.I_x;
            }

            set
            {
                ix = value;
            }
        }
       static private double Iy
        {
            get
            {
                return BuiltUpSection.I_Section.I_y;
            }

            set
            {
                iy = value;
            }
        }
       static private double Zx
        {
            get
            {
                return BuiltUpSection.I_Section.Z_x;
            }

            set
            {
                zx = value;
            }
        }
       static private double Zy
        {
            get
            {
                return BuiltUpSection.I_Section.Z_y;
            }

            set
            {
                zy = value;
            }
        }
       static private double Rx
        {
            get
            {
                return BuiltUpSection.I_Section.r_x;
            }

            set
            {
                rx = value;
            }
        }
       static private double Ry
        {
            get
            {
                return BuiltUpSection.I_Section.r_y;
            }

            set
            {
                ry = value;
            }
        }
       static private double LmdaIn { get; set; }
       static private double Cmx { get; set; }
       static private double Cmy { get; set; }
       static private double LmdaOut { get; set; }
       static private double MxAtStationStart
        {
            get
            {
                return columnTapered.AnalysisResults[0].Moment3[0];
            }

            set
            {
                mxAtStationStart = value;
            }
        }
       static private double MxAtStationEnd
        {
            get
            {
                return columnTapered.AnalysisResults[columnTapered.AnalysisResults.Length].Moment3[0];
            }

            set
            {
                mxAtStationEnd = value;
            }
        }
       static private double MyAtStationStart
        {
            get
            {
                return columnTapered.AnalysisResults[0].Moment2[0];
            }

            set
            {
                myAtStationStart = value;
            }
        }
       static private double MyAtStationEnd
        {
            get
            {
                return columnTapered.AnalysisResults[columnTapered.AnalysisResults.Length].Moment2[0];
            }

            set
            {
                myAtStationEnd = value;
            }
        }
       static private double ShearAtStatonStart
        {
            get
            {
                return columnTapered.AnalysisResults[0].Shear2[0];
            }

            set
            {
                shearAtStatonStart = value;
            }
        }
       static private double ShearAtStatonEnd
        {
            get
            {
                return columnTapered.AnalysisResults[columnTapered.AnalysisResults.Length].Shear2[0];
            }

            set
            {
                shearAtStatonEnd = value;
            }
        }
       static private double AxialForce
        {
            get
            {
                return columnTapered.AnalysisResults[0].Axial[0];
            }

            set
            {
                axialForce = value;
            }
        }
       static private double ColumnLength
        {
            get
            {
                return columnTapered.ExtrusionLine.RhinoLine.Length;
            }

            set
            {
                columnLength = value;
            }
        }
       static private double BucklingLengthInPLan
        {
            get
            {
                return bucklingLengthInPLan;
            }

            set
            {
                bucklingLengthInPLan = value;
            }
        }
       static private double BucklingLengthOutPLan
        {
            get
            {
                return bucklingLengthOutPLan;
            }

            set
            {
                bucklingLengthOutPLan = value;
            }
        }
       static private double Dweb
        {
            get
            {
                return WebHeight - (2 * TFTop) - (2 * TFBottom);
            }

            set
            {
                Dweb = value;
            }
        }
       static private double Sx
        {
            get
            {
                return BuiltUpSection.I_Section.S_xTop;
            }

            set
            {
                sx = value;
            }
        }
       static private double Sy
        {
            get
            {
                return sy;
            }

            set
            {
                sy = value;
            }
        }
       static private bool? WebIsCompact
        {
            get
            {
                return webIsCompact;
            }

            set
            {
                webIsCompact = value;
            }
        }
       static private bool? FlangIsCompact
        {
            get
            {
                return flangIsCompact;
            }

            set
            {
                flangIsCompact = value;
            }
        }
       static private double RafterBeamIx
        {
            get
            {
                return BuiltUpSectionRafter.I_Section.I_x;
            }

            set
            {
                rafterBeamIx = value;
            }
        }
       static private double RafterBeamLength
        {
            get
            {
                return beamTapered.ExtrusionLine.RhinoLine.Length;
            }

            set
            {
                rafterBeamLength = value;
            }
        }

        public static bool CheckCompactSection()
        {
            CheckWebCombactness();
            CheckFlangCompact();
            if ((bool) WebIsCompact && (bool) FlangIsCompact )
            {
                return SectionCompact = true;
            }
            else
            {
              return  SectionCompact = false;
            }
        }
        private static bool? CheckWebCombactness()
        {
            double dwtw = WebHeight / WebThickness;
            //epsi
            double epsi = ((-AxialForce / Area) + (MxAtStationEnd * 100 / Sx)) / ((-AxialForce / Area) - (MxAtStationEnd * 100 / Sx));
            double alpha = 0.5 * (AxialForce / ((WebHeight / 10) * (WebThickness / 10) * Fy) + 1);
            double alphaCondA = (699 / Math.Sqrt(Fy) / (13 * alpha - 1));
            double alphaCondB = (63.6 / alpha) / Math.Sqrt(Fy);
            double alphaResult = (alpha > 0.5) ? alphaCondA : alphaCondB;
            double epsiCondA = (190 / Math.Sqrt(Fy)) / (2 + epsi);
            double epsiCondB = (95 * (1 - epsi) * Math.Sqrt(-epsi)) / Math.Sqrt(Fy);
            double epsiResult = (epsi > -1) ? epsiCondA : epsiCondB;

            if (dwtw < 58 / Math.Sqrt(Fy))
            {
                WebIsCompact = true;
            }
            else if (dwtw > 58 / Math.Sqrt(Fy) && dwtw < 64 / Math.Sqrt(Fy))
            {
                WebIsCompact = false;
            }
            else
            {
                WebIsCompact = null;
            }

            if (epsi > 1)
            {
                WebIsCompact = null;
            }
            if (dwtw < alphaResult )
            {
                WebIsCompact = true;
            }
            else if(dwtw > alphaResult && dwtw < epsiResult)
            {
                WebIsCompact = false;
            }
            else
            {
                WebIsCompact = null;
            }
            return WebIsCompact;
        }
        private static bool? CheckFlangCompact()
        {
            double Ctf = BFTop / (2 * TFTop);

            // = IF(C48 < (15.3 / (H8) ^ 0.5), "Compact", IF(C48 < (21 / (H8) ^ 0.5), "Non compact", "Slender"))

            if (Ctf < (15.3 / Math.Sqrt(Fy)))
            {
                FlangIsCompact = true;
            }
            else if (Ctf < (21 / Math.Sqrt(Fy)))
            {
                FlangIsCompact = false;
            }
            else
            {
               FlangIsCompact = null;
            }
            return FlangIsCompact;
        }
        public static void CheckStresses(SupportType support, double k)
        {
            kFactor = k;
            fbcx = MxAtStationEnd * 100 / Sx;
            fbcy = MyAtStationEnd * 100 / Sy;
            Fbcy = 0.72 * Fy;
            fca = AxialForce / Area;
            MomentCoffient = mxAtStationEnd / MxAtStationStart;
            CB =  Math.Min(1.75 + 1.05 * MomentCoffient + 0.3 * (MomentCoffient * MomentCoffient), 2.3);
            Lu1 =  (20 * (BFTop / 1000)) / Math.Sqrt(Fy);
            Lu2 =  (1380 * CB * (BFTop * TFTop / 100) / (Fy * 10 * WebHeight));
            Lumax = Math.Min(Lu1, Lu2);
            double fltb1 = Math.Min((800 * CB * BFTop * TFTop) / (1000 * (WebHeight - 2 * TFTop) * CompressionFlangUnbracedLengthActual), 0.58 * Fy);
            double Aflang = (TFTop * BFTop + WebThickness * WebHeight / 6) / 100;
            double Iflang = ((TFTop * BFTop * BFTop* BFTop* 3 / 12) + (WebHeight * WebThickness * WebThickness* WebThickness / 36)) / 10000;
            double luRt  = CompressionFlangUnbracedLengthActual * 100 / Aflang;
            double F1 = 0.58 * Fy;
            double F2 = Math.Min(Math.Abs((0.64 - ((luRt * luRt * 2.4) / (1.176 * Math.Sqrt(10) * CB))) * Fy), 0.58 * Fy);
            double F3 = Math.Min((12000 * CB) / luRt * luRt, 0.58 * Fy);
            double Fltb2;
                if (luRt <( 84 * Math.Sqrt((CB / Fy))))
            {
                 Fltb2 = F1;
            }
            else if ((luRt > (84 * Math.Sqrt((CB / Fy))) && (luRt < (188 * Math.Sqrt((CB / Fy))))))
            {
                Fltb2 = F2;
            }
            else
            {
                Fltb2 = F3;
            }
            double fltb = Math.Min(Math.Sqrt(fltb1 * fltb1 + Fltb2 * Fltb2), 0.58 * Fy);
            double fsec;
            if (SectionCompact)
            {
                fsec = 0.64 * Fy;
            }
            else
            {
                fsec = 0.58 * Fy;
            }
            fbcx = fltb;
            switch (support)
            {
                case SupportType.Pinned:
                    Ga = 10;
                    break;
                case SupportType.Fixed:
                    Ga = 1;
                    break;
            }
            Gb = (Ix / LengthOfColumn) / (RafterBeamIx / RafterBeamLength);
            bucklingLengthInPLan = kFactor * bucklingLengthInPLan;
            BucklingLengthOutPLan = kFactor * bucklingLengthOutPLan;
            LmdaIn = BucklingInPlanLength * 100 / Rx;
            LmdaOut = BucklingOutPlanLength * 100 / Ry;
            lmdaMax = Math.Max(LmdaIn, LmdaOut);
            if (lmdaMax < 180)
            {
                LamdaIsSafe = true;
            }
            if (LmdaOut < 100)
            {
                Fc = FYX - FYXZ * Math.Sqrt(lmdaMax);
            }
            else
            {
                Fc = 7500 / (lmdaMax * lmdaMax);
            }
            double fcafc = fca / Fc;
            Cmx = 0.85;
            Cmx = 1;
            if (fcafc < 0.15)
            {
                A1 = 1.00;
                A2 = 1.00;
            }
            else
            {
                A1 = Math.Max((Cmx / (1 - fca / (7500 / (LmdaIn * LmdaIn)))) , 1);
                A2 = Math.Max((Cmy / (1 - fca / (7500 / (LmdaIn * LmdaOut)))) , 1);
            }
            if (fcafc + (fbcx / fltb) * A1 + (fbcy / Fbcy) * A2 > CaseStress)
            {
                IsSafe = true;
            }
        }
        public static void initDesigner(SteelGrade Grade, LoadingType LoadingType, HndzISectionProfile _ColumnSection, HndzISectionProfile _BeamSection,   HndzColumnStandardCase _columnTapered,
         HndzBeamStandrdCase _beamTapered )
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
            BuiltUpSection = _ColumnSection;
            BuiltUpSectionRafter = _BeamSection;
            columnTapered = _columnTapered;
            beamTapered = _beamTapered;
            switch (Grade)
            {
                case SteelGrade.st37:
                    Fy = 2.4;
                    Fu = 3.6;
                    FYX = 1.4;
                    FYXZ = 0.000065;
                    break;
                case SteelGrade.st44:
                    Fy = 2.8;
                    Fu = 4.4;
                    FYX = 1.6;
                    FYXZ = 0.000085;
                    break;
                case SteelGrade.st52:
                    Fy = 3.6;
                    Fu = 5.2;
                    FYX = 2.1;
                    FYXZ = 0.000135;
                    break;
            }
        }

        #region 
        //public double AssumeZx()
        //{
        //    //what is ZX
        //    //assume Fbx = 1.30
        //    Fbx = 1.30;
        //    double zxRequired = mxAtStationEnd*100/Fbx;
        //    return zxRequired;

        //}
        //public checkLocalBuckling()
        //{
        //    double webCapacity = Fy * ()
        //}
        //public bool checkWebCompactness()
        //{
        //    // WebIsCompact 
        //    //check compact section 
        //   // Dweb / WebThickness)  
        //        alpha = 0.5 * (axialForceAt / (((WebHeight / 10) - 2 * (TFTop / 10)) * (WebThickness / 10) * Fy) + 1);
        //    double variable = (699 / Math.Sqrt(Fy) )/ (13 * alpha - 1);
        //    double variable2 = (63.6 / alpha) / Math.Sqrt(Fy);
        //    double variable3 = (variable > variable2) ? variable2 : variable;

        //    double Shoka = = ((-axialForceAt / Area) + (MxAtStationStart * 100 / Sx)) / ((-C13 / C36) - (C10 * 100 / C41))

        //}
        #endregion
    }
}
