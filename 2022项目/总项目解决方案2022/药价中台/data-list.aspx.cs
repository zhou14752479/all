using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 药价中台
{
    public partial class data_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (HttpContext.Current.Request.RequestType == "POST")
            {
                string code = Request["code"];
                Application["code"] = code;
                Response.Write(code);
            }




        }



    }
}