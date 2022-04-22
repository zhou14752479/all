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

namespace 抖音直播
{
    public partial class AI工具 : Form
    {
        public AI工具()
        {
            InitializeComponent();
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
           
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getAnswer);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            
        }

        public void getAnswer()
        {
          
            string ask= "小A结婚早好吗？有过来人说一下吗？";
            for (int i = 0; i <= ask.Length; i++)
            {
               textBox1.Text = ask.Substring(0, i);
                Thread.Sleep(100);
            }

            richTextBox1.Text = "开始大数据筛查分析中......";
            Thread.Sleep(1000);
            richTextBox1.Text = "";

           string answer = "遇良人先成家，遇贵人先立业.....(⓿_⓿)未遇良人未遇贵人，先自强！";
            for (int i = 0; i < answer.Length; i++)
            {
                //richTextBox1.Text = answer.Substring(0,i);
                richTextBox1.AppendText(answer.Substring(i, 1));
                Thread.Sleep(100);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

            //richTextBox1.SelectionStart = richTextBox1.Text.Length;
            //richTextBox1.SelectionLength = 0;
            //richTextBox1.ScrollToCaret();
            if(richTextBox1.Text.Length>500)
            {
                richTextBox1.Text = "";
            }


        }

        private void AI工具_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(getAnswer);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }
    }
}
