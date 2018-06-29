using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;
using HANDAZ.PEB.Core;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    /// <summary>
    /// General I-Section Built Up Section
    /// </summary>
    public class SAPISection : SAPSection
    {
        public SAPISection()
        {

        }
        public SAPISection(string name, SAPMaterial webMaterial, SAPMaterial topFlangeMaterial, SAPMaterial botFlangeMaterial, double height, double topFlangeWidth, double topFlangeThickness, double botFlangeWidth, double botFlangeThickness, double webThickness)
        {
            Name = name;
            WebMaterial = webMaterial;
            TopFlangeMaterial = topFlangeMaterial;
            BotFlangeMaterial = botFlangeMaterial;
            Height = height;
            TopFlangeWidth = topFlangeWidth;
            TopFlangeThickness = topFlangeThickness;
            BotFlangeWidth = botFlangeWidth;
            BotFlangeThickness = botFlangeThickness;
            WebThickness = webThickness;
        }
        public SAPMaterial WebMaterial { get; set; }
        public SAPMaterial TopFlangeMaterial { get; set; }
        public SAPMaterial BotFlangeMaterial { get; set; }
        /// <summary>
        /// t3
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// t2
        /// </summary>
        public double TopFlangeWidth { get; set; }
        /// <summary>
        /// TF
        /// </summary>
        public double TopFlangeThickness { get; set; }
        /// <summary>
        /// t2b
        /// </summary>
        public double BotFlangeWidth { get; set; }
        /// <summary>
        /// TFb
        /// </summary>
        public double BotFlangeThickness { get; set; }
        /// <summary>
        /// tw
        /// </summary>
        public double WebThickness { get; set; }

        public SAPISection ConvertFromHndzIProfile(HndzISectionProfile Isection, HndzStructuralMaterial material)
        {
            SAPISection sapISection = new SAPISection();
            sapISection.Name = Isection.Name;
            sapISection.BotFlangeWidth = Isection.I_Section.b_fBot;
            sapISection.BotFlangeThickness = Isection.I_Section.t_fBot;
            sapISection.TopFlangeWidth = Isection.I_Section.b_fTop;
            sapISection.TopFlangeThickness = Isection.I_Section.t_fTop;
            sapISection.WebThickness = Isection.I_Section.t_w;

            sapISection.Height = Isection.I_Section.d;


            sapISection.BotFlangeMaterial.ConvertFromHndzMaterial(material);
            sapISection.WebMaterial.ConvertFromHndzMaterial(material);
            sapISection.TopFlangeMaterial.ConvertFromHndzMaterial(material);
            return sapISection;
        }

        internal static SAPISection GetAssumedEndTaperSection(SAPISection startSection)
        {
            ISectionAssumption previous = new ISectionAssumption(startSection.Height, startSection.BotFlangeWidth, startSection.BotFlangeThickness, startSection.WebThickness);
            ISectionAssumption next = ISectionAssumption.GetNextEndTaperAssumption(previous);

            StringBuilder nameStr = new StringBuilder();
            nameStr.Append("I ");
            nameStr.Append(next.Height * 100);
            nameStr.Append("x");
            nameStr.Append(next.WebThickness * 100);

            nameStr.Append("/");
            nameStr.Append(next.TopFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(next.TopFlangeThickness * 100);
            nameStr.Append("/");
            nameStr.Append(next.BotFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(next.BotFlangeThickness * 100);
            if (next == null)
            {
                return null;
            }

            SAPISection sec = new SAPISection(nameStr.ToString(), startSection.WebMaterial, startSection.TopFlangeMaterial, startSection.BotFlangeMaterial
                , next.Height, next.TopFlangeWidth, next.TopFlangeThickness, next.BotFlangeWidth, next.BotFlangeThickness, next.WebThickness);

            return sec;
        }

        public override SAPSection GetAssumedSection()
        {
            ISectionAssumption previous = new ISectionAssumption(Height, BotFlangeWidth, BotFlangeThickness, WebThickness);
            ISectionAssumption next = ISectionAssumption.GetNextAssumption(previous);

            StringBuilder nameStr = new StringBuilder();
            nameStr.Append("I ");
            nameStr.Append(next.Height * 100);
            nameStr.Append("x");
            nameStr.Append(next.WebThickness * 100);

            nameStr.Append("/");
            nameStr.Append(next.TopFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(next.TopFlangeThickness * 100);
            nameStr.Append("/");
            nameStr.Append(next.BotFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(next.BotFlangeThickness * 100);
            if (next == null)
            {
                return null;
            }

            SAPISection sec = new SAPISection(nameStr.ToString(), WebMaterial, TopFlangeMaterial, BotFlangeMaterial
                , next.Height, next.TopFlangeWidth, next.TopFlangeThickness, next.BotFlangeWidth, next.BotFlangeThickness, next.WebThickness);

            return sec;
        }

        public HndzISectionProfile ConvertToHndzIProfile(ref HndzStructuralMaterial material)
        {
            HndzISectionProfile profile = new HndzISectionProfile();
            profile.Name = Name;
            profile.I_Section = new SectionI(Name, Height, TopFlangeWidth, BotFlangeWidth, TopFlangeThickness, BotFlangeThickness, WebThickness);


            material = WebMaterial.ConvertToHndzMaterial();
            //material = TopFlangeMaterial.ConvertToHndzMaterial();
            //material = BotFlangeMaterial.ConvertToHndzMaterial();

            return profile;
        }

        public override SAPSection GetInitialSection(SAPMaterial mat)
        {
            ISectionAssumption assumption = ISectionAssumption.GetInitialSection();

            StringBuilder nameStr = new StringBuilder();
            nameStr.Append("I ");
            nameStr.Append(assumption.Height * 100);
            nameStr.Append("x");
            nameStr.Append(assumption.WebThickness * 100);

            nameStr.Append("/");
            nameStr.Append(assumption.TopFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(assumption.TopFlangeThickness * 100);
            nameStr.Append("/");
            nameStr.Append(assumption.BotFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(assumption.BotFlangeThickness * 100);
            if (assumption == null)
            {
                return null;
            }
            SAPISection sec = new SAPISection(nameStr.ToString(), mat, mat, mat
                , assumption.Height, assumption.TopFlangeWidth, assumption.TopFlangeThickness, assumption.BotFlangeWidth, assumption.BotFlangeThickness, assumption.WebThickness);

            return sec;
        }
    }
}
