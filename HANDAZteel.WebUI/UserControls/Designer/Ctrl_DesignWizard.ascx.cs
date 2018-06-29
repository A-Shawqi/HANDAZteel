using HANDAZ.BusinessComponents;
using HANDAZ.Entities;
using HANDAZ.PEB.BusinessComponents;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public enum DesignCodeEnum
    {
        ECP,
        AISC
    }
    public partial class DesignWizard : System.Web.UI.UserControl
    {
        static HndzFrame3D finalFrame = null;
        bool IsInvalidModel;
        static DesignCodeEnum code;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //string[] arr = { "Column Layout", "Bracing", "Elevation" };
                //Grd_Autocad.Rows[0].Cells[0].Text = arr[0];
                //Grd_Autocad.Rows[1].Cells[0].Text = arr[1];
                //Grd_Autocad.Rows[2].Cells[0].Text = arr[2];

            }
            if (IsPostBack)
            {
                HndzFrameTypeEnum type;
                if (Enum.TryParse(Request.QueryString["FrameType"], out type))
                {
                    if (Request.Cookies.Get("Frame") != null)
                    {
                        string absolutePath = Request.Cookies.Get("Frame").Value;

                        using (XmlReader xmlReader = XmlReader.Create(absolutePath))
                        {
                            DataContractSerializer deserializer;
                            switch (type)
                            {
                                case HndzFrameTypeEnum.Undefined:
                                    throw new NotImplementedException("Undefined frame");
                                    break;
                                case HndzFrameTypeEnum.ClearSpan:
                                    deserializer = new DataContractSerializer(typeof(HndzFrameSingleBay3D));
                                    finalFrame = deserializer.ReadObject(xmlReader) as HndzFrameSingleBay3D;
                                    break;
                                case HndzFrameTypeEnum.SingleSlope:
                                    deserializer = new DataContractSerializer(typeof(HndzFrameMonoSlope3D));
                                    finalFrame = deserializer.ReadObject(xmlReader) as HndzFrameMonoSlope3D;
                                    break;
                                case HndzFrameTypeEnum.MultiSpan1:
                                    deserializer = new DataContractSerializer(typeof(HndzFrameMultiSpan13D));
                                    finalFrame = deserializer.ReadObject(xmlReader) as HndzFrameMultiSpan13D;
                                    break;
                                case HndzFrameTypeEnum.MultiSpan2:
                                    deserializer = new DataContractSerializer(typeof(HndzFrameMultiSpan23D));
                                    finalFrame = deserializer.ReadObject(xmlReader) as HndzFrameMultiSpan23D;
                                    break;
                                case HndzFrameTypeEnum.MultiSpan3:
                                    deserializer = new DataContractSerializer(typeof(HndzFrameMultiSpan33D));
                                    finalFrame = deserializer.ReadObject(xmlReader) as HndzFrameMultiSpan33D;
                                    break;
                                case HndzFrameTypeEnum.MultiGable:
                                    deserializer = new DataContractSerializer(typeof(HndzFrameMultiGable3D));
                                    finalFrame = deserializer.ReadObject(xmlReader) as HndzFrameMultiGable3D;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected void Btn_SapAnalyze_Click(object sender, EventArgs e)
        {
            #region Analysis Model and Straining Actions and Design by SAP2000
            SAPAnalysisModel model = new SAPAnalysisModel();
            model.GenerateFrame(finalFrame, finalFrame.Name, false, true);
            #endregion

            SaveFrameChanges();
            Grd_DesignSummary.DataSource = ElementLoadsGridView();
            Grd_DesignSummary.DataBind();
            Grd_SectionSummary.DataSource = LoadSections();
            Grd_SectionSummary.DataBind();

            CreateBIMModel();
            //TODO: Use SAP Analysis
            //TODO: Save the model path and keep it in cookies
            Btn_Sap2000_Down.Visible = true;

            //======================== 
            // rebind grid view to new analysis results
            //HndzFrameSingleBay3D frame = finalFrame as HndzFrameSingleBay3D;
            //Grd_DesignSummary.DataSource = frame.Frames2D;
            //Grd_DesignSummary.DataBind();

        }

        private void SaveFrameChanges()
        {
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

        private void CreateBIMModel()
        {
            HttpCookie cookNameFile = null;
            if (Request.Cookies["FileName"] == null)
            {
                cookNameFile = new HttpCookie("FileName");
            }
            else
            {
                cookNameFile = Response.Cookies["FileName"];
            }


            string fileName = /*"DesignedFrame" +*/ Guid.NewGuid().ToString();
            string filePath = Resources.WebResources.wexbimPath + fileName;
            filePath = Server.MapPath(filePath);
            try
            {
                // HndzFrameSingleBay3D tempFrame = Session["FrameObject"] as HndzFrameSingleBay3D;
                BIM.ConvertToIFC.GenerateIFCProject(finalFrame.BuildingStorey.Building.Project, filePath);
                //BIM.ConvertToIFC.GenerateIFCFrame((HndzFrameSingleBay3D)finalFrame, filePath);
                cookNameFile.Value = fileName;
                Response.Cookies.Add(cookNameFile);
            }
            catch (Exception ex)
            {
                IsInvalidModel = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Alert", "$('btn_error').click();", true);//wrong
            }
        }

        protected void Btn_StaadAnalyze_Click(object sender, EventArgs e)
        {
            if (finalFrame is HndzFrameSingleBay3D)
            {
                HndzFrameSingleBay3D frameS = finalFrame as HndzFrameSingleBay3D;
                STAADAnalysisModel.GenerateClearSpanFrame(frameS, finalFrame.Name);
                Btn_Staad_Down.Visible = true;

            }
            else
            {
                //TODO
                Btn_StaadAnalyze.Enabled = false;
            }

            //TODO: Use STAAD Analysis
            //TODO: Save the model path and keep it in cookies
            // rebind grid view to new analysis results 
            //HndzFrameSingleBay3D frame = finalFrame as HndzFrameSingleBay3D;
            //Grd_DesignSummary.DataSource = frame.Frames2D;
            //Grd_DesignSummary.DataBind();

        }



        protected void Btn_ExportToWord_Click(object sender, EventArgs e)
        {
            //ExportDataToWord.createWordDocument();


        }

        protected void Btn_RobotAnalyze_Click(object sender, EventArgs e)
        {
            //TODO: Use Robot Analysis
            //TODO: Save the model path and keep it in cookies
            //TODO : Deserialize Frame TO Design On robot 


            #region Analysis Model and Straining Actions and Design by SAP2000
            #endregion


            //TODO: Use SAP Analysis
            if (finalFrame is HndzFrameSingleBay3D)
            {
                HndzFrameSingleBay3D frameR = finalFrame as HndzFrameSingleBay3D;
                string Path = Server.MapPath(Resources.WebResources.RobotPath);
                RobotObjectUI.ROBOTDESIGN(frameR);
                RobotObjectUI.SaveFile(Path + finalFrame.GlobalId);
                Btn_robot_Down.Visible = true;
            }
            else
            {
                //TODO
                Btn_RobotAnalyze.Enabled = false;
            }

            // rebind grid view to new analysis results 
            //HndzFrameSingleBay3D frame = finalFrame as HndzFrameSingleBay3D;
            //Grd_DesignSummary.DataSource = frame.Frames2D;
            //Grd_DesignSummary.DataBind();

        }

        protected void Btn_Sap2000_Down_Click(object sender, EventArgs e)
        {
            string path = SAPAnalysisModel.GetPath(finalFrame.Name);
            //File.Copy(path)
            //string absolutePath = Server.MapPath(path);
            Response.ContentType = "Application/sdb";
            Response.AppendHeader("content-disposition",
                    "attachment; filename=" + path);
            Response.TransmitFile(path);
            Response.End();

        }

        protected void Btn_Staad_Down_Click(object sender, EventArgs e)
        {
            string path = STAADAnalysisModel.GetPath(finalFrame.Name);
            //File.Copy(path)
            //string absolutePath = Server.MapPath(path);
            Response.ContentType = "Application/std";
            Response.AppendHeader("content-disposition",
                    "attachment; filename=" + path);
            Response.TransmitFile(path);
            Response.End();
        }

        protected void Btn_Aisc_Click(object sender, EventArgs e)
        {
            code = DesignCodeEnum.AISC;
        }

        protected void Btn_Egc_Click(object sender, EventArgs e)
        {
            code = DesignCodeEnum.ECP;
            //HndzFrameSingleBay3D frame = finalFrame as HndzFrameSingleBay3D;
            //Grd_DesignSummary.DataSource = frame.Frames2D;
            //Grd_DesignSummary.DataBind();
        }

        protected void Btn_SAPAnalyzeDesign_Click(object sender, EventArgs e)
        {
            SAPAnalysisModel model;
            switch (code)
            {
                case DesignCodeEnum.ECP:
                    model = new SAPAnalysisModel();
                    model.GenerateFrame(finalFrame, finalFrame.Name, false, true);
                    if (finalFrame is HndzFrameSingleBay3D)
                    {
                        HndzFrameSingleBay3D frameE = finalFrame as HndzFrameSingleBay3D;
                        HndzDesigner.AssembleSections(frameE);
                    }   // rebind grid view to new analysis results 
                    break;
                case DesignCodeEnum.AISC:
                    model = new SAPAnalysisModel();
                    model.GenerateFrame(finalFrame, finalFrame.Name, true, true);
                    break;
                default:
                    break;
            }
            SaveFrameChanges();
            CreateBIMModel();
            // rebind grid view to new analysis results 
            //HndzFrameSingleBay3D frame = finalFrame as HndzFrameSingleBay3D;
            //Grd_DesignSummary.DataSource = frame.Frames2D;
            //Grd_DesignSummary.DataBind();
            Grd_DesignSummary.DataSource = ElementLoadsGridView();
            Grd_DesignSummary.DataBind();
            Grd_SectionSummary.DataSource = LoadSections();
            Grd_SectionSummary.DataBind();

        }

        protected void Btn_robot_Down_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath(Resources.WebResources.RobotPath) + finalFrame.GlobalId + ".rtd";
            if (File.Exists(path))
            {
                //File.Copy(path)
                //string absolutePath = Server.MapPath(path);
                Response.ContentType = "Application/rtd";
                Response.AppendHeader("content-disposition",
                        "attachment; filename=" + path);
                Response.TransmitFile(path);
                Response.End();
            }
            else
            {
                //ToDo:Send Error Notification to user
            }

        }

        protected void Btn_DownIfc_Click(object sender, EventArgs e)
        {
            string fileName = "PEB";

            if (Request.Cookies.Get("FileName") != null)
            {
                if (Request.Cookies.Get("FileName").Value != "")
                {
                    fileName = Request.Cookies.Get("FileName").Value;
                }
            }
            string path = Resources.WebResources.wexbimPath + fileName + ".ifc";
            path = Server.MapPath(path);
            if (File.Exists(path))
            {
                //File.Copy(path)
                //string absolutePath = Server.MapPath(path);
                Response.ContentType = "Application/ifc";
                Response.AppendHeader("content-disposition",
                        "attachment; filename=" + path);
                Response.TransmitFile(path);
                Response.End();
            }
            else
            {
                //ToDo:Send Error Notification to user
            }
        }
        private DataTable LoadDesignGridView()
        {
            HndzFrameSingleBay3D frameR = finalFrame as HndzFrameSingleBay3D;
            //   frameR.Frames2D.ElementAt(0).LeftBeam.
            DataTable FrameDataTable = new DataTable();
            FrameDataTable.Columns.AddRange(new DataColumn[5]
            {
                            new DataColumn("Frame Id", typeof(string)),
                            new DataColumn("Left Beam ID", typeof(string)),
                            new DataColumn("Right Beam ID", typeof(string)),
                            new DataColumn("Left Column Id", typeof(string)),
                            new DataColumn("Right Column Id", typeof(string)),
            });
            for (int i = 0; i < frameR.FramesCount; i++)
            {
                DataRow dr = FrameDataTable.NewRow();
                FrameDataTable.Rows.Add(frameR.Frames2D.ElementAt(i).GlobalId.ToString(),
                   frameR.Frames2D.ElementAt(i).LeftBeam.GlobalId.ToString(), frameR.Frames2D.ElementAt(i).RightBeam.GlobalId.ToString(),
                    frameR.Frames2D.ElementAt(i).LeftColumn.GlobalId.ToString(), frameR.Frames2D.ElementAt(i).RightColumn.GlobalId.ToString());
            }
            return FrameDataTable;
        }
        private DataTable AutoCadLoad()
        {
            // HndzFrameSingleBay3D frameR = finalFrame as HndzFrameSingleBay3D;
            //   frameR.Frames2D.ElementAt(0).LeftBeam.
            DataTable Cad = new DataTable();
            Cad.Columns.AddRange(new DataColumn[1]
            {
                            new DataColumn("Cad Files", typeof(string)),
                            //new DataColumn("Left Beam ID", typeof(string)),
                            //new DataColumn("Right Beam ID", typeof(string)),
                            //new DataColumn("Left Column Id", typeof(string)),
                            //new DataColumn("Right Column Id", typeof(string)),
            });
            for (int i = 0; i < 3; i++)
            {
                DataRow dr = Cad.NewRow();
                Cad.Rows.Add("Layout", "Elevation", "Bracing");
            }
            return Cad;
        }
        private DataTable ElementLoadsGridView()
        {
            if (finalFrame is HndzFrameSingleBay3D)
            {


                HndzFrameSingleBay3D frameR = finalFrame as HndzFrameSingleBay3D;
                List<HndzStructuralElement> ELements = new List<HndzStructuralElement>();
                ELements.Add(frameR.Frames2D.ElementAt(0).LeftBeam);
                ELements.Add(frameR.Frames2D.ElementAt(0).RightBeam);
                ELements.Add(frameR.Frames2D.ElementAt(0).LeftColumn);
                ELements.Add(frameR.Frames2D.ElementAt(0).RightColumn);

                var p = frameR.Frames2D.ElementAt(0);
                DataTable FrameDataTable = new DataTable();
                FrameDataTable.Columns.AddRange(new DataColumn[10]
               {
                            new DataColumn("Element Name", typeof(string)),
                            new DataColumn("Start Point", typeof(string)),
                            new DataColumn("End Point", typeof(string)),
                            new DataColumn("Station", typeof(string)),
                            new DataColumn("Load Case", typeof(string)),
                            new DataColumn("Axial Force", typeof(string)),
                            new DataColumn("Shear 2", typeof(string)),
                            new DataColumn("Shear 3", typeof(string)),
                             new DataColumn("Moment 2", typeof(string)),
                            new DataColumn("Moment 3", typeof(string)),
               });
                DataRow dr = FrameDataTable.NewRow();
                const int d = 3;
                foreach (HndzStructuralElement Elem in ELements)
                {
                    for (int j = 0; j < frameR.Frames2D.ElementAt(0).LeftBeam.AnalysisResultsEnvelope.Length; j++)
                    {
                        FrameDataTable.Rows.Add(Elem.Name.ToString(),
                        Elem.ExtrusionLine.baseNode.ToString(),
                        Elem.ExtrusionLine.EndNode.ToString(),
                         Math.Round(Elem.AnalysisResultsEnvelope.ElementAt(j).Station, d).ToString(),
                        Elem.AnalysisResultsEnvelope.ElementAt(j).LoadCase.ToString(),
                         Math.Round(Elem.AnalysisResultsEnvelope.ElementAt(j).Axial, d).ToString(),
                         Math.Round(Elem.AnalysisResultsEnvelope.ElementAt(j).Shear2, d).ToString(),
                         Math.Round(Elem.AnalysisResultsEnvelope.ElementAt(j).Shear3, d).ToString(),
                         Math.Round(Elem.AnalysisResultsEnvelope.ElementAt(j).Moment2, d).ToString(),
                         Math.Round(Elem.AnalysisResultsEnvelope.ElementAt(j).Moment3, d).ToString());
                    }
                }
                return FrameDataTable;
            }
            else
            {
                return null;
            }
        }
        private DataTable LoadSections()
        {
            if (finalFrame is HndzFrameSingleBay3D)
            {
                HndzFrameSingleBay3D frameR = finalFrame as HndzFrameSingleBay3D;
                List<HndzStructuralElement> ELements = new List<HndzStructuralElement>();
                ELements.Add(frameR.Frames2D.ElementAt(0).LeftBeam);
                ELements.Add(frameR.Frames2D.ElementAt(0).RightBeam);
                ELements.Add(frameR.Frames2D.ElementAt(0).LeftColumn);
                ELements.Add(frameR.Frames2D.ElementAt(0).RightColumn);
                var p = frameR.Frames2D.ElementAt(0);
                DataTable FrameDataTable = new DataTable();
                FrameDataTable.Columns.AddRange(new DataColumn[9]
               {
                            new DataColumn("Element Name", typeof(string)),
                            new DataColumn("Start Profile Web Height", typeof(string)),
                            new DataColumn("Start Profile Web Thickness", typeof(string)),
                            new DataColumn("Start Profile Flang Width", typeof(string)),
                            new DataColumn("Start Profile Flang Thickness", typeof(string)),
                            new DataColumn("End Profile Web Height", typeof(string)),
                            new DataColumn("End Profile Web Thickness", typeof(string)),
                             new DataColumn("End Profile Flang Width", typeof(string)),
                            new DataColumn("End Profile Flang Thickness", typeof(string)),
               });
                DataRow dr = FrameDataTable.NewRow();
                foreach (HndzStructuralElement Elem in ELements)
                {
                    var Profile = (HndzITaperedProfile)Elem.Profile;
                    FrameDataTable.Rows.Add(Elem.Name.ToString(),
                    //Elem.ExtrusionLine.baseNode.ToString(),
                    //Elem.ExtrusionLine.EndNode.ToString(),
                    Profile.StartProfile.I_Section.d.ToString(),
                    Profile.StartProfile.I_Section.t_w.ToString(),
                    Profile.StartProfile.I_Section.b_f.ToString(),
                    Profile.StartProfile.I_Section.tf.ToString(),
                    Profile.EndProfile.I_Section.d.ToString(),
                    Profile.EndProfile.I_Section.t_w.ToString(),
                    Profile.EndProfile.I_Section.b_f.ToString(),
                    Profile.EndProfile.I_Section.tf.ToString());
                }
                return FrameDataTable;
            }
            return null;
        }

        protected void Btn_GoToXbimViewer_Click(object sender, EventArgs e)
        {
            string url = "/Pages/Designer/IFCViewerFull.aspx";
            string script = string.Format("window.open('{0}');", url);

            Page.ClientScript.RegisterStartupScript(this.GetType(),
                "newPage" + UniqueID, script, true);
            Response.Redirect("/Pages/Designer/IFCViewer.aspx");//ToDo:goto Viewer and final results page isa
        }

        protected void Grd_Autocad_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Btn_ExportToExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Calculation Sheet" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            Grd_SectionSummary.GridLines = GridLines.Both;
            Grd_SectionSummary.HeaderStyle.Font.Bold = true;
            //Grd_SectionSummary.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            Grd_SectionSummary.HeaderStyle.ForeColor = System.Drawing.Color.White;
            //Grd_SectionSummary.RenderControl(htmltextwrtter);

            Grd_SectionSummary.Style.Add("background-color", "#FFFFFFF");

            for (int i = 0; i < Grd_SectionSummary.HeaderRow.Cells.Count; i++)
            {
                Grd_SectionSummary.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
            }
            int j = 1;

            foreach (GridViewRow GvRows in Grd_SectionSummary.Rows)
            {
                GvRows.HorizontalAlign = HorizontalAlign.Center;
                //GvRows.BackColor = ConsoleColor.White;
                if (j <= Grd_SectionSummary.Rows.Count)
                {
                    if (j % 2 != 0)
                    {
                        for (int k = 0; k < GvRows.Cells.Count; k++)
                        {
                            GvRows.Cells[k].Style.Add("background-color", "#EFF3FB");

                        }
                    }
                }

                j++;
            }


            System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();

            Controls.Add(form);
            form.Controls.Add(Grd_SectionSummary);
            form.RenderControl(htmltextwrtter);

            Response.Write(strwritter.ToString());
            Response.End();
        }

        protected void Btn_DownCad1_Click(object sender, EventArgs e)
        {
            if (finalFrame is HndzFrameSingleBay3D)
            {
                HndzFrameSingleBay3D finalFrameSingleBay = finalFrame as HndzFrameSingleBay3D;
                string fileName = finalFrame.BuildingStorey.Building.Project.Owner.Name + "_ColumnsLayout_" +  finalFrame.GlobalId.ToString();

                string path = Resources.WebResources.DXFPath + fileName + ".dxf";
                path = Server.MapPath(path);
                DXF.FrameDrawer.drawColumnLayout(finalFrameSingleBay, path);
                if (File.Exists(path))
                {
                    //File.Copy(path)
                    //string absolutePath = Server.MapPath(path);
                    Response.ContentType = "Application/dxf";
                    Response.AppendHeader("content-disposition",
                            "attachment; filename=" + path);
                    Response.TransmitFile(path);
                    Response.End();
                }
            }
            else
            {
                //ToDo:Send Error Notification to user
            }
        }

        protected void Btn_DownCad2_Click(object sender, EventArgs e)
        {
            if (finalFrame is HndzFrameSingleBay3D)
            {
                HndzFrameSingleBay3D finalFrameSingleBay = finalFrame as HndzFrameSingleBay3D;
                string fileName = finalFrame.BuildingStorey.Building.Project.Owner.Name + "_Elevation_" + finalFrame.GlobalId.ToString();

                string path = Resources.WebResources.DXFPath + fileName + ".dxf";
                path = Server.MapPath(path);
                DXF.FrameDrawer.drawElevation(finalFrameSingleBay, path);
                if (File.Exists(path))
                {
                    //File.Copy(path)
                    //string absolutePath = Server.MapPath(path);
                    Response.ContentType = "Application/dxf";
                    Response.AppendHeader("content-disposition",
                            "attachment; filename=" + path);
                    Response.TransmitFile(path);
                    Response.End();
                }
            }
            else
            {
                //ToDo:Send Error Notification to user
            }
        }

        protected void Btn_DownCad3_Click(object sender, EventArgs e)
        {
            if (finalFrame is HndzFrameSingleBay3D)
            {
                HndzFrameSingleBay3D finalFrameSingleBay = finalFrame as HndzFrameSingleBay3D;
                string fileName = finalFrame.BuildingStorey.Building.Project.Owner.Name + "_Bracing_" +  finalFrame.GlobalId.ToString();

                string path = Resources.WebResources.DXFPath + fileName + ".dxf";
                path = Server.MapPath(path);
                DXF.FrameDrawer.drawBracing(finalFrameSingleBay, path);
                if (File.Exists(path))
                {
                    //File.Copy(path)
                    //string absolutePath = Server.MapPath(path);
                    Response.ContentType = "Application/dxf";
                    Response.AppendHeader("content-disposition",
                            "attachment; filename=" + path);
                    Response.TransmitFile(path);
                    Response.End();
                }
            }
            else
            {
                //ToDo:Send Error Notification to user
            }
        }
    }
}