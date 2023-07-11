using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 宿网办公助手
{
    public partial class 舆情直报 : Form
    {
        public 舆情直报()
        {
            InitializeComponent();
        }

        string cookie = "_identity=ee0e342426f8781aed1c5e6351942c22e97f7a03736fd8f585d52df681efcf89a%3A2%3A%7Bi%3A0%3Bs%3A9%3A%22_identity%22%3Bi%3A1%3Bs%3A19%3A%22%5B100054%2Cnull%2C25200%5D%22%3B%7D; PHPSESSID=0tcam9c8gabnaa8hov6fatsgv0";
      
        public void submit(string title,string url_add,string content)
        {
            string url = "http://back.sq12377.cn/saas/report/addsubmit.html";
            string postdata = "_csrf=irt_vrbRunVr3eDbT3n_JOg-SGa7HswZKYRikaDQtSnmyxfP5aLwRBroroMYAJli2gsYCN9BlStu0BP966XeEA%3D%3D&title=" + title + "&url=" + url_add + "&content=" + content + "&file=";
            string html = 网站监控.PostUrlDefault(url, postdata, cookie);
            MessageBox.Show(html);
        }
        
        public void submit_shoudong()
        {
            try
            {


                if (textBox2.Text=="" || textBox3.Text == "" || textBox4.Text == "" )
                {
                    MessageBox.Show("内容或链接为空");
                    return;
                }

                    string url= textBox2.Text.Trim();
                string title = textBox3.Text.Trim();
                string content = textBox4.Text.Trim();

                submit(title,url,content);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()) ;
            }
        }
        private void 舆情直报_Load(object sender, EventArgs e)
        {

        }

        Thread thread;


        #region 获取Location
        public static string getLocation(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.AllowAutoRedirect = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                return response.GetResponseHeader("Location");
            }
            catch (Exception)
            {

                return url;            }
        }
        #endregion


        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(submit_shoudong);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public void chuli()
        {
            string url = Regex.Match(textBox1.Text.Trim(), @"[a-zA-z]+://[^\s]*").Groups[0].Value;

            string title= Regex.Match(textBox1.Text.Trim(), @"\】([\s\S]*?)h").Groups[1].Value;
            if (url.Contains("douyin"))
            {
                url=getLocation(url);   
            }
            if(title=="")
            {
               if(url!="")
                {
                    title = textBox1.Text.Replace(url, "");
                }
            }
            if (title == "")
            {
                if (url != "")
                {
                    string html = 网站监控.GetUrl(url,"utf-8");
                    title = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value; ;
                }
            }
            textBox2.Text = url;
            textBox3.Text = title;  
            textBox4.Text = title;  
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          Thread  thread = new Thread(chuli);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public void run()
        {
            try
            {
             
                string url = "https://www.12345.suzhou.com.cn/hswz-apis/bbs/post/list?currentPage=1&pageSize=50&themeType=&moduleCode=2&postType=&orderType=1&timeType=&areaCode=&quickSearch=";
                string html =网站监控.GetUrl(url, "utf-8");

                MatchCollection titles = Regex.Matches(html, @"""theme"":""([\s\S]*?)""");
                MatchCollection times = Regex.Matches(html, @"createTime"":""([\s\S]*?)""");
                MatchCollection urls = Regex.Matches(html, @"""id"":([\s\S]*?),");

                for (int a = 0; a < titles.Count; a++)
                {
                    try
                    {
                        string aurl = "https://www.12345.suzhou.com.cn/hswzbbs/post-details?postId="+ urls[a].Groups[1].Value + "&moduleCode=2&isOnFile=0";
                        string time = times[a].Groups[1].Value;

                      
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
         
                        lv1.SubItems.Add(titles[a].Groups[1].Value);
                      
                        lv1.SubItems.Add(time);
                        lv1.SubItems.Add(aurl);

                      

                     


                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                        continue;
                    }
                }



            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void submit_doublieclick()
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            string url = listView1.SelectedItems[0].SubItems[3].Text;
            string title = listView1.SelectedItems[0].SubItems[1].Text;

            string id= Regex.Match(url, @"\d{8,15}").Groups[0].Value;
            string postdata = "{\"postId\":"+id+"}";
            string html = 网站监控.PostUrl("https://www.12345.suzhou.com.cn/hswz-apis/bbs/post/detail",postdata, "","utf-8", "application/json;", "");
           string content = Regex.Match(html, @"""content"":""([\s\S]*?)""").Groups[1].Value;

            submit(title, url, content);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            submit_doublieclick();
        }

        private void 舆情直报_FormClosing(object sender, FormClosingEventArgs e)
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
