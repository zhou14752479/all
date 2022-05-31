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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.amazon.com/-/zh/product-reviews/"+asin+"/ref=cm_cr_arp_d_viewopt_sr?ie=UTF8&reviewerType=all_reviews&filterByStar="+star+"&pageNumber=1");

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("device-memory", @"8");
                request.Headers.Add("sec-ch-device-memory", @"8");
                request.Headers.Add("dpr", @"1");
                request.Headers.Add("sec-ch-dpr", @"1");
                request.Headers.Add("viewport-width", @"2560");
                request.Headers.Add("sec-ch-viewport-width", @"2560");
                request.Headers.Add("rtt", @"200");
                request.Headers.Add("downlink", @"2.5");
                request.Headers.Add("ect", @"4g");
                request.Headers.Add("sec-ch-ua", @""" Not A;Brand"";v=""99"", ""Chromium"";v=""100"", ""Google Chrome"";v=""100""");
                request.Headers.Add("sec-ch-ua-mobile", @"?0");
                request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"navigate");
                request.Headers.Add("Sec-Fetch-User", @"?1");
                request.Headers.Add("Sec-Fetch-Dest", @"document");
                request.Referer = "https://www.amazon.com/-/zh/product-reviews/B082P77FS5/ref=cm_cr_arp_d_viewopt_sr?ie=UTF8&reviewerType=all_reviews&filterByStar=three_star&pageNumber=1";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"session-id=139-2757510-5594321; session-id-time=2082787201l; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=""L5Z9:CN""; ubid-main=133-3327139-8296540; skin=noskin; session-token=""Kkr0pR8S2OXN1CIH3pdLi48USySvZQPsxbkfUS+s05ahW8CbS8PSUiJI1oF1LdY+S2ut1MXeXaet1gEU0jM2gCKHDov/1lGIP26A6YaIZt7UoxivXtW82TWLn5Bp+BaaE0rrlhPBsIT/UPGzOnJoXe3hTEBu1P/6Pg2gx8YIlk18gd4BQTOl2aRsN09Q5PV4svyuNTBgZvMMEUXFjQvDCQ==""; x-amz-captcha-1=1653576419949154; x-amz-captcha-2=h6Ge36C/y4maBfrquJ6CcA==; csm-hit=adb:adblk_no&t:1653569221831&tb:s-B5EWKXCXJZCM1322PYAP|1653569221749");

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
                MessageBox.Show(e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                return "";
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
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
                        //textBox2.Text = html;
                        label2.Text = "正在查询："+ asin;
                        string values = Regex.Match(html, @"a-spacing-base a-size-base\\"">([\s\S]*?)</div>").Groups[1].Value.Replace("\\n", "").Trim();
                        if(values=="")
                        {
                            values = Regex.Match(html, @"a-spacing-base a-size-base"">([\s\S]*?)</div>").Groups[1].Value.Replace("\\n", "").Trim();
                        }
                        //英文下
                        values = values.Replace("total","").Replace("ratings", "#").Replace("with", "").Replace("reviews", "").Replace(" ", "").Replace(",", "").Replace("review", "").Replace("rating", "#").Trim();
                        //中文下
                        values = values.Replace("total", "").Replace("|", "").Replace("全局评级", "#").Replace("全局评论", "").Replace("<span>", "").Replace("</span>", "").Trim();
                       
                        string[] text = values.Split(new string[] { "#" }, StringSplitOptions.None);
                        if (text.Length < 1)
                        {
                          
                            label2.Text = "查询失败";
                            continue;
                        }
                            
                        if (stars[j]=="five_star")
                        {
                            listView1.Items[i].SubItems[3].Text =text[0];
                            listView1.Items[i].SubItems[4].Text = text[1];
                        }
                        if (stars[j] == "four_star")
                        {
                            listView1.Items[i].SubItems[5].Text = text[0];
                            listView1.Items[i].SubItems[6].Text = text[1];
                        }
                        if (stars[j] == "three_star")
                        {
                            listView1.Items[i].SubItems[7].Text = text[0];
                            listView1.Items[i].SubItems[8].Text = text[1];
                        }
                        if (stars[j] == "two_star")
                        {
                            listView1.Items[i].SubItems[9].Text = text[0];
                            listView1.Items[i].SubItems[10].Text = text[1];
                        }
                        if (stars[j] == "one_star")
                        {
                            listView1.Items[i].SubItems[11].Text = text[0];
                            listView1.Items[i].SubItems[12].Text = text[1];
                        }
                        Thread.Sleep(500);

                        if (status == false)
                            return;
                    }
                }
                label2.Text = "查询结束";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(DateTime.Now.ToString("yyyy-MM-dd"));
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
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

        private void Button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
