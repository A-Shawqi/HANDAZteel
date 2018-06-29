using HANDAZ.Entities;
using HANDAZ.PEB.Core.Designers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.BusinessComponents
{
    public static class HndzDesigner
    {
        public static HndzFrameSingleBay3D AssembleSections(HndzFrameSingleBay3D Frame3d)
        {
            double Fy = 2.4;
            double LUSupportedSap = 4.11;
            double Kfactor = 2;
            //==========================
            double KfactorBeamEnd = 0.7;
            double KfactorBeamStart = 0.65;
            //=======================

            double PurlinSpacing = Frame3d.PurlinSpacing/1000;
            //
            foreach (HndzFrameSingleBay2D Frame in Frame3d.Frames2D)
            {
                //===========================
                //=======================
                //Has to Get it form an algorithm in sap 
                //Kfactor Needs to Be calculated from InterActrion Diagrams - 2 For worst Case
                //What i want 
                // Analysis Result at start and and end 
                //Whats my Name ? Gecko

                double BeamLength = Frame.LeftBeam.ExtrusionLine.RhinoLine.Length/1000;
                double ColumnHeight = Frame.LeftColumn.ExtrusionLine.RhinoLine.Length/1000;
                //=================
                double Lbin = ColumnHeight;
                double Lbout = ColumnHeight;
                double LUnsupported = ColumnHeight *100/ 2;

                HndzAnalysisResults RColStartMax = Frame.LeftColumn.AnalysisResults[0];
                HndzAnalysisResults RColMidMax = Frame.LeftColumn.AnalysisResults[1];
                HndzAnalysisResults RColEndMax = Frame.LeftColumn.AnalysisResults[2];

                HndzAnalysisResults RColStartMin = Frame.LeftColumn.AnalysisResults[3];
                HndzAnalysisResults RColMidMin = Frame.LeftColumn.AnalysisResults[4];
                HndzAnalysisResults RColEndMin = Frame.LeftColumn.AnalysisResults[5];

                HndzAnalysisResults RBeamStartMax = Frame.LeftColumn.AnalysisResults[0];
                HndzAnalysisResults RBeamMidMax = Frame.LeftColumn.AnalysisResults[1];
                HndzAnalysisResults RBeamEndMax = Frame.LeftColumn.AnalysisResults[2];

                HndzAnalysisResults RBeamStartMin = Frame.LeftColumn.AnalysisResults[3];
                HndzAnalysisResults RBeamMidMin = Frame.LeftColumn.AnalysisResults[4];
                HndzAnalysisResults RBeamEndMin = Frame.LeftColumn.AnalysisResults[5];
                //=============================================
                //HndzAnalysisResults RBeamEnd = Frame.LeftBeam.AnalysisResults[1];
                //HndzAnalysisResults RBeamStart = Frame.LeftBeam.AnalysisResults[0];
                //==================================
                //station 0 = 0 ,3
                //station 1 = 2 , 5 
                //============================ 

                //foreach (HndzAnalysisResults RColEnd in Frame.LeftBeam.AnalysisResults)
                //{
                SectionI ColEstimated = CrossSectionCalulator.CalculateColumnSection(CrossSectionCalulator.SteelGrade.st37, ColumnHeight, RColStartMax.Axial, RColEndMax.Moment3);

                SectionI ColEndMax = BeamColumnDesign.DesignEnd(ColEstimated.d, ColEstimated.t_w, ColEstimated.b_fTop, ColEstimated.t_fTop,
                        ColEstimated.b_fBot, ColEstimated.t_fBot, RColEndMax.Axial, RColStartMax.Moment3, RColEndMax.Moment3, Fy,
                        LUnsupported, Kfactor, Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway,
                        BeamColumnDesign.SteelGrade.st37, BeamColumnDesign.LoadingType.Case2);
                SectionI ColStartMax = CrossSectionCalulator.SectionTaperedColumn(ColEndMax);


                ColStartMax = BeamColumnDesign.DesignStart(ColStartMax.d, ColStartMax.t_w, ColStartMax.b_fTop, ColStartMax.t_fTop, ColStartMax.b_fBot,
                        ColStartMax.t_fBot, RColStartMax.Axial, RColStartMax.Moment3, RColEndMax.Moment3, Fy, LUnsupported, Kfactor,
                        Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway, BeamColumnDesign.SteelGrade.st37,
                        BeamColumnDesign.LoadingType.Case2);

                ColStartMax = CrossSectionCalulator.PostProcessing(ColStartMax);
                ColEndMax = CrossSectionCalulator.PostProcessing(ColEndMax);
                //=======================================================

             

                //HndzISectionProfile Start = new HndzISectionProfile(ColStartMax);
                //HndzISectionProfile End = new HndzISectionProfile(ColEndMax);

                //HndzITaperedProfile ColumnTaperedSectionMax = new HndzITaperedProfile(Start, End);

                //======================================
                SectionI ColEstimatedMin = CrossSectionCalulator.CalculateColumnSection(CrossSectionCalulator.SteelGrade.st37, ColumnHeight, RColStartMax.Axial, RColEndMax.Moment3);

                SectionI ColEndMin = BeamColumnDesign.DesignEnd(ColEstimatedMin.d, ColEstimatedMin.t_w, ColEstimatedMin.b_fTop, ColEstimatedMin.t_fTop,
                        ColEstimatedMin.b_fBot, ColEstimatedMin.t_fBot, RColEndMin.Axial, RColStartMin.Moment3, RColEndMin.Moment3, Fy,
                        LUnsupported, Kfactor, Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway,
                        BeamColumnDesign.SteelGrade.st37, BeamColumnDesign.LoadingType.Case2);
                SectionI ColStartMin = CrossSectionCalulator.SectionTaperedColumn(ColEndMin);


                ColStartMin = BeamColumnDesign.DesignStart(ColStartMin.d, ColStartMin.t_w, ColStartMin.b_fTop, ColStartMin.t_fTop, ColStartMin.b_fBot,
                        ColStartMin.t_fBot, RColStartMin.Axial, RColStartMin.Moment3, RColEndMin.Moment3, Fy, LUnsupported, Kfactor,
                        Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway, BeamColumnDesign.SteelGrade.st37,
                        BeamColumnDesign.LoadingType.Case2);

                ColStartMin = CrossSectionCalulator.PostProcessing(ColStartMin);
                ColEndMin = CrossSectionCalulator.PostProcessing(ColEndMin);




                //HndzISectionProfile StartMin = new HndzISectionProfile(ColStartMin);
                //HndzISectionProfile EndMin = new HndzISectionProfile(ColEndMax);

                //HndzITaperedProfile ColumnTaperedSectionMin = new HndzITaperedProfile(Start, End);

                SectionI FinalSectionStartCol = CrossSectionCalulator.SectionCompare(ColStartMax, ColStartMin);
                SectionI FinalSectionEndCol = CrossSectionCalulator.SectionCompare(ColEndMax, ColEndMin);

                HndzISectionProfile ColStart = new HndzISectionProfile(FinalSectionStartCol);
                HndzISectionProfile ColEnd = new HndzISectionProfile(FinalSectionEndCol);

                HndzITaperedProfile ColumnTaperedSection = new HndzITaperedProfile(ColEnd, ColStart);
                //==========================================



                // }

                //foreach (HndzAnalysisResults RBeamStart in Frame.LeftBeam.AnalysisResults)
                //{
                SectionI BeamEstimatedMax = CrossSectionCalulator.CalculateBeamSection(CrossSectionCalulator.SteelGrade.st37, BeamLength, RBeamStartMax.Axial, RBeamStartMax.Moment3);

                SectionI BeamStartMax = BeamDesign.DesignStart(BeamEstimatedMax.d, BeamEstimatedMax.t_w, BeamEstimatedMax.b_fTop, BeamEstimatedMax.t_fTop,
                  BeamEstimatedMax.b_fBot, BeamEstimatedMax.t_fBot, RBeamStartMax.Axial, RBeamStartMax.Shear2, RBeamStartMax.Moment3, RBeamEndMax.Moment3, Fy, LUSupportedSap, KfactorBeamStart, BeamLength, PurlinSpacing, BeamDesign.SwayCondtion.PermitedToSway, BeamDesign.SteelGrade.st37, BeamDesign.LoadingType.Case2);

                SectionI BeamEndMAX = CrossSectionCalulator.SectionTaperedBeam(BeamStartMax);

                BeamEndMAX = BeamDesign.DesignEnd(BeamEndMAX.d, BeamEndMAX.t_w, BeamEndMAX.b_fTop, BeamEndMAX.t_fTop,
                        BeamEndMAX.b_fBot, BeamEndMAX.t_fBot,
                        RBeamEndMax.Axial, RBeamEndMax.Shear2, RBeamStartMax.Moment3, RBeamEndMax.Moment3, Fy, PurlinSpacing, KfactorBeamEnd, BeamLength, PurlinSpacing, BeamDesign.SwayCondtion.PermitedToSway, BeamDesign.SteelGrade.st37, BeamDesign.LoadingType.Case2);


                BeamStartMax = CrossSectionCalulator.PostProcessing(BeamStartMax);
                BeamEndMAX = CrossSectionCalulator.PostProcessing(BeamEndMAX);

                //HndzISectionProfile StartBeamMax = new HndzISectionProfile(BeamStartMax);
                //HndzISectionProfile EndBeamMax = new HndzISectionProfile(BeamEndMAX);
                //HndzITaperedProfile BeamTaperedSectionMax = new HndzITaperedProfile(StartBeamMax, EndBeamMax, new Rhino.Geometry.Vector2d(0, 1));

                SectionI BeamEstimatedMin = CrossSectionCalulator.CalculateColumnSection(CrossSectionCalulator.SteelGrade.st37, ColumnHeight, RColStartMax.Axial, RColEndMax.Moment3);

                SectionI BeamEndMin = BeamColumnDesign.DesignEnd(BeamEstimatedMin.d, BeamEstimatedMin.t_w, BeamEstimatedMin.b_fTop, BeamEstimatedMin.t_fTop,
                        BeamEstimatedMin.b_fBot, BeamEstimatedMin.t_fBot, RColEndMin.Axial, RColStartMin.Moment3, RColEndMin.Moment3, Fy,
                        LUnsupported, Kfactor, Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway,
                        BeamColumnDesign.SteelGrade.st37, BeamColumnDesign.LoadingType.Case2);
                SectionI BeamStartMin = CrossSectionCalulator.SectionTaperedColumn(BeamEndMin);


                BeamStartMin = BeamColumnDesign.DesignStart(BeamStartMin.d, BeamStartMin.t_w, BeamStartMin.b_fTop, BeamStartMin.t_fTop, BeamStartMin.b_fBot,
                        BeamStartMin.t_fBot, RColStartMin.Axial, RColStartMin.Moment3, RColEndMin.Moment3, Fy, LUnsupported, Kfactor,
                        Lbin, Lbout, BeamColumnDesign.SwayCondtion.PermitedToSway, BeamColumnDesign.SteelGrade.st37,
                        BeamColumnDesign.LoadingType.Case2);

                BeamStartMin = CrossSectionCalulator.PostProcessing(BeamStartMin);
                BeamEndMin = CrossSectionCalulator.PostProcessing(BeamEndMin);

              //  HndzISectionProfile StartMinBeam = new HndzISectionProfile(BeamStartMin);
               // HndzISectionProfile EndMinBeam = new HndzISectionProfile(BeamEndMin);



                //HndzISectionProfile StartMinBeam = new HndzISectionProfile(BeamEndMin);

                //HndzISectionProfile EndMinBeam = new HndzISectionProfile(BeamStartMin);

                //HndzITaperedProfile BeamTaperedSectionMin = new HndzITaperedProfile(EndMinBeam,StartMinBeam,new Rhino.Geometry.Vector2d(0, 1));


                SectionI FinalSectionStartBeam = CrossSectionCalulator.SectionCompare(BeamStartMax, BeamStartMin);
                SectionI FinalSectionEndBeam = CrossSectionCalulator.SectionCompare(BeamEndMAX, BeamEndMin);

                HndzISectionProfile BeamStart = new HndzISectionProfile(FinalSectionStartBeam);
                HndzISectionProfile BeamEnd = new HndzISectionProfile(FinalSectionEndBeam);

                HndzITaperedProfile BeamTaperedSection = new HndzITaperedProfile(BeamStart, BeamEnd, new Rhino.Geometry.Vector2d(0, 1));


                //==========================================

                //}
                // Sections Assumbtions 

                //========================================
                Frame.LeftColumn.Profile = ColumnTaperedSection;
                Frame.RightColumn.Profile = ColumnTaperedSection;
                Frame.LeftBeam.Profile = BeamTaperedSection;
                Frame.RightBeam.Profile = BeamTaperedSection;

                //SectionChannel AssumedCSection = new SectionChannel("Assumed hotRolled C Section", 180, 80, 10, 4);
                //HndzISectionProfile bigProfile = new HndzISectionProfile(new SectionI("big", 1000 * 2, 500 * 2, 100 * 2, 50 * 2));
                //HndzISectionProfile smallProfile = new HndzISectionProfile(new SectionI("small", 1000, 500, 100, 50));

            }
            return Frame3d;
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
