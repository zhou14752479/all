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
            banbenlabel.Text = "会员版本";
        }
        private void main_Load(object sender, EventArgs e)
        {
          Thread thread = new Thread(welcome);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

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
            for (int i = listView2.SelectedItems.Count - 1; i >= 0; i--)
            {
                ListViewItem item = listView2.SelectedItems[i];
                listView2.Items.Remove(item);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
          
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv3 = listView3.Items.Add(text[i].Trim());

            }
            panel2.Visible = false;
        }


        method md = new method();
        Thread dituthread;
        private void button6_Click(object sender, EventArgs e)
        {
            if (dituthread == null || !dituthread.IsAlive)
            {
                dituthread = new Thread(md.ditu);
                dituthread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
