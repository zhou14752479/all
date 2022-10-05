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

namespace 药房网
{
    public partial class 药房网 : Form
    {
        public 药房网()
        {
            InitializeComponent();
        }

        private void 药房网_Load(object sender, EventArgs e)
        {

        }
        DataTable dt = null;
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            // openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, true);

            }
        }




        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
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
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion



        public void run1()
        {
            try
            {
                foreach (string cate in cates)
                {

                    for (int page = 1; page < 101; page++)
                    {
                        string url = "https://www.yaofangwang.com/catalog-" + cate + "-p" + page + ".html";
                        string html = method.GetUrl(url, "utf-8");
                        MatchCollection ids = Regex.Matches(html, @"<div class=""info medicineInfo"" rel=""([\s\S]*?)""");


                        for (int j = 0; j < ids.Count; j++)
                        {

                            string id = ids[j].Groups[1].Value;
                            textBox2.Text = "正在读取：" + id;
                            string aurl = "https://pub.yaofangwang.com/4000/4000/0/?__cmd=guest.medicine.getMedicineDetail%20as%20goodsInfo%2Cguest.medicine.getShopMedicines%20as%20shopsInfo&__params=%7B%22goodsInfo%22%3A%7B%22mid%22%3A%22" + id + "%22%7D%2C%22shopsInfo%22%3A%7B%22conditions%22%3A%7B%22sort%22%3A%22sprice%22%2C%22sorttype%22%3A%22asc%22%2C%22medicineid%22%3A%22" + id + "%22%7D%2C%22pageSize%22%3A1%2C%22pageIndex%22%3A1%7D%7D";
                            string ahtml = method.GetUrl(aurl, "utf-8");

                            MatchCollection stores = Regex.Matches(ahtml, @"""store_title"":""([\s\S]*?)""");
                            MatchCollection store_prices = Regex.Matches(ahtml, @"""real_price"":([\s\S]*?),");

                            string title = Regex.Match(ahtml, @"""medicine_name"":""([\s\S]*?)""").Groups[1].Value;
                            string guige = Regex.Match(ahtml, @"""standard"":""([\s\S]*?)""").Groups[1].Value;
                            string changjia = Regex.Match(ahtml, @"生产厂家：([\s\S]*?)""").Groups[1].Value;

                            string guoyaozhunzi = Regex.Match(ahtml, @"""authorized_code"":""([\s\S]*?)""").Groups[1].Value;
                            string price_min = Regex.Match(ahtml, @"""price_min"":([\s\S]*?),").Groups[1].Value;

                            for (int a = 0; a < stores.Count; a++)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(title);
                                lv1.SubItems.Add(guige);
                                lv1.SubItems.Add(changjia);
                                lv1.SubItems.Add(guoyaozhunzi);

                               // lv1.SubItems.Add(stores[a].Groups[1].Value);
                                lv1.SubItems.Add(store_prices[a].Groups[1].Value);

                               // lv1.SubItems.Add(price_min);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }


                        }

                        Thread.Sleep(500);

                    }
                }

            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }


        }



        public void run2()
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    //string keyword = System.Web.HttpUtility.UrlEncode(dr[1].ToString());
                    string keyword = dr[1].ToString();
                    textBox2.Text = "正在读取：" + keyword;
                    string url = "https://www.yaofangwang.com/search.html?keyword=" + keyword;
                    string html = method.GetUrl(url, "utf-8");
                    MatchCollection ids = Regex.Matches(html, @"<div class=""info medicineInfo"">([\s\S]*?)medicine\/([\s\S]*?)\/");
                    MessageBox.Show(ids.Count.ToString());

                    for (int j = 0; j < ids.Count; j++)
                    {
                        string id = ids[j].Groups[2].Value;

                        string aurl = "https://pub.yaofangwang.com/4000/4000/0/?__cmd=guest.medicine.getMedicineDetail%20as%20goodsInfo%2Cguest.medicine.getShopMedicines%20as%20shopsInfo&__params=%7B%22goodsInfo%22%3A%7B%22mid%22%3A%22" + id + "%22%7D%2C%22shopsInfo%22%3A%7B%22conditions%22%3A%7B%22sort%22%3A%22sprice%22%2C%22sorttype%22%3A%22asc%22%2C%22medicineid%22%3A%22" + id + "%22%7D%2C%22pageSize%22%3A1%2C%22pageIndex%22%3A1%7D%7D";
                        string ahtml = method.GetUrl(aurl, "utf-8");

                        MatchCollection stores = Regex.Matches(ahtml, @"""store_title"":""([\s\S]*?)""");
                        MatchCollection store_prices = Regex.Matches(ahtml, @"""real_price"":([\s\S]*?),");

                        string title = Regex.Match(ahtml, @"""medicine_name"":""([\s\S]*?)""").Groups[1].Value;
                        string guige = Regex.Match(ahtml, @"""standard"":""([\s\S]*?)""").Groups[1].Value;
                        string changjia = Regex.Match(ahtml, @"生产厂家：([\s\S]*?)""").Groups[1].Value;

                        string guoyaozhunzi = Regex.Match(ahtml, @"""authorized_code"":""([\s\S]*?)""").Groups[1].Value;
                        string price_min = Regex.Match(ahtml, @"""price_min"":([\s\S]*?),").Groups[1].Value;

                        for (int a = 0; a < stores.Count; a++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title);
                            lv1.SubItems.Add(guige);
                            lv1.SubItems.Add(changjia);
                            lv1.SubItems.Add(guoyaozhunzi);

                            lv1.SubItems.Add(stores[a].Groups[1].Value);
                            lv1.SubItems.Add(store_prices[a].Groups[1].Value);

                           // lv1.SubItems.Add(price_min);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }


                    }

                    Thread.Sleep(500);



                }
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }


        }

        bool zanting = true;
        bool status = true;
        Thread thread;

        string[] cates = { "10","15","12", "29", "206", "30", "11", "17", "14", "19", "16", "20" };
        public void run_xcx()
        {
            try
            {
                foreach (string cate in cates)
                {

                    for (int page = 1; page < 999; page++)
                    {

                        string aurl = "https://dian.ysbang.cn/wholesale-drug/sales/getWholesaleList/v4270";
                        string postdata = "";
                        string ahtml = method.PostUrl(aurl,postdata,"", "utf-8", "application/json", "");

                        MatchCollection names = Regex.Matches(ahtml, @"""namecn"":""([\s\S]*?)""");
                        MatchCollection standards = Regex.Matches(ahtml, @"""standard"":""([\s\S]*?)""");
                        MatchCollection mill_title = Regex.Matches(ahtml, @"""mill_title"":""([\s\S]*?)""");
                        MatchCollection authorized_code = Regex.Matches(ahtml, @"""authorized_code"":""([\s\S]*?)""");
                        MatchCollection min_price = Regex.Matches(ahtml, @"""min_price"":([\s\S]*?),");
                        if (names.Count == 0)
                            break;
                        for (int a = 0; a < names.Count; a++)
                        {
                            try
                            {
                                textBox2.Text = "正在读取：" + names[a].Groups[1].Value;
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(names[a].Groups[1].Value);
                                lv1.SubItems.Add(standards[a].Groups[1].Value);
                                lv1.SubItems.Add(mill_title[a].Groups[1].Value);
                                lv1.SubItems.Add(authorized_code[a].Groups[1].Value);
                                lv1.SubItems.Add(min_price[a].Groups[1].Value);

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
                            }
                            catch (Exception)
                            {

                               continue;
                            }
                        }



                        Thread.Sleep(1000);

                    }
                }

            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"kOuseov"))
            {

                return;
            }



            #endregion
            //if (radioButton1.Checked == true)
            //{
            //    if (thread == null || !thread.IsAlive)
            //    {
            //        thread = new Thread(run1);
            //        thread.Start();
            //        Control.CheckForIllegalCrossThreadCalls = false;
            //    }

            //}
            //if (radioButton2.Checked == true)
            //{
            //    if (thread == null || !thread.IsAlive)
            //    {
            //        thread = new Thread(run2);
            //        thread.Start();
            //        Control.CheckForIllegalCrossThreadCalls = false;
            //    }

            //}

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_xcx);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            textBox2.Text = "开始抓取......";
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

        private void 药房网_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
