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

namespace 美团
{
    public partial class 美团附近 : Form
    {
        public 美团附近()
        {
            InitializeComponent();
        }
        #region GET请求
        public static string meituan_GetUrl(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.13(0x17000d2a) NetType/4G Language/zh_CN";
                WebHeaderCollection headers = request.Headers;
                headers.Add("uuid: E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576");
                headers.Add("open_id: oJVP50IRqKIIshugSqrvYE3OHJKQ");
                headers.Add("token: Vteo9CkJqIGMe30FC3iuvnvTr2YAAAAAygoAAMPHPyLNO16W1eYLn1hWsLhD40r-KnDdB70rrl9LN9OHUfVBGbTDt4PCDHH72xKkDA");

                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/328/page-frame.html";
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

        #region GET请求
        public static string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";

                request.Headers.Add("Cookie", "");

                request.Referer = "https://i.meituan.com/wrapapi/poiinfo?poiId=150177929";
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

        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city);
                string html = GetUrl(url);
                Match cityId = Regex.Match(html, @"""cityId"":([\s\S]*?),");

                return cityId.Groups[1].Value;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }



        #endregion

        #region 获取区域
        public ArrayList getareas(string city)
        {
            string Url = "https://" + city + ".meituan.com/meishi/";

            string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"""subAreas"":\[\{""id"":([\s\S]*?),");
            ArrayList lists = new ArrayList();

            foreach (Match item in areas)
            {
                lists.Add(item.Groups[1].Value);
            }

            return lists;
        }

        #endregion

        #region  获取城市拼音缩写

        public string Getsuoxie(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city);
                string html = GetUrl(url);
                Match suoxie = Regex.Match(html, @"""cityAcronym"":""([\s\S]*?)""");

                return suoxie.Groups[1].Value;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }


        }

        #endregion
        bool zanting = true;
        ArrayList tels = new ArrayList();
        #region  主程序
        public void run1()
        {
            string city = textBox1.Text.Trim();
           ArrayList keywords = new ArrayList();
            keywords.Add("摄影");
            if (textBox2.Text != "")
            {
                keywords.Add(textBox2.Text.Trim());
            }
            try
            {


                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("请输入城市！");
                    return;
                }



                string cityId = GetcityId(city);
                ArrayList areas = getareas(Getsuoxie(city));
                foreach (string areaId in areas)
                {
                    foreach (string keyword in keywords)

                    {
                        for (int i = 0; i < 1000; i = i + 15)

                        {




                            string Url = "https://apimobile.meituan.com/group/v4/poi/search/" + cityId + "?riskLevel=71&optimusCode=10&cateId=-1&sort=defaults&userid=-1&offset=" + i + "&limit=15&mypos=33.940975189208984%2C118.24801635742188&uuid=E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576&version_name=10.4.200&supportDisplayTemplates=itemA%2CitemB%2CitemJ%2CitemP%2CitemS%2CitemM%2CitemY%2CitemL&supportTemplates=default%2Chotel%2Cblock%2Cnofilter%2Ccinema&searchSource=miniprogram&ste=_b100000&q=" + keyword.Trim() + "&cityId=" + cityId + "&areaId=" + areaId;

                            string html = meituan_GetUrl(Url); ;  //定义的GetRul方法 返回 reader.ReadToEnd()
                          
                            MatchCollection all = Regex.Matches(html, @"\{""poiid"":([\s\S]*?),");

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {


                                lists.Add("https://i.meituan.com/wrapapi/allpoiinfo?riskLevel=71&optimusCode=10&poiId=" + NextMatch.Groups[1].Value + "&isDaoZong=true");
                            }

                            

                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                            {
                                Thread.Sleep(1000);
                                break;
                            }

                                



                            foreach (string list in lists)

                            {

                                string strhtml1 = meituan_GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                Match name = Regex.Match(strhtml1, @"name"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml1, @"phone"":""([\s\S]*?)""");
                                Match addr = Regex.Match(strhtml1, @"address"":""([\s\S]*?)""");
                             
                                if (!tels.Contains(tel.Groups[1].Value))
                                {
                                    tels.Add(tel.Groups[1].Value);

                                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                    listViewItem.SubItems.Add(name.Groups[1].Value);
                                    listViewItem.SubItems.Add(addr.Groups[1].Value);
                                    listViewItem.SubItems.Add(tel.Groups[1].Value);
                                   
                            
                                    listViewItem.SubItems.Add("1");


                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    Thread.Sleep(1000);


                                }

                                


                            }

                            Thread.Sleep(2000);
                        }

                    }


                }
            }




            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        #endregion

        #region  主程序
        public void run()
        {
            string[] citys = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string city in citys)
            {

                string cityId = GetcityId((city));

                try
                {


                    for (int i = 0; i < 10001; i = i + 100)

                    {

                        string Url = "https://api.meituan.com/group/v5/deal/select/city/" + cityId + "/cate/"+cateid+"?sort=start&mypos=&hasGroup=true&offset=" + i + "&limit=100&poiFields=phone,addr,addr,cates,name,cateId,areaId,districtId,cateName,areaName,mallName,mallId,brandId,iUrl,payInfo,poiid&client=android&utm_source=qqcpd&utm_medium=android&utm_term=254&version_name=5.5.4&utm_content=&utm_campaign=AgroupBgroupC0E0Ghomepage_category1_1__a1&uuid=";

                        string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");

                        MatchCollection address = Regex.Matches(html, @"""addr"":""([\s\S]*?)""");
                        MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                        MatchCollection waimai = Regex.Matches(html, @"""isWaimai"":([\s\S]*?),");



                        if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        for (int j = 0; j < names.Count; j++)


                        {

                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(names[j].Groups[1].Value);

                            listViewItem.SubItems.Add(address[j].Groups[1].Value);
                            listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                            listViewItem.SubItems.Add(waimai[j].Groups[1].Value);



                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            Thread.Sleep(200);
                        }
                        Application.DoEvents();
                        Thread.Sleep(1000);
                    }


                }
                catch (System.Exception ex)
                {
                    ex.ToString();
                }
            }

        }


        string cateid = "1";

        #endregion
        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"meituanfuzhou"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            switch (comboBox1.Text)
            {
                case "餐饮美食":
                    cateid = "1";
                    break;
                case "丽人":
                    cateid = "22";
                    break;
                case "休闲娱乐":
                    cateid = "2";
                    break;

            }

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

            //Thread thread1 = new Thread(new ThreadStart(run1));
            //thread1.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }
        private Point mPoint = new Point();
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
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                
            }
        }
    }
}
