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


       public string fangshi = "";
        public string panduan(MatchCollection results)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < results.Count; i++)
            {
                sb.Append("a"+results[i].Groups[1].Value+"a");

            }
         
            string maohao = Regex.Match(sb.ToString(),@":").Groups[0].Value;
            string shuzi= Regex.Match(sb.ToString(), @"a\da").Groups[0].Value;

            string sheng = Regex.Match(sb.ToString(), @"a胜a|a平a|a负a").Groups[0].Value;
            string rangsheng = Regex.Match(sb.ToString(), @"a让胜a|a让平a|a让负a").Groups[0].Value;
            string bansheng = Regex.Match(sb.ToString(), @"a胜胜a|a胜平a|a胜负a|a平胜a|a平平a|a平负a|a负胜a|a负平a|a负负a").Groups[0].Value;

          
             
           
            if (bansheng != "" && sheng == "" && rangsheng == "" && shuzi == "" && maohao == "")
            {
                return "竞彩足球半全场胜平负";
            }

            else if (bansheng == "" && sheng == "" && rangsheng != "" && shuzi == "" && maohao == "")
            {
                return "竞彩足球让球胜平负";
            }
            else if (bansheng == "" && sheng != "" && rangsheng == "" && shuzi == "" && maohao == "")
            {
                return "竞彩足球胜平负";
            }
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi != "" && maohao == "")
            {
                return "竞彩足球总进球数";
            }
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi == "" && maohao != "")
            {
                return "竞彩足球比分";
            }

            else
            {
                return "竞彩足球混合过关";
            }
        }


        

        public delegate void GetData_jingcai(string txt);
        public GetData_jingcai getdata_jingcai;


        function fc = new function();

        #region 竞彩网解析
        /// <summary>
        /// 竞彩网模拟解析全部
        /// </summary>
        public void  getdatajingcai()
        {
            getdata_jingcai(textBox1.Text);
        }

        #endregion



        #region 500网解析
        public void getdata_500_danguan()
        {
            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");
           
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

            textBox1.Text = textsb.ToString();

            string[] text0 = textsb.ToString().Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            progressBar1.Value = 0;  //清空进度条
            progressBar1.Maximum = text0.Length - 1;  //清空进度条

            MatchCollection matchIds = Regex.Matches(ahtml, @"data-matchnum=""([\s\S]*?)"""); //周三001
            MatchCollection matchNames = Regex.Matches(ahtml, @"data-homesxname=""([\s\S]*?)"" data-awaysxname=""([\s\S]*?)"""); //队伍名
            Dictionary<string, string> dics = new Dictionary<string, string>();

            for (int i = 0; i < matchIds.Count; i++)
            {

                dics.Add(matchIds[i].Groups[1].Value, "主队：" + matchNames[i].Groups[1].Value + " VS 客队：" + matchNames[i].Groups[2].Value);
            }
            for (int a = 0; a < text0.Length; a++)
            {
                string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss").Replace("-", "/");

                string item = text0[a]+"," ;

                GridppReport Report = new GridppReport();
                Report.LoadFromFile(path + "template\\a1.grf");  //500用短票
                string[] text1 = item.Split(new string[] { "	" }, StringSplitOptions.None);
                string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                string fangshi = "";


                string beishu = Regex.Replace(text1[2], ".*/", "");
                string jine = text1[3];
                string guoguan = "1场-单场固定";
                double jiangjin = 1;
                //周三002>让平|3.00
               

                fangshi = "";
                string houzhui = " ";
                MatchCollection peilvs = Regex.Matches(item, @"\|([\s\S]*?),");
                string a1 = "周"+Regex.Match(item, @"周([\s\S]*?)>").Groups[1].Value;
                    string a2 = "("+Regex.Match(item, @">.*").Groups[0].Value.Replace(">","").Replace("|",")@").Replace(",", "0元+(");

                // a2 = a2.Replace("+(","");//去掉末尾多余字符
                a2 = a2.Remove(a2.Length-2,2);
                a2 = a2.Replace("000","00");
                StringBuilder sb = new StringBuilder();
                //MessageBox.Show(a2);
                    if (a2.Contains("让胜")|| a2.Contains("让平") || a2.Contains("让负"))
                    {
                        fangshi = "竞彩足球胜平负";
                       
                    }
              
                else if (a2.Contains(":"))
                    {
                        fangshi = "竞彩足球比分";
                       
                    }
                    else if (a2.Contains("胜胜") || a2.Contains("胜平") || a2.Contains("胜负") || a2.Contains("平胜") || a2.Contains("平平") || a2.Contains("平负") || a2.Contains("负胜") || a2.Contains("负平") || a2.Contains("负负"))
                    {
                        fangshi = "竞彩足球半全场胜平负";
                    a2 = a2.Replace("(", "").Replace(")", "");

                }
                else if (a2.Contains("胜其") || a2.Contains("平其") || a2.Contains("负其"))
                {
                    fangshi = "竞彩足球比分";


                }
                else if (a2.Contains("胜") || a2.Contains("平") || a2.Contains("负"))
                {
                    fangshi = "竞彩足球胜平负";
                  
                    a2 = a2.Replace("(", "").Replace(")", "");
                }
                else
                    {
                        fangshi = "竞彩足球总进球数";
                       // houzhui = " 总进球数";
                    }

                    //if (a2 != "胜" && a2 != "平" && a2 != "负")
                    //{
                    //    a2 = "(" + a2 + ")";
                    //}

                    sb.Append("第1场 " + a1 + houzhui + "\n");
                    sb.Append(dics[a1] + "\n");

                    sb.Append(a2+"\n");

                    jiangjin = jiangjin * Convert.ToDouble(peilvs[0].Groups[1].Value);


                string peilv_max = peilvs[0].Groups[1].Value;
                for (int x = 0; x < peilvs.Count; x++)
                {
                    if(Convert.ToDouble(peilvs[x].Groups[1].Value) >Convert.ToDouble(peilv_max))
                    {

                        peilv_max = peilvs[x].Groups[1].Value;
                    }
                   
                }
                jiangjin = Convert.ToDouble(peilv_max)*Convert.ToDouble(beishu)*2;
                //jiangjin = Math.Round(jiangjin, 2);


                string ganxieyu = "感谢您为公益事业贡献 " + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";
                string zhushu = peilvs.Count.ToString();

              
                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数:" + "单场*"+zhushu+"注" + ";共" + zhushu + "注");



                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                Report.ParameterByName("guoguan").AsString = guoguan;
                Report.ParameterByName("beishu").AsString = beishu;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();

                Report.ParameterByName("dizhi").AsString = ganxieyu + "\n\n" + address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
                fc.CreateXmlFile(fangshi, suiji, guoguan, beishu, jine, sb.ToString(), ganxieyu, address, haoma, time);


                // Report.PrintPreview(true);
              if(体育打票软件.tiaoshi=="1")
                {
                    PreviewForm theForm = new PreviewForm();
                    theForm.AttachReport(Report);
                    theForm.ShowDialog();
                }
                


                Report.Print(false);
                progressBar1.Value = a;
                //label3.Text = ( ((a / (text0.Length-1)) * 100).ToString() + "%");
                Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);

            }
        }

        public void getdata_500_danguan_lanqiu()
        {
            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");
          
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

            textBox1.Text = textsb.ToString();

            string[] text0 = textsb.ToString().Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            progressBar1.Value = 0;  //清空进度条
            progressBar1.Maximum = text0.Length - 1;  //清空进度条

            MatchCollection matchIds = Regex.Matches(ahtml, @"data-matchnum=""([\s\S]*?)"""); //周三001
            MatchCollection matchNames = Regex.Matches(ahtml, @"data-homesxname=""([\s\S]*?)"" data-awaysxname=""([\s\S]*?)"""); //队伍名
            Dictionary<string, string> dics = new Dictionary<string, string>();

            for (int i = 0; i < matchIds.Count; i++)
            {

                dics.Add(matchIds[i].Groups[1].Value, "客队：" + matchNames[i].Groups[2].Value + " VS 主队：" + matchNames[i].Groups[1].Value);
            }
            for (int a = 0; a < text0.Length; a++)
            {
                string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss").Replace("-", "/");
                string item = text0[a] + ",";

                GridppReport Report = new GridppReport();
                Report.LoadFromFile(path + "template\\a1.grf");  //500用短票
                string[] text1 = item.Split(new string[] { "	" }, StringSplitOptions.None);
                string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                string fangshi = "";


                string beishu = Regex.Replace(text1[2], ".*/", "");
                string jine = text1[3];
                string guoguan = "1场-单场固定";
                double jiangjin = 1;
                //周三002>让平|3.00


                fangshi = "";
                string houzhui = " ";
                MatchCollection peilvs = Regex.Matches(item, @"\|([\s\S]*?),");
                string a1 = "周" + Regex.Match(item, @"周([\s\S]*?)>").Groups[1].Value;
                string a2 = "(" + Regex.Match(item, @">.*").Groups[0].Value.Replace(">", "").Replace("|", ")@").Replace(",", "0元+(");

                // a2 = a2.Replace("+(","");//去掉末尾多余字符
                a2 = a2.Remove(a2.Length - 2, 2);
                a2 = a2.Replace("000", "00");
                StringBuilder sb = new StringBuilder();
               
                if (a2.Contains("让分主胜") || a2.Contains("让分主负") )
                {
                    fangshi = "竞彩篮球让分胜负";
                    a2 = a2.Replace("(", "").Replace(")", "").Replace("让","");

                }

                else if (a2.Contains("-") || a2.Contains("26+"))
                {
                    fangshi = "竞彩篮球胜分差";
                    a2 = a2.Replace("胜", "").Replace("负", "");

                }
                else if (a2.Contains("大") || a2.Contains("小"))
                {
                    fangshi = "竞彩篮球大小分";

                }
                else if (a2.Contains("主胜") || a2.Contains("主负"))
                {
                    fangshi = "竞彩篮球胜负";

                    a2 = a2.Replace("(", "").Replace(")", "").Replace("主", "");
                }
                else
                {
                    fangshi = "竞彩足球总进球数";
                   
                }

                //if (a2 != "胜" && a2 != "平" && a2 != "负")
                //{
                //    a2 = "(" + a2 + ")";
                //}

                sb.Append("第1场 " + a1 + houzhui + "\n");
                sb.Append(dics[a1] + "\n");

                sb.Append(a2 + "\n");

                jiangjin = jiangjin * Convert.ToDouble(peilvs[0].Groups[1].Value);


                string peilv_max = peilvs[0].Groups[1].Value;
                for (int x = 0; x < peilvs.Count; x++)
                {
                    if (Convert.ToDouble(peilvs[x].Groups[1].Value) > Convert.ToDouble(peilv_max))
                    {

                        peilv_max = peilvs[x].Groups[1].Value;
                    }

                }
                jiangjin = Convert.ToDouble(peilv_max) * Convert.ToDouble(beishu) * 2;
                //jiangjin = Math.Round(jiangjin, 2);


                string ganxieyu = "感谢您为公益事业贡献 " + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";
                string zhushu = peilvs.Count.ToString();


                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + string.Format("{0:N}", Convert.ToDouble(jiangjin)) + "元\n单倍注数:" + "单场*"+zhushu+"注" + ";共" + zhushu + "注");



                Report.ParameterByName("suiji").AsString = suiji;
                Report.ParameterByName("fangshi").AsString = fangshi;
                Report.ParameterByName("leixing").AsString = leixing;
                Report.ParameterByName("guoguan").AsString = guoguan;
                Report.ParameterByName("beishu").AsString = beishu;

                Report.ParameterByName("jine").AsString = jine;
                Report.ParameterByName("neirong").AsString = sb.ToString();

                Report.ParameterByName("dizhi").AsString = ganxieyu + "\n\n" + address;
                Report.ParameterByName("zhanhao").AsString = haoma;
                Report.ParameterByName("time").AsString = time;
                fc.CreateXmlFile(fangshi, suiji, guoguan, beishu, jine, sb.ToString(), ganxieyu, address, haoma, time);


                // Report.PrintPreview(true);


                //PreviewForm theForm = new PreviewForm();
                //theForm.AttachReport(Report);
                //theForm.ShowDialog();

                Report.Print(false);

                progressBar1.Value = a;
                //label3.Text = ( ((a / (text0.Length-1)) * 100).ToString() + "%");
                Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);

            }
        }

        #endregion



        #region 500网解析---新解析

        public void getdata_500_new()
        {
            getdata_jingcai(textBox1.Text);
        }

        #endregion



        private void jiexi_Load(object sender, EventArgs e)
        {
           
          
        }





        private void button1_Click(object sender, EventArgs e)
        {
            if (fangshi.Contains("单关"))  //500网解析
            {
                if(fangshi.Contains("新解析"))  //500网新解析
                {
                    getdata_500_new();
                }

                else  
                {
                    if (fangshi.Contains("足球"))
                    {
                        getdata_500_danguan();
                    }
                    if (fangshi.Contains("篮球"))
                    {
                        getdata_500_danguan_lanqiu();
                    }

                }
           
                
            }
            else
            {
                getdatajingcai();


            }




            //清空功能
            if (checkBox2.Checked == true)
            {
                textBox1.Text = "";
            }

            if (checkBox3.Checked == true)
            {
                this.Hide();
            }

        }

       

      
    }
}
