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

namespace main._2019_7
{
    public partial class 网站查询 : Form
    {
        public 网站查询()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        string[] urls = { };
        public void ReadText()
        {
            label3.Text = "正在读取邮箱文件...请稍后";
            StreamReader streamReader = new StreamReader(this.textBox1.Text);
            string text = streamReader.ReadToEnd();
            urls= text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            label4.Text = "导入共"+urls.Length;
        }
        private void 网站查询_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        ArrayList finishes = new ArrayList();
        bool status = true;
        #region 主程序
        public void run()
        {
            int a = 0;
            try
            {
               

                foreach (string url in urls)
                {
                    a++;
                    label3.Text = "正在抓取"+url;
                    label4.Text = "导入共" + urls.Length+"已抓取"+a+"条";
                    if (!finishes.Contains(url))
                    {
                        finishes.Add(url);
                        if (url.Contains("@"))
                        {
                            string Url = url.Split('@')[1];

                            string html = method.GetUrl("http://www." + Url, "utf-8");

                            Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>", RegexOptions.IgnoreCase);
                            Match keyword = Regex.Match(html, @"<meta name=""keywords"" content=""([\s\S]*?)""", RegexOptions.IgnoreCase);
                            Match description = Regex.Match(html, @"<meta name=""description"" content=""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            string A1 = title.Groups[1].Value!="" ? title.Groups[1].Value : "FAILED";
                            string A2 = keyword.Groups[1].Value != "" ? keyword.Groups[1].Value : "FAILED";
                            string A3 = description.Groups[1].Value != "" ? description.Groups[1].Value : "FAILED";

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                                lv1.SubItems.Add(A1);
                                lv1.SubItems.Add(A2);
                                lv1.SubItems.Add(A3);
                                lv1.SubItems.Add(url);
                              
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            if (status == false)
                            {
                                return;
                            }
                         


                        }
                    }
                }

                MessageBox.Show("抓取完成");
            }

            




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void Button5_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入文件");
                return;
            }
            ReadText();

            #region 通用导出

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "3.3.3.3")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
                {
                    Thread thread = new Thread(new ThreadStart(run));
                    thread.Start();
                }

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
