using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HANDAZ.PEB.WebUI.UserControls.Designer
{
    public partial class ctrl_FrameTemplate : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_MultiSpan3_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Designer/CustomerInputs.aspx?FrameType=" + HndzFrameTypeEnum.MultiSpan3.ToString());

        }

        protected void btn_MultiSpan2_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Designer/CustomerInputs.aspx?FrameType=" + HndzFrameTypeEnum.MultiSpan2.ToString());

        }

        protected void btn_MultiSpan1_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Designer/CustomerInputs.aspx?FrameType=" + HndzFrameTypeEnum.MultiSpan1.ToString());

        }

        protected void btn_MonoSlope_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Designer/CustomerInputs.aspx?FrameType=" + HndzFrameTypeEnum.SingleSlope.ToString());

        }

        protected void btn_MultiGable_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Designer/CustomerInputs.aspx?FrameType=" + HndzFrameTypeEnum.MultiGable.ToString());

        }

        protected void btn_ClearSpan_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Designer/CustomerInputs.aspx?FrameType=" + HndzFrameTypeEnum.ClearSpan.ToString());
        }
    }
}