﻿using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP药价中台
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
                string username= Request["username"];
                string password = Request["password"];
                string yaopinname = Request["yaopinname"];

                if (method=="uploadprice")
                {
                    run(data,username);
                }

                if (method == "getprice")
                {
                   getprice(username);
                }
                if (method == "deleteprice")
                {
                    deleteprice(username);
                }
                if (method == "getusers")
                {
                    getusers();
                }
                if (method == "adduser")
                {
                    adduser(username,password);
                }
                if (method == "downexcel")
                {
                    downexcel(username);
                }
                if (method == "getoneprice")
                {
                    getoneprice(yaopinname);
                }

            }








        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

       public static string constr = "Host =localhost;Database=yaopin;Username=root;Password=root";

        #region  插入数据库

        public string insertdata(string wenhao, string name, string guige, string price, string yfprice, string jdprice,string username)
        {

            try
            {



                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO datas (wenhao,name,guige,price,yfprice,jdprice,time,username)VALUES('" + wenhao + " ', '" + name + " ', '" + guige + " ', '" + price + " ', '" + yfprice + " ', '" + jdprice + " ', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ', '" + username + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();
                    return "插入成功";

                }
                else
                {
                    return "插入失败";
                }


            }

            catch (System.Exception ex)
            {
                return (ex.ToString());
            }
        }
        #endregion


        #region 查询数据库价格
        public  void getprice(string username)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM datas where username=  '" + username + "' ";
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

        #region 导出表格
        public void downexcel(string username)
        {
            string filename = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/excel/" + username + ".xlsx";

            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM datas where username=  '" + username + "' ";
            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                myDLL.method.DataTableToExcelName(dataTable, filename, true);
                mycon.Close();
                reader.Close();

                 Response.Write(@"<script>alert('生成表格成功！');window.location.href='\\excel\\"+username+".xlsx'</script>");

            }
            else
            {
                Response.Write("生成表格失败");
                mycon.Close();
                reader.Close();

            }

        }
        #endregion

        #region 清空数据库价格
        public void deleteprice(string username)
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "delete from datas where username=  '" + username + "' ";
            MySqlCommand command = new MySqlCommand(query, mycon);
            int count = command.ExecuteNonQuery();
            if (count>0)
            {
               
                Response.Write("清空成功");
                mycon.Close();
              
               
            }
            else
            {
                Response.Write("清空失败");
                mycon.Close();
                
               
            }

        }
        #endregion

        #region 药房网价格
        public string yaofangwang(string wenhao)
        {
            try
            {
                string url = "https://pub.yaofangwang.com/4000/4000/0/guest.medicine.getSearchPageData?pageIndex=1&pageSize=20&keywords=" + wenhao + "&orderBy=&storeid=&searcha_type=&__client=app_wx&app_version=5.0.11&osVersion=miniapp&deviceName=microsoft&os=windows&version=3.9.6&market=microsoft&networkType=true&lat=33.96271241099666&lng=118.24239343364114&user_city_name=%E5%AE%BF%E8%BF%81%E5%B8%82&user_region_id=1739&idfa=wx_0a1gExFa1OBPIF0Iz9Ha1mpmKw2gExFI&device_no=wx_0a1gExFa1OBPIF0Iz9Ha1mpmKw2gExFI";
                string html = GetUrl(url, "utf-8");

                MatchCollection stores = Regex.Matches(html, @"""store_title"":""([\s\S]*?)""");
                MatchCollection store_prices = Regex.Matches(html, @"""real_price"":([\s\S]*?),");

                string name = Regex.Match(html, @"""medicine_name"":""([\s\S]*?)""").Groups[1].Value;
                string guige = Regex.Match(html, @"""standard"":""([\s\S]*?)""").Groups[1].Value;
                //string changjia = Regex.Match(html, @"生产厂家：([\s\S]*?)""").Groups[1].Value;

                string guoyaozhunzi = Regex.Match(html, @"""authorizedCode"":""([\s\S]*?)""").Groups[1].Value;
                string price_min = Regex.Match(html, @"""price_min"":([\s\S]*?),").Groups[1].Value;
                return price_min;
            }
            catch (Exception)
            {

                return "药房网获取失败";
            }

        }


        #endregion


        #region 京东价格
        public string jd(string title)
        {
            try
            {
                string url = "https://search.jd.com/Search?keyword="+title+"&enc=utf-8&wq="+title+"&pvid=38423187e3834c9299643a8354764b6b";
                string html = GetUrl(url, "utf-8");


                string price_min = Regex.Match(html, @"prices:'([\s\S]*?),").Groups[1].Value;
                return price_min;
            }
            catch (Exception)
            {

                return "药房网获取失败";
            }

        }


        #endregion

        #region 获取用户列表
        public void getusers()
        {
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string query = "SELECT * FROM users ";
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

        #region  添加用户

        public string adduser( string username,string password)
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



        #region 获取单个价格
        public void getoneprice(string yaopinname)
        {
            try
            {
                string url = "https://pub.yaofangwang.com/4000/4000/0/guest.medicine.getSearchPageData?pageIndex=1&pageSize=20&keywords=" + yaopinname + "&orderBy=&storeid=&searcha_type=&__client=app_wx&app_version=5.0.11&osVersion=miniapp&deviceName=microsoft&os=windows&version=3.9.6&market=microsoft&networkType=true&lat=33.96271241099666&lng=118.24239343364114&user_city_name=%E5%AE%BF%E8%BF%81%E5%B8%82&user_region_id=1739&idfa=wx_0a1gExFa1OBPIF0Iz9Ha1mpmKw2gExFI&device_no=wx_0a1gExFa1OBPIF0Iz9Ha1mpmKw2gExFI";
                string html = GetUrl(url, "utf-8");


                MatchCollection stores = Regex.Matches(html, @"""store_title"":""([\s\S]*?)""");
                MatchCollection store_prices = Regex.Matches(html, @"""real_price"":([\s\S]*?),");

                MatchCollection name = Regex.Matches(html, @"""medicine_name"":""([\s\S]*?)""");
                MatchCollection guige = Regex.Matches(html, @"""standard"":""([\s\S]*?)""");
                MatchCollection changjia = Regex.Matches(html, @"""title"":""([\s\S]*?)""");

                MatchCollection guoyaozhunzi = Regex.Matches(html, @"""authorizedCode"":""([\s\S]*?)""");
                MatchCollection price_min = Regex.Matches(html, @"""price_min"":([\s\S]*?),");

                string json = "";

                string jdprice = "";
                for (int i = 0; i <name.Count ; i++)
                {
                   json =json+ "{\"wenhao\":\"" + guoyaozhunzi[i].Groups[1].Value + "\",\"name\":\"" + name[i].Groups[1].Value + "\", \"guige\":\"" + guige[i].Groups[1].Value + "\",\"changjia\":\"" + changjia[i].Groups[1].Value + "\",\"yfprice\":\"" + price_min[i].Groups[1].Value + "\",\"jdprice\":\"" + jdprice + "\",\"time\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}"+",";
                }
               
                
               Response.Write("["+json.Remove(json.Length-1,1)+"]");
            }
            catch (Exception)
            {

                Response.Write("{\"name\":\"获取失败\"}");
            }

        }


        #endregion

        public void run(string html,string username)
        {
          
            MatchCollection wenhao = Regex.Matches(html, @"""批准文号"":""([\s\S]*?)""");
            MatchCollection name = Regex.Matches(html, @"""药品名称"":""([\s\S]*?)""");
            MatchCollection guige = Regex.Matches(html, @"""规格"":""([\s\S]*?)""");
            MatchCollection price = Regex.Matches(html, @"""价格"":""([\s\S]*?)""");

            MatchCollection changjia = Regex.Matches(html, @"""生产厂家"":""([\s\S]*?)""");
            MatchCollection shengchan_time = Regex.Matches(html, @"""生产日期"":""([\s\S]*?)""");
            if (wenhao.Count==0)
            {

            }
            
            
            
            for (int i = 0; i < wenhao.Count; i++)
            {
                string yaofang_price = yaofangwang(wenhao[i].Groups[1].Value);
                string jd_price = jd(name[i].Groups[1].Value + guige[i].Groups[1].Value);
                insertdata(wenhao[i].Groups[1].Value,name[i].Groups[1].Value, guige[i].Groups[1].Value, price[i].Groups[1].Value, yaofang_price,jd_price,username);
                Response.Write(yaofang_price);
            }

           
        }















    }
}