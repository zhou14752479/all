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
    public partial class leju : Form
    {
        string html;
        bool loaded = true;

        public leju()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        bool status = true;
        private void leju_Load(object sender, EventArgs e)
        {
            method.get58CityName(comboBox1);
        }


        #region  乐居二手房
        public void run()
        {
            string city = method.Get58pinyin(comboBox1.SelectedItem.ToString());
            try
            {


                for (int i = 1; i <2; i++)
                {
                    String Url = "https://"+city+".zufang.leju.com/house/n"+i+"/";
                    textBox1.Text = Url;

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"linkto=""([\s\S]*?)#", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add("https:" + NextMatch.Groups[1].Value);


                        if (this.status == false)
                            return;
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     


                    if (listView1.Items.Count - 1 > 0)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);
                    }
                }



            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.status = true;


                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            StreamReader reader = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(webBrowser1.Document.Encoding));
            string html = reader.ReadToEnd();
            textBox1.Text = html;
            this.loaded = true;
        }
    }
}
