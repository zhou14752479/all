using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_9
{
    public partial class 拍卖 : Form
    {
        public 拍卖()
        {
            InitializeComponent();
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);

        }
        bool zanting = true;
        #region  淘宝拍卖
        public void tb()
        {
            try
            {

                string[] urls =textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                //for (int i = 1; i < 150; i++)
                //{

                //    string url = "https://sf.taobao.com/item_list.htm?spm=&auction_source=0&st_param=-1&auction_start_seg=-1&page=" + i;
                //    string html = method.GetUrl(url, "gbk");

                //    MatchCollection ids = Regex.Matches(html, @"""id"":([\s\S]*?),");



                //    if (ids.Count == 0)
                //        break;

                //    for (int j = 0; j < ids.Count; j++)
                //    {

                //        string  URL = "https://sf-item.taobao.com/sf_item/" + ids[j].Groups[1].Value+".htm" ;  


                foreach (string URL in urls)
                {
                    string strhtml = method.GetUrl(URL, "gbk");



                    Match a1 = Regex.Match(strhtml, @"<h1>([\s\S]*?)</h1>");
                    Match a2 = Regex.Match(strhtml, @"起 拍 价</span>([\s\S]*?)</span>");
                    Match a3 = Regex.Match(strhtml, @"评 估 价</span>([\s\S]*?)</span>");
                    Match a4 = Regex.Match(strhtml, @"保 证 金</span>([\s\S]*?)</span>");
                    Match a5 = Regex.Match(strhtml, @"加价幅度</span>([\s\S]*?)</span>");
                    Match a6 = Regex.Match(strhtml, @"竞价周期</span>([\s\S]*?)</span>"); //起拍时间
                    Match a7 = Regex.Match(strhtml, @"竞价周期</span>([\s\S]*?)</span>");



                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Replace("&yen;", "").Replace(":", "").Trim());
                    listViewItem.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Replace("&yen;", "").Replace(":", "").Trim());
                    listViewItem.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Replace("&yen;", "").Replace(":", "").Trim());
                    listViewItem.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Replace("&yen;", "").Replace(":", "").Trim());
                    listViewItem.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Replace("&yen;", "").Replace(":", "").Trim());
                    listViewItem.SubItems.Add("无");
                    listViewItem.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Replace("&yen;", "").Replace(":", "").Trim());
                    listViewItem.SubItems.Add(URL);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(1000);


                }


            }



            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

       
        #region  京东拍卖
        public void jd()
        {
            try
            {
                string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string URL in urls)
                {
                    Match aid = Regex.Match(URL, @"\d{9}");

                    string URL1 = "https://api.m.jd.com/api?appid=paimai&functionId=getProductBasicInfo&body={%22paimaiId%22:"+aid.Groups[0].Value+"}&loginType=3&jsonp=jsonp_1568511353899_25794";
                    
                    string strhtml = method.gethtml(URL1,"", "utf-8");

                   

                    Match a1 = Regex.Match(strhtml, @"""title"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(strhtml, @"startPrice"":([\s\S]*?),");
                    Match a3 = Regex.Match(strhtml, @"assessmentPrice"":([\s\S]*?),");
                    Match a4 = Regex.Match(strhtml, @"ensurePrice"":([\s\S]*?),");
                    Match a5 = Regex.Match(strhtml, @"priceLowerOffset"":([\s\S]*?),");
                    Match a6 = Regex.Match(strhtml, @"startTime"":([\s\S]*?),"); //起拍时间
                   // Match a7 = Regex.Match(strhtml, @"竞价周期</span>([\s\S]*?)</span>");



                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(a1.Groups[1].Value);
                    listViewItem.SubItems.Add(a2.Groups[1].Value);
                    listViewItem.SubItems.Add(a3.Groups[1].Value);
                    listViewItem.SubItems.Add(a4.Groups[1].Value);
                    listViewItem.SubItems.Add(a5.Groups[1].Value);
                    listViewItem.SubItems.Add(ConvertStringToDateTime(a6.Groups[1].Value).ToString());
                    listViewItem.SubItems.Add("无");
                    listViewItem.SubItems.Add(URL);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(1000);
                }



                  


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion


        #region  公拍网
        public void gp()
        {
            try
            {
                string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string URL in urls)
                {
                            

                    string strhtml = method.GetUrl(URL, "utf-8");


                    Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)-");
                    Match a2 = Regex.Match(strhtml, @"<span id=""Price_Start"">([\s\S]*?)</span>");
                    Match a3 = Regex.Match(strhtml, @"评估价：([\s\S]*?)元");
                    Match a4 = Regex.Match(strhtml, @"<td>保证金：([\s\S]*?)</td>");
                    Match a5 = Regex.Match(strhtml, @"<span id=""Price_Step"">([\s\S]*?)</span>");
                    Match a6 = Regex.Match(strhtml, @"将于([\s\S]*?)至<"); //起拍时间
                                                                                
                     Match a7 = Regex.Match(strhtml, @"在线拍周期：([\s\S]*?)</td>");



                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(a1.Groups[1].Value);
                    listViewItem.SubItems.Add(a2.Groups[1].Value);
                    listViewItem.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                    listViewItem.SubItems.Add(a4.Groups[1].Value);
                    listViewItem.SubItems.Add(a5.Groups[1].Value);
                    listViewItem.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", ""));
                    listViewItem.SubItems.Add(a7.Groups[1].Value);
                    listViewItem.SubItems.Add(URL);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(1000);
                }






            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion
        private void 拍卖_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入网址");
                return;
            }

            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "13.13.13.13")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                if (radioButton1.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(tb));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                else if (radioButton2.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(jd));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                else if (radioButton3.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(gp));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
          


        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
