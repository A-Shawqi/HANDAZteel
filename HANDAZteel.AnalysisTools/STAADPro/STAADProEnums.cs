using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public enum STAADProSectionTypeEnum
    {
        TAPERED
        //TODO Add more types
    }
    public enum STAADProSupportTypeEnum
    {
        PINNED
        //TODO
    }
    public enum STAADProLoadTypeEnum
    {
        DEAD, LIVE, WIND, NONE
    }
    public enum STAADProDirectionEnum
    {
        X , Y , Z , GX , GY , GZ , PX , PY , PZ
    }
}
