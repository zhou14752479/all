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
namespace 模拟采集
{
    public partial class 拼多多图片评价最新日期 : Form
    {
        public 拼多多图片评价最新日期()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 拼多多图片评价最新日期_KeyPress(object sender, KeyPressEventArgs e)
        {
            //设置所在窗体属性keypreview为true
            if (e.KeyChar == 13) //13表示回dao车键
            {
                webBrowser1.Navigate("https://"+textBox1.Text);
              
            }
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset,string token)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add(token);
               
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

        public void run()
        {


            // var htmldocument = (mshtml.HTMLDocument)webBrowser1.Document.DomDocument;

            // string html = htmldocument.documentElement.outerHTML;
            // textBox2.Text = html;

            // MatchCollection dates = Regex.Matches(html, @"review3\\u002Freview\\u002F([\s\S]*?)\\");
            //DateTime date = Convert.ToDateTime(dates[0].Groups[1].Value);
            // for (int i = 0; i < dates.Count; i++)
            // {

            //     if (date < Convert.ToDateTime(dates[i].Groups[1].Value))
            //     {
            //         date = Convert.ToDateTime(dates[i].Groups[1].Value);

            //     }

            // }

            //ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据  
            //lv1.SubItems.Add(date.ToShortDateString());
            //lv1.SubItems.Add(textBox1.Text);
            Match goodid = Regex.Match(textBox1.Text, @"goods_id=([\s\S]*?)&");
            
            string cookie = method.GetCookies("https://mobile.yangkeduo.com/goods_comments.html?goods_id=1074110620");
            Match t = Regex.Match(cookie, @"PDDAccessToken=.*");
            if (t.Groups[0].Value == "")
            {
                MessageBox.Show("请重新登陆");
                return;
            }
            string token = "accesstoken: " + t.Groups[0].Value.Replace("PDDAccessToken=", "");

            for (int i = 1; i < 999; i++)
            {



                string url = "https://mobile.yangkeduo.com/proxy/api/reviews/" + goodid.Groups[1].Value + "/list?pdduid=7312500755985&page="+i+"&size=10&enable_video=1&enable_group_review=1&label_id=100000000";



                string html = GetUrl(url, "utf-8", token);

                MatchCollection dates = Regex.Matches(html, @"review3/review/2([\s\S]*?)/");
                if (dates.Count > 0)
                {
                    for (int j = 0; j < dates.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add("2"+dates[j].Groups[1].Value);
                       

                    }
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("查询结束");
                    return;
                }
                Thread.Sleep(1000);
              }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html =method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"pddpinglun"))
            {
                MessageBox.Show("验证失败");
                return;
            }

            #endregion


            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        Thread thread;
        private void 拼多多图片评价最新日期_Load(object sender, EventArgs e)
        {
           

            webBrowser1.Navigate("https://mobile.yangkeduo.com/personal.html");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (!list.Contains(DateTime.Parse(listView1.Items[i].SubItems[1].Text)))
                {
                    list.Add(DateTime.Parse(listView1.Items[i].SubItems[1].Text));
                }
            }
            list.Sort();
            listView1.Items.Clear();
            foreach (var item in list)
            {
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据  
                lv1.SubItems.Add(item.ToShortDateString());
            }
        }
    }

}
