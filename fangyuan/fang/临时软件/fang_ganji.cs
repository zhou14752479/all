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

namespace fang.临时软件
{
    public partial class fang_ganji : Form
    {
        public fang_ganji()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fang_ganji_Load(object sender, EventArgs e)
        {

        }
        int page;
        bool status = true;

        #region  赶集本地服务
        public void run()
        {

            page = Convert.ToInt32(textBox3.Text);
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入二级网址！");
                return;
            }

            try
            {

                string[] citys = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                foreach (string city in citys)
                {

                    for (int i = 1; i < page; i++)
                    {
                       
                        string url = "http://"+ city + ".ganji.com/ershoufang/0/pn"+i+"/";

                        string html = method.GetUrl(url, "utf-8");

                        MatchCollection urls = Regex.Matches(html, @"<a target=""_blank"" rel=""nofollow"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                      
                        ArrayList lists = new ArrayList();
 
                        foreach (Match NextMatch in urls)
                        {
                            lists.Add("http:" + NextMatch.Groups[1].Value);

                        }


                      

                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)
                        {
                            textBox1.Text = list;

                            //联系人网址与其他网址不同
                            string strhtml = method.GetUrl(list, "utf-8");
                            Match title = Regex.Match(strhtml, @"title=([\s\S]*?)@");
                            Match tel = Regex.Match(strhtml, @"person_phone([\s\S]*?)>([\s\S]*?)</a>");
                            Match lxr = Regex.Match(strhtml, @"<div class=""name"">([\s\S]*?)target=""_blank"">([\s\S]*?)</a>");
                          

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                            lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]*>", "").Trim());
                            lv1.SubItems.Add(lxr.Groups[2].Value.Trim());
                            lv1.SubItems.Add(Regex.Replace(tel.Groups[2].Value.Trim(), "<[^>]*>", ""));
                     

                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));



                            if (listView1.Items.Count > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }

                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }

                        

                    }

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://sh.ganji.com/ershoufang/36507782718221x.shtml");
        }
    }
}
