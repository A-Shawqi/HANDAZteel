using STAADModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    class Trial3
    {
        StaadModel Model;
        enum Units
        {
            Inch,
            Feet,
            Feet2,
            CentiMeter,
            Meter,
            MilliMeter,
            DeciMeter,
            KiloMeter
        }
        enum ForceUnits
        {
            Kilopound,
            Pound,
            Kilogram,
            MetricTon,
            Newton,
            KiloNewton,
            MegaNewton,
            DecaNewton
        }
        string strfile = @"C:\Users\Mark Laway\Desktop\TestBolterGeist.std";
        public void OpenFIle()
        {
            using (FileStream fs = File.Open(@"C:\SProV8i SS6\STAAD\Staadpro.exe", FileMode.Open, FileAccess.Write, FileShare.None))
            {
                int units = (int)Units.Meter;
                int ForceUnitss = (int)ForceUnits.KiloNewton;
                OpenSTAADUI.OpenSTAAD StaadStart = new OpenSTAADUI.OpenSTAAD();
                StaadStart.NewSTAADFile(strfile, units, ForceUnitss);
            }
        }
    }
}
