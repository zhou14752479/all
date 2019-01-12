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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdf_1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

       


        List<string> listPDFs = new List<string>();
        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            //string DeskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //获取桌面路径
            //if (!Directory.Exists(DeskPath + "\\PDF转换结果\\"))
            //{
            //    Directory.CreateDirectory(DeskPath + "\\PDF转换结果\\");


            //}

            //else
            //{
            //    foreach (string listPDF in listPDFs)
            //    {
            //        method.pdfTotxt(new FileInfo(listPDF), new FileInfo(DeskPath + "\\PDF转换结果\\" + Path.GetFileNameWithoutExtension(listPDF) + ".txt"));
            //        string txtpath = DeskPath + "\\PDF转换结果\\" + Path.GetFileNameWithoutExtension(listPDF) + ".txt";
            //        StreamReader streamReader = new StreamReader(txtpath, Encoding.GetEncoding("gb2312"));
            //        string body = streamReader.ReadToEnd();
            //        textBox2.Text = body;

            //        ListViewItem lv1 = listView1.Items.Add(Path.GetFileNameWithoutExtension(listPDF));
            //        lv1.SubItems.Add(body.Length.ToString());

            //        int times= body.Trim().Replace(" ","").Split(new string[] {textBox1.Text}, StringSplitOptions.None).Length - 1;
            //        lv1.SubItems.Add(times.ToString());

            //    }
            //}

            string DeskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //获取桌面路径
            if (!Directory.Exists(DeskPath + "\\PDF转换结果\\"))
            {
                Directory.CreateDirectory(DeskPath + "\\PDF转换结果\\");


            }
            else
            {

                foreach (string listPDF in listPDFs)
                {
                    method.PDFtoTxt(listPDF, new FileInfo(DeskPath + "\\PDF转换结果\\" + Path.GetFileNameWithoutExtension(listPDF) + ".txt"));
                    string txtpath = DeskPath + "\\PDF转换结果\\" + Path.GetFileNameWithoutExtension(listPDF) + ".txt";
                    StreamReader streamReader = new StreamReader(txtpath, Encoding.GetEncoding("gb2312"));
                    string body = streamReader.ReadToEnd();
                    textBox2.Text = body;

                    ListViewItem lv1 = listView1.Items.Add(Path.GetFileNameWithoutExtension(listPDF));
                    lv1.SubItems.Add(body.Length.ToString());

                    int times = body.Trim().Replace(" ", "").Split(new string[] { textBox1.Text }, StringSplitOptions.None).Length - 1;
                    lv1.SubItems.Add(times.ToString());


                }

            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
