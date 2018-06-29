using System;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPLoadCase:ISAPAPIComponent
    {
        public bool IsDefinedInSAP { get; set; } = false;
        public string Name { get; set; }

        public static explicit operator SAPLoadCase(SAPLoadPattern pattern)
        {
            SAPLoadCase convertedCase = new SAPLoadCase();
            if (pattern.IsAnalysisCase == false)
            {
                //SAP2000API.AddLoadCase(pattern);
                throw new Exception("Can not cast this pattern to a case, because the function is not implemented yet");
            }
            convertedCase.IsDefinedInSAP = pattern.IsDefinedInSAP;
            convertedCase.Name = pattern.Name;
            return convertedCase;
        }
    }
}