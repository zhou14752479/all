using System;
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

namespace main._2019_5
{
    public partial class 花拍 : Form
    {
        public 花拍()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public static string CleanHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml)) return strHtml;
            //删除脚本
            //Regex.Replace(strHtml, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase)
            strHtml = Regex.Replace(strHtml, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //删除标签
            var r = new Regex(@"</?[^>]*>", RegexOptions.IgnoreCase);
            Match m;
            for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
            {
                strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
            }
            return strHtml.Trim();
        }

        public static string COOKIE = "";

        bool status = true;
        bool zanting = true;
        
        public void run()
        {
            COOKIE = Browser.cookie;
            try
            {

                for (int i = Convert.ToInt32(textBox1.Text) * 10; i <= Convert.ToInt32(textBox2.Text) *10; i = i + 10)
                {

                    string Url = "https://seller.huapai.com/goodslist/index?tid=0&status=2&step_status=-1&oid=0&per_page=" + i ;

                    string html = method.GetUrlWithCookie(Url, COOKIE, "utf-8");
                    MatchCollection names = Regex.Matches(html, @"male\.png"" width=""15""\/>([\s\S]*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                   
                    MatchCollection dingdans = Regex.Matches(html, @"订单号：([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection QQs = Regex.Matches(html, @"<div style=""margin-left: 20px;"">([\s\S]*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    for (int j = 0; j < names.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(names[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(dingdans[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(CleanHtml(QQs[j].Groups[1].Value).Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        label1.Text = "当前正在抓取第" + i / 10 + "页";

                        if (this.status == false)
                            return;
                    }

                }

                Thread.Sleep(500);



            }




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void Button4_Click(object sender, EventArgs e)
        {
            Browser web = new Browser("https://seller.huapai.com/");
            web.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 花拍_Load(object sender, EventArgs e)
        {

        }
    }
}
