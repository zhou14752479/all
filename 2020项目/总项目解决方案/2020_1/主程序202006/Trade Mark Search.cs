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
using helper;

namespace 主程序202006
{
    public partial class Trade_Mark_Search : Form
    {
        public Trade_Mark_Search()
        {
            InitializeComponent();
        }

        bool zanting = true;
        string cookie = "JSESSIONID=B15F89F7E214D3A08B3FCEEC83E8B872; GJ8UAX99J13WPQI51=!OviP5hsP1+m74A5tJI9B8RL6BQjqQnh48wVX6o66z99ncY3ul24t8WAkeRJnuwXyMFejmWpvyLLHyjkidoI85vugiKVlO8tyWm50J4I=";
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
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
               
                request.Referer = "https://esearch.ipd.gov.hk/nis-pos-view/tm";
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
        #region 主程序
        public void run()
        {

            try
            {

                int start = Convert.ToInt32(textBox1.Text);
                int end = Convert.ToInt32(textBox2.Text);

                for (int i = 1; i < 99999; i++)
                {


                    string url = "https://esearch.ipd.gov.hk/nis-pos-view/tm/search/?page="+i+"&rows=10";
                    string postdata = "{\"applicationNumber\":[\""+start+"\",\""+end+"\"],\"filingDate\":{},\"documentFilingDate\":{},\"registrationDate\":{},\"expirationDate\":{},\"publicationDateOfAcceptance\":{},\"actualRegistrationDate\":{},\"searchMethod\":\"TM_SEARCHMETHOD_WILDCARD\",\"isDeadRecordIndicator\":\"false\"}";
                    string strhtml = PostUrl(url,postdata,cookie,"utf-8") ;
            
                  
                    MatchCollection ids = Regex.Matches(strhtml, @"""id"":""([\s\S]*?)""");

                    if (ids.Count == 0)
                    {
                        label4.Text = "查询结束" + "......";
                        return;
                    }


                    foreach (Match uid in ids)
                    {
                        string aurl = "https://esearch.ipd.gov.hk/nis-pos-view/tm/details/view/" + uid.Groups[1].Value + "/0/0/1/10/0/1/0/null_null/KCFeIShhcHBsaWNhdGlvbk51bWJlcjpbMCBUTyA0MDAwMDAwMF0pIV4hIEFORCAhXiEoaXNEZWFkUmVjb3JkSW5kaWNhdG9yOihmYWxzZSkpIV4hKSBBTkQgdG1SZWNvcmRTZXE6MQ==";
                        string html = method.gethtml(aurl, cookie);
                        label4.Text = "正在查询" + uid.Groups[1].Value;

                        Match a1 = Regex.Match(html, @"Trade Mark No\.:([\s\S]*?)</dd>");
                        Match a2 = Regex.Match(html, @"Status:([\s\S]*?)</dd>");
                        Match a3 = Regex.Match(html, @"Trade Mark Text:([\s\S]*?)</dd>");
                        Match a4 = Regex.Match(html, @"Mark Type:([\s\S]*?)</dd>");
                        Match a5 = Regex.Match(html, @"Class No\.:([\s\S]*?)</dd>");
                        Match a6 = Regex.Match(html, @"<div class=""panel-body"" ng-bind-html=""'([\s\S]*?)'");
                        Match a7 = Regex.Match(html, @"Others:</span>([\s\S]*?)html=""'([\s\S]*?)'");
                        Match a8 = Regex.Match(html, @"Date of Filing:([\s\S]*?)</dd>");
                        Match a9 = Regex.Match(html, @"Date of Advertisement in Gazette:([\s\S]*?)</dd>");
                        Match a10 = Regex.Match(html, @"Name:</span>([\s\S]*?)</dd>");
                        Match a11 = Regex.Match(html, @"Address:</span>([\s\S]*?)html=""'([\s\S]*?)'");
                        Match a12 = Regex.Match(html, @"Address for Service:</span>([\s\S]*?)html=""'([\s\S]*?)'");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\\\n", " ").Trim());
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[2].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\\\n", " ").Trim());
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a11.Groups[2].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\","").Trim());
                        lv1.SubItems.Add(Regex.Replace(a12.Groups[2].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\", "").Trim());

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        Thread.Sleep(500);


                    }



                }
                label4.Text = "查询结束" + "......";
            }



            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion
        private void Trade_Mark_Search_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE11);
            webBrowser1.Url = new Uri("https://esearch.ipd.gov.hk/nis-pos-view/tm#/quicksearch");
           
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://esearch.ipd.gov.hk/nis-pos-view/tm/allSelectableTypes?lang=en");
            //MessageBox.Show(cookie);
            label4.Text = "开始查询" + "......";
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Trade_Mark_Search_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
