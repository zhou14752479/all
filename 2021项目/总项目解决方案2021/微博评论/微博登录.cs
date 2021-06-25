using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 微博评论
{
    public partial class 微博登录 : Form
    {
        public 微博登录()
        {
            InitializeComponent();
        }

        private void 微博登录_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://weibo.com/?topnav=1&mod=logo");
            //webBrowser1.Navigate("https://m.weibo.cn/profile/5269475300");
        }

        public static string linshiCookie="";
        private void button1_Click(object sender, EventArgs e)
        {
            linshiCookie = method.GetCookies("https://m.weibo.cn/");

            linshiCookie = "_T_WM=93034717987; WEIBOCN_FROM=1110006030; SCF=Alslx_RaEKKXj8jzZBGXOQSLv6Lc3mi-TFwTiE0NdNW1BcyJ-Vc8CF_JeWH8yOOOBRTkj-2oOhQKFX3qMkAo_S8.; SUB=_2A25NzHZHDeRhGeNM7VsV9yvPyzyIHXVvTxoPrDV6PUJbktAKLRf7kW1NThXiEhIRayRD0n6v7OVrGa2uAQDn2Q-C; SUBP=0033WrSXqPxfM725Ws9jqgMF55529P9D9WFAbTDTMuOyH2NWYqT8Dr.s5NHD95Qfeoq4ShMfe057Ws4Dqcji9gSQdcpaUJH4; SSOLoginState=1623721495; MLOGIN=1; M_WEIBOCN_PARAMS=luicode%3D20000174%26uicode%3D20000174; XSRF-TOKEN=65e111";
        }
    }
}
