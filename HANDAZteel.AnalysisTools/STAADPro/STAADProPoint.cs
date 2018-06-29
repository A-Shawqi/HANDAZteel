using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProPoint
    {
        /// <summary>
        /// The name should be passed as a reference, and thus it has to be visible in SAP2000API
        /// </summary>
        private int number;
        private static int counter=0;

        public STAADProPoint(double x, double y, double z):this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public STAADProPoint()
        {
            Number = ++counter;
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool IsSTAADDefined { get; internal set; } = false;
    }
}
