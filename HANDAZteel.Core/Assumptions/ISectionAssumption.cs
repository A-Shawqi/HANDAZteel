using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.Core
{
    public class ISectionAssumption
    {
        public ISectionAssumption(double height, double topFlangeWidth, double topFlangeThickness, double botFlangeWidth, double botFlangeThickness, double webThickness)
        {
            Height = height;
            TopFlangeWidth = topFlangeWidth;
            TopFlangeThickness = topFlangeThickness;
            BotFlangeWidth = botFlangeWidth;
            BotFlangeThickness = botFlangeThickness;
            WebThickness = webThickness;
        }
        public ISectionAssumption(double height, double FlangeWidth, double FlangeThickness, double webThickness) : this(height, FlangeWidth, FlangeThickness, FlangeWidth, FlangeThickness, webThickness)
        {
        }
        public ISectionAssumption()
        {
        }

        public double Height { get; set; }
        public double TopFlangeWidth { get; set; }
        public double TopFlangeThickness { get; set; }
        public double BotFlangeWidth { get; set; }
        public double BotFlangeThickness { get; set; }
        public double WebThickness { get; set; }

        private static double mindoublehick = 0.004; //4mm
        private static double maxThick = 0.040; //40mm

        private static double maxWebHeight = 2.1; //2100mm for now
        private static double minWebHeight = 0.200; //200mm

        private static double maxFlangeWidth = 0.400; //400mm
        private static double minFlangeWidth = 0.125; //200mm

        private static double thickIncrementStep = 0.002;//1mm
        private static double flangeWidthIncrementStep = 0.01;//5mm
        private static double webHeightIncrementStep = 0.1;//100mm

        private static int limitCounter = 0;
        private static int sizeCounter = 1000;


        /// <summary>
        /// This function only handles Symmetric I Built up sections
        /// </summary>
        /// <param name="previousSection"></param>
        /// <returns></returns>
        public static ISectionAssumption GetNextAssumption(ISectionAssumption previousSection)
        {
            ISectionAssumption newSection = new ISectionAssumption();
            
            if (previousSection.Height - previousSection.BotFlangeWidth - previousSection.TopFlangeWidth < maxWebHeight)
            {
                newSection.Height = previousSection.Height + webHeightIncrementStep;
            }
            if (previousSection.BotFlangeWidth < maxFlangeWidth)
            {
                newSection.BotFlangeWidth = newSection.TopFlangeWidth = previousSection.BotFlangeWidth + flangeWidthIncrementStep;
            }

            if (previousSection.BotFlangeThickness < maxThick)
            {
                newSection.BotFlangeThickness = newSection.TopFlangeThickness = previousSection.BotFlangeThickness + thickIncrementStep;
            }
            if(previousSection.WebThickness < maxThick)
            {
                newSection.WebThickness = previousSection.WebThickness + thickIncrementStep;
            }

            if (newSection == previousSection)
            {
                return null;
            }
            else
            {
                newSection.AdjustSectionRatios();
                return newSection;
            }
        }

        public static ISectionAssumption GetNextEndTaperAssumption(ISectionAssumption previousSection)
        {
            ISectionAssumption newSection = new ISectionAssumption();
            //if (previousSection.WebThickness < maxThick)
            //{
            //    newSection.WebThickness = previousSection.WebThickness + thickIncrementStep;
            //}
            //if (previousSection.BotFlangeWidth < maxFlangeWidth)
            //{
            //    newSection.BotFlangeWidth = newSection.TopFlangeWidth = previousSection.BotFlangeWidth + flangeWidthIncrementStep;
            //}
            if ((previousSection.Height - previousSection.BotFlangeThickness - previousSection.TopFlangeThickness)*1.5< maxWebHeight)
            {
                newSection.Height = (previousSection.Height + webHeightIncrementStep)*1.5;
            }
            if (previousSection.BotFlangeThickness*1.5 < maxThick)
            {
                newSection.BotFlangeThickness = newSection.TopFlangeThickness =( previousSection.BotFlangeThickness + thickIncrementStep)*1.5;
            }
            if (newSection == previousSection)
            {
                return null;
            }
            else
            {
                newSection.AdjustSectionRatios();
                return newSection;
            }
            
        }

        private void AdjustSectionRatios()
        {
            AdjustWebRatios();
            AdjustTopFlangeRatios();
            AdjustBotFlangeRatios();
            AdjustWebBotFlangeRatios();
            AdjustWebTopFlangeRatios();
            AdjustWebTopFlangeThickRatios();
            AdjustWebBotFlangeThickRatios();
        }
        private void AdjustWebRatios()
        {
            if (WebThickness > 0)
            {
                if ((Height - BotFlangeThickness - TopFlangeThickness) / WebThickness <= 180)
                {
                    limitCounter = 0;
                    return;
                }
                else
                {
                    if (WebThickness < maxThick)
                    {
                        WebThickness += thickIncrementStep;
                    }
                    if (true)
                    {

                    }
                    if (limitCounter < sizeCounter)
                    {
                        AdjustWebRatios();
                        limitCounter++;
                    }

                    return;
                }
            }
        }
        private void AdjustTopFlangeRatios()
        {
            if (TopFlangeWidth / TopFlangeThickness <= 34)
            {

                limitCounter = 0;
                return;
            }
            else
            {
                if (TopFlangeThickness < maxThick)
                {
                    TopFlangeThickness += thickIncrementStep;
                }
                if (limitCounter < sizeCounter)
                {
                    AdjustTopFlangeRatios();
                    limitCounter++;
                }
                return;
            }
        }
        private void AdjustBotFlangeRatios()
        {
            if (BotFlangeWidth / BotFlangeThickness <= 34)
            {
                limitCounter = 0;
                return;
            }
            else
            {
                if (BotFlangeThickness < maxThick)
                {
                    BotFlangeThickness += thickIncrementStep;
                }
                if (limitCounter < sizeCounter)
                {
                    AdjustBotFlangeRatios();
                    limitCounter++;
                }
                return;
            }
        }
        private void AdjustWebBotFlangeRatios()
        {
            if (BotFlangeWidth !=0)
            {
                if ((Height - BotFlangeThickness - TopFlangeThickness) / BotFlangeWidth <= 5)
                {
                    limitCounter = 0;
                    return;
                }
                else
                {
                    if (BotFlangeWidth < maxFlangeWidth)
                    {
                        BotFlangeWidth += flangeWidthIncrementStep;
                    }
                    if (limitCounter < sizeCounter)
                    {
                        AdjustWebBotFlangeRatios();
                        limitCounter++;
                    }
                }
            }
            return;
        }

        public static ISectionAssumption GetInitialSection()
        {
            ISectionAssumption sec = new ISectionAssumption();
            sec.Height = minWebHeight + mindoublehick + mindoublehick;
            sec.BotFlangeWidth = sec.TopFlangeWidth = minFlangeWidth;
            sec.BotFlangeThickness = sec.TopFlangeThickness = mindoublehick;
            sec.WebThickness = mindoublehick;
            sec.AdjustSectionRatios();
            return sec;
        }

        private void AdjustWebTopFlangeRatios()

        {
            if (TopFlangeWidth>0)
            {
                if ((Height - BotFlangeThickness - TopFlangeThickness) / TopFlangeWidth <= 5)
                {
                    limitCounter = 0;
                    return;
                }
                else
                {
                    if (TopFlangeWidth < maxFlangeWidth)
                    {
                        TopFlangeWidth += flangeWidthIncrementStep;
                    }
                    if (limitCounter < sizeCounter)
                    {
                        AdjustWebTopFlangeRatios();
                        limitCounter++;
                    }
                }
                return;
            }
        }
        private void AdjustWebTopFlangeThickRatios()
        {
            if (TopFlangeThickness / WebThickness <= 2.5)
            {
                limitCounter = 0;
                return;
            }
            else
            {
                if (WebThickness < maxThick)
                {
                    WebThickness += thickIncrementStep;

                }
                if (limitCounter < sizeCounter)
                {
                    AdjustWebTopFlangeThickRatios();
                    limitCounter++;
                }
            }
            return;
        }
        private void AdjustWebBotFlangeThickRatios()
        {
            if (BotFlangeThickness / WebThickness <= 2.5)
            {
                limitCounter = 0;
                return;
            }
            else
            {
                if (WebThickness < maxThick)
                {
                    WebThickness += thickIncrementStep;
                }
                if (limitCounter < sizeCounter)
                {
                    AdjustWebTopFlangeThickRatios();
                    limitCounter++;
                }
                return;
            }
        }
    }
}
