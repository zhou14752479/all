using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202011
{
    public partial class 新疆继续教育 : Form
    {
        public 新疆继续教育()
        {
            InitializeComponent();
        }


        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://servicewechat.com/wx8b422900aa39f849/19/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.13(0x17000d2a) NetType/4G Language/zh_CN";        
                request.AllowAutoRedirect = true;
                 request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

            
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.Referer = "https://servicewechat.com/wx8b422900aa39f849/19/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.13(0x17000d2a) NetType/4G Language/zh_CN";
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

                return ex.ToString();
            }


        }

        #endregion

    

       
        private void 新疆继续教育_Load(object sender, EventArgs e)
        {
          
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Url = new Uri("http://www.xjrsjxjy.com/");



            Control.CheckForIllegalCrossThreadCalls = false;
        }



        ArrayList codelist = new ArrayList();
        int codeindex = 0;
        public string getcode()
        {
            GetUrl("http://47.102.145.207/index.php?codenum=1");
            string codehtml = GetUrl("http://47.102.145.207/getcode.txt");
            string code = Regex.Match(codehtml, @"[A-Za-z0-9]{10,}").Groups[0].Value;
            while (code=="")
            {
                codehtml = GetUrl("http://47.102.145.207/getcode.txt");
                code = Regex.Match(codehtml, @"[A-Za-z0-9]{10,}").Groups[0].Value;
            }
          
            return code ;

        }


        public string login(string user,string pass,string code)
        {
           
            string cre = PostUrl("https://yun.xjrsjxjy.com/API/BindStudent.ashx", "code=" + code + "&idCardNumber=" + user + "&password=" + pass);
          
           
            if (cre != "")
            {
               string credential = cre.Replace("\"", "");
                //写入config.ini配置文件
                IniWriteValue("values", user, credential);

                return credential;
            }
            else
            {
                return "";
            }


        }


        public string getinfo( string newcode,string cre)
        {
            string html = GetUrl("https://yun.xjrsjxjy.com/API/GetStudent.ashx?code="+newcode+"&credential="+cre+"&year=2020 ");
            Match name = Regex.Match(html, @"""Name"":""([\s\S]*?)""");

            return name.Groups[1].Value;
        }
     
  

        private void 新疆继续教育_FormClosing(object sender, FormClosingEventArgs e)
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

        private void listView2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView2_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in filePath)
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                    ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(value[0].Trim());
                    lv2.SubItems.Add(value[1].Trim());
                    lv2.SubItems.Add("");
                    lv2.SubItems.Add("");
                    lv2.SubItems.Add("");
                    lv2.SubItems.Add("");

                }
            }




        }



        xjstudy xj = new xjstudy();

        Thread t;
        public void denglu()
        {
           
            for (int i = 0; i < listView2.CheckedItems.Count; i++)
            {
               
                string card = listView2.CheckedItems[i].SubItems[1].Text;
                string pass = listView2.CheckedItems[i].SubItems[2].Text;

                if (ExistINIFile())
                {


                    string cre = IniReadValue("values", card);  //读取config.ini cre

                    if (cre != "")
                    {
                       
                        string newcode = xj.getcode(cre);
                        string name = getinfo(newcode, cre);

                        if (name != "")
                        {
                            listView2.CheckedItems[i].SubItems[3].Text = name;
                            listView2.CheckedItems[i].SubItems[4].Text = "登录成功";
                            listView2.CheckedItems[i].SubItems[5].Text = cre;
                        }


                        else
                        {
                            string code = getcode();
                            string cre1 = login(card, pass, code);

                            string newcode1 = xj.getcode(cre1);
                            if (cre != "")
                            {
                                name = getinfo(newcode1, cre1);
                                listView2.CheckedItems[i].SubItems[3].Text = name;
                                listView2.CheckedItems[i].SubItems[4].Text = "登录成功";
                                listView2.CheckedItems[i].SubItems[5].Text = cre1;

                            }
                            else
                            {

                                listView2.CheckedItems[i].SubItems[4].Text = "登录失败";
                                listView2.CheckedItems[i].SubItems[5].Text = cre;

                            }
                        }
                    }
                    else
                    {

                      
                        string code = getcode();

                        cre = login(card, pass, code);

                        string newcode = xj.getcode(cre);
                        if (cre != "")
                        {
                            string name = getinfo(newcode, cre);
                            listView2.CheckedItems[i].SubItems[3].Text = name;
                            listView2.CheckedItems[i].SubItems[4].Text = "登录成功";
                            listView2.CheckedItems[i].SubItems[5].Text = cre;

                        }
                        else
                        {

                            listView2.CheckedItems[i].SubItems[4].Text = "登录失败";
                            listView2.CheckedItems[i].SubItems[5].Text = cre;

                        }

                    }

                }
                else
                {
                    MessageBox.Show("不存在ini文件");
                }




            }
        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com:8080/api/vip.html");
       
            if (!html.Contains(@"xinjiang"))
            {
                MessageBox.Show("8080");
                return;
            }

            #endregion
        
            if (t == null || !t.IsAlive)
            {
              

                t = new Thread(denglu);
                t.Start();
                
            }

        }

        private void 开始学习ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView2.CheckedItems.Count; i++)
            {

                string name = listView2.CheckedItems[i].SubItems[3].Text;
                string cre = listView2.CheckedItems[i].SubItems[5].Text;
                xjstudy xj = new xjstudy();

                xj.getlogs += new xjstudy.GetLogs(setlog);  //先绑定委托在执行后面的方法



                xj.credential = cre;
                xj.newt();
                xj.username = name;


            }
        }

        public void setlog(string str)
        {
           
            logtxt.Text += str + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            for (int i = 0; i < listView2.Items.Count; i++)
            {
                listView2.Items[i].Checked = true;
            }

            if (t == null || !t.IsAlive)
            {


                t = new Thread(denglu);
                t.Start();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                listView2.Items[i].Checked = true;
            }
            for (int i = 0; i < listView2.CheckedItems.Count; i++)
            {

                string name = listView2.CheckedItems[i].SubItems[3].Text;
                string cre = listView2.CheckedItems[i].SubItems[5].Text;
                xjstudy xj = new xjstudy();

                xj.getlogs += new xjstudy.GetLogs(setlog);  //先绑定委托在执行后面的方法



                xj.credential = cre;
                xj.newt();
                xj.username = name;


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }
    }
}
