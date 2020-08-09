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
using helper;

namespace 主程序202008
{
    public partial class 美团列表 : Form
    {
        public 美团列表()
        {
            InitializeComponent();
        }
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "_lxsdk_cuid=1727e557370c8-055ba60b44a861-6373664-1fa400-1727e55737034; _hc.v=f01b4976-559d-acf7-fffe-102bfc43157f.1593688376; iuuid=1D7CA7F80A2A482D22EF52A684E80D67E21087FB513468871702F193EBCA55C7; cityname=%E5%AE%BF%E8%BF%81; _lxsdk=1D7CA7F80A2A482D22EF52A684E80D67E21087FB513468871702F193EBCA55C7; webp=1; i_extend=H__a100040__b1; __utma=74597006.2094703603.1595407148.1595407148.1595407148.1; __utmz=74597006.1595407148.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); ci=10; rvct=10%2C184%2C50%2C76%2C230; uuid=351c7f9866864f78bd91.1596429777.1.0.0; _lxsdk_s=173b2a13bd1-c85-246-7a3%7C%7C26";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://sh.meituan.com/s/%E8%B6%B3%E6%B5%B4/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
              
                 request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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

        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://"+city+".meituan.com/";
                string html = GetUrl(url,"utf-8");
                Match cityId = Regex.Match(html, @"currentCity"":{""id"":([\s\S]*?),");

                return cityId.Groups[1].Value;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }

        #endregion

        #region 获取区域
        public ArrayList getareas(string city)
        {
            string Url = "https://" + city + ".meituan.com/meishi/";

            string html = GetUrl(Url,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"""subAreas"":\[\{""id"":([\s\S]*?),");
            ArrayList lists = new ArrayList();

            foreach (Match item in areas)
            {
                lists.Add(item.Groups[1].Value);
            }

            return lists;
        }

        #endregion
        bool zanting = true;
        public void run()
        {
            string[] citys = { "sh", "jn", "nb", "bj"};
            foreach (string city in citys)
            {
                string cityId = GetcityId(city);

                ArrayList areas = getareas(city);

                foreach (string areaId in areas)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        int offset = i * 32;
                      
                        string url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/" + cityId + "?uuid=351c7f9866864f78bd91.1596429777.1.0.0&userid=-1&limit=32&offset=" + offset + "&cateId=-1&q=%E8%B6%B3%E6%B5%B4&areaId=" + areaId;
                       
                        string html = GetUrl(url, "utf-8");
                      

                        MatchCollection strhtmls = Regex.Matches(html, @"imageUrl([\s\S]*?)full");

                        if (strhtmls.Count == 0)
                        {

                            break;
                        }

                        foreach (Match strhtml in strhtmls)
                        
                        {
                            Match title = Regex.Match(strhtml.Groups[1].Value, @"""title"":""([\s\S]*?)""");
                            Match area = Regex.Match(strhtml.Groups[1].Value, @"""areaname"":""([\s\S]*?)""");
                            Match rank = Regex.Match(strhtml.Groups[1].Value, @"""avgscore"":([\s\S]*?),");
                            Match address = Regex.Match(strhtml.Groups[1].Value, @"""address"":""([\s\S]*?)""");
                            Match commengts = Regex.Match(strhtml.Groups[1].Value, @"""comments"":([\s\S]*?),");



                            Match dealhtml = Regex.Match(strhtml.Groups[1].Value, @"deals([\s\S]*?)posdescr");

                            StringBuilder sb = new StringBuilder();
                            MatchCollection dealtitles = Regex.Matches(dealhtml.Groups[1].Value, @"""title"":""([\s\S]*?)""");
                            MatchCollection dealprices = Regex.Matches(dealhtml.Groups[1].Value, @"""price"":([\s\S]*?),");
                            MatchCollection dealyuanjias = Regex.Matches(dealhtml.Groups[1].Value, @"""value"":([\s\S]*?),");
                            MatchCollection dealsales = Regex.Matches(dealhtml.Groups[1].Value, @"""sales"":([\s\S]*?),");
                            for (int j = 0; j < dealtitles.Count; j++)
                            {
                                sb.Append(dealtitles[j].Groups[1].Value+"*"+ dealprices[j].Groups[1].Value+"*"+ dealyuanjias[j].Groups[1].Value+"*"+ dealsales[j].Groups[1].Value+"#");
                            }

                           

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title.Groups[1].Value);
                            lv1.SubItems.Add(area.Groups[1].Value);
                            lv1.SubItems.Add(rank.Groups[1].Value);
                            lv1.SubItems.Add(address.Groups[1].Value);
                            lv1.SubItems.Add(rank.Groups[1].Value);
                            lv1.SubItems.Add(commengts.Groups[1].Value);
                            lv1.SubItems.Add(sb.ToString());
                            lv1.SubItems.Add(areaId);
                            lv1.SubItems.Add(city);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }

                       





                        Thread.Sleep(2000);
                    }

                }
            }




        }
        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 美团列表_Load(object sender, EventArgs e)
        {

        }
    }
}
