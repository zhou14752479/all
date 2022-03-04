using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Threading;

namespace 亚马逊星级评论统计
{
    public partial class 亚马逊星级评论统计 : Form
    {
        public 亚马逊星级评论统计()
        {
            InitializeComponent();
        }


       

        private void Request_www_amazon_com()
        {
           HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.amazon.com/hz/reviews-render/ajax/reviews/get/ref=cm_cr_arp_d_viewopt_sr");

                request.Headers.Add("sec-ch-ua", @""" Not;A Brand"";v=""99"", ""Google Chrome"";v=""97"", ""Chromium"";v=""97""");
                request.Headers.Add("rtt", @"200");
                request.Headers.Add("sec-ch-ua-mobile", @"?0");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.Accept = "text/html,*/*";
                request.Headers.Add("x-requested-with", @"XMLHttpRequest");
                request.Headers.Add("downlink", @"2.35");
                request.Headers.Add("ect", @"4g");
                request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
                request.Headers.Add("origin", @"https://www.amazon.com");
                request.Headers.Add("sec-fetch-site", @"same-origin");
                request.Headers.Add("sec-fetch-mode", @"cors");
                request.Headers.Add("sec-fetch-dest", @"empty");
                request.Referer = "https://www.amazon.com/RRecomfit-360-degree-Phone-Holder-Gooseneck/product-reviews/B00ZCE4BH2/ref=cm_cr_arp_d_viewopt_sr?ie=UTF8&reviewerType=all_reviews&filterByStar=three_star&pageNumber=1";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.Cookie, @"session-id=139-1347714-3595752; session-id-time=2082787201l; i18n-prefs=USD; sp-cdn=""L5Z9:CN""; ubid-main=133-6364598-4421762; lc-main=en_US; session-token=9ZN03TfsLk3dA0SGeGjNFbFyUPWB8HXd3RA9s+7niIEW166my3GsuyaFzS/cPQx94K4swW4As4djh1chVwZcSYzAVXlsRbe6+CH2bFkkRt1slA5LCXwtG0mLt/UqmHYIDdCG9lL26o4vXdvRyvJTLttVQivLqYiDalUTVV/Szj0HT/BPgZC2AbPUoPuFCH8n; csm-hit=tb:E1GKQ46F7R4D3VEQPZHQ+sa-E5088WS2JX0EPJZDMFR8-7EBMNWDDSKVZ56W1YJGE|1646394986156&t:1646394986156&adb:adblk_no");

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = @"sortBy=&reviewerType=all_reviews&formatType=&mediaType=&filterByStar=two_star&pageNumber=1&filterByLanguage=&filterByKeyword=&shouldAppend=undefined&deviceType=desktop&canShowIntHeader=undefined&reftag=cm_cr_arp_d_viewopt_sr&pageSize=10&asin=B07Q6XDZGC&scope=reviewsAjax2";
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();
               
                response = (HttpWebResponse)request.GetResponse();

                string html = "";
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
                // textBox1.Text = html;

                string values = Regex.Match(html, @"a-spacing-base a-size-base\\"">([\s\S]*?)</div>").Groups[1].Value.Replace("\\n","").Trim();
                textBox1.Text = values;
                response.Close();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                
            }
            catch (Exception)
            {
                if (response != null) response.Close();
               
            }

            
        }

        private void 亚马逊星级评论统计_Load(object sender, EventArgs e)
        {
           
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(Request_www_amazon_com);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
          
        }
    }
}
