using HANDAZ.PEB.Entities;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools
{
   public class RobotDefinitions : RobotInit
    {
        public static bool SetMaterial(Material Mymaterial)
        {
            robotApp.Interactive = 0;
            IRobotLabel material = labelServer.Create(IRobotLabelType.I_LT_MATERIAL, Mymaterial.MaterialName);
            RobotMaterialData materialData = material.Data;
            materialData.Type = IRobotMaterialType.I_MT_STEEL;
            materialData.E = Mymaterial.YoungsModulas;
            materialData.NU = Mymaterial.PoissionRatio;
            materialData.Kirchoff = Mymaterial.ShearModulas;
            materialData.RO = Mymaterial.UnitWeight;
            materialData.LX = Mymaterial.ThermalExpansion;
            materialData.DumpCoef = Mymaterial.DampingRatio;
            materialData.CS = Mymaterial.ShearReductionFactor;
            materialData.Default = 1;
            materialData.RT = Mymaterial.TensionLimitstress;
            materialData.RE = Mymaterial.CharchteristicResistance;
            materialData.SaveToDBase();
            robotApp.Project.Structure.Labels.Store(material);
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetColumnSections(Frame frame)
        {
            //needs editing 
            robotApp.Interactive = 0;
            IRobotLabel builtUpSectionLabel = labelServer.Create(IRobotLabelType.I_LT_BAR_SECTION, "Hndz - Tapered");
            RobotBarSectionData builtUpsectionData = builtUpSectionLabel.Data;
            builtUpsectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            builtUpsectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            RobotBarSectionNonstdData builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(0);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, frame.Columns[0].TaperedATEndNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Columns[0].TaperedATEndNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Columns[0].TaperedATEndNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Columns[0].TaperedATEndNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Columns[0].TaperedATEndNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Columns[0].TaperedATEndNode.Tw);
            builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, frame.Columns[0].TaperedAtStartNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Columns[0].TaperedAtStartNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Columns[0].TaperedAtStartNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Columns[0].TaperedAtStartNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Columns[0].TaperedAtStartNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Columns[0].TaperedAtStartNode.Tw);
            builtUpsectionData.CalcNonstdGeometry();
            labelServer.Store(builtUpSectionLabel);


            RobotSelection Selection = robotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            Selection.AddOne(frame.Columns[0].Id);
            Selection.AddOne(frame.Columns[1].Id);

            barServer.SetLabel(Selection, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);

            //barServer.SetLabel(RightColumn, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);


            robotApp.Interactive = 1;
            return true;

        }
        public static bool SetBeamSections(Frame frame)
        {
            robotApp.Interactive = 0;
            IRobotLabel builtUpSectionLabel = labelServer.Create(IRobotLabelType.I_LT_BAR_SECTION, "Hndz - Tapered - Beam");
            RobotBarSectionData builtUpsectionData = builtUpSectionLabel.Data;
            builtUpsectionData.Type = IRobotBarSectionType.I_BST_NS_II;
            builtUpsectionData.ShapeType = IRobotBarSectionShapeType.I_BSST_USER_I_MONOSYM;
            RobotBarSectionNonstdData builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, frame.Beams[0].BeamSectionAtStartNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Beams[0].BeamSectionAtStartNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Beams[0].BeamSectionAtStartNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Beams[0].BeamSectionAtStartNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Beams[0].BeamSectionAtStartNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Beams[0].BeamSectionAtStartNode.Tw);
            builtUpsectionDataNonS = builtUpsectionData.CreateNonstd(0);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B1, frame.Beams[0].BeamSectionATEndNode.B1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_B2, frame.Beams[0].BeamSectionATEndNode.B2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_H, frame.Beams[0].BeamSectionATEndNode.Height);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF1, frame.Beams[0].BeamSectionATEndNode.TF1);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TF2, frame.Beams[0].BeamSectionATEndNode.TF2);
            builtUpsectionDataNonS.SetValue(IRobotBarSectionNonstdDataValue.I_BSNDV_II_TW, frame.Beams[0].BeamSectionATEndNode.Tw);

            builtUpsectionData.CalcNonstdGeometry();
            labelServer.Store(builtUpSectionLabel);


            RobotSelection Selection = robotApp.Project.Structure.Selections.Create(IRobotObjectType.I_OT_BAR);
            Selection.AddOne(frame.Beams[0].Id);
            Selection.AddOne(frame.Beams[1].Id);

            barServer.SetLabel(Selection, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);

            //barServer.SetLabel(RightColumn, IRobotLabelType.I_LT_BAR_SECTION, builtUpSectionLabel.Name);


            robotApp.Interactive = 1;
            return true;
        }

    }
}
