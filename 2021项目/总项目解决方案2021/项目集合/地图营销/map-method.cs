using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;

namespace 地图营销
{
    class map_method
    {
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





    #region 机器码
        public void jiance()
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
                string[] value = secret.Split(new string[] { "asd147" }, StringSplitOptions.None);


                if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                {
                    MessageBox.Show("激活已过期");
                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }

                }


                else if (value[0] != method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian") || key != method.GetMD5(method.GetMacAddress()))
                {

                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }
                }


            }
            else
            {

                string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);
                if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                {
                    IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                    IniWriteValue("values", "secret", str);
                    MessageBox.Show("激活成功");


                }
                else
                {
                    MessageBox.Show("激活码错误");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }

        }

        #endregion



        public bool jihuo = true;
        /// <summary>
        /// 未激活不退出
        /// </summary>
        public void jiance2()
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
                string[] value = secret.Split(new string[] { "asd147" }, StringSplitOptions.None);


                if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                {
                    MessageBox.Show("激活已过期");
                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        jihuo = false;
                        // System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }

                }


                else if (value[0] != method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian") || key != method.GetMD5(method.GetMacAddress()))
                {

                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        jihuo = false;
                        //System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }
                }


            }
            else
            {

                string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);
                if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                {
                    IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                    IniWriteValue("values", "secret", str);
                    MessageBox.Show("激活成功");


                }
                else
                {

                    MessageBox.Show("激活码错误");
                    jihuo = false;
                    // System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }

        }



        public void register(string jihuoma)
        {

            string html = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?method=register&username=" + jihuoma + "&password=1&days=1&type=1111jihuoma", "utf-8");



        }

        public bool login(string jihuoma)
        {
            string html = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?username=" + jihuoma + "&password=1&method=login", "utf-8");
            if (html.Contains("true"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 激活码

        public void jihuoma()
        {
            try
            {



                string macmd5 = method.GetMD5(method.GetMacAddress());
                long expiretime = Convert.ToInt64(method.GetTimeStamp()) + 365 * 24 * 3600;
                if (ExistINIFile())
                {
                    string key = IniReadValue("values", "key");

                    string[] value = key.Split(new string[] { "asd147" }, StringSplitOptions.None);


                    if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                    {
                        MessageBox.Show("激活已过期");
                        string str = Interaction.InputBox("请购买激活码,使用正式版软件！\r\n\r\n无激活码点击确定免费试用", "激活软件", "", -1, -1);
                        string fullstr = str;
                        if (login(fullstr))
                        {
                            MessageBox.Show("激活失败，激活码失效");
                            return;
                        }
                        if (str.Length > 40)
                        {
                            str = str.Remove(0, 10);
                            str = str.Remove(str.Length - 10, 10);

                            str = method.Base64Decode(Encoding.Default, str);
                            string index = str.Remove(str.Length - 16, 16);
                            string time = str.Substring(str.Length - 10, 10);
                            if (Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(time) < 99999999)  //200秒内有效
                            {
                                if (index == "ling" || index == "san")//美团一年
                                {

                                    IniWriteValue("values", "key", macmd5 + "asd147" + expiretime);

                                    MessageBox.Show("激活成功");
                                    register(fullstr);
                                    return;
                                }
                            }
                            if (index == "si")//试用一天
                            {

                                IniWriteValue("values", "key", macmd5 + "asd147" + 86400);

                                MessageBox.Show("激活成功");
                                register(fullstr);
                                return;
                            }
                        }
                        MessageBox.Show("激活码错误，点击试用");
                        jihuo = false; ;
                    }

                }
                else
                {
                    string str = Interaction.InputBox("请购买激活码,使用正式版软件！\r\n\r\n无激活码点击确定免费试用", "激活软件", "", -1, -1);
                    string fullstr = str;
                    if (login(fullstr))
                    {
                        MessageBox.Show("激活失败，激活码失效");
                        return;
                    }
                    if (str.Length > 40)
                    {
                        str = str.Remove(0, 10);
                        str = str.Remove(str.Length - 10, 10);

                        str = method.Base64Decode(Encoding.Default, str);
                        string index = str.Remove(str.Length - 16, 16);
                        string time = str.Substring(str.Length - 10, 10);
                        if (Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(time) < 99999999)  //200秒内有效
                        {
                            if (index == "ling" || index == "san")//美团一年
                            {
                                IniWriteValue("values", "key", macmd5 + "asd147" + expiretime);

                                MessageBox.Show("激活成功");
                                register(fullstr);
                                return;
                            }
                        }
                        if (index == "si")//试用一天
                        {

                            IniWriteValue("values", "key", macmd5 + "asd147" + 86400);

                            MessageBox.Show("激活成功");
                            register(fullstr);
                            return;
                        }
                    }
                    MessageBox.Show("激活码错误，点击试用");
                    jihuo = false; ;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("激活码错误");
                jihuo = false; ;
            }


        }


        #endregion

    }
}
