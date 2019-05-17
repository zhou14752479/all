using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using NPOI.SS.Formula.Functions;
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

namespace main._2019_5
{
    public partial class 足球数据 : Form
    {




        public string gethtml(string url)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,
                Method = "GET",//URL     可选项 默认为Get  
                Encoding = Encoding.GetEncoding("utf-8"),
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = "Hm_lvt_5ffc07c2ca2eda4cc1c4d8e50804c94b=1557975485; __utmc=56961525; __utmz=56961525.1557975489.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); PHPSESSID=3e944cfd954c52e155f6d5d99ad19606779f6240; pm=; LastUrl=; FirstURL=www.okooo.com/; FirstOKURL=http%3A//www.okooo.com/jingcai/; First_Source=www.okooo.com; __utma=56961525.1606818588.1557975489.1557981725.1557985950.3; IMUserID=30498306; IMUserName=%E6%9E%97%E5%AD%94988610; OKSID=3e944cfd954c52e155f6d5d99ad19606779f6240; M_UserName=%22%5Cu6797%5Cu5b54988610%22; M_UserID=30498306; M_Ukey=067e94b82ba40266c3a93fde0c9d9c01; OkAutoUuid=aa5bcc288c3e2610627fc126218f5eb1; OkMsIndex=8; Hm_lpvt_5ffc07c2ca2eda4cc1c4d8e50804c94b=1557987723; __utmb=56961525.45.9.1557987726651",//字符串Cookie     可选项  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "http://www.sufeinet.com",//来源URL     可选项  
                //Allowautoredirect = False,//是否根据３０１跳转     可选项  
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
            return html;

        }


        public 足球数据()
        {
            InitializeComponent();
        }
        #region  主程序








        bool zanting = true;
            public void run()

        {

            DateTime dtStart = DateTime.Parse(dateTimePicker1.Text); ;
            DateTime dtEnd = DateTime.Parse(dateTimePicker2.Text);

            try
            {
                for (DateTime dt = dtStart; dt <= dtEnd; dt = dt.AddDays(1))
                {

                    String Url = "http://www.okooo.com/livecenter/jingcai/?date="+ dt.ToString("yyyy-MM-dd");
                    textBox1.Text = Url;
                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection urls = Regex.Matches(html, @"matchid=""([\s\S]*?)""");
                    MatchCollection ass = Regex.Matches(html, @"<b class=""font_red ctrl_homescore"">([\s\S]*?)</b>");
                    MatchCollection bss = Regex.Matches(html, @"<b class=""font_red ctrl_awayscore"">([\s\S]*?)</b>");

                    ArrayList lists = new ArrayList();

                    foreach (System.Text.RegularExpressions.Match url in urls)
                    {
                        lists.Add("http://www.okooo.com/soccer/match/" + url.Groups[1].Value + "/ah/ajax/?page=0&trnum=0&companytype=BaijiaBooks");
                    }


                    for (int i = 0; i < lists.Count; i++)
                    {

                        string strhtml = gethtml(lists[i].ToString());

                        System.Text.RegularExpressions.Match mainhtml = Regex.Match(strhtml, @"澳门彩票</span>([\s\S]*?)data-pname");

                       // textBox1.Text = mainhtml.Groups[1].Value;

                        System.Text.RegularExpressions.Match a1 = Regex.Match(mainhtml.Groups[1].Value, @"class=""nolink "" ><span>([\s\S]*?)</span>");
                        MatchCollection a2 = Regex.Matches(mainhtml.Groups[1].Value, @"attval=""([\s\S]*?)""");//2个
                        MatchCollection a3 = Regex.Matches(mainhtml.Groups[1].Value, @"<span class="""">([\s\S]*?)</span>"); //4个
                        System.Text.RegularExpressions.Match a4 = Regex.Match(mainhtml.Groups[1].Value, @"class=""nolink ""><span>([\s\S]*?)</span>");
                        System.Text.RegularExpressions.Match a5 = Regex.Match(mainhtml.Groups[1].Value, @"bgObj"">([\s\S]*?)</span>");





                        if (a1.Groups[1].Value.Trim()!=""&&a2.Count > 1 && a3.Count > 3)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(ass[i].Groups[1].Value + "：" + bss[i].Groups[1].Value);   //比分
                            lv1.SubItems.Add(a1.Groups[1].Value.Trim());   //初盘
                            lv1.SubItems.Add(a2[0].Groups[1].Value.Trim());
                            lv1.SubItems.Add(a4.Groups[1].Value.Trim());

                            lv1.SubItems.Add(a3[0].Groups[1].Value.Trim());   //最新盘
                            lv1.SubItems.Add(a2[1].Groups[1].Value.Trim());
                            lv1.SubItems.Add(a3[1].Groups[1].Value.Trim());

                            lv1.SubItems.Add(a5.Groups[1].Value.Trim());   //凯莉
                            lv1.SubItems.Add(a3[2].Groups[1].Value.Trim());
                            lv1.SubItems.Add(a3[3].Groups[1].Value.Trim());

                            lv1.SubItems.Add(Url.Substring(Url.Length - 10, 10));//日期

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }


                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }

                        Thread.Sleep(Convert.ToInt32(200));   //内容获取间隔，可变量        
                    }



                }


            }



            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        private void 足球数据_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            label3.Text = "程序运行中......";
            #region 通用登录
           
            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (System.Text.RegularExpressions.Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                    Thread thread = new Thread(new ThreadStart(run));
                    thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
        
            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion




        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
            label3.Text = "程序已暂停......";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
            label3.Text = "程序运行中......";
            zanting = true;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
