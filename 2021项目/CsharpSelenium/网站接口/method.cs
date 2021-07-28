using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using myDLL;
using System.Data;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace 网站接口
{
    class method
    {

        //string constr = "Host =121.40.209.61;Database=siyi_data;Username=root;Password=c#kaifa6668.";
        string constr = "Host=localhost;Database=siyi_data;Username=root;Password=c#kaifa6668.";
        myDLL.method md = new myDLL.method();

        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }
        #region 网站会员
        public string vip()
        {

            try
            {

                MySqlDataAdapter sda = new MySqlDataAdapter("Select * From vip", constr);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class"); //表名字随便起，第0个就是Ds.Tables[0]
                string json = JsonConvert.SerializeObject(Ds.Tables[0], Formatting.Indented);

                if (json != "")
                {

                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }

                else
                {
                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion
        string COOKIE = "";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {


            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);

                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
               //MessageBox.Show("ex"+ex.ToString()) ;
                return "";

            }

        }
        #endregion

        #region  获取经纬度
        public ArrayList getlat(string city)
        {
            try
            {
                ArrayList areas = new ArrayList();
                string url = "http://www.jsons.cn/lngcode/?keyword=" + city + "&txtflag=0";
                string html = GetUrl(url, "utf-8");
              
                Match ahtml = Regex.Match(html, @"<table class=""table table-bordered table-hover"">([\s\S]*?)</table>");
               
                MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");
               
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].Groups[1].Value.Contains("1"))
                    {
                        areas.Add(values[i].Groups[1].Value.Replace("，", "%2C").Trim());
                    }
                }
                return areas;
            }
            catch (Exception ex)
            {
               
                return null;
                
            }
           
        }
        #endregion
       
        #region  入库Sql

        public bool insertsql(string sql)
        {
            try
            {
               
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();
                    return true;
                }
                else
                {
                    mycon.Close();
                    return false;
                }
               

            }

            catch (System.Exception ex)
            {
               // MessageBox.Show(ex.ToString());
                return false;

            }
        }
        #endregion

        #region  查找Sql

        public string selectsql(string sql,string key)
        {

            try
            {
             
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string citypinyin = reader[key].ToString().Trim();
                    mycon.Close();
                    reader.Close();
                    return citypinyin;
                }
                else
                {
                    mycon.Close();
                    reader.Close();
                    return "useridnull";
                }

             


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region  查找文章

        public string selectarticle(string sql)
        {

            try
            {

                MySqlDataAdapter sda = new MySqlDataAdapter(sql, constr);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class"); //表名字随便起，第0个就是Ds.Tables[0]
                string json = JsonConvert.SerializeObject(Ds.Tables[0], Formatting.Indented);

                if (json != "")
                {

                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }
                else
                {
                    return "articlesnull";
                }




            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region 数据库转datatable
        public DataTable getdatatable(string sql)
        {
            DataTable dt;
            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, constr);
                sda.SelectCommand.CommandTimeout = 6000; //单位秒
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class");

               dt = Ds.Tables["T_Class"];
                return dt;
            }
            catch (Exception ex)
            {
               
                return null;
            }
           
        }

        #endregion

        #region  查找所有任务

        public string selecttask(string userid)
        {

            try
            {

                MySqlDataAdapter sda = new MySqlDataAdapter("Select * From tasks Where userid='" + userid + "' ", constr);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class"); //表名字随便起，第0个就是Ds.Tables[0]
                string json = JsonConvert.SerializeObject(Ds.Tables[0], Formatting.Indented);
               
                if (json!="")
                {
                       
                    return "{ \"count\":"+ Ds.Tables[0].Rows.Count+ ",\"status\":1,\"data\":" + json+"}";
                }
                else
                {
                    return "useridnull";
                }




            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region 地图主程序
        public void Amap(object o)
        {
           

            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string userid = text[0]; 
                string taskid= text[1];
            string city= text[2];
            string keyword= text[3];

          
            insertsql("UPDATE tasks SET status='运行中' where userid= '" + userid + " ' and taskid='" + taskid + " ' ");
            try
            {
                ArrayList areaLats = getlat(city);
             
                int page = 1;
                foreach (string lat in areaLats)
                {
                    for (int i = 1; i < 101; i++)
                    {


                        string url = "https://restapi.amap.com/v3/place/around?appname=1e3bb24ab8f75ba78a7cf8a9cc4734c6&key=1e3bb24ab8f75ba78a7cf8a9cc4734c6&keywords=" +keyword + "&location=" + lat + "&logversion=2.0&page=" + i + "&platform=WXJS&s=rsx&sdkversion=1.2.0";
                        string html =GetUrl(url, "utf-8");
                     
                        MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                       
                        if (names.Count == 0)
                            continue;
                        Thread.Sleep(1000);
                        string sql = "INSERT INTO datas (page,json,userid,taskid)VALUES('" +page + " ', '" + html + " ', '" + userid+ " ', '" + taskid + " ')";
                        insertsql(sql);
                        page = page + 1;
                    }
                }

                insertsql("UPDATE tasks SET status='已完成' where userid= '" + userid + " ' and taskid='" + taskid + " ' ");
                // return "{\"status\":0,\"msg\":\"正在后台抓取，请稍后查看\"}";
            }
            catch (Exception)
            {
              
                // return "{\"status\":0,\"msg\":\"服务异常,请联系管理员\"}";
            }
        }


        #endregion

        #region 获取数据
        public string getdata(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string userid = text[0];
            string taskid = text[1];
            string page = text[2];
            try
            {
                if (taskid == "")
                {
                    string sql = "select json from datas where userid='" + userid + "' AND page='" + page + "' ";
                    string json = selectsql(sql, "json");
                    return json;
                }
                else
                {

                    string sql = "select json from datas where userid='" + userid + "' AND taskid='" + taskid + "' AND page='" + page + "' ";
                    string json = selectsql(sql, "json");
                    return json;
                }
              
            }
            catch (Exception)
            {

                return null ;
            }
        }
        #endregion
      
        #region 注册
        public string register(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string username = System.Web.HttpUtility.UrlDecode(text[0]);
            string password = text[1];
            string passwordmd5 = myDLL.method.GetMD5(password);
            string timestamp = myDLL.method.GetTimeStamp();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");
            try
            {
                string sql = "INSERT INTO users (username,password,registertimestamp,registertime,viptime)VALUES('" + username + " ', '" + password + " ', '" + timestamp + " ','" + time + " ', '" + timestamp + " ')";
                bool status = insertsql(sql);
                if (status == true)
                {
                    return "{\"status\":1,\"msg\":\"注册成功\"}";
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"注册异常，请联系客服\"}";
                }
               
            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"注册异常，请联系客服\"}";
            }
        }
        #endregion




        #region 美团注册
        public string mtregister(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string username = text[0];
            string password = text[1];
            string time = DateTime.Now.AddMinutes(10).ToString("yyyy-MM-dd HH:MM:ss");
            try
            {
                string sql = "INSERT INTO mtusers (username,password,viptime,registertime)VALUES('" + username + " ', '" + password + " ','" + time + " ','" + time + " ')";
                bool status = insertsql(sql);
                if (status == true)
                {
                    return "{\"status\":1,\"msg\":\"注册成功\"}";
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"注册失败，请联系客服\"}";
                }

            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"注册失败，请联系客服\"}";
            }
        }
        #endregion
        #region 美团登录
        public string mtlogin(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string username = text[0];
            string password = text[1];
         
            try
            {
                string sql = "select * from mtusers where username='" + username + "' ";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string pass = reader["password"].ToString().Trim();
                    string viptime = reader["viptime"].ToString().Trim();
                    string registertime = reader["registertime"].ToString().Trim();
               


                    if (pass == password)
                    {
                        mycon.Close();
                        reader.Close();
                        return "{\"status\":1,\"msg\":\"登录成功\",\"username\":\"" + username + "\",\"registertime\":\"" + registertime + "\",\"isvip\":\"" + viptime + "\"}";
                    }
                    else
                    {
                        mycon.Close();
                        reader.Close();
                        return "{\"status\":0,\"msg\":\"登录失败，请联系客服\"}";
                    }

                }
                else
                {
                    mycon.Close();
                    reader.Close();
                    return "{\"status\":0,\"msg\":\"登录失败，请联系客服\"}";
                }





            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"登录异常，请联系客服\"}";
            }
        }
        #endregion
        #region 美团会员充值
        public string mtbuy(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string username = text[0];
            string days = text[1];

            try
            {

                string sql = "select * from mtusers where username='" + username + "' ";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                 
                    string viptime = reader["viptime"].ToString().Trim();

                    string paytime = Convert.ToDateTime(viptime).AddDays(Convert.ToInt32(days)).ToString("yyyy-MM-dd HH:MM:ss");
                    string sql2 = "update mtusers SET viptime='" +paytime + "' where username='" + username + "' ";
                    bool status = insertsql(sql2);
                    if (status == true)
                    {
                        mycon.Close();
                        reader.Close();
                        return "{\"status\":1,\"msg\":\"充值成功\"}";
                    }
                    else
                    {
                        mycon.Close();
                        reader.Close();
                        return "{\"status\":0,\"msg\":\"充值失败，请联系客服\"}";
                    }
                                         
                }
                else
                {
                    mycon.Close();
                    reader.Close();
                    return "{\"status\":0,\"msg\":\"充值失败，请联系客服\"}";
                }

            }
            catch (Exception ex)
            {
               
                return "{\"status\":0,\"msg\":\"充值异常，请联系客服\"}";
            }
        }
        #endregion
        #region 美团查询所有用户
        public string mtall()
        {

            try
            {

                MySqlDataAdapter sda = new MySqlDataAdapter("Select * From mtusers", constr);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class"); //表名字随便起，第0个就是Ds.Tables[0]
                string json = JsonConvert.SerializeObject(Ds.Tables[0], Formatting.Indented);

                if (json != "")
                {

                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }

                else
                {
                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion
        #region 美团删除用户
        public string mtdel(object o)
        {
            string userid = o.ToString().Trim();
            try
            {
                string sql = "DELETE FROM mtusers where id ='" + userid + "'";
         
                bool status = insertsql(sql);
                if (status == true)
                {
                    return "{\"status\":1,\"msg\":\"删除成功\"}";
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"删除失败，请联系客服\"}";
                }

            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"删除异常，请联系客服\"}";
            }
        }
        #endregion
        #region 美团会员检测
        public bool mtjiance(string username)
        {

            try
            {
                string sql = "select * from mtusers where username='" + username + "' ";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {

                    string viptime = reader["viptime"].ToString().Trim();
                    if (Convert.ToDateTime(viptime) > DateTime.Now)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
                else
                {
                    mycon.Close();
                    reader.Close();
                    return false; //没有此mac
                }


            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
        #region  美团主程序
        public string mt_getdata(object o)
        {
            try
            {
                string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                string username = text[0];
                string cityid = text[1];
                string areaid = text[2];
                string cateid = text[3];
                string page = text[4];
                if (mtjiance(username))
                {
                    string Url = "https://m.dianping.com/mtbeauty/index/ajax/shoplist?token=&cityid=" + cityid + "&cateid=22&categoryids=" + cateid + "&lat=33.94114303588867&lng=118.2479019165039&userid=&uuid=&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false&start=" + page + "&limit=100&areaid=" + areaid + "&distance=&subwaylineid=&subwaystationid=&sort=2";

                    string html = GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()
                    return html;
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"账号已过期，请充值\"}"; ;
                }

               
            }

            catch (Exception ex)
            {

                return "{\"status\":0,\"msg\":\""+ ex.ToString()+ "\"}"; ;
            }
                
                
        }

        #endregion



        #region 邮箱简历获取
        public string mtjianliall()
        {

            try
            {

                MySqlDataAdapter sda = new MySqlDataAdapter("Select * From jianlis", constr);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class"); //表名字随便起，第0个就是Ds.Tables[0]
                string json = JsonConvert.SerializeObject(Ds.Tables[0], Formatting.Indented);

                if (json != "")
                {

                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }

                else
                {
                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion
        #region 邮箱简历插入
        public string mtjianliregister(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
           
            try
            {
                string sql = "INSERT INTO jianlis (name,sex,age,birthday,phone,area,job,time,username)VALUES('" + text[0]+ " ', '" +text[1] + " ','" + text[2] + " ','" + text[3] + " ','" + text[4] + " ','" + text[5] + " ','" + text[6] + " ','" + text[7] + " ','" + text[8] + " ')";
                bool status = insertsql(sql);
                if (status == true)
                {
                    return "{\"status\":1,\"msg\":\"注册成功\"}";
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"注册失败，请联系客服\"}";
                }

            }
            catch (Exception )
            {
               

                return "{\"status\":0,\"msg\":\"注册失败，请联系客服\"}";
            }
        }
        #endregion




        

        #region 登录
        public string login(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string username = text[0];
            string password = text[1];
            string time = myDLL.method.GetTimeStamp();
            try
            {
                string sql = "select * from users where username='" + username + "' ";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string pass = reader["password"].ToString().Trim();
                    string id = reader["id"].ToString().Trim();
                    string isvip = reader["isvip"].ToString().Trim();

                    if (pass == password)
                    {
                        mycon.Close();
                        reader.Close();
                        return "{\"status\":1,\"msg\":\"true\",\"userid\":\"" + id + "\",\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"isvip\":" + isvip + "}";
                    }
                    else
                    {
                        mycon.Close();
                        reader.Close();
                        return "{\"status\":0,\"msg\":\"登录失败，请联系客服\"}";
                    }

                }
                else
                {
                    mycon.Close();
                    reader.Close();
                    return "{\"status\":0,\"msg\":\"登录失败，请联系客服\"}";
                }
               




            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"登录异常，请联系客服\"}";
            }
        }
        #endregion

        #region 任务添加
        public string task_add(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string userid = System.Web.HttpUtility.UrlDecode(text[0]);
            string taskname = System.Web.HttpUtility.UrlDecode(text[1]);
            string city = System.Web.HttpUtility.UrlDecode(text[2]);
            string keyword = System.Web.HttpUtility.UrlDecode(text[3]);
            string time = myDLL.method.GetTimeStamp();
            try
            {
                string sql = "INSERT INTO tasks (userid,taskname,createtime,city,keyword)VALUES('" + userid+ " ', '" + taskname + " ', '" + time + " ', '" + city + " ', '" + keyword + " ')";
                bool status = insertsql(sql);
                if (status == true)
                {
                    return "{\"status\":1,\"msg\":\"添加任务成功\"}";
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"添加任务失败，请联系客服\"}";
                }

            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"添加任务异常，请联系客服\"}";
            }
        }
        #endregion

        #region 任务删除
        public string task_del(object o)
        {
            string[] text = o.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            string userid = text[0].Trim();
            string taskid = text[1].Trim();
         
            try
            {
                string sql = "DELETE FROM tasks where taskid in (" + taskid + ") && userid='" + userid + "'";
                string sql2 = "DELETE FROM datas where taskid in (" + taskid + ") && userid='" + userid + "'";
                bool status = insertsql(sql);
                bool status2 = insertsql(sql2);
                if (status == true)
                {
                    return "{\"status\":1,\"msg\":\"删除任务成功\"}";
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"删除任务失败，请联系客服\"}";
                }

            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"删除任务异常，请联系客服\"}";
            }
        }
        #endregion

        #region 文件删除
        public string filedata_del(object o)
        {
            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string userid = text[0].Trim();
            try
            {
                string sql = "DELETE FROM downloadfiles where userid='" + userid + "'";

                bool status = insertsql(sql);
                if (status == true)
                {
                    return "{\"status\":1,\"msg\":\"删除文件数据成功\"}";
                }
                else
                {
                    return "{\"status\":0,\"msg\":\"删除文件数据失败，请联系客服\"}";
                }

            }
            catch (Exception)
            {

                return "{\"status\":0,\"msg\":\"删除文件数据异常，请联系客服\"}";
            }
        }
        #endregion

        #region 任务获取
        public string task_get(object o)
        {
           
            string userid = o.ToString();
           
            try
            {
               
                string json = selecttask(userid);
                return json;
            }
            catch (Exception)
            {

                return null;
            }
        }
        #endregion

        #region 导出表格
        public void exportExcel(object o)
        {
            DataTable table = new DataTable();
            string[] columns = { "商家名称", "类型", "地址", "联系方式", "省份", "城市","地区" };
            foreach (string column in columns)
            {

                table.Columns.Add(column, Type.GetType("System.String"));

            }


            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string userid = text[0].Trim();
            string taskid =text[1].Trim();
            string taskname = text[2].Trim();  
            string fileName = text[3];
            string sqlfilename = fileName.Replace("C:/phpStudy/PHPTutorial/WWW/excel/", "");
         
            string sql2 = "INSERT INTO downloadfiles (userid,filename,createtime)VALUES('" + userid + " ', '" + sqlfilename + " ','" + DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + " ')";
            insertsql(sql2);
            try
            {
             
               
                string sql = "Select json from datas where taskid='" + taskid + "'&& userid='" + userid + "' ";
                // string sql = "select json from datas where taskid='" + taskid + "' ";
               
                DataTable dt = getdatatable(sql);
                if (dt != null)
                {
                   
                    foreach (DataRow dr in dt.Rows)
                    {
                        MatchCollection names = Regex.Matches(dr[0].ToString(), @"""name"":([\s\S]*?),");
                        MatchCollection types = Regex.Matches(dr[0].ToString(), @"""type"":([\s\S]*?),");
                        MatchCollection address= Regex.Matches(dr[0].ToString(), @"""address"":([\s\S]*?),");
                        MatchCollection tels = Regex.Matches(dr[0].ToString(), @"""tel"":([\s\S]*?),");
                        MatchCollection pnames = Regex.Matches(dr[0].ToString(), @"""pname"":([\s\S]*?),");
                        MatchCollection citynames = Regex.Matches(dr[0].ToString(), @"""cityname"":""([\s\S]*?),");
                        MatchCollection adnames = Regex.Matches(dr[0].ToString(), @"""adname"":([\s\S]*?),");
                    
                     
                        for (int i = 0; i < names.Count; i++)
                        {
                            try
                            {
                                DataRow newRow = table.NewRow();
                                newRow["商家名称"] = names[i].Groups[1].Value.Replace("\"","");
                                newRow["类型"] = types[i].Groups[1].Value.Replace("\"", "");
                                newRow["地址"] = address[i].Groups[1].Value.Replace("\"", "");
                                newRow["联系方式"] = tels[i].Groups[1].Value.Replace("\"", "");
                                newRow["省份"] = pnames[i].Groups[1].Value.Replace("\"", "");
                                newRow["城市"] = citynames[i].Groups[1].Value.Replace("\"", "");
                                newRow["地区"] = adnames[i].Groups[1].Value.Replace("\"", "");
                                table.Rows.Add(newRow);

                            }
                            catch (Exception ex)
                            {
                               
                                continue;
                            }
                          
                        }
                       

                    }
                    myDLL.method.DataTableToExcelName(table,fileName,true);
                 

                    // return "{\"status\":1,\"msg\":\"生成表格成功\",\"filename\":\"" + fileName.Replace("C:/phpStudy/PHPTutorial/WWW","") + "\"}";
                }

                

              
                else
                {
                  
                    // return "{\"status\":0,\"msg\":\"生成表格失败\"}";
                }

                insertsql("UPDATE downloadfiles SET msg='点击下载',btnmsg='mini' where userid= '" + userid + " ' and filename='" + sqlfilename + " ' ");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
              //  return "{\"status\":0,\"msg\":\"生成表格异常，请联系客服\"}";
            }
        }
        #endregion


        #region 文章获取
        public string articleslist_get(object o)
        {

            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string page = text[0];

            try
            {

                if (page != "")
                {
                    int start = (Convert.ToInt32(page) * 10) - 10;
                    string sql = "select * from  articles limit " + start + ",10 ";
                  
                    string json = selectarticle(sql);
                    return json;
                }
                else
                {
                    
                    string sql = "select * from  articles limit 0,10 ";
                    string json = selectarticle(sql);
                    return json;
                }
                
            }
            catch (Exception)
            {

                return null;
            }
        }
        #endregion

        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }

        #region 下载文件获取
        public string downloadfiles_get(object o)
        {

            string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            string userid = text[0];

            try
            {

                MySqlDataAdapter sda = new MySqlDataAdapter("Select * From downloadfiles Where userid='" + userid + "' ", constr);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class"); //表名字随便起，第0个就是Ds.Tables[0]
                string json = JsonConvert.SerializeObject(Ds.Tables[0], Formatting.Indented);

                if (json != "")
                {

                    return "{ \"count\":" + Ds.Tables[0].Rows.Count + ",\"status\":1,\"data\":" + json + "}";
                }
                else
                {
                    return "useridnull";
                }

            }
            catch (Exception)
            {

                return null;
            }
        }
        #endregion

        #region 生成静态文章
        public string createHtml()
        {

            try
            {

                string sql = "select * from articles ";
                DataTable dt = getdatatable(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string id= dt.Rows[i][0].ToString();
                    string title= dt.Rows[i][1].ToString();
                    string body = dt.Rows[i][2].ToString();
                    string time = ConvertStringToDateTime(dt.Rows[i][3].ToString()).ToString();
                   
                    string sss = File.ReadAllText(@"C:\phpStudy\PHPTutorial\WWW\articles\模板.html", Encoding.GetEncoding("utf-8"));
                    string stroutput = sss.Replace("{id}",id).Replace("{title}", title).Replace("{body}",body).Replace("{time}", time);

                    File.WriteAllText(@"C:\phpstudy\PHPTutorial\WWW\articles\"+id+".html", stroutput, Encoding.GetEncoding("utf-8"));

                }

                return "{\"status\":0,\"msg\":\"生成html成功\"}";
            }
            catch (Exception)
            {

                return null;
            }
        }
        #endregion


        #region  获取32位MD5加密
        public static string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion

        #region GET请求
        public static string meituan_GetUrl(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.13(0x17000d2a) NetType/4G Language/zh_CN";
                WebHeaderCollection headers = request.Headers;
                headers.Add("uuid: E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576");
                headers.Add("open_id: oJVP50IRqKIIshugSqrvYE3OHJKQ");
                headers.Add("token: Vteo9CkJqIGMe30FC3iuvnvTr2YAAAAAygoAAMPHPyLNO16W1eYLn1hWsLhD40r-KnDdB70rrl9LN9OHUfVBGbTDt4PCDHH72xKkDA");

                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/328/page-frame.html";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                return ex.ToString();



            }
            return "";
        }
        #endregion

        

        #region  旺旺查询

        public  string getSetCookie(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.AllowAutoRedirect = true;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                string content = response.GetResponseHeader("Set-Cookie"); ;
                return content;
            }
            catch (Exception ex)
            {
               
                return "";
                
            }
          


        }
        baiduOCR ocr = new baiduOCR();
        public string getwangwang(string wangwang)
        {

           
            
            try
            {
                if (wangwang != "")
                {
                    string url = "http://139.159.141.200/app/superscanPH/opQuery.jsp?m=queryAliim&aliim=" + wangwang;
                    string html= GetUrl(url,"utf-8").Trim();
                    if (!html.Contains("baseImg"))
                    {
                        COOKIE = getSetCookie("http://139.159.141.200/app/superscanPH/loginPH.jsp?m=login&username=18588777745&password=MUSHANG123&parcame=ajax");
                        html = GetUrl(url, "utf-8").Trim();
                    }

                    //if (html== "")
                    //{
                    //    //判断主用IP是否可用，切换备用IP
                    //    url = "http://106.12.189.59/app/superscanPH/opQuery.jsp?m=queryAliim&aliim=" + wangwang;
                    //    html = GetUrl(url, "utf-8");

                    //}



                    string baseimgUrl = Regex.Match(html, @"baseImg"":""([\s\S]*?)""").Groups[1].Value;
                    string downimgUrl = Regex.Match(html, @"downImg"":""([\s\S]*?)""").Groups[1].Value;
                    string result = ocr.shibie(baseimgUrl);


                    string sex = "男";
                    string downNum = "0";
                    if (result.Contains("女"))
                    {
                        sex = "女";
                    }
                    string buyerCre = Regex.Match(result, @"买家信誉:([\s\S]*?)""").Groups[1].Value;
                    string sellerCredit = Regex.Match(result, @"商家信誉:([\s\S]*?)""").Groups[1].Value;
                    string registDay = Regex.Match(result, @"淘龄([\s\S]*?)天").Groups[1].Value.Replace("\"","").Replace("{", "").Replace("}", "").Replace(":", "").Replace("words","").Replace(",", "").Replace("约", "");

                    string json = "{\"result\":\"正常\",\"sex\":\"" + sex + "\",\"buyerCre\":\"" + buyerCre + "\",\"sellerCredit\":\"" + sellerCredit + "\",\"downNum\":" + downNum + ",\"registDay\":\"" + registDay + "\"}";
                    return json;

                }
                else
                {
                    return "旺旺为空";
                }


            }
            catch (Exception)
            {

                return null;
            }
        }


        #endregion

        
    }
}
