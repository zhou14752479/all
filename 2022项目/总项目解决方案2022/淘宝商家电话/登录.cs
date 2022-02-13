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

namespace 淘宝商家电话
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        //声明传递
        public delegate void SetCookie(string zhanghao, string cookie);
        public SetCookie setcookie;


        public string getwangwang()
        {
            string url = "https://top-tmm.taobao.com/login_api.do?0.4599125148430925";
            string html = method.GetUrlWithCookie(url,cookie,"utf-8");
            string wangwang = Regex.Match(html, @"dnk:'([\s\S]*?)'").Groups[1].Value;
            return wangwang;
        }


        public static string cookie = "";
        private void button1_Click(object sender, EventArgs e)
        {

           
            cookie = method.GetCookies("https://item.manager.taobao.com/taobao/manager/render.htm?pagination.current=1&pagination.pageSize=20&tab=in_stock&table.sort.endDate_m=desc");
            //传递信号

            string wangwang = getwangwang();
            if(wangwang=="")
            {
                MessageBox.Show("自动获取账号昵称失败，请手动填写");
               if(textBox1.Text!="")
                {
                    wangwang = textBox1.Text;
                    setcookie(wangwang, cookie);
                    this.Hide();
                }

            }
            else
            {
                
                setcookie(wangwang, cookie);
                this.Hide();
            }
           
        }
        public void clearIeCookie()
        {
            //发送验证码后 清理cookie

            HtmlDocument document = webBrowser1.Document;
            document.ExecCommand("ClearAuthenticationCache", false, null);
            webBrowser1.Refresh();
        }
        private void 登录_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            
            webBrowser1.Navigate("https://item.manager.taobao.com/taobao/manager/render.htm?tab=in_stock&table.sort.endDate_m=desc&spm=a217wi.openworkbeanchtmall");
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearIeCookie();
            webBrowser1.Navigate("https://item.manager.taobao.com/taobao/manager/render.htm?tab=in_stock&table.sort.endDate_m=desc&spm=a217wi.openworkbeanchtmall");
        }
    }
}
