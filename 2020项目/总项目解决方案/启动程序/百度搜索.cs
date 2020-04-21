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
using helper;

namespace 启动程序
{
    public partial class 百度搜索 : Form
    {
        public 百度搜索()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

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
               // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "BIDUPSID=921AE8F8E5F0D4E93DDEF4BD9A1F627E; PSTM=1517917678; MCITY=277-277%3A; BD_UPN=12314753; BAIDUID=D28ED8EAA7251A9D96133C7D30DCA04B:FG=1; BDUSS=pyLUZVfk0zUmk1dTdMd2ptUFVsMkF-MmhofkRnV1V3RktiUkZpWjVaU2VGcTVlSUFBQUFBJCQAAAAAAAAAAAEAAABVvFgjztK0-MnPzqjSuwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJ6Jhl6eiYZeT3; BDUSS_BFESS=pyLUZVfk0zUmk1dTdMd2ptUFVsMkF-MmhofkRnV1V3RktiUkZpWjVaU2VGcTVlSUFBQUFBJCQAAAAAAAAAAAEAAABVvFgjztK0-MnPzqjSuwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJ6Jhl6eiYZeT3; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; indexpro=pmmq22dmmkpavn1tcudgtfjnj7; bdindexid=34okn15klp0do5mlpaggj6icb6; yjs_js_security_passport=068ae83ec78197dd658b5d32c582df7118c33e97_1586506470_js; H_PS_PSSID=1455_31169_21090_31254_31187_30905_31217_30824_31085_31164_22157; H_PS_645EC=d4712meJuZZ%2FkTi1MEgrRyWWwK1Yc0nJyMAIdz4xRh3UFjHrv%2FJ1keD2cak; WWW_ST=1586581634006";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd=C%23%E9%80%9A%E8%BF%87webbrowser%E6%89%B9%E9%87%8F%E8%8E%B7%E5%8F%96%E6%90%9C%E7%B4%A2%E7%BB%93%E6%9E%9C&oq=%2526lt%253B%2523%25E9%2580%259A%25E8%25BF%2587webbrowser%25E6%2589%25B9%25E9%2587%258F%25E8%258E%25B7%25E5%258F%2596%25E6%2590%259C%25E7%25B4%25A2%25E7%25BB%2593%25E6%259E%259C&rsv_pq=eaba12fb0014d34a&rsv_t=8b5bcOV6%2ByVBy2beLA3KnQGRbWcqK9qq%2Baatz3%2Fg1YYB0KDDWOf5g%2Bge1d8&rqlang=cn&rsv_enter=0&rsv_dl=tb";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
              
                WebHeaderCollection headers = request.Headers;
              
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
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.GetEncoding("utf-8"));
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != "")
                {

                    string url = "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd=" + System.Web.HttpUtility.UrlEncode(array[i]); ;
                    string html = GetUrl(url, "utf-8"); ;

                   
                    Match value = Regex.Match(html, @"为您找到相关结果约([\s\S]*?)个");
                    label1.Text = "正在抓取：" + array[i];
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(array[i]);
                    lv1.SubItems.Add(value.Groups[1].Value);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(100);

                }

            }


        }


        private void 百度搜索_Load(object sender, EventArgs e)
        {
            button3.Click += new EventHandler(btn3_click);
        }

        private void btn3_click(object sender, EventArgs e)
        {


        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

           
            if (html.Contains(@"baidusousuo"))
            {
                if (zanting == false)
                {
                    zanting = true;
                }
                else
                {
                    Thread thread = new Thread(new ThreadStart(run));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }


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
