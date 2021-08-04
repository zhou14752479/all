using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace 微信人脉平台采集
{
    public partial class 微信人脉平台采集1 : Form
    {
        public 微信人脉平台采集1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        bool status = true;
        Thread thread;
        string path = AppDomain.CurrentDomain.BaseDirectory+"images/";
        string COOKIE = "Google=979543f4051ca3595dc3a1492a98a3ed";
        //string COOKIE = "Google=0957ad901750081ce28a1ad8b9764dd9";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string token)
        {
            string html = "";
            
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;
                WebHeaderCollection headers = request.Headers;
                headers.Add("token:"+token);
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

                ex.ToString();
            }
        }



        #endregion

        #region  将Base64字符串转换为图片
        /// <summary>

        /// 将Base64字符串转换为图片

        /// </summary>

        /// <param name="sender"></param>

        /// <param name="e"></param>

        private Image ToImage(string base64)

        {



            try
            {

                byte[] arr = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(arr);
               Bitmap bitmap = new Bitmap(ms);
                Image img = bitmap;
               
                ms.Close();
               
                return img;

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Base64StringToImage 转换失败\nException：" + ex.Message);
                return null;
            }



        }

        #endregion

        string captcha_key = "";
        string token = "";
        public void getcaptcha_key()
        {
            string url = "http://www.huangniba.top/tool/getCaptcha";
            string html = GetUrl(url,"11");  //定义的GetRul方法 返回 reader.ReadToEnd()
            captcha_key = Regex.Match(html, @"""key"":""([\s\S]*?)""").Groups[1].Value;
          // string base64 = Regex.Match(html, @"base64,([\s\S]*?)""").Groups[1].Value.Replace("\\r\\n", "").Replace("\\", "");
           
        }

     
        

            private bool gettoken()
            {
            getcaptcha_key();
            HttpWebResponse response = null;

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.huangniba.top/Login/webLogin");

                    request.KeepAlive = true;
                    request.Accept = "application/json, text/plain, */*";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
                    request.Headers.Add("token", @"null");
                    request.Headers.Add("tid", @"null");
                    request.ContentType = "application/json;charset=UTF-8";
                    request.Headers.Add("Origin", @"http://www.huangniba.top");
                    request.Referer = "http://www.huangniba.top/431484/?/";
                    request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                    request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                   request.Headers.Set(HttpRequestHeader.Cookie, COOKIE);

                    request.Method = "POST";
                    request.ServicePoint.Expect100Continue = false;

                    string body = "{\"username\":\""+textBox3.Text.Trim()+"\",\"password\":\""+textBox4.Text.Trim()+"\",\"captcha_code\":\"\",\"captcha_key\":\""+captcha_key+"\"}";
                    byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                    request.ContentLength = postBytes.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(postBytes, 0, postBytes.Length);
                    stream.Close();

                    response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
              string html = reader.ReadToEnd();
                //MessageBox.Show(html);
               token= Regex.Match(html, @"""token"":""([\s\S]*?)""").Groups[1].Value;
                if (token != "")
                {
                   
                    MessageBox.Show("登录成功");
                    getuserinfo();
                }
                else
                {
                    MessageBox.Show("登录失败");
                    MessageBox.Show(html);
                }
            }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                    else return false;
                }
                catch (Exception)
                {
                    if (response != null) response.Close();
                    return false;
                }

                return true;
            }

        private void getuserinfo()
        {
            string html = GetUrl("http://www.huangniba.top/App/getUserInfo",token);
            string name = Regex.Match(html, @"""nickname"":""([\s\S]*?)""").Groups[1].Value.Replace(".", "");
            groupBox2.Text = "当前账号：   "+name;
            
          
        }



        public string ReplaceBadCharOfFileName(string fileName)
        {
            string str = fileName;
            str = str.Replace("\\", string.Empty);
            str = str.Replace("/", string.Empty);
            str = str.Replace(":", string.Empty);
            str = str.Replace("*", string.Empty);
            str = str.Replace("?", string.Empty);
            str = str.Replace("\"", string.Empty);
            str = str.Replace("<", string.Empty);
            str = str.Replace(">", string.Empty);
            str = str.Replace("|", string.Empty);
            str = str.Replace("&", string.Empty);
            str = str.Replace(" ", string.Empty);    //前面的替换会产生空格,最后将其一并替换掉
            return str;
        }

        public void creatQcode(string path, string currentPath)
        {
            try
            {
                int width = 232; //图片宽度
                int height = 232;//图片长度
                BarcodeWriter barCodeWriter = new BarcodeWriter();
                barCodeWriter.Format = BarcodeFormat.QR_CODE; // 生成码的方式(这里设置的是二维码),有条形码\二维码\还有中间嵌入图片的二维码等
                barCodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");// 支持中文字符串
                barCodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
                barCodeWriter.Options.Height = height;
                barCodeWriter.Options.Width = width;
                barCodeWriter.Options.Margin = 0; //设置的白边大小
                ZXing.Common.BitMatrix bm = barCodeWriter.Encode(path);  //DNS为要生成的二维码字符串
                Bitmap result = barCodeWriter.Write(bm);
                Bitmap Qcbmp = result.Clone(new Rectangle(Point.Empty, result.Size), PixelFormat.Format1bppIndexed);//位深度

                Qcbmp.Save(currentPath);

            }
            catch (Exception ex)
            {

                textBox1.Text = (ex.ToString());


            }
        }

        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }

        public bool shaixuan(string time)
        {
            if (radioButton1.Checked == true)
            {
                if (Convert.ToInt64(GetTimeStamp()) - Convert.ToInt64(time) <= 3600)
                {
                    return true;
                }
            }
            if (radioButton2.Checked == true)
            {
                if (Convert.ToInt64(GetTimeStamp()) - Convert.ToInt64(time) <= 21600)
                {
                    return true;
                }
            }
            if (radioButton3.Checked == true)
            {
                if (Convert.ToInt64(GetTimeStamp()) - Convert.ToInt64(time) <= 43200)
                {
                    return true;
                }
            }
            if (radioButton4.Checked == true)
            {
                if (Convert.ToInt64(GetTimeStamp()) - Convert.ToInt64(time) <= 86400)
                {
                    return true;
                }
            }
            if (radioButton5.Checked == true)
            {
                if (Convert.ToInt64(GetTimeStamp()) - Convert.ToInt64(time) <= 432000)
                {
                    return true;
                }
            }

            return false;
        }

        #region  主程序
        public void run()
        {
            try
            {
                string type = "all";
               
                if (radioButton6.Checked == true)
                {
                    type = "all";
                }
                if (radioButton7.Checked == true)
                {
                    type = "good";
                }
                for (int page = Convert.ToInt32(textBox5.Text.Trim()); page < 999999; page++)
                {
                    textBox5.Text = page.ToString();
                    string url = "http://www.huangniba.top/Quncard/getQuncardList?page="+page+"&words=&type="+type;

                    string html = GetUrl(url, token);  //定义的GetRul方法 返回 reader.ReadToEnd()
                    

                    MatchCollection uids = Regex.Matches(html, @"""id"":([\s\S]*?),");

                    textBox1.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  获取到个数：" + uids.Count +  "\r\n";
                    if (uids.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                    {
                      
                        textBox2.Text = textBox2.Text + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "等待60秒...\r\n";
                        Thread.Sleep(60000);
                     
                        if (page > 1)
                        {
                            page--;
                        }
                    }

                    for (int j = 0; j < uids.Count; j++)
                    {
                        if (uids[j].Groups[1].Value.Length > 4)
                        {

                            string aurl = "http://www.huangniba.top/Quncard/getQunCard?id=" + uids[j].Groups[1].Value;
                            string ahtml = GetUrl(aurl, token);

                            string name = Regex.Match(ahtml, @"""name"":""([\s\S]*?)""").Groups[1].Value.Replace(".", "");
                            string renshu = Regex.Match(ahtml, @"""num"":([\s\S]*?),").Groups[1].Value;
                            string picurl = Regex.Match(ahtml, @"""qrcode"":""([\s\S]*?)""").Groups[1].Value.Replace("\\", "");
                            string time = Regex.Match(ahtml, @"""refresh_time"":([\s\S]*?),").Groups[1].Value;
                            if (picurl != "")
                            {
                                if (time != "")
                                {
                                    if (shaixuan(time))
                                    {
                                        string rename = ReplaceBadCharOfFileName(name) + "----" + renshu + "----" + DateTime.Now.ToString("yyyy-MM-dd");
                                        creatQcode(picurl, path + rename + ".jpg");
                                        textBox1.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + name + "下载成功" + "\r\n";
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (status == false)
                                            return;

                                    }
                                    else
                                    {
                                        textBox1.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + name + "不符合时间" + "\r\n";
                                    }
                                }

                                else
                                {
                                    textBox1.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + name + "时间为空" + "\r\n";
                                }
                            }

                            else
                            {
                                textBox1.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + name + "二维码为空" + "\r\n";
                            }
                        }

                        Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000);
                    }

                    Thread.Sleep(1000);
                }

            }
            catch (System.Exception ex)
            {
              textBox1.Text= (ex.ToString());
            }



        }



        #endregion

        private void 微群人脉平台采集_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html =GetUrl("http://www.acaiji.com/index/index/vip.html", "11");

            if (!html.Contains(@"联通手机卡"))
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

       

        private void button4_Click(object sender, EventArgs e)
        {
            gettoken();
        }

        private void 微信人脉平台采集1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
