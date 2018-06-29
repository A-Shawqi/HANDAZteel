using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    public class HndzSteelMaterial:HndzStructuralMaterial
    {
        public double Fy { get; set; }
        public double Fu { get; set; }
        public double eFy { get; set; }
        public double eFu { get; set; }
        public HndzSteelMaterial(string matName):base(matName,HndzMaterialType.Steel)
        {

        }

        public HndzSteelMaterial(string matName, double weight, double elasticityModulus, double poissonRatio, double thermalCoef, double fy, double fu, double eFy, double eFu) : base(matName, HndzMaterialType.Steel, weight, elasticityModulus, poissonRatio, thermalCoef)
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
        public HndzSteelMaterial(string matName, double fy, double fu, double eFy, double eFu) : base(matName, HndzMaterialType.Steel)
        {
            Fy = fy;
            Fu = fu;
            this.eFy = eFy;
            this.eFu = eFu;
        }
    }
}
