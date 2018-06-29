namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProMaterial
    {
        public string Name { get; set; }
        public double E { get; set; }
        //public double G { get; set; }
        public double Poisson { get; set; }
        public double Density { get; set; }
        public double Alpha { get; set; }
        public double DAMP { get; set; }
        
        public static STAADProMaterial SteelMaterial()
        {
            STAADProMaterial mat = new STAADProMaterial();
            mat.Name = "STEEL";
            mat.Density = 7.85;
            mat.E = 2100;
            mat.Poisson = 0.3;
            mat.DAMP = 0.03;
            mat.Alpha = 1.170E-05;

            return mat;
        }
    }
}