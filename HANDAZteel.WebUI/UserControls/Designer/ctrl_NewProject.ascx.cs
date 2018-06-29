using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using UnitsNet.Units;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class ctrl_NewProject : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Delete old cookies
                if (Request.Cookies["Project"] != null)
                {
                    HttpCookie myCookie = new HttpCookie("Project");
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }
                if (Request.Cookies["FileName"] != null)
                {
                    HttpCookie myCookie = new HttpCookie("FileName");
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }
                if (Request.Cookies["Frame"] != null)
                {
                    HttpCookie myCookie = new HttpCookie("Frame");
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }
                #endregion

                ddl_LengthUnit.DataSource = Enum.GetNames(typeof(LengthUnit));
                ddl_LengthUnit.DataBind();

                ddl_ForceUnit.DataSource = Enum.GetNames(typeof(ForceUnit));
                ddl_ForceUnit.DataBind();

                ddl_TempUnit.DataSource = Enum.GetNames(typeof(TemperatureUnit));
                ddl_TempUnit.DataBind();

                ddl_CoordinatesSystem.DataSource = Enum.GetNames(typeof(HndzWCS));
                ddl_CoordinatesSystem.DataBind();

                ddl_LengthUnit.SelectedIndex = 6;//meter
                ddl_ForceUnit.SelectedIndex = 8;//ton is default
                ddl_TempUnit.SelectedIndex = 1;//c

                txt_ProjectName.Text = Resources.WebResources.DefaultProjectName;
                txt_Description.Text = Resources.WebResources.DefaultProjectDescription;

                txt_Owner_FName.Text = Person.DefaultPerson().Name;
                txt_Designer_FName.Text     = Person.DefaultPerson().Name;
                txt_Consultant_FName.Text   = Person.DefaultPerson().Name;
                txt_Contractor_FName.Text   = Person.DefaultPerson().Name;

                txt_Owner_LName.Text = Person.DefaultPerson().LastName;
                txt_Designer_LName.Text     = Person.DefaultPerson().LastName;
                txt_Consultant_LName.Text = Person.DefaultPerson().LastName;
                txt_Contractor_LName.Text = Person.DefaultPerson().LastName;

                txt_Owner_OrganizationName.Text      = Person.DefaultPerson().Organization;
                txt_Designer_OrganizationName.Text      = Person.DefaultPerson().Organization;
                txt_Consultant_OrganizationName.Text = Person.DefaultPerson().Organization;
                txt_Contractor_OrganizationName.Text = Person.DefaultPerson().Organization;
            }
        }

        protected void btn_Finish_Click(object sender, EventArgs e)
        {
            //ToDo Validate all then create new object of HndzProject to be assigned to frame3D

            HndzProject project = new HndzProject();
      
            project.LengthUnit = (LengthUnit)ddl_LengthUnit.SelectedIndex;
            project.ForceUnit = (ForceUnit)ddl_ForceUnit.SelectedIndex;
            project.TemperatureUnit = (TemperatureUnit)ddl_TempUnit.SelectedIndex;
            project.GlobalCoordinateSystem = (HndzWCS)ddl_CoordinatesSystem.SelectedIndex;
            project.Owner = new Person();
            project.Designer = new Person();
            project.Consultant = new Person();
            project.Contractor = new Person();

            if (!string.IsNullOrWhiteSpace(txt_ProjectName.Text)) project.Name = txt_ProjectName.Text;
            if (!string.IsNullOrWhiteSpace(txt_Description.Text)) project.Description = txt_Description.Text;
            if (!string.IsNullOrWhiteSpace(txt_Owner_FName.Text)) project.Owner.SetName(txt_Owner_FName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Owner_LName.Text)) project.Owner.SetLastName( txt_Owner_LName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Designer_FName.Text  )) project.Owner.SetName(txt_Designer_FName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Consultant_FName.Text)) project.Owner.SetName(txt_Consultant_FName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Contractor_FName.Text)) project.Owner.SetName(txt_Contractor_FName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Designer_LName.Text  )) project.Owner.SetLastName(txt_Designer_LName.Text  );
            if (!string.IsNullOrWhiteSpace(txt_Consultant_LName.Text)) project.Owner.SetLastName(txt_Consultant_LName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Contractor_LName.Text)) project.Owner.SetLastName(txt_Contractor_LName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Designer_OrganizationName.Text  )) project.Owner.SetLastName(txt_Designer_OrganizationName.Text  );
            if (!string.IsNullOrWhiteSpace(txt_Consultant_OrganizationName.Text)) project.Owner.SetLastName(txt_Consultant_OrganizationName.Text);
            if (!string.IsNullOrWhiteSpace(txt_Contractor_OrganizationName.Text)) project.Owner.SetLastName(txt_Contractor_OrganizationName.Text);


            //ToDo: complete assigning all values....................


            DataContractSerializer xmlser = new DataContractSerializer(project.GetType());
            string relativePath = Resources.WebResources.XMLPath + "Project.xml";
            string absolutePath = Server.MapPath(relativePath);
            using (XmlWriter xw = XmlWriter.Create(absolutePath))
            {
                xmlser.WriteObject(xw, project);
            }
            HttpCookie cookName = new HttpCookie("Project");
            cookName.Value = absolutePath;
            Response.Cookies.Add(cookName);

            Response.Redirect("/Pages/Designer/FrameTemplate.aspx");
        }

        
    }
}