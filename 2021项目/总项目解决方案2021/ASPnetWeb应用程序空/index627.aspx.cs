using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPnetWeb应用程序空
{
    public partial class index627 : System.Web.UI.Page
    {
        public void duqu(string changci)
        {
            string path = "/image627/chang1.txt";
            if (changci == "1")
            {
                path = "/image627/chang1.txt";
            }
            if (changci == "2")
            {
                path = "/image627/chang2.txt";
            }
            if (changci == "3")
            {
                path = "/image627/chang3.txt";
            }
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path, Encoding.GetEncoding("UTF-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (text.Length > 6)
            {
                Application["zuori"] = text[0];
                Application["changname"] = text[1];
                Application["a2"] = text[2];
                Application["a3"] = text[3];
                Application["a4"] = text[4];
                Application["a5"] = text[5];
                Application["a6"] = text[6];
                Application["a7"] = text[7];
            }

            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["changci"] != "")
            {
                string changci = Request["changci"];
                duqu(changci);

            }
        }
    }
}