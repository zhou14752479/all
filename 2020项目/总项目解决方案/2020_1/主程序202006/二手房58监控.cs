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

namespace 主程序202006
{
    public partial class 二手房58监控 : Form
    {
        public 二手房58监控()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "Hm_lvt_295da9254bbc2518107d846e1641908e=1582933513,1582983468; 58tj_uuid=6af0cab4-7088-4082-b9f8-7c7438d6ab59; new_uv=2; wmda_new_uuid=1; wmda_uuid=81b63df3f3c3e53f507fd2a66fea6a34; wmda_visited_projects=%3B6333604277682; m58comvp=t29v115.159.229.19; als=0; id58=e87rZV5ZpeYPc5QcCxJKAg==";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://servicewechat.com/wxe5b752fbe3874df1/91/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.12(0x17000c30) NetType/4G Language/zh_CN";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                WebHeaderCollection headers = request.Headers;
                headers.Add("ak: 931d0f0a7f7bc73c7cee04b87a1f3cb83d175517");
                headers.Add("X_AJK_APP: i-weapp");
                headers.Add("ft: ajk-weapp");
                headers.Add("X-Forwarded-For: 112.64.131.100");
               
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

        ArrayList lists = new ArrayList();

        /// <summary>
        /// 手机端列表直接
        /// </summary>
        public void run()
        {

                label2.Text = DateTime.Now.ToString() + "  ：正在监控...";
                string cityId = textBox1.Text.Trim();

                string url = "https://miniapp.58.com/sale/property/list?cid=" + cityId + "&from=58_ershoufang&app=i-wb&platform=ios&b=iPhone&s=iOS12.3.1&t=1590814650&cv=5.0&wcv=5.0&wv=7.0.12&sv=2.11.1&batteryLevel=78&muid=33369ab43c140f725624e8ed4aa4ccaf&weapp_version=1.0.0&user_id=&oid=oIArb4tHXwSbAOMiJpA7LwxGVlY0&udid=oIArb4tHXwSbAOMiJpA7LwxGVlY0&page=1&page_size=25&open_id=&union_id=&token=&source_id=2&orderby=6&entry=1003&city_id=" + cityId;


                string html = GetUrl(url);

                MatchCollection titles = Regex.Matches(html, @",""title"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""community""([\s\S]*?)name"":""([\s\S]*?)"""); //
                MatchCollection loucengs = Regex.Matches(html, @"""floor_level"":""([\s\S]*?)"""); //楼层
                MatchCollection orients = Regex.Matches(html, @"""orient"":""([\s\S]*?)""");  //朝向
                MatchCollection area_nums = Regex.Matches(html, @"""area_num"":""([\s\S]*?)""");  //面积
                MatchCollection prices = Regex.Matches(html, @"{""price"":""([\s\S]*?)""");  //价格

                MatchCollection fitment_names = Regex.Matches(html, @"""fitment_name"":""([\s\S]*?)""");  //装修
                MatchCollection shipTypeStrs = Regex.Matches(html, @"""shipTypeStr"":""([\s\S]*?)""");  //类型

                MatchCollection post_date = Regex.Matches(html, @"""post_date"":""([\s\S]*?)""");  //发布时间
                MatchCollection mobiles = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");  //电话
                MatchCollection users = Regex.Matches(html, @"brokerId([\s\S]*?)name"":""([\s\S]*?)""");  //联系人
                MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");  //位置



                MatchCollection a1 = Regex.Matches(html, @"""room_num"":""([\s\S]*?)""");  //户型
                MatchCollection a2 = Regex.Matches(html, @"""hall_num"":""([\s\S]*?)""");  //户型
                MatchCollection a3 = Regex.Matches(html, @"""toilet_num"":""([\s\S]*?)""");  //户型

                int j = 0;

                    if (!lists.Contains(titles[j].Groups[1].Value))
                    {

                try
                {
                    lists.Add(titles[j].Groups[1].Value);

                    textBox2.Text = "标题:" + titles[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "小区:" + names[j].Groups[2].Value + "\r\n";
                    textBox2.Text += "户型:" + a1[j].Groups[1].Value + "-" + a2[j].Groups[1].Value + "-" + a3[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "面积:" + area_nums[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "售价:" + prices[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "楼层:" + loucengs[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "朝向:" + orients[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "装修:" + fitment_names[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "类型:" + shipTypeStrs[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "位置:" + address[j].Groups[1].Value + "\r\n";
                    textBox2.Text += "发布时间:" + ConvertStringToDateTime(post_date[j].Groups[1].Value).ToString() + "\r\n";
                    textBox2.Text += "当前时间:" + DateTime.Now .ToString() + "\r\n";
                    textBox2.Text += "联系信息:" + users[j].Groups[2].Value + mobiles[j].Groups[1].Value + "\r\n";



                    if (status == false)
                        return;

                    string path = AppDomain.CurrentDomain.BaseDirectory + titles[j].Groups[1].Value + ".txt";
                    System.IO.File.WriteAllText(path, textBox2.Text.Trim(), Encoding.UTF8);
                }
                catch 
                {

                    
                }
                       
            }
              

            


        }
        bool status = true;
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));

        }
        private void 二手房58监控_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            status = true;
            //button1.Enabled = false;
            //Thread thread = new Thread(new ThreadStart(run));
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"ershoufangjiankong"))
            {
                timer1.Start();

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = "停止监控";
            status = false;
            button1.Enabled = true;
            timer1.Stop();
        }

        Thread thread;

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
            run();
        }
    }
}
