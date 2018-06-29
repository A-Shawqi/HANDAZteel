using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class ctrl_AnalysisResults : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<List<string>> All = new List<List<string>>();
                    List<string> temp1 = new List<string>() { "Node ID", "Nx", "Ny", "Nz", "Qz", "Qy", "Qz", "Mx", "My", "Mz" };
                    All.Add(temp1);
                for (int i = 0; i < 50; i++)
                {
                    List<string> temp2 = new List<string>() { "1", "300", "45.2", "300", "22.25", "263.45", "300", "300", "300", "300" };
                    All.Add(temp2);
                }





                DataTable DOF = new DataTable();
                DOF.Columns.AddRange(new DataColumn[2 + 1] {
                            new DataColumn("Node Id", typeof(string)),
                            new DataColumn("Ux", typeof(double)),
                            new DataColumn("Uy",typeof(double))
            });

                for (int i = 0; i < 10; i++)
                {
                    DataRow dr = DOF.NewRow();
                    //if (i < nNodesActual)
                    {
                        DOF.Rows.Add(156.258*(2+ i) *i, 156.258 * (2 + i) * i,
                           156.258 * (2 + i) * i);
                    }
                    //else
                    //{
                    //    DOF.Rows.Add(String.Format("Mid Element No. {0}", i - nNodesActual + 1), QuadNodes.At(i, 9), QuadNodes.At(i, 10));
                    //}
                }

                grv_analysisResults.DataSource = DOF;
                grv_analysisResults.DataBind();
            }
        }

        

        //Export to Word
        //protected void btn_ExportExcel_Click(object sender, EventArgs e)
        //{
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.Charset = "";
        //    string FileName = "Calculation Sheet" + DateTime.Now + ".docx";
        //    StringWriter strwritter = new StringWriter();
        //    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = "application/vnd.ms-word";
        //    Response.AddHeader("Analysis Results", "attachment;filename=" + FileName);
        //    grv_analysisResults.GridLines = GridLines.Both;
        //    //grv_analysisResults.HeaderStyle.Font.Bold = true;
        //    //////////////////////////

        //    //grv_analysisResults.AllowPaging = false;

        //    //grv_analysisResults.DataBind();

        //    //Response.ClearContent();
        //    //Response.AddHeader("Analysis Results", string.Format("attachment;filename{0}", "Countries.doc"));

        //    //Response.Charset = "";
        //    //Response.ContentType = "application/ms-word";
        //    //StringWriter sw = new StringWriter();

        //    //HtmlTextWriter htw = new HtmlTextWriter(sw);


        //    System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();

        //    Controls.Add(form);
        //    form.Controls.Add(grv_analysisResults);
        //    form.RenderControl(htmltextwrtter);
        //    //grv_analysisResults.RenderControl(htw);

        //    Response.Write(strwritter.ToString());
        //    Response.End();
        //}

        //Export to Excel
        protected void btn_ExportWord_Click(object sender, EventArgs e)
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
            grv_analysisResults.GridLines = GridLines.Both;
            grv_analysisResults.HeaderStyle.Font.Bold = true;
            //grv_analysisResults.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            grv_analysisResults.HeaderStyle.ForeColor = System.Drawing.Color.White; 
            //grv_analysisResults.RenderControl(htmltextwrtter);

            grv_analysisResults.Style.Add("background-color", "#FFFFFFF");

            for (int i = 0; i < grv_analysisResults.HeaderRow.Cells.Count; i++)
            {
                grv_analysisResults.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
            }
            int j = 1;

            foreach (GridViewRow GvRows in grv_analysisResults.Rows)
            {
                GvRows.HorizontalAlign = HorizontalAlign.Center;
                //GvRows.BackColor = ConsoleColor.White;
                if (j <= grv_analysisResults.Rows.Count)
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
            form.Controls.Add(grv_analysisResults);
            form.RenderControl(htmltextwrtter);

            Response.Write(strwritter.ToString());
            Response.End();


            //grv_analysisResults.AllowPaging = false;

            //Response.ClearContent();
            //Response.Buffer = true;

            //Response.AddHeader("Analysis Results", string.Format("attachment;filename{0}", "Countries.xls"));

            //Response.ContentType = "application/ms-excel";
            //StringWriter sw = new StringWriter();

            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //grv_analysisResults.Style.Add("background-color", "#FFFFFFF");

            //for (int i = 0; i < grv_analysisResults.HeaderRow.Cells.Count; i++)
            //{
            //    grv_analysisResults.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
            //}
            //int j = 1;

            //foreach (GridViewRow GvRows  in grv_analysisResults.Rows)
            //{
            //    //GvRows.BackColor = ConsoleColor.White;
            //    if (j <= grv_analysisResults.Rows.Count)
            //    {
            //        if (j % 2 != 0)
            //        {
            //            for (int k=0; k< GvRows.Cells.Count; k++)
            //            {
            //                GvRows.Cells[k].Style.Add("background-color", "#EFF3FB");

            //            }
            //        }
            //    }

            //    j++;
            //}
            //System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();

            //Controls.Add(form);
            //form.Controls.Add(grv_analysisResults);
            //form.RenderControl(htw);

            ////grv_analysisResults.RenderControl(htw);
            //Response.Write(sw.ToString());
            //Response.End();
        }
    }
}