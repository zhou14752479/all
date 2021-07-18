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

      


        private void 后台管理_Load(object sender, EventArgs e)
        {
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            treeView1.Nodes[0].Expand();
            tabControl1.SelectedIndex = 4;
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
            if (treeView1.SelectedNode.Text == "注册码管理")
            {
                tabControl1.SelectedIndex = 2;
            }
            if (treeView1.SelectedNode.Text == "系统设置")
            {
                tabControl1.SelectedIndex = 3;
            }
            if (treeView1.SelectedNode.Text == "主页")
            {
                tabControl1.SelectedIndex = 4;
            }

        }

        public void getall()
        {
            listView1.Items.Clear();
            string html = method.GetUrl("http://116.62.170.108:8080/api/mt/getall.html", "utf-8");
            MatchCollection ids = Regex.Matches(html, @"""id"":([\s\S]*?),");
            MatchCollection usernames = Regex.Matches(html, @"""username"": ""([\s\S]*?)""");
            MatchCollection passwords = Regex.Matches(html, @"""password"": ""([\s\S]*?)""");
            MatchCollection times = Regex.Matches(html, @"""registertime"": ""([\s\S]*?)""");
            for (int i = 0; i < ids.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(ids[i].Groups[1].Value);
                lv1.SubItems.Add(usernames[i].Groups[1].Value);
                lv1.SubItems.Add(passwords[i].Groups[1].Value);
                lv1.SubItems.Add(times[i].Groups[1].Value);

            }
        }


        public void register()
        {
            if (user_txt.Text=="" || pass_txt.Text == "")
            {
                MessageBox.Show("请输入账号和密码");
                return;
            }
            string html = method.GetUrl("http://116.62.170.108:8080/api/mt/register.html?username="+user_txt.Text.Trim() + "&password="+pass_txt.Text.Trim()+"&code=1&", "utf-8");
            MessageBox.Show(html);
            user_txt.Text = "";
            pass_txt.Text= "";

        }


        public void delete()
        {
            
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string url = "http://116.62.170.108:8080/api/mt/del.html?userid=" + listView1.CheckedItems[i].SubItems[1].Text.Trim() + "&";
                string html = method.GetUrl(url, "utf-8");
              
                MessageBox.Show(html);
            }


            getall();





        }

        public void updatesoftinfo()
        {
          
            string html = method.GetUrl("http://116.62.170.108:8080/api/mt/updatesoftinfo.html?softname="+textBox2.Text.Trim()+ "&tel=" + textBox3.Text.Trim() + "&logo=" + textBox4.Text.Trim() + "&erweima=" + textBox5.Text.Trim() + "&", "utf-8");
            MessageBox.Show(html);
            

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
            tabControl1.SelectedIndex =3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开文件";  
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox4.Text = openFileDialog1.FileName;
              
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开文件";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox5.Text = openFileDialog1.FileName;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(updatesoftinfo);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            else
            {
                MessageBox.Show("存在空值！");
            }
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
    }
}
