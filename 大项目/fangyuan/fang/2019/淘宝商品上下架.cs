using Microsoft.Win32;
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

namespace fang._2019
{
    public partial class 淘宝商品上下架 : Form
    {
        public 淘宝商品上下架()
        {
            InitializeComponent();
        }


      public static  string COOKIE= "";

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset,string COOKIE)
        {
            try
            {
             
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        private void 淘宝商品上下架_Load(object sender, EventArgs e)
        {
         
        }
        bool zanting = true;
        bool status = true;
        public void run()
        {
            if (COOKIE == "")
            {
                MessageBox.Show("请登录账号！");
                return;
            }
            listView1.Items.Clear();
            try
            {
                string type = "on_sale";
                if (radioButton1.Checked == true)
                {
                    type = "on_sale";
                }

                else if (radioButton2.Checked == true)
                {
                    type = "in_stock";
                }


                for (int i = 1; i < 99999; i++)
                {

                    string url = "https://item.publish.taobao.com/taobao/manager/table.htm?jsonBody=%7B%22filter%22%3A%7B%7D%2C%22pagination%22%3A%7B%22current%22%3A"+i+"%2C%22pageSize%22%3A20%7D%2C%22table%22%3A%7B%22sort%22%3A%7B%7D%7D%2C%22tab%22%3A%22"+type+"%22%7D";

                   
                    
                    string html = GetUrl(url,"utf-8", COOKIE);
                    MatchCollection IDs = Regex.Matches(html, @"itemId"":""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"link"",""text"":""([\s\S]*?)""");

                    if (IDs.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    for (int j = 0; j < IDs.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(IDs[j].Groups[1].Value);
                        lv1.SubItems.Add(titles[j].Groups[1].Value);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (listView1.Items.Count - 1 > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }

                        if (status==false)
                        {
                            return;
                        }
                    }
                    Thread.Sleep(1000);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            #region   读取注册码信息才能运行软件！

            RegistryKey rsg = Registry.CurrentUser.OpenSubKey("zhucema"); //true表可修改                
            if (rsg != null && rsg.GetValue("mac") != null)  //如果值不为空
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }

            else
            {
                MessageBox.Show("请注册软件！");
                login lg = new login();
                lg.Show();
            }

            #endregion
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 删除此商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
            {
                
                string postdata = "jsonBody=%7B%22auctionids%22%3A%5B%22" + this.listView1.SelectedItems[i].SubItems[1].Text + "%22%5D%7D";
                string url = "https://item.publish.taobao.com/taobao/manager/batchFastEdit.htm?optType=batchDeleteItemSubmitPlugin";
                textBox5.Text += this.listView1.SelectedItems[i].SubItems[2].Text + method.PostUrl(url, postdata, COOKIE, "utf-8") + "\r\n";


            

            }
        }

        private void 淘宝商品上下架_Load_1(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // textBox1.Text = webbrowser.cookie;
            webBrowser web = new webBrowser("https://login.taobao.com/member/login.jhtml");
            web.Show();
        }

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = webBrowser.cookie;
            COOKIE = textBox1.Text;
        }

        private void 下架此商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
            {
                textBox5.Text  += this.listView1.SelectedItems[i].SubItems[2].Text + GetUrl("https://item.publish.taobao.com/taobao/manager/fastEdit.htm?optType=batchDownShelfSubmitPlugin&jsonBody=%7B%22itemId%22:%22" + this.listView1.SelectedItems[i].SubItems[1].Text + "%22%7D", "utf-8", COOKIE) + "\r\n";
            }

        }

        private void 上架此商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
            {
                textBox5.Text += this.listView1.SelectedItems[i].SubItems[2].Text + GetUrl("https://item.publish.taobao.com/taobao/manager/fastEdit.htm?optType=batchUpShelfSubmitPlugin&jsonBody=%7B%22itemId%22:%22" + this.listView1.SelectedItems[i].SubItems[1].Text + "%22%7D", "utf-8", COOKIE) + "\r\n";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
