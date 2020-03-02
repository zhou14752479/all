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

namespace 百度知道
{
    public partial class Form1 : Form
    {
        public Form1()
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
        public static string GetUrl(string Url,string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
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
            for (int i = 0; i < array.Length; i++)
            {
                try
                {

                    string URL = "https://zhidao.baidu.com/search?lm=0&rn=10&pn=0&fr=search&ie=gbk&word=" + System.Web.HttpUtility.UrlEncode(array[i].Trim());


                    string html = GetUrl(URL,"GBK");
                    
                    MatchCollection uids = Regex.Matches(html, @"data-rank=""([\s\S]*?):([\s\S]*?)""");

                    string url0 = "https://zhidao.baidu.com/question/"+ uids[0].Groups[2].Value+ ".html";
                    string url1 = "https://zhidao.baidu.com/question/" + uids[1].Groups[2].Value + ".html";
                    string url2 = "https://zhidao.baidu.com/question/" + uids[2].Groups[2].Value + ".html";



                    string html0 = GetUrl(url0,"gbk");
                    string html1 = GetUrl(url1,"gbk");
                    string html2 = GetUrl(url2, "gbk");
                    textBox2.Text = html0;

                    Match article0= Regex.Match(html0, @"<span class=""wgt-best-arrowdown""></span>([\s\S]*?)<div class=""quality-content-view-more mb-15"">");
                    Match article1 = Regex.Match(html1, @"<span class=""wgt-best-arrowdown""></span>([\s\S]*?)<div class=""quality-content-view-more mb-15"">");
                    Match article2 = Regex.Match(html2, @"<span class=""wgt-best-arrowdown""></span>([\s\S]*?)<div class=""quality-content-view-more mb-15"">");

                    StringBuilder sb = new StringBuilder();
                    sb.Append(Regex.Replace(article0.Groups[1].Value, "<[^>]+>", ""));
                    
                    sb.Append(Regex.Replace(article1.Groups[1].Value, "<[^>]+>", ""));
                    
                    sb.Append(Regex.Replace(article2.Groups[1].Value, "<[^>]+>", ""));

                   
                    FileStream fs1 = new FileStream(path + array[i].Trim()+".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                    fs1.Close();

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }

                Thread.Sleep(1000);
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
