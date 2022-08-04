using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

namespace 天眼查供应商抓取
{
    public partial class 天眼查供应商抓取 : Form
    {
        public 天眼查供应商抓取()
        {
            InitializeComponent();
        }
        bool zanting = true;
        bool status = false;
        Thread thread;
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }
        string token = "";
        string domail = "www.acaiji.com/shangxueba2";
        public void gettoken()
        {
            string ahtml = method.GetUrl("http://" + domail + "/shangxueba.php?method=getcookie", "utf-8");

            token = ahtml.Trim().Replace("\r", "").Replace("\n", "");


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
        public string PostUrlDefault(string url, string postData)
        {
            try
            {
              
                string TOKEN = "X-AUTH-TOKEN:" + token;
                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //   request.Proxy = null;//防止代理抓包
                if (token.Substring(0, 1) != "e")
                {
                    token = token.Remove(0, 1);
                }


                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("version:TYC-XCX-WX");
                headers.Add("X-AUTH-TOKEN: " + token);
                //  headers.Add("X-AUTH-TOKEN:eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxNzMzMDc5NjE4NyIsImlhdCI6MTYzOTk5MDgwOSwiZXhwIjoxNjQyNTgyODA5fQ.b_79_YOoEI-gATwz6gYXVWPmZw8fuwtG6DavohbodPRd-Lxoua6CcqeWPrmMsAM-YHZEVj2raqRYdfnktOwfdg");
                //添加头部
                //request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.Proxy = null;//禁止抓包
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/WIFI Language/zh_CN";
                //request.Headers.Add("Cookie", COOKIE);

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

                return ("企业大数据：异常");
            }


        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {
            if (token.Substring(0, 1) != "e")
            {
                token = token.Remove(0, 1);
            }
            string html = "";

          
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.Authorization, "0###oo34J0YSWR0ucFrTbNJwK7MpAWiE###1657684985313###2ed6a2ec3ab12c1623ccd098ee5711fd");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36 MicroMessenger/7.0.9.501 NetType/WIFI MiniProgramEnv/Windows WindowsWechat";
                request.ContentType = "application/json";
                request.Headers.Add("version", @"TYC-XCX-WX");
                request.Referer = "https://servicewechat.com/wx9f2867fc22873452/77/page-frame.html";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");

           
                request.ServicePoint.Expect100Continue = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
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



     public string gettel(string keyword)
        {

            int i = 1;
            string url = "https://capi.tianyancha.com/cloud-tempest/app/searchCompany";
            string postdata = "{\"sortType\":0,\"pageSize\":20,\"pageNum\":" + i + ",\"word\":\"" + keyword + "\",\"allowModifyQuery\":1}";

            string html = PostUrlDefault(url, postdata);

       

            string legalPerson = Regex.Match(html, @"""legalPersonName"":""([\s\S]*?)""").Groups[1].Value;
            string tel = Regex.Match(html, @"""phoneNum"":""([\s\S]*?)""").Groups[1].Value;
            return legalPerson + " " + tel;
        }

        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);

        }
        public void run()
        {
           


            label2.Text = "开始运行...";
            gettoken();
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == "")
                    continue;
                string url = "https://capi.tianyancha.com/cloud-tempest/app/searchCompany";
                //string body = @"{""sortType"":0,""pageSize"":20,""pageNum"":1,""word"":""福建省长乐第一中学"",""allowModifyQuery"":1}";
                string body = "{\"sortType\":0,\"pageSize\":10,\"pageNum\":1,\"word\":\"" + text[i] + "\",\"allowModifyQuery\":1}";

               
                string html = PostUrlDefault(url, body);
               // textBox2.Text = html;


                html= Regex.Match(html, @"companyList([\s\S]*?)searchContent").Groups[1].Value;


                
                MatchCollection ids= Regex.Matches(html, @"{""id"":([\s\S]*?),");
                MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");

                string id = ids[0].Groups[1].Value;


                for (int x= 0; x< ids.Count; x++)
                {
                    if(Regex.Replace(names[x].Groups[1].Value, "<[^>]+>", "")==text[i])
                    {
                        id = ids[x].Groups[1].Value;
                        break;
                    }
                }



                //string id = Regex.Match(html, @"{""id"":([\s\S]*?),").Groups[1].Value;
                string city = Regex.Match(html, @"""city"":""([\s\S]*?)""").Groups[1].Value;

                string legalPerson = Regex.Match(html, @"""legalPersonName"":""([\s\S]*?)""").Groups[1].Value;
                string tel = Regex.Match(html, @"""phoneNum"":""([\s\S]*?)""").Groups[1].Value;
                label2.Text = "开始查询..."+text[i];
                List<string> list = new List<string>();
              
                for (int page = 1; page < 99; page++)
                {
                    try
                    {
                        string aurl = "https://capi.tianyancha.com/cloud-business-state/supply/list?gid=" + id + "&pageNum=" + page + "&pageSize=100&year=-100";
                        string ahtml = GetUrl(aurl, "utf-8");
                       
                        MatchCollection supplier_name = Regex.Matches(ahtml, @"""supplier_name"":""([\s\S]*?)""");
                        MatchCollection source_name = Regex.Matches(ahtml, @"""source_name"":""([\s\S]*?)""");
                        MatchCollection amt = Regex.Matches(ahtml, @"""amt"":([\s\S]*?),");
                        MatchCollection announcement_date = Regex.Matches(ahtml, @"""announcement_date"":([\s\S]*?),");

                        if(source_name.Count==0)
                        {
                            break;
                        }

                       
                        for (int a = 0; a < supplier_name.Count; a++)
                        {
                            if (!list.Contains(supplier_name[a].Groups[1].Value))
                            {
                                list.Add(supplier_name[a].Groups[1].Value);
                                label2.Text = "开始查询...公司：" + supplier_name[a].Groups[1].Value;
                                string v = gettel(supplier_name[a].Groups[1].Value);

                                ListViewItem lv1 = listView1.Items.Add(city); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add(supplier_name[a].Groups[1].Value);

                                lv1.SubItems.Add(v);

                                lv1.SubItems.Add("-");
                                lv1.SubItems.Add("-");
                                lv1.SubItems.Add(amt[a].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(ConvertStringToDateTime(announcement_date[a].Groups[1].Value).ToString("yyyy-MM-dd"));
                                lv1.SubItems.Add(source_name[a].Groups[1].Value);



                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;

                                Thread.Sleep(1000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                       MessageBox.Show(ex.ToString());
                    }
                }

             
                Thread.Sleep(1000);
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }



        #endregion

        private void 天眼查供应商抓取_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"gSrr"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入文本");
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
            label2.Text = "已停止...";
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
          
            listView1.Items.Clear();
        }

        private void 天眼查供应商抓取_FormClosing(object sender, FormClosingEventArgs e)
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
