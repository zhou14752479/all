using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP足球网站
{
    public partial class savedata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Request.RequestType == "POST")
            {
                if (Request.QueryString["type"] == "savedata")
                {
                    save();

                }
                if (Request.QueryString["type"] == "getdata")
                {
                    getdata();
                }




            }


          


        }

        public void save()
        {
           
            string time = Request.Form["time"];
            string zhu = Request.Form["zhu"];
            string ke = Request.Form["ke"];

            string peilv1 = Request.Form["peilv1"];
            string peilv2 = Request.Form["peilv2"];
            string peilv3 = Request.Form["peilv3"];

            string shenglv1 = Request.Form["shenglv1"];
            string shenglv2 = Request.Form["shenglv2"];
            string shenglv3 = Request.Form["shenglv3"];

            string mai1 = Request.Form["mai1"];
            string mai2 = Request.Form["mai2"];
            string mai3 = Request.Form["mai3"];


            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/zuqiudata.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(time+"#"+zhu + "#" + ke + "#" + peilv1 + "#" + peilv2 + "#" + peilv3 + "#" + shenglv1 + "#" + shenglv2 + "#" + shenglv3 + "#" + mai1 + "#" + mai2 + "#" + mai3 + "#");
            sw.Close();
            fs1.Close();
            sw.Dispose();
            Response.Write("{\"msg\":\"保存成功\"}");
        }




        public void getdata()
        {

           
            //Response.Write("{\"zhu\":\"" + zhu + "\",\"ke\":\"" + ke + "\"}");
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/zuqiudata.txt", Encoding.GetEncoding("UTF-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"code\":1,\"msg\":\"成功\",\"data\":[");
            for (int i = 0; i < text.Length; i++)
            {
                
                string[] a = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                if (a.Length > 10)
                {
                    sb.Append("{\"time\":\"" + a[0] + "\",\"zhu\":\"" + a[1] + "\",\"ke\":\"" + a[2] + "\",\"peilv1\":\"" + a[3] + "\",\"peilv2\":\"" + a[4] + "\",\"peilv3\":\"" + a[5] + "\",\"shenglv1\":\"" + a[6] + "\",\"shenglv2\":\"" + a[7] + "\",\"shenglv3\":\"" + a[8] + "\",\"mai1\":\"" + a[9] + "\",\"mai2\":\"" + a[10] + "\",\"mai3\":\"" + a[11] + "\"},");
                }
                }

            sb.Append("]}");
            if(sb.ToString().Substring(sb.ToString().Length-3,1)==",")
            {
                Response.Write(sb.ToString().Remove(sb.ToString().Length - 3, 1));
            }

            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            
        }




    }
}