using System;
using HANDAZ.Entities;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public class SAPPoint : ISAPAPIComponent
    {
        /// <summary>
        /// The name should be passed as a reference, and thus it has to be visible in SAP2000API
        /// </summary>
        internal string name;

        public SAPPoint(string name)
        {
            Name = name;
        }

        public SAPPoint(string name, double x, double y, double z) : this(name)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// This constructor will copy the coordinates of another point but with a different name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="copiedPoint"></param>
        public SAPPoint(string name, SAPPoint copiedPoint) : this(name, copiedPoint.X, copiedPoint.Y, copiedPoint.Z)
        {

        }
        public SAPPoint()
        {
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool IsDefinedInSAP { get; set; } = false;

        public SAPRestraint Restraint { get; set; }
        public SAPJointElementResults AnalysisResults { get; internal set; }
        public SAPJointElementResults AnalysisResultsEnvelope { get; internal set; }

        public void ConvertFromHndzNode(HndzNode node)
        {
            Name = node.Name;
            X = node.Point.X;
            Y = node.Point.Y;
            Z = node.Point.Z;

            AnalysisResults.F1 = node.Reactions.F1;
            AnalysisResults.F2 = node.Reactions.F2;
            AnalysisResults.F3 = node.Reactions.F3;
            AnalysisResults.M1 = node.Reactions.M1;
            AnalysisResults.M2 = node.Reactions.M2;
            AnalysisResults.M3 = node.Reactions.M3;
            AnalysisResults.LoadCase = node.Reactions.LoadCase;
        }

        internal HndzNode ConvertToHndzNode()
        {
            HndzNode node = new HndzNode();
            node.Name = Name;
            node.Point = new Rhino.Geometry.Point3d(X, Y, Z);

            node.Reactions.F1 = AnalysisResults.F1;
            node.Reactions.F2 = AnalysisResults.F2;
            node.Reactions.F3 = AnalysisResults.F3;
            node.Reactions.M1 = AnalysisResults.M1;
            node.Reactions.M2 = AnalysisResults.M2;
            node.Reactions.M3 = AnalysisResults.M3;
            node.Reactions.LoadCase = AnalysisResults.LoadCase;

            return node;
        }

        public HndzSupport ConvertToHndzSupport()
        {
            HndzSupport sup = new HndzSupport(HndzSupportTypeEnum.Pinned, null);//TODO Change this
            sup.AnalysisResults = new HndzJointAnalysisResults[AnalysisResults.NumberResults];
            sup.AnalysisResultsEnvelope = new HndzJointAnalysisResults[AnalysisResultsEnvelope.NumberResults];

            for (int i = 0; i < AnalysisResults.NumberResults; i++)
            {
                sup.AnalysisResults[i] = new HndzJointAnalysisResults(AnalysisResults.Elment[i], AnalysisResults.loadCase[i], AnalysisResults.F1[i],
                    AnalysisResults.F2[i], AnalysisResults.F3[i], AnalysisResults.M1[i], AnalysisResults.M1[i], AnalysisResults.M1[i]);
            }
            for (int i = 0; i < AnalysisResultsEnvelope.NumberResults; i++)
            {
                sup.AnalysisResultsEnvelope[i] = new HndzJointAnalysisResults(AnalysisResultsEnvelope.Elment[i], AnalysisResultsEnvelope.loadCase[i], AnalysisResultsEnvelope.F1[i],
                    AnalysisResultsEnvelope.F2[i], AnalysisResultsEnvelope.F3[i], AnalysisResultsEnvelope.M1[i], AnalysisResultsEnvelope.M1[i], AnalysisResultsEnvelope.M1[i]);
            }
            return sup;
        }
    }
}