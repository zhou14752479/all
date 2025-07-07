using Microsoft.Web.WebView2.Core;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WebView2
{
    public partial class 获取cookie : Form
    {
        public 获取cookie()
        {
            InitializeComponent();
        }

        #region POST请求全参
        public static string PostUrl(string url,  string COOKIE)
        {
            string result;
            try
            {
                string postData = "";
                string charset = "utf-8";
                string contentType = "application/x-www-form-urlencoded";
                string refer = "";
               ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");

                //添加头部
                request.ContentType = contentType;
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = refer;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                result = ex.ToString();

            }
            return result;
        }
        #endregion

        public string[] mobileList = { "13292447109","17521704272","19930453849","18532659748","19931467960", "15049985581" };
      
        private string username = "";
        private string password = "A14752479a";
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public static string cookies = "";


        // 异步获取指定URL的所有Cookie（包括HTTPOnly）
        public async Task<List<Microsoft.Web.WebView2.Core.CoreWebView2Cookie>> GetAllCookiesAsync(string url)
        {
            if (webView21.CoreWebView2 == null)
            {
                throw new InvalidOperationException("WebView2核心尚未初始化");
            }

            // 获取Cookie管理器
            var cookieManager = webView21.CoreWebView2.CookieManager;

            // 获取指定URL的所有Cookie
            var cookies = await cookieManager.GetCookiesAsync(url);
            return cookies;
        }


        // 异步获取指定名称的HTTPOnly Cookie
        public async Task<string> GetHttpOnlyCookieValueAsync(string url, string cookieName)
        {
            var cookies = await GetAllCookiesAsync(url);

            foreach (var cookie in cookies)
            {
                
                if (cookie.Name == cookieName && cookie.IsHttpOnly)
                {
                    return cookie.Value;
                }
            }

            return null; // 未找到匹配的Cookie
        }


        //public void islogin()
        //{
        //    string url= "https://user.zxxk.com/creator/api/v1/creator-center/get-creator-upload-info";
        //    string html = PostUrl(url,cookies);
            
        //    if(!html.Contains("dayUploadNum"))  //cookie失效
        //    {
        //        webView21.Reload();
        //        webView21.Source = new System.Uri("https://sso.zxxk.com/login");
        //        textBox2.Text += DateTime.Now.ToString() + "  cookie失效..." + "\r\n";
        //    }

        //    else
        //    {
        //        textBox2.Text += DateTime.Now.ToString() + "  cookie正常..." + "\r\n";
        //    }
          
           
        //    if (textBox2.Text.Length > 1000)
        //    {
        //        textBox2.Text = "";
        //    }
        //}
      

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox1.Text.Trim()) * 60 * 1000;
            webView21.Reload();
            //cookies = "";
            //timer1.Interval = 5000;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            webView21.Source = new System.Uri("https://user.zxxk.com/");

        }

       

       
      

        async public void writeCookie()
        {
           string ticket = await GetHttpOnlyCookieValueAsync("https://www.zxxk.com/", "xk.passport.ticket.v2");

            cookies = "xk.passport.ticket.v2=" + ticket;
           while (ticket != null)  
            {
                textBox2.Text += DateTime.Now.ToString() + "  获取cookie成功"+"\r\n";
                break;
            }
           

            System.IO.File.WriteAllText(path + "cookie.txt",cookies , Encoding.UTF8);


        }

        private void 获取cookie_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox1.Text.Trim()) * 60 * 1000;
            webView21.NavigationCompleted += WebView2_NavigationCompleted;
            webView21.Source = new System.Uri("https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fuser%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fwww.zxxk.com%2F");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webView21.Reload();
            
        }





        // 辅助方法：转义 JavaScript 字符串
        private string EscapeJsString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r");
        }






        


        private async Task ExecuteLoginScript()
        {
            Random random = new Random();
            int suiji = random.Next(0, mobileList.Length);
            username = mobileList[suiji];

            string script11 = $@"
                            (function() {{
                                // 使用 XPath 查找包含指定文本的 button 元素
                                var xpath = '//button[contains(text(), ""{EscapeJsString("账户密码")}"")]';
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


            webView21.Invoke(new Action(async () =>
            {
                await webView21.CoreWebView2.ExecuteScriptAsync(script11);
            }));

            Thread.Sleep(1000);



            // 组合脚本示例
            string combinedScript = $@"
    (function() {{
        function setInputValue(selector, value) {{
            var xpath = '//input[contains(@placeholder, ""' + selector + '"")]';
            var result = document.evaluate(xpath, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
            var input = result.singleNodeValue;
            
            if (input) {{
                input.value = value;
                input.dispatchEvent(new Event('input', {{ bubbles: true }}));
                input.dispatchEvent(new Event('change', {{ bubbles: true }}));
                return true;
            }}
            return false;
        }}
        
        // 依次设置用户名和密码
        var usernameResult = setInputValue('{EscapeJsString("手机号")}', '{EscapeJsString(username)}');
        var passwordResult = setInputValue('{EscapeJsString("密码")}', '{EscapeJsString(password)}');
        
        return {{ username: usernameResult, password: passwordResult }};
    }})();
";


            webView21.Invoke(new Action(async () =>
            {
                await webView21.CoreWebView2.ExecuteScriptAsync(combinedScript);
            }));




            Thread.Sleep(1000);

            string script22 = $@"
                            (function() {{
                                // 使用 XPath 查找包含指定文本的 button 元素
                                var xpath = '//button[contains(text(), ""{EscapeJsString("登录")}"")]';
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


            webView21.Invoke(new Action(async () =>
            {
                await webView21.CoreWebView2.ExecuteScriptAsync(script22);
            }));


        }



        private async void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // 等待页面完全加载
                await Task.Delay(1000); // 可根据页面加载速度调整

                try
                {
                    // 注入JavaScript执行登录操作
                    await ExecuteLoginScript();
                    writeCookie();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"登录过程出错: {ex.Message}");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(textBox1.Text.Trim()) * 60 * 1000;
            webView21.Source = new System.Uri("https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fuser%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fwww.zxxk.com%2F");
        }

        private void 获取cookie_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
