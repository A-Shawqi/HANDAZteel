using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.PEB.Entities
{
    public class Material
    {
        string materialName;
        double unitWeight;

        double youngsModulas;
        double poissionRatio;
        double shearModulas;
        double thermalExpansion; 

        double dampingRatio;

        double charchteristicResistance;
        double designResistance;
        double shearReductionFactor;
        double tensionLimitstress;

        public string MaterialName
        {
            get
            {
                return materialName;
            }

            set
            {
                materialName = value;
            }
        }

        public double UnitWeight
        {
            get
            {
                return unitWeight;
            }

            set
            {
                unitWeight = value;
            }
        }

        public double YoungsModulas
        {
            get
            {
                return youngsModulas;
            }

            set
            {
                youngsModulas = value;
            }
        }

        public double PoissionRatio
        {
            get
            {
                return poissionRatio;
            }

            set
            {
                poissionRatio = value;
            }
        }

        public double ShearModulas
        {
            get
            {
                return shearModulas;
            }

            set
            {
                shearModulas = value;
            }
        }

        public double ThermalExpansion
        {
            get
            {
                return thermalExpansion;
            }

            set
            {
                thermalExpansion = value;
            }
        }

        public double DampingRatio
        {
            get
            {
                return dampingRatio;
            }

            set
            {
                dampingRatio = value;
            }
        }

        public double CharchteristicResistance
        {
            get
            {
                return charchteristicResistance;
            }

            set
            {
                charchteristicResistance = value;
            }
        }
        public double DesignResistance
        {
            get
            {
                return designResistance;
            }

            set
            {
                designResistance = value;
            }
        }

        public double ShearReductionFactor
        {
            get
            {
                return shearReductionFactor;
            }

            set
            {
                shearReductionFactor = value;
            }
        }

        public double TensionLimitstress
        {
            get
            {
                return tensionLimitstress;
            }

            set
            {
                tensionLimitstress = value;
            }
        }
        public Material(string _materialName, double _unitWeight, double _youngsModulas, double _poissionRatio , double _shearModulas
       , double _thermalExpansion, double _dampingRatio, double _charchteristicResistance, double _designResistance , double _shearReductionFactor, double _tensionLimitstress)
        {
            MaterialName = _materialName;
            UnitWeight = _unitWeight;
            YoungsModulas = _youngsModulas;
            PoissionRatio = _poissionRatio;
            ShearModulas = _shearModulas;
            ThermalExpansion = _thermalExpansion;
            DampingRatio = _dampingRatio;
            CharchteristicResistance = _charchteristicResistance;
            DesignResistance = _designResistance;
            ShearReductionFactor = _shearReductionFactor;
            TensionLimitstress = _tensionLimitstress;

        }
    }
}
