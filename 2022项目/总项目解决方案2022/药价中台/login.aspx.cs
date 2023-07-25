using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 药价中台
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.RequestType == "POST")
            {
                string username = Request["username"];
                string password = Request["password"];
               
                if (username==password && username!="")
                {
                   
                    Response.Write(@"<script>alert('登录成功！');window.location.href='admin.html'</script>");
                    Application["username"] = username;
                }
                else
                {
                    Response.Write(@"<script>alert('用户名或密码错误！');</script>");
                }
            }
        }









    }

}