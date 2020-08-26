using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using helper;

namespace 主程序202008
{
    public partial class 持续获取下载 : Form
    {
        public 持续获取下载()
        {
            InitializeComponent();
        }
        private string GetHttp20200813170758()
        {
           HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://exx.goodsinfo.hscode.net/",
                Method = "POST",
                Host = "exx.goodsinfo.hscode.net",
                ContentType = "application/x-www-form-urlencoded",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                Referer = "http://exx.goodsinfo.hscode.net/",
                Cookie = "Hm_lvt_893bf7e562b2ec1f97bebc97e62cf2e8=1597231320; _qddaz=QD.j2y0li.gpi2kw.kdraaeee; ASP.NET_SessionId=04putkwh0rfwfa1om0u4o1jh",
                PostEncoding = Encoding.UTF8,
                Postdata = "txtUserName="+ System.Web.HttpUtility.UrlEncode(textBox1.Text)+ "&txtPassword="+ System.Web.HttpUtility.UrlEncode(textBox2.Text)  +"&submit=%E7%99%BB%C2%A0%C2%A0%E5%BD%95"
            };
            item.Header.Add("Accept-Encoding", "gzip, deflate");
            item.Header.Add("Accept-Language", "zh-CN,zh;q=0.9");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            cookie = result.Cookie;
            
            if (cookie != null)
            {
                MessageBox.Show("登录成功");
            }
            else
            {
                MessageBox.Show("登录失败");
            }
                return cookie;
           
        }

        string cookie = "";
        private void button1_Click(object sender, EventArgs e)
        {
            GetHttp20200813170758();
          
            
        }


        public void run()
        {
            string url = "http://exx.goodsinfo.hscode.net/Ebay/GetTranslateTask";
            string html = method.PostUrl(url,"",cookie,"utf-8");
           
            textBox3.Text = html;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ceshi1111111"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            if (cookie == "")
            {
                MessageBox.Show("请先登录");
                return;
            }

            timer1.Start();
            timer2.Start();
            textBox3.Text = "正在下载...";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            run();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            label3.Text = "已停止";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString();
        }
    }
}
