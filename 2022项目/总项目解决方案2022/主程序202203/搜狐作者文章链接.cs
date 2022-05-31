using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202203
{
    public partial class 搜狐作者文章链接 : Form
    {
        public 搜狐作者文章链接()
        {
            InitializeComponent();
        }

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion


        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);

        }



        public string getuid(string url)
        {
            string html = method.GetUrl(url, "utf-8");
          string uid=  Regex.Match(html, @"""mkey"":""([\s\S]*?)""").Groups[1].Value.Trim();
            return uid;
           

        }

        List<string> lists=new List<string>();
        #region 主程序
        public void run()
        {


            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 15; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    string uid = getuid(text[i]);
                    //MessageBox.Show(uid);
                    for (int page = 1; page < 999; page++)
                    {
                        label3.Text = "正在抓取第："+page+"页";
                        Thread.Sleep(Convert.ToInt32(textBox2.Text)*1000);
                        string url = "https://v2.sohu.com/author-page-api/author-articles/pc/"+ uid + "?pNo=" + page + "&secretStr=";
                       
                        string html = method.GetUrlWithCookie(url, "IPLOC=CN3200; SUV=220504121648UO9W; gidinf=x099980109ee151e2efaaac7500091988e13f51d683a; __gads=ID=348c1744b680b9c0-2264a8184dd300e4:T=1653191342:RT=1653191342:S=ALNI_Mb5my1DTeMMu1qgOxKra-4P7jYloQ; __gpi=UID=000005a0f2d67d1e:T=1653191342:RT=1653191342:S=ALNI_Mau9F5fGFTLUsIwCycF2WzjmaWOsw; clt=1653486180; cld=20220525214300; t=1653486284797; reqtype=pc", "utf-8");
                        //textBox3.Text = html;
                        MatchCollection links = Regex.Matches(html, @"""link"":""([\s\S]*?)""");
                        MatchCollection times = Regex.Matches(html, @"""publicTime"":([\s\S]*?),");
                        if (links.Count == 0)
                        {
                            break;
                        }
                        for (int a = 0; a < links.Count; a++)
                        {

                            try
                            {
                                DateTime time;
                                if (times[a].Groups[1].Value.Contains("-"))
                                {
                                  string  time2= Regex.Match(times[a].Groups[1].Value, @"""([\s\S]*?)T").Groups[1].Value;
                                    time = Convert.ToDateTime(time2);
                                }
                                else
                                {
                                     time = ConvertStringToDateTime(times[a].Groups[1].Value);
                                }
                               
                                if (time > Convert.ToDateTime(dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00")) && time < Convert.ToDateTime(dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59")))
                                {
                                   
                                        lists.Add(links[a].Groups[1].Value);
                                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                        lv1.SubItems.Add(text[i]);
                                        lv1.SubItems.Add("https://" + links[a].Groups[1].Value + "?sec=wd");
                                        lv1.SubItems.Add(time.ToString("yyyy-MM-dd"));
                                    
                                }
                                if (status == false)
                                    return;
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(times[a].Groups[1].Value);
                                
                            }
                        }


                    }

                }
            }
        
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        Thread thread;
        bool status = true;
        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"jDB8R"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
            lists.Clear();
            status = true;
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入链接");
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 搜狐作者文章链接_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"jDB8R"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }
    }
}
