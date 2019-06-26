using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_6
{
    public partial class 点击获取链接 : Form
    {
        public 点击获取链接()
        {
            InitializeComponent();
        }



       
     



        private void 点击获取链接_Load(object sender, EventArgs e)
        {
             webBrowser1.Navigate("https://www.taobao.com");
           
            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误


            method.SetIE(0);  //设置浏览器版本为枚举值第一个值
        }

        private void WebBrowser1_NewWindow(object sender, CancelEventArgs e)
        {   
          

            if (checkBox1.Checked == false)

            {
                e.Cancel = true;//防止弹窗；
                string url = this.webBrowser1.StatusText;
                this.webBrowser1.Url = new Uri(url); //打开链接
                textBox1.Text = url;

            }

            else if(checkBox1.Checked == true)
            {
                e.Cancel = true;//防止弹窗；
                string url = this.webBrowser1.StatusText;
                textBox1.Text = url;
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                lv1.SubItems.Add(url);   //比分

                if (listView1.Items.Count > 2)
                {
                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                }
            }

        }
      
         


        //屏蔽IE脚本弹出错误
        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Document.Window.Error += OnWebBrowserDocumentWindowError;

        }
        private void OnWebBrowserDocumentWindowError(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用登录
           
            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion
           
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://"+textBox1.Text);
        }



        //屏蔽IE脚本弹出错误
    }
}
