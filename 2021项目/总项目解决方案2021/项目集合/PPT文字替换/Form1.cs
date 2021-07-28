using Spire.Presentation;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPT文字替换
{
    public partial class Form1 : Form
    {
        public Form1()
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
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
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
        Dictionary<string, string> TagValues = new Dictionary<string, string>();
        public void ReplaceText()
        {
           
            {
                //创建一个Dictionary 实例并添加一个item
               
                //TagValues.Add("Spire", "榜单");
                //加载PowerPoint示例文档
                Presentation presentation = new Presentation();
                presentation.LoadFromFile("Sample.pptx", FileFormat.Pptx2010);

                //调用ReplaceTags事件来替换第一个幻灯片里的文本
                ReplaceTags(presentation.Slides[0], TagValues);

                //保存文档
                presentation.SaveToFile("Result.pptx", FileFormat.Pptx2010);
                System.Diagnostics.Process.Start("Result.pptx");
            }
        }
        public void ReplaceTags(Spire.Presentation.ISlide pSlide, Dictionary<string, string> TagValues)
        {
            foreach (IShape curShape in pSlide.Shapes)
            {
                if (curShape is IAutoShape)
                {
                    foreach (TextParagraph tp in (curShape as IAutoShape).TextFrame.Paragraphs)
                    {
                        foreach (var curKey in TagValues.Keys)
                        {
                            if (tp.Text.Contains(curKey))
                            {
                                tp.Text = tp.Text.Replace(curKey, TagValues[curKey]);
                            }
                        }
                    }
                }
            }
        }
        private String toNumber(long number)
        {
            String str = "";
            if (number <= 0)
            {
                str = "";
            }
            else if (number < 10000)
            {
                str = number + "人观看";
            }
            else
            {
               
               long num = number / 10000;
               
                str = num + "W";
            }
            return str;
        }

        #region 抖音热搜
        public void douyinhotwords()
        {
            TagValues.Clear();
            string url = "https://www.iesdouyin.com/web/api/v2/hotsearch/billboard/word/";
            string html = GetUrl(url,"utf-8");
           
            MatchCollection words= Regex.Matches(html, @"""word"":""([\s\S]*?)""");
            MatchCollection values = Regex.Matches(html, @"""hot_value"":([\s\S]*?),");
            for (int i = 1; i < 16; i++)
            {
                string value = words[i-1].Groups[1].Value;
               TagValues.Add("*文字"+i+"*", value);
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(value);
                lv1.SubItems.Add("");
            }

            for (int i = 1; i < 16; i++)
            {
                try
                {
                    string value = toNumber(Convert.ToInt64(values[i-1].Groups[1].Value.Replace("}", ""))).Trim();
                    TagValues.Add("*值" + i + "*", value);
                    listView1.Items[i-1].SubItems[2].Text = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show(values[i].Groups[1].Value);
                }
            }

            TagValues.Add("title", DateTime.Now.ToString("MM月dd日")+"实时热点榜");
            //TagValues.Add("title", "今日实时热点榜");
            TagValues.Add("beizhu", "数据来源：抖音实时热点");
        }
        #endregion

        #region 微博要闻
        public void weibonews()
        {
            TagValues.Clear();
            string url = "https://s.weibo.com/top/summary?cate=socialevent";
            string html = GetUrl(url, "utf-8");

            MatchCollection words = Regex.Matches(html, @">#([\s\S]*?)#");

            for (int i = 1; i < 16; i++)
            {
                string value = words[i-1].Groups[1].Value;
                TagValues.Add("*文字" + i + "*", value);
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(value);
                lv1.SubItems.Add("");
            }

            for (int i = 1; i < 16; i++)
            {
                string value = i.ToString();
                try
                {
                    TagValues.Add("*值" + i + "*", value);
                    listView1.Items[i-1].SubItems[2].Text = value;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }

            TagValues.Add("title", DateTime.Now.ToString("MM月dd日") + "实时热点榜");
            //TagValues.Add("title",  "今日实时热点榜");
            TagValues.Add("beizhu", "数据来源：微博实时热点");
        }
        #endregion

        #region 单日涨粉排行
        public void zhangfen()
        {
            TagValues.Clear();
            string date = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string url = "https://dy.feigua.cn/rank/fans/0/day/"+date+".html";
            string html = GetUrl(url, "utf-8");

            MatchCollection words = Regex.Matches(html, @"<span>([\s\S]*?)</span>");
            MatchCollection values = Regex.Matches(html, @"<td>([\s\S]*?)</td>");
            for (int i = 1; i < 16; i++)
            {
                string value = words[i-1].Groups[1].Value;
                TagValues.Add("*文字" + i + "*", value);
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(value);
                lv1.SubItems.Add("");
            }

            for (int i = 1; i < 16; i++)
            {
                try
                {
                    string value = values[(4 * (i - 1)) + 2].Groups[1].Value.Trim();
                    TagValues.Add("*值" + i + "*", value);
                    listView1.Items[i-1].SubItems[2].Text = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show(values[i].Groups[1].Value);
                }
            }

            TagValues.Add("title", DateTime.Now.ToString("MM月dd日") + "抖音单日涨粉榜");
           // TagValues.Add("title", "抖音单日涨粉榜");
            TagValues.Add("beizhu", "数据来源：抖音");
        }
        #endregion

        #region 奥运奖牌榜
     
        public void aoyun()
        {
            TagValues.Clear();
           
            string url = "https://tiyu.baidu.com/tokyoly/home/tab/%E5%A5%96%E7%89%8C%E6%A6%9C/from/pc";
            string html = GetUrl(url, "utf-8");

            MatchCollection names = Regex.Matches(html, @"<span class=""name""([\s\S]*?)>([\s\S]*?)</span>");
            MatchCollection jins = Regex.Matches(html, @"<div class=""item-gold""([\s\S]*?)>([\s\S]*?)</div>");
            MatchCollection yins = Regex.Matches(html, @"<div class=""item-silver""([\s\S]*?)>([\s\S]*?)</div>");
            MatchCollection tongs = Regex.Matches(html, @"<div class=""item-copper""([\s\S]*?)>([\s\S]*?)</div>");
            MatchCollection alls = Regex.Matches(html, @"<div class=""item-all""([\s\S]*?)>([\s\S]*?)</div>");

            for (int i = 1; i < 16; i++)
            {
                string value = names[i - 1].Groups[2].Value.Trim() + jins[i - 1].Groups[2].Value.Trim() + "金" + yins[i - 1].Groups[2].Value.Trim() + "银" + tongs[i - 1].Groups[2].Value.Trim() + "铜"; 
                TagValues.Add("*文字" + i + "*", value);
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(value);
                lv1.SubItems.Add("");
            }

            for (int i = 1; i < 16; i++)
            {
                try
                {
                    string value = "总计"+alls[i-1].Groups[2].Value.Trim();
                    TagValues.Add("*值" + i + "*", value);
                    listView1.Items[i - 1].SubItems[2].Text = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                   
                }
            }

            TagValues.Add("title",  "东京奥运金牌榜");
            // TagValues.Add("title", "抖音单日涨粉榜");
            TagValues.Add("beizhu", "截止"+ DateTime.Now.ToString("MM月dd日"));
        }
        #endregion

        #region 百度热搜
        public void baiduresou()
        {
            TagValues.Clear();
            string url = "https://top.baidu.com/board?tab=realtime";
            string html = GetUrl(url, "utf-8");

            MatchCollection words = Regex.Matches(html, @"""word"":""([\s\S]*?)""");
            MatchCollection values = Regex.Matches(html, @"""hotScore"":""([\s\S]*?)""");
            for (int i = 1; i < 16; i++)
            {
                string value = words[i-1].Groups[1].Value;
                TagValues.Add("*文字" + i + "*", value);
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(value);
                lv1.SubItems.Add("");
            }

            for (int i = 1; i < 16; i++)
            {
                try
                {
                    string value = toNumber(Convert.ToInt64(values[i-1].Groups[1].Value.Replace("}", ""))).Trim();
                    TagValues.Add("*值" + i + "*", value);
                    listView1.Items[i - 1].SubItems[2].Text = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show(values[i].Groups[1].Value);
                }
            }

            TagValues.Add("title", DateTime.Now.ToString("MM月dd日") + "实时热搜榜");
            //TagValues.Add("title", "今日实时热点榜");
            TagValues.Add("beizhu", "数据来源：百度热搜");
        }
        #endregion

        #region maigoo网站
        public void maigoo()
        {
            TagValues.Clear();
            string url = "https://s.weibo.com/top/summary?cate=socialevent";
            string html = GetUrl(url, "utf-8");

            MatchCollection words = Regex.Matches(html, @">#([\s\S]*?)#");

            for (int i = 1; i < 16; i++)
            {
                string value = words[i - 1].Groups[1].Value;
                TagValues.Add("*文字" + i + "*", value);
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(value);
                lv1.SubItems.Add("");
            }

            for (int i = 1; i < 16; i++)
            {
                string value = i.ToString();
                try
                {
                    TagValues.Add("*值" + i + "*", value);
                    listView1.Items[i - 1].SubItems[2].Text = value;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }

            TagValues.Add("title", DateTime.Now.ToString("MM月dd日") + "实时热点榜");
            //TagValues.Add("title",  "今日实时热点榜");
            TagValues.Add("beizhu", "数据来源：微博实时热点");
        }
        #endregion

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(ReplaceText);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(douyinhotwords);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            if (radioButton3.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(weibonews);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton4.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(zhangfen);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton5.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(aoyun);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            if (radioButton6.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(baiduresou);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }




        }
    }
}
