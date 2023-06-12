using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 客户美团
{
    public partial class dianping : Form
    {
        public dianping()
        {
            InitializeComponent();
        }

        string cookie = "";
        string cateid = "1";
        #region GET请求2
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


        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city.Replace("市", ""));
                string html = GetUrl(url);
                string cityId = Regex.Match(html, @"""cityId"":([\s\S]*?),").Groups[1].Value;

                return cityId;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }

        #endregion

        string path = AppDomain.CurrentDomain.BaseDirectory;

        Dictionary<string, string> areadic = new Dictionary<string, string>();

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #region  主程序
        public void run1()
        {
            int count = 0;

            try
            {
                string[] citys = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string city in citys)
                {
                    if (city.Trim() == "")
                    {
                        continue;
                    }

                    string cityId = GetcityId(city);
                    string areaid = "-3";
                    string[] catenames = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                        foreach (string catename in catenames)
                        {
                            if (catename.Trim() == "")
                            {
                                continue;
                            }

                            switch (catename)
                            {
                                case "美食":
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
                                case "面包甜点":
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
                                case "爱车":
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
                                MatchCollection ranks = Regex.Matches(html, @"""shopPower"":([\s\S]*?),");

                                if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                    break;

                                for (int j = 0; j < names.Count; j++)

                                {

                                    string shouji = "";
                                    string guhua = "";
                                    string[] tels = phone[j].Groups[1].Value.Split(new string[] { "/" }, StringSplitOptions.None);
                                    if (tels.Length == 1)
                                    {
                                        if (phone[j].Groups[1].Value.Contains("-"))
                                        {
                                            guhua = phone[j].Groups[1].Value;
                                        }
                                        else
                                        {
                                            shouji = phone[j].Groups[1].Value;
                                        }
                                    }
                                    if (tels.Length == 2)
                                    {
                                        if (phone[j].Groups[1].Value.Contains("-"))
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

                                    ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                    listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                    listViewItem.SubItems.Add(shouji);
                                    listViewItem.SubItems.Add(guhua);
                                    listViewItem.SubItems.Add(address[j].Groups[1].Value);

                                    listViewItem.SubItems.Add(comboBox2.Text);
                                    listViewItem.SubItems.Add(ranks[j].Groups[1].Value);
                                   // listViewItem.SubItems.Add(cate[j].Groups[1].Value);

                                    Thread.Sleep(100);
                                    count = count + 1;
                                    label4.Text = count.ToString();
                                    if (status == false)
                                        return;
                                }



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

        private string Request_39_98_227_121(string body)
        {

            string html = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://39.98.227.121/mtdpdbser/getdata.php");

                request.Accept = "*/*";
                request.UserAgent = "Baiduspider+(+http://www.baidu.com/search/spider.htm)";
                request.Referer = "https://39.98.227.121/mtdpdbser/getdata.php";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-cn,zh,en");
                request.ContentType = "application/x-www-form-urlencoded";

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;


                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                html = reader.ReadToEnd();
                reader.Close();


                response.Close();
            }
            catch (WebException e)
            {

                return "";
            }

            return html;

        }
        #region 获取区域新
        public void getareas2(string cityid)
        {
            areadics.Clear();

            string Url = "https://m.dianping.com/mtbeauty/index/ajax/loadnavigation?token=gORmhG3WtAc9Pfr4vTbhivSxQk0AAAAADA4AAPrp_ewNUU2qGaRBE9FjidEQTVrC4_z5BShh7mlouJWGaKp4u3_FM5r8Gh5U2I2LrQ&cityid=" + cityid + "&cateid=22&categoryids=22&lat=33.96271&lng=118.24239&userid=&uuid=oJVP50IRqKIIshugSqrvYE3OHJKQ&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false";


            string html = meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"""name"":""([\s\S]*?)"",""id"":([\s\S]*?),");


            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[1].Value.Contains("区") || areas[i].Groups[1].Value.Contains("县"))
                {
                    if (!areas[i].Groups[1].Value.Contains("小区") && !areas[i].Groups[1].Value.Contains("街区") && !areas[i].Groups[1].Value.Contains("商业区") && !areas[i].Groups[1].Value.Contains("城区") && !areas[i].Groups[1].Value.Contains("市区") && !areas[i].Groups[1].Value.Contains("地区") && !areas[i].Groups[1].Value.Contains("社区") && areas[i].Groups[1].Value.Length < 5)
                    {
                        if (!areadics.ContainsKey(areas[i].Groups[1].Value))
                        {

                            areadics.Add(areas[i].Groups[1].Value, areas[i].Groups[2].Value);

                        }
                    }
                }

            }


        }

        #endregion
        #region  获取城市拼音全拼

        public string Getpinyin(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city);
                string html = GetUrl(url);
                Match suoxie = Regex.Match(html, @"""cityPinYin"":""([\s\S]*?)""");

                return suoxie.Groups[1].Value;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }


        }

        #endregion

        Dictionary<string, string> areadics = new Dictionary<string, string>();
        #region GET请求
        public string meituan_GetUrl(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/4G Language/zh_CN";
                WebHeaderCollection headers = request.Headers;
                headers.Add("uuid: E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576");
                headers.Add("mtgsig: {\"a1\":\"1.1\",\"a2\":1634620588257,\"a3\":\"35y5z0zvw6yx5z4418uyu2uyvwx65w0782x076xu37688988u426z26y\",\"a4\":\"bc19900376f1b535039019bc35b5f176e250e32ad7e5125c\",\"a5\":\"JAa73Rfx72tnXVeYw8MDLxPZTTTerl4Af4me1ULhxiBetB6AXdLwp2+LjByyFY4HvTVzwiTHo+Vs58iZE6nq4BmhF0KabSOZyOlt6Xc0hYGOghvfxeOmtWIibqgvO9QIKazOwEWESNgYcAcAsgCYccIJ0ilyWJqgc+qG5RPNTQAx8j2vzKgSEcAhtM/xXMqw4IjqrBHST/WVB9/VV7KQ4vJj1ox1p6Dt\",\"a6\":\"w1.00tlrgKaQ8nF6hTsiCnWypNRjsnhPgjiZt1klOT118rN8SWnNfMJ4xKgbXs6lejv0g1y1GwSzNFDnVx0kOF2VQu5Ki7qmfklvvTwvNHxM3oVmA0pjXjMGAP1+0Fn59TSCPUyqbs9iBKzi/lP75X+7fQ368GCr6dk9ubOsKNE+l1A8OTEBFEovMQxgHRLctnAIjrJKrB9+xNS6v6D77d28saJDOUx5NjRu7ojTyv+06R8IpZcZDIphyvQdruyN2m3UEO8iGKbUxkAB9HWk2hnq5yMM3kim+Ai/OV/raiKcm05hgl+dCoHbvrB3zky80QUHO2gyiTQwSGLHw0DFUANPKhsCFgVmK9sYG0MlOafqQj0fyuNcK/MNjwNGtuOb/8gQBNJ8O0hbV1yS+VUktM8WJqMvg+DSmFX4zUU0RVS6wSZEscUjJ8MroYbdkHET6yqeP5HN44eA2F9ZHa2FkRa6apP+XGoAg7XoaQj518PQjgCDwbHD0RwskPolSnJ31BdoOWlSLDXB3BVxlOvzpQtKnuqt077PyOmXS4mXCJkxOURnd9elQz7RTL1aTP14KvP5RaZmvU3PaGEwhB1DfLvP6CfAwurTGMSXEJ+zMSfc5KXl+CIlqPSZYGhoj9rKg/XJB89nMlNmgIqKwK8cFJ7J4hEC7AEtoRBB6ll3Zh8OBDqZ+CbuTFadF3U4ABbsmjQQfqgof3dGTZJQVpBwL593Zx8PzHDveZm9erO0okGu1Sg=\",\"a7\":\"wxde8ac0a21135c07d\",\"x0\":3,\"d1\":\"a202f030aa475f308799f76cefc1ad7b\"}");
                headers.Add("openIdCipher: AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADhq+X5+N+Cjq/cZyyWQkbVlw1zTBRltsV8Tsu1RC6Eq82jKTGdFzlq8MpEWZIJ53XNCHlmCUGib7Q==");
                // headers.Add("openId: oJVP50IRqKIIshugSqrvYE3OHJKQ");
                //WebProxy proxy = new WebProxy(textBox3.Text.Trim());
                //request.Proxy = proxy;
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
        #region  主程序爬取神灯详情页
        public void run_shendeng()
        {
            int count = 0;

            try
            {
                toolStripStatusLabel1.Text = "正在采集....";

                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("请输入城市！");
                    return;
                }
                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string city in citys)
                {
                    if (city == "")
                    {
                        continue;
                    }
                    string cityid = GetcityId(city.Replace("市", ""));

                    string citypinyin = Getpinyin(city.Replace("市", ""));

                    getareas2(cityid);

                    foreach (string areaid in areadics.Values)
                    {

                        foreach (string keyword in keywords)

                        {
                            for (int i = 0; i < 1000; i = i + 8)

                            {


                                string Url = "https://i.meituan.com/api/vc/mtshoplist/client/easylife?cateId=&cityid=" + cityid + "&start=" + i + "&limit=20&tabKeyWord=" + keyword + "&tagName=%E5%85%A8%E9%83%A8&clienttype=200&dpid=&areaId=" + areaid;

                                string html = meituan_GetUrl(Url); ;  //定义的GetRul方法 返回 reader.ReadToEnd()

                                //textBox2.Text = Url;
                                MatchCollection uids = Regex.Matches(html, @"""shopId"":""([\s\S]*?)""");
                                MatchCollection picurls = Regex.Matches(html, @"picUrls"":\[([\s\S]*?)\@");
                                MatchCollection shopPowers = Regex.Matches(html, @"shopPower"":([\s\S]*?),");
                                if (html.Trim() == "")
                                {
                                    MessageBox.Show("ip被屏蔽");
                                    continue;
                                }
                              
                                if (uids.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                                {

                                    //break;
                                    Thread.Sleep(2000);
                                    continue;

                                }


                                StringBuilder sb = new StringBuilder();
                                for (int z = 0; z < uids.Count; z++)
                                {


                                    sb.Append(uids[z].Groups[1].Value + ",");


                                }


                                string postdata = "{\"cityname\":\"" + citypinyin + "\",\"poiidlist\":[" + sb.ToString().Remove(sb.ToString().Length - 1, 1) + "]}";

                                string strhtml1 = Request_39_98_227_121(postdata);  //定义的GetRul方法 返回 reader.ReadToEnd()
                                strhtml1 = method.Unicode2String(strhtml1);

                                MatchCollection mtids = Regex.Matches(strhtml1, @"mtid"":""([\s\S]*?)""");
                                MatchCollection names = Regex.Matches(strhtml1, @"title"":""([\s\S]*?)""");
                                MatchCollection tels = Regex.Matches(strhtml1, @"phonestr"":""([\s\S]*?)""");
                                MatchCollection addrs = Regex.Matches(strhtml1, @"address"":""([\s\S]*?)""");

                                //MessageBox.Show(strhtml1);

                                for (int a = 0; a < names.Count; a++)
                                {
                                    if (!finishes.Contains(tels[a].Groups[1].Value))
                                    {
                                        string[] tel = tels[a].Groups[1].Value.Split(new string[] { "\\/" }, StringSplitOptions.None);

                                        string phone = "";
                                        string guhua = "";
                                        if (tel.Length == 1)
                                        {
                                            if (tel[0].Contains("-"))
                                            {

                                                guhua = tel[0];
                                            }
                                            else
                                            {
                                                phone = tel[0];

                                            }
                                        }
                                        if (tel.Length > 1)
                                        {
                                            if (tel[0].Contains("-"))
                                            {
                                                phone = tel[1];
                                                guhua = tel[0];
                                            }
                                            if (tel[1].Contains("-"))
                                            {
                                                phone = tel[0];
                                                guhua = tel[1];
                                            }
                                        }

                                        finishes.Add(tels[a].Groups[1].Value);
                                        StringBuilder cpsb = new StringBuilder();

                                        count = count + 1;
                                        label4.Text = count.ToString();
                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(names[a].Groups[1].Value);
                                        listViewItem.SubItems.Add(guhua);
                                        listViewItem.SubItems.Add(phone);
                                        listViewItem.SubItems.Add(addrs[a].Groups[1].Value);
                                        listViewItem.SubItems.Add(city);


                                        if (status == false)
                                            return;
                                        Thread.Sleep(500);

                                    }

                                }

                                Thread.Sleep(2000);
                            }

                        }


                    }
                }
            }




            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            toolStripStatusLabel1.Text = "完成";
        }

        #endregion
        ArrayList finishes = new ArrayList();
        private void Form1_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            listView1.SmallImageList = imgList;
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

        #region  主程序
        public void run()
        {
            int count = 0;

            try
            {
                toolStripStatusLabel1.Text = "正在采集....";

                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("请输入城市！");
                    return;
                }
                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string city in citys)
                {
                    if (city == "")
                    {
                        continue;
                    }
                    string cityid = GetcityId(city.Replace("市", ""));

                    string citypinyin = Getpinyin(city.Replace("市", ""));

                    getareas2(cityid);

                    foreach (string areaid in areadics.Values)
                    {

                        foreach (string keyword in keywords)

                        {
                            for (int i = 0; i < 1000; i = i + 8)

                            {


                                string Url = "https://i.meituan.com/api/vc/mtshoplist/client/easylife?cateId=&cityid=" + cityid + "&start=" + i + "&limit=20&tabKeyWord=" + keyword + "&tagName=%E5%85%A8%E9%83%A8&clienttype=200&dpid=&areaId=" + areaid;

                                string html = meituan_GetUrl(Url); ;  //定义的GetRul方法 返回 reader.ReadToEnd()

                                //textBox2.Text = Url;
                                MatchCollection uids = Regex.Matches(html, @"""shopId"":""([\s\S]*?)""");
                                MatchCollection picurls = Regex.Matches(html, @"picUrls"":\[([\s\S]*?)\@");
                                MatchCollection shopPowers = Regex.Matches(html, @"shopPower"":([\s\S]*?),");
                                if (html.Trim() == "")
                                {
                                    MessageBox.Show("ip被屏蔽");
                                    continue;
                                }

                                if (uids.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                                {

                                    //break;
                                    Thread.Sleep(2000);
                                    continue;

                                }


                                for (int j = 0; j < uids.Count; j++)
                                {

                                    string aurl = "https://i.meituan.com/wrapapi/allpoiinfo?riskLevel=71&optimusCode=10&poiId=" + uids[j].Groups[1].Value + "&isDaoZong=false";

                                    string strhtml = meituan_GetUrl(aurl);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                    // MessageBox.Show(strhtml);
                                    string name = Regex.Match(strhtml, @"name"":""([\s\S]*?)""").Groups[1].Value;
                                    string tels = Regex.Match(strhtml, @"phone"":""([\s\S]*?)""").Groups[1].Value;
                                    string addr = Regex.Match(strhtml, @"address"":""([\s\S]*?)""").Groups[1].Value;



                                    if (!finishes.Contains(tels))
                                    {
                                        string[] tel = tels.Split(new string[] { "/" }, StringSplitOptions.None);

                                        string phone = "";
                                        string guhua = "";
                                        if (tel.Length == 1)
                                        {
                                            if (tel[0].Contains("-"))
                                            {

                                                guhua = tel[0];
                                            }
                                            else
                                            {
                                                phone = tel[0];

                                            }
                                        }
                                        if (tel.Length > 1)
                                        {
                                            if (tel[0].Contains("-"))
                                            {
                                                phone = tel[1];
                                                guhua = tel[0];
                                            }
                                            if (tel[1].Contains("-"))
                                            {
                                                phone = tel[0];
                                                guhua = tel[1];
                                            }
                                        }

                                        finishes.Add(tels);
                                        StringBuilder cpsb = new StringBuilder();

                                        count = count + 1;
                                        label4.Text = count.ToString();
                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(name);
                                        listViewItem.SubItems.Add(guhua);
                                        listViewItem.SubItems.Add(phone);
                                        listViewItem.SubItems.Add(addr);
                                        listViewItem.SubItems.Add(city);


                                        if (status == false)
                                            return;
                                        Thread.Sleep(2000);

                                    }
                                }




                                Thread.Sleep(2000);
                            }

                        }


                    }
                }
            }




            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            toolStripStatusLabel1.Text = "完成";
        }

        #endregion


        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测

            string ahtml = GetUrl("http://124.222.26.180");

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

            if (thread == null || !thread.IsAlive)
            {
              
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            else
            {
                status = false;
            }
            
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
           

          
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

            if (comboBox1.Text.Contains("上海"))
            {
                comboBox2.Text = "上海";
                comboBox2.Items.Clear();
                comboBox2.Items.Add("上海");
                textBox1.Text += "上海";

                return;
            }
            if (comboBox1.Text.Contains("北京"))
            {
                comboBox2.Text = "北京";
                comboBox2.Items.Clear();
                comboBox2.Items.Add("北京");
                textBox1.Text += "北京";
                return;
            }
            if (comboBox1.Text.Contains("重庆"))
            {
                comboBox2.Text = "重庆";
                comboBox2.Items.Clear();
                comboBox2.Items.Add("重庆");
                textBox1.Text += "重庆";
                return;
            }
            if (comboBox1.Text.Contains("天津"))
            {
                comboBox2.Text = "天津";
                comboBox2.Items.Clear();
                comboBox2.Items.Add("天津");
                textBox1.Text += "天津";
                return;
            }


            if (textBox1.Text.Contains(comboBox2.Text))
            {
                MessageBox.Show(comboBox2.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox1.Text += comboBox2.Text + "\r\n";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Contains(comboBox3.Text))
            {
                MessageBox.Show(comboBox3.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox2.Text += comboBox3.Text + "\r\n";
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
