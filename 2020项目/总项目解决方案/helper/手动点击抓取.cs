using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace helper
{
    public partial class 手动点击抓取 : Form
    {
        public 手动点击抓取()
        {
            InitializeComponent();
        }

        private void 手动点击抓取_Load(object sender, EventArgs e)
        {

            



            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);

        }


        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           

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

        public string timegeshi(string date)
        {
            string newdate = "";
            Match dates = Regex.Match(date, @"on .*");
            string date1 = dates.Groups[0].Value.Replace("on", "").Trim();
           date1= date1.Replace("January", "01,").Replace("February", "02,").Replace("March", "03,").Replace("April", "04,").Replace("May", "05,").Replace("June", "06,").Replace("July", "07,").Replace("August", "08,").Replace("September", "09,").Replace("October", "10,").Replace("November", "11,").Replace("December", "12,");
            string[] text = date1.Split(new string[] { "," }, StringSplitOptions.None);
            newdate = text[2] + "-" + text[0] + "-" + text[1].Trim();
            return newdate;

        }

        bool shuju = true;  //判断页码是否有数据


        public void run()
        {
          
            if (!pagelist.Contains(page))
            {
                pagelist.Add(page);
                MatchCollection dates = Regex.Matches(html, @"review-date"" class=""a-size-base a-color-secondary review-date"">([\s\S]*?)</span>");
                MatchCollection xingjis = Regex.Matches(html, @"review-star-rating"" class=""a-icon a-icon-star a-star-([\s\S]*?) ");
                // MatchCollection xingjis = Regex.Matches(html, @"<i data-hook=""cmps-review-star-rating"" class=""a-icon a-icon-star a-star-([\s\S]*?) ");
                MatchCollection neirongs = Regex.Matches(html, @"review-text-content"">([\s\S]*?)</span>");
                MatchCollection sizes = Regex.Matches(html, @"format"">Size:([\s\S]*?)<([\s\S]*?)Color:([\s\S]*?)</a>");

                MatchCollection asins = Regex.Matches(html, @"a-color-secondary"" href=""([\s\S]*?)reviews/([\s\S]*?)/");
                MatchCollection links = Regex.Matches(html, @"review-title-content a-text-bold"" href=""([\s\S]*?)""");
                if (neirongs.Count == 0)
                {
                    shuju = false;
                    return;
                }

                for (int i = 0; i < neirongs.Count; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    try
                    {
                        string nr = neirongs[i].Groups[1].Value.Replace("<span>", "").Trim();

                        string time = timegeshi(dates[i].Groups[1].Value);

                      
                        lv1.SubItems.Add(time);
                        lv1.SubItems.Add(xingjis[i].Groups[1].Value);
                        lv1.SubItems.Add(nr);
                        lv1.SubItems.Add(sizes[i].Groups[1].Value);
                        lv1.SubItems.Add(sizes[i].Groups[3].Value);
                        lv1.SubItems.Add(asins[i].Groups[2].Value);
                        lv1.SubItems.Add("https://www.amazon.com"+links[i].Groups[1].Value);

                    }
                    catch (Exception)
                    {

                    }


                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"amazon"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            for (int i = 1; i <= 999; i++)
            {
                if (shuju == false)
                {
                    return;
                }


                page = i;
                status = false;
                webBrowser1.Navigate(textBox1.Text.Trim()+"&pageNumber=" + i);

                while (this.status == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }

            }
        }
        public static string html; //网页源码传值
        int page = 0;//记录当前页码
        ArrayList pagelist = new ArrayList();
      
       
        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
        bool status = false;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            html = webBrowser1.DocumentText;  //获取到的html
            if (html.Contains("</html>"))
            {
                run();

            }






            }

        public delegate void sendMessage(string name);


        private void button4_Click(object sender, EventArgs e)
        {
        //    Form1 fm1 = new Form1();
          
        //    sendMessage sd = new sendMessage(fm1.getmseeage);
        //    sd += new sendMessage(fm1.getmseeage);
        //    fm1.Show();

        //    sd("1");
           
            listView1.Items.Clear();
        }
    }
}
