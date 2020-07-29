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

namespace 主程序202007
{
    public partial class 京东物流 : Form
    {
        public 京东物流()
        {
            InitializeComponent();
        }

        string cookie = "";

        public void run()
        {
            cookie = Form1.cookie;
           
            label4.Text = "正在查询";
            string begintime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            for (int i = 1; i < 999; i++)
            {

             

                string url = "https://biz-wb.jdwl.com/business/waybillmanage/index?isFromTab=1&deliveryId=&orderId=&beginTime=" + begintime + "+00%3A00%3A00&endTime=" + endtime + "+23%3A59%3A59&receiveName=&receiveMobile=&receiveCompany=&senderName=&senderMobile=&senderCompany=&boxCode=&orderStatusId=&orderState=&cancelKeys=&deliveryI_h=&printSize=100*113&printType=&goodsType=&waybillType=&isRefuse=&orgId=&preallocationValue=&senderProvinceId=&senderCityId=&senderCountyId=&curPage=" + i+"&pageSize=10";
                string html = method.GetUrlWithCookie (url,cookie ,"utf-8");

                
                MatchCollection uids = Regex.Matches(html, @"data-index=""([\s\S]*?)""");
                
                if (uids.Count == 0)
                {
                    MessageBox.Show("抓取结束");
                    label4.Text = "抓取结束";
                    break;
                }

                for (int j = 0; j < uids.Count; j++)
                {
                    label4.Text = "正在查询......."+ uids[j].Groups[1].Value;
                    string aurl = "https://biz-wb.jdwl.com/business/waybillmanage/toDeliveryDetail?orderStatusId=&orderState=&cancelKeys=&deliveryI_h=&detailDeliveryId=" + uids[j].Groups[1].Value;
                    string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");

                    Match zhongliang = Regex.Match(ahtml, @"重量：([\s\S]*?)kg");

                    double value = Convert.ToDouble(textBox1.Text);
                    double zhong = Convert.ToDouble(zhongliang.Groups[1].Value);
                    if (zhong > value)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(uids[j].Groups[1].Value);
                        lv1.SubItems.Add(zhongliang.Groups[1].Value);

                        Thread.Sleep(500);

                    }
                }

                    
                }
            
  
        

        }


        public void run1()
        {
            string[] pinpais = {"%e7%b1%b3%e5%85%b6%e6%9e%97",
"%e9%a9%ac%e7%89%8c",
"%e5%80%8d%e8%80%90%e5%8a%9b",
"%e5%9b%ba%e7%89%b9%e5%bc%82",
"%e6%99%ae%e5%88%a9%e5%8f%b8%e9%80%9a",
"%e9%82%93%e7%a6%84%e6%99%ae",
"%e6%a8%aa%e6%bb%a8",
"%e7%8e%9b%e5%90%89%e6%96%af",
"%e9%9f%a9%e6%b3%b0",
"%e7%99%be%e8%b7%af%e9%a9%b0",
"%e5%9b%ba%e9%93%82",
"%e9%94%a6%e6%b9%96",
"%e4%bd%b3%e9%80%9a",
"%e4%b8%9c%e6%b4%8b",
"%e8%80%90%e5%85%8b%e6%a3%ae",
"%e8%80%90%e7%89%b9%e9%80%9a"};

            string[] chicuns = { "12", "13", "14", "15", "16",  "17",  "18",  "19", "20", "21", "22", "23", "24"};


            for (int i = 0; i < pinpais.Length; i++)
            {
                for (int j = 0; j < chicuns.Length; j++)
                {

                    for (int a = 0; a < 21; a++)
                    {



                        string url = "https://www.fortire.cn/wap1/sp_list.asp?page=3&khmc=&spmc=&x1=&x2=&x3=" + chicuns[j] + "&z=0&zero1=0&zero2=0";



                        string html = method.GetUrlWithCookie(url, cookie, "utf-8");


                        MatchCollection uids = Regex.Matches(html, @"data-index=""([\s\S]*?)""");

                        if (uids.Count == 0)
                        {

                            break;
                        }

                       


                    }
                }
            }


        }
        private void 京东物流_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"jdwl"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();

        }
    }
}
