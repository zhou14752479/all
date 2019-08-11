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
using helper;

namespace 拼多多
{
    public partial class pdd : Form
    {
        public pdd()
        {
            InitializeComponent();
        }

        public static string COOKIE;
        #region 获取店铺内宝贝
        public void run1()
        {

            try
            {
                for (int i = 1; i < 999; i++)
                {


                    Match shopid = Regex.Match("", @"https:\/\/([\s\S]*?)\.");
                    string Url = "https://" + shopid.Groups[1].Value + ".taobao.com/category.htm";
                    string html = method.gethtml(Url, COOKIE);

                    Match title = Regex.Match(html, @"<span class=""shop-name-title"" title=""([\s\S]*?)""");
                    Match midid = Regex.Match(html, @"mid=w-([\s\S]*?)-");

                    string curl = "https://" + shopid.Groups[1].Value + ".taobao.com/i/asynSearch.htm?_ksTS=1561163488418_160&callback=jsonp161&mid=w-" + midid.Groups[1].Value + "-0&wid=" + midid.Groups[1].Value + "&path=/search.htm&search=y&orderType=newOn_desc&viewType=grid&keyword=null&lowPrice=null&highPrice=null&pageNo=" + i;


                    MatchCollection names = Regex.Matches(method.gethtml(curl, COOKIE), @"<img alt=\\""([\s\S]*?)\\");
                    MatchCollection uids = Regex.Matches(method.gethtml(curl, COOKIE), @"data-id=\\""([\s\S]*?)\\");

                    if (method.gethtml(curl, COOKIE).Contains("很抱歉"))
                        break;

                    for (int j = 0; j < names.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(names[j].Groups[1].Value);
                        lv1.SubItems.Add("https://item.taobao.com/item.htm?id=" + uids[j].Groups[1].Value);
                    }




                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(2000);


                }

            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        private void Pdd_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            //method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

        }
    }
}
