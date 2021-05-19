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
namespace 主程序202105
{
    public partial class 推广链接生成 : Form
    {
        public 推广链接生成()
        {
            InitializeComponent();
        }
        List<string> links = new List<string>();
        List<string> skus = new List<string>();
        Thread thread = null;
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请导入链接和SKU");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        int linkscount = 0;

        public void run()
        {

            for (int j = 0; j < skus.Count; j++)
            {
                if (linkscount ==links.Count)
                {
                    linkscount = 0;
                }
              
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    string uid = Regex.Match(links[linkscount], @"&uid=.*").Groups[0].Value.Replace("&uid=", "");
                    string url = "http://p.zjdg.cn/data/search.ashx?itemid=" + skus[j] + "&uid=" + uid + "&channel=JYAOXING&method=getrebatetoolurl";
                   
                    string html = method.GetUrl(url, "utf-8");
                  
                    string result = Regex.Match(html, @"""data"":""([\s\S]*?)""").Groups[1].Value;

                    if (status == false)
                        return;
                    lv1.SubItems.Add(result);
                    Thread.Sleep(100);
                linkscount = linkscount + 1;
                
            }

        }

        bool status = false;
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sfd.FileName;
                StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    links.Add(text[i]);
                    //listView1.Columns.Add(i.ToString(), 100);
                   
                }
               
            }
            

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = sfd.FileName;
                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    skus.Add(text[i]);
                   
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 推广链接生成_Load(object sender, EventArgs e)
        {

        }
    }
}
