using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 家具评论生成
{
    public partial class 家具评论生成 : Form
    {
        public 家具评论生成()
        {
            InitializeComponent();
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
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
        //查询
        public string chaxun(int value)
        {
            string sql = "select content from data where id= " + value;
            string path = System.Environment.CurrentDirectory+"\\win32"; //获取当前程序运行文件夹

            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\api.db");
            mycon.Open();

            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

          
            return cmd.ExecuteScalar().ToString();



        }


        


        public void run()
        {
            string html = GetUrl("https://www.baidu.com/", "utf-8");
            if (html == "")
            {
                return;
            }

            label1.Text =DateTime.Now.ToString()+ "正在调用接口词库......";
            List<string> contentlist = new List<string>();
            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            Random rd = new Random();


            for (int i =1; i < 10500; i=i+rd.Next(0,10))
            {
                string pinglun = chaxun(i);
                for (int j = 0; j < keywords.Length; j++)
                {

                    if (pinglun.Contains(keywords[j]))
                    {
                        if (!contentlist.Contains(pinglun))
                        {
                            contentlist.Add(pinglun);
                            break;
                        }
                    }

                }
                if (contentlist.Count >10)
                {
                    label1.Text = DateTime.Now.ToString()+ "正在生成......";
                    foreach ( string item in contentlist)
                    {
                        string suijici = keywords[rd.Next(0, keywords.Length)] + "很好，";
                        string text=item.Replace("自营", "这家");

                        //text = Regex.Replace(item, "，.*，", suijici);
                       

                        text = text.Replace("自营","这家店");
                        textBox2.Text += text+ "\r\n";
                        Thread.Sleep(2000);
                    }
                    break;
                }
                

            }

           
            if (contentlist.Count > 1 && contentlist.Count <= 10)
            {
                foreach (string item in contentlist)
                {
                    label1.Text = DateTime.Now.ToString() + "正在生成......";
                    // rd.Next(0,keywords.Length)+
                    textBox2.Text += item + "\r\n" + "\r\n";
                    Thread.Sleep(2000);
                }
               
            }

            label1.Text = DateTime.Now.ToString() + "完成";
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
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
            textBox2.Text = "";
        }
    }
}
