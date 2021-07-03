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

namespace main._2019_10
{
    public partial class 登陆 : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        public static string COOKIE = "";
        public 登陆()
        {
            InitializeComponent();
        }

        private void 登陆_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;

            //默认打开淘宝登录
            webBrowser1.Url = new Uri("https://login.taobao.com/member/login.jhtml?style=mini&newMini2=true&from=alimama&redirectURL=http%3A%2F%2Flogin.taobao.com%2Fmember%2Ftaobaoke%2Flogin.htm%3Fis_login%3d1&full_redirect=true&disableQuickLogin=true");
            foreach (Control ctr in splitContainer1.Panel2.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "存值\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "TPL_username")
                {
                    e1.SetAttribute("value", textBox1.Text.Trim());
                }
                if (e1.GetAttribute("name") == "TPL_password")
                {
                    e1.SetAttribute("value", textBox2.Text.Trim());
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://www.alimama.com/member/minilogin.htm?redirect=https%3A%2F%2Fwww.alimama.com%2Findex.htm");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "logname")
                {
                    e1.SetAttribute("value", textBox3.Text.Trim());
                }
                if (e1.GetAttribute("id") == "J_logpassword")
                {
                    e1.SetAttribute("value", textBox4.Text.Trim());
                }
            }
        }
        #region  获取cookie
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;


                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }

        #endregion

      
        private void button4_Click(object sender, EventArgs e)
        {
            //存放所有text的值
            foreach (Control ctr in splitContainer1.Panel2.Controls)
            {
                if (ctr is TextBox)
                {


                    string path = AppDomain.CurrentDomain.BaseDirectory + "存值\\";
                    FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(ctr.Text);
                    sw.Close();
                    fs1.Close();

                }
            }

            COOKIE = GetCookies("https://pub.alimama.com/manage/overview/index.htm");
            淘宝联盟.gett();

          
            淘宝联盟.label1.Text = textBox1.Text;
            this.Hide();
        }

       
    }
}
