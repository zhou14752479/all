using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 大球差值监控
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }
        #region  主程序
        public void run()
        {
            try
            {



                foreach (Control c in groupBox1.Controls)
                {
                    if (c is TextBox)
                    {
                        if (c.Text != "")
                        {



                            string url = c.Text;
                            string html = method.GetUrl(url, "gb2312");

                            MatchCollection values = Regex.Matches(html, @"<TD height=22><FONT color=([\s\S]*?)><B>([\s\S]*?)</B>");
                            if (values.Count > 1)
                            {
                                decimal a = Convert.ToDecimal(values[0].Groups[2].Value);
                                decimal b = Convert.ToDecimal(values[1].Groups[2].Value);
                                if (a- b > Convert.ToDecimal(textBox22.Text))
                                {
                                    MessageBox.Show(url + "达到设置差值,差值为" + (a- b));
                                    textBox23.Text += "达到设置差值,差值为"+(a-b)+"网址："+ url+ "\r\n";
                                }


                            }
                            



                        }
                    }
            

                }

            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

            timer1.Interval = Convert.ToInt32(textBox21.Text) * 1000;
            timer1.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
           
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
    }
}
