using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webbrowser获取参数
{
    public partial class Form1 : Form
    {
        public Form1()
        {
          
            InitializeComponent();
        }
    
       

      
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://product.dangdang.com/29312102.html");
           

           
        }

        private void WebBrowser_BeforeNavigate2(object pDisp, ref object URL, ref object Flags,
ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            try
            {
                string postDataText = System.Text.Encoding.ASCII.GetString(PostData as byte[]);
                MessageBox.Show(postDataText);
            }
            catch (Exception)
            {

                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SHDocVw.WebBrowser wb = (SHDocVw.WebBrowser)webBrowser1.ActiveXInstance;
            wb.BeforeNavigate2 += new DWebBrowserEvents2_BeforeNavigate2EventHandler(WebBrowser_BeforeNavigate2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }
    }
}
