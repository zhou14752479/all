using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using MySql.Data.MySqlClient;

namespace 乐天网店
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public static string cookie = "";


        public void run()

        {

            for (int j = 0; j < listView1.Items.Count; j++)
            {
                try
                {

                    string html = method.GetUrlWithCookie(listView1.Items[j].SubItems[1].Text, cookie, "utf-8");
                        Match post = Regex.Match(html, @"var prd =([\s\S]*?)};");
                        Match title = Regex.Match(html, @"productName"" content=""([\s\S]*?)""");
                        string postdata = post.Groups[1].Value.Trim() + "}";

                      

                        string strhtml = method.PostUrl("http://chn.lottedfs.cn/kr/product/getPrdDtlPromInfoAjax?returnUrl=productNew/common/fragments/prdPriceBenefit", postdata, cookie, "utf-8");
                        Match zhekou = Regex.Match(strhtml, @"<td class=""fcf1"">([\s\S]*?)&");
                        Match jifen = Regex.Match(strhtml, @"<div class=""priceArea""><em>最多</em> <span>([\s\S]*?)</span>");
                     
                            if (listView1.Items[j].SubItems[2].Text == zhekou.Groups[1].Value || listView1.Items[j].SubItems[2].Text=="0")
                            {
                                listView1.Items[j].SubItems[2].Text = zhekou.Groups[1].Value;
                                textBox4.Text +=DateTime.Now.ToString()+ "：网页值无变化：" + title.Groups[1].Value+"\r\n";
                        
                              }
                            else
                            {
                                MessageBox.Show("网页值出现变化！");
                                textBox4.Text = "出现变化的网址：" + title.Groups[1].Value;

                                    FileStream fs1 = new FileStream(path + "config.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1);
                                sw.WriteLine(listView1.Items[j].SubItems[1].Text+ title.Groups[1].Value);
                                sw.Close();
                                fs1.Close();
                            }
                            if (listView1.Items[j].SubItems[3].Text == jifen.Groups[1].Value || listView1.Items[j].SubItems[3].Text == "0")
                            {
                                listView1.Items[j].SubItems[3].Text = jifen.Groups[1].Value;
                               textBox4.Text += DateTime.Now.ToString() + "：网页值无变化：" + title.Groups[1].Value + "\r\n";

                           }
                            else
                            {
                        MessageBox.Show("网页值出现变化！");
                        textBox4.Text = "出现变化的网址：" + title.Groups[1].Value;
                        FileStream fs1 = new FileStream(path + "config.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(listView1.Items[j].SubItems[1].Text + title.Groups[1].Value);
                        sw.Close();
                        fs1.Close();
                    }

                    Thread.Sleep(1000);
                }

                catch
                {

                    continue;
                }


            }

           

          

        }
        private void button1_Click(object sender, EventArgs e)
        {
           
            string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                lv1.SubItems.Add(array[i]);
                lv1.SubItems.Add("0");
                lv1.SubItems.Add("0");
            }

            
            textBox4.Text = "监控已开启";
            timer1.Start();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE8);
            webBrowser1.Url = new Uri("https://chn.lps.lottedfs.cn/kr/member/login");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cookie = method.GetCookies("http://chn.lottedfs.cn/kr/product/productDetail?prdNo=10001805265&prdOptNo=10001805265");
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            webBrowser1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i]+"\r\n";
                  
                }
                   
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
