using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CsharpHttpHelper.Enum;
using CsharpHttpHelper;
using System.Security.Cryptography;
using System.Text;

namespace CsharpHttpHelper_Demo
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("rZmA_05a9_saltkey=DKdSzozy; expires=Tue, 07-Oct-2014 03:16:25 GMT; path=/; domain=.sufeinet.com; httponly,rZmA_05a9_lastvisit=1410056185; expires=Tue, 07-Oct-2014 03:16:25 GMT; path=/; domain=.sufeinet.com,rZmA_05a9_sid=dzOHO6; expires=Mon, 08-Sep-2014 03:16:25 GMT; path=/; domain=.sufeinet.com,rZmA_05a9_lastact=1410059785%09forum.php%09; expires=Mon, 08-Sep-2014 03:16:25 GMT; path=/; domain=.sufeinet.com,rZmA_05a9_onlineusernum=326; expires=Sun, 07-Sep-2014 03:21:25 GMT; path=/; domain=.sufeinet.com,rZmA_05a9_sid=dzOHO6; expires=Mon, 08-Sep-2014 03:16:25 GMT; path=/; domain=.sufeinet.com");


            string result = GetSmallCookie(sb.ToString());
        }


        /// <summary>
        /// 根据字符生成Cookie和精简串，将排除path,expires,domain以及重复项
        /// </summary>
        /// <param name="strcookie">Cookie字符串</param>
        /// <returns>精简串</returns>
        internal static string GetSmallCookie(string strcookie)
        {
            StringBuilder sb = new StringBuilder();
            //将Cookie字符串以,;分开，生成一个字符数组，并删除里面的空项
            string[] list = strcookie.ToString().Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in list)
            {
                string itemcookie = item.ToLower().Trim().Replace("\r\n", string.Empty).Replace("\n", string.Empty);
                //排除空字符串
                if (string.IsNullOrWhiteSpace(itemcookie)) continue;
                //排除不存在=号的Cookie项
                if (!itemcookie.Contains("=")) continue;
                //排除path项
                if (itemcookie.Contains("path=")) continue;
                //排除expires项
                if (itemcookie.Contains("expires=")) continue;
                //排除domain项
                if (itemcookie.Contains("domain=")) continue;
                //排除重复项
                if (sb.ToString().Contains(item)) continue;

                //对接Cookie基本的Key和Value串
                sb.Append(string.Format("{0};", item));
            }
            return sb.ToString();
        }
    }
}