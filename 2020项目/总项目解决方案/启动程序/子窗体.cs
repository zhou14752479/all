using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 启动程序
{
    public partial class 子窗体 : Form
    {
        public 子窗体()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "Hm_lvt_fc2b90cbec55323dbc64f2b6400d86c7=1584760539; Hm_lpvt_fc2b90cbec55323dbc64f2b6400d86c7=1584761623";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.jzj9999.com/news/index.aspx";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);

                WebHeaderCollection headers = request.Headers;
                headers.Add("Sec-Fetch-Mode: cors");
                headers.Add("Sec-Fetch-Site: same-origin");
                headers.Add("X-Requested-With: XMLHttpRequest");

                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = 价格计算.value1;
            label3.Text = 价格计算.value2;
            label10.Text = 价格计算.value3;







            label4.Text = 价格计算.jieguo1.ToString();
            label5.Text = 价格计算.jieguo2.ToString();
            label6.Text = 价格计算.jieguo3.ToString();
            label7.Text = 价格计算.jieguo4.ToString();
            label8.Text = 价格计算.jieguo5.ToString();
            label9.Text = 价格计算.jieguo6.ToString();

            label1.Text = 价格计算.time+" 停盘";

            if (this.Top == 0)
            {
                linkLabel1.LinkColor = Color.Red;
            }
            else
            {
                linkLabel1.LinkColor = Color.DarkBlue;
            }
        }

        private void 子窗体_Load(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            //MessageBox.Show("本机器的分辨率是" + rect.Width.ToString() + "*" + rect.Height.ToString());
            this.Left = rect.Width - 235;
            this.Top = 0;
            this.TopMost = true;
            timer1.Start();

            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            //MessageBox.Show("本机器的分辨率是" + rect.Width.ToString() + "*" + rect.Height.ToString());
            this.Left = rect.Width- 235;
            this.Top = 0;
            this.TopMost = true;
         
        }



        private void 子窗体_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
         , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {




                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void 子窗体_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Red;
        }

        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            价格计算 js = new 价格计算();
            js.Show();
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
        private Point mPoint = new Point();
        private void 子窗体_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void 子窗体_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//最小化 
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (html.Contains(@"jiagejisuan"))
            {
                价格计算 js = new 价格计算();
                js.Show();
                js.getPrice();

                js.Hide();


            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
            
        }
    }
}
