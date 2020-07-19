using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202007
{
    public partial class 内容分列 : Form
    {
        public 内容分列()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 去掉空白行
        /// </summary>
        /// <returns></returns>
        public string removeHang()
        {
            StringBuilder sb = new StringBuilder();
           
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                if (richTextBox1.Lines[i].Trim() != "")
                {
                    sb.Append(richTextBox1.Lines[i].Trim()+ "\r\n");
                }
            }
            return sb.ToString().Replace(" ", "").Trim();
        }
        /// <summary>
        /// 去掉第一第二行
        /// </summary>
        /// <returns></returns>
        public string removediyi(string texts)
        {
            StringBuilder sb = new StringBuilder();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                if (i != 0 && i != 1)
                {
                    sb.Append(text[i].Trim() + "\r\n");
                }
               
            }
            return sb.ToString().Trim();
        }

        public void run()
        {

            string body = removeHang();

            string body1 = removediyi(body);

            string body2 = Regex.Replace(body1, @"\d{10,}", "");

            string[] texts = body.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string username = "";
            if (texts[0].Contains("会员"))
            {
                username = texts[0].Replace("会员名", "").Replace(":", "").Replace("：", "");
            }
            else if (texts[1].Contains("会员"))
            {
                username = texts[1].Replace("会员名", "").Replace(":", "").Replace("：", "");
            }
            else if (texts[0].Contains("文件"))
            {
                username = texts[1].Replace("会员名", "").Replace(":", "").Replace("：", "").Replace("淘宝号","").Replace("+","");
            }
            else
            {
                username = texts[0].Replace("会员名", "").Replace(":", "").Replace("：", "").Replace("淘宝号", "").Replace("+", "");
            }



            MatchCollection orders = Regex.Matches(body, @"11\d{10,}|60\d{10,}|59\d{10,}", RegexOptions.IgnoreCase);
            MatchCollection shops = Regex.Matches(body1, @"([\u4e00-\u9fa5]|[\u4e00-\u9fa5]|[A-Za-z]){2,}");
            MatchCollection prices = Regex.Matches(body2, @"\d{1,6}\.\d{1,6}|\d{1,6}", RegexOptions.IgnoreCase);
            for (int i = 0; i < orders.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(username);
                lv1.SubItems.Add(orders[i].Groups[0].Value);
                lv1.SubItems.Add(prices[i].Groups[0].Value);
                lv1.SubItems.Add(shops[i].Groups[0].Value);
            }
            
            double value = 0;
            double zong = Convert.ToDouble(prices[prices.Count - 1].Groups[0].Value);
            
            for (int j = 0; j < prices.Count-1; j++)
            {
                value =value+ Convert.ToDouble(prices[j].Groups[0].Value);
            }
            label1.Text = value.ToString();

            if (value != zong)
            {
                label1.BackColor = Color.Red;
            }
            else
            {
                label1.BackColor = Color.Green;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"fenlie"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            run();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void 内容分列_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空表格数据吗？", "清空", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                listView1.Items.Clear();
            }
            else
            {
               
            }
           
        }



        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
