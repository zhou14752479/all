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
using myDLL;

namespace 新浪研报
{
    public partial class 新浪研报 : Form
    {
        public 新浪研报()
        {
            InitializeComponent();
        }

        Thread thread;
        bool zanting = true;
        bool status = true;

        int chongshicishu = 0;


        int datatiaoshu = 0;

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
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
        //公司研报
        public void run()
        {
            try
            {
                StreamReader sr = new StreamReader(Application.StartupPath + @"\STK_LIST.csv", method.EncodingType.GetTxtType(Application.StartupPath + @"\STK_LIST.csv"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 1; a < text.Length; a++)
                {
                    string[] codes= text[a].Split(new string[] { "," }, StringSplitOptions.None);
                    codes= codes[0].Split(new string[] { "." }, StringSplitOptions.None);
                    
                    string code = codes[1].ToLower() + codes[0];
                    for (int page = 1; page < 100; page++)
                    {
                       
                        string url = "http://stock.finance.sina.com.cn/stock/go.php/vReport_List/kind/search/index.phtml?t1=2&symbol="+code+"&pubdate=&p=" + page;

                        string html = GetUrlwithIP(url, "tps734.kdlapi.com:15818", "" ,"gb2312");
                        MatchCollection uids = Regex.Matches(html, @"rptid/([\s\S]*?)/");

                        MatchCollection titles = Regex.Matches(html, @"title=""([\s\S]*?)""");
                        MatchCollection types = Regex.Matches(html, @"title=""([\s\S]*?)<td>([\s\S]*?)</td>");
                        MatchCollection dates = Regex.Matches(html, @"<td class=""tal f14"">([\s\S]*?)<td>20([\s\S]*?)</td>");
                        MatchCollection jigous = Regex.Matches(html, @"<div class=""fname05""><span>([\s\S]*?)</span>");
                        MatchCollection yjys = Regex.Matches(html, @"<div class=""fname""><span>([\s\S]*?)</span>");

                        label1.Text = "正在采集：" + code+"   页码："+page + "\r\n";
                      
                        if (html.Contains("服务器返回错误"))
                        {
                            //Thread.Sleep(1000);
                            label1.Text = DateTime.Now.ToString() + "屏蔽正在重试"+ chongshicishu;
                            chongshicishu = chongshicishu + 1;
                            if (page > 0)
                            {
                                page = page - 1;
                            }
                            else
                            {
                                page = 0;
                            }
                           
                            continue;
                        }
                        if (uids.Count == 0&& !html.Contains("服务器返回错误"))
                        {          
                            break;
                        }
                           
                        for (int i = 0; i < uids.Count; i++)
                        {
                            string aurl = "http://stock.finance.sina.com.cn/stock/go.php/vReport_Show/kind/search/rptid/" + uids[i].Groups[1].Value + "/index.phtml";
                            string ahtml = GetUrlwithIP(aurl, "tps734.kdlapi.com:15818", "",  "gb2312");
                            string body = Regex.Match(ahtml, @"<div class=""blk_container"">([\s\S]*?)</div>").Groups[1].Value.Replace("&nbsp;", "");

                            if (ahtml.Contains("服务器返回错误"))
                            {
                                //Thread.Sleep(1000);
                                label1.Text =DateTime.Now.ToString()+ "屏蔽正在重试"+chongshicishu;
                                chongshicishu = chongshicishu + 1;
                                if (i>0)
                                {
                                    i = i - 1;
                                }
                                else
                                {
                                    i = 0;
                                }
                                
                                continue;
                            }
                            chongshicishu = 0;

                            //ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                            //lv1.SubItems.Add(titles[i].Groups[1].Value.Replace("/t", "").Replace(",","，"));
                            //lv1.SubItems.Add(types[i].Groups[2].Value.Replace(",", "，"));
                            //lv1.SubItems.Add(Convert.ToDateTime(dates[i].Groups[2].Value).ToString("yyyyMMdd"));
                            //lv1.SubItems.Add(jigous[i].Groups[1].Value.Replace(",", "，"));
                            //lv1.SubItems.Add(yjys[i].Groups[1].Value.Replace(",", "，"));
                            //lv1.SubItems.Add(Regex.Replace(body, "<[^>]+>", "").Trim().Replace(",", "，"));

                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.csv", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                            sw.WriteLine("\""+titles[i].Groups[1].Value.Replace("/t", "").Replace(",", "，")+"\"" + "," + "\"" + types[i].Groups[2].Value.Replace(",", "，")+ "\"" + "," + "\"" + Convert.ToDateTime(dates[i].Groups[2].Value).ToString("yyyyMMdd")+ "\"" + ","+ "\"" + jigous[i].Groups[1].Value.Replace(",", "，")+ "\"" + ","+ "\"" + yjys[i].Groups[1].Value.Replace(",", "，")+ "\"" + ","+ "\"" + Regex.Replace(body, "<[^>]+>", "").Trim().Replace(",", "，")+ "\"");
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            // Thread.Sleep(1000);
                            if (status == false)
                                return;

                        }
                    }

                }
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


     
        //行业研报
        public void run2()
        {
            try
            {
                StreamReader sr = new StreamReader(Application.StartupPath + @"\STK_LIST.csv", method.EncodingType.GetTxtType(Application.StartupPath + @"\STK_LIST.csv"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 1; a < text.Length; a++)
                {
                    string[] codes = text[a].Split(new string[] { "," }, StringSplitOptions.None);
                    codes = codes[0].Split(new string[] { "." }, StringSplitOptions.None);

                    string code = codes[1].ToLower() + codes[0];

                    for (int page = 1; page < 100; page++)
                    {


                        string url = "http://stock.finance.sina.com.cn/stock/go.php/vReport_List/kind/search/index.phtml?t1=3&industry=sw2_110300&symbol=&pubdate=&p=" + page;

                        string html = method.GetUrl(url, "gb2312");

                        MatchCollection uids = Regex.Matches(html, @"rptid/([\s\S]*?)/");

                        MatchCollection titles = Regex.Matches(html, @"title=""([\s\S]*?)""");
                        MatchCollection types = Regex.Matches(html, @"title=""([\s\S]*?)<td>([\s\S]*?)</td>");
                        MatchCollection dates = Regex.Matches(html, @"<td class=""tal f14"">([\s\S]*?)<td>20([\s\S]*?)</td>");
                        MatchCollection jigous = Regex.Matches(html, @"<div class=""fname05""><span>([\s\S]*?)</span>");
                        MatchCollection yjys = Regex.Matches(html, @"<div class=""fname""><span>([\s\S]*?)</span>");

                        label1.Text += "正在采集：" + page + "\r\n";

                        if (uids.Count == 0)
                            break;
                        for (int i = 0; i < uids.Count; i++)
                        {
                            string aurl = "http://stock.finance.sina.com.cn/stock/go.php/vReport_Show/kind/search/rptid/" + uids[i].Groups[1].Value + "/index.phtml";
                            string ahtml = method.GetUrl(aurl, "gb2312");
                            string body = Regex.Match(ahtml, @"<div class=""blk_container"">([\s\S]*?)</div>").Groups[1].Value.Replace("&nbsp;", "");


                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                            lv1.SubItems.Add(titles[i].Groups[1].Value.Replace("/t", ""));
                            lv1.SubItems.Add(types[i].Groups[2].Value);
                            lv1.SubItems.Add(Convert.ToDateTime(dates[i].Groups[2].Value).ToString("yyyyMMdd"));
                            lv1.SubItems.Add(jigous[i].Groups[1].Value);
                            lv1.SubItems.Add(yjys[i].Groups[1].Value);
                            lv1.SubItems.Add(Regex.Replace(body, "<[^>]+>", "").Trim());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                            //Thread.Sleep(1000);

                        }
                    }
                }
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void 新浪研报_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1,true);
            //method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
