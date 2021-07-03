using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //如果隐藏域的值不为空表示用户是点击的提交按钮发出的post请求
            //if (!string.IsNullOrEmpty(Request.Form["isPostBack"]))
            //{
            //    regis();
            //}


            regis();

        }


        #region 注册函数
        /// <summary>
        /// 注册函数
        /// </summary>

        public void regis()
        {
            string username = Request.Form["username"];
            string password = Request.Form["password"];
            string tell = Request.Form["tell"];
            string time = DateTime.Now.ToString();
            string ip = method.GetIp();
            string mac = method.GetMacAddress();

            if (username !=null && password != null && tell != null)

            {

                string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                MySqlConnection mycon = new MySqlConnection(connStr);
                mycon.Open();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO vip_points (username,password,phone,register_t,ip,mac)VALUES('" + username + " ', '" + password + " ','" + tell + " ','" + time + " ','" + ip + " ', '" + mac + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    Response.Redirect("/success.html");
                    mycon.Close();

                }
                else
                {
                    Response.Redirect("/error.html");
                }

            }

            else
            {
                Response.Redirect("/error.html");
            }

        }


        #endregion







    }
}
