using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Url加密
{
    public partial class 激活 : Form
    {
        public 激活()
        {
            InitializeComponent();
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.m";
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

        private void button1_Click(object sender, EventArgs e)
        {
            //if (ExistINIFile())
            //{
            //    string key = IniReadValue("values", "key");

            //    string url = "http://47.99.104.164/do.php?key=" + key + "&method=login";
            //    string html = URL加密.GetUrl(url, "utf-8");
            //    if (html.Contains("true"))
            //    {
            //       string yuming = Regex.Match(html, @"""url"":""([\s\S]*?)""").Groups[1].Value;

            //        if (yuming == "")
            //        {
            //            string aurl = "http://47.99.104.164/do.php?key=" + key + "&url=" + textBox1.Text.Trim() + "&safecode=" + textBox2.Text.Trim() + "&method=update";
            //            string ahtml = URL加密.GetUrl(aurl, "utf-8");
            //            if (html.Contains("true"))
            //            {
            //                IniWriteValue("values", "url", textBox1.Text.Trim());
            //                MessageBox.Show("绑定成功，请重启软件使用");
            //            }

            //        }
            //        else
            //        {
            //            MessageBox.Show("绑定失败,key已使用");
            //        }




            //    }



            //}
            //else
            //{
            //    MessageBox.Show("key不存在请联系QQ71751777");
            //}


            string url = "http://47.99.104.164/do.php?key=" + textBox2.Text.Trim() + "&method=login";
            string html = URL加密.GetUrl(url, "utf-8");
            if (html.Contains("true"))
            {
                string key = Regex.Match(html, @"""key"":""([\s\S]*?)""").Groups[1].Value;
                string yuming = Regex.Match(html, @"""url"":""([\s\S]*?)""").Groups[1].Value;
                if (yuming == textBox1.Text)
                {
                    MessageBox.Show("key已重新生成，请重启软件使用");
                    IniWriteValue("values", "key", key);
                    IniWriteValue("values", "url", yuming);
                }
                else
                {
                    MessageBox.Show("域名不匹配");
                }
            }
            else
            {
                MessageBox.Show("安全码不存在");
            }


        }




    }
}
