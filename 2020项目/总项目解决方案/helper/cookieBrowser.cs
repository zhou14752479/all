using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace helper
{
    public partial class cookieBrowser : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }





    [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);



        public static string webUrl = "";
        public static string cookie = "";
        public cookieBrowser(string url)
        {
            InitializeComponent();
            webUrl = url;
            method.SetFeatures(11000);
           webBrowser1.ScriptErrorsSuppressed = true;

            //webBrowser1.Url = new Uri(webUrl);
             webBrowser1.Navigate(webUrl);
        }

        private void cookieBrowser_Load(object sender, EventArgs e)
        {
        
           // timer1.Start();
          

        }
        string path = AppDomain.CurrentDomain.BaseDirectory;

        private void button1_Click(object sender, EventArgs e)
        {



            //string cookie = method.GetCookies("https://enjoy.abchina.com/jf-pcweb/transPay/getPayInfo");
            string cookie = method.GetCookies("https://mail.163.com/");
            textBox1.Text = cookie;
            
            //写入config.ini配置文件

            //IniWriteValue("values", "cookie", cookie);

            FileStream fs1 = new FileStream(path + "cookie.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine("&cookie=" + cookie + "&");
            sw.Close();
            fs1.Close();
            sw.Dispose();
           // this.Hide();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            cookie = method.GetCookies(webUrl);
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            //防止弹窗；
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
            textBox2.Text = url;
            
        }

        

        
     

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            //if条件检测按下的是不是Enter键
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.Contains("http://"))
                {
                    webBrowser1.Navigate(textBox2.Text);
                }

                else
                {
                   
                    webBrowser1.Navigate("http://"+textBox2.Text);
                }
                
            }

        }

      

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
           
            
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {

            if (webBrowser1.Url != null)
            {
                textBox2.Text = webBrowser1.Url.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }
    }
}
