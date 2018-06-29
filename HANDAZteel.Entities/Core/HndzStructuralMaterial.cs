using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    public abstract class HndzStructuralMaterial:HndzMaterial
    {
        /// <summary>
        /// Default constructor assigns basic material attribute based on material type
        /// </summary>
        /// <param name="matName"></param>
        /// <param name="matType"></param>
        public HndzStructuralMaterial(string matName, HndzMaterialType matType):base(matName)
        {
            switch (matType)
            {
                case HndzMaterialType.Steel:
                    Weight = 7.85;
                    ElasticityModulus = 2100;
                    PoissonRatio = 0.3;
                    ThermalCoef = 1.170E-05;
                    break;
                case HndzMaterialType.Concrete:
                    Weight = 2.5;
                    ElasticityModulus = 240;
                    PoissonRatio = 0.2;
                    ThermalCoef = 9.900E-06;
                    break;
                case HndzMaterialType.NoDesign:
                    break;
                case HndzMaterialType.Aluminum:
                    break;
                case HndzMaterialType.ColdFormed:
                    break;
                case HndzMaterialType.Rebar:
                    break;
                case HndzMaterialType.Tendon:
                    break;
                case HndzMaterialType.Masonry:
                    break;
                default:
                    break;
            }
        }
        public HndzStructuralMaterial(string matName, HndzMaterialType matType, double weight, double elasticityModulus, double poissonRatio, double thermalCoef):base(matName)
        {
            MatType = matType;
            Weight = weight;
            ElasticityModulus = elasticityModulus;
            PoissonRatio = poissonRatio;
            ThermalCoef = thermalCoef;
        }
        public double Weight { get; set; }
        public double ElasticityModulus { get; set; }
        public double PoissonRatio { get; set; }
        public double ThermalCoef { get; set; }
    }
}
