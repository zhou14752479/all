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

namespace wuba
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            try
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "usid=07WwkCu3b_78aUPT; IPLOC=CN3213; SUV=00BA2DBC3159B8CD5D2585534E6EA580; CXID=5EA7E0DBFC0F423A95BC1EB511A405C7; SUID=CDB859313118960A000000005D25B077; ssuid=7291915575; pgv_pvi=5970681856; start_time=1562896518693; front_screen_resolution=1920*1080; wuid=AAElSJCaKAAAAAqMCGWoVQEAkwA=; FREQUENCY=1562896843272_13; sg_uuid=6358936283; newsCity=%u5BBF%u8FC1; SNUID=9FB9A0C8F8FC6C9FCB42F1E4F9BFB645; sortcookie=1; sw_uuid=3118318168; ld=3Zllllllll2NrO7hlllllVLmmtGlllllGqOxBkllllwlllllVklll5@@@@@@@@@@; sct=20";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://news.sogou.com/news?query=site%3Asohu.com+%B4%F3%CA%FD%BE%DD&_ast=1571813760&_asf=news.sogou.com&time=0&w=03009900&sort=1&mode=1&manual=&dp=1";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            //request.AllowAutoRedirect = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.Headers.Add("Cookie", "");
            request.Referer = "http://data.imiker.com/all_search/hs/buy/all/621210";
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

        #endregion

        bool zanting = true;
        public string userkey;

        public string shibie(string picdata)
        {
            string url = "http://api.dididati.com/v3/upload/base64";
            string postdata = "image="+ System.Web.HttpUtility.UrlEncode(picdata) + "&userkey="+userkey;
            string html = PostUrl(url,postdata);
            return html;
        }


        #region  主程序
        public void run()
        {
            try

            {
                for (int i = 1; i < 21; i++)
                {

                    string url = "https://suqian.58.com/job/pn" + i + "/";

                    string html = GetUrl(url);
                    MatchCollection TitleMatchs = Regex.Matches(html, @"j_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in TitleMatchs)
                    {
                        lists.Add(NextMatch.Groups[3].Value);
                    }
                    foreach (string list in lists)
                    {
                        //结束
                        string URL = "https://wxapp.58.com/phone/get?infoId="+list+"&cateCode=5&legoKey=&thirdKey=y7m9ffkHt584LzETkA7EljxqzMUwKShzGgUJAsy7nfw5Uzn6kTdexpRPvgnBfebO&appCode=21";
                        string strhtml = GetUrl(URL);
                        Match image = Regex.Match(strhtml, @"img\\"":\\""([\s\S]*?)\\""");
                      
                        Match responsId = Regex.Match(strhtml, @"responseid\\"":\\""([\s\S]*?)\\""");

                        string data = shibie(image.Groups[1].Value);
                        Match code= Regex.Match(data, @"""code"":""([\s\S]*?)""");
                      
                        string ahtml = GetUrl("https://wxapp.58.com/phone/vccheck?infoId="+list+"&cateCode=5&rid="+responsId.Groups[1].Value+"&vc="+code.Groups[1].Value+"&thirdKey=y7m9ffkHt584LzETkA7EljxqzMUwKShzGgUJAsy7nfw5Uzn6kTdexpRPvgnBfebO&appCode=21");

                        Match tel= Regex.Match(ahtml, @"result\\"":\\""([\s\S]*?)\\""");
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(list);
                        lv1.SubItems.Add(tel.Groups[1].Value);


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);


                    }
                }
                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            string html = GetUrl("http://api.dididati.com/v3/user/login?username=" + textBox1.Text + "&password=" + textBox2.Text);

            Match a1 = Regex.Match(html, @"""userkey"":""([\s\S]*?)""");
            if (a1.Groups[1].Value == "")
            {
                MessageBox.Show(html);
            }
            else
            {
               
                userkey = a1.Groups[1].Value;
                button1.Text = "登陆成功";
            }
         

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
