using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using HANDAZ.Entities;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public enum LengthTypeEnum
    {
        Relative = 1,
        Absolute = 2
    }
    public class SAPITaperedSection:SAPSection
    {
        public SAPITaperedSection(string name, SAPISection startSection, SAPISection endSection, double length, LengthTypeEnum lengthType = LengthTypeEnum.Relative)
        {
            LengthType = lengthType;
            Name = name;
            StartSection = startSection;
            EndSection = endSection;
            Length = length;
        }

        public SAPITaperedSection()
        {
            
        }
        public SAPISection StartSection { get; set; }
        public SAPISection EndSection { get; set; }
        public LengthTypeEnum LengthType { get; set; }
        private double length;

        public double Length
        {
            get { return length; }
            set {
                if (LengthType == LengthTypeEnum.Relative)
                {
                    if (value < 0 || value >1)
                    {
                        throw new Exception("Built-Up section length is assigned relative but sent as an absolute value");
                    }
                }

                length = value; }
        }

        public void ConvertFromHndzTaperedI(HndzITaperedProfile profile, HndzStructuralMaterial material)
        {
            Name = profile.Name;
            Length = 1;
            LengthType = LengthTypeEnum.Relative;
            StartSection.ConvertFromHndzIProfile(profile.StartProfile, material);
            EndSection.ConvertFromHndzIProfile(profile.EndProfile, material);
        }

        public HndzITaperedProfile ConvertToHndzTaperedI(ref HndzStructuralMaterial material)
        {
            HndzITaperedProfile profile = new HndzITaperedProfile();
            profile.Name = Name;
            profile.StartProfile = StartSection.ConvertToHndzIProfile(ref material);
            profile.EndProfile = EndSection.ConvertToHndzIProfile(ref material);
            return profile;
        }
        public override SAPSection GetAssumedSection()
        {
            SAPISection startSection = (SAPISection)StartSection.GetAssumedSection();
            //SAPISection endSection = SAPISection.GetAssumedEndTaperSection(StartSection);
            SAPISection endSection;
            if (Math.Abs(EndSection.Height - startSection.Height) <= 40)
            {
                endSection = (SAPISection)EndSection.GetAssumedSection();
                endSection = (SAPISection)endSection.GetAssumedSection();
            }
            else
            {
                endSection = EndSection;
            }

            StringBuilder nameStr = new StringBuilder();
            nameStr.Append("I ");
            nameStr.Append(startSection.Height*100);
            nameStr.Append("-");
            nameStr.Append(endSection.Height*100);
            nameStr.Append("x");
            nameStr.Append(startSection.WebThickness*100);
            nameStr.Append("-");
            nameStr.Append(endSection.WebThickness*100);

            nameStr.Append("/");
            nameStr.Append(startSection.TopFlangeWidth*100);
            nameStr.Append("x");
            nameStr.Append(startSection.TopFlangeThickness*100);
            nameStr.Append("/");
            nameStr.Append(startSection.BotFlangeWidth*100);
            nameStr.Append("x");
            nameStr.Append(startSection.BotFlangeThickness*100);
            if (startSection == null)
            {
                return null;
            }
            else if (endSection == null)
            {
                endSection = startSection;
            }
            return new SAPITaperedSection(nameStr.ToString(),startSection,endSection,1);
        }
        public override SAPSection GetInitialSection(SAPMaterial mat)
        {
            SAPISection startSection = new SAPISection();
            SAPISection endSection = new SAPISection();
            startSection.GetInitialSection(mat);
            startSection.GetInitialSection(mat);
            endSection.Height *= 3;

            StringBuilder nameStr = new StringBuilder();
            nameStr.Append("I ");
            nameStr.Append(startSection.Height*100);
            nameStr.Append("-");
            nameStr.Append(endSection.Height*100);
            nameStr.Append("x");
            nameStr.Append(startSection.WebThickness * 100);
            nameStr.Append("-");
            nameStr.Append(endSection.WebThickness * 100);

            nameStr.Append("/");
            nameStr.Append(startSection.TopFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(startSection.TopFlangeThickness * 100);
            nameStr.Append("/");
            nameStr.Append(startSection.BotFlangeWidth * 100);
            nameStr.Append("x");
            nameStr.Append(startSection.BotFlangeThickness * 100);
            if (startSection == null || endSection == null)
            {
                return null;
            }
            return new SAPITaperedSection(nameStr.ToString(), startSection, endSection, 1);
        }
    }
}
