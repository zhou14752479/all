using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace LEDSimulator
{
  
    public partial class Form1 : Form
    {
        private string m_SrcString;
        private LED m_LED;
        private LED m_LED5;
        private LED m_LED2;
        private LED m_LED3;
        private LED m_LED4;


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
            m_LED3 = new LED();
            m_LED4 = new LED();
            m_LED5 = new LED();
            InitializeComponent();

            m_LED.SetText("城市交通诱导显示屏管理系统",800,80);
            m_LED2.SetText("                   丨    丨\n                   丨    丨\n                   丨    丨\n------------------         ---------------------\n                   南京路\n------------------         ---------------------", 1000,1000);
           m_LED3.SetText("\n  谨慎行驶\n  安全出行", 300,300);
            m_LED4.SetText("风向：西北风\n风速：0.5米/秒\n温度：22℃\n湿度：60%",500,500);
            m_LED5.SetText("滚动文字2",800,80);
        }

        //时钟事件
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 101 - Convert.ToInt32(numericUpDown1.Value);
            m_LED.MoveStep();
            m_LED.GetBitmap(ref LEDPictureBox);


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer5.Interval = 101 - Convert.ToInt32(numericUpDown5.Value);
            m_LED5.MoveStep();
            m_LED5.GetBitmap(ref LEDPictureBox5);
        }

        private void addWorkRecord(string workRecord)//把记录加到文件里
        {
            //在将文本写入文件前，处理文本行
            //StreamWriter第二个参数为false覆盖现有文件，为true则把文本追加到文件末尾
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"log.txt", true))
            {
                string value = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "   " + workRecord.Replace("\n","").Replace("\r", "");
                log.Text +=value+"\r\n";
                file.WriteLine(value);// 直接追加文件末尾
            }
        }
        


        private void Form1_Load(object sender, EventArgs e)
        {
           
            m_LED.GetBitmap(ref LEDPictureBox);
            m_LED5.GetBitmap(ref LEDPictureBox5);
            timer1.Start();
            timer5.Start();

            m_LED2.GetBitmap(ref LEDPictureBox2);
            m_LED3.GetBitmap(ref LEDPictureBox3);
            m_LED4.GetBitmap(ref LEDPictureBox4);

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
                sr.Close();
            }
        }

        private void TextBtn_Click(object sender, EventArgs e)
        {
            if (m_TextTB.Text.Trim() != "")
            {
                m_LED.SetText(m_TextTB.Text.Trim(),800,80);
                addWorkRecord(m_TextTB.Text.Trim());
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void TextBtn2_Click(object sender, EventArgs e)
        {
           
        }

      
        private void TextBtn5_Click(object sender, EventArgs e)
        {
            if (m_TextTB5.Text.Trim() != "")
            {
                m_LED5.SetText(m_TextTB5.Text.Trim(),800,80);
                addWorkRecord(m_TextTB5.Text.Trim());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            m_LED2.SetText("                   丨    丨\n                   丨    丨\n                   丨    丨\n------------------         ---------------------\n                   "+textBox1.Text+"\n------------------         ---------------------", 1000, 1000);
           m_LED2.GetBitmap(ref LEDPictureBox2);
            addWorkRecord(textBox1.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_LED3.SetText(textBox2.Text, 300, 300);
            m_LED3.GetBitmap(ref LEDPictureBox3);
            addWorkRecord(textBox2.Text.Trim());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_LED4.SetText(textBox3.Text, 300, 300);
            m_LED4.GetBitmap(ref LEDPictureBox4);
            addWorkRecord(textBox3.Text.Trim());
          


        }
    }
}