using MySql.Data.MySqlClient;
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
using static fang.临时软件.百家号;

namespace fang.临时软件
{
    public partial class 物通网 : Form
    {
        bool denglu = false;

        public 物通网()
        {
            InitializeComponent();
        }

        bool status = true;

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
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "387084CFFF0AD93DF880F4BA042190E5493B8A1B5006C518363D39972896F36E846A1A184B93207E";
                request.AllowAutoRedirect = true;         
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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

        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        //   ArrayList finishes = new ArrayList();

        #region  物通网一手货源
        public void run()
        {


            try
            {

                string province = System.Web.HttpUtility.UrlEncode(comboBox1.Text);

                
                if (comboBox1.Text == "全国")
                {
                    province = "";

                }
                for (int i = 1; i < 3; i++)
                {

                    string url = "http://android.chinawutong.com/PostData.ashx?chechang=&infotype=1&condition=1&tsheng=&txian=&chexing=&huiyuan_id=2264195&fsheng=" + province + "&type=GetGood_new&fshi=&tshi=&pid="+i+"&fxian=&ver_version=1&r_20717=37619";

                    string html = GetUrl(url, "utf-8");

                    MatchCollection goods_names = Regex.Matches(html, @"goods_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection zaizhongs = Regex.Matches(html, @"zaizhong"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection trans_modes = Regex.Matches(html, @"trans_mode"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_phones = Regex.Matches(html, @"huo_phone"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_contacts = Regex.Matches(html, @"huo_contact"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection company_names = Regex.Matches(html, @"company_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    MatchCollection times = Regex.Matches(html, @"data_time"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection from_areas = Regex.Matches(html, @"from_area"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection to_areas = Regex.Matches(html, @"to_area"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    if (goods_names.Count == 0)
                        break;

                    for (int j = 0; j < goods_names.Count; j++)
                    {

                        string strhtml = GetUrl("http://www.chinawutong.com/203/ab" + from_areas[j].Groups[1].Value.Trim() + "k" + from_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");
                        string strhtml1 = GetUrl("http://www.chinawutong.com/203/ab" + to_areas[j].Groups[1].Value.Trim() + "k" + to_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");

                        Match from_area = Regex.Match(strhtml, @"<a>-([\s\S]*?)<");
                        Match to_area = Regex.Match(strhtml1, @"<a>-([\s\S]*?)<");
                        if (goods_names.Count > 0)
                        {
                            //  ListViewItem lv1 = listView1.Items.Add(from_area.Groups[1].Value + "→" + to_area.Groups[1].Value);
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString());
                            lv1.SubItems.Add(goods_names[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(zaizhongs[j].Groups[1].Value.Trim() + "公斤");
                            lv1.SubItems.Add(trans_modes[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(huo_phones[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(huo_contacts[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(company_names[j].Groups[1].Value.Trim());

                            lv1.SubItems.Add(times[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(from_area.Groups[1].Value + "→" + to_area.Groups[1].Value);


                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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


        #region  物通网全部货源
        public void run1()
        {


            try
            {

                string fprovince = System.Web.HttpUtility.UrlEncode(comboBox3.Text);
                string tprovince = System.Web.HttpUtility.UrlEncode(comboBox4.Text);


                if (comboBox1.Text == "全国")
                {
                    fprovince = "";

                }

                if (comboBox2.Text == "全国")
                {
                    tprovince = "";

                }
                for (int i = 1; i < 4; i++)
                {

                    string url = "http://android.chinawutong.com/PostData.ashx?chechang=&infotype=0&condition=1&tsheng="+tprovince+"&txian=&chexing=&huiyuan_id=2264195&fsheng="+fprovince+"&type=GetGood_new&fshi=&tshi=&pid="+i+"&fxian=&ver_version=1&r_28677=6820";

                    string html = GetUrl(url, "utf-8");

                    MatchCollection goods_names = Regex.Matches(html, @"goods_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection zaizhongs = Regex.Matches(html, @"zaizhong"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection trans_modes = Regex.Matches(html, @"trans_mode"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_phones = Regex.Matches(html, @"huo_phone"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_contacts = Regex.Matches(html, @"huo_contact"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection company_names = Regex.Matches(html, @"company_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    MatchCollection times = Regex.Matches(html, @"data_time"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection from_areas = Regex.Matches(html, @"from_area"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection to_areas = Regex.Matches(html, @"to_area"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    if (goods_names.Count == 0)
                        break;

                    for (int j = 0; j < goods_names.Count; j++)
                    {

                        string strhtml = GetUrl("http://www.chinawutong.com/203/ab" + from_areas[j].Groups[1].Value.Trim() + "k" + from_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");
                        string strhtml1 = GetUrl("http://www.chinawutong.com/203/ab" + to_areas[j].Groups[1].Value.Trim() + "k" + to_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");

                        Match from_area = Regex.Match(strhtml, @"<a>-([\s\S]*?)<");
                        Match to_area = Regex.Match(strhtml1, @"<a>-([\s\S]*?)<");
                        if (goods_names.Count > 0)
                        {
                            
                            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString());
                            lv2.SubItems.Add(goods_names[j].Groups[1].Value.Trim());
                            lv2.SubItems.Add(zaizhongs[j].Groups[1].Value.Trim() + "公斤");
                            lv2.SubItems.Add(trans_modes[j].Groups[1].Value.Trim());
                            lv2.SubItems.Add(huo_phones[j].Groups[1].Value.Trim());
                            lv2.SubItems.Add(huo_contacts[j].Groups[1].Value.Trim());
                            lv2.SubItems.Add(company_names[j].Groups[1].Value.Trim());

                            lv2.SubItems.Add(times[j].Groups[1].Value.Trim());
                            lv2.SubItems.Add(from_area.Groups[1].Value + "→" + to_area.Groups[1].Value);


                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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

        #region  重运宝
        public void run2()
        {


            try
            {

                string province = System.Web.HttpUtility.UrlEncode(comboBox1.Text);


                if (comboBox1.Text == "全国")
                {
                    province = "";

                }
                for (int i = 1; i < 2; i++)
                {

                    string url = "http://android.chinawutong.com/PostData.ashx?chechang=&infotype=1&condition=1&tsheng=&txian=&chexing=&huiyuan_id=2264195&fsheng=" + province + "&type=GetGood_new&fshi=&tshi=&pid=" + i + "&fxian=&ver_version=1&r_20717=37619";
                    string postdata = "page="+i+"&code=55882a686a2c4831486fd8809ef7d6e6";
                    string cookie = "PHPSESSID=b3sob5o8tajfro5j39n774pm43";
                    string html = method.PostUrl(url,postdata,cookie,"utf-8");

                    MatchCollection goods_names = Regex.Matches(html, @"goods_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection zaizhongs = Regex.Matches(html, @"zaizhong"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection trans_modes = Regex.Matches(html, @"trans_mode"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_phones = Regex.Matches(html, @"huo_phone"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_contacts = Regex.Matches(html, @"huo_contact"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection company_names = Regex.Matches(html, @"company_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    MatchCollection times = Regex.Matches(html, @"data_time"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection from_areas = Regex.Matches(html, @"from_area"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection to_areas = Regex.Matches(html, @"to_area"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    if (goods_names.Count == 0)
                        break;

                    for (int j = 0; j < goods_names.Count; j++)
                    {

                        string strhtml = GetUrl("http://www.chinawutong.com/203/ab" + from_areas[j].Groups[1].Value.Trim() + "k" + from_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");
                        string strhtml1 = GetUrl("http://www.chinawutong.com/203/ab" + to_areas[j].Groups[1].Value.Trim() + "k" + to_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");

                        Match from_area = Regex.Match(strhtml, @"<a>-([\s\S]*?)<");
                        Match to_area = Regex.Match(strhtml1, @"<a>-([\s\S]*?)<");
                        if (goods_names.Count > 0)
                        {
                            //  ListViewItem lv1 = listView1.Items.Add(from_area.Groups[1].Value + "→" + to_area.Groups[1].Value);
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            lv1.SubItems.Add(goods_names[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(zaizhongs[j].Groups[1].Value.Trim() + "公斤");
                            lv1.SubItems.Add(trans_modes[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(huo_phones[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(huo_contacts[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(company_names[j].Groups[1].Value.Trim());

                            lv1.SubItems.Add(times[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(from_area.Groups[1].Value + "→" + to_area.Groups[1].Value);


                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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

        #region  运立方
        public void yun()
        {
            string postdata = "";

            try
            {
                switch (comboBox5.Text)
                {
                    case "北京市":
                        postdata= @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1010000000}";
                        break;
                    case "天津市":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1020000000}";
                        break;
                    case "河北省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1030000000}";
                        break;
                    case "山西省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1040000000}";
                        break;
                    case "内蒙古区":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1050000000}";
                        break;
                    case "辽宁省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1060000000}";
                        break;
                    case "吉林省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1070000000}";
                        break;
                    case "黑龙江省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1080000000}";
                        break;
                    case "上海市":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1090000000}";
                        break;
                    case "江苏省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1100000000}";
                        break;
                    case "浙江省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1110000000}";
                        break;
                    case "安徽省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1120000000}";
                        break;
                    case "福建省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1130000000}";
                        break;
                    case "江西省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1140000000}";
                        break;
                    case "山东省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1150000000}";
                        break;
                    case "河南省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1160000000}";
                        break;
                    case "湖北省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1170000000}";
                        break;
                    case "湖南省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1180000000}";
                        break;
                    case "广东省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1190000000}";
                        break;
                    case "广西区":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1200000000}";
                        break;

                    case "海南省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1210000000}";
                        break;
                    case "重庆市":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1220000000}";
                        break;
                    case "四川省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1230000000}";
                        break;
                    case "贵州省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1240000000}";
                        break;
                    case "云南省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1250000000}";
                        break;
                    case "西藏区":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1260000000}";
                        break;
                    case "陕西省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1270000000}";
                        break;
                    case "甘肃省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1280000000}";
                        break;
                    case "青海省":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1290000000}";
                        break;
                    case "宁夏区":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1300000000}";
                        break;
                    case "新疆区":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1310000000}";
                        break;
                    case "台湾":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1320000000}";
                        break;
                    case "香港":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1330000000}";
                        break;
                    case "澳门":
                        postdata = @"{""wxUid"" : 74,""expectCarLength"" : """",""expectCarType"" : """",""gfrom"" : """",""pageSize"" : 5,""pageNum"" : 3,""beginAddressCode"" : 1340000000}";
                        break;

                }



                 
                    string url = "https://wechat.yunlifang.com/wechat/findCargo/queryCargo";

                        
                    string html = method.PostUrl(url, postdata, "", "utf-8");


                    MatchCollection from_areas = Regex.Matches(html, @"""startAddress"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection to_areas = Regex.Matches(html, @"simpleEndAddress"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection CarTypes = Regex.Matches(html, @"expectCarType"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection remarks = Regex.Matches(html, @"remark"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection phones = Regex.Matches(html, @"""phone"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection times = Regex.Matches(html, @"sendTime"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                   
                    for (int j = 0; j < from_areas.Count; j++)
                    {


                        if (from_areas.Count > 0)
                        {

                            ListViewItem lv3 = listView3.Items.Add((listView3.Items.Count + 1).ToString());
                            lv3.SubItems.Add(from_areas[j].Groups[1].Value + "→" + to_areas[j].Groups[1].Value);
                            lv3.SubItems.Add(CarTypes[j].Groups[1].Value.Trim());
                            lv3.SubItems.Add(remarks[j].Groups[1].Value.Trim() + "公斤");

                            lv3.SubItems.Add(phones[j].Groups[1].Value.Trim());

                            lv3.SubItems.Add(ConvertStringToDateTime(times[j].Groups[1].Value.Trim()).ToString());

                            

                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }


                        }

                    }

                    Thread.Sleep(500);
                }
            




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void 物通网_Load(object sender, EventArgs e)
        {
           
        }

        private void visualButton1_Click(object sender, EventArgs e)
        {
            if (denglu == false)
            {
                MessageBox.Show("请先登录您的账号！");
                return;
            }


            listView1.Items.Clear();
            this.status = true;

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();

        }

        private void visualButton2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void visualButton3_Click(object sender, EventArgs e)
        {
            this.status = true;
            
        }

        private void visualButton4_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

                try
                {



                    string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
                    MySqlConnection mycon = new MySqlConnection(constr);
                    mycon.Open();

                    MySqlCommand cmd = new MySqlCommand("select * from zhanghaos where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                    MySqlDataReader reader = cmd.ExecuteReader();  



                    if (reader.Read())
                    {

                        string username = reader["username"].ToString().Trim();
                        string password = reader["password"].ToString().Trim();
                    string status = reader["status"].ToString().Trim();


                    if (status == "1")
                    {
                        MessageBox.Show("您的账号在别处登录！");
                        return;
                    }
                    //判断密码
                    if (textBox2.Text.Trim() == password)
                    {

                        MessageBox.Show("登陆成功！");
                        textBox2.Visible = false;
                        textBox1.Visible = false;
                        denglu = true;
                        reader.Close();
                        MySqlCommand cmd1 = new MySqlCommand("UPDATE zhanghaos SET status='1' where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                         cmd1.ExecuteReader();
                        
                    }


                    else

                    {
                            MessageBox.Show("您的密码错误！");
                            return;
                        }

                    }

                    else
                    {
                        MessageBox.Show("未查询到您的账户信息！");
                        return;
                    }


                }

                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }

        private void visualButton4_Click_1(object sender, EventArgs e)
        {
            status = false;
        }

        private void visualButton3_Click_1(object sender, EventArgs e)
        {
            status  = true;
        }

        private void 物通网_FormClosed(object sender, FormClosedEventArgs e)
        {
            string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd1 = new MySqlCommand("UPDATE zhanghaos SET status='0' where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
            cmd1.ExecuteReader();
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView1.Columns[e.Column].Tag == null)
            {
                listView1.Columns[e.Column].Tag = true;
            }
            bool tabK = (bool)listView1.Columns[e.Column].Tag;
            if (tabK)
            {
                listView1.Columns[e.Column].Tag = false;
            }
            else
            {
                listView1.Columns[e.Column].Tag = true;
            }
            listView1.ListViewItemSorter = new ListViewSort(e.Column, listView1.Columns[e.Column].Tag);
            //指定排序器并传送列索引与升序降序关键字
            listView1.Sort();//对列表进行自定义排序
        }

        private void visualButton8_Click(object sender, EventArgs e)
        {
            if (denglu == false)
            {
                MessageBox.Show("请先登录您的账号！");
                return;
            }
            listView2.Items.Clear();
            this.status = true;

            Thread thread = new Thread(new ThreadStart(run1));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void visualButton12_Click(object sender, EventArgs e)
        {
            if (denglu == false)
            {
                MessageBox.Show("请先登录您的账号！");
                return;
            }
            listView3.Items.Clear();
            this.status = true;

            Thread thread = new Thread(new ThreadStart(yun));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void visualButton11_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void visualButton7_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView3), "Sheet1", true);
        }

        private void visualButton6_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

        private void visualButton5_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void visualButton9_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void visualButton10_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

      
    }

}

