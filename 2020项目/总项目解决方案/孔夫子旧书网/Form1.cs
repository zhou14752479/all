using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 孔夫子旧书网
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            try
            {
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {

                    string url = "http://search.kongfz.com/item_result/?select=0&key="+richTextBox1.Lines[i]+"&status=1";
                    string html = method.GetUrl(url, "utf-8");

                    Match bookUrl = Regex.Match(html, @"<a class=""img-box"" target=""_blank""([\s\S]*?)href=""([\s\S]*?)""");
                    if (bookUrl.Groups[2].Value == "")
                    {
                        textBox1.Text += richTextBox1.Lines[i];
                    }
                    else
                    {
                        string bookHtml = method.GetUrl(bookUrl.Groups[2].Value, "utf-8");

                        Match a1 = Regex.Match(bookHtml, @"""bookName"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(bookHtml, @"""author"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(bookHtml, @"""press"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(bookHtml, @"""pubDate"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(bookHtml, @"""edition"":""([\s\S]*?)""");
                        Match a6 = Regex.Match(bookHtml, @"""isbn"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(bookHtml, @"""price"":""([\s\S]*?)""");
                        Match a8 = Regex.Match(bookHtml, @"""binding"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(bookHtml, @"""pageSize"":""([\s\S]*?)""");
                        Match a10 = Regex.Match(bookHtml, @"""usedPaper"":""([\s\S]*?)""");
                        Match a11 = Regex.Match(bookHtml, @"""pageNum"":""([\s\S]*?)""");
                        Match a12 = Regex.Match(bookHtml, @"""wordNum"":""([\s\S]*?)""");
                        Match a13 = Regex.Match(bookHtml, @"""language"":""([\s\S]*?)""");
                        Match a14 = Regex.Match(bookHtml, @"""series"":""([\s\S]*?)""");
                        Match a15 = Regex.Match(bookHtml, @"""catName"":""([\s\S]*?)""");
                        Match a16 = Regex.Match(bookHtml, @"""contentIntroduction"":""([\s\S]*?)""");
                        Match a17 = Regex.Match(bookHtml, @"""authorIntroduction"":""([\s\S]*?)""");
                        Match a18 = Regex.Match(bookHtml, @"""directory"":""([\s\S]*?)""");

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(Unicode2String(a1.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a2.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a3.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a4.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a5.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a6.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a7.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a8.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a9.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a10.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a11.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a12.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a13.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a14.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a15.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a16.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a17.Groups[1].Value));
                        listViewItem.SubItems.Add(Unicode2String(a18.Groups[1].Value));
                    }
                   
                   
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

     

        private void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}
