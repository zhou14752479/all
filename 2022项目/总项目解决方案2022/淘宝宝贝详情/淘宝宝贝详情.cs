using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 淘宝宝贝详情
{
    public partial class 淘宝宝贝详情 : Form
    {
        public 淘宝宝贝详情()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        public static string Md5_utf8(string str)
        {
            //将输入字符串转换成字节数组
            var buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            //接着，创建Md5对象进行散列计算
            var data = MD5.Create().ComputeHash(buffer);

            //创建一个新的Stringbuilder收集字节
            var sb = new StringBuilder();

            //遍历每个字节的散列数据 
            foreach (var t in data)
            {
                //格式每一个十六进制字符串
                sb.Append(t.ToString("X2"));
            }

            //返回十六进制字符串
            return sb.ToString().ToLower();
        }


        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) ";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Referer = "https://h5.m.taobao.com/";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

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
        public void run()
        {

            string token = "ade40475cfcf85d1d98e637f7ccb1c11";
            string cookie = "22784b88086d41275fb8520ce149c6d0; csg=a5363123; dnk=zkg852266010; lgc=zkg852266010; munb=1052347548; sg=080; sgcookie=E100NQWdKoj3gfWTe3vNq0Vr9W4awFs%2Fuc5xP68yKsV8ZsN1Rds1r0YHJWz96tIppJ2sIgWl9Iq9BqO26a9mmFSIjtpNmWAgDjIblYHaFBMWjTj1Rzgw7hfW%2BHit5es4hvqP; skt=5db634de4708ae5a; t=597430916dc5c2dc63d192ef0e521af2; tracknick=zkg852266010; uc1=cookie15=UtASsssmOIJ0bQ%3D%3D&cookie14=UoexMNM6s037Mg%3D%3D&existShop=true&cookie21=UtASsssmfaCOMId3WwGQmg%3D%3D; uc3=nk2=GcOvCmiKUSBXqZNU&lg2=UtASsssmOIJ0bQ%3D%3D&vt3=F8dCvC%2B%2FYs%2FnsW2drPg%3D&id2=UoH62EAv27BqSg%3D%3D; uc4=id4=0%40UOnlZ%2FcoxCrIUsehK6kOrwmm4V9%2F&nk4=0%40GwrkntVPltPB9cR46GncA5asW%2FAOpFs%3D; unb=1052347548; xlly_s=1; _samesite_flag_=true; thw=cn; cna=HzAJG44cEGkCAXpgIXQET9pA; _m_h5_tk=ade40475cfcf85d1d98e637f7ccb1c11_1652714214579; _m_h5_tk_enc=39c24423bc27440fd97050c894c00163";
            try
            {

                string itemid = "659356430223";
               string time = GetTimeStamp();
               
                string str = token +"&"+time+ "&12574478&{\"id\":\"" + itemid + "\",\"detail_v\":\"3.5.0\",\"exParams\":\"{\\\"id\\\":\\\"" + itemid+ "\\\",\\\"appReqFrom\\\":\\\"detail\\\",\\\"container_type\\\":\\\"xdetail\\\",\\\"dinamic_v3\\\":\\\"true\\\",\\\"supportV7\\\":\\\"true\\\",\\\"ultron2\\\":\\\"true\\\"}\",\"itemNumId\":\"" + itemid + "\",\"pageCode\":\"miniAppDetail\",\"_from_\":\"miniapp\"}";

                string sign = Md5_utf8(str);

                string url = "https://h5api.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?jsv=2.6.2&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.taobao.detail.getdetail&v=6.0&ttid=202012%40taobao_h5_9.17.0&isSec=0&ecode=0&AntiFlood=true&AntiCreep=true&H5Request=true&type=jsonp&dataType=jsonp&safariGoLogin=true&mainDomain=taobao.com&subDomain=m&prefix=h5api&getJSONP=true&token="+token+"&callback=mtopjsonp1&data=%7B%22id%22%3A%22" + itemid + "%22%2C%22detail_v%22%3A%223.5.0%22%2C%22exParams%22%3A%22%7B%5C%22id%5C%22%3A%5C%22" + itemid + "%5C%22%2C%5C%22appReqFrom%5C%22%3A%5C%22detail%5C%22%2C%5C%22container_type%5C%22%3A%5C%22xdetail%5C%22%2C%5C%22dinamic_v3%5C%22%3A%5C%22true%5C%22%2C%5C%22supportV7%5C%22%3A%5C%22true%5C%22%2C%5C%22ultron2%5C%22%3A%5C%22true%5C%22%7D%22%2C%22itemNumId%22%3A%22"+itemid+"%22%2C%22pageCode%22%3A%22miniAppDetail%22%2C%22_from_%22%3A%22miniapp%22%7D";

                string html = GetUrlWithCookie(url, cookie, "utf-8");

                textBox1.Text = html;

                MatchCollection skunames = Regex.Matches(html, @"""specInfo"":""([\s\S]*?)""");
                MatchCollection times = Regex.Matches(html, @"""gmtCreate"":""([\s\S]*?)""");
                MatchCollection quantitys = Regex.Matches(html, @"""quantity"":""([\s\S]*?)""");









            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        Thread thread;
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
