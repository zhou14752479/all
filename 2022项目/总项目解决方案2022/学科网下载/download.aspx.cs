using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 学科网下载
{
    public partial class download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string filekey = Request["filekey"];
                string suiji = Request["suiji"];

                if (filekey != "" && filekey != null)
                {
                   
                    filekey = filekey.Replace(suiji, "a");
                    string fileurl = method.Base64Decode(Encoding.GetEncoding("utf-8"), filekey);

                    Response.Redirect(fileurl, false);
                }
                else
                {

                    Response.Write("{\"status\":\"0\",\"msg\":\"地址错误\"}");
                }
            }
            catch (Exception)
            {

                Response.Write("{\"status\":\"0\",\"msg\":\"服务异常，请联系客服\"}");
            }



        }

        
    }
}