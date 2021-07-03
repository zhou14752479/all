using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using System.Text;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpJson_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //简单类型的对象
            SiteInfo siteinfo = new SiteInfo() { Domain = "www.sufeinet.com", SiteName = "苏飞论坛", Stationmaster = "苏飞" };

            string resultjson = HttpHelper.ObjectToJson(siteinfo);
            Response.Write(resultjson);

            List<SiteInfo> list = new List<SiteInfo>();
            list.Add(new SiteInfo() { Domain = "http://www.sufeinet.com/home.php?mod=space&do=notice&view=mypost", SiteName = "苏飞论坛", Stationmaster = "苏飞" });
            list.Add(new SiteInfo() { Domain = "www.baidu.com", SiteName = "百度", Stationmaster = "李彦宏" });
            list.Add(new SiteInfo() { Domain = "www.taobao.com", SiteName = "淘宝", Stationmaster = "马云" });

            resultjson = HttpHelper.ObjectToJson(list);

            Response.Write("<br/><br/>" + resultjson);


            string strjson = "{\"Stationmaster\":\"苏飞\",\"Domain\":\"www.sufeinet.com\",\"SiteName\":\"苏飞论坛\"}";

            SiteInfo objjson = (SiteInfo)HttpHelper.JsonToObject<SiteInfo>(strjson);

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("    {");
            sb.Append("        \"Stationmaster\": \"苏飞\",");
            sb.Append("        \"Domain\": \"www.sufeinet.com\",");
            sb.Append("        \"SiteName\": \"苏飞论坛\"");
            sb.Append("    },");
            sb.Append("    {");
            sb.Append("        \"Stationmaster\": \"李彦宏\",");
            sb.Append("        \"Domain\": \"www.baidu.com\",");
            sb.Append("        \"SiteName\": \"百度\"");
            sb.Append("    },");
            sb.Append("    {");
            sb.Append("        \"Stationmaster\": \"马云\",");
            sb.Append("        \"Domain\": \"www.taobao.com\",");
            sb.Append("        \"SiteName\": \"淘宝\"");
            sb.Append("    }");
            sb.Append("]");

            List<SiteInfo> jsonlist = (List<SiteInfo>)HttpHelper.JsonToObject<List<SiteInfo>>(sb.ToString());



        }
    }

    public class SiteInfo
    {
        /// <summary>
        /// 网站站长
        /// </summary>
        public string Stationmaster { get; set; }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }
    }
}