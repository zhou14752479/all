using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Net;

namespace CsharpHttpHelper.Helper
{
    /// <summary>
    /// 和Url相关的帮助方法  Copyright：http://www.httphelper.com/
    /// </summary>
    internal class HttpUrlHelper
    {
        /// <summary>
        /// 使用指定的编码对象将 URL 编码的字符串转换为已解码的字符串。
        /// </summary>
        /// <param name="text">指定的字符串</param>
        /// <param name="encoding">指定编码默认为Default</param>
        /// <returns></returns>
        internal static string URLDecode(string text, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            return HttpUtility.UrlDecode(text, encoding);
        }
        /// <summary>
        /// 使用指定的编码对象对 URL 字符串进行编码。
        /// </summary>
        /// <param name="text">指定的字符串</param>
        /// <param name="encoding">指定编码默认为Default</param>
        /// <returns></returns>
        internal static string URLEncode(string text, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            return HttpUtility.UrlEncode(text, encoding);
        }

        /// <summary>
        /// 将Url参数字符串转为一个Key和Value的集合
        /// </summary>
        /// <param name="str">要转为集合的字符串</param>
        /// <returns>NameValueCollection</returns>
        internal static NameValueCollection GetNameValueCollection(string str)
        {
            NameValueCollection coll = null;
            try
            {
                coll = HttpUtility.ParseQueryString(str);
            }
            catch { }
            return coll;
        }
        /// <summary>
        /// 提取网站主机部分就是host
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>host</returns>
        internal static string GetUrlHost(string url)
        {
            try
            {
                return new Uri(url).Host;
            }
            catch { return string.Empty; }
        }
        /// <summary>
        /// 提取网址对应的IP地址
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>返回Url对应的IP地址</returns>
        internal static string GetUrlIp(string url)
        {
            try
            {
                IPHostEntry hostInfo = Dns.GetHostByName(GetUrlHost(url));
                return hostInfo.AddressList[0].ToString();
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 获取当前页面的URL（五个数字可选 1~5 ）
        /// </summary>
        /// <param name="selectNO">1完整url,2域名之后,3域名之后不含参数,4只有域名,5获取参数</param>
        /// <returns></returns>
        public string getNowURL(int selectNO)
        {
            string strURL = "";
            switch (selectNO)
            {
                case 1:
                    strURL = HttpContext.Current.Request.Url.ToString();
                    break;
                case 2:
                    strURL = HttpContext.Current.Request.RawUrl;
                    break;
                case 3:
                    strURL = HttpContext.Current.Request.Url.AbsolutePath;
                    break;
                case 4:
                    strURL = HttpContext.Current.Request.Url.Host;
                    break;
                case 5:
                    strURL = HttpContext.Current.Request.Url.Query;
                    break;
                default:
                    strURL = HttpContext.Current.Request.Url.ToString();
                    break;
            }
            return strURL;
        }
    }
}
