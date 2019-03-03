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

namespace fang.临时软件
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        bool status = true;


        #region  58二手房
        public void ershoufang()
        {
            string city = method.Get58pinyin(comboBox1.SelectedItem.ToString());
            try
            {
               

                for (int i = 1; i < 71; i++)
                {
                    String Url = "http://" + city + ".58.com/ershoufang/pn" + i + "/";

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                   
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add("http://" + city + ".58.com/ershoufang/" + NextMatch.Groups[4].Value + "x.shtml");

                        if (this.status == false)
                            return;
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     


                    if (listView1.Items.Count - 1 > 0)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);
                    }
                }

             

            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion


        #region  58二手车
        public void ershouche()
        {
            string city = method.Get58pinyin(comboBox1.SelectedItem.ToString());
            try
            {


                for (int i = 1; i < 71; i++)
                {
                    String Url = "http://" + city + ".58.com/ershouche/pn" + i + "/";

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"data-entid=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add("http://" + city + ".58.com/ershouche/" + NextMatch.Groups[1].Value + "x.shtml");


                        if (this.status == false)
                            return;
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     


                    if (listView1.Items.Count - 1 > 0)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);
                    }
                }



            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region  58二手手机
        public void ershoushouji()
        {
            string city = method.Get58pinyin(comboBox1.SelectedItem.ToString());
            try
            {


                for (int i = 1; i < 71; i++)
                {
                    String Url = "http://" + city + ".58.com/shouji/pn" + i + "/";

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"<tr logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add("http://" + city + ".58.com/shouji/" + NextMatch.Groups[4].Value + "x.shtml");


                        if (this.status == false)
                            return;
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     


                    if (listView1.Items.Count - 1 > 0)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);
                    }
                }



            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region  58二手电脑
        public void ershoudiannao()
        {
            string city = method.Get58pinyin(comboBox1.SelectedItem.ToString());
            try
            {


                for (int i = 1; i < 71; i++)
                {
                    String Url = "http://" + city + ".58.com/diannao/pn" + i + "/";

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"<tr logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add("http://" + city + ".58.com/diannao/" + NextMatch.Groups[4].Value + "x.shtml");


                        if (this.status == false)
                            return;
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     


                    if (listView1.Items.Count - 1 > 0)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);
                    }
                }



            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            method.get58CityName(comboBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.status = true;


            if (comboBox2.Text== "二手房")
            {
                Thread thread = new Thread(new ThreadStart(ershoufang));
                thread.Start();
            }

            else if (comboBox2.Text == "二手车")
            {
                Thread thread = new Thread(new ThreadStart(ershouche));
                thread.Start();
            }
            else if (comboBox2.Text == "二手手机")
            {
                Thread thread = new Thread(new ThreadStart(ershoushouji));
                thread.Start();
            }
            else if (comboBox2.Text == "二手电脑")
            {
                Thread thread = new Thread(new ThreadStart(ershoudiannao));
                thread.Start();
            }

            else 
            {
                MessageBox.Show("请选择分类！");
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
