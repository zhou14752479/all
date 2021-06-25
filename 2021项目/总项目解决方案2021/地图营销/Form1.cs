using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Threading;
using System.Collections;

namespace 地图营销
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox2);
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getsoftinfo);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        Dictionary<string, string> dics = new Dictionary<string, string>();
        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getarea(string city)
        {
            ArrayList areas = new ArrayList();
            string url = "http://www.jsons.cn/lngcode/?keyword=" + System.Web.HttpUtility.UrlEncode(city) + "&txtflag=0";
            string html = method.GetUrl(url,"utf-8");

            Match ahtml = Regex.Match(html, @"<table class=""table table-bordered table-hover"">([\s\S]*?)</table>");

            MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");

            for (int i = 0; i < values.Count; i++)
            {
                //MessageBox.Show(values[i].Groups[1].Value);
                if (values[i].Groups[1].Value.Contains("区") || values[i].Groups[1].Value.Contains("县"))
                {

                    areas.Add(values[i + 1].Groups[1].Value.Replace("，", "%2C").Trim());
                    
                }

            }
            return areas;
        }

        #region 百度地图
        public void baidu()
        {
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

                            string url = "https://api.map.baidu.com/?qt=s&c="+ System.Web.HttpUtility.UrlEncode(area) + "&wd="  + System.Web.HttpUtility.UrlEncode(keyword) + "&rn=120&pn=" + page + "&ie=utf-8&oue=1&fromproduct=jsapi&res=api&ak=bMrhZP1PVeuIeaxeLybCzSvlg0DxsV12";
                            string html = method.GetUrl(url,"utf-8");

                            MatchCollection ahtmls = Regex.Matches(html, @"acc_flag([\s\S]*?)view_type");

                            if (ahtmls.Count == 0)
                                break;

                            for (int i = 0; i < ahtmls.Count; i++)
                            {
                                string name = Regex.Match(ahtmls[i].Groups[1].Value, @"geo_type([\s\S]*?)name"":""([\s\S]*?)""").Groups[2].Value;
                                string phone = Regex.Match(ahtmls[i].Groups[1].Value, @"""tel"":""([\s\S]*?)""").Groups[1].Value;
                                string addres = Regex.Match(ahtmls[i].Groups[1].Value, @"""addr"":""([\s\S]*?)""").Groups[1].Value;
                                string city = Regex.Match(ahtmls[i].Groups[1].Value, @"""city_name"":""([\s\S]*?)""").Groups[1].Value;
                                string location = Regex.Match(ahtmls[i].Groups[1].Value, @"diPointX"":([\s\S]*?),").Groups[1].Value;
                                string shouji = phone;
                                string guhua = "";




                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(Unicode2String(name));
                                lv1.SubItems.Add(shouji);
                                lv1.SubItems.Add(guhua);
                                lv1.SubItems.Add(Unicode2String(addres));
                                lv1.SubItems.Add(Unicode2String(city));
                                lv1.SubItems.Add(keyword);
                                lv1.SubItems.Add(location);
                                if (status == false)
                                    return;
                                Thread.Sleep(300);
                                count = count + 1;
                                label4.Text = count.ToString();
                            }

                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion


        #region 腾讯地图
        public void tengxun()
        {
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

                            string url = "https://apis.map.qq.com/ws/place/v1/search?keyword="+ System.Web.HttpUtility.UrlEncode(keyword) + "&boundary=region("+ System.Web.HttpUtility.UrlEncode(area) + ",0)&key=7RWBZ-TKSK4-Z7IUA-DVYFV-K4EIF-7DFBY&page_size=20&page_index="+page+"&orderby=_distance%20HTTP/1.1";
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

                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion

        #region 搜狗地图
        private string GetHttp(string citykeyword,int page)
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
                Postdata = "what=keyword:"+citykeyword+"&range=bound:12633753.90625,2521527.34375,12646777.34375,2523988.28125:0&othercityflag=1&appid=1361&userdata=3&encrypt=1&pageinfo="+page+",50&locationsort=0&version=7.0&ad=0&level=14&exact=0&type=morebtn&attr=&order=&submittime=0&resultTypes=poi&sort=0"
            };
            item.Header.Add("Accept-Encoding", "identity");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;


        }

        public void sougou()
        {
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
                        ArrayList arealists = getarea(city);


                            for (int page = 1; page < 100; page++)
                            {

                              
                                string html = GetHttp(city+" "+ keyword, page);
                        
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
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    Thread.Sleep(300);
                                    count = count + 1;
                                    label4.Text = count.ToString();
                                }

                                Thread.Sleep(1000);
                            }
                        }
                    }
                
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }


        #endregion
        bool status = true;
        #region 高德地图
        public void gaode()
        {
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
                        ArrayList arealists = getarea(city);

                        foreach (string area in arealists)
                        {
                            for (int page = 1; page < 100; page++)
                            {

                                string url = "https://restapi.amap.com/v3/place/around?appname=1e3bb24ab8f75ba78a7cf8a9cc4734c6&key=1e3bb24ab8f75ba78a7cf8a9cc4734c6&keywords=" + System.Web.HttpUtility.UrlEncode(keyword) + "&location=" + area + "&logversion=2.0&page=" + page + "&platform=WXJS&s=rsx&sdkversion=1.2.0";
                                string html = method.GetUrl(url, "utf-8");

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
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    Thread.Sleep(300);
                                    count = count + 1;
                                    label4.Text = count.ToString();
                                }

                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
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

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {

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

        Thread thread;
        bool zanting = true;
        private void button7_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"WQXTAei"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

            if (logined == false)
            {
                MessageBox.Show("请登录账号");
                tabControl1.SelectedIndex = 2;
                return;
            }

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请选择城市和关键词");
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
            }

            if (radioButton2.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(tengxun);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
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
            }
            if (radioButton4.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(sougou);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
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
            }

            button6.Enabled = false;
            button8.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

      

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
            button6.Enabled = true;
            button8.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox2, comboBox3);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += comboBox3.Text + "\r\n";
        }

        bool logined = false;
        public void login()
        {
            if (textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("请输入账号和密码");
                return;
            }
            string html = method.GetUrl("http://116.62.170.108:8080/api/mt/login.html?username=" + textBox5.Text.Trim() + "&password=" + textBox6.Text.Trim() + "&code=1&", "utf-8");
            if (html.Contains("成功"))
            {
                logined = true;
                MessageBox.Show("登陆成功");
                tabControl1.SelectedIndex = 0;
            }
            else
            {
                logined = false;
                MessageBox.Show("账户不存在或者密码错误");
            };

           
            

        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(login);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public void getsoftinfo()
        {
            string html = method.GetUrl("http://116.62.170.108:8080/api/mt/getsoftinfo.html", "utf-8");
           string softname= Regex.Match(html, @"""softname"": ""([\s\S]*?)""").Groups[1].Value;
            string contacts = Regex.Match(html, @"""contacts"": ""([\s\S]*?)""").Groups[1].Value;

            if (softname != "")
            {
                label1.Text = softname;
            }

            if (contacts != "")
            {
                label17.Text = contacts;
            }


        }
       
    }
}
