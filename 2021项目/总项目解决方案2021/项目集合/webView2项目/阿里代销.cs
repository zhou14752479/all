using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webView2项目
{
    public partial class 阿里代销 : Form
    {
        public 阿里代销()
        {
            InitializeComponent();
        }

        private void 阿里代销_Load(object sender, EventArgs e)
        {
            webView21.Source = new Uri("https://login.1688.com/member/signin.htm");
        }
        int count = 0;
        public void nextUrl()
        {

            if (count < listView1.Items.Count)
            {

                //browser.Load(listView1.Items[count].SubItems[0].Text);
                webView21.Source = new Uri(listView1.Items[count].SubItems[0].Text);
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].BackColor = Color.White;
                }
                listView1.Items[count].BackColor = Color.Red;
                count = count + 1;
            }
        }

        Thread thread;
        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                nextUrl();

            }
        }

        private void 粘贴网址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] text = Clipboard.GetText().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv1 = listView1.Items.Add(text[i]);
            }
        }


       



    }
}
