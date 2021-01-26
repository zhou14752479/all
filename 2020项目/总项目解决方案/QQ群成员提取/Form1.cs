using System;
using System.Collections;
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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace QQ群成员提取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> QQS = new List<string>();

        public void QQadduser()
        {
          

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string QQ = listView1.Items[i].SubItems[1].Text;
                string huashu = textBox1.Text.Trim();
             
                if (QQ!="")
                {
                    if (!QQS.Contains(QQ))
                    {
                      
                        QQS.Add(QQ);

                        driver.Navigate().GoToUrl("https://user.qzone.qq.com/" + QQ);

                        try
                        {
                            driver.FindElement(By.XPath("//a[contains(text(),\"加为好友\")]")).Click();
                        }
                        catch (Exception)
                        {

                            driver.FindElement(By.Id("add_friend")).Click();
                        }

                        Thread.Sleep(1000);
                        driver.FindElement(By.Id("FP-addFriend-verifyinput")).SendKeys(huashu);
                        Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//*[text()=\"确定\"]")).Click();
                        Thread.Sleep(1000);
                    }
                }
            }

            


        }



        public string COOKIE { get; set; }
        public string bkn { get; set; }
        long GetBkn()
        {
            string skey = Regex.Match(COOKIE, @"skey=([\s\S]*?);").Groups[1].Value;
            var hash = 5381;
            for (int i = 0, len = skey.Length; i < len; ++i)
                hash += (hash << 5) + (int)skey[i];
            return hash & 2147483647;
        }

       
        private void button5_Click(object sender, EventArgs e)
        {
            
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }


        public void getqunList()
        {
            bkn = GetBkn().ToString();
            string html = method.PostUrl("https://qun.qq.com/cgi-bin/qun_mgr/get_group_list", "bkn="+bkn, COOKIE,"utf-8", "application/x-www-form-urlencoded", "https://qun.qq.com/member.html");
            MatchCollection quns = Regex.Matches(html, @"""gc"":([\s\S]*?),");
            MatchCollection names = Regex.Matches(html, @"""gn"":""([\s\S]*?)""");
            for (int i = 0; i < quns.Count; i++)
            {

       
                ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                lv2.SubItems.Add(method.Unicode2String(names[i].Groups[1].Value));
                lv2.SubItems.Add(quns[i].Groups[1].Value);
              
            }
        }


        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }
        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }

        public bool panduan(string QQ)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[1].Text.Trim()+ listView1.Items[i].SubItems[3].Text.Trim() == QQ.Trim())
                {
                    return true;
                }

            }
            return false;
        }


        public void getqunmember()
        {
            textBox3.Text =DateTime.Now.ToString()+ "：正在监控......";
            bkn = GetBkn().ToString();

          
            for (int a= 0; a < listView2.CheckedItems.Count; a++)
            {

                string qunhao = listView2.CheckedItems[a].SubItems[2].Text;
               
                string postdata = "gc=" + qunhao.Trim()+ "&st=0&end=20&sort=10&bkn=" + bkn;
                string html = method.PostUrl("https://qun.qq.com/cgi-bin/qun_mgr/search_group_members", postdata, COOKIE, "utf-8", "application/x-www-form-urlencoded", "https://qun.qq.com/member.html");
                MatchCollection QQs = Regex.Matches(html, @"""uin"":([\s\S]*?),");
                MatchCollection join_times = Regex.Matches(html, @"""join_time"":([\s\S]*?),");
                for (int i = 0; i < QQs.Count; i++)
                {
                    textBox3.Text = DateTime.Now.ToString() + "：正在监控......";
                    try
                    {
                        long now = Convert.ToInt64(GetTimeStamp());
                        long join = Convert.ToInt64(join_times[i].Groups[1].Value);
                        // textBox3.Text += now +"---" + join+ "\r\n";
                        if (now - join < 200)
                        {
                            if (!panduan(QQs[i].Groups[1].Value+ qunhao.Trim()))
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(QQs[i].Groups[1].Value);
                                lv1.SubItems.Add(ConvertStringToDateTime(join_times[i].Groups[1].Value).ToString());
                                lv1.SubItems.Add(qunhao);
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        continue;
                    }

                }
                Thread.Sleep(1000);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
            getqunList();
            ChromeOptions options = new ChromeOptions();
            // 禁用图片
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);


            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://i.qq.com/");
        }
        Thread thread;
        Thread thread1;
        IWebDriver driver;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QQqunjiankong"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

          


            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(getqunmember);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Start();
            timer2.Start();
        }

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(getqunmember);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
          


            timer1.Stop();
            textBox3.Text = DateTime.Now.ToString() + "：停止监控......";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            QQadduser();
        }
    }
}
