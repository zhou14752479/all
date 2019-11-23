using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 爱采集网站
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";
            request.ContentLength = postData.Length;
            //request.AllowAutoRedirect = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);
            request.Referer = "http://www.acaiji.com/admin/Index/write.html";
            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            response.GetResponseHeader("Set-Cookie");
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

            string html = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return html;

        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "usid=07WwkCu3b_78aUPT; IPLOC=CN3213; SUV=00BA2DBC3159B8CD5D2585534E6EA580; CXID=5EA7E0DBFC0F423A95BC1EB511A405C7; SUID=CDB859313118960A000000005D25B077; ssuid=7291915575; pgv_pvi=5970681856; start_time=1562896518693; front_screen_resolution=1920*1080; wuid=AAElSJCaKAAAAAqMCGWoVQEAkwA=; FREQUENCY=1562896843272_13; sg_uuid=6358936283; newsCity=%u5BBF%u8FC1; SNUID=9FB9A0C8F8FC6C9FCB42F1E4F9BFB645; sortcookie=1; sw_uuid=3118318168; ld=3Zllllllll2NrO7hlllllVLmmtGlllllGqOxBkllllwlllllVklll5@@@@@@@@@@; sct=20";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://news.sogou.com/news?query=site%3Asohu.com+%B4%F3%CA%FD%BE%DD&_ast=1571813760&_asf=news.sogou.com&time=0&w=03009900&sort=1&mode=1&manual=&dp=1";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();

                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        

        public void run()
        {
            string html = GetUrl("http://www.chinaz.com/news/", "utf-8");

            MatchCollection matches = Regex.Matches(html, @"<h4><a href=""([\s\S]*?)""");
            ArrayList lists = new ArrayList();
            foreach (Match NextMatch in matches)
            {

                lists.Add("http:" + NextMatch.Groups[1].Value);

            }

            foreach (string list in lists)

            {
                string charu1 = "<a href=\"\">美团商家采集软件</a>";
                string charu2 = "<a href=\"\">商家外卖采集软件</a>";
                string charu3 = "<a href=\"\">企业电话采集软件</a>";
                string charu4 = "<a href=\"\">58同城采集软件</a>";
                string charu = charu1 + charu2 + charu3 + charu4;

                string strhtml = GetUrl(list,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()
                Match title = Regex.Match(strhtml, @"""title"": ""([\s\S]*?)""");
                Match description = Regex.Match(strhtml, @"""description"": ""([\s\S]*?)""");
                Match body = Regex.Match(strhtml, @"站长之家编辑"">([\s\S]*?)<div id=""content-media2"">");

                string biaoti = System.Web.HttpUtility.UrlEncode(title.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                string miaoshu = System.Web.HttpUtility.UrlEncode(description.Groups[1].Value, Encoding.GetEncoding("utf-8"));

                string guanjianci = System.Web.HttpUtility.UrlEncode("美团商家采集软件,58同城采集,地图采集,营销软件,电话号码抓取,商家号码抓取,企业号码抓取", Encoding.GetEncoding("utf-8"));

                string neirong = System.Web.HttpUtility.UrlEncode(charu+body.Groups[1].Value.Replace("站长之家", "爱采集网站")+ charu, Encoding.GetEncoding("utf-8")); 
                
                string date = System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Encoding.GetEncoding("utf-8"));
                string url = "http://www.acaiji.com/admin/Index/write.html";
                string postdata = "biaoti=" + biaoti + "&neirong=" + neirong + "&guanjianci=" + guanjianci + "&laiyuan=&zhaiyao=" + miaoshu + "&verification=8563c07a621ea96d3b640c1f2e71f4cb&suolvetu=&fabushijian=" + date + "&zhiding=0&tuijian=0&pinglun=1&xingshi=0";
                string cookie = textBox1.Text;
                string strhtml2 = PostUrl(url, postdata, cookie, "gb2312");

                Thread.Sleep(1000);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread search_thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();

       
        }

     
    }
}
