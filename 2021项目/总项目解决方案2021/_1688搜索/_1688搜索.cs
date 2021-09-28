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


        //"https://data.p4psearch.1688.com/data/ajax/get_premium_offer_list.json?beginpage=1&keywords=%E4%B9%90%E6%B8%85%E5%B8%82%E6%B8%85%E6%B1%9F%E6%96%B0%E7%8E%AF%E6%B4%B2%E5%B7%A5%E5%85%B7%E5%8E%82";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "_m_h5_tk=7bbdfeb502ae948c563f7ae24cada131_1632541876434; _m_h5_tk_enc=afffe89ec6d737dd5ab6ae60069e4e19; ";
          
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36 MicroMessenger/7.0.9.501 NetType/WIFI MiniProgramEnv/Windows WindowsWechat";
                request.Referer = "https://m.1688.com/search.html";
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
            try
            {

                dt = method.ExcelToDataTable(textBox1.Text, true);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    string keyword = System.Web.HttpUtility.UrlEncode(dr[0].ToString());

                    label2.Text = "正在读取：" + dr[0].ToString();
                    // string url = "https://m.1688.com/offer_search/-6D7033.html?sortType=booked&descendOrder=true&filtId=&keywords=" + keyword;
                    string url = "https://h5api.m.1688.com/h5/mtop.1688.wap.seo.offer.get/1.0/?jsv=2.6.1&appKey=12574478&t=1632533258015&sign=e89ab6ab3854ef979b3553c75506fc04&api=mtop.1688.wap.seo.offer.get&v=1.0&type=jsonp&dataType=jsonp&callback=mtopjsonp6&data=%7B%22sortType%22%3A%22booked%22%2C%22filtId%22%3A%22%22%2C%22keywords%22%3A%22"+keyword+"%22%2C%22descendOrder%22%3A%22true%22%2C%22appName%22%3A%22wap%22%2C%22appKey%22%3A%221772295d201e06bd6d56a6ed5cfd252f%22%2C%22beginPage%22%3A1%2C%22pageSize%22%3A20%7D";
                    string html = GetUrl(url, "utf-8");
                 
                    MatchCollection winPortUrls = Regex.Matches(html, @"""winPortUrl"":""([\s\S]*?)""");
                    MatchCollection companyNames = Regex.Matches(html, @"""companyName"":""([\s\S]*?)""");

                    MessageBox.Show(companyNames.Count.ToString());
                    Thread.Sleep(1500);
                    for (int j = 0; j < companyNames.Count; j++)
                    {
                        MessageBox.Show(companyNames[j].Groups[1].Value);
                        if (companyNames[j].Groups[1].Value == dr[0].ToString())
                        {
                            string shopid = Regex.Match(winPortUrls[i].Groups[1].Value, @"winport/([\s\S]*?)\.").Groups[1].Value;
                            string pcurl = "https://" + shopid + ".1688.com/";
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(dr[0].ToString());
                            lv1.SubItems.Add(winPortUrls[i].Groups[1].Value);
                            lv1.SubItems.Add(pcurl);
                          
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
    }
}
