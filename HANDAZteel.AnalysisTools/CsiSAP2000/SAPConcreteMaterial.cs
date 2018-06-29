using SAP2000v18;
namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPConcreteMaterial : SAPMaterial
    {
        private string name;
        private double fc;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double Fc
        {
            get
            {
                return fc;
            }

            set
            {
                fc = value;
            }
        }

        public SAPConcreteMaterial(string name, double fc):base(name,eMatType.Concrete)
        {
            this.Name = name;
            this.Fc = fc;
        }
    }
}