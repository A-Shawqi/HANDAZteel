using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.PEB.AnalysisTools.CsiSAP2000;
using HANDAZ.Entities;
using SAP2000v18;

namespace HANDAZ.PEB.BusinessComponents.AnalysisToolsConverters
{
    public class HndzSAP2000API
    {
        #region General Methods
        public static bool StartApplication(bool attachToInstance, bool isVisible, HndzUnitsEnum units = HndzUnitsEnum.Ton_m_C)
        {
            return SAP2000API.StartApplication(attachToInstance,isVisible,(eUnits)units);
        }
        public static bool CloseApplication()
        {
            return SAP2000API.CloseApplication();
        }
        public static bool NewModel(HndzUnitsEnum units = HndzUnitsEnum.Ton_m_C)
        {
            return SAP2000API.NewModel((eUnits)units);
        }
        public static bool OpenModel(string modelName)
        {
            return SAP2000API.OpenModel(modelName);
        }
        public static bool SaveModel(string modelName)
        {
            return SAP2000API.SaveModel(modelName);
        }
        public static bool AnalayzeModel()
        {
           return SAP2000API.AnalayzeModel();
        }
        public static bool AnalayzeModel(HndzRestraint analyzedDOFs)
        {
            SAPRestraint DOFs = new SAPRestraint();
            DOFs.ConvertFromHndzRestraint(analyzedDOFs);
            return SAP2000API.AnalayzeModel(DOFs);
        }

        public static bool AnalayzeModel(HndzAnalysisDOFs analyzedDOFs)
        {
            return SAP2000API.AnalayzeModel((SAPAnalysisDOFs)analyzedDOFs);
        }

#endregion
        #region Definition Methods
        #region Materials
        public static HndzMaterial[] SetDefaultMaterials()
        {
            SAPMaterial[] sapMaterials= SAP2000API.SetDefaultMaterials();
            HndzMaterial[] hndzMaterials = new HndzMaterial[sapMaterials.Length];
            for (int i = 0; i < sapMaterials.Length; i++)
            {
                sapMaterials[i].ConvertFromHndzMaterial((HndzStructuralMaterial)hndzMaterials[i]);
            }

            return hndzMaterials;
        }
        public static bool DefineCustomMaterial(HndzStructuralMaterial material)
        {
            SAPMaterial sapMaterial = default(SAPMaterial);
            sapMaterial.ConvertFromHndzMaterial(material);
            return SAP2000API.DefineCustomMaterial(sapMaterial);
        }

        #endregion
        #region Sections
        /// <summary>
        /// The general function that takes any type of sections and define it in SAP2000
        /// </summary>
        /// <param name="section"></param>
        public static void AddSection(HndzProfile section)
        {
            //SAPSection sapSection = new SAPSection();
            //return SAP2000API.AddSection(section);
            //TODO: Check how are u going to map an abstract class to another abstract class, search on the Internet on how to do that
            throw new NotImplementedException("TODO: Mapping SAPSection to HndzSection");

        }
        public static bool SetISection(HndzISectionProfile Isection,HndzStructuralMaterial material)
        {
            SAPISection sapISection = default(SAPISection);
                sapISection.ConvertFromHndzIProfile(Isection,material);
            return SAP2000API.SetISection(sapISection);
        }
        /// <summary>
        /// This function assumes that an IBuiltUpSection consists of a start and end section, further development to send an array of sections is recommended
        /// </summary>
        /// <param name="IBuiltUpSection"></param>
        /// <returns></returns>
        public static bool SetITaperedSection(HndzITaperedProfile profile, HndzStructuralMaterial material)
        {
            SAPITaperedSection section = new SAPITaperedSection();
            section.ConvertFromHndzTaperedI(profile, material);
            return SAP2000API.SetITaperedSection(section);
        }

        public static SAPSection GetSection(HndzProfile section)
        {
            //SAPSection sapSection = new SAPSection();
            //return SAP2000API.AddSection(section);
            //TODO: Check how are u going to map an abstract class to another abstract class, search on the Internet on how to do that
            throw new NotImplementedException("TODO: Mapping SAPSection to HndzSection");
        }

        public static HndzITaperedProfile GetITaperedSection(HndzITaperedProfile profile, ref HndzStructuralMaterial material)
        {
            SAPITaperedSection section = new SAPITaperedSection();
            section.ConvertFromHndzTaperedI(profile, material);
            section = SAP2000API.GetITaperedSection(section);
            HndzITaperedProfile hndzProfile = new HndzITaperedProfile();
            hndzProfile = section.ConvertToHndzTaperedI(ref material);
            return hndzProfile;
        }

        public static HndzISectionProfile GetISection(HndzISectionProfile profile, ref HndzStructuralMaterial material)
        {
            SAPISection section = new SAPISection();
            section.ConvertFromHndzIProfile(profile, material);
            section = SAP2000API.GetISection(section);
            HndzISectionProfile hndzProfile = new HndzISectionProfile();
            hndzProfile = section.ConvertToHndzIProfile(ref material);
            return hndzProfile;
        }
        #endregion
        #region Loads
        public static bool AddLoadPattern(SAPLoadPattern pattern)
        {
            throw new NotImplementedException("TODO: Loads are not mapped yet from Hndz to SAP");

        }
        public static bool AddLoadCombination(SAPLoadCombination comb)
        {
            throw new NotImplementedException("TODO: Loads are not mapped yet from Hndz to SAP");

        }
        public static bool AddDesignDefaultCombos(eMatType matType)
        {
            throw new NotImplementedException("TODO: Loads are not mapped yet from Hndz to SAP");

        }
        public static bool AddLoadCase()
        {
            throw new NotImplementedException("TODO: Loads are not mapped yet from Hndz to SAP");

        }
        #endregion
        #endregion
        #region Drawing Methods
        #region Points & Restraints
        public static bool SetPoint(HndzNode node)
        {
            SAPPoint point = new SAPPoint();
            point.ConvertFromHndzNode(node);
            return SAP2000API.SetPoint(point);
        }
        public static bool SetPoint(IEnumerable<HndzNode> nodes)
        {
            foreach (HndzNode node in nodes)
            {
                SAPPoint point = new SAPPoint();
                point.ConvertFromHndzNode(node);
                bool flag = SAP2000API.SetPoint(point);
                if (flag == false)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool SetRestraint(HndzNode node, HndzRestraint restraint)
        {
            SAPPoint point = new SAPPoint();
            point.ConvertFromHndzNode(node);
            SAPRestraint sapRestraint = new SAPRestraint();
            sapRestraint.ConvertFromHndzRestraint(restraint);
            return SAP2000API.SetRestraint(point, sapRestraint);
        }
        public static bool SetRestraint(IEnumerable<HndzNode> nodes, IEnumerable<HndzRestraint> restraints)
        {
            HndzNode[] nodesArr = nodes.ToArray();
            HndzRestraint[] restraintsArr = restraints.ToArray();
            for (int i = 0; i < nodesArr.Length; i++)
            {
                SAPPoint point = new SAPPoint();
                point.ConvertFromHndzNode(nodesArr[i]);
                SAPRestraint sapRestraint = new SAPRestraint();
                sapRestraint.ConvertFromHndzRestraint(restraintsArr[i]);
                bool flag = SAP2000API.SetRestraint(point, sapRestraint);
                if (flag == false)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Frame Elements
        public static bool AddFrameElement(HndzExtrudedElement element, HndzSectionTypeEnum type)
        {
            SAPFrameElement sapElement = new SAPFrameElement();
            sapElement.ConvertFromHndzElement(element,type);
            return SAP2000API.AddFrameElement(sapElement);
        }
        /// <summary>
        /// Send a list/array of elements with the same section types, 
        /// if you are sending different types then you have to call this function more than one time
        /// </summary>
        /// <param name="elements">Elements to be drawn and defined in SAP2000 Model</param>
        /// <param name="type">The type of all these elements</param>
        /// <returns></returns>
        public static bool AddFrameElement(IEnumerable<HndzExtrudedElement> elements, HndzSectionTypeEnum type)
        {
            HndzExtrudedElement[] elementsArr = elements.ToArray();
            SAPFrameElement[] sapElementsArr = new SAPFrameElement[elementsArr.Length];
            for (int i = 0; i < elementsArr.Length; i++)
            {
                sapElementsArr[i].ConvertFromHndzElement(elementsArr[i], type);
            }
            return SAP2000API.AddFrameElement(sapElementsArr);
        }
        #endregion
        #endregion
        #region Loads
        public static bool AddDistributedLoad(SAPFrameElement element, SAPDistributedLoad load)
        {
            throw new NotImplementedException("TODO: Loads are not mapped yet from Hndz to SAP");

        }
        public static bool AddPointLoad(SAPFrameElement element, SAPPointLoad load)
        {
            throw new NotImplementedException("TODO: Loads are not mapped yet from Hndz to SAP");

        }
        public static bool AddPointLoad(SAPPoint point, SAPPointLoad load)
        {
            throw new NotImplementedException("TODO: Loads are not mapped yet from Hndz to SAP");

        }
        #endregion
        #region Analysis Methods
        #region Analysis
        //Implemented in the interface part
        #endregion
        #region Analysis Results
        public static SAPAnalysisResults GetFrameElementAnalysisResults(IEnumerable<SAPLoadCombination> combos, SAPFrameElement elem)
        {
            throw new NotImplementedException("TODO: Analysis Results are not mapped yet from Hndz to SAP");
        }
        public static SAPJointElementResults GetJointAnalysisResults(IEnumerable<SAPLoadCombination> combos, SAPPoint elem)
        {
            throw new NotImplementedException("TODO: Analysis Results are not mapped yet from Hndz to SAP");
        }
        #endregion
        //TODO
        #endregion
        #region Design Methods
        #region Design
        public static IEnumerable<SAPSection> DesignSteelModel(string codeName, SAPLoadCombination deflectionCombo, SAPLoadCombination strengthCombo, IEnumerable<SAPSection> sections, ref SAPDesignStatistics statistics)
        {
            throw new NotImplementedException("TODO: Design are not mapped yet from Hndz to SAP");

        }
        public static IEnumerable<SAPSection> DesignSteelModel(string codeName, IEnumerable<SAPSection> sections, ref SAPDesignStatistics statistics)
        {
            throw new NotImplementedException("TODO: Design are not mapped yet from Hndz to SAP");


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
        public static void ChangeUnits(HndzUnitsEnum newunits)
        {
            SAP2000API.ChangeUnits((eUnits)newunits);
        }
        #endregion
    }
}
