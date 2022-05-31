using CefSharp;
using CefSharp.Handler;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 抖音网页主控
{
    public partial class 浏览器端 : Form
    {
        public string cookies = "";
        public string ip = "";
        public string user = "";
        public string pass = "";
        public string accountName = "";
        public 浏览器端(string cookie,string ip,string user,string pass,string accountName)
        {
            this.cookies = cookie.Trim();   
            this.ip = ip.Trim();   
            this.user = user.Trim();
            this.pass = pass.Trim();    
            this.accountName = accountName.Trim();
            InitializeComponent();
        }

        public RequestContextSettings requestContextSettings = new RequestContextSettings();

        /// <summary>
        /// 初始化浏览器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="accountName">如果需要隔离则添加隔离路径</param>
        /// <returns></returns>
        public ChromiumWebBrowser InitBrowser(string url, string accountName = "")
        {
           
            string path = AppDomain.CurrentDomain.BaseDirectory;
            //RequestContext context = new RequestContext(new RequestContextSettings()
            //{
            //    CachePath = path+"AppCaches/" + this.accountName,
            //});

            requestContextSettings.PersistSessionCookies = true;
            requestContextSettings.CachePath = Environment.CurrentDirectory + "/user/" + accountName + "/";
            requestContextSettings.PersistUserPreferences = false;

            if (ip.Trim() != "")
            {
               string[] ipall = ip.Split(new string[] { ":" }, StringSplitOptions.None);
                if (user != "")
                {
                    CefSharp.CefSharpSettings.Proxy = new CefSharp.ProxyOptions(ipall[0], ipall[1], user, pass);
                }
            }
            RequestContext context = new RequestContext(requestContextSettings);
            ChromiumWebBrowser browser = new ChromiumWebBrowser(url, context);
            return browser;
        }

        string url = function.openUrl;
        //string url = "https://www.ip138.com/";
        private async void 浏览器端_Load(object sender, EventArgs e)
        {

           ChromiumWebBrowser chromiumWebBrowser1 = InitBrowser(url,accountName);
            chromiumWebBrowser1.Parent = this;
            chromiumWebBrowser1.Dock = DockStyle.Fill;

            var cook = Cef.GetGlobalCookieManager();

            string[] cookieall = this.cookies.Split(new string[] { ";" }, StringSplitOptions.None);
            foreach (var item in cookieall)
            {
                string[] cookie = item.Split(new string[] { "=" }, StringSplitOptions.None);
                if(cookie.Length>1)
                {
                    await cook.SetCookieAsync(url, new CefSharp.Cookie
                    {
                        Domain = ".douyin.com",
                        Name = cookie[0].Trim(),
                        Value = cookie[1].Trim(),
                    });
                }
               
                
            }
            
            if (ip.Trim()!="")
            {
               await SetProxy(chromiumWebBrowser1, ip);
                //SetProxy(chromiumWebBrowser1, ip);
                string[] ipall = ip.Split(new string[] { ":" }, StringSplitOptions.None);
                if (user != "")
                {
                    
                    CefSharp.CefSharpSettings.Proxy = new CefSharp.ProxyOptions(ipall[0], ipall[1], user, pass);
                }
            }



            await chromiumWebBrowser1.LoadUrlAsync(url);

      
        }


     

        public class BaseRequestHandler:RequestHandler
        {
            private string _userName;
            private string _password;

            public BaseRequestHandler(string userName, string password)
            {
                this._userName = userName;
                this._password = password;
            }

          

            protected override bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host,
                int port, string realm, string scheme, IAuthCallback callback)
            {
                if (isProxy == true)
                {
                    callback.Continue(_userName, _password);

                    return true;
                }

                return false;
            }
        }
        private async Task SetProxy(ChromiumWebBrowser cwb, string address)
        {
            try
            {
                await Cef.UIThreadTaskFactory.StartNew(delegate
                {
                    var rc = cwb.GetBrowser().GetHost().RequestContext;
                    var v = new Dictionary<string, object>();
                    v["mode"] = "fixed_servers";
                    v["server"] = address;
                    string error;
                    bool success = rc.SetPreference("proxy", v, out error);
                    Console.WriteLine(success);
                });
            }
            catch (Exception e)
            {
                //
            }
        }


    }
}
