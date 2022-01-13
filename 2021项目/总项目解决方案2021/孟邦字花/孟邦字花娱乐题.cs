using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
                if(!text[suiji].Contains("，"))
                {
                    suiji = rd.Next(1, text.Length - 2);
                }
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

        public string gettishi()
        {
            try
            {
                Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同

                StreamReader sr = new StreamReader(Application.StartupPath + @"\data\tishi.txt", EncodingType.GetTxtType(Application.StartupPath + @"\data\tishi.txt"));
                //一次性读取完 
                string html = sr.ReadToEnd();
                string[] text = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int suiji = rd.Next(1, text.Length - 2);
                sr.Close();
                return text[suiji].Replace("，","").Replace("。", "").Replace("？", "");
            }
            catch (Exception ex)
            {

                return "艰难久自知";
            }

        }
        string[] sx = { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        int sxvalue = 0;
        int sxvalue2 = 0;
        public void run()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                StringBuilder sb2 = new StringBuilder();
                for (int i = 0; i < 7; i++)
                {
                    sxvalue = sxvalue + 1;
                    if (sxvalue>11)
                    {
                        sxvalue = 0;
                    }
                   
                    string shengxiao = sx[sxvalue];
                    sb.AppendLine("<tr><th colspan=6 cellspacing=0>"+DateTime.Now.AddDays(i).ToString("yyyy年MM月dd")+"属"+shengxiao+"</th></tr>");
                    sb.AppendLine("<tr>");
                    for (int j = 0; j < 3; j++)
                    {
                        string chengyu = getchengyu();
                       
                        string shici = getshici();
                        string[] text = shici.Split(new string[] { "，" }, StringSplitOptions.None);
                        char[] chars= shici.ToArray();

                        sb.AppendLine("<td class=\"you\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + chengyu + "</br>提示：</td>");
                        sb.AppendLine("<td class=\"all\">");
                        sb.AppendLine("<div>"+ text[0]+ "，</div>");
                        sb.AppendLine("<div>" + text[1] + "</div>");
                        sb.AppendLine("<div>宝钢："+ chars[3]+ "</div>");
                        sb.AppendLine("<div>提示："+ gettishi()+ "，</div></br>");
                        sb.AppendLine("</td>");

                    }
                    sb.AppendLine("</tr>");
                }



                for (int i = 0; i < 7; i++)
                {
                    sxvalue2 = sxvalue2 + 1;
                    if (sxvalue2 > 11)
                    {
                        sxvalue2 = 0;
                    }

                    string shengxiao2 = sx[sxvalue2];
                    sb2.AppendLine("<tr><th colspan=6 cellspacing=0>" + DateTime.Now.AddDays(i).ToString("yyyy年MM月dd") + "属" + shengxiao2 + "</th></tr>");
                    sb2.AppendLine("<tr>");
                    for (int j = 0; j < 3; j++)
                    {
                        string chengyu = getchengyu();

                        string shici = getshici();
                        string[] text = shici.Split(new string[] { "，" }, StringSplitOptions.None);
                        char[] chars = shici.ToArray();

          
                        sb2.AppendLine("<td class=\"all\">");
                        sb2.AppendLine("<div>" + text[0] + "，</div>");
                        sb2.AppendLine("<div>" + text[1] + "</div>");
                        sb2.AppendLine("<div>宝钢：" + chars[3] + "</div>");
                      
                        sb2.AppendLine("</td>");

                    }
                    sb2.AppendLine("</tr>");
                }



                StreamReader sr = new StreamReader(Application.StartupPath + @"\data\indexmuban.html", EncodingType.GetTxtType(Application.StartupPath + @"\data\indexmuban.html"));
                //一次性读取完 
                string body = sr.ReadToEnd();
                body = body.Replace("{body1}",sb.ToString());
                body = body.Replace("{body2}", sb2.ToString());
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                System.IO.File.WriteAllText(Application.StartupPath + @"\data\index.html", body, Encoding.UTF8);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            run();
            OpenBrowserUrl(Application.StartupPath + @"\data\index.html");
            
        }

        /// <summary>
        /// 调用系统浏览器打开网页
        /// http://m.jb51.net/article/44622.htm
        /// http://www.2cto.com/kf/201412/365633.html
        /// </summary>
        /// <param name="url">打开网页的链接</param>
        public static void OpenBrowserUrl(string url)
        {
            try
            {
                // 64位注册表路径
                var openKey = @"SOFTWARE\Wow6432Node\Google\Chrome";
                if (IntPtr.Size == 4)
                {
                    // 32位注册表路径
                    openKey = @"SOFTWARE\Google\Chrome";
                }
                RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);
                // 谷歌浏览器就用谷歌打开，没找到就用系统默认的浏览器
                // 谷歌卸载了，注册表还没有清空，程序会返回一个"系统找不到指定的文件。"的bug
                if (appPath != null)
                {
                    var result = Process.Start("chrome.exe", url);
                    if (result == null)
                    {
                        OpenIe(url);
                    }
                }
                else
                {
                    var result = Process.Start("chrome.exe", url);
                    if (result == null)
                    {
                        OpenDefaultBrowserUrl(url);
                    }
                }
            }
            catch
            {
                // 出错调用用户默认设置的浏览器，还不行就调用IE
                OpenDefaultBrowserUrl(url);
            }
        }

        /// <summary>
        /// 用IE打开浏览器
        /// </summary>
        /// <param name="url"></param>
        public static void OpenIe(string url)
        {
            try
            {
                Process.Start("iexplore.exe", url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // IE浏览器路径安装：C:\Program Files\Internet Explorer
                // at System.Diagnostics.process.StartWithshellExecuteEx(ProcessStartInfo startInfo)注意这个错误
                try
                {
                    if (File.Exists(@"C:\Program Files\Internet Explorer\iexplore.exe"))
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = @"C:\Program Files\Internet Explorer\iexplore.exe",
                            Arguments = url,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(processStartInfo);
                    }
                    else
                    {
                        if (File.Exists(@"C:\Program Files (x86)\Internet Explorer\iexplore.exe"))
                        {
                            ProcessStartInfo processStartInfo = new ProcessStartInfo
                            {
                                FileName = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe",
                                Arguments = url,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };
                            Process.Start(processStartInfo);
                        }
                        else
                        {
                            if (MessageBox.Show(@"系统未安装IE浏览器，是否下载安装？", null, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                // 打开下载链接，从微软官网下载
                                OpenDefaultBrowserUrl("http://windows.microsoft.com/zh-cn/internet-explorer/download-ie");
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        /// <summary>
        /// 打开系统默认浏览器（用户自己设置了默认浏览器）
        /// </summary>
        /// <param name="url"></param>
        public static void OpenDefaultBrowserUrl(string url)
        {
            try
            {
                // 方法1
                //从注册表中读取默认浏览器可执行文件路径
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                if (key != null)
                {
                    string s = key.GetValue("").ToString();
                    //s就是你的默认浏览器，不过后面带了参数，把它截去，不过需要注意的是：不同的浏览器后面的参数不一样！
                    //"D:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
                    var lastIndex = s.IndexOf(".exe", StringComparison.Ordinal);
                    if (lastIndex == -1)
                    {
                        lastIndex = s.IndexOf(".EXE", StringComparison.Ordinal);
                    }
                    var path = s.Substring(1, lastIndex + 3);
                    var result = Process.Start(path, url);
                    if (result == null)
                    {
                        // 方法2
                        // 调用系统默认的浏览器
                        var result1 = Process.Start("explorer.exe", url);
                        if (result1 == null)
                        {
                            // 方法3
                            Process.Start(url);
                        }
                    }
                }
                else
                {
                    // 方法2
                    // 调用系统默认的浏览器
                    var result1 = Process.Start("explorer.exe", url);
                    if (result1 == null)
                    {
                        // 方法3
                        Process.Start(url);
                    }
                }
            }
            catch
            {
                OpenIe(url);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
