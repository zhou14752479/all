using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpMD5_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strmd5 = HttpHelper.ToMD5("admin");

            Response.Write(strmd5);
        }
    }
}