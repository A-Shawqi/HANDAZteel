using OpenSTAADUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using HANDAZ.PEB.Core;
using static HANDAZ.PEB.Core.ASCE107Wind;

namespace MarkTest
{
    public static class StaadFileWriter
    {
        public static bool GenerateFile(string File_Path, float DL, float CL, float LL, float Bay, float B_W, float B_L, float E_HT, float R_HT, RoofType Roof_Type, float WindSpeed, float TopoFactor,
         float DirectFator, RiskCategory RiskCategory,
         ExposureCategory ExposureCategory, bool IsEnclosed, bool IsHuricane)
        {
            double WL_1, WL_2, WL_3, WL_4, WL_1N, WL_2N, WL_3N, WL_4N, WL_1_B, WL_2_B, WL_1_BN, WL_2_BN;

            ASCE107Wind.LoadParameters(WindSpeed, RiskCategory, ExposureCategory, R_HT, E_HT, B_L, B_W, Roof_Type, TopoFactor, DirectFator, IsEnclosed, IsHuricane);
            //Check_1 = txt_Check_1.Text;
            //Check_2 = txt_Check_2.Text;
            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 1", out WL_1);
            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 2", out WL_2);
            ASCE107Wind.netPositivePressureA.TryGetValue("Roof Zone 3", out WL_3);
            ASCE107Wind.netPositivePressureA.TryGetValue("Wall Zone 4", out WL_4);

            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 1", out WL_1N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 2", out WL_2N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Roof Zone 3", out WL_3N);
            ASCE107Wind.netNegativePressureA.TryGetValue("Wall Zone 4", out WL_4N);

            ASCE107Wind.netPositivePressureB.TryGetValue("Roof Zone 2", out WL_1_B);
            ASCE107Wind.netPositivePressureB.TryGetValue("Wall Zone 5", out WL_2_B);

            ASCE107Wind.netNegativePressureB.TryGetValue("Roof Zone 2", out WL_1_BN);
            ASCE107Wind.netNegativePressureB.TryGetValue("Wall Zone 5", out WL_2_BN);

            if (IsLowRise(B_L, B_W))
            {
                StreamWriter Writer = new StreamWriter(File_Path.ToString());
                Writer.WriteLine("STAAD PLANE");
                Writer.WriteLine("INPUT WIDTH 79");
                Writer.WriteLine("UNIT METER KN");
                Writer.WriteLine("JOINT COORDINATES");
                Writer.WriteLine("1 0 0");
                Writer.WriteLine("2 0 " + E_HT);
                //Marks Edit 
                if (Roof_Type == RoofType.Gable)
                {
                    Writer.WriteLine("3 " + B_W / 2 + " " + R_HT);
                    Writer.WriteLine("4 " + B_W + " " + E_HT);
                }
                else if (Roof_Type == RoofType.Monoslope)
                {
                    Writer.WriteLine("3 " + B_W / 2 + " " + (R_HT + E_HT) / 2);
                    Writer.WriteLine("4 " + B_W + " " + R_HT);
                }
                //   Writer.WriteLine("3 " + B_W/2 + " "  + R_HT); 
                //   Writer.WriteLine("4" + B_W + " " + E_HT);
                Writer.WriteLine("5 " + B_W + " " + "0" + "\n");
                Writer.WriteLine("MEMBER INCIDENCES");
                Writer.WriteLine("1 2 1");
                Writer.WriteLine("2 2 3");
                Writer.WriteLine("3 4 3");
                Writer.WriteLine("4 4 5");
                Writer.WriteLine("DEFINE MATERIAL START");
                Writer.WriteLine("ISOTROPIC STEEL");
                Writer.WriteLine("E 205000000");
                Writer.WriteLine("Poisson 0.3");
                Writer.WriteLine("DENSITY 76.8195");
                Writer.WriteLine("ALPHA 0.000012");
                Writer.WriteLine("DAMP 0.03");
                Writer.WriteLine("END DEFINE MATERIAL");
                Writer.WriteLine("UNIT MMS KN");
                Writer.WriteLine("MEMBER PROPERTY AMERICAN");
                Writer.WriteLine("1 TAPERED 770 5 270 200 10 200 10");
                Writer.WriteLine("2 TAPERED 770 5 520 175 10 175 10");
                Writer.WriteLine("3 TAPERED 770 5 520 175 10 175 10");
                Writer.WriteLine("4 TAPERED 770 5 270 200 10 200 10");
                Writer.WriteLine("UNIT METER KN");
                Writer.WriteLine("CONSTANTS");
                Writer.WriteLine("MATERIAL STEEL ALL");
                Writer.WriteLine("SUPPORTS");
                Writer.WriteLine("1 5 " + "PINNED");
                Writer.WriteLine("LOAD 1 LOADTYPE None  TITLE DL");
                Writer.WriteLine("SELFWEIGHT Y -1");
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("1 2 3 4 UNI GY " + (DL * Bay * (-1)));
                Writer.WriteLine("LOAD 2 LOADTYPE None  TITLE CL");
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("2 3 UNI GY " + (CL * Bay * (-1)));
                Writer.WriteLine("LOAD 3 LOADTYPE None  TITLE LL");
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("2 3 UNI GY " + (LL * Bay * (-1)));
                Writer.WriteLine("******** WIND LOAD CASES");
                Writer.WriteLine("LOAD 4 LOADTYPE None  TITLE WLB");
                Writer.WriteLine("MEMBER Load");
                if (WL_1 < 0)
                {
                    Writer.WriteLine("1 UNI GX " + (Math.Abs(WL_1) * Bay * (-1)));
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_1) * Bay);
                }
                if (WL_2 < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2) * Bay * (-1));
                }
                if (WL_3 < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3) * Bay * (-1));
                }
                if (WL_4 < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4) * Bay * (-1));
                }
                Writer.WriteLine("LOAD 5 LOADTYPE None  TITLE WLU");
                Writer.WriteLine("MEMBER Load");
                if (WL_1N < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_1N) * Bay * (-1));
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_1N) * Bay);
                }
                if (WL_2N < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2N) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_2N) * Bay * (-1));
                }
                if (WL_3N < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3N) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_3N) * Bay * (-1));
                }
                if (WL_4N < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4N) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_4N) * Bay * (-1));
                }
                Writer.WriteLine("LOAD 6 LOADTYPE None  TITLE WRB");
                Writer.WriteLine("MEMBER Load");
                if (WL_4 < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4) * (-1) * Bay);
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4) * Bay);
                }
                if (WL_3 < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3) * Bay * (-1));
                }
                if (WL_2 < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2) * Bay * (-1));
                }
                if (WL_1 < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1) * Bay * (-1));
                }
                Writer.WriteLine("LOAD 7 LOADTYPE None  TITLE WRU");
                Writer.WriteLine("MEMBER Load");
                if (WL_4N < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4N) * (-1) * Bay);
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_4N) * Bay);
                }
                if (WL_3N < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3N) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_3N) * Bay * (-1));
                }
                if (WL_2N < 0)
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2N) * Bay);
                }
                else
                {
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_2N) * Bay * (-1));
                }
                if (WL_1N < 0)
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1N) * Bay);
                }
                else
                {
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_1N) * Bay * (-1));
                }
                Writer.WriteLine("MEMBER Load");
                Writer.WriteLine("LOAD 8 LOADTYPE None  TITLE WEB");
                Writer.WriteLine("MEMBER Load");
                if (WL_2_B < 0)
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_B) * (-1) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_B) * Bay);
                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_B) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_B) * Bay * (-1));
                }
                if (WL_1_B < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_B) * Bay);
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_B) * Bay);
                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_B) * Bay * (-1));
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_B) * Bay * (-1));
                }

                Writer.WriteLine("LOAD 9 LOADTYPE None  TITLE WEU");
                Writer.WriteLine("MEMBER Load");
                if (WL_2_BN < 0)
                {

                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_BN) * (-1) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_BN) * Bay);

                }
                else
                {
                    Writer.WriteLine("1 UNI GX " + Math.Abs(WL_2_BN) * Bay);
                    Writer.WriteLine("4 UNI GX " + Math.Abs(WL_2_BN) * Bay * (-1));

                }
                if (WL_1_BN < 0)
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_BN) * Bay);
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_BN) * Bay);

                }
                else
                {
                    Writer.WriteLine("2 UNI Y " + Math.Abs(WL_1_BN) * Bay * (-1));
                    Writer.WriteLine("3 UNI Y " + Math.Abs(WL_1_BN) * Bay * (-1));
                }
                Writer.WriteLine("LOAD COMB 1000 DL+LL");
                Writer.WriteLine("1 1.0 3 1.0");
                Writer.WriteLine("LOAD COMB 1001 DL+CL");
                Writer.WriteLine("1 1.0 2 1.0");
                Writer.WriteLine("LOAD COMB 1002 DL+CL+LL");
                Writer.WriteLine("1 1.0 2 1.0 3 1.0");
                Writer.WriteLine("LOAD COMB 1003 DL+WLB");
                Writer.WriteLine("1 1.0 4 1.0");
                Writer.WriteLine("LOAD COMB 1004 DL+WLU");
                Writer.WriteLine("1 1.0 5 1.0");
                Writer.WriteLine("LOAD COMB 1005 DL+WRB");
                Writer.WriteLine("1 1.0 6 1.0");
                Writer.WriteLine("LOAD COMB 1006 DL+WRU");
                Writer.WriteLine("1 1.0 7 1.0");
                Writer.WriteLine("LOAD COMB 1007 DL+WEB");
                Writer.WriteLine("1 1.0 8 1.0");
                Writer.WriteLine("LOAD COMB 1008 DL+WEU");
                Writer.WriteLine("1 1.0 9 1.0");
                Writer.WriteLine("LOAD COMB 1009 DL+CL+WLB");
                Writer.WriteLine("1 1.0 2 1.0 4 1.0");
                Writer.WriteLine("LOAD COMB 1010 DL+CL+WLU");
                Writer.WriteLine("1 1.0 2 1.0 5 1.0");
                Writer.WriteLine("LOAD COMB 1011 DL+CL+WRB");
                Writer.WriteLine("1 1.0 2 1.0 6 1.0");
                Writer.WriteLine("LOAD COMB 1012 DL+CL+WRU");
                Writer.WriteLine("1 1.0 2 1.0 7 1.0");
                Writer.WriteLine("LOAD COMB 1013 DL+CL+WEB");
                Writer.WriteLine("1 1.0 2 1.0 8 1.0");
                Writer.WriteLine("LOAD COMB 1014 DL+CL+WEU");
                Writer.WriteLine("1 1.0 2 1.0 9 1.0");
                Writer.WriteLine("PERFORM ANALYSIS PRINT ALL");
                Writer.WriteLine("LOAD LIST 1000 TO 1014");
                Writer.WriteLine("Parameter 1");
                Writer.WriteLine("CODE AISC UNIFIED 2005");
                Writer.WriteLine("Method ASD");
                Writer.WriteLine("FYLD 345000 ALL");
                Writer.WriteLine("BEAM 1 ALL");
                Writer.WriteLine("*TRACK 2 ALL");
                Writer.WriteLine("CB 0 ALL");
                Writer.WriteLine("****LZ,KZ");
                Writer.WriteLine("KZ 1.5 MEMB 1 4");
                Writer.WriteLine("LZ " + B_W + " MEMB 2 3");
                Writer.WriteLine("KY 1.0 MEMB 1 4");
                Writer.WriteLine("LY 1.5 MEMB 2 3");
                Writer.WriteLine("UNB " + E_HT + " MEMB 1 4");
                Writer.WriteLine("UNB 1.5 MEMB 2 3");
                Writer.WriteLine("UNT " + E_HT + " MEMB 1 4");
                Writer.WriteLine("UNT 1.5 MEMB 2 3");
                Writer.WriteLine("****************************** CHECK CODE AND WEIGHT");
                Writer.WriteLine("CHECK CODE ALL");
                Writer.WriteLine("UNIT METER KG");
                Writer.WriteLine("STEEL TAKE OFF ALL");
                Writer.WriteLine("FINISH");
                Writer.Close();
                return true;
                //return MessageBox.Show("STAAD FILE IS GENERATED AND SAVED SUCCESSFULLY");
            }
            else
            {
                return false;
                //return MessageBox.Show("Building is NOT Low Rise");
            }
        }
        public static void openStaad()
        {
            Process.Start(@"C:\Users\Mark Laway\Source\Workspaces\Workspace\HANDAZteel\HANDAZteel\HANDAZteel.AnalysisTools\bin\Debug\HandazPEB.Std");
        }
        public static OpenSTAAD GetSTAADUIInterface(string staadFilename)
        {
            OpenSTAAD osObject = null;
            try
            {
                Type type = Type.GetTypeFromProgID("StaadPro.OpenSTAAD");
                string clsid = type.GUID.ToString();
                string lookUpCandidateName = String.Format("!{0}{1}{2}", "{", clsid, "}").ToUpper();
                Hashtable runningObjects = GetRunningObjectTable();
                IDictionaryEnumerator rotEnumerator = runningObjects.GetEnumerator();
                while (rotEnumerator.MoveNext())
                {
                    string candidateName = ((string)rotEnumerator.Key).ToUpper();
                    // check if its a valid STAAD file
                    string extStr = Path.GetExtension(candidateName);
                    if (extStr != null)
                    {
                        if (!String.IsNullOrEmpty(extStr))
                        {
                            if (extStr == ".STD")
                            {
                                if (String.Compare(staadFilename.ToUpper(), candidateName.ToUpper()) == 0)
                                {
                                    // we have a std file opened, but this can be opened by other applications also,
                                    // we need to get the staadobject to see if it is opened by STAAD
                                    osObject = System.Runtime.InteropServices.Marshal.BindToMoniker(candidateName) as
                                    OpenSTAAD;
                                    if (osObject != null)
                                    {
                                        return osObject;
                                    }
                                }
                            }
                        }
                    }
                    if (candidateName.StartsWith(lookUpCandidateName))
                    {
                        osObject = rotEnumerator.Value as OpenSTAAD;
                        if (osObject != null)
                        {
                            // get staad filename
                            string lookupStaadFilename = "";
                            bool includeFullPath = true;
                            Object oArg1 = lookupStaadFilename as object;
                            Object oArg2 = includeFullPath as object;
                            osObject.GetSTAADFile(ref oArg1, oArg2);
                            lookupStaadFilename = oArg1 as string;
                            //
                            if (String.Compare(staadFilename.ToUpper(), lookupStaadFilename.ToUpper()) == 0)
                            {
                                return osObject;
                            }
                        }
                    }
                }
                // alternative 2
                //OpenSTAAD osObject = System.Runtime.InteropServices.Marshal.BindToMoniker(staadFileName);
            }
            catch (COMException)
            {
                throw;
            }
            return null;
        }

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out System.Runtime.InteropServices.ComTypes.IRunningObjectTable prot);
        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(int reserved, out System.Runtime.InteropServices.ComTypes.IBindCtx ppbc);
        private static Hashtable GetRunningObjectTable()
        {
            try
            {
                Hashtable result = new Hashtable();
                IntPtr numFetched = new IntPtr();
                System.Runtime.InteropServices.ComTypes.IRunningObjectTable runningObjectTable;
                System.Runtime.InteropServices.ComTypes.IEnumMoniker monikerEnumerator;
                System.Runtime.InteropServices.ComTypes.IMoniker[] monikers = new System.Runtime.InteropServices.ComTypes.IMoniker[1];
                GetRunningObjectTable(0, out runningObjectTable);
                runningObjectTable.EnumRunning(out monikerEnumerator);
                monikerEnumerator.Reset();
                while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
                {
                    System.Runtime.InteropServices.ComTypes.IBindCtx ctx;
                    CreateBindCtx(0, out ctx);
                    string runningObjectName;
                    monikers[0].GetDisplayName(ctx, null, out runningObjectName);
                    object runningObjectVal;
                    runningObjectTable.GetObject(monikers[0], out runningObjectVal);
                    result[runningObjectName] = runningObjectVal;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
