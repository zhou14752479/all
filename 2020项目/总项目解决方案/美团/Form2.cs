using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 美团
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
           
           
        }
        #region GET请求
        public static string meituan_GetUrl(string Url, string COOKIE)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";

                request.Headers.Add("Cookie", COOKIE);
               
                request.Referer = "https://qd.meituan.com/";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }


        #endregion
        private void PanelReSize(object sender, EventArgs e)
        {
            for (int i = 0; i < ActivePanel.Controls.Count; i++)
            {
                ActivePanel.Controls[i].Left = (ActivePanel.Width - ActivePanel.Controls[i].Width) / 2;
            }

        }
        public System.Windows.Forms.Panel ActivePanel = new Panel();
        private void button1_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel3.Name)
                return;
            ActivePanel = panel3;

            button1.SendToBack();
            button1.Dock = DockStyle.Top;

            button2.SendToBack();
            button2.Dock = DockStyle.Bottom;
            button3.SendToBack();
            button3.Dock = DockStyle.Bottom;
            button4.SendToBack();
            button4.Dock = DockStyle.Bottom;

            panel2.SendToBack();
            panel4.SendToBack();
            panel5.SendToBack();
            panel3.BringToFront();
            panel3.Dock = DockStyle.Fill;

            PanelReSize(this, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel2.Name)
                return;
            ActivePanel = panel2;

            button2.Dock = DockStyle.Top;
            button1.SendToBack();
            button1.Dock = DockStyle.Top;

            button3.SendToBack();
            button3.Dock = DockStyle.Bottom;
            button4.SendToBack();
            button4.Dock = DockStyle.Bottom;

            panel3.SendToBack();
            panel4.SendToBack();
            panel5.SendToBack();
            panel2.BringToFront();
            panel2.Dock = DockStyle.Fill;

            PanelReSize(this, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel4.Name)
                return;
            ActivePanel = panel4;

            button3.Dock = DockStyle.Top;
            button2.SendToBack();
            button2.Dock = DockStyle.Top;
            button1.SendToBack();
            button1.Dock = DockStyle.Top;

            button4.Dock = DockStyle.Bottom;

            panel2.SendToBack();
            panel3.SendToBack();
            panel5.SendToBack();
            panel4.BringToFront();
            panel4.Dock = DockStyle.Fill;

            PanelReSize(this, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ActivePanel.Name == panel5.Name)
                return;
            ActivePanel = panel5;

            button4.Dock = DockStyle.Top;
            button3.SendToBack();
            button3.Dock = DockStyle.Top;
            button2.SendToBack();
            button2.Dock = DockStyle.Top;
            button1.SendToBack();
            button1.Dock = DockStyle.Top;

            panel2.SendToBack();
            panel3.SendToBack();
            panel4.SendToBack();
            panel5.BringToFront();
            panel5.Dock = DockStyle.Fill;

            PanelReSize(this, e);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
          
        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //label6.Text = e.Node.Name;
            //label14.Text = e.Node.Text;
            //return;
        }
    }
}
