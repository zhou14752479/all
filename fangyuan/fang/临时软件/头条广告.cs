using System;
using System.Collections;
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

namespace fang.临时软件
{
    public partial class 头条广告 : Form
    {
        public 头条广告()
        {
            InitializeComponent();
        }

        #region  主函数
        public void run()
        {


            try
            {



                for (int i = 5; i < 16; i++)
                {

                    string url = "https://www.toutiao.com/api/search/content/?aid=24&offset=" + i + "0&format=json&keyword=%E8%B4%A7%E5%88%B0%E4%BB%98%E6%AC%BE&autoload=true&count=20&cur_tab=1&from=search_tab&pd=synthesis";

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection urls = Regex.Matches(html, @"article_url"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in urls)
                    {
                        lists.Add(NextMatch.Groups[1].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;
                    foreach (string list in lists)
                    {

                        string html2 = method.GetUrl(list, "utf-8");

                        MatchCollection urls2 = Regex.Matches(html2, @"shopgoodsid=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists2 = new ArrayList();

                        foreach (Match NextMatch in urls2)
                        {
                            lists2.Add("https://haohuo.snssdk.com/product/ajaxstaticitem?id=" + NextMatch.Groups[1].Value);

                        }
                        foreach (string list2 in lists2)
                        {
                            
                            //联系人网址与其他网址不同
                            string strhtml = method.GetUrl(list2, "utf-8");
                            string str= System.Web.HttpUtility.UrlEncode(strhtml, System.Text.Encoding.GetEncoding("gb2312"));
                            textBox1.Text = str;
                            Match title = Regex.Match(strhtml, @"name"":""([\s\S]*?)""");
                            Match company = Regex.Match(strhtml, @"company_name"":""([\s\S]*?)""");
                            Match tell = Regex.Match(strhtml, @"shop_tel"":""([\s\S]*?)""");
                            Match num = Regex.Match(strhtml, @"sell_num"":([\s\S]*?),");

                            if (title.Groups[1].Value != "")
                            {

                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(Regex.Unescape(title.Groups[1].Value));
                                lv1.SubItems.Add(Regex.Unescape(company.Groups[1].Value));
                                lv1.SubItems.Add(tell.Groups[1].Value);
                                lv1.SubItems.Add(num.Groups[1].Value);


                                Application.DoEvents();
                                Thread.Sleep(Convert.ToInt32(100));



                                if (listView1.Items.Count > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                                }

                                //while (this.status == false)
                                //{
                                //    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                //}
                            }


                        }



                    }

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
