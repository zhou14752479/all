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

        string cookie = "HWWAFSESID=83d4d731f07be5b61e; HWWAFSESTIME=1720525848277";
        Dictionary<string, string> areadics = new Dictionary<string, string>();



        //登录地址

        #region 获取token
        public static string gettoken()
        {
            string url = "http://api.yunmai.vip:686/?id=pc_user&action=api_login&username=18627986383&password=18627986383&sn=EE3116635693C0C4&version=4.1.8&shiyong=1 ";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = false;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response.GetResponseHeader("token");
        }
        #endregion

        //http://api.yunmai.vip:686/?id=pc_user&action=api_login&username=18627986383&password=18627986383&sn=EE3116635693C0C4&version=4.1.8&shiyong=1 
        #region GET请求
        public string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                string COOKIE = cookie;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.124 Safari/537.36 Edg/102.0.1245.41";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("token", token);
                //request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/350/page-frame.html";
                request.Referer = "https://map.tianditu.gov.cn/";
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

        public bool panduan(string shouji, string guhua)
        {
            //if (comboBox4.Text == "全部采集")
            //{
            //    return true;
            //}
            //if (comboBox4.Text == "只采集有联系方式")
            //{
            //    if (shouji != "" || guhua != "")
            //    {
            //        return true;
            //    }

            //}
            //if (comboBox4.Text == "只采集有手机号")
            //{
            //    if (shouji != "" && shouji.Substring(0, 1) == "1")
            //    {
            //        return true;
            //    }

            //}
            //return false;

            return true;
        }








        #region  高德地图
        public void gaode()
        {
            ArrayList addrList = new ArrayList();

            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {

                


                foreach (string keyword in keywords)
                {


                    for (int page = 1; page < 100; page++)
                    {

                        string pro = "";
                        string city = "";

                        if(comboBox1.Text=="所有地区")
                        {
                            pro = "";
                        }
                        else
                        {
                            pro = Regex.Replace(comboBox1.Text, @"\(.*\)", "");
                        }
                        if (comboBox2.Text == "所有地区")
                        {
                            city= "";
                        }
                        else
                        {
                            city = comboBox2.Text;
                        }
                        string url = "http://47.104.240.46:6806/?action=api_gd&keyword=" + keyword + "&province=" + pro + "&city=" +city + "&page=" + page;
                        string html = GetUrl(url);

                        MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                        MatchCollection phones = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                        MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                        MatchCollection photo = Regex.Matches(html, @"""photo"":""([\s\S]*?)""");
                        MatchCollection socpes = Regex.Matches(html, @"""socpe"":""([\s\S]*?)""");
                        MatchCollection source = Regex.Matches(html, @"""source"":""([\s\S]*?)""");
                        if (names.Count == 0)
                            break;

                        for (int i = 0; i < names.Count; i++)
                        {




                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(names[i].Groups[1].Value);
                            lv1.SubItems.Add(photo[i].Groups[1].Value);
                            lv1.SubItems.Add(phones[i].Groups[1].Value);
                            lv1.SubItems.Add(address[i].Groups[1].Value);
                            lv1.SubItems.Add(socpes[i].Groups[1].Value);
                            lv1.SubItems.Add(comboBox1.Text + comboBox2.Text);
                            lv1.SubItems.Add(source[i].Groups[1].Value);
                            lv1.SubItems.Add(keyword);
                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            Thread.Sleep(100);
                            count = count + 1;
                            label4.Text = count.ToString();

                        }

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception e)
            {

               MessageBox.Show(e.ToString());
            }

        }

        #endregion

        #region  腾讯地图
        public void tengxun()
        {
            ArrayList addrList = new ArrayList();

            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {




                foreach (string keyword in keywords)
                {


                    for (int page = 1; page < 100; page++)
                    {

                        string pro = "";
                        string city = "";

                        if (comboBox1.Text == "所有地区")
                        {
                            pro = "";
                        }
                        else
                        {
                            pro = Regex.Replace(comboBox1.Text, @"\(.*\)", ""); 
                        }
                        if (comboBox2.Text == "所有地区")
                        {
                            city = "";
                        }
                        else
                        {
                            city = comboBox2.Text;
                        }
                        string url = "http://47.104.240.46:6806/?action=api_tengxun&keyword=" + keyword + "&province=" + pro + "&city=" + city + "&page=" + page;
                        string html = GetUrl(url);

                        MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                        MatchCollection phones = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                        MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                        MatchCollection photo = Regex.Matches(html, @"""photo"":""([\s\S]*?)""");
                        MatchCollection socpes = Regex.Matches(html, @"""socpe"":""([\s\S]*?)""");
                      
                        if (names.Count == 0)
                            break;

                        for (int i = 0; i < names.Count; i++)
                        {




                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(names[i].Groups[1].Value);
                            lv1.SubItems.Add(photo[i].Groups[1].Value);
                            lv1.SubItems.Add(phones[i].Groups[1].Value);
                            lv1.SubItems.Add(address[i].Groups[1].Value);
                            lv1.SubItems.Add(socpes[i].Groups[1].Value);
                            lv1.SubItems.Add(comboBox1.Text + comboBox2.Text);
                            lv1.SubItems.Add("腾讯地图");
                            lv1.SubItems.Add(keyword);
                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            Thread.Sleep(100);
                            count = count + 1;
                            label4.Text = count.ToString();

                        }

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion

        #region 百度地图
        public void baidu()
        {
            ArrayList addrList = new ArrayList();

            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {




                foreach (string keyword in keywords)
                {


                    for (int page = 0; page < 100; page++)
                    {

                        string pro = "";
                        string city = "";

                        if (comboBox1.Text == "所有地区")
                        {
                            pro = "";
                        }
                        else
                        {
                            pro = comboBox1.Text;
                        }
                        if (comboBox2.Text == "所有地区")
                        {
                            city = "";
                        }
                        else
                        {
                            city = comboBox2.Text;
                        }

                        string cityid = "";
                        string url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=spot&from=webmap&c="+cityid+"&wd="+keyword+"&wd2=&pn="+page+"&nn=0&db=0&sug=0&addr=0&&da_src=pcmappg.poi.page&on_gel=1&rn=50&tn=B_NORMAL_MAP";
                        string html = GetUrl(url);

                        MatchCollection htmls = Regex.Matches(html, @"""acc_flag""([\s\S]*?)status");
                       

                        if (htmls.Count == 0)
                            break;

                        for (int i = 0; i < htmls.Count; i++)
                        {
                            string tel= Regex.Match(htmls[i].Groups[1].Value, @"""tel"":""([\s\S]*?)""").Groups[1].Value;
                            string tel1 = tel;
                            string tel2 = tel;
                            if (tel.Contains(")"))
                            {
                                tel1 = "";
                            }
                            else
                            {
                                tel2 = "";
                            }

                            string addr=  Regex.Match(htmls[i].Groups[1].Value, @"""addr"":""([\s\S]*?)""").Groups[1].Value;
                            string name= Regex.Match(htmls[i].Groups[1].Value, @"primary_uid([\s\S]*?)""name"":""([\s\S]*?)""").Groups[2].Value;
                          string scope=  Regex.Match(htmls[i].Groups[1].Value, @"""std_tag"":""([\s\S]*?)""").Groups[1].Value;
                           
                           
                           


                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(method.Unicode2String(name));
                            lv1.SubItems.Add(tel1);
                            lv1.SubItems.Add(tel2);
                            lv1.SubItems.Add(method.Unicode2String(addr));
                            lv1.SubItems.Add(method.Unicode2String(scope));
                            lv1.SubItems.Add(comboBox1.Text + comboBox2.Text);
                            lv1.SubItems.Add("百度地图");
                            lv1.SubItems.Add(keyword);
                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            Thread.Sleep(100);
                            count = count + 1;
                            label4.Text = count.ToString();

                        }

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion


        #region 360地图
        public void so360()
        {
            ArrayList addrList = new ArrayList();

            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {




                foreach (string keyword in keywords)
                {


                    for (int page = 1; page < 100; page++)
                    {

                        string pro = "";
                        string city = "";

                        if (comboBox1.Text == "所有地区")
                        {
                            pro = "";
                        }
                        else
                        {
                            pro = Regex.Replace(comboBox1.Text, @"\(.*\)", "");
                        }
                        if (comboBox2.Text == "所有地区")
                        {
                            city = "";
                        }
                        else
                        {
                            city = comboBox2.Text;
                        }

                      
                        string url = "http://restapi.map.so.com/newapi?jsoncallback=jQuery18307533632584381849_1465956518437&keyword="+keyword+"&cityname="+city+"&city=&batch="+page+"&number=100&citysuggestion=true&sid=1000&qii=true&city_id=440100&region_id=&_=1719834957568";
                        string html = GetUrl(url);

                      
                        MatchCollection names = Regex.Matches(html, @"""poi_name"":""([\s\S]*?)""");
                        MatchCollection contact_address = Regex.Matches(html, @"""contact_address"":""([\s\S]*?)""");
                        MatchCollection business_scopes = Regex.Matches(html, @"""business_scope"":""([\s\S]*?)""");
                        MatchCollection tels = Regex.Matches(html, @"""tel"":""([\s\S]*?)""");

                        if (names.Count == 0)
                            break;

                        for (int i = 0; i < names.Count; i++)
                        {
                            try
                            {
                                string tel = tels[i].Groups[1].Value;
                                string tel1 = tel;
                                string tel2 = tel;
                                if (tel.Contains(")"))
                                {
                                    tel1 = "";
                                }
                                else
                                {
                                    tel2 = "";
                                }






                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(names[i].Groups[1].Value);
                                lv1.SubItems.Add(tel1);
                                lv1.SubItems.Add(tel2);
                                lv1.SubItems.Add(contact_address[i].Groups[1].Value);
                                lv1.SubItems.Add(business_scopes[i].Groups[1].Value);
                                lv1.SubItems.Add(comboBox1.Text + comboBox2.Text);
                                lv1.SubItems.Add("360地图");
                                lv1.SubItems.Add(keyword);
                                if (status == false)
                                    return;
                                if (listView1.Items.Count > 2)
                                {
                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                }
                                Thread.Sleep(100);
                                count = count + 1;
                                label4.Text = count.ToString();
                            }
                            catch (Exception)
                            {

                                continue;
                            }

                        }

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception e)
            {

               MessageBox.Show(e.ToString());
            }

        }

        #endregion

        #region 天地图
        public void tianditu()
        {
            ArrayList addrList = new ArrayList();

            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {




                foreach (string keyword in keywords)
                {


                    for (int page = 0; page <5000; page=page+10)
                    {

                        string pro = "";
                        string city = "";

                        if (comboBox1.Text == "所有地区")
                        {
                            pro = "";
                        }
                        else
                        {
                            pro = comboBox1.Text;
                        }
                        if (comboBox2.Text == "所有地区")
                        {
                            city = "";
                        }
                        else
                        {
                            city = comboBox2.Text;
                        }


                        string url = "https://api.tianditu.gov.cn/v2/search?type=query&postStr=%7B%22specify%22:156320500,%22queryType%22:%221%22,%22start%22:"+page+",%22mapBound%22:%22117.92927823768889,30.70995380630498,124.25999566981824,32.176200184574355%22,%22yingjiType%22:0,%22queryTerminal%22:10000,%22level%22:8,%22keyWord%22:%22"+ System.Web.HttpUtility.UrlEncode(keyword) + "%22,%22count%22:10,%22sourceType%22:0%7D&tk=75f0434f240669f4a2df6359275146d2";
                        string html = GetUrl(url);


                        MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                        MatchCollection contact_address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                     
                        MatchCollection tels = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");

                        if (names.Count == 0)
                            break;

                        for (int i = 0; i < names.Count; i++)
                        {
                            string tel = tels[i].Groups[1].Value;
                            string tel1 = tel;
                            string tel2 = tel;
                            if (tel.Contains("-"))
                            {
                                tel1 = "";
                            }
                            else
                            {
                                tel2 = "";
                            }






                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(names[i].Groups[1].Value);
                            lv1.SubItems.Add(tel1);
                            lv1.SubItems.Add(tel2);
                            lv1.SubItems.Add(contact_address[i].Groups[1].Value);
                            lv1.SubItems.Add(keyword);
                            lv1.SubItems.Add(comboBox1.Text + comboBox2.Text);
                            lv1.SubItems.Add("天地图");
                            lv1.SubItems.Add(keyword);
                            if (status == false)
                                return;
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                            Thread.Sleep(100);
                            count = count + 1;
                            label4.Text = count.ToString();

                        }

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
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
            comboBox1.Items.Add("所有地区");
            ProvinceCity.ProvinceCity.BindCity(comboBox1, comboBox2);
            comboBox2.Items.Add("所有地区");
            if(comboBox1.Text.Contains("北京" ))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("北京市");
                comboBox2.Text ="北京市";
            }
            if (comboBox1.Text.Contains("上海"))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("上海市");
                comboBox2.Text = "上海";
            }
            if (comboBox1.Text.Contains("天津"))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("天津市");
                comboBox2.Text = "天津";
            }
            if (comboBox1.Text.Contains("重庆"))
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("重庆市");
                comboBox2.Text = "重庆市";
            }
        }




        bool status = true;
        Thread thread;
        string token = "";
        private void button1_Click(object sender, EventArgs e)
        {
           
            //if(token=="")
            //{
            //   token= gettoken();
            //}
          
            status = true;
            Control.CheckForIllegalCrossThreadCalls = false;
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(so360);
                    thread.Start();

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
                    thread = new Thread(so360);
                    thread.Start();

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
                    thread = new Thread(so360);
                    thread.Start();
                    
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
                    thread = new Thread(so360);
                    thread.Start();

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
                    thread = new Thread(so360);
                    thread.Start();

                }
                else
                {
                    status = false;
                }
            }
        }



        Dictionary<string, string> dics = new Dictionary<string, string>();






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


    }
}
