using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace 主程序202102
{
    public partial class 申通快递查询 : Form
    {
       
        public 申通快递查询()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;



        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset,string token)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("x-app-id: WDGJ");
                headers.Add("x-request-id: 05c9081b-6961-4401-9056-bf8a581153e8");
                headers.Add("x-token: "+token);

                //添加头部
                // request.KeepAlive = true;
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

        #region 订单信息
        public void dingdan()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList lists = new ArrayList();
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (text.Length <= 1000)
            {
                for (int i = 0; i < text.Length; i++)
                {

                    sb.Append("\"" + text[i] + "\",");
                }
                lists.Add(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }
            else
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (i>0 && i%1000==0)
                    {
                        lists.Add(sb.ToString().Substring(0, sb.ToString().Length - 1));
                        sb.Clear();
                    }
                    sb.Append("\"" + text[i] + "\",");
                }
                lists.Add(sb.ToString().Substring(0, sb.ToString().Length - 1));
            }
            foreach (string item in lists)
            {

                string url = "https://wangdian.sto.cn/commonTools/orderInquiry/getTabelData";
                string postdata = "{\"current\":1,\"pageSize\":1000,\"startDate\":\"2020-09-01 00:00:00\",\"endDate\":\"" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59\",\"billCode\":[" + item + "],\"networkCode\":\"529509\"}";
                string cookie = textBox2.Text.Trim();
                string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json;", "https://wangdian.sto.cn/page/common-tools/order-inquiry");



                MatchCollection billCodes = Regex.Matches(html, @"""billCode"":""([\s\S]*?)""");
                MatchCollection receiverNames = Regex.Matches(html, @"""receiverName"":""([\s\S]*?)""");
                MatchCollection receiverMobiles = Regex.Matches(html, @"""receiverMobile"":""([\s\S]*?)""");
                MatchCollection receiverAddress = Regex.Matches(html, @"""receiverAddress"":""([\s\S]*?)""");
                MatchCollection weights = Regex.Matches(html, @"""weight"":""([\s\S]*?)""");
                MatchCollection threeCodes = Regex.Matches(html, @"""threeCode"":([\s\S]*?),");

                for (int j = 0; j < billCodes.Count; j++)
                {

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    try
                    {

                        lv1.SubItems.Add(Regex.Replace(billCodes[j].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(weights[j].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(receiverNames[j].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(receiverAddress[j].Groups[1].Value, "<[^>]+>", "").Replace("^", "").Replace(" ", ""));
                        lv1.SubItems.Add(Regex.Replace(receiverMobiles[j].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(threeCodes[j].Groups[1].Value, "<[^>]+>", "").Replace("\"", ""));
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                    }
                    catch (Exception)
                    {

                        lv1.SubItems.Add("");

                    }

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;




                }
            }

        }

        #endregion


        #region 跟踪信息
        public void genzong()
        {
            Match token= Regex.Match(textBox3.Text, @"token=([\s\S]*?)&");
            if (token.Groups[1].Value == "")
            {
                MessageBox.Show("请先输入快递跟踪新的网址");
                return;
            }

            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("请先获取订单信息");
                return;
            }

        
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    string url = "https://wutonggateway.sto.cn/traceProtal/waybill/search?waybill=" + listView1.Items[i].SubItems[1].Text.Trim();

                    string html = GetUrl(url, "utf-8", token.Groups[1].Value);


                    Match memo = Regex.Match(html, @"""scanType"":""派件([\s\S]*?)""memo"":""([\s\S]*?)""");

                    string infos = memo.Groups[2].Value;
                    string time = "";
                    if (memo.Groups[2].Value == "")
                    {
                        MatchCollection memoes = Regex.Matches(html, @"""memo"":""([\s\S]*?)""");
                        MatchCollection uploadTimes = Regex.Matches(html, @"""uploadTime"":""([\s\S]*?)""");
                        infos = memoes[memoes.Count - 1].Groups[1].Value;
                        time = uploadTimes[uploadTimes.Count - 1].Groups[1].Value;
                    }
                    else
                    {
                        MatchCollection memoes = Regex.Matches(html, @"""waybillNo"":""([\s\S]*?)memo");
                        for (int a = 0; a< memoes.Count;a++)
                        {
                            if (memoes[a].Groups[1].Value.Contains("\"scanType\":\"派件\""))
                            {
                                Match t = Regex.Match(memoes[a].Groups[1].Value, @"""uploadTime"":""([\s\S]*?)""");
                                time = t.Groups[1].Value;
                            }

                        }
                    }

                    listView1.Items[i].SubItems[7].Text = (time);
                    listView1.Items[i].SubItems[8].Text = (infos);



                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    Thread.Sleep(200);
                }
                catch (Exception)
                {

                    continue;
                }
            
        
           
            }


        }

        #endregion
        private void 申通快递查询_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"RE72"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
          

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(dingdan);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(genzong);
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

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listView1.Items.Clear();
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            



            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            
        }

       
        public void getCookies()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://wangdian.sto.cn");
            //options.AddArgument("--lang=en"); 


            StringBuilder sb;

            while (true)
            {
                sb = new StringBuilder();
                var _cookies = driver.Manage().Cookies.AllCookies;

                foreach (OpenQA.Selenium.Cookie cookie in _cookies)
                {

                    sb.Append(cookie.Name + "=" + cookie.Value + ";");
                    // driver.Manage().Cookies.AddCookie(cookie);
                }

                if (sb.ToString().Contains("WD_SESSION"))
                {

                    break;
                }
            }
           // driver.Quit();
          
            textBox2.Text = sb.ToString();


        }
        Thread t;
        private void button7_Click_1(object sender, EventArgs e)
        {
            if (t == null || !t.IsAlive)
            {
                t = new Thread(getCookies);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
