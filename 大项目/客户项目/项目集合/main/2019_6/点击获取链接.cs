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
             webBrowser1.Navigate("http://mybrowse.osfipin.com/");
           
            this.webBrowser1.ScriptErrorsSuppressed = true;  //屏蔽IE脚本弹出错误
            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;  //屏蔽IE脚本弹出错误


        }

        private void WebBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            MessageBox.Show(this.webBrowser1.StatusText);

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
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

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

        private void Button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }



        //屏蔽IE脚本弹出错误
    }
}
