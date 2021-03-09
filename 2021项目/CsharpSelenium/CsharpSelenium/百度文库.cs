using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpSelenium
{
    public partial class 百度文库 : Form
    {
        [DllImport("User32.dll")]
        public static extern int GetWindowText(IntPtr WinHandle, StringBuilder Title, int size);
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr WinHandle, StringBuilder Type, int size);
        public delegate bool EnumChildWindow(IntPtr WindowHandle, string num);
        [DllImport("User32.dll")]
        public static extern int EnumChildWindows(IntPtr WinHandle, EnumChildWindow ecw, string name);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(IntPtr hWnd, int Msg, string wParam, string lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, int WM_CHAR, int wParam, int lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, int WM_CHAR, string wParam, string lParam);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_IME_KEYDOWN = 0x0290;
        private const int WM_IME_KEYUP = 0x0291;
        private const int WM_SETTEXT = 0x000C;



        bool status = true;
        public 百度文库()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取所有文档
        /// </summary>
        /// <returns></returns>
        public FileInfo[] getfiles()
        {
            string path = textBox1.Text;
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            return files;
        }
        /// <summary>
        /// 处理文件打开窗口
        /// </summary>
        public void setFilepath(string filepaths)
        {
            try
            {
                var hWnd = IntPtr.Zero;
                var hChild = IntPtr.Zero;

                // Find Save File Dialog Box
                while (hWnd == IntPtr.Zero)
                {
                    Thread.Sleep(500);
                    hWnd = FindWindow("#32770", "打开");
                }
                if (hWnd == IntPtr.Zero) return;

                // Enter fileName
                EnumChildWindows(hWnd, (handle, s) =>
                {
                    //####取标题
                    //StringBuilder title = new StringBuilder(100);
                    //GetWindowText(handle, title, 100);//取标题
                    //if (title.ToString() == s)
                    //{
                    //    hChild = handle;
                    //    return false;
                    //}
                    //return true;
                    //####取类型
                    StringBuilder type = new StringBuilder(100);
                    GetClassName(handle, type, 100);//取类型
                    if (type.ToString() == s)
                    {
                        hChild = handle;
                        return false;
                    }

                    return true;
                }, "Edit");
                SendMessage(hChild, WM_SETTEXT, null, filepaths);
                Thread.Sleep(1000);
                // Press Save button
                hChild = FindWindowEx(hWnd, IntPtr.Zero, "Button", "打开(&O)");
                PostMessage(hChild, WM_IME_KEYDOWN, (int)System.Windows.Forms.Keys.O, 0);
                PostMessage(hChild, WM_IME_KEYUP, (int)System.Windows.Forms.Keys.O, 0);
            }
            catch (Exception ex)
            {
                logstxt.Text = ex.ToString();
                   logstxt.Text += DateTime.Now.ToShortTimeString() + "文件窗口异常" + "\r\n";
              
            }
           
        }


        public void baiduwenku()
        {    //浏览器初始化
            ChromeOptions options = new ChromeOptions();
           
            IWebDriver driver = new ChromeDriver(options);
            
            driver.Manage().Window.Maximize();

            for (int a = 0; a < listView2.Items.Count; a++)
            {
                string user = listView2.Items[a].SubItems[0].Text;
                string usercookie = listView2.Items[a].SubItems[1].Text.Replace(" ","").Trim();
        
            
                driver.Navigate().GoToUrl("https://wenku.baidu.com/");

                Cookie cookie = new Cookie("BDUSS", usercookie, "", DateTime.Now.AddDays(9999));
                driver.Manage().Cookies.AddCookie(cookie);
                Thread.Sleep(200);
                driver.Navigate().GoToUrl("https://wenku.baidu.com/shopmis#/commodityManage/documentation");
                //浏览器初始化
                Thread.Sleep(500);


                if (driver.PageSource.Contains("同意并继续"))
                {

                    driver.FindElement(By.XPath("//*[text()=\"同意并继续\"]")).Click();
                    Thread.Sleep(2000);
                }

                if (driver.PageSource.Contains("立即升级"))
                {
                    try
                    {
                        driver.FindElement(By.XPath("/html/body/section/section/main/div[3]/div/div[1]/button")).Click();
                        Thread.Sleep(2000);
                    }
                    catch (Exception)
                    {

                        
                    }
                   
                }

                FileInfo[] fileInfos = getfiles();
                logstxt.Text = DateTime.Now.ToShortTimeString() + "共读取到文件数量：" + fileInfos.Length.ToString();
                int count = Convert.ToInt32(counttxt.Text);
                try
                {
                    driver.FindElement(By.Id("global-uploader-btn")).Click();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    driver.FindElement(By.XPath("//*[@id=\"app\"]/section/main/div[1]/div/div[1]/div[2]/button/span")).Click();
                }
                Thread.Sleep(1000);
                setFilepath(textBox1.Text + "\\" + fileInfos[0].Name);
                Thread.Sleep(1000);
                if (driver.PageSource.Contains("文档上传量到达当日上限"))
                {
                    driver.Manage().Cookies.DeleteAllCookies();
                    driver.Navigate().Refresh();
                    continue;
                }
                while (true)
                {
                    


                    if (driver.PageSource.Contains("删除"))
                    {
                        Thread.Sleep(1000);
                        driver.FindElement(By.ClassName("btn-submit")).Click();
                        Thread.Sleep(1000);
                        driver.Navigate().Refresh();
                        break;
                    }
                    else if (driver.PageSource.Contains("文档重复"))
                    {
                        Thread.Sleep(1000);
                        driver.Navigate().Refresh();
                        break;
                    }
                    else if (driver.PageSource.Contains("error-msg fl"))
                    {
                        Thread.Sleep(1000);
                        driver.Navigate().Refresh();
                        break;
                    }

                }

   
                if (File.Exists(textBox1.Text + "\\" + fileInfos[0].Name))
                {
                    listView1.Items.Clear();
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(fileInfos[0].Name);
                    lv1.SubItems.Add("已上传，删除");
                    lv1.SubItems.Add(user);
                    //删除文件
                    File.Delete(textBox1.Text + "\\" + fileInfos[0].Name);
                }

                //**************单个上传结束***************************************************




                for (int i = 1; i < fileInfos.Length; i = i + count)
                {
                    if (driver.PageSource.Contains("立即升级"))
                    {
                        try
                        {
                            driver.FindElement(By.XPath("/html/body/section/section/main/div[3]/div/div[1]/button")).Click();
                            Thread.Sleep(1000);
                        }
                        catch (Exception)
                        {


                        }

                    }


                    if (driver.PageSource.Contains("文档上传量到达当日上限"))
                    {
                        driver.Manage().Cookies.DeleteAllCookies();
                        driver.Navigate().Refresh();
                        break;
                    }

                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < count; j++)
                    {
                        sb.Append("\"" + fileInfos[i + j].Name + "\" ");
                    }
                    try
                    {
                        if (driver.PageSource.Contains("升级并完善"))
                        {
                            driver.FindElement(By.ClassName("base-close")).Click();
                            
                        }

                        Thread.Sleep(10000);
                        driver.FindElement(By.Id("global-uploader-btn")).Click();
                    }
                    catch (Exception ex)
                    {

                        logstxt.Text += DateTime.Now.ToShortTimeString() + "上传按钮异常" + "\r\n";
                        ex.ToString();
                        continue;
                    }
                    Thread.Sleep(1000);
                    setFilepath(sb.ToString());
                    Thread.Sleep(1000);

                    int timecount = 1;
                    int zero = 0;
                    while (true)
                    {


                        if (driver.PageSource.Contains("文档上传量到达当日上限"))
                        {
                            status = false;
                            break;
                        }


                        MatchCollection corrects = Regex.Matches(driver.PageSource, @"class=""delect-icon"">([\s\S]*?)</li>");
                        MatchCollection errors = Regex.Matches(driver.PageSource, @"class=""error-msg fl"">([\s\S]*?)</div>");

                        logstxt.Text = (corrects.Count.ToString()) + "       " + errors.Count.ToString();
                        int allcount = corrects.Count + errors.Count+zero;
                        if (errors.Count == count)
                        {
                            status = false;
                            driver.Navigate().Refresh();
                            break;

                        }
                        if (errors.Count == count- 1)
                        {
                            status = false;
                            driver.Navigate().Refresh();
                            break;

                        }

                        if (allcount == count)
                        {
                            zero = 0;
                            Thread.Sleep(1000);
                            driver.FindElement(By.Name("checkDocList")).Click();
                            if (driver.PageSource.Contains("批量修改"))
                            {
                                Thread.Sleep(1000);
                                driver.FindElement(By.ClassName("btn-update-all")).Click();

                                Thread.Sleep(1000);
                                driver.FindElement(By.XPath("//*[@id=\"upload-doc\"]/div/div[3]/div/div[2]/div/div[5]/div/div/label[3]")).Click();
                                Thread.Sleep(1000);
                                driver.FindElement(By.ClassName("save-update")).Click();
                                Thread.Sleep(2000);
                                driver.FindElement(By.ClassName("btn-submit")).Click();
                                Thread.Sleep(1000);
                                while (true)
                                {
                                    if (driver.PageSource.Contains("恭喜您"))
                                    {
                                        status = true;
                                        break;
                                    }
                                    if (driver.PageSource.Contains("提交过于频繁"))
                                    {
                                        Thread.Sleep(5000);
                                        driver.FindElement(By.ClassName("btn-submit")).Click();
                                        
                                    }
                                    if (driver.PageSource.Contains("重新上传"))
                                    {

                                        break;
                                    }
                                }
                                driver.Navigate().Refresh();
                                break;
                            }

                        }

                        if (allcount == count - 1)
                        {
                           
                            timecount = timecount + 1;
                            Thread.Sleep(1000);
                            label4.Text = timecount.ToString();
                            if (timecount == 100)
                            {

                                zero = 1;
                                textBox2.Text = "【执行】"+DateTime.Now.ToShortTimeString() + " timecount：" + timecount + "  count：" + count;
                                timecount = 1;
                            }
                           
                           
                        }

                    }

                    if (status == true)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            File.Delete(textBox1.Text + "\\" + fileInfos[i + j].Name);
                            logstxt.Text += DateTime.Now.ToShortTimeString() + "删除文件：" + fileInfos[i + j].Name + "\r\n";

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(fileInfos[i + j].Name);
                            lv1.SubItems.Add("已上传，删除");
                            lv1.SubItems.Add(user);
                        }
                    }
                    else if (status == false)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            
                            logstxt.Text += DateTime.Now.ToShortTimeString() + "不删除文件：" + fileInfos[i + j].Name + "\r\n";

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(fileInfos[i + j].Name);
                            lv1.SubItems.Add("异常不删除");
                            lv1.SubItems.Add(user);
                        }

                    }




                }





            }

            MessageBox.Show(DateTime.Now.ToString()+"：全部上传结束");

        }


       
        private void 百度文库_Load(object sender, EventArgs e)
        {
            dingshi_start();


            DriveInfo[] infos= DriveInfo.GetDrives();
            foreach (DriveInfo info in infos)
            {
               
                if (info.Name.Contains("D"))
                {
                    textBox1.Text = "D:\\不重复文档";
                    break;
                }
                else
                {
                    textBox1.Text = "F:\\不重复文档";
                    continue;
                }
            }

         
            ListViewItem lv1 = listView2.Items.Add("13295270680"); 
            lv1.SubItems.Add("zBYRXVHOUtPd3EzZ1BLRkZOeTNJNWxTS2k2RlJZOXY1T3FkYVZudXY1V0Jhek5nSVFBQUFBJCQAAAAAAAAAAAEAAABVvFgjztK0-MnPzqjSuwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIHeC2CB3gtgT");
            ListViewItem lv2 = listView2.Items.Add("zhou14752479"); 
            lv2.SubItems.Add("mVJcn45RjVPcUVqV2RLSmN4c2ItWm4zMDFsZnd0MWJ2MlpxYXk0eU1YT3JDWEJmSVFBQUFBJCQAAAAAAQAAAAEAAABio5cbemhvdTE0NzUyNDc5AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKt8SF-rfEhfU");
            ListViewItem lv3 = listView2.Items.Add("q852266010"); 
            lv3.SubItems.Add("DJJcDIxTXk4WnBPR0xISlpuVWhsRUtOem82c0t0SmNkUFQ4b2pqN1hxfndFaGxnSVFBQUFBJCQAAAAAAAAAAAEAAACys-e7cTg1MjI2NjAxMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPCF8V~whfFfZ");
            ListViewItem lv4 = listView2.Items.Add("zhou14752478"); 
            lv4.SubItems.Add("d-UkFjZVllSnNvd3lTTnZFbk5ZRFpFQWNHakRMYmFzdmRXbWdDSldKenlYMlZmSVFBQUFBJCQAAAAAAQAAAAEAAACbWKEeemhvdTE0NzUyNDc4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPLSPV~y0j1fbX");

            ListViewItem lv5 = listView2.Items.Add("zhoukaige6668");
           lv5.SubItems.Add("k5oODdzc05tZm5tcEpBVW5hbUlKSFJ5MFhUcWltUEFWcGhHd2ttU0h1WnMzRkpnSVFBQUFBJCQAAAAAAQAAAAEAAABwsQkdemhvdWthaWdlNjY2OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGxPK2BsTytgb");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
   
                textBox1.Text = dialog.SelectedPath;
            }
        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(baiduwenku);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 百度文库_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        
        public void dingshi_start()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);


        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            string now = DateTime.Now.ToString("HH:mm:ss");
            if (now == "01:01:01")
            {
                if (thread == null || !thread.IsAlive)
                {

                    thread = new Thread(baiduwenku);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

            private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void 文件处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            文件筛选 wj = new 文件筛选();
            wj.Show();
        }

        private void 工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
