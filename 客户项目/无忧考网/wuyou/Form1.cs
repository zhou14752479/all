using System;
using System.Collections;
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
using method;

namespace wuyou
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void run()
        {
            try
            {
                

                    string[] keywords = textBox1.Text.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (string keyword in keywords)
                {

                string url = "http://search.51test.net/cse/search?q="+keyword+"&p=0&s=10721678224061263659&entry=1";
                    
                    string html = methodClass.GetUrl(url, "utf-8", "");
                    
                    MatchCollection urls = Regex.Matches(html, @"cpos=""title"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    
                    string strhtml = methodClass.GetUrl(urls[1].Groups[1].Value, "gb2312", "");

                     MatchCollection matches = Regex.Matches(strhtml, @"<h2><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match match in matches)
                    {
                        lists.Add("http://www.51test.net"+match.Groups[1].Value.Trim());
                    }

                    MessageBox.Show(lists.Count.ToString());

                    for (int i = 0; i < 6; i++)
                    {
                        string ahtml = methodClass.GetUrl(lists[i].ToString(), "gb2312", "");

                        Match title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>");
                        Match body = Regex.Match(ahtml, @"<div class=""content-txt"">([\s\S]*?)<div class=""content_download"">");
                        ListViewItem lv1 = this.listView1.Items.Add(title.Groups[1].Value);
                        lv1.SubItems.Add(body.Groups[1].Value.Replace("<div class=\"describe\"><i></i>", ""));


                        if (listView1.Items.Count > 1)
                        {
                            this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                        }
                        
                        Application.DoEvents();
                        Thread.Sleep(1000);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in listView1.Items)
                {
                    string temp = item.SubItems[0].Text;
                    list.Add(temp + "\r\n"+item.SubItems[1].Text);
                }
                Thread thexp = new Thread(() => export(list)) { IsBackground = true };
                thexp.Start();
            }
        }

        private void export(List<string> list)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + Guid.NewGuid().ToString() + ".docx";

            //删除txt文本
            string path1 = Environment.CurrentDirectory;
            string pattern = "*.txt";
            string[] strFileName = Directory.GetFiles(path1, pattern);
            foreach (var item in strFileName)
            {
                File.Delete(item);

            }
            //删除txt文本

            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("导出完成！存放地址为程序文件夹");

        }

    }
}
