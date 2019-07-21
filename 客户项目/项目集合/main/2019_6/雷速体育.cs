using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System;
using System.Collections;
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

namespace main._2019_6
{
    public partial class 雷速体育 : Form
    {
        public 雷速体育()
        {
            InitializeComponent();
        }

        private void 雷速体育_Load(object sender, EventArgs e)
        {
            
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url,string charset)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = Url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = "acw_tc=2760775c15618530601745242ee8980767526b511f7f11c14a5aed4a53bd71; Hm_lvt_63b82ac6d9948bad5e14b1398610939a=1561853059,1561864228; PHPSESSID=5vco7epunmgm90meneff84ks60; lang=; acw_sc__v2=5d185ca46e2c486cf7cddc208da48b0f0467c8e2; SERVERID=4ab2f7c19b72630dd03ede01228e3e61|1561877670|1561874981; Hm_lpvt_63b82ac6d9948bad5e14b1398610939a=1561877671",//字符串Cookie     可选项  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
               // Referer = "http://www.sufeinet.com",//来源URL     可选项  
               // Allowautoredirect = False,//是否根据３０１跳转     可选项  
                //AutoRedirectCookie = False,//是否自动处理Cookie     可选项  
                                           //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                       //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                              //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                              //ProxyPwd = "123456",//代理服务器密码     可选项  
                              //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;
            
            return html;
        }
        #endregion
        /// <summary>
        /// 获取内容
        /// </summary>
        public void run()
        {
            try
            {
                string URL = "https://live.leisu.com/lanqiu";

                string html = GetUrl(URL, "utf-8");

                Match weihtml = Regex.Match(html, @"html([\s\S]*?)未开始");

                
                MatchCollection ids = Regex.Matches(weihtml.Groups[1].Value, @"<li class=""list-item list-item-([\s\S]*?) ");



                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("https://live.leisu.com/lanqiu/daxiaoqiu-"+id.Groups[1].Value);
                }

                

                foreach (string list in lists)

                {
                    string strhtml = GetUrl(list, "utf-8");
                    MatchCollection matches = Regex.Matches(strhtml, @"<div class=""tip""><span class=""name"">([\s\S]*?)</span>");
                    Match chu = Regex.Match(strhtml, @"handp float-left col-3"">([\s\S]*?)</div>");
                    Match jishi = Regex.Match(strhtml, @"handp float-left col-3 color-([\s\S]*?)"">([\s\S]*?)</div>");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(matches[0].Groups[1].Value+"-"+ matches[1].Groups[1].Value);
                    lv1.SubItems.Add(chu.Groups[1].Value);
                    lv1.SubItems.Add(jishi.Groups[2].Value);
                    lv1.SubItems.Add(DateTime.Now.ToString());


                    if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                    if (jishi.Groups[2].Value != "" && chu.Groups[1].Value != "")
                    {
                        double a = Convert.ToDouble(chu.Groups[1].Value);
                        double b = Convert.ToDouble(jishi.Groups[2].Value);
                        double c = b - a;
                        double d = a - b;

                        
                        if (d > Convert.ToDouble(textBox1.Text)&& Convert.ToDouble(textBox1.Text)>0 && c>0)
                        {
                            sendEmail.send(textBox3.Text,"雷速体育提醒",matches[0].Groups[1].Value + "-" + matches[1].Groups[1].Value+"的比赛达到设定差值请查看！");
                            
                        }
                        if (c< Convert.ToDouble(textBox1.Text)&& Convert.ToDouble(textBox1.Text)<0 && d<0)
                        {
                            sendEmail.send(textBox3.Text, "雷速体育提醒", matches[0].Groups[1].Value + "-" + matches[1].Groups[1].Value + "的比赛达到设定差值请查看！");
                        }
                    }



                }


            }
            catch (Exception ex)
            {

                ex.ToString();
            }


        }


        public void run1()
        {
           
                try
                {
                    string URL = "https://live.leisu.com/lanqiu";

                    string html = GetUrl(URL, "utf-8");

                    Match weihtml = Regex.Match(html, @"html([\s\S]*?)未开始");


                    MatchCollection ids = Regex.Matches(weihtml.Groups[1].Value, @"<li class=""list-item list-item-([\s\S]*?) ");


                    MatchCollection scores = Regex.Matches(weihtml.Groups[1].Value, @"<div class=""float-left col-4"">([\s\S]*?)</div>");



                    for (int a = 1; 12*a-2 < scores.Count; a++)
                    {

                        int x = Convert.ToInt32(scores[12* a -8].Groups[1].Value) + Convert.ToInt32(scores[12*a-4].Groups[1].Value);
                        int y = Convert.ToInt32(scores[12 * a - 7].Groups[1].Value) + Convert.ToInt32(scores[12 * a -3].Groups[1].Value);
                        int z = Convert.ToInt32(scores[12 * a - 6].Groups[1].Value) + Convert.ToInt32(scores[12 * a -2].Groups[1].Value);


                        ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据         

                        lv2.SubItems.Add(x.ToString());
                        lv2.SubItems.Add(y.ToString());
                        lv2.SubItems.Add(z.ToString());

                    if (x<20|x>50)
                    {
                        sendEmail.send(textBox3.Text, "雷速体育提醒", "第一节的比赛达到比分设定值！");

                    }
                    if (y < 20 | y > 50)
                    {
                        sendEmail.send(textBox3.Text, "雷速体育提醒", "第一节的比赛达到比分设定值！");

                    }
                    if (z < 20 |z> 50)
                    {
                        sendEmail.send(textBox3.Text, "雷速体育提醒", "第一节的比赛达到比分设定值！");

                    }


                }
                }
            catch (Exception ex)
            {

                ex.ToString();
            }


        }


        public void jiankong()
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[2].Text != "" && listView1.Items[i].SubItems[3].Text != "")
                {
                    double a = Convert.ToDouble(listView1.Items[i].SubItems[2].Text);
                    double b = Convert.ToDouble(listView1.Items[i].SubItems[3].Text);
                    double c = b - a;
                    double d = 0 - c;
                    if (c > Convert.ToDouble(textBox1.Text) || d> Convert.ToDouble(textBox1.Text))
                    {
                        MessageBox.Show("达到差值了");
                    }
                }
              
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                button1.Text = "监控已开启";
                button1.Enabled = false;
                timer1.Interval = (Convert.ToInt32(textBox2.Text)) * 1000;
                timer1.Start();

            }

            else if (checkBox2.Checked == true)
            {
                button1.Text = "监控已开启";
                button1.Enabled = false;
                timer2.Interval = (Convert.ToInt32(textBox2.Text)) * 1000;
                timer2.Start();

            }

            else
            {
                MessageBox.Show("请选择需要监控的比赛！");
            }




        }


        private void timer1_Tick(object sender, EventArgs e)
        {

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run1));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
