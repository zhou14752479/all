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

namespace 快递超市
{
    public partial class 快递超市 : Form
    {
        public 快递超市()
        {
            InitializeComponent();
        }

        string cookie = "UM_distinctid=17ca63226f83c9-0f3568c98143dd-4343363-1fa400-17ca63226f9320";
        string access_token = "";
        #region  网络图片转Bitmap
        public Bitmap UrlToBitmap(string url)
        {
            WebClient mywebclient = new WebClient();
            mywebclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            mywebclient.Headers.Add("Cookie", cookie);
            byte[] Bytes = mywebclient.DownloadData(url);

            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);

                Bitmap map = new Bitmap(outputImg);
                return map;
            }
        }
        #endregion


        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrlDefault(string url, string postData, string contenttype)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                                     //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("token:" + access_token);
                request.ContentType = contenttype;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", cookie);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        private void button5_Click(object sender, EventArgs e)
        {
            string html = PostUrlDefault("https://kdcs-api.tuxi.net.cn/user/login", "userName=" + textBox1.Text.Trim() + "&password=" + textBox2.Text.Trim() + "&verifyCode=" + textBox3.Text.Trim(), "application/x-www-form-urlencoded");
            access_token = Regex.Match(html, @"""access_token"":""([\s\S]*?)""").Groups[1].Value;

            if (access_token != "")
            {
                MessageBox.Show("登录成功");
            }
            else
            {
                MessageBox.Show(html);
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = UrlToBitmap("https://kdcs-api.tuxi.net.cn/nologin/verifyCode?codeType=1&&d=0.8568435502188521");
        }


        bool status = true;
        public void run()
        {

            try
            {
                for (int page = 1; page <= 9999; page++)
                {


                    string url = "https://kdcs-api.tuxi.net.cn/business/list";
                    string postdata = "{\"pageSize\":500,\"pageIndex\":" + page + ",\"type\":\"6\",\"startTime\":\"" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "\",\"endTime\":\"" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "\",\"expressCompanyCode\":\"ALL\",\"customerName\":\"\",\"mobile\":\"\",\"billCode\":\"\",\"notifyType\":\"4\",\"noticeType\":\"0\",\"notifyStatus\":\"4\",\"sort\":\"desc\"}";
                    string html = PostUrlDefault(url, postdata, "application/json");

                    MatchCollection billCodes = Regex.Matches(html, @"""billCode"":""([\s\S]*?)""");
                    MatchCollection depotNames = Regex.Matches(html, @"""depotName"":""([\s\S]*?)""");
                    MatchCollection expressCompanyNames = Regex.Matches(html, @"""expressCompanyName"":""([\s\S]*?)""");

                    MatchCollection receiveMans = Regex.Matches(html, @"""receiveMan"":""([\s\S]*?)""");
                    MatchCollection receiveManMobiles = Regex.Matches(html, @"""receiveManMobile"":""([\s\S]*?)""");
                    MatchCollection takeCodes = Regex.Matches(html, @"""takeCode"":""([\s\S]*?)""");
                    //MatchCollection billCodes = Regex.Matches(html, @"""billCode"":""([\s\S]*?)""");
                    MatchCollection outDateStrs = Regex.Matches(html, @"""outDateStr"":""([\s\S]*?)""");

                    if (billCodes.Count == 0)
                        return;
                    for (int i = 0; i < billCodes.Count; i++)
                    {
                        if (status == false)
                            return;

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(billCodes[i].Groups[1].Value);
                        listViewItem.SubItems.Add(depotNames[i].Groups[1].Value);
                        listViewItem.SubItems.Add(expressCompanyNames[i].Groups[1].Value);

                        listViewItem.SubItems.Add(receiveMans[i].Groups[1].Value);
                        listViewItem.SubItems.Add(receiveManMobiles[i].Groups[1].Value);
                        listViewItem.SubItems.Add(takeCodes[i].Groups[1].Value);
                        listViewItem.SubItems.Add("");//自提
                        listViewItem.SubItems.Add(outDateStrs[i].Groups[1].Value);

                        if (listView1.Items.Count > 2)
                        {
                            this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                        }
                    }

                    Thread.Sleep(1000);

                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        public void ruku()
        {
            try
            {
                int count = 0;
                int codenum = Convert.ToInt32(textBox4.Text);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                    if (status == false)
                        return;

                    count = count + 1;
                    codenum = codenum + 1;
                    string billCode = richTextBox1.Lines[i].ToString();
                    string url = "https://kdcs-api.tuxi.net.cn/fastentry/add";





                    sb.Append("{\"billCode\":\"" + billCode + "\",\"depotCode\":\"TUXI39300483168\",\"ediUdf2\":\"\",\"ediUdf3\":\"\",\"expressComapnyCode\":\"YTO\",\"failReson\":\"\",\"isNewUser\":0,\"receiveMan\":\"\",\"receiveManMobile\":\"\",\"scanDate\":\"\",\"billCodeScanTime\":\"\",\"\":1,\"staffCode\":\"ZTWJ3670625\",\"takeCode\":\"" + codenum.ToString().PadLeft(6, '0') + "\",\"channel\":5,\"mobileChannel\":5,\"isSecretWaybill\":0,\"waybillType\":0,\"virtualMobile\":null},");

                    if (count >20)
                    {
                        string postdata = "[" + sb.ToString().Remove(sb.ToString().Length - 1, 1) + "]";
                        string html = PostUrlDefault(url, postdata, "application/json");
                        MatchCollection billCodes = Regex.Matches(html, @"""billCode"":""([\s\S]*?)""");
                        MatchCollection reasons = Regex.Matches(html, @"failReson"":""([\s\S]*?)""");
                        for (int a = 0; i < reasons.Count; a++)
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(billCodes[a].Groups[1].Value);
                            listViewItem.SubItems.Add(reasons[a].Groups[1].Value);

                        }
                        
                    }
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void 快递超市_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"uoQUpu"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }



            #endregion

            pictureBox1.Image = UrlToBitmap("https://kdcs-api.tuxi.net.cn/nologin/verifyCode?codeType=1&&d=0.8568435502188521");
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            //access_token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJaVFdKMzY3MDYyNSIsIm9wZW5JZCI6IlJCMkZSN2hGVkVPR3pFVzA5Y3JYZmciLCJncmFudFR5cGUiOiJhY2Nlc3NUb2tlbiIsImV4cCI6MTYzNTA0ODI3MSwiaWF0IjoxNjM1MDQ0NjcxLCJqdGkiOiI5OGQyNjA3OC1kNzc2LTQzOTgtODU4My0wZWZhZjJmN2Y2MWQifQ.ZINe78uPiR1vng1ZSZetNtkY1NcSOPVYMP7KSo4SfKw";
            if (access_token == "")
            {
                MessageBox.Show("请先登录");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(ruku);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
