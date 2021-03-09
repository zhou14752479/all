using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 政府房产交易网
{
    public partial class 房产交易信息 : Form
    {
        public 房产交易信息()
        {
            InitializeComponent();
        }

        fang_method md = new fang_method();

        

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           

            //titletxt.Text += treeView1.SelectedNode.Name;

        }

        private void 房产交易信息_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = md.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"fangchanxinxi"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion

            Control.CheckForIllegalCrossThreadCalls = false;
            md.getlogs += new fang_method.GetLogs(setlog);
            
        }

        public void setlog(string str)
        {
            if (logtxt.Text.Length > 100)
            {
                logtxt.Text = "";

            }
           logtxt.Text += str+Environment.NewLine;
        }

      


        public void lsvstart(string id)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[2].Text == id)
                {
                    listView1.Items[i].SubItems[3].Text = "已启动";
                    return;
                }

            }

            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
            lv1.SubItems.Add(treeView1.SelectedNode.Text);
            lv1.SubItems.Add(treeView1.SelectedNode.Name);
            lv1.SubItems.Add("已启动");
        }


        public void lsvstop(string id)
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[2].Text == id)
                {
                    listView1.Items[i].SubItems[3].Text = "已停止";
                }

            }
        }
        private void 开启任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            switch (treeView1.SelectedNode.Name)
            {
                case "yantai_1":
                    lsvstart("yantai_1");
                    md.yantai_1_start();
                    
                    break;
                case "yantai_2":
                    lsvstart("yantai_2");
                    md.yantai_2_start();
                    break;
                case "jining":
                    lsvstart("jining");
                    md.jining_start();
                    break;
                case "zoucheng":
                    lsvstart("zoucheng");
                    md.zoucheng_start();
                    break;
                case "dljpxqz":
                    lsvstart("dljpxqz");
                    md.dljpxqz_start();
                    break;

                case "dlgxqz":
                    lsvstart("dlgxqz");
                    md.dlgxqz_start();
                    break;
                case "hnzmdxpzrcj":
                    lsvstart("hnzmdxpzrcj");
                    md.hnzmdxpzrcj_start();
                    break;
                case "hnzmdxpysxx":
                    lsvstart("hnzmdxpysxx");
                    md.hnzmdxpysxx_start();
                    break;

            }
        }

        private void 停止任务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (treeView1.SelectedNode.Name)
            {
                case "yantai_1":
                    md.yantai_1_stop();
                    lsvstop("yantai_1");
                    break;
                case "yantai_2":
                    md.yantai_2_stop();
                    lsvstop("yantai_2");         
                    break;
                case "jining":
                    md.jining_stop();
                    lsvstop("jining");
                    break;
                case "zoucheng":
                    md.zoucheng_stop();
                    lsvstop("zoucheng");
                    break;
                case "dljpxqz":
                    md.dljpxqz_stop();
                    lsvstop("dljpxqz");
                    break;

                case "dlgxqz":
                    md.dlgxqz_stop();
                    lsvstop("dlgxqz");
                    break;
                case "hnzmdxpzrcj":
                    lsvstop("hnzmdxpzrcj");
                    md.hnzmdxpzrcj_stop();
                    break;
                case "hnzmdxpysxx":
                    lsvstop("hnzmdxpysxx");
                    md.hnzmdxpysxx_stop();
                    break;

            }
        }

        private void 房产交易信息_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
