using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using Wosad.Common.Section.SectionTypes;

namespace HANDAZ.Entities
{
    [DataContract]
    [Serializable]
    [XmlSerializerFormat]
    public class HndzFrameMonoSlope3D : HndzFrame3D
    {
        #region Properties
        [DataMember, XmlAttribute]
        [XmlArray("Frames2D")]
        public  ICollection<HndzFrameMonoSlope2D> Frames2D { get; set; }

        #endregion

        #region Constructors
        public HndzFrameMonoSlope3D(string name, string description,  /*float ridgeHeight,*/ double length, double baySpacing, double purlinSpacing,
                    HndzLocationEnum location, HndzRoofSlopeEnum roofSlope, HndzRoofAccessibilityEnum roofAccessibiity,
                    HndzBuildingEnclosingEnum buildingEnclosing, HndzImportanceFactorEnum importanceFactor,
                    ICollection<HndzFrameMonoSlope2D> frames2D, ICollection<HndzPurlinStandrdCase> purlins,
                    ICollection<HndzGuirt> girts, HndzStorey storey = null) : base(name, description, location, roofSlope, roofAccessibiity,
                    buildingEnclosing, importanceFactor, length, baySpacing, purlinSpacing, 0, 0, storey)
        {
            Type = HndzFrameTypeEnum.SingleSlope;
            Frames2D = frames2D;
            Purlins = purlins;
            Girts = girts;
            if (Frames2D != null)
            {
                HndzFrameMonoSlope2D anyFrame = Frames2D.ToArray()[0];

                Point3d lowerLeft = anyFrame.LeftColumn.ExtrusionLine.baseNode.Point;
                //Point3d upperLeft = anyFrame.LeftColumn.ExtrusionLine.EndNode.Point;

                Point3d lowerRight = anyFrame.RightColumn.ExtrusionLine.baseNode.Point;
                //Point3d upperRight = anyFrame.RightColumn.ExtrusionLine.EndNode.Point;

                EaveHeight = anyFrame.RightColumn.ExtrusionLine.RhinoLine.Length;
                Width = new Line(lowerLeft, lowerRight).Length;
            }
            switch (RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    RidgeHeight = EaveHeight + 0.2 * Width ;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    RidgeHeight = EaveHeight + 0.1 * Width ;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    RidgeHeight = EaveHeight + 0.05 * Width ;
                    break;
            }

        }

        public HndzFrameMonoSlope3D( /*float ridgeHeight,*/ double length, double baySpacing, double purlinSpacing,
                    HndzLocationEnum location, HndzRoofSlopeEnum roofSlope, HndzRoofAccessibilityEnum roofAccessibiity,
                    HndzBuildingEnclosingEnum buildingEnclosing, HndzImportanceFactorEnum importanceFactor,
                    ICollection<HndzFrameMonoSlope2D> frames2D, ICollection<HndzPurlinStandrdCase> purlins,
                    ICollection<HndzGuirt> girts, HndzStorey storey = null) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, length,
                    baySpacing, purlinSpacing, location, roofSlope, roofAccessibiity,
                    buildingEnclosing, importanceFactor, frames2D, purlins, girts, storey)
        {
        }

        private HndzFrameMonoSlope3D(string name, string description, double length,
                    double baySpacing, double width, double eaveHeight, double purlinSpacing,
                  HndzLocationEnum location, HndzRoofSlopeEnum roofSlope, HndzRoofAccessibilityEnum roofAccessibiity,
                  HndzBuildingEnclosingEnum buildingEnclosing, HndzImportanceFactorEnum importanceFactor,
                  SectionI columnsStartSection = null, SectionI columnsEndSection = null, SectionI beamsStartSection = null,
                  SectionI beamsEndSection = null, SectionChannel purlinsSection = null, HndzStorey storey = null) :
                  base(name, description, location, roofSlope, roofAccessibiity, buildingEnclosing, importanceFactor, length,
                        baySpacing, purlinSpacing, width, eaveHeight, storey)
        {
            switch (RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    RidgeHeight = EaveHeight + 0.2 * Width;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    RidgeHeight = EaveHeight + 0.1 * Width;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    RidgeHeight = EaveHeight + 0.05 * Width;
                    break;
            }
            AssemblePEB(columnsStartSection, columnsEndSection, beamsStartSection, beamsEndSection, purlinsSection);
            //throw new NotImplementedException("sorry,this will be fixed in the next version");
        }



        public HndzFrameMonoSlope3D(double length, double baySpacing, double width, double eaveHeight, double purlinSpacing,
                HndzLocationEnum location, HndzRoofSlopeEnum roofSlope, HndzRoofAccessibilityEnum roofAccessibiity,
                HndzBuildingEnclosingEnum buildingEnclosing, HndzImportanceFactorEnum importanceFactor,
               SectionI columnsStartSection = null, SectionI columnsEndSection = null, SectionI beamsStartSection = null,
                  SectionI beamsEndSection = null, SectionChannel purlinsSection = null, HndzStorey storey = null) :
                this(HndzResources.DefaultName, HndzResources.DefaultDescription, length, baySpacing, width, eaveHeight,
                    purlinSpacing, location, roofSlope, roofAccessibiity, buildingEnclosing, importanceFactor,
                    columnsStartSection, columnsEndSection, beamsStartSection, beamsEndSection, purlinsSection, storey)
        {
        }

        public HndzFrameMonoSlope3D() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, 60, 6, 2,
            HndzLocationEnum.Cairo, HndzRoofSlopeEnum.From1To10, HndzRoofAccessibilityEnum.Inaccessible, HndzBuildingEnclosingEnum.Enclosed,
            HndzImportanceFactorEnum.II, null, null, null)
        {
        }

        public override void AssemblePEB(SectionI columnsStartSection = null, SectionI columnsEndSection = null, SectionI beamsStartSection = null, SectionI beamsEndSection = null, SectionChannel purlinsSection = null)
        {
            const double FirstPurlinSpacingCL = 100;
            //const double FirstPurlinSpacingEdge = 0;

            //SectionI AssumedIBuiltUpSection = new SectionI("Assumed Built up I Section", 600, 250, 20, 10);
            SectionChannel AssumedCSection = new SectionChannel("Assumed hotRolled C Section", 180, 80, 10, 4);
            HndzISectionProfile bigProfile = new HndzISectionProfile(new SectionI("big", 1000 * 2, 500 * 2, 100 * 2, 50 * 2));
            HndzISectionProfile smallProfile = new HndzISectionProfile(new SectionI("small", 1000, 500, 100, 50));


            HndzITaperedProfile assumedProfileColumn = new HndzITaperedProfile(smallProfile, bigProfile, new Vector2d(1, 0));
            HndzITaperedProfile assumedProfileBeam = new HndzITaperedProfile(bigProfile, smallProfile, new Vector2d(0, 1));

            HndzITaperedProfile columnsTaperedProfile = null;
            HndzITaperedProfile beamsTaperedProfile = null;

            #region Check function parameters

            if (columnsStartSection == null && columnsEndSection != null)
            {
                columnsTaperedProfile = new HndzITaperedProfile(new HndzISectionProfile(columnsEndSection),
                                                    new HndzISectionProfile(columnsEndSection), new Vector2d(0, 1));
            }
            else if (columnsEndSection == null && columnsStartSection != null)
            {
                columnsTaperedProfile = new HndzITaperedProfile(new HndzISectionProfile(columnsStartSection),
                                                    new HndzISectionProfile(columnsStartSection), new Vector2d(0, 1));
            }
            else if (columnsStartSection != null && columnsEndSection != null)
            {
                columnsTaperedProfile = new HndzITaperedProfile(new HndzISectionProfile(columnsStartSection),
                                                    new HndzISectionProfile(columnsEndSection), new Vector2d(0, 1));
            }
            else // dah law el el 2 sections b null
            {
                columnsTaperedProfile = assumedProfileColumn;
            }

            if (beamsStartSection == null && beamsEndSection != null)
            {
                beamsTaperedProfile = new HndzITaperedProfile(new HndzISectionProfile(beamsEndSection),
                                                    new HndzISectionProfile(beamsEndSection), new Vector2d(1, 0));
            }
            else if (beamsEndSection == null && beamsStartSection != null)
            {
                beamsTaperedProfile = new HndzITaperedProfile(new HndzISectionProfile(beamsStartSection),
                                                    new HndzISectionProfile(beamsStartSection), new Vector2d(1, 0));
            }
            else if (beamsStartSection != null && beamsEndSection != null)
            {
                beamsTaperedProfile = new HndzITaperedProfile(new HndzISectionProfile(beamsStartSection),
                                                    new HndzISectionProfile(beamsEndSection), new Vector2d(0, 1));
            }
            else // dah law el el 2 sections b null
            {
                beamsTaperedProfile = assumedProfileBeam;
            }

            if (purlinsSection == null) purlinsSection = AssumedCSection;
            #endregion

            columnsStartSection = columnsTaperedProfile.StartProfile.I_Section;
            columnsEndSection = columnsTaperedProfile.EndProfile.I_Section;

            beamsStartSection = beamsTaperedProfile.StartProfile.I_Section;
            beamsEndSection = beamsTaperedProfile.EndProfile.I_Section;
            double beamZoffset = beamsStartSection.d / 2 + beamsStartSection.tf;//ToDo: add to beam and purlin
            Frames2D = new List<HndzFrameMonoSlope2D>(FramesCount);
            Purlins = new List<HndzPurlinStandrdCase>();
            //Girts = new List<HndzGuirt>(2);

            for (int framesCounter = 0; framesCounter < FramesCount; framesCounter++)
            {
                HndzFrameMonoSlope2D Frame = new HndzFrameMonoSlope2D();

                Point3d lowerLeft = new Point3d(-Width / 2, BaySpacing * framesCounter, 0);
                Point3d lowerRight = new Point3d(Width / 2, BaySpacing * framesCounter, 0);

                Point3d upperLeft = new Point3d(-Width / 2, BaySpacing * framesCounter, EaveHeight);
                Point3d upperRight = new Point3d(Width / 2, BaySpacing * framesCounter, RidgeHeight);

                //Point3d ridgeMid = new Point3d(0, BaySpacing * framesCounter, RidgeHeight);

                ////
                HndzLine rColLine = new HndzLine(lowerRight, upperRight);
                Frame.RightColumn = new HndzColumnStandardCase(rColLine, columnsTaperedProfile);

                HndzLine lColLine = new HndzLine(lowerLeft, upperLeft);
                Frame.LeftColumn = new HndzColumnStandardCase(lColLine, columnsTaperedProfile);

                /////
                HndzLine beamine = new HndzLine(upperRight, upperLeft);
                Frame.Beam = new HndzBeamStandrdCase(beamine, beamsTaperedProfile);

                //HndzLine lBeamLine = new HndzLine(upperLeft, ridgeMid);
                //Frame.LeftBeam = new HndzBeamStandrdCase(lBeamLine, beamsTaperedProfile);

                Frame.RightSupport = new HndzSupport(HndzSupportTypeEnum.Pinned, new HndzNode(lowerRight));
                Frame.LeftSupport = new HndzSupport(HndzSupportTypeEnum.Pinned, new HndzNode(lowerLeft));

                Frames2D.Add(Frame);
            }
            double roofSlopeAngle = 1;
            switch (RoofSlope)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlopeAngle = Math.Atan(0.2);
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlopeAngle = Math.Atan(0.1);
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlopeAngle = Math.Atan(0.05);
                    break;
            }

            //double pulinZoffset = beamsStartSection.d / 2 - beamsStartSection.tf / 2; ///////Need Revision "msh 3arf ezay bs hya kda sha8ala 7lw :D"
            double pulinZoffset = beamsEndSection.d / 2; ///////Need Revision "msh 3arf ezay bs hya kda sha8ala 7lw :D"
            //double taperingDiffrence = 0.5*(beamsStartSection.d - beamsEndSection.d);
            double xLeft = -Width / 2 + FirstPurlinSpacingCL;
            double zLeft = EaveHeight + pulinZoffset;
            
            double taperingZoffsetLeft = PurlinSpacing * (beamsStartSection.d - beamsEndSection.d) / (2 * Width);
            do
            {
                Point3d startL = new Point3d(xLeft, 0, zLeft);
                Point3d endL = new Point3d(xLeft, Length, zLeft);

                HndzCSectionProfile assumedProfilePurlin = new HndzCSectionProfile(purlinsSection, new Vector2d(0, -1));

                HndzLine purlinLine = new HndzLine(new Line(startL, endL));
                Purlins.Add(new HndzPurlinStandrdCase(assumedProfilePurlin, purlinLine));

                xLeft += PurlinSpacing;
                zLeft += PurlinSpacing * Math.Tan(roofSlopeAngle) + taperingZoffsetLeft;

            } while (xLeft < Width / 2 + FirstPurlinSpacingCL && zLeft < RidgeHeight + pulinZoffset);


        }
        #endregion
    }
}