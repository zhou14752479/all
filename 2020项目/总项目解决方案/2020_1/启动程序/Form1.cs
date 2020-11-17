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
using helper;

namespace 启动程序
{
    public partial class 厦门大学 : Form
    {
        public 厦门大学()
        {
            InitializeComponent();
        }

        bool zanting = true;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            
            try
            {

                string cookie = method.GetCookies("http://moa.xmu.edu.cn/km/imissive/km_imissive_sign_main/kmImissiveSignMain.do?method=listChildren&categoryId=&q.s_raq=0.012296960934379708&pageno=2&rowsize=15&orderby=docCreateTime&ordertype=down&s_ajax=true"); ;
                string url = "http://moa.xmu.edu.cn/km/review/km_review_index/kmReviewIndex.do?method=list&q.s_raq=0.08639820153425903&pageno=1&rowsize=999&orderby=docCreateTime&ordertype=down&s_ajax=true";
                string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                MatchCollection uids = Regex.Matches(html, @"fdId"",""value"":""([\s\S]*?)""");
                
                foreach (Match uid in uids)
                {
                    string URL = "http://moa.xmu.edu.cn/km/review/km_review_main/kmReviewMain.do?method=view&fdId="+uid.Groups[1].Value;

                    string ahtml = method.GetUrlWithCookie(URL, cookie, "utf-8");

                    
                    Match a1 = Regex.Match(ahtml, @"教工号([\s\S]*?)\<\/xformflag\>");
                    Match a2 = Regex.Match(ahtml, @"出生省市([\s\S]*?)\<\/xformflag\>");
                    Match a3 = Regex.Match(ahtml, @"姓名([\s\S]*?)\<\/xformflag\>");
                    Match a4 = Regex.Match(ahtml, @"出生日期([\s\S]*?)\<\/xformflag\>");
                    Match a5 = Regex.Match(ahtml, @"户口所在地([\s\S]*?)\<\/xformflag\>");
                    Match a6 = Regex.Match(ahtml, @"所在学院\/部门([\s\S]*?)\<\/xformflag\>");
                    Match a7 = Regex.Match(ahtml, @"联系电话([\s\S]*?)\<\/xformflag\>");
                    Match a8 = Regex.Match(ahtml, @"配偶姓名([\s\S]*?)\<\/xformflag\>");
                    Match a9 = Regex.Match(ahtml, @"职务\/职称([\s\S]*?)\<\/xformflag\>");
                    Match a10 = Regex.Match(ahtml, @"邮箱([\s\S]*?)\<\/xformflag\>");
                    Match a11 = Regex.Match(ahtml, @"配偶电话([\s\S]*?)\<\/xformflag\>");
                    Match a12 = Regex.Match(ahtml, @"前往国家\/地区([\s\S]*?)\<\/xformflag\>");
                    Match a13 = Regex.Match(ahtml, @"包括转机城市\）([\s\S]*?)\<\/xformflag\>");
                    Match a14 = Regex.Match(ahtml, @"姓名\（中文\）([\s\S]*?)\<\/xformflag\>");
                    Match a15 = Regex.Match(ahtml, @"启程日期([\s\S]*?)\<\/xformflag\>");
                    Match a16 = Regex.Match(ahtml, @"内日期([\s\S]*?)\<\/xformflag\>");
                    Match a17 = Regex.Match(ahtml, @"_Calculation"" value=""([\s\S]*?)""");
                    Match a18 = Regex.Match(ahtml, @"出访内容([\s\S]*?)\<\/xformflag\>");
                    Match a19 = Regex.Match(ahtml, @"校领导意见([\s\S]*?)</div></div></TD></TR>");  //校领导意见
                    if (a1.Groups[1].Value != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a11.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a13.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a14.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a15.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a16.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a17.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a18.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a19.Groups[1].Value, "<[^>]+>", "").Replace("&nbsp", "").Trim());

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        
                    }
                }

            }

            catch (Exception)
            {

                throw;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Visible = false;
            Thread thread1 = new Thread(new ThreadStart(run));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
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
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Visible = true;
            webBrowser1.Navigate("http://ids.xmu.edu.cn/authserver/login?service=http://moa.xmu.edu.cn/index.jsp");
        }
    }
}
