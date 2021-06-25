using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 淘宝商品抓取
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void getCookie(string url)
        {
            if (url == null)
            {
                Console.WriteLine("Specify the URL to receive the request.");
                Environment.Exit(1);
            }
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                // Print the properties of each cookie.
                foreach (Cookie cook in response.Cookies)
                {
                    Console.WriteLine("Cookie:");
                    Console.WriteLine($"{cook.Name} = {cook.Value}");
                    Console.WriteLine($"Domain: {cook.Domain}");
                    Console.WriteLine($"Path: {cook.Path}");
                    Console.WriteLine($"Port: {cook.Port}");
                    Console.WriteLine($"Secure: {cook.Secure}");

                    Console.WriteLine($"When issued: {cook.TimeStamp}");
                    Console.WriteLine($"Expires: {cook.Expires} (expired? {cook.Expired})");
                    Console.WriteLine($"Don't save: {cook.Discard}");
                    Console.WriteLine($"Comment: {cook.Comment}");
                    Console.WriteLine($"Uri for comments: {cook.CommentUri}");
                    Console.WriteLine($"Version: RFC {(cook.Version == 1 ? 2109 : 2965)}");

                    // Show the string representation of the cookie.
                    Console.WriteLine($"String: {cook}");
                }
            }
        }
        public static HttpWebResponse get(string url)
        {
            string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.68 Safari/537.36 Edg/84.0.522.28";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            // 设置request属性
            myRequest.Method = "GET";
            myRequest.UserAgent = DefaultUserAgent;
            myRequest.Timeout = 300;
            myRequest.KeepAlive = true;
            myRequest.Accept = @"*/*";
            myRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            // headers
            Console.WriteLine("headers is:\n\n\tName\t\tValue\n{0}",myRequest.Headers);


            HttpWebResponse Web_Response = (HttpWebResponse)myRequest.GetResponse();
            Console.WriteLine(((HttpWebResponse)Web_Response).StatusDescription);
            return Web_Response;
        }

        #region 同步通过GET方式发送数据
        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">GET数据</param>
        /// <param name="cookie">GET容器</param>
        /// <returns></returns>
        public static string getwithCookies(string Url, string postDataStr, ref CookieContainer cookies)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            if (cookies.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookies = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookies;
            }

            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            request.Headers["cookie"] = "cna=ecr8FaGj4BkCAXQBA/ISif7h; t=74708015a4c8f195ffecf6910094518a;";
            Console.WriteLine("headers is:\n\n\tName\t\tValue\n{0}", request.Headers);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.Cookies.GetHashCode());
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static string getText(HttpWebResponse Web_Response)
        {
            string html = "";
            foreach (Cookie cook in Web_Response.Cookies)
            {
                Console.WriteLine("Cookie:");
                Console.WriteLine($"{cook.Name} = {cook.Value}");
                Console.WriteLine($"Domain: {cook.Domain}");
                Console.WriteLine($"Path: {cook.Path}");
                Console.WriteLine($"Port: {cook.Port}");
                Console.WriteLine($"Secure: {cook.Secure}");

                Console.WriteLine($"When issued: {cook.TimeStamp}");
                Console.WriteLine($"Expires: {cook.Expires} (expired? {cook.Expired})");
                Console.WriteLine($"Don't save: {cook.Discard}");
                Console.WriteLine($"Comment: {cook.Comment}");
                Console.WriteLine($"Uri for comments: {cook.CommentUri}");
                Console.WriteLine($"Version: RFC {(cook.Version == 1 ? 2109 : 2965)}");

                // Show the string representation of the cookie.
                Console.WriteLine($"String: {cook}");
            }
            if (Web_Response.ContentEncoding.ToLower() == "gzip")  // 如果使用了GZip则先解压
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (var Zip_Stream = new GZipStream(Stream_Receive, CompressionMode.Decompress))
                    {
                        using (StreamReader Stream_Reader = new StreamReader(Zip_Stream, Encoding.UTF8))
                        {
                            html = Stream_Reader.ReadToEnd();

                            
                        }
                    }
                }
            }
            else
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (StreamReader Stream_Reader = new StreamReader(Stream_Receive, Encoding.UTF8))
                    {
                        html = Stream_Reader.ReadToEnd();
                    }
                }
            }

            return html;
        }

        /// <summary>
        /// 将cookie字符串转化为Arraylist
        /// </summary>
        /// <param name="strCookHeader">需要转化的字符串</param>
        /// <returns>Arraylis格式的字符串</returns>
        private static ArrayList ConvertCookieHeaderToArrayList(string strCookHeader)
        {
            strCookHeader = strCookHeader.Replace("\r", "");
            strCookHeader = strCookHeader.Replace("\n", "");
            string[] strCookTemp = strCookHeader.Split(',');
            ArrayList al = new ArrayList();
            int i = 0;
            int n = strCookTemp.Length;
            while (i < n)
            {
                if (strCookTemp[i].IndexOf("expires=", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    al.Add(strCookTemp[i] + "," + strCookTemp[i + 1]);
                    i = i + 1;
                }
                else
                {
                    al.Add(strCookTemp[i]);
                }
                i = i + 1;
            }
            return al;
        }
        /// <summary>
        /// 将Arraylist格式的cookie转化为CookieCollection
        /// </summary>
        /// <param name="al"></param>
        /// <param name="strHost"></param>
        /// <returns>CookieCollection格式的cookie</returns>
        private static CookieCollection ConvertCookieArraysToCookieCollection(ArrayList al, string strHost)
        {
            CookieCollection cc = new CookieCollection();

            int alcount = al.Count;
            string strEachCook;
            string[] strEachCookParts;
            for (int i = 0; i < alcount; i++)
            {
                strEachCook = al[i].ToString();
                strEachCookParts = strEachCook.Split(';');
                int intEachCookPartsCount = strEachCookParts.Length;
                string strCNameAndCValue = string.Empty;
                string strPNameAndPValue = string.Empty;
                string strDNameAndDValue = string.Empty;
                string[] NameValuePairTemp;
                Cookie cookTemp = new Cookie();

                for (int j = 0; j < intEachCookPartsCount; j++)
                {
                    if (j == 0)
                    {
                        strCNameAndCValue = strEachCookParts[j];
                        if (strCNameAndCValue != string.Empty)
                        {
                            int firstEqual = strCNameAndCValue.IndexOf("=");
                            string firstName = strCNameAndCValue.Substring(0, firstEqual);
                            string allValue = strCNameAndCValue.Substring(firstEqual + 1, strCNameAndCValue.Length - (firstEqual + 1));
                            cookTemp.Name = firstName;
                            cookTemp.Value = allValue;
                        }
                        continue;
                    }
                    if (strEachCookParts[j].IndexOf("path", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if (strPNameAndPValue != string.Empty)
                        {
                            NameValuePairTemp = strPNameAndPValue.Split('=');
                            if (NameValuePairTemp[1] != string.Empty)
                            {
                                cookTemp.Path = NameValuePairTemp[1];
                            }
                            else
                            {
                                cookTemp.Path = "/";
                            }
                        }
                        continue;
                    }

                    if (strEachCookParts[j].IndexOf("domain", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if (strPNameAndPValue != string.Empty)
                        {
                            NameValuePairTemp = strPNameAndPValue.Split('=');

                            if (NameValuePairTemp[1] != string.Empty)
                            {
                                cookTemp.Domain = NameValuePairTemp[1];
                            }
                            else
                            {
                                cookTemp.Domain = strHost;
                            }
                        }
                        continue;
                    }
                }

                if (cookTemp.Path == string.Empty)
                {
                    cookTemp.Path = "/";
                }
                if (cookTemp.Domain == string.Empty)
                {
                    cookTemp.Domain = strHost;
                }
                cc.Add(cookTemp);
            }
            return cc;
        }
    }
}
#endregion