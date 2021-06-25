using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
using myDLL;


namespace 主程序202106
{
    public partial class 会计成绩查询 : Form
    {
        [DllImport("AspriseOCR.dll", EntryPoint = "OCR", CallingConvention = CallingConvention.Cdecl)]

        public static extern IntPtr OCR(string file, int type);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpart", CallingConvention = CallingConvention.Cdecl)]

        static extern IntPtr OCRpart(string file, int type, int startX, int startY, int width, int height);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRBarCodes", CallingConvention = CallingConvention.Cdecl)]

        static extern IntPtr OCRBarCodes(string file, int type);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpartBarCodes", CallingConvention = CallingConvention.Cdecl)]

        static extern IntPtr OCRpartBarCodes(string file, int type, int startX, int startY, int width, int height);
        public 会计成绩查询()
        {
            InitializeComponent();
        }

      

        string cookie = "";
        string yzmusername = "zhou14752479";
        string yzmpassword = "zhoukaige00";


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
                request.Referer = "";
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
                string url = "http://kzp.mof.gov.cn/cjcx/img.jsp?timestamp=" + GetTimeStamp();
                Bitmap image = UrlToBitmap(url);



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
        public string shibie1()
        {
            try
            {
                string url = "http://kzp.mof.gov.cn/cjcx/img.jsp?timestamp=" + GetTimeStamp();
                Bitmap image = UrlToBitmap(url);


                string path = AppDomain.CurrentDomain.BaseDirectory + "yzm.jpg";
               
                image.Save(path);
               
                Image img = Image.FromFile(path);
            string code =  Marshal.PtrToStringAnsi(OCRpart(path, -1, 0, 0, img.Width, img.Height));
           
                if (code != "")
                {

                    return code.Replace(",","").Replace("T", "7");
                }
                else
                {

                 label1.Text += "图片验证码错误:" + code + "\r\n";
                    //status = false;
                    return "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                ex.ToString();
                return "";
            }
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
        public void getcookies()
        {
            string url = "http://kzp.mof.gov.cn/cjcx/1_c4036a022596222.jsp";
            WebBrowser browser = new WebBrowser();

            cookie = method.GetCookies(url);
            while (true)
            {
                cookie = method.GetCookies(url);
                if (cookie != null)
                {
                    MessageBox.Show(cookie);
                    if (cookie.Contains("JSESSIONID"))
                    {

                        break;
                    }
                }

            }
        }

        bool status = true;

        Thread thread;
        
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            string pro = "20";

            if (textBox2.Text == "")
            {
                MessageBox.Show("请导入账号");
                return;
            }

            if (cookie == "")
            {
                MessageBox.Show("请先点击获取COOKIE");
                return;
            }

            StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
             
                try
                {

                    if (status == false)
                    {
                        return;
                    }
                  
              
                    string card = Regex.Match(text[i],@"\d{15,}").Groups[0].Value.Trim();
                    string name = Regex.Replace(text[i].Trim(), @"\d", "").Trim();
                    string tel = text[i].Trim().Substring(text[i].Trim().Length-11,11).Trim();
                  
                    string url = "http://kzp.mof.gov.cn/cjcx/2_c4036a022596222.jsp";
                    string code = shibie();
                    string procode = card.Substring(0, 2);
                    switch (procode)
                    {
                        case "12":
                            pro = "12";
                            break;
                        case "13":
                            pro = "13";
                            break;
                        case "14":
                            pro = "14";
                            break;
                        case "15":
                            pro = "15";
                            break;
                        case "21":
                            pro = "16";
                            break;
                        case "22":
                            pro = "17";
                            break;
                        case "23":
                            pro = "18";
                            break;
                        case "31":
                            pro = "19";
                            break;
                        case "32":
                            pro = "20";
                            break;
                        case "33":
                            pro = "21";
                            break;
                        case "34":
                            pro = "22";
                            break;
                        case "35":
                            pro = "23";
                            break;
                        case "36":
                            pro = "24";
                            break;
                        case "37":
                            pro = "25";
                            break;
                        case "41":
                            pro = "26";
                            break;
                        case "42":
                            pro = "27";
                            break;
                        case "43":
                            pro = "28";
                            break;
                        case "44":
                            pro = "29";
                            break;
                        case "45":
                            pro = "30";
                            break;
                        case "46":
                            pro = "31";
                            break;
                        case "51":
                            pro = "32";
                            break;
                        case "50":
                            pro = "33";
                            break;
                        case "52":
                            pro = "34";
                            break;
                        case "53":
                            pro = "35";
                            break;
                        case "54":
                            pro = "36";
                            break;
                        case "61":
                            pro = "37";
                            break;
                        case "62":
                            pro = "38";
                            break;
                        case "63":
                            pro = "39";
                            break;
                        case "64":
                            pro = "40";
                            break;


                    }






                    string postdata = "province="+pro+"&sfzh="+card+"&xm="+ System.Web.HttpUtility.UrlEncode(name) +"&atch="+code;
               
                    string html = PostUrl(url, postdata);
                 
                    string grade1 = "0";
                    string grade2= "0";

                    if (html.Contains("经济法基础")) //查询成功
                    {
                        MatchCollection haos = Regex.Matches(html, @"</font></td>([\s\S]*?)</font>");

                        grade1 = Regex.Replace(haos[0].Groups[1].Value, "<[^>]+>", "").Trim();
                        grade2 = Regex.Replace(haos[1].Groups[1].Value, "<[^>]+>", "").Trim();
                        method.GetUrlWithCookie("http://kzp.mof.gov.cn/cjcx/1_c4036a022596222.jsp", cookie, "utf-8");
                        label1.Text = name + "：" + Convert.ToInt32(grade1).ToString() + "  " + Convert.ToInt32(grade2).ToString();
                    }
                    else
                    {
                        method.GetUrlWithCookie("http://kzp.mof.gov.cn/cjcx/1_c4036a022596222.jsp", cookie, "utf-8");
                        if (html.Contains("未找到"))
                        {
                            label1.Text = name + "：未找到";
                        }
                       else if (html.Contains("正确输入验证码"))
                        {
                            label1.Text = name + "：验证码错误";
                            i = i - 1;
                            continue;
                        }
                        
                    }



                    if (grade1 == "无")
                    {
                        grade1 = "0";
                    }
                    if (grade2 == "无")
                    {
                        grade2 = "0";
                    }

                   
                    if (Convert.ToInt32(grade1) >= 60 && Convert.ToInt32(grade2) >= 60)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(card);
                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(tel);
                       
                    }
                    else
                    {
                        ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据   
                        lv2.SubItems.Add(card);
                        lv2.SubItems.Add(name);
                        lv2.SubItems.Add(tel);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    continue;
                }

                Thread.Sleep(2000);
            }




        }

        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview)
        {

            //if (listview.Items.Count == 0)
            //{
            //    MessageBox.Show("列表为空!");
            //}

            List<string> list = new List<string>();
            foreach (ListViewItem item in listview.Items)
            {
                if (item.SubItems[1].Text.Trim() != "")
                {

                    list.Add(item.SubItems[1].Text+ "	"+ item.SubItems[2].Text+ "	" + item.SubItems[3].Text);
                }


            }
            SaveFileDialog sfd = new SaveFileDialog();

            // string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";
            string path = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName + ".txt";
            }
            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);


        }






        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"DZkGmf"))
            {

                return;
            }

            #endregion
            status = true;
            try
            {
              
                StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
            //string filePath = @"C:\Users\zhou\Desktop\img.jpg";
            //Image img = Image.FromFile(filePath);
            //MessageBox.Show(Marshal.PtrToStringAnsi(OCRpart(filePath, -1, 0, 0, img.Width, img.Height)).Replace("T", "7"));


        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sfd.FileName;
            }
        }

        private void 会计成绩查询_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Process.Start(path + "kuaijihelper.exe");
        }

        private void button5_Click(object sender, EventArgs e)
        {
           ListviewToTxt(listView1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView2);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
