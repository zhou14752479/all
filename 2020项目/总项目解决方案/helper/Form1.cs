using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace helper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string cookie = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            //method.SetWebBrowserFeatures(method.IeVersion.IE10);
            method.SetFeatures(8000);
            webBrowser1.ScriptErrorsSuppressed = true;
            
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cookie = method.GetCookies("https://biz-wb.jdwl.com/business/waybillmanage/index?isFromTab=1&deliveryId=&orderId=&beginTime=2020-07-18+00%3A00%3A00&endTime=2020-07-25+23%3A59%3A59&goodsType=&waybillType=&receiveName=&receiveMobile=&senderName=&senderMobile=&orgId=&orderStatusId=&isRefuse=&receiveCompany=&senderCompany=&preallocationValue=&pageSize=10&printSize=100*113&boxCode=&senderProvinceId=&senderCityId=&senderCountyId=");
            cookie = method.GetCookies("https://item.manager.taobao.com/taobao/manager/render.htm?tab=in_stock&table.sort.endDate_m=desc");
            this.Hide();

        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
