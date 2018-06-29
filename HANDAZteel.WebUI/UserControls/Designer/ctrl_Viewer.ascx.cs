using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class ctrl_Viewer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string getWexbimFullPath()
        {
            string fileName = "PEB";

            if (Request.Cookies.Get("FileName") != null)
            {
                if (Request.Cookies.Get("FileName").Value != "")
                {
                    fileName = Request.Cookies.Get("FileName").Value;
                }
            }
            string filePath = Resources.WebResources.wexbimPath + fileName + ".wexbim";
            return Resources.WebResources.wexbimPath + fileName + ".wexbim";
        }
    }
}