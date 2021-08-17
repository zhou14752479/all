using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEF主程序
{
    public class WinFormResourceRequestHandler : ResourceRequestHandler
    {
        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            var filter = FilterManager.CreateFilter(request.Identifier.ToString());
            return filter;
        }

        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            if (request.Url.ToLower().Contains("query".ToLower()))
            {
                var filter = FilterManager.GetFileter(request.Identifier.ToString()) as TestJsonFilter;
                ASCIIEncoding encoding = new ASCIIEncoding();
                //这里截获返回的数据
                var data = encoding.GetString(filter.DataAll.ToArray());
                MessageBox.Show(data);
            }

        }
    }


}
