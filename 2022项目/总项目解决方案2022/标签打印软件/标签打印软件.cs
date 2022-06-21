using Spire.Pdf;
using Spire.Pdf.Barcode;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.IO;
using iTextSharp.text.pdf;

namespace 标签打印软件
{
    public partial class 标签打印软件 : Form
    {
        public 标签打印软件()
        {
            InitializeComponent();
        }

        public void addtxt(float x,float y,float size, FontStyle fontstyle,string txt)
        {

            foreach (PdfPageBase page in pdf.Pages)
            {
               
                PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Arial",  size,fontstyle ), true);  //定义font

                //绘制文本“Codabar:”到PDF
                PdfTextWidget text = new PdfTextWidget();
                text.Font = font;
                text.Text = txt;
                PdfLayoutResult result = text.Draw(page, x, y);  //在指定位置绘制文本
            }
           
            
        }



        public void addimg(float x,float y,int barheight,string value)
        {
            try
            {
                foreach (PdfPageBase page in pdf.Pages)
                {
                    //绘制Codabar条形码到PDF
                    //PdfCodabarBarcode codabar = new PdfCodabarBarcode(value);  //初始化PdfCodabarBarcode类的实例

                    //绘制Code 39条形码到PDF
                    PdfCode39Barcode codabar = new PdfCode39Barcode(value);
                    codabar.BarcodeToTextGapHeight = 1f;
                   
                    //codabar.EnableCheckDigit = false;
                    //codabar.ShowCheckDigit = false;
                   
                    if(x==100)
                    {
                        codabar.TextDisplayLocation = TextLocation.Bottom;
                        codabar.NarrowBarWidth=2;
                    }
                    else
                    {
                        codabar.TextDisplayLocation = TextLocation.None; ;
                    }
                   

                    codabar.TextColor = Color.Green;
                    codabar.BarHeight = barheight;

                    codabar.Draw(page, new PointF(x, y));  //在指定位置绘制条形码
                }
            }
            catch (Exception ex)
            {

               MessageBox.Show("条形码数值有误");
            }
        }




        Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
        private void button1_Click(object sender, EventArgs e)
        {
        
            pdf.LoadFromFile("sample.pdf");

         
            addtxt(450,177,20f, FontStyle.Regular,textBox1.Text.Trim());
            addtxt(50, 228, 50f, FontStyle.Bold, textBox2.Text.Trim());

            //左侧竖排1-3
            addtxt(230, 473, 38f, FontStyle.Regular, textBox5.Text.Trim());
            addtxt(230, 558, 38f, FontStyle.Regular, textBox6.Text.Trim());
            addtxt(230, 644, 38f, FontStyle.Regular, textBox7.Text.Trim());


            //右侧竖一排1-5
            addtxt(440, 318, 38f, FontStyle.Regular, textBox3.Text.Trim());
            addtxt(440, 393, 38f, FontStyle.Regular, textBox4.Text.Trim());
            addtxt(440, 473, 38f, FontStyle.Regular, textBox8.Text.Trim());
            addtxt(440, 558, 38f, FontStyle.Regular, textBox9.Text.Trim());
            addtxt(440, 630, 55f, FontStyle.Bold, textBox10.Text.Trim());

            //右侧竖二排1-3
            addtxt(630, 473, 38f, FontStyle.Regular, textBox11.Text.Trim());
            addtxt(630, 558, 38f, FontStyle.Regular, textBox12.Text.Trim());

            //最后一行文字
            addtxt(150, 928, 70f, FontStyle.Bold, textBox13.Text.Trim());






            addimg(50,292,60, textBox3.Text.Trim());
            addimg(50, 371,60, textBox4.Text.Trim());
            addimg(50, 448,60, textBox5.Text.Trim());
            addimg(50, 528,60, textBox6.Text.Trim());
            addimg(50, 611,60, textBox7.Text.Trim());

            addimg(100, 798, 100, textBox13.Text.Trim());



            pdf.SaveToFile("output1.pdf");



            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
            lv1.SubItems.Add(textBox1.Text.Trim()) ;
            lv1.SubItems.Add(textBox2.Text.Trim());
            lv1.SubItems.Add(textBox3.Text.Trim());
            lv1.SubItems.Add(textBox4.Text.Trim());
            lv1.SubItems.Add(textBox5.Text.Trim());
            lv1.SubItems.Add(textBox6.Text.Trim());
            lv1.SubItems.Add(textBox7.Text.Trim());
            lv1.SubItems.Add(textBox8.Text.Trim());
            lv1.SubItems.Add(textBox9.Text.Trim());
            lv1.SubItems.Add(textBox10.Text.Trim());
            lv1.SubItems.Add(textBox11.Text.Trim());
            lv1.SubItems.Add(textBox12.Text.Trim());
            lv1.SubItems.Add(textBox13.Text.Trim());


            ClearPdfwenzi("output1.pdf", "output.pdf");
            System.Diagnostics.Process.Start("output.pdf");
            //pdf.Print();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        #region 清除PDF文字水印NUGETusing iTextSharp.text.pdf;
        private void ClearPdfwenzi(string sourcePdf, string outputPdf)
        {
            try
            {
                PRStream stream;
                String content;
                PdfArray contentarray;
                string watermarkText = "Evaluation Warning : The document was created with Spire.PDF for .NET.";
              
                PdfReader reader2 = new PdfReader(sourcePdf);
                reader2.RemoveUnusedObjects();
                PdfDictionary page = reader2.GetPageN(1);//获取第一页
                contentarray = page.GetAsArray(PdfName.CONTENTS);
                if (contentarray != null)
                {
                    //Loop through content
                    for (int j = 0; j < contentarray.Size; j++)
                    {
                        //Get the raw byte stream
                        stream = (PRStream)contentarray.GetAsStream(j);
                        //Convert to a string. NOTE, you might need a different encoding here
                        content = System.Text.Encoding.ASCII.GetString(PdfReader.GetStreamBytes(stream));//获取pdf页内的文字内容
                                                                                                         //Look for the OCG token in the stream as well as our watermarked text
                        if (content.IndexOf("/OC") >= 0 || content.IndexOf(watermarkText) >= 0)//如果pdf内容包含水印文字
                        {
                            //Remove it by giving it zero length and zero data
                            content = content.Replace(watermarkText, "");//替换水印文字为空
                            byte[] byteArray = System.Text.Encoding.Default.GetBytes(content);//转换为byte[]
                            stream.Put(PdfName.LENGTH, new PdfNumber(byteArray.Length));//重新指定大小

                            stream.SetData(byteArray);//重新赋值
                        }
                    }
                }
                FileStream fs = new FileStream(outputPdf, FileMode.Create, FileAccess.Write, FileShare.None);
                PdfStamper stamper = new PdfStamper(reader2, fs);
                //stamper.SetFullCompression();
                if (stamper != null)
                {
                    stamper.Close();
                }

                if (null != fs)
                {
                    fs.Close();
                }

                if (null != reader2)
                {
                    reader2.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
          // ClearPdfwenzi(@"C:\Users\Administrator\Desktop\output测试_加水印.pdf", @"C:\Users\Administrator\Desktop\output.pdf");
            listView1.Items.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = "123";
            textBox2.Text = "123";
            textBox3.Text = "123";
            textBox4.Text = "123";
            textBox5.Text = "123";
            textBox6.Text = "123";
            textBox7.Text = "123";
            textBox8.Text = "123";
            textBox9.Text = "123";
            textBox10.Text = "123";
            textBox11.Text = "123";
            textBox12.Text = "123";
            textBox13.Text = "123";
        }
    }
}
