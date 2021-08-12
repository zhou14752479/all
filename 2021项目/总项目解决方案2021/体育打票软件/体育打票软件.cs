using gregn6Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    public partial class 体育打票软件 : Form
    {
        public 体育打票软件()
        {
            InitializeComponent();
        }
        private GridppReport Report = new GridppReport();
        private void 体育打票软件_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");

            Report.LoadFromFile(@"C:\Users\zhou\Desktop\soft\a.grf");
            //Report.LoadFromFile(@"C:\Grid++Report 6\Samples\Reports\1a.简单表格.grf");
            //Report.DetailGrid.Recordset.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            //    @"User ID=Admin;Data Source=C:\Grid++Report 6\\Samples\Data\Northwind.mdb";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");

            
        }


        public void getdata()
        {
            string html = webBrowser1.Document.Body.OuterHtml;
            string ahtml = webBrowser1.DocumentText;
           
            string fangshi = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            //string suiji = Regex.Match(html, @"""sessionId"":""([\s\S]*?)""").Groups[1].Value;

          
            string guoguan = "过关方式 "+Regex.Match(html, @"checked="""">([\s\S]*?)</span>").Groups[1].Value;
            string beishu = Regex.Match(html, @"</span> x([\s\S]*?)</span> x([\s\S]*?)倍").Groups[2].Value;
            string jine = Regex.Match(html, @"<span id=""consume"">([\s\S]*?)</span>").Groups[1].Value;


            MatchCollection value1 = Regex.Matches(html, @"<span class=""delSelTrBtn"">([\s\S]*?)</tr>");



            MatchCollection matchIds = Regex.Matches(html, @"class=""mCodeCls""([\s\S]*?)>([\s\S]*?)</td>");
            MatchCollection matchNames = Regex.Matches(html, @"<span class=""AgainstInfo"">([\s\S]*?)</span>");
           

            Dictionary<string, string> dics = new Dictionary<string, string>();
            for (int i = 0; i < matchIds.Count; i++)
            {
                dics.Add(matchIds[i].Groups[2].Value, Regex.Replace(matchNames[i].Groups[1].Value, "<[^>]+>", "") );
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < value1.Count; i++)
            {
                string a1 = Regex.Match(value1[i].Groups[1].Value, @"</td><td>周([\s\S]*?)</td>").Groups[1].Value;
                string a2= Regex.Match(value1[i].Groups[1].Value, @"class=""selOption"">([\s\S]*?)</span>").Groups[1].Value;
                sb.Append("第"+(i+1)+"场  周"+a1+"\n");
                sb.Append(dics["周"+a1] + "\n");
                sb.Append(a2+"@" + "\n");
            }




          // textBox1.Text = html;
            // string zhanhao = Regex.Match(html, @"""sessionId"":""([\s\S]*?)""").Groups[1].Value;
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Report.ParameterByName("fangshi").AsString = fangshi;
            Report.ParameterByName("leixing").AsString = leixing;
            Report.ParameterByName("guoguan").AsString = guoguan;
            Report.ParameterByName("beishu").AsString = beishu;
          
            Report.ParameterByName("jine").AsString = jine;
            Report.ParameterByName("neirong").AsString = sb.ToString();

            Report.ParameterByName("time").AsString = time;
        }
        private void button2_Click(object sender, EventArgs e)
        {
           


        }

        private void button7_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion
        private void button4_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html =GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"FCUoF"))
            {

                return;
            }

            #endregion
            getdata();

            // Report.Print(true);
            //Report.PrintPreview(true);
            PreviewForm theForm = new PreviewForm();
            theForm.AttachReport(Report);
            theForm.ShowDialog();
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");
        }
    }
}
