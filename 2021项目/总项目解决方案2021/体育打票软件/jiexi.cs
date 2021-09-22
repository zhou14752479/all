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



        public string panduan(MatchCollection results)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < results.Count; i++)
            {
                sb.Append("a"+results[i].Groups[1].Value+"a");

            }
            MessageBox.Show(sb.ToString());
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


        #region  赔率获取让球文字
        public Dictionary<string, string> getrangqiu_hunhe(string html)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>();
            MatchCollection peilvs = Regex.Matches(html, @"<label class=""rqCls rLine"">([\s\S]*?)</label>([\s\S]*?)solid;"">([\s\S]*?)<([\s\S]*?)solid;"">([\s\S]*?)</span><span([\s\S]*?)>([\s\S]*?)</span>");

            for (int i = 0; i < peilvs.Count; i++)
            {
                if (!dics.ContainsKey(peilvs[i].Groups[3].Value))
                {
                    dics.Add(peilvs[i].Groups[3].Value, peilvs[i].Groups[1].Value);
                }
                if (!dics.ContainsKey(peilvs[i].Groups[5].Value))
                {
                    dics.Add(peilvs[i].Groups[5].Value, peilvs[i].Groups[1].Value);
                }
                if (!dics.ContainsKey(peilvs[i].Groups[7].Value))
                {
                    dics.Add(peilvs[i].Groups[7].Value, peilvs[i].Groups[1].Value);
                }


            }

            return dics;
        }

        #endregion

        /// <summary>
        /// 混合过关的解析
        /// </summary>
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

           textBox1.Text = textsb.ToString();
           
            string[] text0 = textsb.ToString().Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            progressBar1.Value = 0;  //清空进度条
            progressBar1.Maximum = text0.Length - 1;  //清空进度条
            MatchCollection matchIds = Regex.Matches(html, @"class=""mCodeCls""([\s\S]*?)>([\s\S]*?)</td>");
            MatchCollection matchNames = Regex.Matches(html, @"<span class=""AgainstInfo"">([\s\S]*?)</span>");
            Dictionary<string, string> dics = new Dictionary<string, string>();

            for (int i = 0; i < matchIds.Count; i++)
            {
                string matchname = Regex.Replace(matchNames[i].Groups[1].Value, "<[^>]+>", "");
                matchname = Regex.Replace(matchname, @"\[.*?\]", "");
                dics.Add(matchIds[i].Groups[2].Value, matchname);
            }
            for (int a = 0; a < text0.Length; a++)
            {
                string item = text0[a];

                 GridppReport Report = new GridppReport();
                Report.LoadFromFile(path + "template\\a.grf");
                string[] text1 = item.Split(new string[] { "	" }, StringSplitOptions.None);
                string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                string fangshi = "竞彩" + Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;


                string beishu = Regex.Replace(text1[2], ".*/", "");
                string jine = text1[3];
                string guoguan = "过关方式 " + text1[4].Replace("串", "");
                double jiangjin = 1;
                //周三002>让平|3.00
                MatchCollection zhous = Regex.Matches(item, @"周([\s\S]*?)>");
                MatchCollection results = Regex.Matches(item, @">([\s\S]*?)\|");
                MatchCollection prices = Regex.Matches(item, @"\|([\s\S]*?)&");
                StringBuilder sb = new StringBuilder();

             
                if (panduan(results)== "竞彩足球混合过关")
                {
                    fangshi = "竞彩足球混合过关";
                    string houzhui = " 总进球数";
                    for (int i = 0; i < zhous.Count; i++)
                    {
                        string a1 = zhous[i].Groups[1].Value;
                        string a2 = results[i].Groups[1].Value;
                        string a3 = prices[i].Groups[1].Value;


                        string peilv = a3;
                        string rangqiuwenzi = "";
                        Dictionary<string, string> hunherangqiudic = getrangqiu_hunhe(html);

                        if (hunherangqiudic.ContainsKey(peilv))
                        {
                            if (hunherangqiudic[peilv].Contains("-"))
                            {
                                rangqiuwenzi = " 主队让" + hunherangqiudic[peilv].Replace("-", "") + "球";
                            }
                            if (hunherangqiudic[peilv].Contains("+"))
                            {
                                rangqiuwenzi = " 主队受让" + hunherangqiudic[peilv].Replace("+", "") + "球";
                            }
                        }





                        if (a2 == "胜" || a2 == "平" || a2 == "负")
                        {
                           
                            houzhui = " 胜平负";
                        }
                        else if (a2 == "让胜" || a2 == "让平" || a2 == "让负")
                        {
                           
                            houzhui = " 让球胜平负"+ rangqiuwenzi;
                        }
                        else if (a2.Contains(":"))
                        {
                          
                            houzhui = " 比分";
                        }
                        else if (a2.Contains("胜胜")|| a2.Contains("胜平") || a2.Contains("胜负") || a2.Contains("平胜") || a2.Contains("平平") || a2.Contains("平负") || a2.Contains("负胜") || a2.Contains("负平") || a2.Contains("负负"))
                        {
                            fangshi = "竞彩足球半全场胜平负";
                            houzhui = " 比分";
                        }
                        else
                        {
                          
                            houzhui = " 总进球数";
                        }




                       




                        a2 = a2.Replace("让","");
                        if (a2 != "胜" && a2 != "平" && a2 != "负")
                        {
                            a2 = "(" + a2 + ")";
                        }

                        sb.Append("第" + (i + 1) + "场 周" + a1+houzhui + "\n");
                        sb.Append("主队:" + dics["周" + a1].Replace("VS", " VS 客队:") + "\n");
                      
                        sb.Append(a2 + "@" + a3 + "0元\n");
                        jiangjin = jiangjin * Convert.ToDouble(a3);
                    }
                }

                //非混合过关
                else
                {

                    fangshi = panduan(results);
                   
                    for (int i = 0; i < zhous.Count; i++)
                    {
                        string a1 = zhous[i].Groups[1].Value;
                        string a2 = results[i].Groups[1].Value;
                        string a3 = "";
                        if (fangshi == "竞彩足球总进球数" && zhous.Count==1)
                        {
                            // 1	933516609	3/50	300	1串1	周五001>1|4.20,2|3.30,3|3.75
                            a2 = "";
                            a3 ="("+ Regex.Match(item,@">.*").Groups[0].Value.Replace(">","").Replace("|", ")@").Replace(",", "0元+(");
                        }
                        else
                        {
                            a3 = prices[i].Groups[1].Value;
                        }
                       


                        string peilv = a3;
                        string rangqiuwenzi = "";
                        if (a2.Contains("让"))
                        {
                            Dictionary<string, string> hunherangqiudic = getrangqiu_hunhe(html);

                            if (hunherangqiudic.ContainsKey(peilv))
                            {
                                if (hunherangqiudic[peilv].Contains("-"))
                                {
                                    rangqiuwenzi = " 主队让" + hunherangqiudic[peilv].Replace("-", "") + "球";
                                }
                                if (hunherangqiudic[peilv].Contains("+"))
                                {
                                    rangqiuwenzi = " 主队受让" + hunherangqiudic[peilv].Replace("+", "") + "球";
                                }
                            }
                        }



                        a2 = a2.Replace("让", "");
                        if (a2 != "胜" && a2 != "平" && a2 != "负")
                        {
                            a2 = "(" + a2 + ")";
                        }

                        sb.Append("第" + (i + 1) + "场 周" + a1 + rangqiuwenzi + "\n");
                        sb.Append("主队:" + dics["周" + a1].Replace("VS", " VS 客队:") + "\n");

                        if (fangshi == "竞彩足球总进球数" && zhous.Count == 1)
                        {
                            sb.Append( a3 + "0元\n");
                        }
                        else
                        {
                            sb.Append(a2 + "@" + a3 + "0元\n");
                        }


                        try
                        {
                            jiangjin = jiangjin * Convert.ToDouble(a3);
                        }
                        catch (Exception)
                        {

                            ;
                        }
                       
                    }


                }
               
                jiangjin = jiangjin * Convert.ToDouble(jine);
               jiangjin = Math.Round(jiangjin, 2);


                string ganxieyu = "感谢您为公益事业贡献" + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";
                string zhushu = ((Convert.ToDouble(jine) / Convert.ToDouble(beishu)) / 2).ToString();
               
                // sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + sba.ToString().Remove(sba.ToString().Length - 1, 1) + ";共" + zhushu + "注");
                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + text1[4].Replace("串", "x") +"*1注" + ";共" + zhushu + "注");



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

                //Report.Print(false);

               // Report.PrintPreview(true);
                PreviewForm theForm = new PreviewForm();
                theForm.AttachReport(Report);
                theForm.ShowDialog();
                progressBar1.Value = a;
                //label3.Text = ( ((a / (text0.Length-1)) * 100).ToString() + "%");
                Thread.Sleep(Convert.ToInt32(textBox3.Text)*1000);

            }
        }


        public void getdata_500()
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

            textBox1.Text = textsb.ToString();
            
            string[] text0 = textsb.ToString().Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            progressBar1.Value = 0;  //清空进度条
            progressBar1.Maximum = text0.Length - 1;  //清空进度条

            MatchCollection matchIds = Regex.Matches(ahtml, @"data-matchnum=""([\s\S]*?)"""); //周三001
            MatchCollection matchNames = Regex.Matches(ahtml, @"data-homesxname=""([\s\S]*?)"" data-awaysxname=""([\s\S]*?)"""); //队伍名
            Dictionary<string, string> dics = new Dictionary<string, string>();

            for (int i = 0; i < matchIds.Count; i++)
            {
              
                dics.Add(matchIds[i].Groups[1].Value, "主队："+matchNames[i].Groups[1].Value+ " VS 客队："+matchNames[i].Groups[2].Value);
            }
            for (int a = 0; a < text0.Length; a++)
            {
                string item = text0[a];

                GridppReport Report = new GridppReport();
                Report.LoadFromFile(path + "template\\a.grf");
                string[] text1 = item.Split(new string[] { "	" }, StringSplitOptions.None);
                string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                string fangshi = "竞彩" + Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;


                string beishu = Regex.Replace(text1[2], ".*/", "");
                string jine = text1[3];
                string guoguan = "过关方式 " + text1[4].Replace("串", "×");
                double jiangjin = 1;
                //周三002>让平|3.00
                MatchCollection zhous = Regex.Matches(item, @"周([\s\S]*?)>");
                MatchCollection results = Regex.Matches(item, @">([\s\S]*?)\|");
                MatchCollection prices = Regex.Matches(item, @"\|([\s\S]*?)&");
                StringBuilder sb = new StringBuilder();

                fangshi = "竞彩足球混合过关";
                    string houzhui = " 总进球数";
                    for (int i = 0; i < zhous.Count; i++)
                    {
                        string a1 = zhous[i].Groups[1].Value;
                        string a2 = results[i].Groups[1].Value;
                        string a3 = prices[i].Groups[1].Value;


                        if (a2 == "胜" || a2 == "平" || a2 == "负")
                        {
                            fangshi = "竞彩足球胜平负";
                            houzhui = " 胜平负";
                        }
                        else if (a2 == "让胜" || a2 == "让平" || a2 == "让负")
                        {
                            fangshi = "竞彩足球胜平负";
                            houzhui = " 让球胜平负";
                        }
                        else if (a2.Contains(":"))
                        {
                            fangshi = "竞彩足球比分";
                            houzhui = " 比分";
                        }
                        else if (a2.Contains("胜胜") || a2.Contains("胜平") || a2.Contains("胜负") || a2.Contains("平胜") || a2.Contains("平平") || a2.Contains("平负") || a2.Contains("负胜") || a2.Contains("负平") || a2.Contains("负负"))
                        {
                            fangshi = "竞彩足球半全场胜平负";
                            houzhui = " 半全场胜平负";
                        }
                        else
                        {
                            fangshi = "竞彩足球总进球数";
                            houzhui = " 总进球数";
                        }

                        if (a2 != "胜" && a2 != "平" && a2 != "负")
                        {
                            a2 = "(" + a2 + ")";
                        }

                        sb.Append("第" + (i + 1) + "场 " + a1 + houzhui + "\n");
                        sb.Append("主队:" + dics[a1]+ "\n");

                        sb.Append(a2 + "@" + a3 + "0元\n");
                        jiangjin = jiangjin * Convert.ToDouble(a3);
                    }
                
               
                jiangjin = jiangjin * Convert.ToDouble(jine);
                jiangjin = Math.Round(jiangjin, 2);


                string ganxieyu = "感谢您为公益事业贡献" + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";
                string zhushu = ((Convert.ToDouble(jine) / Convert.ToDouble(beishu)) / 2).ToString();

                // sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + sba.ToString().Remove(sba.ToString().Length - 1, 1) + ";共" + zhushu + "注");
                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + text1[4].Replace("串", "x") + "*1注" + ";共" + zhushu + "注");



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

                //Report.Print(false);

                // Report.PrintPreview(true);
                PreviewForm theForm = new PreviewForm();
                theForm.AttachReport(Report);
                theForm.ShowDialog();
                progressBar1.Value = a;
                //label3.Text = ( ((a / (text0.Length-1)) * 100).ToString() + "%");
                Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);

            }
        }



        public void getdata_500_danguan()
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
                a2 = a2.Replace("+(","");//去掉末尾多余字符


                StringBuilder sb = new StringBuilder();
               
                    if (a2.Contains("让胜")|| a2.Contains("让平") || a2.Contains("让负"))
                    {
                        fangshi = "竞彩足球胜平负";
                        //houzhui = " 让球胜平负";  //500解析不需要
                    }
              
                else if (a2.Contains(":"))
                    {
                        fangshi = "竞彩足球比分";
                        //houzhui = " 比分";
                    }
                    else if (a2.Contains("胜胜") || a2.Contains("胜平") || a2.Contains("胜负") || a2.Contains("平胜") || a2.Contains("平平") || a2.Contains("平负") || a2.Contains("负胜") || a2.Contains("负平") || a2.Contains("负负"))
                    {
                        fangshi = "竞彩足球半全场胜平负";
                       // houzhui = " 半全场胜平负";
                    }
                else if (a2.Contains("胜") || a2.Contains("平") || a2.Contains("负"))
                {
                    fangshi = "竞彩足球胜平负";
                   // houzhui = " 胜平负";
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
                


                jiangjin = jiangjin * Convert.ToDouble(jine);
                jiangjin = Math.Round(jiangjin, 2);


                string ganxieyu = "感谢您为公益事业贡献" + Math.Round(Convert.ToDouble(Convert.ToDouble(jine) * 0.21), 2) + "元";
                string zhushu = peilvs.Count.ToString();

              
                sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" +Convert.ToDouble(jiangjin).ToString("F2")+ "元\n单倍注数:" + "单场*1注" + ";共" + zhushu + "注");



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

                //Report.Print(false);

                // Report.PrintPreview(true);
                PreviewForm theForm = new PreviewForm();
                theForm.AttachReport(Report);
                theForm.ShowDialog();
                progressBar1.Value = a;
                //label3.Text = ( ((a / (text0.Length-1)) * 100).ToString() + "%");
                Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);

            }
        }
        private void jiexi_Load(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        }





        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Text.Contains("500"))
            {
                getdata_500_danguan();
            }
            else
            {
                getdata();
            }
           

         

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
