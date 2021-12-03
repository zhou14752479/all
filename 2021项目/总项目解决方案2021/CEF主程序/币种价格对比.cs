using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using myDLL;

namespace CEF主程序
{
    public partial class 币种价格对比 : Form
    {
        public 币种价格对比()
        {
            InitializeComponent();
        }
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
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
        string path = AppDomain.CurrentDomain.BaseDirectory;



        public string getdodoprice_buy(string addr)
        {
            string url = "https://route-api.dodoex.io/dodoapi/getdodoroute?fromTokenAddress=0x55d398326f99059ff775485246999027b3197955&fromTokenDecimals=18&toTokenAddress="+addr+"&toTokenDecimals=18&fromAmount=1000000000000000000000&slippage=3&userAddr=0x0000000000000000000000000000000000000000&chainId=56&deadLine=1636704923&source=dodoV2AndMixWasm&apiKey=d61f2eb1ce8d";
            string html = GetUrl(url,"utf-8");
            string price = Regex.Match(html, @"resPricePerToToken"":([\s\S]*?),").Groups[1].Value;
            string resAmount = Regex.Match(html, @"resAmount"":([\s\S]*?),").Groups[1].Value;
            return price+"#"+ resAmount;
        }


        public string getdodoprice_sell(string addr,string amount)
        {
            string url = "https://route-api.dodoex.io/dodoapi/getdodoroute?fromTokenAddress="+addr+"&fromTokenDecimals=18&toTokenAddress=0x55d398326f99059ff775485246999027b3197955&toTokenDecimals=18&fromAmount="+amount+"000000000000000000&slippage=3&userAddr=0x0000000000000000000000000000000000000000&chainId=56&deadLine=1636704923&source=dodoV2AndMixWasm&apiKey=d61f2eb1ce8d";
            string html = GetUrl(url, "utf-8");
            string price = Regex.Match(html, @"resPricePerFromToken"":([\s\S]*?),").Groups[1].Value;
           // string resAmount = Regex.Match(html, @"resAmount"":([\s\S]*?),").Groups[1].Value;
            return price;
        }



        private void 币种价格对比_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"Z4LaV"))
            {

                return;
            }



            #endregion
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                string file = path + "//币种//asce.xlsx";

               

                DataTable dt = method.ExcelToDataTable(file, false);
             
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string name = dt.Rows[i][0].ToString().Trim();
                    string addr= dt.Rows[i][1].ToString().Trim();
                    string url = dt.Rows[i][2].ToString().Trim();
                    ListViewItem lv1 = listView1.Items.Add(name); //使用Listview展示数据                                                     
                    lv1.SubItems.Add(addr);
                    lv1.SubItems.Add(url);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                }


                string file2 = path + "//币种//mxec.xlsx";

                DataTable dt2 = method.ExcelToDataTable(file2, false);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {

                    string name = dt2.Rows[i][0].ToString().Trim();
                    string addr = dt2.Rows[i][1].ToString().Trim();
                    string url = dt2.Rows[i][2].ToString().Trim();
                    ListViewItem lv2 = listView2.Items.Add(name); //使用Listview展示数据                                                     
                    lv2.SubItems.Add(addr);
                    lv2.SubItems.Add(url);
                    lv2.SubItems.Add("");
                    lv2.SubItems.Add("");
                    lv2.SubItems.Add("");
                }



            }

            catch (Exception ex)
            {

                MessageBox.Show("币种文件不存在");
            }
        }


        public void getprice(ChromiumWebBrowser browser)
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("function tempFunction() {");
                sb.AppendLine(" return document.body.innerHTML; ");
                // sb.AppendLine(" return document.getElementsByTagName('body')[0].outerHTML; ");
                sb.AppendLine("}");
                sb.AppendLine("tempFunction();");
                var task01 = browser.GetBrowser().GetFrame(browser.GetBrowser().GetFrameNames()[0]).EvaluateScriptAsync(sb.ToString());
                task01.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        if (response.Success == true)
                        {
                            if (response.Result != null)
                            {
                                //string resultStr = response.Result.ToString();
                                string resultStr = GetHtmlFromChromiumWebBrowser(browser);


                                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                                {

                                    if (listView1.CheckedItems[i].SubItems[0].Text == browser.Name)
                                    {
                                        try
                                        {
                                            string addr = listView1.CheckedItems[i].SubItems[1].Text;
                                            MatchCollection prices = Regex.Matches(resultStr, @"data-price=""([\s\S]*?)""");
                                            MatchCollection counts = Regex.Matches(resultStr, @"<span data-v-41faa446="""">([\s\S]*?)</span>");
                                            if (prices.Count > 100)
                                            {
                                                listView1.CheckedItems[i].BackColor = Color.White;
                                                string buy_price = prices[100].Groups[1].Value;
                                                string buy_count = counts[100].Groups[1].Value;

                                                string[] text = getdodoprice_buy(addr).Split(new string[] { "#" }, StringSplitOptions.None);
                                                string buy_price_dodo = Convert.ToDouble(text[0]).ToString("0.000000");
                                                string buy_amount_dodo = Convert.ToDouble(text[1]).ToString("0.000000");

                                                string sell_price_dodo = getdodoprice_sell(addr, Math.Floor(Convert.ToDouble(buy_amount_dodo)).ToString());
                                                string sell_price = prices[0].Groups[1].Value;
                                                // string sell_count = counts[0].Groups[1].Value;


                                                listView1.CheckedItems[i].SubItems[3].Text = buy_price_dodo;
                                                listView1.CheckedItems[i].SubItems[4].Text = buy_amount_dodo;
                                                listView1.CheckedItems[i].SubItems[5].Text = sell_price_dodo;


                                                //上面红色 下面绿色
                                                //交易所买入价(即红色部分的价）
                                                //交易所卖出价(即绿色部分的价）
                                                double price_bili = 1 + Convert.ToDouble(Convert.ToDouble(textBox2.Text) / 100);
                                                double amount_bili = Convert.ToDouble(Convert.ToDouble(textBox3.Text) / 100);

                                                double price_bili2 =Convert.ToDouble(Convert.ToDouble(100 - Convert.ToDouble(textBox2.Text)) / 100);

                                              
                                                if (Convert.ToDouble(buy_price) > Convert.ToDouble(buy_price_dodo) * price_bili && Convert.ToDouble(buy_count) >= Convert.ToDouble(buy_amount_dodo) * amount_bili)
                                                {
                                                    listView1.CheckedItems[i].BackColor = Color.Green;
                                                    log(DateTime.Now.ToLongTimeString() +" "+ listView1.CheckedItems[i].SubItems[0].Text + "--交易所卖出价：" + buy_price + "--DODO买入价：" + buy_price_dodo);
                                                    Beep(800, 500);

                                                }
                                                if (Convert.ToDouble(sell_price) < Convert.ToDouble(sell_price_dodo) * price_bili2 && Convert.ToDouble(buy_count) >= Convert.ToDouble(buy_amount_dodo) * amount_bili)
                                                {
                                                    listView1.CheckedItems[i].BackColor = Color.Red;
                                                    log(DateTime.Now.ToLongTimeString() + " "+listView1.CheckedItems[i].SubItems[0].Text+"--交易所买入价：" + sell_price + "--DODO卖出价：" + sell_price_dodo);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                            ex.ToString();
                                        }
                                    }
                                }
                                //textBox4.Text = resultStr;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            GetControls(tabControl1);

        }

        private string GetHtmlFromChromiumWebBrowser(ChromiumWebBrowser browser)
        {
            Task<String> TaskHtml = browser.GetBrowser().MainFrame.GetSourceAsync();

            string HtmlResponse = TaskHtml.Result;

            return HtmlResponse;

        }


        public void log(string value)
        {

            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\币种\\log.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(value);
            sw.Close();
            fs1.Close();
            sw.Dispose();
        }


        private void GetControls(Control fatherControl)
        {
            Control.ControlCollection sonControls = fatherControl.Controls;
            //遍历所有控件
            foreach (Control control in sonControls)
            {
                if (control is ChromiumWebBrowser)
                {
                    ChromiumWebBrowser browser = (ChromiumWebBrowser)control;
                    getprice(browser);
                }

                if (control.Controls != null)
                {
                    GetControls(control);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetControls(tabControl1);
        }


        List<string> lists = new List<string>();
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string coin = listView1.CheckedItems[i].SubItems[0].Text;
                string url = listView1.CheckedItems[i].SubItems[2].Text;
                if (!lists.Contains(url))
                {
                    lists.Add(url);
                    TabPage tab = new TabPage();



                    ChromiumWebBrowser browser = new ChromiumWebBrowser(url);
                    browser.Dock = DockStyle.Fill;
                    tab.Controls.Add(browser);
                    tab.Text = coin;
                    browser.Name = coin;
                    tab.Name = coin;
                    tabControl1.TabPages.Add(tab);
                    // browser.FrameLoadEnd += Browser_FrameLoadEnd; //调用加载事件
                }

            }
        }


        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                //for (int i = 0; i < text.Length; i++)
                //{
                //    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                //    lv1.SubItems.Add(text[i]);
                //}
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void 移除此币种ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage page = tabControl1.SelectedTab;
            tabControl1.TabPages.Remove(page);
        }
    }
}
