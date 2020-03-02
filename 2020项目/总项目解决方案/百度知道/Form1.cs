using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotRas;
using System.Collections.ObjectModel;

namespace 百度知道
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url,string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
  
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void run()
        {
            string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                try
                {

                    string URL = "https://zhidao.baidu.com/search?lm=0&rn=10&pn=0&fr=search&ie=gbk&word=" + System.Web.HttpUtility.UrlEncode(array[i].Trim());


                    string html = GetUrl(URL,"GBK");
                    
                    MatchCollection uids = Regex.Matches(html, @"data-rank=""([\s\S]*?):([\s\S]*?)""");

                    string url0 = "https://zhidao.baidu.com/question/"+ uids[0].Groups[2].Value+ ".html";
                    string url1 = "https://zhidao.baidu.com/question/" + uids[1].Groups[2].Value + ".html";
                    string url2 = "https://zhidao.baidu.com/question/" + uids[2].Groups[2].Value + ".html";



                    string html0 = GetUrl(url0,"gbk");
                    string html1 = GetUrl(url1,"gbk");
                    string html2 = GetUrl(url2, "gbk");
                    textBox2.Text +=DateTime.Now.ToString()+ "：正在抓取"+ array[i]+"\r\n";

                    Match article0= Regex.Match(html0, @"<span class=""wgt-best-arrowdown""></span>([\s\S]*?)<div class=""quality-content-view-more mb-15"">");
                    Match article1 = Regex.Match(html1, @"<span class=""wgt-best-arrowdown""></span>([\s\S]*?)<div class=""quality-content-view-more mb-15"">");
                    Match article2 = Regex.Match(html2, @"<span class=""wgt-best-arrowdown""></span>([\s\S]*?)<div class=""quality-content-view-more mb-15"">");

                    StringBuilder sb = new StringBuilder();
                    sb.Append(Regex.Replace(article0.Groups[1].Value.Replace("</p>","\r\n").Replace("<br />", "\r\n"), "<[^>]+>", ""));
                    
                    sb.Append(Regex.Replace(article1.Groups[1].Value.Replace("</p>", "\r\n").Replace("<br />", "\r\n"), "<[^>]+>", ""));
                    
                    sb.Append(Regex.Replace(article2.Groups[1].Value.Replace("</p>", "\r\n").Replace("<br />", "\r\n"), "<[^>]+>", ""));

                   
                    FileStream fs1 = new FileStream(path + array[i].Trim()+".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(sb.ToString());
                    sw.Close();
                    fs1.Close();

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    

                }
                catch
                {

                    continue;
                }

                Thread.Sleep(1000);
            }
            MessageBox.Show("执行结束");


        }
        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='百度知道'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "百度知道")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }

                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
           
        }
        /// <summary>
        /// 创建或更新一个PPPOE连接(指定PPPOE名称)
        /// </summary>
        static void CreateOrUpdatePPPOE(string updatePPPOEname)
        {
            RasDialer dialer = new RasDialer();
            RasPhoneBook allUsersPhoneBook = new RasPhoneBook();
            string path = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
            allUsersPhoneBook.Open(path);
            // 如果已经该名称的PPPOE已经存在，则更新这个PPPOE服务器地址
            if (allUsersPhoneBook.Entries.Contains(updatePPPOEname))
            {
                allUsersPhoneBook.Entries[updatePPPOEname].PhoneNumber = " ";
                // 不管当前PPPOE是否连接，服务器地址的更新总能成功，如果正在连接，则需要PPPOE重启后才能起作用
                allUsersPhoneBook.Entries[updatePPPOEname].Update();
            }
            // 创建一个新PPPOE
            else
            {
                string adds = string.Empty;
                ReadOnlyCollection<RasDevice> readOnlyCollection = RasDevice.GetDevices();
                //                foreach (var col in readOnlyCollection)
                //                {
                //                    adds += col.Name + ":" + col.DeviceType.ToString() + "|||";
                //                }
                //                _log.Info("Devices are : " + adds);
                // Find the device that will be used to dial the connection.
                RasDevice device = RasDevice.GetDevices().Where(o => o.DeviceType == RasDeviceType.PPPoE).First();
                RasEntry entry = RasEntry.CreateBroadbandEntry(updatePPPOEname, device);    //建立宽带连接Entry
                entry.PhoneNumber = " ";
                allUsersPhoneBook.Entries.Add(entry);
            }
        }

        /// <summary>
        /// 断开 宽带连接
        /// </summary>
        public static void Disconnect()
        {
            ReadOnlyCollection<RasConnection> conList = RasConnection.GetActiveConnections();
            foreach (RasConnection con in conList)
            {
                con.HangUp();
            }
        }
        /// <summary>
        /// 宽带连接，成功返回true,失败返回 false
        /// </summary>
        /// <param name="PPPOEname">宽带连接名称</param>
        /// <param name="username">宽带账号</param>
        /// <param name="password">宽带密码</param>
        /// <returns></returns>
        public static bool Connect(string PPPOEname, string username, string password)
        {
            try
            {
                CreateOrUpdatePPPOE(PPPOEname);
                using (RasDialer dialer = new RasDialer())
                {
                    dialer.EntryName = PPPOEname;
                    dialer.AllowUseStoredCredentials = true;
                    dialer.Timeout = 1000;
                    dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
                    dialer.Credentials = new NetworkCredential(username, password);
                    dialer.Dial();
                    return true;
                }
            }
            catch (RasException re)
            {
                //MessageBox.Show(re.ErrorCode + " " + re.Message);
                return false;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("开启成功");
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Connect("宽带连接",textBox3.Text,textBox4.Text);
            
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }
    }
}
