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

namespace fang.临时软件
{
    public partial class 京东搜索 : Form
    {
        public 京东搜索()
        {
            InitializeComponent();
        }
        bool zanting = true;

        public string[] ReadText()
        {
            string currentDirectory = Environment.CurrentDirectory;
            StreamReader streamReader = new StreamReader(this.textBox1.Text,Encoding.Default);
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

                    
                    string url = "https://search.jd.com/Search?keyword=" + keyword + "&enc=utf-8";
                    string html = method.GetUrl(url, "utf-8");
                    Match match = Regex.Match(html, @"data-sku=""([\s\S]*?)""");
                    Match count = Regex.Match(html, @"result_count:'([\s\S]*?)'");

                    
                    string URL = "https://item.jd.com/" + match.Groups[1].Value.Trim() + ".html";
                    string strhtml = method.GetUrl(URL, "gbk");

                    string commentUrl = "https://club.jd.com/comment/productCommentSummaries.action?referenceIds=" + match.Groups[1].Value.Trim();
                    string commenthtml = method.GetUrl(commentUrl, "gbk");

                    MatchCollection matches = Regex.Matches(strhtml, @"mbNav-([\s\S]*?)"">([\s\S]*?)</a>");
                    Match commentCount = Regex.Match(commenthtml, @"CommentCountStr"":""([\s\S]*?)""");
                    StringBuilder sb = new StringBuilder();
                    foreach (Match item in matches)
                    {
                        sb.Append(item.Groups[2].Value.Trim() + "  ");
                    }

                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count+1).ToString());
                    listViewItem.SubItems.Add(keyword);
                    listViewItem.SubItems.Add(count.Groups[1].Value.ToString());
                    listViewItem.SubItems.Add(sb.ToString());
                    listViewItem.SubItems.Add(commentCount.Groups[1].Value.ToString());
                    if (this.listView1.Items.Count>2)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void 京东搜索_Load(object sender, EventArgs e)
        {

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
            Thread thread = new Thread(new ThreadStart(this.run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
