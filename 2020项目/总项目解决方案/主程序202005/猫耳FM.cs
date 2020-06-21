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
using CsharpHttpHelper;
using helper;

namespace 主程序202005
{
    public partial class 猫耳FM : Form
    {
        public 猫耳FM()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = Url,
                Method = "GET",
                Host = "www.missevan.com",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                Cookie = "_uab_collina=159046363586457619328264; token=5ecc8d6a72769f13c2e00361%7C2637d423574f5506%7C1590463850%7C543ab637d70d22f6; MSESSID=49li2bipu07qk58iq8ufu6pahk; Hm_lvt_91a4e950402ecbaeb38bd149234eb7cc=1590463636,1590463825,1590482098; Hm_lpvt_91a4e950402ecbaeb38bd149234eb7cc=1590482098; SERVERID=832fef4323c87b883d6becf9932943f1|1590482201|1590482098",
                ProxyIp = "tps158.kdlapi.com:15818",

            };


            item.Header.Add("Sec-Fetch-Site", "none");
            item.Header.Add("Sec-Fetch-Mode", "navigate");
            item.Header.Add("Sec-Fetch-User", "?1");
            item.Header.Add("Sec-Fetch-Dest", "document");
            item.Header.Add("Accept-Encoding", "gzip, deflate, br");
            item.Header.Add("Accept-Language", "zh-CN,zh;q=0.9");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;



        }
        #endregion



        private void button1_Click(object sender, EventArgs e)
        {
          

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
          


          
        }
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        public void run()
        {

            try
            {
               
                for (int i = Convert.ToInt32(textBox1.Text); i < Convert.ToInt32(textBox2.Text); i++)
                {
                    string url = "https://www.missevan.com/dramaapi/filter?page="+i+"&filters=0_0_0_0_0&order=3&page_size=20";
                    label3.Text = "正在抓取："+i;
                    string html = GetUrl(url);
                    
                    if (html.Contains("检测到异常"))
                    {
                        MessageBox.Show("异常");

                    }
                        
                    MatchCollection uids = Regex.Matches(html, @"""id"":([\s\S]*?),");
                 

                    for (int j = 0; j < uids.Count; j++)
                    {
                        string URL = "https://www.missevan.com/dramaapi/getdrama?drama_id=" + uids[j].Groups[1].Value;
                        string ahtml = GetUrl(URL);

                        if (ahtml.Contains("检测到异常"))
                        {
                            MessageBox.Show("异常");

                        }
                        Match tags = Regex.Match(ahtml, @"tags([\s\S]*?)\]");
                        Match zhizuos = Regex.Match(ahtml, @"""organization([\s\S]*?)}");


                        Match name = Regex.Match(ahtml, @"""name"":""([\s\S]*?)""");
                        Match bofang = Regex.Match(ahtml, @"view_count"":([\s\S]*?),");
                        MatchCollection qishu = Regex.Matches(ahtml, @"""soundstr"":""([\s\S]*?)""");
                        MatchCollection tag = Regex.Matches(tags.Groups[1].Value, @"""name"":""([\s\S]*?)""");
                        Match zhizuo = Regex.Match(zhizuos.Groups[1].Value, @"""name"":""([\s\S]*?)""");
                        Match price = Regex.Match(ahtml, @"""price"":([\s\S]*?),");
                        MatchCollection cvids = Regex.Matches(ahtml, @"cv_info"":{""id"":([\s\S]*?),");
                        ;
                        MatchCollection cvroles = Regex.Matches(ahtml, @"""character"":""([\s\S]*?)""");

                        StringBuilder tagtag = new StringBuilder();
                        foreach (Match tag1 in tag)
                        {
                            tagtag.Append(tag1.Groups[1].Value+" ");

                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(name.Groups[1].Value);
                        lv1.SubItems.Add(bofang.Groups[1].Value);
                        lv1.SubItems.Add(qishu.Count.ToString());
                        lv1.SubItems.Add(tagtag.ToString());
                        lv1.SubItems.Add(zhizuo.Groups[1].Value);
                        lv1.SubItems.Add(price.Groups[1].Value);

                     
                        for (int a = 0; a < cvids.Count; a++)
                        {
                            if (a == 0)
                            {
                                StringBuilder zuopin = new StringBuilder();
                                string zz = "";
                                for (int b = 1; b < 99; b++)
                                {
                                    string vchtml = GetUrl("https://www.missevan.com/dramaapi/cvinfo?cv_id=" + cvids[a].Groups[1].Value + "&page=" + b);
                                    if (vchtml.Contains("检测到异常"))
                                    {
                                        MessageBox.Show("异常");

                                    }
                                    Match zuozhe = Regex.Match(vchtml, @"""name"":""([\s\S]*?)""");
                                    zz = zuozhe.Groups[1].Value;

                                    MatchCollection zuopins = Regex.Matches(vchtml, @"""drama""([\s\S]*?)""name"":""([\s\S]*?)""");
                                    if (zuopins.Count == 0)
                                        break;
                                    foreach (Match zp in zuopins)
                                    {
                                        zuopin.Append(Unicode2String(zp.Groups[2].Value));
                                    }
                                }

                                lv1.SubItems.Add(Unicode2String(zz));
                                lv1.SubItems.Add(cvroles[a].Groups[1].Value);
                                lv1.SubItems.Add(zuopin.ToString());
                            }

                            else
                            {
                                ListViewItem lv2 = listView1.Items.Add((listView1.Items.Count + 1).ToString());

                                lv2.SubItems.Add("");
                                lv2.SubItems.Add("");
                                lv2.SubItems.Add("");
                                lv2.SubItems.Add("");
                                lv2.SubItems.Add("");
                                lv2.SubItems.Add("");

                                StringBuilder zuopin = new StringBuilder();
                                string zz = "";
                                for (int b = 1; b < 99; b++)
                                {
                                    string vchtml = GetUrl("https://www.missevan.com/dramaapi/cvinfo?cv_id=" + cvids[a].Groups[1].Value + "&page=" + b);
                                    if (vchtml.Contains("检测到异常"))
                                    {
                                        MessageBox.Show("异常");

                                    }
                                    Match zuozhe = Regex.Match(vchtml, @"""name"":""([\s\S]*?)""");
                                    zz = zuozhe.Groups[1].Value;
                                    MatchCollection zuopins = Regex.Matches(vchtml, @"""drama""([\s\S]*?)""name"":""([\s\S]*?)""");
                                    if (zuopins.Count == 0)
                                        break;
                                    foreach (Match zp in zuopins)
                                    {
                                        zuopin.Append(Unicode2String(zp.Groups[2].Value));
                                    }
                                }

                                lv2.SubItems.Add(Unicode2String(zz));
                                lv2.SubItems.Add(cvroles[a].Groups[1].Value);
                                lv2.SubItems.Add(zuopin.ToString());

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }






                          
                        }




                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void 猫耳FM_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
