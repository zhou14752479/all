using Newtonsoft.Json;
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
using static main.Form1;

namespace main
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
           
        }
        #region 获取百度citycode
        public int getcityId(string cityName)
        {
            try

            {

                String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=s&da_src=searchBox.button&wd=" + cityName + "&c=289&src=0&wd2=&pn=0&sug=0&l=12&b=(13461858.87,3636969.979999999;13584738.87,3670185.979999999)&from=webmap&biz_forward={%22scaler%22:1,%22styles%22:%22pl%22}&sug_forward=&tn=B_NORMAL_MAP&nn=0&u_loc=13166533,3998088&ie=utf-8";

                string html = method.GetUrl(Url,"utf-8");


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
        #region  Baidu地图json采集

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

                            string html = method.GetUrl(Url,"utf-8");


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
                               

                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(content.name);
                                lv1.SubItems.Add(content.tel);
                                lv1.SubItems.Add(content.addr);
                                lv1.SubItems.Add(city);
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
                            string html = method.GetUrl(Url,"utf-8");


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



                            foreach (string poid in lists)

                            {
                               

                                string Url1 = "http://map.qq.com/m/detail/poi/poid=" + poid;
                                string strhtml = method.GetUrl(Url1,"utf-8");


                                string title = @"<div class=""poiDetailTitle "">([\s\S]*?)</div>";
                                string Rxg = @"<a href=""tel:([\s\S]*?)""";
                                string Rxg1 = @"span class=""poiDetailAddrTxt"">([\s\S]*?)</span>";



                                Match titles = Regex.Match(strhtml, title);
                                Match tell = Regex.Match(strhtml, Rxg);
                                Match address = Regex.Match(strhtml, Rxg1);

                             
                                if (tell.Groups[1].Value.Trim() != "")

                                {

                                   

                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(titles.Groups[1].Value);
                                    lv1.SubItems.Add(tell.Groups[1].Value);
                                    lv1.SubItems.Add(address.Groups[1].Value);
                                    lv1.SubItems.Add(city);



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

                textBox3.Text = ex.ToString();

            }

        }
        #endregion

    }
}
