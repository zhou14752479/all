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

namespace 客户美团
{
    public partial class 全国工商企业信息采集 : Form
    {
        public 全国工商企业信息采集()
        {
            InitializeComponent();
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
                headers.Add("X-AUTH-TOKEN: " + token);
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
        Thread thread;
        bool status = true;
        string domail = "www.acaiji.com/shangxueba2";
        public void gettoken()
        {
            //string ahtml = method.GetUrl("http://" + domail + "/shangxueba.php?method=getcookie", "utf-8");

            //token = ahtml.Trim().Replace("\r", "").Replace("\n", "");

            token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxODYyNzk4NjM4MyIsImlhdCI6MTcyMDE3MTA3MSwiZXhwIjoxNzIyNzYzMDcxfQ.wtkqQwQV72vswlqtQ_2FbN5dl2hba6y72xyrUEow0ibSZ96Z0wQrXKpWXpVyrTVr_7Z_O0toECmk9SAzibo72w";
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

        string token = "50cdedd4679ce52bb252bad277384d29";
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入行业");
                return;
            }
           
            status = true;

            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(tyc);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            else
            {
                status = false;
            }
        }
            /// <summary>
            /// 天眼查
            /// </summary>
            public void tyc()
            {
            int count = 0;
            status = true;
                gettoken();

                StringBuilder sb = new StringBuilder();

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


            if (comboBox4.Text != "不限")
            {
                switch (comboBox4.Text)
                {
                    case "0-1年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-365)) + ",\"estiblishTimeEnd\":" + GetTimeStamp() + "");
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



                if (radioButton1.Checked == true)
                {
                // sb.Append(",\"hasPhone\":\"1\"");
                sb.Append(",\"hasMobile\":\"1\"");
            }


                if (radioButton2.Checked == true)
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
                            string postdata = "{\"sortType\":0,\"pageSize\":100,\"pageNum\":" + i + ",\"word\":\"" + textBox1.Text.Trim() + "\",\"allowModifyQuery\":1" + sb.ToString() + "}";
                            string html2 = PostUrlDefault(url, postdata, "");

                            if (html2.Contains("登录后可查看更多公司的数据") || html2.Contains("开通VIP"))
                            {
                                textBox1.Text = "正在重新获取token  请稍后";
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

                            MatchCollection legalPerson = Regex.Matches(html, @"""legalPersonName"":""([\s\S]*?)""");
                            MatchCollection regCap = Regex.Matches(html, @"""regCapital"":""([\s\S]*?)""");
                            MatchCollection StartDate = Regex.Matches(html, @"""estiblishTime"":""([\s\S]*?)""");
                            MatchCollection Address = Regex.Matches(html, @"""regLocation"":""([\s\S]*?)""");
                            MatchCollection businessScope = Regex.Matches(html, @"""businessScope"":""([\s\S]*?)""");
                            MatchCollection tel = Regex.Matches(html, @"phoneList([\s\S]*?)phoneInfoList");
                            if (names.Count == 0)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }
                        //listView1.Items.Add("");
                        for (int j = 0; j < names.Count; j++)
                            {
                                try
                                {
                                    Thread.Sleep(500);
                                   // textBox1.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + names[j].Groups[2].Value.Replace("<em>", "").Replace("</em>", "");

                                   
                                ListViewItem lv1 = new ListViewItem((listView1.Items.Count + 1).ToString());
                                lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<em>", "").Replace("</em>", ""));
                                     lv1.SubItems.Add(legalPerson[j].Groups[1].Value);
                                    lv1.SubItems.Add(regCap[j].Groups[1].Value);
                                    lv1.SubItems.Add(StartDate[j].Groups[1].Value.Replace("00:00:00.0",""));
                                 

                                listView1.Items.Add(lv1);
                                string telall= tel[j].Groups[1].Value.Replace("[", "").Replace("]", "").Replace("\"", "").Replace(":", "");

                                string tel1 = "";
                                string tel2 = "";

                                //MessageBox.Show(tel[j].Groups[1].Value);
                                    string[] text = tel[j].Groups[1].Value.Replace("[", "").Replace("]", "").Replace("\"", "").Replace(":", "").Split(new string[] { "," }, StringSplitOptions.None);
                                    foreach (var item in text)
                                    {
                                        if (!item.Contains("-"))
                                        {
                                            if (item.Length > 10)
                                            {
                                                tel2 = item;

                                            }
                                        }
                                        else
                                        {
                                            tel1 = item;
                                        }

                                    }
                                lv1.SubItems.Add(tel2);
                                lv1.SubItems.Add(tel1);
                              
                                lv1.SubItems.Add(businessScope[j].Groups[1].Value);
                                lv1.SubItems.Add(Address[j].Groups[1].Value);
                               

                                if (status == false)
                                        return;
                                count = count + 1;
                                label4.Text = count.ToString();
                                if (listView1.Items.Count > 2)
                                {
                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                }
                            }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("企业大数据：异常");
                                    textBox1.Text = ex.ToString();
                                    continue;
                                }
                            }


                            Thread.Sleep(1000);

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("企业大数据：异常"+ ex.ToString());
                        }

                    }




                }
                catch (Exception ex)
                {

                    MessageBox.Show("企业大数据：异常");
                }
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
        private void 全国工商企业信息采集_Load(object sender, EventArgs e)
        {
            
            getAllfromJson();
            getprovincefromJson();
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            listView1.SmallImageList = imgList;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1, 6);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            getcityfromJson(comboBox6.Text);
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            getareafromJson(comboBox7.Text);
        }
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
              
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
