using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 故障设备
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.ToString());
                return ex.ToString();
            }


        }

        #endregion
      
        string cookie = "";
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            cookie = textBox1.Text.Trim();
            listView1.Items.Clear();
            label2.Text = "已启动...";
            try
            {
                string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string today = DateTime.Now.ToString("yyyy-MM-dd");


                listView1.Columns.Add("ID", 80, HorizontalAlignment.Center);
              
                listView1.Columns.Add("FAUTYPE", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("ORGPROPERTY", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("PAIDANSTATUS", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("faultlevel", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("FAU", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("ISCROSSPLATFROM", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("FAULTSTATUS", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("FAULTTYPE", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("MAINTSUPNAME", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("RESPONDTIMEOUT", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("ORGQEVNUM", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("INSTALLWAY", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("FAULTID", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("FIXERANDTEL", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("ADDRESS", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("ADDRESSDETIL", 80, HorizontalAlignment.Center);
              
                listView1.Columns.Add("REPORTTIME", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("STATUSDESC", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("ORGNAME", 80, HorizontalAlignment.Center);
                listView1.Columns.Add("DEVTYPE", 80, HorizontalAlignment.Center);

                for (int page = 1; page < 100; page++)
                {

                    string url = "http://22.230.26.10:7000/em_psbc/faultRecord/queryList.do";
                    // string postdata = "flag=&fkorg=1&queryClick=1&search_EQ_a_terminalid=&devid=&deviceTypeNo=00%2C02&faulttype=1%2C3&faultstatus=2%2C3%2C4%2C5%2C6%2C9%2C7&upmodulename=&GE_a_reporttime=&LE_a_reporttime=&orgname=&orgattach=02&deviceStateNO=01&page="+page+"&rows=10&sort=REPORTTIME&order=desc";
                    string postdata = "flag=&fkorg=1&queryClick=1&search_EQ_a_terminalid=&devid=&deviceTypeNo=00%2C02&faulttype=1%2C3&faultstatus=1%2C2%2C3%2C4%2C5%2C6%2C9&upmodulename=&GE_a_reporttime="+today+"+00%3A00%3A00&LE_a_reporttime="+today+"+23%3A59%3A59&orgname=&orgattach=02&deviceStateNO=01&page="+page+"&rows=10&sort=REPORTTIME&order=desc";

                   // MessageBox.Show(postdata);
                    string html = PostUrl(url, postdata, cookie, "utf-8");
                    //textBox2.Text = html;
                   
                    MatchCollection a2 = Regex.Matches(html, @"""FAUTYPE"":""([\s\S]*?)""");
                    MatchCollection a3= Regex.Matches(html, @"""ORGPROPERTY"":""([\s\S]*?)""");
                    MatchCollection a4 = Regex.Matches(html, @"""PAIDANSTATUS"":""([\s\S]*?)""");
                    MatchCollection a5 = Regex.Matches(html, @"""faultlevel"":""([\s\S]*?)""");
                    MatchCollection a6 = Regex.Matches(html, @"""TERMINALID"":""([\s\S]*?)""");
                    MatchCollection a7= Regex.Matches(html, @"""FAU"":""([\s\S]*?)""");
                    MatchCollection a8 = Regex.Matches(html, @"""MODULENAME"":""([\s\S]*?)""");
                    MatchCollection a9= Regex.Matches(html, @"""ISCROSSPLATFROM"":""([\s\S]*?)""");
                    MatchCollection a10 = Regex.Matches(html, @"""DEVICESTATE"":""([\s\S]*?)""");
                    MatchCollection a11= Regex.Matches(html, @"""FAULTSTATUS"":""([\s\S]*?)""");
                    MatchCollection a12 = Regex.Matches(html, @"""TMLVERSIONID"":""([\s\S]*?)""");
                    MatchCollection a13 = Regex.Matches(html, @"""FAULTTYPE"":""([\s\S]*?)""");
                    MatchCollection a14= Regex.Matches(html, @"""MAINTSUPNAME"":""([\s\S]*?)""");
                    MatchCollection a15 = Regex.Matches(html, @"""RESPONDTIMEOUT"":""([\s\S]*?)""");
                    MatchCollection a16 = Regex.Matches(html, @"""ORGQEVNUM"":""([\s\S]*?)""");
                    MatchCollection a17 = Regex.Matches(html, @"""INSTALLWAY"":""([\s\S]*?)""");
                    MatchCollection a18 = Regex.Matches(html, @"""FIXERANDTEL"":""([\s\S]*?)""");
                    MatchCollection a19 = Regex.Matches(html, @"""ADDRESS"":""([\s\S]*?)""");
                    MatchCollection a20 = Regex.Matches(html, @"""ADDRESSDETIL"":""([\s\S]*?)""");
                  
                    MatchCollection a22 = Regex.Matches(html, @"""REPORTTIME"":""([\s\S]*?)""");
                    MatchCollection a23= Regex.Matches(html, @"""STATUSDESC"":""([\s\S]*?)""");
                    MatchCollection a24 = Regex.Matches(html, @"""ORGNAME"":""([\s\S]*?)""");
                    MatchCollection a25 = Regex.Matches(html, @"""DEVTYPE"":""([\s\S]*?)""");


                    for (int i = 0; i < a2.Count; i++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                      
                        lv1.SubItems.Add(a2[i].Groups[1].Value);
                        lv1.SubItems.Add(a3[i].Groups[1].Value);
                        lv1.SubItems.Add(a4[i].Groups[1].Value);
                        lv1.SubItems.Add(a5[i].Groups[1].Value);
                        lv1.SubItems.Add(a6[i].Groups[1].Value);
                        lv1.SubItems.Add(a7[i].Groups[1].Value);
                        lv1.SubItems.Add(a8[i].Groups[1].Value);
                        lv1.SubItems.Add(a9[i].Groups[1].Value);
                        lv1.SubItems.Add(a10[i].Groups[1].Value);
                        lv1.SubItems.Add(a11[i].Groups[1].Value);
                        lv1.SubItems.Add(a12[i].Groups[1].Value);
                        lv1.SubItems.Add(a13[i].Groups[1].Value);
                        lv1.SubItems.Add(a14[i].Groups[1].Value);
                        lv1.SubItems.Add(a15[i].Groups[1].Value);
                        lv1.SubItems.Add(a16[i].Groups[1].Value);
                        lv1.SubItems.Add(a17[i].Groups[1].Value);
                        lv1.SubItems.Add(a18[i].Groups[1].Value);
                        lv1.SubItems.Add(a19[i].Groups[1].Value);
                        lv1.SubItems.Add(a20[i].Groups[1].Value);
                      
                        lv1.SubItems.Add(a22[i].Groups[1].Value);
                        lv1.SubItems.Add(a23[i].Groups[1].Value);
                        lv1.SubItems.Add(a24[i].Groups[1].Value);
                        lv1.SubItems.Add(a25[i].Groups[1].Value);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }

                    if (a2.Count == 0)
                    {
                        break;

                    }

                    Thread.Sleep(1000);
                }

                label2.Text = "已完成";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        Thread thread;
        bool zanting = true;
        private void button3_Click(object sender, EventArgs e)
        {
           
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
