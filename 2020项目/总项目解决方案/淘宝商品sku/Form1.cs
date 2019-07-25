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
using 类库;


namespace 淘宝商品sku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string COOKIE;
        bool zanting = true;
        #region 宝贝监控
        public void run()
        {

            try
            {

                foreach (ListViewItem item in listView1.Items)
                {
                    string url = "http://shop.m.taobao.com/shop/shop_search.htm?q=" + item.SubItems[1].Text;

                    Match shopid = Regex.Match(method.gethtml(url, COOKIE, "utf-8"), @"shop_id=([\s\S]*?)""");

                    string Url = "https://shop" + shopid.Groups[1].Value + ".taobao.com/category.htm";

                    string html = method.gethtml(Url, COOKIE, "gb2312");
                    if (html == null)
                        break;
                    //Match title = Regex.Match(html, @"<span class=""shop-name-title"" title=""([\s\S]*?)""");  
                    Match midid = Regex.Match(html, @"mid=w-([\s\S]*?)-");

                    string curl = "https://shop" + shopid.Groups[1].Value + ".taobao.com/i/asynSearch.htm?_ksTS=1561163488418_160&callback=jsonp161&mid=w-" + midid.Groups[1].Value + "-0&wid=" + midid.Groups[1].Value + "&path=/search.htm&search=y&orderType=newOn_desc&viewType=grid&keyword=null&lowPrice=null&highPrice=null";


                    Match count = Regex.Match(method.gethtml(curl, COOKIE, "utf-8"), @"共搜索到<span>([\s\S]*?)</span>");

                   
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
    
                    lv1.SubItems.Add("");


                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(500);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。

                    }




                }
            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
