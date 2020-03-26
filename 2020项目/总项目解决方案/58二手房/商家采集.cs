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

namespace _58二手房
{
    public partial class 商家采集 : Form
    {
        public 商家采集()
        {
            InitializeComponent();
        }
        bool zanting = true;
        bool status = true;


        private void 商家采集_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            try
            {
                string url = "http://api.map.baidu.com/geocoder?address="+ System.Web.HttpUtility.UrlEncode(textBox1.Text) + "&output=json&key=37492c0ee6f924cb5e934fa08c6b1676";


                Match lon = Regex.Match(method.GetUrl(url, "utf-8"), @"""lng"":([\s\S]*?),");
                Match lat = Regex.Match(method.GetUrl(url, "utf-8"), @"""lat"":([\s\S]*?)\}");

               
               
                string URL = "https://yaofa.58.com/search/card?xei4i=undefined&latitude="+lat.Groups[1].Value.Trim()+ "&longitude=" + lon.Groups[1].Value.Trim() + "&size=9999";

                string html = method.GetUrl(URL, "utf-8");

                MatchCollection names = Regex.Matches(html, @"""company"":""([\s\S]*?)""");
                MatchCollection mobiles = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                MatchCollection realNames = Regex.Matches(html, @"""realName"":""([\s\S]*?)""");
                MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");


                for (int i = 0; i < names.Count; i++)
                {

                  

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(names[i].Groups[1].Value);
                    lv1.SubItems.Add(mobiles[i].Groups[1].Value);
                    lv1.SubItems.Add(realNames[i].Groups[1].Value);
                    lv1.SubItems.Add(address[i].Groups[1].Value);





                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }
                    if (status == false)
                        return;


                }


            }


            catch (Exception)
            {

                throw;
            }
        }
            private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"58shangjia"))
            {
                status = true;
                button1.Enabled = false;
                zanting = true;
                Thread thread1 = new Thread(new ThreadStart(run));
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
