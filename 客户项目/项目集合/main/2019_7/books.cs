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

namespace main._2019_7
{
    public partial class books : Form
    {
        public books()
        {
            InitializeComponent();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            getUrlbyWeb get = new getUrlbyWeb();
            get.Show();
        }

        bool zanting = true;

        public void run()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            try
            {

                for (int i = 1; i <51; i++)
                {

                    label3.Text = "正在抓取第" + i + "页" + "........";

                          string Url = getUrlbyWeb.URL+ "?page="+i;


                    string html = method.gethtml(Url, "", "gb2312");
                    if (html == null)
                        break;
                    MatchCollection titles = Regex.Matches(html, @"<span title=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection authers = Regex.Matches(html, @"<div class='product-search-result__author'>([\s\S]*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection banbens = Regex.Matches(html, @"<ul class='horizontal-dictionary'>([\s\S]*?)<li class='product-search-result__price'>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection prices = Regex.Matches(html, @"<li class='product-search-result__price'>([\s\S]*?)</li>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection pics = Regex.Matches(html, @"<img class=""product-cover__image"" alt="""" src=""([\s\S]*?)width", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                 
                    for (int j = 0; j < titles.Count; j++)

                    {
                        MatchCollection price = Regex.Matches(prices[j].Groups[1].Value, @"student_price&quot;:&quot;([\s\S]*?)&", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        label3.Text = "正在抓取...."+ titles[j].Groups[1].Value.Trim();
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(titles[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(authers[j].Groups[1].Value.Replace("by","").Trim());
                        lv1.SubItems.Add(banbens[j].Groups[1].Value.Replace("<li class='product-search-result__edition'>Edition: ","").Replace("</li>","").Trim());
                        StringBuilder sb = new StringBuilder();
                        for (int a = 0; a < price.Count; a++)
                        {
                            sb.Append(price[a].Groups[1].Value+"，");
                        }
                        lv1.SubItems.Add(sb.ToString());
                       

                        method.downloadFile(pics[j].Groups[1].Value, path , titles[j].Groups[1].Value.Replace(":","").Replace(",","").Replace("\\", "").Replace("/", "").Replace(".", "") + ".jpg");

                    }



                }
            }

            catch (System.Exception ex)
            {

              MessageBox.Show( ex.ToString());
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            label3.Text = "已启动.........";

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Books_Load(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
