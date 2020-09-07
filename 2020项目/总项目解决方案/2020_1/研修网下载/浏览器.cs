using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 研修网下载
{
    public partial class 浏览器 : Form
    {
        public 浏览器()
        {
            InitializeComponent();
        }
        public static string cookie = "";
        string path = AppDomain.CurrentDomain.BaseDirectory+"cookie\\";
        private void button1_Click(object sender, EventArgs e)
        {
            string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "userName")
                {
                    e1.SetAttribute("value", text[Convert.ToInt32(textBox1.Text) - 1].Trim());
                }
                if (e1.GetAttribute("name") == "passWord")
                {
                    e1.SetAttribute("value", "qwe123456");
                }
            }

            //点击登陆

            HtmlElementCollection es2 = dc.GetElementsByTagName("button");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es2)
            {
                if (e1.GetAttribute("id") == "submit")
                {
                    e1.InvokeMember("click");
                }

            }



           
        }

        private void 浏览器_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;

           
              webBrowser1.Navigate("http://www.yanxiu.com/login.html?l=true");
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.yanxiu.com/login.html?l=true");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            cookie = method.GetCookies("http://i.yanxiu.com/?j=true&fl=true");


            System.IO.File.WriteAllText(path + textBox1.Text.Trim() + ".txt", cookie, Encoding.UTF8);
        }
    }
}
