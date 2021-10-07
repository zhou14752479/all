using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;

namespace 江苏政务注册
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
        public Form1()
        {
            InitializeComponent();

        }

        string shoujiusername = "";
        string shoujipassword = "";

        string yzmusername = "";
        string yzmpassword = "";

        string cookie = "";
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
               // request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "http://www.jszwfw.gov.cn/jsjis/h5/jszfjis/view/perregister.html";
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
                Bitmap image = UrlToBitmap("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.22401224248317164;");



                string param = "{\"username\":\"" + yzmusername + "\",\"password\":\"" + yzmpassword + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);
             
                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                if (result.Groups[1].Value != "")
                {
                   
                    return result.Groups[1].Value;
                }
                else
                {
                   
                    logtxtBox.Text += "图片验证码错误:"+PostResult + "\r\n";
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


        public string token = "";

        public void gettoken()
        {
            string url = "http://yccode.net:9321/api/auth?userName=" + textBox1.Text.Trim() + "&passWord=" + textBox2.Text.Trim() + "&userType=1";
            string html = method.GetUrl(url, "utf-8");
            token = Regex.Match(html, @"token"":""([\s\S]*?)""").Groups[1].Value;

        }
        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <returns></returns>
        public string getmobile()
        {

            string url = "http://yccode.net:9321/api/getPhoneNumber?token=" + token + "&projectId=10812&mobileNo=&sectionNo=&taskCount=&selectOperator=&mobileCarrier=&regionalCondition=&selectArea=&area=&taskType=1&usedNumber=&manyTimes=";
            string html = method.GetUrl(url, "utf-8");

            string mobileNo = Regex.Match(html, @"mobileNo"":""([\s\S]*?)""").Groups[1].Value;
            string mId = Regex.Match(html, @"mId"":""([\s\S]*?)""").Groups[1].Value;
            if (mId != "")
            {
                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "获取手机号成功" + "\r\n";
                return mobileNo + "&" + mId;

            }
            else
            {
                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "获取手机号失败" + html + "\r\n";
                return "";
            }
        }

        int dengdaiduanxinmaseconds = 0;
        /// <summary>
        /// 获取手机短信
        /// </summary>
        /// <returns></returns>
        public string getduanxinma(string mid)
        {
            Thread.Sleep(5000);
            dengdaiduanxinmaseconds = dengdaiduanxinmaseconds + 5;
            string url = "http://yccode.net:9321/api/getMessage?token=" + token + "&mId=" + mid + "&developer=";
            string html = method.GetUrl(url, "utf-8");
          
            string code = Regex.Match(html, @"\d{6}").Groups[0].Value;
            if (code != "")
            {

                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "获取手机短信验证码码成功" + "\r\n" + html;
                return code;

            }
            else
            {
                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "正在获取手机短信码,还未收到,已等待" + dengdaiduanxinmaseconds + "\r\n" + html;
                return "";
            }

        }

        string fasongmsg = "";

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="imgcode"></param>
        /// <param name="card"></param>
        public bool sendmobile(string mobile,string imgcode,string card)
        {
            string url = "http://www.jszwfw.gov.cn/jsjis/h5/interface/sendmobilecode.do?mobile="+mobile+"&verifycode="+imgcode+"&usertype=1&papersNumber="+card;
            string html = method.GetUrlWithCookie(url,cookie,"utf-8");
          
            if (html.Contains("成功"))
            {
                logtxtBox.Text += "发送手机短信验证码成功" + "\r\n";
                return true;
            }
            else
            {
                logtxtBox.Text += "发送手机短信验证码失败"+html + "\r\n";
                fasongmsg = html;
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
                string[] value = secret.Split(new string[] { "asd147" }, StringSplitOptions.None);

               
                    if (Convert.ToInt32(value[1]) < Convert.ToInt32(myDLL.method.GetTimeStamp()))
                    {
                        MessageBox.Show("激活已过期");
                        string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", myDLL.method.GetMD5(myDLL.method.GetMacAddress()), -1, -1);
                        string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

                        if (text[0] == myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian"))
                        {
                            IniWriteValue("values", "key", myDLL.method.GetMD5(myDLL.method.GetMacAddress()));
                            IniWriteValue("values", "secret", str);
                            MessageBox.Show("激活成功");


                        }
                        else
                        {
                            MessageBox.Show("激活码错误");
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                            return;
                        }

                    }




                    else if (value[0] != myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian") || key != myDLL.method.GetMD5(myDLL.method.GetMacAddress()))
                    {

                        string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", myDLL.method.GetMD5(myDLL.method.GetMacAddress()), -1, -1);
                        string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);

                        if (text[0] == myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian"))
                        {
                            IniWriteValue("values", "key", myDLL.method.GetMD5(myDLL.method.GetMacAddress()));
                            IniWriteValue("values", "secret", str);
                            MessageBox.Show("激活成功");


                        }
                        else
                        {
                            MessageBox.Show("激活码错误");
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                            return;
                        }
                    }
               

            }
            else
            {

                string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", myDLL.method.GetMD5(myDLL.method.GetMacAddress()), -1, -1);
                string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);
                if (text[0] == myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian"))
                {
                    IniWriteValue("values", "key", myDLL.method.GetMD5(myDLL.method.GetMacAddress()));
                    IniWriteValue("values", "secret", str);
                    MessageBox.Show("激活成功");


                }
                else
                {
                    MessageBox.Show("激活码错误");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }






            if (ExistINIFile())
            {
                textBox1.Text = IniReadValue("values", "shoujiusername");
                textBox2.Text = IniReadValue("values", "shoujipassword");
                textBox3.Text = IniReadValue("values", "yzmusername");
                textBox4.Text = IniReadValue("values", "yzmpassword");
            }


            shoujiusername = textBox1.Text.Trim();
            shoujipassword = textBox2.Text.Trim();
            yzmusername = textBox3.Text.Trim();
            yzmpassword = textBox4.Text.Trim();
        }
        bool status = true;
       
      
        Thread thread;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

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
                    string card = dr[1].ToString();


                    string mobilemid = getmobile();
                    string[] text = mobilemid.Split(new string[] { "&" }, StringSplitOptions.None);
                    if (text.Length < 1)
                    {
                        logtxtBox.Text = "获取手机号失败";
                        return;
                    }

                 
                    string mobile = text[0];
                    string mid = text[1];


                    bool fasongstatus = sendmobile(mobile, shibie(), card);
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
                        fasongstatus = sendmobile(mobile, shibie(), card);
                    }
                    string mobilecode = getduanxinma(mid);
                    while (true)
                    {
                        if (status == false)
                        {
                            return;
                        }
                        mobilecode = getduanxinma(mid);
                        if (dengdaiduanxinmaseconds == Convert.ToInt32(textBox6.Text))
                        {
                            dengdaiduanxinmaseconds = 0;
                            mobile = getmobile();
                            sendmobile(mobile, shibie(), card);
                        }
                        if (mobilecode != "")
                        {
                            dengdaiduanxinmaseconds = 0;
                            break;
                        }



                    }
                    string url = "http://www.jszwfw.gov.cn/jsjis/h5/interface/perregisterNew.do?jsondata=%7B%22pwd%22%3A%22123q123q%22%2C%22name%22%3A%22" + name + "%22%2C%22papersNumber%22%3A%22" + card + "%22%2C%22paperType%22%3A%221%22%2C%22realType%22%3A1%2C%22mobile%22%3A%22" + mobile + "%22%2C%22mobileCode%22%3A%22" + mobilecode + "%22%7D";
                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(card);
                    lv1.SubItems.Add(dr[0].ToString());
                    lv1.SubItems.Add(mobile);
                    lv1.SubItems.Add(html);

                }
                catch (Exception ex)
                {

                    continue;
                }
            }




        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"Xzp4YYO"))
            {

                return;
            }



            #endregion
            gettoken();
            logtxtBox.Text = "开始执行..";
            if (textBox5.Text == "")
            {
                MessageBox.Show("请先导入数据表格");
                return;
            }

            if (cookie == "")
            {
                MessageBox.Show("请先点击获取COOKIES");
                return;

            }

            IniWriteValue("values", "shoujiusername",textBox1.Text.Trim());
            IniWriteValue("values", "shoujipassword", textBox2.Text.Trim());
            IniWriteValue("values", "yzmusername", textBox3.Text.Trim());
            IniWriteValue("values", "yzmpassword", textBox4.Text.Trim());


            status = true;
            timer1.Start();
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
            timer1.Stop();
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

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            logtxtBox.Select(this.logtxtBox.TextLength, 0);
            logtxtBox.ScrollToCaret();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            WebBrowser browser = new WebBrowser();
            browser.Url = new Uri("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.08807624779516066;");
            cookie = method.GetCookies("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.08807624779516066;");
            while (true)
            {
                cookie = method.GetCookies("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.08807624779516066;");
                if (cookie != null)
                {
                    //if (cookie.Contains("JSESSIONID"))
                    //{
                    //    MessageBox.Show("成功");

                    //    break;
                    //}
                    if (cookie.Contains("SESSIONID"))
                    {
                        MessageBox.Show("成功");

                        break;
                    }
                }
                
            }
            
        }


        public void setcookies()
        {
            WebBrowser browser = new WebBrowser();
            browser.Url = new Uri("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.08807624779516066;");
            cookie = method.GetCookies("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.08807624779516066;");
            while (true)
            {
                cookie = method.GetCookies("http://www.jszwfw.gov.cn/jsjis/admin/verifyCode.do?code=4&var=rand&width=90&height=45&random=0.08807624779516066;");
                if (cookie != null)
                {
                    if (cookie.Contains("JSESSIONID"))
                    {
                        //MessageBox.Show("成功");
                        logtxtBox.Text = "更新cookies成功";
                        break;
                    }
                }

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            setcookies();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
