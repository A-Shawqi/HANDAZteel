using HANDAZ.BusinessComponents;
using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class Ctrl_EgyptianCode : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public static void Designer(HndzFrameSingleBay3D AnalyzedFrame)
        {
            HndzFrameSingleBay3D designedFrame = HndzDesigner.AssembleSections(AnalyzedFrame);
            
        }
    }
}