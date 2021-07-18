using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 订单导出
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
        public string cookie { get; set; }
     
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString());
            MessageBox.Show("导出成功");
        }


        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }

      
        public string GetUrlWithCookie(string Url)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";

                request.Referer = Url;
                request.Headers.Add("Cookie", cookie);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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
     
        #region 主程序
        public void run()
        {
            string startdate = "";
            string enddate = "";
            if (checkBox1.Checked == true)
            {
                startdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                enddate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            }

            for (int i = Convert.ToInt32(textBox1.Text); i <= Convert.ToInt32(textBox2.Text); i++)
            {


                string url = "http://www.dadimt.com/admin/order/list.jhtml?consignee=&phone=&taobaoid=&sn=&trackingNo=&beginDate=" + startdate + "&endDate=" + enddate + "&shop=&orderStatus=&shippingStatus=&productCategory=&shippingMethodId=&aftersaleTypeId=&pageSize=20&searchProperty=&orderProperty=&orderDirection=&pageNumber=" + i;

                string html = GetUrlWithCookie(url);

                MatchCollection uids = Regex.Matches(html, @"ids"" value=""([\s\S]*?)""");
                MatchCollection tbids = Regex.Matches(html, @"class=""taobaoid"">([\s\S]*?)<");

                for (int j = 0; j < uids.Count; j++)
                {

                    string aurl = "http://www.dadimt.com/admin/order/view.jhtml?id=" + uids[j].Groups[1].Value;
                    string ahtml = Regex.Match(GetUrlWithCookie(aurl), @"小计([\s\S]*?)input tabContent").Groups[1].Value;
                    string bhtml = Regex.Matches(GetUrlWithCookie(aurl), @"<table class=""input tabContent"">([\s\S]*?)</table>")[2].Groups[1].Value;

                    try
                    {
                        MatchCollection tds = Regex.Matches(ahtml, @"<td([\s\S]*?)>([\s\S]*?)</td>");
                        MatchCollection td2s = Regex.Matches(bhtml, @"<td>([\s\S]*?)</td>");
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(tbids[j].Groups[1].Value);
                        for (int a = 0; a < tds.Count; a++)
                        {
                          
                             lv1.SubItems.Add(Regex.Replace(tds[a].Groups[2].Value, "<[^>]+>", "").Trim());
                        }

                        for (int b = 0; b < td2s.Count; b++)
                        {

                            lv1.SubItems.Add(Regex.Replace(td2s[b].Groups[1].Value, "<[^>]+>", "").Trim());
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                        Thread.Sleep(100);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

            }



        }

        #endregion
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 读取config.ini
            if (ExistINIFile())
            {
                cookie = IniReadValue("values", "cookie");
               
            }
           
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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

        private void button6_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "获取cookie.exe");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView1,true);
        }
    }
}
