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

namespace 青少年普法
{
    public partial class 青少年普法 : Form
    {
        public 青少年普法()
        {
            InitializeComponent();
        }

        public static string rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf(string aefesfsgvsg, string ewrwrwer324234)
        {
            string sfsfewfwefwr234vsdadacszc = method.GetUrl("http://www.acaiji.com/zhejiang/zhejiang.php?type=" + aefesfsgvsg + "&time=" + ewrwrwer324234, "utf-8");
            return sfsfewfwefwr234vsdadacszc.Trim();
        }
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }




        private delegate string Encrypt(string pwd, string pubickey);

        public string getencrypt(string pwd,string pubickey)
        {

            string result = webBrowser1.Document.InvokeScript("jsencrypt", new object[] { pwd,pubickey}).ToString();
            return result;
        }





        public string getpublicKey()
        {
            //对应验证码
            //https://service-rr3eta5l-1251413566.sh.apigw.tencentcs.com/userAuthApi/user/captcha?uuid=5c0448dd-eca3-4918-acdf-e14b5ce9a83c
            string url = "https://service-rr3eta5l-1251413566.sh.apigw.tencentcs.com/userAuthApi/user/login/rsaKey?uuid=5c0448dd-eca3-4918-acdf-e14b5ce9a83c";
            string html = method.GetUrl(url,"utf-8");
            string key = Regex.Match(html, @"""data"":""([\s\S]*?)""").Groups[1].Value;
            return key; 
        }


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset,string token)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                WebHeaderCollection headers = request.Headers;
                headers.Add("access-token:"+token);
                headers.Add("source:1");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈


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

        public void run()
        {
            string gregegedrgerheh = rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("shebaocbd", "timestr");
            string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[2];

            if (DateTime.Now > Convert.ToDateTime(expiretime))
            {
                return;
            }
            dt = method.ExcelToDataTable(textBox1.Text, true);
            try
            {

                string yzm = shibie();

                for (int a = 0; a < dt.Rows.Count; a++)
                {
                   
                    DataRow dr = dt.Rows[a];
                    string card = dr[0].ToString();
                    string name = dr[1].ToString();
                   
                    string passowrd = dr[2].ToString();
                    string timestr = GetTimeStamp();

                    string pubickey=getpublicKey(); 
                    Encrypt aa = new Encrypt(getencrypt);
                   
                    IAsyncResult iar = BeginInvoke(aa, new object[] { card+","+timestr, pubickey });
                    string cardencrypt = EndInvoke(iar).ToString();


                    Encrypt bb = new Encrypt(getencrypt);
                    IAsyncResult iar2 = BeginInvoke(bb, new object[] { passowrd+"," + timestr, pubickey });
                    string paswordencrypt = EndInvoke(iar2).ToString();


                    string url = "https://service-rr3eta5l-1251413566.sh.apigw.tencentcs.com/userAuthApi/authorization/web/login/auth";

                   
                    label3.Text = "正在查询：" + name;

                    
                    string postdata = "{\"loginInfo\":\""+cardencrypt+"\",\"userName\":\""+name+"\",\"password\":\""+paswordencrypt+"\",\"captcha\":\""+yzm+"\",\"source\":2,\"sourceId\":3,\"uuid\":\"5c0448dd-eca3-4918-acdf-e14b5ce9a83c\"}";
                    string html = method.PostUrl(url,postdata,"", "utf-8", "application/json", "");
                 
                    if(html.Contains("验证码错误"))
                    {
                        yzm = shibie();
                    }
                    string token = Regex.Match(html, @"""token"":""([\s\S]*?)""").Groups[1].Value;

                    string ahtml = GetUrl("https://service-rr3eta5l-1251413566.sh.apigw.tencentcs.com/userAuthApi/user/info", "utf-8",token);
                    label3.Text = html;
                    string grade = Regex.Match(ahtml, @"""grade"":""([\s\S]*?)""").Groups[1].Value;
                    string className = Regex.Match(ahtml, @"""className"":""([\s\S]*?)""").Groups[1].Value;
                    string schoolName = Regex.Match(ahtml, @"""schoolName"":""([\s\S]*?)""").Groups[1].Value;
                    string ownPhase = Regex.Match(ahtml, @"""ownPhase"":""([\s\S]*?)""").Groups[1].Value;

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(card);
                    lv1.SubItems.Add(grade);
                    lv1.SubItems.Add(className);
                    lv1.SubItems.Add(schoolName);
                    lv1.SubItems.Add(ownPhase);
                  
                    Thread.Sleep(1000);
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
                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

     
        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;


        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 青少年普法_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(path + "rsa.html"); //按照姓名找回 执行加密RSA JS方法
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
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

        private void button6_Click(object sender, EventArgs e)
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

        private void 青少年普法_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }




        #region  图鉴图像识别
        #region  网络图片转Bitmap

        public Bitmap UrlToBitmap(string url)
        {
            string cookie = "";
            WebClient mywebclient = new WebClient();
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
        #region 图片转base64

        string yzmusername = "zhou14752479";
        string yzmpassword = "zhoukaige00";

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
       
        public string shibie()
        {
            try
            {
                Bitmap image = UrlToBitmap("https://service-rr3eta5l-1251413566.sh.apigw.tencentcs.com/userAuthApi/user/captcha?uuid=5c0448dd-eca3-4918-acdf-e14b5ce9a83c");



                string param = "{\"username\":\"" + yzmusername + "\",\"password\":\"" + yzmpassword + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = method.PostUrlDefault("http://api.ttshitu.com/base64", param,"");

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                if (result.Groups[1].Value != "")
                {

                    return result.Groups[1].Value;
                }
                else
                {

                   label3.Text= "图片验证码错误:" + PostResult + "\r\n";
                    // status = false;
                    return "";
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        #endregion






    }
}
