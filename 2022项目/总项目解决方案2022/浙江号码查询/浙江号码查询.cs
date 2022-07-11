using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

namespace 浙江号码查询
{
    public partial class 浙江号码查询 : Form
    {
        public 浙江号码查询()
        {
            InitializeComponent();
        }


        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;

        #region POST请求

        public string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", "");
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

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
                return ex.ToString();
            }


        }

        #endregion

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
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, false);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
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
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private delegate string Encrypt(string pwd);//代理

        public string encrypt(string pwd)
        {

            string result = webBrowser1.Document.InvokeScript("RSA", new object[] { pwd }).ToString();
            return result;
        }

        public void run()
        {

          
            if (DateTime.Now > Convert.ToDateTime("2023-05-26"))
            {
                return;
            }

            int allcount = 0;
           
            for (int a = 0; a < dt.Rows.Count; a++)
            {
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                {
                    if (p.ProcessName == "Fiddler")
                    {
                        TestForKillMyself();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }

               
                if (allcount==0 || allcount>100)
                {
                  bool s11= yz(allcount.ToString());
                    if(s11==false)
                    {
                        MessageBox.Show("查询受限");
                        return;
                    }

                    allcount = 0;
                }
                allcount = allcount + 1;



                DataRow dr = dt.Rows[a];
            
                string phone = dr[0].ToString();

              
                label3.Text = "正在查询：" + phone;
                Thread.Sleep(500);
                Encrypt aa = new Encrypt(encrypt);
                IAsyncResult iar = BeginInvoke(aa, new object[] { phone });
                string phonecrypt = EndInvoke(iar).ToString();

                string url = "https://puser.zjzwfw.gov.cn/sso/newusp.do";
                string postdata = "action=regByMobile&mobilephone=" + System.Web.HttpUtility.UrlEncode(phonecrypt);
               
                string html = PostUrl(url, postdata);

                string username = Regex.Match(html, @"""username"":""([\s\S]*?)""").Groups[1].Value;
              
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
              
                lv1.SubItems.Add(phone);
                lv1.SubItems.Add(username);

                while (zanting == false)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
                if (status == false)
                    return;
            }



        }

        private void button5_Click(object sender, EventArgs e)
        {
          
            listView1.Items.Clear();
        }


        public string getcount()
        {
            string url = "http://acaiji.com/haomachaxun/do.php";
            string postdata = "method=getcount";
            string html = method.PostUrlDefault(url,postdata,"");
           
            return html;   
        }

        public string updatecount(string count)
        {
            string url = "http://acaiji.com/haomachaxun/do.php";
            string postdata = "method=updatecount&count="+count+"&time="+ DateTime.Now.ToString("yyyy-MM-dd");
            string html = method.PostUrlDefault(url, postdata, "");
         
            return html;
        }


        public bool yz(string input)
        {
            try
            {
                bool s11 = false;
                string html = getcount();
                string count2 = Regex.Match(html, @"""count"":""([\s\S]*?)""").Groups[1].Value;
                string time = Regex.Match(html, @"""time2"":""([\s\S]*?)""").Groups[1].Value;
                string limit = Regex.Match(html, @"""limit"":""([\s\S]*?)""").Groups[1].Value;

                if (time == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    if (Convert.ToInt32(count2) < Convert.ToInt32(limit))
                    {
                        int all = Convert.ToInt32(count2) + Convert.ToInt32(input);
                        updatecount(all.ToString());
                        s11 = true;
                    }
                    else
                    {
                        s11 = false;
                    }

                }
                else
                {
                    updatecount(input);
                    s11 = true;
                }

                return s11;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private void 浙江号码查询_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"3v1c"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion

            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == "Fiddler")
                {
                    TestForKillMyself();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            webBrowser1.Navigate("http://www.acaiji.com/sfrz/jsencrypt.html"); //按照姓名找回 执行加密RSA JS方法 
        }
    }
}
