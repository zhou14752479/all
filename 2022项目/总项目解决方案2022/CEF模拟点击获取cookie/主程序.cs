using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEF模拟点击获取cookie
{
    public partial class 主程序 : Form
    {
        public 主程序()
        {
           
            InitializeComponent();
            InitializeCefSharp();
            InitializeBrowser();

        }

        private void 主程序_Load(object sender, EventArgs e)
        {
            
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private ChromiumWebBrowser browser;
        string loginurl = "https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fuser%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fwww.zxxk.com%2F";

        // 初始化CefSharp
        private void InitializeCefSharp()
        {
            try
            {
                var settings = new CefSettings();
                // 可以设置缓存路径等
                 settings.CachePath = System.IO.Path.Combine(Application.StartupPath, "cache");
                // 禁用不必要的功能以减少缓存生成
                settings.CefCommandLineArgs.Add("disable-gpu-shader-disk-cache");
                settings.CefCommandLineArgs.Add("disable-cache");
                settings.CefCommandLineArgs.Add("disable-application-cache");
                settings.CefCommandLineArgs.Add("disk-cache-size", "0"); // 内存缓存也限制
                // 检查是否已初始化（使用CefSharp的API）
                if (!Cef.IsShutdown)
                {
                    // 执行初始化
                    Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CefSharp初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void InitializeBrowser()
        {
           

            browser = new ChromiumWebBrowser("http://www.zxxk.com");
            browser.Dock = DockStyle.Fill;
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
            panel1.Controls.Add(browser);
        }

        private async void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                Text = "页面加载完成 - " + await GetPageTitleAsync();
                toolStripTextBox1.Text =DateTime.Now.ToString("HH:mm:ss")+ Text;
                Thread.Sleep(500);
                // 模拟输入和点击（使用CSS选择器，例如#username表示id为username的元素）
                bool aaa= await SimulateClickAsync22("账户密码");
                if (aaa)
                {
                    Thread.Sleep(1000);
                    bool inputSuccess = await SimulateInputAsync("#username", "17606117606");

                    bool inputSuccess2 = await SimulateInputAsync("#password", "A14752479a");
                    Thread.Sleep(500);
                    bool clickSuccess = await SimulateClickAsync("#accountLoginBtn");
                    getcefcookie();
                }

                getcefcookie();
            }
        }

       

        // 模拟输入
        private async Task<bool> SimulateInputAsync(string selector, string text)
        {
            string script = $@"
                (function() {{
                    var elem = document.querySelector('{selector}');
                    if(elem) {{
                        elem.value = '{EscapeJavaScriptString(text)}';
                        elem.dispatchEvent(new Event('input', {{bubbles: true}}));
                        elem.dispatchEvent(new Event('change', {{bubbles: true}}));
                        return true;
                    }}
                    return false;
                }})();
            ";

            var result = await browser.EvaluateScriptAsync(script);
            return result.Success && (bool)result.Result;
        }

        // 模拟点击
        private async Task<bool> SimulateClickAsync(string selector)
        {
            string script = $@"
                (function() {{
                    var elem = document.querySelector('{selector}');
                    if(elem) {{
                        elem.click();
                        return true;
                    }}
                    return false;
                }})();
            ";

            var result = await browser.EvaluateScriptAsync(script);
            return result.Success && (bool)result.Result;
        }

        // 模拟点击
        private async Task<bool> SimulateClickAsync22(string txt)
        {
            string script= $@"
                            (function() {{
                                // 使用 XPath 查找包含指定文本的 button 元素
                                var xpath = '//button[contains(text(), ""{txt}"")]';
                                var result = document.evaluate(xpath, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
                                var element = result.singleNodeValue;
                                
                                if (element) {{
                                    // 模拟鼠标点击事件
                                    var clickEvent = new MouseEvent('click', {{
                                        bubbles: true,
                                        cancelable: true,
                                        view: window
                                    }});
                                    element.dispatchEvent(clickEvent);
                                    return true;
                                }}
                                return false;
                            }})();
                        ";

            var result = await browser.EvaluateScriptAsync(script);
            return result.Success && (bool)result.Result;
        }



        // 转义JavaScript字符串
        private string EscapeJavaScriptString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input.Replace("\\", "\\\\")
                        .Replace("'", "\\'")
                        .Replace("\"", "\\\"")
                        .Replace("\r", "\\r")
                        .Replace("\n", "\\n")
                        .Replace("\t", "\\t");
        }



        private async Task<string> GetPageTitleAsync()
        {
            try
            {
                // 执行JavaScript获取标题
                var result = await browser.EvaluateScriptAsync("document.title");
                if (result.Success && result.Result != null)
                {
                    return result.Result.ToString();
                }
                return "未知标题";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取标题出错: {ex.Message}");
                return "获取标题失败";
            }
        }

       

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            browser.Load(loginurl);
            timer1.Start();
            timer1.Interval = 300000;
        }


        #region cefsharp获取cookie
        
        string cookies = "";


        private void visitor_SendCookie(CefSharp.Cookie obj)
        {
            if (obj.Name == "xk.passport.ticket.v2")
            {
                cookies += obj.Name + "=" + obj.Value + ";";

                if (obj.Value != null && obj.Value != "")
                {
                    System.IO.File.WriteAllText(path + "cookie.txt", cookies, Encoding.UTF8);
                   
                }
            }
 
        }


        public class CookieVisitor : CefSharp.ICookieVisitor
        {
            public event Action<CefSharp.Cookie> SendCookie;
            public void Dispose()
            {

            }

            public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
            {
                deleteCookie = false;
                if (SendCookie != null)
                {
                    SendCookie(cookie);
                }

                return true;

            }

        }
        #endregion

        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void getcefcookie()
        {
            cookies = ""; //把之前加载的重复的cookie清空
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);
        }

        private void 弹出cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getcefcookie();
           
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            browser.Load(loginurl);
        }
    }
}
