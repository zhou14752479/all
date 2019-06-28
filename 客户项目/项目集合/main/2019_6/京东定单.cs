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

namespace main._2019_6
{
    public partial class 京东定单 : Form
    {
        public 京东定单()
        {
            InitializeComponent();
        }

        private void 京东定单_Load(object sender, EventArgs e)
        {

        }

        public static string cookie = "";

        public void run()
        {
            try
            {
                for (int i = 1; i < 50; i++)
                {

                    string url = "https://order.jd.com/center/list.action?d=1&s=4096&page="+i;
                    string html = method.GetUrlWithCookie(url,cookie, "gb2312");
                    
                    MatchCollection ids = Regex.Matches(html, @"datasubmit-([\s\S]*?)""");
                    MatchCollection names = Regex.Matches(html, @" <span class=""txt"">([\s\S]*?)</span>");

                   
                    if (ids.Count == 0)
                        break;

                    for (int j = 0; j <ids.Count; j++)
                    {
                        textBox1.Text += "正在获取" + ids[j].Groups[1].Value+"信息" + "\r\n";
                        string wuliuURL = "https://details.jd.com/lazy/getOrderTrackInfoMultiPackage.action?orderId="+ids[j].Groups[1].Value;  //物流单号
                        string wuliuhtml = method.GetUrlWithCookie(wuliuURL, cookie, "utf-8");
                        Match danhao = Regex.Match(wuliuhtml, @"carriageId"":""([\s\S]*?)""");
                        textBox1.Text += "正在获取物流单号信息" + "\r\n";

                        string tuihuo = "https://myjd.jd.com/afs/ajax/getOrderAfsServiceOp.action?orderId=" + ids[j].Groups[1].Value;  //退货
                        string tuihuohtml = method.GetUrlWithCookie(tuihuo, cookie, "utf-8");
                        Match tui = Regex.Match(tuihuohtml, @"track"":""([\s\S]*?)""");

                        string tuihuoURL = "https://myjd.jd.com/afs/view/detailById.action?afsServiceId="+tui.Groups[1].Value;  //退货


                        textBox1.Text += "正在获取退货信息"+"\r\n";
                        string strhtml=  method.GetUrlWithCookie(tuihuoURL, cookie, "gb2312");


                        Match tuixinxi = Regex.Match(strhtml, @"<td>发货地址信息</td>([\s\S]*?)</tr>");

                       
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(names[j].Groups[1].Value);
                        listViewItem.SubItems.Add(danhao.Groups[1].Value);
                        listViewItem.SubItems.Add(tuixinxi.Groups[1].Value.Replace("<br />","").Replace("<strong>","").Replace("</strong>","").Replace("<td>","").Replace("</td>","").Replace(" ","").Trim());

                        textBox1.Text = "";

                    }

                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("https://home.jd.com/");
            web.Show();
        }

        private void SplitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            textBox2.Text = webBrowser.cookie;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
           cookie = textBox2.Text;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            textBox1.Text = "软件开始运行请勿重新点击" + "\r\n";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            #region 通用导出

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "1.1.1.1")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

            }
            else
            {
                MessageBox.Show("IP不符");
               
            }
            #endregion
        }
    }
}
