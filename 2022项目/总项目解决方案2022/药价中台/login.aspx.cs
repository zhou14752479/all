using MySql.Data.MySqlClient;
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
               
                if (username!="" && password !="")
                {
                    alogin(username,password);
                }
                else
                {
                    Response.Write(@"<script>alert('用户名或密码为空！');</script>");
                }
            }
        }




        #region  登陆函数

        public void alogin(string username,string password)
        {


            try
            {
              
                MySqlConnection mycon = new MySqlConnection(api.constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from users where username='" + username + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string user = reader["username"].ToString().Trim();
                    string pass= reader["password"].ToString().Trim();
                  
                    if (pass == password)
                    {
                        HttpCookie hcUserName1 = new HttpCookie("username"); // 创建一个名为uname的cookie
                        hcUserName1.Expires = DateTime.Now.AddDays(3); // 设置该cookie的有效时间
                        hcUserName1.Value = username; // 给cookie赋值（也就是你想保存的账号，或者密码）
                        HttpContext.Current.Response.Cookies.Add(hcUserName1); // 提交cookie

                        Response.Write(@"<script>alert('登录成功！');window.location.href='admin.html'</script>");

                    }

                    else
                    {

                        Response.Write(@"<script>alert('用户名与密码不匹配！');</script>");
                    }


                }

                else
                {
                    Response.Write(@"<script>alert('用户名不存在！');</script>");
                }


            }

            catch (System.Exception ex)
            {
                Response.Write(@"<script>alert('登录异常，请联系管理员');</script>");
            }

        }


        #endregion




    }

}