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
using Tea;

namespace 高德地图
{
    public partial class main : System.Web.UI.Page
    {
        public static string constr = "Host =localhost;Database=ditu;Username=root;Password=root";
        string my_yzm = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sendyzm("17606117606");
            string mobile = Request["mobile"];
            string method = Request["method"];
            string username = Request["username"];
            string password = Request["password"];

            string taskname = Request["taskname"];
            string keyword = Request["keyword"];
            string city= Request["city"];
            string userid = Request["userid"];
            string taskid = Request["taskid"];
            string yzm = Request["yzm"];

            if (method== "register")
            {
                register(username,password,yzm);
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

            if (method == "sendyzm")
            {
                sendyzm(mobile);
            }
        }


        #region  注册

        public string register(string username, string password,string yzm)
        {

            try
            {
                if(yzm.Trim()!= my_yzm.Trim())
                {
                    Response.Write("验证码错误");
                }


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



        #region  阿里云短信验证码

        public static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient()
        {



            // 工程代码泄露可能会导致 AccessKey 泄露，并威胁账号下所有资源的安全性。以下代码示例仅供参考。
            // 建议使用更安全的 STS 方式，更多鉴权访问方式请参见：https://help.aliyun.com/document_detail/378671.html。
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                AccessKeyId = Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_ID"),     //Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_ID"),
                                                                                                     // 必填，请确保代码运行环境设置了环境变量 ALIBABA_CLOUD_ACCESS_KEY_SECRET。
                AccessKeySecret = Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_SECRET"),           //Environment.GetEnvironmentVariable("ALIBABA_CLOUD_ACCESS_KEY_SECRET"),


            };
            // Endpoint 请参考 https://api.aliyun.com/product/Dysmsapi
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
        }


        public void sendyzm(string mobile)
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 10000);
            AlibabaCloud.SDK.Dysmsapi20170525.Client client = CreateClient();
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                SignName = "景澜软件",
                PhoneNumbers = mobile,
                TemplateCode = "SMS_278110432",
               
                TemplateParam = "{\"code\":\""+ randomNumber + "\"}",
            };
            AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
            try
            {
                // 复制代码运行请自行打印 API 的返回值
                client.SendSmsWithOptions(sendSmsRequest, runtime);
            }
            catch (TeaException error)
            {
                // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
                // 错误 message
                Response.Write(error.Message);
                // 诊断地址
                Response.Write(error.Data["Recommend"]);
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            }
        }
        #endregion

    }
}