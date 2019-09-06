using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_8
{
    public partial class 交易所 : Form
    {
        public 交易所()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string referUrl)
        {
            try
            {
                          
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
                request.Referer = referUrl;
                request.AllowAutoRedirect = true;
               
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            request.AllowAutoRedirect = false;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
           
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }

        #endregion

        bool zanting = true;
        private void 交易所_Load(object sender, EventArgs e)
        {

        }

        public string getName(string code)
        {
            Match name = Regex.Match(method.GetUrl("https://suggest3.sinajs.cn/suggest/type=&key="+code+"&name=suggestdata_1567749930682", "gb2312"), @"=""([\s\S]*?),");
            return (name.Groups[1].Value);
        }

        #region 上海证券交易所

        public void sse()
        {
            DateTime dtStart = DateTime.Parse(dateTimePicker1.Text); ;
            DateTime dtEnd = DateTime.Parse(dateTimePicker2.Text);

            string startdate = dtStart.ToString("yyyy-MM-dd");
            string enddate = dtEnd.ToString("yyyy-MM-dd");

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i= 0; i < text.Length; i++)
            {
                string name = getName(text[i]);
                for (int j= 1; j < 50; j++)
                {
                    string url = "http://query.sse.com.cn/security/stock/queryCompanyBulletin.do?jsonCallBack=jsonpCallback56167&isPagination=true&productId="+text[i]+"&keyWord=&securityType=0101%2C120100%2C020100%2C020200%2C120200&reportType2=&reportType=ALL&beginDate="+startdate+"&endDate="+enddate+"&pageHelp.pageSize=25&pageHelp.pageCount=50&pageHelp.pageNo="+j+"&pageHelp.beginPage="+j+"&pageHelp.cacheSize=1&pageHelp.endPage=50&_=1567736111023";
                    string referUrl = "http://www.sse.com.cn/disclosure/listedinfo/announcement/";
                    
                    string html = GetUrl(url,referUrl);
                    textBox2.Text += DateTime.Now.ToString() + "正在抓取........" + text[i] + "\r\n";

                    MatchCollection dates = Regex.Matches(html, @"""SSEDATE"":""([\s\S]*?)""");
                  
                    MatchCollection titles = Regex.Matches(html, @"""TITLE"":""([\s\S]*?)""");
                    MatchCollection links = Regex.Matches(html, @"""URL"":""([\s\S]*?)""");

                    if (dates.Count == 0)
                        break;

      
                        for (int a = 0; a < dates.Count; a++)
                        {

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(dates[a].Groups[1].Value);
                            lv1.SubItems.Add(text[i]);
                            lv1.SubItems.Add(name);
                            lv1.SubItems.Add(titles[a].Groups[1].Value);
                            lv1.SubItems.Add("http://static.sse.com.cn" + links[a].Groups[1].Value.Replace("\\",""));
                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }


                    }
                
                
            }
            textBox2.Text += DateTime.Now.ToString() + "抓取结束" + "\r\n";
        }
        #endregion

        #region 深圳证券交易所

        public void szse()
        {
            DateTime dtStart = DateTime.Parse(dateTimePicker1.Text); ;
            DateTime dtEnd = DateTime.Parse(dateTimePicker2.Text);

            string startdate = dtStart.ToString("yyyy-MM-dd");
            string enddate = dtEnd.ToString("yyyy-MM-dd");

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 1; j < 50; j++)
                {
                    string url = "http://www.szse.cn/api/disc/announcement/annList?random=0.3188454187775951";
                    string json = "{\"seDate\":[\""+startdate+"\",\""+enddate+"\"],\"stock\":[\""+text[i]+"\"],\"channelCode\":[\"listedNotice_disc\"],\"pageSize\":30,\"pageNum\":"+j+"}";
                    
                    string html = PostUrl(url,json);
                    textBox2.Text +=DateTime.Now.ToString()+ "正在抓取........"+text[i]+"\r\n";
                    
                    MatchCollection dates = Regex.Matches(html, @"""publishTime"":""([\s\S]*?)""");
                    Match name = Regex.Match(html, @"secName"":\[""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                    MatchCollection links = Regex.Matches(html, @"""id"":""([\s\S]*?)""");

                    if (dates.Count == 0)
                        break;


                    for (int a = 0; a < dates.Count; a++)
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(dates[a].Groups[1].Value);
                        lv1.SubItems.Add(text[i]);
                        lv1.SubItems.Add(name.Groups[1].Value);
                        lv1.SubItems.Add(titles[a].Groups[1].Value);
                        lv1.SubItems.Add("http://www.szse.cn/disclosure/listed/bulletinDetail/index.html?" + links[a].Groups[1].Value);
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }


                }


            }

            textBox2.Text += DateTime.Now.ToString() + "抓取结束" + "\r\n";
        }
        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "12.12.12.12")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                if (comboBox1.Text == "上海证券交易所")
                {
                    Thread thread = new Thread(new ThreadStart(sse));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }

                else if (comboBox1.Text == "深圳证券交易所")
                {
                    Thread thread = new Thread(new ThreadStart(szse));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }


            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
           
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
