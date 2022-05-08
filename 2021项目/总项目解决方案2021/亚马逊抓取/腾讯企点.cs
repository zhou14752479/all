using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 亚马逊抓取
{
    public partial class 腾讯企点 : Form
    {
        public 腾讯企点()
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
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion
        public static IWebDriver getdriver(bool headless)
        {
            //关闭cmd窗口
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;


            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = "Chrome/Application/chrome.exe";
            //禁用图片
           // options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            if (headless)
            {
                //options.AddArgument("window-size=1920,1080");
                options.AddArgument("--headless");

            }
            //options.AddArgument("--disable-gpu");

            return new ChromeDriver(driverService, options);
        }


        IWebDriver driver;
        private void button1_Click(object sender, EventArgs e)
        {
            driver = getdriver(false);
            driver.Navigate().GoToUrl("https://console.qidian.qq.com/spa/cl/cusbase/#/cusBase/view_1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            driver.FindElement(By.XPath("//*[@id=\"adminNewCusbase\"]/div/div[1]/div[3]/div/div/div[2]/div[1]/div[1]/ul/li[5]/a")).Click();
            Thread.Sleep(1000);
            for (int i = 0; i < 999999; i++)
            {
                if(driver.PageSource.Contains("number\">"+textBox1.Text+"</a>"))
                {
                    break;
                }
                driver.FindElement(By.XPath("//*[@id=\"adminNewCusbase\"]/div/div[1]/div[3]/div/div/div[2]/div[1]/div[1]/ul/li[6]/a")).Click();


                Thread.Sleep(1000);
            }
        }

        private void 腾讯企点_Load(object sender, EventArgs e)
        {
            //#region 通用检测


            //if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"WcMa"))
            //{
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //    return;
            //}

            //#endregion
        }
    }
}
