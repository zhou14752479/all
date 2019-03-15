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
    public partial class 域名收录 : Form
    {
        public 域名收录()
        {
            InitializeComponent();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
            lv1.SubItems.Add(textBox1.Text);
            textBox1.Text = "";
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
        bool zanting = true;
        
        ArrayList finishes = new ArrayList();

        #region  主函数
        public void run()

        {
            ArrayList lists = getListviewValue1(listView1);

            try

            {

                foreach (string id in lists)
                {

                    
                    string html = method.GetUrl("http://www.sogou.com//web?query=site%3A"+id, "utf-8");

                    string wwwhtml = method.GetUrl("http://www.sogou.com//web?query=www."+id,"utf-8");
                    string shoulu = "未收录";
                    if (!wwwhtml.Contains("未收录"))
                    {
                        shoulu = "已收录";
                    }
                        Match nums = Regex.Match(html, @"'rn':([\s\S]*?),");
                    Match fanlians = Regex.Match(wwwhtml, @"totalItems([\s\S]*?)--");

                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(id);

                    lv2.SubItems.Add(nums.Groups[1].Value);
                    lv2.SubItems.Add(shoulu);
                    lv2.SubItems.Add(fanlians.Groups[1].Value.ToString());


                    if (listView2.Items.Count - 1 > 1)
                            {
                                listView2.EnsureVisible(listView2.Items.Count - 1);
                            }
                            //如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。

                            while (this.zanting == false)
                            {
                                Application.DoEvents();
                            }
                            Thread.Sleep(1000);


                        }

                    }     

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

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

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }
    }
}
