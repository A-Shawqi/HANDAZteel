using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;
using static HANDAZ.Core.Designers.CrossSectionCalulator;

namespace HANDAZ.PEB.Core.Designers
{
   public static class BeamSectionCalculator
        
    {
        //our beam is lateraly unsupported ---
        // the purlin is supported 
        // the rafter is not supported 
        // normal stress 
        // Check Deflection 
        //calculate loads and Redesign this shit 
        //
        private static double appliedMomentMx;
        private static double appliedMomentMy;

        public static HndzAnalysisResults AppliedLoads { get; private set; }
        public static double AppliedMomentMX
        {
            get { return AppliedLoads.Moment3[0]; }
            set { appliedMomentMx = value; }
        }
        public static double AppliedMomentMy
        {
            get { return AppliedLoads.Moment2[0]; }
            set { appliedMomentMy = value; }
        }
        public static double Fy { get; set; }
        //public static SectionI CalculateBeamSection(HndzAnalysisResults _AppliedLoads , SteelGrade Grade ,double _BeamLength)
        //{
        //    AppliedLoads = _AppliedLoads;
        //    switch (Grade)
        //    {
        //        case SteelGrade.st37:
        //            Fy = 2.4;
        //            break;
        //        case SteelGrade.st44:
        //            Fy = 2.8;
        //            break;
        //        case SteelGrade.st52:
        //            Fy = 3.6;
        //            break;
        //        default:
        //            break;
        //    }
        //    //Zxrequired
        //    //double Mx = _AppliedLoads.Moment3[0];
        //    //double My = _AppliedLoads.Moment2[0];
        //    double ZxBeam = AppliedMomentMX / (0.64 * Fy) + ( 8 * AppliedMomentMy / (0.72 * Fy));
        //    //HEight Comes back in mm 
        //    double HwebRequired = _BeamLength / 40;
        //    //the momebt would be the moment coming from 




        //}
    }
}
