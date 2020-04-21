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

namespace 启动程序
{
    public partial class 东方财富 : Form
    {
        public 东方财富()
        {
            InitializeComponent();
        }

  
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url); 
               
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
               
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse; 
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
        
        public void run()
        {
            try
            {
                for (int i = 1; i < 742; i++)
                {
                    string url = "http://data.eastmoney.com/DataCenter_V3/gdfx/data.ashx?SortType=NDATE,SCODE,RANK&SortRule=1&PageIndex="+i+"&PageSize=50&jsObj=UFgoiMMm&type=NSHDDETAIL&date=&gdlx=0&cgbd=0&rt=52911910";
                    string html= GetUrl(url,"gb2312");
                    MatchCollection a1s = Regex.Matches(html, @"""SHAREHDNAME"":""([\s\S]*?)""");
                    MatchCollection a2s = Regex.Matches(html, @"""SHAREHDTYPE"":""([\s\S]*?)""");
                    MatchCollection a3s = Regex.Matches(html, @"""RANK"":([\s\S]*?),");
                    MatchCollection a4s = Regex.Matches(html, @"""SCODE"":""([\s\S]*?)""");
                    MatchCollection a5s = Regex.Matches(html, @"""SNAME"":""([\s\S]*?)""");
                    MatchCollection a6s = Regex.Matches(html, @"""RDATE"":""([\s\S]*?)""");
                    MatchCollection a7s = Regex.Matches(html, @"""SHAREHDNUM"":([\s\S]*?),");
                    MatchCollection a8s = Regex.Matches(html, @"""ZB"":([\s\S]*?),");
                    MatchCollection a9s = Regex.Matches(html, @"""BDSUM"":([\s\S]*?)}");
                    MatchCollection a10s = Regex.Matches(html, @"""BDBL"":([\s\S]*?),");
                    MatchCollection a11s = Regex.Matches(html, @"""BZ"":""([\s\S]*?)""");
                    MatchCollection a12s = Regex.Matches(html, @"""LTAG"":([\s\S]*?),");
                    MatchCollection a13s = Regex.Matches(html, @"""NDATE"":""([\s\S]*?)""");


                    for (int j = 0; j <a1s.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1s[j].Groups[1].Value);
                        lv1.SubItems.Add(a2s[j].Groups[1].Value);
                        lv1.SubItems.Add(a3s[j].Groups[1].Value);
                        lv1.SubItems.Add(a4s[j].Groups[1].Value);
                        lv1.SubItems.Add(a5s[j].Groups[1].Value);
                        lv1.SubItems.Add(a6s[j].Groups[1].Value);
                        lv1.SubItems.Add(a7s[j].Groups[1].Value);
                        lv1.SubItems.Add(a8s[j].Groups[1].Value);
                        lv1.SubItems.Add(a9s[j].Groups[1].Value);
                        lv1.SubItems.Add(a10s[j].Groups[1].Value);
                        lv1.SubItems.Add(a11s[j].Groups[1].Value);
                        lv1.SubItems.Add(a12s[j].Groups[1].Value);
                        lv1.SubItems.Add(a13s[j].Groups[1].Value);
                    }
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void 东方财富_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(run));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
