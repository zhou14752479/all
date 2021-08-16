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

namespace 地图营销
{
    public partial class 后台管理 : Form
    {
        public 后台管理()
        {
            InitializeComponent();
        }

        string type = "shangxueba";


        private void 后台管理_Load(object sender, EventArgs e)
        {
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            treeView1.Nodes[0].Expand();
            tabControl1.SelectedIndex = 3;
            //webBrowser1.ScriptErrorsSuppressed = true;
            if (type == "map")
            {
                linkLabel3.Visible = false;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Text == "用户列表")
            {
                tabControl1.SelectedIndex = 0;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(getall);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (treeView1.SelectedNode.Text == "用户添加")
            {
                tabControl1.SelectedIndex = 1;
            }
          
            if (treeView1.SelectedNode.Text == "主页")
            {
                tabControl1.SelectedIndex = 3;
            }

        }


       

        public void getall()
        {
            listView1.Items.Clear();
            string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=getall", "utf-8");
           
            MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
            MatchCollection usernames = Regex.Matches(html, @"""username"":""([\s\S]*?)""");
            MatchCollection passwords = Regex.Matches(html, @"""password"":""([\s\S]*?)""");
            MatchCollection times = Regex.Matches(html, @"""viptime"":""([\s\S]*?)""");
            MatchCollection types = Regex.Matches(html, @"""type"":""([\s\S]*?)""");
            for (int i = 0; i < ids.Count; i++)
            {
                if (types[i].Groups[1].Value == type)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                                                                    //lv1.SubItems.Add(ids[i].Groups[1].Value);
                    lv1.SubItems.Add(usernames[i].Groups[1].Value);
                    lv1.SubItems.Add(passwords[i].Groups[1].Value);
                    lv1.SubItems.Add(times[i].Groups[1].Value);
                }

            }
        }


        public void register()
        {
            if (user_txt.Text=="" || pass_txt.Text == "")
            {
                MessageBox.Show("请输入账号和密码");
                return;
            }
            string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=register&username="+user_txt.Text.Trim()+"&password="+pass_txt.Text.Trim()+"&days="+numericUpDown1.Value+ "&type=" +type, "utf-8");
            MessageBox.Show(html.Trim());
            user_txt.Text = "";
            pass_txt.Text= "";

        }


        public void delete()
        {
            
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string url = "http://www.acaiji.com/shangxueba/shangxueba.php?method=del&username="+ listView1.CheckedItems[i].SubItems[1].Text.Trim();
                string html = method.GetUrl(url, "utf-8");
              
                MessageBox.Show(html.Trim());
            }


            getall();





        }

       

        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getall);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(register);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex =1;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            webBrowser1.Navigate("https://passport.shangxueba.com/user/userlogin.aspx?url=https%3A//www.shangxueba.com/ask/32327.html");
            tabControl1.SelectedIndex =2;
        }

       

       

       

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(delete);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        
              
              
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cookie = method.GetCookies("https://www.shangxueba.com/ask/ajax/zuijiainfo.aspx?id=32327&t=1629017810540");
            textBox1.Text = cookie;
          
            if (cookie != "")
            {
                string url = "http://www.acaiji.com/shangxueba/shangxueba.php?method=setcookie";
                string postdata = "cookie="+ System.Web.HttpUtility.UrlEncode(cookie);
                string msg = method.PostUrl(url,postdata,"","utf-8", "application/x-www-form-urlencoded","");
                MessageBox.Show(msg.Trim());
            }
            else
            {
                MessageBox.Show("获取cookie失败，请检查网络");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://passport.shangxueba.com/user/userlogin.aspx?url=https%3A//www.shangxueba.com/ask/32327.html");
        }
    }
}
