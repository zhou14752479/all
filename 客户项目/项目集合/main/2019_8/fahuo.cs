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

namespace main._2019_8
{
    public partial class fahuo : Form
    {
        public fahuo()
        {
            InitializeComponent();
        }
        ArrayList lists = new ArrayList();
        public static string COOKIE1;
        public static string COOKIE2;

        public static string yuming= "www.hiyunyin.com";
        /// <summary>
        /// 录入到发货易
        /// </summary>
        public void run()
        {
            COOKIE1 = textBox2.Text;
            COOKIE2 = textBox3.Text;
            try
            {
                for (int a = 1; a < 5; a++)
                {

                    string url = "http://"+yuming+"/admin/orders/defaults.html?orderId=&productText=&receiverName=&orderStatus=&page=" + a;
                    string html = method.GetUrlWithCookie(url, COOKIE1, "utf-8");

                    MatchCollection fahuos = Regex.Matches(html, @"layui-btn-mini"">([\s\S]*?)</a>");
                    MatchCollection dingdans = Regex.Matches(html, @"<td>6\d{9}");
                    MatchCollection uids = Regex.Matches(html, @"name=""ids\[\]"" value=""([\s\S]*?)""");
                    MatchCollection bodys = Regex.Matches(html, @"relative;"">([\s\S]*?)</td>");


                    for (int i = 0; i < fahuos.Count; i++)
                    {

                        string dingdan = dingdans[i].Groups[0].Value.Replace("<td>", "");
                        string uid = uids[i].Groups[1].Value;
                        if (!lists.Contains(dingdan + "-" + uid))
                        {

                            lists.Add(dingdan + "-" + uid);
                        }



                        string value = bodys[i].Groups[1].Value.Replace("<br/>", "");
                        string api1 = "http://a22.fahuoyi.com/order/recognizeConsigneeInfo?consigneeInfo=" + value;
                        string ahtml = method.GetUrlWithCookie(api1, COOKIE2, "utf-8");
                        Match a1 = Regex.Match(ahtml, @"""consigneeName"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(ahtml, @"""mobile"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(ahtml, @"""provinceCode"":([\s\S]*?),");
                        Match a4 = Regex.Match(ahtml, @"""cityCode"":([\s\S]*?),");
                        Match a5 = Regex.Match(ahtml, @"""districtCode"":([\s\S]*?),");
                        Match a6 = Regex.Match(ahtml, @"""street"":""([\s\S]*?)""");

                        string name = System.Web.HttpUtility.UrlEncode(a1.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                        string mobile = System.Web.HttpUtility.UrlEncode(a2.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                        string provinceCode = System.Web.HttpUtility.UrlEncode(a3.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                        string cityCode = System.Web.HttpUtility.UrlEncode(a4.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                        string districtCode = System.Web.HttpUtility.UrlEncode(a5.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                        string street = System.Web.HttpUtility.UrlEncode(a6.Groups[1].Value, Encoding.GetEncoding("utf-8"));

                        string data = "shopId=234605&orderJson=%7B%22consigneeName%22%3A%22" + name + "%22%2C%22mobile%22%3A%22" + mobile + "%22%2C%22telephone%22%3A%22%22%2C%22provinceCode%22%3A%22" + provinceCode + "%22%2C%22cityCode%22%3A%22" + cityCode + "%22%2C%22districtCode%22%3A%22" + districtCode + "%22%2C%22street%22%3A%22" + street + "%22%2C%22zipCode%22%3A%22%22%2C%22shippingCost%22%3A%220%22%2C%22weight%22%3A%220%22%2C%22buyerUsername%22%3A%22%22%2C%22originalId%22%3A%22" + dingdan + "%22%2C%22buyerRemarks%22%3A%22%22%2C%22sellerRemarks%22%3A%22%22%2C%22codAmounts%22%3A0%2C%22orderItems%22%3A%5B%5D%7D&frequentConsigneeId=";

                        string api2 = "http://a22.fahuoyi.com/manualOrder/saveRest?" + data;
                        string bhtml = method.GetUrlWithCookie(api2, COOKIE2, "utf-8");



                        if (!bhtml.Contains("已存在"))
                        {
                            textBox1.Text += DateTime.Now.ToString() + "\r\n" + bhtml + "\r\n";
                            textBox1.Text += DateTime.Now.ToString() + "\r\n" + "录入成功！" + "\r\n";




                        }
                        //else
                        //{

                        //    textBox1.Text += DateTime.Now.ToString() + "\r\n" + "没有新订单需要录入发货易" + "\r\n";
                        //    Thread.Sleep(1000);
                        //}
                        Thread.Sleep(500);

                    }


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            

        }

       

        /// <summary>
        /// 快递发货
        /// </summary>
        public void run1()
        {
            COOKIE1 = textBox2.Text;
            COOKIE2 = textBox3.Text;
            try
            {
                if (lists.Count > 0)
                {
                   
                    foreach (string list in lists)
                    {
                        string[] text = list.Split(new string[] { "-" }, StringSplitOptions.None);

                        string curl = "http://a22.fahuoyi.com/order/searchRest?originalId=" + text[0].ToString() + "&isHistoryOrder=false";
                        string chtml = method.GetUrlWithCookie(curl, COOKIE2, "utf-8"); //获取快递单号
                        Match kuaidi = Regex.Match(chtml, @"""waybillNo"":""([\s\S]*?)""");


                        if (kuaidi.Groups[1].Value != "")
                        {
                            string durl = "http://" + yuming + "/admin/orders/save.html?kd=" + kuaidi.Groups[1].Value.Trim() + "&id=" + text[1].ToString();
                            string dhtml = method.GetUrlWithCookie(durl, COOKIE1, "utf-8"); //录入快递单号 发货


                            textBox1.Text += DateTime.Now.ToString() + "\r\n" + "快递单号录入成功，发货成功！" + "\r\n";
                            lists.Remove(list);

                        }
                        else
                        {
                            textBox1.Text += DateTime.Now.ToString() + "\r\n" + "厂家快递还未发货！" + "\r\n";
                        }

                        Thread.Sleep(1000);
                       
                    }


                }

                else
                {
                    textBox1.Text += DateTime.Now.ToString() + "\r\n" + "暂无厂家发货的快递单号需要录入系统！" + "\r\n";

                }

   


            }
            catch (Exception ex)
            {

                ex.ToString();
            }


        }




        private void Fahuo_Load(object sender, EventArgs e)
        {
           
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("请先登录");
                return;
            }
            
            
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "17.17.17.17")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Thread thread1 = new Thread(new ThreadStart(run1));
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                timer1.Interval = (Convert.ToInt32(textBox4.Text)) * 1000 * 60;
                timer1.Start();


            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
          
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = "";
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

            Thread thread1 = new Thread(new ThreadStart(run1));
            thread1.Start();
           
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("http://" + yuming + "/admin/orders/defaults.html?orderId=&productText=&receiverName=&orderStatus=");
            web.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("http://a22.fahuoyi.com/order/index?shopId=228152&tab=2&autoSyncOrder=true");
            web.Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = webBrowser.cookie;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            textBox3.Text = webBrowser.cookie;
        }

        private void Fahuo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                notifyIcon1.Visible = true;
                this.Hide();
                return;
            }
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
