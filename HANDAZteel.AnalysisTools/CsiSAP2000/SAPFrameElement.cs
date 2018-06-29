using System;
using HANDAZ.Entities;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPFrameElement : ISAPAPIComponent
    {
        internal string name;
        public bool IsDefinedInSAP { get; set; } = false;

        public SAPFrameElement(string name, SAPPoint startPoint, SAPPoint endPoint, SAPSection section) : this(name)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Section = section;
        }

        public SAPFrameElement(string name)
        {
            Name = name;
            Label = Name;
            StartPoint = new SAPPoint(name + "p1");
            EndPoint = new SAPPoint(name + "p2");
            Section = null;
            AnalysisResults = null;
        }

        public SAPFrameElement()
        {
            StartPoint = new SAPPoint();
            EndPoint = new SAPPoint();
            Section = new SAPISection(); //TODO : This is temporary to prevent null exceptions
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public SAPPoint StartPoint { get; set; }
        public SAPPoint EndPoint { get; set; }
        public SAPSection Section { get; set; }
        public SAPAnalysisResults AnalysisResults { get; internal set; }
        public SAPAnalysisResults AnalysisResultsEnvelope { get; internal set; }
        public SAPDesignResults DesignResults { get; set; }
        public string Label { get; internal set; }
        public bool? IsDesignPassed { get; internal set; } = null;


        public void ConvertToHndzElement(HndzStructuralElement hndzElement)
        {
            SAPITaperedSection sapSec = (SAPITaperedSection)Section;

            //HndzStructuralElement hndzElement;
            SectionI startSec = new SectionI(sapSec.StartSection.Name, sapSec.StartSection.Height * 1000, sapSec.StartSection.BotFlangeWidth * 1000, sapSec.StartSection.BotFlangeThickness * 1000, sapSec.StartSection.WebThickness * 1000);
            SectionI endSec = new SectionI(sapSec.EndSection.Name, sapSec.EndSection.Height * 1000, sapSec.EndSection.BotFlangeWidth * 1000, sapSec.EndSection.BotFlangeThickness * 1000, sapSec.EndSection.WebThickness * 1000);
            if (hndzElement.Profile is HndzITaperedProfile)
            {
                HndzITaperedProfile profile = hndzElement.Profile as HndzITaperedProfile;
                hndzElement.Name = Name;
                hndzElement.Profile.Name = Name;
                profile.StartProfile.I_Section = startSec;
                profile.StartProfile.Name = sapSec.StartSection.Name;
                profile.EndProfile.I_Section = endSec;
                profile.EndProfile.Name = sapSec.EndSection.Name;

            }

            hndzElement.AnalysisResults = new HndzAnalysisResults[AnalysisResults.NumberResults];
            hndzElement.AnalysisResultsEnvelope = new HndzAnalysisResults[AnalysisResultsEnvelope.NumberResults];

            for (int i = 0; i < AnalysisResults.NumberResults; i++)
            {
                hndzElement.AnalysisResults[i] = new HndzAnalysisResults(AnalysisResults.Station[i], 
                    AnalysisResults.LoadCase[i], AnalysisResults.Axial[i], AnalysisResults.Shear2[i], AnalysisResults.Shear3[i], AnalysisResults.TortionalMoment[i]
                    , AnalysisResults.Moment2[i], AnalysisResults.Moment3[i]);
                
            }
            for (int i = 0; i < AnalysisResultsEnvelope.NumberResults; i++)
            {
                hndzElement.AnalysisResultsEnvelope[i] = new HndzAnalysisResults(AnalysisResultsEnvelope.Station[i],
                    AnalysisResultsEnvelope.LoadCase[i], AnalysisResultsEnvelope.Axial[i], AnalysisResultsEnvelope.Shear2[i], AnalysisResultsEnvelope.Shear3[i], AnalysisResultsEnvelope.TortionalMoment[i]
                    , AnalysisResultsEnvelope.Moment2[i], AnalysisResultsEnvelope.Moment3[i]);
            }
            hndzElement.IsDesignPassed = IsDesignPassed;
            //TODO: Section and material
            //return hndzElement;
        }
        public void ConvertFromHndzElement(HndzExtrudedElement element, HndzSectionTypeEnum type)
        {
            switch (type)
            {
                case HndzSectionTypeEnum.HotRolledC:
                    throw new NotImplementedException("HotRolled is not ready");
                    break;
                case HndzSectionTypeEnum.HotRolledI:
                    throw new NotImplementedException("HotRolled is not ready");
                    break;
                case HndzSectionTypeEnum.BuiltUpI:
                    HndzBeamStandrdCase beam = null;
                    HndzColumnStandardCase column = null;
                    if (element is HndzBeamStandrdCase)
                    {
                        beam = (HndzBeamStandrdCase)element;
                        Name = Label = beam.Name;

                        SAPISection iSection = new SAPISection();
                        Section = iSection.ConvertFromHndzIProfile((HndzISectionProfile)beam.Profile, (HndzStructuralMaterial)beam.Material);
                        Section = iSection;

                        StartPoint.ConvertFromHndzNode(beam.ExtrusionLine.baseNode);
                        EndPoint.ConvertFromHndzNode(beam.ExtrusionLine.EndNode);
                    }
                    else if (element is HndzColumnStandardCase)
                    {
                        column = (HndzColumnStandardCase)element;

                        Name = Label = column.Name;

                        SAPISection iSection = new SAPISection();
                        Section = iSection.ConvertFromHndzIProfile((HndzISectionProfile)column.Profile, (HndzStructuralMaterial)column.Material);
                        Section = iSection;

                        StartPoint.ConvertFromHndzNode(column.ExtrusionLine.baseNode);
                        EndPoint.ConvertFromHndzNode(column.ExtrusionLine.EndNode);
                    }

                    break;
                case HndzSectionTypeEnum.TaperedI:
                    HndzBeamStandrdCase beamTapered = null;
                    HndzBeamStandrdCase columnTapered = null;
                    if (element is HndzBeamStandrdCase)
                    {
                        //beamTapered = (HndzBeamTapered)element;
                        //Name = Label = beamTapered.Name;

                        //SAPITaperedSection iSection = new SAPITaperedSection();
                        //iSection.ConvertFromHndzTaperedI(beamTapered.Profile, (HndzStructuralMaterial)beamTapered.Material);
                        //Section = iSection;

                        //StartPoint.ConvertFromHndzNode(beamTapered.ExtrusionLine.baseNode);
                        //EndPoint.ConvertFromHndzNode(beamTapered.ExtrusionLine.EndNode);
                        throw new NotImplementedException();
                    }
                    else if (element is HndzBeamStandrdCase)
                    {
                        columnTapered = (HndzBeamStandrdCase)element;
                        Name = Label = columnTapered.Name;

                        if (columnTapered.Profile is HndzITaperedProfile)
                        {
                            HndzITaperedProfile columnTaperedProfile = columnTapered.Profile as HndzITaperedProfile;
                            SAPITaperedSection iSection = new SAPITaperedSection();
                            iSection.ConvertFromHndzTaperedI(columnTaperedProfile, (HndzStructuralMaterial)columnTapered.Material);
                            Section = iSection;

                            StartPoint.ConvertFromHndzNode(columnTapered.ExtrusionLine.baseNode);
                            EndPoint.ConvertFromHndzNode(columnTapered.ExtrusionLine.EndNode);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}