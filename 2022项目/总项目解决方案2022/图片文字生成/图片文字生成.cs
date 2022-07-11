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

namespace 图片文字生成
{
    public partial class 图片文字生成 : Form
    {
        public 图片文字生成()
        {
            InitializeComponent();
        }
        
       public string path = AppDomain.CurrentDomain.BaseDirectory;
        
        private void AddTextToImg(string texts)
        {
            //判断指定图片是否存在
            if (!File.Exists(path+"/原图.png"))
            {
                throw new FileNotFoundException("The file don't exist!");
            }
            


            string[] text = texts.Split(new string[] { "," }, StringSplitOptions.None);

            Image image = Image.FromFile(path + "/原图.png");
            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //字体大小
            float fontSize = 10.15f;
            //文本的长度
            float textWidth = text.Length * fontSize;
         
        
            System.Drawing.Font font = new System.Drawing.Font("黑体", fontSize, System.Drawing.FontStyle.Regular);
            //白笔刷，画文字用
            Brush Brush = new SolidBrush(System.Drawing.Color.Black);
          
            g.DrawString(text[0], font,Brush,845,106);
            g.DrawString(text[1], font, Brush, 232, 160);

            g.DrawString(text[2], font, Brush, 388, 190);
            g.DrawString(text[3], font, Brush, 593, 190);
            g.DrawString(text[4], font, Brush, 906, 190);

            g.DrawString(text[5], font, Brush, 117, 261);
            g.DrawString(text[6], font, Brush, 276, 261);
            g.DrawString(text[7], font, Brush, 354, 261);
            g.DrawString(text[8], font, Brush, 432, 261);
            g.DrawString(text[9], font, Brush, 514, 261);
            g.DrawString(text[10], font, Brush, 602, 261);
            g.DrawString(text[11], font, Brush, 812, 261);


            g.DrawString(text[12], font, Brush, 145, 416);
            g.DrawString(text[13], font, Brush, 352, 416);
            g.DrawString(text[14], font, Brush, 611, 416);


            g.DrawString(text[15], font, Brush, 848, 446);
            g.DrawString(text[16], font, Brush, 208, 497);
            g.DrawString(text[17], font, Brush, 224, 586);
            g.DrawString(text[18], font, Brush, 219, 667);
            g.DrawString(text[19], font, Brush, 219, 707);



            string imgpath = textBox2.Text + text[0] + ".jpg";
            bitmap.Save(imgpath, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            bitmap.Dispose();
            image.Dispose();

           
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion

        private void 图片文字生成_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Poa6"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion

            textBox2.Text = path + "images\\";
        }


        DataTable dt;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("表格为空");
                return;
            }
            
            status = true;

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }



        public void run()
        {
            DataTable dt = method.ExcelToDataTable(textBox1.Text, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                   StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < 20; j++)
                    {
                        sb.Append(dt.Rows[i][j].ToString().Trim()+",");
                    }

                    AddTextToImg(sb.ToString());
                    textBox3.Text = dt.Rows[i][0].ToString()+"生成成功";
                }
                catch (Exception ex)
                {
                    textBox3.Text = ex.ToString();
                }
               
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, true);

            }
        }

        bool status = true;

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
