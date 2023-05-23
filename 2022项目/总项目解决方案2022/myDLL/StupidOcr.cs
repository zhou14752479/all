using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace myDLL
{
    public class StupidOcr
    {
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("Authtoken:f801017874ae403c888fd8983b462a6d");
                //headers.Add("Routename:trackingExpress");
                
                request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = "";

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
        #region  网络图片转Bitmap
        public static Image UrlToBitmap(string url, string cookie)
        {

            WebClient mywebclient = new WebClient();
            mywebclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            mywebclient.Headers.Add("Cookie", cookie);
            byte[] Bytes = mywebclient.DownloadData(url);

            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);

                Bitmap map = new Bitmap(outputImg);
                return map;
            }


            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
            //request.Referer = url;

            //request.Headers.Add("Cookie", cookie);

            //request.KeepAlive = true;

            //HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //Stream stream = response.GetResponseStream();
            //return Image.FromStream(stream);

        }
        #endregion
        public static string ImageToBase64(Image image)
        {
            string result;
            try
            {
                
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0L;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                result = Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }


        #region  StupidOCR
        public static string  OCR(string imgurl,string cookie)
        {
            try
            {
                Image image = UrlToBitmap(imgurl,cookie);
                string bs64 = ImageToBase64(image);
                string url = "http://127.0.0.1:6688/identify_ArithmeticCAPTCHA";
                string postdata = "{\"ImageBase64\": \""+bs64+"\"}";
                string html = PostUrlDefault(url,postdata,cookie);
                string value = Regex.Match(html, @"""raw_result"":([\s\S]*?),").Groups[1].Value.Replace("\"", "").Trim();
                return value;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        #endregion


    }
}
