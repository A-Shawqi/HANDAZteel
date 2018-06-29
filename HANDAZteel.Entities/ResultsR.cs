using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.PEB.Entities
{
   public class ResultsR
    {
        public double FX { get; set; }
        public double FY { get; set; }
        public double FZ { get; set; }
        public double MX { get; set; }
        public double MY { get; set; }
        public double MZ { get; set; }
        public double KY { get; set; }
        public double Kz { get; set; }

        public ResultsR(double _FX, double _FY, double _FZ, double _MX, double _MY, double _MZ, double _KY, double _Kz)
        {
            this.FX = _FX;    
            this.FY = _FY;    
            this.FZ = _FX;    
            this.MX = _MX;    
            this.MY = _MY;    
            this.MZ = _MZ;    
            this.Kz = _Kz;
            this.KY = _KY;

        }
    }                
}                      
                      
                       