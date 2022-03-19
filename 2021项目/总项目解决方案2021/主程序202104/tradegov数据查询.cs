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
using CsharpHttpHelper;
using myDLL;

namespace 主程序202104
{
    public partial class tradegov数据查询 : Form
    {
        public tradegov数据查询()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }

        string cookie = "";
        private string Request_trade_gov_ng(string url)
        {
            
                HttpHelper http = new HttpHelper();
               HttpItem item = new HttpItem()
                {
                    URL = url,
                    Method = "GET",
                    Host = "trade.gov.ng",
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36",
                    Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                    Cookie =cookie ,
                };
               
            item.Header.Add("sec-ch-ua-mobile", "?0");
            item.Header.Add("Sec-Fetch-Site", "none");
            item.Header.Add("Sec-Fetch-Mode", "navigate");
            item.Header.Add("Sec-Fetch-User", "?1");
            item.Header.Add("Sec-Fetch-Dest", "document");
            item.Header.Add("Accept-Encoding", "gzip, deflate, br");
            item.Header.Add("Accept-Language", "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
           HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        

    }

        DataTable dt = null;
    public void run1()
        {
            cookie = method.getSetCookie("https://trade.gov.ng/trade/son/search.do?parameters%5B0%5D.type=STRING&parameters%5B0%5D.name=referenceNumber&parameters%5B0%5D.operation=EQUALS&parameters%5B0%5D.value%5B0%5D=NGA-SCN20-019601-V0&type=SC&method%3Asearch=Search");

       
            try
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                  
                    DataRow dr = dt.Rows[i];
                    string shuru = System.Web.HttpUtility.UrlEncode(dr[0].ToString());
                   // MessageBox.Show(dr[0].ToString());
                    string url = "https://trade.gov.ng/trade/son/search.do?parameters%5B0%5D.type=STRING&parameters%5B0%5D.name=referenceNumber&parameters%5B0%5D.operation=EQUALS&parameters%5B0%5D.value%5B0%5D="+shuru+"&type=SC&method%3Asearch=Search";
                   // string url = "https://trade.gov.ng/son/print.do?type=SC&number="+shuru;
                    string html = Request_trade_gov_ng(url);
                  
                    Match a1 =  Regex.Match(html, @"Exporter's Name:</td>([\s\S]*?)</td>");
                    MatchCollection address = Regex.Matches(html, @"Address:</td>([\s\S]*?)</td>");
                    Match a2 = Regex.Match(html, @"Importer's Name:</td>([\s\S]*?)</td>");
                    Match a3 = Regex.Match(html, @"Destination Port:<br>([\s\S]*?)</td>");
                    Match a4 = Regex.Match(html, @"Country of Supply:<br>([\s\S]*?)</td>");
                    Match a5 = Regex.Match(html, @"SONCO Ref. No.:<br>([\s\S]*?)</td>");
                    Match a6 = Regex.Match(html, @"Invoice No:<br>([\s\S]*?)</td>");
                    Match a7 = Regex.Match(html, @"SON Country Office:<br>([\s\S]*?)</td>");
                    Match a8 = Regex.Match(html, @"Country of Origin:<br>([\s\S]*?)</td>");
                    Match a9 = Regex.Match(html, @"BL or AWB No.:<br>([\s\S]*?)</td>");
                    Match a10 = Regex.Match(html, @"Carrier: <br>([\s\S]*?)</td>");

                    string ahtml= Regex.Match(html, @"soncap-bg items"">([\s\S]*?)</div>").Groups[1].Value;
                    MatchCollection products = Regex.Matches(ahtml, @"<td>([\s\S]*?)</td>");

                    StringBuilder sb = new StringBuilder();
                    for (int a = 0; a < products.Count/3; a++)
                    {
                        sb.Append(products[3*a].Groups[1].Value+"  "+ products[(3 * a)+1].Groups[1].Value+"  "+ products[(3 * a)+2].Groups[1].Value+"\r\n");
                    }

                    if (address.Count > 1)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                        lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(address[0].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(address[1].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(sb.ToString());
                        Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }





                }
                method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }



        public void run2()
        {
            cookie = method.getSetCookie("https://trade.gov.ng/trade/son/search.do?parameters%5B0%5D.type=STRING&parameters%5B0%5D.name=referenceNumber&parameters%5B0%5D.operation=EQUALS&parameters%5B0%5D.value%5B0%5D=NGA-SCN20-019601-V0&type=PC&method%3Asearch=Search");


            try
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    string shuru =System.Web.HttpUtility.UrlEncode(dr[0].ToString());

                    string url = "https://trade.gov.ng/trade/son/search.do?parameters%5B0%5D.type=STRING&parameters%5B0%5D.name=referenceNumber&parameters%5B0%5D.operation=EQUALS&parameters%5B0%5D.value%5B0%5D="+shuru+"&type=PC&method%3Asearch=Search";
                   // string url = "https://trade.gov.ng/son/print.do?type=PC&number="+shuru;
                    string html = Request_trade_gov_ng(url);

                    Match a1 = Regex.Match(html, @"Product Certificate No.:</td>([\s\S]*?)</td>");
                   
                    Match a2 = Regex.Match(html, @"Issued to:</td>([\s\S]*?)</td>");
                    Match a3 = Regex.Match(html, @"Issued Date:</td>([\s\S]*?)</td>");
                    Match a4 = Regex.Match(html, @"Remarks:</td>([\s\S]*?)</td>");
                    Match a5 = Regex.Match(html, @"Issued by</strong>:</td>([\s\S]*?)</td>");


                    string ahtml = Regex.Match(html, @"soncap-bg items"">([\s\S]*?)</div>").Groups[1].Value;
                    MatchCollection products = Regex.Matches(ahtml, @"<td>([\s\S]*?)</td>");

                    StringBuilder sb = new StringBuilder();
                    for (int a = 0; a < products.Count / 4; a++)
                    {
                        sb.Append(products[4 * a].Groups[1].Value + "  " + products[(4 * a) + 1].Groups[1].Value + "  " + products[(4 * a) + 2].Groups[1].Value + "  " + products[(4 * a) + 3].Groups[1].Value + "\r\n");
                    }


                    ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(dr[0].ToString());
                    lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());

                    lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());

                    lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                    lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                    lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                    lv1.SubItems.Add(sb.ToString());

                    Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;





                }
                method.DataTableToExcelTime(method.listViewToDataTable(this.listView2), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }


        public void run3()
        {
            cookie = method.getSetCookie("https://trade.gov.ng/cbn/formm/search.do?parameters%5B0%5D.type=STRING&parameters%5B0%5D.name=code&parameters%5B0%5D.operation=EQUALS&parameters%5B0%5D.value%5B0%5D=MF20170091126&method%3Asearch=Search");

          
            try
            {
               
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    try
                    {
                        DataRow dr = dt.Rows[i];
                        string shuru = System.Web.HttpUtility.UrlEncode(dr[0].ToString());
                       
                        string url = "https://trade.gov.ng/cbn/formm/search.do?parameters%5B0%5D.type=STRING&parameters%5B0%5D.name=code&parameters%5B0%5D.operation=EQUALS&parameters%5B0%5D.value%5B0%5D=" + shuru + "&method%3Asearch=Search";

                        string html = Request_trade_gov_ng(url);

                        MatchCollection a1s = Regex.Matches(html, @"<td class="""" style="""">([\s\S]*?)</td>");


                        if (a1s.Count > 5)
                        {

                            ListViewItem lv1 = listView3.Items.Add((listView3.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(Regex.Replace(a1s[0].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a1s[1].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a1s[2].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a1s[3].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a1s[4].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a1s[5].Groups[1].Value, "<[^>]+>", "").Trim());

                            Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }

                        method.DataTableToExcelTime(method.listViewToDataTable(this.listView3), true);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }




                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        
        private void tradegov数据查询_Load(object sender, EventArgs e)
        {
            

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入表格");
                return;
            }
            dt = method.ExcelToDataTable(textBox1.Text, true);

            status = true;
            if (radioButton1.Checked == true)
            {
                tabControl1.SelectedIndex = 0;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run1);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton2.Checked == true)
            {
                tabControl1.SelectedIndex = 1;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run2);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton3.Checked == true)
            {
                tabControl1.SelectedIndex = 2;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run3);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
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
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"JM3wXz"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

           
            if (radioButton1.Checked == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            }
            if (radioButton2.Checked == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
            }
            if (radioButton3.Checked == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView3), "Sheet1", true);
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
        }

        private void tradegov数据查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (textBox4.Text == ""|| textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("请输入组合");
                return;
            }


            dt = new DataTable();
            DataColumn Column = new DataColumn("value");
          
            dt.Columns.Add(Column);

            for (int i = Convert.ToInt32(textBox5.Text); i < Convert.ToInt32(textBox6.Text); i++)
            {
                DataRow newRow  = dt.NewRow();

                newRow[0] = textBox4.Text+i+textBox7.Text;
                dt.Rows.Add(newRow);
            }


            status = true;
            if (radioButton1.Checked == true)
            {
                tabControl1.SelectedIndex = 0;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run1);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton2.Checked == true)
            {
                tabControl1.SelectedIndex = 1;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run2);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton3.Checked == true)
            {
                tabControl1.SelectedIndex = 2;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run3);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }



    }
}
