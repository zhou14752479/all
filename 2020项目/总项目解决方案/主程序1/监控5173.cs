using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序1
{
    public partial class 监控5173 : Form
    {
        public 监控5173()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            try
            {
                string url = "http://yxbmall.5173.com/gamegold-facade-frontend/services/goods/QueryAllCategoryGoods?t=1588512625251&callback=jQuery1520847470474957774_1588512624480&currentUrl=http%3A%2F%2Fs.5173.com%2Fsearch%2F858f058e63e74156a1d4dcf3239df20c-ahp3ma-205jbk-duj53o-sxldtr-wr1n3g-0-0-0-a-a-a-a-a-0-0-0-0.shtml&_=1588512625252";
                string html = GetUrl(url, "utf-8");

                Match price = Regex.Match(html, @"""unitPrice"":([\s\S]*?),");
                Match count = Regex.Match(html, @"""sellableCount"":([\s\S]*?),");
                if (price.Groups[1].Value.Trim() != textBox2.Text.Trim())
                {
                    MessageBox.Show("出现价格变动");
                    timer1.Stop();
                }

                if (count.Groups[1].Value.Trim() != textBox3.Text.Trim())
                {
                    MessageBox.Show("出现库存变动");
                    timer1.Stop();
                }
                textBox1.Text = DateTime.Now.ToString() + "：" + "\r\n" + "\r\n";

                textBox1.Text += "当前价格： "+ price.Groups[1].Value+ "\r\n" + "\r\n";

                

                textBox1.Text +="当前库存： " + count.Groups[1].Value+ "\r\n";

              
            }


            catch (Exception)
            {

                throw;
            }
        }
        private void 监控5173_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"5173"))
            {
                
                Thread thread1 = new Thread(new ThreadStart(run));
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                timer1.Start();
                timer1.Interval = Convert.ToInt32(textBox4.Text)*1000;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            run();
        }
    }
}
