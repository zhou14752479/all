using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 京东图片
{
    public partial class 京东图片 : Form
    {
        public 京东图片()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;


            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }


        /// <summary>
        /// 获取详情图
        /// </summary>
        /// <returns></returns>
        public string getxqpic(string uid)
        {
            string url = "https://cd.jd.com/description/channel?skuId=" + uid + "&mainSkuId=" + uid + "&charset=utf-8&cdn=2&callback=showdesc";
            string html = method.GetUrl(url, "utf-8");

            MatchCollection pics = Regex.Matches(html, @"background-image:url\(([\s\S]*?)\)");
            StringBuilder sb = new StringBuilder();
            foreach (Match item in pics)
            {
                sb.Append("https:" + item.Groups[1].Value + "\r\n");
            }
            return sb.ToString();
        }

        public void run()
        {
            try
            {
                DataTable dt = method.ExcelToDataTable(textBox1.Text, true);



                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    string itemid = dt.Rows[i][0].ToString().Trim();
                    label1.Text =DateTime.Now.ToString()+ "正在获取："+itemid;
                    string url = "https://item.jd.com/"+ itemid + ".html";
                    string html = method.GetUrl(url, "utf-8");
                    string cate = Regex.Match(html, @"catName: \[([\s\S]*?)\]").Groups[1].Value.Trim().Replace("\"", "");
                    string title = Regex.Match(html, @"name: '([\s\S]*?)'").Groups[1].Value.Trim();
                    string pinpai = Regex.Match(html, @"ellipsis"" title=""([\s\S]*?)""").Groups[1].Value.Trim();
                    string xinghao = Regex.Match(html, @"data-value=""([\s\S]*?)""").Groups[1].Value.Trim();

                    string zhupic = Regex.Match(html, @"imageList: \[([\s\S]*?)\]").Groups[1].Value.Trim().Replace("\"", "");

                    string[] cates = cate.Split(new string[] { "," }, StringSplitOptions.None);
                    string[] zhupics = zhupic.Split(new string[] { "," }, StringSplitOptions.None);


                    StringBuilder zhupicsb = new StringBuilder();
                    foreach (var item in zhupics)
                    {
                        zhupicsb.Append("https://img14.360buyimg.com/n1/" + item + "\r\n");
                    }



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(url);
                    lv1.SubItems.Add(title);
                    lv1.SubItems.Add(cates[0]);
                    lv1.SubItems.Add(cates[1]);
                    lv1.SubItems.Add(cates[2]);
                    lv1.SubItems.Add(pinpai);
                    lv1.SubItems.Add(xinghao);



                    lv1.SubItems.Add(zhupicsb.ToString());

                    string xqpic = getxqpic(itemid);
                    lv1.SubItems.Add(xqpic.ToString());
                    MessageBox.Show(title);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
