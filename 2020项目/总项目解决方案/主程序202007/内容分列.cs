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
            return sb.ToString().Trim();
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


        private void button1_Click(object sender, EventArgs e)
        {
            string body = removeHang();
           
            string body1 = removediyi(body);

            string  body2= Regex.Replace(body1, @"\d{10,}", "");
            richTextBox1.Text = body2;
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
                username = texts[1].Replace("会员名", "").Replace(":", "").Replace("：", "");
            }
            else
            {
                username = texts[0].Replace("会员名", "").Replace(":", "").Replace("：", "");
            }

           

            MatchCollection orders = Regex.Matches(body, @"11\d{10,}|60\d{10,}");
            MatchCollection shops = Regex.Matches(body1, @"[\u4e00-\u9fa5]{2,}");
            MatchCollection prices = Regex.Matches(body2, @"\d{1,6}\.\d{1,6}|\d{1,6}", RegexOptions.IgnoreCase);
            for (int i = 0; i <orders.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(username);
                lv1.SubItems.Add(orders[i].Groups[0].Value);
                lv1.SubItems.Add(prices[i].Groups[0].Value);
                lv1.SubItems.Add(shops[i].Groups[0].Value);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void 内容分列_Load(object sender, EventArgs e)
        {

        }
    }
}
