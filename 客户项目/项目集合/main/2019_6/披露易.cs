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

namespace main._2019_6
{
    public partial class 披露易 : Form
    {
        public 披露易()
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
                string COOKIE = "JSESSIONID=-ojzNCIsxKKiKp-HArT1rMUDJ903YH-FEBZLuXvo; TS014a5f8b=015e7ee603708c985041dfbbd0bee7c53e638324311c9c0b0fbdf83b935793d6c5cb99c8de09c53b92d88ecc768c7d01393d83ed8283683637338df779e2389ca4527221d8; WT_FPC=id=49.70.16.112-3694228448.30748387:lv=1561858788094:ss=1561858424110; TS0168982d=01043817a52559ae1aa9938a98dcce97a7008122d663de38d3c1610f95aee60947345ee142c8aacf75e02229db087c071bb77f2c52";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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
        /// 获取ID
        /// </summary>
        public string getId()
        {
            try
            {

                string URL = "https://www1.hkexnews.hk/search/prefix.do?&callback=callback&lang=ZH&type=A&name=" + textBox1.Text + "&market=SEHK";

                string html = GetUrl(URL, "utf-8");

                Match aid = Regex.Match(html, @"stockId"":([\s\S]*?),");
                return aid.Groups[1].Value;

            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }


        }

        bool zanting = true;
        /// <summary>
        /// 获取内容
        /// </summary>
        public void getFile()
        {
            try
            {

                DateTime dtStart = DateTime.Parse(dateTimePicker1.Text); ;
                DateTime dtEnd = DateTime.Parse(dateTimePicker2.Text);

                string startdate = dtStart.ToString("yyyyMMdd"); 
                string enddate = dtEnd.ToString("yyyyMMdd"); 

                string URL = "https://www1.hkexnews.hk/search/titleSearchServlet.do?sortDir=0&sortByOptions=DateTime&category=0&market=SEHK&stockId="+getId()+"&documentType=-1&fromDate="+ startdate + "&toDate="+ enddate + "&title=&searchType=0&t1code=-2&t2Gcode=-2&t2code=-2&rowRange=9999&lang=zh";

                    string html = GetUrl(URL, "utf-8");

                MatchCollection dates = Regex.Matches(html, @"""DATE_TIME\\"":\\""([\s\S]*?)\\");
                MatchCollection codes = Regex.Matches(html, @"""STOCK_CODE\\"":\\""([\s\S]*?)\\");
                MatchCollection names = Regex.Matches(html, @"""STOCK_NAME\\"":\\""([\s\S]*?)\\");


                MatchCollection urls = Regex.Matches(html, @"""FILE_LINK\\"":\\""([\s\S]*?)\\");
                    MatchCollection filenames = Regex.Matches(html, @"""TITLE\\"":\\""([\s\S]*?)\\");

                if (urls.Count > 0)
                { 
                    for (int j = 0; j < urls.Count; j++)
                    {
                        textBox2.Text = DateTime.Now.ToShortTimeString() + "正在获取"+textBox1.Text+"的信息"+"\r\n";
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(dates[j].Groups[1].Value);
                        lv1.SubItems.Add(codes[j].Groups[1].Value);
                        lv1.SubItems.Add(names[j].Groups[1].Value);
                        lv1.SubItems.Add(filenames[j].Groups[1].Value);
                        lv1.SubItems.Add("https://www1.hkexnews.hk"+urls[j].Groups[1].Value);
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }


                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        private void 披露易_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToShortTimeString() + "软件开始运行.....";

            Thread thread = new Thread(new ThreadStart(getFile));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
