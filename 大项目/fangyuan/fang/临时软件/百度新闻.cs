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

namespace fang.临时软件
{
    public partial class 百度新闻 : Form
    {
        public 百度新闻()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        ArrayList titles = new ArrayList();

        bool zanting = true;
        #region  主函数
        public void baidu()

        {


            try

            {

                for (int i = 1230739200; i < 1547654399; i = i + 86400)
                {
                    int atime = i;
                    int btime = i + 86399;

                    for (int j = 1; j < 50; j++)
                    {
                        string url = "http://news.baidu.com/ns?tn=newstitle&word=%E6%88%BF%E4%BB%B7&pn=" + j + "0&ct=1&tn=news&rn=20&ie=utf-8&bt=" + atime + "&et=" + btime;
                        textBox1.Text = url;
                        string html = method.GetUrl( url,"utf-8");


                        MatchCollection biaotis = Regex.Matches(html, @"<h3 class=""c-title"">([\s\S]*?)</h3>");

                        if (biaotis.Count == 0)
                            break;

                        MatchCollection times = Regex.Matches(html, @"<div class=""c-title-author"">([\s\S]*?)20([\s\S]*?)<");


                        for (int z = 0; z < biaotis.Count; z++)
                        {
                            string value = Regex.Replace(biaotis[z].Groups[1].Value, "<[^>]*>", "");
                            if (!titles.Contains(value))
                            {
                                
                                titles.Add(value);
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(value);
                                lv1.SubItems.Add("20"+times[z].Groups[2].Value.ToString());
                                lv1.SubItems.Add(url);
                                if (listView1.Items.Count - 1 > 0)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                            }

                            
                        }

                    }

                }
            }

            catch (Exception ex)
            {
              MessageBox.Show(  ex.ToString());
            }
        }

        #endregion

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        #region  点评
        public void run()

        {
            string[] ids= textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            try

            {

                foreach (string id in ids)
                {

                    for (int i = 0; i < 1500; i=i+10)
                    {
                        string postData = "{\"moduleInfoList\":[{\"moduleName\":\"reviewlist\",\"query\":{\"shopId\":\""+id+"\",\"offset\":"+i+",\"limit\":10,\"type\":1,\"pageDomain\":\"m.dianping.com\"}}],\"pageEnName\":\"shopreviewlist\"}";
                       
                        string html = PostUrl(postData);

                        
                        MatchCollection reviewIds = Regex.Matches(html, @"reviewId"":([\s\S]*?),");

                        MatchCollection stars= Regex.Matches(html, @"star"":([\s\S]*?),");
                        
                        MatchCollection bodys = Regex.Matches(html, @"reviewBody"":""([\s\S]*?)"",");

                        if (reviewIds.Count == 0)
                            break;
                        for (int j = 0; j < reviewIds.Count; j++)
                        {
                            //string html2 = method.GetUrl("http://www.dianping.com/review/"+reviewIds[j].Groups[1].Value, "utf-8");
                            //textBox2.Text = html2;
                            //Match As = Regex.Match(html2, @"<div class=""review-rank"">([\s\S]*?)环境：([\s\S]*?) ");
                            //Match Bs = Regex.Match(html2, @"<div class=""review-rank"">([\s\S]*?)护理：([\s\S]*?) ");
                            //Match Cs = Regex.Match(html2, @"<div class=""review-rank"">([\s\S]*?)月子餐：([\s\S]*?) ");
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(id.ToString());
                            lv1.SubItems.Add(reviewIds[j].Groups[1].Value.ToString());
                            lv1.SubItems.Add(stars[j].Groups[1].Value.ToString());
                            //lv1.SubItems.Add(As.Groups[2].Value);
                            //lv1.SubItems.Add(Bs.Groups[2].Value);
                            //lv1.SubItems.Add(Cs.Groups[2].Value);
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add(bodys[j].Groups[1].Value);
                            if (listView1.Items.Count - 1 > 0)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }

                            //Thread.Sleep(1000);
                        }


                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void skinButton2_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    Thread thread = new Thread(new ThreadStart(baidu));
            //    thread.Start();
            //}

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();


        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl( string postData)
        {
            string COOKIE = "_lxsdk_cuid=16233c680c7c8-081a8e5d136c39-3b60450b-1fa400-16233c680c7c8; _lxsdk=16233c680c7c8-081a8e5d136c39-3b60450b-1fa400-16233c680c7c8; _hc.v=8165c808-5cfd-45a0-0d36-3e7f9d78b4eb.1521287070; s_ViewType=10; switchcityflashtoast=1; _tr.u=jU5PhtbV9q3sLhwV; Hm_lvt_dbeeb675516927da776beeb1d9802bd4=1521289865; __mta=46068251.1521289881671.1521289895545.1521289900801.3; aburl=1; ua=dpuser_5678141658; ctu=90a81cde43e1e0934a456ec54b747c93d0e6b58b8c9732b3ed676c7795f37d7a; __mta=46068251.1521289881671.1521289900801.1524484488372.4; cy=1; cye=shanghai; dper=64bac4be18dfd707b0badc079694425dde63784b826f4e704aefe0d1c187dc79e838bd69e917174ab7d77264518419c92caa12d142816bb4d5d36e8201ec9c2035bc713eee679dc0d3b6a4751c22b69ccc58f51360d5195f1821452d31949d50; cityid=100; default_ab=shopreviewlist%3AA%3A1; m_flash2=1; msource=default; logan_session_token=xeut2otac1yrbvadx7uv; logan_custom_report=; _lxsdk_s=168caa4597c-bbf-7c2-9ba%7C%7C1";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://m.dianping.com/isoapi/module");
            request.Method = "Post";
            request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            request.AllowAutoRedirect = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

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

        private void 百度新闻_Load(object sender, EventArgs e)
        {

        }
    }
}
