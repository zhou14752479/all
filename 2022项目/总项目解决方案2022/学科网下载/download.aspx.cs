using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 学科网下载
{
    public partial class download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filekey = Request["filekey"];


            if (filekey != "" && filekey != null)
            {
               
                string fileurl = Base64Decode(Encoding.GetEncoding("utf-8"), filekey);
               
                Response.Redirect(fileurl, false);
            }
            else
            {

                Response.Write("{\"status\":\"0\",\"msg\":\"地址错误\"}");
            }



        }

        #region Base64解码
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        #endregion
    }
}