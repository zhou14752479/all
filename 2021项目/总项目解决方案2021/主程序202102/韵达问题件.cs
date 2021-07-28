using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace 主程序202102
{
    public partial class 韵达问题件 : Form
    {
        public 韵达问题件()
        {
            InitializeComponent();
        }
        Thread thread;
        string cookie = "";
        bool zanting = true;
        bool status = true;


        public void getCookies()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://pcs.yundasys.com:15114/problem/public/index.php/admin/index/index.html");
            //options.AddArgument("--lang=en"); 
           
            StringBuilder sb;
            Thread.Sleep(15000);
            while (true)
            {
                sb = new StringBuilder();
                var _cookies = driver.Manage().Cookies.AllCookies;

                foreach (OpenQA.Selenium.Cookie cookie in _cookies)
                {

                    sb.Append(cookie.Name + "=" + cookie.Value + ";");
                    // driver.Manage().Cookies.AddCookie(cookie);
                }

                if (sb.ToString().Contains("PHPSESSID") && sb.ToString().Contains("CASTGC=") && sb.ToString().Contains("user_name"))
                {

                    break;
                }
                else
                {
                    driver.Navigate().GoToUrl("http://pcs.yundasys.com:15114/problem/public/index.php/admin/index/index.html");
                    Thread.Sleep(5000);
                }
            }
            driver.Quit();

            textBox2.Text = sb.ToString();


        }
        Thread t;
        private void button1_Click(object sender, EventArgs e)
        {
            if (t == null || !t.IsAlive)
            {
                t = new Thread(getCookies);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            //string path = AppDomain.CurrentDomain.BaseDirectory;
            //Process.Start(path + "helper.exe");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }


        public string getinfos(string item)
        {
            string url = "http://pcs.yundasys.com:15114/problem/public/index.php/admin/issue_release/get_order_site.html?order_no="+item+"&type=166";
            string html = method.GetUrlWithCookie(url,cookie,"utf-8");
            return html;
        }

        public void run()
        {


            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string item in text)
            {
                if (item != "")
                {
                    try
                    {

                        string html = getinfos(item);
                       
                        string deal_site = deal_site = Regex.Match(html, @"\d{5,7}").Groups[0].Value;
                        string send_name = System.Web.HttpUtility.UrlEncode(Regex.Match(html, @"send_name"":""([\s\S]*?)""").Groups[1].Value);
                        string send_phone = Regex.Match(html, @"send_phone"":""([\s\S]*?)""").Groups[1].Value;
                        string send_prov = Regex.Match(html, @"send_prov"":""([\s\S]*?)""").Groups[1].Value;
                        string send_city = Regex.Match(html, @"send_city"":""([\s\S]*?)""").Groups[1].Value;
                        string send_area = Regex.Match(html, @"send_area"":""([\s\S]*?)""").Groups[1].Value;
                        string send_detail = System.Web.HttpUtility.UrlEncode(Regex.Match(html, @"send_detail"":""([\s\S]*?)""").Groups[1].Value);

                        string rece_name = System.Web.HttpUtility.UrlEncode(Regex.Match(html, @"rece_name"":""([\s\S]*?)""").Groups[1].Value);
                        string rece_phone = Regex.Match(html, @"rece_phone"":""([\s\S]*?)""").Groups[1].Value;
                        string rece_prov = Regex.Match(html, @"rece_prov"":""([\s\S]*?)""").Groups[1].Value;
                        string rece_city = Regex.Match(html, @"rece_city"":""([\s\S]*?)""").Groups[1].Value;
                        string rece_area = Regex.Match(html, @"rece_area"":""([\s\S]*?)""").Groups[1].Value;
                        string rece_detail = System.Web.HttpUtility.UrlEncode(Regex.Match(html, @"rece_detail"":""([\s\S]*?)""").Groups[1].Value);

                        string order_state = System.Web.HttpUtility.UrlEncode(textBox3.Text.Trim());

                      

                        string url = String.Format("http://pcs.yundasys.com:15114/problem/public/index.php/admin/issue_release/dorelease.html?order_type=166&order_no={0}&deal_site={1}&notify_site=&order_state={2}&freight_no=&depart_prove=&send_name={3}&send_phone={4}&send_prov={5}&send_city={6}&send_area={7}&send_detail={8}&rece_name={9}&rece_phone={10}&rece_prov={11}&rece_city={12}&rece_area={13}&rece_detail={14}&mail_type=&order_brand=&order_pack=&order_color=&order_shape=&order_texture=&order_weight=&order_pic%5B%5D=&order_pic=", 

                            item,deal_site,order_state,send_name,send_phone,send_prov,send_city,send_area,send_detail, rece_name,rece_phone,rece_prov,rece_city,rece_area,rece_detail);

                        //textBox3.Text = url;
                      
                        string ahtml = method.GetUrlWithCookie(url, cookie, "utf-8");
                     

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                        lv1.SubItems.Add(item);
                        lv1.SubItems.Add(ahtml);


                    }
                    catch (Exception ex)
                    {

                      continue;

                    }
                    Thread.Sleep(1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                }
            }

        }



        private void button3_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            //try
            //{
            //    StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
            //    //一次性读取完 
            //    string texts = sr.ReadToEnd();

            //    cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
            //    textBox2.Text = cookie;
            //    sr.Close();  //只关闭流
            //    sr.Dispose();   //销毁流内存

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.ToString());
            //}

            cookie = textBox2.Text;


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listView1.Items.Clear();
        }

        private void 韵达问题件_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();

        }
    }
}
