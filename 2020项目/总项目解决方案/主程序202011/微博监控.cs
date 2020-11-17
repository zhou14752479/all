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


namespace 主程序202011
{
    public partial class 微博监控 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);
        public 微博监控()
        {
            InitializeComponent();
        }

        ArrayList finishes = new ArrayList();
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"weibojiankong"))
            {
                MessageBox.Show("");
                return;
            }

            #endregion
            timer1.Start();
        }
        #region unicode转中文
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        #endregion;
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.tianyancha.com/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

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


        public string panduan(string weibo)
        {
            string keyword = "";
            string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string key in text)
            {

                if (key != "")
                {
                    if (weibo.Contains(key))
                    {
                        return key;
                    }

                }

            }

            return keyword;
        }


        public void run()
        {
           
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string uid in text)
            {
               
                if (uid != "")
                {
                    string URL = "https://m.weibo.cn/api/container/getIndex?profile_ftype[]=1&profile_ftype[]=1&is_all[]=1%3Fprofile_ftype%3D1&is_all[]=1&is_all[]=1&jumpfrom=weibocom&type=uid&containerid=107603"+uid;
                    string html = GetUrl(URL);  //定义的GetRul方法 返回 reader.ReadToEnd()

                    Match name = Regex.Match(Unicode2String(html), @"""screen_name"":""([\s\S]*?)""");
                    MatchCollection weibo = Regex.Matches(Unicode2String(html), @"""raw_text"":""([\s\S]*?)""");

                    for (int i = 0; i <2; i++)
                    {
                        try
                        {
                            string keyword = panduan(weibo[i].Groups[1].Value);
                            if (keyword != "")
                            {
                                if (!finishes.Contains(weibo[i].Groups[1].Value))
                                {
                                    finishes.Add(weibo[i].Groups[1].Value);
                                    Beep(800, 1200);
                                    MessageBox.Show("新微博提醒符合关键字：" + keyword);
                                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                    listViewItem.SubItems.Add(name.Groups[1].Value);
                                    listViewItem.SubItems.Add(keyword);
                                    listViewItem.SubItems.Add(weibo[i].Groups[1].Value);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString()); ;
                        }
                      
                    }
                    

                }



            }

        }




        Thread thread;
        private void timer1_Tick(object sender, EventArgs e)
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
          
            timer1.Stop();
        }

        private void 微博监控_Load(object sender, EventArgs e)
        {

        }
    }
}
