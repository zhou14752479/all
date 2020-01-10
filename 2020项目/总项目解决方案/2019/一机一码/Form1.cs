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

namespace 一机一码
{
    public partial class Form1 : Form
    {
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
        public Form1()
        {
            InitializeComponent();
            SetIE(IeVersion.强制ie10);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://zh.surebet.com/surebets?utf8=%E2%9C%93&selector%5Border%5D=profit&selector%5Boutcomes%5D%5B%5D=&selector%5Boutcomes%5D%5B%5D=2&selector%5Boutcomes%5D%5B%5D=3&selector%5Bmin_profit%5D=0.0&selector%5Bmax_profit%5D=&selector%5Bmin_roi%5D=&selector%5Bmax_roi%5D=&selector%5Bsettled_in%5D=0&selector%5Bbookies_settings%5D=0%3A66%3A%3A%3B0%3A69%3A%3A%3B0%3A72%3A%3A%3B4%3A65%3A%3A%3B0%3A71%3A%3A%3B0%3A70%3A%3A%3B0%3A68%3A%3A%3B0%3A117%3A%3A%3B0%3A109%3A%3A%3B4%3A21%3A%3A%3B0%3A26%3A%3A%3B0%3A23%3A%3A%3B4%3A0%3A%3A%3B0%3A32%3A%3A%3B0%3A99%3A%3A%3B0%3A29%3A%3A%3B0%3A10%3A%3A%3B0%3A45%3A%3A%3B0%3A58%3A%3A%3B0%3A14%3A%3A%3B0%3A11%3A%3A%3B0%3A38%3A%3A%3B0%3A55%3A%3A%3B0%3A33%3A%3A%3B4%3A120%3A%3A%3B0%3A49%3A%3A%3B0%3A62%3A%3A%3B0%3A12%3A%3A%3B0%3A46%3A%3A%3B0%3A112%3A%3A%3B0%3A24%3A%3A%3B0%3A82%3A%3A%3B0%3A124%3A%3A%3B4%3A103%3A%3A%3B4%3A5%3A%3A%3B4%3A102%3A%3A%3B0%3A6%3A%3A%3B4%3A4%3A%3A%3B0%3A30%3A%3A%3B0%3A15%3A%3A%3B0%3A123%3A%3A%3B0%3A50%3A%3A%3B0%3A9%3A%3A%3B4%3A41%3A%3A%3B4%3A3%3A%3A%3B0%3A8%3A%3A%3B0%3A113%3A%3A%3B0%3A83%3A%3A%3B0%3A119%3A%3A%3B4%3A63%3A%3A%3B4%3A61%3A%3A%3B0%3A39%3A%3A%3B4%3A31%3A%3A%3B0%3A51%3A%3A%3B0%3A2%3A%3A%3B0%3A7%3A%3A%3B0%3A106%3A%3A%3B4%3A1%3A%3A%3B0%3A105%3A%3A%3B4%3A101%3A%3A%3B0%3A118%3A%3A%3B0%3A25%3A%3A%3B0%3A114%3A%3A%3B0%3A40%3A%3A%3B0%3A43%3A%3A%3B4%3A18%3A%3A%3B0%3A59%3A%3A%3B0%3A115%3A%3A%3B4%3A122%3A%3A%3B0%3A86%3A%3A%3B0%3A17%3A%3A%3B0%3A104%3A%3A%3B0%3A53%3A%3A%3B0%3A28%3A%3A%3B4%3A44%3A%3A%3B4%3A27%3A%3A&selector%5Bexclude_sports_ids_str%5D=0+32+1+2+3+7+6+5+28+8+9+26+10+11+12+14+27+16+30+13+17+18+29+19+31+20+21+23+22+24+25&selector%5Bextra_filters%5D=&commit=%E8%BF%87%E6%BB%A4&narrow=");
           webBrowser1.ScriptErrorsSuppressed = true;
        }

       
    }
}
