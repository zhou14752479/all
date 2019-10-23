using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 常用代码管理软件
{
    public partial class add : Form
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
        public add()
        {
            InitializeComponent();
        }
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion
        private void TextBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

      
       
        private void Button1_Click(object sender, EventArgs e)
        {
            string biaoti = removeValid(textBox1.Text).Trim();

            FileStream fs = new FileStream(path + biaoti + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(textBox1.Text);
            sw.Close();
            fs.Close();

            FileStream fs1 = new FileStream(path + biaoti + "_body.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw1 = new StreamWriter(fs1);
            sw1.WriteLine(richTextBox1.Text);
            sw1.Close();
            fs1.Close();
            DialogResult dr = MessageBox.Show("添加成功是否继续添加？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                textBox1.Text = "";
                richTextBox1.Text = "";

            }
            else
            {
                this.Hide();
            }

        }

        private void RichTextBox1_Enter(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
