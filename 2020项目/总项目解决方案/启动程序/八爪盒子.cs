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

namespace 启动程序
{
    public partial class 八爪盒子 : Form
    {
        public 八爪盒子()
        {
            InitializeComponent();
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
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization:Basic NzAyNzkwNzc5ZDU4NjJjMmMzYmRlOWVmOWFhZjA1NzQ6MzZiNDA4YTljODJlMWUzYjc4Nzc2ODY2ODk4MzFjNDI=");
                headers.Add("Agent-info: client=ios;osVersion=12.3.1;screenWidth=1242;screenHeight=2208;appVersion=5.1");
                headers.Add("Agent-Info2: BaZhuaHeZiOK");
                //添加头部
               
                request.ContentLength = postData.Length;
                request.KeepAlive = true;
                request.UserAgent = "BaZhuaHeZi-iOS/5.1 Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148";
                
                request.Headers.Add("origin", "http://api.ibole.net");
                request.Referer = "https://accounts.ebay.com/acctxs/user";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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


        public string getcount()
        {
            string url = "https://api.ibole.net/candidate/mine";
            string postdata = "offset=0&pageSize=10&storageDateEnd=" + textBox2.Text + "%2023%3A59%3A59&storageDateStart=" + textBox1.Text + "%2000%3A00%3A00";
            string html = PostUrl(url, postdata);

            Match all = Regex.Match(html, @"""totalCount"":([\s\S]*?)\}");
            return all.Groups[1].Value;
        }
        bool zanting = true;
        bool status = true;
        public void run()

        {
            label5.Text = getcount();
            int yi = 0;
           
            try
            {
                for (int i = 0; i < 100001; i=i+10)
                {


                    string url = "https://api.ibole.net/candidate/mine";
                    string postdata = "offset="+i+"&pageSize=10&storageDateEnd=" + textBox2.Text + "%2023%3A59%3A59&storageDateStart=" + textBox1.Text + "%2000%3A00%3A00";
                    string html = PostUrl(url, postdata);

                    MatchCollection uids = Regex.Matches(html, @"""candidateId"":""([\s\S]*?)""");

                    if (uids.Count == 0)
                        return;
                    foreach (Match uid in uids)
                    {
                        string strhtml = PostUrl("https://api.ibole.net/candidate/detail", "candidateId=" + uid.Groups[1].Value);

                        Match a1 = Regex.Match(strhtml, @"""realName"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(strhtml, @"职位：([\s\S]*?)\\r");
                        Match a3 = Regex.Match(strhtml, @"地址：([\s\S]*?)\\r");
                        Match a4 = Regex.Match(strhtml, @"工作年限：([\s\S]*?)\\r");
                        Match a5 = Regex.Match(strhtml, @"性别：([\s\S]*?)\\r");
                        Match a6 = Regex.Match(strhtml, @"生日：([\s\S]*?)\\r");
                        Match a7 = Regex.Match(strhtml, @"""companyName"":""([\s\S]*?)""");
                        Match a8 = Regex.Match(strhtml, @"""school"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(strhtml, @"专业：([\s\S]*?)\\r");
                        Match a10 = Regex.Match(strhtml, @"学位：([\s\S]*?)\\r");
                        Match a11 = Regex.Match(strhtml, @"手机：([\s\S]*?)\\r");
                        Match a12 = Regex.Match(strhtml, @"邮箱：([\s\S]*?)\\r");
                        Match a13 = Regex.Match(strhtml, @"自我评价：([\s\S]*?)\\r");
                        Match a14 = Regex.Match(strhtml, @"求职意向\\r\\n([\s\S]*?)  ([\s\S]*?) ");
                        //Match a15 = Regex.Match(strhtml, @"自我评价：([\s\S]*?)\\r");
                        //Match a16 = Regex.Match(strhtml, @"语言技能\\r\\n([\s\S]*?) ");


                        Match jiaoyus = Regex.Match(strhtml, @"教育背景\\r\\n([\s\S]*?)求职意向");
                        Match xiangmus = Regex.Match(strhtml, @"工作经验\\r\\n([\s\S]*?)""");
                        //string[] jiaoyu = jiaoyus.Groups[1].Value.Split(new string[] { "\\r\\n" }, StringSplitOptions.None);


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1.Groups[1].Value);
                        lv1.SubItems.Add(a2.Groups[1].Value);
                        lv1.SubItems.Add(a3.Groups[1].Value);
                        lv1.SubItems.Add(a4.Groups[1].Value);
                        lv1.SubItems.Add(a5.Groups[1].Value);
                        lv1.SubItems.Add(a6.Groups[1].Value);
                        lv1.SubItems.Add(a7.Groups[1].Value);
                        lv1.SubItems.Add(a8.Groups[1].Value);
                        lv1.SubItems.Add(a9.Groups[1].Value);
                        lv1.SubItems.Add(a10.Groups[1].Value);
                        lv1.SubItems.Add(a11.Groups[1].Value);
                        lv1.SubItems.Add(a12.Groups[1].Value);
                        lv1.SubItems.Add(a13.Groups[1].Value);
                        lv1.SubItems.Add(a14.Groups[1].Value);
                        lv1.SubItems.Add(a14.Groups[2].Value);

                        lv1.SubItems.Add(Regex.Replace(jiaoyus.Groups[1].Value, @"语言技能。*", ""));
                        lv1.SubItems.Add(xiangmus.Groups[1].Value);
                        yi = yi + 1;
                        label6.Text = yi.ToString();

                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                        if (status == false)
                            return;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

       

        private void 八爪盒子_Load(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"bazhuahezi"))
            {

                status = true;
                Thread thread1 = new Thread(new ThreadStart(run));
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
