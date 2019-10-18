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

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void TextBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        public string getRand()
        {

            int ran = 0;
            byte[] r = new byte[8];
            Random rand = new Random((int)(DateTime.Now.Ticks % 1000000));
            //生成8字节原始数据
            for (int i = 0; i < 8; i++)
                //while循环剔除非字母和数字的随机数
                do
                {
                    //数字范围是ASCII码中字母数字和一些符号
                    ran = rand.Next(48, 122);
                    r[i] = Convert.ToByte(ran);
                } while ((ran >= 58 && ran <= 64) || (ran >= 91 && ran <= 96));
            //转换成8位String类型               
            string randomID = Encoding.ASCII.GetString(r);
            return(randomID);

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            string biaoti = getRand();

            FileStream fs = new FileStream(path + biaoti + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(textBox1.Text);
            sw.Close();
            fs.Close();

            FileStream fs1 = new FileStream(path + biaoti + "_body.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw1 = new StreamWriter(fs1);
            sw1.WriteLine(textBox2.Text);
            sw1.Close();
            fs1.Close();
            DialogResult dr = MessageBox.Show("添加成功是否继续添加？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
               
            }
            else
            {
                this.Hide();
            }

        }
    }
}
