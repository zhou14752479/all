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

namespace 主程序202305
{
    public partial class 发票51 : Form
    {
        public 发票51()
        {
            InitializeComponent();
        }


        Thread thread;

        bool zanting = true;
        bool status = true;
        string cookie = "Hm_lvt_e6205dc065614ddaf8f52688bf0d362c=1684632349; Hm_lpvt_e6205dc065614ddaf8f52688bf0d362c=1684632349; protalsid=de79e5bf-a21e-4b19-b2ea-5e31ab4712b4";
        public static string PostUrlDefault(string url, string postData, string COOKIE,string type)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("Authtoken:f801017874ae403c888fd8983b462a6d");
                //headers.Add("Routename:trackingExpress");
                request.ContentType =type;
                //request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = "";

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
                result = ex.ToString();
            }
            return result;
        }
        private void button3_Click(object sender, EventArgs e)
        {

            #region 通用检测


            string html =method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"BsEFq"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion

            status = true;
            Random random = new Random();
            int suiji=random.Next(100, 999);   
            cookie = method.getSetCookie("https://www.51fapiao.cn/serverapi/webServer/webapi/gencode?tmp=" + random);

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public string getocr()
        {
            Random random = new Random();
            int suiji = random.Next(100, 999);
            string imgurl = "https://www.51fapiao.cn/serverapi/webServer/webapi/gencode?tmp="+random;
          string html =  StupidOcr.OCR(imgurl,cookie);
           
            return html;   
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;

        List<string> lists = new List<string>();

        #region 主程序
        public void run()
        {

            string spath = path + "\\发票\\"+DateTime.Now.ToString("yyyyMMdd")+"\\";
            if (!Directory.Exists(spath))
            {
                Directory.CreateDirectory(spath); //创建文件夹
            }

            int fp1 = Convert.ToInt32(textBox2.Text);
            int fp2= Convert.ToInt32(textBox4.Text);

            double je1=Convert.ToDouble(textBox3.Text);
            double je2= Convert.ToDouble(textBox5.Text);



            for (int i = fp1; i <=fp2; i++)
            {


                for (double j = je1; j <= je2; j=j+0.01)
                {

                    try
                    {
                        //if(lists.Contains(i.ToString("D8") + j))
                        //{
                        //    continue;
                        //}

                        //lists.Add(i.ToString("D8") + j);
                      
                      
                        string code = getocr();
                       
                        string url = "https://www.51fapiao.cn/serverapi/webServer/webapi/queryInv";
                        string postdata = "fpdm="+textBox1.Text+"&fphm=" + i.ToString("D8") + "&kprq=" + textBox6.Text.Trim() + "&kphjje=" + j.ToString("f2") + "&yzm=" + code + "&uuid=&flag=1&skip=0&jeflag=1";

                        string html = PostUrlDefault(url, postdata, cookie, "application/x-www-form-urlencoded");
                        html = System.Web.HttpUtility.UrlDecode(html);
                        
                        if (html.Contains("验证码错误"))
                        {
                            j = j - 0.01;
                            continue;
                        }
                        //MessageBox.Show(html);


                       

                        string xzMsg = Regex.Match(html, @"""xzMsg"":""([\s\S]*?)""").Groups[1].Value;
                        string cxKey = Regex.Match(html, @"""cxKey"":""([\s\S]*?)""").Groups[1].Value;

                        if (cxKey != "")
                        {
                            //MessageBox.Show(cxKey);
                            xzMsg = "成功";
                            string fileurl = "https://www.51fapiao.cn/serverapi/webServer/webapi/getPdfImg/" + cxKey + "/0.png";
                            downloadFile(fileurl, spath, i.ToString("D8") + ".jpg", cookie);

                        }


                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(textBox1.Text);
                        lv1.SubItems.Add(i.ToString("D8"));
                        lv1.SubItems.Add(j.ToString("f2"));
                        lv1.SubItems.Add(textBox6.Text);
                        lv1.SubItems.Add(xzMsg);


                        if (html.Contains("日期校验失败"))
                        {
                           run_jiaoyandate(i.ToString("D8"), j.ToString("f2"));
                        }


                        if (listView1.Items.Count > 2)
                        {
                            this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                        }
                        if (html.Contains("太快了"))
                            break;

                        if (cxKey != "")
                        {
                            //成功后跳过金额筛选，查下一个发票号码
                            break;
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }
                    
                }

            }
            MessageBox.Show("查询结束");
        }

        #endregion

        #region 下载文件带cookie
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "");
                bool flag = !Directory.Exists(subPath);
                if (flag)
                {
                    Directory.CreateDirectory(subPath);
                }
                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {
                ex.ToString();
            }
        }
        #endregion




        public void run_jiaoyandate(string haoma,string jine)
        {
            string spath = path + "\\发票\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            DateTime dt1=Convert.ToDateTime(textBox6.Text.Trim());
            DateTime dt2 = Convert.ToDateTime(textBox7.Text.Trim());

            for (DateTime dt= dt1; dt < dt2; dt=dt.AddDays(1))
            {
                try
                {
                 


                    string code = getocr();

                    string url = "https://www.51fapiao.cn/serverapi/webServer/webapi/queryInv";
                    string postdata = "fpdm=" + textBox1.Text + "&fphm=" +haoma + "&kprq=" + dt.ToString("yyyy-MM-dd") + "&kphjje=" + jine+ "&yzm=" + code + "&uuid=&flag=1&skip=0&jeflag=1";

                    string html = PostUrlDefault(url, postdata, cookie, "application/x-www-form-urlencoded");
                    html = System.Web.HttpUtility.UrlDecode(html);

                    if (html.Contains("验证码错误"))
                    {
                       dt= dt.AddDays(-1);
                        continue;
                    }


                    string xzMsg = Regex.Match(html, @"""xzMsg"":""([\s\S]*?)""").Groups[1].Value;
                    string cxKey = Regex.Match(html, @"""cxKey"":""([\s\S]*?)""").Groups[1].Value;

                    if (cxKey != "")
                    {
                        //MessageBox.Show(cxKey);
                        xzMsg = "成功";
                        string fileurl = "https://www.51fapiao.cn/serverapi/webServer/webapi/getPdfImg/" + cxKey + "/0.png";
                        downloadFile(fileurl, spath, haoma + ".jpg", cookie);

                    }


                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(textBox1.Text);
                    lv1.SubItems.Add(haoma);
                    lv1.SubItems.Add(jine);
                    lv1.SubItems.Add(dt.ToString("yyyy-MM-dd"));
                    lv1.SubItems.Add(xzMsg);

                    if (cxKey != "")
                    {

                        return;
                    }
                    if (html.Contains("太快了"))
                        return;

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }

           
        }
        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
          
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

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 发票51_FormClosing(object sender, FormClosingEventArgs e)
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

        private void 发票51_Load(object sender, EventArgs e)
        {
            bool qidong = false;
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName.Contains("StupidOCR"))
                {
                  
                    qidong = true;  
                }
            }
            if(qidong==false)
            {
                Process.Start(path + "\\StupidOCR\\StupidOCR.exe");
            }
            
        }
    }
}
