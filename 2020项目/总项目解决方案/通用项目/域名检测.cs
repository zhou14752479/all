using System;
using System.Collections;
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
using CsharpHttpHelper;
using helper;
namespace 通用项目
{
    public partial class 域名检测 : Form
    {
        public 域名检测()
        {
            InitializeComponent();
        }
        #region GET请求获取跳转后url
        public static string getUrl(string url)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.AllowAutoRedirect = false;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                string content = response.GetResponseHeader("Location"); ;
                return content;
            }
            catch 
            {
                return "1";
                
            }


        }
        #endregion


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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
       
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 10000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();

                reader.Close();
                response.Close();
                return content;


            }
            catch 
            {
                return "1";

            }
           
        }
        #endregion
        bool zanting = true;
        bool status = true;
        ArrayList finishes = new ArrayList();
        ArrayList wans = new ArrayList();
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
                if (!wans.Contains(array[i]))
                {
                    wans.Add(array[i]);
                    label1.Text = "正在检测：" + array[i];
                    try
                    {
                        string url = "";
                        if (array[i].Contains("http"))
                        {
                            url = array[i];
                        }
                        else
                        {
                            url = "http://" + array[i];
                        }



                        string html = GetUrl(url);
                        Match head = Regex.Match(html, @"<head>([\s\S]*?)</head>");
                        if (html != "1" && html != "")
                        {

                            if (!finishes.Contains(head.Groups[1].Value))
                            {
                                finishes.Add(head.Groups[1].Value);
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(url);
                            }

                        }
                        else if (html == "1")
                        {
                            continue;
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                        {
                            return;
                        }

                    }
                    catch
                    {

                        continue;
                    }

                }
            }
            label1.Text = "完成";

        }

        
        private void 域名检测_Load(object sender, EventArgs e)
        {

        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"\u57df\u540d\u68c0\u6d4b"))
            {
                status = true;
                button2.Enabled = false;
                for (int i = 0; i < 10; i++)
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

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            status = false;
        }
    }
}
