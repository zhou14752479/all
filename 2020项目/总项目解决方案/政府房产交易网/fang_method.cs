using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 政府房产交易网
{
    class fang_method
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";

        public delegate void GetLogs(string log);
        public event GetLogs getlogs;

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
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
                ex.ToString();

            }
            return "";
        }
        #endregion


        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "application/json";
                request.ContentLength = postData.Length;
               // request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        #region POST请求获取返回头
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlgethead(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                // request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
               string Location= response.GetResponseHeader("Location");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return Location;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        #region 追加写入csv


        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径bai</param>
        public void WriteCsv(DataTable dt, string dic, string csvname)
        {
            
            string fullPath = path+dic+"/" + csvname + ".csv";
            if (!Directory.Exists(path + dic + "/"))
            {
                Directory.CreateDirectory(path + dic + "/"); //创建文件夹
            }
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            string data = "";
           // 写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);


            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"')
                    || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                    {
                        str = string.Format("\"{0}\"", str);
                    }
                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
           // DialogResult result = MessageBox.Show("CSV文件保存成功！");

        }




        #endregion


        #region 烟台
        public System.Timers.Timer yantai_1_Timer = new System.Timers.Timer();

        public void yantai_1_stop()
        {

            yantai_1_Timer.Enabled = false;
            yantai_1_Timer.Elapsed -= new System.Timers.ElapsedEventHandler(yantai_1_OnTimedEvent);

        }
        public void yantai_1_start()
        {

            yantai_1_Timer.Interval = 1000;
            yantai_1_Timer.Enabled = true;
            yantai_1_Timer.Elapsed += new System.Timers.ElapsedEventHandler(yantai_1_OnTimedEvent);


        }
        DataTable table = new DataTable();
        private void yantai_1_OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            string now = DateTime.Now.ToString("HH:mm:ss");
            if (now == "10:20:00")
            {
                try
                {
                string[] columns = { "日期", "所在区", "住宅套数", "住宅面积", "总成交套数", "总成交面积" };
                foreach (string column in columns)
                {

                    table.Columns.Add(column, Type.GetType("System.String"));

                }

                string url = "http://www.ytfcjy.com/CMS/SPF/SPFIndex";
                    string html = GetUrl(url, "utf-8");
                    MatchCollection a1s = Regex.Matches(html, @"<td style=""text-align:center;"">([\s\S]*?)</td>");
                    MatchCollection a2s = Regex.Matches(html, @"<td style=""text-align:right;"">([\s\S]*?)</td>");
                   
                   

                    for (int i = 0; i < a1s.Count; i++)
                    {
                        DataRow newRow = table.NewRow();
                    newRow["日期"] = DateTime.Now.ToLongDateString();
                    newRow["所在区"] = a1s[i].Groups[1].Value;
                        newRow["住宅套数"] = a2s[4 * i].Groups[1].Value;
                        newRow["住宅面积"] = a2s[(4 * i) + 1].Groups[1].Value;
                        newRow["总成交套数"] = a2s[(4 * i) + 2].Groups[1].Value;
                        newRow["总成交面积"] = a2s[(4 * i) + 3].Groups[1].Value;
                    

                        table.Rows.Add(newRow);

                    }
                    WriteCsv(table,"烟台", "yantai_1");
                    getlogs( "烟台每日成交导出csv成功！");


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }

            else
            {
                getlogs(DateTime.Now.ToString("HH:mm:ss") + "烟台每日成交不在设定时间等待执行！");
            }

        }


        Thread yantai_2_thread;
        public void yantai_2_start()
        {
            if (yantai_2_thread == null || !yantai_2_thread.IsAlive)
            {

                yantai_2_thread = new Thread(yantai_2);
                yantai_2_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void yantai_2_stop()
        {
            if (yantai_2_thread == null || !yantai_2_thread.IsAlive)
            {
                yantai_2_thread.Abort();
            }
        }

        public void yantai_2()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "预售楼号", "规划总建筑面积", "售楼处地址", "容积率", "售楼电话", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }
                for (int i = 0; i < 9999; i=i+20)
                {
                    getlogs(DateTime.Now.ToString() + "抓取烟台预售许可信息第 "+((i/20)+1)+"页");
                    string url = "http://www.ytfcjy.com/CMS/SPF/GetYSXKList?startno="+i+"&endno="+(i+20)+"&value=%7B%22District%22%3A%22%E6%89%80%E6%9C%89%E5%8C%BA%E5%9F%9F%22%2C%22Code%22%3A%22%22%2C%22ProjectName%22%3A%22%22%7D";
                    string html = GetUrl(url, "utf-8");
                    MatchCollection IDs = Regex.Matches(html, @"""ID"":""([\s\S]*?)""");
                    if (IDs.Count == 0)
                    {
                        WriteCsv(table, "烟台", "yantai_2_");
                        getlogs(DateTime.Now.ToString() + "烟台预售许可信息导出csv成功！");
                        return;
                    }
                   
                    for (int j = 0; j < IDs.Count; j++)
                    {
                        string aurl = "http://www.ytfcjy.com/CMS/SPF/YSXKDetail/"+IDs[j].Groups[1].Value;
                        string ahtml = GetUrl(aurl, "utf-8");
                        Match a1 = Regex.Match(ahtml, @"预售楼号([\s\S]*?)FieldValue"">([\s\S]*?)<");
                        Match a2 = Regex.Match(ahtml, @"规划总建筑面积([\s\S]*?)FieldValue"">([\s\S]*?)<");
                        Match a3 = Regex.Match(ahtml, @"售楼处地址([\s\S]*?)FieldValue"">([\s\S]*?)<");
                        Match a4 = Regex.Match(ahtml, @"容积率([\s\S]*?)FieldValue"">([\s\S]*?)<");
                        Match a5 = Regex.Match(ahtml, @"售楼电话([\s\S]*?)FieldValue"">([\s\S]*?)<");


                        DataRow newRow = table.NewRow();
                        newRow["预售楼号"] = a1.Groups[2].Value;
                        newRow["规划总建筑面积"] = a2.Groups[2].Value;
                        newRow["售楼处地址"] = a3.Groups[2].Value;
                        newRow["容积率"] = a4.Groups[2].Value;
                        newRow["售楼电话"] = a5.Groups[2].Value;

                        table.Rows.Add(newRow);
                        Thread.Sleep(100);
                        
                    }

                   

                    

                }
            }
            catch (Exception ex)
            {

               ex.ToString();
            }

        }
        #endregion


        #region  济宁
        Thread jining_thread;

        public void jining_start()
        {
            if (jining_thread == null || !jining_thread.IsAlive)
            {

                jining_thread = new Thread(jining);
                jining_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void jining_stop()
        {
            if (jining_thread == null || !jining_thread.IsAlive)
            {
                jining_thread.Abort();
            }
        }

   


        public void jining()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "项目名称", "开发商", "项目位置", "建设规模", "总面积", "竣工时间","已售面积","已售价格", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }
                for (int i = 1; i < 2; i++)
                {
                    getlogs(DateTime.Now.ToString() + "抓取济宁楼盘信息第 " + i + "页");
                    string url = "http://www.jifcw.com/jn_web_dremis_dev/web_house_dir/Show_GoodsHouse_More.aspx";
                    string htm = GetUrl(url, "utf-8");

                    Match suiji1 = Regex.Match(htm, @"VIEWSTATE"" value=""([\s\S]*?)""");
                    Match suiji2 = Regex.Match(htm, @"EVENTVALIDATION"" value=""([\s\S]*?)""");

                    string postdata = "__EVENTTARGET=ctl00%24ContentPlaceHolder2%24AspNetPager1&__EVENTARGUMENT=" + i + "&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";
                    string html = PostUrl(url, postdata, "ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");
                   
                    MatchCollection IDs = Regex.Matches(html, @"<a id=""([\s\S]*?)doPostBack\('([\s\S]*?)'");
                    if (IDs.Count == 0)
                    {
                        WriteCsv(table, "济宁", "jining_" + DateTime.Now.ToString("yyyy-MM-dd"));
                        getlogs(DateTime.Now.ToString() + "济宁楼盘信息导出csv成功！");
                        return;
                    }

                    for (int j = 0; j < IDs.Count; j++)
                    {
                      
                        string aurl = "http://www.jifcw.com/jn_web_dremis_dev/web_house_dir/Show_GoodsHouse_More.aspx";
                      

                        string posts = "__EVENTTARGET="+ System.Web.HttpUtility.UrlEncode(IDs[j].Groups[2].Value) + "&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";
                     
                        string ahtml = PostUrl(aurl, posts, " ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");

                        Match a1 = Regex.Match(ahtml, @"item_name"">([\s\S]*?)<");
                        Match a2 = Regex.Match(ahtml, @"enter_name"">([\s\S]*?)<");
                        Match a3 = Regex.Match(ahtml, @"tem_seat"">([\s\S]*?)<");
                        Match a4 = Regex.Match(ahtml, @"item_area"">([\s\S]*?)<");
                        Match a5 = Regex.Match(ahtml, @"lb_area"">([\s\S]*?)<");
                        Match a6 = Regex.Match(ahtml, @"ew_date"">([\s\S]*?)<");
                        Match a7 = Regex.Match(ahtml, @"count1_lb_y_area"">([\s\S]*?)<");
                        Match a8 = Regex.Match(ahtml, @"count1_lb_z_price"">([\s\S]*?)<");

                        DataRow newRow = table.NewRow();
                        newRow["项目名称"] = a1.Groups[1].Value;
                        newRow["开发商"] = a2.Groups[1].Value;
                        newRow["项目位置"] = a3.Groups[1].Value;
                        newRow["建设规模"] = a4.Groups[1].Value;
                        newRow["总面积"] = a5.Groups[1].Value;
                        newRow["竣工时间"] = a6.Groups[1].Value;
                        newRow["已售面积"] = a7.Groups[1].Value;
                        newRow["已售价格"] = a8.Groups[1].Value;
                        table.Rows.Add(newRow);
                        Thread.Sleep(100);

                    }


                }

                WriteCsv(table, "济宁","jining_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "济宁楼盘信息导出csv成功！");
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion 济宁


        #region  邹城
        Thread zoucheng_thread;

        public void zoucheng_start()
        {
            if (zoucheng_thread == null || !zoucheng_thread.IsAlive)
            {

                zoucheng_thread = new Thread(zoucheng);
                zoucheng_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void zoucheng_stop()
        {
            if (zoucheng_thread == null || !zoucheng_thread.IsAlive)
            {
                zoucheng_thread.Abort();
            }
        }




        public void zoucheng()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "项目名称", "开发商", "项目位置", "建设规模", "总面积", "竣工时间", "已售面积", "已售价格", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }
                for (int i = 1; i < 2; i++)
                {
                    getlogs(DateTime.Now.ToString() + "抓取邹城楼盘信息第 " + i + "页");
                    string url = "http://www.zcsfdc.com/web_dremis/web_house_dir/Show_GoodsHouse_More.aspx";
                    string htm = GetUrl(url, "utf-8");

                    Match suiji1 = Regex.Match(htm, @"VIEWSTATE"" value=""([\s\S]*?)""");
                    Match suiji2 = Regex.Match(htm, @"EVENTVALIDATION"" value=""([\s\S]*?)""");

                    string postdata = "__EVENTTARGET=ctl00%24ContentPlaceHolder2%24AspNetPager1&__EVENTARGUMENT=" + i + "&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";
                    string html = PostUrl(url, postdata, "ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");

                    MatchCollection IDs = Regex.Matches(html, @"<a id=""([\s\S]*?)doPostBack\('([\s\S]*?)'");
                    if (IDs.Count == 0)
                    {
                        WriteCsv(table, "邹城","zoucheng_" + DateTime.Now.ToString("yyyy-MM-dd"));
                        getlogs(DateTime.Now.ToString() + "邹城楼盘信息导出csv成功！");
                        return;
                    }

                    for (int j = 0; j < IDs.Count; j++)
                    {

                        string aurl = "http://www.zcsfdc.com/web_dremis/web_house_dir/Show_GoodsHouse_More.aspx";


                        string posts = "__EVENTTARGET=" + System.Web.HttpUtility.UrlEncode(IDs[j].Groups[2].Value) + "&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";

                        string ahtml = PostUrl(aurl, posts, " ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");

                        Match a1 = Regex.Match(ahtml, @"item_name"">([\s\S]*?)<");
                        Match a2 = Regex.Match(ahtml, @"enter_name"">([\s\S]*?)<");
                        Match a3 = Regex.Match(ahtml, @"tem_seat"">([\s\S]*?)<");
                        Match a4 = Regex.Match(ahtml, @"item_area"">([\s\S]*?)<");
                        Match a5 = Regex.Match(ahtml, @"lb_area"">([\s\S]*?)<");
                        Match a6 = Regex.Match(ahtml, @"ew_date"">([\s\S]*?)<");
                        Match a7 = Regex.Match(ahtml, @"count1_lb_y_area"">([\s\S]*?)<");
                        Match a8 = Regex.Match(ahtml, @"count1_lb_z_price"">([\s\S]*?)<");

                        DataRow newRow = table.NewRow();
                        newRow["项目名称"] = a1.Groups[1].Value;
                        newRow["开发商"] = a2.Groups[1].Value;
                        newRow["项目位置"] = a3.Groups[1].Value;
                        newRow["建设规模"] = a4.Groups[1].Value;
                        newRow["总面积"] = a5.Groups[1].Value;
                        newRow["竣工时间"] = a6.Groups[1].Value;
                        newRow["已售面积"] = a7.Groups[1].Value;
                        newRow["已售价格"] = a8.Groups[1].Value;
                        table.Rows.Add(newRow);
                        Thread.Sleep(100);

                    }


                }

                WriteCsv(table,"邹城", "zoucheng_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "邹城楼盘信息导出csv成功！");
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion 济宁

        #region  大连金普新区
        Thread dljpxqz_thread;

        public void dljpxqz_start()
        {
            if (dljpxqz_thread == null || !dljpxqz_thread.IsAlive)
            {

                dljpxqz_thread = new Thread(dljpxqz);
                dljpxqz_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void dljpxqz_stop()
        {
            if (dljpxqz_thread == null || !dljpxqz_thread.IsAlive)
            {
                dljpxqz_thread.Abort();
            }
        }




        public void dljpxqz()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "序号","商品房", "存量房", "合计", "单位",DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }
              
                  
                    string url = "http://www.fczw.cn/index.shtml";
                    string html = GetUrl(url, "gb2312");
             
                Match ahtml = Regex.Match(html, @"周交易信息统计([\s\S]*?)</div>");
                MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");
              
            
                    DataRow newRow = table.NewRow();
                    newRow["序号"] = "套数";
                    newRow["商品房"] = values[0].Groups[1].Value;
                    newRow["存量房"] = values[1].Groups[1].Value;
                newRow["合计"] = values[2].Groups[1].Value;
                newRow["单位"] = values[3].Groups[1].Value;
                table.Rows.Add(newRow);

                DataRow newRow1 = table.NewRow();
                newRow1["序号"] = "面积";
                newRow1["商品房"] = values[4].Groups[1].Value;
                newRow1["存量房"] = values[5].Groups[1].Value;
                newRow1["合计"] = values[6].Groups[1].Value;
                newRow1["单位"] = values[7].Groups[1].Value;
                table.Rows.Add(newRow1);

                DataRow newRow2 = table.NewRow();
                newRow2["序号"] = "均价";
                newRow2["商品房"] = values[8].Groups[1].Value;
                newRow2["存量房"] = values[9].Groups[1].Value;
                newRow2["合计"] = values[10].Groups[1].Value;
                newRow2["单位"] = values[11].Groups[1].Value;
                table.Rows.Add(newRow2);

                DataRow newRow3 = table.NewRow();
                newRow3["序号"] = "总额";
                newRow3["商品房"] = values[12].Groups[1].Value;
                newRow3["存量房"] = values[13].Groups[1].Value;
                newRow3["合计"] = values[14].Groups[1].Value;
                newRow3["单位"] = values[15].Groups[1].Value;
                table.Rows.Add(newRow3);

                WriteCsv(table, "大连金普新区", "dljpxqz_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "大连金普新区周统计信息导出csv成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion 


        #region  大连高新区
        Thread dlgxqz_thread;

        public void dlgxqz_start()
        {
            if (dlgxqz_thread == null || !dlgxqz_thread.IsAlive)
            {

                dlgxqz_thread = new Thread(dlgxqz);
                dlgxqz_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void dlgxqz_stop()
        {
            if (dlgxqz_thread == null || !dlgxqz_thread.IsAlive)
            {
                dlgxqz_thread.Abort();
            }
        }




        public void dlgxqz()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "序号", "商品房", "存量房", "合计", "单位", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }


                string url = "http://fcjyw.dlhitech.gov.cn/index.shtml";
                string html = GetUrl(url, "gb2312");

                Match ahtml = Regex.Match(html, @"周交易信息统计([\s\S]*?)</div>");
                MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");


                DataRow newRow = table.NewRow();
                newRow["序号"] = "套数";
                newRow["商品房"] = values[0].Groups[1].Value;
                newRow["存量房"] = values[1].Groups[1].Value;
                newRow["合计"] = values[2].Groups[1].Value;
                newRow["单位"] = values[3].Groups[1].Value;
                table.Rows.Add(newRow);

                DataRow newRow1 = table.NewRow();
                newRow1["序号"] = "面积";
                newRow1["商品房"] = values[4].Groups[1].Value;
                newRow1["存量房"] = values[5].Groups[1].Value;
                newRow1["合计"] = values[6].Groups[1].Value;
                newRow1["单位"] = values[7].Groups[1].Value;
                table.Rows.Add(newRow1);

                DataRow newRow2 = table.NewRow();
                newRow2["序号"] = "均价";
                newRow2["商品房"] = values[8].Groups[1].Value;
                newRow2["存量房"] = values[9].Groups[1].Value;
                newRow2["合计"] = values[10].Groups[1].Value;
                newRow2["单位"] = values[11].Groups[1].Value;
                table.Rows.Add(newRow2);

                DataRow newRow3 = table.NewRow();
                newRow3["序号"] = "总额";
                newRow3["商品房"] = values[12].Groups[1].Value;
                newRow3["存量房"] = values[13].Groups[1].Value;
                newRow3["合计"] = values[14].Groups[1].Value;
                newRow3["单位"] = values[15].Groups[1].Value;
                table.Rows.Add(newRow3);

                WriteCsv(table, "大连高新区", "dlgxqz_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "大连高新区周统计信息导出csv成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion 


        #region  7、河南驻马店西平
        Thread hnzmdxpzrcj_thread;

        public void hnzmdxpzrcj_start()
        {
            if (hnzmdxpzrcj_thread == null || !hnzmdxpzrcj_thread.IsAlive)
            {

                hnzmdxpzrcj_thread = new Thread(hnzmdxpzrcj);
                hnzmdxpzrcj_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void hnzmdxpzrcj_stop()
        {
            if (hnzmdxpzrcj_thread == null || !hnzmdxpzrcj_thread.IsAlive)
            {
                hnzmdxpzrcj_thread.Abort();
            }
        }




        public void hnzmdxpzrcj()
        {
            try
            {
                getlogs(DateTime.Now.ToString() + "河南驻马店西平昨日成交正在抓取！");
                DataTable table = new DataTable();
                string[] columns = { "区域", "销售楼号", "成交套数", "成交面积", "成交均价", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }


                string url = "http://120.194.81.163/WebIssue/ExternalServer/Samples/price.asp?QueryItem=%D7%F2%C8%D5%D7%A1%D5%AC%BE%F9%BC%DB";
                string html = GetUrl(url, "gb2312");

            
                MatchCollection values = Regex.Matches(html, @"font-size: 12px>([\s\S]*?)</td>");

                for (int i = 0; i < values.Count/5; i++)
                {
                    DataRow newRow = table.NewRow();
                    newRow["区域"] = values[(5 * i) ].Groups[1].Value;
                    newRow["销售楼号"] = values[(5*i)+1].Groups[1].Value;
                    newRow["成交套数"] = values[(5 * i) + 2].Groups[1].Value;
                    newRow["成交面积"] = values[(5 * i) + 3].Groups[1].Value;
                    newRow["成交均价"] = values[(5 * i) + 4].Groups[1].Value;
                    table.Rows.Add(newRow);
                }
               

            

                WriteCsv(table, "河南驻马店西平", "hnzmdxpzrcj_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "河南驻马店西平昨日成交导出csv成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        Thread hnzmdxpysxx_thread;

        public void hnzmdxpysxx_start()
        {
            if (hnzmdxpysxx_thread == null || !hnzmdxpysxx_thread.IsAlive)
            {

                hnzmdxpysxx_thread = new Thread(hnzmdxpysxx);
                hnzmdxpysxx_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void hnzmdxpysxx_stop()
        {
            if (hnzmdxpysxx_thread == null || !hnzmdxpysxx_thread.IsAlive)
            {
                hnzmdxpysxx_thread.Abort();
            }
        }




        public void hnzmdxpysxx()
        {
            getlogs(DateTime.Now.ToString() + "河南驻马店西平预售许可信息正在抓取！");
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "开发商", "项目名称", "楼栋面积", "批准时间", "项目位置", "项目类型", "项目进度", "预售证号", "土地使用权证", "土地权证类型", "建设用地批准书", "规划许可证", "施工许可证", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }

                for (int i = 1; i < 12; i++)
                {

                    string url = "http://120.194.81.163/bit-xpxxzs/xmlpzs/webissue.asp?page="+i;
                    string html = GetUrl(url, "gb2312");


                    MatchCollection ids = Regex.Matches(html, @"<td><a href=""bsdetail\.asp\?id=([\s\S]*?)""");

                    for (int j = 0; j < ids.Count; j++)
                    {
                        string aurl = "http://120.194.81.163/bit-xpxxzs/xmlpzs/bsdetail.asp?id="+ids[j].Groups[1].Value;
                        string ahtml = GetUrl(aurl, "gb2312");

                        MatchCollection values = Regex.Matches(ahtml, @"<td colspan=""5"">([\s\S]*?)</td>");

                        DataRow newRow = table.NewRow();
                        newRow["开发商"] = values[0].Groups[1].Value;
                        newRow["项目名称"] = values[1].Groups[1].Value;
                        newRow["楼栋面积"] = values[2].Groups[1].Value;
                        newRow["批准时间"] = values[3].Groups[1].Value;
                        newRow["项目位置"] = values[4].Groups[1].Value;
                        newRow["项目类型"] = values[5].Groups[1].Value;
                        newRow["项目进度"] = values[6].Groups[1].Value;
                        newRow["预售证号"] = values[7].Groups[1].Value;
                        newRow["土地使用权证"] = Regex.Replace(values[8].Groups[1].Value, "<[^>]+>", "").Trim();
                        newRow["土地权证类型"] = Regex.Replace(values[9].Groups[1].Value, "<[^>]+>", "").Trim();
                        newRow["建设用地批准书"] = values[10].Groups[1].Value;
                        newRow["规划许可证"] = values[11].Groups[1].Value;
                        newRow["施工许可证"] = values[12].Groups[1].Value;
                        table.Rows.Add(newRow);
                    }

                }


                WriteCsv(table, "河南驻马店西平", "hnzmdxpysxx_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "河南驻马店西平导出csv成功！");
            }
            catch (Exception ex)
            {

                getlogs( ex.ToString());
            }

        }





        #endregion 


        #region  10、安徽宣城广德
        Thread ahxcgdtoday_thread;

        public void ahxcgdtoday_start()
        {
            if (ahxcgdtoday_thread == null || !ahxcgdtoday_thread.IsAlive)
            {

                ahxcgdtoday_thread = new Thread(ahxcgdtoday);
                ahxcgdtoday_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void ahxcgdtoday_stop()
        {
            if (ahxcgdtoday_thread == null || !ahxcgdtoday_thread.IsAlive)
            {
                ahxcgdtoday_thread.Abort();
            }
        }




        public void ahxcgdtoday()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "楼盘名称", "地区", "坐落", "预定套数", "交易套数", "交易均价",DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }


                string url = "http://gd.tmsf.com/hzfccs/spf/show.php";
                string html = GetUrl(url, "gb2312");


                MatchCollection values = Regex.Matches(html, @"<div align=""center""([\s\S]*?)</div>");

                for (int i = 0; i < values.Count / 5; i++)
                {
                    DataRow newRow = table.NewRow();
                    newRow["楼盘名称"] = values[(6 * i)].Groups[1].Value.Replace(">","").Trim();
                    newRow["地区"] = values[(6 * i) + 1].Groups[1].Value.Replace(">", "").Trim();
                    newRow["坐落"] = values[(6 * i) + 2].Groups[1].Value.Replace(">", "").Trim();
                    newRow["预定套数"] = values[(6 * i) + 3].Groups[1].Value.Replace(">", "").Trim();
                    newRow["交易套数"] = values[(6 * i) + 4].Groups[1].Value.Replace(">", "").Trim();
                    newRow["交易均价"] = values[(6 * i) + 5].Groups[1].Value.Replace("class=\"black\">", "").Trim();
                    table.Rows.Add(newRow);
                }




                WriteCsv(table, "安徽宣城广德", "ahxcgdtoday_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "安徽宣城广德导出csv成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        Thread ahxcgdysxx_thread;

        public void ahxcgdysxx_start()
        {
            if (ahxcgdysxx_thread == null || !ahxcgdysxx_thread.IsAlive)
            {

                ahxcgdysxx_thread = new Thread(ahxcgdysxx);
                ahxcgdysxx_thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        public void ahxcgdysxx_stop()
        {
            if (ahxcgdysxx_thread == null || !ahxcgdysxx_thread.IsAlive)
            {
                ahxcgdysxx_thread.Abort();
            }
        }




        public void ahxcgdysxx()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "开发商", "项目名称", "楼栋面积", "批准时间", "项目位置", "项目类型", "项目进度", "预售证号", "土地使用权证", "土地权证类型", "建设用地批准书", "规划许可证", "施工许可证", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }

                for (int i = 1; i < 28; i++)
                {

                    string url = "http://gd.tmsf.com/hzfccs/spf/index.php?page=" + i;
                    string html = GetUrl(url, "gb2312");


                    MatchCollection ids = Regex.Matches(html, @"<td><a href=""bsdetail\.asp\?id=([\s\S]*?)""");

                    for (int j = 0; j < ids.Count; j++)
                    {
                        string aurl = "http://120.194.81.163/bit-xpxxzs/xmlpzs/bsdetail.asp?id=" + ids[j].Groups[1].Value;
                        string ahtml = GetUrl(aurl, "gb2312");

                        MatchCollection values = Regex.Matches(ahtml, @"<td colspan=""5"">([\s\S]*?)</td>");

                        DataRow newRow = table.NewRow();
                        newRow["开发商"] = values[0].Groups[1].Value;
                        newRow["项目名称"] = values[1].Groups[1].Value;
                        newRow["楼栋面积"] = values[2].Groups[1].Value;
                        newRow["批准时间"] = values[3].Groups[1].Value;
                        newRow["项目位置"] = values[4].Groups[1].Value;
                        newRow["项目类型"] = values[5].Groups[1].Value;
                        newRow["项目进度"] = values[6].Groups[1].Value;
                        newRow["预售证号"] = values[7].Groups[1].Value;
                        newRow["土地使用权证"] = Regex.Replace(values[8].Groups[1].Value, "<[^>]+>", "").Trim();
                        newRow["土地权证类型"] = Regex.Replace(values[9].Groups[1].Value, "<[^>]+>", "").Trim();
                        newRow["建设用地批准书"] = values[10].Groups[1].Value;
                        newRow["规划许可证"] = values[11].Groups[1].Value;
                        newRow["施工许可证"] = values[12].Groups[1].Value;
                        table.Rows.Add(newRow);
                    }

                }


                WriteCsv(table, "安徽宣城广德", "ahxcgdysxx_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "安徽宣城广德导出csv成功！");
            }
            catch (Exception ex)
            {

                getlogs(ex.ToString());
            }

        }





        #endregion 


        #region  11、枣庄
       
        public void zaozhuang()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "商品房网签套数", "商品房网签面积(㎡)", "商品房网签金额(万元)", "住宅网签套数", "住宅网签面积(㎡)", "住宅网签金额(万元)", "非住宅网签套数", "非住宅网签面积(㎡)", "非住宅网签金额(万元)", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }


                string url = "http://60.214.99.136:18602/tradewebissue/callData?label=jrwq&page=1&limit=10";
                string html = GetUrl(url, "gb2312");


                MatchCollection values1 = Regex.Matches(html, @"""qyts"":""([\s\S]*?)""");
                MatchCollection values2 = Regex.Matches(html, @"""qymj"":""([\s\S]*?)""");
                MatchCollection values3 = Regex.Matches(html, @"""qyje"":""([\s\S]*?)""");
                MatchCollection values4 = Regex.Matches(html, @"""zzqyts"":""([\s\S]*?)""");
                MatchCollection values5 = Regex.Matches(html, @"""zzqymj"":""([\s\S]*?)""");
                MatchCollection values6 = Regex.Matches(html, @"""zzqyje"":""([\s\S]*?)""");
                MatchCollection values7 = Regex.Matches(html, @"""fzzqyts"":""([\s\S]*?)""");
                MatchCollection values8 = Regex.Matches(html, @"""fzzqymj"":""([\s\S]*?)""");
                MatchCollection values9 = Regex.Matches(html, @"""fzzqyje"":""([\s\S]*?)""");

                for (int i = 0; i < values1.Count ; i++)
                {
                    DataRow newRow = table.NewRow();
                    newRow["商品房网签套数"] = values1[i].Groups[1].Value;
                    newRow["商品房网签面积(㎡)"] = values2[i].Groups[1].Value;
                    newRow["商品房网签金额(万元)"] = values3[i].Groups[1].Value;
                    newRow["住宅网签套数"] = values4[i].Groups[1].Value;
                    newRow["住宅网签面积(㎡)"] = values5[i].Groups[1].Value;
                    newRow["住宅网签金额(万元)"] = values6[i].Groups[1].Value;
                    newRow["非住宅网签套数"] = values7[i].Groups[1].Value;
                    newRow["非住宅网签面积(㎡)"] = values8[i].Groups[1].Value;
                    newRow["非住宅网签金额(万元)"] = values9[i].Groups[1].Value;
                    table.Rows.Add(newRow);
                }




                WriteCsv(table, "枣庄", "zaozhuangtoday_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "枣庄今日签约导出csv成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        #endregion

        #region  12、淄博

        public void zibo()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "商品房网签套数", "商品房网签面积(㎡)", "商品房网签金额(万元)", "住宅网签套数", "住宅网签面积(㎡)", "住宅网签金额(万元)", "非住宅网签套数", "非住宅网签面积(㎡)", "非住宅网签金额(万元)", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }


                string url = "http://60.214.99.136:18602/tradewebissue/callData?label=jrwq&page=1&limit=10";
                string html = GetUrl(url, "gb2312");


                MatchCollection values1 = Regex.Matches(html, @"""qyts"":""([\s\S]*?)""");
                MatchCollection values2 = Regex.Matches(html, @"""qymj"":""([\s\S]*?)""");
                MatchCollection values3 = Regex.Matches(html, @"""qyje"":""([\s\S]*?)""");
                MatchCollection values4 = Regex.Matches(html, @"""zzqyts"":""([\s\S]*?)""");
                MatchCollection values5 = Regex.Matches(html, @"""zzqymj"":""([\s\S]*?)""");
                MatchCollection values6 = Regex.Matches(html, @"""zzqyje"":""([\s\S]*?)""");
                MatchCollection values7 = Regex.Matches(html, @"""fzzqyts"":""([\s\S]*?)""");
                MatchCollection values8 = Regex.Matches(html, @"""fzzqymj"":""([\s\S]*?)""");
                MatchCollection values9 = Regex.Matches(html, @"""fzzqyje"":""([\s\S]*?)""");

                for (int i = 0; i < values1.Count; i++)
                {
                    DataRow newRow = table.NewRow();
                    newRow["商品房网签套数"] = values1[i].Groups[1].Value;
                    newRow["商品房网签面积(㎡)"] = values2[i].Groups[1].Value;
                    newRow["商品房网签金额(万元)"] = values3[i].Groups[1].Value;
                    newRow["住宅网签套数"] = values4[i].Groups[1].Value;
                    newRow["住宅网签面积(㎡)"] = values5[i].Groups[1].Value;
                    newRow["住宅网签金额(万元)"] = values6[i].Groups[1].Value;
                    newRow["非住宅网签套数"] = values7[i].Groups[1].Value;
                    newRow["非住宅网签面积(㎡)"] = values8[i].Groups[1].Value;
                    newRow["非住宅网签金额(万元)"] = values9[i].Groups[1].Value;
                    table.Rows.Add(newRow);
                }




                WriteCsv(table, "枣庄", "zaozhuangtoday_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "枣庄今日签约导出csv成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }



        #endregion 


        #region 13、 曲阜




        public void qufu()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "项目名称", "开发商", "项目位置", "建设规模", "总面积", "竣工时间", "已售面积", "已售价格", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }
                for (int i = 1; i < 2; i++)
                {
                    getlogs(DateTime.Now.ToString() + "抓取曲阜楼盘信息第 " + i + "页");
                    string url = "http://www.qfsfc.com/qf_web_dremis/web_house_dir/Show_GoodsHouse_More_new.aspx";
                    string htm = GetUrl(url, "utf-8");

                    Match suiji1 = Regex.Match(htm, @"VIEWSTATE"" value=""([\s\S]*?)""");
                    Match suiji2 = Regex.Match(htm, @"EVENTVALIDATION"" value=""([\s\S]*?)""");

                    string postdata = "__EVENTTARGET=ctl00%24ContentPlaceHolder2%24AspNetPager1&__EVENTARGUMENT=" + i + "&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";
                    string html = PostUrl(url, postdata, "ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");

                    MatchCollection IDs = Regex.Matches(html, @"<a id=""([\s\S]*?)doPostBack\('([\s\S]*?)'");
                    if (IDs.Count == 0)
                    {
                        WriteCsv(table, "曲阜", "jining_" + DateTime.Now.ToString("yyyy-MM-dd"));
                        getlogs(DateTime.Now.ToString() + "曲阜楼盘信息导出csv成功！");
                        return;
                    }

                    for (int j = 0; j < IDs.Count; j++)
                    {

                        string aurl = "http://www.qfsfc.com/qf_web_dremis/web_house_dir/Show_GoodsHouse_More_new.aspx";


                        string posts = "__EVENTTARGET=" + System.Web.HttpUtility.UrlEncode(IDs[j].Groups[2].Value) + "&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";

                        string ahtml = PostUrl(aurl, posts, " ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");

                        Match a1 = Regex.Match(ahtml, @"item_name"">([\s\S]*?)<");
                        Match a2 = Regex.Match(ahtml, @"enter_name"">([\s\S]*?)<");
                        Match a3 = Regex.Match(ahtml, @"tem_seat"">([\s\S]*?)<");
                        Match a4 = Regex.Match(ahtml, @"item_area"">([\s\S]*?)<");
                        Match a5 = Regex.Match(ahtml, @"lb_area"">([\s\S]*?)<");
                        Match a6 = Regex.Match(ahtml, @"ew_date"">([\s\S]*?)<");
                        Match a7 = Regex.Match(ahtml, @"count1_lb_y_area"">([\s\S]*?)<");
                        Match a8 = Regex.Match(ahtml, @"count1_lb_z_price"">([\s\S]*?)<");

                        DataRow newRow = table.NewRow();
                        newRow["项目名称"] = a1.Groups[1].Value;
                        newRow["开发商"] = a2.Groups[1].Value;
                        newRow["项目位置"] = a3.Groups[1].Value;
                        newRow["建设规模"] = a4.Groups[1].Value;
                        newRow["总面积"] = a5.Groups[1].Value;
                        newRow["竣工时间"] = a6.Groups[1].Value;
                        newRow["已售面积"] = a7.Groups[1].Value;
                        newRow["已售价格"] = a8.Groups[1].Value;
                        table.Rows.Add(newRow);
                        Thread.Sleep(100);

                    }


                }

                WriteCsv(table, "曲阜", "qufu_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "曲阜楼盘信息导出csv成功！");
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion 济宁


        #region 14、  金寨

        public void jinzhai()
        {

            DataTable table = new DataTable();
            string[] columns = { "滚动信息",  DateTime.Now.ToLongDateString() };
            foreach (string column in columns)
            {
                table.Columns.Add(column, Type.GetType("System.String"));
            }


            string url = "http://www.ahjzfdc.cn/Web/Report/CountToday.aspx";
            string html = GetUrl(url, "utf-8");


            Match value= Regex.Match(html, @"<div id=""scroll_begin"">([\s\S]*?)</div>");

           
                DataRow newRow = table.NewRow();
                newRow["滚动信息"] = Regex.Replace(value.Groups[1].Value, "<[^>]+>", "").Trim();

            table.Rows.Add(newRow);
            

            WriteCsv(table, "金寨", "jinzhai_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "金寨滚动信息导出csv成功！");
           

        }


        public void jinzhai1()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "预售许可证编号", "项目名称", "坐落位置", "售房单位", "房屋用途性质", "预售对象", "开盘日期", "预售总建筑面积", "预售套数", "发证机关", "发证日期", "预售栋", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }
                for (int i = 1; i < 2; i++)
                {
                    getlogs(DateTime.Now.ToString() + "抓取金寨楼盘信息第 " + i + "页");
                    string url = "http://www.ahjzfdc.cn/Web/PreSellInfo/ShowPreSellCertList.aspx";
                    string htm = GetUrl(url, "utf-8");

                    Match suiji1 = Regex.Match(htm, @"VIEWSTATE"" value=""([\s\S]*?)""");
                    Match suiji2 = Regex.Match(htm, @"EVENTVALIDATION"" value=""([\s\S]*?)""");

                    string postdata = "__EVENTTARGET=ctl00%24ContentPlaceHolder2%24AspNetPager1&__EVENTARGUMENT=" + i + "&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";
                    string html = PostUrl(url, postdata, "ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");

                    MatchCollection IDs = Regex.Matches(html, @"DocID=([\s\S]*?)""");
                    if (IDs.Count == 0)
                    {
                        WriteCsv(table, "金寨", "jinzhai1_" + DateTime.Now.ToString("yyyy-MM-dd"));
                        getlogs(DateTime.Now.ToString() + "金寨预售信息导出csv成功！");
                        return;
                    }

                    for (int j = 0; j < IDs.Count; j++)
                    {

                        string aurl = "http://www.ahjzfdc.cn/Web/PreSellInfo/ShowPreSellCertInfo.aspx?DocID="+IDs[j].Groups[1].Value;

                        string ahtml = GetUrl(aurl,"utf-8");

                        Match a1 = Regex.Match(ahtml, @"<span id=""证书编号"">([\s\S]*?)</span>");
                        Match a2 = Regex.Match(ahtml, @"<span id=""项目名称"">([\s\S]*?)</span>");
                        Match a3 = Regex.Match(ahtml, @"<span id=""房地坐落"">([\s\S]*?)</span>");
                        Match a4 = Regex.Match(ahtml, @"<span id=""预售单位"">([\s\S]*?)</span>");
                        Match a5 = Regex.Match(ahtml, @"<span id=""性质"">([\s\S]*?)</span>");
                        Match a6 = Regex.Match(ahtml, @"<span id=""预售对象"">([\s\S]*?)</span>");
                        Match a7 = Regex.Match(ahtml, @"<span id=""开盘日期"">([\s\S]*?)</span>");
                        Match a8 = Regex.Match(ahtml, @"<span id=""预售总建筑面积"">([\s\S]*?)</span>");
                        Match a9 = Regex.Match(ahtml, @"<span id=""预售套数"">([\s\S]*?)</span>");
                        Match a10 = Regex.Match(ahtml, @"<span id=""发证机关"">([\s\S]*?)</span>");
                        Match a11 = Regex.Match(ahtml, @"<span id=""发证日期"">([\s\S]*?)</span>");
                        Match a12 = Regex.Match(ahtml, @"<span id=""txtDong"">([\s\S]*?)</span>");

                        DataRow newRow = table.NewRow();
                        newRow["预售许可证编号"] = a1.Groups[1].Value;
                        newRow["项目名称"] = a2.Groups[1].Value;
                        newRow["坐落位置"] = a3.Groups[1].Value;
                        newRow["售房单位"] = a4.Groups[1].Value;
                        newRow["房屋用途性质"] = a5.Groups[1].Value;
                        newRow["预售对象"] = a6.Groups[1].Value;
                        newRow["开盘日期"] = a7.Groups[1].Value;
                        newRow["预售总建筑面积"] = a8.Groups[1].Value;
                        newRow["预售套数"] = a9.Groups[1].Value;
                        newRow["发证机关"] = a10.Groups[1].Value;
                        newRow["发证日期"] = a11.Groups[1].Value;
                        newRow["预售栋"] = Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "");
                        table.Rows.Add(newRow);
                        Thread.Sleep(100);

                    }


                }

                WriteCsv(table, "金寨", "jinzhai1_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "金寨楼盘信息导出csv成功！");
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion


        #region 14、  舒城

        public void shucheng()
        {

            DataTable table = new DataTable();
            string[] columns = { "滚动信息", DateTime.Now.ToLongDateString() };
            foreach (string column in columns)
            {
                table.Columns.Add(column, Type.GetType("System.String"));
            }


            string url = "http://www.ahscfdc.com/Web/Report/CountToday.aspx";
            string html = GetUrl(url, "utf-8");


            Match value = Regex.Match(html, @"<div id=""scroll_begin"">([\s\S]*?)</div>");


            DataRow newRow = table.NewRow();
            newRow["滚动信息"] = Regex.Replace(value.Groups[1].Value, "<[^>]+>", "").Trim();

            table.Rows.Add(newRow);


            WriteCsv(table, "舒城", "shucheng_" + DateTime.Now.ToString("yyyy-MM-dd"));
            getlogs(DateTime.Now.ToString() + "舒城滚动信息导出csv成功！");


        }


        public void shucheng1()
        {
            try
            {
                DataTable table = new DataTable();
                string[] columns = { "预售许可证编号", "项目名称", "坐落位置", "售房单位", "房屋用途性质", "预售对象", "开盘日期", "预售总建筑面积", "预售套数", "发证机关", "发证日期", "预售栋", DateTime.Now.ToLongDateString() };
                foreach (string column in columns)
                {
                    table.Columns.Add(column, Type.GetType("System.String"));
                }
                for (int i = 1; i < 2; i++)
                {
                    getlogs(DateTime.Now.ToString() + "抓取舒城楼盘信息第 " + i + "页");
                    string url = "http://www.ahscfdc.com/Web/PreSellInfo/ShowPreSellCertList.aspx";
                    string htm = GetUrl(url, "utf-8");

                    Match suiji1 = Regex.Match(htm, @"VIEWSTATE"" value=""([\s\S]*?)""");
                    Match suiji2 = Regex.Match(htm, @"EVENTVALIDATION"" value=""([\s\S]*?)""");

                    string postdata = "__EVENTTARGET=ctl00%24ContentPlaceHolder2%24AspNetPager1&__EVENTARGUMENT=" + i + "&__LASTFOCUS=&__VIEWSTATE=" + System.Web.HttpUtility.UrlEncode(suiji1.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&__VIEWSTATEGENERATOR=D817B387&__EVENTVALIDATION=" + System.Web.HttpUtility.UrlEncode(suiji2.Groups[1].Value, Encoding.GetEncoding("utf-8")) + "&ctl00%24login_xj%24tb_login_name=&ctl00%24login_xj%24tb_password=&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_enterprice=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24ddl_district=0&ctl00%24ContentPlaceHolder2%24web_item_search1%24tb_seat=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24tb_value=&ctl00%24ContentPlaceHolder2%24GoodsHouse1%24ddl_type=0";
                    string html = PostUrl(url, postdata, "ASP.NET_SessionId=scr2wy45vchmwcb5a3tt5hen", "utf-8");

                    MatchCollection IDs = Regex.Matches(html, @"DocID=([\s\S]*?)""");
                    if (IDs.Count == 0)
                    {
                        WriteCsv(table, "舒城", "shucheng1_" + DateTime.Now.ToString("yyyy-MM-dd"));
                        getlogs(DateTime.Now.ToString() + "舒城预售信息导出csv成功！");
                        return;
                    }

                    for (int j = 0; j < IDs.Count; j++)
                    {

                        string aurl = "http://www.ahscfdc.com/Web/PreSellInfo/ShowPreSellCertInfo.aspx?DocID= " + IDs[j].Groups[1].Value;

                        string ahtml = GetUrl(aurl, "utf-8");

                        Match a1 = Regex.Match(ahtml, @"<span id=""证书编号"">([\s\S]*?)</span>");
                        Match a2 = Regex.Match(ahtml, @"<span id=""项目名称"">([\s\S]*?)</span>");
                        Match a3 = Regex.Match(ahtml, @"<span id=""房地坐落"">([\s\S]*?)</span>");
                        Match a4 = Regex.Match(ahtml, @"<span id=""预售单位"">([\s\S]*?)</span>");
                        Match a5 = Regex.Match(ahtml, @"<span id=""性质"">([\s\S]*?)</span>");
                        Match a6 = Regex.Match(ahtml, @"<span id=""预售对象"">([\s\S]*?)</span>");
                        Match a7 = Regex.Match(ahtml, @"<span id=""开盘日期"">([\s\S]*?)</span>");
                        Match a8 = Regex.Match(ahtml, @"<span id=""预售总建筑面积"">([\s\S]*?)</span>");
                        Match a9 = Regex.Match(ahtml, @"<span id=""预售套数"">([\s\S]*?)</span>");
                        Match a10 = Regex.Match(ahtml, @"<span id=""发证机关"">([\s\S]*?)</span>");
                        Match a11 = Regex.Match(ahtml, @"<span id=""发证日期"">([\s\S]*?)</span>");
                        Match a12 = Regex.Match(ahtml, @"<span id=""txtDong"">([\s\S]*?)</span>");

                        DataRow newRow = table.NewRow();
                        newRow["预售许可证编号"] = a1.Groups[1].Value;
                        newRow["项目名称"] = a2.Groups[1].Value;
                        newRow["坐落位置"] = a3.Groups[1].Value;
                        newRow["售房单位"] = a4.Groups[1].Value;
                        newRow["房屋用途性质"] = a5.Groups[1].Value;
                        newRow["预售对象"] = a6.Groups[1].Value;
                        newRow["开盘日期"] = a7.Groups[1].Value;
                        newRow["预售总建筑面积"] = a8.Groups[1].Value;
                        newRow["预售套数"] = a9.Groups[1].Value;
                        newRow["发证机关"] = a10.Groups[1].Value;
                        newRow["发证日期"] = a11.Groups[1].Value;
                        newRow["预售栋"] = Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "");
                        table.Rows.Add(newRow);
                        Thread.Sleep(100);

                    }


                }

                WriteCsv(table, "舒城", "shucheng1_" + DateTime.Now.ToString("yyyy-MM-dd"));
                getlogs(DateTime.Now.ToString() + "金寨楼盘信息导出csv成功！");
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
    }
}
