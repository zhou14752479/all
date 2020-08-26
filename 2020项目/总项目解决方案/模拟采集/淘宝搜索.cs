using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 模拟采集
{
    public partial class 淘宝搜索 : Form
    {
        public 淘宝搜索()
        {
            InitializeComponent();
        }
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        ArrayList wangwangList = new ArrayList();

        public void run()
        {
            

            MatchCollection shops = Regex.Matches(html, @"SHOPNAME\\"":\\""([\s\S]*?)\\""");
            MatchCollection wangwangs = Regex.Matches(html, @"WANGWANGID\\"":\\""([\s\S]*?)\\""");
            MatchCollection grades = Regex.Matches(html, @"GRADE\\"":\\""([\s\S]*?)\\""");
            MatchCollection ismalls = Regex.Matches(html, @"ISMALL\\"":\\""([\s\S]*?)\\""");
            MatchCollection sells = Regex.Matches(html, @"SELL\\"":\\""([\s\S]*?)\\""");


            //if (shops.Count == 0)
            //{
            //    shuju = false;
            //    return;
            //}

           
            for (int i = 0; i < shops.Count; i++)
            {


                string sell = Unicode2String(sells[i].Groups[1].Value).Replace("\\", "");
                string xinyu = Unicode2String(grades[i].Groups[1].Value).Replace("\\", "");
                string ismall = Unicode2String(ismalls[i].Groups[1].Value).Replace("\\", "").Trim();
                string wangwang = Unicode2String(wangwangs[i].Groups[1].Value).Replace("\\", "");

                try
                {
                    if (Convert.ToInt32(sell) >= Convert.ToInt32(textBox2.Text) && Convert.ToInt32(xinyu) <= Convert.ToInt32(textBox3.Text) && ismall == "0")
                    {
                        if (!wangwangList.Contains(wangwang))
                        {
                            wangwangList.Add(wangwang);
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(wangwang);
                            listViewItem.SubItems.Add(Unicode2String(shops[i].Groups[1].Value).Replace("\\", ""));

                            listViewItem.SubItems.Add(xinyu);
                            listViewItem.SubItems.Add(ismall);
                            listViewItem.SubItems.Add(sell);


                        }
                    }
                }
                catch(Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }



                
            }

            if (status == false)
            {
                return;
            }

        }

        bool shuju = true;  //判断页码是否有数据
        bool status = false;
        public static string html; //网页源码传值
       

        ArrayList pageKeyList = new ArrayList();

        string key = "";
        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"zhitongche"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            textBox4.Text += "正在查询......"+"\r\n";

            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in keywords)
            {
              

                key = keyword;
                for (int i = 0; i < 20; i++)
                {
                    textBox4.Text += "正在查询"+keyword+"第"+(i+1)+"页"+"\r\n";
                    this.textBox4.Focus();
                    this.textBox4.Select(this.textBox1.TextLength, 0);
                    this.textBox4.ScrollToCaret();

                    if (shuju == false)
                    {
                        return;
                    }
                    int p = i * 44;

                    status = false;
                    webBrowser1.Navigate("https://s.taobao.com/search?q=" + System.Web.HttpUtility.UrlEncode(keyword) + "&s=" + p);
                   

                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(5000);
                }


            }

           textBox4.Text += "查询结束";
            MessageBox.Show("查询结束");
        }

        private void 淘宝搜索_Load(object sender, EventArgs e)
        {

            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml");
        }
        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;

            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.DocumentText;
               
                run();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗？", "清空", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                listView1.Items.Clear();
                textBox4.Text = "";
            }
            else
            {

            }
        }
    }
}
