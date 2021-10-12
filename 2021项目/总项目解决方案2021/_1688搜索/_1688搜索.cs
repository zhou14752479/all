using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace _1688搜索
{
    public partial class _1688搜索 : Form
    {
        public _1688搜索()
        {
            InitializeComponent();
        }

        string COOKIE = "";
        //"https://data.p4psearch.1688.com/data/ajax/get_premium_offer_list.json?beginpage=1&keywords=%E4%B9%90%E6%B8%85%E5%B8%82%E6%B8%85%E6%B1%9F%E6%96%B0%E7%8E%AF%E6%B4%B2%E5%B7%A5%E5%85%B7%E5%8E%82";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {
            string html = "";
         
          
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36 MicroMessenger/7.0.9.501 NetType/WIFI MiniProgramEnv/Windows WindowsWechat";
                request.Referer = "https://www.1688.com/";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        Thread thread;
        bool zanting = true;
        bool status = true;
        DataTable dt = null;

        private void button1_Click(object sender, EventArgs e)
        {
            COOKIE = method.GetCookies("https://s.1688.com/selloffer/offer_search.htm?keywords=%C0%D6%C7%E5%CA%D0%C7%E5%BD%AD%D0%C2%BB%B7%D6%DE%B9%A4%BE%DF%B3%A7&n=y&netType=1%2C11%2C16&spm=a260k.dacugeneral.search.0");
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
               

            }
        }



        public void run()
        {
            status = true;
            try
            {

                dt = method.ExcelToDataTable(textBox1.Text, true);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    string keyword = System.Web.HttpUtility.UrlEncode(dr[0].ToString(), Encoding.GetEncoding("GB2312"));

                    label2.Text = "正在读取：" + dr[0].ToString();
                     string url = "https://s.1688.com/selloffer/offer_search.htm?keywords="+ keyword + "&n=y&netType=1%2C11%2C16&spm=a260k.dacugeneral.search.0";
                  
                    string html = GetUrl(url, "utf-8");
                    //textBox2.Text = html;
                  
                    MatchCollection companyNames = Regex.Matches(html, @"""memberCreditUrl"":""([\s\S]*?)/page([\s\S]*?)""name"":""([\s\S]*?)""");

                    //MessageBox.Show(companyNames.Count.ToString());
                    Thread.Sleep(1500);
                    for (int j = 0; j < companyNames.Count; j++)
                    {
                       
                        if (companyNames[j].Groups[3].Value == dr[0].ToString())
                        {
                            string shopurl = companyNames[j].Groups[1].Value;
                       
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(dr[0].ToString());
                            lv1.SubItems.Add(shopurl);
                          
                            break;
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }


                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void _1688搜索_Load(object sender, EventArgs e)
        {
            method.SetFeatures(10000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://login.taobao.com/?redirect_url=https%3A%2F%2Flogin.1688.com%2Fmember%2Fjump.htm%3Ftarget%3Dhttps%253A%252F%252Flogin.1688.com%252Fmember%252FmarketSigninJump.htm%253FDone%253D%25252F%25252Fwww.1688.com%25252F&style=tao_custom&from=1688web");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }
    }
}
