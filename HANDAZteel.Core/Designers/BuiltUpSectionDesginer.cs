using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;
using HANDAZ.Entities;

namespace HANDAZ.Core.Designers
{ 
    public class BuiltUpSectionDesginer
    {
        //==============================
        // <3<3<3<3<3<3<3<3<3<3<3<3<3<3<3
        // From Gecko With Love <3<3<3<3 


        //================================
        //this functions must return a handaz section profile of type I profile
        //this class should take a Specific load combinations 
        // + ve moment 
        // max -ve moment 
        //max N 
        // this is a the class designer for saftey 

        SectionI BuiltUpSection;
        HndzAnalysisResults AppliedLoads;
        public enum SteelGrade
        {
            st37,
            st44,
            st52
        }
        public enum Sway
        {
            permitted,
            NotPermitted
        }
        public enum Compactness
        {
            Compact,
            NonCompact,
            Slender
        }
        //======================
        //====================
        private double bFTop;
        private double tFTop;
        private double bFBottom;
        private double tFBottom;
        private double webHeight;
        private double webThickness;
        private double bfMinimum;
        //==========================
        private double area;
        private double ix;
        private double iy;
        private double zx;
        private double zy;
        private double rx;
        private double ry;
        //========================
        private double lmdaX;
        private double lmdaY;
        private double fca;
        private double fex;
        private double fey;
        //========================
        private double mx;
        private double my;
        private double q ;
        private double n;

        public double lmdaMax { get; set; }
        public double A1 { get; set; }
        public double A2 { get; set; }
        private double Fbx { get; set; }
        private double FYield { get; set; }
        private double Fby { get; set; }
        private double Fc { get; set; }
        public double Zxrequired { get; set; }
        public double ZYrequired { get; set; }

        public double BucklingLengthX { get; set; }
        public double BucklingLengthY { get; set; }
        public double BucklingLengthMax { get; set; }

        public double BFTop
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

        public double TFTop
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

        public double BFBottom
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

        public double TFBottom
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

        public double WebHeight
        {
            get
            {
                return BuiltUpSection.d;
            }

            set
            {
                webHeight = value;
            }
        }

        public double WebThickness
        {
            get
            {
                return BuiltUpSection.t_w;
            }

            set
            {
                webThickness = value;
            }
        }

        public double Area
        {
            get
            {
                return BuiltUpSection.A;
            }

            set
            {
                area = value;
            }
        }

        public double Ix
        {
            get
            {
                return BuiltUpSection.I_x;
            }

            set
            {
                ix = value;
            }
        }

        public double Iy
        {
            get
            {
                return BuiltUpSection.I_y;
            }

            set
            {
                iy = value;
            }
        }

        public double Zx
        {
            get
            {
                return BuiltUpSection.Z_x;
            }

            set
            {
                zx = value;
            }
        }

        public double Zy
        {
            get
            {
                return BuiltUpSection.Z_y;
            }

            set
            {
                zy = value;
            }
        }

        public double Mx
        {
            get
            {
                return AppliedLoads.Moment3[0];
            }

            set
            {
                mx = value;
            }
        }

        public double My
        {
            get
            {
                return AppliedLoads.Moment2[0];
            }

            set
            {
                my = value;
            }
        }

        public double Q
        {
            get
            {
                return AppliedLoads.Shear2[0];
            }

            set
            {
                q = value;
            }
        }

        public double N
        {
            get
            {
                return AppliedLoads.Axial[0];
            }

            set
            {
                n = value;
            }
        }

        public double Rx
        {
            get
            {
                return BuiltUpSection.r_x;
            }

            set
            {
                rx = value;
            }
        }

        public double Ry
        {
            get
            {
                return BuiltUpSection.r_y;
            }

            set
            {
                ry = value;
            }
        }

        private double LmdaX
        {
            get
            {
                return BucklingLengthX/Rx;
            }

            set
            {
                lmdaX = value;
            }
        }

        private double LmdaY
        {
            get
            {
                return BucklingLengthY/Ry;
            }

            set
            {
                lmdaY = value;
            }
        }

        public double Fca
        {
            get
            {
                return Fc/Area;
            }

            set
            {
                fca = value;
            }
        }

        public double Fex
        {
            get
            {
                return 7500/(LmdaX * LmdaX);
            }

            set
            {
                fex = value;
            }
        }

        public double Fey
        {
            get
            {
                return 7500 / (LmdaY * lmdaY);
            }

            set
            {
                fey = value;
            }
        }

        public double BfMinimum
        {
            get
            {
                return bfMinimum = (BFTop < BFBottom ) ? BFTop:BFBottom;
            }

            set
            {
                bfMinimum = value;
            }
        }


        //=================================
        //To Design We have to First
        //Get the Reruired Zx and we do so by
        public void GetAllowableNormalStress(SteelGrade Grade, Sway swaycondition)
        {
            lmdaMax = (LmdaX > LmdaY) ? lmdaX : LmdaY;
            if (lmdaMax > 100)
            {
                Fc = lmdaMax / 7500;
            }
            else
            {
                switch (Grade)
                {
                    case SteelGrade.st37:
                        Fc = 1.4 - 0.000065 * lmdaMax * lmdaMax;
                        break;
                    case SteelGrade.st44:
                        Fc = 1.6 - 0.000085 * lmdaMax * lmdaMax;
                        break;
                    case SteelGrade.st52:
                        Fc = 2.1 - 0.000135 * lmdaMax * lmdaMax;
                        break;
                    default:
                        break;
                }
            }
            double Cmx;
            double Cmy;
            if (Fca / Fc < 0.15)
            {
                A1 = A2 = 1;
            }
            else if (Fca / Fc > 0.15)
            {
                switch (swaycondition)
                {
                    case Sway.permitted:
                        Cmx = 0.85;
                        Cmy = 0.85;
                        A1 = ((Cmx / (1 - (Fca / Fex))) < 1) ? (Cmx / (1 - (Fca / Fex))) : 1;
                        A2 = ((Cmy / (1 - (Fca / Fey))) < 1) ? (Cmy / (1 - (Fca / Fey))) : 1;
                        break;
                    case Sway.NotPermitted:
                        //not handled 
                        break;
                    default:
                        break;
                }
            }
        }
        public void GetFbx(Compactness Compact)
        {
            BucklingLengthMax = (BucklingLengthX > BucklingLengthY) ? BucklingLengthX : BucklingLengthY;
            if (BucklingLengthMax <= ((20 * bfMinimum * BfMinimum) / Math.Sqrt(FYield)))
            {
                switch (Compact)
                {
                    case Compactness.Compact:
                        Fbx = 0.64 * FYield;
                        Fby = 0.72 * FYield;
                        break;
                    case Compactness.NonCompact:
                        Fbx = Fby = 0.58 * FYield;
                        break;
                    case Compactness.Slender:
                        break;
                    default:
                        break;
                }
               
            }
        }
        public void GetFby()
        {

        }

        #region 
        ////============================
        //public bool CheckStress()
        //{
        //   return ((N / Area) / Fc) +( A1 * (Mx / Zx) / Fbx) + (A2 * (My / Zy) / Fby) > 1;

        //}
        //private double CheckWebThickness()
        //{

        //    return 0.0;

        //}
        //private double CheckWebHeight()
        //{
        //    return 0.0;

        //}
        //private double CheckTopFlangWidth()
        //{
        //    return 0.0;

        //}
        //private double CheckTopFlangThickness()
        //{
        //    return 0.0;

        //}
        //private double CheckBottomFlangThickness()
        //{
        //    return 0.0;

        //}
        //private double CheckBottomFlangWidth()
        //{
        //    return 0.0;

        //}
        //private double CheckMoment()
        //{
        //    return 0.0;

        //}
        //private double CheckShear()
        //{


        //    return 0.0;

        //}
        //public void /*- to be changed to handaz profile */ RetriveBuiltUpSection()
        //{



        //}

        #endregion
        public BuiltUpSectionDesginer(SectionI BuiltUpSection ,HndzAnalysisResults AppliedLoads)
        {
            this.BuiltUpSection = BuiltUpSection;
            this.AppliedLoads = AppliedLoads;
        }

    }
}
