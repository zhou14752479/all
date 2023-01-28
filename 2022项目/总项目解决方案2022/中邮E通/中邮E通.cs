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

namespace 中邮E通
{
    public partial class 中邮E通 : Form
    {
        public 中邮E通()
        {
            InitializeComponent();
        }
        #region POST默认请求
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
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
              
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

               // request.Referer = url;
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
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }
        string username = "528237012";
        string cookie = "JSESSIONID=fq0O2Nz1wxlOm5gw8nCdMsYolZ3vnoN4ckTXMB4X_i36rQillXsa!-919988174";
        #region 入库
        public void run_ru()
        {


            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (text[i] != "")
                {
                    label3.Text = "正在入库：" + text[i];

                    Thread.Sleep(500);
                    string url = "http://211.156.200.95:8081/ztyj/kjjs.do";
                    string html = method.PostUrlDefault(url, "method=yjCheck&ydh=" + text[i] + "&wlgs=%E7%89%A9%E6%B5%81%E5%85%AC%E5%8F%B8&dxtzfs=2&randnum=Mon+Oct+24+2022+18%3A56%3A48+GMT%2B0800+(%E4%B8%AD%E5%9B%BD%E6%A0%87%E5%87%86%E6%97%B6%E9%97%B4)&V_HHBH=2&v_cndf=0", cookie);
                    
                    string V_WLJP = Regex.Match(html, @"""V_WLJP"":""([\s\S]*?)""").Groups[1].Value;
                   
                    string V_YJXT = Regex.Match(html, @"""V_YJXT"":""([\s\S]*?)""").Groups[1].Value;

                    if (V_WLJP != "")
                    {
                        string time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                        string str = "{\"loginJgbh\":" + username + ",\"V_EXPRESSCOMPANY\":\"" + V_WLJP + "\",\"MAILNUM\":\"" + text[i] + "\",\"sjh\":\"" + textBox2.Text + "\",\"xm\":\"-\",\"XYD_YJLY\":\"00\",\"V_TDJGBH\":\"\",\"dxtzfs\":\"2\",\"V_HHBH\":\"3\",\"wlbz\":\"0\",\"ORDER_SOURCE\":\"0\",\"randnum\":\"" + time + ".626Z\"}";
                        string parasStr = method.Base64Encode(Encoding.GetEncoding("utf-8"),str);
                    
                        
                        string rupostdata = "parasStr=" + System.Web.HttpUtility.UrlEncode(parasStr) + "&loginJgbh="+username+"&V_EXPRESSCOMPANY=" + System.Web.HttpUtility.UrlEncode(V_WLJP) + "&xm=-&wlbz=0";
                        string ruhtml = PostUrlDefault("http://211.156.200.95:8081/ztyj/kjjs.do?method=newOtherPost", rupostdata, cookie);
                      
                        if(ruhtml.Contains("刷单号码"))
                        {
                            MessageBox.Show("请修改手机号码，然后点击确定");
                            i = i - 1;
                            continue;
                        }

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text[i]);
                        lv1.SubItems.Add(ruhtml);
                    }
                    else
                    {
                       
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text[i]);
                        lv1.SubItems.Add(V_YJXT);

                    }

                  



                }
            }

        }

        #endregion



        #region 出库
        public void run_chu()
        {
            listView1.Items.Clear();    
            for (int page = 0; page < 999; page++)
            {
                string url = "http://211.156.200.95:8081/ztyj/khqj.do?method=queryOutpost&ydh=&sjh=&cxsj=week&csrftoken=0.1453876897645866";

                string html = method.PostUrlDefault(url, "page=1&rows=50",cookie);
                MatchCollection LSH = Regex.Matches(html, @"""LSH"":([\s\S]*?),");
                MatchCollection YJHM = Regex.Matches(html, @"""YJHM"":""([\s\S]*?)""");

                if (LSH.Count == 0)
                {
                    label3.Text = "出库结束";
                    return;
                }
                for (int i = 0; i < LSH.Count; i++)
                {


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    label3.Text = "正在入库：" + YJHM[i].Groups[1].Value;


                    Thread.Sleep(500);

                    string aurl = "http://211.156.200.95:8081/ztyj/khqj.do";
                    string ahtml = method.PostUrlDefault(aurl, "method=fastOutGoing&lx=1&lshlist="+LSH[i].Groups[1].Value+"&table=1", cookie);



                   
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(YJHM[i].Groups[1].Value);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(ahtml);
                }
            }
                  



                
            

        }

        #endregion
        bool zanting = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            cookie = 登录.cookie;
            
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"L7bf7"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion

            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入单号");
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_ru);
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cookie = 登录.cookie;
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"L7bf7"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion

         
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_chu);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();
        }

        private void 中邮E通_Load(object sender, EventArgs e)
        {

        }
    }
}
