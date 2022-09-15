using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace myDLL
{
    public class yzm_ttshitu
    {
        public static string cookie = "";



        #region POST请求

        public static string PostUrl(string url, string postData)
        {
            try
            {
                string charset = "utf-8";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                WebHeaderCollection headers = request.Headers;
                //headers.Add("token: HdPB6c4iYom6hCeTSG6F0Yt22BQm8xbXeC4hXJxc4AllsbL4cj/jTbp8rqom7duLoxSwReeGbFimNeMjYbQjSbkEK6EEfRgwxattv9uDlon4Ok2OTen7BznqWEpv9HQBbp+QCT1FJZXsdOACERF6R1cVpKQJetfYY7NO84umV6vnwfG0HSiqTXTIHcsiIvK0Wh07dIv6bFv7vCum3BZgxhVnyeUPOZScKRytbd1NSmyHffAXiNNRNBPuzOxozvbdiZXR+ppssW3r/0CSxS3I2hfb0yOQpgCgk2DjvyU5Zkw9Ng1Q5PHs7Y/26qG80yuR0s7SPlWYYzqQKEeb89bCNA==");
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.ContentLength = postData.Length;
                // request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;

                request.Accept = "application/json, text/javascript, */*; q=0.01";


                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/register";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                string html = "";
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


                return ex.Message;
            }


        }

        #endregion
        
        #region  网络图片转Bitmap
        public static Bitmap UrlToBitmap(string url)
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

        public static string shibie(string yzmusername, string yzmpassword,string yzmurl)
        {
            try
            {

               
                Bitmap image = UrlToBitmap(yzmurl);
                string param = "{\"username\":\"" + yzmusername + "\",\"password\":\"" + yzmpassword + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                if (result.Groups[1].Value != "")
                {

                    return result.Groups[1].Value;
                }
                else
                {

                  
                    return PostResult;
                }

            }
            catch (Exception ex)
            {

               
                return ex.ToString();
            }
        }
    }
}
