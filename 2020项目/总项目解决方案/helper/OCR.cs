using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace helper
{
   public  class OCR
    {
        #region  网络图片转Bitmap
        public static Bitmap UrlToBitmap(string url,string cookie)
        {
            WebClient mywebclient = new WebClient();
            mywebclient.Headers.Add("Cookie", cookie);
            byte[] Bytes = mywebclient.DownloadData(url);
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);

                Bitmap map = new Bitmap(outputImg);
                return map;
            }
        }
        #endregion

        #region POST请求

        public static string PostUrl(string url, string postData,string cookie)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "http://czt.sc.gov.cn/kj/toPage?toPage=cms/isreal";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }


        }

        #endregion

        #region 图片转base64
        public static string ImgToBase64String(Bitmap bmp)
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
            catch
            {
                return null;
            }
        }

        #endregion

        public static string shibie(string url,string cookie)
        {
            try
            {
                Bitmap image = UrlToBitmap(url,cookie);



                string param = "{\"username\":\"zhou14752479\",\"password\":\"zhoukaige00\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param,"");

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                return result.Groups[1].Value;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }
        }



    }
}
