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
using myDLL;

namespace 主程序202006
{
    public partial class aliexpress : Form
    {
        public aliexpress()
        {
            InitializeComponent();
        }

        public string COOKIE = "";
        private void aliexpress_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://posting.aliexpress.com/wsproduct/photobank.htm?spm=5261.newworkbench.aenewheader.3.32e93e5f8TEDO8");
            webBrowser1.ScriptErrorsSuppressed = true;
            
        }

        ArrayList lists = new ArrayList();
        public void run()
        {

            try
            {



                for (int i =1; i <999; i++)
                {
                   
                    string url = "https://photobank.aliexpress.com/photobank/ajaxPhotobank.htm?ctoken=z82bhst7kv9o&event=searchImage&location=allGroup&page=" + i;
                   ;
                    string html = method.GetUrlWithCookie(url,COOKIE,"utf-8");
                    MatchCollection aids = Regex.Matches(html, @"""url"":""([\s\S]*?)""");
                  

                    for (int j = 0; j < aids.Count; j++)
                    {
                        if (!lists.Contains(aids[j].Groups[1].Value))
                        {
                            lists.Add(aids[j].Groups[1].Value);
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(aids[j].Groups[1].Value);
                        }
                        else
                        {
                            MessageBox.Show(i.ToString());
                            MessageBox.Show("爬取结束");
                            return;
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        if (status == false)
                            return;

                    }

                    Thread.Sleep(1000);
                }

               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"doc88"))
            {
               
                COOKIE = method.GetCookies("https://posting.aliexpress.com/wsproduct/photobank.htm?spm=5261.newworkbench.aenewheader.3.32e93e5f8TEDO8");
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

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
        bool status = true;
        bool zanting = true;
        private void button7_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
