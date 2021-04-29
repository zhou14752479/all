using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Text.RegularExpressions;
using System.Threading;

namespace 模拟采集谷歌版
{
    public partial class 发货易 : Form
    {
        public 发货易()
        {
            InitializeComponent();
        }
    

        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://a44.fahuoyi.com/order/index?shopId=1551861&tab=5");
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 发货易_Load(object sender, EventArgs e)
        {
          
            browser.Load("https://a44.fahuoyi.com/order/index?shopId=1551861&tab=5");
            browser.Parent = splitContainer1.Panel1;
            browser.Dock = DockStyle.Fill;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Dwzy2DZ"))
            {

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


        public void run()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function tempFunction() {");
            //sb.AppendLine(" return document.body.innerHTML; "); 
            sb.AppendLine(" return document.getElementsByTagName('body')[0].outerHTML; ");
            sb.AppendLine("}");
            sb.AppendLine("tempFunction();");
            var task01 = browser.GetBrowser().GetFrame(browser.GetBrowser().GetFrameNames()[0]).EvaluateScriptAsync(sb.ToString());
            task01.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    var response = t.Result;
                    if (response.Success == true)
                    {
                        if (response.Result != null)
                        {
                            string html = response.Result.ToString();


                           
                            MatchCollection goods = Regex.Matches(html, @"""title"":""([\s\S]*?)""");


                            textBox1.Text = html;

                            //for (int i = 0; i < skus.Count; i++)
                            //{
                            //    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            //    lv1.SubItems.Add(skus[i].Groups[1].Value.Trim());
                            //    lv1.SubItems.Add(prices[i].Groups[1].Value.Trim());
                            //}
                        }
                    }
                }
            });





        }
        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
