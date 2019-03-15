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
        #region  主函数1
        public void run1()

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
                         string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");
                        

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
                        string prttern11 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
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
                        string p11 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15= @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
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


        #region  主函数2
        public void run2()

        {


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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"错中{2}错{2}中";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"错中{2}错{2}中{2}错";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"错中{2}错{2}中{2}错{2}中";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"错中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }


                        int b = 0;

                        string p2 = @"中错{2}中{2}错";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"中错{2}中{2}错{2}中";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"中错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"中错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
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


        #region  主函数3
        public void run3()

        {


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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"中错中{1,}错中{1,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"中错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"中错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern17 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches17 = Regex.Matches(sb.ToString(), prttern17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern18 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches18 = Regex.Matches(sb.ToString(), prttern18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern19 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches19 = Regex.Matches(sb.ToString(), prttern19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern20 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches20 = Regex.Matches(sb.ToString(), prttern20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern21 = @"中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches21 = Regex.Matches(sb.ToString(), prttern21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }
                        else if (matches16.Count > 0 && matches17.Count == 0)
                        {
                            a = 16;
                        }
                        else if (matches17.Count > 0 && matches18.Count == 0)
                        {
                            a = 17;
                        }
                        else if (matches18.Count > 0 && matches19.Count == 0)
                        {
                            a = 18;
                        }
                        else if (matches19.Count > 0 && matches20.Count == 0)
                        {
                            a = 19;
                        }
                        else if (matches20.Count > 0 && matches21.Count == 0)
                        {
                            a = 20;
                        }
                     


                        int b = 0;

                        string p2 = @"错中错{1,}中错{1,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"错中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"错中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p17 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches17 = Regex.Matches(sb.ToString(), p17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p18 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches18 = Regex.Matches(sb.ToString(), p18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p19 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches19 = Regex.Matches(sb.ToString(), p19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p20 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches20 = Regex.Matches(sb.ToString(), p20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p21 = @"错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches21 = Regex.Matches(sb.ToString(), p21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
                        }
                        else if (amatches16.Count > 0 && amatches17.Count == 0)
                        {
                            b = 16;
                        }
                        else if (amatches17.Count > 0 && amatches18.Count == 0)
                        {
                            b = 17;
                        }
                        else if (amatches18.Count > 0 && amatches19.Count == 0)
                        {
                            b = 18;
                        }
                        else if (amatches19.Count > 0 && amatches20.Count == 0)
                        {
                            b = 19;
                        }
                        else if (amatches20.Count > 0 && amatches21.Count == 0)
                        {
                            b = 20;
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


        #region  主函数4
        public void run4()

        {


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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"中错中{2,}错中{2,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"中错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"中错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern17 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches17 = Regex.Matches(sb.ToString(), prttern17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern18 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches18 = Regex.Matches(sb.ToString(), prttern18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern19 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches19 = Regex.Matches(sb.ToString(), prttern19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern20 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches20 = Regex.Matches(sb.ToString(), prttern20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern21 = @"中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches21 = Regex.Matches(sb.ToString(), prttern21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }
                        else if (matches16.Count > 0 && matches17.Count == 0)
                        {
                            a = 16;
                        }
                        else if (matches17.Count > 0 && matches18.Count == 0)
                        {
                            a = 17;
                        }
                        else if (matches18.Count > 0 && matches19.Count == 0)
                        {
                            a = 18;
                        }
                        else if (matches19.Count > 0 && matches20.Count == 0)
                        {
                            a = 19;
                        }
                        else if (matches20.Count > 0 && matches21.Count == 0)
                        {
                            a = 20;
                        }



                        int b = 0;

                        string p2 = @"错中错{2,}中错{2,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"错中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"错中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p17 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches17 = Regex.Matches(sb.ToString(), p17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p18 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches18 = Regex.Matches(sb.ToString(), p18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p19 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches19 = Regex.Matches(sb.ToString(), p19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p20 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches20 = Regex.Matches(sb.ToString(), p20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p21 = @"错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches21 = Regex.Matches(sb.ToString(), p21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
                        }
                        else if (amatches16.Count > 0 && amatches17.Count == 0)
                        {
                            b = 16;
                        }
                        else if (amatches17.Count > 0 && amatches18.Count == 0)
                        {
                            b = 17;
                        }
                        else if (amatches18.Count > 0 && amatches19.Count == 0)
                        {
                            b = 18;
                        }
                        else if (amatches19.Count > 0 && amatches20.Count == 0)
                        {
                            b = 19;
                        }
                        else if (amatches20.Count > 0 && amatches21.Count == 0)
                        {
                            b = 20;
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


        #region  主函数5
        public void run5()

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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"^中{2,}错{2,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"^中{2,}错{2,}中{2,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"^中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"^中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }


                        int b = 0;

                        string p2 = @"^错{2,}中{2,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"^错{2,}中{2,}错{2,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"^错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"^错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}错{2,}中{2,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
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

        #region  主函数6
        public void run6()

        {


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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"^错中{2}错{2}中";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"^错中{2}错{2}中{2}错";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"^错中{2}错{2}中{2}错{2}中";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"^错中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"^错中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }


                        int b = 0;

                        string p2 = @"^中错{2}中{2}错";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"^中错{2}中{2}错{2}中";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"^中错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"^中错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"^中错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错{2}中{2}错";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
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

        #region  主函数7
        public void run7()

        {


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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"^中错中{1,}错中{1,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"^中错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern17 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches17 = Regex.Matches(sb.ToString(), prttern17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern18 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches18 = Regex.Matches(sb.ToString(), prttern18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern19 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches19 = Regex.Matches(sb.ToString(), prttern19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern20 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches20 = Regex.Matches(sb.ToString(), prttern20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern21 = @"^中错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}错中{1,}";
                        MatchCollection matches21 = Regex.Matches(sb.ToString(), prttern21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }
                        else if (matches16.Count > 0 && matches17.Count == 0)
                        {
                            a = 16;
                        }
                        else if (matches17.Count > 0 && matches18.Count == 0)
                        {
                            a = 17;
                        }
                        else if (matches18.Count > 0 && matches19.Count == 0)
                        {
                            a = 18;
                        }
                        else if (matches19.Count > 0 && matches20.Count == 0)
                        {
                            a = 19;
                        }
                        else if (matches20.Count > 0 && matches21.Count == 0)
                        {
                            a = 20;
                        }



                        int b = 0;

                        string p2 = @"^错中错{1,}中错{1,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"^错中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p17 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches17 = Regex.Matches(sb.ToString(), p17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p18 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches18 = Regex.Matches(sb.ToString(), p18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p19 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches19 = Regex.Matches(sb.ToString(), p19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p20 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches20 = Regex.Matches(sb.ToString(), p20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p21 = @"^错中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}中错{1,}";
                        MatchCollection amatches21 = Regex.Matches(sb.ToString(), p21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
                        }
                        else if (amatches16.Count > 0 && amatches17.Count == 0)
                        {
                            b = 16;
                        }
                        else if (amatches17.Count > 0 && amatches18.Count == 0)
                        {
                            b = 17;
                        }
                        else if (amatches18.Count > 0 && amatches19.Count == 0)
                        {
                            b = 18;
                        }
                        else if (amatches19.Count > 0 && amatches20.Count == 0)
                        {
                            b = 19;
                        }
                        else if (amatches20.Count > 0 && amatches21.Count == 0)
                        {
                            b = 20;
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

        #region  主函数8
        public void run8()

        {


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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"^中错中{2,}错中{2,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"^中错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern17 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches17 = Regex.Matches(sb.ToString(), prttern17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern18 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches18 = Regex.Matches(sb.ToString(), prttern18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern19 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches19 = Regex.Matches(sb.ToString(), prttern19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern20 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches20 = Regex.Matches(sb.ToString(), prttern20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern21 = @"^中错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}错中{2,}";
                        MatchCollection matches21 = Regex.Matches(sb.ToString(), prttern21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }
                        else if (matches16.Count > 0 && matches17.Count == 0)
                        {
                            a = 16;
                        }
                        else if (matches17.Count > 0 && matches18.Count == 0)
                        {
                            a = 17;
                        }
                        else if (matches18.Count > 0 && matches19.Count == 0)
                        {
                            a = 18;
                        }
                        else if (matches19.Count > 0 && matches20.Count == 0)
                        {
                            a = 19;
                        }
                        else if (matches20.Count > 0 && matches21.Count == 0)
                        {
                            a = 20;
                        }



                        int b = 0;

                        string p2 = @"^错中错{2,}中错{2,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"^错中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p17 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches17 = Regex.Matches(sb.ToString(), p17, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p18 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches18 = Regex.Matches(sb.ToString(), p18, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p19 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches19 = Regex.Matches(sb.ToString(), p19, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p20 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches20 = Regex.Matches(sb.ToString(), p20, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p21 = @"^错中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}中错{2,}";
                        MatchCollection amatches21 = Regex.Matches(sb.ToString(), p21, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
                        }
                        else if (amatches16.Count > 0 && amatches17.Count == 0)
                        {
                            b = 16;
                        }
                        else if (amatches17.Count > 0 && amatches18.Count == 0)
                        {
                            b = 17;
                        }
                        else if (amatches18.Count > 0 && amatches19.Count == 0)
                        {
                            b = 18;
                        }
                        else if (amatches19.Count > 0 && amatches20.Count == 0)
                        {
                            b = 19;
                        }
                        else if (amatches20.Count > 0 && amatches21.Count == 0)
                        {
                            b = 20;
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

        #region  主函数9
        public void run9()

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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"中{3,}错{3,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"中{3,}错{3,}中{3,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }


                        int b = 0;

                        string p2 = @"错{3,}中{3,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"错{3,}中{3,}错{3,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
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

        #region  主函数10
        public void run10()

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
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=100&lotid=pk10&eid=" + url, "utf-8");


                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        int a = 0;

                        string prttern2 = @"^中{3,}错{3,}";
                        MatchCollection matches2 = Regex.Matches(sb.ToString(), prttern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern3 = @"^中{3,}错{3,}中{3,}";
                        MatchCollection matches3 = Regex.Matches(sb.ToString(), prttern3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern4 = @"^中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches4 = Regex.Matches(sb.ToString(), prttern4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern5 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches5 = Regex.Matches(sb.ToString(), prttern5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern6 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches6 = Regex.Matches(sb.ToString(), prttern6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern7 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches7 = Regex.Matches(sb.ToString(), prttern7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern8 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches8 = Regex.Matches(sb.ToString(), prttern8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern9 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches9 = Regex.Matches(sb.ToString(), prttern9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern10 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches10 = Regex.Matches(sb.ToString(), prttern10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern11 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches11 = Regex.Matches(sb.ToString(), prttern11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern12 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches12 = Regex.Matches(sb.ToString(), prttern12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern13 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches13 = Regex.Matches(sb.ToString(), prttern13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern14 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches14 = Regex.Matches(sb.ToString(), prttern14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern15 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection matches15 = Regex.Matches(sb.ToString(), prttern15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string prttern16 = @"^中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection matches16 = Regex.Matches(sb.ToString(), prttern16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

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
                        else if (matches10.Count > 0 && matches11.Count == 0)
                        {
                            a = 10;
                        }
                        else if (matches11.Count > 0 && matches12.Count == 0)
                        {
                            a = 11;
                        }
                        else if (matches12.Count > 0 && matches13.Count == 0)
                        {
                            a = 12;
                        }
                        else if (matches13.Count > 0 && matches14.Count == 0)
                        {
                            a = 13;
                        }
                        else if (matches14.Count > 0 && matches15.Count == 0)
                        {
                            a = 14;
                        }
                        else if (matches15.Count > 0 && matches16.Count == 0)
                        {
                            a = 15;
                        }


                        int b = 0;

                        string p2 = @"^错{3,}中{3,}";
                        MatchCollection amatches2 = Regex.Matches(sb.ToString(), p2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p3 = @"^错{3,}中{3,}错{3,}";
                        MatchCollection amatches3 = Regex.Matches(sb.ToString(), p3, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p4 = @"^错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches4 = Regex.Matches(sb.ToString(), p4, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p5 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches5 = Regex.Matches(sb.ToString(), p5, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p6 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches6 = Regex.Matches(sb.ToString(), p6, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p7 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches7 = Regex.Matches(sb.ToString(), p7, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p8 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches8 = Regex.Matches(sb.ToString(), p8, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p9 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches9 = Regex.Matches(sb.ToString(), p9, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p10 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches10 = Regex.Matches(sb.ToString(), p10, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p11 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches11 = Regex.Matches(sb.ToString(), p11, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p12 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches12 = Regex.Matches(sb.ToString(), p12, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p13 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches13 = Regex.Matches(sb.ToString(), p13, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p14 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches14 = Regex.Matches(sb.ToString(), p14, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p15 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}";
                        MatchCollection amatches15 = Regex.Matches(sb.ToString(), p15, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string p16 = @"^错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}错{3,}中{3,}";
                        MatchCollection amatches16 = Regex.Matches(sb.ToString(), p16, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (amatches2.Count > 0 && amatches3.Count == 0)
                        {
                            b = 2;
                        }
                        else if (amatches3.Count > 0 && amatches4.Count == 0)
                        {
                            b = 3;
                        }
                        else if (amatches4.Count > 0 && amatches5.Count == 0)
                        {
                            b = 4;
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
                            b = 7;
                        }
                        else if (amatches8.Count > 0 && amatches9.Count == 0)
                        {
                            b = 8;
                        }
                        else if (amatches9.Count > 0 && amatches10.Count == 0)
                        {
                            b = 9;
                        }
                        else if (amatches10.Count > 0 && amatches11.Count == 0)
                        {
                            b = 10;
                        }
                        else if (amatches11.Count > 0 && amatches12.Count == 0)
                        {
                            b = 11;
                        }
                        else if (amatches12.Count > 0 && amatches13.Count == 0)
                        {
                            b = 12;
                        }
                        else if (amatches13.Count > 0 && amatches14.Count == 0)
                        {
                            b = 13;
                        }
                        else if (amatches14.Count > 0 && amatches15.Count == 0)
                        {
                            b = 14;
                        }
                        else if (amatches15.Count > 0 && amatches16.Count == 0)
                        {
                            b = 15;
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
                if (checkBox1.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run1));
                    thread.Start();
                    
                }

                else if (checkBox2.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run2));
                    thread.Start();
                   
                }

                else if (checkBox3.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run3));
                    thread.Start();
                    
                }

                else if (checkBox4.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run4));
                    thread.Start();
                    
                }

                else if (checkBox5.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run5));
                    thread.Start();
                   
                }
                else if (checkBox6.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run6));
                    thread.Start();
                   
                }
                else if (checkBox7.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run7));
                    thread.Start();
                    
                }
                else if (checkBox8.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run8));
                    thread.Start();
                    
                }
                else if (checkBox9.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run9));
                    thread.Start();

                }
                else if (checkBox10.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run10));
                    thread.Start();

                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run1));
                thread.Start();
                timer1.Start();
            }

            else if (checkBox2.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run2));
                thread.Start();
                timer1.Start();
            }

            else if (checkBox3.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run3));
                thread.Start();
                timer1.Start();
            }

            else if (checkBox4.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run4));
                thread.Start();
                timer1.Start();
            }

            else if (checkBox5.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run5));
                thread.Start();
                timer1.Start();
            }
            else if (checkBox6.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run6));
                thread.Start();
                timer1.Start();
            }
            else if (checkBox7.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run7));
                thread.Start();
                timer1.Start();
            }
            else if (checkBox8.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run8));
                thread.Start();
                timer1.Start();
            }

            else if (checkBox9.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run9));
                thread.Start();
                timer1.Start();
            }

            else if (checkBox10.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run10));
                thread.Start();
                timer1.Start();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        public class ListViewSort : IComparer
        {
            private int col;
            private bool descK;

            public ListViewSort()
            {
                col = 0;
            }
            public ListViewSort(int column, object Desc)
            {
                descK = (bool)Desc;
                col = column; //当前列,0,1,2...,参数由ListView控件的ColumnClick事件传递
            }
            public int Compare(object x, object y)
            {
                int tempInt = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                if (descK)
                {
                    return -tempInt;
                }
                else
                {
                    return tempInt;
                }
            }

        }

        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView2.Columns[e.Column].Tag == null)
            {
                listView2.Columns[e.Column].Tag = true;
            }
            bool tabK = (bool)listView2.Columns[e.Column].Tag;
            if (tabK)
            {
                listView2.Columns[e.Column].Tag = false;
            }
            else
            {
                listView2.Columns[e.Column].Tag = true;
            }
            listView2.ListViewItemSorter = new ListViewSort(e.Column, listView2.Columns[e.Column].Tag);
            //指定排序器并传送列索引与升序降序关键字
            listView2.Sort();//对列表进行自定义排序
        }


    }
}
