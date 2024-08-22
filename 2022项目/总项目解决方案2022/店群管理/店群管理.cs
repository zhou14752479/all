using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace 店群管理
{
    public partial class 店群管理 : Form
    {
        public 店群管理()
        {
            InitializeComponent();

            InitializeCefSharp();
        }
        public static void InitializeCefSharp()
        {
            // 创建CefSettings的实例
            CefSettings settings = new CefSettings();

            // 设置无痕模式
            settings.CefCommandLineArgs.Add("incognito", "");

            // 设置其他需要的CefSharp设置
            // ...

            // 初始化CefSharp
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }
        private ChromiumWebBrowser browser;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            browser = new ChromiumWebBrowser("https://fxg.jinritemai.com/login/common");
            browser.Dock = DockStyle.Fill;
            tabPage3.Controls.Add(browser);
        }

      
        private void 店群管理_Load(object sender, EventArgs e)
        {
           
        }
    }
}
