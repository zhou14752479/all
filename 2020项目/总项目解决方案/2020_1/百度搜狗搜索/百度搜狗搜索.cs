using Microsoft.Win32;
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
using System.Windows.Forms;

namespace 百度搜狗搜索
{
    public partial class 百度搜狗搜索 : Form
    {
        public 百度搜狗搜索()
        {
            InitializeComponent();
        }
        public static void SetFeatures(UInt32 ieMode)
        {
            //传入11000是IE11, 9000是IE9, 只不过当试着传入6000时, 理应是IE6, 可实际却是Edge, 这时进一步测试, 当传入除IE现有版本以外的一些数值时WebBrowser都使用Edge内核
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                throw new ApplicationException();
            }
            //获取程序及名称
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
        }



        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString());
            MessageBox.Show("导出成功");
        }

        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }


        bool zanting = true;

        public static string html;
        bool status = false;
       
        private void Form1_Load(object sender, EventArgs e)
        {
           

            SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser2.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
        }
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           

                if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                    return;
                if (e.Url.ToString() != webBrowser1.Url.ToString())
                    return;

                if (webBrowser1.DocumentText.Contains("</html>") || webBrowser1.DocumentText.Contains("</HTML>"))
                {
                    while (zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }


                    html = webBrowser1.DocumentText;
                    if (comboBox1.Text == "百度")
                    {

                        baidurun();
                    }
                    if (comboBox1.Text == "搜狗")
                    {

                        sougourun();
                    }

                    status = true;

                }
                else
                {
                    Application.DoEvents();
                }
          
        }


        string laiyuanURL = "";
        /// <summary>
        /// 百度主程序
        /// </summary>
        public void baidurun()
        {

            MatchCollection titles = Regex.Matches(html, @"}""><!--s-text-->([\s\S]*?)<!");
            MatchCollection yumings = Regex.Matches(html, @"mu=""http([\s\S]*?)""");
            if (titles.Count == 0)
            {
                zanting = false;
                return; //测试无内容 是否继续执行
            }
           

            for (int i = 0; i < titles.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(titles[i].Groups[1].Value.Replace("<EM>","").Replace("</EM>", ""));
                lv1.SubItems.Add("http"+yumings[i].Groups[1].Value);
                lv1.SubItems.Add(laiyuanURL);

            }


        }

        /// <summary>
        /// 搜狗主程序
        /// </summary>
        public void sougourun()
        {

            MatchCollection titles = Regex.Matches(html, @"}""><!--s-text-->([\s\S]*?)<!");
            MatchCollection yumings = Regex.Matches(html, @"mu=""http([\s\S]*?)""");
            if (titles.Count == 0)
            {
                zanting = false;
                return; //测试无内容 是否继续执行
            }


            for (int i = 0; i < titles.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(titles[i].Groups[1].Value.Replace("<EM>", "").Replace("</EM>", ""));
                lv1.SubItems.Add("http" + yumings[i].Groups[1].Value);
                lv1.SubItems.Add(laiyuanURL);

            }


        }

        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }
        public string GetTimeStamp2(int value)
        {
            TimeSpan tss = DateTime.UtcNow.AddDays(value) - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }
        public void baiduqishi()
        {

           
            string now = GetTimeStamp();
            string start = GetTimeStamp2(-30);
            if (radioButton1.Checked == true)
            {
                start = GetTimeStamp2(-7);
            }



            string html = GetUrl("http://mr1024.hl98.cn/MrLiang_citys.txt","gb2312");
            MatchCollection citys = Regex.Matches(html, @"""citysName"": ""([\s\S]*?)""");
            


            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string keyword in keywords)
            {
                foreach (Match city in citys)
                {
                    

                    string  keyword1 = city.Groups[1].Value + keyword;

                    status = false;
                    string url = "https://www.baidu.com/s?rtt=1&bsst=1&cl=2&tn=news&word=" + System.Web.HttpUtility.UrlEncode(keyword1) + "&pn=00&gpc=stf%3D"+start+"%2C"+now+"%7Cstftype%3D1&tfflag=1";
                 
                   
                    webBrowser1.Navigate(url);

                    laiyuanURL = url;

                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(2000);
                }
                
            }
        }

        public void sougouqishi()
        {


        



            string html = GetUrl("http://mr1024.hl98.cn/MrLiang_citys.txt", "gb2312");
            MatchCollection citys = Regex.Matches(html, @"""citysName"": ""([\s\S]*?)""");



            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string keyword in keywords)
            {
                foreach (Match city in citys)
                {


                    string keyword1 = city.Groups[1].Value + keyword;

                    status = false;
                    string url = "https://www.sogou.com/web?query=" + System.Web.HttpUtility.UrlEncode(keyword1);


                    webBrowser1.Navigate(url);

                    laiyuanURL = url;

                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(2000);
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


            try
            {
               
                string COOKIE = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
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
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"baidusougou"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion


            if (thread == null || !thread.IsAlive)
            {
                if(comboBox1.Text=="百度")
                {
                    thread = new Thread(baiduqishi);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (comboBox1.Text == "搜狗")
                {
                    thread = new Thread(sougouqishi);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView1,true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
         
            if (listView1.SelectedItems.Count > 0)
            {
                webBrowser2.Visible = true;
                webBrowser1.Visible = false;
                webBrowser2.Navigate(listView1.SelectedItems[0].SubItems[3].Text);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Visible = true;
            webBrowser2.Visible = false;
            zanting = true;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            if (listView1.SelectedItems.Count > 0)
            {
                webBrowser2.Visible = true;
                webBrowser1.Visible = false;
                webBrowser2.Navigate(listView1.SelectedItems[0].SubItems[2].Text);

            }
        }
    }
}
