using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
namespace 主程序202005
{
    public partial class 商品监控 : Form
    {
        public 商品监控()
        {
            InitializeComponent();
        }
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);
        #region GET请求解决基础连接关闭无法获取HTML
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string outStr = "";
            string tmpStr = "";

            try
            {
                 System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "waf_cookie=34777c9d-f2d2-4c98f977e05c45345dd7c1cc7657feefcde1; _ydclearance=d144fac6c93ec1bcb254b265-2c35-4377-8ad8-eb1debda1293-1589109047; PHPSESSID=l1hl9vmg3t4epj5cq93q16pb17; XSRF-TOKEN=eyJpdiI6IktEUitiODQzRGVJSGFOSVRBWHFzTWc9PSIsInZhbHVlIjoiZmtoR0UzMUQweVVSc1R4MVMwSEw4VXBIalpvb056RVpcL2NReGJJamtiSmo1T1wvN3BXb0NScjNTcXZvQm04cVFZIiwibWFjIjoiZTZlODJmMDBlZWU2OGIwODdlZDI3NGU4YzY3YTY2ZDg4MWYzMTVkOWZkNjhhZTBhN2YyODQ5YjYwMDIwNmEyZCJ9; sdfaka_pro_session=eyJpdiI6InY0SU5tV0FGVnNcL3F6bnU1bVJiR0FRPT0iLCJ2YWx1ZSI6ImhVbFZjYUV5Ukk3bTNUejFZUVhZbjZCOEJ6VzFJSGtHSnFcL2EzRUw4SzZraUNENzNmb1IyaWpkbGtKdkJDQ3B1IiwibWFjIjoiYzdhZWM0ODJjODQwNWJiOWFlZWM5ZDBjMGY1YWMyYTE0NzI2ODhkNTJkOGYxNmE2N2JmNGYxNDk5Y2IyODM2MSJ9";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "http://www.maca365.cn/trade/103.html";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                try
                {//循环获取
                    while ((tmpStr = reader.ReadLine()) != null)
                    {
                        outStr += tmpStr;
                    }
                }
                catch
                {

                }
                reader.Close();
                response.Close();

                return outStr;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return ex.ToString();

            }

        }
        #endregion

        private void 商品监控_Load(object sender, EventArgs e)
        {

        }

        public void run()
        {
            string url = "http://www.maca365.cn/trade/103.html";
            string html = GetUrl(url,"utf-8");

            if (!html.Contains("库存不足"))
            {
                for (int i = 0; i < 20; i++)
                {
                    Beep(800, 300);
                }

            }
            else
            {
                textBox2.Text += DateTime.Now.ToString() + " ：" + "库存不足.." + "\r\n";
                if (textBox2.Lines.Length == 10)
                {
                    textBox2.Text = "";
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox1.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
