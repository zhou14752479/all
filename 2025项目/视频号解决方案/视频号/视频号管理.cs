using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 视频号
{
    public partial class 视频号管理 : Form
    {
        public 视频号管理()
        {
            InitializeComponent();
        }

        private void 视频号管理_Load(object sender, EventArgs e)
        {

        }

        //public static string constr = "Host=115.190.168.152;Database=sph;Username=root;Password=eM2bN0mE4lD2";

        // public static string constr = "Host=1.15.140.126;Database=sph;Username=root;Password=eM2bN0mE4lD2";
        //public static string constr = "Host=115.190.176.30;Database=sph;Username=root;Password=eM2bN0mE4lD2";
        //public static string constr = "Host=115.190.166.221;Database=sph;Username=root;Password=eM2bN0mE4lD2";

        //public static string constr = "Host=1.117.225.132;Database=sph;Username=root;Password=eM2bN0mE4lD2";
        public static string constr = "Host=110.40.191.114;Database=sph;Username=root;Password=eM2bN0mE4lD2";
        // 存储已显示的数据ID，用于判断是否为新增项
        private HashSet<string> existingIds = new HashSet<string>();
        public void run()
        {
            label2.Text = DateTime.Now.ToString() + "正在获取...";
         

            using (MySqlConnection conn = new MySqlConnection(constr))
            {
                try
                {
                    string query = "SELECT * FROM users";
                    conn.Open(); // 打开数据库连接
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader(); // 执行查询并获取数据阅读器

                    while (reader.Read()) // 逐行读取数据
                    {
                        string uniqId=reader["uniqId"].ToString();
                        if (!existingIds.Contains(uniqId))
                        {
                            ListViewItem item = new ListViewItem((listView1.Items.Count + 1).ToString()); // 添加第一列数据到项中
                            item.SubItems.Add(reader["nickname"].ToString());
                            item.SubItems.Add(uniqId); // 添加第二列数据到子项中
                            item.SubItems.Add(reader["fansCount"].ToString());
                            item.SubItems.Add(reader["cookies"].ToString());
                            item.SubItems.Add("正常");
                            item.SubItems.Add(reader["beizhu"].ToString());
                            listView1.Items.Add(item); // 将项添加到ListView中


                            existingIds.Add(uniqId);
                            // 自动滚动到最后一行
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }
                    }
                    reader.Close(); // 关闭阅读器
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message); // 显示错误信息
                }
            }
        

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
           string title= jiance();
            if (title == "开")
            {
                timer1.Interval = Convert.ToInt32(textBox1.Text) * 1000;
                timer1.Start();
                timer2.Start();
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }


        public string jiance()
        {
            string html = PostUrlDefault("https://docs.qq.com/doc/DWWZtaG9OcGtCY3N0","","");
            string title = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value.Trim();
            return title;
        }



        public void driver(string ck)
        {    //浏览器初始化
            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = "Chrome/Application/chrome.exe";
           
            // 创建Chrome驱动服务
            var chromeService = ChromeDriverService.CreateDefaultService();

            // 隐藏驱动程序的控制台窗口
            chromeService.HideCommandPromptWindow = true;

            IWebDriver driver = new ChromeDriver(chromeService,options);
            driver.Manage().Window.Maximize();

            try
            {
                string sessionid = Regex.Match(ck, @"sessionid=([\s\S]*?);").Groups[1].Value;
                string wxuin = Regex.Match(ck, @"wxuin=([\s\S]*?);").Groups[1].Value;
                driver.Navigate().GoToUrl("https://channels.weixin.qq.com");

                OpenQA.Selenium.Cookie cookie = new OpenQA.Selenium.Cookie("sessionid", sessionid, "", DateTime.Now.AddDays(9999));
                OpenQA.Selenium.Cookie cookie2 = new OpenQA.Selenium.Cookie("wxuin", wxuin, "", DateTime.Now.AddDays(9999));
                driver.Manage().Cookies.AddCookie(cookie);
                driver.Manage().Cookies.AddCookie(cookie2);
                Thread.Sleep(500);
                //driver.Navigate().Refresh();
                driver.Navigate().GoToUrl("https://channels.weixin.qq.com");
                //浏览器初始化
                Thread.Sleep(500);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void 打开视频号助手ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {

                string cookie = listView1.SelectedItems[0].SubItems[4].Text;

                driver(cookie);
               

            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
           
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
                request.Proxy = null;
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
                request.Referer = "https://channels.weixin.qq.com/platform";
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

        private void 删除账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
               int i= listView1.SelectedItems[0].Index;
                string uniqId = listView1.SelectedItems[0].SubItems[2].Text.Trim();
                existingIds.Remove(uniqId);
                listView1.Items.RemoveAt(i);
                delete(uniqId);
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
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

        private void 添加备注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string str = Interaction.InputBox("提示信息", "添加备注", "文本内容", -1, -1);
                string uniqId = listView1.SelectedItems[0].SubItems[2].Text.Trim();
                listView1.SelectedItems[0].SubItems[6].Text=str;
                addbeizhu(uniqId,str);
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }


           
        }

        private void 视频号管理_FormClosing(object sender, FormClosingEventArgs e)
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

        #region  添加备注

        public static void addbeizhu(string uniqId, string beizhu)
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("update users SET beizhu= '" + beizhu + " ' where uniqId='" + uniqId + " ' ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                mycon.Close();
            }

            catch (System.Exception ex)
            {

            }
        }
        #endregion

        #region  添加公告

        public  void addgg()
        {

            if(textBox2.Text.Trim()=="")
            {
                MessageBox.Show("公告为空");
                return;
            }
            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("update gg SET gonggao= '" + textBox2.Text.Trim()+ " ' ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if(count>0)
                {
                    MessageBox.Show("修改成功"  );
                }
                mycon.Close();
            }

            catch (System.Exception ex)
            {

            }
        }
        #endregion

        #region  删除账号

        public static void delete(string uniqId)
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("delete from users  where uniqId='" + uniqId + " ' ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                mycon.Close();
            }

            catch (System.Exception ex)
            {

            }
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            addgg();
        }


        public void getstatus()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    string ck = listView1.Items[i].SubItems[4].Text.ToString();
                    string url = "https://channels.weixin.qq.com/cgi-bin/mmfinderassistant-bin/auth/auth_data";

                    string html = PostUrlDefault(url, "", ck);
                    string nickname = Regex.Match(html, @",""nickname"":""([\s\S]*?)""").Groups[1].Value;
                    if (nickname.Trim() == "")
                    {
                        listView1.Items[i].SubItems[5].Text = "失效";
                    }
                    else
                    {
                        listView1.Items[i].SubItems[5].Text = "正常";
                    }

                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getstatus);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 复制账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
               
               string str= listView1.SelectedItems[0].SubItems[1].Text;
                System.Windows.Forms.Clipboard.SetText(str); //复制
            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }
        }
    }
}
