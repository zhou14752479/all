using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 思忆美团
{
    class functions
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public string path = AppDomain.CurrentDomain.BaseDirectory + "system\\";
        public string inipath = AppDomain.CurrentDomain.BaseDirectory + "system\\" + "config.ini";
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

        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }

        public string jiqima = "";
      
        public  static string vip = "";

        #region  读取分类
        public Dictionary<string, string> catedic = new Dictionary<string, string>();
        public void Getcates(ComboBox cob)
        {

            try
            {
                StreamReader sr = new StreamReader(path + "cates.json", myDLL.method.EncodingType.GetTxtType(path + "cates.json"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string html = Regex.Match(texts, @"cateData([\s\S]*?)\}\}\}").Groups[1].Value;
               
                MatchCollection cates = Regex.Matches(html, @"""id"":([\s\S]*?),""name"":""([\s\S]*?)""");
               
                for (int i = 0; i < cates.Count; i++)
                {
                    if (!catedic.ContainsKey(cates[i].Groups[2].Value))
                    {
                        cob.Items.Add(cates[i].Groups[2].Value);
                        catedic.Add(cates[i].Groups[2].Value, cates[i].Groups[1].Value);
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

                
            }

            catch (System.Exception ex)
            {
              ex.ToString();
            }

        }



        #endregion

        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city);
                string html =  myDLL.method.GetUrl(url,"utf-8");
                Match cityId = Regex.Match(html, @"""cityId"":([\s\S]*?),");

                return cityId.Groups[1].Value;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }



        #endregion

        #region 获取区域
        public ArrayList getareas(string city)
        {
            string Url = "https://" + city + ".meituan.com/meishi/";

            string html = myDLL.method.GetUrl(Url,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"""subAreas"":\[\{""id"":([\s\S]*?),");
            ArrayList lists = new ArrayList();

            foreach (Match item in areas)
            {
                lists.Add(item.Groups[1].Value);
            }

            return lists;
        }

        #endregion

        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion

        #region  获取32位MD5加密
        public static string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion

        public string getjiqima()
        {
            return GetMD5(GetMacAddress()+"8o8osiyiruanjian.").ToUpper();
        }

        public string getsign(string timestamp)
        {
           
            return GetMD5(jiqima+timestamp).ToUpper();
        }


        #region 注册

        public void register(object o)
        {
            string[] values = o.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            string username = values[0].Trim();
            string password = values[1].Trim();
            string code = values[2].Trim();
            try
            {
                string url = "http://www.acaiji.com:8080/api/mt/register.html?username=" + username+"&password="+password+"&code="+code+"&";
                string html = method.GetUrl(url,"utf-8");
                MessageBox.Show(html);
            }
            catch (Exception ex)
            {

               ex.ToString() ;
            }

        }

        #endregion

        #region 登录

        public bool login(string username,string password,string code)
        {
          
            try
            {
                string url = "http://www.acaiji.com:8080/api/mt/login.html?username=" + username + "&password=" + password + "&code=" + code + "&";
                string html = method.GetUrl(url, "utf-8");
             
                string mac = Regex.Match(html, @"code"":""([\s\S]*?)""").Groups[1].Value;
                string isvip = Regex.Match(html, @"isvip"":""([\s\S]*?)""").Groups[1].Value;
                if (mac != "")
                {
                    functions.vip = isvip;
                    return true;
                }

                else {
                    MessageBox.Show("登录失败，请联系客服");
                    return false;
                }
            }
            catch (Exception ex)
            {
               
                ex.ToString();
                return false;


            }

        }

        #endregion

    }
}
