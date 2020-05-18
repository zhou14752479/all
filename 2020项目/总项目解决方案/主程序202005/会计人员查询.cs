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
using helper;

namespace 主程序202005
{
    public partial class 会计人员查询 : Form
    {
        public 会计人员查询()
        {
            InitializeComponent();
        }
        string cookie = "";
        #region  网络图片转Bitmap
        public  Bitmap UrlToBitmap(string url)
        {
            WebClient mywebclient = new WebClient();
            mywebclient.Headers.Add("Cookie",cookie);
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

        public  string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "http://czt.sc.gov.cn/kj/toPage?toPage=cms/isreal";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                MessageBox.Show( ex.ToString());
                return "";
            }


        }

        #endregion

        #region 图片转base64
        public static string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch 
            {
                return null;
            }
        }

        #endregion

        public string shibie ()
        {
            try
            {
                Bitmap image = UrlToBitmap("http://czt.sc.gov.cn/kj/captcha.jpg?randdom=0.8007734420064332");



                string param = "{\"username\":\"zhou14752479\",\"password\":\"zhoukaige00\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                return result.Groups[1].Value;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }
        }


        public void run()
        {
            cookie = method.GetCookies("http://czt.sc.gov.cn/kj/captcha.jpg?randdom=0.8007734420064332");
            string yanzhengma = shibie();

            try
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {


                    string url = "http://czt.sc.gov.cn/kj/idxx";
                    string postdata = "useryzm=" + yanzhengma.Trim() + "&ID_TYPE=1&ID_NUM=" + text[i].Trim();
                    string html = PostUrl(url, postdata);
                    
                    if (!html.Contains("验证码不正确") && !html.Contains("身份证"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add("不存在此人信息");
                        lv1.SubItems.Add(text[i].Trim());

                       
                    }

                    else
                    {
                        MatchCollection result = Regex.Matches(html, @"ISHG"" : ""([\s\S]*?)""");
                        Match name = Regex.Match(html, @"NAME"" : ""([\s\S]*?)""");
                        Match area = Regex.Match(html, @"XZQH"" : ""([\s\S]*?)""");



                        if (html.Contains("验证码不正确"))
                        {
                            cookie = method.GetCookies("http://czt.sc.gov.cn/kj/captcha.jpg?randdom=0.8007734420064332");
                            yanzhengma = shibie();
                            postdata = "useryzm=" + yanzhengma + "&ID_TYPE=1&ID_NUM=" + text[i].Trim();
                            html = PostUrl(url, postdata);
                            MatchCollection result2 = Regex.Matches(html, @"ISHG"" : ""([\s\S]*?)""");
                            Match name2 = Regex.Match(html, @"NAME"" : ""([\s\S]*?)""");
                            Match area2 = Regex.Match(html, @"XZQH"" : ""([\s\S]*?)""");

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(name2.Groups[1].Value);
                            lv1.SubItems.Add(text[i].Trim());
                            lv1.SubItems.Add(area2.Groups[1].Value.Trim());


                            for (int j = result2.Count - 1; j >= 0; j--)
                            {

                                lv1.SubItems.Add(result2[j].Groups[1].Value);
                            }
                        }
                        else
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(name.Groups[1].Value);
                            lv1.SubItems.Add(text[i].Trim());


                            for (int j = result.Count - 1; j >= 0; j--)
                            {

                                lv1.SubItems.Add(result[j].Groups[1].Value);
                            }
                            lv1.SubItems.Add(area.Groups[1].Value.Trim());
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        if (status == false)
                            return;

                        Thread.Sleep(200);


                    }









                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        bool status = true;
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (html.Contains(@"kuaijichaxun"))
            {
                status = true;
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

        private void 会计人员查询_Load(object sender, EventArgs e)
        {
            WebBrowser web = new WebBrowser();
            web.Navigate("http://czt.sc.gov.cn/kj/captcha.jpg?randdom=0.8007734420064332");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
