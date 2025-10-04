using Gecko;
using Gecko.DOM;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using 微博Cookie.Common;
using 微博Cookie.Handler;
using static Gecko.ObserverNotifications;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace 微博Cookie
{
    public partial class MainForm : Form
    {
        private GeckoWebBrowser Browser;

        public MainForm()
        {

            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            initFireFox();
            
            string dbPath = Path.Combine(Environment.CurrentDirectory, "Cookies", "cookies.sqlite");//cookie目录
            if (File.Exists(dbPath))
            {
                DbHelperSQLite.connectionString = string.Format("Data Source={0};", dbPath);
            }
            else
            {
                MessageBox.Show("请在浏览器加载完成后重启软件");
            }




            // 初始化系统托盘
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = SystemIcons.Application; // 设置图标
            notifyIcon.Text = "我的程序"; // 鼠标悬停提示
            notifyIcon.Visible = true;

            // 添加右键菜单（显示/退出）
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem showItem = new ToolStripMenuItem("显示窗口");
            showItem.Click += (s, e) => this.Show(); // 显示窗口

            ToolStripMenuItem yincangItem = new ToolStripMenuItem("隐藏窗口");
            yincangItem.Click += (s, e) => this.Hide(); // 隐藏窗口
            ToolStripMenuItem exitItem = new ToolStripMenuItem("退出");
            exitItem.Click += (s, e) => Application.Exit(); // 退出程序
           
            
            contextMenu.Items.Add(showItem);
            contextMenu.Items.Add(yincangItem);
            contextMenu.Items.Add(exitItem);
            notifyIcon.ContextMenuStrip = contextMenu;
        }
        // 系统托盘控件
        private NotifyIcon notifyIcon;
        private void MainForm_Load(object sender, EventArgs e)
        {
           
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        //13307634084

        /// <summary>
        /// 浏览器初始化
        /// </summary>
        private void initFireFox()
        {
            var app_dir = Environment.CurrentDirectory;//程序目录
            string directory = Path.Combine(app_dir, "Cookies");//cookie目录
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);//检测目录是否存在
            Gecko.Xpcom.ProfileDirectory = directory;//绑定cookie目录
          


            Xpcom.Initialize(Application.StartupPath + "/xulrunner");
            GeckoPreferences.Default["extensions.blocklist.enabled"] = true;
            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;//设置偏好：字体
            GeckoPreferences.User["privacy.donottrackheader.enabled"] = true;//设置浏览器不被追踪
            GeckoPreferences.User["intl.accept_languages"] = "zh-CN,zh;q=0.9,en;q=0.8";//不设置的话默认是英文区
            GeckoPreferences.User["devtools.debugger.remote-enabled"] = true;

            XpcomHepler.RemoveCookie();//清空cookie

            Browser = new GeckoWebBrowser();
            Browser.Parent = p2;
            Browser.Dock = DockStyle.Fill;

          
            //Browser.Navigate("https://passport.weibo.cn/signin/login?entry=mweibo&res=wel");
            Browser.Navigate("https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fm.zxxk.com%2Fuser%2F");
            
        }
        string currentDirectory = Directory.GetCurrentDirectory();

       
       

       

        public void getticket()
        {
            string parentDirectory = Path.GetDirectoryName(currentDirectory);
            string cookie = XpcomHepler.ReadCookie();
            if (string.IsNullOrEmpty(cookie))
            {
                MessageBox.Show("请先登录微博在读取cookie");
            }
            else
            {
                string tiket = Regex.Match(cookie, @"xk.passport.ticket.v2=([\s\S]*?);").Groups[1].Value;
                if (tiket != "")
                {
                    tiket = "xk.passport.ticket.v2=" + tiket;
                    System.IO.File.WriteAllText(parentDirectory + "/cookie.txt", tiket, Encoding.UTF8);
                   toolStripLabel1.Text= DateTime.Now.ToString("HH:mm:ss") + "获取tiket成功";
                }
                else
                {
                    toolStripLabel1.Text = DateTime.Now.ToString("HH:mm:ss") + "获取tiket失败";
                }


                //MessageBox.Show(cookie, "这是你要的Cookie字符串，请自行调试使用");
            }
        }

       

      

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            Browser.Navigate("https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fm.zxxk.com%2Fuser%2F");
            //Browser.Refresh();
            getticket();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Browser.Navigate("https://sso.zxxk.com/login?service=https%3A%2F%2Fauth.zxxk.com%2Fcallback%3Fcurl%3Dhttps%3A%2F%2Fm.zxxk.com%2Fuser%2F");
        }

        //杭州阿里云可用
        //public string[] mobileList = { "13292447109", "17521704272", "19930453849", "18532659748", "19931467960", "13330025712", "19931412287", "15512536734" };
       
        //重庆天翼云
        public string[] mobileList = { "13379949644" , "18510708725", "18614048388" };
        private string username = "";
        private string password= "A14752479a";

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int suiji = random.Next(0, mobileList.Length);
            username = mobileList[suiji];

            var usernameElement = Browser.Document.GetElementById("username") as GeckoInputElement;
            if (usernameElement != null)
            {
                usernameElement.Value = username;
               
            }
           

            // 查找并输入密码
            var passwordElement = Browser.Document.GetElementById("password") as GeckoInputElement;
            if (passwordElement != null)
            {
                passwordElement.Value = password;
               
            }

            Thread.Sleep(500);
            var loginButton = Browser.Document.GetElementById("accountLoginBtn") as GeckoHtmlElement;
            if (loginButton != null)
            {
                loginButton.Click();
               
            }

        }




        /// <summary>
        /// 禁用浏览器加载图片
        /// </summary>
        /// <param name="browser">GeckoWebBrowser 控件</param>
        private void DisableImageLoading(GeckoWebBrowser browser)
        {
            // 获取浏览器偏好设置
            var prefs = Xpcom.GetService<nsIPrefBranch>("@mozilla.org/preferences-service;1");

            if (prefs != null)
            {
                try
                {
                    // 禁用自动加载图片
                    prefs.SetIntPref("permissions.default.image", 2);

                    // 禁用 favicon 加载
                    prefs.SetBoolPref("browser.chrome.favicons", false);

                    // 禁用图片动画
                    prefs.SetIntPref("image.animation_mode", 2);
                }
                catch (Exception ex)
                {
                   // MessageBox.Show($"设置图片加载偏好失败: {ex.Message}");
                }
            }
        }

        private void 关闭图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableImageLoading(Browser); // 禁用图片加载
        }

        private void 清理cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XpcomHepler.RemoveCookie();
        }

        private void 清理历史记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XpcomHepler.RemoveHistory();
        }

        private void 读取cookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string parentDirectory = Path.GetDirectoryName(currentDirectory);
            string cookie = XpcomHepler.ReadCookie();
            if (string.IsNullOrEmpty(cookie))
            {
                toolStripLabel1.Text = DateTime.Now.ToString("HH:mm:ss") + "请先登录微博在读取cookie";
            }
            else
            {
                string tiket = Regex.Match(cookie, @"xk.passport.ticket.v2=([\s\S]*?);").Groups[1].Value;
                if (tiket != "")
                {
                    tiket = "xk.passport.ticket.v2=" + tiket;
                    System.IO.File.WriteAllText(parentDirectory + "/cookie.txt", tiket, Encoding.UTF8);
                    MessageBox.Show(tiket, "获取tiket成功");
                }
                else
                {
                    MessageBox.Show("未获取到ticket");
                }


                //MessageBox.Show(cookie, "这是你要的Cookie字符串，请自行调试使用");
            }

            timer1.Start();
            timer1.Interval = 300000;
        }
    }
}
