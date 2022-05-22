using OpenQA.Selenium;
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
using System.IO;
using System.Diagnostics;

namespace 主程序selenium
{
    public partial class 虎牙种豆 : Form
    {
        public 虎牙种豆()
        {
            InitializeComponent();
        }


        IWebDriver driver;
        private void button1_Click(object sender, EventArgs e)
        {
            driver = function.getdriver(false, false);
            driver.Navigate().GoToUrl("https://i.huya.com/index.php?m=Guess");



        }

        #region 主程序
        public void run()
        {
            //    int total = 0;
            //    double wan = 0;

            for (int j = 1; j <= 2; j++)
            {
                for (int page = 1; page < 9999; page++)
                {

                    string url = "https://i.huya.com/index.php?m=MatchGuess&do=ajaxGetGuessHistory&page=" + page + "&transType=" + j.ToString();

                    string html = method.GetUrlWithCookie(url, selectcookie, "utf-8");
                    html = method.Unicode2String(html);
                   // textBox1.Text = html;
                   if (html.Contains("未登陆"))
                    {
                        MessageBox.Show("登录失效，请重新添加登录账号");
                       
                        listView1.Items.Remove(listView1.SelectedItems[0]);
                        return;
                    }
                    MatchCollection ids = Regex.Matches(html, @"""lTransId"":([\s\S]*?),");
                    MatchCollection lUpTime = Regex.Matches(html, @"""lTransTime"":""([\s\S]*?)""");
                    MatchCollection nick = Regex.Matches(html, @"""nick"":""([\s\S]*?)""");
                    MatchCollection sGambleTitle = Regex.Matches(html, @"""sGambleTitle"":""([\s\S]*?)""");
                    MatchCollection sUnitName = Regex.Matches(html, @"""sUnitName"":""([\s\S]*?)""");
                    MatchCollection iState = Regex.Matches(html, @"""iState"":([\s\S]*?),");
                    MatchCollection lTransNum = Regex.Matches(html, @"""lTransNum"":([\s\S]*?),");
                    MatchCollection lWinNum = Regex.Matches(html, @"""lWinNum"":([\s\S]*?),");

                    string type = "我的种豆";
                    if(j==1)
                    {
                        type = "我开种的";
                    }
                    if (j == 2)
                    {
                        type = "我的种豆";
                    }
                    if (ids.Count == 0)
                    {
                        break;
                    }
                    for (int a = 0; a < ids.Count; a++)
                    {
                        try
                        {
                            if (Convert.ToInt32(lUpTime[a].Groups[1].Value) > Convert.ToInt32(method.GetTimeStamp()) - 172800)
                            {
                                ListViewItem lv2 = listView2.Items.Add(ids[a].Groups[1].Value); //使用Listview展示数据
                                lv2.SubItems.Add(method.ConvertStringToDateTime(lUpTime[a].Groups[1].Value).ToString());
                                lv2.SubItems.Add(method.Unicode2String(nick[a].Groups[1].Value));
                                lv2.SubItems.Add(method.Unicode2String(sGambleTitle[a].Groups[1].Value));
                                lv2.SubItems.Add(method.Unicode2String(sUnitName[a].Groups[1].Value));
                                lv2.SubItems.Add(method.Unicode2String(iState[a].Groups[1].Value));

                                lv2.SubItems.Add(lTransNum[a].Groups[1].Value.Replace("\"", ""));

                                string num = lWinNum[a].Groups[1].Value.Replace("\"", "");
                                if (!lWinNum[a].Groups[1].Value.Replace("\"", "").Contains("-"))
                                {
                                    num = (Convert.ToInt32(lWinNum[a].Groups[1].Value.Replace("\"", "")) - Convert.ToInt32(lTransNum[a].Groups[1].Value.Replace("\"", ""))).ToString();

                                }

                                lv2.SubItems.Add(num);
                                lv2.SubItems.Add(type);

                                //total = total + Math.Abs(Convert.ToInt32(num));
                                //wan = wan + Math.Floor(Math.Abs(Convert.ToDouble(num)) / 10000);

                                //label2.Text = total.ToString();
                                //label3.Text = wan.ToString();
                            }
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }
                    Thread.Sleep(100);
                }

            }
        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            var cookies = driver.Manage().Cookies.AllCookies;

            StringBuilder sb = new StringBuilder();
            foreach (var item in cookies)
            {
                sb.Append(item.Name + "=" + item.Value + ";");

            }

            string cookie = sb.ToString();
            string huyaid = Regex.Match(driver.PageSource, @"<dd id=""user_y""([\s\S]*?)>([\s\S]*?)</dd>").Groups[2].Value;
            string name = Regex.Match(driver.PageSource, @"<a class=""UserHd([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value;
            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
            lv1.SubItems.Add(huyaid);
            lv1.SubItems.Add(name);
            lv1.SubItems.Add(cookie);

        }
        public static void KillProcess(string processName)
        {
            foreach (Process p in Process.GetProcesses())
            {
                bool flag = p.ProcessName.Contains(processName);
                if (flag)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                        Console.WriteLine("已杀掉" + processName + "进程！！！");
                    }
                    catch (Win32Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }
                    catch (InvalidOperationException e2)
                    {
                        Console.WriteLine(e2.Message.ToString());
                    }
                }
            }
        }
        private void 虎牙种豆_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"ZmS45"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion




            KillProcess("chromedriver");
            StreamReader sr = new StreamReader(path, method.EncodingType.GetTxtType(path));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "----" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {

                string[] value = text[i].Split(new string[] { "####" }, StringSplitOptions.None);
                if (value.Length > 2)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(value[0]);
                    lv1.SubItems.Add(value[1]);
                    lv1.SubItems.Add(value[2]);
                }
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }

        public string selectcookie = "";
        Thread thread;

        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Math.Abs(-15).ToString());
            //double a = 58888 / 10000;
            //MessageBox.Show(Math.Floor(a).ToString() );

            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选中一个账号");
                return;
            }

            selectcookie = (listView1.SelectedItems[0].SubItems[3].Text);

            listView2.Items.Clear();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "cookie.txt";
        private void 虎牙种豆_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    sb.Append(listView1.Items[i].SubItems[1].Text + "####" + listView1.Items[i].SubItems[2].Text + "####" + listView1.Items[i].SubItems[3].Text + "----");
                }
                System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            double wan = 0;
            for (int i = 0; i < listView2.SelectedItems.Count; i++)
            {
                string num = listView2.SelectedItems[i].SubItems[7].Text;
                wan = wan + Math.Floor(Math.Abs(Convert.ToDouble(num)) / 10000);


                label2.Text = wan.ToString();

            }


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView2.Items.Clear();
        }
    }
}
