using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 美团
{
    public partial class 登陆 : Form
    {
        public 登陆()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory; //获取当前程序运行文件夹

        private void label5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {

            }
        }
        #region 获取公网IP
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }

            }
        }

        #endregion

        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion

        #region 注册码随机生成函数
        /// <summary>
        /// 注册码随机生成函数
        /// </summary>
        /// <returns></returns>
        public static string Random()

        {


            //string[] array = {"A","B","C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            string Hour = DateTime.Now.Hour.ToString();                    //获取当前小时
            string Hour2 = Math.Pow(Convert.ToDouble(Hour), 2).ToString();  //获取当前小时的平方
            //string zimu = array[DateTime.Now.Hour];                        //获取当前小时作为数组索引的字母
            string key = Hour + "1475" + (Hour2) + "2479" + Hour;

            return key;



        }


        #endregion


        public void login()
        {
            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");
            if (html.Contains(skinTextBox1.Text) && html.Contains(skinTextBox2.Text))
            {
                skinButton1.Text = "正在连接服务器......";

                Application.DoEvents();
                System.Threading.Thread.Sleep(200);

                skinButton1.Text = "正在验证用户名和密码......";
                Application.DoEvents();
                System.Threading.Thread.Sleep(200);



                //记住账号密码
                if (checkBox1.Checked == true)
                {

                    FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(skinTextBox1.Text);
                    sw.WriteLine(skinTextBox2.Text);
                    sw.Close();
                    fs1.Close();

                }
                Form1.username = skinTextBox1.Text;
                Form1 fm1 = new Form1();
                fm1.Show();
                this.Hide();
            }

            else
            {
                MessageBox.Show("账户不存在或者密码错误");
            }
        }
        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (skinTextBox1.Text == "" || skinTextBox1.Text == "用户名或者手机号")
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (skinTextBox2.Text == "" || skinTextBox2.Text == "请输入密码")
            {
                MessageBox.Show("请输入密码！");
                return;
            }

            login();

            
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
                //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
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

       

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }

        private void 登陆_Load(object sender, EventArgs e)
        {
            if (File.Exists(path + "config.txt"))
            {
                StreamReader sr = new StreamReader(path + "config.txt", Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                skinTextBox1.Text = text[0];
                skinTextBox2.Text = text[1];
                sr.Close();
            }
        }
    }
}
