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

namespace 虎牙直邀请软件
{
    public partial class 虎牙直播邀请 : Form
    {
        public 虎牙直播邀请()
        {
            InitializeComponent();
        }

        public static string cookie = "";
        public void getcatename()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string url = "https://bojianger.com/huya/data/api/common/get_categorys.do?date=" + date + "&keyword=&categoryName=total&categoryId=0&clubName=total&clubNo=total&orderBy=audience_count&getType=all&pageNum=1&pageSize=20";
            string html = method.GetUrl(url, "utf-8");
            MatchCollection catenames = Regex.Matches(html, @"""cate_name"":""([\s\S]*?)""");
            for (int i = 0; i < catenames.Count; i++)
            {
                if (!comboBox1.Items.Contains(catenames[i].Groups[1].Value))
                {
                    comboBox1.Items.Add(catenames[i].Groups[1].Value);
                }
            }
         

        }

        public string gethuyaId(string uid)
        {
            string url = "https://www.huya.com/"+uid;
            string html = method.GetUrl(url, "utf-8");
          string huyaId = Regex.Match(html, @"""yyid"":([\s\S]*?),").Groups[1].Value;
            return huyaId.Replace("\"","");

        }

        int zhubocount = 0;
        int zhubocount_success = 0;
        int zhubocount_fail = 0;
        public void run()
        {

            zhubocount = 0;
            try
            {
               
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string categoryName = System.Web.HttpUtility.UrlEncode(comboBox1.Text);
                string clubname = System.Web.HttpUtility.UrlEncode(comboBox2.Text);
                string clubno = "total";

                if(comboBox2.Text=="未签约")
                {
                    clubno = "0";
                }
                for (int page = 1; page < 9999; page++)
                {
                    string url = "https://bojianger.com/huya/data/api/common/anchor_list.do?date=" + date + "&keyword=&categoryName=" + categoryName + "&categoryId=&clubName=" + clubname + "&clubNo="+clubno+"&orderBy=audience_count&getType=all&pageNum=" + page + "&pageSize=30";
                    string html = method.GetUrl(url, "utf-8");
                    MatchCollection rids = Regex.Matches(html, @"""rid"":([\s\S]*?),");
                    MatchCollection names = Regex.Matches(html, @"rid([\s\S]*?)avator");
                    MatchCollection club_names = Regex.Matches(html, @"""club_name"":""([\s\S]*?)""");

                    if (rids.Count == 0)
                    {
                        textBox1.Text += DateTime.Now.ToLongTimeString() + "：抓取完成";
                        return;
                    }

                    for (int a = 0; a < rids.Count; a++)
                    {
                        if (status == false)
                            return;
                        try
                        {
                            string name = Regex.Match(names[a].Groups[1].Value, @"""name"":""([\s\S]*?)""").Groups[1].Value;
                         
                               
                                string huyaid = gethuyaId(rids[a].Groups[1].Value);
                                string yaoqing = "未邀请";

                            if(huyaid=="")
                            {
                                huyaid = "违规";
                            }
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(huyaid);
                                lv1.SubItems.Add(name);
                                lv1.SubItems.Add(rids[a].Groups[1].Value);
                                lv1.SubItems.Add(yaoqing);
                                lv1.SubItems.Add(club_names[a].Groups[1].Value);
                                Thread.Sleep(100);
                                if (listView1.Items.Count > 2)
                                {
                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                }

                            zhubocount = zhubocount + 1;
                            textBox1.Text = "已采集到主播数："+zhubocount;


                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }

                    Thread.Sleep(1000);
                }
              
            }
            catch (Exception ex)
            {

                textBox1.Text=ex.ToString();
            }
        }




        public void yaoyue()
        {
            zhubocount_success = 0;
            zhubocount_fail = 0;
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                
                try
                {

                    int selecttime = comboBox4.SelectedIndex + 1;
                    string huyaid = listView1.CheckedItems[i].SubItems[1].Text;
                    string url = "https://ghback.huya.com/index.php?m=Complay&do=owSignInvite&channelId=92325&yy_id="+huyaid+"&percent="+numericUpDown1.Value+"&selecttime="+ selecttime + "&message="+ System.Web.HttpUtility.UrlEncode(textBox1.Text.Trim());
                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                    string message = Regex.Match(html, @"""message"":""([\s\S]*?)""").Groups[1].Value;
                    listView1.CheckedItems[i].SubItems[4].Text =method.Unicode2String(message);
                    if(message.Contains("成功"))
                    {
                        zhubocount_success = zhubocount_success+ 1;
                        textBox1.Text = "已采集到主播数：" + zhubocount + ";成功邀约数：" + zhubocount_success+";邀请失败数："+zhubocount_fail ;
                    }

                    else
                    {
                        zhubocount_fail = zhubocount_fail + 1;
                        textBox1.Text = "已采集到主播数：" + zhubocount + ";成功邀约数：" + zhubocount_success + ";邀请失败数：" + zhubocount_fail;
                    }
                    if (yaoyue_status == false)
                        return;
                }
                catch (Exception ex)
                {
                    textBox1.Text = ex.ToString();
                    continue;
                }
            }
        }
        private void 虎牙直播邀请_Load(object sender, EventArgs e)
        {
            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"bH3Q"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
            getcatename();
        }


        Thread thread;
        bool status = true;
        bool yaoyue_status = true;
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
            status = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            yaoyue_status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(yaoyue);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            yaoyue_status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    listView1.Items[i].Checked = false;
                }
                else 
                {
                    listView1.Items[i].Checked = true;
                }
            }
        }
    }
}
