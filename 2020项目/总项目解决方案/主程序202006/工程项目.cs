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

namespace 主程序202006
{
    public partial class 工程项目 : Form
    {
        public 工程项目()
        {
            InitializeComponent();
        }
        bool zanting = true;

        /// <summary>
        public void run()
        {
            string begindate = DateTime.Now.AddDays(-9).ToString("yyyy-mm-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-mm-dd");
            for (int i = 1; i < 314; i++)
            {
                string url = "http://deal.ggzy.gov.cn/ds/deal/dealList_find.jsp";
                string postdata = "TIMEBEGIN_SHOW="+begindate+ "&TIMEEND_SHOW=" + enddate + "&TIMEBEGIN=" + begindate + "&TIMEEND=" + enddate + "&SOURCE_TYPE=1&DEAL_TIME=02&DEAL_CLASSIFY=01&DEAL_STAGE=0100&DEAL_PROVINCE=0&DEAL_CITY=0&DEAL_PLATFORM=0&BID_PLATFORM=0&DEAL_TRADE=0&isShowAll=1&PAGENUMBER="+i+"&FINDTXT=";
                MessageBox.Show(postdata);
                string html = method.PostUrl(url,postdata,"", "utf-8");

                MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                MatchCollection diqu = Regex.Matches(html, @"""districtShow"":""([\s\S]*?)""");
                MatchCollection type = Regex.Matches(html, @"""classifyShow"":""([\s\S]*?)""");
                MatchCollection stageShow = Regex.Matches(html, @"""stageShow"":""([\s\S]*?)""");
                MatchCollection timeShow = Regex.Matches(html, @"""timeShow"":""([\s\S]*?)""");
                MatchCollection urls = Regex.Matches(html, @"""url"":""([\s\S]*?)""");
              


                for (int a = 0; a < titles.Count; a++)

                {

                    try
                    {



                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(titles[a].Groups[1].Value);
                        lv1.SubItems.Add(diqu[a].Groups[1].Value);
                        lv1.SubItems.Add(type[a].Groups[1].Value);
                        lv1.SubItems.Add(stageShow[a].Groups[1].Value);
                        lv1.SubItems.Add(timeShow[a].Groups[1].Value);
                        lv1.SubItems.Add(urls[a].Groups[1].Value);

                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                    }
                    catch
                    {

                        continue;
                    }

                   

                }
                Thread.Sleep(1000);



            }
        }
        private void 工程项目_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
