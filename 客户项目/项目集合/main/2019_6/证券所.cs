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

namespace main._2019_6
{
    public partial class 证券所 : Form
    {
        public 证券所()
        {
            InitializeComponent();
        }

        private void 证券所_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
       
        /// <summary>
        /// 上海
        /// </summary>
        public void run()
        {
            DateTime dt = DateTime.Parse(dateTimePicker1.Text);
            try
            {


                string Url = "http://acaiji.com/sse.php?date="+ dt.ToString("yyyyMMdd");

                string html = method.gethtml(Url,"", "utf-8");

                MatchCollection codes = Regex.Matches(html, @"""secCode"":""([\s\S]*?)""");
                MatchCollection jianchengs = Regex.Matches(html, @"""secAbbr"":""([\s\S]*?)""");
                MatchCollection mairus = Regex.Matches(html, @"""branchNameB"":""([\s\S]*?)""");
                MatchCollection mairujine = Regex.Matches(html, @"""branchTxAmtB"":""([\s\S]*?)""");

                MatchCollection maichus = Regex.Matches(html, @"""branchNameS"":""([\s\S]*?)""");
                MatchCollection maichujine = Regex.Matches(html, @"""branchTxAmtS"":""([\s\S]*?)""");

                for (int i = 0; i < codes.Count; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(codes[i].Groups[1].Value);
                    lv1.SubItems.Add(jianchengs[i].Groups[1].Value);
                    lv1.SubItems.Add(mairus[i].Groups[1].Value);
                    lv1.SubItems.Add(mairujine[i].Groups[1].Value);
                    lv1.SubItems.Add(maichus[i].Groups[1].Value);
                    lv1.SubItems.Add(maichujine[i].Groups[1].Value);

                }

            }


            catch (System.Exception ex)
            {

               ex.ToString();
            }

        }


        public void run1()
        {
            DateTime dt = DateTime.Parse(dateTimePicker1.Text);
            try
            {


                string Url = "http://reportdocs.static.szse.cn/files/text/jy/jy" + dt.ToString("yyMMdd")+".txt";

                string html = method.GetUrl(Url,  "gb2312");
                textBox1.Text = html;

            }


            catch (System.Exception ex)
            {

               ex.ToString();
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text == "上海证券交易所")
            {
                label3.Text = "正在获取上海证券交易所信息";
                textBox1.Visible = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }
            else if (comboBox1.Text == "深圳证券交易所")
            {
                label3.Text = "正在获取深圳证券交易所信息";
                textBox1.Visible = true;
                Thread thread = new Thread(new ThreadStart(run1));
                thread.Start();
            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
