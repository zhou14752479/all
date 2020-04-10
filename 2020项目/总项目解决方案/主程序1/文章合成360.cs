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
    public partial class 文章合成360 : Form
    {
        public 文章合成360()
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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "test_cookie_enable=null; gtHuid=1; __guid=34870781.1373236603797016800.1562739718134.5598; env_webp=1; __huid=10F9yZ1gPUzMQVelDGBbsr0P%2BuTxXQBvlkDMVZDryrZ1Y%3D; __autoShowTip=show; WDTKID=06eb592db18175a8; search_last_kw=%u9ED1%u5E3DSEO%u57F9%u8BAD%u54EA%u5BB6%u597D; test_cookie_enable=null; __sid=9114931.3664770601979494000.1585901622776.2344; opqopq=50fae34f6a17488d8a29b3b71ce6e6e5.1585901628; Q=u%3Dmubh14752479%26n%3D%26le%3D%26m%3DZGp2WGWOWGWOWGWOWGWOWGWOAwN2%26qid%3D2921239753%26im%3D1_t01923d359dad425928%26src%3Dpcw_so_wenda%26t%3D1; T=s%3Ddde4c539a11cad2bde9accbcf719ac34%26t%3D1585902138%26lm%3D%26lf%3D2%26sk%3D1d2e0c1a2a4c3efcba754493a3443ab8%26mt%3D1585902138%26rc%3D%26v%3D2.0%26a%3D1; WDLGDAY=2921239753; count=17; monitor_count=17; __WDFLOGIN=2921239753; search_last_sid=5429749525a5c53e13f3dbc6f95bcbb5; __gid=9114931.522140443.1585898449185.1585902148540.54";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://wenda.so.com/search/?q=%E9%BB%91%E5%B8%BDSEO%E5%9F%B9%E8%AE%AD%E5%93%AA%E5%AE%B6%E5%A5%BD";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                WebHeaderCollection headers = request.Headers;
                headers.Add("Upgrade-Insecure-Requests: 1");
               
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
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion

       

        bool zanting = true;
        bool status = true;
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void saveTxt(string title,string  body)
        {
           

            FileStream fs1 = new FileStream(path + title+".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(body);
            sw.Close();
            fs1.Close();
        }
        #region  主程序
        public void run()
        {


            try
            {
                StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {


                    string Url = "https://wenda.so.com/search/?q="+ System.Web.HttpUtility.UrlEncode(array[i]); ;


                    string html = GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()
                    
                    MatchCollection urls = Regex.Matches(html, @"aId=""([\s\S]*?)""");


                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < urls.Count; j++)
       
                    {
                        string url = "https://wenda.so.com/q/" + urls[j].Groups[1].Value;

                        string strhtml1 = GetUrl(url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                        Match body = Regex.Match(strhtml1, @"<div class=""resolved-cnt src-import"">([\s\S]*?)<div class=""mod-added");

                        string main = Regex.Replace(body.Groups[1].Value, "<(?!/?p)(?!br )[^>]*>", "");  //除了P <br />其他的去掉

                        sb.Append(main.Replace("<p>","").Replace("</p>","\r\n").Replace("<br />", "\r\n"));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                        {
                            return;
                        }
                        if (j < 5)
                        {
                            textBox2.Text += "正在下载" + array[i] + "第" + (j+1) + "篇文章" + "\r\n";
                        }

                      

                        Thread.Sleep(1000);
                    }

                    textBox2.Text += "正在保存"+ array[i] + "文章 "+"\r\n";
                    saveTxt(array[i], sb.ToString());

                }

                textBox2.Text += "已完成";

            }


            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        #endregion
        private void 文章合成360_Load(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"360wenda"))
            {

                MessageBox.Show("验证失败");
                return;


            }

            #endregion
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }
    }
}
