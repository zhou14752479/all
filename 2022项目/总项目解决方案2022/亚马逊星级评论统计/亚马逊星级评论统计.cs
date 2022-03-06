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
using myDLL;

namespace 亚马逊星级评论统计
{
    public partial class 亚马逊星级评论统计 : Form
    {
        public 亚马逊星级评论统计()
        {
            InitializeComponent();
        }




        private string Request_www_amazon_com(string asin,string star)
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

                string body = @"sortBy=&reviewerType=all_reviews&formatType=&mediaType=&filterByStar="+star+"&pageNumber=1&filterByLanguage=&filterByKeyword=&shouldAppend=undefined&deviceType=desktop&canShowIntHeader=undefined&reftag=cm_cr_arp_d_viewopt_sr&pageSize=10&asin="+asin+"&scope=reviewsAjax2";
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
                response.Close();
                return html;
               
               
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                return "";
                
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return "";
            }

            
        }


        public void run()
        {
            try
            {
                string[] stars = { "five_star", "four_star", "three_star", "two_star", "one_star" };
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    for (int j = 0; j < stars.Length; j++)
                    {
                        string asin = listView1.Items[i].SubItems[2].Text;
                        string html = Request_www_amazon_com(asin,stars[j]);
                        string values = Regex.Match(html, @"a-spacing-base a-size-base\\"">([\s\S]*?)</div>").Groups[1].Value.Replace("\\n", "").Trim();
                        if(stars[j]=="five_star")
                        {
                            listView1.Items[i].SubItems[3].Text = values;
                        }
                        if (stars[j] == "four_star")
                        {
                            listView1.Items[i].SubItems[4].Text = values;
                        }
                        if (stars[j] == "three_star")
                        {
                            listView1.Items[i].SubItems[5].Text = values;
                        }
                        if (stars[j] == "two_star")
                        {
                            listView1.Items[i].SubItems[6].Text = values;
                        }
                        if (stars[j] == "one_star")
                        {
                            listView1.Items[i].SubItems[7].Text = values;
                        }
                        Thread.Sleep(500);
                        if (status == false)
                            return;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void 亚马逊星级评论统计_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"hGRLg"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (textBox1.Text=="")
            {
                MessageBox.Show("请导入文本");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
          
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd"));
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }


        bool status = true;
        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
