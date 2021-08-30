using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace CEF主程序
{
    public partial class 流量抓取 : Form
    {
        public 流量抓取()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser browser;
        public static string json;
        private void 流量抓取_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("https://m.ok6.icu/");
            // Cef.Initialize(new CefSettings());


            Control.CheckForIllegalCrossThreadCalls = false;
            splitContainer1.Panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;

       
            browser.RequestHandler = new WinFormsRequestHandler();//request请求的具体实现
    

        }

        Dictionary<string, double> dics = new Dictionary<string, double>();
        public void run()
        {
          
            if (json != null && json != "")
            {
               

                json = method.Unicode2String(json);
               
                listView1.Columns.Clear();
                MatchCollection names = Regex.Matches(json, @"""name"":""([\s\S]*?)""");
                listView1.Columns.Add("ID", 150, HorizontalAlignment.Center);
                listView1.Columns.Add("时间", 150, HorizontalAlignment.Center);

                listView2.Columns.Add("ID", 150, HorizontalAlignment.Center);
                listView2.Columns.Add("时间", 150, HorizontalAlignment.Center);
                for (int i = 0; i < names.Count; i++)
                {
                    listView1.Columns.Add(names[i].Groups[1].Value, 150, HorizontalAlignment.Center);
                    listView2.Columns.Add(names[i].Groups[1].Value, 150, HorizontalAlignment.Center);
                }

                string value = Regex.Match(流量抓取.json, "labels\":\\[([\\s\\S]*?)\\]").Groups[1].Value;
                string[] array = value.Split(new string[]{ ","}, StringSplitOptions.None);
                MatchCollection matchCollection = Regex.Matches(流量抓取.json, "data\":\\[([\\s\\S]*?)\\]");

                string riqi = "";
                for (int i = 0; i < array.Length; i++)
                {
                    ListViewItem listViewItem = this.listView1.Items.Add(this.listView1.Items.Count.ToString());
                    string date = array[i].Replace("\"", "");

                    if (date.Contains("日"))
                    {
                        riqi = Regex.Match(date, @"\d{2}月\d{2}日").Groups[0].Value;
                        listViewItem.SubItems.Add(riqi);
                        dics.Add(riqi,0);
                    }
                    else
                    {
                        if (Convert.ToInt32(date) >= DateTime.Now.Hour)
                        {
                            listViewItem.SubItems.Add(riqi);
                        }
                        else
                        {
                            listViewItem.SubItems.Add(Convert.ToDateTime(riqi).AddDays(1).ToString("MM月dd日"));
                        }
                    }
                   
                   
                  

                    for (int j = 0; j < matchCollection.Count; j++)
                    {
                        string[] array2 = matchCollection[j].Groups[1].Value.Split(new string[]{","}, StringSplitOptions.None);

                        long num = Convert.ToInt64(array2[i].Replace("\"", ""));
                            double num2 = (double)(num / 1024L);
                        double value2 = num2 / 1024.0;
                        value2 = Math.Round(value2, 2);
                        listViewItem.SubItems.Add(value2.ToString() + "MB");



                    }
                }

                huizong();
            }
        }


        public void huizong()
        {
            foreach (var item in dics.Keys)
            {
              
                ListViewItem listViewItem = this.listView2.Items.Add(this.listView2.Items.Count.ToString());
                listViewItem.SubItems.Add(item);
                for (int a = 2; a < listView1.Columns.Count; a++)
                {

                
                    double all = 0;

                   
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                      

                        if (listView1.Items[i].SubItems[1].Text.Trim() == item.Trim())
                        {
                            all = all + Convert.ToDouble(listView1.Items[i].SubItems[a].Text.Replace("MB", ""));
                        }

                    }
                    if (all > 1024)
                    {
                        all = all / 1024;
                        all=Math.Round(all, 2);
                        listViewItem.SubItems.Add(all.ToString() + "GB");
                    }
                    else
                    {
                        listViewItem.SubItems.Add(all.ToString()+"MB");
                    }
                   
                }

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"XTAeic"))
            {
                return;
            }

            #endregion

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        Thread thread;

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

      
    }
}
