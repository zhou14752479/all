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

namespace 主程序202106
{
    public partial class 美团酒店 : Form
    {
        public 美团酒店()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        #region GET请求
        public static string meituan_GetUrl(string Url)
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
        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city);
                string html = method.GetUrl(url,"utf-8");
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
        public ArrayList getAreaId(string cityid)
        {

            ArrayList lists = new ArrayList();
            Dictionary<string, string> dics = new Dictionary<string, string>(); ;
            string Url = "https://i.meituan.com/wrapapi/search/filters?riskLevel=71&optimusCode=10&ci=" + cityid;

            string html = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"{""id"":([\s\S]*?),([\s\S]*?)""name"":""([\s\S]*?)""");


            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[3].Value.Contains("区") || areas[i].Groups[3].Value.Contains("县"))
                {
                    if (!areas[i].Groups[3].Value.Contains("小区") && !areas[i].Groups[3].Value.Contains("街区") && !areas[i].Groups[3].Value.Contains("商业区") && !areas[i].Groups[3].Value.Contains("城区") && !areas[i].Groups[3].Value.Contains("市区") && !areas[i].Groups[3].Value.Contains("地区") && !areas[i].Groups[3].Value.Contains("社区") && areas[i].Groups[3].Value.Length < 5)
                    {
                        if (!dics.ContainsKey(areas[i].Groups[3].Value))
                        {
                            lists.Add(areas[i].Groups[1].Value);

                        }
                    }
                }

            }

            return lists;
        }

        #endregion


        #region 获取区域新
        public ArrayList getareas2(string cityid)
        {
            ArrayList lists = new ArrayList();

            string Url = "https://m.dianping.com/mtbeauty/index/ajax/loadnavigation?token=gORmhG3WtAc9Pfr4vTbhivSxQk0AAAAADA4AAPrp_ewNUU2qGaRBE9FjidEQTVrC4_z5BShh7mlouJWGaKp4u3_FM5r8Gh5U2I2LrQ&cityid=" + cityid + "&cateid=22&categoryids=22&lat=33.96271&lng=118.24239&userid=&uuid=oJVP50IRqKIIshugSqrvYE3OHJKQ&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false";

            string html = meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"""name"":""([\s\S]*?)"",""id"":([\s\S]*?),");


            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[1].Value.Contains("区") || areas[i].Groups[1].Value.Contains("县"))
                {
                    if (!areas[i].Groups[1].Value.Contains("小区") && !areas[i].Groups[1].Value.Contains("街区") && !areas[i].Groups[1].Value.Contains("商业区") && !areas[i].Groups[1].Value.Contains("城区") && !areas[i].Groups[1].Value.Contains("市区") && !areas[i].Groups[1].Value.Contains("地区") && !areas[i].Groups[1].Value.Contains("社区") && areas[i].Groups[1].Value.Length < 5)
                    {
                        if (!lists.Contains(areas[i].Groups[2].Value))
                        {
                            lists.Add(areas[i].Groups[2].Value);

                        }
                    }
                }

            }

            return lists;
        }

        List<string> finishes = new List<string>();
        #endregion


        #region 筛选
        public string shaixuan(string tel)
        {
            try
            {
                string haoma = tel;
                string[] tels = tel.Split(new string[] { "\\" }, StringSplitOptions.None);

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
                if (checkBox3.Checked == true)
                {
                    finishes.Add(haoma);
                }
                return haoma.Trim();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(tel+"   " +ex.ToString());
                return "";
            }


        }
        #endregion
        #region  主函数
        public void run()

        {

           
            try

            {
                label1.Text = "开始采集";

                string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var item in text)
                {
                    string cityid = GetcityId(item.Replace("市", ""));
                    ArrayList areaIds = getareas2(cityid);

                    foreach (string areaId in areaIds)
                    {


                        for (int i = 0; i < 1001; i = i + 20)
                        {
                            string url = "https://ihotel.meituan.com/hbsearch/HotelSearch?utm_medium=pc&version_name=999.9&cateId=20&attr_28=129&cityId=" + cityid + "&areaId=" + areaId + "&offset=" + i + "&limit=20&startDay=" + DateTime.Now.ToString("yyyyMMdd") + "&endDay=" + DateTime.Now.ToString("yyyyMMdd") + "&q=&sort=defaults";

                            textBox1.Text = url;
                            string html = method.GetUrl(url, "utf-8");


                            MatchCollection matchs = Regex.Matches(html, @"""realPoiId"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            MatchCollection prices = Regex.Matches(html, @"""lowestPrice"":([\s\S]*?),");

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in matchs)
                            {
                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            label1.Text = "获取到数量：" + lists.Count.ToString();
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                            {
                                Thread.Sleep(1000);
                                if (i > 20)
                                {
                                    i = i - 20;
                                }
                                continue;
                            }



                            for (int j = 0; j < lists.Count; j++)
                            {

                                string strhtml = method.GetUrl("https://hotel.meituan.com/" + lists[j] + "/", "utf-8");

                                Match titles = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                                Match addr = Regex.Match(strhtml, @"""addr"":""([\s\S]*?)""");
                                Match zhuangxiu = Regex.Match(strhtml, @"装修时间"",""attrValue"":""([\s\S]*?)""");
                                Match fangjian = Regex.Match(strhtml, @"客房总量"",""attrValue"":""([\s\S]*?)""");
                                Match phone = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");

                                Match type = Regex.Match(strhtml, @"""hotelStar"":""([\s\S]*?)""");
                                Match city = Regex.Match(strhtml, @"""cityName"":""([\s\S]*?)""");


                                string newphone = shaixuan(phone.Groups[1].Value.Replace("u002F", " "));
                                if (newphone != "")
                                {
                                    if (!finishes.Contains(newphone))
                                    {

                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                        lv1.SubItems.Add(titles.Groups[1].Value);

                                        lv1.SubItems.Add(addr.Groups[1].Value);
                                        lv1.SubItems.Add(zhuangxiu.Groups[1].Value);
                                        lv1.SubItems.Add(fangjian.Groups[1].Value);
                                        lv1.SubItems.Add(newphone);

                                        lv1.SubItems.Add(type.Groups[1].Value);
                                        lv1.SubItems.Add(comboBox2.Text);
                                        lv1.SubItems.Add(city.Groups[1].Value);
                                        lv1.SubItems.Add("无");
                                    }
                                }

                                Thread.Sleep(100);
                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                if (this.status == false)

                                {
                                    return;
                                }


                            }
                        }

                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"xlD23Y"))
            {
                return;
            }

            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 美团酒店_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox2);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text.Contains("上海"))
            {
               comboBox3.Text = "上海";
                return;
            }
            if (comboBox2.Text.Contains("北京"))
            {
                comboBox3.Text = "北京";
                return;
            }
            if (comboBox2.Text.Contains("重庆"))
            {
                comboBox3.Text = "重庆";
                return;
            }
            if (comboBox2.Text.Contains("天津"))
            {
                comboBox3.Text = "天津";
                return;
            }
            ProvinceCity.ProvinceCity.BindCity(comboBox2, comboBox3);
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!textBox2.Text.Contains(comboBox3.Text))
            {
                textBox2.Text += comboBox3.Text + "\r\n";
            }
        }
    }
}
