using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApplication2
{
    /// <summary>
    /// doRegister 的摘要说明
    /// </summary>
    public class doRegister : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
            string username = context.Request.Form["username"];
            string password = context.Request.Form["password"];
            string tell = context.Request.Form["tell"];
            string time = DateTime.Now.ToString();
            string ip = method.GetIp();
            string mac = method.GetMacAddress();

            if (username !="" && password !="" && tell != "")

            {

                string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                MySqlConnection mycon = new MySqlConnection(connStr);
                mycon.Open();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO vip_points (username,password,phone,register_t,ip,mac)VALUES('" + username + " ', '" + password + " ','" + tell + " ','" + time + " ','" + ip + " ', '" + mac + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)

                context.Response.Redirect("/success.html");
                mycon.Close();

            }
            else
            {
                context.Response.Redirect("/error.html");
            }





        
    }




        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}