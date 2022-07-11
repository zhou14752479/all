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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using myDLL;
using Spire.Doc;
using Spire.Pdf;
using Spire.Presentation;
using Spire.Xls;

namespace 文件关键字检索
{
    public partial class 文件关键字检索 : Form
    {
        public 文件关键字检索()
        {
            InitializeComponent();
        }


        public void Director(string dir)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(dir);
                FileInfo[] files = d.GetFiles();//文件
                DirectoryInfo[] directs = d.GetDirectories();//文件夹
                foreach (FileInfo f in files)
                {
                    ListViewItem item2 = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), dir + "\\" + f.Name, System.IO.Path.GetExtension(f.Name), "","" }));

                }

            }
            catch (Exception)
            {

                MessageBox.Show("请拖入文件夹");
            }

        }

        List<string> list=new List<string>();   
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            list.Clear();

            StreamReader sr2 = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\lastkeypath.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\lastkeypath.txt"));
            //一次性读取完 
            string lastpath = sr2.ReadToEnd();
           
            sr2.Close();  //只关闭流
            sr2.Dispose();   //销毁流内存

            if(lastpath!="")
            {
                openFileDialog1.InitialDirectory = lastpath;
            }
           
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                   if(text[i]!="")
                    {
                        list.Add(text[i]);  
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }



            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\lastkeypath.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(System.IO.Path.GetDirectoryName(openFileDialog1.FileName));
            sw.Close();
            fs1.Close();
            sw.Dispose();
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
        private void 文件关键字检索_Load(object sender, EventArgs e)
        {
            #region 通用检测

            //string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            //if (!html.Contains(@"DPuR"))
            //{
            //    TestForKillMyself();
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //}

            if (DateTime.Now>Convert.ToDateTime("2022-08-08"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            #endregion
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                Director(dialog.SelectedPath); 
            }
            
        }

        private void 选择文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
           // openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                ListViewItem item2 = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), openFileDialog1.FileName, System.IO.Path.GetExtension(openFileDialog1.FileName), "", "" }));



            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();    
        }





        string path = AppDomain.CurrentDomain.BaseDirectory;

        #region PPT

        public string getppt(string file)
        {

            //实例化一个Presentation对象
            Presentation ppt = new Presentation();

            //加载PPT文档
            ppt.LoadFromFile(file, Spire.Presentation.FileFormat.Pptx2010);

            //实例化一个StringBuilder 对象
            StringBuilder sb = new StringBuilder();

            //提取PPT所有页面的文本
            for (int i = 0; i < ppt.Slides.Count; i++)
            {
                for (int j = 0; j < ppt.Slides[i].Shapes.Count; j++)
                {
                    if (ppt.Slides[i].Shapes[j] is IAutoShape)
                    {
                        IAutoShape shape = ppt.Slides[i].Shapes[j] as IAutoShape;
                        if (shape.TextFrame != null)
                        {
                            foreach (TextParagraph tp in shape.TextFrame.Paragraphs)
                            {
                                sb.Append(tp.Text + Environment.NewLine);
                            }
                        }

                    }
                }
            }

            //将提取到的文本写为.txt格式并保存到本地路径
            // File.WriteAllText("获取文本pptx.txt", sb.ToString());
            return sb.ToString();

        }


        #endregion

        #region DOC
        public string getdoc(string file)
        {
            Document doc = new Document();
            doc.LoadFromFile(file);

            //使用GetText方法获取文档中的所有文本
            string sb = doc.GetText();
            return sb.ToString();
            //File.WriteAllText("文本doc.txt", sb.ToString());
        }
        #endregion

      
        #region xlsx
        public string getxlsx(string file)
        {
            //创建 Workbook 类的实例
            Workbook workbook = new Workbook();
            //加载Excel文件
            workbook.LoadFromFile(file);

            //获取第一张工作表
            Worksheet sheet = workbook.Worksheets[0];

            //将工作表保存为CSV
            sheet.SaveToFile("excel.txt", ",", Encoding.UTF8);

            StreamReader sr = new StreamReader("excel.txt", method.EncodingType.GetTxtType("excel.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
           File.Delete("excel.txt");
            return texts;
           
        }

        #endregion

        #region pdf
        public string getpdf(string file)
        {
            //实例化一个PdfDocument对象
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();

            //加载PDF文档
            doc.LoadFromFile(file);

            //实例化一个StringBuilder 对象
            StringBuilder content = new StringBuilder();

            //提取PDF所有页面的文本
            foreach (PdfPageBase page in doc.Pages)
            {
                content.Append(page.ExtractText());
            }

            return content.ToString();
        }

        #endregion


        #region 使用ITextSharp读取pdf的文本内容
        public string readpdf(string fileName)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    //currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.GetEncoding("utf-8"), Encoding.UTF8.GetBytes(currentText)));
                    text.Append(currentText);
                   
                }
                pdfReader.Close();
            }

           // richTextBox1.Text=text.ToString();
            return text.ToString();
        }

        #endregion


        public void run()
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string filename = listView1.Items[i].SubItems[1].Text.ToString();
                string gs=listView1.Items[i].SubItems[2].Text.ToString().Trim();

                string body = "";

                if (gs==".doc" || gs ==".docx")
                {
                     body = getdoc(filename);
                    
                }

                else if (gs == ".ppt" || gs == ".pptx")
                {
                     body = getppt(filename);


                }


                else if (gs == ".xls" || gs == ".xlsx")
                {
                   
                    body = getxlsx(filename);
                   
                }
                else if (gs == ".pdf")
                {

                    // body = getpdf(filename);
                    body = readpdf(filename);

                }

                else if (gs == ".wps")
                {
                    File.Copy(filename, filename.Replace(".wps",".docx"));  
                    body = getdoc(filename.Replace(".wps", ".docx"));
                    File.Delete(filename.Replace(".wps", ".docx"));  
                }
                else if (gs == ".et")
                {
                    File.Copy(filename, filename.Replace(".et", ".xls"));
                    body = getxlsx(filename.Replace(".et", ".xls"));
                     File.Delete(filename.Replace(".et", ".xls"));
                }

                else if (gs == ".dps")
                {
                    File.Copy(filename, filename.Replace(".dps", ".pptx"));
                    body = getppt(filename.Replace(".dps", ".pptx"));
                    File.Delete(filename.Replace(".dps", ".pptx"));
                }

                else
                {
                    listView1.Items[i].SubItems[3].Text = "格式不符";
                    listView1.Items[i].SubItems[4].Text = "格式不符";
                    continue;
                }
               
                string shifou = "否";

                StringBuilder sb = new StringBuilder(); 

                foreach (string item in list)
                {
                    if(body.Contains(item))
                    {
                        shifou = "是";
                        sb.Append(item+",");
                    }

                    //判断文件名是否包含关键词
                   else if (filename.Contains(item))
                    {
                        shifou = "是";
                        sb.Append(item + ",");
                    }
                }


               

                listView1.Items[i].SubItems[3].Text = shifou;
                listView1.Items[i].SubItems[4].Text = sb.ToString();

            

            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;

        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            Array filePath = ((Array)e.Data.GetData(DataFormats.FileDrop));

            foreach (var item in filePath)
            {
                ListViewItem item2 = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), item.ToString(), System.IO.Path.GetExtension(item.ToString()), "", "" }));

            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(run);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 文件关键字检索_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
