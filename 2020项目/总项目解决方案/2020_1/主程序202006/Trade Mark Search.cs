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
using myDLL;

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
        public static string PostUrl(string url, string postData, string COOKIE, string charset,string token)
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
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("X-XSRF-TOKEN:"+token);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
               
                request.Referer = "https://esearch.ipd.gov.hk/nis-pos-view/";
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
                string token = Regex.Match(cookie, @"XSRF-TOKEN=([\s\S]*?);").Groups[1].Value;
                if (token == "")
                {
                    token = Regex.Match(cookie, @"XSRF-TOKEN=.*?").Groups[0].Value.Replace("XSRF-TOKEN=","");
                }
                for (int i = 1; i < 99999; i++)
                {


                    string url = "https://esearch.ipd.gov.hk/nis-pos-view/tm/search/?page="+i+"&rows=10";
                    //string postdata = "{\"applicationNumber\":[\""+start+"\",\""+end+"\"],\"filingDate\":{},\"documentFilingDate\":{},\"registrationDate\":{},\"expirationDate\":{},\"publicationDateOfAcceptance\":{},\"actualRegistrationDate\":{},\"searchMethod\":\"TM_SEARCHMETHOD_WILDCARD\",\"isDeadRecordIndicator\":\"false\"}";
                    string postdata = "{\"searchMethod\":\"TM_SEARCHMETHOD_WILDCARD\",\"filingDate\":{},\"documentFilingDate\":{},\"applicationNumber\":[\""+start+"\",\""+end+"\"],\"registrationDate\":{},\"isDeadRecordIndicator\":\"false\",\"expirationDate\":{},\"publicationDateOfAcceptance\":{},\"actualRegistrationDate\":{}}";
                    string strhtml = PostUrl(url,postdata,cookie,"utf-8",token) ;

                    
                    MatchCollection ids = Regex.Matches(strhtml, @"""id"":""([\s\S]*?)""");

                    if (ids.Count == 0)
                    {
                        label4.Text = "查询结束" + "......";
                        return;
                    }


                    foreach (Match uid in ids)
                    {
                        string aurl = "https://esearch.ipd.gov.hk/nis-pos-view/tm/details/view/" + uid.Groups[1].Value + "/0/0/1/10/0/1/0/null_null/KCFeIShhcHBsaWNhdGlvbk51bWJlcjpbMCBUTyA0MDAwMDAwMF0pIV4hIEFORCAhXiEoaXNEZWFkUmVjb3JkSW5kaWNhdG9yOihmYWxzZSkpIV4hKSBBTkQgdG1SZWNvcmRTZXE6MQ%3D%3D";
                        string html = method.GetUrlWithCookie(aurl, cookie,"utf-8");
                       // textBox3.Text = strhtml;
                        label4.Text = "正在查询" + uid.Groups[1].Value;

                        Match a1 = Regex.Match(html, @"""trademarkNumber"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"""status"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(html, @"""markText"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(html, @"""markTypeBag"":\[([\s\S]*?)\]");
                        Match a5 = Regex.Match(html, @"""classNumber"":\[([\s\S]*?)\]");
                        Match a6 = Regex.Match(html, @"""specificationList"":\[([\s\S]*?)\]");
                        Match a7 = Regex.Match(html, @"""dlco"":""([\s\S]*?)""");

                        Match a8 = Regex.Match(html, @"""dateOfFiling"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(html, @"""dateOfAdvertisementInGazette"":""([\s\S]*?)""");
                       string dateOfPublicationForRegistration = Regex.Match(html, @"""dateOfPublicationForRegistration"":""([\s\S]*?)""").Groups[1].Value;
                        string dateOfRegistration = Regex.Match(html, @"""dateOfRegistration"":""([\s\S]*?)""").Groups[1].Value;
                        string actualRegistrationDate = Regex.Match(html, @"""actualRegistrationDate"":""([\s\S]*?)""").Groups[1].Value;
                        string ExpireDate = Regex.Match(html, @"""expiryDate"":""([\s\S]*?)""").Groups[1].Value;



                        Match a10 = Regex.Match(html, @"""ownerName"":""([\s\S]*?)""");
                        Match a11 = Regex.Match(html, @"""ownerAddress"":""([\s\S]*?)""");
                        Match a12 = Regex.Match(html, @"""ownerAddressForService"":""([\s\S]*?)""");
                        string a13 = Regex.Match(html, @"""agentDetails"":""([\s\S]*?)""").Groups[1].Value.Replace("\\n", " ").Replace("\\", "").Replace("\"", "").Trim();



                        string endorsementSectionTypes = Regex.Match(html, @"endorsementSectionTypes([\s\S]*?)\]").Groups[1].Value;
                        //詳細背景資料／批註事項 Historical Details/Endorsement
                        MatchCollection endorsementDate = Regex.Matches(endorsementSectionTypes, @"""endorsementDate"":""([\s\S]*?)""");
                        MatchCollection endorsementTitle = Regex.Matches(endorsementSectionTypes, @"""endorsementTitle"":""([\s\S]*?)""");
                        StringBuilder endorsementDate_sb = new StringBuilder();
                        StringBuilder endorsementTitle_sb = new StringBuilder();
                        for (int a = 0; a < endorsementDate.Count; a++)
                        {
                            endorsementDate_sb.Append(endorsementDate[a].Groups[1].Value+"\n");
                            endorsementTitle_sb.Append(endorsementTitle[a].Groups[1].Value+"\n");
                        }


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim().Replace("\"",""));
                        lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim().Replace("\"", ""));
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\\\n", " ").Replace("\"", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\\\n", " ").Replace("\"", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(dateOfPublicationForRegistration);
                        lv1.SubItems.Add(dateOfRegistration);
                        lv1.SubItems.Add(actualRegistrationDate);
                        lv1.SubItems.Add(ExpireDate);




                        lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a11.Groups[1].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\","").Replace("\"", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "").Replace("\\n", " ").Replace("\\", "").Replace("\"", "").Trim());
                        lv1.SubItems.Add(a13);
                        lv1.SubItems.Add(endorsementDate_sb.ToString());
                        lv1.SubItems.Add(endorsementTitle_sb.ToString());
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
           
            webBrowser1.Url = new Uri("https://esearch.ipd.gov.hk/nis-pos-view/#/");
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            cookie = method.GetCookies("https://esearch.ipd.gov.hk/nis-pos-view/tm#/quicksearch");
            
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
