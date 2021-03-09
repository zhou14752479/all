using System;
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
using myDLL;
namespace 主程序202009
{
    public partial class QQ邮件获取 : Form
    {
        public QQ邮件获取()
        {
            InitializeComponent();
        }

        string COOKIE = "pgv_pvi=1821155328; pgv_pvid=8645291092; XWINDEXGREY=0; RK=pLyB0zmT6N; ptcz=3573443604a581d1754b3233ec5841a9be6e82a39e2add5432f2968248434e31; eas_sid=D175l9b0m2N1x675B2z7m8V9F1; pt_sms_phone=132******80; LW_uid=y1m5F9u3S3v9c4Z88610Q0p8h6; ied_qq=o0852266010; uin_cookie=o0852266010; o_cookie=852266010; pac_uid=1_852266010; LW_sid=P1k5E929s8K8e2M0K4z6x6a099; pgv_si=s8761630720; wimrefreshrun=0&; qm_logintype=qq; qm_flag=0; qm_domain=https://mail.qq.com; edition=mail.qq.com; newpt=2; CCSHOW=000001; webp=1; pgv_info=ssid=s1682125480; uin=o2972599134; skey=@e9au01mb0; p_uin=o2972599134; pt4_token=EU5OmjuEs6dU8VIhLhk-CQLhKwV*s3BnIJCuH*ODNqk_; p_skey=Ub93tZZqhpoywpr0kBHqx3PdbPnlKb4RdONcn-fQaFk_; qqmail_alias=2972599134@qq.com; sid=-1322368162&01d3d070c12b272d9bbaadf237aff071,qVWI5M3RaWnFocG95d3ByMGtCSHF4M1BkYlBubEtiNFJkT05jbi1mUWFGa18.; qm_username=2972599134; qm_ptsk=-1322368162&@e9au01mb0; foxacc=852266010&0|-1322368162&0; ssl_edition=sail.qq.com; qm_loginfrom=852266010&wsk|-1322368162&wpt; username=-1322368162&2972599134; xm_uin=13102663997473630; xm_sid=zV5KcIxDUTAuLnY1ALFGWgAA; xm_skey=13102663997473630&6cccce4dc56ee4589c46bbbacccddc80; new_mail_num=852266010&1236|-1322368162&244";
        

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
            
                 request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
            {


                string url = "https://mail.qq.com/cgi-bin/mail_list?sid="+sid+"&folderid=1&folderkey=1&page="+i+"&s=inbox&topmails=0&showinboxtop=1&ver=592297.0&cachemod=maillist&cacheage=7200&r=&selectall=0#stattime=1601086114071";

                string html = GetUrl(url, "gb18030");
               
                MatchCollection mailids = Regex.Matches(html, @"addrvip=""false"" mailid=""([\s\S]*?)""");


                for (int j = 0; j < mailids.Count; j++)
                {
                    string aurl = "https://mail.qq.com/cgi-bin/readmail?folderid=1&folderkey=1&t=readmail&mailid=" + mailids[j].Groups[1].Value + "&mode=pre&maxage=3600&base=12.65&ver=16664&sid="+sid+"#stattime=1601085966167";
                    string ahtml = GetUrl(aurl, "gb18030");

                    Match title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>");
                    Match sjtime = Regex.Match(ahtml, @"<b id=""local-time-caption"" class=""tcolor"">([\s\S]*?)</b>");
                    Match ordertime = Regex.Match(ahtml, @"Shipping Method([\s\S]*?)</td>([\s\S]*?</td>)");
                    Match sjmail = Regex.Match(ahtml, @"g_uin=""([\s\S]*?)""");
                    Match orderNumber = Regex.Match(ahtml, @"Order Number([\s\S]*?)</table>");
                    MatchCollection xinghao = Regex.Matches(ahtml, @"line-height: 24px;"">([\s\S]*?)</td>");
                    Match size = Regex.Match(ahtml, @"line-height: 22px;"">S([\s\S]*?)</td>");

                    Match person = Regex.Match(ahtml, @"Shipping to:([\s\S]*?)</td>");
                    Match address = Regex.Match(ahtml, @"<span class=""address"">([\s\S]*?)</span>");
                    Match zhuangtai = Regex.Match(ahtml, @"sans-serif !important;"">([\s\S]*?)</span>");
                    MatchCollection ShippingMethod = Regex.Matches(ahtml, @"<td style=""font-size: 0px; max-width:40px;"">&nbsp;</td>([\s\S]*?)</td>");


                    label1.Text = "正在抓取：" + Regex.Replace(orderNumber.Groups[1].Value, "<[^>]+>", "").Trim();





                    if (title.Groups[1].Value.Contains("Looking for") )
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(sjtime.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(ordertime.Groups[2].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(sjmail.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(orderNumber.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(xinghao[1].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace("S"+size.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(xinghao[0].Groups[1].Value, "<[^>]+>", "").Trim());  //运单号

                        lv1.SubItems.Add(Regex.Replace(person.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(address.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(zhuangtai.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(ShippingMethod[1].Groups[1].Value, "<[^>]+>", "").Trim());
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                    else if (title.Groups[1].Value.Contains("Thank You for"))
                    {
                        Match Subtotal = Regex.Match(ahtml, @"Subtotal</td>([\s\S]*?)</td>");
                        MatchCollection GiftCard = Regex.Matches(ahtml, @"Gift Card</td>([\s\S]*?)</td>");
                        Match Handling = Regex.Match(ahtml, @"Shipping & Handling</td>([\s\S]*?)</td>");
                        Match Total = Regex.Match(ahtml, @">Total</td>([\s\S]*?)</td>");




                        ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv2.SubItems.Add(Regex.Replace(sjtime.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(ordertime.Groups[2].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(sjmail.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(orderNumber.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(xinghao[1].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace("S"+size.Groups[1].Value, "<[^>]+>", "").Trim());
                    

                        lv2.SubItems.Add(Regex.Replace(person.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(address.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(zhuangtai.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(ShippingMethod[1].Groups[1].Value, "<[^>]+>", "").Trim());

                        lv2.SubItems.Add(Regex.Replace(Subtotal.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(GiftCard[0].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(GiftCard[1].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(Handling.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv2.SubItems.Add(Regex.Replace(Total.Groups[1].Value, "<[^>]+>", "").Trim());
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }

                    else if (title.Groups[1].Value.Contains("Your Payment Overview"))
                    {
                        Match ordertime3 = Regex.Match(ahtml, @"ORDER DATE:([\s\S]*?)</td>([\s\S]*?</td>)");
                       
                        Match orderNumber3 = Regex.Match(ahtml, @"ORDER NUMBER:([\s\S]*?)<hr size");
                        Match xinghao3 = Regex.Match(ahtml, @"<img border=""0""([\s\S]*?)</td>([\s\S]*?)<span style");
                        Match size3 = Regex.Match(ahtml, @"Size:([\s\S]*?)</td>");

                        Match person3 = Regex.Match(ahtml, @"<td style=""font-size:1px; padding-top:5px; padding-bottom:5px;"">([\s\S]*?)<br  />([\s\S]*?)</td>");
                        
                        Match zhuangtai3 = Regex.Match(ahtml, @"<td align=""center"" style=""border:none;"">([\s\S]*?)</td>");
                        Match ShippingMethod3= Regex.Match(ahtml, @"SHIPPING METHOD:([\s\S]*?)<hr size");



                        Match Subtotal = Regex.Match(ahtml, @"Subtotal:([\s\S]*?)</tr>");
                        Match Handling = Regex.Match(ahtml, @"Shipping & Handling:([\s\S]*?)</tr>");
                        Match tax = Regex.Match(ahtml, @"Tax:([\s\S]*?)</tr>");
                        Match Total = Regex.Match(ahtml, @"TOTAL:([\s\S]*?)</tr>");
                        Match style = Regex.Match(ahtml, @"Style#:([\s\S]*?)</td>");




                        ListViewItem lv3 = listView3.Items.Add((listView3.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv3.SubItems.Add(Regex.Replace(sjtime.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(ordertime3.Groups[2].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(sjmail.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(orderNumber3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(xinghao3.Groups[2].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(size3.Groups[1].Value, "<[^>]+>", "").Trim());


                        lv3.SubItems.Add(Regex.Replace(person3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(person3.Groups[2].Value, "<[^>]+>", "").Trim());  //地址
                        lv3.SubItems.Add(Regex.Replace(zhuangtai3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(ShippingMethod3.Groups[1].Value, "<[^>]+>", "").Trim());

                        lv3.SubItems.Add(Regex.Replace(Subtotal.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(Handling.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(tax.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(Total.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv3.SubItems.Add(Regex.Replace(style.Groups[1].Value, "<[^>]+>", "").Trim().Replace("&zwnj;", ""));
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }



                    Thread.Sleep(1000);
                }
            }
            label1.Text = "抓取结束";


        }
            
        

        bool zanting = true;
        Thread thread;
        private void QQ邮件获取_Load(object sender, EventArgs e)
        {

        }

        public string sid = "";

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"qqmail"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            //sid = Form1.SID;
            //COOKIE = Form1.cookie;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text=="表一")
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            }
            else if (comboBox1.Text == "表二")
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
            }
            else if (comboBox1.Text == "表三")
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView3), "Sheet1", true);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Form1 fm1 = new Form1();
            //fm1.Show();
        }
    }
}
