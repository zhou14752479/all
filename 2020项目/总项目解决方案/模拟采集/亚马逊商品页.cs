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
using helper;
using Microsoft.VisualBasic;

namespace 模拟采集
{
    public partial class 亚马逊商品页 : Form
    {
        public 亚马逊商品页()
        {
            InitializeComponent();
        }
        string cookie = "CGIC=IocBdGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC45; ANID=AHWqTUnKSXTCZbSndZCDlNqsU14codPMRJJJSosA1xm2GHuM192ryqWqW2NDXlZr; NID=204=Tb9elz_FtTTLllf3Q-zjdzqv3r1PZ8JFKU9d8kG0CB-IK-LaADDp84mI_ngto8NKJZUp1EEahgZUWthGCeRVexspvzkz1WFSKARAvSDdo0eYKh2hZiPXeFtQIIrh_oE4LWXYPskOIQi52RUrKEsgkTZX2IIK2dQakcgFUNMJ4i4; 1P_JAR=2020-10-15-01; DV=o_Lv2Qtdsh8ycNiZldgA7vwYbL2eUlc4dyRxmh2zKAIAAODoGxMnRVuoSeyqAAA";

        string[] useragents = {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.100 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Gecko/20100308 (KHTML, like Gecko) Firefox/56.0",
             "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.25 Safari/537.36 Core/1.70.3776.400 QQBrowser/10.6.4212.400",
              "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.25 Safari/537.36 Core/1.70.3676.400 QQBrowser/10.5.3738.400",
               "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; McAfee)",
                "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko Core/1.70.3775.400 QQBrowser/10.6.4209.400",
                 "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:68.9) Gecko/20100101 Goanna/4.6 Firefox/68.9 PaleMoon/28.13.0",
                  "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.135 Safari/537.36 OPR/70.0.3728.189",
                   "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.102 Safari/537.36 OPR/71.0.3770.148",
        };
        
        
        
        
        
        
        
        
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                //添加头部
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                WebHeaderCollection headers = request.Headers;
                headers.Add("x-client-data: CJG2yQEIo7bJAQjEtskBCKmdygEI0qDKAQiZtcoBCKvHygEI9cfKAQjnyMoBCOnIygEItMvKAQ==");
                request.Referer = "";
                Random rd = new Random();
                request.UserAgent = useragents[rd.Next(0,10)];
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
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

        public void pipei(string ahtml,string asin,string url, string keyword)
        {
            Match title = Regex.Match(ahtml, @"<span id=""productTitle"" class=""a-size-large product-title-word-break"">([\s\S]*?)</span>");
            Match review = Regex.Match(ahtml, @"acrCustomerReviewText"" class=""a-size-base"">([\s\S]*?) ");
            Match star = Regex.Match(ahtml, @"class=""a-size-medium a-color-base"">([\s\S]*?)</span>");
            Match avalable = Regex.Match(ahtml, @"Amazon.com:([\s\S]*?)""");


            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据   
            lv1.SubItems.Add(asin);                                                              // lv1.SubItems.Add(asins[j].Groups[1].Value);
            lv1.SubItems.Add(title.Groups[1].Value.Trim());
           
            lv1.SubItems.Add(url);
            lv1.SubItems.Add(keyword);
            lv1.SubItems.Add(review.Groups[1].Value.Trim());
            lv1.SubItems.Add(star.Groups[1].Value.Trim());
        }


        Random rd = new Random();
        

        public void main()
        {
            seconds = 0;
            status = true;


            int start = rd.Next(0, 20)*10;

            try
            {

           
            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in keywords)
            {

                for (int i = start; i < 100001; i=i+10)
                {

                       
                        string url = "https://www.google.co.kr/search?q=site:www.amazon." + domain+ "/stores/+currently+unavailable+" + keyword + "&ei=LqaHX5CGHtbk-gTjpZ2wCA&start="+i+"&sa=N&ved=2ahUKEwiQ1eu7ubXsAhVWsp4KHeNSB4Y4qgEQ8tMDegQIBhBC&biw=1920&bih=936";

                         string html = GetUrl(url, "utf-8");
                        //string html = method.gethtml(url, "");

                        MatchCollection URLs = Regex.Matches(html, @"<div class=""yuRUbf""><a href=""([\s\S]*?)""");
                 
                    if (URLs.Count == 0)
                    {
                        break;
                        
                    }

                    for (int j = 0; j < URLs.Count; j++)
                    {
                            label1.Text = "采集中......";
                            textBox2.Text += "正在抓取" + URLs[j].Groups[1].Value+"\r\n";

                            string strhtml = method.gethtml(URLs[j].Groups[1].Value, cookie);
                            
                            if (URLs[j].Groups[1].Value.Contains("/dp/"))
                            {
                                if (strhtml.Contains(bukeshou) || strhtml.Contains("目前无货"))
                                {
                                    Match a = Regex.Match(URLs[j].Groups[1].Value, @"/dp/B.*");
                                    pipei(strhtml, a.Groups[0].Value.Replace("/dp/", ""), URLs[j].Groups[1].Value, keyword);
                                    continue;
                                }
                            }
                          

                           // Match asin1 = Regex.Match(strhtml, @"""ASINList"":\[([\s\S]*?)\]");
                            //string[] asins= asin1.Groups[1].Value.Replace("\"","").Split(new string[] { "," }, StringSplitOptions.None);

                            //foreach (string asin in asins)
                            //{
                            //    string aurl = "https://www.amazon."+domain+"/dp/"+asin;

                            //    string ahtml = method.gethtml(aurl, cookie);

                            //    if (ahtml.Contains(bukeshou) || ahtml.Contains("目前无货"))
                            //    {

                            //        pipei(ahtml,asin,aurl,keyword);

                            //    }


                            //}
                            MatchCollection asins = Regex.Matches(strhtml, @"""asin"":""([\s\S]*?)""");
                            MatchCollection title = Regex.Matches(strhtml, @"""altText"":""([\s\S]*?)""");
                            MatchCollection review = Regex.Matches(strhtml, @"""count"":\{""displayString"":""([\s\S]*?)""");
                            MatchCollection star = Regex.Matches(strhtml, @"rating"":\{""displayString"":""([\s\S]*?)""");
                            MatchCollection avalable = Regex.Matches(strhtml, @"""primaryMessage"":""([\s\S]*?)""");

                            for (int a = 0;a < title.Count; a++)
                            {
                                Thread.Sleep(3000);
                                try
                                {
                                    if (avalable[a].Groups[1].Value.Trim().Contains(bukeshou) || avalable[a].Groups[1].Value.Contains("目前无货"))
                                    {

                                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据   
                                        lv1.SubItems.Add(asins[a].Groups[1].Value.Trim());                                                              // lv1.SubItems.Add(asins[j].Groups[1].Value);
                                        lv1.SubItems.Add(title[a].Groups[1].Value.Trim());

                                        lv1.SubItems.Add("https://www.amazon." + domain + "/dp/" + asins[a].Groups[1].Value.Trim());
                                        lv1.SubItems.Add(keyword);
                                        lv1.SubItems.Add(review[a].Groups[1].Value.Trim());
                                        lv1.SubItems.Add(star[a].Groups[1].Value.Trim());
                                        
                                        if (status == false)
                                        {
                                            label1.Text = "已停止";
                                            return;
                                        }
                                        while (zanting == false)
                                        {
                                            seconds = 0;
                                            label1.Text = "暂停中.....点击继续 恢复采集";
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。


                                        }

                                    }

                                }
                                catch (Exception ex)
                                {

                                   ex.ToString();
                                }
                            }


                            

                            label1.Text = "采集中..";
                        }
                        Thread.Sleep(2000);

                        
                    }

            }

                label1.Text = "查询结束";




            }
            catch (Exception ex)
            {

            ex.ToString() ;
            }
        }
           


    public void getdetail()
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string asin = listView1.Items[i].SubItems[1].Text;
                string aurl = "https://www.amazon.com/dp/" + asin + "/";

                string strhtml = method.gethtml(aurl, "");
                string available = "0";
                if(strhtml.Contains("Currently unavailable"))
                {
                    available = "1";
                }

                Match title = Regex.Match(strhtml, @"<span id=""productTitle"" class=""a-size-large product-title-word-break"">([\s\S]*?)</span>");
                Match review = Regex.Match(strhtml, @"acrCustomerReviewText"" class=""a-size-base"">([\s\S]*?) ");
                Match star = Regex.Match(strhtml, @"class=""a-size-medium a-color-base"">([\s\S]*?)</span>");
                Match avalable = Regex.Match(strhtml, @"Amazon.com:([\s\S]*?)""");
                listView1.Items[i].SubItems[2].Text = title.Groups[1].Value.Trim();
                listView1.Items[i].SubItems[3].Text = review.Groups[1].Value.Trim();
                listView1.Items[i].SubItems[4].Text = star.Groups[1].Value.Trim();
                listView1.Items[i].SubItems[5].Text = aurl;
                listView1.Items[i].SubItems[7].Text = available;
            }
            MessageBox.Show( "查询结束");
        }




        public void run()
        {



            MatchCollection asins = Regex.Matches(html, @"<div data-asin=""([\s\S]*?)""");

            if (asins.Count == 0)
            {
                shuju = false;
                return;
            }

            for (int j = 0; j < asins.Count; j++)
            {
                if (asins[j].Groups[1].Value != "")
                {
                    
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(asins[j].Groups[1].Value);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(key);
                    lv1.SubItems.Add("");




                }


            }




        }

        bool shuju = true;  //判断页码是否有数据
        bool status = false;
        public static string html; //网页源码传值
       
        ArrayList pageKeyList = new ArrayList();

        string key = "";

        public void qishi()
        {
            

            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in keywords)
            {


                key = keyword;
                for (int i = 1; i < 2; i++)
                {
                    if (shuju == false)
                    {
                        return;
                    }


                    status = false;
                    webBrowser1.Navigate("https://www.amazon." + domain + "/s?k=" + System.Web.HttpUtility.UrlEncode(keyword) + "&page=" + i);



                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    // Thread.Sleep(1000);
                }


            }

           
            Thread thread = new Thread(new ThreadStart(getdetail));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        Thread thread;
        string domain = "com";
        string bukeshou = "currently unavailable";
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"amazondetail"))
            {
                MessageBox.Show("验证失败");
                return;
            }

            #endregion

            
            switch (comboBox1.Text.Trim())
            {
                case "美国":
                    domain = "com";
                    bukeshou = "Currently unavailable";
                    break;

                case "加拿大":
                    domain = "ca";
                    bukeshou = "Currently unavailable";
                    break;
                case "英国":
                    domain = "co.uk";
                    bukeshou = "Currently unavailable";
                    break;
                case "法国":
                    domain = "fr";
                    bukeshou = "Actuellement indisponible";
                    break;
                case "西班牙":
                    domain = "es";
                    bukeshou = "No disponible";
                    break;
                case "德国":
                    domain = "de";
                    bukeshou = "Derzeit nicht verfügbar";
                    break;
                case "意大利":
                    domain = "it";
                    bukeshou = "Non disponibile";
                    break;
                case "日本":
                    domain = "ip";
                    bukeshou = "No disponible";
                    break;


            }

           
                if (thread == null || !thread.IsAlive)
                {
                       timer1.Start();
                    thread = new Thread(main);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            
           
        }

        private void 亚马逊商品页_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            //string str = Interaction.InputBox("请输入密码登录", "请输入密码登录", "软件密码", -1, -1);
            //if (str != "147258")
            //{
            //    MessageBox.Show("密码错误");
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //}
        }
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;

            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.DocumentText;

                run();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true);
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗？", "清空", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                listView1.Items.Clear();
            }
            else
            {

            }
        }

        public  string username="";

        private void 亚马逊商品页_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                GetUrl("http://111.229.244.97/do.php?method=xiugai&username=" + username + "&islogin=0", "utf-8");
                //Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
            
        }
        int seconds = 0;
        bool zanting = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (zanting == true)
            {
                seconds = seconds + 1;
                if (seconds == 1200)
                {
                    zanting = false;
                    method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true);
                }
            }

            if (zanting == false)
            {
                label1.Text = "暂停中.....点击继续 恢复采集";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "采集中......";
            zanting = true;
        }
    }
}
