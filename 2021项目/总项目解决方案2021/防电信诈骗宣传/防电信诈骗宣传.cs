
using Microsoft.Win32;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 防电信诈骗宣传
{
    public partial class 防电信诈骗宣传 : Form
    {
        public 防电信诈骗宣传()
        {
            InitializeComponent();
        }
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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
        public void quanguo()
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                string keyword = "电诈";
                string url = "http://sousuo.cpd.com.cn/was5/web/search?channelid=290934&searchword="+ System.Web.HttpUtility.UrlEncode(keyword);
                string html = GetUrl(url,"utf-8");
              
                MatchCollection titles = Regex.Matches(html, @"class=""searchresulttitle"" target=""_blank"">([\s\S]*?)</a>([\s\S]*?)<div>([\s\S]*?)</div>");
                MatchCollection urls = Regex.Matches(html, @"<div><a href=""([\s\S]*?)""");
                for (int i = 0; i < 6; i++)
                {
                    string title = Regex.Replace(titles[i].Groups[1].Value.Trim(), "<[^>]+>", "");
                    string subtitle = Regex.Replace(titles[i].Groups[3].Value.Trim(), "<[^>]+>", "");
                    string site = Regex.Replace(urls[i].Groups[1].Value.Trim(), "<[^>]+>", "").Trim();
                    if (subtitle.Length>22)
                    {
                        subtitle = subtitle.Substring(0,22)+"...";
                    }
                 
                    sb.Append("<li class=\"clearfix\">");
                    sb.Append("<div class=\"pic\"><a href=\"" + site + "\"><img src=\"" + pics[i] + "\"></a></div>");
                    sb.Append("<div class=\"tit\">");
                    sb.Append("<h3><a href=\"" + site + "\">" + title+"</a></h3>");
                    sb.Append("<p>"+subtitle+"</p>");
                    sb.Append("<div class=\"intro\"></div>");
                    sb.Append("</div>");
                    sb.Append("</li>");
                }

                StreamReader sr = new StreamReader(Application.StartupPath + @"\index\index.html", EncodingType.GetTxtType(Application.StartupPath + @"\index\index.html"));
                //一次性读取完 
                string body = sr.ReadToEnd();
                string oldvalue= Regex.Match(body, @"<ul class=""clearfix"">([\s\S]*?)</ul>").Groups[1].Value;

                string oldvalue2 = Regex.Match(body, @"<span style=""color:red"">([\s\S]*?)</span>").Groups[1].Value;
                body = body.Replace(oldvalue2, "全国");

                body = body.Replace(oldvalue, sb.ToString());
               
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                System.IO.File.WriteAllText(Application.StartupPath + @"\index\index.html", body, Encoding.UTF8);

            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }



        public void sheng()
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                //string keyword = "电诈";
                //string url = "http://gat.hubei.gov.cn/site/gat/search.html?searchWord="+ System.Web.HttpUtility.UrlEncode(keyword) + "&siteId=36&pageSize=10&type=58872" ;
                //string html = GetUrl(url, "utf-8");
                StreamReader sr2 = new StreamReader(Application.StartupPath + @"\index\pro.mhtml", EncodingType.GetTxtType(Application.StartupPath + @"\index\pro.mhtml"));
                //一次性读取完 
                string html= sr2.ReadToEnd();
                MatchCollection titles = Regex.Matches(html, @"title\)"" title=""([\s\S]*?)""");
                MatchCollection urls = Regex.Matches(html, @"<a ng-href=""([\s\S]*?)""");
                MatchCollection subtitles = Regex.Matches(html, @"<span ng-bind-html=""myHTML\(result.Extendedone\)"" class=""ng-binding"">([\s\S]*?)</div>");
                for (int i = 0; i < 6; i++)
                {
                    string title = Regex.Replace(titles[i].Groups[1].Value.Trim(), "<[^>]+>", "").Trim();
                    string subtitle = Regex.Replace(subtitles[i].Groups[1].Value.Trim(), "<[^>]+>", "").Trim();

                    if (subtitle.Length > 22)
                    {
                        subtitle = subtitle.Substring(0, 22) + "...";
                    }
                   
                    sb.Append("<li class=\"clearfix\">");
                    sb.Append("<div class=\"pic\"><a href=\""+urls[i].Groups[1].Value+"\"><img src=\"" + pics[i] + "\"></a></div>");
                    sb.Append("<div class=\"tit\">");
                    sb.Append("<h3><a href=\"" + urls[i].Groups[1].Value + "\">" + title + "</a></h3>");
                    sb.Append("<p>" + subtitle + "</p>");
                    sb.Append("<div class=\"intro\"></div>");
                    sb.Append("</div>");
                    sb.Append("</li>");
                }

                StreamReader sr = new StreamReader(Application.StartupPath + @"\index\index.html", EncodingType.GetTxtType(Application.StartupPath + @"\index\index.html"));
                //一次性读取完 
                string body = sr.ReadToEnd();


                string oldvalue = Regex.Match(body, @"<ul class=""clearfix"">([\s\S]*?)</ul>").Groups[1].Value;
                body = body.Replace(oldvalue, sb.ToString());

                string oldvalue2 = Regex.Match(body, @"<span style=""color:red"">([\s\S]*?)</span>").Groups[1].Value;
                body = body.Replace(oldvalue2, "湖北省");


                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                System.IO.File.WriteAllText(Application.StartupPath + @"\index\index.html", body, Encoding.UTF8);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        //string[] pics = { "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202111/19114901jndm.jpg", "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202110/28101701o4xx.jpg",
        //    "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202110/210900227ey3.png",
        //    "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202109/151109047mpe.jpg",
        //    "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202109/151102417jne.jpg",
        //    "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202106/04111441c6sh.jpg",
        //};

        string[] pics = {  "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202109/151109047mpe.jpg",
           
            "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202108/01171417vh5o.png",
             "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202106/04111441c6sh.jpg",
            "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202109/151102417jne.jpg",
            "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202108/01163447l6uc.JPG",
             "http://gaj.xiaogan.gov.cn/u/cms/sgaj/202106/25113716drlp.png",
           
        };
        public void shi()
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                string keyword = "电诈";
                string url = "http://gaj.xiaogan.gov.cn/search.jspx?q=" + System.Web.HttpUtility.UrlEncode(keyword);
                string html = GetUrl(url, "utf-8");

                MatchCollection titles = Regex.Matches(html, @"<h2 class=""result-title"">([\s\S]*?)</h2>");
                MatchCollection subtitles = Regex.Matches(html, @"<p class=""result-text"">([\s\S]*?)</p>");
                MatchCollection urls = Regex.Matches(html, @"<h2 class=""result-title"">([\s\S]*?)<a href=""([\s\S]*?)""");

                for (int i = 0; i < 6; i++)
                {
                    string title = Regex.Replace(titles[i].Groups[1].Value.Trim(), "<[^>]+>", "").Trim();
                    string subtitle = Regex.Replace(subtitles[i].Groups[1].Value.Trim(), "<[^>]+>", "").Trim();
                    string site= Regex.Replace(urls[i].Groups[2].Value.Trim(), "<[^>]+>", "").Trim();
                    if (subtitle.Length > 22)
                    {
                        subtitle = subtitle.Substring(0, 22) + "...";
                    }

                    sb.Append("<li class=\"clearfix\">");
                    sb.Append("<div class=\"pic\"><a href=\""+site+"\"><img src=\""+pics[i]+"\"></a></div>");
                    sb.Append("<div class=\"tit\">");
                    sb.Append("<h3><a href=\"" + site + "\">" + title + "</a></h3>");
                    sb.Append("<p>" + subtitle + "</p>");
                    sb.Append("<div class=\"intro\"></div>");
                    sb.Append("</div>");
                    sb.Append("</li>");
                }

                StreamReader sr = new StreamReader(Application.StartupPath + @"\index\index.html", EncodingType.GetTxtType(Application.StartupPath + @"\index\index.html"));
                //一次性读取完 
                string body = sr.ReadToEnd();


                string oldvalue = Regex.Match(body, @"<ul class=""clearfix"">([\s\S]*?)</ul>").Groups[1].Value;
                body = body.Replace(oldvalue, sb.ToString());

                string oldvalue2 = Regex.Match(body, @"<span style=""color:red"">([\s\S]*?)</span>").Groups[1].Value;
                body = body.Replace(oldvalue2, "孝感市");


                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                System.IO.File.WriteAllText(Application.StartupPath + @"\index\index.html", body, Encoding.UTF8);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

      
        private void 防电信诈骗宣传_Load(object sender, EventArgs e)
        {
            #region 通用检测

            if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"6WxcM"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
            SetFeatures(11000);

            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(Application.StartupPath + @"\index\index.html");
            sheng();
            webBrowser1.Navigate(Application.StartupPath + @"\index\index.html");


        }

       
        private void timer1_Tick(object sender, EventArgs e)
        {
           sheng();
            //webBrowser1.Navigate(Application.StartupPath + @"\index\index.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
           webBrowser1.GoBack();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
               
            }
        }
    }
}
