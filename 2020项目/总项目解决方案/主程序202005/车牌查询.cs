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
using helper;

namespace 主程序202005
{
    public partial class 车牌查询 : Form
    {
        public 车牌查询()
        {
            InitializeComponent();
        }
        string COOKIE = "";
        private void button1_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;

            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合



            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name").ToLower() == "j_username")
                {
                    e1.SetAttribute("value", textBox3.Text.Trim());
                }
                if (e1.GetAttribute("name").ToLower() == "j_password")
                {
                    e1.SetAttribute("value", textBox4.Text.Trim());
                }
            }

      


      
        }

        private void 车牌查询_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://157.122.153.67:9000/khyx/");
        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {


            try
            {
                string[] text = textBox8.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    string url = "http://157.122.153.67:9000/khyx/qth/price/quoteRenew.do?licenseNo4Renew=" + System.Web.HttpUtility.UrlEncode(text[i].Trim()) + "&licenseType4Renew=02&isOwner=true&isQuoteRenewByLicenseNo=1";

                    string html = method.gethtml(url, COOKIE);


                    MatchCollection a1s = Regex.Matches(html, @"""handlerCode"":""([\s\S]*?)""");
                    MatchCollection a2s = Regex.Matches(html, @"""relationPolicyNo"":""([\s\S]*?)""");
                    MatchCollection a3s = Regex.Matches(html, @"""insuredName"":""([\s\S]*?)""");
                    MatchCollection a4s = Regex.Matches(html, @"""licenseNo"":""([\s\S]*?)""");
                    MatchCollection a5s = Regex.Matches(html, @"""endDate"":""([\s\S]*?)""");
                    MatchCollection a6s = Regex.Matches(html, @"""lastDamagedBI"":([\s\S]*?),");
                    MatchCollection a7s = Regex.Matches(html, @"""lastDamagedCI"":([\s\S]*?),");
                    MatchCollection a8s = Regex.Matches(html, @"""noDamYearsBI"":([\s\S]*?),");
                    MatchCollection a9s = Regex.Matches(html, @"""noDamYearsCI"":([\s\S]*?),");
                    MatchCollection a10s = Regex.Matches(html, @"""comCode"":""([\s\S]*?)""");

                    if (a1s.Count == 0)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        lv1.SubItems.Add("无");
                        continue; 
                    }


                    for (int j = a1s.Count-1; j >=0; j--)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1s[j].Groups[1].Value);
                        lv1.SubItems.Add(a2s[j].Groups[1].Value);
                        lv1.SubItems.Add(a3s[j].Groups[1].Value);
                        lv1.SubItems.Add(a4s[j].Groups[1].Value);
                        lv1.SubItems.Add(a5s[j].Groups[1].Value);
                        lv1.SubItems.Add(a6s[j].Groups[1].Value);
                        lv1.SubItems.Add(a7s[j].Groups[1].Value);
                        lv1.SubItems.Add(a8s[j].Groups[1].Value);
                        lv1.SubItems.Add(a9s[j].Groups[1].Value);
                        lv1.SubItems.Add(a10s[j].Groups[1].Value);


                    }
                }




            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"chepaichaxun"))
            {
             
                COOKIE = method.GetCookies("http://157.122.153.67:9000/khyx/logon.do");
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox8.Text += array[i] + "\r\n";

                }

            }
        }
    }
}
