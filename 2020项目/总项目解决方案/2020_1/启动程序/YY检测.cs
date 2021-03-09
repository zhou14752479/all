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

namespace 启动程序
{
    public partial class YY检测 : Form
    {
        public YY检测()
        {
            InitializeComponent();
        }




        bool zanting = true;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);


            string[] ips = method.GetUrl(textBox2.Text, "utf-8").Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int iptime = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != "")
                {
                    string[] values = array[i].Split(new string[] { "----" }, StringSplitOptions.None);

                    
                    string html =method.GetUrlwithIP("https://aq.yy.com/p/pwd/fgt/mnew/dpch.do?account="+values[0].Trim()+"&busifrom=&appid=1&yyapi=false", ips[iptime],"");
                    while (html.Contains("IP验证次数过多"))
                    {
                        iptime = iptime + 1;
                        html = method.GetUrlwithIP("https://aq.yy.com/p/pwd/fgt/mnew/dpch.do?account=" + values[0].Trim() + "&busifrom=&appid=1&yyapi=false", ips[iptime],"");

                    }

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(array[i]);

                    label1.Text = "正在检测" + array[i];
                    if (html.Contains("LEVEL5_BAN"))
                    {
                        lv1.SubItems.Add("手机类型");

                    }
                    else if (html.Contains("账号不存在"))
                    {
                        lv1.SubItems.Add("账号不存在");

                    }

                    else if (html.Contains("未设置密保"))
                    {
                        lv1.SubItems.Add("未设置密保");

                    }

                    else if (html.Contains("LEVEL5_NORMAL"))
                    {
                        lv1.SubItems.Add("密保问题");

                    }
                    else if (html.Contains("LEVEL3_EMAIL"))
                    {
                        lv1.SubItems.Add("邮箱类型");

                    }

                    else
                    {
                        lv1.SubItems.Add("其他类型");
                    }
                    //lv1.SubItems.Add(ips[iptime]);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }




            }
            
        }


        private void YY检测_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"yyjiance"))
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请导入卡号");
                    return;
                }

                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入代理IP链接");
                    return;
                }
                button2.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
