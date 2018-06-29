namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPJointElementResults
    {

        public SAPJointElementResults()
        {
            obj = new string[1];
            elment = new string[1];
            loadCase = new string[1];
            stepType = new string[1];
            stepNum = new double[1];
            f1 = new double[1];
            f2 = new double[1];
            f3 = new double[1];
            m1 = new double[1];
            m2 = new double[1];
            m3 = new double[1];
        }
        internal int numberResults;
        internal string[] obj;
        internal string[] elment;
        internal string[] loadCase;
        internal string[] stepType;
        internal double[] stepNum;
        internal double[] f1;
        internal double[] f2;
        internal double[] f3;
        internal double[] m1;
        internal double[] m2;
        internal double[] m3;

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

        public string[] Obj
        {
            get
            {
                return obj;
            }

            internal set
            {
                obj = value;
            }
        }

        public string[] Elment
        {
            get
            {
                return elment;
            }

            internal set
            {
                elment = value;
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

        public double[] F1
        {
            get
            {
                return f1;
            }

            internal set
            {
                f1 = value;
            }
        }

        public double[] F2
        {
            get
            {
                return f2;
            }

            internal set
            {
                f2 = value;
            }
        }

        public double[] F3
        {
            get
            {
                return f3;
            }

            internal set
            {
                f3 = value;
            }
        }

        public double[] M1
        {
            get
            {
                return m1;
            }

            internal set
            {
                m1 = value;
            }
        }

        public double[] M2
        {
            get
            {
                return m2;
            }

            internal set
            {
                m2 = value;
            }
        }

        public double[] M3
        {
            get
            {
                return m3;
            }

            internal set
            {
                m3 = value;
            }
        }
    }
}