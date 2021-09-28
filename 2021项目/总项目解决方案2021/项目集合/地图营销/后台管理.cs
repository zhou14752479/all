using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using myDLL;

namespace 地图营销
{
    public partial class 后台管理 : Form
    {
        public 后台管理()
        {
            InitializeComponent();
        }

        //string type = "shangxueba";

        string type = "douyin";
        private void 后台管理_Load(object sender, EventArgs e)
        {
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            treeView1.Nodes[0].Expand();
            tabControl1.SelectedIndex = 3;
            //webBrowser1.ScriptErrorsSuppressed = true;
            if (type != "shangxueba")
            {
               
                linkLabel3.Visible = false;
            }

            Control.CheckForIllegalCrossThreadCalls = false;
           
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Text == "用户列表")
            {
                tabControl1.SelectedIndex = 0;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(getall);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (treeView1.SelectedNode.Text == "用户添加")
            {
                tabControl1.SelectedIndex = 1;
            }
          
            if (treeView1.SelectedNode.Text == "主页")
            {
                tabControl1.SelectedIndex = 3;
            }
            if (treeView1.SelectedNode.Text == "账号生成")
            {
                if (type == "shangxueba")
                {

                    tabControl1.SelectedIndex = 4;
                }
                else
                {
                    tabControl1.SelectedIndex = 1;
                }
               
            }
        }
  

        public void getall()
        {
            listView1.Items.Clear();
            string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=getall", "utf-8");
           
            MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
            MatchCollection usernames = Regex.Matches(html, @"""username"":""([\s\S]*?)""");
            MatchCollection passwords = Regex.Matches(html, @"""password"":""([\s\S]*?)""");
            MatchCollection times = Regex.Matches(html, @"""viptime"":""([\s\S]*?)""");
            MatchCollection types = Regex.Matches(html, @"""type"":""([\s\S]*?)""");
            for (int i = 0; i < ids.Count; i++)
            {
                //if (types[i].Groups[1].Value == type)
                //{
                //    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                //                                                                                    //lv1.SubItems.Add(ids[i].Groups[1].Value);
                //    lv1.SubItems.Add(usernames[i].Groups[1].Value);
                //    lv1.SubItems.Add(passwords[i].Groups[1].Value);
                //    lv1.SubItems.Add(times[i].Groups[1].Value);

                //}


                if (types[i].Groups[1].Value != "map" && types[i].Groups[1].Value != "shangxueba")
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                                                                    //lv1.SubItems.Add(ids[i].Groups[1].Value);
                    lv1.SubItems.Add(usernames[i].Groups[1].Value);
                    lv1.SubItems.Add(passwords[i].Groups[1].Value);
                    // lv1.SubItems.Add(times[i].Groups[1].Value);
                    lv1.SubItems.Add(types[i].Groups[1].Value);
                }

            }

          
        }

        public void register()
        {
            if (user_txt.Text=="" || pass_txt.Text == "")
            {
                MessageBox.Show("请输入账号和密码");
                return;
            }

            decimal days = numericUpDown1.Value + (numericUpDown2.Value/24);
            // string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=register&username="+user_txt.Text.Trim()+"&password="+pass_txt.Text.Trim()+"&days="+days+ "&type=" +type, "utf-8");
            string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=register&username=" + user_txt.Text.Trim() + "&password=" + pass_txt.Text.Trim() + "&days=" + days + "&type=" + textBox4.Text, "utf-8");
            MessageBox.Show(html.Trim());
            user_txt.Text = "";
            pass_txt.Text= "";

        }


        public void delete()
        {
            
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string url = "http://www.acaiji.com/shangxueba/shangxueba.php?method=del&username="+ listView1.CheckedItems[i].SubItems[1].Text.Trim();
                string html = method.GetUrl(url, "utf-8");
              
                MessageBox.Show(html.Trim());
            }


            getall();


        }

        public void autodelete()
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if(Convert.ToDateTime(listView1.Items[i].SubItems[3].Text)<DateTime.Now)
                {
                    string url = "http://www.acaiji.com/shangxueba/shangxueba.php?method=del&username=" + listView1.Items[i].SubItems[1].Text.Trim();
                    string html = method.GetUrl(url, "utf-8");
                }
               
              
            }


            getall();


        }

        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getall);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(register);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex =1;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            browser = new ChromiumWebBrowser("https://passport.shangxueba.com/user/userlogin.aspx?url=https%3A//www.shangxueba.com/ask/32327.html");
            // Cef.Initialize(new CefSettings());
            panel2.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
           browser.LifeSpanHandler = new OpenPageSelf();
            browser.FrameLoadEnd += Browser_FrameLoadEnd;

            tabControl1.SelectedIndex =2;
        }

       

       

       

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(delete);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        
              
              
            
        }

        ChromiumWebBrowser browser;

        #region   cefsharp在自己窗口打开链接
        //调用 browser.LifeSpanHandler = new OpenPageSelf();
        /// <summary>
        /// 在自己窗口打开链接
        /// </summary>
        internal class OpenPageSelf : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser browserControl, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
    string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
    IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                newBrowser = null;
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
                chromiumWebBrowser.Load(targetUrl);
                return true; //Return true to cancel the popup creation copyright by codebye.com.
            }
        }



        #endregion


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

            textBox1.Text = cookies;
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


        private void button1_Click(object sender, EventArgs e)
        {
            //CookieVisitor visitor = new CookieVisitor();
            //visitor.SendCookie += visitor_SendCookie;
            //ICookieManager cookieManager = browser.GetCookieManager();
            //cookieManager.VisitAllCookies(visitor);


            if (cookies != "")
            {
                textBox1.Text = cookies;
                string url = "http://www.acaiji.com/shangxueba/shangxueba.php?method=setcookie";
                string postdata = "cookie=" + System.Web.HttpUtility.UrlEncode(cookies);
                string msg = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");
                MessageBox.Show(msg.Trim());

            }
            else
            {
                MessageBox.Show("更新失败，请重试");
            }
           

        }

        private void button5_Click(object sender, EventArgs e)
        {
           browser.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(autodelete);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }


        public void suijizhanghao()
        {
            try
            {
                string zimu = "123456789abcdefghjkmnpqrstuvwxyz";

                for (int a = 0; a < Convert.ToInt32(textBox2.Text); a++)
                {
                    Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同

                    string value = "";
                    for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
                    {

                        int suijizimu = rd.Next(0, 30);
                        value = value + zimu[suijizimu];
                    }


                    ListViewItem lv1 = listView2.Items.Add(value); //使用Listview展示数据
                    lv1.SubItems.Add(value);
                    decimal days = numericUpDown3.Value + (numericUpDown4.Value / 24);
                    string html = method.GetUrl("http://www.acaiji.com/shangxueba/shangxueba.php?method=register&username=" + value + "&password=" + value + "&days=" + days + "&type=" + type, "utf-8");
                }


            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(suijizhanghao);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }
    }
}
