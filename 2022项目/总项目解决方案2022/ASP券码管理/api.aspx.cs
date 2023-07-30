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

namespace ASP券码管理
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
                string usertype= Request["usertype"];
                string shanghuname = Request["shanghuname"];
                string userid = Request["userid"];
                string code = Request["code"];
                string xm_code = Request["xm_code"];
                string xm_name = Request["xm_name"];
                string codeadd_count = Request["codeadd_count"];
                if (method == "login")
                {
                    login(username, password);
                }
                if (method == "adduser")
                {
                    adduser(username, password,usertype,shanghuname);
                }
                if (method == "getusers")
                {
                    getusers();
                }
                if (method == "deluser")
                {
                    deluser(userid);
                }

                if (method == "getcodes")
                {
                    getcodes(username,code,xm_code);
                }
                if (method == "addcodes")
                {
                   
                    addcodes(data, username,  xm_code, xm_name, codeadd_count);
                }
                if (method == "querencode")
                {
                    querencode(code);
                }
            }
        }



             public static string constr = "Host =localhost;Database=quan;Username=root;Password=root";




        #region  登录

        public void login(string username, string password)
            {

            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM users where username=  '" + username + "' ";
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
                Response.Write("{\"status\":\"0\"}");
                mycon.Close();
                reader.Close();

            }
          
        }
        #endregion

        #region  添加用户

        public void adduser(string username, string password,string usertype,string shanghuname)
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO users (username,password,usertype,shanghuname,time)VALUES('" + username + " ', '" + password + " ', '" + usertype + " ','" + shanghuname + " ','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    mycon.Close();
                    Response.Write("{\"status\":\"1\"}");
                }
                else
                {
                    Response.Write("{\"status\":\"0\"}");
                }
            }

            catch (System.Exception ex)
            {
                Response.Write("{\"status\":\"0\"}");
            }
        }
        #endregion

        #region 查询所有用户
        public void getusers()
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM users";
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

        #region  删除账号

        public void deluser(string userid)
        {

            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "delete FROM users where userid=  '" + userid + "' ";
            MySqlCommand cmd = new MySqlCommand(query, mycon);
            int count = cmd.ExecuteNonQuery();
            if (count>0)
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

        #region 查询code
        public void getcodes(string username,string code,string xm_code)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM codes where username=  '" + username + "' ";
            if (code!=null)
            {
                if (code.Trim() != "")
                {
                    query = "SELECT * FROM codes where code=  '" + code + "' ";
                }
            }

            if (xm_code!=null)
            {
                if(xm_code.Trim() != "")
                {
                    query = "SELECT * FROM codes where xm_code=  '" + xm_code + "' ";
                }
               
            }

          
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

        #region 确认code
        public void querencode(string code)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM codes where code=  '" + code + "' ";
          
            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();  
                string date = reader["date"].ToString().Trim();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                string json = JsonConvert.SerializeObject(dataTable);

                Response.Write(json);

                reader.Close();
                mycon.Close();

                if (date.Trim()=="")
                {
                    MySqlConnection con = new MySqlConnection(constr);
                    con.Open();
                    string sql = "update codes set date='" + DateTime.Now.ToString("yyyy-MM-dd") + "',time='" + DateTime.Now.ToString("HH:mm:ss") + "' where code=  '" + code + "' ";
                    MySqlCommand cmd= new MySqlCommand(query, con);
                    MySqlDataReader re = cmd.ExecuteReader();
   
                    re.Close();
                   con.Close();
                }
              
              

            }
            else
            {

                mycon.Close();
                reader.Close();

            }

        }
        #endregion

        #region 修改code
        public void xiugaicode(string code)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM codes where code=  '" + code + "' ";

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

        #region 获取随机数
        public string getsuijizimushuzi()
        {
            string zimu = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";

            Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
            string value = "";
            for (int i = 0; i <11 ; i++)
            {
                int suijizimu = rd.Next(0, 33);
                value = value + zimu[suijizimu];
            }

            return value;
        }
        #endregion

        #region  添加券码

        public void addcodes(string codes, string username, string xm_code, string xm_name,string acount)
        {
            //INSERT INTO datas (code,username,xm_code,xm_name,date,time)VALUES('" + code + " ', '" + username + " ', '" + xm_code + " ','" + xm_name + " ','" + DateTime.Now.ToString("yyyy-MM-dd") + " ','" + DateTime.Now.ToString("HH:mm:ss") + " ')
            MatchCollection codess = Regex.Matches(codes, @"""CODE"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                //string sql = "INSERT INTO codes VALUES (1006, 'H', 'S.', 'T', 'S.', 'S.', 'S.'),(1007, 'J', 'C', 'O', 'S.', 'S.', 'S.'),(1008, 'B', NULL, 'E', 'S.', 'S.', 'S.'),(1014, 'N', NULL, 'A', 'S.', 'S.', 'S.'); ";

                StringBuilder sb = new StringBuilder(); 
               

                for (int i = 0; i < codess.Count; i++)
                {
                    sb.Append("('"+codess[i].Groups[1].Value+ "','" + username + "','" + xm_code + "','" + xm_name + "','',''  ),");
                }

                for (int i = 0; i <Convert.ToInt32(acount); i++)
                {
                    string code = getsuijizimushuzi();
                    sb.Append("('" + code + "','" + username + "','" + xm_code + "','" + xm_name + "','',''),");
                }


                string sql = "INSERT INTO codes VALUES "+sb.ToString().Remove(sb.ToString().Length-1,1) +"; ";

               
              
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
    }
}