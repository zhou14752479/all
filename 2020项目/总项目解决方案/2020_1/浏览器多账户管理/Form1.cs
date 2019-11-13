using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using Microsoft.Win32;

namespace 浏览器多账户管理
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetIE(IeVersion.标准ie10);
        }

        #region  设置IE版本 亲测可用

        //SetIE(IeVersion.标准ie10);
        private enum IeVersion
        {
            强制ie10,//10001 (0x2711) Internet Explorer 10。网页以IE 10的标准模式展现，页面!DOCTYPE无效
            标准ie10,//10000 (0x02710) Internet Explorer 10。在IE 10标准模式中按照网页上!DOCTYPE指令来显示网页。Internet Explorer 10 默认值。
            强制ie9,//9999 (0x270F) Windows Internet Explorer 9. 强制IE9显示，忽略!DOCTYPE指令
            标准ie9,//9000 (0x2328) Internet Explorer 9. Internet Explorer 9默认值，在IE9标准模式中按照网页上!DOCTYPE指令来显示网页。
            强制ie8,//8888 (0x22B8) Internet Explorer 8，强制IE8标准模式显示，忽略!DOCTYPE指令
            标准ie8,//8000 (0x1F40) Internet Explorer 8默认设置，在IE8标准模式中按照网页上!DOCTYPE指令展示网页
            标准ie7//7000 (0x1B58) 使用WebBrowser Control控件的应用程序所使用的默认值，在IE7标准模式中按照网页上!DOCTYPE指令来展示网页
        }

        private void SetIE(IeVersion ver)
        {
            string productName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;//获取程序名称

            object version;
            switch (ver)
            {
                case IeVersion.标准ie7:
                    version = 0x1B58;
                    break;
                case IeVersion.标准ie8:
                    version = 0x1F40;
                    break;
                case IeVersion.强制ie8:
                    version = 0x22B8;
                    break;
                case IeVersion.标准ie9:
                    version = 0x2328;
                    break;
                case IeVersion.强制ie9:
                    version = 0x270F;
                    break;
                case IeVersion.标准ie10:
                    version = 0x02710;
                    break;
                case IeVersion.强制ie10:
                    version = 0x2711;
                    break;
                default:
                    version = 0x1F40;
                    break;
            }

            RegistryKey key = Registry.CurrentUser;
            RegistryKey software =
                key.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\" + productName);
            if (software != null)
            {
                software.Close();
                software.Dispose();
            }
            RegistryKey wwui =
                key.OpenSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            //该项必须已存在
            if (wwui != null) wwui.SetValue(productName, version, RegistryValueKind.DWord);
        }


        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://login.taobao.com/member/login.jhtml?style=b2b&css_style=b2b&from=b2b&newMini2=true&full_redirect=true&redirect_url=https%3A%2F%2Flogin.1688.com%2Fmember%2Fjump.htm%3Ftarget%3Dhttps%253A%252F%252Flogin.1688.com%252Fmember%252FmarketSigninJump.htm%253FDone%253Dhttps%25253A%25252F%25252Fwww.1688.com%25252F&reg=http%3A%2F%2Fmember.1688.com%2Fmember%2Fjoin%2Fenterprise_join.htm%3Flead%3Dhttps%253A%252F%252Fwww.1688.com%252F%26leadUrl%3Dhttps%253A%252F%252Fwww.1688.com%252F%26tracelog%3Dnotracelog_s_reg");
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            webBrowser1.Navigate(webBrowser1.StatusText);
            
            e.Cancel = true;

        }

        private void 添加店铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Button button1 = new Button();
            button1.Text = textBox4.Text;
            button1.Name = textBox2.Text + "-" + textBox3.Text;
            button1.Dock = DockStyle.Top;
            button1.Height = 40;
            button1.Font = new Font("Tahoma", 10, FontStyle.Bold);
             splitContainer1.Panel1.Controls.Add(button1);
            panel2.Visible = false;

            button1.Click += new EventHandler(aBtn_Click);//使用事件函数句柄指向一个具体的函数
        }

        private void aBtn_Click(object sender, EventArgs e)
        {


            Button btn = (Button)sender;//获取被点击的控件,按钮
          

            string[] text = btn.Name.Split(new string[] { "-" }, StringSplitOptions.None);


            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "TPL_username")
                {
                    e1.SetAttribute("value", text[0].Trim());
                }
                if (e1.GetAttribute("name") == "TPL_password")
                {
                    e1.SetAttribute("value", text[1].Trim());
                }
            }
        }
    }
}
