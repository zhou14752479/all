using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace CsharpSelenium
{
    public partial class 孔夫子旧书网 : Form
    {
        public 孔夫子旧书网()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请导入isbn");
                return;
            }

            status = true;
            if (thread == null || !thread.IsAlive)
            {
             
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button1_Click(object sender, EventArgs e)
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

        static void AddCookieByString(IWebDriver driver, string cookieString)
        {
            string[] cookies = cookieString.Split(';');
            foreach (string cookie2 in cookies)
            {
                string[] cookieNameValue = cookie2.Split('=');
                if (cookieNameValue.Length == 2)
                {
                    Cookie cookie = new Cookie(cookieNameValue[0], cookieNameValue[1]);
                    try
                    {
                        driver.Manage().Cookies.AddCookie(cookie);
                    }
                    catch (Exception)
                    {
                        continue;
                       
                    }
                }
                else
                {
                    throw new ArgumentException("Cookie string is not in name=value format.");
                }
            }
           
        }



        public void savetxt(string a,string b,string c,string d,string e ,string  f)
        {


            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(a + "  " + b + "  " + c + "  " + d + "  " + e + "  " + f);
            sw.Close();
            fs1.Close();
            sw.Dispose();
        }


        #region 主程序
        public void run()
        {

            try
            {
                ChromeOptions options = new ChromeOptions();
                options.BinaryLocation = "Chrome/Application/chrome.exe";
                // options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                options.AddArgument("--disable-gpu");

                IWebDriver driver = new ChromeDriver(options);
                //driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://www.kongfz.com/");



                //AddCookieByString(driver,  "kfz_uuid=930594df-dc42-49d3-933a-513c5aa7544f; Hm_lvt_33be6c04e0febc7531a1315c9594b136=1713963270; shoppingCartSessionId=bf96c8533a0bdd06e8e47556884cf211; kfz-tid=06cbe2cc29ee6077ad6979e021b9daba; Hm_lvt_bca7840de7b518b3c5e6c6d73ca2662c=1713963270,1714048900; utm_source=101002001000; PHPSESSID=06a00eca1636e28ce4b7562ad52179da8e8475f3; kfz_trace=930594df-dc42-49d3-933a-513c5aa7544f|16134930|6758f7fba719d453|101002001000; reciever_area=1001000000; acw_tc=276077d717140523666123336e5b1ec3c32ab1bb45116dfd44d7e5365605ed");


                //Thread.Sleep(1000);
                //driver.Navigate().GoToUrl("https://www.kongfz.com/");
                Thread.Sleep(60000);

                //Cookie cookie = new Cookie("shoppingCartSessionId", "cac0103a18f12726671fb4ae1c87608f; leftMenuStatus", "", DateTime.Now.AddDays(9999));
                //driver.Manage().Cookies.AddCookie(cookie);
                //Thread.Sleep(1000);
                // driver.Navigate().GoToUrl("https://www.kongfz.com/");

                StreamReader keysr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                string ReadTxt = keysr.ReadToEnd();
                string[] text = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != "")
                    {

                        try
                        {
                            label1.Text = text[i];
                            driver.Navigate().GoToUrl("https://search.kongfz.com/product/?keyword=" + text[i] + "&dataType=0&sortType=7&page=1&actionPath=sortType");

                            Thread.Sleep(1000);
                            MatchCollection prices = Regex.Matches(driver.PageSource, @"price-int"">([\s\S]*?)</div>");
                            MatchCollection fees = Regex.Matches(driver.PageSource, @"快递:([\s\S]*?)</span>");




                            if (status == false)
                            {
                                return;
                            }



                            driver.Navigate().GoToUrl("https://search.kongfz.com/product/?dataType=1&keyword=" + text[i]);
                            Thread.Sleep(1000);
                            Match sale = Regex.Match(driver.PageSource, @"result-count__number"">([\s\S]*?)</span>");

                            string sales = sale.Groups[1].Value;


                            if (sales.Trim() == "")
                            {
                                sales = "无";
                            }

                            if (prices.Count > 2)
                            {
                                string b = Regex.Replace(prices[0].Groups[1].Value.Replace("￥", ""), "<[^>]+>", "");
                                string c = fees[0].Groups[1].Value.Replace("￥", "");
                                string d = Regex.Replace(prices[2].Groups[1].Value.Replace("￥", ""), "<[^>]+>", "");
                                string e = fees[2].Groups[1].Value.Replace("￥", "");

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add(b);
                                lv1.SubItems.Add(c);
                                lv1.SubItems.Add(d);
                                lv1.SubItems.Add(e);
                                lv1.SubItems.Add(sales);

                                savetxt(text[i], b, c, d, e, sales);
                            }
                            else if (prices.Count > 0 && prices.Count < 3)
                            {
                                string b = Regex.Replace(prices[0].Groups[1].Value.Replace("￥", ""), "<[^>]+>", "");
                                string c = fees[0].Groups[1].Value.Replace("￥", "");

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add(b);
                                lv1.SubItems.Add(c);
                                lv1.SubItems.Add("无");
                                lv1.SubItems.Add("无");
                                lv1.SubItems.Add(sales);

                                savetxt(text[i], b, c, "无", "无", sales);
                            }
                            else
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add("无");
                                lv1.SubItems.Add("无");
                                lv1.SubItems.Add("无");
                                lv1.SubItems.Add("无");
                                lv1.SubItems.Add(sales);

                                savetxt(text[i], "无", "无", "无", "无", sales);
                            }
                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }

                    Thread.Sleep(2000);
                }

                MessageBox.Show("完成");

                // Thread.Sleep(1000);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }
        #endregion



    }
}
