using System;
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

namespace 商标局浏览器
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
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
        private void Button1_Click(object sender, EventArgs e)
        {
           

            

            string URL = "http://h.xunlianip.com/Users-whiteIpAddNew.html?appid=272&appkey=b821d79eb7b33c43965fa59fb21511e0&whiteip=" + textBox1.Text.Trim();
           string ahtml = GetUrl(URL, "utf-8");
            if (ahtml.Contains("最多"))
            {
                string html = GetUrl("http://h.xunlianip.com/Users-whiteIpListNew.html?appid=272&appkey=b821d79eb7b33c43965fa59fb21511e0", "utf-8");
                Match ip1 = Regex.Match(html, @"\[""([\s\S]*?)""");
                string shanchuHtml = GetUrl("http://h.xunlianip.com/Users-whiteIpDelNew.html?appid=272&appkey=b821d79eb7b33c43965fa59fb21511e0&whiteip=" + ip1.Groups[1].Value, "utf-8");
                Thread.Sleep(2000);
                string URL1 = "http://h.xunlianip.com/Users-whiteIpAddNew.html?appid=272&appkey=b821d79eb7b33c43965fa59fb21511e0&whiteip=" + textBox1.Text.Trim();
                textBox2.Text= GetUrl(URL1, "utf-8");

            }
            else
            {
                textBox2.Text = ahtml;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
