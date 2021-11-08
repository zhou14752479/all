using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 车辆采样消杀登记
{
    class function
    {
        string path = AppDomain.CurrentDomain.BaseDirectory;
       

      
        /// <summary>
        /// 查询数据库
        /// </summary>
        public DataTable chaxundata(string sql)
        {
            try
            {
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\cardata.db");
                mycon.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(sql, mycon);
                DataTable dt = new DataTable();
                mAdapter.Fill(dt);
                mycon.Close();
                return dt;

            }
            catch (SQLiteException ex)
            {
               
                return null;


            }

        }

        /// <summary>
        /// 查询字段
        /// </summary>
        public string chaxunvalue(string sql)
        {
            try
            {
                string value = "";
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\cardata.db"))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = string.Format(sql);
                        using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                value = dr["body"].ToString();

                            }
                        }
                    }
                    con.Close();
                }

                return value;

            }
            catch (SQLiteException ex)
            {
               
                return null;


            }

        }



        /// <summary>
        /// 执行SQL
        /// </summary>
        public bool SQL(string sql)
        {
            try
            {

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\cardata.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                int status = cmd.ExecuteNonQuery();  //执行sql语句
                if (status > 0)
                {
                    return true;

                }

                mycon.Close();
                return false;
            }
            catch (Exception)
            {


                return false;
            }

        }



        /// <summary>
        /// 插入数据库
        /// </summary>
        public bool insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\cardata.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                int status = cmd.ExecuteNonQuery();  //执行sql语句
                if (status > 0)
                {
                    return true;
                   
                }

                mycon.Close();
                return false;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());
                return false;

            }

        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "sid=64a4a095f95b472f56b387f81def9f98;USER_FLAG_CHECK=9416a352d1e261a30bc190a337c2a96f;appkey=0a0834aee67bcee63b8a39435300636d;appid=wx862d8e26109609cb;mpChannelId=388635;openid=odI0R5S8UPqCxO5UtR6u6V76SfKQ;wxclient=gxhwx;ie_ai=1;";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://servicewechat.com/wx91d27dbf599dff74/534/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion


    }
}
