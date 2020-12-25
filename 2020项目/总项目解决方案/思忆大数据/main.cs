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

namespace 思忆大数据
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }
        public void welcome()
        {
            label1.Text = "欢迎...";
            Thread.Sleep(100);
            label1.Text = "欢迎使用...";
            Thread.Sleep(100);
            label1.Text = "欢迎使用思忆...";
            Thread.Sleep(100);
            label1.Text = "欢迎使用思忆大数据...";
            Thread.Sleep(100);
            label1.Text = "欢迎使用思忆大数据采集...";
            Thread.Sleep(100);
            label1.Text = "欢迎使用思忆大数据采集软件......";
            Thread.Sleep(100);
            label1.Text = "欢迎使用思忆大数据采集软件";
            Thread.Sleep(100);
          
        }
        private void main_Load(object sender, EventArgs e)
        {
          
            usernamelabel.Text ="欢迎用户："+ method.username;
            expiretimelabel.Text = md.getone(method.username);
          Thread thread = new Thread(welcome);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            ProvinceCity.ProvinceCity.BindProvince(comboBox5);
           
        }

        private void registerbtn_Click(object sender, EventArgs e)
        {
            login lg = new login();
            lg.Show();
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
            login lg = new login();
            lg.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void 删除此项ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = listView3.SelectedItems.Count - 1; i >= 0; i--)
            {
                ListViewItem item = listView3.SelectedItems[i];
                listView3.Items.Remove(item);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            md.citys.Clear();
            string[] text = textBox3.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv3 = listView3.Items.Add(text[i].Trim());
                md.citys.Add(text[i].Trim());
            }
            panel2.Visible = false;
        }


        method md = new method();
        Thread dituthread;
        private void button6_Click(object sender, EventArgs e)
        {
            if (dituthread == null || !dituthread.IsAlive)
            {
                md.status = true;
                md.keywords = md.getkeywords(listView4);
          

                dituthread = new Thread(new ParameterizedThreadStart(md.ditu));
                ListView o = listView1;
                dituthread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox5, comboBox6);
            for (int i = 0; i < comboBox6.Items.Count; i++)
            {
                textBox3.Text += comboBox6.Items[i] + "\r\n";
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text += comboBox6.Text + "\r\n";
        }

        private void addKey_Click(object sender, EventArgs e)
        {
            string key = key_tbox.Text.Replace(" ", "");
            if (key != "")
            {
                for (int i = 0; i < listView4.Items.Count; i++)
                {
                    if (listView4.Items[i].SubItems[0].Text.Contains(key))
                    {
                        MessageBox.Show(key + "：重复输入");
                        return;
                    }

                }
                listView4.Items.Add(key);
            }
            else
            {
                MessageBox.Show("输入为空");
            }
            key_tbox.Text = "";
        }

        private void 删除此项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = listView4.SelectedItems.Count - 1; i >= 0; i--)
            {
                ListViewItem item = listView4.SelectedItems[i];
                listView4.Items.Remove(item);
            }
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 清空所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView4.Items.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.Text)
            {
                case "全部采集":
                    md.telpanduan = "全部采集";
                    break;
                case "只采集有联系方式":
                    md.telpanduan = "只采集有联系方式";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (md.zanting == false)
            {

                md.zanting = true;
            }
            else
            {
                md.zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            md.status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
           method.DataTableToExcel(method.listViewToDataTable(listView1), "Sheet1", true);
        }

        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void 官方网站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }

       

        private void 购买软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com/index/index/buy");
           
        }

        private void 售后问题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            string nowtime = md.GetTimeStamp();
            int shengyu = Convert.ToInt32(md.expiretime) - Convert.ToInt32(nowtime);
            shengyulabel.Text = (shengyu / 86400) + "天" + ((shengyu % 86400) / 3600) + "小时" + ((shengyu % 86400) % 3600)/60+"分钟" + ((shengyu % 86400) % 3600) % 60 + "秒";
            if (Convert.ToInt32(nowtime) > Convert.ToInt32(md.expiretime))
            {
                timer1.Stop();
               
                MessageBox.Show("账号已过期");
               
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }
           
        }

        private void 视频教程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com/index/index/help");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("请扫码支付购买");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("当前已是最新版");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
