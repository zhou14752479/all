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
using myDLL;

namespace 主程序202103
{
    public partial class 求购597mm : Form
    {
        public 求购597mm()
        {
            InitializeComponent();
        }
        Thread thread;
        string cookie = "bdshare_firstime=1613808835295; Hm_lvt_e6885059788fc2617b304f7df85469c8=1613808888,1614411582,1614740603; destoon_username=eyzj365; destoon_auth=U2ADZlE2DDIFMlVvAw1WYgV%2FV34BYQo%2FWjdbbgsNBmZUXAJnC2kFNFM2A2dWZgRjBzdRNQgwBDsMMwY0AThRNVMzAzVRYgxrBTRVMwNlVmMFMldhATgKOVpgW2MLNwZjVGQ%3D; Hm_lvt_7adeae9b43e0fbccb9b7537726ae8fb1=1614740561,1615183492,1615537780,1615688276; Hm_lpvt_7adeae9b43e0fbccb9b7537726ae8fb1=1615689441";
        public bool panduan(string id)
        {
            if (checkBox2.Checked == true)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].SubItems[2].Text == id)
                    {
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return false;
            }
        }


        #region 主程序
        public void run()
        {
            textBox1.Text = DateTime.Now.ToString() + "：开始本次抓取...";
            string firstdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                for (int i = 1; i < 100; i++)
                {

                    
                    string url = "http://www.597mm.com/buy/?page="+i;

                    string html = method.GetUrlWithCookie(url,cookie ,"gbk");

                    MatchCollection uids = Regex.Matches(html, @"<u><a href=""([\s\S]*?)""");


                    for (int j = 0; j < uids.Count; j++)
                    {
                    if (!panduan(uids[j].Groups[1].Value))
                    {
                        string aurl = uids[j].Groups[1].Value;
                        string ahtml = method.GetUrlWithCookie(aurl, cookie, "gbk");
                        try
                        {

                            Match title = Regex.Match(ahtml, @"<h1 class=""title_trade"">([\s\S]*?)</h1>");
                            Match lxr = Regex.Match(ahtml, @"联系人</span>([\s\S]*?)<");
                            Match phone = Regex.Match(ahtml, @"电话</span>([\s\S]*?)</li>");
                            Match tel = Regex.Match(ahtml, @"手机</span>([\s\S]*?)</li>");
                            Match area = Regex.Match(ahtml, @"所在地</span>([\s\S]*?)</li>");
                            Match address = Regex.Match(ahtml, @"地址</span>([\s\S]*?)</li>");
                            Match date = Regex.Match(ahtml, @"class=""pubDate"">([\s\S]*?)</td>");
                            Match content= Regex.Match(ahtml, @"id=""content"">([\s\S]*?)<div");
                            Match youxiaoqi = Regex.Match(ahtml, @"有效期至：</td>([\s\S]*?)</td>");
                            if (Convert.ToDateTime(date.Groups[1].Value)< DateTime.Now.Date)
                            {
                                textBox1.Text = DateTime.Now.ToString()+ "：本次抓取结束,等待下次执行...";
                                return;
                            }
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title.Groups[1].Value);
                            lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(phone.Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(area.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(address.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(date.Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(content.Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(youxiaoqi.Groups[1].Value, "<[^>]+>", ""));
                            Thread.Sleep(10);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }

                }

            }
            
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ZkGmf7"))
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

            timer1.Start();
            timer1.Interval=Convert.ToInt32(textBox2.Text)*60*1000;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                timer1.Start();
            }
            else
            {
                textBox1.Text = DateTime.Now.ToString() + "：结束抓取...";
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString() + "：结束抓取...";
            timer1.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 求购597mm_Load(object sender, EventArgs e)
        {

        }
    }
}
