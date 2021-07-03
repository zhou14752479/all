using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace mouseClick
{
    class Ocr
    {

        public string split_txt;

        #region  入口程序

        //调用方法OCR_sougou_SogouOCR
        public string OCR_sougou(Image image)
        {
            try
            {
                this.split_txt = "";

                int i = image.Width;
                int j = image.Height;
                if (i < 300)
                {
                    while (i < 300)
                    {
                        j *= 2;
                        i *= 2;
                    }
                }
                if (j < 120)
                {
                    while (j < 120)
                    {
                        j *= 2;
                        i *= 2;
                    }
                }
                Bitmap bitmap = new Bitmap(i, j);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(image, 0, 0, i, j);
                graphics.Save();
                graphics.Dispose();
                JArray jarray = JArray.Parse(((JObject)JsonConvert.DeserializeObject(this.OCR_sougou_SogouOCR(bitmap)))["result"].ToString());
                bitmap.Dispose();
                string rxg = @"content"": ""([\s\S]*?)""";
                Match match = Regex.Match(jarray.ToString(), rxg);
                return match.Groups[1].Value.ToString().Replace(@"\n", "");
            }
            catch
            {
                return "";
            }
        }

        #endregion


        #region   OCR_sougou_SogouOCR

        //调用OCR_sougou_SogouGet和OCR_sougou_Content_Length
        public string OCR_sougou_SogouOCR(Image img)
        {
            CookieContainer cookie = new CookieContainer();
            string url = "http://pic.sogou.com/pic/upload_pic.jsp";
            string str = this.OCR_sougou_SogouPost(url, cookie, this.OCR_sougou_Content_Length(img));
            string url2 = "http://pic.sogou.com/pic/ocr/ocrOnline.jsp?query=" + str;
            string refer = "http://pic.sogou.com/resource/pic/shitu_intro/word_1.html?keyword=" + str;
            return this.OCR_sougou_SogouGet(url2, cookie, refer);
        }

        #endregion

        # region OCR_sougou_SogouGet
        public string OCR_sougou_SogouGet(string url, CookieContainer cookie, string refer)
        {
            string text = "";
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = "GET";
            httpWebRequest.CookieContainer = cookie;
            httpWebRequest.Referer = refer;
            httpWebRequest.Timeout = 10000;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("X-Requested-With: XMLHttpRequest");
            httpWebRequest.Headers.Add("Accept-Encoding: gzip,deflate");
            httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)";
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.ProtocolVersion = new Version(1, 1);
            string result;
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream stream = httpWebResponse.GetResponseStream();
                    if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                        streamReader.Close();
                        httpWebResponse.Close();
                    }
                }
                result = text;
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion

        #region OCR_sougou_Content_Length
        //调用OCR_ImgToByte
        public byte[] OCR_sougou_Content_Length(Image img)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("------WebKitFormBoundary1ZZDB9E4sro7pf0g\r\nContent-Disposition: form-data; name=\"pic_path\"; filename=\"test2018.jpg\"\r\nContent-Type: image/jpeg\r\n\r\n");
            byte[] array = this.OCR_ImgToByte(img);
            byte[] bytes2 = Encoding.UTF8.GetBytes("\r\n------WebKitFormBoundary1ZZDB9E4sro7pf0g--\r\n");
            byte[] array2 = new byte[bytes.Length + array.Length + bytes2.Length];
            bytes.CopyTo(array2, 0);
            array.CopyTo(array2, bytes.Length);
            bytes2.CopyTo(array2, bytes.Length + array.Length);
            return array2;
        }
        #endregion

        #region OCR_ImgToByte
        private byte[] OCR_ImgToByte(Image img)
        {
            byte[] result;
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                img.Save(memoryStream, ImageFormat.Jpeg);
                byte[] array = new byte[memoryStream.Length];
                memoryStream.Position = 0L;
                memoryStream.Read(array, 0, (int)memoryStream.Length);
                memoryStream.Close();
                result = array;
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion

        #region    OCR_sougou_SogouPost
        public string OCR_sougou_SogouPost(string url, CookieContainer cookie, byte[] content)
        {
            string text = "";
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = cookie;
            httpWebRequest.Timeout = 10000;
            httpWebRequest.Referer = "http://pic.sogou.com/resource/pic/shitu_intro/index.html";
            httpWebRequest.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary1ZZDB9E4sro7pf0g";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Headers.Add("Origin: http://pic.sogou.com");
            httpWebRequest.Headers.Add("Accept-Encoding: gzip,deflate");
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)";
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.ProtocolVersion = new Version(1, 1);
            httpWebRequest.ContentLength = (long)content.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(content, 0, content.Length);
            requestStream.Close();
            string result;
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Stream stream = httpWebResponse.GetResponseStream();
                    if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                        streamReader.Close();
                        httpWebResponse.Close();
                    }
                }
                result = text;
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion



    }
}

