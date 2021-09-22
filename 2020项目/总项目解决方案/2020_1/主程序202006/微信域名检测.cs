using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202006
{
    public partial class 微信域名检测 : Form
    {
        public 微信域名检测()
        {
            InitializeComponent();
        }
        bool zanting = true;

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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.Headers.Add("Cookie", COOKIE);
              
                request.KeepAlive = true;
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
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < array.Length; i++)
            {
               
              string url1= "http://" + array[i];
                string url2 = "http://a." + array[i];

                string url = "http://mp.weixinbridge.com/mp/wapredirect?url=" + url1 + "&action=appmsg_redirect&uin=&biz=MzUxMTMxODc2MQ==&mid=100000007&idx=1&type=1&scene=0";
                string URL= "http://mp.weixinbridge.com/mp/wapredirect?url=" + url2 + "&action=appmsg_redirect&uin=&biz=MzUxMTMxODc2MQ==&mid=100000007&idx=1&type=1&scene=0";


                string html = GetUrl(url, "utf-8");
                string html2 = GetUrl(URL, "utf-8");

                label1.Text = "正在检测" + array[i];
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(array[i]);
               
                if (html.Contains("如需浏览") || html.Contains("停止访问"))
                {
                    lv1.SubItems.Add("屏蔽");
                    

                }
                else 
                {
                    lv1.SubItems.Add("正常");
                   
                }
                Random rd = new Random();
                int ym = rd.Next(99);
                lv1.SubItems.Add(ym+"." + array[i]);
                if (html2.Contains("如需浏览") || html2.Contains("停止访问"))
                {
                    lv1.SubItems.Add("屏蔽");


                }
                else
                {
                    lv1.SubItems.Add("正常");

                }



                if (listView1.Items.Count > 2)
                {
                    listView1.EnsureVisible(listView1.Items.Count - 1);
                }
               



                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }

            }




        }

    
        private void button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"weixinyuming"))
            {
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 微信域名检测_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
