using CefSharp;
using CefSharp.WinForms;
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

namespace CefSharp谷歌
{
    public partial class shopee : Form
    {
        public shopee()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
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
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString());
            MessageBox.Show("导出成功");
        }

        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {




                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));

        }
        /// <summary>
        /// 规格时间
        /// </summary>
        public void run()
        {


            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                Match itemId = Regex.Match(textBox1.Text+",", @"i\.([\s\S]*?)\.([\s\S]*?),");
                
               

                string url = "https://shopee.com.my/api/v2/item/get_ratings?filter=0&flag=1&itemid="+ itemId.Groups[2].Value .Trim()+ "&limit=6&offset=" + (i * 6) + "&shopid=" + itemId.Groups[1].Value.Trim() + "&type=0";
                string html = GetUrl(url, "utf-8");

                MatchCollection ahtmls = Regex.Matches(html, @"show_reply([\s\S]*?)delete_reason");
                foreach (Match ahtml in ahtmls)
                {
                    Match guige = Regex.Match(ahtml.Groups[1].Value, @"""model_name"":""([\s\S]*?)""");
                    Match time = Regex.Match(ahtml.Groups[1].Value, @"anonymous([\s\S]*?)""ctime"":([\s\S]*?),");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(guige.Groups[1].Value);
                    lv1.SubItems.Add(ConvertStringToDateTime(time.Groups[2].Value).ToString("yyyy-MM-dd- HH:mm"));
                }

                Thread.Sleep(500);





            }




        }
        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://seller.shopee.com.my/");
        private void shopee_Load(object sender, EventArgs e)
        {
            browser.Load("https://seller.shopee.com.my/");
            browser.Parent = this.splitContainer1.Panel1;
            browser.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"shopee"))
            {
              
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

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("function _func(){var test=document.getElementsByTagName('html')[0].innerHTML; return test}");//运行页面上js的test方法
            Task<CefSharp.JavascriptResponse> t = browser.EvaluateScriptAsync("_func()");
            t.Wait();// 等待js 方法执行完后，获取返回值 t.Result 是 CefSharp.JavascriptResponse 对象t.Result.Result 是一个 object 对象，来自js的 callTest2() 方法的返回值
            if (t.Result.Result != null)
            {
                
               string html = t.Result.Result.ToString();
                MatchCollection uids = Regex.Matches(html, @"Order ID&nbsp;([\s\S]*?)</span>");
                MatchCollection uids2 = Regex.Matches(html, @"订单编号&nbsp;([\s\S]*?)</span>");
                if (uids.Count > 0)
                {
                    for (int i = 0; i < uids.Count; i++)
                    {
                        ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(uids[i].Groups[1].Value);
                    }
                }

                else
                {
                    for (int i = 0; i < uids2.Count; i++)
                    {
                        ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(uids2[i].Groups[1].Value);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView1,true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView2, true);
        }
    }
}
