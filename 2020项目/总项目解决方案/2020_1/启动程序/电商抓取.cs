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

namespace 启动程序
{
    public partial class 电商抓取 : Form
    {
        public 电商抓取()
        {
            InitializeComponent();
        }
        string keyword = "";
        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 速卖通
        /// </summary>
        public void aliexpress()
        {
            


            try
            {
                string url = "https://www.aliexpress.com/wholesale?catId=0&initiative_id=SB_20200328235416&SearchText="+System.Web.HttpUtility.UrlEncode(keyword);

                string html = method.GetUrl(url, "utf-8");

                MatchCollection idss = Regex.Matches(html, @"""productId"":([\s\S]*?),");
               

                for (int j = 0; j < idss.Count; j++)
                {

                    string URL = "https://www.aliexpress.com/item/"+idss[j].Groups[1].Value+".html";
                    string strhtml = method.GetUrl(URL, "utf-8");

                    Match title = Regex.Match(strhtml, @"<title>([\s\S]*?)</title>");
                    Match weight = Regex.Match(strhtml, @"Item Weight([\s\S]*?)attrValue"":""([\s\S]*?)""");
                    Match application = Regex.Match(strhtml, @"subject"":""([\s\S]*?)""");
                    Match packagesize = Regex.Match(strhtml, @"formatedActivityPrice"":""([\s\S]*?)""");
                    Match price = Regex.Match(strhtml, @"formatedActivityPrice"":""([\s\S]*?)""");
                    Match No = Regex.Match(strhtml, @"Part No\.([\s\S]*?)attrValue"":""([\s\S]*?)""");
                    

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                   
                    lv1.SubItems.Add(title.Groups[1].Value);
                    lv1.SubItems.Add(weight.Groups[2].Value);
                    lv1.SubItems.Add(application.Groups[1].Value);
                    lv1.SubItems.Add(packagesize.Groups[1].Value);
                    lv1.SubItems.Add(No.Groups[2].Value);
                    lv1.SubItems.Add(price.Groups[1].Value);


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    if (status == false)

                        return;
                    Thread.Sleep(1000);
                }
              
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }


          
        }


        /// <summary>
        /// 亚马逊
        /// </summary>
        public void amazon()
        {



            try
            {
                string url = "https://www.amazon.com/s?k="+System.Web.HttpUtility.UrlEncode(keyword) + "&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";

                string html = method.GetUrl(url, "utf-8");

                MatchCollection idss = Regex.Matches(html, @"<div data-asin=""([\s\S]*?)""");
                
                for (int j = 0; j < idss.Count; j++)
                {

                    string URL = "https://www.amazon.com/-/zh/dp/" + idss[j].Groups[1].Value + "/ref=sr_1_1";
                    string strhtml = method.GetUrl(URL, "utf-8");

                    Match title = Regex.Match(strhtml, @"<title>([\s\S]*?)</title>");
                    Match weight = Regex.Match(strhtml, @"Item Weight([\s\S]*?)attrValue"":""([\s\S]*?)""");
                    Match application = Regex.Match(strhtml, @"subject"":""([\s\S]*?)""");
                    Match packagesize = Regex.Match(strhtml, @"Item Package Quantity([\s\S]*?)</td>");
                    Match price = Regex.Match(strhtml, @"data-asin-price=""([\s\S]*?)""");
                    Match No = Regex.Match(strhtml, @"零件编号([\s\S]*?)</td>");


                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   

                    lv1.SubItems.Add(title.Groups[1].Value);
                    lv1.SubItems.Add(weight.Groups[2].Value);
                    lv1.SubItems.Add(application.Groups[1].Value);
                    lv1.SubItems.Add(Regex.Replace(packagesize.Groups[1].Value, "<[^>]+>", "").Trim());
                    lv1.SubItems.Add(Regex.Replace(No.Groups[1].Value, "<[^>]+>", "").Trim());
                    lv1.SubItems.Add(price.Groups[1].Value);


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    if (status == false)

                        return;
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }



        }
        bool zanting = true;
        bool status = true;

        private void 电商抓取_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            keyword = textBox1.Text;

            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"aliexpress"))
            {
                status = true;
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(amazon));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                //Thread thread = new Thread(new ThreadStart(aliexpress));
                //thread.Start();
                //Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
