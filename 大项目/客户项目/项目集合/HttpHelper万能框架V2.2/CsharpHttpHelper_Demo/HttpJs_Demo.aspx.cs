using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CsharpHttpHelper;

namespace CsharpHttpHelper_Demo
{
    public partial class HttpJs_Demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strjs = string.Empty;
            using (StreamReader sr = new StreamReader(Server.MapPath("test.js")))
            {
                strjs = sr.ReadToEnd();
            }

            //调用不带参数的方法
            string main1 = HttpHelper.JavaScriptEval(strjs, "main1()");

            //调用带参数的方法
            string main2 = HttpHelper.JavaScriptEval(strjs, "main2(25)");

            //直接执行JS代码
            string jiafa = HttpHelper.JavaScriptEval(string.Empty, "25+1+4");

            //直接执行JS代码
            string time = HttpHelper.JavaScriptEval(string.Empty, " new Date().toString()");
        }
    }
}