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
    public partial class 连续出现 : Form
    {
        public 连续出现()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
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
            StringBuilder sbz = new StringBuilder();
            for (int i = 0; i <Convert.ToInt32(textBox1.Text); i++)
            {
                sbz.Append("中");
            }

            StringBuilder sbc = new StringBuilder();
            for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
            {
                sbc.Append("错");
            }

          
            ArrayList lists = getListviewValue1(listView1);

            try

            {

                foreach (string list in lists)
                {
                    if (!finishes.Contains(list))
                    {
                        finishes.Add(list);
                        string url = list.Replace("https://pk10.17500.cn/exp/index/eid/", "").Replace(".html", ""); ;

                        if (url == "")
                            return;
                        string html = method.GetUrl("https://pk10.17500.cn/exp/results.html?num=30&lotid=pk10&eid=" + url, "utf-8");

                        string prttern = @"中|错";
                        MatchCollection matches = Regex.Matches(html, prttern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        
                        StringBuilder sb = new StringBuilder();
                        foreach (Match NextMatch in matches)
                        {

                            sb.Append(NextMatch.Groups[0].Value);

                        }

                        if (sb.ToString().Contains(sbz.ToString()) || sb.ToString().Contains(sbc.ToString()))
                        {
                            MessageBox.Show(list+"出现连续"+ textBox1.Text+"次数");
                        }


                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void 连续出现_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            timer1.Start();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
