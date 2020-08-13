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

            if (text.Length > 2)
            {
                newdate = text[2] + "-" + text[0] + "-" + text[1].Trim();

            }
            else
            {
                date1 = date1.Replace(","," ");
               text = date1.Split(new string[] { " " }, StringSplitOptions.None);
                newdate = text[3] + "-" + text[1] + "-" + text[0].Trim();
                
            }
         
            

            return newdate;



        }

        bool shuju = true;  //判断页码是否有数据

       
        public void run()
        {
          
            if (!pagelist.Contains(page))
            {
                pagelist.Add(page);

                MatchCollection strhtmls= Regex.Matches(html, @"<div class=""a-row""><a class=""a-link-normal""([\s\S]*?)</span></div></div></ul>");

                //MatchCollection dates = Regex.Matches(html, @"review-date"" class=""a-size-base a-color-secondary review-date"">([\s\S]*?)</span>");  //1477
                //MatchCollection xingjis = Regex.Matches(html, @"review-star-rating"" class=""a-icon a-icon-star a-star-([\s\S]*?) ");   //1465
                
                //MatchCollection neirongs = Regex.Matches(html, @"review-text-content"">([\s\S]*?)</span>");                 //1472
                //MatchCollection sizes = Regex.Matches(html, @"format"">Size:([\s\S]*?)<([\s\S]*?)Color:([\s\S]*?)</a>");   //1477

                //MatchCollection asins = Regex.Matches(html, @"a-color-secondary"" href=""([\s\S]*?)reviews/([\s\S]*?)/");  //1472
                //MatchCollection links = Regex.Matches(html, @"review-title-content a-text-bold"" href=""([\s\S]*?)""");     //1460
                if (strhtmls.Count == 0)
                {
                    shuju = false;
                    return;
                }



                for (int i = 0; i < strhtmls.Count; i++)
                {
                    string strhtml = strhtmls[i].Groups[1].Value;
                    Match dates = Regex.Match(strhtml, @"review-date"" class=""a-size-base a-color-secondary review-date"">([\s\S]*?)</span>");  //1477
                    Match xingjis = Regex.Match(strhtml, @"review-star-rating"" class=""a-icon a-icon-star a-star-([\s\S]*?) ");   //1465

                    Match neirongs = Regex.Match(strhtml, @"review-text-content"">([\s\S]*?)</span>");

                    //Match sizes = Regex.Match(strhtml, @"format"">Size:([\s\S]*?)<([\s\S]*?)Colo([\s\S]*?)</a>");  
                    //Match sizes1 = Regex.Match(strhtml, @"format"">Color:([\s\S]*?)<([\s\S]*?)Size:([\s\S]*?)</a>");   


                    Match sizes = Regex.Match(strhtml, @"format"">Größe:([\s\S]*?)<([\s\S]*?)Farbe:([\s\S]*?)</a>");
                    Match sizes1 = Regex.Match(strhtml, @"format"">Color:([\s\S]*?)<([\s\S]*?)Size:([\s\S]*?)</a>");


                    Match asins = Regex.Match(strhtml, @"a-color-secondary"" href=""([\s\S]*?)reviews/([\s\S]*?)/");  
                    Match links = Regex.Match(strhtml, @"review-title-content a-text-bold"" href=""([\s\S]*?)""");

                    string size = "";
                    string color = "";

                    size = sizes.Groups[1].Value != "" ? sizes.Groups[1].Value : sizes1.Groups[3].Value;
                    color= sizes.Groups[3].Value != "" ? sizes.Groups[3].Value : sizes1.Groups[1].Value;

                    color= color.Replace("u","").Replace("r", "").Replace(":", "");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    try
                    {
                        string nr = neirongs.Groups[1].Value.Replace("<span>", "").Trim();

                        string time = timegeshi(dates.Groups[1].Value);

                      
                        lv1.SubItems.Add(time);
                        lv1.SubItems.Add(xingjis.Groups[1].Value);
                        lv1.SubItems.Add(nr);
                        lv1.SubItems.Add(size);
                        lv1.SubItems.Add(color);
                        lv1.SubItems.Add(asins.Groups[2].Value);
                        lv1.SubItems.Add("https://www.amazon.com"+links.Groups[1].Value);

                     

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
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

            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入网址");
                return;
            }


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
        bool status = false;
        public static string html; //网页源码传值
        int page = 0;//记录当前页码
        ArrayList pagelist = new ArrayList();
      
       
        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
       
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
