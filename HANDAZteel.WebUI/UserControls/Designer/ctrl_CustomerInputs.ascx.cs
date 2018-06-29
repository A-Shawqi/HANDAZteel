using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HANDAZ.PEB.BusinessComponents;
using HANDAZ.Entities;
using Rhino.Geometry;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;
using Xbim.Common.Geometry;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.GeometricConstraintResource;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MaterialResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PresentationOrganizationResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.RepresentationResource;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.IO;
using Xbim.XbimExtensions.Interfaces;
using XbimGeometry.Interfaces;
using System.Xml;
using System.Runtime.Serialization;
using UnitsNet.Units;
using UnitsNet;
using HANDAZ.BusinessComponents;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class ctrl_CustomerInputs : System.Web.UI.UserControl
    {
        bool IsInvalidModel = false;
        static HndzProject project;
        static LengthUnit selectedUnit;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_BaySpacing.Attributes.Add("readonly", "readonly");
                if (Request.Cookies.Get("Project") != null)
                {
                    string absolutePath = Request.Cookies.Get("Project").Value;
                    using (XmlReader xmlReader = XmlReader.Create(absolutePath))
                    {
                        DataContractSerializer deserializer = new DataContractSerializer(typeof(HndzProject));
                        project = deserializer.ReadObject(xmlReader) as HndzProject;
                    }
                    AdjustDefaultValuesAndUnits();
                }
            }
        }

        private void AdjustDefaultValuesAndUnits()
        {
            selectedUnit = project.LengthUnit;
            Length width = Length.FromMeters(20);
            Length length = Length.FromMeters(60);
            //Length baySpacing = Length.FromMeters(6);
            Length eaveHeight = Length.FromMeters(6);

            lbl_UnitLandLength.Text = project.LengthUnit.ToString();
            lbl_UnitLandWidth.Text = project.LengthUnit.ToString();
            lbl_UnitEaveHeight.Text = project.LengthUnit.ToString();
            lbl_UnitBaySpacing.Text = project.LengthUnit.ToString();

            ddl_Location.DataSource = Enum.GetNames(typeof(HndzLocationEnum));
            ddl_Location.DataBind();

            ddl_RoofAccessability.DataSource = Enum.GetNames(typeof(HndzRoofAccessibilityEnum));
            ddl_RoofAccessability.DataBind();

            ddl_RoofSlope.DataSource = new string[3] { "1:5", "1:10", "1:20" }; //After using, it should be mapped to roofslope Enum
            ddl_RoofSlope.DataBind();
            txt_LandWidth.Text = width.As(selectedUnit).ToString();
            txt_LandLength.Text = length.As(selectedUnit).ToString();
            //txt_BaySpacing.Text = baySpacing.As(selectedUnit).ToString();
            txt_EaveHeight.Text = eaveHeight.As(selectedUnit).ToString();

            ddl_RoofAccessability.SelectedIndex = 1;
            ddl_RoofSlope.SelectedIndex = 1;
            txt_FramesCount.Text = "11";
            txt_BaySpacing.Text = (Convert.ToInt32(double.Parse(txt_LandLength.Text) / (int.Parse(txt_FramesCount.Text) - 1))).ToString();
            //txt_BaySpacing.Text = "6";//angz
            //txt_FramesCount.Text = (Convert.ToInt32(double.Parse(txt_LandLength.Text) / double.Parse(txt_LandLength.Text))+1).ToString();
        }

        protected void btn_Preview_Click(object sender, EventArgs e)
        {
            ConvertIntoFrame();

        }


        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            lbl_ErrorBaySpacing.Visible = false;

            double baySpacing = 0;
            bool baySpacingIsValid = double.TryParse(txt_BaySpacing.Text, out baySpacing);


            double width = Length.From(double.Parse(txt_LandWidth.Text), selectedUnit).Millimeters;
            double length = Length.From(double.Parse(txt_LandLength.Text), selectedUnit).Millimeters;
            baySpacing = Length.From(baySpacing, selectedUnit).Millimeters;
            double eaveHeight = Length.From(double.Parse(txt_EaveHeight.Text), selectedUnit).Millimeters;
            if (!baySpacingIsValid || baySpacing < 5000 || baySpacing > 10000)//ToDo:need Revision
            {
                lbl_ErrorBaySpacing.Visible = true;
                return;
            }


            HndzFrame3D finalFrame = null;
            HndzFrameTypeEnum type;
            HndzBuilding bui = new HndzBuilding(project);
            HndzStorey storey = new HndzStorey(bui, 0);
            if (Enum.TryParse(Request.QueryString["FrameType"], out type))
            {
                switch (type)
                {
                    case HndzFrameTypeEnum.Undefined:
                        throw new NotImplementedException("Undefined Frame");
                        break;
                    case HndzFrameTypeEnum.ClearSpan:
                        finalFrame = new HndzFrameSingleBay3D(length, baySpacing, width,
            eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
             , (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
            HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.SingleSlope:
                        finalFrame = new HndzFrameMonoSlope3D(length, baySpacing, width,
            eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
             , (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
            HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiSpan1:
                        finalFrame = new HndzFrameMultiSpan13D(length, baySpacing, width,
            eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
             , (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
            HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiSpan2:
                        finalFrame = new HndzFrameMultiSpan23D(length, baySpacing, width,
eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
, (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiSpan3:
                        finalFrame = new HndzFrameMultiSpan33D(length, baySpacing, width,
eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
, (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiGable:
                        finalFrame = new HndzFrameMultiGable3D(length, baySpacing, width,
eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
, (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    default:
                        break;
                }

                finalFrame.FramesCount = Convert.ToInt32(txt_FramesCount.Text);
                finalFrame.Type = type;

                DataContractSerializer xmlser = new DataContractSerializer(finalFrame.GetType());
                string relativePath = Resources.WebResources.XMLPath + string.Format("Frame No.{0}.xml", finalFrame.GlobalId.ToString());
                string absolutePath = Server.MapPath(relativePath);
                using (XmlWriter xw = XmlWriter.Create(absolutePath))
                {
                    xmlser.WriteObject(xw, finalFrame);
                }

                HttpCookie cookName = null;
                if (Request.Cookies["Frame"] == null) //mafrod 3mro ma yd5l hna bs e7tyaty
                {
                    cookName = new HttpCookie("Frame");

                }
                else
                {
                    cookName = Response.Cookies["Frame"];
                }
                cookName.Value = absolutePath;
                Response.Cookies.Add(cookName);
            }
            else
            {


                #region IFC Creation
                //HttpCookie cookNameFile = null;
                //if (Request.Cookies["FileName"] == null)
                //{
                //    cookNameFile = new HttpCookie("FileName");
                //}
                //else
                //{
                //    cookNameFile = Response.Cookies["FileName"];
                //    #region Delete old files
                //    //string oldFileName = cookName.Value;
                //    //string oldFilePath = Resources.WebResources.wexbimPath + oldFileName;
                //    //oldFilePath = Server.MapPath(oldFilePath);

                //    //if ((File.Exists(oldFilePath + ".ifc")))
                //    //{
                //    //    File.Delete(oldFilePath + ".ifc");
                //    //}
                //    //if ((File.Exists(oldFilePath + ".wexbim")))
                //    //{
                //    //    File.Delete(oldFilePath + ".wexbim");
                //    //}
                //    #endregion
                //}


                //string fileName = "DesignedFrame" + Guid.NewGuid().ToString();
                //string filePath = Resources.WebResources.wexbimPath + fileName;
                //filePath = Server.MapPath(filePath);
                //try
                //{
                //    BIM.ConvertToIFC.GenerateIFCProject(project, filePath);
                //    cookNameFile.Value = fileName;
                //    Response.Cookies.Add(cookNameFile);
                //    Response.Redirect("/Pages/Designer/DesignWizard.aspx?FrameType=");//ToDo:goto Viewer and final results page isa
                //}
                //catch (Exception ex)
                //{
                //    IsInvalidModel = true;
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Alert", "$('btn_error').click();", true);//wrong
                //}
                #endregion
            }
            Session["FrameObject"] = finalFrame;
            Response.Redirect("/Pages/Designer/DesignWizard.aspx?FrameType=" + finalFrame.Type.ToString());
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            //This is wrong, this should be a client side action using Javascript not ASP.NET
            txt_BaySpacing.Text = string.Empty;
            txt_EaveHeight.Text = string.Empty;
            txt_LandLength.Text = string.Empty;
            txt_LandWidth.Text = string.Empty;
        }



        public string InvalidModel()
        {
            if (IsInvalidModel)
            {
                btn_Preview.Visible = false;
                btn_Submit.Visible = false;
                return "visible";
            }
            btn_Preview.Visible = true;
            btn_Submit.Enabled = true;
            return "hidden";
        }
        private void ConvertIntoFrame()
        {
            lbl_ErrorBaySpacing.Visible = false;

            double baySpacing = 0;
            bool baySpacingIsValid = double.TryParse(txt_BaySpacing.Text, out baySpacing);


            double width = Length.From(double.Parse(txt_LandWidth.Text), selectedUnit).Millimeters;
            double length = Length.From(double.Parse(txt_LandLength.Text), selectedUnit).Millimeters;
            baySpacing = Length.From(baySpacing, selectedUnit).Millimeters;
            double eaveHeight = Length.From(double.Parse(txt_EaveHeight.Text), selectedUnit).Millimeters;
            if (!baySpacingIsValid || baySpacing < 5000 || baySpacing > 10000)//ToDo:need Revision
            {
                lbl_ErrorBaySpacing.Visible = true;
                return;
            }


            HndzFrame3D finalFrame = null;
            HndzFrameTypeEnum type;
            HndzBuilding bui = new HndzBuilding(project);
            HndzStorey storey = new HndzStorey(bui, 0);
            if (Enum.TryParse(Request.QueryString["FrameType"], out type))
            {
                switch (type)
                {
                    case HndzFrameTypeEnum.Undefined:
                        throw new NotImplementedException("Undefined Frame");

                    case HndzFrameTypeEnum.ClearSpan:
                        finalFrame = new HndzFrameSingleBay3D(length, baySpacing, width,
            eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
             , (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
            HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.SingleSlope:
                        finalFrame = new HndzFrameMonoSlope3D(length, baySpacing, width,
            eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
             , (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
            HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiSpan1:
                        finalFrame = new HndzFrameMultiSpan13D(length, baySpacing, width,
            eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
             , (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
            HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiSpan2:
                        finalFrame = new HndzFrameMultiSpan23D(length, baySpacing, width,
eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
, (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiSpan3:
                        finalFrame = new HndzFrameMultiSpan33D(length, baySpacing, width,
eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
, (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    case HndzFrameTypeEnum.MultiGable:
                        finalFrame = new HndzFrameMultiGable3D(length, baySpacing, width,
eaveHeight, 2000, (HndzLocationEnum)ddl_Location.SelectedIndex, (HndzRoofSlopeEnum)ddl_RoofSlope.SelectedIndex
, (HndzRoofAccessibilityEnum)ddl_RoofAccessability.SelectedIndex, HndzBuildingEnclosingEnum.PartiallyEnclosed,
HndzImportanceFactorEnum.II, null, null, null, null, null, storey);
                        break;
                    default:
                        break;
                }
                finalFrame.FramesCount = Convert.ToInt32(txt_FramesCount.Text);
                finalFrame.Type = type;

                HttpCookie cookName = null;
                if (Request.Cookies["FileName"] == null)
                {
                    cookName = new HttpCookie("FileName");
                }
                else
                {
                    cookName = Response.Cookies["FileName"];
                }

                string fileName = Guid.NewGuid().ToString();
                string filePath = Resources.WebResources.wexbimPath + fileName;
                filePath = Server.MapPath(filePath);



                try
                {
                  bool isGenerated=  BIM.ConvertToIFC.GenerateIFCProject(finalFrame.BuildingStorey.Building.Project, filePath);
                    cookName.Value = fileName;
                    Response.Cookies.Add(cookName);
                }
                catch (Exception)
                {
                    IsInvalidModel = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Alert", "$('btn_error').click();", true);//wrong
                }
                return;
            }
        }

        protected void txt_FramesCount_TextChanged(object sender, EventArgs e)
        {
            TextBox txt_framesCount = (TextBox)sender;
            int nFrames = 0;
            bool isConverted = int.TryParse(txt_framesCount.Text, out nFrames);
            if (isConverted && nFrames != 0)
            {
                txt_BaySpacing.Text = (double.Parse(txt_LandLength.Text) / (nFrames - 1)).ToString();
            }
        }
    }
}