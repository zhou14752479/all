using CsharpHttpHelper;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Text.RegularExpressions;

namespace 邮箱地址确认
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

		private string GetHttp20220511150408(string postdata)
		{
			HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "https://rakko.tools/tools/131/emailValidatorController.php",
                Method = "POST",
                Host = "rakko.tools",
                Accept = "application/json, text/javascript, */*; q=0.01",
                ContentType = "application/x-www-form-urlencoded; charset=UTF-8",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36",
                Referer = "https://rakko.tools/tools/131/",
                Cookie = cookie,
                PostEncoding = Encoding.UTF8,
                Postdata = postdata
			};
			item.Header.Add("sec-ch-ua", "\"Not A; Brand\";v=\"99\", \"Chromium\";v=\"100\", \"Google Chrome\";v=\"100\"");
			item.Header.Add("X-Requested-With", "XMLHttpRequest");
			item.Header.Add("sec-ch-ua-mobile", "?0");
			item.Header.Add("sec-ch-ua-platform", "\"Windows\"");
			item.Header.Add("Sec-Fetch-Site", "same-origin");
			item.Header.Add("Sec-Fetch-Mode", "cors");
			item.Header.Add("Sec-Fetch-Dest", "empty");
			item.Header.Add("Accept-Encoding", "gzip, deflate, br");
			item.Header.Add("Accept-Language", "zh-CN,zh;q=0.9");
			HttpResult result = http.GetHtml(item);
			string html = result.Html;
			return html;
		}


		private void Form1_Load(object sender, EventArgs e)
        {
           
			#region 通用检测


			if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"VA7Mx"))
			{
				
				System.Diagnostics.Process.GetCurrentProcess().Kill();

				return;
			}

			#endregion
		}

        string cookie = "";
        string tokenId = "";
        string token = "";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;
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


        #region GET请求获取Set-cookie
        public void getSetCookie(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = false;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

            cookie = response.GetResponseHeader("Set-Cookie"); ;
         
            string charset = "utf-8";
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

            tokenId= Regex.Match(html, @"tokenId = '([\s\S]*?)'").Groups[1].Value;
            token = Regex.Match(html, @"token = '([\s\S]*?)'").Groups[1].Value;
          
        }
        #endregion

        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview, int i)
        {
            if (listview.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in listview.Items)
                {
                    if (item.SubItems[i].Text.Trim() != "" && item.SubItems[2].Text.Trim() == "1")
                    {

                        list.Add(item.SubItems[i].Text);
                    }


                }
                Thread thexp = new Thread(() => export(list)) { IsBackground = true };
                thexp.Start();
            }
        }


        private static void export(List<string> list)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";

            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);
        }



        #endregion

        public void run()
        {
            getSetCookie("https://rakko.tools/tools/131/");
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                for (int i = 0; i < text.Length; i=i+9)
                {
                    label1.Text = "正在查询......"+ text[i];
                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                   StringBuilder sb = new StringBuilder();
                    for (int j = 0; j <10; j++)
                    {
                        try
                        {
                            sb.Append(text[i + j].Replace("@", "%40") + "%0A");
                        }
                        catch (Exception)
                        {

                           
                        }
                    }
                    string postdata = "token_id="+tokenId+"&token="+token+"&emails=" + sb.ToString();
                   

                    string html = GetHttp20220511150408(postdata);
                  
                    html= Regex.Match(html, @"result"":{([\s\S]*?)}").Groups[1].Value;

                   // MessageBox.Show(html);
                    string[] mails = html.Split(new string[] { "," }, StringSplitOptions.None);

                    for (int a= 0; a <mails.Length; a++)
                    {
                        string[] values = mails[a].Split(new string[] { ":" }, StringSplitOptions.None);
                      if(values.Length >1)
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(values[0].Replace("\"",""));
                            lv1.SubItems.Add(values[1]);
                            if(values[1].Trim()=="1")
                            {
                                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\live.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(values[0].Replace("\"", ""));
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();
                            }
                            if (values[1].Trim() == "0")
                            {
                                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\die.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(values[0].Replace("\"", ""));
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();
                            }
                        }
                    }
                  

                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }


                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



        }
        private void button4_Click(object sender, EventArgs e)
        {
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{

				textBox1.Text = openFileDialog1.FileName;

			}
		}

        bool status = true;
        bool zanting = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
		    if(textBox1.Text=="")
            {
                MessageBox.Show("请导入邮箱");
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

        private void button3_Click(object sender, EventArgs e)
        {
			status = false;
		}

        private void button5_Click(object sender, EventArgs e)
        {
			listView1.Items.Clear();
		}

        private void button2_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView1,1);
        }

        private void button6_Click(object sender, EventArgs e)
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
    }
}
