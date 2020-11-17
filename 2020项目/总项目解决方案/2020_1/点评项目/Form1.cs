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


namespace 点评项目
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
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
        string COOKIE = "_lxsdk_cuid=172415f6ef4c8-07c00d4610f257-6373664-1fa400-172415f6ef5c8; _lxsdk=172415f6ef4c8-07c00d4610f257-6373664-1fa400-172415f6ef5c8; _hc.v=189a18fa-f5f0-a2d1-15fa-43abc1680af2.1602832655; s_ViewType=10; ta.uuid=1317006582533660762; isUuidUnion=true; iuuid=172415f6ef4c8-07c00d4610f257-6373664-1fa400-172415f6ef5c8; dper=d27b58b505d7b0bd250acd4db2d096619c7ed2104a1af72aef9bf4f4ad9326ba0ae20f11b64259730fc1088a4fea381b95767a6f417dbdd190c0302d19133fc226725a69a1e82f9e7fa12543daf5ca0245917bedd7101eea4e19fd1adb4bcf46; ua=%E5%91%A8%E5%87%AF%E6%AD%8C; ctu=47f5eba3de69acfd21b70c8ae58e405b7207c5996bee265edc60334adf04114e; ri=1000321300; m_set_info=%7B%22ri%22%3A%221000321300%22%2C%22rv%22%3A%221602833832189%22%2C%22ui%22%3A%22875973616%22%7D; rv=1602833832189; cy=2; cye=beijing; switchcityflashtoast=1; default_ab=citylist%3AA%3A1%7Cshop%3AA%3A11%7Cindex%3AA%3A3%7CshopList%3AA%3A5; ll=7fd06e815b796be3df069dec7836c3df; Hm_lvt_602b80cf8079ae6591966cc70a3940e7=1602832663,1603422051,1604307494,1605330909; msource=default; cityid=1; Hm_lpvt_602b80cf8079ae6591966cc70a3940e7=1605340757; _lxsdk_s=175c5a6b745-b6a-19c-8b8%7C%7C700";
        public  string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
             
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }

        public string  chuli(string font)
        {
            if (font.Contains("&#xebf2;"))
            {
                font = font.Replace("&#xebf2;", "0");
            }
            if (font.Contains("&#xf023;"))
            {
                font = font.Replace("&#xf023;", "2");
            }
            if (font.Contains("&#xe1a6;"))
            {
                font = font.Replace("&#xe1a6;", "3");
            }
            if (font.Contains("&#xf8ff;"))
            {
                font = font.Replace("&#xf8ff;", "4");
            }
            if (font.Contains("&#xf6fe;"))
            {
                font = font.Replace("&#xf6fe;", "5");
            }
            if (font.Contains("&#xe273;"))
            {
                font = font.Replace("&#xe273;", "6");
            }
            if (font.Contains("&#xf7d4;"))
            {
                font = font.Replace("&#xf7d4;", "7");
            }
            if (font.Contains("&#xf04b;"))
            {
                font = font.Replace("&#xf04b;", "8");
            }
            if (font.Contains("&#xf4ab;"))
            {
                font = font.Replace("&#xf4ab;", "9");
            }

            return font;

        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void run()
        {
       
            try
            {


                for (int i = 1; i < 51; i++)

                {

                    string Url = "http://www.dianping.com/shanghai/ch10/r854p" + i;

                    string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                    MatchCollection names = Regex.Matches(html, @"data-hippo-type=""shop"" title=""([\s\S]*?)""");
                    MatchCollection averages = Regex.Matches(html, @"mean-price([\s\S]*?)人均([\s\S]*?)</span>");

                    MatchCollection kw = Regex.Matches(html, @"口味<b>([\s\S]*?)</b>");
                    MatchCollection hj = Regex.Matches(html, @"环境<b>([\s\S]*?)</b>");
                    MatchCollection fw = Regex.Matches(html, @"服务<b>([\s\S]*?)</b>");


                    if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    for (int j = 0; j < names.Count; j++)


                    {

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(names[j].Groups[1].Value);
                        listViewItem.SubItems.Add(chuli(Regex.Replace(averages[j].Groups[2].Value, "<[^>]+>", "")).Trim().Replace("￥", ""));
                        listViewItem.SubItems.Add(chuli(Regex.Replace(kw[j].Groups[1].Value, "<[^>]+>", "")));
                        listViewItem.SubItems.Add(chuli(Regex.Replace(hj[j].Groups[1].Value, "<[^>]+>", "")));
                        listViewItem.SubItems.Add(chuli(Regex.Replace(fw[j].Groups[1].Value, "<[^>]+>", "")));


                        if (status == false)
                            return;
                    }
                    Application.DoEvents();
                    Thread.Sleep(2000);


                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        bool status = true;
        Thread thread;


        private void button1_Click(object sender, EventArgs e)
        {

            COOKIE = textBox1.Text;
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}
