using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace 网站接口
{
   public class baiduOCR
    {
        #region 根据图片地址获取图片的二进制流
        /// <summary>
        /// 根据图片地址获取图片的二进制流
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static Bitmap Getbmp(string imageUrl)
        {
            string COOKIE = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl);
            request.Proxy = null;
            request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "http://139.159.141.200/app/superscanPH/searchAliimPH.jsp?time=1624578906481";
            request.Timeout = 30000;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0)";
            request.Method = "GET";
            request.Headers.Add("Cookie", COOKIE);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                return null;
            Stream resStream = response.GetResponseStream();
            Bitmap bmp = new Bitmap(resStream);
            response.Close();
            request.Abort();
            return bmp;
           
        }

        #endregion


        #region  bitmap转base64
        public static string BitmapToBase64(Bitmap bmp)
        {
            try
            {

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion



        public static string APIKey = "DNLovFdaPekGFy6BFsFRQ4Ew";
        public static string SecretKey = "ED1dms6gHPotiVKxWbYAyGTP3NLyUdyX";
        public static string access_token = "";
        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <returns></returns>
        public static String getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", APIKey));
            paraList.Add(new KeyValuePair<string, string>("client_secret", SecretKey));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Match AccessToken = Regex.Match(result, @"""access_token"":""([\s\S]*?)""");
            return AccessToken.Groups[1].Value;
        }


        #region 识别文字
        public string shibie(string picurl)
        {
            access_token = getAccessToken();
        
            //string base64encode = System.Web.HttpUtility.UrlEncode(base64);


            string token = access_token;
            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            string base64 = BitmapToBase64(Getbmp(picurl));
            String str = "image=" + HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string result = reader.ReadToEnd();
          
            return result;


        }
        #endregion


    }
}
