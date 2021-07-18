using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LEDSimulator
{
   //该源码下载自C#编程网|www.cpbcw.com
    public partial class Form1 : Form
    {
        private string m_SrcString;
        private LED m_LED;
        private LED m_LED2;
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
        public Form1()
        {     
            m_LED = new LED();
            m_LED2 = new LED();
            InitializeComponent();

            m_LED.SetText("城市交通诱导显示屏管理系统");
            m_LED2.SetText("滚动文字2");
        }

        //时钟事件
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval =101-Convert.ToInt32(numericUpDown1.Value);
            m_LED.MoveStep();           
            m_LED.GetBitmap(ref LEDPictureBox);

         
        }


        private void addWorkRecord(string workRecord)//把记录加到文件里
        {
            //在将文本写入文件前，处理文本行
            //StreamWriter第二个参数为false覆盖现有文件，为true则把文本追加到文件末尾
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"log.txt", true))
            {
                log.Text += workRecord+"\r\n";
                file.WriteLine(workRecord);// 直接追加文件末尾
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
           

            m_LED.GetBitmap(ref LEDPictureBox);
            m_LED2.GetBitmap(ref LEDPictureBox2);
            timer1.Start();
            timer2.Start();
            string path = AppDomain.CurrentDomain.BaseDirectory+"log.txt";

            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, EncodingType.GetTxtType(path));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    log.Text += text[i] + "\r\n";
                }
            }
        }

        private void TextBtn_Click(object sender, EventArgs e)
        {
            if (m_TextTB.Text.Trim() != "")
            {
                m_LED.SetText(m_TextTB.Text.Trim());
                addWorkRecord(m_TextTB.Text.Trim());
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void TextBtn2_Click(object sender, EventArgs e)
        {
            if (m_TextTB2.Text.Trim() != "")
            {
                m_LED2.SetText(m_TextTB2.Text.Trim());
                addWorkRecord(m_TextTB2.Text.Trim());
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = 101-Convert.ToInt32(numericUpDown2.Value);
            m_LED2.MoveStep();
            m_LED2.GetBitmap(ref LEDPictureBox2);
        }
    }
}