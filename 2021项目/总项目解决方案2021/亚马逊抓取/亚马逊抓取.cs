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
using myDLL;

namespace 亚马逊抓取
{
    public partial class 亚马逊抓取 : Form
    {
        public 亚马逊抓取()
        {
            InitializeComponent();
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
        }

        int index { get; set; }

        string cookie = "session-id=147-3224729-6027332; session-id-time=2082787201l; i18n-prefs=USD; lc-main=en_US; ubid-main=133-2258364-8692801; session-token=KDqk+y3UXy8qOdfLB8DonvGIx8mkT9sSyJnGoDYeqSm+nwCHqhiN1NsgBHJs07GDw8DX8TnizQGWRi17H0YagbvFngjxcBGu8KUZ8Uwz1MI7RaLcR7IT8np9j5uJRk6Ib9PsfqwXNybR9i239XjecLTLLW0vWmwLexhXrkqpa2ci7PW4HJa+HClbrF+I1UEG; csm-hit=tb:06REF2EJ5YA9F0RJPDFJ+s-06REF2EJ5YA9F0RJPDFJ|1634004953556&t:1634004953556&adb:adblk_no";



        private delegate void InvokeHandler();


        
        
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            int pageall = 12;
           
            for (int page = 1; page < pageall; page++)
            {
                try
                {
                    cookie = "";

                    if (status == false)
                    {
                        return;
                    }

                    string url = "https://www.amazon.com/s?k=Home+Office+Desk+Chairs&i=garden&rh=n%3A3733721&page="+page+"&_encoding=UTF8&c=ts&qid=1634002990&ts_id=3733721&ref=sr_pg_2";
                    string html = method.GetUrlWithCookie(url,cookie,"utf-8");
                   // textBox2.Text = html;
                    MatchCollection htmls = Regex.Matches(html, @"<span cel_widget_id=""MAIN-SEARCH_RESULTS([\s\S]*?)</div></div></div></div></div>");
                   // MessageBox.Show(htmls.Count.ToString());
                   //MatchCollection pagealls= Regex.Matches(html, @"aria-disabled=""true"">([\s\S]*?)</li>");

                   // if(pagealls.Count>0)
                   // {
                   //     pageall = Convert.ToInt32(pagealls[pagealls.Count - 1].Groups[1].Value);
                   // }
                    
                    for (int i = 0; i < htmls.Count; i++)
                    {
                        string title = Regex.Match(htmls[i].Groups[1].Value, @"<span class=""a-size-base-plus a-color-base a-text-normal"">([\s\S]*?)</span>").Groups[1].Value;
                        string price = Regex.Match(htmls[i].Groups[1].Value, @"data-a-color=""base""><span class=""a-offscreen"">([\s\S]*?)</span>").Groups[1].Value;
                        string asin = Regex.Match(htmls[i].Groups[1].Value, @";asin=([\s\S]*?)&").Groups[1].Value;
                        string star = Regex.Match(htmls[i].Groups[1].Value, @"<div class=""a-row a-size-small"">([\s\S]*?)label=""([\s\S]*?)out").Groups[2].Value;
                        string comment = Regex.Match(htmls[i].Groups[1].Value, @"<span class=""a-size-base"">([\s\S]*?)</span>").Groups[1].Value;
                       

                        //子线程中
                        this.Invoke(new InvokeHandler(delegate ()
                        {
                            this.index = this.dataGridView1.Rows.Add();
                            dataGridView1.Rows[index].Cells[0].Value = index.ToString();
                            dataGridView1.Rows[index].Cells[1].Value = asin;
                            dataGridView1.Rows[index].Cells[2].Value = asin;
                            dataGridView1.Rows[index].Cells[3].Value = asin;
                            dataGridView1.Rows[index].Cells[4].Value = asin;

                            dataGridView1.Rows[index].Cells[5].Value = title;
                            dataGridView1.Rows[index].Cells[6].Value = price;
                            dataGridView1.Rows[index].Cells[7].Value = star;
                            dataGridView1.Rows[index].Cells[8].Value = comment;
                        }));

                      
                      

                    }
                    Thread.Sleep(1000);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    continue;
                }
            }




        }


        bool status = true;
        Thread thread;
        private void login_btn_Click(object sender, EventArgs e)
        {
            status = true;

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
        }

        private void 亚马逊抓取_Load(object sender, EventArgs e)
        {

        }
    }
}
