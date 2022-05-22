using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序selenium
{
    public partial class 获取COOKIE : Form
    {
        public 获取COOKIE()
        {
            InitializeComponent();
        }
        public static void KillProcess(string processName)
        {
            foreach (Process p in Process.GetProcesses())
            {
                bool flag = p.ProcessName.Contains(processName);
                if (flag)
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit();
                        Console.WriteLine("已杀掉" + processName + "进程！！！");
                    }
                    catch (Win32Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }
                    catch (InvalidOperationException e2)
                    {
                        Console.WriteLine(e2.Message.ToString());
                    }
                }
            }
        }
        private void 获取COOKIE_Load(object sender, EventArgs e)
        {
            KillProcess("chromedriver");
        }


        IWebDriver driver;
        private void button1_Click(object sender, EventArgs e)
        {
            driver = function.getdriver(false, false);
            driver.Navigate().GoToUrl("https://10.4.188.1/portal/a");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cookies = driver.Manage().Cookies.AllCookies;

            StringBuilder sb = new StringBuilder();
            foreach (var item in cookies)
            {
                sb.Append(item.Name + "=" + item.Value + ";");

            }

            string cookie = sb.ToString();
            textBox1.Text = cookie;
        }



    }
}
