using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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

namespace 主程序
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
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
                //request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
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

        public void getnew()


        {

            string url = "";
            //string url = "https://www.1686100.com/api/LotteryPlan/getPksPlanList.do?lotCode=10012&rows=20&date=" + DateTime.Now.ToString("yyyy-MM-dd");

            if (radioButton1.Checked==true)
            {
                url = "https://www.1686100.com/api/pks/getPksHistoryList.do?lotCode=10012";
            }

            if (radioButton2.Checked == true)
            {
               url = "https://www.1686100.com/api/pks/getPksHistoryList.do?date="+ DateTime.Now.ToString("yyyy-MM-dd") + "&lotCode=10012";
            }


            string html = GetUrl(url, "utf-8");

            MatchCollection qishus = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");
            
            MatchCollection times = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
            MatchCollection results = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

            //MessageBox.Show(qishus.Count.ToString());
            for (int j = 0; j < qishus.Count; j++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(qishus[j].Groups[1].Value);
               
                lv1.SubItems.Add(times[j].Groups[1].Value);
                lv1.SubItems.Add(results[j].Groups[1].Value);


            }

            if (qishus.Count > 3)

            {
                textBox2.Text = results[1].Groups[1].Value.Replace("01","1").Replace("02", "2").Replace("03", "3").Replace("04", "4").Replace("05", "5").Replace("06", "6").Replace("07", "7").Replace("08", "8").Replace("09", "9");
                textBox3.Text = results[2].Groups[1].Value.Replace("01", "1").Replace("02", "2").Replace("03", "3").Replace("04", "4").Replace("05", "5").Replace("06", "6").Replace("07", "7").Replace("08", "8").Replace("09", "9");
                textBox4.Text = results[3].Groups[1].Value.Replace("01", "1").Replace("02", "2").Replace("03", "3").Replace("04", "4").Replace("05", "5").Replace("06", "6").Replace("07", "7").Replace("08", "8").Replace("09", "9");
                textBox5.Text = results[4].Groups[1].Value.Replace("01", "1").Replace("02", "2").Replace("03", "3").Replace("04", "4").Replace("05", "5").Replace("06", "6").Replace("07", "7").Replace("08", "8").Replace("09", "9");
            }



        }
      
        /// <summary>
        /// 读取数据库
        /// </summary>
        public void getdata()
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand("select result from datas", mycon);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(rdr);

                for (int i = 0; i < table.Rows.Count; i++) // 遍历行
                {

                    resultList.Add(table.Rows[i]["result"]);

                }
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }

        bool status = true;
        ArrayList resultList = new ArrayList();
        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            listView1.Items.Clear();
            getnew();
        }
        public int getxiangsi(string s, string y)
        {
            int geshu = 0;
            try
            {
                string[] shuru = s.Split(new string[] { "," }, StringSplitOptions.None);
                string[] yuan = y.Split(new string[] { "," }, StringSplitOptions.None);


                //MessageBox.Show(shuru[0], yuan[0]);
                if (shuru[0] == yuan[0])
                {
                    geshu = geshu + 1;
                }

                if (shuru[1] == yuan[1])
                {
                    geshu = geshu + 1;
                }
                if (shuru[2] == yuan[2])
                {
                    geshu = geshu + 1;
                }
                if (shuru[3] == yuan[3])
                {
                    geshu = geshu + 1;
                }
                if (shuru[4] == yuan[4])
                {
                    geshu = geshu + 1;
                }
                if (shuru[5] == yuan[5])
                {
                    geshu = geshu + 1;
                }
                if (shuru[6] == yuan[6])
                {
                    geshu = geshu + 1;
                }
                if (shuru[7] == yuan[7])
                {
                    geshu = geshu + 1;
                }
                if (shuru[8] == yuan[8])
                {
                    geshu = geshu + 1;
                }
                if (shuru[9] == yuan[9])
                {
                    geshu = geshu + 1;
                }
            }
            catch (Exception)
            {

              
            }

            return geshu;
        }
        #region run1
        public void run1()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("值为空");
                return;
            }
            string shuru = "";
            if (textBox1.Text.Contains(","))
            {
                shuru = textBox1.Text.Trim();
            }
            else
            {
                foreach (var item in textBox1.Text.Trim())
                {
                    shuru += item + ",";
                }
                shuru = shuru.Remove(shuru.Length - 1, 1);

            }


            for (int i = 0; i < resultList.Count; i++)
            {

                //.Replace("01","1").Replace("02", "2").Replace("03", "3").Replace("04", "4").Replace("05", "5").Replace("06", "6").Replace("07", "7").Replace("08", "8").Replace("09", "9")
                int value = getxiangsi(shuru, resultList[i].ToString().Replace("01", "1").Replace("02", "2").Replace("03", "3").Replace("04", "4").Replace("05", "5").Replace("06", "6").Replace("07", "7").Replace("08", "8").Replace("09", "9"));

                label6.Text = "正在分析" + resultList[i];
                label7.Text = "正在分析" + resultList[i];
                label8.Text = "正在分析" + resultList[i];
                label9.Text = "正在分析" + resultList[i];
                label10.Text = "正在分析" + resultList[i];
                if (value > 5 && shuru != resultList[i].ToString())
                {
                    //textBox6.Text += resultList[i].ToString() + "\r\n";
                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 9, 9));
                    ListViewItem lv3 = listView3.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 6, 6));
                    ListViewItem lv4 = listView4.Items.Add((listView2.Items.Count + 1).ToString() + ":" + resultList[i - 1].ToString().Remove(resultList[i - 1].ToString().Length - 2, 2));
                    button2.Enabled = true;
                }

                if (status == false)
                    return;

                if (listView2.Items.Count > 6)
                    return;
            }




        }

        #endregion

        

        

        

        
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();

            //button2.Enabled = false;
            status = true;
            getdata();

            Thread thread1 = new Thread(new ThreadStart(run1));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
    
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            danshuang();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            daxiao();
        }
        /// <summary>
        /// 大小
        /// </summary>
        /// <param name="dan"></param>
        public void daxiao()

        {
            DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/d") + " 0:0:0");


            


            string url = "https://api.api861861.com/pks/getPksHistoryList.do?lotCode=10012&date=" + DateTime.Now.ToString("yyyy-MM-dd");



            string html = method.GetUrl(url, "utf-8");

            MatchCollection qishus = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");

            MatchCollection times = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
            MatchCollection results = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

            for (int j = 0; j < qishus.Count; j++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(qishus[j].Groups[1].Value);
               
                lv1.SubItems.Add(times[j].Groups[1].Value);

                StringBuilder sb = new StringBuilder();
                string[] text = results[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                for (int a = 0; a < text.Length; a++)
                {
                    if (Convert.ToInt32(text[a]) < 6)
                    {
                        sb.Append("小 ");
                    }
                    else
                    {
                        sb.Append("大 ");
                    }

                }

                lv1.SubItems.Add(sb.ToString());


            }



        }

        /// <summary>
        /// 单双
        /// </summary>
        /// <param name="dan"></param>
        public void danshuang()

        {
            DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/d") + " 0:0:0");


            string url = "https://api.api861861.com/pks/getPksHistoryList.do?lotCode=10012&date=" + DateTime.Now.ToString("yyyy-MM-dd");



            string html = method.GetUrl(url, "utf-8");

            MatchCollection qishus = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");

            MatchCollection times = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
            MatchCollection results = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

            for (int j = 0; j < qishus.Count; j++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(qishus[j].Groups[1].Value);
                
                lv1.SubItems.Add(times[j].Groups[1].Value);

                StringBuilder sb = new StringBuilder();
                string[] text = results[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                for (int a = 0; a < text.Length; a++)
                {
                    if (Convert.ToInt32(text[a]) % 2 == 1)
                    {
                        sb.Append("单 ");
                    }
                    else
                    {
                        sb.Append("双 ");
                    }

                }

                lv1.SubItems.Add(sb.ToString());


            }





        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            listView1.Items.Clear();
            getnew();
            Thread thread1 = new Thread(new ThreadStart(run1));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
   
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 插入数据库
        /// </summary>
        public void insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }

        


        string startdate = "2021-01-01";
        string enddate = "2022-01-06";
        public void inserdata2()
        {




            for (DateTime dt = Convert.ToDateTime(startdate); dt < Convert.ToDateTime(enddate); dt = dt.AddDays(1))
            {
                try
                {
                    string url = "https://www.1686100.com/api//LotteryPlan/getBetInfoList.do?date="+ dt.ToString("yyyy-MM-dd") + "&lotCode=10012";
                    label6.Text = dt.ToString("yyyy-MM-dd");

                    string html = GetUrl(url, "utf-8");
                
                    MatchCollection times = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
                    MatchCollection preDrawIssues = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");
                    MatchCollection preDrawCodes = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

                   

                    for (int i = 0; i < preDrawCodes.Count; i++)
                    {
                        string sql = "INSERT INTO datas(time,code,result)VALUES('" + times[i].Groups[1].Value + "','" + preDrawIssues[i].Groups[1].Value + "','" + preDrawCodes[i].Groups[1].Value + "')";

                        insertdata(sql);



                    }

                }
                catch (Exception ex)
                {

                    //MessageBox.Show(ex.ToString());
                }
            }

            MessageBox.Show("完成");
        }

        Thread thread;

        private void button3_Click(object sender, EventArgs e)
        {
            startdate = Convert.ToDateTime("2021-05-10").ToString("yyyy-MM-dd");
            enddate = DateTime.Now.ToString("yyyy-MM-dd");
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(inserdata2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
