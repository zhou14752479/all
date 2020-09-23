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
using helper;


namespace 阿里搜索
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }


        //public string cookie = "__sw_newuno_count__=1; cna=8QJMFUu4DhACATFZv2JYDtwd; UM_distinctid=16cefea0a30a40-0b81729ae68b29-f353163-1fa400-16cefea0a313bc; ali_apache_id=10.147.104.33.1567393188668.359538.6; lid=zkg852266010; ali_ab=49.89.65.102.1569737961650.7; _bl_uid=mLkp11C34O4leghtCl0nwX80npmO; hng=CN%7Czh-CN%7CCNY%7C156; taklid=4f874566ce0649de88b438e91f27dc36; __last_loginid__=zkg852266010; ali_apache_track=c_mid=b2b-1052347548|c_lid=zkg852266010|c_ms=1; alisw=swIs1200%3D1%7C; h_keys=\" % u8fde % u8863 % u88d9 % u5c0f % u4e2a % u5b50#%u673a%u68b0#%u673a%u68b0%u5236%u9020#%u7537%u88c5#%u72d7%u7cae%u673a#%u4eff%u771f%u82b1%u9676%u74f7%u82b1%u74f6%u5957%u88c5%u529e%u516c%u5ba4%u82b1%u827a%u6885%u82b1%u6b27%u5f0f%u4e32%u6885%u63d2%u82b1%u5047%u82b1%u82b1%u675f%u88c5%u9970%u6446%u4ef6\"; ad_prefer=\"2019/12/24 16:02:04\"; cookie2=174175b89e94125a26946ead30ea45e6; t=792ea994957bef8e4a71539f91876594; _tb_token_=e633aba6be38e; uc4=id4=0%40UOnlZ%2FcoxCrIUsehKGzvOQal50zl&nk4=0%40GwrkntVPltPB9cR46GnfFQuVx7MXZaQ%3D; __cn_logon__=false; alicnweb=touch_tb_at%3D1577174539028%7Clastlogonid%3Dzkg852266010; l=cBETLNflqaVt8jBDBOCgVuIRGd7ThIRfguPRwVAvi_5BisTs-YbOojOlEU96cjWhGO8p40tUd_2tBUb3JjSL10lYzIC0J; isg=BPn5mZTMg-mBsFxbfkaRtoh4CGUTruXJjzErOBsuHyCfohg0YlOaiD4wJObxGoXw";

        public string cookie = "";
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "https://dnzmjx.1688.com/?spm=a262cb.19918180.keqnoz0r.6.66763cb7PVN8zY";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                request.Accept = "*/*";
                request.Timeout = 100000;
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
        public void run()

        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }

            string keyword = System.Web.HttpUtility.UrlEncode(textBox5.Text.Trim(), Encoding.GetEncoding("GB2312"));
            string priceStart = textBox1.Text.Trim();
            string priceEnd = textBox2.Text.Trim();
            for (int i = 1; i < 101; i++)
            {
                string url = "https://s.1688.com/selloffer/offer_search.htm?priceStart="+priceStart+"&descendOrder=true&sortType=quantity_sum_month&uniqfield=userid&keywords="+keyword+"&earseDirect=false&priceEnd="+priceEnd+"&from=taoSellerSearch&netType=1%2C11&n=y&filt=y#sm-filtbar=&beginPage="+i+"&offset=0";
                string html = method.GetUrlWithCookie(url,cookie, "GBK");
                MatchCollection matches = Regex.Matches(html, @"gotoDetail=""2"" href=""([\s\S]*?)""");
              
                for (int j = 0; j < matches.Count; j++)
                {
                   
                    string aurl = matches[j].Groups[1].Value+"/page/contactinfo.htm";
                    string ahtml = method.GetUrlWithCookie(aurl, cookie, "gbk");
                   
                    Match lxr = Regex.Match(ahtml, @"class=""membername"" target=""_blank"">([\s\S]*?)</a>");

                    Match tel = Regex.Match(ahtml, @"移动电话：</dt>([\s\S]*?)</dd>");

                    Match tel11 = Regex.Match(tel.Groups[1].Value, @"\d{11}");
                    if (tel11.Groups[0].Value != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(lxr.Groups[1].Value);
                        lv1.SubItems.Add(tel11.Groups[0].Value);
                    }
                  
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(1000);
                }
                Thread.Sleep(Convert.ToInt32(textBox7.Text));
            }

            button1.Enabled = true;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"1688917"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            cookie = 登录.cookie;
            button1.Enabled = false;
         


                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.expotTxt(listView1);
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
            登录 dl = new 登录();
            dl.Show();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            listView1.Items.Clear();
        }
    }
}
