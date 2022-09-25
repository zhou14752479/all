using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace douyinSelenium
{
    public class function
    {
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        public static string Ex_Proxy_Name = "proxy.zip";
        public static IWebDriver getdriver(bool headless, bool nopic,string ip,string user,string pass)
        {
            //关闭cmd窗口
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;




        ChromeOptions options = new ChromeOptions();
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            options.AddArguments(ip);
            bool isproxysetting = true;
            isproxysetting = Rebuild_Extension_Proxy(user, pass);
            if (isproxysetting)
            {
                // Driver 設定
                options = new ChromeOptions();
                options.Proxy = null;
                options.AddArguments("--proxy-server=" + ip);
                options.AddExtension(Ex_Proxy_Name);

                options.AddArgument("ignore-certificate-errors");

                options.AddArgument("disable-infobars");//去掉提示：Chrome正收到自动测试软件的控制
                options.AddArgument("window-size=800,500");
                options.BinaryLocation = "Chrome/Application/chrome.exe";
                //禁用图片
                if (nopic)
                {
                    options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                }

                if (headless)
                {

                    options.AddArgument("--headless");

                }
                //options.AddArgument("--disable-gpu");

                return new ChromeDriver(driverService, options);
            }
            else
            {
                return new ChromeDriver(driverService, null);
            }
        }



        #region  获取32位MD5加密
        public static string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion
        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion

        public static string login(string kami)
        {
            string jiqima = GetMD5(GetMacAddress()).ToUpper();
            string url = "http://43.154.174.221/kami.php";
            string postdata = "type=yz&kami="+kami.Trim()+"&jiqima="+jiqima;
           string html= PostUrlDefault(url, postdata, "");
            string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            return msg; 
        }




        public static void KillProcess(string processName)
        {
            foreach (Process p in Process.GetProcesses())
            {
                bool flag = p.ProcessName.Contains(processName);
                if (flag)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                        Console.WriteLine("已杀掉" + processName + "进程！！！");
                    }
                    catch (Win32Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }
                    catch (InvalidOperationException e2)
                    {
                        Console.WriteLine(e2.Message.ToString());
                    }
                }
            }
        }


        public static string GetUrl(string Url, string COOKIE)
        {
            string html = "";
            string charset = "utf-8";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)";
                request.Referer = Url;
                request.Proxy = null;
                WebHeaderCollection headers = request.Headers;
                headers.Add("sdk-version: 2");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                if (response.Headers["Content-Encoding"] == "gzip")
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
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

        public static string getnickname(string cookie)
        {
            try
            {
                string url = "https://api3-core-c-lf.amemv.com/aweme/v1/user/profile/self/?aid=6383";

                string html = GetUrl(url, cookie);
                string nickname = Regex.Match(html, @"""nickname"":""([\s\S]*?)""").Groups[1].Value;
                string uid = Regex.Match(html, @"""unique_id"":""([\s\S]*?)""").Groups[1].Value;
                if (nickname == "")
                {
                    nickname = "掉线";
                }
                if (uid == "")
                {
                    uid = "掉线";
                }
                return uid + "," + nickname;

            }
            catch (Exception ex)
            {

                return "异常,异常";
            }
        }


        public static string getzhibojianname(string url,string cookie)
        {
            try
            {
             
                string html = GetUrl(url, cookie);
                string name = Regex.Match(html, @"<h1 class=""([\s\S]*?)>([\s\S]*?)</h1>").Groups[2].Value;
                return name;

            }
            catch (Exception ex)
            {

                return "异常,异常";
            }
        }

        #region 获取txt编码
        //调用：EncodingType.GetTxtType(textBox1.Text)
        public class EncodingType
        {
            /// <summary> 
            /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
            /// </summary> 
            /// <param name=“FILE_NAME“>文件路径</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetTxtType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = GetType(fs);
                fs.Close();
                return r;
            }

            /// <summary> 
            /// 通过给定的文件流，判断文件的编码类型 
            /// </summary> 
            /// <param name=“fs“>文件流</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetType(FileStream fs)
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
                Encoding reVal = Encoding.Default;

                BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                r.Close();
                return reVal;

            }

            /// <summary> 
            /// 判断是否是不带 BOM 的 UTF8 格式 
            /// </summary> 
            /// <param name=“data“></param> 
            /// <returns></returns> 
            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
                byte curByte; //当前分析的字节. 
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前 
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1 
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }
        }

        #endregion



        public static bool Rebuild_Extension_Proxy(string proxy_UserName, string proxy_PassWord)
        {
            bool result = false;

            FileStream zipToOpen = null;
            ZipArchive archive = null;
            ZipArchiveEntry readmeEntry = null;
            StreamWriter writer = null;
            string background = "";
            string manifest = "";

            try
            {
                background = @"
                var Global = {
                    currentProxyAouth:
                    {
                        username: '',
                        password: ''
                    }
                }

                Global.currentProxyAouth = {
                        username: '" + proxy_UserName + @"',
                        password: '" + proxy_PassWord + @"'
                }

                chrome.webRequest.onAuthRequired.addListener(
                    function(details, callbackFn) {
                        console.log('onAuthRequired >>>: ', details, callbackFn);
                        callbackFn({
                            authCredentials: Global.currentProxyAouth
                        });
                    }, {
                        urls: [""<all_urls>""]
                    }, [""asyncBlocking""]);

                chrome.runtime.onMessage.addListener(
                    function(request, sender, sendResponse) {
                        console.log('Background recieved a message: ', request);

                        POPUP_PARAMS = {};
                        if (request.command && requestHandler[request.command])
                            requestHandler[request.command] (request);
                    }
                );";

                manifest = @"
                {
                    ""version"": ""1.0.0"",
                    ""manifest_version"": 2,
                    ""name"": ""Chrome Proxy"",
                    ""permissions"": [
                        ""proxy"",
                        ""tabs"",
                        ""unlimitedStorage"",
                        ""storage"",
                        ""<all_urls>"",
                        ""webRequest"",
                        ""webRequestBlocking""
                    ],
                    ""background"": {
                        ""scripts"": [""background.js""]
                    },
                    ""minimum_chrome_version"":""22.0.0""
                }";

                zipToOpen = new FileStream(System.Environment.CurrentDirectory + "\\" + Ex_Proxy_Name, FileMode.Create);
                archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);

                readmeEntry = archive.CreateEntry("background.js");
                writer = new StreamWriter(readmeEntry.Open());
                writer.WriteLine(background);
                writer.Close();

                readmeEntry = archive.CreateEntry("manifest.json");
                writer = new StreamWriter(readmeEntry.Open());
                writer.WriteLine(manifest);
                writer.Close();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (writer != null) { writer.Close(); writer.Dispose(); writer = null; }
                if (readmeEntry != null) { readmeEntry = null; }
                if (archive != null) { archive.Dispose(); archive = null; }
                if (zipToOpen != null) { zipToOpen.Close(); zipToOpen.Dispose(); zipToOpen = null; }
            }

            return result;
        }



    }
}
