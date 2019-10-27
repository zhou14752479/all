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
        /// <summary>
        /// 公示期
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

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(a1.Groups[1].Value);
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
    }
}
