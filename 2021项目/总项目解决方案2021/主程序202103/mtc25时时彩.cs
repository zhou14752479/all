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

namespace 主程序202103
{
    public partial class mtc25时时彩 : Form
    {
        public mtc25时时彩()
        {
            InitializeComponent();
        }
        

        #region 加拿大时时彩
        public void jianadashishicai()
        {

          
            for (DateTime dt = dateTimePicker1.Value; dt < dateTimePicker2.Value; dt=dt.AddDays(1))
            {


                string url = "https://www.mtc25.com/static//data/"+dt.ToString("yyyyMMdd") + "3HistoryLottery.json?_=1615265002031";

                string html = GetUrl(url,"utf-8");
               
                MatchCollection times = Regex.Matches(html, @"""openTime"":""([\s\S]*?)""");
                MatchCollection values = Regex.Matches(html, @"""openNum"":""([\s\S]*?)""");
              
                for (int j = 0; j < times.Count; j++)
                {

                    try
                    {
                        string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(times[j].Groups[1].Value);
                        lv1.SubItems.Add(text[0]);
                        lv1.SubItems.Add(text[1]);
                        lv1.SubItems.Add(text[2]);
                        lv1.SubItems.Add(text[3]);
                        lv1.SubItems.Add(text[4]);
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
                Thread.Sleep(2000);
            }

        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "x-session-token=TNcMoxb%2B6BXDBVsSH%2F%2FMswKVIpOo1AKb5MsPi%2BWI8gfctqrLJO66QA%3D%3D; _skin_=red";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

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

        #region 台湾宾果
        public void taiwanbinguo()
        {


            for (DateTime dt = dateTimePicker1.Value; dt < dateTimePicker2.Value; dt = dt.AddDays(1))
            {


                string url = "https://www.mtc25.com/static//data/" + dt.ToString("yyyyMMdd") + "43HistoryLottery.json?_=1615265002031";
              
                string html = GetUrl(url, "utf-8");

                MatchCollection times = Regex.Matches(html, @"""openTime"":""([\s\S]*?)""");
                MatchCollection values = Regex.Matches(html, @"""openNum"":""([\s\S]*?)""");
               
                for (int j = 0; j < times.Count; j++)
                {

                    try
                    {
                        string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(times[j].Groups[1].Value);
                        lv1.SubItems.Add(text[0]);
                        lv1.SubItems.Add(text[1]);
                        lv1.SubItems.Add(text[2]);
                        int total = Convert.ToInt32(text[0])+ Convert.ToInt32(text[1])+ Convert.ToInt32(text[2]);
                        lv1.SubItems.Add(total.ToString());
                        //lv1.SubItems.Add(text[3]);
                        //lv1.SubItems.Add(text[4]);
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
                Thread.Sleep(2000);
            }

        }

        #endregion


        #region 加拿大28
        public void jianada28()
        {


            for (DateTime dt = dateTimePicker1.Value; dt < dateTimePicker2.Value; dt = dt.AddDays(1))
            {


                string url = "https://www.mtc25.com/static//data/" + dt.ToString("yyyyMMdd") + "41HistoryLottery.json?_=1615265002031";
              
                string html = GetUrl(url, "utf-8");
               
                MatchCollection times = Regex.Matches(html, @"""openTime"":""([\s\S]*?)""");
                MatchCollection values = Regex.Matches(html, @"""openNum"":""([\s\S]*?)""");

                for (int j = 0; j < times.Count; j++)
                {

                    try
                    {
                        string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(times[j].Groups[1].Value);
                        lv1.SubItems.Add(text[0]);
                        lv1.SubItems.Add(text[1]);
                        lv1.SubItems.Add(text[2]);
                        int total = Convert.ToInt32(text[0]) + Convert.ToInt32(text[1]) + Convert.ToInt32(text[2]);
                        lv1.SubItems.Add(total.ToString());
                        //lv1.SubItems.Add(text[3]);
                        //lv1.SubItems.Add(text[4]);
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
                Thread.Sleep(2000);
            }

        }

        #endregion


        #region 幸运快乐8
        public void xingyunkuaile8()
        {


            for (DateTime dt = dateTimePicker1.Value; dt < dateTimePicker2.Value; dt = dt.AddDays(1))
            {


                string url = "https://www.mtc25.com/static//data/" + dt.ToString("yyyyMMdd") + "83HistoryLottery.json?_=1615265002031";

                string html = GetUrl(url, "utf-8");

                MatchCollection times = Regex.Matches(html, @"""openTime"":""([\s\S]*?)""");
                MatchCollection values = Regex.Matches(html, @"""openNum"":""([\s\S]*?)""");

               
                for (int j = 0; j < times.Count; j++)
                {

                    try
                    {


                        string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                        int total = 0;
                        string daxiao = "和";
                        string danshuang = "双";
                        foreach (string item in text)
                        {
                            total = total + Convert.ToInt32(item);
                        }
                        if (total > 810)
                        {
                            daxiao = "大";
                        }
                        if (total < 810)
                        {
                            daxiao = "小";
                        }

                        if (total % 2 == 1)
                        {
                            danshuang = "单";
                        }




                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(times[j].Groups[1].Value);
                        lv1.SubItems.Add(values[j].Groups[1].Value);
                        lv1.SubItems.Add(total.ToString()+" "+daxiao+" "+danshuang);
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
                Thread.Sleep(3000);
            }

        }

        #endregion


        #region 比特币一分
        public void btc1()
        {


            for (DateTime dt = dateTimePicker1.Value; dt < dateTimePicker2.Value; dt = dt.AddDays(1))
            {


                string url = "https://api.365kaik.com/api/v1/trend/getHistoryList?t=1628381966&lotCode=10035&date="+ dt.ToString("yyyy-MM-dd")+"&pageSize=2000&pageNum=0";

                string html = GetUrl(url, "utf-8");

                MatchCollection times = Regex.Matches(html, @"""drawTime"":""([\s\S]*?)""");
                MatchCollection qishus = Regex.Matches(html, @"""drawIssue"":""([\s\S]*?)""");
                MatchCollection values = Regex.Matches(html, @"""drawCode"":""([\s\S]*?)""");

                for (int j = 0; j < times.Count; j++)
                {
                    string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                    int total = 0;
                    string daxiao = "小";
                    string danshuang = "双";
                    foreach (string item in text)
                    {
                        total = total + Convert.ToInt32(item);
                    }
                    if (total > 22)
                    {
                        daxiao = "大";
                    }
                   

                    if (total % 2 == 1)
                    {
                        danshuang = "单";
                    }

                    try
                    {
                      
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(times[j].Groups[1].Value);
                        lv1.SubItems.Add(qishus[j].Groups[1].Value);
                        lv1.SubItems.Add(values[j].Groups[1].Value);
                        lv1.SubItems.Add(total.ToString());
                        lv1.SubItems.Add(daxiao);
                        lv1.SubItems.Add(danshuang);


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
                Thread.Sleep(2000);
            }

        }

        #endregion
        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void mtc25时时彩_Load(object sender, EventArgs e)
        {

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
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(btc1);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void mtc25时时彩_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
