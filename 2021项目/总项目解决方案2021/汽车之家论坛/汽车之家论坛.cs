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

namespace 汽车之家论坛
{
    public partial class 汽车之家论坛 : Form
    {
        public 汽车之家论坛()
        {
            InitializeComponent();
        }
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                request.Accept = "*/*";
                request.Timeout = 100000;
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                return (ex.ToString());



            }

        }
        #endregion


        public int getpage(string url)
        {
            
            string html2 = GetUrlWithCookie(url, "", "gb2312");
            string page = Regex.Match(html2, @"<span class=""fr"">共([\s\S]*?)页").Groups[1].Value;
            return Convert.ToInt32(page)+1;
        }


        public string getvideoviews(string id)
        {
            string url = "https://club.autohome.com.cn/common/api/ClubObjectCounter/GetCount?type=2&idList="+id;
            string html = GetUrlWithCookie(url, "", "gb2312");
            string view = Regex.Match(html, @"views"":([\s\S]*?)\}").Groups[1].Value;
            return view;
        }

        #region 主程序
        public void run()
        {

            
            for (int i = 0; i <richTextBox1.Lines.Length;i++)
            {
               
                string startUrl = richTextBox1.Lines[i].ToString().Trim();
                int totalpage = getpage(startUrl);

                for (int page = 1; page < totalpage; page++)
                {

                  string url=  Regex.Replace(startUrl, @"\d\.html", page.ToString()+".html");

                   
                    string html2 = GetUrlWithCookie(url, "", "gb2312");
                    string html = Regex.Match(html2, @"<ul class=""list_dlsubtitle"">([\s\S]*?)<div class=""pagearea"">").Groups[1].Value;
             

                    string name = Regex.Match(html2, @"<h1 title=""([\s\S]*?)""").Groups[1].Value;

                    MatchCollection uids = Regex.Matches(html, @"<dd class=""cli_dd"" lang=""([\s\S]*?)""");
                    MatchCollection videois = Regex.Matches(html, @"<dd class=""cli_dd"" ([\s\S]*?)<span");

                    MatchCollection titles = Regex.Matches(html, @"<a class=""a_topic""([\s\S]*?)href=""([\s\S]*?)"">([\s\S]*?)</a>");
                    MatchCollection dates = Regex.Matches(html, @"<span class=""tdate"">([\s\S]*?)</span>");

                    MatchCollection anames = Regex.Matches(html, @"<span class=""tcount"">([\s\S]*?)linkblack"">([\s\S]*?)</a>");
                    MatchCollection adates = Regex.Matches(html, @"<span class=""ttime"">([\s\S]*?)</span>");

                    MatchCollection dds = Regex.Matches(html, @"<dl class=""list_dl""   lang=""([\s\S]*?)""");
                    if (uids.Count == 0)
                        break;

                    StringBuilder sb = new StringBuilder();

                    foreach (Match uid in uids)
                    {
                        sb.Append(uid.Groups[1].Value+"%2C");
                    }


                  

                    Dictionary<string, string> videoidsdic = new Dictionary<string, string>();
                    for (int a = 0; a < videois.Count; a++)
                    {
                     
                        Match uid = Regex.Match(videois[a].Groups[1].Value, @"lang=""([\s\S]*?)""");
                        Match videoid = Regex.Match(videois[a].Groups[1].Value, @"data-videoid=([\s\S]*?)>");
                        if (videoid.Groups[1].Value != "")
                        {
                   
                            videoidsdic.Add(uid.Groups[1].Value, videoid.Groups[1].Value);
                        }


                       
                    }
                  

                    Dictionary<string, string> dics = new Dictionary<string, string>();
                    string aurl = "https://clubajax.autohome.com.cn/topic/rv?fun=jsonprv&callback=jsonprv&ids="+sb.ToString()+"&r=Thu+Apr+01+2021+12%3A27%3A32+GMT%2B0800+(%E4%B8%AD%E5%9B%BD%E6%A0%87%E5%87%86%E6%97%B6%E9%97%B4)&callback=jsonprv&_=1617251252573";
                    string ahtml = GetUrlWithCookie(aurl, "", "gb2312");
                    MatchCollection values = Regex.Matches(ahtml, @"""topicid"":([\s\S]*?),");
                    MatchCollection values2 = Regex.Matches(ahtml, @"""views"":([\s\S]*?),");

                    for (int a = 0; a < values.Count; a++)
                    {
                        dics.Add(values[a].Groups[1].Value,values2[a].Groups[1].Value);

                    }
                    for (int j = 0; j < uids.Count; j++)
                    {
                        try
                        {
                            string[] text = dds[j].Groups[1].Value.Split(new string[] { "|" }, StringSplitOptions.None);

                            string biaozhi = text[7] + text[8] + text[9];

                      
                            string liulanl = dics[uids[j].Groups[1].Value];
                            string icon = "";
                            switch (biaozhi)
                            {
                                case "0181":
                                    icon = "问";
                                    break;
                                case "301":
                                    icon = "精";
                                    break;
                                case "001":
                                    icon = "图";
                                    break;
                                case "0180":
                                    icon = "问";
                                    break;
                                case "101":
                                    icon = "荐";
                                    break;
                            }

                           

                            if (videoidsdic.ContainsKey(uids[j].Groups[1].Value))
                            {
                                icon = "视频";
                                liulanl = getvideoviews(videoidsdic[uids[j].Groups[1].Value]);
                            }
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(titles[j].Groups[3].Value);
                            lv1.SubItems.Add("https://club.autohome.com.cn" + titles[j].Groups[2].Value);
                            lv1.SubItems.Add(liulanl);
                            lv1.SubItems.Add(text[3]);

                            lv1.SubItems.Add(dates[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(icon);
                            lv1.SubItems.Add(name);

                            lv1.SubItems.Add(text[10]);
                            lv1.SubItems.Add(anames[j].Groups[2].Value.Trim());
                            lv1.SubItems.Add(adates[j].Groups[1].Value.Trim());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                           continue;
                        }
                    }

                    Thread.Sleep(1000);
                }
            }


        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrlWithCookie("http://www.acaiji.com/index/index/vip.html","", "utf-8");

            if (!html.Contains(@"yIepkOu"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        Thread thread;
        bool status = true;
        bool zanting = true;

        #region  listView导出CSV
        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
        /// 
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString(), Encoding.Default);
            MessageBox.Show("导出成功");
        }


        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView1,true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 汽车之家论坛_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 汽车之家论坛_Load(object sender, EventArgs e)
        {

        }
    }
}
