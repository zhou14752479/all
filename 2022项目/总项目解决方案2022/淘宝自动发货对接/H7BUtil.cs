using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace 淘宝自动发货对接
{
    /// <summary>
    ///  蜂巢开放客户端SDK
    ///  文档：http://help.fw199.com/docs/h7b/taobao
    ///  Date： 2021-06-10
    /// </summary>

    public class H7BUtil
    {
        // 修改密钥appId和appSecret
        private static readonly string appId = "sv8aqEXPhUJJAIR8";
        private static readonly string appSecret = "51QahTFDc6VhCiB7";

        public static String HttpPost(string url, Dictionary<string, string> requestArgs)
        {

            WebRequest apiRequest = WebRequest.Create(url);
            apiRequest.Method = "POST";
            apiRequest.ContentType = "application/x-www-form-urlencoded";
            requestArgs.Add("appid", appId);
            requestArgs.Add("sign", Sign(requestArgs, appSecret));
            string postData = "";
            foreach (var p in requestArgs)
            {
                if (!String.IsNullOrEmpty(postData))
                {
                    postData += "&";
                }
                string tmpStr = String.Format("{0}={1}", p.Key, HttpUtility.UrlEncode(p.Value));
                postData += tmpStr;
            }

            using (var sw = new StreamWriter(apiRequest.GetRequestStream()))
            {
                sw.Write(postData);
            }
            WebResponse serverResponse = null;
            try
            {
                serverResponse = apiRequest.GetResponse();
            }
            catch (WebException we)
            {
                throw we;
            }

            using (Stream apiDataStream = serverResponse.GetResponseStream())
            {
                using (StreamReader apiReader = new StreamReader(apiDataStream, Encoding.GetEncoding("utf-8")))
                {
                    string apiResult = apiReader.ReadToEnd();
                    apiReader.Close();
                    serverResponse.Close();
                    return apiResult;
                }
            }
        }

        public static long GetTimeStamp()
        {
            long timeStamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            return timeStamp / 1000;
        }

        //参数签名
        public static string Sign(IDictionary<string, string> args, string ClientSecret)
        {
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(args, StringComparer.Ordinal);
            string str = "";
            foreach (var m in sortedParams)
            {
                str += (m.Key + m.Value);
            }
            str = ClientSecret + str + ClientSecret;
            var encodeStr = MD5Encrypt(str);
            return encodeStr;
        }

        public static string MD5Encrypt(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }

    }
}
