using HANDAZ.Entities;
using HANDAZ.PEB.Core.Designers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.PEB.Core.Designers
{
   
    public static class HndzDesigner
    {
        public static HndzFrameSingleBay3D AssembleSections(HndzFrameSingleBay3D Frame3d)
        {

            foreach (HndzFrameSingleBay2D Frame in Frame3d.Frames2D)
            {
                double Fy = 2.4;
                double LUSupportedSap = 4.11;
                //Kfactor Needs to Be calculated from InterActrion Diagrams - 2 For worst Case
                double Kfactor = 2;
                double KfactorBeamEnd = 0.7;
                double KfactorBeamStart = 0.65;
                double Lbin = 8.5;
                double Lbout = 5;
                double BeamLength = 12;
                double PurlinSpacing = 2;
                double ColumnHeight = Frame.LeftBeam.ExtrusionLine.RhinoLine.Length;
                double LUnsupported = ColumnHeight / 2;
                //What i want 
                // Analysis Result at start and and end 

                //Whats my Name ? Gecko
                HndzAnalysisResults RColStartMax = Frame.LeftBeam.AnalysisResults[0];
                HndzAnalysisResults RColMidMax = Frame.LeftBeam.AnalysisResults[1];
                HndzAnalysisResults RColEndMax = Frame.LeftBeam.AnalysisResults[2];

                HndzAnalysisResults RColStartMin = Frame.LeftBeam.AnalysisResults[3];
                HndzAnalysisResults RColMidMin = Frame.LeftBeam.AnalysisResults[4];
                HndzAnalysisResults RColEndMin = Frame.LeftBeam.AnalysisResults[5];
                //=============================================
                //HndzAnalysisResults RBeamEnd = Frame.LeftBeam.AnalysisResults[1];
                //HndzAnalysisResults RBeamStart = Frame.LeftBeam.AnalysisResults[0];
                //==================================
                //station 0 = 0 ,3
                //station 1 = 2 , 5 
                //============================ 

                //foreach (HndzAnalysisResults RColEnd in Frame.LeftBeam.AnalysisResults)
                //{
                SectionI ColEstimated = CrossSectionCalulator.CalculateColumnSection(CrossSectionCalulator.SteelGrade.st37, ColumnHeight, RColStartMax.Axial, RColStartMax.Moment3);

                SectionI ColEnd = BeamColumnDesign.DesignEnd(ColEstimated.d, ColEstimated.t_w, ColEstimated.b_fTop, ColEstimated.t_fTop,
                        ColEstimated.b_fBot, ColEstimated.t_fBot, RColEndMax.Axial, RColStartMax.Moment3, RColEndMax.Moment3, Fy,
                        LUnsupported, Kfactor, Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway,
                        BeamColumnDesign.SteelGrade.st37, BeamColumnDesign.LoadingType.Case2);
                    SectionI ColStart = CrossSectionCalulator.SectionTaperedColumn(ColEnd);


                    ColStart = BeamColumnDesign.DesignStart(ColStart.d, ColStart.t_w, ColStart.b_fTop, ColStart.t_fTop, ColStart.b_fBot,
                            ColStart.t_fBot, RColStartMax.Axial, RColEndMax.Moment3, RColEndMax.Moment3, Fy, LUnsupported, Kfactor,
                            Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway, BeamColumnDesign.SteelGrade.st37,
                            BeamColumnDesign.LoadingType.Case2);

                    HndzISectionProfile Start = new HndzISectionProfile(ColStart);
                    HndzISectionProfile End = new HndzISectionProfile(ColEnd);
                    HndzITaperedProfile Section = new HndzITaperedProfile(Start, End);

                Frame.LeftBeam.Profile = Section;
                Frame.RightBeam.Profile = Section;
                // }

                //foreach (HndzAnalysisResults RBeamStart in Frame.LeftBeam.AnalysisResults)
                //{
                //    SectionI BeamEstimated = CrossSectionCalulator.CalculateBeamSection(CrossSectionCalulator.SteelGrade.st37, BeamLength, RBeamStart.Axial, RBeamStart.Moment3);

                //    SectionI BeamStart = BeamDesign.DesignStart(BeamEstimated.d, BeamEstimated.t_w, BeamEstimated.b_fTop, BeamEstimated.t_fTop,
                //      BeamEstimated.b_fBot, BeamEstimated.t_fBot, RBeamStart.Axial, RBeamStart.Shear2, RBeamStart.Moment3, RBeamEnd.Moment3, Fy, LUSupportedSap, KfactorBeamStart, BeamLength, PurlinSpacing, BeamDesign.SwayCondtion.PermitedToSway, BeamDesign.SteelGrade.st37, BeamDesign.LoadingType.Case2);

                //    SectionI BeamEnd = CrossSectionCalulator.SectionTaperedBeam(BeamStart);

                //    BeamEnd = BeamDesign.DesignEnd(BeamEnd.d, BeamEnd.t_w, BeamEnd.b_fTop, BeamEnd.t_fTop,
                //            BeamEnd.b_fBot, BeamEnd.t_fBot,
                //            RBeamEnd.Axial, RBeamEnd.Shear2, RBeamStart.Moment3, RBeamEnd.Moment3, Fy, PurlinSpacing, KfactorBeamEnd, BeamLength, PurlinSpacing, BeamDesign.SwayCondtion.PermitedToSway, BeamDesign.SteelGrade.st37, BeamDesign.LoadingType.Case2);


                //    HndzISectionProfile Start = new HndzISectionProfile(BeamStart);
                //    HndzISectionProfile End = new HndzISectionProfile(BeamEnd);

                //    HndzITaperedProfile Section = new HndzITaperedProfile(Start, End);



                //}
                // double AppliedLoads = 
            }
            return null;
        }

        //public static SectionI CompareProfiles(List<SectionI> CreatedProfiles)
        //{
        //    foreach (SectionI Profile in CreatedProfiles)
        //    {

        //        sort using the icomparer
        //        CreatedProfiles.Sort()
        //       return CreatedProfiles.Last<SectionI>();
        //    }
        //}

        //public static 
        //double Fy = 2.4;
        //double LUnsupported = 500;
        //double LUSupportedSap = 4.11;
        //double Kfactor = 2;
        //double KfactorBeamEnd = 0.7;
        //double KfactorBeamStart = 0.65;
        //double Lbin = 8.5;
        //double Lbout = 5;
        //double BeamLength = 12;
        //double ColumnHeight = 9.5;
        //double PurlinSpacing = 2;
        ////================================

        //HndzAnalysisResults RColStart = new HndzAnalysisResults(0, "EnvCol", 20, 0, 0, 0, 0, 0);
        //HndzAnalysisResults RColEnd = new HndzAnalysisResults(1, "EnvCol", 20, 0, 0, 0, 0, 18);

        //SectionI ColEstimated = CrossSectionCalulator.CalculateColumnSection(CrossSectionCalulator.SteelGrade.st37, ColumnHeight, RColEnd.Axial, RColEnd.Moment3);


        //SectionI ColEnd = BeamColumnDesign.DesignEnd(ColEstimated.d, ColEstimated.t_w, ColEstimated.b_fTop, ColEstimated.t_fTop,
        //    ColEstimated.b_fBot, ColEstimated.t_fBot, RColEnd.Axial, RColStart.Moment3, RColEnd.Moment3, Fy,
        //    LUnsupported, Kfactor, Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway,
        //    BeamColumnDesign.SteelGrade.st37, BeamColumnDesign.LoadingType.Case2);


        //SectionI ColStart = CrossSectionCalulator.SectionTaperedColumn(ColEnd);


        //ColStart = BeamColumnDesign.DesignStart(ColStart.d, ColStart.t_w, ColStart.b_fTop, ColStart.t_fTop, ColStart.b_fBot,
        //        ColStart.t_fBot, RColStart.Axial,RColStart.Moment3, RColEnd.Moment3, Fy,LUnsupported, Kfactor,
        //        Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway, BeamColumnDesign.SteelGrade.st37,
        //        BeamColumnDesign.LoadingType.Case2);
        //    //======================
        //    //Beam Example 
        //    HndzAnalysisResults RBeamStart = new HndzAnalysisResults(0, "EnvBeam", 0.45, 3.15, 0, 0, 0, 8.20);
        //HndzAnalysisResults RBeamEnd = new HndzAnalysisResults(1, "EnvBeam", 5.24, 3.15, 0, 0, 0, 9.74);

        //SectionI BeamEstimated = CrossSectionCalulator.CalculateBeamSection(CrossSectionCalulator.SteelGrade.st37, BeamLength, RBeamStart.Axial, RBeamStart.Moment3);

        //SectionI BeamStart = BeamDesign.DesignStart(BeamEstimated.d, BeamEstimated.t_w, BeamEstimated.b_fTop, BeamEstimated.t_fTop,
        //  BeamEstimated.b_fBot, BeamEstimated.t_fBot, RBeamStart.Axial, RBeamStart.Shear2, RBeamStart.Moment3, RBeamEnd.Moment3, Fy, LUSupportedSap, KfactorBeamStart, BeamLength, PurlinSpacing, BeamDesign.SwayCondtion.PermitedToSway, BeamDesign.SteelGrade.st37, BeamDesign.LoadingType.Case2);

        //SectionI BeamEnd = CrossSectionCalulator.SectionTaperedBeam(BeamStart);

        //BeamEnd = BeamDesign.DesignEnd(BeamEnd.d, BeamEnd.t_w, BeamEnd.b_fTop, BeamEnd.t_fTop,
        //        BeamEnd.b_fBot, BeamEnd.t_fBot,
        //        RBeamEnd.Axial, RBeamEnd.Shear2, RBeamStart.Moment3, RBeamEnd.Moment3, Fy, PurlinSpacing, KfactorBeamEnd, BeamLength, PurlinSpacing, BeamDesign.SwayCondtion.PermitedToSway, BeamDesign.SteelGrade.st37, BeamDesign.LoadingType.Case2);

    }
}
