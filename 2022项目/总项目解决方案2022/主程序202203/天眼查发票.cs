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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202203
{
    public partial class 天眼查发票 : Form
    {
        public 天眼查发票()
        {
            InitializeComponent();
        }

        private void 天眼查发票_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.tianyancha.com/");
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"mpxO"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
               
                return;
            }

            #endregion
            getAllfromJson();
            getprovincefromJson();
        }

     

        Thread thread;
        string domail = "www.acaiji.com/shangxueba2";
        public void gettoken()
        {
            string ahtml = method.GetUrl("http://" + domail + "/shangxueba.php?method=getcookie", "utf-8");

            token = ahtml.Trim().Replace("\r", "").Replace("\n", "");


        }
        bool zanting = true;
        bool status = true;
        string token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxMzMzNzkzNzY5MSIsImlhdCI6MTY0OTkxNjAzMywiZXhwIjoxNjUyNTA4MDMzfQ.1WDcVoFeP5TPO5FUZO_3LB30wVAkwheOEd_2Vb7JUW8B8Fv01UK3fodTx-2UGGje7Sc7MTwR2bazjG1jpyqw4Q";
       

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
                if (token.Substring(0, 1) != "e")
                {
                    token = token.Remove(0, 1);
                }


                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("version:TYC-XCX-WX");
                headers.Add("X-AUTH-TOKEN:" + token);
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

                return ("企业大数据：异常");
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
           
            StringBuilder sb = new StringBuilder();

            string areacode = "";
            //获取区域code
            if (comboBox6.Text != "全部")
            {
                if (comboBox7.Text == "全部")
                {
                    areacode = dicp[comboBox6.Text];
                }
                else if (comboBox8.Text == "全部")
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
                sb.Append(",\"orgType\":\"" + comboBox1.Text + "\"");
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
                if (comboBox3.Text == "在业存续")
                {
                    sb.Append(",\"regStatus\":\"存续\"");
                }
                else
                {
                    sb.Append(",\"regStatus\":\"" + comboBox3.Text + "\"");
                }

            }


            //if (comboBox4.Text != "不限")
            //{
            //    switch (comboBox4.Text)
            //    {
            //        case "0-1年":
            //            sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-365)) + ",\"estiblishTimeEnd\":" + GetTimeStamp() + "");
            //            break;
            //        case "1-2年":
            //            sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-730)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-365)) + "");
            //            break;
            //        case "2-3年":
            //            sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-1095)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-730)) + "");
            //            break;
            //        case "3-5年":
            //            sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-1825)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-1095)) + "");
            //            break;
            //        case "5-10年":
            //            sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-3650)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-1825)) + "");
            //            break;
            //        case "10年以上":
            //            sb.Append(",\"estiblishTimeStart\":-4669997486139,\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-3650)) + "");
            //            break;
            //    }
            //}

            sb.Append(",\"estiblishTimeStart\":"+ GetChinaTicks(dateTimePicker1.Value)+ ",\"estiblishTimeEnd\":" + GetChinaTicks(dateTimePicker2.Value) + "");

            //行业
            if (comboBox5.Text != "不限")
            {
                switch (comboBox5.SelectedIndex)
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

          

            try
            {
                for (int i = 1; i < 51; i++)
                {
                    try
                    {


                        string url = "https://capi.tianyancha.com/cloud-tempest/app/searchCompany";
                        string postdata = "{\"sortType\":0,\"pageSize\":100,\"pageNum\":" + i + ",\"word\":\"" + textBox1.Text.Trim() + "\",\"allowModifyQuery\":1" + sb.ToString() + "}";
                      
                        string html2 = PostUrlDefault(url, postdata, "");
                        //textBox3.Text = html2;

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

                        string html = Regex.Match(html2, @"companyList([\s\S]*?)brandAndAgencyList").Groups[1].Value;
                        MatchCollection names = Regex.Matches(html, @"{""id"":([\s\S]*?),""name"":""([\s\S]*?)""");

                        string totalPage = Regex.Match(html2, @"""companyTotalPage"":([\s\S]*?),").Groups[1].Value;


                        //搜索结果
                        try
                        {
                            label9.Text = "搜索结果：" + Convert.ToInt32(totalPage) * 100;
                            if (names.Count < 100 && names.Count > 0)
                            {
                                label9.Text = "搜索结果：" + (Convert.ToInt32(totalPage) - 1) * 100 + names.Count;
                            }
                        }
                        catch (Exception)
                        {

                            
                        }



                        if (names.Count == 0)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        for (int j = 0; j < names.Count; j++)
                        {
                           
                            string uid = names[j].Groups[1].Value.Replace("<em>", "").Replace("</em>", "");
                            string ahtml = method.GetUrlWithCookie("https://www.tianyancha.com/cloud-wechat/qrcode.json?gid=" + uid + "&_=1649821424041", cookie, "utf-8");
                            string name = Regex.Match(ahtml, @"""name"":""([\s\S]*?)""").Groups[1].Value;
                            string taxnum = Regex.Match(ahtml, @"""taxnum"":""([\s\S]*?)""").Groups[1].Value;
                            string address = Regex.Match(ahtml, @"""address"":""([\s\S]*?)""").Groups[1].Value;
                            string phone = Regex.Match(ahtml, @"""phone"":""([\s\S]*?)""").Groups[1].Value;
                            string bank = Regex.Match(ahtml, @"""bank"":""([\s\S]*?)""").Groups[1].Value;
                            string bankAccount = Regex.Match(ahtml, @"""bankAccount"":""([\s\S]*?)""").Groups[1].Value;
                            textBox2.Text = "正在获取：" + name;


                            //筛选电话
                            if (radioButton1.Checked==true && phone.Trim()=="")
                            {
                                textBox2.Text =  name+"：号码为空跳过....";
                                continue;
                            }
                            if (radioButton2.Checked == true && phone.Trim() != "")
                            {
                                textBox2.Text = name + "：号码不为空跳过....";
                                continue;
                            }


                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(name);
                            lv1.SubItems.Add(taxnum);
                            lv1.SubItems.Add(address);
                            lv1.SubItems.Add(phone);
                            lv1.SubItems.Add(bank);
                            lv1.SubItems.Add(bankAccount);



                            //筛选关键词导出文本
                            string filename = "data";
                            if (checkBox2.Checked == true)
                            {
                                if (bank.Contains("建设") || bank.Contains("建行"))
                                {
                                    filename = "建行";
                                }
                                else if (bank.Contains("浦发") || bank.Contains("浦东") || bank.Contains("浦行"))
                                {
                                    filename = "浦发";
                                }
                                else if (bank.Contains("招商") || bank.Contains("招行"))
                                {
                                    filename = "招商";

                                }
                                else
                                {
                                    filename = "其他";
                                }
                            }
                            Thread.Sleep(2000);
                            if (bank=="")
                            {
                                continue;
                            }

                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\"+filename+".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));

                            //旧
                            //sw.WriteLine(name + "-" + taxnum + "-" + address + "-" + phone + "-" + bank + "-" + bankAccount);
                            //新

                            sw.WriteLine(name + "----" + bank + "----" + bankAccount);
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(3000);
                        }


                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("企业大数据：异常" + ex.ToString());
                    }

                }




            }
            catch (Exception ex)
            {

                MessageBox.Show("企业大数据：异常"+ ex.ToString());
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
        string cookie = "";
        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://www.tianyancha.com/");
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

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

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

        string path = AppDomain.CurrentDomain.BaseDirectory;
        Dictionary<string, string> dicp = new Dictionary<string, string>();
        Dictionary<string, string> dicc = new Dictionary<string, string>();
        Dictionary<string, string> dica = new Dictionary<string, string>();

        Dictionary<string, string> dic_value = new Dictionary<string, string>();

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            getcityfromJson(comboBox6.Text);
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            getareafromJson(comboBox7.Text);
        }

        private void 天眼查发票_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
