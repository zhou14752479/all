using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 思忆大数据
{
    class method
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }


        #region GET请求
        public static string GetUrl(string Url)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";

                request.Headers.Add("Cookie", "");

                request.Referer = "";
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
                return ex.ToString();

            }
            
        }
        #endregion


        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }



        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public string register(string username,string password)
        {
            try
            {
                string url = "http://acaiji.com/api/do.php?method=add&username="+ username + "&password="+password+"&time="+ GetTimeStamp();
                string html = GetUrl(url);
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public string login(string username, string password)
        {
            try
            {
                string url = "http://acaiji.com/api/do.php?method=login&username=" + username + "&password=" + password;
                string html = GetUrl(url);
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }

       
        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getlat(string city)
        {
            ArrayList areas = new ArrayList();
            string url = "http://www.jsons.cn/lngcode/?keyword=" + System.Web.HttpUtility.UrlEncode(city) + "&txtflag=0";
            string html = GetUrl(url);

            Match ahtml = Regex.Match(html, @"<table class=""table table-bordered table-hover"">([\s\S]*?)</table>");

            MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Groups[1].Value.Contains("1"))
                {
                    areas.Add(values[i].Groups[1].Value.Replace("，", "%2C").Trim());
                }
            }
            return areas;
        }


        /// <summary>
        /// 获取关键词
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getkeywords(ListView lv)
        {
            ArrayList keywords = new ArrayList();
            for (int i = 0; i < lv.Items.Count; i++)
            {
                keywords.Add(lv.Items[i].SubItems[0].Text);
            }
            return keywords;
        }

        bool zanting;
        ArrayList citys = new ArrayList();
        /// <summary>
        /// 地图主程序
        /// </summary>
        public void ditu(ListView LV,ListView LV1,string telpanduan)
        {
            ArrayList keywords = getkeywords(LV1);
            if (keywords.Count == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }


            try
            {

                if (citys.Count == 0)
                {
                    MessageBox.Show("请添加城市");
                    return;
                }

                foreach (string keyword in keywords)
                {

                    foreach (string city in citys)
                    {
                        ArrayList areaLats = getlat(city);

                        foreach (string lat in areaLats)
                        {


                            for (int page = 1; page < 100; page++)
                            {


                                string url = "https://restapi.amap.com/v3/place/around?appname=1e3bb24ab8f75ba78a7cf8a9cc4734c6&key=1e3bb24ab8f75ba78a7cf8a9cc4734c6&keywords=" + System.Web.HttpUtility.UrlEncode(keyword) + "&location=" + lat + "&logversion=2.0&page=" + page + "&platform=WXJS&s=rsx&sdkversion=1.2.0";
                                string html = GetUrl(url);



                                MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                                MatchCollection tels = Regex.Matches(html, @"""tel"":([\s\S]*?),");
                                MatchCollection address = Regex.Matches(html, @"""address"":([\s\S]*?),");
                                MatchCollection pros = Regex.Matches(html, @"""pname"":([\s\S]*?),");
                                MatchCollection citynames = Regex.Matches(html, @"""cityname"":([\s\S]*?),");
                                MatchCollection areas = Regex.Matches(html, @"""adname"":([\s\S]*?),");
                                MatchCollection types = Regex.Matches(html, @"""type"":([\s\S]*?),");

                                if (names.Count == 0)
                                    break;

                                for (int i = 0; i < names.Count; i++)
                                {
                                    if (telpanduan == "全部采集")
                                    {

                                        ListViewItem lv1 = LV.Items.Add((LV.Items.Count + 1).ToString()); //使用Listview展示数据

                                        lv1.SubItems.Add(names[i].Groups[1].Value);
                                        lv1.SubItems.Add(tels[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(pros[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(citynames[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(areas[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(types[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(keyword);
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                    }
                                    else if (telpanduan== "只采集有联系方式")
                                    {
                                        if (tels[i].Groups[1].Value.Replace("\"", "") != "[]")
                                        {
                                            ListViewItem lv1 = LV.Items.Add((LV.Items.Count + 1).ToString()); //使用Listview展示数据

                                            lv1.SubItems.Add(names[i].Groups[1].Value);
                                            lv1.SubItems.Add(tels[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(pros[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(citynames[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(areas[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(types[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(keyword);
                                            while (this.zanting == false)
                                            {
                                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                            }
                                        }
                                    }

                                }
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }





    }
}
