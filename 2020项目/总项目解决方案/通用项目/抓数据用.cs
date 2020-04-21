using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 通用项目
{
    public partial class 抓数据用 : Form
    {
        public 抓数据用()
        {
            InitializeComponent();
        }

        bool zanting = true;
        public static string COOKIE = "";

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrl(string url, string postData)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/json";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }
        
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp+"0000" );
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);

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


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "ali_apache_id=11.9.134.78.1562220413259.182473.2; t=196a840aec96a0b0eaf06ec105e442f8; cna=8QJMFUu4DhACATFZv2JYDtwd; tfstk=cR0PB0VQUULy5FtDfPzUdS_ZkXrRZplotZP_qcYy_PH93crliP6LGqXTgWw9ouf..; acs_usuc_t=acs_rt=d11d3a621aa843dcb59aee559a88d2e1; _m_h5_tk=e6264148d11cd21258e70c9fefba68b5_1585022581073; _m_h5_tk_enc=2020cd223596a802c67a94d19e3423b9; v=0; cookie2=178f844925ada1d21cb8b57e2547da62; _tb_token_=7e97e5e37631; _samesite_flag_=true; _hvn_login=4; xman_us_t=ctoken=bpaxluccx0lf&l_source=alibaba&x_user=tXfZ9/bOsEdPP7MDmir80bF/xmduLijhx7wabENBR64=&x_lid=cn1360010753xkqd&sign=y&need_popup=y; xman_us_f=x_locale=zh_CN&x_l=1&last_popup_time=1585014363116&x_user=CN|sherry|zhao|cgs|978609901&no_popup_today=n; intl_locale=zh_CN; intl_common_forever=zksRQwkaNWVKhfIiQ9Frb0wlj5d4f51s+uAlej5Nd5imToXDglX5+w==; xman_f=vY3TdyEgKByUPDz/KPmGgrzSa98uQLlMeEC1UdRZU+YVfsr6GAubwUcDFbNCeY/NCNggIxW1aD91IVzMyGvqKwRVQXe7ewCt51L2l8LhfovrU5qW1Z/rv9NtC+FCbsgT56Wp9jUJtXQXHr7RFAP1DDbO6eAbKq0MBqW30Mfak4GopGopi+Zch3B7lwYofwpeBKHTILScvEIvASVc5mqPDbq8fpxO2jfrrYwezwDxIX/Wk8gzeIclOxjvHTOfMi2gNbGuTh2S5YWZK7tEd815m3Kf+yuSv+vAN6+W5XaWqvH45WFXE6v0RyY1myvgYY1KfZqJipMIOCA2Q2wg++3IWv6hcHqzf1jlQ0Ru8o7b1waTcaABMOjQL6J3E8yHPJLQ4N1TZer95W5JlGd/OmIAcsIE33/yNFvq; ali_apache_tracktmp=W_signed=Y; csg=8f24cc4e; ali_apache_track=ms=|mt=3|mid=cn1360010753xkqd; l=dBgBNOcqqmaTCs8EBOfGC42gwx7T1tdjMsPrE67l5IB1t6bn9dL9tHwEqemJj3QQEt5vfFxrVHyCZR3MWD4LRxZsp3vdDNsjuapw8e1..; isg=BLGxXrc-SzlNH-TYJLipU_P9wD1LniUQl9nzUJPvRXscuuUM3ezx4JaY3E7ccr1I; xman_t=dlkTFlWElabXfmqy8JktQZZGqn/iQwWoxbTQIQvywCFrKvvlfuKQrzELJGm4nKP8qlacRgRSV8yA0s77q3CDe0Ejkh7WLRgD+U02xUg/QGm21UMZpckEcYJ5fSICxAErVTddgLgtsEGbvZxQHGcV7eadYsXOxnihx26GrMNE3Ryxs1Ti/JHJ5jH/jaBTnWmOnMTufACGGrCTlPaRfsyyqdtvP/4AaI845kTj/p0WtBXZGS0joJhot4BSjVLF/67m9WxawUy/6cB7glhhsIHOMgS+7vAduRuM8Dd4/x8zH/YdZwoTmFuDdIt3g8bXbaqwsuokgDWDr2PP1lCRfqIeAK+hLQ9TT7w1MLoHZOnSR3kF4X6k+NMsGLCuEh1kRrVGu4lszGwRiRCaUpFLEF4DfF0QBB1WIp9VUBwi3NONFzwmnMzg7jxfyqNJ9CTBzzMsSdt6EET0KFwJNuX/Jjw1NmLJzeJdHev9nc/lYf/83hk+K/nwP6egOviT4fkLRMUpB/t4h96tCtq2ZuHW6CiHFgD3TZAGIUGUmNDudb4n//OXrShRo9nLtKQPvSkUEYqbVRrZYTRW7eHZpigmxngSUACVVOzJ0Dnyhp5gJxsZuIjN1A07+ppvK4NAySB1k4js4CxPZtjbmsDW49hCHf+Fvcrq1pyS8eA0reBdgiUGdgnJzNXTe5UdXl0HRXKmy6KUpOnCjOYAPqs=";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        public void qichacha()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text,Encoding.GetEncoding("utf-8"));
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    string url = "https://xcx.qichacha.com/wxa/v1/base/advancedSearchNew?searchKey=" + System.Web.HttpUtility.UrlEncode(array[i].Trim())+"&searchIndex=&sortField=&isSortAsc=false&province=&cityCode=&countyCode=&industryCode=&subIndustryCode=&industryV3=&token=" + textBox2.Text + "&startDateBegin=&startDateEnd=&registCapiBegin=&registCapiEnd=&insuredCntStart=&insuredCntEnd=&coyType=&statusCode=&hasPhone=&hasMobilePhone=&hasEmail=&hasTM=&hasPatent=&hasSC=&hasShiXin=&hasFinance=&hasIPO=&pageIndex=1&searchType=0";

                    string html = method.GetUrl(url, "utf-8");

                    if (html.Contains("已失效"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add("失效");
                        MessageBox.Show("已失效");
                    }

                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        Match tellit = Regex.Match(html, @"TelList"":""\[([\s\S]*?)\]");
                        MatchCollection tel = Regex.Matches(tellit.Groups[1].Value, @"t\\"":\\""([\s\S]*?)\\");

                        foreach (Match t in tel)
                        {
                            sb.Append(t.Groups[1].Value + "#");
                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add(sb.ToString());
                    }

                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    // continue;
                }
               
            }

        }


        
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
           
                    for (int j = 0; j < 247; j++)
                    {
                        string url = "http://gs.amac.org.cn/amac-infodisc/api/pof/manager?rand=0.3952054052459051&page=" + j + "&size=100";

                        string html = PostUrl(url, "{}");
                        MatchCollection a1 = Regex.Matches(html, @"""officeProvince"":""([\s\S]*?)""");
                        MatchCollection a2 = Regex.Matches(html, @"""managerName"":""([\s\S]*?)""");
                        MatchCollection a3 = Regex.Matches(html, @"""artificialPersonName"":""([\s\S]*?)""");
                        MatchCollection a4 = Regex.Matches(html, @"""registerDate"":([\s\S]*?),");
                        MatchCollection a5 = Regex.Matches(html, @"""establishDate"":([\s\S]*?),");
                        MatchCollection a6 = Regex.Matches(html, @"""primaryInvestType"":""([\s\S]*?)""");

                        MatchCollection ids = Regex.Matches(html, @"""url"":""([\s\S]*?)""");

                        for (int a = 0; a < ids.Count; a++)

                        {

                    try
                    {
                        string strhtml = method.GetUrl("http://gs.amac.org.cn/amac-infodisc/res/pof/manager/" + ids[a].Groups[1].Value, "utf-8");
                        Match zhuce = Regex.Match(strhtml, @"注册资本\(万元\)\(人民币\)</td>([\s\S]*?)</td>");
                        Match bili = Regex.Match(strhtml, @"注册资本实缴比例</td>([\s\S]*?)</td>");

                        //FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "新文档.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                        //StreamWriter sw = new StreamWriter(fs1);
                        //string value = a1[a].Groups[1].Value + "#" + a2[a].Groups[1].Value + "#" + a3[a].Groups[1].Value + "#" + ConvertStringToDateTime(a4[a].Groups[1].Value).ToString() + "#" + ConvertStringToDateTime(a5[a].Groups[1].Value).ToString() + "#" + zhuce.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim() + "#" + bili.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim() +"#"+ a6[a].Groups[1].Value;
                        //sw.WriteLine(value);
                        //sw.Close();
                        //fs1.Close();

                        
                        //label1.Text = zhi.ToString();
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1[a].Groups[1].Value);
                        lv1.SubItems.Add(a2[a].Groups[1].Value);
                        lv1.SubItems.Add(a3[a].Groups[1].Value);
                        lv1.SubItems.Add(ConvertStringToDateTime(a4[a].Groups[1].Value).ToString());
                        lv1.SubItems.Add(ConvertStringToDateTime(a5[a].Groups[1].Value).ToString());
                        lv1.SubItems.Add(zhuce.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim());
                        lv1.SubItems.Add(bili.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim());
                        lv1.SubItems.Add(a6[a].Groups[1].Value);
                    }
                    catch 
                    {

                        continue;
                    }
                            

                            

                        }

                        
                     }
                }




        /// <summary>
        /// 阿里巴巴国际站
        /// </summary>
        public void alibaba()
        {

            for (int i= 1; i< 314; i++)
            {
                string url = "https://hz-mydata.alibaba.com/self/.json?action=CommonAction&iName=getVisitors&isVip=true&0.7587257170853245&ctoken=bpaxluccx0lf&dmtrack_pageid=da5dc07a0bba829d5e79669d1710a38dcbaf56abff&orderBy=&orderModel=&pageSize=15&pageNO="+i+"&statisticsType=day&selected=0&startDate=2020-02-21&endDate=2020-03-22&searchKeyword=&buyerRegion=&buyerCountry=&subMemberSeq=&isMcFb=false&isAtmFb=false&mailable=false&mailed=false&hasRemarks=false&statisticType=os&desTime=1585014429629";

                string html = GetUrl(url,"utf-8");
                MatchCollection a1s = Regex.Matches(html, @"""visitorId"":""([\s\S]*?)""");
                MatchCollection a2s = Regex.Matches(html, @"""buyerCountry"":""([\s\S]*?)""");
                MatchCollection a3s = Regex.Matches(html, @"""staySecond"":([\s\S]*?),");
                MatchCollection uids = Regex.Matches(html, @"""buyerCookieId"":""([\s\S]*?)""");
                MatchCollection startdates = Regex.Matches(html, @"""statDate"":""([\s\S]*?)""");



                for (int a = 0; a < a1s.Count; a++)

                {

                    try
                    {

                        string URL = "https://hz-mydata.alibaba.com/self/.json?action=CommonAction&iName=getVisitorDetails&0.676711856315128&ctoken=bpaxluccx0lf&dmtrack_pageid=da5dc07a0bba829d5e79669d1710a38dcbaf56abff&statisticsType=day&selected=0&startDate=2020-02-21&endDate=2020-03-22&orderBy=&orderModel=&pageSize=5&pageNO=1&buyerCookieId="+uids[a].Groups[1].Value+"&statDate="+startdates[a].Groups[1].Value;

                      
                        string strhtml = GetUrl(URL, "utf-8");

                       
                        Match time = Regex.Match(strhtml, @"""logTime"":""([\s\S]*?)""");
                        Match link = Regex.Match(strhtml, @"""realUrl"":""([\s\S]*?)""");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1s[a].Groups[1].Value);
                        lv1.SubItems.Add(a2s[a].Groups[1].Value);
                        lv1.SubItems.Add(a3s[a].Groups[1].Value);

                        lv1.SubItems.Add(time.Groups[1].Value);
                        lv1.SubItems.Add(link.Groups[1].Value);
                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                    }
                    catch
                    {

                        continue;
                    }

                    Thread.Sleep(2000);


                }


            }
        }

        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion
        /// <summary>
        /// 抓数据
        /// </summary>
        public void zhuashuju()
        {

            for (int i = 1; i < 48653; i++)
            {
                string url = "http://www.mitan.com.cn/question/"+i+".html";

                string html = GetUrl(url, "utf-8");

                Match title = Regex.Match(html, @"<title>([\s\S]*?)-");
                Match body= Regex.Match(html, @"<div class=""detail-content"">([\s\S]*?)<div class=""more-operate mb20"">");
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(removeValid(title.Groups[1].Value.Trim()));

                string body1 = Regex.Replace(body.Groups[1].Value, "<[^>]+>", "").Replace("pcqad1()", "").Trim();

                FileStream fs1 = new FileStream(path + removeValid(title.Groups[1].Value.Trim()) + ".docx", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(title.Groups[1].Value.Trim());
                sw.WriteLine(body1);
                sw.Close();
                fs1.Close();


            }
        }

        /// <summary>
        /// 抓数据
        /// </summary>
        public void zhuashuju1()
        {

            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            MatchCollection shis = Regex.Matches(text, @"卷([\s\S]*?)卷");

            for (int i = 0; i < shis.Count; i++)
            {

                MessageBox.Show(shis[i].Groups[1].Value);
            }


        }


        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(new ThreadStart(run));
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;

            Thread thread = new Thread(new ThreadStart(zhuashuju1));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.expotTxt(listView1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1,true);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 抓数据用_Load(object sender, EventArgs e)
        {

        }
    }
}
