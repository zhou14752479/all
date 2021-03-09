using System;
using System.Collections;
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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "";
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                path = this.openFileDialog1.FileName;

            }

            DataTable dt = method.ExcelToDataSet(path).Tables[0];
            dataGridView1.DataSource = dt;
        }
        public void run()
        {
            try
            {


                for (int i = 1; i < 100; i++)
                {
                    string url = "http://sijiao.118cha.com/";
                    string postdata = System.Web.HttpUtility.UrlEncode(textBox1.Text);

                    string html = method.PostUrl(url, postdata);

                    MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                    


                    Thread.Sleep(500);



                }






            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(aRun));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        public void aRun()
        {
            string url = "http://sijiao.118cha.com/";
            string postdata = "q="+System.Web.HttpUtility.UrlEncode(textBox1.Text);
            
            string html = method.PostUrl(url,postdata);
            Match code = Regex.Match(html, @"</a></td><td><a href=""([\s\S]*?)""");
            textBox2.Text = code.Groups[1].Value.Remove(4);
        }

        public void bRun()
        {
            string url = "http://sijiao.118cha.com/";

            string xingming = textBox3.Text.Trim();
            ArrayList lists = new ArrayList();

            for (int i = 0; i < xingming.Length; i++)
            {
                lists.Add(textBox3.Text.Substring(i,1));
            }

            StringBuilder sb = new StringBuilder();
            if (lists.Count == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    string postdata = "q=" + System.Web.HttpUtility.UrlEncode(lists[i].ToString());
                    string html = method.PostUrl(url, postdata);
                    Match code = Regex.Match(html, @"</a></td><td><a href=""([\s\S]*?)""");
                    sb.Append(code.Groups[1].Value.Remove(4));
                }

                textBox4.Text = sb.ToString();
            }

            else if (lists.Count == 3)
            {

                string postdata = "q=" + System.Web.HttpUtility.UrlEncode(lists[0].ToString());

                string html = method.PostUrl(url, postdata);
                Match code = Regex.Match(html, @"</a></td><td><a href=""([\s\S]*?)""");
                sb.Append(code.Groups[1].Value.Remove(4));

                for (int i = 1; i < 3; i++)
                {             
                    string postdata1 = "q=" + System.Web.HttpUtility.UrlEncode(lists[i].ToString());
                    string html1 = method.PostUrl(url, postdata1);
                    Match code1 = Regex.Match(html1, @"</a></td><td><a href=""([\s\S]*?)""");
                    sb.Append(code1.Groups[1].Value.Remove(2));
                }
                textBox4.Text = sb.ToString();
            }
            else if (lists.Count == 4)
            {
                foreach (string list in lists)
                {
                    string postdata = "q=" + System.Web.HttpUtility.UrlEncode(list);

                    string html = method.PostUrl(url, postdata);
                    Match code = Regex.Match(html, @"</a></td><td><a href=""([\s\S]*?)""");
                    sb.Append(code.Groups[1].Value.Remove(2));

                }
                textBox4.Text = sb.ToString();
            }

           
        }


        public void cRun()
        {

            for (int j = 0; j < dataGridView1.Rows.Count - 1; j++)
            {
                string url = "http://sijiao.118cha.com/";

                string xingming = dataGridView1.Rows[j].Cells[1].Value.ToString();
                ArrayList lists = new ArrayList();

                for (int i = 0; i < xingming.Length; i++)
                {
                    lists.Add(xingming.Substring(i, 1));
                }

                StringBuilder sb = new StringBuilder();
                if (lists.Count == 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        string postdata = "q=" + System.Web.HttpUtility.UrlEncode(lists[i].ToString());
                        string html = method.PostUrl(url, postdata);
                        Match code = Regex.Match(html, @"</a></td><td><a href=""([\s\S]*?)""");
                        sb.Append(code.Groups[1].Value.Remove(4));
                    }

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    lv1.SubItems.Add(dataGridView1.Rows[j].Cells[1].Value.ToString().Trim());
                    lv1.SubItems.Add(sb.ToString());
                    

                }

                else if (lists.Count == 3)
                {

                    string postdata = "q=" + System.Web.HttpUtility.UrlEncode(lists[0].ToString());

                    string html = method.PostUrl(url, postdata);
                    Match code = Regex.Match(html, @"</a></td><td><a href=""([\s\S]*?)""");
                    sb.Append(code.Groups[1].Value.Remove(4));

                    for (int i = 1; i < 3; i++)
                    {
                        string postdata1 = "q=" + System.Web.HttpUtility.UrlEncode(lists[i].ToString());
                        string html1 = method.PostUrl(url, postdata1);
                        Match code1 = Regex.Match(html1, @"</a></td><td><a href=""([\s\S]*?)""");
                        sb.Append(code1.Groups[1].Value.Remove(2));
                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    lv1.SubItems.Add(dataGridView1.Rows[j].Cells[1].Value.ToString().Trim());
                    lv1.SubItems.Add(sb.ToString());
                    

                }
                else if (lists.Count == 4)
                {
                    foreach (string list in lists)
                    {
                        string postdata = "q=" + System.Web.HttpUtility.UrlEncode(list);

                        string html = method.PostUrl(url, postdata);
                        Match code = Regex.Match(html, @"</a></td><td><a href=""([\s\S]*?)""");
                        sb.Append(code.Groups[1].Value.Remove(2));

                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    lv1.SubItems.Add(dataGridView1.Rows[j].Cells[1].Value.ToString().Trim());
                    lv1.SubItems.Add(sb.ToString());
                    

                }
                Thread.Sleep(10);
            }
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(bRun));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < DataGridView1.Rows.Count - 1; i++)
            //{
            //    for (int j = 0; j < DataGridView1.Columns.Count; j++)
            //    {
            //        s = DataGridView1.Rows[i].Cells[j].Value;
            //    }
            //}
            Thread thread = new Thread(new ThreadStart(cRun));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();

        }
    }
}
