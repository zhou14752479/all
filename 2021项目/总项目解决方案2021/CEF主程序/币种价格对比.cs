using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

       
        string path = AppDomain.CurrentDomain.BaseDirectory;



        public string getdodoprice_buy(string addr)
        {
            string url = "https://route-api.dodoex.io/dodoapi/getdodoroute?fromTokenAddress=0x55d398326f99059ff775485246999027b3197955&fromTokenDecimals=18&toTokenAddress="+addr+"&toTokenDecimals=8&fromAmount=1000000000000000000000&slippage=3&userAddr=0x0000000000000000000000000000000000000000&chainId=56&deadLine=1636704923&source=dodoV2AndMixWasm&apiKey=d61f2eb1ce8d";
            string html = method.GetUrl(url,"utf-8");
            string price = Regex.Match(html, @"resPricePerFromToken"":([\s\S]*?),").Groups[1].Value;
            return price;
        }






        private void 币种价格对比_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                string file = path + "//币种//币种.xlsx";

                // method.ShowDataInListView(,listView1);

                DataTable dt = method.ExcelToDataTable(file, false);
             
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string name = dt.Rows[i][0].ToString().Trim();
                    string addr= dt.Rows[i][1].ToString().Trim();
                    string url = dt.Rows[i][2].ToString().Trim();
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据                                                     
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(addr);
                    lv1.SubItems.Add(url);
                    lv1.SubItems.Add("");
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show("币种.xlsx文件不存在");
            }
        }


        public void getprice(ChromiumWebBrowser browser)
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
                                if(listView1.CheckedItems[i].SubItems[1].Text==browser.Name)
                                {
                                    try
                                    {
                                        string addr = listView1.CheckedItems[i].SubItems[2].Text;
                                        MatchCollection prices = Regex.Matches(resultStr, @"data-price=""([\s\S]*?)""");
                                        MatchCollection shuliangs = Regex.Matches(resultStr, @"<span data-v-41faa446="""">([\s\S]*?)</span>");
                                        if (prices.Count > 99)
                                        {
                                            string buy_price = prices[99].Groups[1].Value;
                                            string buy_price_dodo = getdodoprice_buy(addr);

                                            listView1.CheckedItems[i].SubItems[4].Text = buy_price + "#   "+ buy_price_dodo+"  # " + shuliangs[99].Groups[1].Value;
                                            if (Convert.ToDouble(buy_price) > Convert.ToDouble(buy_price_dodo) * 1.05)
                                            {
                                                listView1.CheckedItems[i].BackColor = Color.Green;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        ex.ToString();
                                    }
                                }
                            }
                            textBox4.Text = resultStr;
                        }
                    }
                }
            });

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

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                TabPage tab = new TabPage();

                string coin = listView1.CheckedItems[i].SubItems[1].Text;
                string url = listView1.CheckedItems[i].SubItems[3].Text;

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
}
