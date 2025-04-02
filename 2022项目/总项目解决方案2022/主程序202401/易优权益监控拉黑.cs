using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;

namespace 主程序202401
{

    
    



        public partial class 易优权益监控拉黑 : Form
        {


        public static string GetDouyinVideoId(string url)
        {
           
            try
            {

                string html = GetUrl2(url,"utf-8");
              
                var match = Regex.Match(html, @"/video/(\d+)");

                return match.Groups[1].Value;
            }
            catch (Exception ex)
            {
                
                return "";
            }
        }


        public  string Getlikecount(string link)
        {

            try
            {
                string url = "https://api.itfaba.com/dyVideo/detail?apiKey=f00619b4f4014601f27ce91468089827";
                string postdata = "shorturl="+link;
                string html =PostUrlDefault2(url,postdata, "");
               
                var match = Regex.Match(html, @"""digg_count"":([\s\S]*?),");

                return match.Groups[1].Value.Trim();
            }
            catch (Exception ex)
            {

                return "";
            }
        }


        public 易优权益监控拉黑()
        {
            InitializeComponent();
        }


        #region POST默认请求
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion

        #region POST默认请求
        public static string PostUrlDefault2(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion


        #region GET请求
        public string GetUrl(string Url, string charset)
        {
            string COOKIE = "";
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //request.Proxy = null;
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                WebHeaderCollection headers = request.Headers;
              
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region GET请求
        public static string GetUrl2(string Url, string charset)
        {
            string COOKIE = "";
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //request.Proxy = null;
                request.AllowAutoRedirect = false;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        string COOKIE = "";

        
        #region 网址转图片
        public static Image GetImage(string url)
        {
            Image result;
            try
            {
                WebRequest request = WebRequest.Create(url);
                Image img;
                using (WebResponse response = request.GetResponse())
                {
                    bool flag = response.ContentType.IndexOf("text") != -1;
                    if (flag)
                    {
                        Stream responseStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        string srcString = reader.ReadToEnd();
                        return null;
                    }
                    img = Image.FromStream(response.GetResponseStream());
                }
                result = img;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                result = null;
            }
            return result;
        }
        #endregion


        #region  时间戳转时间
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(Convert.ToDouble(timeStamp));
        }
        #endregion

        public static string cookie = "";
        public void login()
        {

            if (cookie.Trim() != "")
                return;
            string postData = "{\"account\":\"wuxuedi\",\"password\":\"ad19991015\"}";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + textBox2.Text.Trim() + "/api/console/login/verify");
            request.Method = "Post";
           // request.Headers.Add("Cookie", "sessionID=4937ff6cf2bc045ee6809d13cc93ba84; ");
            //request.Proxy = null;
            //WebHeaderCollection headers = request.Headers;
            //headers.Add("version:TYC-XCX-WX");
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json";
            request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
            request.Headers.Add("Accept-Encoding", "gzip");
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            cookie = response.GetResponseHeader("Set-Cookie");
            string token = Regex.Match(cookie, @"auth_token=([\s\S]*?);").Groups[1].Value;
            cookie = "auth_token=" + token;
            if (token != "")
            {
                label1.Text = "登录成功";
            }

          
        }

 

        Dictionary<string, string> dics = new Dictionary<string, string>();


        Dictionary<string, string> dic2= new Dictionary<string, string>();


        List<string> list = new List<string>();


        public void readdata()
        {
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                string[] texta = text[i].Split(new string[] { "#" }, StringSplitOptions.None);

                if(!dics.ContainsKey(texta[2]))
                {
                    dics.Add(texta[2], texta[1]);
                }
               
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }

        public  void run()
        {
            readdata();
            listView1.Items.Clear();
            try
            {

              


                string url = "http://" + textBox2.Text.Trim() + "/api/console/OrderManage/Paging";

                string html = PostUrlDefault(url, "{\"type\":0,\"page\":1,\"list_rows\":50,\"status\":3,\"goods_name\":\"\",\"docking_status\":0,\"id\":null,\"goods_id\":null,\"customer_id\":null,\"buy_params\":null,\"docking_site_id\":0,\"supplier_id\":null}", cookie);


             
                string html2 = PostUrlDefault(url, "{\"type\":0,\"page\":1,\"list_rows\":50,\"status\":1,\"goods_name\":\"\",\"docking_status\":0,\"id\":null,\"goods_id\":null,\"customer_id\":null,\"buy_params\":null,\"docking_site_id\":0,\"supplier_id\":null}", cookie);


                html = html2 + html;


                if (html.Contains("登录"))
                {
                    login();
                }
                MatchCollection links = Regex.Matches(html, @"name"": ""作品链接([\s\S]*?)""value"": ""([\s\S]*?)""");
                MatchCollection ids = Regex.Matches(html, @"""id"":([\s\S]*?),");

                string pattern = @"status_changes([\s\S]*?)status"":([\s\S]*?),";
                MatchCollection status = Regex.Matches(html, pattern);



                for (int i = 0; i < links.Count; i++)
                {
                   
                    try
                    {
                        string link = links[i].Groups[2].Value;
                       
                        string uid = "";

                        if (!dic2.ContainsKey(link))
                        {
                            uid = GetDouyinVideoId(link);
                            dic2.Add(link, uid);
                        }

                       else
                        {
                            uid=dic2[link]; 
                        }
                       
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(ids[2 * i].Groups[1].Value);
                        lv1.SubItems.Add(link);
                      
                        lv1.SubItems.Add(uid);
                      
                        //判断是否拉黑
                        if(dics.ContainsKey(uid))
                        {
                           
                            lv1.SubItems.Add(dics[uid]);


                            //避免重复提醒
                            if(!list.Contains(uid))
                            {
                                list.Add(uid);
                                Speak("发现拉黑链接");
                                sendmsg(link, "ID:" + ids[2 * i].Groups[1].Value + "链接：" + link);
                            }
                        }
                        else
                        {
                            lv1.SubItems.Add("");
                        }

                        lv1.SubItems.Add(status[i].Groups[2].Value.Replace("1","已付款").Replace("3", "处理中"));

                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show(ex.ToString());
                        continue;
                    }
                }


               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }
        public static void Speak(string text)
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                // 设置语音参数
                synth.Volume = 100;  // 音量 0-100
                synth.Rate = 0;     // 语速 -10到10

                // 选择语音（需系统已安装对应语音包）
                foreach (var voice in synth.GetInstalledVoices())
                {
                    if (voice.VoiceInfo.Culture.Name == "zh-CN") // 使用中文语音
                    {
                        synth.SelectVoice(voice.VoiceInfo.Name);
                        break;
                    }
                }

                synth.Speak(text);  // 同步播放
              // synth.SpeakAsync(text); // 异步播放
            }
        }

        #region 发送wxpush消息
        public void sendmsg(string title, string neirong)
        {
            bool flag = title.Trim() != "";
            if (flag)
            {

                string url = "https://wxpusher.zjiecode.com/api/send/message";
                string postdata = string.Concat(new string[]
                {
                    "{\"appToken\":\"AT_LtFEsRJbxhHfYSd5RVWGMXsaGYHPRm8c\",\"content\":\"",
                    title,
                    neirong,
                    "\",\"contentType\":2,\"uids\":[\""+textBox1.Text+"\"],  \"verifyPay\":false, \"verifyPayType\":0}" });
                string html = PostUrlDefault(url, postdata, "");
            }
        }
        #endregion
        Thread thread;
        public static string domain= "gl.yy114.top";
        private void 易优权益监控拉黑_Load(object sender, EventArgs e)
        {
           
            pictureBox1.Image = GetImage("http://wxpusher.zjiecode.com/api/qrcode/voD1HgXICEUzBMQQq6fhNY996iSiqKBCpml2CBqxshU0abAJJq5AVGerJUHT1H4u.jpg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入网址");
                return;
            }
           
            易优权益监控拉黑.domain = textBox2.Text.Trim();
            label1.Text = "正在登录....";
            login();
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public string readTxt(string name)
        {
            listView1.Items.Clear();
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\"+name+".txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\"+name+".txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            sr.Close();  //只关闭流
            sr.Dispose();

            return texts;   

        }


        private void button3_Click(object sender, EventArgs e)
        {

           // MessageBox.Show(Getlikecount("https://v.douyin.com/i5vV3Y8m/"));
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

      
      

        private  void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "")
            {

                string uid = GetDouyinVideoId(textBox3.Text.Trim());
                if(uid=="")
                {
                    MessageBox.Show("获取uid失败");
                    return;
                }

                textBox5.Text = uid;
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(textBox3.Text + "#" + textBox4.Text + "#" + uid);
                sw.Close();
                fs1.Close();
                sw.Dispose();
                MessageBox.Show("添加成功");
            }
        }


        public void writeTxt(string txt,string name)
        {

         
            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\"+name+".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(txt);
            sw.Close();
            fs1.Close();
            sw.Dispose();
          
        }

        private void 易优权益监控拉黑_FormClosing(object sender, FormClosingEventArgs e)
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

        private void 复制链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (listView2.SelectedItems.Count > 0)
            {
                System.Windows.Forms.Clipboard.SetText(listView2.SelectedItems[0].SubItems[2].Text);
            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string uid = GetDouyinVideoId(textBox3.Text.Trim());
                textBox5.Text = uid;
                // 读取文件所有行
                string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt");

                // 过滤不包含关键字的行（区分大小写）
                var filteredLines = lines.Where(line => !line.Contains(textBox5.Text.Trim())).ToArray();

                // 写回原文件（覆盖）
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", filteredLines);

               MessageBox.Show("已成功删除包含关键字的所有行！");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("错误：文件未找到！");
            }
        }


        public void deleteTxt(string link)
        {
            try
            {
             
                string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt");

                // 过滤不包含关键字的行（区分大小写）
                var filteredLines = lines.Where(line => !line.Contains(link.Trim())).ToArray();

                // 写回原文件（覆盖）
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt", filteredLines);

                
            }
            catch (FileNotFoundException)
            {
               
            }
        }
        Thread thread2;
        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox2.Text=="")
            {
                MessageBox.Show("请输入网址");
                return;
            }
            if (DateTime.Now > Convert.ToDateTime("2025-06-26"))
            {
                return;
            }
            易优权益监控拉黑.domain = textBox2.Text.Trim();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt";

            if(!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
      
            if(cookie=="")
            {
                login();
            }
            
            timer2.Start();
            if (thread2 == null || !thread2.IsAlive)
            {
                thread2 = new Thread(run_tongbu);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        Dictionary<string, string>  likecountDic = new Dictionary<string, string>();

        public void run_tongbu()
        {

            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt"));
            //一次性读取完 
            string tongbu_texts = sr.ReadToEnd();
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存


          

            listView2.Items.Clear();
            try
            {

               

                string url = "http://" + 易优权益监控拉黑.domain + "/api/console/OrderManage/Paging";

                string html = 易优权益监控拉黑.PostUrlDefault(url, "{\"page\":1,\"list_rows\":50,\"status\":1,\"id\":null,\"goods_id\":null,\"buy_params\":\"\"}", cookie);


                //MessageBox.Show(html);

                MatchCollection links = Regex.Matches(html, @"name"": ""作品链接([\s\S]*?)""value"": ""([\s\S]*?)""");
                MatchCollection ids = Regex.Matches(html, @"""id"":([\s\S]*?),");

                MatchCollection buy_number = Regex.Matches(html, @"""buy_number"":([\s\S]*?),");
                MatchCollection start_num = Regex.Matches(html, @"""start_num"":([\s\S]*?),");
             
                if(links.Count==0)
                {
                    label1.Text = "无符合订单";
                }

                for (int i = 0; i < links.Count; i++)
                {
                    string uid = ids[2 * i].Groups[1].Value.Trim();
                    string link = links[i].Groups[2].Value.Trim();
                    string buy = buy_number[i].Groups[1].Value.Trim();
                  
                    

                    if (delist_list.Contains(link))
                    {
                        continue;
                    }

                    if (!tongbu_texts.Contains(link))
                    {
                        string startnum = Getlikecount(link).Trim();
                     
                        writeTxt(uid + "#" + link + "#" + buy + "#" + startnum, "tongbu");
                    }

                }
                StreamReader sr2 = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt"));
                //一次性读取完 
                string tongbu_texts2 = sr2.ReadToEnd();
                sr2.Close();  //只关闭流
                sr2.Dispose();   //销毁流内存

                string[] text2 = tongbu_texts2.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text2.Length; i++)
                {

                    try
                    {
                       
                        string[] v = text2[i].Split(new string[] { "#" }, StringSplitOptions.None);
                     
                        string linkk = v[1];
                       
                        string start = v[3];
                      
                        if (delist_list.Contains(linkk))
                        {
                            continue;
                        }



                        string currentnum = Getlikecount(linkk);
                      
                      

                        string[] ss = getstatus(linkk.Trim()).Trim().Split(new string[] { "#" }, StringSplitOptions.None);
                        string aid = ss[0];
                        string buys = ss[1];
                        string status = ss[2];
                        //MessageBox.Show(aid+"  "+buys+"  "+status);
                        tongbu(aid, start, currentnum);

                        ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(aid);
                        lv1.SubItems.Add(linkk);
                        lv1.SubItems.Add(buys);
                        lv1.SubItems.Add(start);
                        lv1.SubItems.Add(currentnum);


                      
                      

                        int cha = Convert.ToInt32(currentnum) - Convert.ToInt32(start);
                      
                        lv1.SubItems.Add(cha.ToString());
                        lv1.SubItems.Add(status);


                        if (cha >= Convert.ToInt32(buys.Trim()))
                        {

                            易优权益监控拉黑.Speak(v[0]+ "：点赞任务完成");
                        }


                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        label1.Text=ex.Message;
                        //MessageBox.Show(ex.ToString());
                        continue;
                    }
                }



              
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }


        }



    


        public void tongbu(string id, string start_num, string current_num)
        {
           
            string url = "http://" + 易优权益监控拉黑.domain + "/api/supplier/Order/ScheduleHandle";
            string postdata = "{\"id\":" + id + ",\"start_num\":" + start_num + ",\"current_num\":" + current_num + "}";

            string html = 易优权益监控拉黑.PostUrlDefault(url, postdata, cookie);
           
            if (html.Contains("商品不存在"))
            {
                url = "http://" + 易优权益监控拉黑.domain + "/api/console/OrderManage/ScheduleHandle";
                html = 易优权益监控拉黑.PostUrlDefault(url, postdata, cookie);
            }
           
        }

        public string getstatus(string link)
        {

            string url = "http://" + 易优权益监控拉黑.domain + "/api/console/OrderManage/Paging";
            string postdata = "{\"type\":0,\"page\":1,\"list_rows\":10,\"status\":0,\"goods_name\":\"\",\"docking_status\":0,\"id\":null,\"goods_id\":null,\"customer_id\":null,\"buy_params\":\"" + link + "\",\"docking_site_id\":0,\"supplier_id\":null}";

            string html = 易优权益监控拉黑.PostUrlDefault(url, postdata, cookie);

            string status=  Regex.Match(html, @"""status"":([\s\S]*?),").Groups[1].Value.Trim().Replace("1","已付款").Replace("2", "待处理").Replace("3", "处理中").Replace("4", "补单中").Replace("5", "退单中").Replace("6", "已完成").Replace("7", "已退单").Replace("8", "已退款");
            string buy = Regex.Match(html, @"""buy_number"":([\s\S]*?),").Groups[1].Value.Trim();
            string id = Regex.Match(html, @"""id"":([\s\S]*?),").Groups[1].Value.Trim();


            return   id+"#"+buy+"#"+status;
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (listView2.SelectedItems.Count > 0)
            {
                System.Diagnostics.Process.Start(listView2.SelectedItems[0].SubItems[2].Text);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
          

            if (thread2== null || !thread2.IsAlive)
            {
                thread2 = new Thread(run_tongbu);
                thread2.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        List<string> delist_list = new List<string>();
        private void 删除链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                delist_list.Add(listView2.SelectedItems[0].SubItems[2].Text);
                string link = listView2.SelectedItems[0].SubItems[2].Text;

                try
                {
                
                    // 读取文件所有行
                    string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt");

                    // 过滤不包含关键字的行（区分大小写）
                    var filteredLines = lines.Where(line => !line.Contains(link.Trim())).ToArray();

                    // 写回原文件（覆盖）
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt", filteredLines);

                    
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("错误：文件未找到！");
                }
                MessageBox.Show("删除成功");

            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }

        }

        private void 修改同步ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                string uid = listView2.SelectedItems[0].SubItems[1].Text;
                string link = listView2.SelectedItems[0].SubItems[2].Text;
                string old_start = listView2.SelectedItems[0].SubItems[4].Text;
                string currentnum = listView2.SelectedItems[0].SubItems[5].Text;

                string startnum = Interaction.InputBox("提示信息", "请输入初始值", "0", -1, -1);


                tongbu(uid, startnum, currentnum);

                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\tongbu.txt";
                List<string> lines = new List<string>(File.ReadAllLines(filePath));
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains(link))
                    {
                        lines[i] = lines[i].Replace("#"+ old_start, "#" + startnum);
                    }
                }
                File.WriteAllLines(filePath, lines);



                MessageBox.Show("修改成功");
            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
           
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private void 复制链接ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                System.Windows.Forms.Clipboard.SetText(listView1.SelectedItems[0].SubItems[2].Text);
            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                if(listView2.Items[i].SubItems[2].Text.Trim()==textBox7.Text.Trim())
                {
                    this.listView2.Items[i].EnsureVisible();
                    this.listView2.Items[i].Selected = true;
                    this.listView2.Items[i].SubItems[2].BackColor= Color.Blue;    
                }
            }
        }


        Thread thread3;
        private void button10_Click(object sender, EventArgs e)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\delest.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            timer3.Start();
            if (thread3 == null || !thread3.IsAlive)
            {
                thread3 = new Thread(run_data);
                thread3.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

            timer3.Stop();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            添加账号 add = new 添加账号();
            add.Show();
        }

        List<string> jingbao_list=new List<string>();   
        public void run_data()
        {
         
            string delesttexts = readTxt("delest"); ;

            listView3.Items.Clear();
            try
            {
               
                string texts = readTxt("cookie");


                string[] ck_text = texts.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int a = 0; a < ck_text.Length; a++)
                {


                    string[] aaa = ck_text[a].Split(new string[] { "###" }, StringSplitOptions.None);
                    string beizhu = aaa[0].Trim();
                    string ad_id = aaa[1].Trim();
                    string cookie = aaa[2].Trim();

                    string url = "https://localads.chengzijianzhan.cn/api/lamp/pc/v2/statistics/data/statQuery";
                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    //string ad_id = "1827112997806155";

                    string FrameId = "7303877448767242291";
                    string Moduleid = "7301514013974741030";
                    string DataSetKey = "100073";
                   // string postdata = "{\"PageParams\":{\"Offset\":\"0\",\"Limit\":\"100\"},\"StartTime\":\"" + today + " 00:00:00\",\"EndTime\":\"" + today + " 23:59:59\",\"Filters\":{\"ConditionRelationshipType\":1,\"Conditions\":[{\"Field\":\"advertiser_id\",\"Values\":[\"" + ad_id + "\"],\"Operator\":7},{\"Field\":\"derivate_is_order\",\"Values\":[\"1\"],\"Operator\":7},{\"Field\":\"adlab_mode\",\"Values\":[\"1\"],\"Operator\":8}]},\"Dimensions\":[\"ad_relation_order_id\",\"stat_time_day\"],\"OrderBy\":[{\"Type\":2,\"Field\":\"stat_time_day\"},{\"Type\":2,\"Field\":\"stat_cost\"}],\"Metrics\":[\"stat_cost\",\"show_cnt\",\"click_cnt\",\"video_oto_pay_order_stat_amount\",\"dy_like\",\"dy_comment\",\"dy_share\",\"dy_collect\",\"conversion_cost\"],\"FrameId\":\"" + FrameId + "\",\"ModuleId\":\"" + Moduleid + "\",\"DataSetKey\":\"" + DataSetKey + "\"}";

                    string postdata = "{\"PageParams\":{\"Limit\":\"100\",\"Offset\":\"0\"},\"StartTime\":\""+today+" 00:00:00\",\"EndTime\":\""+today+" 23:59:59\",\"Filters\":{\"ConditionRelationshipType\":1,\"Conditions\":[{\"Field\":\"advertiser_id\",\"Values\":[\""+ad_id+ "\"],\"Operator\":7},{\"Field\":\"derivate_is_order\",\"Values\":[\"1\"],\"Operator\":7},{\"Field\":\"adlab_mode\",\"Values\":[\"1\"],\"Operator\":8}]},\"Dimensions\":[\"creative_id\",\"stat_time_day\"],\"OrderBy\":[{\"Type\":2,\"Field\":\"stat_time_day\"},{\"Type\":2,\"Field\":\"stat_cost\"}],\"Metrics\":[\"stat_cost\",\"dy_like\",\"conversion_cost\",\"show_cnt\",\"click_cnt\",\"video_oto_pay_order_stat_amount\",\"dy_comment\",\"dy_share\",\"dy_collect\"],\"FrameId\":\"" + FrameId + "\",\"ModuleId\":\"" + Moduleid + "\",\"DataSetKey\":\"" + DataSetKey + "\"}";
                   
                    string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json", "");

                    MatchCollection creative_name = Regex.Matches(html, @"creative_name"":{""Value"":0,""ValueStr"":""([\s\S]*?),");



                    MatchCollection ad_ids = Regex.Matches(html, @"ad_id"":{""Value"":([\s\S]*?),");
                    MatchCollection stat_cost = Regex.Matches(html, @"stat_cost"":{""Value"":([\s\S]*?),");
                    MatchCollection dy_like = Regex.Matches(html, @"dy_like"":{""Value"":([\s\S]*?),");
                    MatchCollection conversion_cost = Regex.Matches(html, @"conversion_cost"":{""Value"":([\s\S]*?),");
                    MatchCollection show_cnt = Regex.Matches(html, @"show_cnt"":{""Value"":([\s\S]*?),");
                
                    //MatchCollection click_cnt = Regex.Matches(html, @"click_cnt"":{""Value"":([\s\S]*?),");
                    //MatchCollection pay = Regex.Matches(html, @"video_oto_pay_order_stat_amount"":{""Value"":([\s\S]*?),");
                 



                    for (int i = 0; i < creative_name.Count; i++)
                    {

                        try
                        {

                            string adid = ad_ids[i].Groups[1].Value.Replace("\"", "");

                            if (delesttexts.Contains(adid))
                            {
                                continue;
                            }
                            if (i > 0 && stat_cost[i].Groups[1].Value == "0")
                            {
                                continue;
                            }
                            ListViewItem lv1 = listView3.Items.Add((listView3.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(beizhu);
                            lv1.SubItems.Add(adid);
                            lv1.SubItems.Add(creative_name[i].Groups[1].Value);
                            
                            lv1.SubItems.Add(stat_cost[i].Groups[1].Value);
                            lv1.SubItems.Add(dy_like[i].Groups[1].Value);

                            double jisuan_chengben=Convert.ToDouble(stat_cost[i].Groups[1].Value)/Convert.ToDouble(dy_like[i].Groups[1].Value);

                            lv1.SubItems.Add(jisuan_chengben.ToString("F3"));
                            lv1.SubItems.Add(conversion_cost[i].Groups[1].Value);
                            lv1.SubItems.Add(show_cnt[i].Groups[1].Value);



                            if (a % 2 == 1)
                            {
                                lv1.BackColor = Color.LightBlue;
                            }

                            if (Convert.ToDouble(stat_cost[i].Groups[1].Value) > Convert.ToDouble(textBox9.Text.Trim()) && jisuan_chengben > Convert.ToDouble(textBox8.Text.Trim()))
                            {
                                if (!jingbao_list.Contains(beizhu))
                                {
                                    lv1.BackColor = Color.Red; 
                                    易优权益监控拉黑.Speak("转化成本超额");
                                }

                            }
                        }
                        catch (Exception ex)
                        {

                            //MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (thread3 == null || !thread3.IsAlive)
            {
                thread3 = new Thread(run_data);
                thread3.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 解除警报ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {

                string beizhu = listView3.SelectedItems[0].SubItems[1].Text;

                jingbao_list.Add(beizhu);

            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
        }

        private void 删除此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {

                for (int i = 0; i < listView3.SelectedItems.Count; i++)
                {
                    string adid = listView3.SelectedItems[i].SubItems[2].Text;

                    try
                    {

                        writeTxt(adid, "delest");


                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("错误：文件未找到！");
                    }
                }
               
                MessageBox.Show("删除成功");

            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
           
        }
    }
}
