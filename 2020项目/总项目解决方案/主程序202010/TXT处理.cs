using System;
using System.Collections;
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

namespace 主程序202010
{
    public partial class TXT处理 : Form
    {
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

        public TXT处理()
        {
            InitializeComponent();
        }

        public ArrayList getFileName()
        {
            ArrayList lists = new ArrayList();

            //string path = AppDomain.CurrentDomain.BaseDirectory;
            string path = textBox1.Text;
            DirectoryInfo folder = new DirectoryInfo(path);
            for (int i = 0; i < folder.GetFiles("*.txt").Count(); i++)
            {
                lists.Add(folder.GetFiles("*.txt")[i].FullName);
            }
            return lists;
        }

        private void button1_Click(object sender, EventArgs e)
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

                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox3.Text = this.openFileDialog1.FileName;
            }
        }


        public string chuli(string txtname)
        {
            string title = Path.GetFileNameWithoutExtension(txtname);
            if (radioButton1.Checked == true)
            {
                return title;
            }
            if (radioButton2.Checked == true)
            {
                string[] text =title.Split(new string[] {textBox2.Text.Trim()}, StringSplitOptions.None);
                return text[0];
            }
            if (radioButton3.Checked == true)
            {
                if (textBox3.Text == "")
                {
                    textBox4.Text = "路径为空";
                    return title;
                }
                ArrayList lists = new ArrayList();
               
               
                string[] FileRead = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (String ReadLine in FileRead)
                {
                    if (title.Contains(ReadLine.Trim()))
                    {
                        lists.Add(ReadLine.Trim());
                    }
                }
                Random rd = new Random();
                

                if (lists.Count > 0)
                {
                    int value = rd.Next(0, lists.Count);
                    
                    return lists[value].ToString();
                }
                else
                {
                    return title;
                }
             
             }
            else
            {
                return title;
            }
        }

        string ReadTxt = "";
        StreamReader keysr = null;
        public void run()
        {
            
                if (radioButton3.Checked == true)
                {
                    keysr = new StreamReader(textBox3.Text, EncodingType.GetTxtType(textBox3.Text));

                    ReadTxt = keysr.ReadToEnd();
                }

                string path = textBox1.Text;
                DirectoryInfo folder = new DirectoryInfo(path);
                int c = folder.GetFiles("*.txt").Count();
                for (int i = 0; i <c; i++)
                {
                try
                {
                    label4.Text ="共"+c +"已处理："+(i+1);
                    string fullname = folder.GetFiles("*.txt")[i].FullName;
                   // textBox4.Text += "正在处理：" + fullname + "\r\n";
                    StringBuilder sb = new StringBuilder();
                    
                    String ReadTxt;
                    
                    StreamReader sr = new StreamReader(fullname,EncodingType.GetTxtType(fullname));

                    ReadTxt = sr.ReadToEnd();


                    //首先判断是否有img
                    if (ReadTxt.Contains("img"))
                    {
                        string[] FileRead = ReadTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        foreach (String ReadLine in FileRead)
                        {
                            string key = chuli(fullname);
                            if (key == "")
                            {
                                key= Path.GetFileNameWithoutExtension(fullname);
                            }
                            string linevalue = ReadLine;
                            if (ReadLine.Contains("img"))
                            {
                               
                                if(checkBox1.Checked==true && !ReadLine.Contains("title"))
                                {
                                    linevalue = linevalue.Replace("web_uri", "title=\"" + key + "\"  web_uri");
                                }
                                if (checkBox2.Checked == true && !ReadLine.Contains("alt"))
                                {
                                    linevalue = linevalue.Replace("web_uri", "alt=\"" + key + "\"  web_uri");
                                }
                               
                                sb.AppendLine(linevalue);

                            }
                            else
                            {
                                sb.AppendLine(ReadLine);
                            }
                           
                        }
                        sr.Close();
                        sr.Dispose();
                        StreamWriter sw = new StreamWriter(fullname);

                        sw.Write(sb.ToString());
                        
                        sw.Close();
                        sw.Dispose();
                        
                    }
                   

                }
               
                  catch (Exception ex)
                {

                    continue;
                }
            }

            MessageBox.Show("处理结束");
            keysr.Close();
            keysr.Dispose();



        }
        Thread thread;
        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void TXT处理_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void TXT处理_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
