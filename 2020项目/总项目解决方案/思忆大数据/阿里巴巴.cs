using System;
using System.Collections;
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

namespace 思忆大数据
{
    public partial class 阿里巴巴 : Form
    {
        public 阿里巴巴()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url,string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = textBox3.Text;
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

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            string[] citys = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string city in citys)
            {
             string cityutf8=  System.Web.HttpUtility.UrlEncode(city, Encoding.GetEncoding("utf-8"));
              
                foreach (string keyword in keywords)
                {
                    string keywordgb2312 = System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"));
                    for (int page =1; page < 101; page++)
                {
                    string url = "https://search.1688.com/service/companySearchBusinessService?keywords="+keywordgb2312+ "&beginPage="+page+ "&async=true&asyncCount=20&pageSize=20&requestId=1212261852001608611613516000553&sessionId=9517c8c5e4324a6bbc33a050e316d77b&startIndex=0&province=%E6%B2%B3%E5%8C%97&city="+cityutf8;
                    
                    string html = GetUrl(url, "utf-8");
  
                    MatchCollection titles = Regex.Matches(html, @"""company"":""([\s\S]*?)""");
                      
                        MatchCollection addresss = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                        MatchCollection subjects = Regex.Matches(html, @"""subject"":""([\s\S]*?)""");
                        MatchCollection tpServiceYears = Regex.Matches(html, @"""tpServiceYear"":([\s\S]*?),");
                        MatchCollection domainUris = Regex.Matches(html, @"""domainUri"":""([\s\S]*?)""");
                      
                        if (titles.Count == 0)
                    {
                        break;

                    }
                        for (int j = 0; j < titles.Count; j++)
                        {

                            try
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(titles[j].Groups[1].Value);
                                lv1.SubItems.Add(addresss[j].Groups[1].Value);
                                lv1.SubItems.Add(subjects[j].Groups[1].Value);

                                lv1.SubItems.Add(keyword);
                                lv1.SubItems.Add(tpServiceYears[j].Groups[1].Value.Replace("}", ""));
                                lv1.SubItems.Add("https://"+domainUris[j].Groups[1].Value);
                            }
                            catch (Exception)
                            {

                                continue;
                            }
                           
                          

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }


                       
                        }
                        Thread.Sleep(2000);
                    }

                    
                }
            }
            MessageBox.Show("抓取结束");


        }
        bool zanting = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 阿里巴巴_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 阿里巴巴_Load(object sender, EventArgs e)
        {

        }
    }
}
