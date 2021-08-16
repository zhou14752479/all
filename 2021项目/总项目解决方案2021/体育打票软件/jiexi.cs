using gregn6Lib;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    public partial class jiexi : Form
    {
        public jiexi()
        {
            InitializeComponent();
        }
        #region ini读取
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

        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private GridppReport Report = new GridppReport();



        public void getdata()
        {
            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");

            string suiji = bianma + function.getsuijima();
            string zhanhao = haoma;
            string ahtml = 体育打票软件.ahtml;
            string html = 体育打票软件.html;

            if (setup.muban != "" && setup.muban != null)
            {
                Report.LoadFromFile(path + "template\\" + setup.muban + ".grf");
            }
            else
            {
                Report.LoadFromFile(path + "template\\a.grf");
            }

            string[] text = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            string[] text1 = text[0].Trim().Split(new string[] { "	" }, StringSplitOptions.None);

           
            //string fangshi = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            //string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            //string suiji = Guid.NewGuid().ToString();



            //string guoguan = "过关方式 " + Regex.Match(html, @"checked="""">([\s\S]*?)</span>").Groups[1].Value;
            string beishu = Regex.Replace(text1[2], ".*/", "");
            string jine = text1[3];

            MessageBox.Show(beishu+"   "+jine);
            MatchCollection value1 = Regex.Matches(html, @"<span class=""delSelTrBtn"">([\s\S]*?)</tr>");

            MatchCollection matchIds = Regex.Matches(html, @"class=""mCodeCls""([\s\S]*?)>([\s\S]*?)</td>");
            MatchCollection matchNames = Regex.Matches(html, @"<span class=""AgainstInfo"">([\s\S]*?)</span>");


            Dictionary<string, string> dics = new Dictionary<string, string>();
            for (int i = 0; i < matchIds.Count; i++)
            {
                dics.Add(matchIds[i].Groups[2].Value, Regex.Replace(matchNames[i].Groups[1].Value, "<[^>]+>", ""));
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < value1.Count; i++)
            {
                string a1 = Regex.Match(value1[i].Groups[1].Value, @"</td><td>周([\s\S]*?)</td>").Groups[1].Value;
                string a2 = Regex.Match(value1[i].Groups[1].Value, @"class=""selOption"">([\s\S]*?)</span>").Groups[1].Value;
                sb.Append("第" + (i + 1) + "场  周" + a1 + "\n");
                sb.Append(dics["周" + a1] + "\n");
                sb.Append(a2 + "@" + "\n");
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            Report.ParameterByName("suiji").AsString = suiji;
            //Report.ParameterByName("fangshi").AsString = fangshi;
            //Report.ParameterByName("leixing").AsString = leixing;
            //Report.ParameterByName("guoguan").AsString = guoguan;
            Report.ParameterByName("beishu").AsString = beishu;

            Report.ParameterByName("jine").AsString = jine;
            Report.ParameterByName("neirong").AsString = sb.ToString();

            Report.ParameterByName("zhanhao").AsString = haoma;
            Report.ParameterByName("time").AsString = time;
        }



        private void jiexi_Load(object sender, EventArgs e)
        {
            textBox2.Text= DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        }


    


        private void button1_Click(object sender, EventArgs e)
        {
            getdata();

            // Report.Print(true);
            //Report.PrintPreview(true);
            PreviewForm theForm = new PreviewForm();
            theForm.AttachReport(Report);
            theForm.ShowDialog();

            if (checkBox2.Checked == true)
            {
               textBox1.Text="";
            }

            if (checkBox3.Checked == true)
            {
                this.Hide();
            }
        }
    }
}
