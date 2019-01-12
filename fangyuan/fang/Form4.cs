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

namespace fang
{
    public partial class Form4 : Form
    {
        bool loaded = false;   //该变量表示网页是否正在加载.

        bool status= true;

        public Form4()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url,string charset)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
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





        #region  房天下
        public void fangtianxia()

        {
            
            try
            {
                string[] citys = textBox1.Text.Split(',');
                foreach (string city in citys)
                {


                    for (int i = 1; i < 2; i++)
                    {
                        String url = "http://" + city + ".esf.fang.com/house/i3"+ i+"/";

                        webBrowser1.Url = new Uri(url);

                        

                        while (this.loaded==false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                       
                        string html = textBox2.Text;

                        MatchCollection matches = Regex.Matches(html, @"<h4 class=""clearfix"">([\s\S]*?)href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();

                        foreach (Match match in matches)
                        {
                            lists.Add("http://" + city + ".esf.fang.com" + match.Groups[2].Value);
                        }

                       
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        foreach (string list in lists)
                        {
                            
                            // textBox3.Text += list;
                            webBrowser1.Url = new Uri(list);
                            while (this.loaded == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (this.status == false)
                                return;
                            
                            string strhtml = textBox2.Text;

                            string rxg = @"<title>([\s\S]*?)</title>";
                            string Rxg = @"<span class=""zf_mfname"">([\s\S]*?)</span>";
                            string Rxg1 = @"class=""zf_mftel"">([\s\S]*?)</span>";
                            string Rxg2 = @"company_j"">([\s\S]*?)</span>";


                            Match title = Regex.Match(strhtml, rxg, RegexOptions.IgnoreCase);
                            Match name = Regex.Match(strhtml, Rxg, RegexOptions.IgnoreCase);
                            Match tel = Regex.Match(strhtml, Rxg1, RegexOptions.IgnoreCase);
                            Match company = Regex.Match(strhtml, Rxg2, RegexOptions.IgnoreCase);


                            ListViewItem lv1 = listView1.Items.Add(title.Groups[1].Value.Trim()); //使用Listview展示数据

                            lv1.SubItems.Add(name.Groups[1].Value.Trim());
                            lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                            lv1.SubItems.Add(company.Groups[1].Value.Trim());

                            Thread.Sleep(1000);
                        }

                       



                    }
                }
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(fangtianxia));
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            StreamReader reader = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(webBrowser1.Document.Encoding));
            string html = reader.ReadToEnd();
            textBox2.Text = html;
            this.loaded = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = false;
        }
    }
}
