using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;
using static myDLL.method;

namespace 美团
{
    public partial class 美团附近 : Form
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


        public 美团附近()
        {
            InitializeComponent();
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += comboBox3.Text + "\r\n";

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox2, comboBox3);

        }
        #region GET请求
        public string meituan_GetUrl(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/4G Language/zh_CN";
                WebHeaderCollection headers = request.Headers;
                //headers.Add("uuid: E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576");
                headers.Add("mtgsig: {\"a1\":\"1.1\",\"a2\":1634620588257,\"a3\":\"35y5z0zvw6yx5z4418uyu2uyvwx65w0782x076xu37688988u426z26y\",\"a4\":\"bc19900376f1b535039019bc35b5f176e250e32ad7e5125c\",\"a5\":\"JAa73Rfx72tnXVeYw8MDLxPZTTTerl4Af4me1ULhxiBetB6AXdLwp2+LjByyFY4HvTVzwiTHo+Vs58iZE6nq4BmhF0KabSOZyOlt6Xc0hYGOghvfxeOmtWIibqgvO9QIKazOwEWESNgYcAcAsgCYccIJ0ilyWJqgc+qG5RPNTQAx8j2vzKgSEcAhtM/xXMqw4IjqrBHST/WVB9/VV7KQ4vJj1ox1p6Dt\",\"a6\":\"w1.00tlrgKaQ8nF6hTsiCnWypNRjsnhPgjiZt1klOT118rN8SWnNfMJ4xKgbXs6lejv0g1y1GwSzNFDnVx0kOF2VQu5Ki7qmfklvvTwvNHxM3oVmA0pjXjMGAP1+0Fn59TSCPUyqbs9iBKzi/lP75X+7fQ368GCr6dk9ubOsKNE+l1A8OTEBFEovMQxgHRLctnAIjrJKrB9+xNS6v6D77d28saJDOUx5NjRu7ojTyv+06R8IpZcZDIphyvQdruyN2m3UEO8iGKbUxkAB9HWk2hnq5yMM3kim+Ai/OV/raiKcm05hgl+dCoHbvrB3zky80QUHO2gyiTQwSGLHw0DFUANPKhsCFgVmK9sYG0MlOafqQj0fyuNcK/MNjwNGtuOb/8gQBNJ8O0hbV1yS+VUktM8WJqMvg+DSmFX4zUU0RVS6wSZEscUjJ8MroYbdkHET6yqeP5HN44eA2F9ZHa2FkRa6apP+XGoAg7XoaQj518PQjgCDwbHD0RwskPolSnJ31BdoOWlSLDXB3BVxlOvzpQtKnuqt077PyOmXS4mXCJkxOURnd9elQz7RTL1aTP14KvP5RaZmvU3PaGEwhB1DfLvP6CfAwurTGMSXEJ+zMSfc5KXl+CIlqPSZYGhoj9rKg/XJB89nMlNmgIqKwK8cFJ7J4hEC7AEtoRBB6ll3Zh8OBDqZ+CbuTFadF3U4ABbsmjQQfqgof3dGTZJQVpBwL593Zx8PzHDveZm9erO0okGu1Sg=\",\"a7\":\"wxde8ac0a21135c07d\",\"x0\":3,\"d1\":\"a202f030aa475f308799f76cefc1ad7b\"}");
                headers.Add("openIdCipher: AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADhq+X5+N+Cjq/cZyyWQkbVlw1zTBRltsV8Tsu1RC6Eq82jKTGdFzlq8MpEWZIJ53XNCHlmCUGib7Q==");
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
           
            string Url = "https://i.meituan.com/wrapapi/search/filters?riskLevel=71&optimusCode=10&ci=" + cityid;

            string html = meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"{""id"":([\s\S]*?),([\s\S]*?)""name"":""([\s\S]*?)""");


            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[3].Value.Contains("区") || areas[i].Groups[3].Value.Contains("县"))
                {
                    if (!areas[i].Groups[3].Value.Contains("小区") && !areas[i].Groups[3].Value.Contains("街区") && !areas[i].Groups[3].Value.Contains("商业区") && !areas[i].Groups[3].Value.Contains("城区") && !areas[i].Groups[3].Value.Contains("市区") && !areas[i].Groups[3].Value.Contains("地区") && !areas[i].Groups[3].Value.Contains("社区") && areas[i].Groups[3].Value.Length < 5)
                    {
                        if (!areadics.ContainsKey(areas[i].Groups[3].Value))
                        {
                            areadics.Add(areas[i].Groups[3].Value, areas[i].Groups[1].Value);
                           
                        }
                    }
                }

            }


        }

        #endregion


        #region 获取区域新
        public void getareas2(string cityid)
        {
            areadics.Clear();

            string Url = "https://m.dianping.com/mtbeauty/index/ajax/loadnavigation?token=gORmhG3WtAc9Pfr4vTbhivSxQk0AAAAADA4AAPrp_ewNUU2qGaRBE9FjidEQTVrC4_z5BShh7mlouJWGaKp4u3_FM5r8Gh5U2I2LrQ&cityid=" + cityid+"&cateid=22&categoryids=22&lat=33.96271&lng=118.24239&userid=&uuid=oJVP50IRqKIIshugSqrvYE3OHJKQ&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false" ;
         

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

        #region  获取城市缩写

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

        bool zanting = true;
        bool status = true;
        ArrayList tels = new ArrayList();
       

        ArrayList finishes = new ArrayList();

        #region  主程序进入详情页
        public void run1()
        {
           
         
            try
            {


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

                    getareas(cityid);
                    foreach (string areaid in areadics.Values)
                    {

                        foreach (string keyword in keywords)

                        {
                            for (int i = 0; i < 1000; i = i + 15)

                            {


                                string Url = "https://apimobile.meituan.com/group/v4/poi/search/miniprogram/"+cityid+"?riskLevel=71&optimusCode=10&cateId=-1&sort=defaults&userid=875973616&token=1xJvk8LnaH5uRJ9WIEZox2pa99sAAAAA3A4AAKzWRCSrTuTmzu3HawOj2Po92W7ng58CgBzdk8xsx6DembWVtwkDgbGqkQKrILpf9g&offset="+i+"&limit=8&cityId="+cityid+"&mypos=33.939910888671875%2C118.25335693359375&uuid=17c923ced47c8-128ea2668d900c-0-0-17c923ced47c8&version_name=11.6.200&supportDisplayTemplates=itemA%2CitemB%2CitemJ%2CitemP%2CitemS%2CitemM%2CitemY%2CitemL&supportTemplates=default%2Chotel%2Cblock%2Cnofilter%2Ccinema&searchSource=miniprogram&ste=_b100000&openId=oJVP50IRqKIIshugSqrvYE3OHJKQ&q="+keyword+"&areaId=" + areaid;
                                
                                string html = meituan_GetUrl(Url); ;  //定义的GetRul方法 返回 reader.ReadToEnd()
                            
                                MatchCollection uids = Regex.Matches(html, @"\{""poiid"":([\s\S]*?),");

                              

                                if (uids.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                                {
                                    Thread.Sleep(1000);
                                    break;
                                }



                                foreach (string uid in uids)

                                {
                                    string aurl = "https://i.meituan.com/wrapapi/allpoiinfo?riskLevel=71&optimusCode=10&poiId=" + uid + "&isDaoZong=true";
                                    string strhtml1 = meituan_GetUrl(aurl);  //定义的GetRul方法 返回 reader.ReadToEnd()
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


            toolStripStatusLabel1.Text = "开始抓取......";

            try
            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string city in citys)
                {
                    if (city == "")
                    {
                        continue;
                    }
                    string cityid = GetcityId(city.Replace("市", ""));
                    getareas(cityid);
                    foreach (string areaid in areadics.Values)
                    {
                      
                        string[] catenames = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        foreach (string catename in catenames)
                        {
                           
                            if (catename == "")
                            {
                                continue;
                            }
                            string cateid = catedic[catename];
                          
                            for (int i = 0; i < 1001; i = i + 100)

                            {

                                toolStripStatusLabel1.Text = "正在抓取："+city+"--"+catename+"--"+i;
                               string Url = "https://m.dianping.com/mtbeauty/index/ajax/shoplist?token=&cityid=" + cityid + "&cateid=22&categoryids=" + cateid + "&lat=33.94114303588867&lng=118.2479019165039&userid=&uuid=&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false&start=" + i + "&limit=100&areaid=" + areaid + "&distance=&subwaylineid=&subwaystationid=&sort=2";

                               
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
                                    //if (!finishes.Contains(phone[j].Groups[1].Value))
                                    //{
                                    //    finishes.Add(phone[j].Groups[1].Value);
                                        ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                        listViewItem.SubItems.Add(address[j].Groups[1].Value);
                                        listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                                        listViewItem.SubItems.Add(cate[j].Groups[1].Value);
                                        listViewItem.SubItems.Add(shangquan[j].Groups[1].Value);
                                        listViewItem.SubItems.Add(city);
                                        Thread.Sleep(500);
                                   // }
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    if (status == false)
                                        return;
                                }

                              
                                Thread.Sleep(1000);
                                if (names.Count == 0 || names.Count < 100)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                                {
                                    break;
                                }
                            }
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


        #region  主程序爬取神灯详情页
        public void run_shendeng()
        {


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


                                string Url = "https://i.meituan.com/api/vc/mtshoplist/client/easylife?cateId=&cityid="+cityid+"&start="+i+"&limit=20&tabKeyWord="+keyword+"&tagName=%E5%85%A8%E9%83%A8&clienttype=200&dpid=&areaId=" + areaid;

                                string html = meituan_GetUrl(Url); ;  //定义的GetRul方法 返回 reader.ReadToEnd()

                                //textBox2.Text = html;
                                MatchCollection uids = Regex.Matches(html, @"""shopId"":""([\s\S]*?)""");

                             
                               if(html.Trim()=="")
                                {
                                    MessageBox.Show("ip被屏蔽");
                                    continue;
                                }
                              
                                if (uids.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                                {
                                    break;
                                   // Thread.Sleep(2000);
                                    //continue;

                                }


                                StringBuilder sb = new StringBuilder();
                                foreach (Match uid in uids)
                                {
                                   
                                    sb.Append(uid.Groups[1].Value+",");
                                 
                                     
                                }
                             
                               
                                string postdata = "{\"cityname\":\""+citypinyin+"\",\"poiidlist\":["+sb.ToString().Remove(sb.ToString().Length-1,1)+"]}";
                               
                                string strhtml1 =Request_39_98_227_121(postdata);  //定义的GetRul方法 返回 reader.ReadToEnd()
                                strhtml1 = method.Unicode2String(strhtml1);
                               
                               
                                MatchCollection names = Regex.Matches(strhtml1, @"title"":""([\s\S]*?)""");
                                MatchCollection tels = Regex.Matches(strhtml1, @"phonestr"":""([\s\S]*?)""");
                                MatchCollection addrs = Regex.Matches(strhtml1, @"address"":""([\s\S]*?)""");



                                for (int a = 0; a < names.Count; a++)
                                {
                                    if (!finishes.Contains(tels[a].Groups[1].Value))
                                    {
                                        string newphone = shaixuan(tels[a].Groups[1].Value);
                                        if (newphone != "")
                                        {
                                           

                                            finishes.Add(tels[a].Groups[1].Value);
                                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                            listViewItem.SubItems.Add(names[a].Groups[1].Value);
                                            listViewItem.SubItems.Add(addrs[a].Groups[1].Value);
                                            if (jihuo == false)
                                            {
                                                if (newphone.Length > 4)
                                                {
                                                    listViewItem.SubItems.Add(newphone.Substring(0, 4) + "*******");
                                                }

                                            }
                                            else
                                            {
                                                listViewItem.SubItems.Add(newphone);
                                            }
                                           
                                            listViewItem.SubItems.Add(keyword);
                                            listViewItem.SubItems.Add(city);
                                            while (this.zanting == false)
                                            {
                                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                            }
                                            if (status == false)
                                                return;
                                           Thread.Sleep(500);
                                        }
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
               // MessageBox.Show(ex.ToString());
            }

            toolStripStatusLabel1.Text = "完成";
        }

        #endregion

        #region  全国餐饮店
        public void getall()
        {
            ArrayList cityids = getallcitys();

            foreach (string cityid in cityids)
            {

                try
                {
                    for (int i = 0; i < 100001; i = i + 100)

                    {

                        string Url = "https://m.dianping.com/mtbeauty/index/ajax/shoplist?token=&cityid=" + cityid + "&cateid=22&categoryids=1&lat=33.94114303588867&lng=118.2479019165039&userid=&uuid=&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false&start=" + i + "&limit=100&areaid=-1&distance=&subwaylineid=&subwaystationid=&sort=2";

                        string html = meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()



                        MatchCollection names = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                        MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                        MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                        MatchCollection cate = Regex.Matches(html, @"mainCategoryName"":""([\s\S]*?)""");
                        MatchCollection shangquan = Regex.Matches(html, @"""areaName"":""([\s\S]*?)""");

                        MatchCollection newShop = Regex.Matches(html, @"""newShop"":([\s\S]*?),");

                        if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        for (int j = 0; j < names.Count; j++)

                        {
                            if (newShop[j].Groups[1].Value.Trim().ToLower()=="true")
                            {
                               
                                ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                listViewItem.SubItems.Add(address[j].Groups[1].Value);
                                listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                                listViewItem.SubItems.Add(cate[j].Groups[1].Value);
                                listViewItem.SubItems.Add(shangquan[j].Groups[1].Value);
                                listViewItem.SubItems.Add(newShop[j].Groups[1].Value);
                            }
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                        }

                    Thread.Sleep(1000);



                }
                catch (System.Exception ex)
                {
                    ex.ToString();
                }
            }

        }
        #endregion

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset,  string refer)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
              
                request.ContentType = "application/x-www-form-urlencoded";
               
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = refer;
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

                return ex.ToString();
            }


        }

        #endregion

        #region 筛选
        public string shaixuan(string tel)
        {
            try
            {
               
                string haoma = tel;
                string[] tels = tel.Split(new string[] { "\\/" }, StringSplitOptions.None);

                if (checkBox1.Checked == true)
                {
                    if (tels.Length == 0)
                    {
                        haoma = "";
                        return haoma;
                    }

                }
                if (checkBox2.Checked == true)
                {
                    if (tels.Length == 1)
                    {
                        if (!tel.Contains("-") && tels[0].Length > 10)
                        {
                            haoma = tel;
                            return haoma;
                        }
                        else
                        {
                            return "";
                        }
                    }

                    if (tels.Length == 2)
                    {
                        if (!tels[0].Contains("-") && tels[0].Length > 10)
                        {
                            haoma = tels[0];
                        }

                        else if (!tels[1].Contains("-") && tels[1].Length > 10)
                        {
                            haoma = tels[1];
                        }
                        else
                        {
                            haoma = "";
                        }
                    }
                }
               
                return haoma;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(tel+"   " +ex.ToString());
                return "";
            }


        }
        #endregion

        bool jihuo = true;
        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public AcceptAllCertificatePolicy()
            {
            }


            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                // Always accept
                return true;
            }
        }





        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


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
        Thread thread;

        private void Button1_Click(object sender, EventArgs e)
        {
            jihuoma();
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"abc147258"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            if (Convert.ToDateTime("2022-05-31")<DateTime.Now)
            {
                return;
            }

            status = true;



            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_shendeng);
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
        public string path = AppDomain.CurrentDomain.BaseDirectory;
        public static Dictionary<string, string> catedic = new Dictionary<string, string>();
        #region  读取分类

        public void Getcates(ComboBox cob)
        {

            try
            {
                StreamReader sr = new StreamReader(path + "cates.json", EncodingType.GetTxtType(path + "cates.json"));
                //一次性读取完 
                string html = sr.ReadToEnd();

                MatchCollection cates = Regex.Matches(html, @"""id"":([\s\S]*?),""name"":""([\s\S]*?)""");

                for (int i = 0; i < cates.Count; i++)
                {
                    if (!catedic.ContainsKey(cates[i].Groups[2].Value.Trim()))
                    {
                        cob.Items.Add(cates[i].Groups[2].Value.Trim());
                        catedic.Add(cates[i].Groups[2].Value.Trim(), cates[i].Groups[1].Value.Trim());
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存


            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }

        }



        #endregion
        private void 美团附近_Load(object sender, EventArgs e)
        {
           
            //jiance();
            Getcates(comboBox1);
            ProvinceCity.ProvinceCity.BindProvince(comboBox2);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process.Start("http://www.acaiji.com/");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text += comboBox1.Text + "\r\n";
        }

        #region 机器码
        public void jiance()
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
                string[] value = secret.Split(new string[] { "asd147" }, StringSplitOptions.None);


                if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                {
                    MessageBox.Show("激活已过期");
                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }

                }


                else if (value[0] != method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian") || key != method.GetMD5(method.GetMacAddress()))
                {

                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }
                }


            }
            else
            {

                string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);
                if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                {
                    IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                    IniWriteValue("values", "secret", str);
                    MessageBox.Show("激活成功");


                }
                else
                {
                    MessageBox.Show("激活码错误");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }

        }

        #endregion


        public void register(string jihuoma)
        {

            string html = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?method=register&username=" + jihuoma + "&password=1&days=1&type=1111jihuoma", "utf-8");



        }

        public bool login(string jihuoma)
        {
            string html = method.GetUrl("http://www.acaiji.com/shangxueba2/shangxueba.php?username=" + jihuoma + "&password=1&method=login", "utf-8");
            if (html.Contains("true"))
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
                long expiretime = Convert.ToInt64(method.GetTimeStamp()) + 365 * 24 * 3600;
                if (ExistINIFile())
                {
                    string key = IniReadValue("values", "key");

                    string[] value = key.Split(new string[] { "asd147" }, StringSplitOptions.None);


                    if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                    {
                        MessageBox.Show("激活已过期");
                        string str = Interaction.InputBox("请购买激活码,使用正式版软件！\r\n\r\n无激活码点击确定免费试用", "激活软件", "", -1, -1);
                        string fullstr = str;
                        if (login(fullstr))
                        {
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
                            if (Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(time) < 99999999)  //200秒内有效
                            {
                                if (index == "yi" || index == "san")//美团一年
                                {

                                    IniWriteValue("values", "key", macmd5 + "asd147" + expiretime);

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
                        jihuo = false; ;
                    }

                }
                else
                {
                    string str = Interaction.InputBox("请购买激活码,使用正式版软件！\r\n\r\n无激活码点击确定免费试用", "激活软件", "", -1, -1);
                    string fullstr = str;
                    if (login(fullstr))
                    {
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
                        if (Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(time) < 99999999)  //200秒内有效
                        {
                            if (index == "yi" || index == "san")//美团一年
                            {
                                IniWriteValue("values", "key", macmd5 + "asd147" + expiretime);

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
                    MessageBox.Show("激活码错误");
                    jihuo = false; ;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("激活码错误，点击试用");
                jihuo = false; ;
            }


        }


        #endregion
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
                string tel = listView1.Items[i].SubItems[3].Text.Trim();
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
        Thread thread1;
        private void button6_Click(object sender, EventArgs e)
        {

            if (thread1 == null || !thread1.IsAlive)
            {
                Thread thread1 = new Thread(creatVcf);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < comboBox3.Items.Count; i++)
            {
                if (!textBox1.Text.Contains(comboBox3.Items[i].ToString()))
                {
                    textBox1.Text += comboBox3.Items[i].ToString() + "\r\n";
                }
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://121.40.209.61/alipay2/");
            }
            catch (Exception)
            {

                System.Diagnostics.Process.Start("explorer.exe", "http://121.40.209.61/alipay2/");
            }
        }
    }
}
