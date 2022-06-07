using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202203
{
    public partial class 题目格式 : Form
    {
        public 题目格式()
        {
            InitializeComponent();
        }

        int count = 18;
        private void button1_Click(object sender, EventArgs e)
        {
            count=count+1;  
               StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"question-each\">");

            sb.AppendLine("<!-- 标题 -->");
            sb.AppendLine("<div class=\"question-name\">"+textBox1.Text.Replace(" ", "&nbsp;").Trim()+"</div>");
            sb.AppendLine("<!-- 选项 -->");
            sb.AppendLine("<div class=\"question-option\">");


            string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string A = "";
            string B = "";
            string C = "";
            string D = "";
            foreach (string item in text)
            {
                if(item.Trim()!="")
                {
                    if(A=="")
                    {
                        A = item.Trim().Replace(" ", "&nbsp;");
                    }
                    else if(B == "")
                    {
                        B = item.Trim().Replace(" ", "&nbsp;");
                    }
                    else if (C== "")
                    {
                        C = item.Trim().Replace(" ", "&nbsp;");
                    }
                    else if (D == "")
                    {
                        D= item.Trim().Replace(" ", "&nbsp;");
                    }
                }
            }
            sb.AppendLine("<label><input type=\"radio\" value=\"A\" name=\"single["+count+"]\" required>"+A+"</label>");
            sb.AppendLine("<label><input type=\"radio\" value=\"B\" name=\"single[" + count + "]\" required>" + B + "</label>");
            sb.AppendLine("<label><input type=\"radio\" value=\"C\" name=\"single[" + count + "]\" required>" + C + "</label>");
            sb.AppendLine("<label><input type=\"radio\" value=\"D\" name=\"single[" + count + "]\" required>" + D + "</label>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            richTextBox1.Text+=sb.ToString()+"\r\n";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = textBox1.Text + "<br/>";
        }
    }
}
