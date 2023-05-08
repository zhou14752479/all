using System;
using System.Collections;
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
using CsharpHttpHelper;
using myDLL;

namespace 客户美团
{
    public partial class ditu : Form
    {
        public ditu()
        {
            InitializeComponent();
        }

        string cookie = "";
        Dictionary<string, string> areadics = new Dictionary<string, string>();

        #region GET请求
        public string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                string COOKIE = cookie;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.11(0x17000b21) NetType/4G Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("openIdCipher", "AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADhS4d+zREPZw1PQNF/0Zp8SLSbtYsmCKZFYbIjL5Ty7FJZwQ/bkMIGOGHHGqk1Nld5+rcxtuifNmA==");
                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/350/page-frame.html";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        public bool panduan(string shouji,string guhua)
        {
            if (comboBox4.Text == "全部采集")
            {
                return true;
            }
            if (comboBox4.Text == "只采集有联系方式")
            {
                if (shouji != "" || guhua != "")
                {
                    return true;
                }
                
            }
            if (comboBox4.Text == "只采集有手机号")
            {
                if (shouji !="" && shouji.Substring(0, 1) == "1")
                {
                    return true;
                }
                
            }
            return false;

        }



        #region 百度地图
        public void baidu()
        {
            ArrayList addrList = new ArrayList();
            string[] areas = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {


                foreach (string area in areas)
                {


                    foreach (string keyword in keywords)
                    {


                        for (int page = 1; page < 100; page++)
                        {

                            string url = "https://api.map.baidu.com/?qt=s&c=" + System.Web.HttpUtility.UrlEncode(area) + "&wd=" + System.Web.HttpUtility.UrlEncode(keyword) + "&rn=120&pn=" + page + "&ie=utf-8&oue=1&fromproduct=jsapi&res=api&ak=bMrhZP1PVeuIeaxeLybCzSvlg0DxsV12";
                            string html = method.GetUrl(url, "utf-8");

                            MatchCollection ahtmls = Regex.Matches(html, @"acc_flag([\s\S]*?)view_type");

                            if (ahtmls.Count == 0)
                                break;

                            for (int i = 0; i < ahtmls.Count; i++)
                            {
                                string name = Regex.Match(ahtmls[i].Groups[1].Value, @"geo_type([\s\S]*?)name"":""([\s\S]*?)""").Groups[2].Value;
                                string phone = Regex.Match(ahtmls[i].Groups[1].Value, @"""tel"":""([\s\S]*?)""").Groups[1].Value;
                                string address = Regex.Match(ahtmls[i].Groups[1].Value, @"""addr"":""([\s\S]*?)""").Groups[1].Value;
                                string city = Regex.Match(ahtmls[i].Groups[1].Value, @"""city_name"":""([\s\S]*?)""").Groups[1].Value;
                                string location = Regex.Match(ahtmls[i].Groups[1].Value, @"diPointX"":([\s\S]*?),").Groups[1].Value;

                                string shouji = "";
                                string guhua = "";
                                string[] tels = phone.Split(new string[] { "," }, StringSplitOptions.None);
                                if (tels.Length == 1)
                                {
                                    if (phone.Contains("("))
                                    {
                                        guhua = phone;
                                    }
                                    else
                                    {
                                        shouji = phone;
                                    }
                                }
                                if (tels.Length == 2)
                                {
                                    if (phone.Contains("("))
                                    {
                                        if (tels[0].Contains("-"))
                                        {
                                            guhua = tels[0];
                                            shouji = tels[1];
                                        }
                                        else
                                        {
                                            guhua = tels[1];
                                            shouji = tels[0];
                                        }
                                    }
                                    else
                                    {
                                        guhua = "";
                                        shouji = tels[0];
                                    }
                                }
                                if (panduan(shouji, guhua)==false)
                                {
                                    continue;
                                }


                                if (!addrList.Contains(address))
                                {
                                    addrList.Add(address);
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                    lv1.SubItems.Add(Unicode2String(name));
                                    lv1.SubItems.Add(shouji);
                                    lv1.SubItems.Add(guhua);
                                    lv1.SubItems.Add(Unicode2String(address));
                                    lv1.SubItems.Add(Unicode2String(city));
                                    lv1.SubItems.Add(keyword);
                                    lv1.SubItems.Add(location);
                                    if (status == false)
                                        return;
                                    Thread.Sleep(300);
                                    count = count + 1;
                                    label4.Text = count.ToString();
                                }
                            }

                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception e)
            {

                e.ToString();
            }

        }

        #endregion

        #region 腾讯地图
        public void tengxun()
        {
            ArrayList addrList = new ArrayList();
            string[] areas = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {


                foreach (string area in areas)
                {


                    foreach (string keyword in keywords)
                    {


                        for (int page = 1; page < 100; page++)
                        {

                            string url = "https://apis.map.qq.com/ws/place/v1/search?keyword=" + System.Web.HttpUtility.UrlEncode(keyword) + "&boundary=region(" + System.Web.HttpUtility.UrlEncode(area) + ",0)&key=7RWBZ-TKSK4-Z7IUA-DVYFV-K4EIF-7DFBY&page_size=20&page_index=" + page + "&orderby=_distance%20HTTP/1.1";
                            string html = method.GetUrl(url, "utf-8");

                            MatchCollection names = Regex.Matches(html, @"""title"": ""([\s\S]*?)""");
                            MatchCollection phones = Regex.Matches(html, @"""tel"": ""([\s\S]*?)""");
                            MatchCollection address = Regex.Matches(html, @"""address"": ""([\s\S]*?)""");
                            MatchCollection citys = Regex.Matches(html, @"""city"": ""([\s\S]*?)""");
                            MatchCollection locations = Regex.Matches(html, @"""lat"": ([\s\S]*?),");
                            if (names.Count == 0)
                                break;

                            for (int i = 0; i < address.Count; i++)
                            {

                                string shouji = "";
                                string guhua = "";
                                string[] tels = phones[i].Groups[1].Value.Split(new string[] { ";" }, StringSplitOptions.None);
                                if (tels.Length == 1)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        guhua = phones[i].Groups[1].Value;
                                    }
                                    else
                                    {
                                        shouji = phones[i].Groups[1].Value;
                                    }
                                }
                                if (tels.Length == 2)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        if (tels[0].Contains("-"))
                                        {
                                            guhua = tels[0];
                                            shouji = tels[1];
                                        }
                                        else
                                        {
                                            guhua = tels[1];
                                            shouji = tels[0];
                                        }
                                    }
                                    else
                                    {
                                        guhua = "";
                                        shouji = tels[0];
                                    }
                                }

                                if (panduan(shouji, guhua) == false)
                                {
                                    continue;
                                }

                                if (!addrList.Contains(address[i].Groups[1].Value))
                                {
                                    addrList.Add(address[i].Groups[1].Value);
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                    lv1.SubItems.Add(Unicode2String(names[i].Groups[1].Value));
                                    lv1.SubItems.Add(shouji);
                                    lv1.SubItems.Add(guhua);
                                    lv1.SubItems.Add(Unicode2String(address[i].Groups[1].Value));
                                    lv1.SubItems.Add(Unicode2String(citys[i].Groups[1].Value));
                                    lv1.SubItems.Add(keyword);
                                    lv1.SubItems.Add(locations[i].Groups[1].Value);
                                    if (status == false)
                                        return;
                                    Thread.Sleep(300);
                                    count = count + 1;
                                    label4.Text = count.ToString();
                                }
                            }

                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception e)
            {

               e.ToString();
            }

        }

        #endregion


        #region 搜狗地图
        private string GetHttp(string citykeyword, int page)
        {
            
            //北京市 东城区 餐饮
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://map.sogou.com/EngineV6/search/json",
                Method = "POST",
                ContentType = "application/x-www-form-urlencoded",
                Host = "map.sogou.com",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36",
                PostEncoding = Encoding.UTF8,
                Postdata = "what=keyword:" + citykeyword + "&range=bound:12633753.90625,2521527.34375,12646777.34375,2523988.28125:0&othercityflag=1&appid=1361&userdata=3&encrypt=1&pageinfo=" + page + ",50&locationsort=0&version=7.0&ad=0&level=14&exact=0&type=morebtn&attr=&order=&submittime=0&resultTypes=poi&sort=0"
            };
            item.Header.Add("Accept-Encoding", "identity");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;


        }

        public void sougou()
        {
            ArrayList addrList = new ArrayList();
            string[] cityss = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {


                foreach (string city in cityss)
                {
                    foreach (string keyword in keywords)
                    {
                     

                        for (int page = 1; page < 100; page++)
                        {


                            string html = GetHttp(city + " " + keyword, page);

                            MatchCollection names = Regex.Matches(html, @"""caption"":""([\s\S]*?)""");
                            MatchCollection phones = Regex.Matches(html, @"""phone"":([\s\S]*?),");
                            MatchCollection address = Regex.Matches(html, @"""address"":([\s\S]*?),");
                            MatchCollection citys = Regex.Matches(html, @"""city"":([\s\S]*?),");
                            MatchCollection locations = Regex.Matches(html, @"""minx"":([\s\S]*?),");
                            if (names.Count == 0)
                                break;

                            for (int i = 0; i < names.Count; i++)
                            {


                                string shouji = "";
                                string guhua = "";
                                string[] tels = phones[i].Groups[1].Value.Split(new string[] { ";" }, StringSplitOptions.None);
                                if (tels.Length == 1)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        guhua = phones[i].Groups[1].Value;
                                    }
                                    else
                                    {
                                        shouji = phones[i].Groups[1].Value;
                                    }
                                }
                                if (tels.Length == 2)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        if (tels[0].Contains("-"))
                                        {
                                            guhua = tels[0];
                                            shouji = tels[1];
                                        }
                                        else
                                        {
                                            guhua = tels[1];
                                            shouji = tels[0];
                                        }
                                    }
                                    else
                                    {
                                        guhua = "";
                                        shouji = tels[0];
                                    }
                                }
                                if (phones[i].Groups[1].Value == "[]")
                                {
                                    shouji = "";
                                }

                                if (panduan(shouji, guhua) == false)
                                {
                                    continue;
                                }
                                if (!addrList.Contains(address[i].Groups[1].Value))
                                {
                                    addrList.Add(address[i].Groups[1].Value);
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                    lv1.SubItems.Add(names[i].Groups[1].Value);
                                    lv1.SubItems.Add(shouji.Replace("\"", ""));
                                    lv1.SubItems.Add(guhua.Replace("\"", ""));
                                    lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                    lv1.SubItems.Add(citys[i].Groups[1].Value.Replace("\"", ""));
                                    lv1.SubItems.Add(keyword);
                                    lv1.SubItems.Add(locations[i].Groups[1].Value.Replace("\"", ""));
                                    if (status == false)
                                        return;

                                    Thread.Sleep(300);
                                    count = count + 1;
                                    label4.Text = count.ToString();
                                }
                            }

                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception e)
            {

               e.ToString();
            }

        }


        #endregion

        #region  高德地图
        public void gaode()
        {
            ArrayList addrList = new ArrayList();
            string[] areas = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            string[] keywords= textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {


                for (int a = 0; a < areas.Length; a++)
                {

                    foreach (string keyword in keywords)
                {
                        string lat = dics[areas[a].Trim()];

                        for (int page = 1; page < 100; page++)
                        {

                            string url = "https://restapi.amap.com/v3/place/around?appname=1e3bb24ab8f75ba78a7cf8a9cc4734c6&key=1e3bb24ab8f75ba78a7cf8a9cc4734c6&keywords=" + System.Web.HttpUtility.UrlEncode(keyword) + "&location=" + lat + "&logversion=2.0&page=" + page + "&platform=WXJS&s=rsx&sdkversion=1.2.0";
                            string html = GetUrl(url);

                            MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                            MatchCollection phones = Regex.Matches(html, @"""tel"":([\s\S]*?),");
                            MatchCollection address = Regex.Matches(html, @"""address"":([\s\S]*?),");       
                            MatchCollection citys = Regex.Matches(html, @"""cityname"":([\s\S]*?),");   
                             MatchCollection locations = Regex.Matches(html, @"""location"":([\s\S]*?),");   
                            if (names.Count == 0)
                                break;

                            for (int i = 0; i < names.Count; i++)
                            {


                                string shouji = "";
                                string guhua = "";
                                string[] tels = phones[i].Groups[1].Value.Split(new string[] { ";" }, StringSplitOptions.None);
                                if (tels.Length == 1)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        guhua = phones[i].Groups[1].Value;
                                    }
                                    else
                                    {
                                        shouji = phones[i].Groups[1].Value;
                                    }
                                }
                                if (tels.Length == 2)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        if (tels[0].Contains("-"))
                                        {
                                            guhua = tels[0];
                                            shouji = tels[1];
                                        }
                                        else
                                        {
                                            guhua = tels[1];
                                            shouji = tels[0];
                                        }
                                    }
                                    else
                                    {
                                        guhua = "";
                                        shouji = tels[0];
                                    }
                                }
                                if (phones[i].Groups[1].Value == "[]")
                                {
                                    shouji = "";
                                }
                                if (panduan(shouji, guhua) == false)
                                {
                                    continue;
                                }

                                if (!addrList.Contains(address[i].Groups[1].Value))
                                {
                                    addrList.Add(address[i].Groups[1].Value);
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                    lv1.SubItems.Add(names[i].Groups[1].Value);
                                    lv1.SubItems.Add(shouji.Replace("\"", ""));
                                    lv1.SubItems.Add(guhua.Replace("\"", ""));
                                    lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                    lv1.SubItems.Add(citys[i].Groups[1].Value.Replace("\"", ""));
                                    lv1.SubItems.Add(keyword);
                                    lv1.SubItems.Add(locations[i].Groups[1].Value.Replace("\"", ""));
                                    if (status == false)
                                        return;
                                    Thread.Sleep(300);
                                    count = count + 1;
                                    label4.Text = count.ToString();
                                }
                            }

                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception e)
            {

                e.ToString();
            }

        }

        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox1);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox1, comboBox2);
        }




        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"YVoWQ"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

            #region 通用检测

            string ahtml = GetUrl("http://139.129.92.113/");

            if (!ahtml.Contains(@"siyisoft"))
            {

                return;
            }



            #endregion

            status = true;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("区域或行业为空");
                return;
            }

            status = true;
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(gaode);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                else
                {
                    status = false;
                }
            }

            if (radioButton2.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(tengxun);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                else
                {
                    status = false;
                }
            }
            if (radioButton3.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(baidu);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                else
                {
                    status = false;
                }
            }
            if (radioButton4.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(sougou);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                else
                {
                    status = false;
                }
            }
            if (radioButton5.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(tengxun);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                else
                {
                    status = false;
                }
            }
            


        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {



        }


        Dictionary<string, string> dics = new Dictionary<string, string>();
        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getarea_old(string city)
        {
            dics.Clear();
            comboBox3.Items.Clear();
               ArrayList areas = new ArrayList();
            string url = "http://www.jsons.cn/lngcode/?keyword=" + System.Web.HttpUtility.UrlEncode(city.Replace("市", "")) + "&txtflag=0";
            string html = GetUrl(url);
            
            Match ahtml = Regex.Match(html, @"<table class=""table table-bordered table-hover"">([\s\S]*?)</table>");
         
            MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");

            for (int i = 0; i < values.Count; i++)
            {
                //MessageBox.Show(values[i].Groups[1].Value);
                if (values[i].Groups[1].Value.Contains("区") || values[i].Groups[1].Value.Contains("县") || values[i].Groups[1].Value.Contains("市"))
                {
                    if (!comboBox3.Items.Contains(values[i].Groups[1].Value.Trim()))
                    {
                        if (!values[i].Groups[1].Value.Trim().Contains(city))
                        {
                            comboBox3.Items.Add(values[i].Groups[1].Value.Trim());
                            dics.Add(values[i].Groups[1].Value.Trim(), values[i + 1].Groups[1].Value.Replace("，", "%2C").Trim());
                        }
                    }
                }

            }


            dics.Add(city, values[2].Groups[1].Value.Replace("，", "%2C").Trim());

            return areas;
        }



        /// <summary>
        /// 获取地区新新新新新
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getarea(string city)
        {
            dics.Clear();
            comboBox3.Items.Clear();
            ArrayList areas = new ArrayList();
            string url = "https://jingwei.supfree.net/";
            string html = method.GetUrl(url, "gb2312");

            MatchCollection values = Regex.Matches(html, @"<a href=""kongzi.asp\?id=([\s\S]*?)"">([\s\S]*?)</a>");

         

            for (int i = 0; i < values.Count; i++)
            {
               
                if (values[i].Groups[2].Value.Trim()==city)
                {
                   
                    string aurl = "https://jingwei.supfree.net/kongzi.asp?id=" + values[i].Groups[1].Value.Trim();
                    string ahtml = method.GetUrl(aurl,"gb2312");
                   
                    MatchCollection avalues = Regex.Matches(ahtml, @"<li><a href=""mengzi.asp\?id=([\s\S]*?)"" title=""([\s\S]*?)经纬度"">");
                    //MessageBox.Show(avalues.Count.ToString());
                    for (int j= 0; j <avalues.Count; j++)
                    {
                       
                        if (!comboBox3.Items.Contains(avalues[j].Groups[2].Value.Trim()))
                        {
                            string burl = "https://jingwei.supfree.net/mengzi.asp?id=" + avalues[j].Groups[1].Value.Trim();
                            string bhtml = method.GetUrl(burl, "gb2312");
                            string lng= Regex.Match(bhtml, @"经度：<span class=""bred botitle18"">([\s\S]*?)</span>").Groups[1].Value.Trim();
                            string lat = Regex.Match(bhtml, @"纬度：<span class=""bred botitle18"">([\s\S]*?)</span>").Groups[1].Value.Trim();

                            comboBox3.Items.Add(avalues[j].Groups[2].Value.Trim());
                            dics.Add(avalues[j].Groups[2].Value.Trim(), lng+ "%2C"+lat);

                        }
                    }


                    break;
                }

            }




            return areas;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
      
            listView1.Items.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(comboBox2.Text))
            {
                MessageBox.Show(comboBox2.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox1.Text += comboBox2.Text + "\r\n";
            getarea(comboBox2.SelectedItem.ToString());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(comboBox3.Text))
            {
                MessageBox.Show(comboBox3.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox1.Text += comboBox3.Text + "\r\n";
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

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < comboBox2.Items.Count; i++)
            {
                if (!textBox1.Text.Contains(comboBox2.Items[i].ToString()))
                {
                    textBox1.Text += comboBox2.Items[i].ToString() + "\r\n";
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < comboBox3.Items.Count; i++)
            {
                textBox1.Text += comboBox3.Items[i].ToString() + "\r\n";
            }
        }
    }
}
