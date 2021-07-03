using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System.IO;
using System.Drawing;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpImage_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string u = HttpHelper.GetUrlIp("http://www.sufeinet.com");


            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://www.sufeinet.com/template/veikei_dz_life_20130810_plus/images/logo.png?2014-06-06",//URL     必需项    
                Method = "get",//URL     可选项 默认为Get   
                ResultType = ResultType.Byte
            };
            HttpResult result = http.GetHtml(item);

            Image img = HttpHelper.GetImage(result.ResultByte);
        }
    }
}