using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 身份补齐验证
{
    public partial class 号码生成后8 : Form
    {
        public 号码生成后8()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

            }
        }
      


        public void run()
        {
            try
            {

                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            

                textBox2.Text = "";
                for (int i = 0; i < text.Length; i++)
                {

                    if (text[i] != "")
                    {

                        for (int month = 1; month <= 12; month++)
                        {
                           
                            for (int day = 1; day <= 31; day++)
                            {

                                if (textBox2.Text.Length > 1000)
                                    textBox2.Text = "";


                                for (int a = 0; a < 100; a++)
                                {
                                    string month2 = month.ToString();
                                    if (month < 10)
                                    {
                                        month2 = "0" + month;
                                    }
                                    string day2 = day.ToString();
                                    if (day < 10)
                                    {
                                        day2 = "0" + day;
                                    }
                                    string a2 = a.ToString();
                                    if (a< 10)
                                    {
                                        a2 = "0" + a;
                                    }

                                    string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                                    string name = value[0].Trim();
                                    string card = value[1].Trim().Replace("******", "abcdef");
                                    card = card.Replace("abcd", month2 + day2);
                                    card = card.Replace("ef",  a2);
                                    textBox2.Text ="正在生成第"+ (i+1)+"  "+name+"----"+card +"\r\n";


                                    if (身份补齐验证.CheckIDCard18(card) == false)
                                    {

                                        continue;
                                    }

                                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                    sw.WriteLine(name+"----"+ card);
                                    sw.Close();
                                    fs1.Close();
                                    sw.Dispose();
                                }



                            }
                        }










                    }
                }

            }
            catch (Exception ex)
            {

                textBox2.Text = ex.ToString();
            }

        }


        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 号码生成后8_Load(object sender, EventArgs e)
        {

        }
    }
}
