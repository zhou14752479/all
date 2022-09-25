using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 远程数据管理
{
    public partial class 上传获取客户端 : Form
    {
        public 上传获取客户端()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }




        private void cbx_startup()
        {
            // 要设置软件名称，有唯一性要求，最好起特别一些
            string SoftWare = "SunnyNetEaseCloud";

            // 注意this.uiCheckBox1.Checked时针对Winfom程序的，如果是命令行程序要另外设置一个触发值
            if (checkBox1.Checked)
            {

                Console.WriteLine("设置开机自启动，需要修改注册表", "提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.CurrentUser; //
                                                       // 添加到 当前登陆用户的 注册表启动项     
                try
                {
                    //  
                    //SetValue:存储值的名称   
                    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

                    // 检测是否之前有设置自启动了，如果设置了，就看值是否一样
                    string old_path = (string)rk2.GetValue(SoftWare);
                    Console.WriteLine("\r\n注册表值: {0}", old_path);

                    if (old_path == null || !path.Equals(old_path))
                    {
                        rk2.SetValue(SoftWare, path);
                       //MessageBox.Show("添加开启启动成功");
                    }

                    rk2.Close();
                    rk.Close();

                }
                catch (Exception ee)
                {
                   // MessageBox.Show("开机自启动设置失败");

                }
            }
            else
            {
                // 取消开机自启动
                //MessageBox.Show("取消开机自启动，需要修改注册表", "提示");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.CurrentUser;
                try
                {
                    // SetValue: 存储值的名称
                    RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

                    string old_path = (string)rk2.GetValue(SoftWare);
                    Console.WriteLine("\r\n注册表值: {0}", old_path);

                    rk2.DeleteValue(SoftWare, false);
                    MessageBox.Show("取消开启启动成功");
                    rk2.Close();
                    rk.Close();
                }
                catch (Exception ee)
                {
                    //MessageBox.Show(ee.Message.ToString(), "提 示", MessageBoxButtons.OK, MessageBoxIcon.Error);  // 提示
                    MessageBox.Show("取消开机自启动失败");
                }
            }
        }





 






    #region POST请求
    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="postData">发送的数据包</param>
    /// <param name="COOKIE">cookie</param>
    /// <param name="charset">编码格式</param>
    /// <returns></returns>
    public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

              
               // request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
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
        public string getip()
            {

            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
                var addressV = iPHostEntry.AddressList.FirstOrDefault(q => q.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);//ip4地址
                if (addressV != null)
                    return addressV.ToString();
                return "127.0.0.1";
            }
            catch (Exception ex)
            {
                return "127.0.0.1";
            }
        }

        public string getwaiIP()
        {
            try
            {
                string url = "https://ip.cn/api/index?ip=&type=0";
                string html = GetUrl(url,"utf-8");
                string ip = Regex.Match(html, @"""ip"":""([\s\S]*?)""").Groups[1].Value;
                return ip;
            }
            catch (Exception)
            {

                return "";
            }
        }
        private void 上传获取客户端_Load(object sender, EventArgs e)
        {

            checkBox1.Checked = true;
            textBox1.Text = getwaiIP();
            quanjuser();
            //#region 通用检测

            //string html = PostUrl("http://www.acaiji.com/index/index/vip.html","","", "utf-8");

            //if (!html.Contains(@"Dgx17"))
            //{
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();

            //}

            //#endregion

          

           
            //读取config.ini
            if (ExistINIFile())
            {
                textBox2.Text = "Q";

                //读取

                //getTextBox1.Text = IniReadValue("values", "a1");
                //getTextBox2.Text = IniReadValue("values", "a2");
                //getTextBox3.Text = IniReadValue("values", "a3");
                //getTextBox4.Text = IniReadValue("values", "a4");
                //getTextBox5.Text = IniReadValue("values", "a5");
                //getTextBox6.Text = IniReadValue("values", "a6");
                //getTextBox7.Text = IniReadValue("values", "a7");
                //getTextBox8.Text = IniReadValue("values", "a8");
                //getTextBox9.Text = IniReadValue("values", "a9");
                //getTextBox10.Text = IniReadValue("values", "a10");


                getdic.Add("a1", getTextBox1.Text);
                getdic.Add("a2", getTextBox2.Text);
                getdic.Add("a3", getTextBox3.Text);
                getdic.Add("a4", getTextBox4.Text);
                getdic.Add("a5", getTextBox5.Text);
                getdic.Add("a6", getTextBox6.Text);
                getdic.Add("a7", getTextBox7.Text);
                getdic.Add("a8", getTextBox8.Text);
                getdic.Add("a9", getTextBox9.Text);
                getdic.Add("a10", getTextBox10.Text);









                //上传



                //uploadTextBox1.Text = IniReadValue("values2", "a1");
                //uploadTextBox2.Text = IniReadValue("values2", "a2");
                //uploadTextBox3.Text = IniReadValue("values2", "a3");
                //uploadTextBox4.Text = IniReadValue("values2", "a4");
                //uploadTextBox5.Text = IniReadValue("values2", "a5");
                //uploadTextBox6.Text = IniReadValue("values2", "a6");
                //uploadTextBox7.Text = IniReadValue("values2", "a7");
                //uploadTextBox8.Text = IniReadValue("values2", "a8");
                //uploadTextBox9.Text = IniReadValue("values2", "a9");
                //uploadTextBox10.Text = IniReadValue("values2", "a10");


                uploaddic.Add("a1", uploadTextBox1.Text);
                uploaddic.Add("a2", uploadTextBox2.Text);
                uploaddic.Add("a3", uploadTextBox3.Text);
                uploaddic.Add("a4", uploadTextBox4.Text);
                uploaddic.Add("a5", uploadTextBox5.Text);
                uploaddic.Add("a6", uploadTextBox6.Text);
                uploaddic.Add("a7", uploadTextBox7.Text);
                uploaddic.Add("a8", uploadTextBox8.Text);
                uploaddic.Add("a9", uploadTextBox9.Text);
                uploaddic.Add("a10", uploadTextBox10.Text);





            }

        }


        Dictionary<string, string>  uploaddic=new Dictionary<string, string>();
        Dictionary<string, string> getdic = new Dictionary<string, string>();



        public string upload(string key,string value)
        {
            string ip = textBox1.Text.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            string url = "http://www.acaiji.com/yuancheng/yuancheng.php?method=update";
            string postData = "ip="+ip+"&key="+key+"&value="+value;
            string html = PostUrl(url,postData,"","utf-8");
            //MessageBox.Show(postData);
            //MessageBox.Show(html);
            return html;
        }


        public string getall(string key)
        {
            string ip = textBox1.Text.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            string url = "http://www.acaiji.com/yuancheng/yuancheng.php?method=getall";
            string postData = "ip=" + ip;
            string html = PostUrl(url, postData, "", "utf-8");
            if (key!="a10")
            {
              
                MatchCollection values = Regex.Matches(html, @""":([\s\S]*?),");
                MatchCollection keys = Regex.Matches(html, @",""([\s\S]*?)""");




                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i].Groups[1].Value == key)
                    {

                        return values[(i + 1)].Groups[1].Value.Replace("\"", "");
                    }
                }
            }
            else
            {
                return Regex.Match(html, @"a10"":([\s\S]*?)}").Groups[1].Value;
            }

            return "";
        }



        private void button1_Click(object sender, EventArgs e)
        {
           getdic.Clear();
            //写入config.ini配置文件
            IniWriteValue("values", "a1", getTextBox1.Text);
            IniWriteValue("values", "a2", getTextBox2.Text);
            IniWriteValue("values", "a3", getTextBox3.Text);
            IniWriteValue("values", "a4", getTextBox4.Text);
            IniWriteValue("values", "a5", getTextBox5.Text);
            IniWriteValue("values", "a6", getTextBox6.Text);
            IniWriteValue("values", "a7", getTextBox7.Text);
            IniWriteValue("values", "a8", getTextBox8.Text);
            IniWriteValue("values", "a9", getTextBox9.Text);
            IniWriteValue("values", "a10", getTextBox10.Text);

            getdic.Add("a1",getTextBox1.Text);
            getdic.Add("a2", getTextBox2.Text);
            getdic.Add("a3", getTextBox3.Text);
            getdic.Add("a4", getTextBox4.Text);
            getdic.Add("a5", getTextBox5.Text);
            getdic.Add("a6", getTextBox6.Text);
            getdic.Add("a7", getTextBox7.Text);
            getdic.Add("a8", getTextBox8.Text);
            getdic.Add("a9", getTextBox9.Text);
            getdic.Add("a10", getTextBox10.Text);
        }


        private void 上传获取客户端_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            string value = "";
            foreach (string item in getdic.Keys)
            {
                if(e.KeyChar.ToString().ToLower()== getdic[item])
                {
                    value =getall(item.Trim());
                    textBox2.Text = item+"值为："+value;
                }

            }


            foreach (string item in uploaddic.Keys)
            {
                if (e.KeyChar.ToString().ToLower() == uploaddic[item])
                {
                    string html = upload(item, textBox4.Text.Trim());
                   
                    if(html.Contains("更新成功"))
                    {
                        textBox3.Text = item + "：值上传成功" ;
                    }
                    else
                    {
                        textBox3.Text = "数据上传或数据未改变";
                    }
                }

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

           


            uploaddic.Clear();
            //写入config.ini配置文件
            IniWriteValue("values2", "a1", uploadTextBox1.Text);
            IniWriteValue("values2", "a2", uploadTextBox2.Text);
            IniWriteValue("values2", "a3", uploadTextBox3.Text);
            IniWriteValue("values2", "a4", uploadTextBox4.Text);
            IniWriteValue("values2", "a5", uploadTextBox5.Text);
            IniWriteValue("values2", "a6", uploadTextBox6.Text);
            IniWriteValue("values2", "a7", uploadTextBox7.Text);
            IniWriteValue("values2", "a8", uploadTextBox8.Text);
            IniWriteValue("values2", "a9", uploadTextBox9.Text);
            IniWriteValue("values2", "a10", uploadTextBox10.Text);

            uploaddic.Add("a1", uploadTextBox1.Text);
            uploaddic.Add("a2", uploadTextBox2.Text);
            uploaddic.Add("a3", uploadTextBox3.Text);
            uploaddic.Add("a4", uploadTextBox4.Text);
            uploaddic.Add("a5", uploadTextBox5.Text);
            uploaddic.Add("a6", uploadTextBox6.Text);
            uploaddic.Add("a7", uploadTextBox7.Text);
            uploaddic.Add("a8", uploadTextBox8.Text);
            uploaddic.Add("a9", uploadTextBox9.Text);
            uploaddic.Add("a10", uploadTextBox10.Text);
        }



        #region  全局快捷键

        public delegate void HotkeyEventHandler(int HotKeyID);
        private int Hotkeya1;
        private int Hotkeya2;
        private int Hotkeya3;
        private int Hotkeya4;
        private int Hotkeya5;
        private int Hotkeya6;
        private int Hotkeya7;
        private int Hotkeya8;
        private int Hotkeya9;
        private int Hotkeya10;


        private int Hotkeyb1;
        private int Hotkeyb2;
        private int Hotkeyb3;
        private int Hotkeyb4;
        private int Hotkeyb5;
        private int Hotkeyb6;
        private int Hotkeyb7;
        private int Hotkeyb8;
        private int Hotkeyb9;
        private int Hotkeyb10;
        public class Hotkey : System.Windows.Forms.IMessageFilter
        {
            Hashtable keyIDs = new Hashtable();
            IntPtr hWnd;

            public event HotkeyEventHandler OnHotkey;

            public enum KeyFlags
            {
                MOD_ALT = 0x1,
                MOD_CONTROL = 0x2,
                MOD_SHIFT = 0x4,
                MOD_WIN = 0x8
            }
            [DllImport("user32.dll")]
            public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);

            [DllImport("user32.dll")]
            public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);

            [DllImport("kernel32.dll")]
            public static extern UInt32 GlobalAddAtom(String lpString);

            [DllImport("kernel32.dll")]
            public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);

            public Hotkey(IntPtr hWnd)
            {
                this.hWnd = hWnd;
                Application.AddMessageFilter(this);
            }

            public int RegisterHotkey(Keys Key, KeyFlags keyflags)
            {
                UInt32 hotkeyid = GlobalAddAtom(System.Guid.NewGuid().ToString());
                RegisterHotKey((IntPtr)hWnd, hotkeyid, (UInt32)keyflags, (UInt32)Key);
                keyIDs.Add(hotkeyid, hotkeyid);
                return (int)hotkeyid;
            }

            public void UnregisterHotkeys()
            {
                Application.RemoveMessageFilter(this);
                foreach (UInt32 key in keyIDs.Values)
                {
                    UnregisterHotKey(hWnd, key);
                    GlobalDeleteAtom(key);
                }
            }

            public bool PreFilterMessage(ref System.Windows.Forms.Message m)
            {
                if (m.Msg == 0x312)
                {
                    if (OnHotkey != null)
                    {
                        foreach (UInt32 key in keyIDs.Values)
                        {
                            if ((UInt32)m.WParam == key)
                            {
                                OnHotkey((int)m.WParam);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public void OnHotkey(int HotkeyID) //Ctrl+F2隐藏窗体，再按显示窗体。
        {
            string value = "";

            if (HotkeyID == Hotkeya1)
            {
                 value = getall("a1");
                textBox2.Text = "a1值为：" + value;
               if(value!=null&&value!="")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
               else
                {
                    MessageBox.Show("获取值为空");
                }
               
            }
            if (HotkeyID == Hotkeya2)
            {
                 value = getall("a2");
                textBox2.Text = "a2值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya3)
            {
                value = getall("a3");
                textBox2.Text = "a3值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya4)
            {
                 value = getall("a4");
                textBox2.Text = "a4值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya5)
            {
                 value = getall("a5");
                textBox2.Text = "a5值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya6)
            {
                 value = getall("a6");
                textBox2.Text = "a6值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya7)
            {
                 value = getall("a7");
                textBox2.Text = "a7值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya8)
            {
                 value = getall("a8");
                textBox2.Text = "a8值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya9)
            {
                 value = getall("a9");
                textBox2.Text = "a9值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }
            if (HotkeyID == Hotkeya10)
            {
                 value = getall("a10");
                textBox2.Text = "a10值为：" + value;
                if (value != null && value != "")
                {
                    System.Windows.Forms.Clipboard.SetText(value); //复制
                }
                else
                {
                    MessageBox.Show("获取值为空");
                }
            }



            if (HotkeyID == Hotkeyb1)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a1", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a1：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb2)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a2", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a2：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb3)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a3", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a3：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb4)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a4", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a4：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb5)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a5", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a5：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb6)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a6", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a6：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb7)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a7", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a7：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb8)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a8", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a8：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb9)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a9", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a9：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }
            if (HotkeyID == Hotkeyb10)
            {
                textBox4.Text = (String)Clipboard.GetDataObject().GetData(DataFormats.Text);
                string html = upload("a10", textBox4.Text.Trim());

                if (html.Contains("成功"))
                {
                    textBox3.Text = "a10：值上传成功";
                }
                else
                {
                    textBox3.Text = "数据上传或数据未改变";
                }
            }

        }


        #endregion


    


        public void quanjuser()
        {
            Hotkey hotkey;
            hotkey = new Hotkey(this.Handle);

            //Hotkey1 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.F2, Hotkey.KeyFlags.MOD_CONTROL); //定义快键(Ctrl + F2)
            Hotkeya1 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D1, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya2 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D2, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya3 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D3, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya4 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D4, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya5 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D5, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya6 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D6, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya7 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D7, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya8 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D8, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya9 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D9, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeya10 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D0, Hotkey.KeyFlags.MOD_CONTROL);


            Hotkeyb1 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.A, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb2 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.S, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb3 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.D, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb4 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.F, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb5 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.G, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb6 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.H, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb7 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.J, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb8 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.K, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb9 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.L, Hotkey.KeyFlags.MOD_CONTROL);
            Hotkeyb10 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.Z, Hotkey.KeyFlags.MOD_CONTROL);


            hotkey.OnHotkey += new HotkeyEventHandler(OnHotkey);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cbx_startup();
        }
    }
}
