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
using System.Threading;
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
       



        public void getdata()
        {
            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");
            string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss").Replace("-", "/");
            string suiji = bianma + function.getsuijima();
            string zhanhao = haoma;
            string ahtml = 体育打票软件.ahtml;
            string html = 体育打票软件.html;

           

            StringBuilder textsb = new StringBuilder();
            string[] text = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string item in text)
            {

                if (item != "")
                {
                    if (item.Contains("/"))
                    {
                        textsb.Append("\r\n" + item);
                    }
                    else
                    {
                        textsb.Append("&" + item + "&");
                    }
                }

            }
            string[] text0 = textsb.ToString().Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            progressBar1.Value = 0;  //清空进度条
            progressBar1.Maximum = text0.Length - 1;  //清空进度条
            for (int a = 0; a < text0.Length; a++)
            {
                string item = text0[a];

                 GridppReport Report = new GridppReport();
                if (setup.muban != "" && setup.muban != null)
                {
                    Report.LoadFromFile(path + "template\\" + setup.muban + ".grf");
                }
                else
                {
                    Report.LoadFromFile(path + "template\\a.grf");
                }
                string[] text1 = item.Split(new string[] { "	" }, StringSplitOptions.None);


                string fangshi = "竞彩" + Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;


                string beishu = Regex.Replace(text1[2], ".*/", "");
                string jine = text1[3];
                string guoguan = "过关方式 " + text1[4].Replace("串", "×");

                //周三002>让平|3.00
                MatchCollection zhous = Regex.Matches(item, @"周([\s\S]*?)>");
                MatchCollection results = Regex.Matches(item, @">([\s\S]*?)\|");
                MatchCollection prices = Regex.Matches(item, @"\|([\s\S]*?)&");




                MatchCollection matchIds = Regex.Matches(html, @"class=""mCodeCls""([\s\S]*?)>([\s\S]*?)</td>");
                MatchCollection matchNames = Regex.Matches(html, @"<span class=""AgainstInfo"">([\s\S]*?)</span>");


                Dictionary<string, string> dics = new Dictionary<string, string>();
                for (int i = 0; i < matchIds.Count; i++)
                {
                    dics.Add(matchIds[i].Groups[2].Value, Regex.Replace(matchNames[i].Groups[1].Value, "<[^>]+>", ""));
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < zhous.Count; i++)
                {
                    string a1 = zhous[i].Groups[1].Value;
                    string a2 = results[i].Groups[1].Value;
                    string a3 = prices[i].Groups[1].Value;
                    sb.Append("第" + (i + 1) + "场  周" + a1 + "\n");
                    sb.Append("主队:" + dics["周" + a1].Replace("VS", "VS客队:") + "\n");
                    sb.Append(a2 + "@" + a3 + "\n");
                }



                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                Report.ParameterByName("guoguan").AsString = guoguan;
                Report.ParameterByName("beishu").AsString = beishu;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();

                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;

                // Report.Print(false);
                //Report.PrintPreview(true);
                PreviewForm theForm = new PreviewForm();
                theForm.AttachReport(Report);
                theForm.ShowDialog();
                progressBar1.Value = a;
                label3.Text = ( ((a / (text0.Length-1)) * 100).ToString() + "%");
                Thread.Sleep(Convert.ToInt32(textBox3.Text)*1000);

            }
        }



        private void jiexi_Load(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        }





        private void button1_Click(object sender, EventArgs e)
        {
            getdata();

         

            //if (checkBox2.Checked == true)
            //{
            //   textBox1.Text="";
            //}

            //if (checkBox3.Checked == true)
            //{
            //    this.Hide();
            //}
        }
    }
}
