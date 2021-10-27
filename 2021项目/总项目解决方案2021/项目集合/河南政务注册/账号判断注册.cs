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

namespace 河南政务注册
{
    public partial class 账号判断注册 : Form
    {
        public 账号判断注册()
        {
            InitializeComponent();
        }

        string cookie = "";

        #region POST请求

        public string PostUrl(string url, string postData)
        {
            try
            {
                string charset = "utf-8";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                WebHeaderCollection headers = request.Headers;
                headers.Add("token: HdPB6c4iYom6hCeTSG6F0Yt22BQm8xbXeC4hXJxc4AllsbL4cj/jTbp8rqom7duLoxSwReeGbFimNeMjYbQjSbkEK6EEfRgwxattv9uDlon4Ok2OTen7BznqWEpv9HQBbp+QCT1FJZXsdOACERF6R1cVpKQJetfYY7NO84umV6vnwfG0HSiqTXTIHcsiIvK0Wh07dIv6bFv7vCum3BZgxhVnyeUPOZScKRytbd1NSmyHffAXiNNRNBPuzOxozvbdiZXR+ppssW3r/0CSxS3I2hfb0yOQpgCgk2DjvyU5Zkw9Ng1Q5PHs7Y/26qG80yuR0s7SPlWYYzqQKEeb89bCNA==");
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.ContentLength = postData.Length;
                // request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;

                request.Accept = "application/json, text/javascript, */*; q=0.01";


                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/register";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                string html = "";
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


                return ex.Message;
            }


        }

        #endregion

        private delegate void ClearCookie();//代理     
        public void clearIeCookie()
        {
            //发送验证码后 清理cookie

            HtmlDocument document = webBrowser1.Document;
            document.ExecCommand("ClearAuthenticationCache", false, null);
            webBrowser1.Refresh();
           
        }


        private delegate string Encrypt(string pwd);//代理

        public string getencrypt(string pwd)
        {

            string result = webBrowser1.Document.InvokeScript("encrypt", new object[] { pwd }).ToString();
            return result;
        }


        #region  网络图片转Bitmap
        public Bitmap UrlToBitmap(string url)
        {
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

                string yzmusername = textBox1.Text;
                string yzmpassword = textBox2.Text;
                Bitmap image = UrlToBitmap("https://login.hnzwfw.gov.cn/tacs-uc/verify/verifyCodeImg?rnd=0.16975696392922401");
                string param = "{\"username\":\"" + yzmusername + "\",\"password\":\"" + yzmpassword + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                if (result.Groups[1].Value != "")
                {

                    return result.Groups[1].Value;
                }
                else
                {

                    logtxtBox.Text += "图片验证码错误:" + PostResult + "\r\n";
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
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            try
            {
                if (textBox5.Text == "")
                {
                    MessageBox.Show("请选择表格导入");
                    return;
                }
                DataTable dt = method.ExcelToDataTable(textBox5.Text, true);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {

                        cookie = method.GetCookies("https://login.hnzwfw.gov.cn/tacs-uc/verify/verifyCodeImg?rnd=0.16975696392922401");
                        if (status == false)
                        {
                            return;
                        }

                        DataRow dr = dt.Rows[i];

                        Encrypt aa = new Encrypt(getencrypt);
                        IAsyncResult iar = BeginInvoke(aa, new object[] { dr[1].ToString() });
                        string cradencrypt = EndInvoke(iar).ToString();


                        string name = System.Web.HttpUtility.UrlEncode(dr[0].ToString());
                        string card = System.Web.HttpUtility.UrlEncode(cradencrypt);

                        string code = shibie() ;

                     
                        string url = "https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/forgetPwdFirst?times=99314e33-16c3-4af2-a432-2b2e1dd5454c";
                    
                        string postdata = "userName="+name+"&certNo="+card+"&certType=111&staticCode="+code;
                        string html = PostUrl(url, postdata);
                       
                        if(html.Contains("校验码不正确"))
                        {
                            i = i - 1;
                            continue;
                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(dr[0].ToString());
                        lv1.SubItems.Add(dr[1].ToString());
                        lv1.SubItems.Add(html);

                        //清理cookie
                        ClearCookie cc = new ClearCookie(clearIeCookie);
                        BeginInvoke(cc);
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                        //continue;
                    }
                }
            }
            catch (Exception ex)
            {

                ;
            }




        }




        Thread thread;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox5.Text == "")
            {
                MessageBox.Show("请先导入数据表格");
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

        private void button5_Click(object sender, EventArgs e)
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
                textBox5.Text = openFileDialog1.FileName;



            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 账号判断注册_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/register");
            webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
