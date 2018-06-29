using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProTaperedSection:STAADProSection
    {
        public STAADProTaperedSection():base(STAADProSectionTypeEnum.TAPERED)
        {
            
        }

        public STAADProTaperedSection(double startDepth, double webThickness, double endDepth, double topFlangeWidth, double topFlangeThickness, double botFlangeWidth, double botFlangeThickness):this()
        {
            StartDepth = startDepth;
            WebThickness = webThickness;
            EndDepth = endDepth;
            TopFlangeWidth = topFlangeWidth;
            TopFlangeThickness = topFlangeThickness;
            BotFlangeWidth = botFlangeWidth;
            BotFlangeThickness = botFlangeThickness;
        }

        public STAADProTaperedSection(double startDepth, double webThickness, double endDepth, double flangeWidth, double flangeThickness):this( startDepth, webThickness, endDepth, flangeWidth, flangeThickness, flangeWidth, flangeThickness)
        {
        }

        /// <summary>
        /// f1 = Depth of section at start node. 
        /// </summary>
        public double StartDepth { get; set; }
        /// <summary>
        /// f2 = Thickness of web. 
        /// </summary>
        public double WebThickness { get; set; }
        /// <summary>
        /// f3 = Depth of section at end node. 
        /// </summary>
        public double EndDepth { get; set; }
        /// <summary>
        /// f4 = Width of top flange. 
        /// </summary>
        public double TopFlangeWidth { get; set; }
        /// <summary>
        /// f5 = Thickness of top flange. 
        /// </summary>
        public double TopFlangeThickness { get; set; }
        /// <summary>
        /// f6 = Width of bottom flange. Defaults to f4 if left out. 
        /// </summary>
        public double BotFlangeWidth { get; set; }
        /// <summary>
        /// f7 = Thickness of bottom flange. Defaults to f5 left out
        /// </summary>
        public double BotFlangeThickness { get; set; }
    }
}
