using myDLL;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 高德地图
{
    public partial class main : System.Web.UI.Page
    {
        public static string constr = "Host =localhost;Database=ditu;Username=root;Password=root";
        protected void Page_Load(object sender, EventArgs e)
        {


            string method = Request["method"];
            string username = Request["username"];
            string password = Request["password"];

            string taskname = Request["taskname"];
            string keyword = Request["keyword"];
            string city= Request["city"];
            string userid = Request["userid"];
            string taskid = Request["taskid"];


            if (method== "register")
            {
                register(username,password);
            }
            if (method == "login")
            {
                login(username, password);
            }
            if (method == "addtask")
            {
                addtask(userid,taskname,keyword,city);
            }
            if (method == "gettask")
            {
                gettask(userid);
            }
            if (method == "starttask")
            {
                starttask(taskid,keyword,city);
            }
            if (method == "getdata")
            {
                getdata(taskid);
            }
            if(method== "createExcel")
            {
                createExcel(taskid);
            }
        }


        #region  注册

        public string register(string username, string password)
        {

            try
            {



                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO users (username,password,time)VALUES('" + username + " ', '" + password + " ', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();
                    return "{\"status\":\"1\"}";

                }
                else
                {
                    return "{\"status\":\"0\"}";
                }


            }

            catch (System.Exception ex)
            {
                return (ex.ToString());
            }
        }
        #endregion

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

                    Response.Write("{\"userid\":\"" + userid + "\",\"username\":\"" + username + "\",\"status\":1}");
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

        #region  插入数据

        public void insert(string name,string addr,string tel,string area,string taskid)
        {

            try
            {



                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO datas (name,addr,tel,area,taskid)VALUES('" + name + " ', '" + addr + " ', '" + tel + " ', '" + area + " ' ,'" +taskid+"')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();
                   // return "{\"status\":\"1\"}";

                }
                else
                {
                    //return "{\"status\":\"0\"}";
                }


            }

            catch (System.Exception ex)
            {
               // return (ex.ToString());
            }
        }
        #endregion


        #region  添加任务

        public void addtask(string userid, string taskname, string keyword,string city)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            string sql = "INSERT INTO task (userid,taskname,keyword,city,time,status)VALUES('" + userid + " ','" + taskname + " ', '" + keyword + " ', '" + city + " ', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ','0')";

            try
            {



                
                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


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
                Response.Write(ex.ToString()+"\r\n"+sql);
            }
        }
        #endregion

        #region 获取任务
        public void gettask(string userid)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM task where userid=  '" + userid + "' ORDER BY time desc";

          

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

        #region 采集程序
        public void starttask(string taskid, string keyword, string city)
        {
            // string url = "https://restapi.amap.com/v5/place/text?keywords=装饰公司&region=宿迁市&show_fields=business&page_size=25&page_num=1&key=adabc1b5b1dd1ab353f6fcb716adb40e";
            try
            {

                List<string>  keylist= new List<string>();  
                keylist.Add("7b3a5ec27d0b2ff9a6333bd75711a091");
                keylist.Add("0f23cb3ac24f5f809638313b5d23e6fa");
                keylist.Add("f3ebc214cc87d707a79772d61f1855c0");
                keylist.Add("3582c889a0f886d113336b20dd57da90");
                keylist.Add("adabc1b5b1dd1ab353f6fcb716adb40e");

                Response.Write("{\"status\":\"1\"}");
                for (int page = 1; page < 101; page++)
                {
                   
                    string key = keylist[0];

                    string url = "https://restapi.amap.com/v5/place/text?keywords=" + System.Web.HttpUtility.UrlEncode(keyword) + "&region=" + System.Web.HttpUtility.UrlEncode(city) + "&show_fields=business&page_size=25&page_num=" + page + "&key="+key;
                   
                    string html = myDLL.method.GetUrl(url, "utf-8");
                if(html.Contains("USER_DAILY_QUERY_OVER_LIMIT"))
                    {
                        key = keylist[1];
                        url = "https://restapi.amap.com/v5/place/text?keywords=" + System.Web.HttpUtility.UrlEncode(keyword) + "&region=" + System.Web.HttpUtility.UrlEncode(city) + "&show_fields=business&page_size=25&page_num=" + page + "&key=" + key;

                        html = myDLL.method.GetUrl(url, "utf-8");
                    }


                    MatchCollection parent = Regex.Matches(html, @"""parent""([\s\S]*?)location");

                    for (int i = 0; i < parent.Count; i++)
                    {
                        string name = Regex.Match(parent[i].Groups[1].Value, @"""name"":""([\s\S]*?)""").Groups[1].Value;
                        string addr = Regex.Match(parent[i].Groups[1].Value, @"""address"":""([\s\S]*?)""").Groups[1].Value;

                        string tel = Regex.Match(parent[i].Groups[1].Value, @"""tel"":""([\s\S]*?)""").Groups[1].Value;

                        insert(name, addr, tel, city,taskid);
                    }


                }

               
               
            }
            catch (Exception ex)
            {
                Response.Write("{\"status\":\"1\"}");
                //Response.Write(ex.ToString());
            }

           

        }
        #endregion


        #region 获取数据
        public void getdata(string taskid)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM datas where taskid=  '" + taskid + "' ";



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




        #region 生成表格
        public void createExcel(string taskid)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM datas where taskid=  '" + taskid + "' ";

            string currentFolderPath = HttpContext.Current.Request.PhysicalPath;

            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                string filename = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/admin/excel/" + taskid + ".xlsx";
                method.DataTableToExcelName(dataTable, filename, true);
               
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





    }
}