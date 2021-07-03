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
using myDLL;

namespace 坦程余额查询
{
    public partial class 坦程余额查询 : Form
    {
        public 坦程余额查询()
        {
            InitializeComponent();
        }

        private void 坦程余额查询_Load(object sender, EventArgs e)
        {
            //listView1.Columns.Add("ID", 60, HorizontalAlignment.Center);
        }

        private void button6_Click(object sender, EventArgs e)
        {
          
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(sfd.FileName, method.EncodingType.GetTxtType(sfd.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                for (int i = 0; i < text.Length; i++)
                {
                    textBox1.Text += text[i] + "\r\n";
                }

            }
        }

        public void run()
        {
            try
            {


                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    label1.Text = "正在查询：" + text[i];
                    if (text[i] != "")
                    {
                        string param = "{\"cardNo\":\""+text[i]+"\",\"openId\":\"oRpf9tp6H4FrFHJtj7fahJ8LGEis\"}";
                        param = method.Base64Encode(Encoding.GetEncoding("utf-8"), param);
                        string url = "http://hz.4008812356.com/tc_vsmp/getOilCardBalanceByWx?params=" + param;
                        string html = method.GetUrl(url, "utf-8");
                        html = method.Base64Decode(Encoding.GetEncoding("utf-8"), html);
                     
                        string balance = Regex.Match(html, @"""balance"":""([\s\S]*?)""").Groups[1].Value;
                        string cardBalance = Regex.Match(html, @"""cardBalance"":""([\s\S]*?)""").Groups[1].Value;
                        string preBalance = Regex.Match(html, @"""preBalance"":""([\s\S]*?)""").Groups[1].Value;

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text[i]);
                        if (checkBox1.Checked == true)
                        {
                            lv1.SubItems.Add(balance);
                        }
                        else
                        {
                            lv1.SubItems.Add("");
                        }
                        if (checkBox2.Checked == true)
                        {
                            lv1.SubItems.Add(cardBalance);
                        }
                        else
                        {
                            lv1.SubItems.Add("");
                        }
                        if (checkBox3.Checked == true)
                        {
                            lv1.SubItems.Add(preBalance);
                        }
                        else
                        {
                            lv1.SubItems.Add("");
                        }

                        Thread.Sleep(1000);

                        if (status == false)
                            return;
                    }



                }
                label1.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        bool zanting = true;
        Thread thread;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"Eq7ZF"))
            {

                return;
            }

            #endregion
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        public void run1()
        {
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
            
                string param = "{\"cardNo\":\"" +listView1.CheckedItems[i].SubItems[1].Text + "\",\"openId\":\"oRpf9tp6H4FrFHJtj7fahJ8LGEis\"}";
                param = method.Base64Encode(Encoding.GetEncoding("utf-8"), param);
                string url = "http://hz.4008812356.com/tc_vsmp/getOilCardBalanceByWx?params=" + param;
                string html = method.GetUrl(url, "utf-8");
                html = method.Base64Decode(Encoding.GetEncoding("utf-8"), html);

                string balance = Regex.Match(html, @"""balance"":""([\s\S]*?)""").Groups[1].Value;
                string cardBalance = Regex.Match(html, @"""cardBalance"":""([\s\S]*?)""").Groups[1].Value;
                string preBalance = Regex.Match(html, @"""preBalance"":""([\s\S]*?)""").Groups[1].Value;

              
                if (checkBox1.Checked == true)
                {
                   listView1.CheckedItems[i].SubItems[2].Text= balance;
                }
                else
                {
                    listView1.CheckedItems[i].SubItems[2].Text = "";
                }
                if (checkBox2.Checked == true)
                {
                    listView1.CheckedItems[i].SubItems[3].Text = cardBalance;
                }
                else
                {
                    listView1.CheckedItems[i].SubItems[3].Text = "";
                }
                if (checkBox3.Checked == true)
                {
                    listView1.CheckedItems[i].SubItems[4].Text = preBalance;
                }
                else
                {
                    listView1.CheckedItems[i].SubItems[4].Text = "";
                }

            }
        }

        private void 复制卡号ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run1);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            Array file = (System.Array)e.Data.GetData(DataFormats.FileDrop);
           
            foreach (object I in file)
            {
                StreamReader sr = new StreamReader(I.ToString(), method.EncodingType.GetTxtType(I.ToString()));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                for (int i = 0; i < text.Length; i++)
                {
                    textBox1.Text += text[i] + "\r\n";
                }
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void 坦程余额查询_FormClosing(object sender, FormClosingEventArgs e)
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
