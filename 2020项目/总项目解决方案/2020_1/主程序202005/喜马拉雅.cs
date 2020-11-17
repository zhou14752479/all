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
using helper;

namespace 主程序202005
{
    public partial class 喜马拉雅 : Form
    {
        public 喜马拉雅()
        {
            InitializeComponent();
        }
        public void run()
        {

            try
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    string url = text[i];

                    string html = method.gethtml(url, "utf-8");
                    MatchCollection urls = Regex.Matches(html, @"""url"":""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @",""title"":""([\s\S]*?)""");
                    MatchCollection bofangs = Regex.Matches(html, @"""playCount"":([\s\S]*?),");
                   
                    for (int j = 0; j < titles.Count; j++)
                    {
                        string URL = "https://m.ximalaya.com" + urls[j].Groups[1].Value;
                        string ahtml = method.gethtml(URL, "utf-8");

                        Match fans = Regex.Match(ahtml, @"style=""font-size:16px""></i>([\s\S]*?)<");
                        Match name = Regex.Match(ahtml, @"<span class=""name sG_"">([\s\S]*?)<");
                        Match dingyue = Regex.Match(ahtml, @"已订阅<!-- -->([\s\S]*?)<");
                        // Match vip = Regex.Match(ahtml, @"vipPayType"":([\s\S]*?),");

                        double bf = 0;
                        double dy = 0;
                        double fan = 0;

                        if (bofangs[j].Groups[1].Value.Replace("\"", "").Contains("万"))
                        {
                            bf = Convert.ToDouble(bofangs[j].Groups[1].Value.Replace("\"", "").Replace("万", "")) * 10000;
                        }
                        else if (bofangs[j].Groups[1].Value.Replace("\"", "").Contains("亿"))
                        {
                            bf = Convert.ToDouble(bofangs[j].Groups[1].Value.Replace("\"", "").Replace("亿", "")) * 100000000;
                        }
                        else
                        {
                            if (bofangs[j].Groups[1].Value.Replace("\"", "") != "")
                            {
                                bf = Convert.ToDouble(bofangs[j].Groups[1].Value.Replace("\"", ""));
                            }
                        }




                        if (dingyue.Groups[1].Value.Contains("万"))
                        {
                            dy = Convert.ToDouble(dingyue.Groups[1].Value.Replace("万", "")) * 10000;
                        }
                        else if (dingyue.Groups[1].Value.Contains("亿"))
                        {
                            dy = Convert.ToDouble(dingyue.Groups[1].Value.Replace("亿", "")) * 100000000;
                        }
                        else
                        {
                            if (dingyue.Groups[1].Value != "")
                            {
                                dy = Convert.ToDouble(dingyue.Groups[1].Value);
                            }
                        }



                        if (fans.Groups[1].Value.Contains("万"))
                        {
                            fan = Convert.ToDouble(fans.Groups[1].Value.Replace("万", "")) * 10000;
                        }
                        else if (fans.Groups[1].Value.Contains("亿"))
                        {
                            fan= Convert.ToDouble(fans.Groups[1].Value.Replace("亿", "")) * 100000000;
                        }
                        else
                        {
                            if (fans.Groups[1].Value != "")
                            {
                                fan = Convert.ToDouble(fans.Groups[1].Value);
                            }
                        }




                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(titles[j].Groups[1].Value);
                        lv1.SubItems.Add(bf.ToString());
                        lv1.SubItems.Add(dy.ToString());
                        lv1.SubItems.Add(name.Groups[1].Value);
                        lv1.SubItems.Add(fan.ToString());
                        if (ahtml.Contains("albumTag _df"))
                        {
                            lv1.SubItems.Add("1");
                        }
                        else
                        {
                            lv1.SubItems.Add("0");
                        }
                        Thread.Sleep(50);

                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"ximalaya"))
            {
               
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void 喜马拉雅_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
