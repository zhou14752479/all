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
using myDLL;

namespace 浙江号码匹配
{
    public partial class 浙江身份号码验证 : Form
    {
        public 浙江身份号码验证()
        {
            InitializeComponent();
        }
        bool status = true;
        bool zanting = true;
        string cookie = "";
        Thread thread;

        #region  网络图片转Bitmap
        public Bitmap UrlToBitmap(string url)
        {
            try
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
            catch (Exception)
            {

                return null;
            }
        }
        #endregion

        #region POST请求

        public string PostUrl(string url, string postData)
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
                request.Referer = "http://puser.zjzwfw.gov.cn/sso/newusp.do?action=forgotPwd&servicecode=zjdsjgrbs#";
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

                //MessageBox.Show(ex.ToString());
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

        public string shibie()
        {
            try
            {
                Bitmap image = UrlToBitmap("http://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg");



                string param = "{\"username\":\"" + textBox2.Text.Trim() + "\",\"password\":\"" + textBox3.Text.Trim() + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                label1.Text = PostResult;
                return result.Groups[1].Value;
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
                return "";
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            //openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                //打开文件对话框选择的文件
                textBox4.Text = openFileDialog1.FileName;
              
             
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        public void run()
        {
            textBox1.Text = DateTime.Now.ToLongTimeString() + ": 开始查询";
            cookie = method.GetCookies("http://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg");

           
            try
            {
                DataTable dt = method.ExcelToDataTable(textBox4.Text, true);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    try
                    {
                        string name = dt.Rows[i][1].ToString().Trim();
                        string card = dt.Rows[i][0].ToString().Trim();
                        string phone = dt.Rows[i][2].ToString().Trim();
                       
                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(card);
                        lv1.SubItems.Add(phone);


                        cookie = method.GetCookies("http://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg");
                        string yanzhengma = shibie();
                        string url = "https://puser.zjzwfw.gov.cn/sso/newusp.do";
                        string postdata = "action=getUserid&loginname=" + phone + "&verifycode=" + yanzhengma;
                        string html = PostUrl(url, postdata);
                        while (html == "{\"result\":-1}")
                        {
                            textBox1.Text = DateTime.Now.ToShortTimeString() + "识别错误...";
                            yanzhengma = shibie();
                            postdata = "action=getUserid&loginname=" + phone+ "&verifycode=" + yanzhengma;
                            html = PostUrl(url, postdata);
                            Thread.Sleep(100);
                        }
                        if (html == "{\"result\":-2}")
                        {
                            lv1.SubItems.Add("账号不存在");
                        }


                        //string mobilephone = Regex.Match(html, @"""mobilephone"":""([\s\S]*?)""").Groups[1].Value;       
                        //if (mobilephone != "")
                        //{
                        //    if (mobilephone.Substring(0, 3) == phone.Substring(0, 3) && mobilephone.Substring(7, 4) == phone.Substring(7, 4))
                        //    {
                        //        lv1.SubItems.Add("true");

                        //    }
                        //    else
                        //    {
                        //        lv1.SubItems.Add("false");

                        //    }

                        //}
                        //else
                        //{
                        //    lv1.SubItems.Add("手机号不存在");
                        //}


                        string idcard = Regex.Match(html, @"""idcard"":""([\s\S]*?)""").Groups[1].Value;
                        if (idcard != "")
                        {
                          
                            if (idcard.Substring(0, 1) == card.Substring(0,1) && idcard.Substring(idcard.Length-1, 1) == card.Substring(card.Length - 1, 1))
                            {
                                lv1.SubItems.Add("true");

                            }
                            else
                            {
                                lv1.SubItems.Add("false");

                            }

                        }
                        else
                        {
                            lv1.SubItems.Add("身份证号不存在");
                        }


                    }
                    catch (Exception ex)
                    {
                        lv1.SubItems.Add("触发异常");
                        continue;
                    }
                   
                            

                }

            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }

        private delegate string Encrypt(string pwd);//代理

        public string getencrypt(string pwd)
        {

            string result = webBrowser1.Document.InvokeScript("RSA", new object[] { pwd }).ToString();
            return result;
        }


        /// <summary>
        /// 注册页码判断手机号
        /// </summary>
        public void run2()
        {
            textBox1.Text = DateTime.Now.ToLongTimeString() + ": 开始查询";
            cookie = "_uab_collina=163160610589460717741822; JSESSIONID=7C3F4B8B9F4F3BD39650544B87932AAC; ZJZWFWSESSIONID=13b2d86e-95c4-4e66-b985-f26753d17810";


            try
            {
                DataTable dt = method.ExcelToDataTable(textBox4.Text, true);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    try
                    {
                        string name = dt.Rows[i][1].ToString().Trim();
                        string card = dt.Rows[i][0].ToString().Trim();
                        string phone = dt.Rows[i][2].ToString().Trim();

                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(card);
                        lv1.SubItems.Add(phone);


                   
                        string url = "https://puser.zjzwfw.gov.cn/sso/newusp.do";

                        Encrypt aa = new Encrypt(getencrypt);
                        IAsyncResult iar = BeginInvoke(aa, new object[] { phone });
                        string phonecrypt = EndInvoke(iar).ToString();


                        string postdata = "action=regByMobile&mobilephone="+ System.Web.HttpUtility.UrlEncode(phonecrypt);
                        string html = PostUrl(url, postdata);

                        //MessageBox.Show(html);


                        string username= Regex.Match(html, @"""username"":""([\s\S]*?)""").Groups[1].Value;
                        if (username != "")
                        {
                            //名字最后一位相同 则匹配
                            if (username.Substring(username.Length - 1, 1) == name.Substring(name.Length - 1, 1))
                            {
                                lv1.SubItems.Add("true");

                            }
                            else
                            {
                                lv1.SubItems.Add("false");

                            }

                        }
                        else
                        {
                            lv1.SubItems.Add("空");
                        }

                        lv1.SubItems.Add(html);
                    }
                    catch (Exception ex)
                    {
                        lv1.SubItems.Add("触发异常");
                        lv1.SubItems.Add("触发异常");
                        continue;
                    }



                }

            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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

        private void 浙江身份号码验证_Load(object sender, EventArgs e)
        {
            //WebBrowser web = new WebBrowser();
            //web.Navigate("http://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg");
           webBrowser1.Navigate("https://puser.zjzwfw.gov.cn/sso/newusp.do?action=register&servicecode=zjdsjgrbs#"); //按照姓名找回 执行加密RSA JS方法
        }
    }
}
