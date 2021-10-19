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

namespace 优学优考网题库抓取
{
    public partial class 优学优考网题库抓取 : Form
    {
        public 优学优考网题库抓取()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {
            string html = "";
          
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", zhuaqucookie);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
              
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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

                return ex.ToString();
            }


        }

        #endregion

        public void run()
        {

            try
            {
                for (int i = Convert.ToInt32(textBox4.Text); i <=800000; i++)
                {
                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(i);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();
                    textBox4.Text = i.ToString();

                    string url = "http://www.yxykw.com/type/type.jsp?id="+i;
                    string html = GetUrl(url,"utf-8");
                    MatchCollection uids = Regex.Matches(html, @"window.open\('([\s\S]*?)'");
                    foreach (Match uid in uids)
                    {
                        if (astatus == false)
                            return;
                        string aurl = "http://www.yxykw.com"+ uid.Groups[1].Value;
                        string ahtml = GetUrl(aurl,"utf-8");
                        string title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                        string qustion = Regex.Match(ahtml, @"<div class=""s_mess2_m"">([\s\S]*?)</div>").Groups[1].Value;
                        string anwser = Regex.Match(ahtml, @"<div class=""replyCon"">([\s\S]*?)</div>").Groups[1].Value;
                      
                        if (anwser=="")
                        {
                            MessageBox.Show("答案为空，请检查账号是否到期");
                            continue;
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        string status=   fabu(System.Web.HttpUtility.UrlEncode(title), System.Web.HttpUtility.UrlEncode(qustion), System.Web.HttpUtility.UrlEncode(anwser));
                        if(status=="成功")
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(title);
                            listViewItem.SubItems.Add(status);
                        }
                        else
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(title);
                            listViewItem.SubItems.Add(status);
                            return;
                        }
                    }
                    
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        Thread thread;
        bool zanting = true;
        string zhuaqucookie = "";
        public string fabucookie = "";
        bool astatus = true;

        public string fabu(string title,string question,string anwser)
        {
            try
            {
                string url = "http://tiku.ucms.club/login.php?m=admin&c=Article&a=add&lang=cn";
                string postdata = "title="+title+"&typeid=86&jumplinks=&litpic_local=&litpic_remote=&users_price=&part_free=0&size=1&addonFieldExt[content]="+question+"&addonFieldExt[daan]="+anwser+"&tags=&seo_title=&seo_keywords=&seo_description=&author=小编&click=987&arcrank=0&add_time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"&tempview=view_article.htm&type_tempview=view_article.htm&htmlfilename=&free_content=&gourl=";
               string html= PostUrlDefault(url,postdata,fabucookie);
                if(html.Contains("成功发布文档"))
                {
                    return "成功";
                }
                else
                {
                    MessageBox.Show("未登录后台");
                    return "失败";
                }

            }
            catch (Exception ex)
            {
               
                MessageBox.Show(ex.ToString());
                return "失败";
            }


        }

        string  cookie = "PHPSESSID=al7re7smf6jqm8fo8iuoub0gfb; admin_lang=cn; home_lang=cn; ENV_GOBACK_URL=%2Flogin.php%3Fm%3Dadmin%26c%3DArticle%26a%3Dindex%26typeid%3D86%26lang%3Dcn; ENV_LIST_URL=%2Flogin.php%3Fm%3Dadmin%26c%3DArticle%26a%3Dindex%26lang%3Dcn; workspaceParam=welcome%7CIndex; users_id=1";
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



        #region 优学网登录
       

        private static void WriteMultipartBodyToRequest(HttpWebRequest request, string body)
        {
            string[] multiparts = Regex.Split(body, @"<!>");
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (string part in multiparts)
                {
                    if (File.Exists(part))
                    {
                        bytes = File.ReadAllBytes(part);
                    }
                    else
                    {
                        bytes = System.Text.Encoding.UTF8.GetBytes(part.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
                    }

                    ms.Write(bytes, 0, bytes.Length);
                }

                request.ContentLength = ms.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    ms.WriteTo(stream);
                }
            }
        }

        private string Request_www_yxykw_com()
        {
           HttpWebResponse response = null;
            string html = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.yxykw.com/login");

                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
                request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundaryC3DJOm7qt35A9ReI";
                request.Accept = "*/*";
                request.Headers.Add("Origin", @"http://www.yxykw.com");
                request.Referer = "http://www.yxykw.com/939cb97751a92ed935963c541e835c69.html";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.Cookie, @"UM_distinctid=17c1613cfdc495-065b735bd36bb9-4343363-1fa400-17c1613cfdd25a; Hm_lvt_ba0f6e2f9570215a05a3f233a2678280=1632458535,1634522675,1634547773,1634609800; CNZZDATA1278581140=1942934328-1632450962-%7C1634606449; Hm_lpvt_ba0f6e2f9570215a05a3f233a2678280=1634612416");

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = string.Format(@"------WebKitFormBoundaryC3DJOm7qt35A9ReI
Content-Disposition: form-data; name=""yzm""

{0}
------WebKitFormBoundaryC3DJOm7qt35A9ReI--
",textBox5.Text.Trim());
                WriteMultipartBodyToRequest(request, body);

                response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                html = reader.ReadToEnd();
                zhuaqucookie =  response.GetResponseHeader("Set-Cookie"); 
                reader.Close();
                return html;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return html;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return  html;
            }
            return html;
           
        }

        #endregion
        private void 优学优考网题库抓取_Load(object sender, EventArgs e)
        {
              StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "//data.txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
           textBox4.Text = sr.ReadToEnd().Trim();
            sr.Close();
            sr.Dispose();
            pictureBox1.Image = UrlToBitmap("http://tiku.ucms.club/login.php?m=admin&c=Admin&a=vertify&lang=cn");
        }


        
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"uoQUpu"))
            {

                return;
            }



            #endregion

            if(zhuaqucookie=="" || fabucookie=="")
            {
                MessageBox.Show("请先登录");
                return;
            }
            astatus = true;
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = UrlToBitmap("http://tiku.ucms.club/login.php?m=admin&c=Admin&a=vertify&lang=cn");
        }

        private void button5_Click(object sender, EventArgs e)
        {
           string html= PostUrlDefault("http://tiku.ucms.club/login.php?m=admin&c=Admin&a=login&_ajax=1&lang=cn&t=0.9288685268426105","user_name=" +textBox1.Text.Trim()+ "&password=" + textBox2.Text.Trim() + "&vertify="+textBox3.Text.Trim(),cookie);
            string msg= Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            fabucookie = cookie;
            MessageBox.Show(msg);
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
          string html=  Request_www_yxykw_com();
            MessageBox.Show(html);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            astatus = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 优学优考网题库抓取_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
