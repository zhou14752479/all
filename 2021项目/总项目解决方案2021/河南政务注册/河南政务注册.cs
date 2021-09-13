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
    public partial class 河南政务注册 : Form
    {
        public 河南政务注册()
        {
            InitializeComponent();
        }

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

                //MessageBox.Show(ex.ToString());
                return "";
            }


        }

        #endregion

        string cookie = "SESSION=Yjc4NjM0ZTItOWYzNC00NjcwLTgzYzQtYzQ0Y2Q1NWRhY2Nm";
      


        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <returns></returns>
        public string getmobile()
        {
           
            string url = "http://yccode.net:9321/api/getPhoneNumber?token="+token+"&projectId=10668&mobileNo=&sectionNo=&taskCount=&selectOperator=&mobileCarrier=&regionalCondition=&selectArea=&area=&taskType=1&usedNumber=&manyTimes=";
            string html = method.GetUrl(url, "utf-8");
          
            string mobileNo = Regex.Match(html, @"mobileNo"":""([\s\S]*?)""").Groups[1].Value;
            string mId = Regex.Match(html, @"mId"":""([\s\S]*?)""").Groups[1].Value;
            if (mId!="")
            {
                logtxtBox.Text =DateTime.Now.ToLongTimeString()+ "获取手机号成功" + "\r\n";
                return mobileNo+"&"+mId;

            }
            else
            {
                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "获取手机号失败" + html + "\r\n";
                return "";
            }
        }

        int dengdaiduanxinmaseconds = 0;

        public string token = "";

        public void gettoken()
        {
            string url = "http://yccode.net:9321/api/auth?userName="+textBox1.Text.Trim()+"&passWord="+textBox2.Text.Trim()+"&userType=1";
            string html = method.GetUrl(url,"utf-8");
            token = Regex.Match(html, @"token"":""([\s\S]*?)""").Groups[1].Value;
            
        }
        /// <summary>
        /// 获取手机短信
        /// </summary>
        /// <returns></returns>
        public string getduanxinma(string mid)
        {
           
            dengdaiduanxinmaseconds = dengdaiduanxinmaseconds + 5;
            string url = "http://yccode.net:9321/api/getMessage?token="+token+"&mId="+mid+"&developer=";
            string html = method.GetUrl(url, "utf-8");
         
            string code = Regex.Match(html, @"\d{6}").Groups[0].Value;
            if (code!="")
            {

                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "获取手机短信验证码码成功" + "\r\n"+ html;
                return code;

            }
            else
            {
                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "正在获取手机短信码,还未收到,已等待" + dengdaiduanxinmaseconds + "\r\n"+ html;
                return "";
            }
            Thread.Sleep(5000);
        }

       
        string fasongmsg = "";

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="imgcode"></param>
        /// <param name="card"></param>
        public bool sendmobile(string mobile)
        {

            try
            {
                string url = "https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/mobileSendWithCode";
                string postdata = "mobile=" + System.Web.HttpUtility.UrlEncode(getencrypt(mobile));

                string html = PostUrl(url, postdata);
             
                //发送验证码后 清理cookie
                HtmlDocument document = webBrowser1.Document;
                document.ExecCommand("ClearAuthenticationCache", false, null);
                webBrowser1.Refresh();
                if (html.Contains("成功"))
                {
                    logtxtBox.Text = DateTime.Now.ToLongTimeString() + "发送手机短信验证码成功" + "\r\n";
                    return true;
                }
                else
                {
                    logtxtBox.Text = DateTime.Now.ToLongTimeString() + "发送手机短信验证码失败" + html + "\r\n";
                    fasongmsg = html;
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false ;
            }
        }


       
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 河南政务注册_Load(object sender, EventArgs e)
        {
            //不含参数，有返回值
            //string value = webBrowser1.Document.InvokeScript("ceshi").ToString();
            //MessageBox.Show(value);
            // webBrowser1.Navigate(path+ "index_encrypt.html");
            webBrowser1.Navigate("https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/register");
            webBrowser1.ScriptErrorsSuppressed = true;
          
           
        }

        public string getencrypt(string pwd)
        {

            string result = webBrowser1.Document.InvokeScript("encrypt", new object[] { pwd }).ToString();
            return result;
        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
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


                    if (status == false)
                    {
                        return;
                    }

                    DataRow dr = dt.Rows[i];

                    string name = System.Web.HttpUtility.UrlEncode(dr[0].ToString());
                    string card = System.Web.HttpUtility.UrlEncode(getencrypt(dr[1].ToString()));
                    string mobilemid= getmobile();
                    string[] text = mobilemid.Split(new string[] { "&" }, StringSplitOptions.None);
                    if (text.Length < 1)
                    {
                        logtxtBox.Text = "获取手机号失败";
                        return;
                    }
                       
                    string mobile = text[0];
                    string mid = text[1];

                    bool fasongstatus = sendmobile(mobile);
                    while (!fasongstatus)
                    {
                        Thread.Sleep(1000);
                        if (status == false)
                        {
                            return;
                        }

                        if (fasongmsg.Contains("频繁"))
                        {
                            logtxtBox.Text += "触发发送验证码频繁正在等待120秒.." + "\r\n";
                            Thread.Sleep(120000);
                        }
                        fasongstatus = sendmobile(mobile);
                    }
                    string code = getduanxinma(mid);
                    while (true)
                    {
                        if (status == false)
                        {
                            return;
                        }
                        code = getduanxinma(mid);
                        if (dengdaiduanxinmaseconds == Convert.ToInt32(textBox6.Text))
                        {
                            dengdaiduanxinmaseconds = 0;
                            mobile = getmobile();
                            sendmobile(mobile);
                        }
                        if (code != "")
                        {
                            dengdaiduanxinmaseconds = 0;
                            break;
                        }
                      
                    }
                    string pwd = System.Web.HttpUtility.UrlEncode(getencrypt(textBox3.Text.Trim()));
                    string mobile_encrypt= System.Web.HttpUtility.UrlEncode(getencrypt(mobile));
                    string url = "https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/saveUserInfo?times=b6fdd2d4-184d-4ab1-8943-e4e6b308815a";
                    string postdata = "userName="+name+"&certNo="+card+"&certEffDate=&certExpDate=&certType=111&loginPwd="+pwd+"&userMobile="+ mobile_encrypt + "&code="+code+"&certNation=&backUrl=";
                    string html = PostUrl(url,postdata);
                    //MessageBox.Show(html);
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(dr[0].ToString());
                    lv1.SubItems.Add(dr[1].ToString());
                    lv1.SubItems.Add(mobile);
                    lv1.SubItems.Add(html);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    continue;
                }
            }




        }
        
        Thread thread;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            gettoken();

            //if (textBox5.Text == "")
            //{
            //    MessageBox.Show("请先导入数据表格");
            //    return;
            //}
            //status = true;
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            run();

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            cookie = method.GetCookies("https://login.hnzwfw.gov.cn/tacs-uc/naturalMan/register");
            MessageBox.Show(cookie);
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
    }
}
