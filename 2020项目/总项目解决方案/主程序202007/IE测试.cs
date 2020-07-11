using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202007
{
    public partial class IE测试 : Form
    {
        public IE测试()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 修改注册表信息使WebBrowser使用指定版本IE内核
        /// </summary>
        public static void SetFeatures(UInt32 ieMode)
        {
            //传入11000是IE11, 9000是IE9, 只不过当试着传入6000时, 理应是IE6, 可实际却是Edge, 这时进一步测试, 当传入除IE现有版本以外的一些数值时WebBrowser都使用Edge内核
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                throw new ApplicationException();
            }
            //获取程序及名称
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
        }

        private void IE测试_Load(object sender, EventArgs e)
        {
            // SetFeatures(9000); //iE9
            SetFeatures(1000); //不存在则用edge
           webBrowser1.Navigate("https://www.nike.com/cn/t/air-zoom-bb-nxt-ep-%E7%94%B7-%E5%A5%B3%E7%AF%AE%E7%90%83%E9%9E%8B-Wp9dZd/CK5708-001");
           
        }
    }
}
