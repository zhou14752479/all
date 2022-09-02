﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 身份验证
{
    public partial class 身份验证 : Form
    {
        public 身份验证()
        {
            InitializeComponent();
        }

        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;
        #region POST请求

        public string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", "");
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
             
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                //MessageBox.Show(ex.ToString());
                return ex.ToString();
            }


        }

        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 身份验证_Load(object sender, EventArgs e)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == "Fiddler")
                {
                    TestForKillMyself();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            webBrowser1.Navigate("http://www.acaiji.com/sfrz/jsencrypt.html"); //按照姓名找回 执行加密RSA JS方法
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, false);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private delegate string Encrypt(string pwd);//代理

        public string encrypt(string pwd)
        {

            string result = webBrowser1.Document.InvokeScript("RSA", new object[] { pwd }).ToString();
            return result;
        }


        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion

        public void run()
        {
            int pcount = 0;
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.ProcessName == "身份验证")
                {
                  
                    pcount++;

                }
                
            }
           
            if (pcount > 1)
            {
              
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }



            string nowname = "";
            if (DateTime.Now > Convert.ToDateTime("2023-05-26"))
            {
                return;
            }

            for (int a = 0; a < dt.Rows.Count; a++)
            {
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
                {
                    if (p.ProcessName == "Fiddler")
                    {
                        TestForKillMyself();
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }

               



                DataRow dr = dt.Rows[a];
                string name= dr[0].ToString();
                string card = dr[1].ToString();
                string phone = dr[2].ToString();

                if(name.Trim()=="")
                {
                    continue;
                }

                if (nowname ==name)
                {
                    continue;
                }

                    label3.Text = "正在查询："+name;
                Thread.Sleep(400);
                //Thread.Sleep(500); //tbapi

                Encrypt aa = new Encrypt(encrypt);
                IAsyncResult iar = BeginInvoke(aa, new object[] { phone });
                string phonecrypt = EndInvoke(iar).ToString();

                string url = method.Base64Decode(Encoding.UTF8,"aHR0cHM6Ly9wdXNlci56anp3ZncuZ292LmNuL3Nzby9uZXd1c3AuZG8=");

                string postdata = "action=regByMobile&mobilephone=" + System.Web.HttpUtility.UrlEncode(phonecrypt);
               
                //textBox1.Text = postdata;
                string html = PostUrl(url, postdata);
              
                string username = Regex.Match(html, @"""username"":""([\s\S]*?)""").Groups[1].Value;
                string mobilephone = Regex.Match(html, @"""idcard"":""([\s\S]*?)""").Groups[1].Value;
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString());
                lv1.SubItems.Add(name);
                lv1.SubItems.Add(card);
                lv1.SubItems.Add(phone);
                if (username!="")
                {
                    try
                    {
                        if ( name.Substring(name.Length - 1, 1) == username.Substring(username.Length - 1, 1) && username.Length == name.Length)
                        {

                            if(username.Substring(username.Length - 2, 1)!="*" && username.Substring(username.Length - 2, 1)== name.Substring(name.Length - 2, 1))
                            {
                                lv1.SubItems.Add("true");
                                nowname = name;
                            }
                            else
                            {
                                if(username.Substring(username.Length - 2, 1) == "*" )
                                {
                                    lv1.SubItems.Add("true");
                                    nowname = name;
                                }
                                else
                                {
                                    lv1.SubItems.Add("false");
                                }
                            }

                           

                        }
                        else
                        {
     
                            lv1.SubItems.Add("false");
                        }
                    }
                    catch (Exception)
                    {
                        //lv1.SubItems.Add("验证失败2");  //tbapi
                        lv1.SubItems.Add("false");
                        continue;
                    }
                }
                else
                {
                    lv1.SubItems.Add("false");
                    // lv1.SubItems.Add("验证失败1"); ////tbapi
                }


                while (zanting == false)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
                if (status == false)
                    return;
            }



        }

        private void button5_Click(object sender, EventArgs e)
        {
           
            listView1.Items.Clear();
            
        }

        private void 身份验证_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
