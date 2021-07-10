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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 新疆继续教育
{
    public partial class 单个版 : Form
    {
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
        public string GetUrl(string Url)
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


        ArrayList codelist = new ArrayList();
       
        public string getcode()
        {
            //GetUrl("http://47.102.145.207/index.php?codenum=1");
            //string codehtml = GetUrl("http://47.102.145.207/getcode.txt");
            //string code = Regex.Match(codehtml, @"[A-Za-z0-9]{10,}").Groups[0].Value;
            //while (code == "")
            //{
            //    codehtml = GetUrl("http://47.102.145.207/getcode.txt");
            //    code = Regex.Match(codehtml, @"[A-Za-z0-9]{10,}").Groups[0].Value;
               

            //}
           // return "091zHk000xgfQL1gF13008Lgvm2zHk0F";
            return  textBox3.Text;

        }



        public string login(string user, string pass, string code)
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


        public string getinfo(string newcode, string cre)
        {
            string html = GetUrl("https://yun.xjrsjxjy.com/API/GetStudent.ashx?code=" + newcode + "&credential=" + cre + "&year=2020 ");
            Match name = Regex.Match(html, @"""Name"":""([\s\S]*?)""");

            return name.Groups[1].Value;
        }

        xjstudy xj = new xjstudy();

        Thread t;

        string CRE = "";
        public void denglu()
        {

            string card = textBox1.Text.Trim();
            string pass = textBox2.Text.Trim();

            if (ExistINIFile())
            {


                string cre = IniReadValue("values", card);  //读取config.ini cre

                if (cre != "")
                {

                    string newcode = getcode(cre);
                    string name = getinfo(newcode, cre);
                   
                    if (name != "")
                    {
                        logtxt.Text = name + "：登录成功";
                        CRE = cre;
                    }


                    else
                    {
                        string code = getcode();
                      
                        string cre1 = login(card, pass, code);

                        string newcode1 = getcode(cre1);
                        if (cre1 != "")
                        {
                            name = getinfo(newcode1, cre1);
                         
                            logtxt.Text = name + "：登录成功";
                            CRE = cre1;
                        }
                        else
                        {

                            logtxt.Text = "登录失败";

                        }
                    }
                }
                else
                {


                    string code = getcode();

                    cre = login(card, pass, code);

                    string newcode = getcode(cre);
                    if (cre != "")
                    {
                        string name = getinfo(newcode, cre);
                       
                        logtxt.Text = name + "：登录成功";
                        CRE = cre;

                    }
                    else
                    {

                        logtxt.Text = "登录失败";

                    }

                }

            }
            else
            {
                MessageBox.Show("不存在ini文件");
            }





        }



        public void getcouse()
        {


            string code = getcode(CRE);

            for (int year = 2014; year <= 2021; year++)
            {


                string url = "https://yun.xjrsjxjy.com/API/GetStudyCoursewares.ashx?code=" + code + "&credential=" + CRE + "&year=" + year + "&isFree=false";

                string html = GetUrl(url);
                if (html.Contains("登录超时"))
                {
                    MessageBox.Show("账号已掉线");

                }
                MatchCollection ids = Regex.Matches(html, @"""Id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""Name"":""([\s\S]*?)""");
                MatchCollection Progress = Regex.Matches(html, @"""LearningProgress"":([\s\S]*?),");



                for (int j = 0; j < ids.Count; j++)
                {

                    string coursewareId = ids[j].Groups[1].Value.ToString();
                    ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(year.ToString());
                    lv2.SubItems.Add(names[j].Groups[1].Value);
                    lv2.SubItems.Add(Convert.ToDouble(Progress[j].Groups[1].Value)*100+"%");
                    lv2.SubItems.Add(ids[j].Groups[1].Value);
                }


            }
        }
        public 单个版()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (t == null || !t.IsAlive)
            {

                listView2.Items.Clear();
                t = new Thread(getcouse);
                t.Start();

            }
        }

        private void 单个版_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"epkOu"))
            {

                return;
            }



            #endregion

            if (t == null || !t.IsAlive)
            {
                t = new Thread(denglu);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        public void setlog(string str)
        {

            logtxt.Text += str + Environment.NewLine;
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
              
              
                Thread t = new Thread(run);
                t.Start();
              


           

        }

        public void run()
        {
            for (int i = 0; i < listView2.CheckedItems.Count; i++)
            {

                string name = listView2.CheckedItems[i].SubItems[2].Text;
                string coursewareId = listView2.CheckedItems[i].SubItems[4].Text;
                string year = listView2.CheckedItems[i].SubItems[1].Text;
                string code = getcode(CRE);
                string aurl = "https://yun.xjrsjxjy.com/API/GetChapters.ashx?code=" + code + "&credential=" + CRE + "&year=" + year + "&coursewareId=" + coursewareId;

                string ahtml = GetUrl(aurl);
                if (ahtml.Contains("登录超时"))
                {
                    MessageBox.Show("账号已掉线");

                }
                MatchCollection aids = Regex.Matches(ahtml, @"""Id"":""([\s\S]*?)"",([\s\S]*?)""TotalSeconds"":([\s\S]*?)\}");
                MatchCollection LearningProgress = Regex.Matches(ahtml, @"""LearningProgress"":([\s\S]*?),");


                for (int z = 0; z < aids.Count; z++)
                {
                    string jindu = (Convert.ToDouble(LearningProgress[z].Groups[1].Value) * 100).ToString() + "%";

                    logtxt.Text += (name + "：" + jindu + "\r\n");
                    if (LearningProgress[z].Groups[1].Value != "1")
                    {

                        string o = year + "#" + coursewareId + "#" + aids[z].Groups[1].Value + "#" + aids[z].Groups[3].Value;
                        study((object)o);

                    }

                }
            }

            logtxt.Text += ("选中课程学习结束");


        }


        public void study(object parame)
        {
            
            string code = getcode(CRE);
            string[] text = parame.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            string year = text[0];
            string coursewareId = text[1];
            string chapterId = text[2];

            int totalseconds = (Convert.ToInt32(Convert.ToDouble(text[3]))) + 150;

            string url = "https://yun.xjrsjxjy.com/API/SaveLearningProgress.ashx";
            string postdata = "code=" + code + "&credential=" + CRE + "&year=" + year + "&coursewareId=" + coursewareId + "&chapterId=" + chapterId + "&type=0&progress=0";

            string html = PostUrl(url, postdata);
            
            for (int i = 1; i < totalseconds; i++)
            {
                string postdata1 = "code=" + code + "&credential=" + CRE + "&year=" + year + "&coursewareId=" + coursewareId + "&chapterId=" + chapterId + "&type=1&progress=" + i;

                if (i % 60 == 0)
                {
                    PostUrl(url, postdata1);

                    logtxt.Text +=  i+ "\r\n";
                }
                Thread.Sleep(1011);
            }

        }

        public string getcode(string credential)
        {
            string code = xj.GetMD5(credential + "Xj@Jx#Jy$123");
            return code;

        }

        private void 单个版_Load(object sender, EventArgs e)
        {

        }
    }
}
