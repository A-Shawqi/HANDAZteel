using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HANDAZ.PEB.Core.ASCE107Wind;

namespace HANDAZ.PEB.Core
{
    /// <summary>
    /// Wind Load Analysis Library
    /// Per ASCE 7-10 Code for Enclosed or Partially Enclosed Buildings
    /// Using Method 2: Analytical Procedure (Section 27 & 28) for Low-Rise Buildings
    /// </summary>
    public static class ASCE107Wind
    {
        #region Variables
        private static double roofAngle;
        private static float meanRoofHeight;

        private static bool isLowRise;

        private static float PIP; //+ve internal pressure
        private static float NIP; //-ve internal pressure

        private static float alpha; //alpha
        private static float zg;
        private static double kh;
        private static double qh;

        private static double a;

        #region Pressure Coefficients Tables
        public static Dictionary<string, double> GCpfA = new Dictionary<string, double>();
        public static Dictionary<string, double> netPositivePressureA = new Dictionary<string, double>();
        public static Dictionary<string, double> netNegativePressureA = new Dictionary<string, double>();
        public static Dictionary<string, double> GCpfB = new Dictionary<string, double>();
        public static Dictionary<string, double> netPositivePressureB = new Dictionary<string, double>();
        public static Dictionary<string, double> netNegativePressureB = new Dictionary<string, double>();
        #endregion
        #endregion
        #region Enumerations
        public enum RiskCategory
        {
            I, II, III, IV
        }
        public enum ExposureCategory
        {
            B, C, D
        }
        public enum RoofType
        {
            Gable, Monoslope
        }
        #endregion
        static ASCE107Wind()
        {
            //External Pressure Coeffients, GCpf (Fig. 28.4-1):
            //For Load Case B:
            GCpfB.Add("Wall Zone 1", 0.40);
            GCpfB.Add("Roof Zone 2", -0.69);
            GCpfB.Add("Roof Zone 3", -0.37);
            GCpfB.Add("Wall Zone 4", -0.29);
            GCpfB.Add("Wall Zone 5", -0.45);
            GCpfB.Add("Wall Zone 6", -0.45);
            GCpfB.Add("Wall Zone 1E", 0.61);
            GCpfB.Add("Roof Zone 2E", -1.07);
            GCpfB.Add("Roof Zone 3E", -0.53);
            GCpfB.Add("Wall Zone 4E", -0.43);
            GCpfB.Add("Roof Zone 5E", 0.61);
            GCpfB.Add("Wall Zone 6E", -0.43);
        }
        /// <summary>
        /// This function takes the building parameters to calculate different pressure coefficients applied in ASCE7-10 Specifications
        /// </summary>
        /// <param name="WindSpeed">Km/hr  (Wind Map, Figure 26.5-1A-C)</param>
        /// <param name="Bldg">(Table 1.5-1 Risk Category)</param>
        /// <param name="exp">Exposure Category(Sect. 26.7)</param>
        /// <param name="Hridge">Ridge Height</param>
        /// <param name="Heave">Eave Height</param>
        /// <param name="LBuilding">Building Length,Parallel to Building Ridge</param>
        /// <param name="WBuilding">Building Width, Normal to building ridge</param>
        /// <param name="roof">Roof Type</param>
        /// <param name="Kzt">Topographical Factor (Sect. 26.8 & Figure 26.8-1)</param>
        /// <param name="Kd">Direction Factor(Table 26.6)</param>
        /// <param name="isEnclosed">Is the building enclosed or not? according to (Sect. 26.2 & Table 26.11-1)</param>
        /// <param name="isHurricane">Is the building in Hurricane region or not</param>
        public static void LoadParameters(float WindSpeed, RiskCategory Bldg, ExposureCategory exp, float Hridge, float Heave,
            float LBuilding, float WBuilding, RoofType roof, float Kzt, float Kd, bool isEnclosed, bool isHurricane)
        {
            //Calculating theta
            //============================================
            CalculateRoofAngle(Hridge, Heave, WBuilding, roof);
            //============================================
            //Calculating mean roof
            //============================================
            CalculateMeanRoofHeight(Hridge, Heave);
            //============================================
            //Check low rise criteria
            //============================================
            IsLowRise(LBuilding, WBuilding);
            //============================================
            //Calculating +ve & -ve Internal pressure coef.
            //============================================
            CalculateInternalPressure(isEnclosed);
            //============================================
            //Calculating alpha,kH,zg,qh
            //============================================
            CalculateCoefficients(WindSpeed, exp, Kzt, Kd);
            //============================================
            //Wall and Roof End Zone Widths 'a'  and '2*a' (Fig. 28.4-1):
            //============================================
            CalculateEndZoneWidths(LBuilding, WBuilding);
            //============================================
            //calculating External Pressure Coeffients, GCpf (Fig. 28.4-1): 
            //for load case A:
            CalculateExternalPressure();
            //============================================
            //MWFRS Wind Load for Load Case A & B, Calculating Net Pressures:
            CalculateNetPressure();
        }

        private static void CalculateNetPressure()
        {
            foreach (KeyValuePair<string, double> kvp in GCpfA)
            {
                try
                {
                    netPositivePressureA.Add(kvp.Key, qh * (kvp.Value - PIP));
                    netNegativePressureA.Add(kvp.Key, qh * (kvp.Value - NIP));
                }
                catch
                {
                }

            }
            foreach (KeyValuePair<string, double> kvp in GCpfB)
            {
                try
                {
                    netPositivePressureB.Add(kvp.Key, qh * (kvp.Value - PIP));
                netNegativePressureB.Add(kvp.Key, qh * (kvp.Value - NIP));
            }
                catch
            {
            }
        }
        }

        private static void CalculateExternalPressure()
        {
            double Z1 = 0;
            double Z2 = 0;
            double Z3 = 0;
            double Z4 = 0;
            double Z1E = 0;
            double Z2E = 0;
            double Z3E = 0;
            double Z4E = 0;

            if (roofAngle <= 5)
            {
                foreach (KeyValuePair<string, double> kvp in GCpfB)
                {
                    try
                    {
                        GCpfA.Add(kvp.Key, kvp.Value);
                    }
                    catch
                    {
                    }
                }
                return;
            }
            else if (roofAngle <= 20)
            {
                Z1 = (0.53 - 0.4) * (roofAngle - 5) / (20 - 5) + 0.4;
                Z2 = (-0.69 - (-0.69)) * (roofAngle - 5) / (20 - 5) + (-0.69);
                Z3 = (-0.48 - (-0.37)) * (roofAngle - 5) / (20 - 5) + (-0.37);
                Z4 = (-0.43 - (-0.29)) * (roofAngle - 5) / (20 - 5) + (-0.29);
                Z1E = (0.8 - 0.61) * (roofAngle - 5) / (20 - 5) + 0.61;
                Z2E = (-1.07 - (-1.07)) * (roofAngle - 5) / (20 - 5) + (-1.07);
                Z3E = (-0.69 - (-0.53)) * (roofAngle - 5) / (20 - 5) + (-0.53);
                Z4E = (-0.64 - (-0.43)) * (roofAngle - 5) / (20 - 5) + (-0.43);
            }
            else if (roofAngle <= 30)
            {
                Z1 = (0.56 - 0.53) * (roofAngle - 20) / (30 - 20) + 0.53;
                Z2 = (0.21 - (-0.69)) * (roofAngle - 20) / (30 - 20) + (-0.69);
                Z3 = (-0.43 - (-0.48)) * (roofAngle - 20) / (30 - 20) + (-0.48);
                Z4 = (-0.37 - (-0.43)) * (roofAngle - 20) / (30 - 20) + (-0.43);
                Z1E = (0.69 - 0.8) * (roofAngle - 20) / (30 - 20) + 0.8;
                Z2E = (0.27 - (-1.07)) * (roofAngle - 20) / (30 - 20) + (-1.07);
                Z3E = (-0.53 - (-0.69)) * (roofAngle - 20) / (30 - 20) + (-0.69);
                Z4E = (-0.48 - (-0.64)) * (roofAngle - 20) / (30 - 20) + (-0.64);
            }
            else if (roofAngle <= 45)
            {
                Z1 = 0.56;
                Z2 = 0.21;
                Z3 = 0.43;
                Z4 = -0.37;
                Z1E = 0.69;
                Z2E = 0.27;
                Z3E = -0.53;
                Z4E = -0.48;
            }
            else
            {
                Z1 = 0.56;
                Z2 = ((0.56 - 0.21) * (roofAngle - 45) / (90 - 45) + 0.21);
                Z3 = (-0.37 - (-0.43)) * (roofAngle - 45) / (90 - 45) + (-0.43);
                Z4 = -0.37;
                Z1E = 0.69;
                Z2E = ((0.69 - 0.27) * (roofAngle - 45) / (90 - 45) + 0.27);
                Z3E = (-0.48 - (-0.53)) * (roofAngle - 45) / (90 - 45) + (-0.53);
                Z4E = -0.48;
            }
            GCpfA.Add("Wall Zone 1", Z1);
            GCpfA.Add("Roof Zone 2", Z2);
            GCpfA.Add("Roof Zone 3", Z3);
            GCpfA.Add("Wall Zone 4", Z4);
            GCpfA.Add("Wall Zone 1E", Z1E);
            GCpfA.Add("Roof Zone 2E", Z2E);
            GCpfA.Add("Roof Zone 3E", Z3E);
            GCpfA.Add("Wall Zone 4E", Z4E);
        }

        private static double CalculateEndZoneWidths(float LBuilding, float WBuilding)
        {
            float minLW = Math.Min(WBuilding, LBuilding);
            return a = Math.Max(3, Math.Max(0.04 * minLW, Math.Min(0.4 * meanRoofHeight, 0.1 * minLW))); //Needs revision
        }

        private static void CalculateCoefficients(float WindSpeed, ExposureCategory exp, float Kzt, float Kd)
        {
            switch (exp)
            {
                case ExposureCategory.B:
                    alpha = 7f;
                    zg = 365.76f;
                    if (meanRoofHeight < 9.1)
                    {
                        kh = 2.1 * Math.Pow((9.1 / zg), (2 / alpha));
                    }
                    else
                    {
                        kh = 2.1 * Math.Pow((meanRoofHeight / zg), (2 / alpha));
                    }
                    break;
                case ExposureCategory.C:
                    alpha = 9.5f;
                    zg = 274.32f;
                    if (meanRoofHeight < 4.6)
                    {
                        kh = 2.1 * Math.Pow((4.6 / zg), (2 / alpha));
                    }
                    else
                    {
                        kh = 2.1 * Math.Pow((meanRoofHeight / zg), (2 / alpha));
                    }
                    break;
                case ExposureCategory.D:
                    alpha = 11.5f;
                    zg = 213.36f;
                    if (meanRoofHeight < 4.6)
                    {
                        kh = 2.1 * Math.Pow((4.6 / zg), (2 / alpha));
                    }
                    else
                    {
                        kh = 2.1 * Math.Pow((meanRoofHeight / zg), (2 / alpha));
                    }
                    break;
                default:
                    break;
            }
            qh = (0.613 * kh * Kzt * Kd * (WindSpeed * 1000 / 3600) * (WindSpeed * 1000 / 3600)) / 1000;
        }

        private static void CalculateInternalPressure(bool isEnclosed)
        {
            if (isEnclosed == true)
            {
                PIP = 0.18f;
                NIP = -0.18f;
            }
            else
            {
                PIP = 0.55f;
                NIP = -0.55f;
            }
        }
        /// <summary>
        /// Checks whether the building is considered low rise or high rise
        /// </summary>
        /// <param name="LBuilding">Building Length,Parallel to Building Ridge</param>
        /// <param name="WBuilding">Building Width, Normal to building ridge</param>
        /// <returns>true for low rise building, false for high rise</returns>
        public static bool IsLowRise(float LBuilding, float WBuilding)
        {
            if (meanRoofHeight <= Math.Max(18, Math.Min(WBuilding, LBuilding)))
            {
                return isLowRise = true;
            }
            else
            {
                return isLowRise = false;
            }
        }

        /// <summary>
        /// Calculates mean roof height
        /// </summary>
        /// <param name="Hridge">Ridge Height</param>
        /// <param name="Heave">Eave Height</param>
        /// <returns>mean roof height</returns>
        public static double CalculateMeanRoofHeight(float Hridge, float Heave)
        {
            if (roofAngle <= 10)
            {
                meanRoofHeight = Heave;
            }
            else
            {
                meanRoofHeight = Heave + (Hridge - Heave) / 2;
            }
            return meanRoofHeight;
        }

        /// <summary>
        /// Calculates the Angle of the building roof measured from the +ve X-Axis, Units consistency is mandatory
        /// </summary>
        /// <param name="Hridge">Ridge Height</param>
        /// <param name="Heave">Eave Height</param>
        /// <param name="WBuilding">Building Width, Normal to building ridge</param>
        /// <param name="roof">Roof Type</param>
        /// <returns>the Angle of the building roof in radian</returns>
        public static double CalculateRoofAngle(float Hridge, float Heave, float WBuilding, RoofType roof)
        {
            if (roof == RoofType.Gable)
            {
                roofAngle = Math.Atan((Hridge - Heave) / (WBuilding / 2));
            }
            else if (roof == RoofType.Monoslope)
            {
                roofAngle = Math.Atan((Hridge - Heave) / WBuilding);
            }
            return roofAngle;
        }
    }
    public static class BoltV3
    {
        /// <summary>
        /// Generates STAAD file according to buildind geometry and applied loads
        /// </summary>
        /// <param name="File_Path">directory to save generated file inside</param>
        /// <param name="DL">Dead Load</param>
        /// <param name="CL"></param>
        /// <param name="LL">Live Load</param>
        /// <param name="Bay">Bay Spacing</param>
        /// <param name="B_W">Building Width</param>
        /// <param name="B_L">Building Length</param>
        /// <param name="E_HT">Eave Height</param>
        /// <param name="R_HT">Ridge Height</param>
        /// <param name="Roof_Type"></param>
        /// <param name="WindSpeed"></param>
        /// <param name="TopoFactor">Topographic Factor</param>
        /// <param name="DirectFator"></param>
        /// <param name="RiskCategory"></param>
        /// <param name="ExposureCategory"></param>
        /// <param name="IsEnclosed"></param>
        /// <param name="IsHuricane"></param>
        /// <returns>True if the generated successfully</returns>
        public static bool GenerateFile(string File_Path, float DL, float CL, float LL, float Bay, float B_W, float B_L, float E_HT, float R_HT, RoofType Roof_Type, float WindSpeed, float TopoFactor,
            float DirectFator, RiskCategory RiskCategory,
            ExposureCategory ExposureCategory, bool IsEnclosed, bool IsHuricane)
        {
            double WL_1, WL_2, WL_3, WL_4, WL_1N, WL_2N, WL_3N, WL_4N, WL_1_B, WL_2_B, WL_1_BN, WL_2_BN;

            ASCE107Wind.LoadParameters(WindSpeed, RiskCategory, ExposureCategory, R_HT, E_HT, B_L, B_W, Roof_Type, TopoFactor, DirectFator, IsEnclosed, IsHuricane);
            //Check_1 = txt_Check_1.Text;
            //Check_2 = txt_Check_2.Text;
            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 1", out WL_1);
            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 2", out WL_2);
            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 3", out WL_3);
            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 4", out WL_4);

            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 1", out WL_1N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 2", out WL_2N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 3", out WL_3N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 4", out WL_4N);

            ASCE107Wind.netPositivePressureB.TryGetValue("Roof Zone 2", out WL_1_B);
            ASCE107Wind.netPositivePressureB.TryGetValue("Wall Zone 5", out WL_2_B);

            ASCE107Wind.netNegativePressureB.TryGetValue("Roof Zone 2", out WL_1_BN);
            ASCE107Wind.netNegativePressureB.TryGetValue("Wall Zone 5", out WL_2_BN);




            if (IsLowRise(B_L, B_W))
            {
                StreamWriter Writer = new StreamWriter(File_Path.ToString());
                Writer.WriteLine("*CEI 36 - ITI");
                Writer.WriteLine("STAAD PLANE");
                Writer.WriteLine("INPUT WIDTH 79");
                Writer.WriteLine("UNIT METER KN");
                Writer.WriteLine("JOINT COORDINATES");
                Writer.WriteLine("1 0 0");
                Writer.WriteLine("2 0 " + E_HT);
                //Marks Edit 
                if (Roof_Type == RoofType.Gable)
                {
                    Writer.WriteLine("3 " + B_W / 2 + " " + R_HT);
                    Writer.WriteLine("4 " + B_W + " " + E_HT);
                }
                else if (Roof_Type == RoofType.Monoslope)
                {
                    Writer.WriteLine("3 " + B_W / 2 + " " + (R_HT + E_HT) / 2);
                    Writer.WriteLine("4 " + B_W + " " + R_HT);
                }
                //   Writer.WriteLine("3 " + B_W/2 + " "  + R_HT); 
                //   Writer.WriteLine("4" + B_W + " " + E_HT);
                Writer.WriteLine("5 " + B_W + " " + "0" + "\n");
                Writer.WriteLine("MEMBER INCIDENCES");
                Writer.WriteLine("1 2 1");
                Writer.WriteLine("2 2 3");
                Writer.WriteLine("3 4 3");
                Writer.WriteLine("4 4 5");
                Writer.WriteLine("DEFINE MATERIAL START");
                Writer.WriteLine("ISOTROPIC STEEL");
                Writer.WriteLine("E 205000000");
                Writer.WriteLine("Poisson 0.3");
                Writer.WriteLine("DENSITY 76.8195");
                Writer.WriteLine("ALPHA 0.000012");
                Writer.WriteLine("DAMP 0.03");
                Writer.WriteLine("END DEFINE MATERIAL");
                Writer.WriteLine("UNIT MMS KN");
                Writer.WriteLine("MEMBER PROPERTY AMERICAN");
                Writer.WriteLine("1 TAPERED 770 5 270 200 10 200 10");
                Writer.WriteLine("2 TAPERED 770 5 520 175 10 175 10");
                Writer.WriteLine("3 TAPERED 770 5 520 175 10 175 10");
                Writer.WriteLine("4 TAPERED 770 5 270 200 10 200 10");
                Writer.WriteLine("UNIT METER KN");
                Writer.WriteLine("CONSTANTS");
                Writer.WriteLine("MATERIAL STEEL ALL");
                Writer.WriteLine("SUPPORTS");
                Writer.WriteLine("1 5 " + "PINNED");
                Writer.WriteLine("LOAD 1 LOADTYPE None  TITLE DL");
                Writer.WriteLine("SELFWEIGHT Y -1");
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("1 2 3 4 UNI GY " + (DL * Bay * (-1)));
                Writer.WriteLine("LOAD 2 LOADTYPE None  TITLE CL");
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("2 3 UNI GY " + (CL * Bay * (-1)));
                Writer.WriteLine("LOAD 3 LOADTYPE None  TITLE LL");
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("2 3 UNI GY " + (LL * Bay * (-1)));
                Writer.WriteLine("******** WIND LOAD CASES");
                Writer.WriteLine("LOAD 4 LOADTYPE None  TITLE WLB");
                Writer.WriteLine("MEMBER Load");
                if (WL_1 < 0)
                {
                    Writer.WriteLine("1 UNI GX " + (Math.Abs(WL_1) * Bay * (-1)));

                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_1) * Bay);
                }
                if (WL_2 < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2) * Bay * (-1));
                }
                if (WL_3 < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3) * Bay * (-1));
                }
                if (WL_4 < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4) * Bay * (-1));
                }
                Writer.WriteLine("LOAD 5 LOADTYPE None  TITLE WLU");
                Writer.WriteLine("MEMBER Load");
                if (WL_1N < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_1N) * Bay * (-1));
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_1N) * Bay);
                }
                if (WL_2N < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2N) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2N) * Bay * (-1));
                }
                if (WL_3N < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3N) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3N) * Bay * (-1));
                }
                if (WL_4N < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4N) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4N) * Bay * (-1));
                }
                Writer.WriteLine("LOAD 6 LOADTYPE None  TITLE WRB");
                Writer.WriteLine("MEMBER Load");
                if (WL_4 < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4) * (-1) * Bay);
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4) * Bay);
                }
                if (WL_3 < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3) * Bay * (-1));
                }
                if (WL_2 < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2) * Bay * (-1));
                }
                if (WL_1 < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1) * Bay * (-1));
                }
                Writer.WriteLine("LOAD 7 LOADTYPE None  TITLE WRU");
                Writer.WriteLine("MEMBER Load");
                if (WL_4N < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4N) * (-1) * Bay);
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4N) * Bay);
                }
                if (WL_3N < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3N) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3N) * Bay * (-1));
                }
                if (WL_2N < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2N) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2N) * Bay * (-1));
                }
                if (WL_1N < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1N) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1N) * Bay * (-1));
                }
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("LOAD 8 LOADTYPE None  TITLE WEB");
                Writer.WriteLine("MEMBER Load");
                if (WL_2_B < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_B) * (-1) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_B) * Bay);
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_B) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_B) * Bay * (-1));
                }
                if (WL_1_B < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_B) * Bay);
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_B) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_B) * Bay * (-1));
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_B) * Bay * (-1));
                }

                Writer.WriteLine("LOAD 9 LOADTYPE None  TITLE WEU");
                Writer.WriteLine("MEMBER Load");
                if (WL_2_BN < 0)
                {

                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_BN) * (-1) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_BN) * Bay);

                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_BN) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_BN) * Bay * (-1));

                }
                if (WL_1_BN < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_BN) * Bay);
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_BN) * Bay);

                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_BN) * Bay * (-1));
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_BN) * Bay * (-1));
                }
                Writer.WriteLine("LOAD COMB 1000 DL+LL");
                Writer.WriteLine("1 1.0 3 1.0");
                Writer.WriteLine("LOAD COMB 1001 DL+CL");
                Writer.WriteLine("1 1.0 2 1.0");
                Writer.WriteLine("LOAD COMB 1002 DL+CL+LL");
                Writer.WriteLine("1 1.0 2 1.0 3 1.0");
                Writer.WriteLine("LOAD COMB 1003 DL+WLB");
                Writer.WriteLine("1 1.0 4 1.0");
                Writer.WriteLine("LOAD COMB 1004 DL+WLU");
                Writer.WriteLine("1 1.0 5 1.0");
                Writer.WriteLine("LOAD COMB 1005 DL+WRB");
                Writer.WriteLine("1 1.0 6 1.0");
                Writer.WriteLine("LOAD COMB 1006 DL+WRU");
                Writer.WriteLine("1 1.0 7 1.0");
                Writer.WriteLine("LOAD COMB 1007 DL+WEB");
                Writer.WriteLine("1 1.0 8 1.0");
                Writer.WriteLine("LOAD COMB 1008 DL+WEU");
                Writer.WriteLine("1 1.0 9 1.0");
                Writer.WriteLine("LOAD COMB 1009 DL+CL+WLB");
                Writer.WriteLine("1 1.0 2 1.0 4 1.0");
                Writer.WriteLine("LOAD COMB 1010 DL+CL+WLU");
                Writer.WriteLine("1 1.0 2 1.0 5 1.0");
                Writer.WriteLine("LOAD COMB 1011 DL+CL+WRB");
                Writer.WriteLine("1 1.0 2 1.0 6 1.0");
                Writer.WriteLine("LOAD COMB 1012 DL+CL+WRU");
                Writer.WriteLine("1 1.0 2 1.0 7 1.0");
                Writer.WriteLine("LOAD COMB 1013 DL+CL+WEB");
                Writer.WriteLine("1 1.0 2 1.0 8 1.0");
                Writer.WriteLine("LOAD COMB 1014 DL+CL+WEU");
                Writer.WriteLine("1 1.0 2 1.0 9 1.0");
                Writer.WriteLine("PERFORM ANALYSIS PRINT ALL");
                Writer.WriteLine("LOAD LIST 1000 TO 1014");
                Writer.WriteLine("Parameter 1");
                Writer.WriteLine("CODE AISC UNIFIED 2005");
                Writer.WriteLine("Method ASD");
                Writer.WriteLine("FYLD 345000 ALL");
                Writer.WriteLine("BEAM 1 ALL");
                Writer.WriteLine("*TRACK 2 ALL");
                Writer.WriteLine("CB 0 ALL");
                Writer.WriteLine("****LZ,KZ");
                Writer.WriteLine("KZ 1.5 MEMB 1 4");
                Writer.WriteLine("LZ " + B_W + " MEMB 2 3");
                Writer.WriteLine("KY 1.0 MEMB 1 4");
                Writer.WriteLine("LY 1.5 MEMB 2 3");
                Writer.WriteLine("UNB " + E_HT + " MEMB 1 4");
                Writer.WriteLine("UNB 1.5 MEMB 2 3");
                Writer.WriteLine("UNT " + E_HT + " MEMB 1 4");
                Writer.WriteLine("UNT 1.5 MEMB 2 3");
                Writer.WriteLine("****************************** CHECK CODE AND WEIGHT");
                Writer.WriteLine("CHECK CODE ALL");
                Writer.WriteLine("UNIT METER KG");
                Writer.WriteLine("STEEL TAKE OFF ALL");
                Writer.WriteLine("FINISH");
                Writer.Close();
                return true;
                //return MessageBox.Show("STAAD FILE IS GENERATED AND SAVED SUCCESSFULLY");
            }
            else
            {
                return false;
                //return MessageBox.Show("Building is NOT Low Rise");
            }
        }
    }
}

