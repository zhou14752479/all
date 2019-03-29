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

namespace fang._2019
{
    public partial class 拼多多搜索 : Form
    {
        public 拼多多搜索()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {

        }
        bool zanting = true;

        public string[] ReadText()
        {
            string currentDirectory = Environment.CurrentDirectory;
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        public void run()
        {
            try
            {
                string[] array = this.ReadText();
                foreach (string keyword in array)
                {


                    string url = "http://mobile.yangkeduo.com/search_result.html?search_key="+keyword+"&sort_type=_sales";
                    string html = method.GetUrl(url, "utf-8");
                    if (!html.Contains("没有找到"))
                    {
                        Match match = Regex.Match(html, @"""goodsID"":([\s\S]*?),");

                      
                        string URL = "http://mobile.yangkeduo.com/goods.html?goods_id=" + match.Groups[1].Value.Trim()+ "&refer_page_name=search_result&refer_page_id=10015_1553736821291_8KrSI6PtCa&refer_page_sn=10015";

                        string strhtml = method.GetUrl(URL, "utf-8");

                        Match counts = Regex.Match(strhtml, @"已拼([\s\S]*?)<");
                        Match price = Regex.Match(strhtml, @"""price-range"">([\s\S]*?)<");
                        Match commentCount = Regex.Match(strhtml, @"commentsAmount"":([\s\S]*?),");
                        
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(keyword);
                        listViewItem.SubItems.Add(counts.Groups[1].Value.ToString().Replace(":","").Trim());
                        listViewItem.SubItems.Add(price.Groups[1].Value.ToString());
                        listViewItem.SubItems.Add(commentCount.Groups[1].Value.ToString());

                        if (this.listView1.Items.Count > 2)
                        {
                            this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                        }

                        Application.DoEvents();
                        Thread.Sleep(Convert.ToInt32(textBox2.Text));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }

                    else
                    {
                        Match sousuo = Regex.Match(html, @"“<!-- -->([\s\S]*?)<!-- -->");

                       
                        ListViewItem listViewItem = this.listView2.Items.Add((listView2.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(keyword);
                        listViewItem.SubItems.Add(sousuo.Groups[1].Value.ToString());                 
                        if (this.listView2.Items.Count > 2)
                        {
                            this.listView2.EnsureVisible(this.listView2.Items.Count - 1);
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void 拼多多搜索_Load(object sender, EventArgs e)
        {

        }
    }
}
