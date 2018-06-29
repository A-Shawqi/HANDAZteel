using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class Ctrl_DesignCode : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Btn_DesignAmerican_Click(object sender, EventArgs e)
        {

        }

        protected void Btn_DesignEcg_Click(object sender, EventArgs e)
        {
            HndzFrameSingleBay3D AnalyzedFrame;
            string absolutePath = Request.Cookies.Get("Frame").Value;
            using (XmlReader xmlReader = XmlReader.Create(absolutePath))
            {
                DataContractSerializer deserializer = new DataContractSerializer(typeof(HndzProject));
                AnalyzedFrame = deserializer.ReadObject(xmlReader) as HndzFrameSingleBay3D;
            }
            Ctrl_EgyptianCode.Designer(AnalyzedFrame);
        }
    }
}