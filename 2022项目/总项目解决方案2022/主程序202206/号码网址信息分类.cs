using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202206
{
    public partial class 号码网址信息分类 : Form
    {
        public 号码网址信息分类()
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

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈


                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        #region 导出文本
        public static void expotTxt(ListView lv1, int i)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "txt文件导出";
            sfd.Filter = "txt|*.txt";
            string path = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName;

                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem item in lv1.Items)
                {
                    try
                    {
                        List<string> list = new List<string>();

                        string temp1 = item.SubItems[i].Text+"----"+ item.SubItems[i+1].Text+"----"+ item.SubItems[i+3].Text;

                        list.Add(temp1);
                        foreach (string tel in list)
                        {
                            sb.AppendLine(tel);
                        }




                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("导出完成");
            }

        }
        #endregion
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.listView1.SelectedItems.Count == 0)
                    return;
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch (Exception)
            {
                MessageBox.Show("异常");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    string[] values= text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                   if(values.Length>1)
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(values[0]);
                        lv1.SubItems.Add(values[1]);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                    }
                   
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        public string getdata(string url)
        {
            string html = GetUrl(url,"utf-8");
            return html;
        }
        private void 添加备注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            string str = Interaction.InputBox("提示信息", "标题", "文本内容", -1, -1);
            listView1.SelectedItems[0].SubItems[4].Text = str;

        }

        private void 刷新获取信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            listView1.SelectedItems[0].SubItems[3].Text = getdata(listView1.SelectedItems[0].SubItems[2].Text);
        }

        private void 移除到单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            export("单", listView1.SelectedItems[0].SubItems[1].Text+"----"+ listView1.SelectedItems[0].SubItems[2].Text+"----"+ listView1.SelectedItems[0].SubItems[4].Text);
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }




        public void export(string name,string value)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\"+name+".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(value);
            sw.Close();
            fs1.Close();
            sw.Dispose();
        }

        private void 移除到废ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            export("废", listView1.SelectedItems[0].SubItems[1].Text + "----" + listView1.SelectedItems[0].SubItems[2].Text + "----" + listView1.SelectedItems[0].SubItems[4].Text);
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void 移除到限制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            export("限制", listView1.SelectedItems[0].SubItems[1].Text + "----" + listView1.SelectedItems[0].SubItems[2].Text + "----" + listView1.SelectedItems[0].SubItems[4].Text);
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void 号码网址信息分类_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"2piFF"))
            {
                //TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            string str = Interaction.InputBox("提示信息", "标题", "文本内容", -1, -1);
            listView1.SelectedItems[0].SubItems[4].Text = str;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            listView1.SelectedItems[0].SubItems[3].Text = getdata(listView1.SelectedItems[0].SubItems[2].Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            export("单", listView1.SelectedItems[0].SubItems[1].Text + "----" + listView1.SelectedItems[0].SubItems[2].Text + "----" + listView1.SelectedItems[0].SubItems[4].Text);
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            export("废", listView1.SelectedItems[0].SubItems[1].Text + "----" + listView1.SelectedItems[0].SubItems[2].Text + "----" + listView1.SelectedItems[0].SubItems[4].Text);
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            export("限制", listView1.SelectedItems[0].SubItems[1].Text + "----" + listView1.SelectedItems[0].SubItems[2].Text + "----" + listView1.SelectedItems[0].SubItems[4].Text);
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            expotTxt(listView1,1);
        }
    }
}
