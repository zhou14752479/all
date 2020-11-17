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

namespace 启动程序
{
    public partial class 图片下载 : Form
    {
        public 图片下载()
        {
            InitializeComponent();
        }


        
  



        #region 下载文件
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch
            {


            }
        }
        #endregion
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url,string COOKIE)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://m.mm131.net/chemo/89_5.html";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
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

        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 图片下载_Load(object sender, EventArgs e)
        {
        }
        bool zanting = true;

        public void run()
        {
            try
            {
                string cookie = "Hm_lvt_672e68bf7e214b45f4790840981cdf99=1586074013; UM_distinctid=1714960cd47138-019f4ff4f49f88-2393f61-1fa400-1714960cd48262; CNZZDATA1277874215=493529657-1586070373-%7C1586075773; Hm_lpvt_672e68bf7e214b45f4790840981cdf99=1586076817";
                for (int i = 0; i < 999; i++)
                {
                    string url = "https://m.mm131.net/more.php?page=" + i;
                    string html = GetUrl(url, cookie);
                    MatchCollection uids = Regex.Matches(html, @"data-img=""https:\/\/img1\.mmmw\.net\/pic\/([\s\S]*?)\/");
                    MatchCollection names = Regex.Matches(html, @"alt=""([\s\S]*?)""");
                    for (int a = 0; a < uids.Count; a++)
                    {
                        string name = System.Web.HttpUtility.UrlDecode(names[a].Groups[1].Value);
                        string sPath = path + name + "/";
                        if (!Directory.Exists(sPath))
                        {
                            Directory.CreateDirectory(sPath); //创建文件夹
                        }

                        for (int j = 1; j < 99;j++)
                        {
                            
                            downloadFile("https://img1.mmmw.net/pic/" + uids[a].Groups[1].Value + "/" + j + ".jpg",sPath,j+".jpg",cookie);

                            textBox1.Text += "正在下载："+name+"  第"+j+"张"+"\r\n";
                            this.textBox1.Focus();
                            this.textBox1.Select(this.textBox1.TextLength, 0);
                            this.textBox1.ScrollToCaret();

                            Thread.Sleep(100);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }

                    }

                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"tupianxiazai"))
            {
                if (zanting == false)
                {
                    zanting = true;
                }
                else
                {
                    Thread search_thread = new Thread(new ThreadStart(run));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    search_thread.Start();
                }
               

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion

           



        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void 图片下载_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
