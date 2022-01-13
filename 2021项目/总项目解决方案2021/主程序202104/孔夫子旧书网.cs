using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202104
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




        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string charset)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.Proxy = null;//防止代理抓包

                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "IOS_KFZ_COM_3.10.0_iPhone14,5_15.1.1 #App Store,6FB137584B08435A8842DEF8F4E8D38E";
                request.Headers.Add("Cookie", "acw_tc=276077d616418046192976172ebd1e7fae6ea4270c4763e88726b0f3a719f5");

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        public string getdingjia(string url)
        {
           
            string html = method.GetUrl(url, "utf-8");
           string price = Regex.Match(html, @"orgprice"" content=""([\s\S]*?)""").Groups[1].Value;
            return price;
        }



      

        string shopname = "";
        /// <summary>
        /// 全部价格
        /// </summary>
        public void run()
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请导入店铺网址txt");
                return;
            }


            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
            if(text[i]=="")
                {
                    continue;
                }

                for (int page = 1; page < 9999; page++)
                {
                    try
                    {
                        label2.Text = "正在抓取第：" + page + "页";

                        string shopid = Regex.Match(text[i], @"\d{4,}").Groups[0].Value;

                        string url = "http://shop.kongfz.com/" + shopid + "/all/0_50_0_0_" + page + "_sort_desc_0_0/";

                        string html = method.GetUrl(url, "utf-8");
                        shopname= Regex.Match(html, @"<div class=""shop_top_text"">([\s\S]*?)</div>").Groups[1].Value;
                        MatchCollection itemids = Regex.Matches(html, @"<div class=""item-row clearfix""([\s\S]*?)itemid=""([\s\S]*?)""");
                        MatchCollection userids = Regex.Matches(html, @"<div class=""item-row clearfix""([\s\S]*?)userid=""([\s\S]*?)""");
                        MatchCollection isbns = Regex.Matches(html, @"<div class=""item-row clearfix""([\s\S]*?)isbn=""([\s\S]*?)""");
                        MatchCollection names = Regex.Matches(html, @"class=""row-name"">([\s\S]*?)</a>");
                        MatchCollection prices = Regex.Matches(html, @"<div class=""row-price"">([\s\S]*?)</div>");
                        MatchCollection pinxiangs = Regex.Matches(html, @"<div class=""row-quality"">([\s\S]*?)</div>");
                        MatchCollection times = Regex.Matches(html, @"<div class=""add-time-box"">([\s\S]*?)</div>");
                        if (names.Count == 0)
                        {
                            label2.Text = "采集结束";
                            break;
                        }



                        StringBuilder sb = new StringBuilder();
                        for (int a= 0; a< itemids.Count; a++)
                        {
                            sb.Append("%7B%22userId%22%3A%22"+userids[a].Groups[2].Value+"%22%2C%22itemId%22%3A%22"+ itemids[a].Groups[2].Value+ "%22%7D%2C");
                        }
                        
                        string aurl = "https://shop.kongfz.com/book/shopsearch/getShippingFee?callback=jQuery111208550195114156878_1633577971044&params=%7B%22params%22%3A%5B"+sb.ToString().Remove(sb.ToString().Length-3,3)+"%5D%2C%22area%22%3A%221006000000%22%7D&_=1633577971050";
                        textBox1.Text = aurl;
                        string ahtml = method.GetUrl(aurl, "utf-8");
                     
                        MatchCollection fees = Regex.Matches(ahtml, @"""totalFee"":""([\s\S]*?)""");







                        for (int j = 0; j < names.Count; j++)
                        {


                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(Regex.Replace(isbns[j].Groups[2].Value.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(names[j].Groups[1].Value.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(prices[j].Groups[1].Value.Trim(), "<[^>]+>", "").Replace("￥", ""));

                            // string aurl = "http://book.kongfz.com/" + id + "/" + aids[j].Groups[2].Value + "/";
                            //lv1.SubItems.Add(getdingjia(aurl));


                          
                            lv1.SubItems.Add(Regex.Replace(pinxiangs[j].Groups[1].Value.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(fees[j].Groups[1].Value.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(shopname);
                            lv1.SubItems.Add(Regex.Replace(times[j].Groups[1].Value.Trim(), "<[^>]+>", "").Replace("上书","").Trim());
                            Thread t = new Thread(() =>
                            {
                                try
                                {

                                    string bhtml = PostUrl("https://app.kongfz.com/invokeBook/app/api/getItemInfo", "itemId=" + itemids[j].Groups[2].Value + "&shopId=" + shopid, "utf-8");
                                    bhtml = method.Unicode2String(bhtml);
                                    string dingjia = Regex.Match(bhtml, @"定价""([\s\S]*?)c"":""([\s\S]*?)""").Groups[2].Value.Trim().Replace("元", "");
                                    string kucun = Regex.Match(bhtml, @"""stock"":""([\s\S]*?)""").Groups[1].Value.Trim();
                                    lv1.SubItems.Add(dingjia);
                                    lv1.SubItems.Add(kucun);

                                }
                                catch (Exception ex)
                                {
                                    lv1.SubItems.Add("");
                                    lv1.SubItems.Add("");
                                }

                            });
                            t.Start();

                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(100);

                        }
                        Thread.Sleep(1000);

                    }


                    catch (Exception ex)
                    {

                        //MessageBox.Show(ex.ToString());
                    }
                }
                string path = AppDomain.CurrentDomain.BaseDirectory+"\\excel\\";
                if (checkBox1.Checked == true)
                {
                    method.DataTableToExcelName(method.listViewToDataTable(this.listView1), path + shopname + ".xlsx", true);
                    listView1.Items.Clear();
                }
            }
            label2.Text = "采集结束";



        }
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            ////string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            ////if (!html.Contains(@"OaO6Wx"))
            ////{

            ////    return;
            ////}



            #endregion
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入店铺网址");
                return;
            }
            if(DateTime.Now>Convert.ToDateTime("2022-02-20"))
            {
                MessageBox.Show("余额不足");
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 孔夫子旧书网_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 孔夫子旧书网_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }
    }
}
