using org.pdfbox.pdmodel;
using org.pdfbox.util;
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

namespace pdf_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public string geshi { get; set; }

        string DeskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //获取桌面路径
        List<string> listPDFs = new List<string>();
        private object thread;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择PDF文件";
            ofd.InitialDirectory = @"C:\Users\SpringRain\Desktop";
            ofd.Multiselect = true;
            ofd.Filter = "PDF文件|*.pdf|所有文件|*.*";
            ofd.ShowDialog();
            //获得我们在文件夹中选择所有文件的全路径
            string[] path = ofd.FileNames;
            for (int i = 0; i < path.Length; i++)
            {
                //将文件的文件名加载到ListBox中
                listBox1.Items.Add(Path.GetFileName(path[i]));
                //将文件的全路径存储到泛型集合中
                listPDFs.Add(path[i]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                this.geshi = ".docx";
            }
            if (radioButton2.Checked == true)
            {
                this.geshi = ".txt";
            }


           
            if (!Directory.Exists(DeskPath + "\\PDF转换结果\\"))
            {
                Directory.CreateDirectory(DeskPath + "\\PDF转换结果\\");

            }

            for (int i = 0; i < listPDFs.Count; i++)
            {

                method.PDFtoTxt(listPDFs[i], new FileInfo(DeskPath + "\\PDF转换结果\\" + Path.GetFileNameWithoutExtension(listPDFs[i]) + this.geshi));
                //method.pdfTotxt(new FileInfo(listPDFs[i]), new FileInfo(DeskPath + "\\PDF转换结果\\" + Path.GetFileNameWithoutExtension(listPDFs[i]) + this.geshi));

                progressBar1.Maximum = 100;//设置最大长度值
                progressBar1.Minimum = 0;//设置当前值
                progressBar1.Step = (100 / listPDFs.Count);//设置没次增长多少              
                progressBar1.PerformStep();
                label6.Text = progressBar1.Value.ToString() + "%";
                Thread.Sleep(3000);
               
            }


        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string filePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();


            //将文件的文件名加载到ListBox中
            listBox1.Items.Add(Path.GetFileName(filePath));
            //将文件的全路径存储到泛型集合中
            listPDFs.Add(filePath);

        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }
        private Point mPoint = new Point();
        private void SplitContainer1_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void SplitContainer1_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(DeskPath + "\\PDF转换结果\\");
            }
            catch (Exception ex)
            {

                MessageBox.Show("文件夹不存在");
            }
           
        }
    }
}
