using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202110
{
    public partial class 文本拆分 : Form
    {
        public 文本拆分()
        {
            InitializeComponent();
        }


        #region 获取txt编码
        //调用：EncodingType.GetTxtType(textBox1.Text)
        public class EncodingType
        {
            /// <summary> 
            /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
            /// </summary> 
            /// <param name=“FILE_NAME“>文件路径</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetTxtType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = GetType(fs);
                fs.Close();
                return r;
            }

            /// <summary> 
            /// 通过给定的文件流，判断文件的编码类型 
            /// </summary> 
            /// <param name=“fs“>文件流</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetType(FileStream fs)
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
                Encoding reVal = Encoding.Default;

                BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                r.Close();
                return reVal;

            }

            /// <summary> 
            /// 判断是否是不带 BOM 的 UTF8 格式 
            /// </summary> 
            /// <param name=“data“></param> 
            /// <returns></returns> 
            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
                byte curByte; //当前分析的字节. 
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前 
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1 
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";


            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }



        public void SplitTxt(string file)
        {
            int lineOfEach = Convert.ToInt32(textBox2.Text);   //设置每个文本上限
            int fileIndex = 0;  //起始文件名

           
            var outputFile = Path.GetFileNameWithoutExtension(file) + "_" + fileIndex + ".txt";
            string headerLine = "";
            int lineIndex = 1;

            StreamWriter sw = new StreamWriter(outputFile, false, Encoding.UTF8);
            using (StreamReader sr = new StreamReader(file, EncodingType.GetTxtType(file)))
            {
                headerLine = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    lineIndex++;
                    sw.WriteLine(line);

                    if (lineIndex % lineOfEach == 0)
                    {
                        fileIndex++;
                        outputFile = Path.GetFileNameWithoutExtension(file) + "_" + fileIndex + ".txt";
                        sw.Flush();
                        sw.Close();

                        sw = new StreamWriter(outputFile, false, Encoding.UTF8);
                       
                    }               
                }
               
            }
            sw.Close();
           
        }



        private void 文本拆分_Load(object sender, EventArgs e)
        {

        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先选择文本");
                return;
            }

            SplitTxt(textBox1.Text);
        }


        public ArrayList getFileName()
        {
            ArrayList lists = new ArrayList();

            string path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo folder = new DirectoryInfo(path);
            for (int i = 0; i < folder.GetFiles("*.log").Count(); i++)
            {
                lists.Add(folder.GetFiles("*.log")[i].FullName);
            }
            return lists;
        }

        public void getcuowu()
        {
            try
            {
                ArrayList lists = getFileName();
                for (int i = 0; i < lists.Count; i++)
                {
                    string txtname = lists[i].ToString();

                    StreamReader sr = new StreamReader(txtname, EncodingType.GetTxtType(txtname));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();
                    string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    for (int j = 0; j < text.Length; j++)
                    {

                        if (text[j].Contains("nil"))
                        {
                            string tel = Regex.Match(text[j], @"\d{11}").Groups[0].Value;
                            richTextBox1.AppendText(tel + "\r\n");
                        }

                    }
                    sr.Close();  //只关闭流
                    sr.Dispose();   //销毁流内存

                }

                MessageBox.Show("获取完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getcuowu);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           

        }


        public void quchu()
        {
            try
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("请先选择文本");
                    return;
                }

                string outputFile = path + Path.GetFileNameWithoutExtension(textBox3.Text) + "_new.txt";
                StreamWriter sw = new StreamWriter(outputFile, false, Encoding.UTF8);

                StreamReader sr = new StreamReader(textBox3.Text, EncodingType.GetTxtType(textBox3.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int j = 0; j < text.Length; j++)
                {

                    if (!richTextBox1.Text.Contains(text[j]))
                    {
                        sw.WriteLine(text[j]);
                    }

                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                sw.Close();
                MessageBox.Show("生成成功：路径" + outputFile);
            }
            catch (Exception ex)
            {
MessageBox.Show(ex.ToString());
            }
        }

        Thread thread;
        private void button5_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(quchu);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";


            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox3.Text = openFileDialog1.FileName;



            }
        }
    }
}
