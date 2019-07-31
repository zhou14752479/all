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

namespace main._2019_6
{
    public partial class jd : Form
    {
        public jd()
        {
            InitializeComponent();
        }
        bool zanting = true;
        ArrayList finishes = new ArrayList();
        #region  电脑端
        public void run()
        {
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length; a++)
                {

                    if (!finishes.Contains(text[a]))
                    {
                        finishes.Add(text[a]);
                        string url = text[a];
                        // string url = "https://mall.jd.com/index-145893.html";
                        //  string html = method.gethtml(url,"", "utf-8");
                        string html = GetUrl(url);

                        Match match = Regex.Match(html, @"search-"" \+ ([\s\S]*?) ");

                        string URL = "https://mall.jd.com/view_search-" + match.Groups[1].Value + "-0-99-1-24-1.html";
                        textBox2.Text = "正在抓取" + text[a];
                        string html2 = GetUrl(URL);
                        Match id1 = Regex.Match(html2, @"m_render_pageInstance_id=""([\s\S]*?)""");
                        Match id2 = Regex.Match(html2, @"m_render_layout_instance_id=""([\s\S]*?)""");
                        Match id3 = Regex.Match(html2, @"SearchList-([\s\S]*?) ");

                        Match shopid = Regex.Match(html2, @"shopId = ""([\s\S]*?)""");
                        Match id5 = Regex.Match(html2, @"m_render_app_id=""([\s\S]*?)""");
                        Match id6 = Regex.Match(html2, @"vender_id"" value=""([\s\S]*?)""");
                        // string zurl = "https://module-jshop.jd.com/module/allGoods/goods.html?callback=jQuery4333181&sortType=0&appId=" + match.Groups[1].Value + "&pageInstanceId=" + id1.Groups[1].Value + "&searchWord=&pageNo=2&direction=1&instanceId=" + id2.Groups[1].Value + "&modulePrototypeId=55555&moduleTemplateId="+ id3.Groups[1].Value;
                        if (id1.Groups[1].Value == "")
                        {
                            break;
                        }

                        string ZURL = "https://module-jshop.jd.com/module/getModuleHtml.html?orderBy=99&direction=1&pageNo=1&categoryId=0&pageSize=24&pagePrototypeId=8&pageInstanceId=" + id1.Groups[1].Value + "&moduleInstanceId=" + id1.Groups[1].Value + "&prototypeId=68&templateId=" + id3.Groups[1].Value + "&appId=" + id5.Groups[1].Value + "&layoutInstanceId=" + id2.Groups[1].Value + "&origin=0&shopId=" + shopid.Groups[1].Value + "&venderId=" + id6.Groups[1].Value + "&callback=jshop_module_render_callback";
                        string strhtml = GetUrl(ZURL);


                        MatchCollection urls = Regex.Matches(strhtml, @"data-id=\\""([\s\S]*?)\\""");



                        for (int i = 0; i < urls.Count; i++)
                        {

                            string url2 = "https://item.jd.com/" + urls[i].Groups[1].Value + ".html";
                            string priceUrl = "https://p.3.cn/prices/get?skuid=J_" + urls[i].Groups[1].Value;
                            string commenUrl = "https://club.jd.com/comment/productCommentSummaries.action?referenceIds=" + urls[i].Groups[1].Value + "&callback=jQuery6926331";
                            string html3 = method.GetUrl(url2, "gb2312");

                            string priceHtml = GetUrl(priceUrl);
                            string commenHtml = GetUrl(commenUrl);
                            //Match name = Regex.Match(html, @"<title>([\s\S]*?)-");
                            //Match count = Regex.Match(html3, @"<em>共([\s\S]*?)条");

                            Match biaoti = Regex.Match(html3, @"<div class=""item ellipsis"" title=""([\s\S]*?)""");
                            Match price = Regex.Match(priceHtml, @"""p"":""([\s\S]*?)""");
                            Match comments = Regex.Match(commenHtml, @"CommentCountStr"":""([\s\S]*?)""");
                            MatchCollection items = Regex.Matches(html3, @"mbNav-([\s\S]*?)"">([\s\S]*?)</a>");

                            StringBuilder sb = new StringBuilder();
                            for (int z = 0; z < items.Count; z++)
                            {
                                sb.Append(items[z].Groups[2].Value + "-");
                            }

                            if (biaoti.Groups[1].Value != "")
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      

                                lv1.SubItems.Add(biaoti.Groups[1].Value);
                                lv1.SubItems.Add(price.Groups[1].Value);

                                lv1.SubItems.Add(comments.Groups[1].Value);
                                lv1.SubItems.Add(sb.ToString());
                                lv1.SubItems.Add(url2);

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }

                          
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region  手机端
        public void run1()
        {
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length - 1; a++)
                {
                    
                        string url = text[a];
                    textBox2.Text += "正在抓取"+url+"\r\n";
                        Match shopID = Regex.Match(url, @"index-([\s\S]*?)\.");
                    for (int i = 0; i < 999; i++)
                    {


                        string ZURL = "https://wqsou.jd.com/search/searchjson?datatype=1&page="+i+"&pagesize=100&merge_sku=yes&qp_disable=yes&key=ids%2C%2C" + shopID.Groups[1].Value + "&source=omz&_=1564383664458&sceneval=2&g_login_type=1&callback=jsonpCBKV&g_ty=ls";

                        string html = GetUrl(ZURL);

                        MatchCollection Names = Regex.Matches(html, @"warename"": ""([\s\S]*?)""");
                        MatchCollection prices = Regex.Matches(html, @"dredisprice"": ""([\s\S]*?)""");
                        MatchCollection comments = Regex.Matches(html, @"commentcount"": ""([\s\S]*?)""");
                        MatchCollection uids = Regex.Matches(html, @"wareid"": ""([\s\S]*?)""");
                        MatchCollection catids = Regex.Matches(html, @"catid"": ""([\s\S]*?)""");

                        if (Names.Count == 0)
                            break;

                        for (int j = 0; j < Names.Count; j++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      

                            lv1.SubItems.Add(Names[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(prices[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(comments[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(catids[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add("https://item.jd.com/"+uids[j].Groups[1].Value+ ".html");
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Jd_Load(object sender, EventArgs e)
        {
            
        }


        public string GetUrl(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Referer = url;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
    
                Thread thread = new Thread(new ThreadStart(run1));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = true;        }
    }
}
