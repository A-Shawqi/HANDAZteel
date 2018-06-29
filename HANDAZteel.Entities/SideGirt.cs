using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HANDAZ.PEB.Entities
{
    public class SideGirt
    {
        double sideGirtSpacing_Zdirection;
        bool hasSagRods;
        public double SideGirtSpacing_Zdirection
        {
            get
            {
                return sideGirtSpacing_Zdirection;
            }

            set
            {
                sideGirtSpacing_Zdirection = value;
            }
        }
        public bool HasSagRods
        {
            get
            {
                return hasSagRods;
            }

            set
            {
                hasSagRods = value;
            }
        }
    }
}