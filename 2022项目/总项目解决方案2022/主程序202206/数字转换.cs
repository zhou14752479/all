using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202206
{
    public partial class 数字转换 : Form
    {
        public 数字转换()
        {
            InitializeComponent();
        }

        public void run()
        {
            string[] text = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int count = 0;
            string value = "";
            for (int i = 0; i <text.Length; i++)
            {
                
                if(text[i]!="")
                {
                    count++;


                    value= value+text[i].Remove(0,1)+" ";


                    if (count == 10)
                    {
                        textBox2.Text += value + "\r\n";
                        count = 0;
                        value = "";
                    }
                }
               
            }
            textBox2.Text += value + "\r\n";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            run();
        }
    }
}
