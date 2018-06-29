using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.Entities;
using HANDAZ.PEB.Core;
using static HANDAZ.PEB.Core.ASCE107Wind;

namespace HANDAZ.PEB.BusinessComponents
{
    class RobotLoadCalculator
    {
        public static double CoverLoad { get; set; }
        public static double WindLeft { get; set; }
        public static double WindRight { get; set; }
        public static double LiveLoad { get; set; }
        public static void GenrateLoads(HndzFrameSingleBay3D hndzFrame)
        {
            CustomerInputs inputs = new CustomerInputs();
            inputs.Width = hndzFrame.Width / 1000;
            inputs.Length = hndzFrame.Length / 1000;
            inputs.NoFrames = hndzFrame.FramesCount;
            inputs.BaySpacing = hndzFrame.BaySpacing / 1000;
            inputs.EaveHeight = hndzFrame.EaveHeight / 1000;
            inputs.RoofSlope = hndzFrame.RoofSlope;
            inputs.ExposureCategory = (ExposureCategory)hndzFrame.ExposureCategory;
            inputs.RiskCategory = (RiskCategory)hndzFrame.RiskCategory;
            inputs.RoofAccessibility = hndzFrame.RoofAccessibility;
            double roofSlope;
            switch (inputs.RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            CoverLoad = 0.01 * inputs.BaySpacing;
            LiveLoad = 0.057 * inputs.BaySpacing;
            double WL_1, WL_2, WL_3, WL_4, WL_1N, WL_2N, WL_3N, WL_4N, WL_1_B, WL_2_B, WL_1_BN, WL_2_BN;
            int windSpeed;
            switch (inputs.Location)
            {
                case HndzLocationEnum.Cairo:
                    windSpeed = 130;
                    break;
                case HndzLocationEnum.Alexandria:
                    windSpeed = 90;
                    break;
                case HndzLocationEnum.Matrouh:
                    windSpeed = 130;
                    break;
                case HndzLocationEnum.Aswan:
                    windSpeed = 130;
                    break;
                case HndzLocationEnum.Sinai:
                    windSpeed = 130;
                    break;
                default:
                    windSpeed = 130;
                    break;
            }
            ASCE107Wind.LoadParameters(windSpeed, (RiskCategory)inputs.RiskCategory, (ExposureCategory)inputs.ExposureCategory, (float)inputs.RidgeHeight / 1000, (float)inputs.EaveHeight, (float)inputs.Length, (float)inputs.Width, RoofType.Gable, 1, 0.85f, true, false); //TODO: Make it not hard coded
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

            //TODO: Temp conversion from kn to Ton .... till u implement units conversion
            WL_1 = WL_1 / 10;
            WL_2 = WL_2 / 10;
            WL_3 = WL_3 / 10;
            WL_4 = WL_4 / 10;

            WL_1N = WL_1N / 10;
            WL_2N = WL_2N / 10;
            WL_3N = WL_3N / 10;
            WL_4N = WL_4N / 10;

            WL_1_B = WL_1_B / 10;
            WL_2_B = WL_2_B / 10;

            WL_1_BN = WL_1_BN / 10;
            WL_2_BN = WL_2_BN / 10;


        }
    }
}
