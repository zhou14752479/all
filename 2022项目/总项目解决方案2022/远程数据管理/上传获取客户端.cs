using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private void 上传获取客户端_Load(object sender, EventArgs e)
        {
            //#region 通用检测

            //string html = PostUrl("http://www.acaiji.com/index/index/vip.html","","", "utf-8");

            //if (!html.Contains(@"Dgx17"))
            //{
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();

            //}

            //#endregion

            userControl1.groupBox1.Text = "A1";
            userControl2.groupBox1.Text = "A2";
            userControl3.groupBox1.Text = "A3";
            userControl4.groupBox1.Text = "A4";
            userControl5.groupBox1.Text = "A5";
            userControl6.groupBox1.Text = "A6";
            userControl7.groupBox1.Text = "A7";
            userControl8.groupBox1.Text = "A8";
            userControl9.groupBox1.Text = "A9";
            userControl10.groupBox1.Text = "A10";
            userControl11.groupBox1.Text = "B1";
            userControl12.groupBox1.Text = "B2";
            userControl13.groupBox1.Text = "B3";
            userControl14.groupBox1.Text = "B4";
            userControl15.groupBox1.Text = "B5";
            userControl16.groupBox1.Text = "B6";
            userControl17.groupBox1.Text = "B7";
            userControl18.groupBox1.Text = "B8";
            userControl19.groupBox1.Text = "B9";
            userControl20.groupBox1.Text = "B10";
            //读取config.ini
            if (ExistINIFile())
            {

                //读取
              
                userControl1.textBox1.Text = IniReadValue("values", "a1");
                userControl2.textBox1.Text = IniReadValue("values", "a2");
                userControl3.textBox1.Text = IniReadValue("values", "a3");
                userControl4.textBox1.Text = IniReadValue("values", "a4");
                userControl5.textBox1.Text = IniReadValue("values", "a5");
                userControl6.textBox1.Text = IniReadValue("values", "a6");
                userControl7.textBox1.Text = IniReadValue("values", "a7");
                userControl8.textBox1.Text = IniReadValue("values", "a8");
                userControl9.textBox1.Text = IniReadValue("values", "a9");
                userControl10.textBox1.Text = IniReadValue("values", "a10");


                getdic.Add("a1", userControl1.textBox1.Text);
                getdic.Add("a2", userControl2.textBox1.Text);
                getdic.Add("a3", userControl3.textBox1.Text);
                getdic.Add("a4", userControl4.textBox1.Text);
                getdic.Add("a5", userControl5.textBox1.Text);
                getdic.Add("a6", userControl6.textBox1.Text);
                getdic.Add("a7", userControl7.textBox1.Text);
                getdic.Add("a8", userControl8.textBox1.Text);
                getdic.Add("a9", userControl9.textBox1.Text);
                getdic.Add("a10", userControl10.textBox1.Text);









                //上传


              
                userControl11.textBox1.Text = IniReadValue("values2", "a1");
                userControl12.textBox1.Text = IniReadValue("values2", "a2");
                userControl13.textBox1.Text = IniReadValue("values2", "a3");
                userControl14.textBox1.Text = IniReadValue("values2", "a4");
                userControl15.textBox1.Text = IniReadValue("values2", "a5");
                userControl16.textBox1.Text = IniReadValue("values2", "a6");
                userControl17.textBox1.Text = IniReadValue("values2", "a7");
                userControl18.textBox1.Text = IniReadValue("values2", "a8");
                userControl19.textBox1.Text = IniReadValue("values2", "a9");
                userControl20.textBox1.Text = IniReadValue("values2", "a10");


                uploaddic.Add("a1", userControl11.textBox1.Text);
                uploaddic.Add("a2", userControl12.textBox1.Text);
                uploaddic.Add("a3", userControl13.textBox1.Text);
                uploaddic.Add("a4", userControl14.textBox1.Text);
                uploaddic.Add("a5", userControl15.textBox1.Text);
                uploaddic.Add("a6", userControl16.textBox1.Text);
                uploaddic.Add("a7", userControl17.textBox1.Text);
                uploaddic.Add("a8", userControl18.textBox1.Text);
                uploaddic.Add("a9", userControl19.textBox1.Text);
                uploaddic.Add("a10", userControl20.textBox1.Text);





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
            IniWriteValue("values", "a1", userControl1.key);
            IniWriteValue("values", "a2", userControl2.key);
            IniWriteValue("values", "a3", userControl3.key);
            IniWriteValue("values", "a4", userControl4.key);
            IniWriteValue("values", "a5", userControl5.key);
            IniWriteValue("values", "a6", userControl6.key);
            IniWriteValue("values", "a7", userControl7.key);
            IniWriteValue("values", "a8", userControl8.key);
            IniWriteValue("values", "a9", userControl9.key);
            IniWriteValue("values", "a10", userControl10.key);

            getdic.Add("a1",userControl1.key);
            getdic.Add("a2", userControl2.key);
            getdic.Add("a3", userControl3.key);
            getdic.Add("a4", userControl4.key);
            getdic.Add("a5", userControl5.key);
            getdic.Add("a6", userControl6.key);
            getdic.Add("a7", userControl7.key);
            getdic.Add("a8", userControl8.key);
            getdic.Add("a9", userControl9.key);
            getdic.Add("a10", userControl10.key);
        }

        private void 上传获取客户端_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.F1:
            //        MessageBox.Show("F1");
            //        break;
            //    case Keys.F2:
            //        MessageBox.Show("F2");
            //        break;
            //}

          


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
                    //MessageBox.Show(html);
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
            IniWriteValue("values2", "a1", userControl11.key);
            IniWriteValue("values2", "a2", userControl12.key);
            IniWriteValue("values2", "a3", userControl13.key);
            IniWriteValue("values2", "a4", userControl14.key);
            IniWriteValue("values2", "a5", userControl15.key);
            IniWriteValue("values2", "a6", userControl16.key);
            IniWriteValue("values2", "a7", userControl17.key);
            IniWriteValue("values2", "a8", userControl18.key);
            IniWriteValue("values2", "a9", userControl19.key);
            IniWriteValue("values2", "a10", userControl20.key);

            uploaddic.Add("a1", userControl11.key);
            uploaddic.Add("a2", userControl12.key);
            uploaddic.Add("a3", userControl13.key);
            uploaddic.Add("a4", userControl14.key);
            uploaddic.Add("a5", userControl15.key);
            uploaddic.Add("a6", userControl16.key);
            uploaddic.Add("a7", userControl17.key);
            uploaddic.Add("a8", userControl18.key);
            uploaddic.Add("a9", userControl19.key);
            uploaddic.Add("a10", userControl20.key);
        }



        #region  全局快捷键

        public delegate void HotkeyEventHandler(int HotKeyID);
        private int Hotkey1;
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
            if (HotkeyID == Hotkey1)
            {
                MessageBox.Show("1");
            }
            else
            {
                MessageBox.Show("2");
            }
        }


        #endregion


        private void button3_Click(object sender, EventArgs e)
        {
            Hotkey hotkey;
            hotkey = new Hotkey(this.Handle);

            //Hotkey1 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.F2, Hotkey.KeyFlags.MOD_CONTROL); //定义快键(Ctrl + F2)
            Hotkey1 = hotkey.RegisterHotkey(System.Windows.Forms.Keys.W, Hotkey.KeyFlags.MOD_CONTROL); //定义快键(Ctrl + F2)

            hotkey.OnHotkey += new HotkeyEventHandler(OnHotkey);

        }
    }
}
