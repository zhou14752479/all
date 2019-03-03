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
    public partial class 连续出现 : Form
    {
        public 连续出现()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);


                }
            }
        }

        /// <summary>
        /// 获取第二列
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue1(ListView listview)

        {
            ArrayList values = new ArrayList();

            for (int i = 0; i < listview.Items.Count; i++)
            {
                ListViewItem item = listview.Items[i];

                values.Add(item.SubItems[1].Text);


            }

            return values;

        }

        ArrayList finishes = new ArrayList();
        #region  主函数
        public void run()

        {
            //StringBuilder sbz = new StringBuilder();
            //for (int i = 0; i <Convert.ToInt32(textBox1.Text); i++)
            //{
            //    sbz.Append("中");
            //}

            //StringBuilder sbc = new StringBuilder();
            //for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
            //{
            //    sbc.Append("错");
            //}

          
            ArrayList lists = getListviewValue1(listView1);

            try

            {

                foreach (string list in lists)
                {
                    if (!finishes.Contains(list))
                    {
                        
                        finishes.Add(list);
                        string url = list.Replace("https://pk10.17500.cn/exp/index/eid/", "").Replace(".html", ""); ;

                        if (url == "")
                            return;
                         string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=30&lotid=pk10&eid=" + url, "utf-8");
                        

                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        
                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;
                       
                        string prttern2 = @"中{2,}错{2,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3= @"中{2,}错{2,}中{2,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (matches2.Count > 0 && matches3.Count == 0)
                        {
                            a = 2;
                        }
                        else if (matches3.Count > 0 && matches4.Count == 0)
                        {
                            a = 3;
                        }
                        else if (matches4.Count > 0 && matches5.Count == 0)
                        {
                            a = 4;
                        }
                        else if (matches5.Count > 0 && matches6.Count == 0)
                        {
                            a = 5;
                        }
                        else if (matches6.Count > 0 && matches7.Count == 0)
                        {
                            a = 6;
                        }
                        else if (matches7.Count > 0 && matches8.Count == 0)
                        {
                            a = 7;
                        }
                        else if (matches8.Count > 0 && matches9.Count == 0)
                        {
                            a = 8;
                        }
                        else if (matches9.Count > 0 && matches10.Count == 0)
                        {
                            a = 9;
                        }



                        int b = 0;

                        string p2 = @"错{2,}中{2,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"错{2,}中{2,}错{2,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b= 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b= 4;
                        }
                        else if (amatches5.Count > 0 && amatches6.Count == 0)
                        {
                            b = 5;
                        }
                        else if (amatches6.Count > 0 && amatches7.Count == 0)
                        {
                            b = 6;
                        }
                        else if (amatches7.Count > 0 && amatches8.Count == 0)
                        {
                            b= 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b= 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }

                        int c = a > b ? a : b;
                       

                        if (c >= Convert.ToInt32(textBox1.Text))
                        {
                            ListViewItem lv1 = listView2.Items.Add(list); //使用Listview展示数据
                            lv1.SubItems.Add(c.ToString());
                        }


                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void 连续出现_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            timer1.Start();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
