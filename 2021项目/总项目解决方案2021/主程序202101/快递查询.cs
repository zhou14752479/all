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

namespace 主程序202101
{
    public partial class 快递查询 : Form
    {
        public 快递查询()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        public void run()
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择TXT");
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
                    label3.Text = text[i];
                    string url = "https://express.baidu.com/express/api/express?query_from_srcid=&isBaiduBoxApp=10002&isWisePc=10020&tokenV2=P6FxStnRxMGjrpsXwFnPiVBkAooWRibl0BscIn9bxjywQKUW_ngAPwC6BLtQh_ZU&cb=jQuery1102039369380869411574_1609898924454&appid=4001&com=yunda&nu=" + text[i].Trim() + "&vcode=&token=&qid=eac37f1c0016bf9c&_=1609898924455";
                   
                    string html = method.GetUrl(url,"utf-8");

                    Match name = Regex.Match(method.Unicode2String(html), @"""RetMsg"":""([\s\S]*?)""");
                   

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add("请求成功");
                    lv1.SubItems.Add(name.Groups[1].Value.Replace("<br/>", "  "));

                    Thread.Sleep(1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                }
            }



        }
        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"qichachafapiao"))
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
