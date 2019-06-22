using System;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Net;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace _58
{
    public partial class Map : Form
    {
        public Map()
        {
            InitializeComponent();
        }

        private void Map_Load(object sender, EventArgs e)
        {
            
        }



        public class JsonParser
        {
            public List<Content> Content;
  
        }

        public class Content
        {       
            public string name;
            public string tel;
            public string addr;
        }

        #region 获取百度citycode
        public int getcityId(string cityName)
        {
            try

            {

                String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=s&da_src=searchBox.button&wd=" + cityName + "&c=289&src=0&wd2=&pn=0&sug=0&l=12&b=(13461858.87,3636969.979999999;13584738.87,3670185.979999999)&from=webmap&biz_forward={%22scaler%22:1,%22styles%22:%22pl%22}&sug_forward=&tn=B_NORMAL_MAP&nn=0&u_loc=13166533,3998088&ie=utf-8";

                string html = Method.GetUrl(Url);


                MatchCollection Matchs = Regex.Matches(html, @"""code"":([\s\S]*?),", RegexOptions.IgnoreCase);



                    
                int cityId = Convert.ToInt32(Matchs[0].Groups[1].Value);
                return cityId;

            }
            catch (System.Exception ex)
            {
                ex.ToString();
                return 1;
            }




        }
        #endregion

        #region GET请求
        public static string GetUrl(string Url)
        {
            try
            {
                string COOKIE = "_lxsdk_cuid=1609b86916cc8-08cd9a34568e29-36624209-1fa400-1609b86916dc8; _ga=GA1.2.1236833492.1515143511; _lx_utm=utm_source%3DBaidu%26utm_medium%3Dorganic; iuuid=528F52FB7FFA9E4668A9276A34739DE509B4A411CC08F2612EC2A51E0A8C1963; webp=1; latlng=33.96193,118.27549,1515660280105; cityname=%E5%8C%97%E4%BA%AC; i_extend=C_b2GimthomepagesearchH__a100001__b1; __utma=74597006.1236833492.1515143511.1515660278.1515660278.1; __utmz=74597006.1515660278.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); Hm_lvt_cc903faaed69cca18f7cf0997b2e62c9=1515659805; uuid=b88bdc17d9c849dfbdf2.1514437775.1.0.0; oc=iy-AAAVEh0gZfS-yGtUwEbVlM5pMtV-7Euvq2JQK04C5q_4NVbXpcz6xIrPM5JkvKyiGyKTXq7V95G6J9XLtCpBqcuNEGco58QajBfygzio_7FN9neSawQJ3GyM9kmuTRSS4T4Xjjvm08E8yETxxT0pXBz0rnJV93MVhmhAjHW0; ci=1; rvct=1%2C334%2C20%2C55%2C59%2C45%2C50%2C30%2C56; lat=39.924772; lng=116.600145; __mta=109370042.1514437776048.1515723458022.1515724799303.29; _lxsdk_s=160e7fb3fa7-b85-6d2-38%7C%7C60";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
                WebClient webclient = new WebClient();
                webclient.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion
       

        


        #region  百度地图采集

        public void baidu()

        {

            try

            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                
                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                int pages = 10;


                foreach (string city in citys)

                {
                    int cityid = getcityId(city + "市");  //获取 citycode;

                    foreach (string keyword in keywords)

                    {

                        for (int i = 0; i <= pages; i++)

                        {

                            int j = i - 1>0 ? i-1 :0;
                            
                            String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=con&from=webmap&c="+cityid+"&wd="+keyword+"&wd2=&pn="+i+"&nn="+j+"0&db=0&sug=0&addr=0&&da_src=pcmappg.poi.page&on_gel=1&src=7&gr=3&l=13.2&auth=6GNgMOxNx%40CM2KzLPeYAvKP725L6c0z5uxHLVTTxHNNtBnlQADZZzy1uVt1GgvPUDZYOYIZuVt1cv3uztHee%40ewWvPWv3GuxtVwi04960vyACFIMOSU7ucEWe1GD8zv7u%40ZPuHt0A%3DH73uzCCyoET1jlBhlADM5ZYYDMJ7zlp55CKBvaaZyY&device_ratio=1&tn=B_NORMAL_MAP&u_loc=13167726,4000141&ie=utf-8";


                           
                            string html = Method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"""primary_uid"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                            textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页\r\n";



                            JsonParser jsonParser = JsonConvert.DeserializeObject<JsonParser>(html);



                            foreach (Content content in jsonParser.Content)
                            {
                                int index = this.dataGridView1.Rows.Add();
                                this.dataGridView1.Rows[index].Cells[0].Value = index;
                                this.dataGridView1.Rows[index].Cells[1].Value = content.name;
                                this.dataGridView1.Rows[index].Cells[2].Value = content.tel;
                                this.dataGridView1.Rows[index].Cells[3].Value = content.addr;
                                this.dataGridView1.Rows[index].Cells[4].Value = keyword.Trim();
                                this.dataGridView1.Rows[index].Cells[5].Value = city;

                                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                                if (visualButton2.Text == "已停止")
                                {
                                    return;
                                }
                            }

                            Application.DoEvents();
                            Thread.Sleep(10);   //内容获取间隔，可变量

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




        #region  腾讯地图采集

        public void tengxun()

        {

            try

            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);






                int pages = 250;

                foreach (string city in citys)

                {
                    
                    string cityutf8 = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("utf-8"));
                    foreach (string keyword in keywords)

                    {
                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));
                        for (int i = 0; i <= pages; i++)


                        {
                            
                            String Url = "http://map.qq.com/m/place/result/city=" + cityutf8 + "&word=" + keywordutf8 + "&bound=&page=" + i + "&cpos=&mode=list";

                            textBox3.Text = Url;
                            string html = Method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"poid=([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                            textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页\r\n";

                            Application.DoEvents();
                            Thread.Sleep(100);



                            
                            foreach (string poid in lists)

                            {
                                int index = this.dataGridView1.Rows.Add();

                                string Url1 = "http://map.qq.com/m/detail/poi/poid=" + poid;
                                string strhtml = Method.GetUrl(Url1);


                                string title = @"<div class=""poiDetailTitle "">([\s\S]*?)</div>";
                                string Rxg = @"<a href=""tel:([\s\S]*?)""";
                                string Rxg1 = @"span class=""poiDetailAddrTxt"">([\s\S]*?)</span>";



                                Match titles = Regex.Match(strhtml, title);
                                Match tell = Regex.Match(strhtml, Rxg);
                                Match address = Regex.Match(strhtml, Rxg1);

                                if (visualButton2.Text == "已停止")
                                {
                                    return;
                                }
                                if(tell.Groups[1].Value.Trim() != "")

                                {

                                    this.dataGridView1.Rows[index].Cells[0].Value = index;
                                    this.dataGridView1.Rows[index].Cells[1].Value = titles.Groups[1].Value;
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];

                                    this.dataGridView1.Rows[index].Cells[2].Value += tell.Groups[1].Value.Trim() + ",";

                                    this.dataGridView1.Rows[index].Cells[3].Value = address.Groups[1].Value;

                                    this.dataGridView1.Rows[index].Cells[4].Value = keyword;
                                    this.dataGridView1.Rows[index].Cells[5].Value = city;



                                }




                                Application.DoEvents();
                                Thread.Sleep(500);   //内容获取间隔，可变量
                            }

                        }


                    }
                }
            }

            catch (System.Exception ex)
            {

                textBox3.Text  = ex.ToString();
                
            }

        }
        #endregion


        #region  腾讯地图json采集

        public void txMap()

        {

            try

            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);


                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                int pages = 200;


                foreach (string city in citys)

                {
                    int cityid = getcityId(city + "市");  //获取 citycode;

                    foreach (string keyword in keywords)

                    {

                        for (int i = 0; i <= pages; i++)

                        {


                            String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=con&from=webmap&c=" + cityid + "&wd=" + keyword + "&wd2=&pn=" + i + "&nn=" + i + "0&db=0&sug=0&addr=0&&da_src=pcmappg.poi.page&on_gel=1&src=7&gr=3&l=13&tn=B_NORMAL_MAP&u_loc=13167420,3999298&ie=utf-8";

                            string html = Method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"""primary_uid"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                            textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页\r\n";



                            JsonParser jsonParser = JsonConvert.DeserializeObject<JsonParser>(html);



                            foreach (Content content in jsonParser.Content)
                            {
                                int index = this.dataGridView1.Rows.Add();
                                this.dataGridView1.Rows[index].Cells[0].Value = index;
                                this.dataGridView1.Rows[index].Cells[1].Value = content.name;
                                this.dataGridView1.Rows[index].Cells[2].Value = content.tel;
                                this.dataGridView1.Rows[index].Cells[3].Value = content.addr;
                                this.dataGridView1.Rows[index].Cells[4].Value = keyword.Trim();
                                this.dataGridView1.Rows[index].Cells[5].Value = city;

                                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                                if (visualButton2.Text == "已停止")
                                {
                                    return;
                                }
                            }






                            Application.DoEvents();
                            Thread.Sleep(100);   //内容获取间隔，可变量

                        }

                    }
                }
            }

            catch (System.Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }
        #endregion


        #region  高德地图采集

        public void gaode()

        {

            try

            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);




                int pages = 200;

                foreach (string city in citys)

                {


                    int citycode = gaodeCityId(city);
                    foreach (string keyword in keywords)

                    {
                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));

                        for (int i = 0; i <= pages; i++)
                        {

                            String Url = "http://m.amap.com/service/poi/keywords.json?pagenum=" + i + "&user_loc=undefined&geoobj=&city=" + citycode + "&keywords=" + keywordutf8;

                            string html = Method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"""diner_flag"":([\s\S]*?),""id"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[2].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                            textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页\r\n";


                            Application.DoEvents();
                            Thread.Sleep(100);




                            foreach (string poid in lists)

                            {
                                int index = this.dataGridView1.Rows.Add();

                                string Url1 = "https://www.amap.com/detail/" + poid + "?citycode=" + citycode;
                                string strhtml = Method.GetUrl(Url1);


                                string title = @"<h4 class=""detail_title"">([\s\S]*?)</h4>";
                                string Rxg = @"""telephone"":""([\s\S]*?)""";
                                string Rxg1 = @"""address"":""([\s\S]*?)""";



                                MatchCollection titles = Regex.Matches(strhtml, title);
                                MatchCollection tell = Regex.Matches(strhtml, Rxg);
                                MatchCollection address = Regex.Matches(strhtml, Rxg1);

                                if (visualButton2.Text == "已停止")
                                {
                                    return;
                                }


                                foreach (Match match in titles)
                                {

                                    this.dataGridView1.Rows[index].Cells[0].Value = index;
                                    this.dataGridView1.Rows[index].Cells[1].Value = match.Groups[1].Value;
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];

                                }
                                foreach (Match match in tell)
                                {

                                    this.dataGridView1.Rows[index].Cells[2].Value += match.Groups[1].Value.Trim() + ",";


                                }
                                foreach (Match match in address)
                                {


                                    this.dataGridView1.Rows[index].Cells[3].Value = match.Groups[1].Value;
                                    

                                }
                             

                                this.dataGridView1.Rows[index].Cells[4].Value = keyword;
                                this.dataGridView1.Rows[index].Cells[5].Value = city;
                                


                                Application.DoEvents();
                                Thread.Sleep(500);   //内容获取间隔，可变量
                            }

                        }


                    }
                }
            }

            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
            }

        }
        #endregion


        #region  360地图采集

        public void map_360()

        {

            try

            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);






                int pages = 100;

                foreach (string city in citys)

                {
                    string cityutf8 = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("utf-8"));
                    foreach (string keyword in keywords)

                    {
                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));
                        for (int i = 1; i <= pages; i++)

                            

                        {
                            

                            String Url = "https://ditu.so.com/app/pit?jsoncallback=jQuery18308131636402501483_1525852464213&keyword="+keywordutf8+"&cityname="+cityutf8+"&batch="+i+"%2c"+(i+1)+ "%2c"+(i+2)+"%2c"+(i+3)+"%2c"+(i+4)+"&number=10";

                            

                            string html = Method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"""pguid"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                            textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页\r\n";

                            Application.DoEvents();
                            Thread.Sleep(100);




                            foreach (string poid in lists)

                            {
                                int index = this.dataGridView1.Rows.Add();

                                string Url1 = "https://m.map.so.com/onebox/?type=detail&id="+poid+"&mso_x=&mso_y=&d=mobile&src=map_wap&fields=movies_all";
                                string strhtml = Method.GetUrl(Url1);


                                string title = @"data_poi_name = ""([\s\S]*?)""";
                                string Rxg = @"href=""tel:([\s\S]*?)""";
                                string Rxg1 = @"data_poi_address = ""([\s\S]*?)""";



                                MatchCollection titles = Regex.Matches(strhtml, title);
                                MatchCollection tell = Regex.Matches(strhtml, Rxg);
                                MatchCollection address = Regex.Matches(strhtml, Rxg1);

                                if (visualButton2.Text== "已停止")
                                {
                                    return;
                                }


                                foreach (Match match in titles)
                                {

                                    this.dataGridView1.Rows[index].Cells[0].Value = index;
                                    this.dataGridView1.Rows[index].Cells[1].Value = match.Groups[1].Value;
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];

                                }
                                foreach (Match match in tell)
                                {

                                    this.dataGridView1.Rows[index].Cells[2].Value += match.Groups[1].Value.Trim() + ",";


                                }
                                foreach (Match match in address)
                                {


                                    this.dataGridView1.Rows[index].Cells[3].Value = match.Groups[1].Value;
                                 

                                }
                              

                                this.dataGridView1.Rows[index].Cells[4].Value = keyword;
                                this.dataGridView1.Rows[index].Cells[5].Value = city;

                             

                                Application.DoEvents();
                                Thread.Sleep(500);   //内容获取间隔，可变量
                            }

                        }


                    }
                }
            }

            catch (System.Exception ex)
            {

                textBox3.Text = ex.ToString();
            }

        }
        #endregion


        #region  搜狗地图Post采集

        public void sougou()

        {
            string cookie = "CXID=D44C63D34623066DA36D11B4B82C488C; SUV=1518337255419884; SMAPUVID=1518337255419884; SUV=1801190926013760; IPLOC=CN3213; sct=1; SNUID=D67B2A4373761425DCE0226F733E2FA7; ad=zMGYjlllll2zYIpclllllVr1fv6lllllGq6poyllll9llllljZlll5@@@@@@@@@@; SUID=A10659313565860A5A6291B800040AA1; wP_w=544ebe2c0329~HXcgwyvcH_c_5BDcHXULrZvNyX9XwbmSJXPsNNDN3XNBbbDJbdNhb; activecity=%u5BBF%u8FC1%2C13168867%2C3999623%2C12; ho_co=";
            string url = "http://map.sogou.com/EngineV6/search/json";

            string charset = "gb2312";
            try

            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);




                int pages = 200;

                foreach (string city in citys)

                {
                    //搜索的城市和关键词都需要两次url编码

                    string city1= System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("utf-8"));
                    string cityutf8 = System.Web.HttpUtility.UrlEncode(city1, System.Text.Encoding.GetEncoding("utf-8"));
                    foreach (string keyword in keywords)

                    {
                        string keyword1= System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));
                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword1, System.Text.Encoding.GetEncoding("utf-8"));

                        for (int i = 1; i <= pages; i++)
                        {

                            string postData = "what=keyword%3A"+ keywordutf8+ "&range=bound%3A00000000.5%2C0000000.5%2C99999999.5%2C9999999.5%3A0&othercityflag=1&appid=1361&thiscity=" + cityutf8+"&lastcity="+cityutf8+"&userdata=3&encrypt=1&pageinfo=" + i+ "%2C10&locationsort=0&version=7.0&ad=0&level=12&exact=1&type=&attr=&order=&submittime=0&resultTypes=poi&sort=0&reqid=1526008949358471&cb=parent.IFMS.search";

                            string html = Method.PostUrl(url, postData, cookie,charset);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"""dataid"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                            textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页\r\n";


                            Application.DoEvents();
                            Thread.Sleep(100);




                            foreach (string poid in lists)

                            {
                                int index = this.dataGridView1.Rows.Add();
                                

                                string Url1 = "http://map.sogou.com/poi/1_"+poid+".htm";
                                string strhtml = GetUrl(Url1);


                                string title = @"""caption"":""([\s\S]*?)""";
                                string Rxg = @"""phone"":""([\s\S]*?)""";
                                string Rxg1 = @"""address"":""([\s\S]*?)""";



                                MatchCollection titles = Regex.Matches(strhtml, title);
                                MatchCollection tell = Regex.Matches(strhtml, Rxg);
                                MatchCollection address = Regex.Matches(strhtml, Rxg1);

                                if (visualButton2.Text == "已停止")
                                {
                                    return;
                                }


                                foreach (Match match in titles)
                                {

                                    this.dataGridView1.Rows[index].Cells[0].Value = index;
                                    this.dataGridView1.Rows[index].Cells[1].Value = match.Groups[1].Value;
                                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];

                                }
                                foreach (Match match in tell)
                                {

                                    this.dataGridView1.Rows[index].Cells[2].Value += match.Groups[1].Value.Trim() + ",";


                                }
                                foreach (Match match in address)
                                {


                                    this.dataGridView1.Rows[index].Cells[3].Value = match.Groups[1].Value;
                                   

                                }
                               

                                this.dataGridView1.Rows[index].Cells[4].Value = keyword;
                                this.dataGridView1.Rows[index].Cells[5].Value = city;


                                Application.DoEvents();
                                Thread.Sleep(500);   //内容获取间隔，可变量
                            }

                        }


                    }
                }
            }

            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
            }

        }
        #endregion


       

        #region  获取高德地图CITYID

        public int gaodeCityId(string city)
        {

            try
            {



                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select gaode_citycode from gaode_city where gaode_cityname='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                int cityId =  Convert.ToInt32( reader["gaode_citycode"]);
                mycon.Close();
                reader.Close();
                return cityId;


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 1;
            }


        }

        #endregion

        private void 注册账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            register rg = new register();
            rg.Show();
        }

        private void 登陆账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }

        private void Map_MouseEnter(object sender, EventArgs e)
        {
           
        }

        private void 查看教程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void 合作模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Map_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("您确定要关闭吗？");
        }

        
       

       

 
       
        

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();   //初始化menu
                menu.MenuItems.Add("清空数据");
                menu.MenuItems.Add("去除固话");  //添加菜单项c1
                                             //menu.MenuItems.Add("添加城市");   //添加菜单项c2

                menu.Show(dataGridView1, new Point(e.X, e.Y));   //在点(e.X, e.Y)处显示menu

                menu.MenuItems[0].Click += new EventHandler(clear);
                menu.MenuItems[1].Click += new EventHandler(RemoveTell);

                textBox3.Text = "";

            }
        }
        public void clear(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();

        }
        #region  去除固话

        public void RemoveTell(object sender, EventArgs e)

        {

            try
            {
                for (int i = 0; i <= dataGridView1.Rows.Count; i++)
                {
                    string Lpv = dataGridView1.Rows[i].Cells[2].Value.ToString();

                    if (Lpv.Contains("-") || Lpv=="")                //Contains()包含, indexof()返回字符在字符串中首次出现的位置，若没有返回 -1
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }
            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion

        private void label12_Click(object sender, EventArgs e)
        {
            //ArrayList list = getPoi("宿迁市政府");
            //MessageBox.Show(list[0].ToString());

            name nm = new name();
            nm.MyProperty = 6;
             MessageBox.Show( nm.MyProperty.ToString());
           //测试get set使用

        }

     

        private void visualButton1_Click(object sender, EventArgs e)
        {
            visualButton2.Text = "停止采集";

           

            if (textBox1.Text == "" || textBox2.Text == "")

            {
                MessageBox.Show("请输入城市和关键字！");
                return;
            }
            #region 通用登录

            bool value = false;
            string html = Method.GetUrl("http://acaiji.com/success/ip.php");
            string localip = Method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                visualProgressIndicator1.Show();

                if (radioButton2.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(tengxun));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }
                else if (radioButton3.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(gaode));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }
                else if (radioButton4.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(map_360));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }
                else if (radioButton5.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(sougou));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }

                else if (radioButton6.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(baidu));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion


            
        }

        private void visualButton3_Click(object sender, EventArgs e)
        {
            Method.DataTableToExcel(Method.DgvToTable(dataGridView1), "Sheet1", true);
        }

        private void visualButton4_Click(object sender, EventArgs e)
        {
            Method.Txt(dataGridView1);
           
        }

        private void visualButton2_Click(object sender, EventArgs e)
        {
            visualButton2.Text = "已停止";
            visualProgressIndicator1.Hide();
        }

        private void dataGridView1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();   //初始化menu

                menu.MenuItems.Add("去除固话");
                menu.MenuItems.Add("清空数据");   //添加菜单项c1
                                              //menu.MenuItems.Add("添加城市");   //添加菜单项c2

                menu.Show(dataGridView1, new Point(e.X, e.Y));   //在点(e.X, e.Y)处显示menu


                menu.MenuItems[0].Click += new EventHandler(RemoveTell);
                menu.MenuItems[1].Click += new EventHandler(clear);

                textBox3.Text = "";

            }
        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Text += e.Node.Text+"\r\n";
        }
    }

    class name : Form

    {
        //测试get set使用 label12调用这个字段
        private int myVar;

        public int MyProperty
        {
            get {
                    if (myVar != 2)

                    {
                        MessageBox.Show("他是2");
                    }

                    return myVar;

                }
            set {

                if (myVar ==6)

                {
                    MessageBox.Show("请不要改为6");
                }
                myVar = value;

                }





        }


    }
}
