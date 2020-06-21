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

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

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

            try
            {



                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < 51; i++)
            {
                string url = "https://feed.mix.sina.com.cn/api/roll/get?pageid=153&lid=2968&k=&num=50&page="+i+"&r=0.5994593305544764&callback=jQuery1112040224612137885263_1590907640770&_=1590907640781";

                string html = GetUrl(url);
                MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");

               
                //int intLong = 0;
                //int a = 1;
                for (int j = 0; j < titles.Count; j++)
                {
                    string tt = Unicode2String(titles[j].Groups[1].Value);
                       
                        label2.Text = tt;
                        if (tt != "" && !lists.Contains(tt))
                        {
                            lists.Add(tt);
                            sb.Append(tt + "\r\n");
                            
                        }
                        //intLong = System.Text.Encoding.Default.GetByteCount(sb.ToString());
                        //if (intLong >50000)
                        //{

                        //    FileStream fs1 = new FileStream(path + a + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        //    StreamWriter sw = new StreamWriter(fs1);
                        //    sw.WriteLine(sb.ToString());
                        //    sw.Close();
                        //    fs1.Close();
                        //    a = a + 1;
                        //    sb.Clear();



                        //}
                              
                

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }
                    

                }

                label2.Text = "完毕";
                FileStream fs1 = new FileStream(path +"结果.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(sb.ToString());
                sw.Close();
                fs1.Close();
               
                sb.Clear();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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
