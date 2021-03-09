using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202011
{
    public partial class 抖音接口 : Form
    {
        public 抖音接口()
        {
            InitializeComponent();
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));

        }
       
        
        bool zanting = true;
        string next = "";
        public void getvedio()
        {
            for (int i = 0; i <30; i++)
            {
                
                    string html = method.GetUrl("https://api.vnil.cn/api/batch/getList?appkey=ty9c8pn3rzvym65km5weks48&platform=douyin&uid=52905332217&cursor=" + next, "utf-8");

                    Match nextcursor = Regex.Match(html, @"next_cursor"":([\s\S]*?),");
                    Match has_more = Regex.Match(html, @"has_more"":([\s\S]*?)}");

                    MatchCollection urls = Regex.Matches(html, @"""url"":""([\s\S]*?)""");
                    next = nextcursor.Groups[1].Value;
                    for (int j = 0; j < urls.Count; j++)
                    {
                    try
                    {

                        Match aid = Regex.Match(urls[j].Groups[1].Value, @"\d{10,}");
                        string aurl = "https://www.iesdouyin.com/web/api/v2/aweme/iteminfo/?item_ids=" + aid.Groups[0].Value;
                        string strhtml = method.GetUrl(aurl, "utf-8");
                        Match title = Regex.Match(strhtml, @"""share_title"":""([\s\S]*?)""");
                        Match like = Regex.Match(strhtml, @"""digg_count"":([\s\S]*?),");
                        Match comment = Regex.Match(strhtml, @"""comment_count"":([\s\S]*?),");
                        Match creatime = Regex.Match(strhtml, @"""create_time"":([\s\S]*?),");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(urls[j].Groups[1].Value);
                        lv1.SubItems.Add(title.Groups[1].Value.Replace("}",""));
                        lv1.SubItems.Add(like.Groups[1].Value.Replace("}", ""));
                        lv1.SubItems.Add(comment.Groups[1].Value.Replace("}", ""));
                        lv1.SubItems.Add(ConvertStringToDateTime(creatime.Groups[1].Value).ToString().Replace("}", ""));
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        Thread.Sleep(100);
                        if (has_more.Groups[1].Value.Trim() == "false")
                        {
                            MessageBox.Show("抓取结束");
                            return;
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
                }
                

        
        }

        public static string GetRedirectUrl(string url)
        {

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "HEAD";
                req.Timeout = 5000;
                req.Referer = "";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                req.Headers.Add("Cookie", "MONITOR_WEB_ID=2b91ee0d-614c-44e9-bd15-0dbbe27f05c8");
                req.AllowAutoRedirect = false;
                HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
                //if (myResp.StatusCode == HttpStatusCode.Redirect)
                //{
                //    url = myResp.GetResponseHeader("Location");
                //}
                url = myResp.GetResponseHeader("location");
                return url;
            }
            catch (Exception ex)
            {

              
                return "";
            }
        }
        public void getvedio2()
        {
            try
            {
                string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int j = 0; j < urls.Length; j++)
                {
                  
                    try
                    {

                        string trueurl = method.getSFlocation(urls[j].Trim(),"","");
                     
                        string id = Regex.Match(trueurl, @"video/([\s\S]*?)/").Groups[1].Value;
                       
                        string aurl = "https://www.iesdouyin.com/web/api/v2/aweme/iteminfo/?item_ids=" + id;
                        string strhtml = method.GetUrl(aurl, "utf-8");
                        Match title = Regex.Match(strhtml, @"""share_title"":""([\s\S]*?)""");
                        Match like = Regex.Match(strhtml, @"""digg_count"":([\s\S]*?),");
                        Match comment = Regex.Match(strhtml, @"""comment_count"":([\s\S]*?),");
                        Match creatime = Regex.Match(strhtml, @"""create_time"":([\s\S]*?),");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(urls[j]);
                        lv1.SubItems.Add(title.Groups[1].Value.Replace("}", ""));
                        lv1.SubItems.Add(like.Groups[1].Value.Replace("}", ""));
                        lv1.SubItems.Add(comment.Groups[1].Value.Replace("}", ""));
                       lv1.SubItems.Add(ConvertStringToDateTime(creatime.Groups[1].Value).ToString().Replace("}", ""));
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                        continue;
                      
                    }


                }
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }


        }
        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            if (thread == null || !thread.IsAlive)
            {
             
                thread = new Thread(getvedio2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        Thread thread;
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 抖音接口_Load(object sender, EventArgs e)
        {

        }
    }
}
