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

        public static string COOKIE1;
        public static string COOKIE2;
        public void run()
        {
            COOKIE1 = textBox2.Text;
            COOKIE2 = textBox3.Text;
            try
            {
                ArrayList daqus = new ArrayList();
                string url = "http://www.cmykjx.com/admin/orders/defaults.html?orderId=&productText=&receiverName=&orderStatus=";
                string html = method.GetUrlWithCookie(url, COOKIE1, "utf-8");
                
                MatchCollection dingdans = Regex.Matches(html, @"<td>6\d{9}");
                MatchCollection  bodys = Regex.Matches(html, @"relative;"">([\s\S]*?)</td>");

                for (int i = 0; i < bodys.Count; i++)
                {
                    string dingdan = dingdans[i].Groups[0].Value.Replace("<td>","");
                    string value = bodys[i].Groups[1].Value.Replace("<br/>", "");
                    string api1 = "http://a22.fahuoyi.com/order/recognizeConsigneeInfo?consigneeInfo="+ value;
                    string ahtml = method.GetUrlWithCookie(api1, COOKIE2, "utf-8");
                    Match a1 = Regex.Match(ahtml, @"""consigneeName"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(ahtml, @"""mobile"":""([\s\S]*?)""");
                    Match a3 = Regex.Match(ahtml, @"""provinceCode"":([\s\S]*?),");
                    Match a4 = Regex.Match(ahtml, @"""cityCode"":([\s\S]*?),");
                    Match a5 = Regex.Match(ahtml, @"""districtCode"":([\s\S]*?),");
                    Match a6 = Regex.Match(ahtml, @"""street"":""([\s\S]*?)""");

                    string name= System.Web.HttpUtility.UrlEncode(a1.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                    string mobile = System.Web.HttpUtility.UrlEncode(a2.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                    string provinceCode = System.Web.HttpUtility.UrlEncode(a3.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                    string cityCode = System.Web.HttpUtility.UrlEncode(a4.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                    string districtCode = System.Web.HttpUtility.UrlEncode(a5.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                    string street = System.Web.HttpUtility.UrlEncode(a6.Groups[1].Value, Encoding.GetEncoding("utf-8"));
                  
                    string data = "shopId=228152&orderJson=%7B%22consigneeName%22%3A%22"+name+"%22%2C%22mobile%22%3A%22"+mobile+"%22%2C%22telephone%22%3A%22%22%2C%22provinceCode%22%3A%22"+provinceCode+"%22%2C%22cityCode%22%3A%22"+cityCode+"%22%2C%22districtCode%22%3A%22"+districtCode+"%22%2C%22street%22%3A%22"+street+"%22%2C%22zipCode%22%3A%22%22%2C%22shippingCost%22%3A%220%22%2C%22weight%22%3A%220%22%2C%22buyerUsername%22%3A%22%22%2C%22originalId%22%3A%22"+dingdan+"%22%2C%22buyerRemarks%22%3A%22%22%2C%22sellerRemarks%22%3A%22%22%2C%22codAmounts%22%3A0%2C%22orderItems%22%3A%5B%5D%7D&frequentConsigneeId=";

                    string api2 = "http://a22.fahuoyi.com/manualOrder/saveRest?" + data;
                    string bhtml = method.GetUrlWithCookie(api2, COOKIE2, "utf-8");

                   
                    textBox1.Text += DateTime.Now.ToString()+ "\r\n" + bhtml + "\r\n";
                    Thread.Sleep(2000);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

            

        
        private void Fahuo_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "9.9.9.9")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
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
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
