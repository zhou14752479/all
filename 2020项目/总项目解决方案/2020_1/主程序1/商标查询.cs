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
using helper;

namespace 主程序1
{
    public partial class 商标查询 : Form
    {
        public 商标查询()
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

                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization: appId=19001&timestamp=1585623031450&sign=369d983492c3f36c0eec1e8c2925503e");
                headers.Add("X-token: null");
              
                //添加头部
                request.ContentType = "application/json";
                //request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
              
                request.Referer = "https://www.qccip.com/trademark/search/index.html?searchType=APPLICANT&keyword=%E5%B9%BF%E4%B8%9C%E6%80%9D%E8%AF%BA%E4%BC%9F%E6%99%BA%E8%83%BD%E6%8A%80%E6%9C%AF";
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

                return ex.ToString();
            }


        }
        bool zanting = true;
        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "JSESSIONID=E9350A267A4F1733A3125E4F4C1DB49A";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                WebHeaderCollection headers = request.Headers;
                headers.Add("Upgrade-Insecure-Requests: 1");
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
               
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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

        public void run()
        {
           
                StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                lv1.SubItems.Add(""); 
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");

                    
                    if (array[i] == "")
                        break;



                    string name = array[i];
                    Dictionary<int, int> dic = new Dictionary<int, int>();
                    for (int a = 1; a < 99; a++)
                    {

                        string url = "https://wx.xcwz.com.cn/applet/getTMPages.do?searchKey=" + name + "&searchType=3&pageNo=" + a + "&t=1585621054635";
                        string html = GetUrl(url, "utf-8");
                        MatchCollection items = Regex.Matches(html, @"""intCls"":""([\s\S]*?)""");

                        for (int j = 0; j < items.Count; j++)
                        {
                            int value = Convert.ToInt32(items[j].Groups[1].Value);
                            if (!dic.ContainsKey(value))
                            {
                                dic.Add(value, 1);   //1代表只有1个

                            }
                            else
                            {
                                dic[value]++;       //包含了则增加1
                            }

                        }
                        if (items.Count == 0)
                        {

                            foreach (KeyValuePair<int, int> item in dic)
                            {
                              
                                lv1.SubItems[1].Text = name;
                                lv1.SubItems[2].Text = DateTime.Now.ToShortDateString();
                                lv1.SubItems[item.Key + 2].Text = item.Value.ToString();

                            }


                            break;
                        }




                    }




                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }
                    Thread.Sleep(Convert.ToInt32(textBox4.Text));
                }

                catch
                {
                    continue;

                }

            }
           

        }
        private void 商标查询_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "qwe14752479")
            {
                MessageBox.Show("平台sign验证失败");
                return;
            }
            
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"shangbiaochaxun"))
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
            
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 商标查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
