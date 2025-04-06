using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 高德地图
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            string url = "https://restapi.amap.com/v5/place/text?keywords=装饰公司&region=宿迁市&show_fields=business&page_size=25&page_num=1&key=adabc1b5b1dd1ab353f6fcb716adb40e";
        }
    }
}