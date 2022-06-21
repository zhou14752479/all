using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP足球网站
{
    public partial class savedata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Request.RequestType == "POST")
            {
                if (Request.QueryString["type"] == "savedata")
                {
                    Response.Write("123");

                }

            }
        }
    }
}