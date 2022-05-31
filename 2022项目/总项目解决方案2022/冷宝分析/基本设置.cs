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

namespace 冷宝分析
{
    public partial class 基本设置 : Form
    {
        public 基本设置()
        {
            InitializeComponent();
        }

        private void 基本设置_Load(object sender, EventArgs e)
        {
            textBox1.Text = function.softname;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs1 = new FileStream(function.path + "//info.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(textBox1.Text.Trim());
            sw.Close();
            fs1.Close();
            sw.Dispose();
            function.softname=textBox1.Text;
        }

      

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          if(comboBox1.Text=="黄色")
            {
                function.colorname = "Info";
            }
            if (comboBox1.Text == "红色")
            {
                function.colorname = "Coral";
            }
            if (comboBox1.Text == "蓝色")
            {
                function.colorname = "ActiveCaption";
            }


            FileStream fs1 = new FileStream(function.path + "//color.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(function.colorname);
                sw.Close();
                fs1.Close();
                sw.Dispose();

            
        }
    }
}
