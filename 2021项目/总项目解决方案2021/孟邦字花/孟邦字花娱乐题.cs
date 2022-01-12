using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 孟邦字花
{
    public partial class 孟邦字花娱乐题 : Form
    {
        public 孟邦字花娱乐题()
        {
            InitializeComponent();
        }

        private void 孟邦字花娱乐题_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            SetFeatures(11000);
           
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

        #region 修改注册表信息使WebBrowser使用指定版本IE内核 传入11000是IE11
        public static void SetFeatures(UInt32 ieMode)
        {
            //传入11000是IE11, 9000是IE9, 只不过当试着传入6000时, 理应是IE6, 可实际却是Edge, 这时进一步测试, 当传入除IE现有版本以外的一些数值时WebBrowser都使用Edge内核
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                throw new ApplicationException();
            }
            //获取程序及名称
            string appName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            //设置浏览器对应用程序(appName)以什么模式(ieMode)运行
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
            //不晓得设置有什么用
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
        }
        #endregion
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
             
            }
        }

        public string getshici()
        {
            try
            {
                Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同

                StreamReader sr = new StreamReader(Application.StartupPath + @"\data\shici.txt", EncodingType.GetTxtType(Application.StartupPath + @"\data\shici.txt"));
                //一次性读取完 
                string html = sr.ReadToEnd();
                string[] text = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int suiji = rd.Next(1, text.Length - 2);
                sr.Close();
                return text[suiji];
            }
            catch (Exception ex)
            {

               return "明日登峰须造极，渺观宇宙我心宽。";
            }
           
        }
        public string getchengyu()
        {
            try
            {
                Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同

                StreamReader sr = new StreamReader(Application.StartupPath + @"\data\chengyu.txt", EncodingType.GetTxtType(Application.StartupPath + @"\data\chengyu.txt"));
                //一次性读取完 
                string html = sr.ReadToEnd();
                string[] text = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int suiji = rd.Next(1, text.Length - 2);
                sr.Close();
                return text[suiji];
            }
            catch (Exception ex)
            {

                return "一往无前";
            }

        }
        public void run()
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {

                    }
                }

                StreamReader sr = new StreamReader(Application.StartupPath + @"\index\index.html", EncodingType.GetTxtType(Application.StartupPath + @"\index\index.html"));
                //一次性读取完 
                string body = sr.ReadToEnd();

                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                System.IO.File.WriteAllText(Application.StartupPath + @"\index\index.html", body, Encoding.UTF8);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
          
            MessageBox.Show(getshici());
            webBrowser1.Refresh();
        }
    }
}
