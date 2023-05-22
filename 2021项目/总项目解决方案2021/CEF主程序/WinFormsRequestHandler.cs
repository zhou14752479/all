using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEF主程序
{
    public class WinFormsRequestHandler : RequestHandler
    {

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //NOTE: In most cases you examine the request.Url and only handle requests you are interested in
            if (request.Url.ToLower().Contains("index"))
                {

                //getToken

                //    //获取request请求postdata参数
                //    using (var postData = request.PostData)
                //    {
                //        if (postData != null)
                //        {
                //            var elements = postData.Elements;

                //            var charSet = request.GetCharSet();

                //            foreach (var element in elements)
                //            {
                //                if (element.Type == PostDataElementType.Bytes)
                //                {
                //                    CEF主程序.body = element.GetBody(charSet);
                //                    MessageBox.Show(CEF主程序.body);

                //                }
                //            }


                //        }

                //    }


                //获取request请求hearder参数
                //StringBuilder sb = new StringBuilder();
                ////sb.Append(request.Url + "\n");
                //foreach (var item in request.Headers)
                //{
                //    sb.Append(item.ToString() + ":" + request.Headers[item.ToString()] + "\n");
                //}
             
                //string path = AppDomain.CurrentDomain.BaseDirectory;
                //FileStream fs1 = new FileStream(path + "token.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                //StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                //sw.WriteLine(sb.ToString());
                //sw.Close();
                //fs1.Close();
                //sw.Dispose();
                //getdata(request.Url);

            }
            StringBuilder sb= new StringBuilder();
            //sb.Append(request.Url + "\n");
            foreach (var item in request.Headers)
            {
                sb.Append(item.ToString() + ":" + request.Headers[item.ToString()] + "\n");
            }
         
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FileStream fs1 = new FileStream(path + "token.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(sb.ToString());
            sw.Close();
            fs1.Close();
            sw.Dispose();
            getdata(request.Url);

            return new WinFormResourceRequestHandler();
        }

        public delegate void GetData(string url);
        public GetData getdata;
        
    }
}
