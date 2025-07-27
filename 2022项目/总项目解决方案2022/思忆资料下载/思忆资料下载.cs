using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace 思忆资料下载
{
    public partial class 思忆资料下载 : Form
    {
        public 思忆资料下载()
        {
            InitializeComponent();
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;


        }


        private Point mPoint = new Point();
        public static string KEY = "";
        public string savePath = "";
        bool zanting = true;
        bool status = true;
        Thread thread;


        #region GET请求
        public static string GetUrl(string Url, string charset)
        {
            string COOKIE = "";
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.AllowAutoRedirect = true;
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

        #region 下载文件带cookie
        public static void downloadFile(string URLAddress, string subPath, string name)
        {
            try
            {
                string COOKIE = "";
                string path = Directory.GetCurrentDirectory();
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
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
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }



        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }

        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox1.Text == "输入资料网址每行一个")
            {
                textBox1.Text = "";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                savePath = dialog.SelectedPath;
            }
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

        private void button8_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            status |= true;
            if (savePath == "")
            {
                MessageBox.Show("请选择保存文件夹");
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


        /// <summary>
        /// 网址批量下载
        /// </summary>
        public void run()
        {
            if (textBox1.Text == "" || textBox1.Text.Contains("网址"))
            {
                MessageBox.Show("请输入资料网址");
                return;
            }

            string[] text = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == "")
                {
                    continue;
                }

                try
                {
                    string link = text[i].Trim();
                    string fileid = Regex.Match(link, @"\d{6,}").Groups[0].Value;
                    string url = "http://8.153.165.134:8080/api.aspx?method=getfile&key="+KEY+"&link="+link;
                    string html = GetUrl(url, "utf-8");

                    string ss = Regex.Match(html, @"""status"":""([\s\S]*?)""").Groups[1].Value.Trim();
                    string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value.Trim();
                    string fileurl = Regex.Match(html, @"""fileurl"":""([\s\S]*?)""").Groups[1].Value.Trim();
                    string filename = Regex.Match(html, @"""filename"":""([\s\S]*?)""").Groups[1].Value.Trim();

                    label9.Text = msg;
                    if (ss == "0")
                        return;


                    downloadFile(fileurl,savePath,filename);
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(filename);
                    lv1.SubItems.Add(msg.Replace("请查看浏览器下载列表", "已保存至文件夹"));

                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    Thread.Sleep(3000);

                }


                catch (Exception ex)
                {

                    label9.Text=("请下载最新版客户端");
                }
            }


        }



        /// <summary>
        /// 专技下载
        /// </summary>
        public void run2()
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("请输入专辑网址");
                return;
            }
           
            string ahtml = GetUrl(textBox3.Text.Trim(), "utf-8");
            MatchCollection aids = Regex.Matches(ahtml, @"id=""preview_([\s\S]*?)""");

            if(aids.Count==0)
            {
                MessageBox.Show("专辑网址错误，请输入最后一层专辑网址");
                return;
            }
          
            for (int i = 0; i < aids.Count; i++)


            {
                if (aids[i].Groups[1].Value == "")
                {
                    continue;
                }

                try
                {
                    string link = "https://www.zxxk.com/soft/"+ aids[i].Groups[1].Value + ".html";
                  
                    string url = "http://8.153.165.134:8080/api.aspx?method=getfile&key=" + KEY + "&link=" + link;
                    string html = GetUrl(url, "utf-8");

                    string ss = Regex.Match(html, @"""status"":""([\s\S]*?)""").Groups[1].Value.Trim();
                    string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value.Trim();
                    string fileurl = Regex.Match(html, @"""fileurl"":""([\s\S]*?)""").Groups[1].Value.Trim();
                    string filename = Regex.Match(html, @"""filename"":""([\s\S]*?)""").Groups[1].Value.Trim();

                    label10.Text = msg;
                    if (ss == "0")
                        return;
                    downloadFile(fileurl, savePath, filename);
                    ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(filename);
                    lv2.SubItems.Add(msg.Replace("请查看浏览器下载列表", "已保存至文件夹"));

                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    Thread.Sleep(3000);

                }


                catch (Exception ex)
                {

                    label10.Text = ("请下载最新版客户端");
                }
            }


        }
        #region 购买

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "http://8.153.165.134/pay/index.html?key="+KEY;
            try
            {
                // 尝试使用Chrome浏览器
                if (TryOpenWithSpecificBrowser(url, GetChromePath()))
                {
                    button2.Text = "已使用Chrome浏览器打开";
                    return;
                }

                // 尝试使用360浏览器
                if (TryOpenWithSpecificBrowser(url, Get360BrowserPath()))
                {
                    button2.Text = "已使用360浏览器打开";
                    return;
                }

                // 使用系统默认浏览器
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                button2.Text = "已使用默认浏览器打开";
            }
            catch (Exception ex)
            {
                button2.Text = "打开失败，已复制网址" + url+"到剪切板，请打开浏览器粘贴网址打开";
                System.Windows.Forms.Clipboard.SetText(url); //复制
            }
        }



        private string GetChromePath()
        {
            string chromePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Google\\Chrome\\Application\\chrome.exe");

            return File.Exists(chromePath) ? chromePath : null;
        }

        private string Get360BrowserPath()
        {
            string[] possiblePaths = new[]
            {
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "360\\360Chrome\\Chrome\\Application\\360chrome.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "360\\360Chrome\\Chrome\\Application\\360chrome.exe")
    };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        private bool TryOpenWithSpecificBrowser(string url, string browserPath)
        {
            if (string.IsNullOrEmpty(browserPath))
                return false;

            try
            {
                Process.Start(browserPath, url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region  获取mac地址
        public static string GetMacAddress()
        {
            string result;
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementBaseObject managementBaseObject in moc)
                {
                    ManagementObject mo = (ManagementObject)managementBaseObject;
                    bool flag = (bool)mo["IPEnabled"];
                    if (flag)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                result = strMac;
            }
            catch
            {
                result = "unknown";
            }
            return result;
        }
        #endregion

        #region  MD5加密
        public static string GetMD5(string txt)
        {
            string result;
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                result = sb.ToString();
            }
            return result;
        }



        #endregion

        #endregion

        private void 思忆资料下载_Load(object sender, EventArgs e)
        {
            KEY = "kehuduan_"+GetMD5(GetMacAddress()+"147258");
            label4.Text = KEY;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();    
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            status |= true;
            if (savePath == "")
            {
                MessageBox.Show("请选择保存文件夹");
                return;
            }
            status = true;

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                savePath = dialog.SelectedPath;
            }
        }

        private void button9_Click(object sender, EventArgs e)
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

        private void button12_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
