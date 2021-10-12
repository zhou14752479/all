using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Runtime.InteropServices;
using System.IO;

namespace CEF主程序
{
  

    public partial class 抖音评论采集 : Form
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

        public 抖音评论采集()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser browser;


        #region 弹出登录框
        Form f = new Form();
        TextBox t = new TextBox();
        TextBox t2 = new TextBox();
        public void loginform()
        {
           

            Label l1 = new Label();
            l1.AutoSize = true;
            Label l2 = new Label();
            l2.AutoSize = true;

            l1.Location = new System.Drawing.Point(20,60);
            l1.Size = new System.Drawing.Size(147, 14);

            l2.Location = new System.Drawing.Point(20, 100);
            l2.Size = new System.Drawing.Size(147, 14);

            l1.Text = "账号：";
            l2.Text = "密码：";      
            Button bt = new Button();
            bt.Text = "点击登录";
           bt.Click += new System.EventHandler(bt_Click);

            t.Location = new System.Drawing.Point(80,60);
            t2.Location = new System.Drawing.Point(80, 100);


            bt.Location = new System.Drawing.Point(80, 140);


            f.Controls.Add(bt);
            f.Controls.Add(l1);
            f.Controls.Add(l2);
            f.Controls.Add(t);
            f.Controls.Add(t2);

            f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            f.Text = "登录";
            f.ShowDialog();
        }



        long dianshu = 100;
        private void bt_Click(object sender, EventArgs e)
        {

            string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=login&username=" + t.Text.Trim() + "&password=" + t2.Text.Trim(), "utf-8").Trim();
            MessageBox.Show(html);
            if (html.Contains("成功"))
            {
                string ahtml = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=getall", "utf-8");

                MatchCollection ids = Regex.Matches(ahtml, @"""id"":""([\s\S]*?)""");
                MatchCollection usernames = Regex.Matches(ahtml, @"""username"":""([\s\S]*?)""");
                MatchCollection passwords = Regex.Matches(ahtml, @"""password"":""([\s\S]*?)""");
                MatchCollection times = Regex.Matches(ahtml, @"""viptime"":""([\s\S]*?)""");
                MatchCollection types = Regex.Matches(ahtml, @"""type"":""([\s\S]*?)""");
                for (int i = 0; i < ids.Count; i++)
                {


                    if (types[i].Groups[1].Value!="map" && types[i].Groups[1].Value != "shangxueba")
                    {
                        if (usernames[i].Groups[1].Value == t.Text.Trim())
                        {
                         
                            if (ExistINIFile())
                            {
                                string key = IniReadValue("values", "key");
                                string secret = IniReadValue("values", "secret");
                                if(Convert.ToDateTime(key).Day==DateTime.Now.Day)
                                {
                                    dianshu = Convert.ToInt64(secret);
                                }
                                else
                                {
                                    dianshu = Convert.ToInt64(types[i].Groups[1].Value);
                                    IniWriteValue("values", "key", DateTime.Now.ToString());
                                    IniWriteValue("values", "secret", dianshu.ToString());

                                }
                            }
                            else
                            {
                                dianshu = Convert.ToInt64(types[i].Groups[1].Value);
                                IniWriteValue("values", "key", DateTime.Now.ToString());
                                IniWriteValue("values", "secret", dianshu.ToString());

                            }
                              
                        }
                    }


                }
                f.Close();
            }
            else
            {

                MessageBox.Show("登录失败");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }


          
        }
        #endregion

        private void 抖音评论采集_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"5kHXN"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }


            #endregion
            //if (DateTime.Now > Convert.ToDateTime("2021-09-30"))
            //{
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //}


           // loginform();

            // browser = new ChromiumWebBrowser("https://www.douyin.com/video/7003247679228644621?previous_page=main_page&tab_name=home");
            browser = new ChromiumWebBrowser("https://dian.ysbang.cn/index.html#/indexContent?searchKey=&_t=1633924287163");
            Control.CheckForIllegalCrossThreadCalls = false;
            tabPage1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;
        }


        #region cefsharp获取cookie
        //browser.FrameLoadEnd += Browser_FrameLoadEnd;调用加载事件
        string cookies = "";
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            cookies = ""; //把之前加载的重复的cookie清空
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);


        }

        private void visitor_SendCookie(CefSharp.Cookie obj)
        {
            //obj.Domain.TrimStart('.') + "^" +
            cookies += obj.Name + "=" + obj.Value + ";";

            toolStripTextBox1.Text = cookies;
        }


        public class CookieVisitor : CefSharp.ICookieVisitor
        {
            public event Action<CefSharp.Cookie> SendCookie;
            public void Dispose()
            {

            }

            public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
            {
                deleteCookie = false;
                if (SendCookie != null)
                {
                    SendCookie(cookie);
                }

                return true;

            }

        }

        #endregion



        List<string> lists = new List<string>();
        public void getdata2(string url)
        {
            string html = method.GetUrlWithCookie(url, toolStripTextBox1.Text, "utf-8");
            MatchCollection uids = Regex.Matches(html, @"""uid"":""([\s\S]*?)""");
            MatchCollection short_ids = Regex.Matches(html, @"""short_id"":""([\s\S]*?)""");
            MatchCollection nicknames = Regex.Matches(html, @"""nickname"":""([\s\S]*?)""");

            MatchCollection create_times = Regex.Matches(html, @"""create_time"":([\s\S]*?),");
            MatchCollection texts = Regex.Matches(html, @"""text"":""([\s\S]*?)""");

            for (int i = 0; i < uids.Count; i++)
            {
                if (!lists.Contains(uids[i].Groups[1].Value))
                {

                    //筛选
                    if(checkBox1.Checked==true)
                    {
                        DateTime dt = ConvertStringToDateTime(create_times[i].Groups[1].Value);
                        if(dt<dateTimePicker1.Value || dt>dateTimePicker2.Value)
                        {
                            continue;
                        }

                    }
                    if (checkBox2.Checked == true)
                    {
                       
                        if (!texts[i].Groups[1].Value.Contains(textBox1.Text))
                        {
                            continue;
                        }

                    }
                    if(dianshu==0)
                    {
                        label2.Text = "点数不足，请明天再试";
                        return;
                    }
                    dianshu = dianshu - 1;
                    IniWriteValue("values", "secret", dianshu.ToString());
                    lists.Add(uids[i].Groups[1].Value);
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(uids[i].Groups[1].Value);
                    lv1.SubItems.Add(short_ids[i].Groups[1].Value);
                    lv1.SubItems.Add(nicknames[i].Groups[1].Value);
                    lv1.SubItems.Add(ConvertStringToDateTime(create_times[i].Groups[1].Value).ToString());
                    lv1.SubItems.Add(texts[i].Groups[1].Value);
                }
            }

        }

        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }
        private void 前进ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Load(toolStripTextBox1.Text);
        }

        private void 获取cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += visitor_SendCookie;
            ICookieManager cookieManager = browser.GetCookieManager();
            cookieManager.VisitAllCookies(visitor);
        }

        private void 获取request参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WinFormsRequestHandler winr = new WinFormsRequestHandler();
            browser.RequestHandler = winr;//request请求的具体实现


            winr.getdata = new WinFormsRequestHandler.GetData(getdata2);
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 后退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Back();
        }


        
    }
}
