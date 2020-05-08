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

namespace 主程序1
{
    public partial class 搜搜问问 : Form
    {
        public 搜搜问问()
        {
            InitializeComponent();
        }
        bool zanting = true;
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
                string COOKIE = "BIDUPSID=35F7904BBBAFB976E9BF12373C531E13; PSTM=1587179912; BAIDUID=35F7904BBBAFB976D676C974113470C3:FG=1; BDUSS=F0ZWx1V2lyVzVJSTlYdUZ3VjUxZWdWSENjeDBXZElXaGUxQX5sRGdsUFk5OHhlSVFBQUFBJCQAAAAAAAAAAAEAAACys-e7cTg1MjI2NjAxMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANhqpV7YaqVeV2; ZD_ENTRY=empty; Hm_lvt_6859ce5aaf00fb00387e6434e4fcc925=1587284070,1587449363,1587799758,1588509140; shitong_key_id=2; Hm_lpvt_6859ce5aaf00fb00387e6434e4fcc925=1588509389; shitong_data=de206a9319cb7aaa1bdb183cfec95c4ae0a1a6e05afff40873ffcdc2fa1d49e22ce36b153d5bcf1e434e9c438244c0b618cd4b950b8467893a09462ea8ce668c79d4f4a911f5fe03c4a7995f2522037aa3eadb1b0a11bcefc2bacae3b6d71f998816311f502d28c28118e5f77a51c48b446c745e220e36e2bb593a3c1ac63da46f7b91ff2271e21b549a2322e3de48a3; shitong_sign=c0171d4b; PMS_JT=%28%7B%22s%22%3A1588509394458%2C%22r%22%3A%22https%3A//zhidao.baidu.com/search%3Fword%3D%25E9%25BB%2591%25E5%25B8%25BDSEO%26ie%3Dgbk%26site%3D-1%26sites%3D0%26date%3D0%26pn%3D0%22%7D%29";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

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
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void run()
        {
            string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int a = 0; a< array.Length; a++)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    ArrayList articleList = new ArrayList();
                    ArrayList titleList = new ArrayList();
                    for (int i = 1;i < 4; i++)
                    {

                        string URL = "https://www.sogou.com/sogou?query="+System.Web.HttpUtility.UrlEncode(array[a])+"&insite=wenwen.sogou.com&pid=sogou-wsse-a9e18cb5dd9d3ab4&rcer=&page="+i+"&ie=utf8";
                        string html = GetUrl(URL, "utf-8");

                        MatchCollection uids = Regex.Matches(html, @"z%2F([\s\S]*?)&");

                        for (int j = 0; j < uids.Count; j++)
                        {

                            string url = "https://wenwen.sogou.com/z/" + uids[j].Groups[1].Value;
                            string ahtml = GetUrl(url, "utf-8");
                            textBox2.Text += DateTime.Now.ToString() + "：正在抓取第"+i+"页第"+(j+1) +"篇"+ "\r\n";

                            Match title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>");
                            Match article = Regex.Match(ahtml, @"<pre class=""replay-info-txt answer_con"">([\s\S]*?)</pre>");

                            titleList.Add(title.Groups[1].Value);

                            string article1 = Regex.Replace(article.Groups[1].Value, "([0-9]|[a-z]){10,}", "");
                            articleList.Add(Regex.Replace(article1, "<[^>]+>", ""));
                           
                            Thread.Sleep(1000);
                        }
                       




                    }
                    textBox2.Text = "正在合成..........." + "\r\n";


                    try
                    {

                  
                    sb.Append(articleList[1]);
                    sb.Append("\r\n");

                    sb.Append(titleList[0]); sb.Append("\r\n");
                    sb.Append(articleList[0]);
                    sb.Append("\r\n");

                    sb.Append(articleList[5]);
                    sb.Append("\r\n");

                    sb.Append(articleList[14]);
                    sb.Append("\r\n");

                    sb.Append(articleList[18]);
                    sb.Append("\r\n");

                    sb.Append(articleList[22]);
                    sb.Append("\r\n");

                    sb.Append(titleList[12]); sb.Append("\r\n");
                    sb.Append(articleList[12]);
                    sb.Append("\r\n");

                    sb.Append(articleList[10]);
                    sb.Append("\r\n");

                    sb.Append(articleList[4]);
                    sb.Append("\r\n");

                    sb.Append(titleList[19]); sb.Append("\r\n");
                    sb.Append(articleList[19]);

                    }
                    catch
                    {

                        continue;
                    }


                    FileStream fs1 = new FileStream(path + "文件\\" + array[a].Trim() + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                    fs1.Close();

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                }
                catch 
                {

                    continue;
                }

                Thread.Sleep(1000);
            }
            MessageBox.Show("执行结束");


        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 搜搜问问_Load(object sender, EventArgs e)
        {

        }
    }
}
