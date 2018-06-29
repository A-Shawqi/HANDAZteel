namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    /// <summary>
    /// Temporary Class to hold results
    /// </summary>
    public class SAPAnalysisResults
    {
        public SAPAnalysisResults()
        {
            FrameText = new string[1];
            Shear3 = new double[1];
            Station = new double[1];
            ElementText = new string[1];
            ElementStation = new double[1];
            LoadCase = new string[1];
            StepType = new string[1];
            StepNum = new double[1];
            Axial = new double[1];
            Shear2 = new double[1];
            TortionalMoment = new double[1];
            Moment2 = new double[1];
            Moment3 = new double[1];
            
        }
        internal int numberResults;
        internal string[] frameText;
        internal double[] station;
        internal string[] elementText;
        internal double[] elementStation;
        internal string[] loadCase;
        internal string[] stepType;
        internal double[] stepNum;
        internal double[] axial;
        internal double[] shear2;
        internal double[] shear3;
        internal double[] tortionalMoment;
        internal double[] moment2;
        internal double[] moment3;

        public int NumberResults
        {
            get
            {
                return numberResults;
            }

            internal set
            {
                numberResults = value;
            }
        }

        public string[] FrameText
        {
            get
            {
                return frameText;
            }

            internal set
            {
                frameText = value;
            }
        }

        public double[] Station
        {
            get
            {
                return station;
            }

            internal set
            {
                station = value;
            }
        }

        public string[] ElementText
        {
            get
            {
                return elementText;
            }

            internal set
            {
                elementText = value;
            }
        }

        public double[] ElementStation
        {
            get
            {
                return elementStation;
            }

            internal set
            {
                elementStation = value;
            }
        }

        public string[] LoadCase
        {
            get
            {
                return loadCase;
            }

            internal set
            {
                loadCase = value;
            }
        }

        public string[] StepType
        {
            get
            {
                return stepType;
            }

            internal set
            {
                stepType = value;
            }
        }

        public double[] StepNum
        {
            get
            {
                return stepNum;
            }

            internal set
            {
                stepNum = value;
            }
        }

        public double[] Axial
        {
            get
            {
                return axial;
            }

            internal set
            {
                axial = value;
            }
        }

        public double[] Shear2
        {
            get
            {
                return shear2;
            }

            internal set
            {
                shear2 = value;
            }
        }

        public double[] Shear3
        {
            get
            {
                return shear3;
            }

            internal set
            {
                shear3 = value;
            }
        }

        public double[] TortionalMoment
        {
            get
            {
                return tortionalMoment;
            }

            internal set
            {
                tortionalMoment = value;
            }
        }

        public double[] Moment2
        {
            get
            {
                return moment2;
            }

            internal set
            {
                moment2 = value;
            }
        }

        public double[] Moment3
        {
            get
            {
                return moment3;
            }

            internal set
            {
                moment3 = value;
            }
        }
    }
}