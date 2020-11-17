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

namespace 主程序202006
{
    public partial class 手机网页58 : Form
    {
        public 手机网页58()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        string COOKIE = " f=n; id58=e87rZV671PQoW2PlFuw4Ag==; 58tj_uuid=7bf790cd-54c0-4cc0-a0cd-29c33b72de07; wmda_uuid=382435b658e6e61704aa58ce03e6b522; wmda_new_uuid=1; als=0; xxzl_deviceid=gKqSAIzWFN86bQTmdCGyq7R42ZIhVql5vPAHe%2FaanH9KqG21yFDbajXYw4HZUBSd; param8616=0; param8716kop=1; xxzl_smartid=8e3bbea68f9ae5e52f0d296a899a4e38; Hm_lvt_295da9254bbc2518107d846e1641908e=1590367824; gr_user_id=696572b5-e213-46b1-b860-f1e8c48d23e7; Hm_lvt_3bb04d7a4ca3846dcc66a99c3e861511=1590582047; Hm_lvt_e15962162366a86a6229038443847be7=1590582047; city=bj; 58home=bj; new_uv=27; utm_source=; init_refer=; spm=; wmda_session_id_11187958619315=1592630841757-62fc6efe-0ef4-0d05; new_session=0; wmda_visited_projects=%3B11187958619315%3B1731916484865%3B10104579731767%3B6333604277682%3B1409632296065%3B9561808484917%3B2385390625025; __utma=253535702.89089782.1592630914.1592630914.1592630914.1; __utmc=253535702; __utmz=253535702.1592630914.1.1.utmcsr=bj.58.com|utmccn=(referral)|utmcmd=referral|utmcct=/house.shtml; qz_gdt=; launchFlag=1; hasLaunchPage=%7Cl_house_%E5%95%86%E9%93%BA%7C; xxzl_cid=5cfcbff03c36460a8c7c8ba7f5531a4a; xzuid=8aa351ff-c4e7-48cc-acea-c731cf0a8d5f; _house_list_show_time_=3; xzfzqtoken=%2BrARxQwJl0MPnnPf08o3cWCvOUxugHIZNqruZhaIu91hRzmzP5o%2FtVyFu50C92M0in35brBb%2F%2FeSODvMgkQULA%3D%3D; f=n; ppStore_fingerprint=E7FE01B25259770117C0501A2CEF9D363D2C99B185AAD0C9%EF%BC%BF1592633099819; JSESSIONID=C5DD56ED0F99D2F3F64C4D2657B39115; _house_detail_show_time_=2";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
              
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.12(0x17000c2c) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("");

                //添加头部
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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

        public string gettel(string uid)
        {
            string url = "https://wxapp.58.com/phone/get?appCode=21&infoId=" + uid + "&cateCode=5&legoKey=&dataType=&slotid=&spm=&thirdKey="+textBox1.Text.Trim();
            string html = GetUrl(url);
            Match tel = Regex.Match(html, @"result\\"":\\""([\s\S]*?)\\""");
            return tel.Groups[1].Value;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        ArrayList finishes = new ArrayList();
        public void getlink()
        {
            StreamReader sr = new StreamReader(path + "已完成.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {

                finishes.Add(text[i]);


            }
            sr.Close();
        }

        
        public void run()
        {
            getlink();

            string url = "https://m.58.com/bj/shengyizr/0/";

                string html = GetUrl(url);
                MatchCollection links = Regex.Matches(html, @"<a class='link' href=""([\s\S]*?)""");
            



                for (int a = 0; a < links.Count; a++)

                {
                FileStream fs1 = new FileStream(path + "已完成.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(links[a].Groups[1].Value);
                sw.Close();
                fs1.Close();


                if (!finishes.Contains(links[a].Groups[1].Value) && links[a].Groups[1].Value.Contains("shangpu"))
                {


                    try
                    {

                        string strhtml = GetUrl(links[a].Groups[1].Value);

                        Match uid = Regex.Match(strhtml, @"infoid = ""([\s\S]*?)""");

                        Match title = Regex.Match(strhtml, @"houseTitle = ""([\s\S]*?)""");
                        Match addr = Regex.Match(strhtml, @"""dizhi"":""([\s\S]*?)""");
                        Match linkman = Regex.Match(strhtml, @"""linkman"":""([\s\S]*?)""");
                        MatchCollection prices = Regex.Matches(strhtml, @"<span><span>([\s\S]*?)</span>");//租金 转让费 面积
                        Match kuan = Regex.Match(strhtml, @"面宽([\s\S]*?)米");//面宽
                        Match content = Regex.Match(strhtml, @"<article class=""house-des"">([\s\S]*?)</article>");
                        Match louceng = Regex.Match(strhtml, @"楼层</span>([\s\S]*?)</li>");//楼层
                        Match type = Regex.Match(strhtml, @"类型</span>([\s\S]*?)</li>");//类型
                        Match area = Regex.Match(strhtml, @"<div class=""main-address""><p>([\s\S]*?)</p>");//类型
                        MatchCollection images = Regex.Matches(strhtml, @"<div data-background=""([\s\S]*?)""");



                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(title.Groups[1].Value);
                        lv1.SubItems.Add(addr.Groups[1].Value);
                        lv1.SubItems.Add(linkman.Groups[1].Value);
                        lv1.SubItems.Add(prices[0].Groups[1].Value);
                        lv1.SubItems.Add(prices[1].Groups[1].Value);
                        lv1.SubItems.Add(prices[2].Groups[1].Value);
                        lv1.SubItems.Add(kuan.Groups[1].Value);
                        lv1.SubItems.Add(content.Groups[1].Value);
                        lv1.SubItems.Add(louceng.Groups[1].Value);
                        lv1.SubItems.Add(type.Groups[1].Value);
                        lv1.SubItems.Add(area.Groups[1].Value);




                        string atitle = System.Web.HttpUtility.UrlEncode(title.Groups[1].Value);
                        string aaddr = System.Web.HttpUtility.UrlEncode(addr.Groups[1].Value);
                        string alinkman = System.Web.HttpUtility.UrlEncode(linkman.Groups[1].Value);
                        string zujin = System.Web.HttpUtility.UrlEncode(prices[0].Groups[1].Value + "万");
                        string zhuanrang = System.Web.HttpUtility.UrlEncode(prices[1].Groups[1].Value);
                        string mianji = System.Web.HttpUtility.UrlEncode(prices[2].Groups[1].Value);
                        string miankuan = System.Web.HttpUtility.UrlEncode(kuan.Groups[1].Value);
                        string neirong = System.Web.HttpUtility.UrlEncode(content.Groups[1].Value);
                        string lc = System.Web.HttpUtility.UrlEncode(louceng.Groups[1].Value);
                        string leixing = System.Web.HttpUtility.UrlEncode(type.Groups[1].Value);
                        string quyu = System.Web.HttpUtility.UrlEncode(area.Groups[1].Value);
                        string province = System.Web.HttpUtility.UrlEncode("北京");
                        string tel = gettel(uid.Groups[1].Value);
                        string ya = "1";
                        string fu = "1";
                        StringBuilder ima = new StringBuilder();
                        foreach (Match imag in images)
                        {
                            ima.Append("&row[store_images][]=" + imag.Groups[1].Value);
                        }

                        string mages = ima.ToString();
                        if (atitle != "" && tel != "")
                        {

                            string apiurl = "http://sc.souchengwang.com/api/Collection/AutoUpdate?row[store_state]=%E8%BD%AC%E8%AE%A9&row[store_category]=" + leixing + "&row[store_quyu]=" + quyu + "&row[store_province]=" + province + "&row[store_city]=" + aaddr + "&row[store_title]=" + atitle + "&row[store_user]=" + alinkman + "&row[store_phone]=" + tel + "&row[store_status]=%e8%90%a5%e4%b8%9a%e4%b8%ad&row[store_area]=" + mianji + "&row[store_money]=" + zujin + "&row[store_move_money]=" + zhuanrang + "&row[ya]=" + ya + "&row[fu]=" + fu + "&row[width]=" + miankuan + "&row[floor]=" + lc + "&row[from]=58%E5%90%8C%E5%9F%8E&row[keliu][]=%E5%8A%9E%E5%85%AC%E4%BA%BA%E7%BE%A4&row[keliu][]=%E5%AD%A6%E7%94%9F%E4%BA%BA%E7%BE%A4&row[keliu][]=%E5%B1%85%E6%B0%91%E4%BA%BA%E7%BE%A4&row[keliu][]=%E6%97%85%E6%B8%B8%E4%BA%BA%E7%BE%A4&row[facility][]=%E5%8F%AF%E6%98%8E%E7%81%AB&row[facility][]=%E6%9C%89%E8%AF%81%E7%85%A7&row[facility][]=%E4%B8%8A%E6%B0%B4&row[facility][]=%E4%B8%8B%E6%B0%B4&row[facility][]=380%E4%BC%8F&row[facility][]=%E7%85%A4%E6%B0%94%E7%BD%90&row[facility][]=%E7%83%9F%E7%AE%A1%E9%81%93&row[facility][]=%E6%8E%92%E6%B1%A1%E7%AE%A1%E9%81%93&row[facility][]=%E5%81%9C%E8%BD%A6%E4%BD%8D&row[facility][]=%E5%A4%A9%E7%84%B6%E6%B0%94&row[facility][]=%E5%A4%96%E6%91%86%E5%8C%BA&row[facility][]=%E5%AE%A2%E6%A2%AF&row[facility][]=%E8%B4%A7%E6%A2%AF&row[facility][]=%E4%B8%AD%E5%A4%AE%E7%A9%BA%E8%B0%83&row[facility][]=%E5%A4%A9%E7%84%B6%E6%B0%94&row[facility][]=%E7%BD%91%E7%BB%9C&row[facility][]=%E6%9A%96%E6%B0%94&row[facility][]=%E6%89%B6%E6%A2%AF&row[facility][]=%E6%8E%92%E7%83%9F&row[facility][]=%E6%8E%92%E6%B1%A1%E7%AE%A1%E7%85%A4&row[realclass]=%E6%AD%A5%E8%A1%8C%E5%95%86%E4%B8%9A%E8%A1%97&row[store_content]=" + neirong + "&imgs=&row[siteimg]=&row[isshow]=0" + mages;
                            string status = GetUrl(apiurl);

                        }
                        else
                        {
                            MessageBox.Show("过期");
                            return;
                        }
                    }
                    catch
                    {

                        continue;
                    }

                    Thread.Sleep(10000);

                }

                else
                {
                  
                }
                }
 
        }
        private void 手机网页58_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
