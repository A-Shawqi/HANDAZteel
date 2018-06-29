using HANDAZ.Entities;
using SAP2000v18;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public abstract class SAPMaterial:ISAPAPIComponent
    {
        public bool IsDefinedInSAP { get; set; } = false;
        public SAPMaterial(string matName, eMatType matType, double weight, double elasticityModulus, double poissonRatio, double thermalCoef)
        {
            MatName = matName;
            MatType = matType;
            Weight = weight;
            ElasticityModulus = elasticityModulus;
            PoissonRatio = poissonRatio;
            ThermalCoef = thermalCoef;
        }
        /// <summary>
        /// Default constructor assigns basic material attribute based on material type
        /// </summary>
        /// <param name="matName"></param>
        /// <param name="matType"></param>
        public SAPMaterial(string matName, eMatType matType)
        {
            MatName = matName;
            switch (matType)
            {
                case eMatType.Steel:
                    Weight = 7.85;
                    ElasticityModulus = 2100;
                    PoissonRatio = 0.3;
                    ThermalCoef = 0.0000117;
                    break;
                case eMatType.Concrete:
                    Weight = 2.5;
                    ElasticityModulus = 240;
                    PoissonRatio = 0.2;
                    ThermalCoef = 0.0000099;
                    break;
                case eMatType.NoDesign:
                    break;
                case eMatType.Aluminum:
                    break;
                case eMatType.ColdFormed:
                    break;
                case eMatType.Rebar:
                    break;
                case eMatType.Tendon:
                    break;
                case eMatType.Masonry:
                    break;
                default:
                    break;
            }
        }
        public string MatName { get; set; }
        public eMatType MatType { get; set; }
        public double Weight { get; set; }
        public double ElasticityModulus { get; set; }
        public double PoissonRatio { get; set; }
        public double ThermalCoef { get; set; }

        public SAPMaterial ConvertFromHndzMaterial(HndzStructuralMaterial material)
        {
            SAPMaterial sapMaterial = null;
            switch (material.MatType)
            {
                case HndzMaterialType.Steel:
                    HndzSteelMaterial matS = (HndzSteelMaterial)material;
                    sapMaterial = new SAPSteelMaterial(matS.Name, matS.Weight, matS.ElasticityModulus, matS.PoissonRatio, matS.ThermalCoef, matS.Fy, matS.Fu, matS.eFy, matS.eFu);
                    break;
                case HndzMaterialType.Concrete:
                    //HndzConcreteMaterial matC = (HndzConcreteMaterial)material;
                    //sapMaterial = new SAPConcreteMaterial(matS.Name, matS.Weight, matS.ElasticityModulus, matS.PoissonRatio, matS.ThermalCoef, matS.Fy, matS.Fu, matS.eFy, matS.eFu);
                    //TODO
                    throw new NotImplementedException();
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

            return sapMaterial;
        }

        internal HndzStructuralMaterial ConvertToHndzMaterial()
        {
            HndzStructuralMaterial material = null;
            switch (material.MatType)
            {
                case HndzMaterialType.Steel:
                    //TODO: cast from steel handaz to sap steel
                    material = new HndzSteelMaterial(MatName, Weight, ElasticityModulus, PoissonRatio, ThermalCoef);
                    break;
                case HndzMaterialType.Concrete:
                    throw new NotImplementedException();
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

            return material;
        }
    }
}
