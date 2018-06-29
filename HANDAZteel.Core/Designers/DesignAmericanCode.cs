using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.Core.Designers
{
    class DesignAmericanCode
    {
        public static void Design()
        {
            //Pr = Required Axial Strenth
            //Pe Design Axial Tensile Strength
            //Required Flextural Strength
            //design flexural strength

            double Pr = 0;
            double Pc = 0;
            double PrPc = Pr / Pc;

            double Mrx = 0;
            double Mcx = 0;
            double Mry = 0;
            double Mcy = 0;

            bool ColumnSafe;
            if (PrPc >= 0.2)
            {
                if (PrPc + (8 / 9) * ((Mrx / Mcx) + (Mry / Mcy)) <= 1.0)
                {
                    ColumnSafe = true;
                }
                else
                {
                    ColumnSafe = false;
                }
            }
            else
            {
                if ((PrPc/2) + (8 / 9) * ((Mrx / Mcx) + (Mry / Mcy)) <= 1.0)
                {
                    ColumnSafe = true;
                }
                else
                {
                    ColumnSafe = false;
                }
            }

        }
    }
}
