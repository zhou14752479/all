using Microsoft.VisualBasic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace douyinSelenium
{
    public partial class 主控端 : Form
    {
       




        public 主控端()
        {
            InitializeComponent();
        }


        bool hide = false;

        public delegate void UpdateTxt(string xuhao, string cookies, string ip, string user, string pass);
        //定义一个委托变量
        public UpdateTxt updateTxt;

        public void UpdateTxtMethod(string xuhao, string cookies, string ip, string user, string pass)
        {
          

            try
            {

                bool headless = false;
                headless = false;

                IWebDriver driver = function.getdriver(headless,false,ip,user,pass);

                driver.Manage().Window.Minimize();
                
                if(textBox1.Text.Contains("live.douyin"))
                {
                    driver.Navigate().GoToUrl("https://www.douyin.com/");
                }
               else
                {
                    driver.Navigate().GoToUrl("https://live.douyin.com/");
                }
               
                string html = driver.PageSource;
               zhibojianname = Regex.Match(html, @"<h1 class=""([\s\S]*?)>([\s\S]*?)</h1>").Groups[2].Value;
                string[] cookieall = cookies.Split(new string[] { ";" }, StringSplitOptions.None);
                foreach (var item in cookieall)
                {
                    string[] cookie = item.Split(new string[] { "=" }, StringSplitOptions.None);
                    if (cookie.Length > 1)
                    {
                        Cookie cook = new Cookie(cookie[0].Trim(), cookie[1].Trim(), "", DateTime.Now.AddDays(1));
                        driver.Manage().Cookies.AddCookie(cook);
                    }


                }
                //Thread.Sleep(200);
                driver.Navigate().GoToUrl(textBox1.Text);
                foreach (var item in cookieall)
                {
                    string[] cookie = item.Split(new string[] { "=" }, StringSplitOptions.None);
                    if (cookie.Length > 1)
                    {
                        Cookie cook = new Cookie(cookie[0].Trim(), cookie[1].Trim(), "", DateTime.Now.AddDays(1));
                        driver.Manage().Cookies.AddCookie(cook);
                    }


                }
                driver.Navigate().GoToUrl(textBox1.Text);
                StringBuilder sb = new StringBuilder(); 
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                {
                   
                    if(!currentPid.Contains(p.Id))
                    {
                        currentPid.Add(p.Id);
                       
                        browserDict.Add(p.Id, xuhao);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }

        }


        string zhibojianname = "";
        bool status = true;
   

        private void 主控端_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
           
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", function.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                string[] text2 = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                if(text2.Length>5)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                 
                    lv1.SubItems.Add(text2[1]);
                    lv1.SubItems.Add(text2[2]);
                    lv1.SubItems.Add(text2[3]);
                    lv1.SubItems.Add(text2[4]);
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(text2[6]);
                    lv1.SubItems.Add(text2[7]);
                    lv1.SubItems.Add(text2[8]);
                }
               
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            
        }

     

        private void button7_Click(object sender, EventArgs e)
        {
            status = false; 
            function.KillProcess("chromedriver");
            function.KillProcess("chrome");
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                listView1.CheckedItems[i].SubItems[5].Text = "已退出链接";

            }

        }

        private void 选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.SelectedItems[0].Checked = true;
        }

        private void 取消选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.SelectedItems[0].Checked = false;

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                listView1.SelectedItems[0].Remove();


            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[0].Text = (i + 1).ToString();
            }
        }

        private void 导入代理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> iplist = new List<string>();
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName, function.EncodingType.GetTxtType(openFileDialog1.FileName));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    for (int j = 0; j < text.Length; j++)
                    {
                        string[] ips = text[j].Split(new string[] { "/" }, StringSplitOptions.None);
                        if (ips.Length > 3)
                        {
                            if (ips[0].Contains("."))
                            {
                                iplist.Add(text[j]);

                            }

                        }

                    }

                    sr.Close();  //只关闭流
                    sr.Dispose();   //销毁流内存
                }

                int a = 0;

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (a >= iplist.Count)
                    {
                        a = 0;
                    }
                    string[] ips = iplist[a].Split(new string[] { "/" }, StringSplitOptions.None);
                    listView1.Items[i].SubItems[6].Text = ips[0] + ":" + ips[1];
                    listView1.Items[i].SubItems[7].Text = ips[2];
                    listView1.Items[i].SubItems[8].Text = ips[3];
                    a = a + 1;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void 全部退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            status = false;
            function.KillProcess("chromedriver");
            function.KillProcess("chrome");
        }

        
        List<int> currentPid=new List<int>(); 
        Dictionary<int, string> browserDict=new Dictionary<int, string>();
        private void button4_Click(object sender, EventArgs e)
        {

            //foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            //{
            //    currentPid.Add(p.Id);

            //}
            //if (thread1 == null || !thread1.IsAlive)
            //{
            //    thread1 = new Thread(main);
            //    thread1.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}

            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                if (listView1.CheckedItems[i].Checked == true)
                {
                    string xuhao = listView1.CheckedItems[i].SubItems[0].Text;


                    exitdic[xuhao] = "newlink";


                }
            }

        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string filePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();
            string[] text = html.Split(new string[] { "sid_guard=" }, StringSplitOptions.None);


            int count = text.Length;
            if(count>201)
            {
                count = 200;
            }
            for (int i = 0; i < count; i++)
            {
                if (text[i].Trim() != "")
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("sid_guard=" + text[i].Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.Checked = true;
                }
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }



        public void main()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入链接");
                return;
            }
            

            status = true;


            threadcount = 0;

            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                if (status == false)
                    return;
                listView1.CheckedItems[i].SubItems[5].Text = "正在进入链接...";

                string xuhao = listView1.CheckedItems[i].SubItems[0].Text;
                string cookies = listView1.CheckedItems[i].SubItems[4].Text;

                string ip = listView1.CheckedItems[i].SubItems[6].Text;
                string user = listView1.CheckedItems[i].SubItems[7].Text;
                string pass = listView1.CheckedItems[i].SubItems[8].Text;


                var t1 = new Thread(() => test(textBox1.Text, cookies, ip, user, pass,xuhao));

                t1.Start();
                threadcount++;
                Thread.Sleep(100);
                while (threadcount >9)
                {
                    Application.DoEvents();
                }

            }

        }

        Thread thread1;
        private void button6_Click(object sender, EventArgs e)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                currentPid.Add(p.Id);

            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(login);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (thread1 == null || !thread1.IsAlive)
            {
                thread1 = new Thread(main);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        public void login()
        {
            try
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {
                    string cookie = listView1.CheckedItems[i].SubItems[4].Text;
                    string html = function.getnickname(cookie);
                    string[] text = html.Split(new string[] { "," }, StringSplitOptions.None);
                    if (text.Length > 1)
                    {
                        listView1.CheckedItems[i].SubItems[1].Text = text[0];
                        listView1.CheckedItems[i].SubItems[2].Text = text[1];
                        if (text[1] != "掉线")
                        {
                            listView1.CheckedItems[i].SubItems[3].Text = "登录成功";
                        }
                        else
                        {
                            listView1.CheckedItems[i].SubItems[3].Text = "登录失败";
                        }
                    }

                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        Thread thread;

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                if (listView1.CheckedItems[i].Checked == true)
                {
                    string xuhao = listView1.CheckedItems[i].SubItems[0].Text;

                    exitdic[xuhao] = "1";

                    listView1.CheckedItems[i].SubItems[5].Text = "已退出";
                }
            }

        }

        Dictionary<string,string> exitdic= new Dictionary<string,string>();
        Dictionary<string, string> handledic = new Dictionary<string, string>();
        void test(string url,string cookies,string ip,string user,string pass,string xuhao)
        {
            int time = 0;
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                IWebDriver driver = GetChromeBrowser(ip,user,pass);
                string[] cookieall = cookies.Split(new string[] { ";" }, StringSplitOptions.None);
                if (textBox1.Text.Contains("live.douyin"))
                {
                    driver.Navigate().GoToUrl("https://www.douyin.com/");
                }
                else
                {
                    driver.Navigate().GoToUrl("https://live.douyin.com/");
                }
                
                foreach (var item in cookieall)
                {
                    string[] cookie = item.Split(new string[] { "=" }, StringSplitOptions.None);
                    if (cookie.Length > 1)
                    {
                        Cookie cook = new Cookie(cookie[0].Trim(), cookie[1].Trim(), "", DateTime.Now.AddDays(1));
                        driver.Manage().Cookies.AddCookie(cook);
                    }


                }

                driver.Navigate().GoToUrl(url);
                driver.Manage().Window.Minimize();
               
                foreach (var item in cookieall)
                {
                    string[] cookie = item.Split(new string[] { "=" }, StringSplitOptions.None);
                    if (cookie.Length > 1)
                    {
                        Cookie cook = new Cookie(cookie[0].Trim(), cookie[1].Trim(), "", DateTime.Now.AddDays(1));
                        driver.Manage().Cookies.AddCookie(cook);
                    }


                }
                driver.Navigate().GoToUrl(url);

                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("chrome"))
                {

                    if (!currentPid.Contains(p.Id))
                    {
                        currentPid.Add(p.Id);

                        browserDict.Add(p.Id, xuhao);
                        //MessageBox.Show(p.Id.ToString() + "  " + xuhao);
                    }

                }

             

                if (exitdic.ContainsKey(xuhao))
                {
                    exitdic[xuhao]="0";
                   
                }
                else
                {
                   
                   exitdic.Add(xuhao, "0");
                }

                if (!handledic.ContainsKey(xuhao))
                {
                    handledic.Add(xuhao, driver.CurrentWindowHandle);
                }

                
              
               

            



                string html = driver.PageSource;
                zhibojianname = Regex.Match(html, @"<h1 class=""([\s\S]*?)>([\s\S]*?)</h1>").Groups[2].Value;
                threadcount = threadcount - 1;
                if (zhibojianname == "")
                {
                    zhibojianname = "进入链接成功";
                }

                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {
                    if (listView1.CheckedItems[i].SubItems[5].Text == "正在进入链接...")
                    {
                        listView1.CheckedItems[i].SubItems[5].Text = zhibojianname;
                    }
                }



              
                //var g = Guid.NewGuid();
                //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                //js.ExecuteScript($"document.title = '{g}'");
                //心跳包定时刷新
                while (true)
                {
                    if (checkBox2.Checked == true)
                    {
                        if (time == 180)
                        {
                            for (int i = 0; i < driver.WindowHandles.Count; i++)
                            {
                                driver.SwitchTo().Window(driver.WindowHandles[i]);
                                driver.Navigate().Refresh();
                                Thread.Sleep(10);
                            }
                            
                            time = 0;
                        }

                        time = time + 1;
                    }

                    if (exitdic[xuhao] == "1")
                    {
                        //driver.Manage().Window.Minimize();
                        //driver.Quit();
                       
                      
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        driver.Close();
                        exitdic[xuhao] = "0";
                    }
                    if (exitdic[xuhao] == "2")
                    {
                        driver.Close();
                    }

                    if (exitdic[xuhao] == "newlink")
                    {
                        driver.SwitchTo().Window(driver.WindowHandles[0]);
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript($"window.open('{textBox1.Text}');");
                        exitdic[xuhao] = "0";
                    }

                    Thread.Sleep(1000);

                }

            }
            catch (Exception ex)
            {

               //MessageBox.Show(ex.ToString()) ;
            }
        }
        public static string Ex_Proxy_Name = "proxy.zip";

     
        public static IWebDriver GetChromeBrowser(string ip,string user,string pass)
        {
            

            var options = new ChromeOptions();
           // options.AddArguments("--disable-notifications");
           //options.AddArguments("--no-sandbox");
           // options.AddArguments("--disable-dev-shm-usage");
           // options.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
           // //chromeoptions.AddArguments("--remote-debugging-port=9222");
           options.BinaryLocation = "Chrome/Application/chrome.exe";
            var chromeDriverService = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            chromeDriverService.HideCommandPromptWindow = true;



            
              ChromeDriver browser = null;


            //设置IP
            
            options.AddArguments(ip);
            bool isproxysetting = true;
            isproxysetting = function.Rebuild_Extension_Proxy(user, pass);
            if (isproxysetting)
            {
                options = new ChromeOptions();
                options.Proxy = null;
                options.AddArguments("--proxy-server=" + ip);
                options.AddExtension(Ex_Proxy_Name);

                options.AddArguments("--disable-notifications");
                options.AddArguments("--no-sandbox");
                options.AddArguments("--disable-dev-shm-usage");
                options.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
                //chromeoptions.AddArguments("--remote-debugging-port=9222");
                options.BinaryLocation = "Chrome/Application/chrome.exe";
                browser = new ChromeDriver(chromeDriverService, options);

            }
            else
            {
                browser = new ChromeDriver(chromeDriverService, options);
            }



            //int nCount = 0;
            //while (browser == null && nCount < 5)
            //{

            //    try
            //    {
            //        browser = new ChromeDriver(chromeDriverService, options, TimeSpan.FromSeconds(180));
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Error initializing chrome browser: " + ex.Message);
            //    }
            //    nCount++;
            //}




          
            return browser;
        }



      static  int threadcount = 0;
        private void button1_Click(object sender, EventArgs e)
        {
           

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    listView1.Items[i].Checked = false;
                }
                else
                {
                    listView1.Items[i].Checked = true;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {


            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    listView1.Items[i].Remove();
                }
            }

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[0].Text=(i+1).ToString();
            }
        }

        private void 退出当前链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                if (listView1.SelectedItems[i].Checked == true)
                {
                    string xuhao = listView1.SelectedItems[i].SubItems[0].Text;

                    //foreach (int pid in browserDict.Keys)
                    //{
                    //    if (browserDict[pid] == xuhao)
                    //    {

                    //        KillProcExec(pid);
                    //    }
                    //}

                    exitdic[xuhao] = "1";

                    listView1.SelectedItems[i].SubItems[5].Text = "已退出";
                }
            }

           
        }

        public bool KillProcExec(int procId)
        {
            string cmd = string.Format("taskkill /f /t /im {0}", procId); //强制结束指定进程

            Process ps = null;
            try
            {
                ps = ExecCmd();
                ps.Start();
                ps.StandardInput.WriteLine(cmd + "&exit");
                return true;
            }
            catch
            {
                throw;
            }
            finally
            {
                ps.Close();
            }
        }

        public static Process ExecCmd()
        {
            //cmd = cmd.Trim().TrimEnd('&') + "&exit";


            Process p = null;
            try
            {
                p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
            }
            catch (Exception e)
            {
                throw;
            }

            return p;
        }

        private void 更换此账号cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = false;

            if (listView1.SelectedItems.Count > 0)
            {
                string str = Interaction.InputBox("输入新cookie", "标题", "", -1, -1);
                listView1.SelectedItems[0].SubItems[4].Text = str;
            }

            this.TopMost = true;

        }

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);



        List<int> hwndlist=new List<int>(); 
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                foreach (int item in hwndlist)
                {
                    ShowWindow(item, SW_SHOW);

                }

            }
            else
            {
                hwndlist.Clear();
                int hWnd = 0;
                Process[] processRunning = Process.GetProcesses();
                foreach (Process pr in processRunning)
                {
                    if (pr.ProcessName == "chrome.exe" || pr.ProcessName == "chrome")
                    {

                        hWnd = pr.MainWindowHandle.ToInt32();
                        hwndlist.Add(hWnd);
                        ShowWindow(hWnd, SW_HIDE);
                    }
                }

            }
        }

        private void 主控端_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);

                StringBuilder sb = new StringBuilder(); 
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    for (int j = 0; j < listView1.Columns.Count; j++)
                    {
                        if(j<listView1.Columns.Count-1)
                        {
                            sb.Append(listView1.Items[i].SubItems[j].Text.Trim()+",");
                        }
                        else
                        {
                            sb.AppendLine(listView1.Items[i].SubItems[j].Text.Trim());
                        }
                        
                    }
                }

                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(sb.ToString());
                sw.Close();
                fs1.Close();
                sw.Dispose();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

       
        private void 隐藏窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                if (listView1.SelectedItems[i].Checked == true)
                {
                    string xuhao = listView1.SelectedItems[i].SubItems[0].Text;


                    exitdic[xuhao] = "2";

                   
                }
            }




        }

      


    }
}
