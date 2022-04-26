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

namespace 主程序202203
{
    public partial class 数字处理 : Form
    {
        public 数字处理()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                if(richTextBox1.Lines[i] != "")
                {
                    string value= richTextBox1.Lines[i];
                    MatchCollection  shuzis= Regex.Matches(richTextBox1.Lines[i], @"\d{6,}");
                    MatchCollection shuzis2 = Regex.Matches(richTextBox1.Lines[i], @"\d{6}");
                    for (int j= 0; j< shuzis.Count; j++)
                    {
                        value = value.Replace(shuzis[j].Groups[0].Value, shuzis2[j].Groups[0].Value); 
                    }
                    richTextBox2.Text +=value+"\r\n";
                }
               
            }
            MessageBox.Show("完成");
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }
    }
}
