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

namespace 主程序202005
{
    public partial class 数据抓取 : Form
    {
        public 数据抓取()
        {
            InitializeComponent();
        }

        bool zanting = true;
        bool status = true;
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
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://foreign.mingluji.com/Finland_Medicine_/_Health_/_Beauty";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
              
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

        #region 主程序
        public void run()
        {

            try
            {

                for (int i = 0; i< 99; i++)
                {


                    string url = textBox1.Text.Trim()+"/" +i;
                    if (radioButton2.Checked == true)
                    {
                        url = textBox2.Text.Trim();
                        i = 98;

                    }


                    string html = GetUrl(url,"utf-8");
                   
                    Match ahtml = Regex.Match(html, @"<td valign=([\s\S]*?)<td valign=");
                  
                    MatchCollection uids = Regex.Matches(ahtml.Groups[1].Value, @"<li> <a href=""/([\s\S]*?)""");

                    if (radioButton2.Checked == true)
                    {
                        ahtml = Regex.Match(html, @"<div lang=""en""([\s\S]*?)<div class=""printfooter"">");
                        uids = Regex.Matches(ahtml.Groups[1].Value, @"<li><a href=""/([\s\S]*?)""");
                    }


                    if (uids.Count == 0)
                        return;
                
           
                   
                    for (int j = 0; j< uids.Count; j++)
                    {
                        string strhtml = GetUrl("https://foreign.mingluji.com/"+uids[j].Groups[1].Value, "utf-8");
                        Match name = Regex.Match(strhtml, @"<span itemprop=""name"">([\s\S]*?)</span>");
                        Match email = Regex.Match(strhtml, @"\(电子邮件\):([\s\S]*?)</dd>");
                       
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据  
                       
                        lv1.SubItems.Add(name.Groups[1].Value);
                        lv1.SubItems.Add(Regex.Replace(email.Groups[1].Value, "<[^>]+>", ""));



                        if (status == false)
                            return;



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                    Thread.Sleep(500);



                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        #endregion
        private void 数据抓取_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"kuaijichaxun"))
            {
                button1.Enabled = false;
                status = true;
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

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
           status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
