using System;
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
using System.Web;
using System.Windows.Forms;
using helper;


namespace 商标生成
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool zanting = true;
        

        #region 注册人
        public void run()
        {
            try

            {
             
                    textBox4.Text+=  "正在抓取" +textBox3.Text + "\r\n";
                    string url = "http://v.juhe.cn/trademark/marklist?applicantCn="+ HttpUtility.UrlEncode(textBox3.Text.Trim())+"&idCardNo=&key="+textBox1.Text;

                   
                    string html = method.GetUrl(url, "utf-8");

                if (html.Contains("不足"))
                {
                    MessageBox.Show("当前APPKEY剩余次数不足");
                    return;
                }
                   

                    MatchCollection a1 = Regex.Matches(html, @"""appDate"":""([\s\S]*?)""");
                    MatchCollection a2 = Regex.Matches(html, @"""applicantCn"":""([\s\S]*?)""");
                    MatchCollection a3 = Regex.Matches(html, @"""currentStatus"":""([\s\S]*?)""");
                    MatchCollection a4 = Regex.Matches(html, @"""intCls"":""([\s\S]*?)""");
                    MatchCollection a5 = Regex.Matches(html, @"""regNo"":""([\s\S]*?)""");
                    MatchCollection a6 = Regex.Matches(html, @"""tmName"":""([\s\S]*?)""");

                
                for (int i = 0; i < a1.Count; i++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(a1[i].Groups[1].Value);
                        lv1.SubItems.Add(a2[i].Groups[1].Value);
                        lv1.SubItems.Add(a3[i].Groups[1].Value);
                        lv1.SubItems.Add(a4[i].Groups[1].Value);
                        lv1.SubItems.Add(a5[i].Groups[1].Value);
                        lv1.SubItems.Add(a6[i].Groups[1].Value);

                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(500);

                }


            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 注册号
        public void run1()
        {
            try

            {

                string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    string[] regnos = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                    string url = "http://v.juhe.cn/trademark/detail?regNo=" + regnos[0] + "&intCls=" + regnos[1] + "&key=" + textBox1.Text;
                    string html = method.GetUrl(url, "utf-8");


                    Match a1 = Regex.Match(html, @"""regNo"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(html, @"""tmName"":""([\s\S]*?)""");
                    Match a3 = Regex.Match(html, @"""intCls"":""([\s\S]*?)""");
                    Match a4 = Regex.Match(html, @"""applicantCn"":""([\s\S]*?)""");
                    Match a5 = Regex.Match(html, @"""addressCn"":""([\s\S]*?)""");
                    Match a6 = Regex.Match(html, @"""appDate"":""([\s\S]*?)""");
                    Match a7 = Regex.Match(html, @"""agent"":""([\s\S]*?)""");

                    Match img = Regex.Match(html, @"""tmImg"":""([\s\S]*?)""");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                    lv1.SubItems.Add(a1.Groups[1].Value);
                    lv1.SubItems.Add(a2.Groups[1].Value);
                    lv1.SubItems.Add(a3.Groups[1].Value);
                    lv1.SubItems.Add(a4.Groups[1].Value);
                    lv1.SubItems.Add(a5.Groups[1].Value);
                    lv1.SubItems.Add(a6.Groups[1].Value);
                    lv1.SubItems.Add(a7.Groups[1].Value);



                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(500);

                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("请输入APPKEY和注册人");
                return;
            }
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run1));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            textBox2.Text += listView1.SelectedItems[0].SubItems[5].Text + listView1.SelectedItems[0].SubItems[4].Text + "\r\n";
        }
    }
}
