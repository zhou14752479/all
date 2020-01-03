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
using helper;
using MySql.Data.MySqlClient;

namespace 借款应用
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        ArrayList finishes = new ArrayList();
        private void Button4_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1);
        }
        public string cookie = "";
        public void run()
        {

            try
            {
               

                for (int i = 1; i <= Convert.ToInt32(textBox1.Text); i++)
                {
                    string url = "http://hb.fedayingsfew.biz/admin/loan/index.html?page="+i;
                    
                    string html = method.GetUrlWithCookie(url,cookie,"utf-8");
                   
                    MatchCollection matchs = Regex.Matches(html, @"<td align=""center"">([\s\S]*?)</td>");

                    for (int j = 0; j < 25; j++)
                    {
                        finishes.Add(matchs[(11 * j) + 2].Groups[1].Value.Trim());
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(matchs[(11*j)+3].Groups[1].Value.Trim());
                        listViewItem.SubItems.Add(matchs[(11*j)+2].Groups[1].Value.Trim());
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                       
                    Thread.Sleep(1000);
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        public void run1()
        {

            try
            {


                for (int i = 1; i <= Convert.ToInt32(textBox1.Text); i++)
                {
                    string url = "http://hb.fedayingsfew.biz/admin/user/index.html?page=" + i;

                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");

                    MatchCollection matchs = Regex.Matches(html, @"<td align=""center"">([\s\S]*?)</td>");

                    for (int j = 0; j < 25; j++)
                    {
                        if (!finishes.Contains(matchs[(10 * j) + 1].Groups[1].Value.Trim()))
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(matchs[(10 * j) + 2].Groups[1].Value.Trim());
                            listViewItem.SubItems.Add(matchs[(10 * j) + 1].Groups[1].Value.Trim());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }
                    }

                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (cookie == "")
            {
                MessageBox.Show("请先登录");
                return;
            }

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

            Thread thread1 = new Thread(new ThreadStart(run1));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                cookie = method.getheader("http://hb.fedayingsfew.biz/admin/index/login.html", "username=" + textBox2.Text + "&password=" + textBox3.Text);
                if (cookie != "")
                {
                    MessageBox.Show("登陆成功");

                }
            }
           
            
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
