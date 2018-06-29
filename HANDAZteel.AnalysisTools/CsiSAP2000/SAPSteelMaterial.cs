using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP2000v18;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPSteelMaterial:SAPMaterial
    {
        public double Fy { get; set; }
        public double Fu { get; set; }
        public double eFy { get; set; }
        public double eFu { get; set; }
        public static object Steel52 { get; internal set; }

        //TODO Constructor that takes these parameters

        //public SAPSteelMaterial(string matName, double weight, double elasticityModulus, double poissonRatio, double thermalCoef):base(matName,eMatType.Steel, weight, elasticityModulus, poissonRatio, thermalCoef)
        //{

        //}
        public SAPSteelMaterial(string matName):base(matName,eMatType.Steel)
        {

        }

        public SAPSteelMaterial(string matName, double weight, double elasticityModulus, double poissonRatio, double thermalCoef,double fy, double fu, double eFy, double eFu) : base(matName, eMatType.Steel, weight, elasticityModulus, poissonRatio, thermalCoef)
        {
            Fy = fy;
            Fu = fu;
            this.eFy = eFy;
            this.eFu = eFu;
        }

        /// <summary>
        /// Sets default steel material characteristics but the design parameters are left to the user
        /// </summary>
        /// <param name="matName"></param>
        /// <param name="fy"></param>
        /// <param name="fu"></param>
        /// <param name="eFy"></param>
        /// <param name="eFu"></param>
        public SAPSteelMaterial(string matName, double fy, double fu, double eFy, double eFu) : base(matName, eMatType.Steel)
        {
            Fy = fy;
            Fu = fu;
            this.eFy = eFy;
            this.eFu = eFu;
        }
    }
}
