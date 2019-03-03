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

namespace fang.临时软件
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        bool status = true;

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {


                    ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(text[i]);


                }
            }
        }
        /// <summary>
        /// 获取第二列
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue1(ListView listview)

        {
            ArrayList values = new ArrayList();

            for (int i = 0; i < listview.Items.Count; i++)
            {
                ListViewItem item = listview.Items[i];

                values.Add(item.SubItems[1].Text);


            }

            return values;

        }

        #region  主函数
        public void run()

        {
            ArrayList lists = getListviewValue1(listView2);

            try

            {

                foreach (string URL in lists)
                {
                    string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹


                    string html = method.GetUrl("https://whois.aizhan.com/"+URL+"/", "utf-8");

                  
                    string prttern = @"域名持有人/机构邮箱</td>([\s\S]*?)<a href=""/reverse-whois([\s\S]*?)""";
                    Match matches = Regex.Match(html, prttern);

                    for (int i = 1; i < 100; i++)
                    {

                        string strhtml = method.GetUrl("https://whois.aizhan.com/reverse-whois" + matches.Groups[2].Value + "&page=" + i, "utf-8");


                        string rxg = @"rel=""nofollow"" href=""([\s\S]*?)""";
                        string rxg1 = @"请输入您要查询的邮箱"" value=""([\s\S]*?)""";

                        MatchCollection urls = Regex.Matches(strhtml, rxg);
                        Match mail = Regex.Match(strhtml, rxg1);

                        if (urls.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        for (int j = 0; j < urls.Count; j++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(urls[j].Groups[1].Value);
                            lv1.SubItems.Add(mail.Groups[1].Value);

                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }


                            if (this.status == false)
                                return;

                        }

                    }
                   


                }
            }

            catch (Exception ex)
            {
                MessageBox.Show( ex.ToString());
            }
        }

        #endregion

        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.status = true;
            if (listView2.Items.Count < 1)
            {

                MessageBox.Show("请先添加网址");
                return;
            }
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();

        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void 删除改项ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
