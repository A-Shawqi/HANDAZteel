using System;
using System.Collections.Generic;
using System.Linq;
using SAP2000v18;
using HANDAZ.Entities;
using System.Collections;
using System.IO;

namespace HANDAZ.PEB.AnalysisTools.CsiSAP2000
{
    public enum SAPAnalysisDOFs
    {
        SpaceFrame,
        PlaneFrame,
        PlaneGrid,
        SpaceTruss
    }
    public static class SAP2000API //: IAnalysisTool //TOBE Implemented no matter what or how 
    {
        #region Members
        public static string ProgramPath { get; set; }
        public static string ModelDirectory { get; set; }
        private static cOAPI mySapObject;
        private static cSapModel mySapModel;
        #endregion
        #region Constructors
        static SAP2000API()
        {
            mySapObject = null;
            mySapModel = null;
            ProgramPath = SAP2000Resources.ProgramPath;
            ModelDirectory = SAP2000Resources.ModelDirectory;
            if (!System.IO.Directory.Exists(ModelDirectory))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(ModelDirectory);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
        #region Interface Methods
        public static bool StartApplication(bool attachToInstance, bool isVisible, eUnits units = eUnits.Ton_m_C)
        {
            if (attachToInstance == false)
            {
                try
                {
                    cHelper myHelper = new Helper();
                    mySapObject = myHelper.CreateObject(ProgramPath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    mySapObject = (cOAPI)System.Runtime.InteropServices.Marshal.GetActiveObject("CSI.SAP2000.API.SapObject");
                }
                catch
                {
                    try
                    {
                        if (mySapObject == null)
                        {
                            cHelper myHelper = new Helper();
                            mySapObject = myHelper.CreateObject(ProgramPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                }
            }
            if (isVisible == true)
            {
                mySapObject.Visible();
            }
            else
            {
                mySapObject.Hide();
            }
            mySapObject.ApplicationStart(units);
            mySapModel = mySapObject.SapModel;
            return true;
        }
        public static bool CloseApplication()
        {
            int flag = mySapObject.ApplicationExit(true);
            mySapModel = null;
            mySapObject = null;
            if (flag != 0)
            {
                return false;
            }
            return true;
        }
        public static bool NewModel(eUnits units = eUnits.Ton_m_C)
        {
            mySapModel = mySapObject.SapModel;
            int flag = default(int);
            flag = mySapModel.InitializeNewModel(units);
            flag = mySapModel.File.NewBlank();
            if (flag != 0)
            {
                return false;
            }
            //SetDefaultMaterials();
            return true;
        }

        public static bool OpenModel(string modelName)
        {
            string modelPath = ModelDirectory + System.IO.Path.DirectorySeparatorChar + modelName + ".sdb";
            int flag = mySapModel.File.OpenFile(modelPath);
            if (flag != 0)
            {
                return false;
            }
            return true;
        }

        public static bool SaveModel(string modelName)
        {
            string modelPath = ModelDirectory + modelName + ".sdb";
            string fullPath = Path.GetFullPath(modelPath);
            int flag = mySapModel.File.Save(fullPath);

            if (flag != 0)
            {
                return false;
            }
            return true;
        }
        public static string GetPath(string modelName)
        {
            return Path.GetFullPath(ModelDirectory + System.IO.Path.DirectorySeparatorChar + modelName + ".sdb");
        }
        public static bool AnalayzeModel()
        {
            int flag = mySapModel.Analyze.RunAnalysis();
            if (flag != 0)
            {
                return false;
            }
            return true;
        }
        public static bool AnalayzeModel(SAPRestraint analyzedDOFs)
        {
            mySapModel.Analyze.SetActiveDOF(ref analyzedDOFs.values);
            return AnalayzeModel();
        }
        public static bool AnalayzeModel(SAPAnalysisDOFs analyzedDOFs)
        {
            SAPRestraint DOFs = null;
            switch (analyzedDOFs)
            {
                case SAPAnalysisDOFs.SpaceFrame:
                    DOFs = new SAPRestraint(true, true, true, true, true, true);
                    break;
                case SAPAnalysisDOFs.PlaneFrame:
                    DOFs = new SAPRestraint(true, false, true, false, true, false);
                    break;
                case SAPAnalysisDOFs.PlaneGrid:
                    DOFs = new SAPRestraint(false, false, true, true, true, false);
                    break;
                case SAPAnalysisDOFs.SpaceTruss:
                    DOFs = new SAPRestraint(true, true, true, false, false, false);
                    break;
            }
            mySapModel.Analyze.SetActiveDOF(ref DOFs.values);
            return AnalayzeModel();
        }

        #endregion
        #region Definition Methods
        #region Materials
        public static SAPMaterial[] SetDefaultMaterials()
        {
            SAPMaterial[] materials = new SAPMaterial[3];
            materials[0] = new SAPSteelMaterial("StGr37", 7.85, 21000000, 0.3, 1.170E-05, 24000, 36000, 24000, 36000);
            materials[1] = new SAPSteelMaterial("StGr44", 7.85, 21000000, 0.3, 1.170E-05, 28000, 44000, 28000, 44000);
            materials[2] = new SAPSteelMaterial("StGr52", 7.85, 21000000, 0.3, 1.170E-05, 36000, 52000, 36000, 52000);
            // materials[3] = new SAPConcreteMaterial("RC300", 2400);

            foreach (SAPMaterial material in materials)
            {
                DefineCustomMaterial(material);
            }
            return materials;
        }
        public static bool DefineCustomMaterial(SAPMaterial material)
        {
            int flag = mySapModel.PropMaterial.SetMaterial(material.MatName, material.MatType);

            flag = mySapModel.PropMaterial.SetWeightAndMass(material.MatName, 1, material.Weight);

            flag = mySapModel.PropMaterial.SetMPIsotropic(material.MatName, material.ElasticityModulus, material.PoissonRatio, material.ThermalCoef);

            switch (material.MatType)
            {
                case eMatType.Steel:
                    SAPSteelMaterial steelMaterial = (SAPSteelMaterial)material;
                    flag = mySapModel.PropMaterial.SetOSteel(steelMaterial.MatName, steelMaterial.Fy, steelMaterial.Fu, steelMaterial.eFy, steelMaterial.eFu, 1, 0, 0.015, 0.11, 0.17);
                    break;
                case eMatType.Concrete:
                    SAPConcreteMaterial rcMaterial = (SAPConcreteMaterial)material;
                    //TODO   //mySapModel.PropMaterial.SetOConcrete(rcMaterial.MatName 
                    break;
                case eMatType.NoDesign:
                    break;
                case eMatType.Aluminum:
                    break;
                case eMatType.ColdFormed:
                    break;
                case eMatType.Rebar:
                    break;
                case eMatType.Tendon:
                    break;
                case eMatType.Masonry:
                    break;
                default:
                    break;
            }
            if (flag != 0)
            {
                material.IsDefinedInSAP = false;
                return false;
            }
            else
            {
                material.IsDefinedInSAP = true;
                return true;

            }
        }
        #endregion
        #region Sections
        /// <summary>
        /// The general function that takes any type of sections and define it in SAP2000
        /// </summary>
        /// <param name="section"></param>
        public static void AddSection(SAPSection section)
        {
            if (section is SAPISection)
            {
                SetISection((SAPISection)section);
            }
            else if (section is SAPITaperedSection)
            {
                SetITaperedSection((SAPITaperedSection)section);
            }
            //TODO else if another section .. etc
        }
        public static bool SetISection(SAPISection Isection)
        {
            //TODO extract dimensions from shawkey's section
            string name = Isection.Name;
            string topFlangMat = Isection.TopFlangeMaterial.MatName;
            string webMat = Isection.WebMaterial.MatName;
            string botFlangMat = Isection.BotFlangeMaterial.MatName;
            if (Isection.TopFlangeMaterial.IsDefinedInSAP == false)
            {
                DefineCustomMaterial(Isection.TopFlangeMaterial);
            }
            if (Isection.BotFlangeMaterial.IsDefinedInSAP == false)
            {
                DefineCustomMaterial(Isection.BotFlangeMaterial);
            }
            if (Isection.WebMaterial.IsDefinedInSAP == false)
            {
                DefineCustomMaterial(Isection.WebMaterial);
            }
            int flag = mySapModel.PropFrame.SetHybridISection(name, topFlangMat, webMat, botFlangMat, Isection.Height, Isection.TopFlangeWidth,
                Isection.TopFlangeThickness, Isection.WebThickness, Isection.BotFlangeWidth, Isection.BotFlangeThickness);
            if (flag != 0)
            {
                Isection.IsDefinedInSAP = false;
                return false;
            }
            Isection.IsDefinedInSAP = true;
            return true;
        }
        /// <summary>
        /// This function assumes that an IBuiltUpSection consists of a start and end section, further development to send an array of sections is recommended
        /// </summary>
        /// <param name="IBuiltUpSection"></param>
        /// <returns></returns>
        public static bool SetITaperedSection(SAPITaperedSection IBuiltUpSection)
        {

            string[] startSections = { IBuiltUpSection.StartSection.Name };
            string[] endSections = { IBuiltUpSection.EndSection.Name };
            double[] lengths = { IBuiltUpSection.Length };
            int[] types = { (int)IBuiltUpSection.LengthType };
            int[] EI33 = { 3 }; //Cubic change in inertia @ axis 3 -3 
            int[] EI22 = { 1 };//Linear change in inertia @ axis 2-2
            if (IBuiltUpSection.StartSection.IsDefinedInSAP == false)
            {
                SetISection(IBuiltUpSection.StartSection);
            }
            if (IBuiltUpSection.EndSection.IsDefinedInSAP == false)
            {
                SetISection(IBuiltUpSection.EndSection);
            }
            int flag = mySapModel.PropFrame.SetNonPrismatic(IBuiltUpSection.Name, 1, ref startSections, ref endSections, ref lengths, ref types, ref EI33, ref EI22);
            if (flag != 0)
            {
                IBuiltUpSection.IsDefinedInSAP = false;
                return false;
            }
            IBuiltUpSection.IsDefinedInSAP = true;
            return true;
        }

        public static SAPSection GetSection(SAPSection section)
        {
            if (section is SAPISection)
            {
                SAPISection temp = (SAPISection)section;
                temp = GetISection(temp);
                section = temp;
            }
            else if (section is SAPITaperedSection)
            {
                SAPITaperedSection temp = (SAPITaperedSection)section;
                temp = GetITaperedSection(temp);
                section = temp;
            }

            //TODO else if another section .. etc
            return section;
        }

        public static SAPITaperedSection GetITaperedSection(SAPITaperedSection section)
        {
            int NumberItems = 0;
            string[] StartSec = new string[1];
            string[] EndSec = new string[1];
            double[] MyLength = new double[1];
            int[] MyType = new int[1];
            int[] EI33 = new int[1];
            int[] EI22 = new int[1];
            int Color = 0;
            string Notes = string.Empty;
            string GUID = string.Empty;
            int flag = mySapModel.PropFrame.GetNonPrismatic(section.Name, ref NumberItems, ref StartSec, ref EndSec, ref MyLength, ref MyType, ref EI33, ref EI22, ref Color, ref Notes, ref GUID);
            if (flag != 0)
            {
                return null;
            }
            else
            {
                //Updating the section with the new sections
                section.StartSection = GetISection(section.StartSection);
                section.EndSection = GetISection(section.EndSection);

                section.Length = MyLength[0];
                section.LengthType = (LengthTypeEnum)MyType[0];
                section.IsDefinedInSAP = true;
            }
            return section;
        }

        public static SAPISection GetISection(SAPISection section)
        {
            string MatPropTopFlange = string.Empty;
            string MatPropWeb = string.Empty;
            string MatPropBotFlange = string.Empty;
            double T3 = 0;
            double T2 = 0;
            double Tf = 0;
            double Tw = 0;
            double T2b = 0;
            double Tfb = 0;
            int Color = 0;
            string Notes = string.Empty;
            string GUID = string.Empty;
            int flag = mySapModel.PropFrame.GetHybridISection(section.Name, ref MatPropTopFlange, ref MatPropWeb, ref MatPropBotFlange, ref T3, ref T2, ref Tf, ref Tw, ref T2b, ref Tfb, ref Color, ref Notes, ref GUID);
            if (flag != 0)
            {
                return null;
            }
            else
            {
                section.TopFlangeMaterial.MatName = MatPropBotFlange;
                section.WebMaterial.MatName = MatPropWeb;
                section.BotFlangeMaterial.MatName = MatPropBotFlange;

                section.TopFlangeWidth = T2;
                section.TopFlangeThickness = Tf;

                section.BotFlangeWidth = T2b;
                section.BotFlangeThickness = Tfb;

                section.WebThickness = Tw;
                section.IsDefinedInSAP = true;
            }
            return section;
        }
        public static bool DeleteSection(SAPSection sec)
        {
            int flag = mySapModel.PropFrame.Delete(sec.Name);
            if (flag != 0)
            {
                return false;
            }
            else
            {
                sec.IsDefinedInSAP = false;
                return true;
            }
        }
        public static bool DeleteTaperedSection(SAPITaperedSection sec)
        {
            int flag = mySapModel.PropFrame.Delete(sec.Name);
            if (flag != 0)
            {
                return false;
            }
            else
            {
                sec.IsDefinedInSAP = false;
                DeleteSection(sec.StartSection);
                DeleteSection(sec.EndSection);

                return true;
            }
        }
        #endregion
        #region Loads
        public static bool AddLoadPattern(SAPLoadPattern pattern)
        {
            int flag = mySapModel.LoadPatterns.Add(pattern.Name, pattern.Type, pattern.SelfWeightMultiplyer, pattern.IsAnalysisCase);
            if (flag != 0)
            {
                pattern.IsDefinedInSAP = false;
                return false;
            }
            pattern.IsDefinedInSAP = true;
            return true;
        }
        public static bool AddLoadCombination(SAPLoadCombination comb)
        {
            int flag = mySapModel.RespCombo.Add(comb.Name, (int)comb.CombType);//Adding a new combination
            if (comb.LoadCases != null)
            {
                for (int i = 0; i < comb.LoadCases.Count; i++)
                {
                    eCNameType type = eCNameType.LoadCase;
                    string caseName = comb.LoadCases[i].Name;
                    if (comb.LoadCases[i].IsDefinedInSAP == false)
                    {
                        //AddLoadCase(); //TODO
                    }
                    float scaleFactor = comb.LoadCasesFactors[i];
                    mySapModel.RespCombo.SetCaseList(comb.Name, ref type, caseName, scaleFactor);//Adding defined load cases to the combination
                }
            }


            if (comb.LoadCombos != null)
            {
                for (int i = 0; i < comb.LoadCombos.Count; i++)
                {
                    eCNameType type = eCNameType.LoadCombo;
                    string comboName = comb.LoadCombos[i].Name;
                    if (comb.LoadCombos[i].IsDefinedInSAP == false)
                    {
                        AddLoadCombination(comb.LoadCombos[i]);
                    }
                    float scaleFactor = comb.LoadCombosFactors[i];
                    mySapModel.RespCombo.SetCaseList(comb.Name, ref type, comboName, scaleFactor);//Adding defined load combos to the combination
                }
            }
            if (flag != 0)
            {
                comb.IsDefinedInSAP = false;

                return false;
            }
            comb.IsDefinedInSAP = true;
            return true;
        }
        public static SAPLoadCombination[] AddDesignDefaultCombos(eMatType matType)
        {
            SAPLoadCombination[] combos;
            int flag = 1;

            switch (matType)
            {
                case eMatType.Steel:
                    flag = mySapModel.RespCombo.AddDesignDefaultCombos(true, false, false, false);
                    break;
                case eMatType.Concrete:
                    flag = mySapModel.RespCombo.AddDesignDefaultCombos(false, true, false, false);
                    break;
                case eMatType.NoDesign:
                    return null;
                case eMatType.Aluminum:
                    flag = mySapModel.RespCombo.AddDesignDefaultCombos(false, false, true, false);
                    break;
                case eMatType.ColdFormed:
                    flag = mySapModel.RespCombo.AddDesignDefaultCombos(true, false, false, true);
                    break;
                case eMatType.Rebar:
                    return null;
                case eMatType.Tendon:
                    return null;
                case eMatType.Masonry:
                    return null;
            }
            if (flag != 0)
            {
                return null;
            }
            combos = SAP2000API.GetLoadCombinationsList();
            return combos;
        }

        private static SAPLoadCombination[] GetLoadCombinationsList()
        {
            SAPLoadCombination[] combos;
            int noCombos = 0;
            string[] names = new string[1];
            int flag = mySapModel.RespCombo.GetNameList(ref noCombos, ref names);
            if (flag != 0)
            {
                return null;
            }
            else
            {
                combos = new SAPLoadCombination[noCombos];

                for (int i = 0; i < noCombos; i++)
                {
                    combos[i] = new SAPLoadCombination();
                    combos[i].Name = names[i];
                    bool comboFlag = GetLoadCombinationCases(combos[i].Name, out combos[i].loadCases, out combos[i].loadCombos, out combos[i].loadCasesFactors, out combos[i].loadCombosFactors);
                    combos[i].IsDefinedInSAP = true;
                    if (comboFlag == false)
                    {
                        throw new Exception("Couldn't retrieve combo cases");
                    }
                }
            }
            return combos;
        }

        private static bool GetLoadCombinationCases(string comboName, out List<SAPLoadCase> loadCases, out List<SAPLoadCombination> loadCombos, out List<float> loadCasesFactors, out List<float> loadCombosFactors)
        {
            loadCases = new List<SAPLoadCase>();
            loadCombos = new List<SAPLoadCombination>();

            loadCasesFactors = new List<float>();
            loadCombosFactors = new List<float>();

            int noComponents = 0;
            string[] names = new string[1];
            eCNameType[] types = new eCNameType[1];
            double[] factors = new double[1];
            int flag = mySapModel.RespCombo.GetCaseList(comboName, ref noComponents, ref types, ref names, ref factors);
            if (flag != 0)
            {
                loadCases = null;
                loadCombos = null;
                return false;
            }
            else
            {
                for (int i = 0; i < noComponents; i++)
                {
                    if (i >= types.Count())
                    {
                        continue;
                    }
                    switch (types[i])
                    {
                        case eCNameType.LoadCase:
                            SAPLoadCase Case = new SAPLoadCase();
                            Case.Name = names[i];
                            Case.IsDefinedInSAP = true;
                            loadCases.Add(Case);
                            loadCasesFactors.Add((float)factors[i]);
                            break;
                        case eCNameType.LoadCombo:
                            SAPLoadCombination combo = new SAPLoadCombination();
                            combo.Name = names[i];
                            combo.IsDefinedInSAP = true;
                            bool comboFlag = GetLoadCombinationCases(combo.Name, out combo.loadCases, out combo.loadCombos, out combo.loadCasesFactors, out combo.loadCasesFactors);
                            if (comboFlag == false)
                            {
                                throw new Exception("Couldn't retrieve combo cases");
                            }
                            else
                            {
                                loadCombos.Add(combo);
                                loadCombosFactors.Add((float)factors[i]);
                            }
                            break;
                        default:
                            break;
                    }
                }
                return true;
            }
        }
        public static bool AddLoadCase()
        {
            //TODO: To be implemented but no need at the moment since each load pattern is a load case
            int flag = 1;
            if (flag != 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        #endregion
        #region Drawing Methods
        #region Points & Restraints
        public static bool SetPoint(SAPPoint point)
        {
            int flag = mySapModel.PointObj.AddCartesian(point.X, point.Y, point.Z, ref point.name);
            if (flag != 0)
            {
                point.IsDefinedInSAP = false;
                return false;
            }
            point.IsDefinedInSAP = true;

            if (point.Restraint != null)
            {
                if (point.Restraint.IsDefinedInSAP == false)
                {
                    SetRestraint(point, point.Restraint);
                }
            }
            return true;
        }
        public static bool SetPoint(IEnumerable<SAPPoint> points)
        {
            int flag;
            foreach (SAPPoint point in points)
            {
                flag = mySapModel.PointObj.AddCartesian(point.X, point.Y, point.Z, ref point.name);
                if (flag != 0)
                {
                    point.IsDefinedInSAP = false;
                    return false;
                }
                point.IsDefinedInSAP = true;

                if (point.Restraint != null)
                {
                    if (point.Restraint.IsDefinedInSAP == false)
                    {
                        SetRestraint(point, point.Restraint);
                    }
                }

            }
            return true;
        }
        public static bool SetRestraint(SAPPoint point, SAPRestraint restraint)
        {
            int flag = mySapModel.PointObj.SetRestraint(point.name, ref restraint.values);
            if (flag != 0)
            {
                restraint.IsDefinedInSAP = false;
                return false;
            }
            restraint.IsDefinedInSAP = true;
            point.Restraint = restraint;
            return true;
        }
        public static bool SetRestraint(IEnumerable<SAPPoint> points, IEnumerable<SAPRestraint> restraints)
        {
            int flag;
            SAPPoint[] pointsArr = points.ToArray();
            SAPRestraint[] restraintsArr = restraints.ToArray();

            for (int i = 0; i < restraintsArr.Length; i++)
            {
                flag = mySapModel.PointObj.SetRestraint(pointsArr[i].name, ref restraintsArr[i].values);
                if (flag != 0)
                {
                    restraintsArr[i].IsDefinedInSAP = false;
                    return false;
                }
                restraintsArr[i].IsDefinedInSAP = true;
                pointsArr[i].Restraint = restraintsArr[i];
            }
            return true;
        }
        #endregion
        #region Frame Elements
        public static bool AddFrameElement(SAPFrameElement element)
        {
            int flag;
            if (element.StartPoint.IsDefinedInSAP == false)
            {
                SetPoint(element.StartPoint);
            }
            if (element.EndPoint.IsDefinedInSAP == false)
            {
                SetPoint(element.EndPoint);
            }
            if (element.Section != null)
            {
                if (element.Section.IsDefinedInSAP == false)
                {
                    AddSection(element.Section);
                }
                flag = mySapModel.FrameObj.AddByPoint(element.StartPoint.Name, element.EndPoint.Name, ref element.name, element.Section.Name, element.Label);
            }
            else
            {
                flag = mySapModel.FrameObj.AddByPoint(element.StartPoint.Name, element.EndPoint.Name, ref element.name);

            }
            if (flag != 0)
            {
                element.IsDefinedInSAP = false;
                return false;

            }
            element.IsDefinedInSAP = true;
            mySapModel.View.RefreshView(0, false);
            mySapModel.View.RefreshView(1, false);


            return true;
        }

        public static bool AddFrameElement(IEnumerable<SAPFrameElement> elements)
        {
            int flag;
            foreach (SAPFrameElement element in elements)
            {
                if (element.StartPoint.IsDefinedInSAP == false)
                {
                    SetPoint(element.StartPoint);
                }
                if (element.EndPoint.IsDefinedInSAP == false)
                {
                    SetPoint(element.EndPoint);
                }
                if (element.Section != null)
                {
                    if (element.Section.IsDefinedInSAP == false)
                    {
                        AddSection(element.Section);
                    }
                    flag = mySapModel.FrameObj.AddByPoint(element.StartPoint.Name, element.EndPoint.Name, ref element.name, element.Section.Name, element.Label);
                }
                else
                {
                    flag = mySapModel.FrameObj.AddByPoint(element.StartPoint.Name, element.EndPoint.Name, ref element.name);
                    mySapModel.FrameObj.SetDesignProcedure(element.name, 1); //Set this element to be designed
                }
                if (flag != 0)
                {
                    element.IsDefinedInSAP = false;
                    return false;
                }
                mySapModel.View.RefreshWindow();

                element.IsDefinedInSAP = true;
            }
            return true;
        }
        #endregion
        #endregion
        #region Loads
        public static bool AddDistributedLoad(SAPFrameElement element, SAPDistributedLoad load)
        {
            string coordinates = "Global";
            if (load.LoadDirection == HANDAZ.Entities.HndzLoadDirectionEnum.Local1axis || load.LoadDirection == HANDAZ.Entities.HndzLoadDirectionEnum.Local2axis || load.LoadDirection == HANDAZ.Entities.HndzLoadDirectionEnum.Local3axis)
            {
                coordinates = "Local";
            }
            int flag = mySapModel.FrameObj.SetLoadDistributed(element.Name, load.LoadType.Name, 1, (int)load.LoadDirection, load.Dist1, load.Dist2, load.Val1, load.Val2, coordinates, load.IsRelativeDist, load.IsReplacement);
            if (flag != 0)
            {
                load.IsDefinedInSAP = false;
                return false;
            }
            load.IsDefinedInSAP = true;
            return true;

        }
        public static bool AddDistributedLoad(IEnumerable<SAPFrameElement> elements, SAPDistributedLoad load)
        {
            //TODO Check this function and perform Quality standards
            foreach (SAPFrameElement element in elements)
            {
                string coordinates = "Global";
                if (load.LoadDirection == HndzLoadDirectionEnum.Local1axis || load.LoadDirection == HndzLoadDirectionEnum.Local2axis || load.LoadDirection == HndzLoadDirectionEnum.Local3axis)
                {
                    coordinates = "Local";
                }
                int flag = mySapModel.FrameObj.SetLoadDistributed(element.Name, load.LoadType.Name, 1, (int)load.LoadDirection, load.Dist1, load.Dist2, load.Val1, load.Val2, coordinates, load.IsRelativeDist, load.IsReplacement);
                if (flag != 0)
                {
                    load.IsDefinedInSAP = false;
                    return false;
                }
                load.IsDefinedInSAP = true;
            }
            return true;

        }
        public static bool AddPointLoad(SAPFrameElement element, SAPPointLoad load)
        {
            int flag = mySapModel.FrameObj.SetLoadPoint(element.Name, load.LoadType.Name, 1, (int)load.LoadDirection, load.Distance, load.Value);
            if (flag != 0)
            {
                load.IsDefinedInSAP = false;
                return false;
            }
            load.IsDefinedInSAP = true;
            return true;
        }
        public static bool AddPointLoad(SAPPoint point, SAPPointLoad load)
        {
            double[] forces = { load.Value };
            int flag = mySapModel.PointObj.SetLoadForce(point.Name, load.LoadType.Name, ref forces);
            if (flag != 0)
            {
                load.IsDefinedInSAP = false;
                return false;
            }
            load.IsDefinedInSAP = true;
            return true;
        }
        #endregion
        #region Analysis Methods
        #region Analysis
        //Implemented in the interface part
        #endregion
        #region Analysis Results
        public static SAPAnalysisResults GetFrameElementAnalysisResults(IEnumerable<SAPLoadCombination> combos, SAPFrameElement elem, bool isEnvelope = false)
        {
            int flag = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
            foreach (SAPLoadCombination combo in combos)
            {
                if (combo.IsDefinedInSAP == false)
                {
                    AddLoadCombination(combo);
                }
                flag = mySapModel.Results.Setup.SetComboSelectedForOutput(combo.Name, true);
                if (flag != 0)
                {
                    return null;
                }
            }
            SAPAnalysisResults res;
            if (isEnvelope == false)
            {
                res = elem.AnalysisResults = new SAPAnalysisResults();
            }
            else
            {
                res = elem.AnalysisResultsEnvelope = new SAPAnalysisResults();
            }
            flag = mySapModel.Results.FrameForce(elem.Name, eItemTypeElm.ObjectElm, ref res.numberResults, ref res.frameText, ref res.station, ref res.elementText, ref res.elementStation,
                ref res.loadCase, ref res.stepType, ref res.stepNum, ref res.axial, ref res.shear2, ref res.shear3, ref res.tortionalMoment, ref res.moment2, ref res.moment3);
            if (flag != 0)
            {
                return null;
            }
            return res;
        }
        public static List<SAPFrameElement> GetFrameElementAnalysisResults(IEnumerable<SAPLoadCombination> combos, ref List<SAPFrameElement> elems, bool isEnvelope = false)
        {
            //TODO Check quality
            if (isEnvelope == false)
            {
                foreach (SAPFrameElement elem in elems)
                {
                    elem.AnalysisResults = GetFrameElementAnalysisResults(combos, elem);
                }
            }
            else
            {
                foreach (SAPFrameElement elem in elems)
                {
                    elem.AnalysisResultsEnvelope = GetFrameElementAnalysisResults(combos, elem, true);
                }
            }
            return elems;
        }
        public static SAPJointElementResults GetJointAnalysisResults(IEnumerable<SAPLoadCombination> combos, SAPPoint elem)
        {
            int flag = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
            foreach (SAPLoadCombination combo in combos)
            {
                flag = mySapModel.Results.Setup.SetComboSelectedForOutput(combo.Name, true);
                if (flag != 0)
                {
                    return null;
                }
            }
            SAPJointElementResults res = elem.AnalysisResults = new SAPJointElementResults();
            flag = mySapModel.Results.JointReact(elem.Name, eItemTypeElm.ObjectElm, ref res.numberResults, ref res.obj, ref res.elment, ref res.loadCase, ref res.stepType,
               ref res.stepNum, ref res.f1, ref res.f2, ref res.f3, ref res.m1, ref res.m2, ref res.m3);
            if (flag != 0)
            {
                return null;
            }
            return res;
        }
        public static void GetJointAnalysisResults(IEnumerable<SAPLoadCombination> combos, ref List<SAPFrameElement> frameElements, bool isEnvelope = false)
        {
            if (isEnvelope == false)
            {
                for (int i = 0; i < frameElements.Count; i++)
                {
                    frameElements[i].StartPoint.AnalysisResults = SAP2000API.GetJointAnalysisResults(combos, frameElements[i].StartPoint);
                    frameElements[i].StartPoint.AnalysisResults = SAP2000API.GetJointAnalysisResults(combos, frameElements[i].StartPoint);

                }
            }
            else
            {
                for (int i = 0; i < frameElements.Count; i++)
                {
                    frameElements[i].StartPoint.AnalysisResultsEnvelope = SAP2000API.GetJointAnalysisResults(combos, frameElements[i].StartPoint);
                    frameElements[i].StartPoint.AnalysisResultsEnvelope = SAP2000API.GetJointAnalysisResults(combos, frameElements[i].StartPoint);
                }
            }
        }
        #endregion
        #endregion
        #region Design Methods
        #region Design
        public static bool DesignSteelModel(string codeName, SAPLoadCombination deflectionCombo, SAPLoadCombination strengthCombo, bool ToChangeSections, IEnumerable<SAPFrameElement> elements, ref SAPDesignStatistics statistics)
        {

            //Setting Design combinations
            int flag = mySapModel.DesignSteel.SetComboDeflection(deflectionCombo.Name, true);
            flag = mySapModel.DesignSteel.SetComboStrength(deflectionCombo.Name, true);
            if (flag != 0)
            {
                return false;
            }
            else
            {
                return DesignSteelModel(codeName, ref statistics, ToChangeSections, elements);
            }
        }
        public static bool DesignSteelModel(string codeName, ref SAPDesignStatistics statistics, bool ToChangeSections, params IEnumerable<SAPFrameElement>[] elements)
        {
            // List<SAPITaperedSection> sectionsInventory = new List<SAPITaperedSection>();

            int flag = mySapModel.DesignSteel.SetCode(codeName); //Setting the design Code
            AddDesignDefaultCombos(eMatType.Steel);
            flag = mySapModel.DesignSteel.StartDesign();//Starting the design process
            if (flag != 0)
            {
                //Design Failed
                return false;
            }
            else
            {
                //A Question to be answered, should we use here verify passed or verify sections
                flag = mySapModel.DesignSteel.VerifyPassed(ref statistics.noVerified, ref statistics.noFailed, ref statistics.noUnchecked, ref statistics.failedElementsNames);
            }
            if (flag != 0)
            {
                //Retrieving Sections Failed
                return false;
            }
            else
            {
                //Changing Flags
                foreach (IEnumerable<SAPFrameElement> param in elements)
                {
                    foreach (string name in statistics.failedElementsNames)
                    {
                        //TODO: Optimizing search algorithm here is important
                        SAPFrameElement element = param.FirstOrDefault(c => c.Name == name);
                        if (element != null)
                        {
                            element.IsDesignPassed = false;
                            if (ToChangeSections == true)
                            {
                                //sectionsInventory.Add((SAPITaperedSection)element.Section);
                                //int k = 0;
                                //while (sectionsInventory[k]!=null)
                                //{
                                //    sectionsInventory.Add(sectionsInventory[k]);
                                //    k++;
                                //    mySapModel.SetModelIsLocked(false);
                                //    AddSection(sectionsInventory[k]);
                                //}
                                //AnalayzeModel();
                                int iterationsNo = 0;
                                bool flagD = true;
                                while (element.IsDesignPassed == false && iterationsNo < 30)
                                {
                                    SAPSection sec = element.Section.GetAssumedSection();
                                    if (sec == null || element.Section == sec || flagD == false)
                                    {
                                        flagD = true;
                                        break;
                                    }
                                    SAPSection sapSec = GetSection(sec);
                                    if (sapSec != null)
                                    {
                                        sec = sapSec;
                                    }
                                    flagD = SetDesignSection(ref element, sec);
                                    iterationsNo++;
                                }
                            }
                        }
                    }
                    foreach (SAPFrameElement element in param)
                    {
                        if (element.IsDesignPassed == null)
                        {
                            element.IsDesignPassed = true;
                        }
                    }
                }

                //    flag = mySapModel.DesignSteel.VerifySections(ref statistics.noDesigned, ref statistics.changedElementsNames);
                //}
                //if (flag != 0)
                //{
                //    //Retrieving changed sections failed
                //    return false;
                //}
                //else if (statistics.ChangedElementsNames[0] != null)
                //{
                //    foreach (IEnumerable<SAPFrameElement> param in elements)
                //    {
                //        foreach (string name in statistics.ChangedElementsNames)
                //        {
                //            SAPSection section = param.FirstOrDefault(c => c.Name == name).Section;
                //            if (section != null)
                //            {
                //                section = GetSection(section);
                //            }
                //        }
                //    }
                //}
                return true;
            }
        }
        private static bool VerifyDesignSection(ref SAPFrameElement elem, SAPSection sec)
        {
            string name = elem.Name;
            mySapModel.SelectObj.PropertyFrame(elem.Name);//Select Frame Element

            SAPDesignStatistics statistics = new SAPDesignStatistics();
            mySapModel.DesignSteel.StartDesign();
            int flag = mySapModel.DesignSteel.VerifyPassed(ref statistics.noVerified, ref statistics.noFailed, ref statistics.noUnchecked, ref statistics.failedElementsNames);
            if (flag != 0)
            {
                //Retrieving Sections Failed
                return false;
            }
            else
            {
                if (statistics.failedElementsNames.FirstOrDefault(c => c == name) == null)
                {
                    elem.IsDesignPassed = true;
                }
            }
            mySapModel.SelectObj.PropertyFrame(elem.Name, true);//DeSelect Frame Element
            return true;
        }
        public static bool SetDesignSection(ref SAPFrameElement elem, SAPSection sec)
        {
            VerifyDesignSection(ref elem, elem.Section);
            if (sec.IsDefinedInSAP == false)
            {
                mySapModel.SetModelIsLocked(false);
                // DeleteSection(elem.Section);
                AddSection(sec);
                AnalayzeModel();
            }
            int flag = mySapModel.DesignSteel.SetDesignSection(elem.Name, sec.Name, false);
            if (flag != 0)
            {
                return false;
            }
            else
            {
                elem.Section = sec;
                VerifyDesignSection(ref elem, sec);
                return true;
            }
        }
        #endregion
        #region Design Results

        #endregion
        #endregion

        #region Additional Utilities Methods
        /// <summary>
        /// Changes the units of the current SAP2000 Interface
        /// </summary>
        /// <param name="newunits"></param>
        public static void ChangeUnits(eUnits newunits)
        {
            mySapModel.SetPresentUnits(newunits);
        }
        #endregion
        #region Getting Things from SAP2000
        public static SAPFrameElement[] GetAllFrameElements()
        {
            int noElements = 0;
            string[] elementsNames = new string[1]; //Elements Names

            int flag = mySapModel.FrameObj.GetNameList(ref noElements, ref elementsNames);
            if (flag != 0)
            {
                //Failed to retrieve Elements
                return null;
            }
            SAPFrameElement[] elements = new SAPFrameElement[noElements];
            for (int i = 0; i < noElements; i++)
            {
                elements[i] = new SAPFrameElement();
                elements[i].Name = elements[i].Label = elementsNames[i];
                elements[i].Section = GetSectionByElement(elements[i].Name);
                GetPointsByElement(ref elements[i]);
            }
            return elements;
        }
        public static SAPLoadCombination[] GetAllLoadCombinations()
        {
            int noResults = 0;
            string[] names = new string[1];
            SAPLoadCombination[] combos = null;
            int flag = mySapModel.RespCombo.GetNameList(ref noResults, ref names);
            if (flag != 0)
            {
                //Failed to retrieve the combinations
                return null;
            }
            else
            {
                combos = new SAPLoadCombination[noResults];
                for (int i = 0; i < noResults; i++)
                {
                    combos[i] = new SAPLoadCombination();
                    combos[i].Name = names[i];
                    combos[i].IsDefinedInSAP = true;
                    combos[i].LoadCases = null; //TODO Get load cases
                    combos[i].LoadCombos = null;//TODO Get load cases
                }
            }
            return combos;
        }
        private static bool GetPointsByElement(ref SAPFrameElement element)
        {
            element.StartPoint = new SAPPoint();
            element.EndPoint = new SAPPoint();

            int flag = mySapModel.FrameObj.GetPoints(element.name, ref element.StartPoint.name, ref element.EndPoint.name);
            if (flag != 0)
            {
                return false;
            }
            else
            {
                element.StartPoint = GetPointCoordinates(element.StartPoint.name);
                element.EndPoint = GetPointCoordinates(element.EndPoint.name);
                return true;
            }
        }

        private static SAPPoint GetPointCoordinates(string pointName)
        {
            SAPPoint point = new SAPPoint();
            point.name = pointName;
            double x = 0;
            double y = 0;
            double z = 0;

            int flag = mySapModel.PointObj.GetCoordCartesian(pointName, ref x, ref y, ref z);
            if (flag != 0)
            {
                return null;
            }
            else
            {
                point.X = x;
                point.Y = y;
                point.Z = z;
                return point;
            }
        }

        private static SAPSection GetSectionByElement(string elementName)
        {
            //TODO : THIS FUNCTION IS TERRIBLE, REWRITE IT AGIAN
            SAPSection section = null;
            int flag;
            string propName = string.Empty;
            string SAlist = string.Empty;
            flag = mySapModel.FrameObj.GetSection(elementName, ref propName, ref SAlist);
            if (flag != 0)
            {
                return null;
            }
            else
            {
                //Try to get this section as if it was a tapered section
                SAPITaperedSection tSec = new SAPITaperedSection();
                tSec.Name = propName;
                tSec = GetITaperedSection(tSec);
                section = tSec;
                if (tSec == null)
                {
                    //It's not tapered then it's I built up section
                    SAPISection sec = new SAPISection();
                    sec.Name = propName;
                    sec = GetISection(sec);
                    section = sec;
                }
                if (section == null)
                {
                    section = new SAPITaperedSection();
                    section.Name = propName;
                    section.IsDefinedInSAP = true;
                }
            }
            return section;
        }
        #endregion
    }
}
