using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 淘宝问答
{
    public class method
    {
       public static DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }

        public static string Md5_utf8(string str)
        {
            //将输入字符串转换成字节数组
            var buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            //接着，创建Md5对象进行散列计算
            var data = MD5.Create().ComputeHash(buffer);

            //创建一个新的Stringbuilder收集字节
            var sb = new StringBuilder();

            //遍历每个字节的散列数据 
            foreach (var t in data)
            {
                //格式每一个十六进制字符串
                sb.Append(t.ToString("X2"));
            }

            //返回十六进制字符串
            return sb.ToString().ToLower();
        }
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
        #region base64加密
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        #endregion
        #region base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        #endregion
        public static List<string> keys_list = new List<string>();

        #region 获取时间戳  秒
        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }
        #endregion



        #region GET请求获取Set-cookie
        public static string getSetCookie(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = false;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

            string content = response.GetResponseHeader("Set-Cookie"); ;
            return content;


        }
        #endregion
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

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
        public string  answer(string itemid,string reviewcookie)
        {
            //string reviewcookie = "thw=cn; enc=HYxeMUeTzCthhdPIdLk0WU7%2Fs2GTWjmKTCc5HJfIS%2BbcjvBNH6zITVSd2Ld0w8jbIvuevTcpWUHeEY2DSUG6pw%3D%3D; useNativeIM=false; t=fafab523e1e10e43a754fee738329ec8; _tb_token_=f37e57a41456e; _m_h5_tk=c9ee96bb84b08dba6ea7ac8663fd1d7b_1672306390155; _m_h5_tk_enc=b54497dbf76f891a02f64628debc7d79; xlly_s=1; cookie2=973d2bcb7e84fd4a8fc2b7e12dc8f740; _samesite_flag_=true; cna=42NtGwhsrnACAXnit7rR4XOG; sgcookie=E100LJ0siFmvsmBAchPSzPlk1Mt9FVoXchvKKZdbDeB51Jg7gwDZha%2F6%2F2xN0ZeWRXo8E9%2B0TTQhShi6mrjpxokdLsCbxYDcHhBO80IUGvvadNk%3D; unb=2209546201082; uc3=lg2=VFC%2FuZ9ayeYq2g%3D%3D&nk2=rOrPqG7pTwQ%3D&id2=UUphw2BBUoAdThjIpw%3D%3D&vt3=F8dCvjESzLnWpM60Fbg%3D; csg=6e8c6550; lgc=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; cancelledSubSites=empty; cookie17=UUphw2BBUoAdThjIpw%3D%3D; dnk=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; skt=c8176230baa750d9; existShop=MTY3MjMwMTE2Ng%3D%3D; uc4=nk4=0%40rsQTqDbhIf9finOOQMGJTYjRzA%3D%3D&id4=0%40U2grGN9DEVUYAflxDbRDtn4wr%2FolMzoZ; tracknick=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; _cc_=VFC%2FuZ9ajQ%3D%3D; _l_g_=Ug%3D%3D; sg=%E4%BA%A72c; _nk_=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; cookie1=Uoe2yNKkZmH3pUqfoam9E5vpHGsvP4NJjcjuwL0Swlw%3D; mt=ci=0_1; uc1=cookie21=UIHiLt3xTwwM1Oej1w%3D%3D&pas=0&cookie15=UIHiLt3xD8xYTw%3D%3D&cookie14=UoezTU0XYr1adw%3D%3D&cookie16=UIHiLt3xCS3yM2h4eKHS9lpEOw%3D%3D&existShop=true; tfstk=cw8lBbsK63SSUX3phL_SWnql1D7hZSrPBe-wuj8NtYpXgQ8VizE4bZ6Ki6HsUk1..; l=fBIrrPgITnyRL7hLXOfwPurza77OSIRAguPzaNbMi9fP_yCD5o_cB6SBWFLkC3GVF6PBR3z1UrspBeYBqIVI5iWc_EleaQkmnmOk-Wf..; isg=BJaWPWGl6cwE1t1oeg5Xkqm650yYN9pxKJRQXgD_PXlDwzZdaMeNgf2hXV8v8NKJ";

            string token = "";
            string neirong = "ㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤㅤ";
            token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
            string time = GetTimeStamp();
            string str = token + "&" + time + "&12574478&{\"topicId\":\""+itemid+"\",\"content\":[],\"desc\":\""+neirong+"\",\"from\":\"\"}";
            string sign = Md5_utf8(str);

            string aurl = "https://h5api.m.taobao.com/h5/mtop.taobao.bala.tag.answer/1.0/?jsv=2.6.1&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.taobao.bala.tag.answer&v=1.0&preventFallback=true&type=jsonp&dataType=jsonp&callback=mtopjsonp4&data=%7B%22topicId%22%3A%22"+itemid+"%22%2C%22content%22%3A%5B%5D%2C%22desc%22%3A%22"+ System.Web.HttpUtility.UrlEncode(neirong)+"%22%2C%22from%22%3A%22%22%7D";

            string html = GetUrlWithCookie(aurl, reviewcookie, "utf-8");

            //MessageBox.Show(html);
            if (html.Contains("过期"))
            {
                string cookiestr = getSetCookie(aurl);
                string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";

                token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                time = GetTimeStamp();
                str = token + "&" + time + "&12574478&{\"topicId\":\"" + itemid + "\",\"content\":[],\"desc\":\"" + neirong + "\",\"from\":\"\"}";
                sign = Md5_utf8(str);

                aurl = "https://h5api.m.taobao.com/h5/mtop.taobao.bala.tag.answer/1.0/?jsv=2.6.1&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.taobao.bala.tag.answer&v=1.0&preventFallback=true&type=jsonp&dataType=jsonp&callback=mtopjsonp4&data=%7B%22topicId%22%3A%22" + itemid + "%22%2C%22content%22%3A%5B%5D%2C%22desc%22%3A%22" + System.Web.HttpUtility.UrlEncode(neirong) + "%22%2C%22from%22%3A%22%22%7D";

                //MessageBox.Show(html);

                html = GetUrlWithCookie(aurl, reviewcookie, "utf-8");
            }


            if(html.Contains("Session"))
            {
                html = "";
            }
            return html;

        }








        public string getitemidbylink(string link)
        {
            try
            {
                string html = GetUrlWithCookie(link, "", "utf-8");
                string id = Regex.Match(html, @"&id=([\s\S]*?)&").Groups[1].Value;
                return id;

            }
            catch (Exception ex)
            {

                return "未获取";
            }
        }

    }
}
