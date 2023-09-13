using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP舆情统计
{
    public partial class api : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Request.RequestType == "POST")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string data = Encoding.UTF8.GetString(b);


                string method = Request["method"];
                string username = Request["username"];
                string password = Request["password"];
              
                string uid = Request["uid"];

                string website = Request["website"]; 
                string title = Request["title"]; 
                string feedback = Request["feedback"];
                string time = Request["time"];
                string author = Request["author"];
                string AssignedUnite = Request["AssignedUnite"];
                string finish = Request["finish"];
                if (method == "login")
                {
                    login(username, password);
                }
               

                if (method == "getdatas")
                {
                    getdatas(title);
                }
                if (method == "add")
                {

                    add(website,  title,  feedback,  time,  author,  AssignedUnite,  finish);
                }
              
                if (method == "del")
                {
                    del(uid);
                }
                if (method == "edite")
                {
                    edite(uid, website, title, feedback, time, author, AssignedUnite, finish);
                }
            }


        }




        #region 查询data
        public void getdatas(string title)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            //string query = "SELECT * FROM datas where username=  '" + username + "' ORDER BY date desc";

            string query = "SELECT * FROM datas  ORDER BY time desc";


            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                string json = JsonConvert.SerializeObject(dataTable);

                Response.Write(json);
                mycon.Close();
                reader.Close();

            }
            else
            {

                mycon.Close();
                reader.Close();

            }

        }
        #endregion

        public static string constr = "Host =localhost;Database=yuqing;Username=root;Password=root";


        #region  登录

        public void login(string username, string password)
        {

            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM users where username= '" + username + "' ";
            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();

            //DataTable dataTable = new DataTable();
            //dataTable.Load(reader);
            //string json = JsonConvert.SerializeObject(dataTable);

            if (reader.Read())
            {
                string userid = reader["userid"].ToString().Trim();
                string pass = reader["password"].ToString().Trim();
                string usertype = reader["usertype"].ToString().Trim();
                if (pass == password)
                {



                    Response.Write("{\"userid\":\"" + userid + "\",\"username\":\"" + username + "\",\"usertype\":\"" + usertype + "\"}");
                    mycon.Close();
                    reader.Close();
                }
                else
                {
                    Response.Write("{\"status\":\"0\"}");
                    mycon.Close();
                    reader.Close();

                }

            }
            else
            {
                Response.Write("{\"status\":\"0\"}");
                mycon.Close();
                reader.Close();

            }



        }
        #endregion

        #region  添加券码

        public void add(string website, string title, string feedback, string time, string author,string AssignedUnite,  string finish)
        {
           
       
            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

            
                string sql = "INSERT INTO datas (website,title,feedback,author,AssignedUnite,finish)VALUES('" + website + " ', '" + title + " ', '" + feedback + " ','" + time + " ','" + author + " ','" + AssignedUnite + " ','" + finish + " ')";



                MySqlCommand cmd = new MySqlCommand(sql, mycon); ;         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    mycon.Close();
                    Response.Write("{\"status\":\"1\"}");
                }
                else
                {
                    mycon.Close();
                    Response.Write("{\"status\":\"0\"}");
                }
            }

            catch (System.Exception ex)
            {
                Response.Write(ex.ToString());

            }
        }
        #endregion
        #region 修改code
        public void edite(string uid, string website, string title, string feedback, string time, string author, string AssignedUnite, string finish)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            string query = "update codes set website='" + website + "',title='" + title + "',feedback='" + feedback + "',author='" + author + "',time='" + time + "',AssignedUnite='"+ AssignedUnite + "',finish='"+finish+"' where uid=  '" + uid + "' ";
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            int count = cmd.ExecuteNonQuery();
            if (count > 0)
            {


                Response.Write("{\"status\":\"1\"}");
                mycon.Close();


            }
            else
            {
                Response.Write(query);
                Response.Write("{\"status\":\"0\"}");
                mycon.Close();


            }

        }
        #endregion

        #region 删除code
        public void del(string code)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "delete FROM codes where code=  '" + code + "' ";
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            int count = cmd.ExecuteNonQuery();
            if (count > 0)
            {


                Response.Write("{\"status\":\"1\"}");
                mycon.Close();


            }
            else
            {
                Response.Write("{\"status\":\"0\"}");
                mycon.Close();


            }

        }
        #endregion
    }
}