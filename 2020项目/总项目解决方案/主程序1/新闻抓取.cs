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
    public partial class 新闻抓取 : Form
    {
        public 新闻抓取()
        {
            InitializeComponent();
        }

        #region GET请求解决基础连接关闭无法获取HTML
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            string outStr = "";
            string tmpStr = "";

            string outStr1 = "";
            string tmpStr1 = "";

            try
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "1s1k453=ysyk_web41; JSESSIONID=B60EE23B15521D87DF4CF1168CB25BE1; UM_distinctid=1704d60505528a-0f30e2260ef312-2393f61-1fa400-1704d6050576ae; CNZZDATA1253333710=885277478-1581844031-%7C1582260061; CNZZDATA1253416210=1453523664-1581844817-%7C1582261788";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));

                StreamReader reader1 = new StreamReader(response.GetResponseStream(), Encoding.Default);

                try
                {//循环获取
                    while ((tmpStr = reader.ReadLine()) != null)
                    {
                        outStr += tmpStr;
                    }
                }
                catch
                {

                }

                try
                {//循环获取
                    while ((tmpStr1 = reader1.ReadLine()) != null)
                    {
                        outStr1 += tmpStr1;
                    }
                }
                catch
                {

                }

                if (!isLuan(outStr)) //判断utf8是否有乱码
                {

                    reader.Close();
                    response.Close();

                    return outStr;
                }

                else
                {

                    reader1.Close();
                    response.Close();

                    return outStr1;
                }


            }
            catch (System.Exception ex)
            {
                
                return ex.ToString();

            }

        }
        #endregion

        static bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }

        bool zanting = true;
        string path = AppDomain.CurrentDomain.BaseDirectory;

        ArrayList lists = new ArrayList();
        /// <summary>
        /// 标题
        /// </summary>
        public void run()
        {
          
            string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                

                string html = GetUrl(array[i]);
                MatchCollection hrefs = Regex.Matches(html, @"<a.*?href=""(.*?)"".*?>(.*?)</a>");

                StringBuilder sb = new StringBuilder();
                int intLong = 0;
                int a = 1;
                for (int j = 0; j < hrefs.Count; j++)
                {
                    try
                    {
                        string URL = hrefs[j].Groups[1].Value;
                        string ahtml = GetUrl(URL);
                        Match title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>");
                        label2.Text = "正在采集："+title.Groups[1].Value;
                        if (title.Groups[1].Value != "" && !lists.Contains(title.Groups[1].Value))
                        {
                            lists.Add(title.Groups[1].Value);
                            sb.Append(title.Groups[1].Value + "\r\n");
                            
                        }
                        intLong = System.Text.Encoding.Default.GetByteCount(sb.ToString());
                        if (intLong >50000)
                        {

                            FileStream fs1 = new FileStream(path + a + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            sw.WriteLine(sb.ToString());
                            sw.Close();
                            fs1.Close();
                            a = a + 1;
                            sb.Clear();



                        }
                       
                       
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                       // continue;
                    }

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }
              

            }

        }

        private void 新闻抓取_Load(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (html.Contains(@"biaotizhuaqu"))
            {
                button2.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            zanting = false;
            //Random ran = new Random();
            //int n = ran.Next(10);
            //MessageBox.Show(n.ToString());
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            zanting = true;
        }
    }
}
