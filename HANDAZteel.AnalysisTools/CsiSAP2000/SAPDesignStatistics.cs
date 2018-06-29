namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPDesignStatistics
    {
        public SAPDesignStatistics()
        {
            NoVerified = 0;
            NoFailed = 0;
            NoUnchecked = 0;
            FailedElementsNames = new string[1];

            NoDesigned = 0;
            ChangedElementsNames = new string[1];
        }
        internal int noVerified;
        internal int noFailed;
        internal int noUnchecked;
        internal string[] failedElementsNames;

        internal int noDesigned;
        internal string[] changedElementsNames;

        public int NoVerified
        {
            get
            {
                return noVerified;
            }

            internal set
            {
                noVerified = value;
            }
        }

        public int NoFailed
        {
            get
            {
                return noFailed;
            }

            internal set
            {
                noFailed = value;
            }
        }

        public int NoUnchecked
        {
            get
            {
                return noUnchecked;
            }

            internal set
            {
                noUnchecked = value;
            }
        }

        public string[] FailedElementsNames
        {
            get
            {
                return failedElementsNames;
            }

            internal set
            {
                failedElementsNames = value;
            }
        }

        public int NoDesigned
        {
            get
            {
                return noDesigned;
            }

            internal set
            {
                noDesigned = value;
            }
        }

        public string[] ChangedElementsNames
        {
            get
            {
                return changedElementsNames;
            }

            internal set
            {
                changedElementsNames = value;
            }
        }
    }
}