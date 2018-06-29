using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.Entities;
using HANDAZ.PEB.Core;

namespace HANDAZ.PEB.BusinessComponents
{
    #region Public Enumerations
    public enum LocationEnum {
        Cairo,
        Alexandria
    }
    public enum RoofSlopeEnum {
        From1To5,
        From1To10,
        From1To20
    }

    public enum RoofAccessibilityEnum {
        Accessible,
        Inaccessible
    }
    #endregion
    [Serializable]
    public class CustomerInputs
    {
        #region Basic Inputs
        public double Width { get; set; }
        public double Length { get; set; }
        public double EaveHeight { get; set; }
        private double baySpacing;
        private double ridgeHeight;

        public double RidgeHeight
        {
            get {
                //switch (RoofSlope)
                //{
                //    case HndzRoofSlopeEnum.From1To5:
                //        ridgeHeight =  EaveHeight + Width/2 * 0.2;
                //        break;
                //    case HndzRoofSlopeEnum.From1To10:
                //        ridgeHeight = EaveHeight + Width/2 * 0.1;
                //        break;
                //    case HndzRoofSlopeEnum.From1To20:
                //        ridgeHeight = EaveHeight + Width/2 * 0.05;
                //        break;
                //    default:
                //        break;
                //}

                return ridgeHeight; }
            set { ridgeHeight = value; }
        }


        public double BaySpacing
        {
            get { return baySpacing; }
            set
            {
                baySpacing = value;
                //if (baySpacing != 0)
                //{
                //    noFrames = (int)(Length / baySpacing) + 1;

                //}
                return;
            }
        }
        private int noFrames;
        public int NoFrames
        {
            get { return noFrames; }
            set {
                noFrames = value;
                //if (noFrames != 0)
                //{
                //    baySpacing = Length / noFrames;
                //}
                return;
            }
        }
        public HndzLocationEnum Location { get; set; }
        public HndzRoofSlopeEnum RoofSlope { get; set; }
        public HndzRoofAccessibilityEnum RoofAccessibility { get; set; }
        #endregion
        public AdvancedInputs AdvInputs { get; set; }
        public ASCE107Wind.RiskCategory RiskCategory { get; internal set; }
        public ASCE107Wind.ExposureCategory ExposureCategory { get; internal set; }
    }
}
