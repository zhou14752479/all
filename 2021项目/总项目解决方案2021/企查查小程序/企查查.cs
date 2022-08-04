using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;

namespace qccxcx
{
    public partial class 企查查 : Form
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
        public 企查查()
        {
            InitializeComponent();
        }


        string domail = "www.acaiji.com/shangxueba2";
        public void gettoken()
        {
            string ahtml = method.GetUrl("http://"+domail+"/shangxueba.php?method=getcookie", "utf-8");
          
            token = ahtml.Trim().Replace("\r","").Replace("\n", "");
          
           
        }
        bool zanting = true;
        bool status = true;
        string token = "50cdedd4679ce52bb252bad277384d29";
       public static bool jihuo = true;

        /// <summary>
        /// 企查查
        /// </summary>
        public void qcc()
        {

            status = true;
            gettoken();
           
            try
            {


                for (int i = 1; i < 126; i++)
                {
                    try
                    {

                        string url = "https://xcx.qcc.com/mp-weixin/forwardApp/v3/base/advancedSearch?token="+token+ "&t=1637065131000&hasMobilePhone=MN&pageIndex="+i+"&needGroup=yes&insuredCntStart=&insuredCntEnd=&startDateBegin=&startDateEnd=&registCapiBegin=&registCapiEnd=&countyCode=&province=&sortField=&isSortAsc=&searchKey=" + System.Web.HttpUtility.UrlEncode(textBox1.Text) + "&searchIndex=default&industryV3=";

                        string html = method.GetUrl(url, "utf-8");

                        if(html.Contains("无效的sessionToken"))
                        {
                            gettoken();
                            if(i>1)
                            {
                                i = i - 1;
                            }
                            continue;
                        }
                        MatchCollection uids = Regex.Matches(html, @"""CreditCode"":""([\s\S]*?)""");
                        MatchCollection entName = Regex.Matches(html, @"""Name"":""([\s\S]*?)""");
                        MatchCollection legalPerson = Regex.Matches(html, @"""OperName"":""([\s\S]*?)""");
                        MatchCollection regCap = Regex.Matches(html, @"""RegistCapi"":""([\s\S]*?)""");
                        MatchCollection StartDate = Regex.Matches(html, @"""StartDate"":""([\s\S]*?)""");
                        MatchCollection Address = Regex.Matches(html, @"""Address"":""([\s\S]*?)""");
                       
                        MatchCollection tel= Regex.Matches(html, @"""k"":""5"",""v"":""\[{\\""k\\"":\\""([\s\S]*?)\\""");
                        if (uids.Count == 0)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        for (int j = 0; j < uids.Count; j++)
                        {
                            try
                            {
                                //Thread.Sleep(600);
                                textBox2.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + entName[i].Groups[1].Value;
                              

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                lv1.SubItems.Add(entName[j].Groups[1].Value.Replace("<em>","").Replace("</em>", ""));
                                lv1.SubItems.Add(legalPerson[j].Groups[1].Value);
                                lv1.SubItems.Add(regCap[j].Groups[1].Value);
                                lv1.SubItems.Add(StartDate[j].Groups[1].Value);
                                lv1.SubItems.Add(Address[j].Groups[1].Value);
                              
                                //if (jihuo == false)
                                //{
                                //    if (tel.Length > 4)
                                //    {
                                //        tel = tel.Substring(0, 4) + "*******";
                                //    }

                                //}

                                lv1.SubItems.Add(tel[j].Groups[1].Value);

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }
                            catch (Exception ex)
                            {
                                textBox2.Text = ex.ToString();
                                continue;
                            }
                        }

                      
                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                        textBox2.Text = ex.ToString();
                    }

                }




            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }
        /// <summary>
        /// 时间转换 毫秒级别的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetChinaTicks(DateTime dateTime)
        {
            //北京时间相差8小时
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 8, 0, 0, 0), TimeZoneInfo.Local);
            long t = (dateTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位   
            return t.ToString();
        }


        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {
                string TOKEN = "X-AUTH-TOKEN:" + token;
                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //   request.Proxy = null;//防止代理抓包
               if(token.Substring(0,1)!="e")
                {
                    token = token.Remove(0,1);
                }
               

                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("version:TYC-XCX-WX");
                headers.Add("X-AUTH-TOKEN: "+token);
              //  headers.Add("X-AUTH-TOKEN:eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxNzMzMDc5NjE4NyIsImlhdCI6MTYzOTk5MDgwOSwiZXhwIjoxNjQyNTgyODA5fQ.b_79_YOoEI-gATwz6gYXVWPmZw8fuwtG6DavohbodPRd-Lxoua6CcqeWPrmMsAM-YHZEVj2raqRYdfnktOwfdg");
                //添加头部
                //request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.Proxy = null;//禁止抓包
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/WIFI Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return("企业大数据：异常");
            }


        }

        #endregion

        /// <summary>
        /// 天眼查
        /// </summary>
        public void tyc()
        {

            status = true;
            gettoken();

            StringBuilder sb=new StringBuilder();

            string areacode = "";
            //获取区域code
            if(comboBox6.Text!= "全部")
            {
                if(comboBox7.Text== "全部")
                {
                    areacode = dicp[comboBox6.Text];
                }
                else if(comboBox8.Text== "全部")
                {
                    areacode = dicc[comboBox7.Text];
                }
                else
                {
                    areacode = dica[comboBox8.Text];
                }
                sb.Append(",\"customAreaCode\":\"" + areacode + "\"");
            }


            if (comboBox1.Text != "不限")
            {           
                sb.Append(",\"orgType\":\""+comboBox1.Text+"\"");
            }


            if (comboBox2.Text != "不限")
            {
                switch (comboBox2.Text)
                {
                    case "0-100":
                        sb.Append(",\"moneyStart\":0,\"moneyEnd\":100");
                        break;
                    case "100-200":
                        sb.Append(",\"moneyStart\":100,\"moneyEnd\":200");
                        break;
                    case "200-500":
                        sb.Append(",\"moneyStart\":200,\"moneyEnd\":500");
                        break;
                    case "500-1000":
                        sb.Append(",\"moneyStart\":500,\"moneyEnd\":1000");
                        break;
                    case "1000-":
                        sb.Append(",\"moneyStart\":1000,\"moneyEnd\":null");
                        break;
                }
            }

            if (comboBox3.Text != "不限")
            {
                if(comboBox3.Text=="在业存续")
                {
                    sb.Append(",\"regStatus\":\"存续\"");
                }
                else
                {
                    sb.Append(",\"regStatus\":\"" + comboBox3.Text + "\"");
                }
                
            }


            if (comboBox4.Text != "不限")
            {
                switch (comboBox4.Text)
                {
                    case "0-1年":
                        sb.Append(",\"estiblishTimeStart\":"+ GetChinaTicks(DateTime.Now.AddDays(-365)) + ",\"estiblishTimeEnd\":"+ GetTimeStamp() + "");
                        break;
                    case "1-2年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-730)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-365)) + "");
                        break;
                    case "2-3年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-1095)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-730)) + "");
                        break;
                    case "3-5年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-1825)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-1095)) + "");
                        break;
                    case "5-10年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-3650)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-1825)) + "");
                        break;
                    case "10年以上":
                        sb.Append(",\"estiblishTimeStart\":-4669997486139,\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-3650)) + "");
                        break;
                }
            }

            //行业
            if (comboBox5.Text != "不限")
            {
                switch(comboBox5.SelectedIndex)
                {
                    case 1:
                        sb.Append(",\"categoryGuobiao2017\":\"A\"");
                        break;
                    case 2:
                        sb.Append(",\"categoryGuobiao2017\":\"B\"");
                        break;
                    case 3:
                        sb.Append(",\"categoryGuobiao2017\":\"C\"");
                        break;
                    case 4:
                        sb.Append(",\"categoryGuobiao2017\":\"D\"");
                        break;
                    case 5:
                        sb.Append(",\"categoryGuobiao2017\":\"E\"");
                        break;
                    case 6:
                        sb.Append(",\"categoryGuobiao2017\":\"F\"");
                        break;
                    case 7:
                        sb.Append(",\"categoryGuobiao2017\":\"G\"");
                        break;
                    case 8:
                        sb.Append(",\"categoryGuobiao2017\":\"H\"");
                        break;
                    case 9:
                        sb.Append(",\"categoryGuobiao2017\":\"I\"");
                        break;
                    case 10:
                        sb.Append(",\"categoryGuobiao2017\":\"J\"");
                        break;
                    case 11:
                        sb.Append(",\"categoryGuobiao2017\":\"K\"");
                        break;
                    case 12:
                        sb.Append(",\"categoryGuobiao2017\":\"L\"");
                        break;
                    case 13:
                        sb.Append(",\"categoryGuobiao2017\":\"M\"");
                        break;
                    case 14:
                        sb.Append(",\"categoryGuobiao2017\":\"N\"");
                        break;
                    case 15:
                        sb.Append(",\"categoryGuobiao2017\":\"O\"");
                        break;
                    case 16:
                        sb.Append(",\"categoryGuobiao2017\":\"P\"");
                        break;
                    case 17:
                        sb.Append(",\"categoryGuobiao2017\":\"Q\"");
                        break;
                    case 18:
                        sb.Append(",\"categoryGuobiao2017\":\"R\"");
                        break;
                    case 19:
                        sb.Append(",\"categoryGuobiao2017\":\"S\"");
                        break;
                    case 20:
                        sb.Append(",\"categoryGuobiao2017\":\"T\"");
                        break;
                }
                
            }

            if (radioButton1.Checked == true)
            {
                sb.Append(",\"hasPhone\":\"1\"");
            }


            if (radioButton2.Checked==true)
            {
                sb.Append(",\"hasMobile\":\"1\"");
            }

            
            try
            {
                for (int i = 1; i < 51; i++)
                {
                    try
                    {

                       
                        string url = "https://capi.tianyancha.com/cloud-tempest/app/searchCompany";
                        string postdata = "{\"sortType\":0,\"pageSize\":100,\"pageNum\":"+i+",\"word\":\""+textBox1.Text.Trim()+"\",\"allowModifyQuery\":1"+sb.ToString()+"}";
                        string html2 = PostUrlDefault(url,postdata,"");
                       
                        if (html2.Contains("登录后可查看更多公司的数据") || html2.Contains("开通VIP"))
                        {
                            textBox2.Text = "正在重新获取token  请稍后";
                             gettoken();
                            Thread.Sleep(3000);
                            if (i > 1)
                            {
                                i = i - 1;
                            }
                            continue;
                        }

                       string html=Regex.Match(html2, @"companyList([\s\S]*?)brandAndAgencyList").Groups[1].Value;
                        MatchCollection names = Regex.Matches(html, @"{""id"":([\s\S]*?),""name"":""([\s\S]*?)""");
                        //textBox2.Text = html;
                        MatchCollection legalPerson = Regex.Matches(html, @"""legalPersonName"":""([\s\S]*?)""");
                        MatchCollection regCap = Regex.Matches(html, @"""regCapital"":""([\s\S]*?)""");
                        MatchCollection StartDate = Regex.Matches(html, @"""estiblishTime"":""([\s\S]*?)""");
                        MatchCollection Address = Regex.Matches(html, @"""regLocation"":""([\s\S]*?)""");
                        MatchCollection businessScope = Regex.Matches(html, @"""businessScope"":""([\s\S]*?)""");
                        MatchCollection tel = Regex.Matches(html, @"phoneList([\s\S]*?)phoneInfoList");                 
                        MatchCollection emails = Regex.Matches(html, @"emails"":""([\s\S]*?)""");
                        if (names.Count == 0)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        for (int j = 0; j < names.Count; j++)
                        {
                            try
                            {
                                Thread.Sleep(500);
                                textBox2.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + names[j].Groups[2].Value.Replace("<em>", "").Replace("</em>", "");


                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<em>", "").Replace("</em>", ""));
                               // lv1.SubItems.Add(legalPerson[j].Groups[1].Value);
                                lv1.SubItems.Add(regCap[j].Groups[1].Value);
                                lv1.SubItems.Add(StartDate[j].Groups[1].Value);
                                //lv1.SubItems.Add(Address[j].Groups[1].Value);


                                lv1.SubItems.Add(businessScope[j].Groups[1].Value);


                                string tel2 = tel[j].Groups[1].Value.Replace("[", "").Replace("]", "").Replace("\"", "").Replace(":", "");
                                if(radioButton2.Checked==true)
                                {
                                    tel2 = "";
                                    string[] text = tel[j].Groups[1].Value.Replace("[", "").Replace("]", "").Replace("\"", "").Replace(":", "").Split(new string[] { "," }, StringSplitOptions.None);
                                    foreach (var item in text)
                                    {
                                        if(!item.Contains("-"))
                                        {
                                            if(item.Length>10)
                                            {
                                                tel2 = tel2+ item;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (jihuo == false)
                                {
                                    if (tel2.Length > 4)
                                    {
                                        tel2 = tel2.Substring(0, 4) + "*******";
                                    }

                                }

                                string email = emails[j].Groups[1].Value.Replace("\\t","");


                                lv1.SubItems.Add(tel2);
                                lv1.SubItems.Add(email);

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("企业大数据：异常");
                                textBox2.Text = ex.ToString();
                                continue;
                            }
                        }


                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("企业大数据：异常");
                    }

                }




            }
            catch (Exception ex)
            {

                MessageBox.Show("企业大数据：异常");
            }
        }

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion

        private void 企查查_Load(object sender, EventArgs e)
        {
            //if (DateTime.Now > Convert.ToDateTime("2022-01-20"))
            //{
            //    TestForKillMyself();
            //}

            getAllfromJson();
            getprovincefromJson();
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"PlSUh"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }



            #endregion
        }

        public void register(string jihuoma)
        {
                
            string html = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?method=register&username=" + jihuoma + "&password=1&days=1&type=1111jihuoma", "utf-8");

         

        }

        public bool login(string jihuoma)
        {
            string html = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?username="+jihuoma+"&password=1&method=login", "utf-8");
            if(html.Contains("true"))
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
                //long expiretime = Convert.ToInt64(method.GetTimeStamp()) + 365 * 24 * 3600;
                if (ExistINIFile())
                {
                    string key = IniReadValue("values", "key");

                    string[] value = key.Split(new string[] { "asd147" }, StringSplitOptions.None);
                    if (value[0] != macmd5)
                    {
                        jihuo = false;
                        MessageBox.Show("激活失败，软件已绑定其他电脑");
                        return;
                    }

                    if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                    {
                        MessageBox.Show("激活已过期");
                        string str = Interaction.InputBox("请购买激活码,使用正式版软件！", "激活软件", "", -1, -1);
                        string fullstr = str;
                        if (login(fullstr))
                        {
                            企查查.jihuo = false; ;
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
                            if (Convert.ToInt64(method.GetTimeStamp())< Convert.ToInt64(time))  
                            {
                                if (index == "er" || index == "san")//美团一年
                                {

                                    IniWriteValue("values", "key", macmd5 + "asd147" + time);

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
                        企查查.jihuo = false; ;
                    }

                }
                else
                {
                    string str = Interaction.InputBox("请购买激活码,使用正式版软件！\r\n\r\n无激活码点击确定免费试用", "激活软件", "", -1, -1);
                    string fullstr = str.Trim();
                    if (login(fullstr))
                    {
                        企查查.jihuo = false; 
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
                        if (Convert.ToInt64(method.GetTimeStamp()) < Convert.ToInt64(time))  //200秒内有效
                        {
                            if (index == "er" || index == "san")//美团一年
                            {
                                IniWriteValue("values", "key", macmd5 + "asd147" + time);

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
                    MessageBox.Show("激活码错误,点击试用");
                    企查查.jihuo = false; ;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("激活码错误,点击试用");
                企查查.jihuo = false; ;
            }

            
        }


        #endregion
        Thread thread;
        private void button7_Click(object sender, EventArgs e)
        {

            //激活 jihuo = new 激活();
            //jihuo.Show();
            
           jihuoma();
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(tyc);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
        public void creatVcf()

        {

            string text = method.GetTimeStamp() + ".vcf";
            if (File.Exists(text))
            {
                if (MessageBox.Show("“" + text + "”已经存在，是否删除它？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                File.Delete(text);
            }
            UTF8Encoding encoding = new UTF8Encoding(false);
            StreamWriter streamWriter = new StreamWriter(text, false, encoding);
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string name = listView1.Items[i].SubItems[1].Text.Trim();
                string tel = listView1.Items[i].SubItems[7].Text.Trim();
                if (name != "" && tel != "")
                {
                    streamWriter.WriteLine("BEGIN:VCARD");
                    streamWriter.WriteLine("VERSION:3.0");

                    streamWriter.WriteLine("N;CHARSET=UTF-8:" + name);
                    streamWriter.WriteLine("FN;CHARSET=UTF-8:" + name);

                    streamWriter.WriteLine("TEL;TYPE=CELL:" + tel);



                    streamWriter.WriteLine("END:VCARD");

                }
            }
            streamWriter.Flush();
            streamWriter.Close();
            MessageBox.Show("生成成功！文件名是：" + text);



        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //gettoken();
            if (thread == null || !thread.IsAlive)
            {
                Thread thread = new Thread(creatVcf);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string token= Interaction.InputBox("请输入密码启动软件", "请输入密码", "请输入密码", -1, -1);
            //string url = "http://" + domail + "/shangxueba.php?method=setcookie";
            //string postdata = "cookie=" + System.Web.HttpUtility.UrlEncode(token);
            //string msg = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");
            //MessageBox.Show(msg.Trim());
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        Dictionary<string, string> dicp = new Dictionary<string, string>();
        Dictionary<string, string> dicc = new Dictionary<string, string>();
        Dictionary<string, string> dica = new Dictionary<string, string>();

        Dictionary<string, string> dic_value = new Dictionary<string, string>();



        public void getAllfromJson()
        {
            StreamReader sr = new StreamReader(path + "data\\city.json", method.EncodingType.GetTxtType(path + "data\\city.json"));
            //一次性读取完 
            string jsonText = sr.ReadToEnd();
          
            MatchCollection values = Regex.Matches(jsonText, @"""name"":""([\s\S]*?)"",""areaCode"":""([\s\S]*?)""");
            for (int i = 0; i < values.Count; i++)
            {
                if (!dic_value.ContainsKey(values[i].Groups[1].Value))
                {
                    dic_value.Add(values[i].Groups[1].Value, values[i].Groups[2].Value);
                   
                }

            }
            sr.Close();
        }

        public void getprovincefromJson()
        {

            StreamReader sr = new StreamReader(path + "data\\city.json", method.EncodingType.GetTxtType(path + "data\\city.json"));
            //一次性读取完 

            string jsonText = sr.ReadToEnd();
            MatchCollection provinces = Regex.Matches(jsonText, @"{""province"":""([\s\S]*?)""");
            for (int i = 0; i < provinces.Count; i++)
            {
                if (!dicp.ContainsKey(provinces[i].Groups[1].Value))
                {
                    dicp.Add(provinces[i].Groups[1].Value, dic_value[provinces[i].Groups[1].Value]);
                    comboBox6.Items.Add(provinces[i].Groups[1].Value);

                }

            }
            sr.Close();

        }

        public void getcityfromJson(string province)
        {


            comboBox7.Items.Clear();
            comboBox7.Items.Add("全部");
            StreamReader sr = new StreamReader(path + "data\\city.json", method.EncodingType.GetTxtType(path + "data\\city.json"));
            //一次性读取完 

            string jsonText = sr.ReadToEnd();
            MatchCollection provinces = Regex.Matches(jsonText, @"{""province"":""([\s\S]*?)"",""city"":""([\s\S]*?)"",""name"":""([\s\S]*?)"",""areaCode"":""([\s\S]*?)""");
            for (int i = 0; i < provinces.Count; i++)
            {
                if (provinces[i].Groups[1].Value == province)
                {
                    if (!dicc.ContainsKey(provinces[i].Groups[2].Value))
                    {
                        dicc.Add(provinces[i].Groups[2].Value, dic_value[provinces[i].Groups[2].Value]);
                        comboBox7.Items.Add(provinces[i].Groups[2].Value);
                       
                    }
                }

            }
            sr.Close();

            if (comboBox6.Text == "广东省")
            {
                dicc.Add("东莞市", "00441900V2020");
                comboBox7.Items.Add("东莞市");
            }
        }

        public void getareafromJson(string city)
        {
            comboBox8.Items.Clear();
            comboBox8.Items.Add("全部");
           StreamReader sr = new StreamReader(path + "data\\city.json", method.EncodingType.GetTxtType(path + "data\\city.json"));
            //一次性读取完 

            string jsonText = sr.ReadToEnd();
            MatchCollection provinces = Regex.Matches(jsonText, @"{""province"":""([\s\S]*?)"",""city"":""([\s\S]*?)"",""name"":""([\s\S]*?)"",""areaCode"":""([\s\S]*?)""");
            for (int i = 0; i < provinces.Count; i++)
            {
                if (provinces[i].Groups[2].Value == city)
                {
                    if (!dica.ContainsKey(provinces[i].Groups[3].Value))
                    {
                        dica.Add(provinces[i].Groups[3].Value, provinces[i].Groups[4].Value);
                        comboBox8.Items.Add(provinces[i].Groups[3].Value);

                    }
                }

            }
            sr.Close();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

            getcityfromJson(comboBox6.Text);
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            getareafromJson(comboBox7.Text);
        }

        private void 企查查_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
