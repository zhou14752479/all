using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Aspose.Cells;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Net;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.Web.WebView2.Core;

namespace 驾校一点通
{
    public partial class 驾校一点通 : Form
    {
        public 驾校一点通()
        {
            InitializeComponent();
        }
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        #region 修改注册表信息使WebBrowser使用指定版本IE内核 传入11000是IE11
        public static void SetFeatures(UInt32 ieMode)
        {
            //传入11000是IE11, 9000是IE9, 只不过当试着传入6000时, 理应是IE6, 可实际却是Edge, 这时进一步测试, 当传入除IE现有版本以外的一些数值时WebBrowser都使用Edge内核
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                throw new ApplicationException();
            }
            //获取程序及名称
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
        }
        #endregion

        #region  获取cookie
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;


                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }

        #endregion


        string cookie = "";
        string link = "";
        private void button1_Click(object sender, EventArgs e)
        {
            
            getcookies();
           
            status = true;
            if (textBox1.Text=="")
            {
                MessageBox.Show("请输入采集链接");
                return;
            }
            link = textBox1.Text;
            this.button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(this.start));
            thread.Start();
        }
        private void start()
        {
            HttpHelper httpHelper = new HttpHelper();
            HttpItem httpItem = new HttpItem();
            Workbook workbook = new Workbook();
            Style style = workbook.Styles[workbook.Styles.Add()];
            style.Font.Name = "宋体";
            style.Font.Size = 12;
            style.VerticalAlignment = TextAlignmentType.Center;
            Worksheet worksheet = workbook.Worksheets[0];
            if (radioButton1.Checked == true)
            {
                this.colle(ref worksheet, style, "科一", this.link);
            }
            if (radioButton2.Checked == true)
            {
                this.colle2(ref worksheet, style, "科一", this.link);
            }
           

            workbook.Worksheets.Add();
            string fileName = string.Concat(new string[]
            {
                Application.StartupPath,
               Path.DirectorySeparatorChar.ToString(),
                "生成结果",
                DateTime.Now.ToString("MM月dd日HH时mm分ss秒"),
                ".xlsx"
            });
            workbook.Save(fileName);
            textBox2.Text=("生成完成");
            button1.Enabled = true;
        }
      
        public void colle(ref Worksheet sheet, Style st, string name, string url)
        {
            HttpHelper httpHelper = new HttpHelper();
            HttpItem httpItem = new HttpItem();
            Cells cells = sheet.Cells;
            string[] array = new string[]
            {
                "站点编号",
                "题型",
                "题目",
                "选项",
                "答案",
                "解答",
                "关联图片",
                "相关图片"
            };
            int num = array.Length;
            checked
            {
                int num2 = num - 1;
                for (int i = 0; i <= num2; i++)
                {
                    cells[0, i].PutValue(array[i]);
                    cells[0, i].SetStyle(st);
                    cells.SetRowHeight(1, 25.0);
                }
              textBox2.Text=("开始读取" + "题目");
                cells = sheet.Cells;
                int num3 = num - 1;
                for (int j = 0; j <= num3; j++)
                {
                    cells[0, j].PutValue(array[j]);
                    cells[0, j].SetStyle(st);
                    cells.SetRowHeight(1, 25.0);
                }
                httpItem.URL = url;
                httpItem.Method = "GET";
                bool flag = !string.IsNullOrEmpty(this.cookie);
                if (flag)
                {
                    httpItem.Cookie = this.cookie;
                }
                httpItem.Allowautoredirect = true;
                HttpResult html = httpHelper.GetHtml(httpItem);
                string text = html.Html;
                MatchCollection ids = Regex.Matches(html.Html, @"data-id=""([\s\S]*?)""");


                for (int num4 = 0; num4 < ids.Count; num4++)
                {

                    if (status == false)
                        return;
                    try
                    {
                  
                            url = "http://mnks.jxedt.com/get_question?r=" + (new Random().NextDouble()).ToString() + "&index=" + ids[num4].Groups[1].Value;
                            httpItem.URL = url;
                            html = httpHelper.GetHtml(httpItem);
                            string json = html.Html.Replace("\\这", "\\\\这");
                            JObject jobject = JObject.Parse(json);
                            string text2 = jobject["options"].ToString();
                            textBox2.Text += (string.Concat(new string[]
                            {
                            "第",
                            num4.ToString(),
                            "题：题目：",
                            jobject["question"].ToString()+"\r\n"
                            }));
                            bool flag2 = jobject["Type"].ToString().Equals("1");
                            if (flag2)
                            {
                                text2 = "√-×";
                            }
                            else
                            {
                                text2 = string.Concat(new string[]
                                {
                                jobject["a"].ToString(),
                                "-",
                                jobject["b"].ToString(),
                                "-",
                                jobject["c"].ToString(),
                                "-",
                                jobject["d"].ToString()
                                });
                            }

                            string text3 = jobject["ta"].ToString();
                            string text4 = "";
                            bool flag3 = jobject["Type"].ToString().Equals("1");
                            if (flag3)
                            {
                                text4 = Convert.ToDouble(text3) == 1.0 ? "√" : "×";
                            }
                            else
                            {
                                bool flag4 = text3.IndexOf("1") >= 0;
                                if (flag4)
                                {
                                    text4 = text4 + jobject["a"].ToString() + "-";
                                }
                                bool flag5 = text3.IndexOf("2") >= 0;
                                if (flag5)
                                {
                                    text4 = text4 + jobject["b"].ToString() + "-";
                                }
                                bool flag6 = text3.IndexOf("3") >= 0;
                                if (flag6)
                                {
                                    text4 = text4 + jobject["c"].ToString() + "-";
                                }
                                bool flag7 = text3.IndexOf("4") >= 0;
                                if (flag7)
                                {
                                    text4 = text4 + jobject["d"].ToString() + "-";
                                }
                                text4 = text4.Trim(new char[]
                                {
                                '-'
                                });
                            }
                            cells[num4, 0].PutValue(ids[num4].Groups[1].Value);
                            cells[num4, 0].SetStyle(st);
                            cells[num4, 1].PutValue(jobject["Type"].ToString().ToString().Replace("1", "判断").Replace("2", "单选").Replace("3", "多选"));
                            cells[num4, 1].SetStyle(st);
                            cells[num4, 2].PutValue(jobject["question"].ToString());
                            cells[num4, 2].SetStyle(st);
                            cells[num4, 3].PutValue(text2);
                            cells[num4, 3].SetStyle(st);
                            cells[num4, 4].PutValue(text4);
                            cells[num4, 4].SetStyle(st);
                            cells[num4, 5].PutValue(jobject["bestanswer"].ToString());
                            cells[num4, 5].SetStyle(st);
                            cells[num4, 6].PutValue(jobject["imageurl"].ToString());
                            cells[num4, 6].SetStyle(st);
                            cells[num4, 7].PutValue(jobject["sinaimg"].ToString());
                            cells[num4, 7].SetStyle(st);
                            cells.SetRowHeight(num4, 20.0);
                            //num4++;
                        
                    }
                    catch (Exception ex)
                    {

                       // MessageBox.Show(ex.ToString());
                    }
                }


            }
        }


        public void colle2(ref Worksheet sheet, Style st, string name, string url)
        {
            HttpHelper httpHelper = new HttpHelper();
            HttpItem httpItem = new HttpItem();
            Cells cells = sheet.Cells;
            string[] array = new string[]
            {
                "站点编号",
                "题型",
                "题目",
                "选项A",
                "选项B",
                "选项C",
                "选项D",
                "答案",
                "解答",
                "关联图片",
                "相关图片"
            };
            int num = array.Length;
            checked
            {
                int num2 = num - 1;
                for (int i = 0; i <= num2; i++)
                {
                    cells[0, i].PutValue(array[i]);
                    cells[0, i].SetStyle(st);
                    cells.SetRowHeight(1, 25.0);
                }
                textBox2.Text = ("开始读取" + "题目");
                cells = sheet.Cells;
                int num3 = num - 1;
                for (int j = 0; j <= num3; j++)
                {
                    cells[0, j].PutValue(array[j]);
                    cells[0, j].SetStyle(st);
                    cells.SetRowHeight(1, 25.0);
                }
                httpItem.URL = url;
                httpItem.Method = "GET";
                bool flag = !string.IsNullOrEmpty(this.cookie);
                if (flag)
                {
                    httpItem.Cookie = this.cookie;
                }
                httpItem.Allowautoredirect = true;
                HttpResult html = httpHelper.GetHtml(httpItem);
                MatchCollection ids = Regex.Matches(html.Html, @"data-id=""([\s\S]*?)""");


                for (int num4 = 0; num4 < ids.Count; num4++)
                {
                   
                    if (status == false)
                        return;
                    try
                    {
                       
                     
                  
                            url = "http://mnks.jxedt.com/get_question?r=" + (new Random().NextDouble()).ToString() + "&index=" + ids[num4].Groups[1].Value;
                            httpItem.URL = url;
                            html = httpHelper.GetHtml(httpItem);
                            string json = html.Html.Replace("\\这", "\\\\这");
                            JObject jobject = JObject.Parse(json);
                            string text2 = jobject["options"].ToString();
                            textBox2.Text += (string.Concat(new string[]
                            {
                   
                            "第",
                            num4.ToString(),
                            "题：题目：",
                            jobject["question"].ToString()+"\r\n"
                            }));
                            bool flag2 = jobject["Type"].ToString().Equals("1");
                            string xuangxiang1 = "";
                            string xuangxiang2 = "";
                            string xuangxiang3 = "";
                            string xuangxiang4 = "";
                            if (flag2)
                            {
                                //判断题
                                //text2 = "√-×";
                                xuangxiang1 = "√";
                                xuangxiang2 = "×";
                            }
                            else
                            {
                                //选择题

                                //text2 = string.Concat(new string[]
                                //{
                                //jobject["a"].ToString(),
                                //"-",
                                //jobject["b"].ToString(),
                                //"-",
                                //jobject["c"].ToString(),
                                //"-",
                                //jobject["d"].ToString()
                                //});
                                xuangxiang1 = jobject["a"].ToString();
                                xuangxiang2 = jobject["b"].ToString();
                                xuangxiang3= jobject["c"].ToString();
                                xuangxiang4 = jobject["d"].ToString();
                            }

                            string text3 = jobject["ta"].ToString();
                            string text4 = "";
                            bool flag3 = jobject["Type"].ToString().Equals("1");
                            if (flag3)
                            {
                                text4 = Convert.ToDouble(text3) == 1.0 ? "√" : "×";
                            }
                            else
                            {
                                bool flag4 = text3.IndexOf("1") >= 0;
                                if (flag4)
                                {
                                    text4 = text4 + jobject["a"].ToString() + "-";
                                }
                                bool flag5 = text3.IndexOf("2") >= 0;
                                if (flag5)
                                {
                                    text4 = text4 + jobject["b"].ToString() + "-";
                                }
                                bool flag6 = text3.IndexOf("3") >= 0;
                                if (flag6)
                                {
                                    text4 = text4 + jobject["c"].ToString() + "-";
                                }
                                bool flag7 = text3.IndexOf("4") >= 0;
                                if (flag7)
                                {
                                    text4 = text4 + jobject["d"].ToString() + "-";
                                }
                                text4 = text4.Trim(new char[]
                                {
                                '-'
                                });
                            }
                            cells[num4, 0].PutValue(ids[num4].Groups[1].Value);
                            cells[num4, 0].SetStyle(st);
                            cells[num4, 1].PutValue(jobject["Type"].ToString().ToString().Replace("1", "判断").Replace("2", "单选").Replace("3", "多选"));
                            cells[num4, 1].SetStyle(st);
                            cells[num4, 2].PutValue(jobject["question"].ToString());
                            cells[num4, 2].SetStyle(st);

                            cells[num4, 3].PutValue(xuangxiang1);   //选项
                            cells[num4, 3].SetStyle(st);
                            cells[num4, 4].PutValue(xuangxiang2);   //选项
                            cells[num4, 4].SetStyle(st);
                            cells[num4, 5].PutValue(xuangxiang3);   //选项
                            cells[num4, 5].SetStyle(st);
                            cells[num4, 6].PutValue(xuangxiang4);   //选项
                            cells[num4, 6].SetStyle(st);



                            cells[num4, 7].PutValue(text4);   //答案
                            cells[num4, 7].SetStyle(st);
                            cells[num4, 8].PutValue(jobject["bestanswer"].ToString());
                            cells[num4, 8].SetStyle(st);
                            cells[num4, 9].PutValue(jobject["imageurl"].ToString());
                            cells[num4, 9].SetStyle(st);
                            cells[num4, 10].PutValue(jobject["sinaimg"].ToString());
                            cells[num4, 10].SetStyle(st);
                            cells.SetRowHeight(num4, 20.0);
                            //num4++;
                        
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }


            }
        }

        private  void 驾校一点通_Load(object sender, EventArgs e)
        {
            #region 通用检测

         
            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"8Ok2d"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion

            //await webView21.EnsureCoreWebView2Async();
            //this.webView21.CoreWebView2.Navigate("https://www.baidu.com");

            Control.CheckForIllegalCrossThreadCalls = false;
            webView21.Source = new System.Uri("https://mnks.jxedt.com/", System.UriKind.Absolute);
           
        }

        

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.textBox2.SelectionStart = this.textBox2.Text.Length;
            this.textBox2.SelectionLength = 0;
            this.textBox2.ScrollToCaret();
        }

        bool status = true;

        private void button2_Click(object sender, EventArgs e)
        {

            button1.Enabled = true;
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
           
            textBox2.Text = "";
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

        private void 驾校一点通_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
           textBox1.Text= webView21.Source.ToString();
        }

        Task<String> GetMathResultTask()
        {
            return webView21.ExecuteScriptAsync("Math.sin(Math.PI/2)");
        }


        String GetMathResult()
        {
            // a) Application freezes
            var result = webView21.ExecuteScriptAsync("Math.sin(Math.PI/2)").GetAwaiter().GetResult();
            return result;

            // b) return null
            //String result = null;
            //Task task = new Task(async () => { result = await webView21.ExecuteScriptAsync("Math.sin(Math.PI/2)"); });
            //task.RunSynchronously();
            //return result;

            // c) Excepion: // InvalidCastException: Das COM-Objekt des Typs "System.__ComObject" kann nicht in den Schnittstellentyp "Microsoft.Web.WebView2.Core.Raw.ICoreWebView2Controller" umgewandelt werden. Dieser Vorgang konnte nicht durchgeführt werden, da der QueryInterface-Aufruf an die COM - Komponente für die Schnittstelle mit der IID "{4D00C0D1-9434-4EB6-8078-8697A560334F}" aufgrund des folgenden Fehlers nicht durchgeführt werden konnte: Schnittstelle nicht unterstützt(Ausnahme von HRESULT: 0x80004002(E_NOINTERFACE)).
            //String result = Task.Run(() => GetMathResultTask()).Result;
            //return result;
        }



      async  void getcookies()
        {
            var result = await webView21.ExecuteScriptAsync("document.cookie");
            this.cookie = result;
            //MessageBox.Show(result);
        }

       




    }
}
