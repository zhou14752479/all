using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEF主程序
{
    public class yaoshibang_WinFormsRequestHandler : RequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //NOTE: In most cases you examine the request.Url and only handle requests you are interested in
            if (request.Url.ToLower().Contains("getWholesaleList/v4270".ToLower()))
            {



                   //获取request请求postdata参数
                using (var postData = request.PostData)
                {
                    if (postData != null)
                    {
                        var elements = postData.Elements;

                        var charSet = request.GetCharSet();

                        foreach (var element in elements)
                        {
                            if (element.Type == PostDataElementType.Bytes)
                            {
                                CEF主程序.body = element.GetBody(charSet);
                                getdata(CEF主程序.body);

                            }
                        }


                    }

                }







            }
            return new WinFormResourceRequestHandler();
        }


        public delegate void GetData(string url);
        public GetData getdata;
    }
    }
