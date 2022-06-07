using Microsoft.VisualBasic;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
                Thread.Sleep(200);
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
        public void addweb()
        {


            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                //Task.Run(() =>
                //{
                //    for (int i = 0; i < listView1.CheckedItems.Count; i++)
                //    {
                //        if (status == false)
                //            return;
                //        listView1.CheckedItems[i].SubItems[5].Text = "正在进入链接...";

                //        string xuhao = listView1.CheckedItems[i].SubItems[0].Text;
                //        string cookie = listView1.CheckedItems[i].SubItems[4].Text;

                //        string ip = listView1.CheckedItems[i].SubItems[6].Text;
                //        string user = listView1.CheckedItems[i].SubItems[7].Text;
                //        string pass = listView1.CheckedItems[i].SubItems[8].Text;


                //        // this.BeginInvoke(updateTxt, cookie, ip, user, pass);
                //        UpdateTxtMethod(xuhao, cookie,ip,user,pass);
                //        //Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000);


                //        listView1.CheckedItems[i].SubItems[5].Text = zhibojianname;
                //    }
                //});

              
                    for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    {
                        if (status == false)
                            return;
                  
                            listView1.CheckedItems[i].SubItems[5].Text = "正在进入链接...";

                            string xuhao = listView1.CheckedItems[i].SubItems[0].Text;
                            string cookie = listView1.CheckedItems[i].SubItems[4].Text;

                            string ip = listView1.CheckedItems[i].SubItems[6].Text;
                            string user = listView1.CheckedItems[i].SubItems[7].Text;
                            string pass = listView1.CheckedItems[i].SubItems[8].Text;

                    UpdateTxtMethod(xuhao, cookie, ip, user, pass);
                    listView1.CheckedItems[i].SubItems[5].Text = zhibojianname;
                   
                    }
               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }

        private void 主控端_Load(object sender, EventArgs e)
        {

        }

     

        private void button7_Click(object sender, EventArgs e)
        {
            status = false; 
            function.KillProcess("chromedriver");
            function.KillProcess("chrome"); 
            

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
            if(textBox1.Text=="")
            {
                MessageBox.Show("请输入链接");
                return;
            }


            status = true;
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                currentPid.Add(p.Id);
                
            }
            addweb();
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string filePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();
            string[] text = html.Split(new string[] { "sid_guard=" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
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

        private void button6_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(login);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }



            //进入链接
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入链接");
                return;
            }


            status = true;
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                currentPid.Add(p.Id);

            }
            addweb();
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
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(login);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

      

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
        }

        private void 退出当前链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                if (listView1.SelectedItems[i].Checked == true)
                {
                   string xuhao=listView1.SelectedItems[i].SubItems[0].Text;
                
                    foreach (int pid in browserDict.Keys)
                    {
                        if(browserDict[pid]==xuhao)
                        {
                           
                            KillProcExec(pid);
                        }
                    }



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
            if (listView1.SelectedItems.Count > 0)
            {
                string str = Interaction.InputBox("输入新cookie", "标题", "", -1, -1);
                listView1.SelectedItems[0].SubItems[4].Text = str;
            }
                
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
    }
}
