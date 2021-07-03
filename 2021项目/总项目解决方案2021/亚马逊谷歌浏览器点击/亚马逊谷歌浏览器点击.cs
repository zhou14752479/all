using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;


namespace 亚马逊谷歌浏览器点击
{
    public partial class 亚马逊谷歌浏览器点击 : Form
    {
        #region 设置IE全局代理
        public static bool UnsetProxy()
        {
            return SetProxy(null, null);
        }
        public static bool SetProxy(string strProxy)
        {
            return SetProxy(strProxy, null);
        }

        public static bool SetProxy(string strProxy, string exceptions)
        {
            InternetPerConnOptionList list = new InternetPerConnOptionList();

            int optionCount = string.IsNullOrEmpty(strProxy) ? 1 : (string.IsNullOrEmpty(exceptions) ? 2 : 3);
            InternetConnectionOption[] options = new InternetConnectionOption[optionCount];
            //use  a proxy server
            options[0].M_Option = PerConnOption.INTERNET_PER_CONN_FLAGS;
            options[0].M_Value.M_Int = (int)((optionCount < 2) ? PerConnFlags.PROXY_TYPE_DIRECT :
                (PerConnFlags.PROXY_TYPE_DIRECT | PerConnFlags.PROXY_TYPE_PROXY));
            //use this proxy server
            if (optionCount > 1)
            {
                options[1].M_Option = PerConnOption.INTERNET_PER_CONN_PROXY_SERVE;
                options[1].M_Value.M_StringPtr = Marshal.StringToHGlobalAuto(strProxy);
                //except for these addresses
                if (optionCount > 2)
                {
                    options[2].M_Option = PerConnOption.INTERNET_PER_CONN_PROXY_BYPASS;
                    options[2].M_Value.M_StringPtr = Marshal.StringToHGlobalAuto(exceptions);
                }
            }

            //default stuff
            list.DwSize = Marshal.SizeOf(list);
            list.SzConnection = IntPtr.Zero;
            list.DwOptionCount = options.Length;
            list.DwOptionError = 0;

            int optSize = Marshal.SizeOf(typeof(InternetConnectionOption));
            //make a pointer out of all that
            IntPtr optionPtr = Marshal.AllocCoTaskMem(optSize * options.Length);
            //copy the array  over into that  spot in memory
            for (int i = 0; i < options.Length; ++i)
            {
                IntPtr opt = new IntPtr(optionPtr.ToInt64() + (i * optSize));
                Marshal.StructureToPtr(options[i], opt, false);
            }
            list.Options = optionPtr;

            //and then make a pointer out of the whole list
            IntPtr ipcoListPtr = Marshal.AllocCoTaskMem((Int32)list.DwSize);
            Marshal.StructureToPtr(list, ipcoListPtr, false);

            //finally,call the api mehod
            int returnvalue = NativeMethods.InternetSetOption(IntPtr.Zero,
                InternetOption.INTERNET_OPTION_PER_CONNECTION_OPTION,
                ipcoListPtr, list.DwSize) ? -1 : 0;
            if (returnvalue == 0)
            {
                //get the error codes,they might be helpful
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return (returnvalue < 0);
        }



        #region WinInet structures

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct InternetPerConnOptionList
        {
            public int DwSize; //size of the INTERNET_PER_CONN_OPTION_LIST struct
            public IntPtr SzConnection; //connection name to set/query options
            public int DwOptionCount; //number of options to set/query
            public int DwOptionError; //on error ,which option failed
            public IntPtr Options;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct InternetConnectionOption
        {
            static readonly int Size;
            public PerConnOption M_Option;
            public InternetConnectionOptionValue M_Value;
            static InternetConnectionOption()
            {
                InternetConnectionOption.Size = Marshal.SizeOf(typeof(InternetConnectionOption));
            }

            //Nested types
            [StructLayout(LayoutKind.Explicit)]
            public struct InternetConnectionOptionValue
            {
                [FieldOffset(0)]
                public System.Runtime.InteropServices.ComTypes.FILETIME M_FileTime;

                [FieldOffset(0)]
                public int M_Int;

                [FieldOffset(0)]
                public IntPtr M_StringPtr;
            }
        }

        #endregion

        #region WinInet enums

        /// <summary>
        /// options manifests for Internet{Query|Set} Option
        /// </summary>
        public enum InternetOption : uint
        {
            INTERNET_OPTION_PER_CONNECTION_OPTION = 75
        }

        /// <summary>
        /// Options used in INTERNET_PER_CONN_OPTION struct
        /// </summary>
        public enum PerConnOption
        {
            /// <summary>
            /// Sets or retrieves the connection type.The vlaue member will contain one or more of the values from PerConnFlags
            /// </summary>
            INTERNET_PER_CONN_FLAGS = 1,
            /// <summary>
            /// Sets or retrieves a string containing the proxy servers 
            /// </summary>
            INTERNET_PER_CONN_PROXY_SERVE = 2,
            /// <summary>
            /// Sets or retrieves a string containing the urls what do not  user the proxy server 
            /// </summary>
            INTERNET_PER_CONN_PROXY_BYPASS = 3,
            /// <summary>
            /// Sets or retrieves a string containing the url to the automatic configuration script
            /// </summary>
            INTERNET_PER_CONN_AUTOCONFIG_URL = 4
        }

        /// <summary>
        /// PER_CONN_FLAGS
        /// </summary>
        [Flags]
        public enum PerConnFlags
        {
            PROXY_TYPE_DIRECT = 0x00000001, //direct to net 
            PROXY_TYPE_PROXY = 0x00000002, //via named proxy
            PROXY_TYPE_AUTO_PROXY_URL = 0x00000004, //autoproxy url
            PROXY_TYPE_AUTO_DETECT = 0x00000008 // use autoproxy detection
        }

        #endregion

        internal static class NativeMethods
        {
            [DllImport("WinInet.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool InternetSetOption(IntPtr hInternet, InternetOption dwOption, IntPtr lpBuffer, int dwBufferLength);
        }
        #endregion




        internal class OpenPageSelf : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser browserControl, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
                string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
                IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                newBrowser = null;
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
                chromiumWebBrowser.Load(targetUrl);
                return true; //Return true to cancel the popup creation copyright by codebye.com.
            }
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

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

        

        public 亚马逊谷歌浏览器点击()
        {
            InitializeComponent();
        }
        public ChromiumWebBrowser browser;
        private void 亚马逊谷歌浏览器点击_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            //var settings = new CefSettings();
            //settings.CachePath = "cache";
            //settings.CefCommandLineArgs.Add("proxy-server", "222.37.106.112:46603");

            // Cef.Initialize(settings);
            //browser = new ChromiumWebBrowser("https://api.ipify.org/?format=jsonp");
            //browser.Parent = splitContainer1.Panel1;
            //browser.Dock = DockStyle.Fill;
            //browser.LifeSpanHandler = new OpenPageSelf();   //设置在当前窗口打开

        }


        List<string> iplist = new List<string>();
        int iPcount = 0;
        public string getipone()
        {
            iPcount = iPcount + 1;
            if (iPcount < iplist.Count)
            {
                return iplist[iPcount];
            }
            else
            {
                
                getip1000();
                iPcount = 0;
                return iplist[0];
            }
           
        }

        public void getip1000()
        {
            iplist.Clear();
            string html = GetUrl(textBox5.Text,"utf-8");
            string[] ips = html.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var ip in ips)
            {
                iplist.Add(ip);
            }
          
        }

        public void  getItemUrls()
        {
            

            int goodcount = 0;
            int startpage = 1;
            string shurupage = Regex.Match(textBox3.Text, @"page=([\s\S]*?)&").Groups[1].Value;
            if (shurupage != "")
            {
                startpage = Convert.ToInt32(shurupage);
            }

            for (int i = startpage; i < Convert.ToInt32(textBox4.Text)+1; i++)
            {

                string url = Regex.Replace(textBox3.Text, "&page=.*", "&page="+i);
               
                string html = GetUrl(url,"utf-8");


                MatchCollection titles = Regex.Matches(html, @"<span class=""a-size-medium a-color-base a-text-normal"">([\s\S]*?)</span>");
                MatchCollection links = Regex.Matches(html, @"<a class=""a-link-normal s-no-outline"" href=""([\s\S]*?)""");

                for (int j = 0; j < titles.Count; j++)
                {

                    if (!links[j].Groups[1].Value.Contains("slredirect"))
                    {
                        try
                        {
                            toolStripStatusLabel1.Text = "当前采集页码数" + i + "，已获取商品链接：" + goodcount;
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.Checked = true;
                            lv1.SubItems.Add(titles[j].Groups[1].Value);
                            lv1.SubItems.Add("https://www.amazon.com" + links[j].Groups[1].Value);
                            goodcount = goodcount + 1;
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception)
                        {

                            continue;
                        }
                    }
                }
                Thread.Sleep(1000);
            }
          
        }

        public void run()
        {
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请勾选需要点击的链接");
                return;
            }

            toolStripStatusLabel1.Text = "开始执行......";
            getip1000();
            if (iplist.Count == 0)
            {
                MessageBox.Show("ip提取失败，请检查IP白名单");
                return;

            }


            int Urlcount = listView1.CheckedItems.Count;
            toolStripStatusLabel1.Text = "共获取到链接数：" + Urlcount;
            for (int j = 1; j < Convert.ToInt32(textBox2.Text) + 1; j++)
            {

                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {
                    string nowurl = listView1.CheckedItems[i].SubItems[2].Text;

                    textBox1.Text = nowurl;
                    toolStripStatusLabel1.Text = "共获取到链接数：" + Urlcount + "  正在点击第" + (i + 1) + "个链接   " + "  当前链接已点击次数：" + j;
                    string ip = getipone();
                    label5.Text = ip;
                    if (!ip.Contains("false"))
                    {
                        SetProxy(ip);
                    }
                    //browser.Load(nowurl);
                    webBrowser1.Navigate(nowurl);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    Thread.Sleep(Convert.ToInt32(numericUpDown1.Value) * 1000);
                }


            }
            toolStripStatusLabel1.Text = "全部完成";
        }


        Thread thread;
        bool status = true;
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            SetProxy("");
            #region 通用检测


            string html = GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"oWQXTAei"))
            {

                return;
            }

            #endregion
            
            if (textBox5.Text == "")
            {
                MessageBox.Show("请输入代理IP地址");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getItemUrls);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetProxy("");
            status = false;
            toolStripStatusLabel1.Text = "手动停止";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;
            }
        }



    }
}
