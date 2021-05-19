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
using myDLL;

namespace 美团
{
    public partial class 美团附近 : Form
    {
        public 美团附近()
        {
            InitializeComponent();
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cityId = GetcityId((comboBox3.Text.Replace("市","")));
            getareas(cityId);

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox2, comboBox3);

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


        #region 获取所有城市ID
        public ArrayList getallcitys()
        {
            string Url = "https://www.meituan.com/changecity/";

            string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection ids = Regex.Matches(html, @"{""id"":([\s\S]*?),");
            ArrayList lists = new ArrayList();

            foreach (Match item in ids)
            {
                lists.Add(item.Groups[1].Value);
            }

            return lists;
        }

        #endregion

        Dictionary<string, string> areadics = new Dictionary<string, string>();

        #region 获取区域
        public void getareas(string cityid)
        {
            areadics.Clear();
            comboBox4.Items.Clear();
            string Url = "https://i.meituan.com/wrapapi/search/filters?riskLevel=71&optimusCode=10&ci="+cityid;

            string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"{""id"":([\s\S]*?),([\s\S]*?)""name"":""([\s\S]*?)""");
          

            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[3].Value.Contains("区") || areas[i].Groups[3].Value.Contains("县"))
                {
                    if (!areas[i].Groups[3].Value.Contains("小区") && !areas[i].Groups[3].Value.Contains("街区") && !areas[i].Groups[3].Value.Contains("商业区") && !areas[i].Groups[3].Value.Contains("城区") && !areas[i].Groups[3].Value.Contains("市区") && !areas[i].Groups[3].Value.Contains("地区") && !areas[i].Groups[3].Value.Contains("社区") && areas[i].Groups[3].Value.Length<5)
                    {
                        if (!areadics.ContainsKey(areas[i].Groups[3].Value))
                        {
                            areadics.Add(areas[i].Groups[3].Value, areas[i].Groups[1].Value);
                            if (!comboBox4.Items.Contains(areas[i].Groups[3].Value))
                            {
                                comboBox4.Items.Add(areas[i].Groups[3].Value);
                            }
                        }
                    }
                }
                   
            }

          
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
        bool status = true;
        ArrayList tels = new ArrayList();
        string cateid = "1";
        string cityId = "1";

        ArrayList finishes = new ArrayList();
        #region  主程序进入详情页
        public void run1()
        {
            string city = "";
           ArrayList keywords = new ArrayList();
           
            //if (textBox2.Text != "")
            //{
            //    keywords.Add(textBox2.Text.Trim());
            //}
            try
            {


                //if (textBox1.Text.Trim() == "")
                //{
                //    MessageBox.Show("请输入城市！");
                //    return;
                //}



                string cityId = GetcityId(city);
               
                ArrayList areas = null;
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

            ArrayList areaidlist = new ArrayList();

            if (comboBox4.Text == "")
            {
                foreach (string item in comboBox4.Items)
                {
                    areaidlist.Add(areadics[item]);
                }
            }

            else
            {
                areaidlist.Add(areadics[comboBox4.Text]);
            }



            try
            {

                foreach (string areaid in areaidlist)
                {
                    string[] catenames = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    foreach (string catename in catenames)
                    {

                        switch (catename)
                        {
                            case "餐饮美食":
                                cateid = "1";
                                break;
                            case "小吃快餐":
                                cateid = "36";
                                break;
                            case "丽人":
                                cateid = "22";
                                break;
                            case "休闲娱乐":
                                cateid = "2";
                                break;
                            case "饮品":
                                cateid = "21329";
                                break;
                            case "蛋糕甜点":
                                cateid = "11";
                                break;
                            case "美发":
                                cateid = "74";
                                break;
                            case "美容美体":
                                cateid = "76";
                                break;
                            case "婚纱摄影":
                                cateid = "20178";
                                break;
                            case "汽车":
                                cateid = "27";
                                break;
                            case "教育":
                                cateid = "20285";
                                break;
                            case "KTV":
                                cateid = "10";
                                break;
                            case "足疗":
                                cateid = "52";
                                break;
                            case "洗浴汗蒸":
                                cateid = "112";
                                break;
                            case "宠物医院":
                                cateid = "20691";
                                break;
                            case "瑜伽舞蹈":
                                cateid = "220";
                                break;

                        }
                        for (int i = 0; i < 1001; i = i + 100)

                        {
                            string Url = "https://m.dianping.com/mtbeauty/index/ajax/shoplist?token=&cityid=" + cityId + "&cateid=22&categoryids=" + cateid + "&lat=33.94114303588867&lng=118.2479019165039&userid=&uuid=&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false&start=" + i + "&limit=100&areaid=" + areaid + "&distance=&subwaylineid=&subwaystationid=&sort=2";
                          
                            string html = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            MatchCollection names = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                            MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                            MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                            MatchCollection cate = Regex.Matches(html, @"mainCategoryName"":""([\s\S]*?)""");
                            MatchCollection shangquan = Regex.Matches(html, @"""areaName"":""([\s\S]*?)""");

                            if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            for (int j = 0; j < names.Count; j++)

                            {
                                if (!finishes.Contains(phone[j].Groups[1].Value))
                                {
                                    finishes.Add(phone[j].Groups[1].Value);
                                    ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                    listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                    listViewItem.SubItems.Add(address[j].Groups[1].Value);
                                    listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                                    listViewItem.SubItems.Add(cate[j].Groups[1].Value);
                                    listViewItem.SubItems.Add(shangquan[j].Groups[1].Value);
                                    listViewItem.SubItems.Add(comboBox2.Text);
                                }
                            }

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(1000);
                        }
                    }

                }
            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
            


        }


      
        #endregion


        #region  全国饮品店
        public void getall()
        {
            ArrayList cityids =getallcitys();

            foreach (string cityid in cityids)
            { 

                try
                {


                    for (int i = 0; i < 100001; i = i + 100)

                    {

                        string Url = "https://api.meituan.com/group/v5/deal/select/city/" + cityid + "/cate/21329?sort=start&mypos=&hasGroup=true&offset=" + i + "&limit=100&poiFields=phone,addr,addr,cates,name,cateId,areaId,districtId,cateName,areaName,mallName,mallId,brandId,iUrl,payInfo,poiid&client=android&utm_source=qqcpd&utm_medium=android&utm_term=254&version_name=5.5.4&utm_content=&utm_campaign=AgroupBgroupC0E0Ghomepage_category1_1__a1&uuid=";

                        string html = meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");

                        MatchCollection address = Regex.Matches(html, @"""addr"":""([\s\S]*?)""");
                        MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                        MatchCollection waimai = Regex.Matches(html, @"""isWaimai"":([\s\S]*?),");

                        MatchCollection cate = Regex.Matches(html, @"cateName"":""([\s\S]*?)""");
                        MatchCollection area = Regex.Matches(html, @"areaName"":""([\s\S]*?)""");
                        MatchCollection shangquan = Regex.Matches(html, @"mallName"":""([\s\S]*?)""");


                        if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        for (int j = 0; j < names.Count; j++)
                        {

                            if (!tels.Contains(phone[j].Groups[1].Value))
                            {
                                tels.Add(phone[j].Groups[1].Value);
                                bool panduan = true;
                        
                                if (panduan)
                                {
                                  
                                        if (!phone[j].Groups[1].Value.Contains("-") && !phone[j].Groups[1].Value.Contains("400"))
                                        {

                                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                            listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                            listViewItem.SubItems.Add(address[j].Groups[1].Value);
                                            listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                                            listViewItem.SubItems.Add(waimai[j].Groups[1].Value);

                                            listViewItem.SubItems.Add(cate[j].Groups[1].Value);
                                            listViewItem.SubItems.Add(area[j].Groups[1].Value);
                                            listViewItem.SubItems.Add(shangquan[j].Groups[1].Value);

                                        }
                          

                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    if (status == false)
                                        return;
                                }

                                else
                                {
                                    toolStripStatusLabel1.Text = names[j].Groups[1].Value+"不符合条件";
                                }
                            }
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
        #endregion

      
    
        Thread thread;

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"MBfRdu"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

            status = true;
           

          
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(getall);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}

        }

        private void Button2_Click(object sender, EventArgs e)
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

        private void Button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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

      
        private void 美团附近_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox2);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com/");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += comboBox1.Text + "\r\n";
        }
    }
}
