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
    public partial class 百家号 : Form
    {
        public 百家号()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
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
        ArrayList finishes = new ArrayList();

        #region  主函数
        public void run()

        {
            ArrayList lists = getListviewValue1(listView1);

            try

            {

                foreach (string id in lists)
                {
                    string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹


                    string html = method.GetUrl("https://author.baidu.com/pipe?tab=2&app_id="+id+"&num=20&pagelets[]=article&reqID=1&isspeed=0", "utf-8");

                        MatchCollection titles = Regex.Matches(html, @"<h2>([\s\S]*?)</h2>");
                    MatchCollection reads = Regex.Matches(html, @"<div class=\\""pv\\"">([\s\S]*?)阅读");
                    MatchCollection times = Regex.Matches(html, @"<div class=\\""time\\"">([\s\S]*?)</div>");

                    if (titles.Count > 0)
                    {
                        for (int j = 0; j < titles.Count; j++)
                        {
                            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count+1).ToString()); //使用Listview展示数据
                            lv2.SubItems.Add(titles[j].Groups[1].Value);
                            lv2.SubItems.Add(reads[j].Groups[1].Value);
                            lv2.SubItems.Add(times[j].Groups[1].Value);

                            if (listView2.Items.Count - 1 > 1)
                            {
                                listView2.EnsureVisible(listView2.Items.Count - 1);
                            }

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
            thread.Start();
        }
    }
}
