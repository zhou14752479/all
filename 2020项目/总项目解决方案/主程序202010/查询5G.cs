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
using helper;

namespace 主程序202010
{
    public partial class 查询5G : Form
    {
        public 查询5G()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }
        Thread thread;
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html =method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"5Gchaxun"))
            {
                MessageBox.Show("验证失败");
                return;
            }

            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        public void run()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择手机号");
                return;
            }
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    label3.Text = "正在查询："+text[i];
                    string url = "https://www.hzuni.com/unicom_wx/fg_search_yzm.aspx";
                    string postdata = "phone=" + text[i].Trim();
                    string html = method.PostUrl(url, postdata, "", "utf-8");
                  
                    Match name = Regex.Match(html, @"""pust_name_1"": ""([\s\S]*?)""");
                    Match name1 = Regex.Match(html, @"""pust_explain_1"": ""([\s\S]*?)""");

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add("请求成功");
                    lv1.SubItems.Add(name.Groups[1].Value + name1.Groups[1].Value);

                    Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                }
            }

           
           
        }
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 查询5G_Load(object sender, EventArgs e)
        {

        }
    }
}
