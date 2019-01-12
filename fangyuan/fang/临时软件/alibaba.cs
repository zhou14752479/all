using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang.临时软件
{
    public partial class alibaba : Form
    {
        bool status = true;

        public alibaba()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public string[] ReadText()
        {
            string currentDirectory = Environment.CurrentDirectory;
            StreamReader streamReader = new StreamReader(this.textBox1.Text);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[]
            {
        "\r\n"
            }, StringSplitOptions.None);
        }
     

        public void run()
        {
            try
            {
                string[] array = this.ReadText();
                foreach (string text in array)
                {
                    string url = "http://m-quan.cn/couponsys/console/web/convertible?exchangeNo=" + text + "&activityId=94";
                    string url2 = method.GetUrl(url, "utf-8");
                    Match match = Regex.Match(url2, @"q_ma""([\s\S]*?)f=""([\s\S]*?)""");
                    ListViewItem listViewItem = this.listView1.Items.Add(text);
                    listViewItem.SubItems.Add(match.Groups[2].Value.Trim());
                    this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                    Application.DoEvents();
                    Thread.Sleep(Convert.ToInt32(1000));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region  主程序

        ArrayList finishes = new ArrayList();

        public void Run()
        {

            
            try
            {

                int page = 100;

                string keywords = System.Web.HttpUtility.UrlEncode(textBox1.Text.Trim(), System.Text.Encoding.GetEncoding("GB2312"));

                for (int j = 0; j < 100; j = j++)
                {
                    string price2 = (Convert.ToSingle(textBox2.Text)+0.1).ToString();

                    for (int i = 1; i < page; i++)
                {

                        String Url = "https://s.1688.com/selloffer/offer_search.htm?keywords=" + keywords + "&n=y&priceStart=" + textBox2.Text + "&priceEnd=" + price2 + "&netType=1%2C11&beginPage=" + i + "&offset=8";

                        textBox4.Text += Url + "\r\n";

                        string html = method.GetUrlWithCookie(Url, "GBK", webBrowser.cookie);


                        MatchCollection TitleMatchs = Regex.Matches(html, @"trace-obj_value=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://detail.1688.com/offer/" + NextMatch.Groups[1].Value + ".html");

                        }


                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        foreach (string list in lists)
                        {
                            if (!finishes.Contains(list))
                            {
                                finishes.Add(list);

                                String Url1 = list;
                                string strhtml = method.GetUrl(Url1, "GBK");  //定义的GetRul方法 返回 reader.ReadToEnd()

                                string Rxg = @"content=""name=([\s\S]*?);";
                                string Rxg2 = @"class=""membername"" target=""_blank"">([\s\S]*?)    ";
                                string Rxg1 = @"title"" content=""([\s\S]*?)""";
                                string Rxg3 = @"地址：([\s\S]*?)</span>";
                                string Rxg4 = @"biz-type-model"">([\s\S]*?)</span>";
                                string Rxg5 = @"所在地区：</label>([\s\S]*?)</span>";
                                string Rxg6 = @"description"" content=""([\s\S]*?)""";

                                Match name = Regex.Match(strhtml, Rxg);
                                Match title = Regex.Match(strhtml, Rxg1);
                                Match lxr = Regex.Match(strhtml, Rxg2);
                                Match addr = Regex.Match(strhtml, Rxg3);
                                Match zhuying = Regex.Match(strhtml, Rxg4);
                                Match area = Regex.Match(strhtml, Rxg5);
                                Match jieshao = Regex.Match(strhtml, Rxg6);


                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(title.Groups[1].Value);
                                lv1.SubItems.Add(list);
                                lv1.SubItems.Add(name.Groups[1].Value);
                                lv1.SubItems.Add(addr.Groups[1].Value.ToString().Trim());
                                lv1.SubItems.Add(zhuying.Groups[1].Value);
                                lv1.SubItems.Add(Regex.Replace(area.Groups[1].Value.Trim(), "<[^>]*>", ""));
                                lv1.SubItems.Add(jieshao.Groups[1].Value);

                                if (listView1.Items.Count > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                                }
                                // string temp = Regex.Replace(area.Groups[1].Value, "<[^>]*>", "");


                                while (this.status == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                            }
                        }

                    }
                }

                }
            


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion



        //private void button1_Click(object sender, EventArgs e)
        //{
        //    bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
        //    if (flag)
        //    {
        //        this.textBox1.Text = this.openFileDialog1.FileName;
        //    }
        //}


        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            //for (int i = 0; i < 50; i++)
            //{
            //    Thread thread = new Thread(new ThreadStart(Run));
            //    thread.Start();
            //}

           Thread thread = new Thread(new ThreadStart(Run));
           thread.Start();

            Thread thread1 = new Thread(new ThreadStart(Run));
            thread1.Start();

            Thread thread2 = new Thread(new ThreadStart(Run));
            thread2.Start();

            Thread thread3 = new Thread(new ThreadStart(Run));
            thread3.Start();

            Thread thread4 = new Thread(new ThreadStart(Run));
            thread4.Start();

            Thread thread5 = new Thread(new ThreadStart(Run));
            thread5.Start();

            Thread thread6 = new Thread(new ThreadStart(Run));
            thread6.Start();

            Thread thread7 = new Thread(new ThreadStart(Run));
            thread7.Start();

            Thread thread8 = new Thread(new ThreadStart(Run));
            thread8.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void alibaba_Load(object sender, EventArgs e)
        {

        }

        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {

            i = i + 1;
            float a = (float)listView1.Items.Count/i+3;
             label6.Text = a.ToString() +"条/秒";



        }

        private void button6_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("https://login.1688.com/member/signin.htm");
            web.Show();
        }
    }
}
