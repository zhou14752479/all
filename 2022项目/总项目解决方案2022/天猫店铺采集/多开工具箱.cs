using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 天猫店铺采集
{
    public partial class 多开工具箱 : Form
    {
        public 多开工具箱()
        {
            InitializeComponent();
        }

        private void 多开工具箱_Load(object sender, EventArgs e)
        {
            //textBox2.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        int x = 0;
        int y = 0;


        public void run()
        {
            天猫店铺采集 duo = new 天猫店铺采集();
            duo.Show();
            x = x + 10;
            y = y + 10;
            duo.StartPosition = FormStartPosition.Manual;
            duo.Location = (Point)new Size(x, y);
           



        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {


            for (int i = 0; i <numericUpDown1.Value; i++)
            {
                string appName = "天猫店铺采集1.exe";
                Process.Start(appName);
                Thread.Sleep(1000);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                
                MessageBox.Show("请导入文本");
                return;
            }


            int duokaicount = Convert.ToInt32(numericUpDown1.Value);
            int count = Convert.ToInt32(numericUpDown2.Value);
            

            int linshi = 0;
            int name = 1;
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            for (int a = 0; a < text.Length; a++)
            {
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + name+".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(text[a]);
                sw.Close();
                fs1.Close();
                sw.Dispose();

                linshi = linshi + 1;
                if(linshi>=count)
                {
                    name = name + 1;
                    linshi = 0;
                   
                }

               
                if (name > duokaicount)
                {
                    MessageBox.Show("生成完成");
                    return;
                }
                MessageBox.Show("生成完成");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process current=Process.GetCurrentProcess();    

            Process[] ps = Process.GetProcessesByName("天猫店铺采集");
            foreach (Process item in ps)
            {
                if(item.Id!=current.Id)
                {
                    item.Kill();
                }
               
            }



            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "duokai.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine("1");
            sw.Close();
            fs1.Close();
            sw.Dispose();
        }
    }
}
