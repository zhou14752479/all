using gregn6Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    class function
    {
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
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

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

        string path = AppDomain.CurrentDomain.BaseDirectory;
      
        public static string getsuijima()
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同

            string value1 = "";
            string value2 = "";
            string value3 = "";
            for (int i = 0; i < 16; i++)
            {

                int suiji = rd.Next(0, 10);
                value1 = value1 + suiji;
            }

            for (int i = 0; i < 8; i++)
            {

                int suiji = rd.Next(0, 10);
                value2 = value2 + suiji;
            }



            string zimu = "ABCDE0123456789";

            Random rd2 = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
            
            for (int i = 0; i < 8; i++)
            {
                
                int suiji = rd.Next(0, 15);
                value3 = value3 + zimu[suiji];
            }

            return value1 + " "+ value2 + " "+value3;
        }


        #region 混合过关
        public void getdata(GridppReport Report,string html,string ahtml)
        {

            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");


            if (setup.muban != "" && setup.muban != null)
            {
                Report.LoadFromFile(path + "template\\"  + "a.grf");
            }
            else
            {
                Report.LoadFromFile(path + "template\\a.grf");
            }
           
            string fangshi = "竞彩"+Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            string suiji = bianma + function.getsuijima();
            string zhanhao = haoma;
            string jiangjin= Regex.Match(html, @"<span id=""bonus"">([\s\S]*?)</span>").Groups[1].Value;


            string guoguan = Regex.Match(html, @"checked="""">([\s\S]*?)</span>").Groups[1].Value;
            string beishu = Regex.Match(html, @"x \d{1,2}倍").Groups[0].Value.Replace("x","").Replace("倍", "").Trim();
           
            string jine = Regex.Match(html, @"<span id=""consume"">([\s\S]*?)</span>").Groups[1].Value;

           string zhushu = (Convert.ToInt32(jine) / 2).ToString();
            string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss").Replace("-", "/");



            MatchCollection matchIds = Regex.Matches(html, @"class=""mCodeCls""([\s\S]*?)>([\s\S]*?)</td>");
            MatchCollection matchNames = Regex.Matches(html, @"<span class=""AgainstInfo"">([\s\S]*?)</span>");
           
            Dictionary<string, string> dics = new Dictionary<string, string>();
            for (int i = 0; i < matchIds.Count; i++)
            {
                string matchname = Regex.Replace(matchNames[i].Groups[1].Value, "<[^>]+>", "");
                matchname = Regex.Replace(matchname, @"\[.*?\]", "");
                dics.Add(matchIds[i].Groups[2].Value, matchname);
            }

            StringBuilder sb = new StringBuilder();


            //MatchCollection value1 = Regex.Matches(html, @"<span class=""delSelTrBtn"">([\s\S]*?)</tr>");
            //for (int i = 0; i < value1.Count; i++)
            //{

            //    string a1 = Regex.Match(value1[i].Groups[1].Value, @"</td><td>周([\s\S]*?)</td>").Groups[1].Value;
            //    string a2 = Regex.Match(value1[i].Groups[1].Value, @"class=""selOption"">([\s\S]*?)</span>").Groups[1].Value;
            //    sb.Append("第" + (i + 1) + "场  周" + a1 + "\n");
            //    sb.Append(dics["周" + a1] + "\n");
            //    sb.Append(a2 + "@" + "\n");
            //}

            MatchCollection value1 = Regex.Matches(html, @"<span>周([\s\S]*?)</span>");
            MatchCollection value2 = Regex.Matches(html, @"204\);"">周([\s\S]*?)</span>");

            List<string> lists = new List<string>();
            Dictionary<string, string> resultdics = new Dictionary<string, string>();

            for (int i = 0; i < value1.Count; i++)
            {
                string a1 = Regex.Match("周" + value1[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + " 胜平负";
                string a2 = Regex.Match(value1[i].Groups[1].Value, @"\(([\s\S]*?)\)").Groups[1].Value+"元";
                if (resultdics.ContainsKey(a1))
                {
                    if (!lists.Contains(a1+a2))
                    {
                        lists.Add(a1+a2);
                         resultdics[a1] = resultdics[a1]+ "+" + a2;
                    }
                  
                }
                else
                {
                    lists.Add(a1+a2);
                    resultdics.Add(a1 , "主队:" + dics["周" + a1.Replace("胜平负", "").Replace("让球", "").Trim()].Replace(" VS ", "VS客队:") + "\n" + a2);

                }
               
            }



            for (int i = 0; i < value2.Count; i++)
            {
                string a1 = Regex.Match("周" + value2[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value + " 让球胜平负";
                string a2 = Regex.Match(value2[i].Groups[1].Value, @"\(([\s\S]*?)\)").Groups[1].Value + "元";
                if (resultdics.ContainsKey(a1))
                {
                    if (!lists.Contains(a2))
                    {
                        lists.Add(a2);
                        resultdics[a1] =  resultdics[a1] + "+" + a2;
                    }

                }
                else
                {
                    lists.Add(a2);
                    resultdics.Add(a1, "主队:" + dics["周" + a1.Replace("胜平负","").Replace("让球","").Trim()].Replace("VS", "VS客队:") + "\n" + a2);

                }

            }


            int a = 1;
            foreach (var item in resultdics.Keys)
            {

                sb.Append("第" + a + "场周" + item + "\n"+resultdics[item]+"\n");
                a = a + 1;
            }

            sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:"+jiangjin+"元\n单倍注数:"+guoguan+"*"+zhushu+"注;共"+zhushu+"注");

          

            Report.ParameterByName("suiji").AsString = suiji;
            Report.ParameterByName("fangshi").AsString = fangshi;
            Report.ParameterByName("leixing").AsString = leixing;
            Report.ParameterByName("guoguan").AsString = "过关方式 "+guoguan;
            Report.ParameterByName("beishu").AsString = beishu;

            Report.ParameterByName("jine").AsString = jine;
            Report.ParameterByName("neirong").AsString = sb.ToString();


            Report.ParameterByName("dizhi").AsString = address;
            Report.ParameterByName("zhanhao").AsString = haoma;
            Report.ParameterByName("time").AsString = time;
        }

        #endregion

        #region 胜平负/让球胜平负
        public void getdata_shengpingfu(GridppReport Report, string html, string ahtml)
        {

            string address = IniReadValue("values", "address");
            string haoma = IniReadValue("values", "haoma");
            string bianma = IniReadValue("values", "bianma");


            if (setup.muban != "" && setup.muban != null)
            {
                Report.LoadFromFile(path + "template\\"  + "a1.grf");
            }
            else
            {
                Report.LoadFromFile(path + "template\\a1.grf");
            }

            string fangshi = "竞彩" + Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            string leixing = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
            string suiji = bianma + function.getsuijima();
            string zhanhao = haoma;
            string jiangjin = Regex.Match(html, @"<span id=""bonus"">([\s\S]*?)</span>").Groups[1].Value;


            // string guoguan = Regex.Match(html, @"checked="""" index=""0"">([\s\S]*?)<").Groups[1].Value;  //需要修改
            string guoguan = "1场-单场固定";
            string beishu = Regex.Match(html, @"x\d{1,2}倍").Groups[0].Value.Replace("x", "").Replace("倍", "").Trim();

            string jine = Regex.Match(html, @"<span id=""consume"">([\s\S]*?)</span>").Groups[1].Value;

            string zhushu = ((Convert.ToInt32(jine) / 2)/(Convert.ToInt32(beishu))).ToString();
            string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss").Replace("-", "/");


            
            //MatchCollection matchIds = Regex.Matches(html, @"singleindex=([\s\S]*?)周([\s\S]*?)</td>");
            MatchCollection matchIds = Regex.Matches(html, @"dataindex=([\s\S]*?)周([\s\S]*?)</td>");
            if (fangshi == "竞彩足球胜平负")
            {
                matchIds = Regex.Matches(html, @"singleindex=([\s\S]*?)周([\s\S]*?)</td>");
            }

            if (fangshi == "竞彩足球比分")
            {
                matchIds = Regex.Matches(html, @"lindex=([\s\S]*?)周([\s\S]*?)</td>");
            }

           
            MatchCollection matchNames = Regex.Matches(html, @"球队介绍页([\s\S]*?)>([\s\S]*?)</a><label>");
          

            //MessageBox.Show("周" + Regex.Replace(matchIds[0].Groups[2].Value, "<[^>]+>", ""));
            //MessageBox.Show(Regex.Replace(matchNames[0].Groups[2].Value, "<[^>]+>", ""));
            Dictionary<string, string> dics = new Dictionary<string, string>();
            for (int i = 0; i < matchIds.Count; i++)
            {
                string matchname = Regex.Replace(matchNames[i].Groups[2].Value, "<[^>]+>", "");
                matchname = Regex.Replace(matchname, @"\[.*?\]", "");

                dics.Add("周" + Regex.Replace(matchIds[i].Groups[2].Value, "<[^>]+>", ""), matchname);
            }

            StringBuilder sb = new StringBuilder();

           


            string body = Regex.Match(html, @"打印</a>([\s\S]*?)</tr></table></div>").Groups[1].Value;
            MatchCollection value1 = Regex.Matches(body, @"<span title=""([\s\S]*?)"">([\s\S]*?)\(([\s\S]*?)\)</span>");
            MatchCollection value2 = Regex.Matches(body, @"204\);"">周([\s\S]*?)</span>");  //灰色区域 暂时不处理


            List<string> lists = new List<string>();
            Dictionary<string, string> resultdics = new Dictionary<string, string>();
           
            if (value1.Count == 0)  //单关 只选择一场比赛
            {
                value1 = Regex.Matches(body, @"<span>周([\s\S]*?)\(([\s\S]*?)\)</span>");
                MatchCollection value1shuzhi = Regex.Matches(body, @"</span>x([\s\S]*?)倍</td>([\s\S]*?)</td>");
                for (int i = 0; i < value1.Count; i++)
                {
                    double a3 = Convert.ToDouble(Regex.Replace(value1shuzhi[i].Groups[2].Value, "<[^>]+>", "")) / (Convert.ToDouble(value1shuzhi[i].Groups[1].Value)*2);

                  string  a33= Regex.Replace(a3.ToString(), @""".*", "");
                    string a1 = "周" + value1[i].Groups[1].Value;
                    string a2 = "("+value1[i].Groups[2].Value+ ")" + "@"+a33+"元";

                   
                    if (resultdics.ContainsKey(a1))
                    {
                        if (!lists.Contains(a1 + a2))
                        {
                            lists.Add(a1 + a2);
                            resultdics[a1] = resultdics[a1] + "+" + a2;
                        }

                    }
                    else
                    {
                        lists.Add(a1 + a2);
                        resultdics.Add(a1, "主队:" + dics[a1].Replace(" VS ", "VS客队:") + "\n" + a2);

                    }

                }
            }

            else
            {

               
                for (int i = 0; i < value1.Count; i++)
                {
                    string a1 = value1[i].Groups[2].Value;

                    string a33 = Regex.Replace(value1[i].Groups[1].Value, @""".*", "");
                    string a2 = "("+value1[i].Groups[3].Value+ ")" + "@" + a33+"元";

                    if (resultdics.ContainsKey(a1))
                    {
                        if (!lists.Contains(a1 + a2))
                        {
                            lists.Add(a1 + a2);
                            resultdics[a1] = resultdics[a1] + "+" + a2;
                        }

                    }
                    else
                    {
                        lists.Add(a1 + a2);
                        resultdics.Add(a1, "主队:" + dics[a1].Replace(" VS ", "VS客队:") + "\n" + a2);

                    }

                }
            }



            //for (int i = 0; i < value2.Count; i++)
            //{
            //    string a1 = Regex.Match("周" + value2[i].Groups[1].Value, @"周([\s\S]*?)\(").Groups[1].Value;
            //    string a2 = Regex.Match(value2[i].Groups[1].Value, @"\(([\s\S]*?)\)").Groups[1].Value;
            //    if (resultdics.ContainsKey(a1))
            //    {
            //        if (!lists.Contains(a2))
            //        {
            //            lists.Add(a2);
            //            resultdics[a1] = resultdics[a1] + "+" + a2;
            //        }

            //    }
            //    else
            //    {
            //        lists.Add(a2);
            //        resultdics.Add(a1, "主队:" + dics[a1].Replace("VS", "VS客队:") + "让球胜平负\n" + a2);

            //    }

            //}


            int a = 1;
            foreach (var item in resultdics.Keys)
            {

                sb.Append("第" + a + "场" + item + "\n" + resultdics[item] + "\n");
                a = a + 1;
            }
            string guoguan1 = "单场";
            sb.Append("(选项固定奖金额为每1元投注对应的奖金额)\n本票最高可能固定奖金:" + jiangjin + "元\n单倍注数:" + guoguan1 + "*" + zhushu + "注;共" + zhushu + "注");



            Report.ParameterByName("suiji").AsString = suiji;
            Report.ParameterByName("fangshi").AsString = fangshi;
            Report.ParameterByName("leixing").AsString = leixing;
            Report.ParameterByName("guoguan").AsString = guoguan;
            Report.ParameterByName("beishu").AsString = beishu;

            Report.ParameterByName("jine").AsString = jine;
            Report.ParameterByName("neirong").AsString = sb.ToString();


            Report.ParameterByName("dizhi").AsString = address;
            Report.ParameterByName("zhanhao").AsString = haoma;
            Report.ParameterByName("time").AsString = time;
        }

        #endregion

    }
}
