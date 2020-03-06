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

namespace 主程序
{
    public partial class 淘宝阿里商品 : Form
    {
        public 淘宝阿里商品()
        {
            InitializeComponent();
        }
        public static string COOKIE = "";
        #region 淘宝
        public void taobao(string url)
        {

            try
            {

                string html = method.GetUrlWithCookie(url, COOKIE,"gbk");
                
                Match name = Regex.Match(html, @"<title>([\s\S]*?)-");

                Match main = Regex.Match(html, @"skuList([\s\S]*?)salesProp");
                string zhu = main.Groups[1].Value;


                MatchCollection xuanxiangs = Regex.Matches(zhu, @"""names"":""([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(zhu, @"""price"":""([\s\S]*?)""");

                MatchCollection skus = Regex.Matches(zhu, @"""skuId"":""([\s\S]*?)""");



                for (int i = 0; i < xuanxiangs.Count; i++)
                {
                    string[] text = xuanxiangs[i].Groups[1].Value.Split(new string[] { " " }, StringSplitOptions.None);



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(name.Groups[1].Value);
                    lv1.SubItems.Add(text[0]);
                    lv1.SubItems.Add(text[1]);
                    lv1.SubItems.Add(prices[i].Groups[1].Value);
                    lv1.SubItems.Add(skus[i].Groups[1].Value);
                }





            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region 阿里巴巴
        public void alibab(string url)
        {

            try
            {

                string html = method.gethtml(url, COOKIE);
                Match name = Regex.Match(html, @"<h1 class=""d-title"">([\s\S]*?)</h1>");

                Match  main = Regex.Match(html, @"skuMap:\{([\s\S]*?)end");
                string zhu = "}," + main.Groups[1].Value;


                MatchCollection xuanxiangs = Regex.Matches(zhu, @"\},""([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(zhu,@"""price"":""([\s\S]*?)""");
                
                MatchCollection skus = Regex.Matches(zhu, @"""skuId"":([\s\S]*?)\}");
              

              
                for (int i = 0; i < xuanxiangs.Count; i++)
                {
                    string[] text = xuanxiangs[i].Groups[1].Value.Split(new string[] { "&gt;" }, StringSplitOptions.None);



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(name.Groups[1].Value);
                    lv1.SubItems.Add(text[0]);
                    lv1.SubItems.Add(text[1]);
                    lv1.SubItems.Add(prices[i].Groups[1].Value);
                    lv1.SubItems.Add(skus[i].Groups[1].Value);
                }





            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        private void 淘宝阿里商品_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                taobao(textBox1.Text);
            }
            else if(radioButton2.Checked == true)
            {
                alibab(textBox2.Text);

            }
           
           
           
        }
    }
}
