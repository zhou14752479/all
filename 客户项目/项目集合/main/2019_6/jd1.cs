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
    public partial class jd1 : Form
    {
        public jd1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
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
        ArrayList finishes = new ArrayList();
        public void run()
        {
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length - 1; a++)
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

                        string html2 = method.gethtml(URL, "", "utf-8");
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

                        Match name = Regex.Match(html, @"<title>([\s\S]*?)</title>");
                        Match count = Regex.Match(strhtml, @"<em>共([\s\S]*?)条");

                        if (count.Groups[1].Value != "" && name.Groups[1].Value != "")
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      

                            lv1.SubItems.Add(name.Groups[1].Value);
                            lv1.SubItems.Add(count.Groups[1].Value);
                            lv1.SubItems.Add(text[a]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void jd1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("唯一购买VX：17606117606，后台监控侵权必究");
        }
    }
}
