using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.PEB.AnalysisTools.STAADPro;

namespace HANDAZ.PEB.BusinessComponents
{
    public class STAADAnalysisModel
    {
        public static void GenerateClearSpanFrame(HndzFrameSingleBay3D inputs, string modelName)
        {

            #region Extracted Inputs

            double width = inputs.Width/1000;
            double length = inputs.Length/1000;
            int noFrames = 1;//inputs.FramesCount; //TODO
            double BaySpacing = inputs.BaySpacing/1000;
            double eaveHeight = inputs.EaveHeight/1000;
            HndzRoofSlopeEnum slopeEnum = inputs.RoofSlope;
            double roofSlope;
            switch (slopeEnum)
            {
                case HndzRoofSlopeEnum.From1To5:
                    roofSlope = 0.2;
                    break;
                case HndzRoofSlopeEnum.From1To10:
                    roofSlope = 0.1;
                    break;
                case HndzRoofSlopeEnum.From1To20:
                    roofSlope = 0.05;
                    break;
                default:
                    roofSlope = 0.1;
                    break;
            }
            #endregion//Done

            #region Starting The program
            STAADProFileWriter std = new STAADProFileWriter(modelName);
            #endregion

            #region Defining I-Sections and Tapered Sections, assumed
            STAADProTaperedSection taperedColumn = new STAADProTaperedSection(0.3, 0.005, 0.8, 0.2, 0.01);
            STAADProTaperedSection taperedBeam = new STAADProTaperedSection(0.5, 0.005, 0.8, 0.2, 0.01);
            STAADProTaperedSection ISectionBeam = new STAADProTaperedSection(0.5, 0.005, 0.5, 0.2, 0.01);
            #endregion

            #region Drawing 3D Frame and pre-design assumptions
            STAADProMember[] leftCols, rightCols;
            STAADProMember[,] leftBeams, rightBeams;
            //Columns
            leftCols = new STAADProMember[noFrames];
            rightCols = new STAADProMember[noFrames];
            for (int i = 0; i < noFrames; i++)
            {
                leftCols[i] = new STAADProMember();
                leftCols[i].StartPoint = new STAADProPoint(0, 0, i * inputs.BaySpacing);
                leftCols[i].EndPoint = new STAADProPoint(0, eaveHeight, i * inputs.BaySpacing);
                leftCols[i].Section = taperedColumn;

                rightCols[i] = new STAADProMember();
                rightCols[i].StartPoint = new STAADProPoint(width,0, i * inputs.BaySpacing);
                rightCols[i].EndPoint = new STAADProPoint(width, eaveHeight, i * inputs.BaySpacing);
                rightCols[i].Section = taperedColumn;
            }

            //Beams
            int noSegments = (int)((width / 2) / 6);
            leftBeams = new STAADProMember[noFrames, noSegments];
            rightBeams = new STAADProMember[noFrames, noSegments];
            for (int i = 0; i < noFrames; i++)
            {
                for (int j = 0; j < noSegments; j++)
                {
                    leftBeams[i, j] = new STAADProMember();
                    try
                    {
                        leftBeams[i, j].StartPoint = leftBeams[i, j - 1].EndPoint;
                    }
                    catch 
                    {
                        leftBeams[i, j].StartPoint = leftCols[leftCols.Length-1].EndPoint;
                    }
                    //leftBeams[i, j].StartPoint = new STAADProPoint(j * 6, (double)j / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Y, i * inputs.BaySpacing);
                    leftBeams[i, j].EndPoint = new STAADProPoint((1 + j) * 6, (double)(1 + j) / noSegments * roofSlope * width / 2 + leftCols[i].EndPoint.Y, i * inputs.BaySpacing);
                    leftBeams[i, j].Section = ISectionBeam;

                    rightBeams[i, j] = new STAADProMember();
                    try
                    {
                        rightBeams[i, j].StartPoint = rightBeams[i, j - 1].EndPoint;
                    }
                    catch
                    {
                        rightBeams[i, j].StartPoint = rightCols[rightCols.Length-1].EndPoint;
                    }
                    rightBeams[i, j].EndPoint = new STAADProPoint(width - (1 + j) * 6, (double)(1 + j) / noSegments * roofSlope * width / 2 + rightCols[i].EndPoint.Y, i * inputs.BaySpacing);
                    rightBeams[i, j].Section = ISectionBeam;
                }
                //Modifications for exceptions
                leftBeams[i, 0].Section = taperedBeam; //Tapered edge section
                rightBeams[i, 0].Section = taperedBeam; //Tapered edge section

                try
                {
                    leftBeams[i, noSegments - 1].StartPoint = leftBeams[i, noSegments - 2].EndPoint;
                    leftBeams[i, noSegments - 1].EndPoint = new STAADProPoint(width / 2, width / 2 * roofSlope + leftCols[i].EndPoint.Y, i * inputs.BaySpacing);

                    rightBeams[i, noSegments - 1].EndPoint = rightBeams[i, noSegments - 2].EndPoint;
                    rightBeams[i, noSegments - 1].StartPoint = leftBeams[i, noSegments - 1].EndPoint;
                }
                catch (Exception)
                {
                }
            }
            List<STAADProMember> beams = new List<STAADProMember>();
            for (int i = 0; i < leftBeams.GetLength(0); i++)
            {
                for (int j = 0; j < leftBeams.GetLength(1); j++)
                {
                    beams.Add(leftBeams[i, j]);
                }
            }
            for (int i = 0; i < rightBeams.GetLength(0); i++)
            {
                for (int j = 0; j < rightBeams.GetLength(1); j++)
                {
                    beams.Add(rightBeams[i, j]);
                }
            }
            List<STAADProPoint> joints = new List<STAADProPoint>();
            for (int i = 0; i < beams.Count; i++)
            {
                joints.Add(beams[i].StartPoint);
                joints.Add(beams[i].EndPoint);
            }
            for (int i = 0; i < rightCols.Length; i++)
            {
                joints.Add(rightCols[i].StartPoint);
                joints.Add(rightCols[i].EndPoint);
            }
            for (int i = 0; i < leftCols.Length; i++)
            {
                joints.Add(leftCols[i].StartPoint);
                joints.Add(leftCols[i].EndPoint);
            }
            STAADProPoint[] supportedPoints = new STAADProPoint[2];
            STAADProSupportTypeEnum[] supportedTypes = new STAADProSupportTypeEnum[2];

            supportedPoints[0] = leftCols[0].StartPoint;
            supportedTypes[0] = STAADProSupportTypeEnum.PINNED;
            supportedPoints[1] = rightCols[0].StartPoint;
            supportedTypes[1] = STAADProSupportTypeEnum.PINNED;


            std.AddJoints(joints);
            std.AddMembers(leftCols, rightCols, beams);

            #region Supports

            std.DefineSuppots(supportedPoints, supportedTypes);
            #endregion

            #endregion

            #region Defining Materials
            std.DefineMaterial(STAADProMaterial.SteelMaterial());//TODO: Temp
            #endregion


            std.DefineConstants();

            #region Load Combinations and Loads

            STAADProLoadPattern cover= new STAADProLoadPattern(STAADProLoadTypeEnum.DEAD, "CL");
            STAADProLoadPattern live = new STAADProLoadPattern(STAADProLoadTypeEnum.LIVE, "LL");
            std.DefineLoadPattern(cover);
            std.AddMemberLoad(beams,new STAADProUniformLoad(STAADProDirectionEnum.GY,-0.01*inputs.BaySpacing));
            std.DefineLoadPattern(live);
            std.AddMemberLoad(beams, new STAADProUniformLoad(STAADProDirectionEnum.GY, -0.057 * inputs.BaySpacing));
            //TODO Wind Load
            //TODO Load Combinations
            //TODO remove hardcoded numbers
            #endregion
            std.PerformAnalysis();
            std.PerformDesign();

            std.CloseFile();
        }
        public static string GetPath(string modelName)
        {
            return STAADProFileWriter.GetPath(modelName);
        }
    }
}
