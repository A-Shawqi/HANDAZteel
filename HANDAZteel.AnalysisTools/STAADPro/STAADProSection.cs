namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    
    public abstract class STAADProSection
    {

        public STAADProSection(STAADProSectionTypeEnum sectionType)
        {
            SectionType = sectionType;
        }

        public STAADProSectionTypeEnum SectionType { get; set; }
    }
}