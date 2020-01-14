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
namespace 联通手机号
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool zanting = true;
        public void run()
        {

            try
            {
                for (int i = 0; i < 9999; i++)
                {


                    string url = "https://msgo.10010.com/NumApp/NumberCenter/qryNum?callback=jsonp_queryMoreNums&provinceCode=34&cityCode=330&monthFeeLimit=0&goodsId=341610241535&searchCategory=3&net=01&amounts=200&codeTypeCode=&searchValue=&qryType=02&goodsNet=4&channel=msg-xsg&_=1578983826340";
                    string html = method.GetUrl(url, "utf-8");

                    textBox3.Text = DateTime.Now.ToString() + ":正在采集";
                    MatchCollection haomas = Regex.Matches(html, @"\d{11}");
                    if (haomas.Count == 0)
                        return;
                        for (int j = 0; j < haomas.Count; j++)
                     {
                        string haoma = haomas[j].Groups[0].Value;
                        ListViewItem listViewItem = this.listView1.Items.Add(haoma);
                        
                        Match match3 = Regex.Match(haoma, @"\d([0-9])(?!\1)([0-9])\2{2}\d");  //三连
                        Match match7 = Regex.Match(haoma, @"\d{7}([0-9]012|[^0]123|[^1]234|[^2]345|[^3]456|[^4]567|[^5]678|[^6]789)");  // 尾号ABC
                        Match match6 = Regex.Match(haoma, @"\d{6}([0-9])(?!\1)([0-9])\2(?!\2)([0-9])\3");   //尾号AABB
                        Match match5 = Regex.Match(haoma, @"\d{7}([0-9])(?!\1)([0-9])\1\2");   //尾号ABAB
                        Match match10= Regex.Match(haoma, @"\d{5}(\d{3})\1");   //尾号ABCABC
                        Match match8 = Regex.Match(haoma, @"\d{5}([0-9])(?!\1)([0-9])\2{2}(?!\2)([0-9])");   //尾号AAABA

                        //第二排
                        Match match19 = Regex.Match(haoma, @"\d{7}([0-9])(?!\1)([0-9])\2{2}");   //尾号AAA
                        Match match18 = Regex.Match(haoma, @"\d([0-9])(?!\1)([0-9])\2{3}\d");   //四连

                        Match match16 = Regex.Match(haoma, @"\d{5}([0-9]0123|[^0]1234|[^1]2345|[^2]3456|[^3]4567|[^4]5678|[^5]6789)");   //尾号ABCD
                        Match match15 = Regex.Match(haoma, @"\d{4}([0-9])(?!\1)([0-9])\2(?!\2)([0-9])\3(?!\3)([0-9])\4");   //尾号AABBCC
                        Match match13 = Regex.Match(haoma, @"(01234|12345|23456|34567|45678|56789)");   //中间ABCDE
                        Match match11 = Regex.Match(haoma, @"\d{4}([0-9])(?!\1)([0-9])\2(?!\2)([0-9])\3(?!\3)([0-9])\4");   
                        {
                            this.listView3.Items.Add(haoma);
                        }
                        if (match7.Groups[1].Value != "")
                        {
                            this.listView7.Items.Add(haoma);
                        }
                        if (match6.Groups[1].Value != "")
                        {
                            this.listView6.Items.Add(haoma);
                        }
                        if (match5.Groups[1].Value != "")
                        {
                            this.listView5.Items.Add(haoma);
                        }
                        if (match10.Groups[1].Value != "")
                        {
                            this.listView10.Items.Add(haoma);
                        }
                        if (match8.Groups[1].Value != "")
                        {
                            this.listView8.Items.Add(haoma);
                        }
                        if (match19.Groups[1].Value != "")
                        {
                            this.listView19.Items.Add(haoma);
                        }

                        if (match18.Groups[1].Value != "")
                        {
                            this.listView18.Items.Add(haoma);
                        }


                        if (match16.Groups[1].Value != "")
                        {
                            this.listView16.Items.Add(haoma);
                        }
                        if (match15.Groups[1].Value != "")
                        {
                            this.listView15.Items.Add(haoma);
                        }

                        if (match13.Groups[1].Value != "")
                        {
                            this.listView13.Items.Add(haoma);
                        }
                        if (match11.Groups[1].Value != "")
                        {
                            this.listView11.Items.Add(haoma);
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        this.listView1.Items[j].EnsureVisible();

                    }

                }

                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
