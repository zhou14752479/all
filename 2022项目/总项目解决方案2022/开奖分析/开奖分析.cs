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

namespace 开奖分析
{
    public partial class 开奖分析 : Form
    {
        public 开奖分析()
        {
            InitializeComponent();
        }

        public string yuming = "1680687kai.co";
        public string lotCode = "10012";


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

        #region 即时开奖
        public void gethaoma()
        {
            try
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();
                listView1.Columns.Add("序号", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("期号", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("开奖时间", 150, HorizontalAlignment.Center);
                listView1.Columns.Add("号码", 300, HorizontalAlignment.Center);
                listView1.Columns.Add("冠亚和", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("龙虎", 100, HorizontalAlignment.Center);
                string url = "https://"+yuming+"/api/pks/getPksHistoryList.do?lotCode="+lotCode;
                string html = GetUrl(url,"utf-8");
                MatchCollection preDrawIssue = Regex.Matches(html, @"""preDrawIssue"":([\s\S]*?),");
                MatchCollection preDrawTime = Regex.Matches(html, @"""preDrawTime"":""([\s\S]*?)""");
                MatchCollection preDrawCode = Regex.Matches(html, @"""preDrawCode"":""([\s\S]*?)""");

                MatchCollection sumFS = Regex.Matches(html, @"""sumFS"":([\s\S]*?),");
                MatchCollection sumBigSamll = Regex.Matches(html, @"""sumBigSamll"":([\s\S]*?),");
                MatchCollection sumSingleDouble = Regex.Matches(html, @"""sumSingleDouble"":([\s\S]*?),");

                MatchCollection firstDT = Regex.Matches(html, @"""firstDT"":([\s\S]*?),");
                MatchCollection secondDT = Regex.Matches(html, @"""secondDT"":([\s\S]*?),");
                MatchCollection thirdDT = Regex.Matches(html, @"""thirdDT"":([\s\S]*?),");
                MatchCollection fourthDT = Regex.Matches(html, @"""fourthDT"":([\s\S]*?),");
                MatchCollection fifthDT = Regex.Matches(html, @"""fifthDT"":([\s\S]*?),");

                for (int i = 0; i < preDrawIssue.Count; i++)
                {

                    string guanyahe = sumFS[i].Groups[1].Value+ 
                        sumBigSamll[i].Groups[1].Value.Replace("0","大").Replace("1", "小") +
                         sumSingleDouble[i].Groups[1].Value.Replace("0", "单").Replace("1", "双");

                    string longhu= firstDT[i].Groups[1].Value + secondDT[i].Groups[1].Value + thirdDT[i].Groups[1].Value+fourthDT[i].Groups[1].Value+fifthDT[i].Groups[1].Value;
                    longhu= longhu.Replace("0", "龙").Replace("1", "虎");

                   
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                    lv1.SubItems.Add(preDrawIssue[i].Groups[1].Value);
                    lv1.SubItems.Add(preDrawTime[i].Groups[1].Value);
                    lv1.SubItems.Add(preDrawCode[i].Groups[1].Value);
                    lv1.SubItems.Add(guanyahe);
                    lv1.SubItems.Add(longhu);
                   
                }

            }
            catch (Exception ex)
            {

               toolStripStatusLabel1.Text=ex.ToString();
            }
        }
        #endregion

        #region 今日分析
        public void jinrifenxi()
        {
            try
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();
                listView1.Columns.Add("号码", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("位置1", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置2", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置3", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置4", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置5", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置6", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置7", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置8", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置9", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("位置10", 100, HorizontalAlignment.Center);
                string url = "https://"+yuming+"/api/pks/queryToDayNumberLawOfStatistics.do?date=&lotCode=" + lotCode;
                string html = GetUrl(url, "utf-8");
                MatchCollection accumulate = Regex.Matches(html, @"""accumulate"":([\s\S]*?),");
                MatchCollection missing = Regex.Matches(html, @"""missing"":([\s\S]*?),");
              
                for (int i = 0; i < 10; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count ).ToString()); //使用Listview展示数据    
                    lv1.SubItems.Add(accumulate[(10*i)].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+1].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+2].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+3].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+4].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+5].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+6].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+7].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+8].Groups[1].Value);
                    lv1.SubItems.Add(accumulate[(10 * i)+9].Groups[1].Value);

                    //lv1.SubItems.Add(missing[i].Groups[1].Value);
                  
                }

            }
            catch (Exception ex)
            {

                toolStripStatusLabel1.Text = ex.ToString();
            }
        }
        #endregion

        #region 长龙统计
        public void changlong()
        {
            try
            {
                listView1.Columns.Clear();
                listView1.Items.Clear();
                listView1.Columns.Add("日期", 100, HorizontalAlignment.Center);
                listView1.Columns.Add("2期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("3期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("4期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("5期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("6期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("7期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("8期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("9期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("10期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("11期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("12期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("13期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("14期", 60, HorizontalAlignment.Center);
                listView1.Columns.Add("15期", 60, HorizontalAlignment.Center);
             
                string url = "https://"+yuming+"/api/pks/queryPksDailyDragon.do?days=30&type=1&rank=1&lotCode=" + lotCode;
                string html = GetUrl(url, "utf-8");
                MatchCollection dragon = Regex.Matches(html, @"dragon"":\[([\s\S]*?)\]");
                MatchCollection date = Regex.Matches(html, @"""date"":""([\s\S]*?)""");

                for (int i = 0; i < dragon.Count; i++)
                {
                    string[] text = dragon[i].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                   
                    ListViewItem lv1 = listView1.Items.Add((date[i].Groups[1].Value).ToString()); //使用Listview展示数据   
                   
                    for (int j = 0; j < text.Length; j++)
                    {
                        lv1.SubItems.Add(text[j]);
                    }
                 

                 

                }

            }
            catch (Exception ex)
            {

                toolStripStatusLabel1.Text = ex.ToString();
            }
        }
        #endregion
        private void 开奖分析_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            listView1.SmallImageList = imgList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gethaoma();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            jinrifenxi();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changlong();
        }
    }
}
