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

namespace 常用代码管理软件
{
    public partial class 点评评论 : Form
    {
        public 点评评论()
        {
            InitializeComponent();
        }

        public static string COOKIE= "_lxsdk_cuid=16cb6dd4ef5c8-0ab1639fef8078-f353163-1fa400-16cb6dd4ef5c8; _lxsdk=16cb6dd4ef5c8-0ab1639fef8078-f353163-1fa400-16cb6dd4ef5c8; _hc.v=b4c63e9f-f96f-ca4a-2ad0-dfa8208e6726.1566436056; cy=100; cye=suqian; s_ViewType=10; dper=0f2c70b22cd0a4b60f7c81b238310b7fd64ab7b3d48c3d4860c0025c016df65f8e5c74a50efbdfa99009933d4ba0f5343595a6175a3df0df9b1ee9f00d2ed39bf4a4b42c7448bcae3ba26ef811de8529ad3679fb33278bf54fa9fc1e7d2c4363; ll=7fd06e815b796be3df069dec7836c3df; ua=dpuser_5678141658; ctu=90a81cde43e1e0934a456ec54b747c9309c7243af5f8f610acaafc50d303f141; uamo=17606117606; cityid=100; switchcityflashtoast=1; source=m_browser_test_33; default_ab=shop%3AA%3A5%7Cindex%3AA%3A1%7CshopList%3AC%3A4; _lxsdk_s=16dd765a208-d3c-d3a-5fb%7C%7C183";

        public void run()
        {
            for (int i = 2; i < 3; i++)
            {
                Match uid = Regex.Match(textBox1.Text,@"\d{6,}");
               
                string url = "http://www.dianping.com/shop/"+uid.Groups[0].Value+"/review_all/p"+i;
                string html = method.GetUrlWithCookie(url, COOKIE, "utf-8");
                MatchCollection view1s = Regex.Matches(html, @"<div class=""review-words"">([\s\S]*?)</div>");
                MatchCollection views2s = Regex.Matches(html, @"<div class=""review-words Hide"">([\s\S]*?)<div class=""less-words"">");
                foreach (Match view1 in view1s)
                {

                    MatchCollection svg = Regex.Matches(view1.Groups[1].Value, @"<.*?>");
                    for (int x = 0; x < svg.Count; x++)
                    {
                        string value = view1.Groups[1].Value.Replace(svg[x].Groups[1].Value,"");
                    }
                  

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add("111");
                    
                }
            }
           

        }


        public string getSVG()
        {
            Match uid = Regex.Match(textBox1.Text, @"\d{6,}");

            return "";
        }

        private void 点评评论_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            run();
        }
    }
}
