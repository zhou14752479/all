using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 商标局浏览器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
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
                IntPtr opt = new IntPtr(optionPtr.ToInt32() + (i * optSize));
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
               
                string COOKIE = "usid=07WwkCu3b_78aUPT; IPLOC=CN3213; SUV=00BA2DBC3159B8CD5D2585534E6EA580; CXID=5EA7E0DBFC0F423A95BC1EB511A405C7; SUID=CDB859313118960A000000005D25B077; ssuid=7291915575; pgv_pvi=5970681856; start_time=1562896518693; front_screen_resolution=1920*1080; wuid=AAElSJCaKAAAAAqMCGWoVQEAkwA=; FREQUENCY=1562896843272_13; sg_uuid=6358936283; newsCity=%u5BBF%u8FC1; SNUID=9FB9A0C8F8FC6C9FCB42F1E4F9BFB645; sortcookie=1; sw_uuid=3118318168; ld=3Zllllllll2NrO7hlllllVLmmtGlllllGqOxBkllllwlllllVklll5@@@@@@@@@@; sct=20";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://news.sogou.com/news?query=site%3Asohu.com+%B4%F3%CA%FD%BE%DD&_ast=1571813760&_asf=news.sogou.com&time=0&w=03009900&sort=1&mode=1&manual=&dp=1";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

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

        internal static class NativeMethods
    {
        [DllImport("WinInet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool InternetSetOption(IntPtr hInternet, InternetOption dwOption, IntPtr lpBuffer, int dwBufferLength);
    }

        private void Button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            try
            {
                SetProxy("");
                string url = "http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7";

                string html = GetUrl(url, "utf-8");
                
                if (html.Contains("不是白名单"))
                {
                    GetUrl("http://h.xunlianip.com/Users-whiteIpAddNew.html?appid=272&appkey=b821d79eb7b33c43965fa59fb21511e0&whiteip="+GetIP(), "utf-8");

                    textBox1.Text = "请重新点击更换IP";
                }

                else if (html.Contains(":"))
                {

                    SetProxy(html.Trim());

                    textBox1.Text = DateTime.Now.ToString() + "  重置成功" + "\r\n";
                    textBox1.Text += DateTime.Now.ToString() + "  当前IP："+ html+ "\r\n";
                    textBox1.Text += DateTime.Now.ToString() + "  可以使用了"+"\r\n";
                }
                else
                {
                    textBox1.Text = DateTime.Now.ToString() + "  重置失败";
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
          

          
        }

        #region 获取公网IP
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }

            }
        }

        #endregion

        
        private void Form1_Load(object sender, EventArgs e)
        {
          
            label3.Text = GetIP();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                SetProxy("");
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }

        }

        private void Label2_Click(object sender, EventArgs e)
        {
           
        }

        int i = 100;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (i != 0)
            {
                i = i - 1;
                button1.Text = i.ToString();

                button1.Enabled = false;


            }
            else
            {
                i = 100;
                button1.Enabled = true;
                button1.Text = "更换IP";
                timer1.Stop();
            }

           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetProxy("");
        }
    }
}
