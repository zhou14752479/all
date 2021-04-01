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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 新疆继续教育网页版
{
    public partial class 新疆继续教育网页版 : Form
    {
        public 新疆继续教育网页版()
        {
            InitializeComponent();
        }
       

        private string MakeRequests(string url,string postdata,string SOAPAction)
        {
            string html = "";
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Accept = "*/*";
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN");
                request.Referer = "http://platform.xjrsjxjy.com/ClientBin/TechnicalPlatform.Client.xap?v=20200604183056";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity");
                request.ContentType = "text/xml; charset=utf-8";
                //request.Headers.Add("SOAPAction", @"""http://tempuri.org/IClientService/UserLogin""");
                request.Headers.Add("SOAPAction", SOAPAction);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; rv:11.0) like Gecko";
                request.Headers.Set(HttpRequestHeader.Pragma, "no-cache");

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = postdata;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                response = (HttpWebResponse)request.GetResponse();
               
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                response.Close();

                return html;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return "false";
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return "false";
            }

            return html;
        }

        Thread thread;


        public void login()
        {
            string url = "http://platform.xjrsjxjy.com/ClientService.svc";
            string body = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body><UserLogin xmlns=""http://tempuri.org/""><loginKey>79RO8Q9vcEIABnAFstnPi0RExoQ4pIFIAZV/hojQfMU=</loginKey></UserLogin></s:Body></s:Envelope>";
            textBox1.Text = MakeRequests(url, body, @"""http://tempuri.org/IClientService/UserLogin""");

        }

        public void getlesson()
        {
            string url = "http://student.xjrsjxjy.com:802/ClientService.svc";
            string body = @"<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/""><s:Body><GetStudentCoursewaresForClient xmlns=""http://tempuri.org/""><credential>lUtsiwAU6hoX8D3q3HCMundRHRAjJq7vX2/WERI9D5eawjYxgR1suti2I0fpreJCssPqYR/V9M0J11vgzrbjzQ==</credential><isFree>false</isFree><year>2020</year><pageIndex>0</pageIndex><pageSize>2147483647</pageSize></GetStudentCoursewaresForClient></s:Body></s:Envelope>";

            textBox1.Text = MakeRequests(url, body, @"""http://tempuri.org/IClientService/GetStudentCoursewaresForClient""");

        }
        public void run()
        {
            getlesson();


        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
