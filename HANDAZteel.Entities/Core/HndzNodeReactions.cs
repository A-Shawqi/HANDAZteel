namespace HANDAZ.Entities
{
    public class HndzNodeReactions
    {
        private string[] loadCase;
        private double[] f1;
        private double[] f2;
        private double[] f3;
        private double[] m1;
        private double[] m2;
        private double[] m3;

        public HndzNodeReactions(string[] loadCase, double[] f1, double[] f2, double[] f3, double[] m1, double[] m2, double[] m3)
        {
            this.loadCase = loadCase;
            this.f1 = f1;
            this.f2 = f2;
            this.f3 = f3;
            this.m1 = m1;
            this.m2 = m2;
            this.m3 = m3;
        }

        public string[] LoadCase
        {
            get
            {
                return loadCase;
            }

            set
            {
                loadCase = value;
            }
        }

        public double[] F1
        {
            get
            {
                return f1;
            }

            set
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

            set
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

            set
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

            set
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

            set
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

            set
            {
                m3 = value;
            }
        }
    }
}