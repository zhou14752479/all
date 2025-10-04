using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 模板下载
{
    public partial class api : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                Response.ContentType = "text/html";
                string method = Request["method"].Trim();
                string key = Request["key"].Trim();
                string mubanid = Request["mubanid"].Trim();

                if (method == "getmuban135" && mubanid != "" && key != "")
                {
                    getmuban135(key, mubanid);

                }
                else
                {
                    Response.Write("模板ID输入有误，请联系客服");
                }
            }
            catch (Exception ex)
            {
                Response.Write("服务异常，请联系客服");
            }
        }



        #region 获取135编辑器getmuban_135
        public void getmuban135(string key, string mubanid)
        {

            MySqlConnection mycon = new MySqlConnection(method.constr);
            mycon.Open();
            string query = "SELECT * FROM mykeys where mykey= '" + key + "' ";
            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();



            if (reader.Read())
            {
                string cishu = reader["cishu"].ToString().Trim();
              

                mycon.Close();
                reader.Close();



                if (Convert.ToInt32(cishu) <= 0)
                {
                    Response.Write("剩余次数不足，请联系客服");
                    return;
                }

               
               


                string url = "https://www.135editor.com/editor_styles/" + mubanid + "?preview=1";

                string html = method.GetUrlWithCookie(url);

                string name = Regex.Match(html, @"""name"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string content = Regex.Match(html, @"""content"":""([\s\S]*?)"",""sort").Groups[1].Value.Trim();
                name =method.Unicode2String(name);  
                content=method.Unicode2String(content).Replace("\\","");
               

                if (content == "")
                {
                    Response.Write("解析失败，请联系客服");
                    return;
                }
                else
                {
                    int cishu_new = Convert.ToInt32(cishu) - 1;
                   
                    Response.Write(content);
                    method.editekey(key); //下载成功  减去次数

                }

            }
            else
            {
                Response.Write("秘钥错误或不存在，请联系客服！");
                mycon.Close();
                reader.Close();

            }

        }
        #endregion








    }
}