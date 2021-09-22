using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        #region  主函数
        public void run()

        {

           
            try

            {
                label1.Text = "开始采集";
                string cityid = GetcityId(comboBox3.Text.Replace("市", ""));
                ArrayList areaIds = getAreaId(cityid);

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
                        label1.Text = "获取到数量："+lists.Count.ToString();
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            continue;


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




                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(titles.Groups[1].Value);

                            lv1.SubItems.Add(addr.Groups[1].Value);
                            lv1.SubItems.Add(zhuangxiu.Groups[1].Value);
                            lv1.SubItems.Add(fangjian.Groups[1].Value);
                            lv1.SubItems.Add(phone.Groups[1].Value.Replace("u002F", " "));
                           
                            lv1.SubItems.Add(type.Groups[1].Value);
                            lv1.SubItems.Add(comboBox2.Text);
                            lv1.SubItems.Add(city.Groups[1].Value);
                            lv1.SubItems.Add(prices[j].Groups[1].Value);



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
    }
}
