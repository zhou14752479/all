using Newtonsoft.Json;
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
    public partial class 搜源网 : Form
    {
        public 搜源网()
        {
            InitializeComponent();
        }
        ArrayList finishes = new ArrayList();
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
            ArrayList lists = getListviewValue1(listView1);

            try

            {

                foreach (string list in lists)
                {
                    string URL = "https://bl.backend.semrush.com/?key=c3e728669724466c097a401ecddaba67&target="+list+"&type=backlinks_overview&method=nojsonp&target_type=root_domain&export_columns=backlinks_anchors%2Cdomains_num%2Chosts_num%2Ctexts_num%2Curls_num%2Cips_num%2Cipclassc_num%2Cgeodomains%2Czones%2Cfollows_num%2Cforms_num%2Cframes_num%2Cimages_num%2Cbacklinks%2Cbacklinks_pages%2Cbacklinks_refdomains";
                    string html = method.GetUrl(URL, "utf-8");

                    MatchCollection titles = Regex.Matches(html, @"source_title"":""([\s\S]*?)""");
                    MatchCollection anchors = Regex.Matches(html, @"anchor"":""([\s\S]*?)""");

                    if (titles.Count > 0)
                    {
                        for (int i = 0; i < titles.Count; i++)
                        {

                            ListViewItem lv3 = listView3.Items.Add(listView3.Items.Count.ToString()); //使用Listview展示数据
                            lv3.SubItems.Add(list);
                            lv3.SubItems.Add(titles[i].Groups[1].Value);                          

                            if (listView3.Items.Count - 1 > 1)
                            {
                                listView3.EnsureVisible(listView3.Items.Count - 1);                           
                            }

                        }
                    }

                    if (anchors.Count > 0)
                    {
                        for (int i = 0; i < anchors.Count; i++)
                        {

                            ListViewItem lv3 = listView3.Items.Add(listView3.Items.Count.ToString()); //使用Listview展示数据
                            lv3.SubItems.Add(list);
                            lv3.SubItems.Add(anchors[i].Groups[1].Value);

                            if (listView3.Items.Count - 1 > 1)
                            {
                                listView3.EnsureVisible(listView3.Items.Count - 1);
                            }

                        }
                    }
                    else
                    {
                        ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                        lv2.SubItems.Add(list);
                       
                    }

                    Thread.Sleep(1000);
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
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
            lv1.SubItems.Add(textBox1.Text);
            textBox1.Text = "";
        }

        private void skinButton8_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {


                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);


                }
            }
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView3), "Sheet1", true);
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            this.listView2.Items.Clear();
            this.listView3.Items.Clear();
        }

        private void 搜源网_Load(object sender, EventArgs e)
        {

        }
    }
}
