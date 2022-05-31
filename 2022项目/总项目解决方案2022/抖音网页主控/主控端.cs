
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
using System.Windows.Forms;

namespace 抖音网页主控
{
    public partial class 主控端 : Form
    {
        public 主控端()
        {
            InitializeComponent();
        }
      
        private void 主控端_Load(object sender, EventArgs e)
        {
          
            updateTxt = new UpdateTxt(UpdateTxtMethod);
        }


        public delegate void UpdateTxt(string cookie, string ip, string user,string pass,string accountName);
        //定义一个委托变量
        public UpdateTxt updateTxt;

        public void UpdateTxtMethod(string cookie,string ip, string user, string pass, string accountName)
        {
            
           
           浏览器端 web = new 浏览器端(cookie, ip, user, pass,accountName);
            if(checkBox1.Checked==true)
            {
                web.Show();
            }
            
        }

        public void addweb()
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    {
                        listView1.CheckedItems[i].SubItems[5].Text="正在进入链接...";

                        string uid = listView1.CheckedItems[i].SubItems[1].Text;
                        string cookie = listView1.CheckedItems[i].SubItems[4].Text;
                        string ip = listView1.CheckedItems[i].SubItems[6].Text;
                        string user = listView1.CheckedItems[i].SubItems[7].Text;
                        string pass = listView1.CheckedItems[i].SubItems[8].Text;
                        
                        this.BeginInvoke(updateTxt, cookie, ip, user, pass,uid);
                        Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000);
                        listView1.CheckedItems[i].SubItems[5].Text = "进入链接成功";
                    }
                });

              
               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string filePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();
            string[] text = html.Split(new string[] { "sid_guard=" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Trim() != "")
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("sid_guard="+text[i].Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.Checked = true;
                }
            }

        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

       
        public void login()
        {
            try
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {
                    string cookie = listView1.CheckedItems[i].SubItems[4].Text;
                   string html= function.getnickname(cookie);
                    string[] text = html.Split(new string[] { "," }, StringSplitOptions.None);
                    if(text.Length>1)
                    {
                        listView1.CheckedItems[i].SubItems[1].Text = text[0];
                        listView1.CheckedItems[i].SubItems[2].Text =text[1];
                        if(text[1]!="掉线")
                        {
                            listView1.CheckedItems[i].SubItems[3].Text = "登录成功";
                        }
                        else
                        {
                            listView1.CheckedItems[i].SubItems[3].Text = "登录失败";
                        }
                    }

                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        Thread thread;
      private void button6_Click(object sender, EventArgs e)
        {
          

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(login);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(login);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 主控端_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    listView1.Items[i].Checked = false;
                }
                else
                {
                    listView1.Items[i].Checked = true;
                }
               
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
               if(listView1.Items[i].Checked==true)
                {
                    listView1.Items[i].Remove();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                function.hide = false;
            }
            if (checkBox1.Checked == false)
            {
                function.hide = true;
            }
            function.openUrl = textBox1.Text.Trim();
            addweb();
            
        }

        private void 选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count>0)
             listView1.SelectedItems[0].Checked = true; 
        }

        private void 取消选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.SelectedItems[0].Checked = false;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.SelectedItems[0].Remove();
        }

        private void 导入代理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<string> iplist=new List<string>(); 
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName, function.EncodingType.GetTxtType(openFileDialog1.FileName));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                   
                        for (int j = 0; j < text.Length; j++)
                        {
                            string[] ips = text[j].Split(new string[] { "/" }, StringSplitOptions.None);
                            if (ips.Length > 3)
                            {
                                if (ips[0].Contains("."))
                                {
                                iplist.Add(text[j]);
                                   
                                }

                            }

                        }
                    
                    sr.Close();  //只关闭流
                    sr.Dispose();   //销毁流内存
                }

                int a = 0;

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                  if(a>=iplist.Count)
                    {
                        a = 0;
                    }
                    string[] ips =iplist[a].Split(new string[] { "/" }, StringSplitOptions.None);
                    listView1.Items[i].SubItems[6].Text = ips[0] + ":" + ips[1];
                    listView1.Items[i].SubItems[7].Text = ips[2];
                    listView1.Items[i].SubItems[8].Text = ips[3];
                    a = a + 1;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            function.KillProcess("msedgewebview2");
        }

        private void 全部退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            function.KillProcess("msedgewebview2");
        }
    }
}
